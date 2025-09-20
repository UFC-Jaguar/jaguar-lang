using Common.Data;
using BackEnd;
using FrontEnd.Lexing;
using FrontEnd.Parsing;

namespace Common.Helpers {
    public class Running {
        public static MemoryManager Run(string fileName, string contentFromFileName) {
            //Generate Tokens
            var result = new MemoryManager();
            LexerTokens tokensSet = LexerRun(fileName, contentFromFileName);
            if (tokensSet.Error != null) {
                result.Value = null;
                result.Error = tokensSet.Error;
                return result;
            }

            //Generate AST
            AstInfo ast = ParserRun(tokensSet);
            if (ast.Error != null) {
                result.Value = null;
                result.Error = ast.Error;
                return result;
            }
            //RunLexer program
            Interpreter interpreter = new Interpreter();
            JMemory memory = new JMemory("<program>");
            result = interpreter.Visit(ast.Node, memory);
            return result;
        }
        public static LexerTokens LexerRun(string fileName, string contentFromFileName) { // Gerar Tokens
            Lexer lexer = new Lexer(fileName, contentFromFileName);
            LexerTokens tokensSet = lexer.MakeTokens();
            if (tokensSet.Error != null)
                return new LexerTokens(null, tokensSet.Error);
            return tokensSet;
        }
        public static AstInfo ParserRun(LexerTokens tokensSet) { //# Gerar AST
            Parser parser = new Parser(tokensSet.Tokens);
            AstInfo ast = parser.Parsing();
            return ast;
        }
    }
}
