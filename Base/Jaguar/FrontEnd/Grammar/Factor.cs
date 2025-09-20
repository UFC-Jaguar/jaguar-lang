using Common.Nodes;
using FrontEnd.Parsing;
using Common.Helpers;
using Common.Data;
using FrontEnd.Lexing;

namespace FrontEnd.Grammar {
    public class Factor: Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            Token tok = parser.Current;
            if (Util.Em(tok.Type, Consts.PLUS, Consts.MINUS)) {
                parser.NextToken(ast);
                Visitor factor = ast.Registry(this.Rule(parser));// Recursao uma vez apenas ???
                if (ast.Error != null) return ast;
                return ast.Success(new NoOpUnaria(tok, factor));
            }
            return new Pow().Rule(parser);//return this.power()
        }
    }
}
