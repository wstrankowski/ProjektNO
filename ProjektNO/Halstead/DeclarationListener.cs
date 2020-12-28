using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektNO.Halstead
{
    class DeclarationListener : CBaseListener
    {
        public IList<Tuple<int, int>> StartStops
        {
            get
            {
                return _startStops;
            }
        }
        private readonly IList<Tuple<int, int>> _startStops = new List<Tuple<int, int>>();

        public override void ExitDeclaration([NotNull] CParser.DeclarationContext context)
        {
            base.ExitDeclaration(context);
            var start = context.Start.TokenIndex;
            var stop = context.Stop.TokenIndex;
            _startStops.Add(Tuple.Create(start, stop));
        }
    }
}
