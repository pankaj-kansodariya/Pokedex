using Pokedex.Common.Enums;
using Pokedex.Common.Models;
using System;

namespace Pokedex.Manager.Providers
{
    public class TranslationProvider : ITranslationProvider
    {
        public TranslationType ProvideTransactionType(PokemonModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var transactionType = TranslationType.Shakespeare;

            if (string.Compare(model.Habitat, "cave", true) == 0 || model.IsLegendary)
                transactionType = TranslationType.Yoda;

            return transactionType;
        }
    }
}
