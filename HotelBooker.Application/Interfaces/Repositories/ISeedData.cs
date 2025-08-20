namespace HotelBooker.Application.Interfaces.Repositories
{
    public interface ISeedData
    {
        Task SeedAsync(int hotelsToSeed, CancellationToken ct);
        Task ResetAsync(CancellationToken ct);
    }
}
