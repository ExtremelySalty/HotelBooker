using System.Text.Json;

namespace HotelBooker.WebApi.Tests.Integration
{
    public abstract class IntegrationTestBase : CustomWebApplicationFactory
    {
        protected static async Task<TEntity?> DeserializeAsync<TEntity>(HttpResponseMessage response)
            => await JsonSerializer.DeserializeAsync<TEntity>
            (
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
    }
}
