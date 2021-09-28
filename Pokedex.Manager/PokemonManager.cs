using Microsoft.Extensions.Logging;
using Pokedex.Common.Enums;
using Pokedex.Manager.Models;
using Pokedex.Manager.Services;
using System;
using System.Threading.Tasks;

namespace Pokedex.Manager
{
    public class PokemonManager : IPokemonManager
    {

        private readonly IPokemonService _pokemonService;
        private readonly ITranslationService _translationService;
        private readonly ILogger _logger;
        public PokemonManager(IPokemonService pokemonService,
            ITranslationService translationService,
            ILogger<PokemonManager> logger)
        {
            _pokemonService = pokemonService ?? throw new ArgumentNullException(nameof(pokemonService));
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PokemonModel> GetPokemanBasicInfo(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            var model = await _pokemonService.GetDetails(name);
            return PokemonModel.FromServiceModel(model);
        }
        public async Task<string> GetPokemanTranslation(string content, TranslationType translationType)
        {
            string result = null;
            try
            {
                if (!string.IsNullOrEmpty(content))
                    result = await _translationService.Translate(content, translationType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while {translationType} Translating {content}");
            }
            return result ?? content;
        }
    }
}
