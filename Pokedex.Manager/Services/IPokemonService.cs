using Pokedex.Manager.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Manager.Services
{
    public interface IPokemonService
    {
        Task<PokemonSpeciesModel> GetDetails(string name);
    }
}
