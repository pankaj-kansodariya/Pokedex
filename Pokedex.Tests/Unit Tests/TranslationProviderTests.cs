using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokedex.Common.Enums;
using Pokedex.Common.Models;
using Pokedex.Manager.Providers;
using System;

namespace Pokedex.Tests.Unit_Tests
{
    [TestClass]
    public class TranslationProviderTests
    {
        private readonly ITranslationProvider _translationProvider = new TranslationProvider();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_ThrowException_When_ArgumentIsNull()
        {
            var result = _translationProvider.ProvideTransactionType(null);
        }

        [TestMethod]
        public void Should_ReturnYode_When_HabitatIsCave()
        {
            var result = _translationProvider.ProvideTransactionType(new PokemonModel
            {
                Habitat = "cave"
            });
            Assert.AreEqual(result, TranslationType.Yoda);
        }

        [TestMethod]
        public void Should_ReturnYode_When_IsLegendary()
        {
            var result = _translationProvider.ProvideTransactionType(new PokemonModel
            {
                IsLegendary = true
            });
            Assert.AreEqual(result, TranslationType.Yoda);
        }

        [TestMethod]
        public void Should_ReturnShakespeare_When_NotIsLegendaryAndHabitatIsNotCave()
        {
            var result = _translationProvider.ProvideTransactionType(new PokemonModel
            {
                IsLegendary = false,
                Habitat = "test"
            });
            Assert.AreEqual(result, TranslationType.Shakespeare);
        }
    }
}
