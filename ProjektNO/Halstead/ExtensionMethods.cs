using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektNO.Halstead
{
    public static class ExtensionMethods
    {
        private static readonly int FirstOperator = CLexer.LeftParen;
        private static readonly int LastOperator = CLexer.Ellipsis;
        private static readonly IEnumerable<int> NonOperators = new List<int>
        {
            CLexer.Colon,
            CLexer.Semi
        };

        private static readonly IEnumerable<int> AdditionalOperators = new List<int>
        {
            CLexer.Break,
            CLexer.Case,
            CLexer.Continue,
            CLexer.Default,
            CLexer.Do,
            CLexer.Else,
            CLexer.For,
            CLexer.Goto,
            CLexer.If,
            CLexer.Return,
            CLexer.Sizeof,
            CLexer.Switch,
            CLexer.While,
            CLexer.Double,
            CLexer.Char,
            CLexer.Float,
            CLexer.Int,
            CLexer.Long,
            CLexer.Short,
            CLexer.Bool,
            CLexer.Void,
        };

        private static readonly IEnumerable<int> Operands = new List<int>
        {
            CLexer.Identifier,
            CLexer.Constant,
            CLexer.DigitSequence,
            CLexer.StringLiteral,
        };
        public static IList<IToken> GetOperators(this IList<IToken> tokens)
        {
            return tokens.Where(
                    t => t.Type >= FirstOperator 
                        && t.Type <= LastOperator
                        && !NonOperators.Contains(t.Type)
                    || AdditionalOperators.Contains(t.Type)
                    || IsMethodInvocation(tokens, t)
                ).ToList();
        }

        public static IList<IToken> GetOperands(this IList<IToken> tokens)
        {
            return tokens.Where(
                    t => Operands.Contains(t.Type)
                    && !IsMethodInvocation(tokens, t)
                ).ToList();
        }

        private static bool IsMethodInvocation(IList<IToken> tokens, IToken token)
        {
            if(token.Type != CLexer.Identifier)
            {
                return false;
            }
            var indexOfToken = tokens.IndexOf(token);
            if (indexOfToken + 1 < tokens.Count)
            {
                var nextToken = tokens[indexOfToken + 1];
                return nextToken.Type == CLexer.LeftParen;
            }
            return false;
        }

    }
}
