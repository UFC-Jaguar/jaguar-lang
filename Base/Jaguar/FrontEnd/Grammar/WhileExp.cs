using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class WhileExp : Grammar {
        public AstInfo Rule(Parser parser) {
		    AstInfo ast = new AstInfo();
            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.WHILE])) {
                return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.WHILE] + "'"
                ));
            }
		    parser.NextToken(ast);

            Visitor condition = ast.Registry(new Exp().Rule(parser));
		    if (ast.Error!=null) 
                return ast;

		    if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DO])) {
                return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.DO] + "'"
                ));
            }
		    parser.NextToken(ast);

            /* Multilines */
            if (parser.Current.Type == Consts.NEWLINE){
              parser.NextToken(ast);

              Visitor statements = ast.Registry(new Statements().Rule(parser)); 
              if (ast.Error!=null) return ast;

              if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) { 
                return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                ));
              }
              parser.NextToken(ast);

              return ast.Success(new NoWhile(condition, statements, true));
            }
            /*End Multilines */
            Visitor stm = ast.Registry(new Stm().Rule(parser)); // Inline
		    if (ast.Error!=null) 
                return ast;
		    return ast.Success(new NoWhile(condition, stm, false));
        }
    }
}
