using Microsoft.EntityFrameworkCore;
using SoftwareDevelopmentTest.DAL;
using SoftwareDevelopmentTest.Services;
namespace SoftwareDevelopmentTest;

public class Worker : BackgroundService
{
    private readonly IBuildingService _buildingService;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public Worker(IBuildingService buildingService,
                  IDbContextFactory<AppDbContext> contextFactory)
    {
        _buildingService = buildingService;
        _contextFactory = contextFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var floors = await _buildingService.GetFloors();
        using var appDbContext = await _contextFactory.CreateDbContextAsync(stoppingToken);
        foreach (var floor in floors)
        {
            floor.Fixtures = await _buildingService.GetFloorDetails(floor.Id);
            floor.Id = default;
        }
        appDbContext.Floors.AddRange(floors);
        await appDbContext.SaveChangesAsync();
        Console.WriteLine("Floors saved.....");
    }
}
