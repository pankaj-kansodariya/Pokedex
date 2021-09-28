using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Pokedex.API;
using Pokedex.API.Controllers;
using Pokedex.Common.Enums;
using Pokedex.Manager;
using Pokedex.Manager.Models;
using Pokedex.Manager.Providers;
using System;
using System.Threading.Tasks;

namespace Pokedex.Tests.IntegrationTests
{
    [TestClass]
    public class PokemonControllerTests
    {
        private readonly ITranslationProvider _translationProvider = Substitute.For<ITranslationProvider>();
        private readonly ILogger<PokemonController> _logger = Substitute.For<ILogger<PokemonController>>();
        private readonly IPokemonManager _pokemonManager = Substitute.For<IPokemonManager>();
        private readonly IMemoryCache _memoryCache_Pokemon = Substitute.For<IMemoryCache>();
        private readonly IMemoryCache _memoryCache_Translation = Substitute.For<IMemoryCache>();
        private const string _testName = "Test";
        private readonly PokemonModel _pokemonModel = new PokemonModel
        {
            Name = "Test",
            Description = "TestDescription",
            Habitat = "TestHabitat",
            IsLegendary = true
        };
        private readonly PokemonModel _translatedPokemonModel = new PokemonModel
        {
            Name = "Test",
            Description = "Translated-TestDescription",
            Habitat = "TestHabitat",
            IsLegendary = true
        };
        private PokemonController _pokemonController;

        [TestInitialize]
        public void TestInitialize()
        {
            _pokemonController = new PokemonController(_pokemonManager, _translationProvider, _memoryCache_Pokemon, _memoryCache_Translation, _logger);
        }

        private bool AreBothPokemonSame(PokemonModel first, PokemonModel second)
        {
            return string.Compare(first.Name, second.Name, true) == 0 &&
                string.Compare(first.Description, second.Description, true) == 0 &&
                string.Compare(first.Habitat, second.Habitat, true) == 0 &&
                first.IsLegendary == second.IsLegendary;
        }
        [TestMethod]
        public async Task GetBasicInfo_Should_GetFromService_WhenNothingInCache()
        {
            _pokemonManager.GetPokemanBasicInfo(Arg.Any<string>()).Returns(Task.FromResult<PokemonModel>(_pokemonModel));
            _memoryCache_Pokemon.TryGetValue(Arg.Any<string>(), out Arg.Any<PokemonModel>()).Returns(false);
            var actionResult = await _pokemonController.GetBasicInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual(response.StatusCode, APIConstants.HttpStatusCode_OK);
            var result = response.Value;
            Assert.IsInstanceOfType(result, typeof(PokemonModel));
            var model = (PokemonModel)result;
            Assert.IsTrue(AreBothPokemonSame(model, _pokemonModel));
        }

        [TestMethod]
        public async Task GetBasicInfo_Should_GetFromCache_WhenInCache()
        {
            _memoryCache_Pokemon.TryGetValue(Arg.Any<string>(), out Arg.Any<PokemonModel>())
                .Returns(x =>
                {
                    x[1] = _pokemonModel;
                    return true;
                });
            var actionResult = await _pokemonController.GetBasicInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual(response.StatusCode, APIConstants.HttpStatusCode_OK);
            var result = response.Value;
            Assert.IsInstanceOfType(result, typeof(PokemonModel));
            var model = (PokemonModel)result;
            Assert.IsTrue(AreBothPokemonSame(model, _pokemonModel));
        }

        [TestMethod]
        public async Task GetBasicInfo_Should_ReturnBadRequest_WhenInvalidInput()
        {
            var actionResult = await _pokemonController.GetBasicInfo("");
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual((int)response.StatusCode, APIConstants.HttpStatusCode_BadRequest);
            Assert.AreSame(response.Value, APIConstants.ERROR_InvalidInput);
        }

        [TestMethod]
        public async Task GetBasicInfo_Should_ReturnNotFound_When_PokemonNotFound()
        {
            _pokemonManager.GetPokemanBasicInfo(Arg.Any<string>())
                .Returns(Task.FromException<PokemonModel>(new ArgumentException("NOT FOUND")));
            var actionResult = await _pokemonController.GetBasicInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual((int)response.StatusCode, APIConstants.HttpStatusCode_NotFound);
            Assert.IsTrue(string.Compare(response.Value.ToString(), $"{_testName}: NOT FOUND", true) == 0);
        }

        [TestMethod]
        public async Task GetBasicInfo_Should_ReturnInternalServerError_WhenServiceFails()
        {
            _memoryCache_Pokemon.TryGetValue(Arg.Any<string>(), out Arg.Any<PokemonModel>()).Returns(false);
            _pokemonManager.GetPokemanBasicInfo(Arg.Any<string>())
                .Returns(Task.FromException<PokemonModel>(new Exception("Error")));
            var actionResult = await _pokemonController.GetBasicInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual((int)response.StatusCode, APIConstants.HttpStatusCode_InternalServerError);
            Assert.AreSame(response.Value, APIConstants.ERROR_UnexpectedError);
        }

        [TestMethod]
        public async Task GetTranslatedInfo_Should_GetFromService_WhenNothingInCache()
        {
            _memoryCache_Pokemon.TryGetValue(Arg.Any<string>(), out Arg.Any<PokemonModel>())
                .Returns(x =>
                {
                    x[1] = _pokemonModel;
                    return true;
                });
            _translationProvider.ProvideTransactionType(Arg.Any<PokemonModel>()).Returns(TranslationType.Yoda);
            _pokemonManager.GetPokemanTranslation(Arg.Any<string>(), Arg.Any<TranslationType>()).Returns(Task.FromResult<string>(_translatedPokemonModel.Description));
            var actionResult = await _pokemonController.GetTranslatedInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual(response.StatusCode, APIConstants.HttpStatusCode_OK);
            var result = response.Value;
            Assert.IsInstanceOfType(result, typeof(PokemonModel));
            var model = (PokemonModel)result;
            Assert.IsTrue(AreBothPokemonSame(model, _translatedPokemonModel));

        }

        [TestMethod]
        public async Task GetTranslatedInfo_Should_GetFromCache_WhenInCache()
        {
            _memoryCache_Pokemon.TryGetValue(Arg.Any<string>(), out Arg.Any<PokemonModel>())
                .Returns(x =>
                {
                    x[1] = _pokemonModel;
                    return true;
                });
            _memoryCache_Translation.TryGetValue(Arg.Any<string>(), out Arg.Any<string>())
                .Returns(x =>
                {
                    x[1] = _translatedPokemonModel.Description;
                    return true;
                });
            _translationProvider.ProvideTransactionType(Arg.Any<PokemonModel>()).Returns(TranslationType.Yoda);
            var actionResult = await _pokemonController.GetTranslatedInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual(response.StatusCode, APIConstants.HttpStatusCode_OK);
            var result = response.Value;
            Assert.IsInstanceOfType(result, typeof(PokemonModel));
            var model = (PokemonModel)result;
            Assert.IsTrue(AreBothPokemonSame(model, _translatedPokemonModel));
        }

        [TestMethod]
        public async Task GetTranslatedInfo_Should_ReturnBadRequest_WhenInvalidInput()
        {
            var actionResult = await _pokemonController.GetTranslatedInfo("");
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual((int)response.StatusCode, APIConstants.HttpStatusCode_BadRequest);
            Assert.AreSame(response.Value, APIConstants.ERROR_InvalidInput);
        }

        [TestMethod]
        public async Task GetTranslatedInfo_Should_ReturnInternalServerError_WhenServiceFails()
        {
            _memoryCache_Pokemon.TryGetValue(Arg.Any<string>(), out Arg.Any<PokemonModel>()).Returns(false);
            _pokemonManager.GetPokemanBasicInfo(Arg.Any<string>())
                .Returns(Task.FromException<PokemonModel>(new Exception("Error")));
            var actionResult = await _pokemonController.GetBasicInfo(_testName);
            Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
            var response = (ObjectResult)actionResult;
            Assert.AreEqual((int)response.StatusCode, APIConstants.HttpStatusCode_InternalServerError);
            Assert.AreSame(response.Value, APIConstants.ERROR_UnexpectedError);
        }
    }
}
