using System.Xml;
using SoftwareDevelopmentTest.DAL.Entities;

namespace SoftwareDevelopmentTest.Services
{
    #nullable disable
    public class BuildingService : IBuildingService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;

        public BuildingService(
            HttpClient httpClient,
            IAuthenticationService authenticationService,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _configuration = configuration;
        }

        public async Task<List<Floor>> GetFloors()
        {
            var httpResponseMessage = await SendRequest("/floor/list");
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            return ParseFloors(response).ToList();
        }

        public async Task<List<Fixture>> GetFloorDetails(int floorId)
        {
            var httpMessage = await SendRequest($"/fixture/location/list/floor/{floorId}/fixtured");
            httpMessage.EnsureSuccessStatusCode();
            var response = await httpMessage.Content.ReadAsStringAsync();
            return ParseFixtures(response).ToList();
        }

        private async Task<HttpResponseMessage> SendRequest(string requestUri)
        {
            var authDetails = _authenticationService.GetAuthHeaderValues();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_configuration["RelativeUrl"]}{requestUri}");
            requestMessage.Headers.Add("ts", authDetails.TimeStamp);
            requestMessage.Headers.Add("Authorization", authDetails.Token);
            requestMessage.Headers.Add("APIkey", authDetails.APIkey);
            return await _httpClient.SendAsync(requestMessage);
        }

        private IEnumerable<Floor> ParseFloors(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var floorNodes = xmlDocument.ChildNodes[1];
            var current = floorNodes.FirstChild;
            
            while (current != null)
            {
                var result = new Floor
                {
                    Id = int.Parse(current["id"].InnerText),
                    Campus = current["campus"].InnerText,
                    Company = current["company"].InnerText,
                    Description = current["description"].InnerText,
                    FloorPlanUrl = current["floorPlanUrl"].InnerText,
                    Name = current["name"].InnerText,
                    ParentFloorId = ConvertToInt(current["parentFloorId"].InnerText)
                };
                current = current.NextSibling;
                yield return result;
            }
        }

        private IEnumerable<Fixture> ParseFixtures(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            var current = xmlDocument.ChildNodes[1].FirstChild;

            while (current != null)
            {
                var result = new Fixture
                {
                    AreaId = ConvertToInt(current["areaId"]?.InnerText),
                    GroupId = ConvertToInt(current["groupId"].InnerText).Value,
                    MacAddress = current["macAddress"].InnerText,
                    Name = current["name"].InnerText,
                    Xaxis = ConvertToInt(current["xaxis"].InnerText).Value,
                    Yaxis = ConvertToInt(current["yaxis"].InnerText).Value
                };
                current = current.NextSibling;
                yield return result;
            }
        }

        private int? ConvertToInt(string value)
        {
            return int.TryParse(value, out int result) ? result : null;
        }
    }
}

