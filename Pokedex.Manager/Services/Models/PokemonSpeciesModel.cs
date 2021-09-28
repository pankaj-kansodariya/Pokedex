using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Manager.Services.Models
{
    public class PokemonSpeciesModel
    {
        public string Name { get; set; }
        [JsonProperty("flavor_text_entries")]
        public IEnumerable<FlavorTextEntryModel> FlavorTextEntries { get; set; }

        public HabitatModel Habitat { get; set; }
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
        public class FlavorTextEntryModel
        {
            [JsonProperty("flavor_text")]
            public string FlavorText { get; set; }
            public LanguageModel Language { get; set; }
        }
        public class LanguageModel
        {
            public string Name { get; set; }
            [JsonProperty("url")]
            public string URL { get; set; }
        }

        public class HabitatModel
        {
            public string Name { get; set; }
            [JsonProperty("url")]
            public string URL { get; set; }
        }
    }
}
