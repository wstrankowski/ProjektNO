using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektNO.Halstead
{
    class HalsteadCalculator
    {
        private readonly int _n1;
        private readonly int _n2;
        private readonly int _L1;
        private readonly int _L2;
        public int n1 { get { return _n1; } }
        public int n2 { get { return _n2; } }
        public int L1 { get { return _L1; } }
        public int L2 { get { return _L2; } }
        public HalsteadCalculator(IDictionary<string, int> operatorToCount, IDictionary<string, int> operandToCount)
        {
            _n1 = GetUniqueKeys(operatorToCount);
            _n2 = GetUniqueKeys(operandToCount);
            _L1 = GetTotalCount(operatorToCount);
            _L2 = GetTotalCount(operandToCount);
        }

        private int GetUniqueKeys(IDictionary<string, int> dictionary)
        {
            return dictionary.Keys.Count();
        }

        private int GetTotalCount(IDictionary<string, int> dictionary)
        {
            var total = 0;
            foreach (var key in dictionary.Keys)
            {
                total += dictionary[key];
            }
            return total;
        }

        public int l => _n1 + _n2;
        public int L => _L1 + _L2;
        public double V => L * Math.Log(_n1 + _n2, 2);
        public double T => _n2 != 0
            ? _n1 * _L2 / (2 * _n2)
            : double.PositiveInfinity;
        public double E => V * T;
        public double N => V / 3000;

    }
}
