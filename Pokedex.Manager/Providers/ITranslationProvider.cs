using Pokedex.Common.Enums;
using Pokedex.Manager.Models;

namespace Pokedex.Manager.Providers
{
    public interface ITranslationProvider
    {
        TranslationType ProvideTransactionType(PokemonModel model);
    }
}
