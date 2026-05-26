using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;
using FrontEnd.Lexing;
using System;

namespace FrontEnd.Grammar {
    public class ForInExp : Grammar {
        public AstInfo Rule(Parser parser) { // Its not used!
            throw new Exception("ForInExp.Rule(parser): Call ForInExp.RuleT(parser, TokenID)");
        }
        public AstInfo RuleT(Parser parser, Token idtoken) {
            AstInfo ast = new AstInfo();
            parser.NextToken(ast);

            Visitor list_exp_visitor = ast.Registry(new Exp().Rule(parser));
            if (ast.Error!=null) return ast;

            Visitor statements_body_visitor = ast.Registry(new Statements().Rule(parser));
            if (ast.Error!=null) return ast;

            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END]))
                return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                ));

            parser.NextToken(ast);
            return ast.Success(new NoForIn(idtoken, statements_body_visitor, list_exp_visitor));
        }
    }
}
