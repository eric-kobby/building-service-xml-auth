using SoftwareDevelopmentTest.DAL.Entities;

namespace SoftwareDevelopmentTest.Services
{
    public interface IBuildingService
    {
        Task<List<Floor>> GetFloors();
        Task<List<Fixture>> GetFloorDetails(int floorId);
    }
}