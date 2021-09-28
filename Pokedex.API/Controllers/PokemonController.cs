using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pokedex.Manager;
using Pokedex.Manager.Models;
using Pokedex.Manager.Providers;
using System;
using System.Threading.Tasks;

namespace Pokedex.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonManager _pokemanManager;
        private readonly ITranslationProvider _translationProvider;
        private readonly IMemoryCache _memoryCache_Pokemon;
        private readonly IMemoryCache _memoryCache_Translation;
        private readonly ILogger<PokemonController> _logger;
        private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddDays(1),
            Priority = CacheItemPriority.Normal,
            SlidingExpiration = TimeSpan.FromHours(1),
            Size = 1024
        };

        public PokemonController(
            IPokemonManager pokemanManager,
            ITranslationProvider translationProvider,
            IMemoryCache memoryCache_Pokemon,
            IMemoryCache memoryCache_Translation,
            ILogger<PokemonController> logger)
        {
            _pokemanManager = pokemanManager ?? throw new ArgumentNullException(nameof(pokemanManager));
            _translationProvider = translationProvider ?? throw new ArgumentNullException(nameof(translationProvider));
            _memoryCache_Pokemon = memoryCache_Pokemon ?? throw new ArgumentNullException(nameof(memoryCache_Pokemon));
            _memoryCache_Translation = memoryCache_Translation ?? throw new ArgumentNullException(nameof(memoryCache_Translation));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<PokemonModel> GetPokemonBasicInfo(string name)
        {
            if (!_memoryCache_Pokemon.TryGetValue(name, out PokemonModel model))
            {
                model = await _pokemanManager.GetPokemanBasicInfo(name);
                _memoryCache_Translation.Set(name, model, _cacheOptions);
            }
            return model;
        }

        [Route("{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(PokemonModel), APIConstants.HttpStatusCode_OK)]
        [ProducesResponseType(typeof(string), APIConstants.HttpStatusCode_BadRequest)]
        [ProducesResponseType(typeof(string), APIConstants.HttpStatusCode_InternalServerError)]
        public async Task<IActionResult> GetBasicInfo([FromRoute] string name)
        {
            if (string.IsNullOrEmpty(name))
                return StatusCode(statusCode: APIConstants.HttpStatusCode_BadRequest, value: APIConstants.ERROR_InvalidInput);

            try
            {
                PokemonModel model = await GetPokemonBasicInfo(name);
                return StatusCode(statusCode: APIConstants.HttpStatusCode_OK, value: model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while GetBasicInfo for {name}");
                return StatusCode(statusCode: APIConstants.HttpStatusCode_InternalServerError, value: APIConstants.ERROR_UnexpectedError);
            }
        }

        [Route("translated/{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(PokemonModel), APIConstants.HttpStatusCode_OK)]
        [ProducesResponseType(typeof(string), APIConstants.HttpStatusCode_BadRequest)]
        [ProducesResponseType(typeof(string), APIConstants.HttpStatusCode_InternalServerError)]
        public async Task<IActionResult> GetTranslatedInfo([FromRoute] string name)
        {

            if (string.IsNullOrEmpty(name))
                return StatusCode(statusCode: APIConstants.HttpStatusCode_BadRequest, value: APIConstants.ERROR_InvalidInput);

            PokemonModel responseModel = null;
            try
            {
                var model = await GetPokemonBasicInfo(name);
                var translationType = _translationProvider.ProvideTransactionType(model);
                var key = $"{name}-{translationType}";
                if (!_memoryCache_Translation.TryGetValue(key, out string description))
                {
                    description = await _pokemanManager.GetPokemanTranslation(model?.Description, translationType);
                    if (description != null)
                        _memoryCache_Translation.Set(key, description, _cacheOptions);
                }
                responseModel = new PokemonModel(model) { Description = description ?? model.Description };
                return StatusCode(statusCode: APIConstants.HttpStatusCode_OK, value: responseModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while GetTranslatedInfo for {name}");
                return StatusCode(statusCode: APIConstants.HttpStatusCode_InternalServerError, value: APIConstants.ERROR_UnexpectedError);
            }
        }
    }
}
