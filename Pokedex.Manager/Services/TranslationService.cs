using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokedex.Common.Configurations;
using Pokedex.Common.Enums;
using Pokedex.Manager.Services.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.Manager.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly TranslationServiceConfigurations _configurations;

        public TranslationService(HttpClient httpClient, IOptions<TranslationServiceConfigurations> configurationOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configurations = configurationOptions?.Value ?? throw new ArgumentNullException(nameof(configurationOptions));

            _httpClient.BaseAddress = new Uri(_configurations.BaseURL);
        }

        public async Task<string> Translate(string content, TranslationType translationType)
        {
            var url = string.Format(_configurations.LanguageServiceURL, translationType.ToString().ToLower(), content);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TranslatedModel>(responseContent);
            if (model?.Success?.Total == 1)
                return model?.Contents?.Translated;
            return null;
        }
    }
}
