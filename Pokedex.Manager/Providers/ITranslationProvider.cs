using Pokedex.Common.Enums;
using Pokedex.Common.Models;

namespace Pokedex.Manager.Providers
{
    public interface ITranslationProvider
    {
        TranslationType ProvideTransactionType(PokemonModel model);
    }
}
