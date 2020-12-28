using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektNO.Halstead
{
    class TokensExtractor
    {
        public static bool SkipDeclarations;
        private readonly IList<IToken> _tokens;
        public IList<IToken> Tokens
        {
            get
            {
                return _tokens;
            }
        }
        public TokensExtractor(AntlrInputStream inputStream)
        {
            var commonTokenStream = GetCommonTokenStream(inputStream);
            _tokens = commonTokenStream.GetTokens();
            if (SkipDeclarations)
            {
                var tokensToRemove = GetTokensToRemove(commonTokenStream);
                RemoveTokens(tokensToRemove);
            }
        }

        private CommonTokenStream GetCommonTokenStream(AntlrInputStream inputStream)
        {
            CLexer lexer = new CLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            commonTokenStream.Fill();
            return commonTokenStream;
        }

        private IList<IToken> GetTokensToRemove(CommonTokenStream commonTokenStream)
        {
            var declarationListener = GetDeclarationListener(commonTokenStream);
            var tokensToRemove = new List<IToken>();
            foreach (var startStop in declarationListener.StartStops)
            {
                var tokens = commonTokenStream.GetTokens(startStop.Item1, startStop.Item2);
                if(IsMethodDeclaration(tokens))
                {
                    tokensToRemove.AddRange(tokens);
                }
            }
            return tokensToRemove;
        }

        private bool IsMethodDeclaration(IList<IToken> tokens) =>
            tokens.FirstOrDefault(x => x.Type == CLexer.LeftParen) != null;

        private static DeclarationListener GetDeclarationListener(CommonTokenStream commonTokenStream)
        {
            CParser parser = new CParser(commonTokenStream);
            parser.RemoveParseListeners();
            var declarationListener = new DeclarationListener();
            parser.AddParseListener(declarationListener);
            parser.blockItemList();
            return declarationListener;
        }

        private void RemoveTokens(IList<IToken> tokensToRemove)
        {
            foreach(var token in tokensToRemove)
            {
                _tokens.Remove(token);
            }
        }
    }
}
