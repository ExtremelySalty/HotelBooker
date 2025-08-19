namespace HotelBooker.Application.Interfaces.Repositories
{
    public interface ISeedData
    {
        Task SeedAsync(CancellationToken ct);
        Task ResetAsync(CancellationToken ct);
    }
}
