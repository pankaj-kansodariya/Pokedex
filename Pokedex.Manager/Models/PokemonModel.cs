using Pokedex.Manager.Services.Models;
using System.Linq;

namespace Pokedex.Manager.Models
{
    public class PokemonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public PokemonModel()
        {

        }
        public PokemonModel(PokemonModel source)
        {
            if (source == null) throw new System.ArgumentNullException(nameof(source), "Source object can not be null");
            Name = source?.Name;
            Description = source?.Description;
            Habitat = source?.Habitat;
            IsLegendary = (source?.IsLegendary).GetValueOrDefault(false);
        }
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
