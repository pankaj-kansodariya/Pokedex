using Pokedex.Common.Enums;
using Pokedex.Manager.Models;
using System.Threading.Tasks;

namespace Pokedex.Manager
{
    public interface IPokemonManager
    {
        Task<PokemonModel> GetPokemanBasicInfo(string name);
        Task<string> GetPokemanTranslation(string content, TranslationType translationType);
    }
}
