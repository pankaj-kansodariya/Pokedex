using Pokedex.Manager.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Manager.Models
{
    public class PokemonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }        
        public static PokemonModel FromServiceModel(PokemonSpeciesModel model)
        {
            return new PokemonModel
            {
                Name = model?.Name,
                Description = model?.FlavorTextEntries.Where(x => string.Compare(x?.Language?.Name, "en", true) == 0)
                    .FirstOrDefault()?.FlavorText,
                Habitat = model?.Habitat?.Name,
                IsLegendary = (model?.IsLegendary).GetValueOrDefault(false)
            };
        }
    }
}
