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
		    Token idToken = parser.Current;
            parser.NextToken(ast);

		    if (parser.Current.Type != Consts.EQ){
                //if (parser.Current.Matches(Consts.KEY, Consts.IN)) return (new ForInExp().Rule(idtoken));
			    
                return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax, "Expected '='"));
		    }
		    parser.NextToken(ast);

            Visitor start_exp_visitor = ast.Registry(new Exp().Rule(parser));
		    if (ast.Error!=null) return ast;

		    if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.COLON])) { 
			    return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.COLON] + "'"
                ));
		    }
		    parser.NextToken(ast);

            Visitor stop_exp_visitor = ast.Registry(new Exp().Rule(parser));
		    if (ast.Error!=null) return ast;

            Visitor incremental_exp_visitor = null;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.COLON])) {
			    parser.NextToken(ast);

                incremental_exp_visitor = stop_exp_visitor;

                stop_exp_visitor = ast.Registry(new Exp().Rule(parser));
			    if (ast.Error!=null) return ast;
            }

		    //if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DO])) {
            //    return ast.Fail(new TError(
			//	    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
            //        "Expected '" + Consts.KEYS[Consts.IDX.DO] + "'"
            //    ));
            //}
            //parser.NextToken(ast);
            /* MultiLine */
            //if (parser.Current.Type == Consts.NEWLINE){
            //parser.NextToken(ast);

            Visitor statements_body_visitor = ast.Registry(new Statements().Rule(parser)); 
            if (ast.Error!=null) return ast;
            
            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
            return ast.Fail(new TError(
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
            ));
            }
            parser.NextToken(ast);

            return ast.Success(new NoFOR(idToken, start_exp_visitor, stop_exp_visitor, incremental_exp_visitor, statements_body_visitor));
            //}
            /* End MultiLine */
            
            /*
            Visitor body = ast.Registry(new Stm().Rule(parser));
		    if (ast.Error!=null) return ast;

            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                ));
            }
            parser.NextToken(ast);

            return ast.Success(new NoFOR(idToken, start_exp_visitor, stop_exp_visitor, incremental_exp_visitor, body, false));
            */
        }
    }
}
