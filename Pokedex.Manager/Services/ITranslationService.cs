using Pokedex.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Manager.Services
{
    public interface ITranslationService
    {
        Task<string> Translate(string content, TranslationType translationType);
    }
}
