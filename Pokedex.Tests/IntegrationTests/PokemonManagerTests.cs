using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Pokedex.Common.Enums;
using Pokedex.Manager;
using Pokedex.Manager.Services;
using Pokedex.Manager.Services.Models;
using System;
using System.Threading.Tasks;

namespace Pokedex.Tests.IntegrationTests
{
    [TestClass]
    public class PokemonManagerTests
    {
        private readonly IPokemonService _pokemonService = Substitute.For<IPokemonService>();
        private readonly ITranslationService _translationService = Substitute.For<ITranslationService>();
        private readonly ILogger<PokemonManager> _logger = Substitute.For<ILogger<PokemonManager>>();
        private IPokemonManager _pokemonManager;
        private const string _testContent = "Test";

        [TestInitialize]
        public void TestInitialize()
        {
            _pokemonManager = new PokemonManager(_pokemonService, _translationService, _logger);
        }

        [TestMethod]
        public async Task GetPokemanBasicInfo_Should_Return_Result_When_PassedValidData()
        {
            var model = new PokemonSpeciesModel
            {
                Habitat = new PokemonSpeciesModel.HabitatModel { Name = "TestHabitat" },
                IsLegendary = true,
                Name = "TestName",
                FlavorTextEntries = new PokemonSpeciesModel.FlavorTextEntryModel[]
                                    {
                                        new PokemonSpeciesModel.FlavorTextEntryModel
                                        {
                                            FlavorText = "EnglishText",
                                            Language = new PokemonSpeciesModel.LanguageModel { Name = "en" }
                                        },
                                        new PokemonSpeciesModel.FlavorTextEntryModel
                                        {
                                            FlavorText = "NonEnglishText",
                                            Language = new PokemonSpeciesModel.LanguageModel { Name = "ab" }
                                        }

                                    }
            };
            _pokemonService.GetDetails(Arg.Any<string>()).Returns(Task.FromResult<PokemonSpeciesModel>(model));
            var result = await _pokemonManager.GetPokemanBasicInfo(_testContent);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsLegendary);
            Assert.IsTrue(string.Compare(result.Name, "TestName", true) == 0);
            Assert.IsTrue(string.Compare(result.Description, "EnglishText", true) == 0);
        }

        [TestMethod]
        public async Task GetPokemanBasicInfo_Should_Return_Null_When_PassedInValidData()
        {
            var result = await _pokemonManager.GetPokemanBasicInfo("");
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetPokemanBasicInfo_Should_ThrowException_When_PokemonNotFound()
        {
            _pokemonService.GetDetails(Arg.Any<string>()).Returns(Task.FromException<PokemonSpeciesModel>(new ArgumentException("Not Found")));
            var result = await _pokemonManager.GetPokemanBasicInfo(_testContent);
        }
        [TestMethod]
        public async Task GetPokemanTranslation_Should_Return_Null_When_PassedInValidData()
        {
            var result = await _pokemonManager.GetPokemanTranslation(null, TranslationType.Yoda);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPokemanTranslation_Should_Return_SameContent_When_TranslationFails()
        {
            _translationService.Translate(Arg.Any<string>(), Arg.Any<TranslationType>())
                .Returns(Task.FromException<string>(new Exception("Error")));
            var result = await _pokemonManager.GetPokemanTranslation(_testContent, TranslationType.Yoda);
            Assert.IsNotNull(result);
            Assert.IsTrue(string.Compare(result, _testContent, true) == 0);
        }

        [TestMethod]
        public async Task GetPokemanTranslation_Should_Return_SameContent_When_TranslationReturnsNull()
        {
            _translationService.Translate(Arg.Any<string>(), Arg.Any<TranslationType>())
                .Returns(Task.FromResult<string>(null));
            var result = await _pokemonManager.GetPokemanTranslation(_testContent, TranslationType.Yoda);
            Assert.IsNotNull(result);
            Assert.IsTrue(string.Compare(result, _testContent, true) == 0);
        }
    }
}
