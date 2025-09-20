using FrontEnd.Lexing;
using FrontEnd.Parsing;
using System;

namespace Common.Environment {
    public class Runner {
        public static string prompt = "Jaguar";
        public static LexerTokens RunLexer(string fn, string text) { // Gerar Tokens
            Lexer lexer = new Lexer(fn, text);
            LexerTokens lr = lexer.MakeTokens();
            if (lr.Error != null)
                return new LexerTokens(null, lr.Error);
            return lr;
        }
        public static AstInfo RunParser(LexerTokens lr) { //# Gerar AST
            Parser parser = new Parser(lr.Tokens);
            AstInfo ast = parser.Parsing();
            return ast;
        }
    }
}
