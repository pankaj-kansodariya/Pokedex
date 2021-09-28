using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokedex.Common.Configurations;
using Pokedex.Manager.Services.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.Manager.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly PokemonServiceConfigurations _configurations;

        public PokemonService(HttpClient httpClient,
            IOptions<PokemonServiceConfigurations> configurationOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configurations = configurationOptions?.Value ?? throw new ArgumentNullException(nameof(configurationOptions));

            if (string.IsNullOrEmpty(_configurations.BaseURL))
                throw new ArgumentNullException(nameof(configurationOptions), "BaseURL is missing for PokemonService");
            _httpClient.BaseAddress = new Uri(_configurations.BaseURL);

        }

        public async Task<PokemonSpeciesModel> GetDetails(string name)
        {
            var response = await _httpClient.GetAsync(string.Format(_configurations.PokemonSpeciesURL, name));
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PokemonSpeciesModel>(responseContent);
        }
    }
}
