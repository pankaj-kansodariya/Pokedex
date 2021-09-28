using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Manager.Services.Models
{
    public class TranslatedModel
    {
        public SuccessModel Success { get; set; }
        public ContentsModel Contents { get; set; }
        public class SuccessModel
        {
            public int Total { get; set; }
        }
        public class ContentsModel
        {
            public string Translated { get; set; }
            public string Text { get; set; }
            public string Translation { get; set; }
        }
    }
}
