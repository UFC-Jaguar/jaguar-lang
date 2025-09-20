using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;
using FrontEnd.Lexing;

namespace FrontEnd.Grammar {
    public class LogicExp : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.NOT])) {
                Token opTok = parser.Current;
			    parser.NextToken(ast);

                Visitor notLogic = ast.Registry(new LogicExp().Rule(parser));
			    if (ast.Error!=null) return ast;
			    return ast.Success(new NoOpUnaria(opTok, notLogic));
            }
            Visitor logic = ast.Registry(NoOpBinaria.Perform(parser, new ArithExp(), Consts.EE, Consts.NE, Consts.LT, Consts.GT, Consts.LTE, Consts.GTE));
		    if (ast.Error!=null){
			    return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected int, float, identifier, '+', '-', '(', '[', '" +
                    Consts.KEYS[Consts.IDX.IF]+"', '"+
                    Consts.KEYS[Consts.IDX.FOR]+"', '"+
                    Consts.KEYS[Consts.IDX.WHILE]+"', '"+
                    Consts.KEYS[Consts.IDX.DEF]+"' Or '"+
                    Consts.KEYS[Consts.IDX.NOT]+"'"
                ));
            }
		    return ast.Success(logic);
        }
    }
}
