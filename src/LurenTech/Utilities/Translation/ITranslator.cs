using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LurenTech.Utilities.Translation
{
    public interface ITranslator
    {

    }

    public interface ITranslator<BE, Model> : ITranslator
    {
        BE Translate(Model item);
        Model Translate(BE item);
    }
}
