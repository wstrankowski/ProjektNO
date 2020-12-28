using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjektNO.Halstead
{
    class DictionariesCreator
    {
        private readonly IDictionary<string, int> _operatorToCount = new Dictionary<string, int>();
        private readonly IDictionary<string, int> _operandToCount = new Dictionary<string, int>();
        public IDictionary<string, int> OperatorToCount
        {
            get
            {
                return _operatorToCount;
            }
        }
        public IDictionary<string, int> OperandToCount
        {
            get
            {
                return _operandToCount;
            }
        }

        public DictionariesCreator(TokensExtractor tokensExtractor)
        {
            var tokens = tokensExtractor.Tokens;
            FillDictionaries(tokens);
            MergeSeparableOperators();
        }

        private void FillDictionaries(IList<IToken> tokens)
        {
            var items = tokens.GetOperators();
            FillDictionary(_operatorToCount, items);
            items = tokens.GetOperands();
            FillDictionary(_operandToCount, items);
        }

        private void FillDictionary(IDictionary<string, int> dictionary, IList<IToken> items)
        {
            foreach (var item in items)
            {
                var key = item.Text;
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, 1);
                }
                else
                {
                    dictionary[key]++;
                }
            }
        }

        private void MergeSeparableOperators()
        {
            MergeOperators("(", ")");
            MergeOperators("[", "]");
            MergeOperators("{", "}");
        }

        private void MergeOperators(string left, string right)
        {
            if (_operatorToCount.ContainsKey(left))
            {
                var count = _operatorToCount[left];
                _operatorToCount.Remove(left);
                _operatorToCount.Remove(right);
                _operatorToCount.Add(left + right, count);
            }
        }
    }
}
