using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;
using FrontEnd.Lexing;

namespace FrontEnd.Grammar {
    public class ForExp : Grammar {
        public AstInfo Rule(Parser parser) {
		    AstInfo ast = new AstInfo();

            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.FOR])) {
                return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.FOR] + "'"
                ));
            }
		    parser.NextToken(ast);

            if (parser.Current.Type != Consts.ID){
			    return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected identifier"
			    ));
            }
		    Token var_name = parser.Current;
            parser.NextToken(ast);

		    if (parser.Current.Type != Consts.EQ){
			    return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '='"
			    ));
		    }
		    parser.NextToken(ast);

            Visitor start_value = ast.Registry(new Exp().Rule(parser));
		    if (ast.Error!=null) return ast;

		    if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.TO])) { 
			    return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.TO] + "'"
                ));
		    }
		    parser.NextToken(ast);

            Visitor end_value = ast.Registry(new Exp().Rule(parser));
		    if (ast.Error!=null) return ast;

            Visitor step_value = null;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.STEP])) {
			    parser.NextToken(ast);

			    step_value = ast.Registry(new Exp().Rule(parser));
			    if (ast.Error!=null) return ast;
            }

		    if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DO])) {
                return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.DO] + "'"
                ));
            }
            parser.NextToken(ast);
            /* MultiLine */
            if (parser.Current.Type == Consts.NEWLINE){
              parser.NextToken(ast);

              Visitor true_body = ast.Registry(new Statements().Rule(parser)); 
              if (ast.Error!=null) return ast;
              if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                ));
              }
              parser.NextToken(ast);

              return ast.Success(new NoFOR(var_name, start_value, end_value, step_value, true_body, true));
            }
            /* End MultiLine */

            Visitor body = ast.Registry(new Stm().Rule(parser)); // ???
		    if (ast.Error!=null) return ast;

		    return ast.Success(new NoFOR(var_name, start_value, end_value, step_value, body, false));
        }
    }
}
