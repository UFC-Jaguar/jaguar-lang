using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;
using FrontEnd.Lexing;

namespace FrontEnd.Grammar {
    public class FuncDef : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();/*
            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DEF])) {
                return ast.Fail(new TError(
                    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" + Consts.KEYS[Consts.IDX.DEF] + "'"
                ));
            }*/
            parser.NextToken(ast);
            Token var_name_tok = null;
            if (parser.Current.Type == Consts.ID) {
                var_name_tok = parser.Current;
                parser.NextToken(ast);
                if (parser.Current.Type != Consts.LPAR) {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected '('"
                    ));
                }
            } else {
                if (parser.Current.Type != Consts.LPAR) {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected identifier Or '('"
                    ));
                }
            }
		    parser.NextToken(ast);
		    var arg_name_toks = new List<Token>();

		    if (parser.Current.Type == Consts.ID){
			    arg_name_toks.Add(parser.Current);
			    parser.NextToken(ast);

                while (parser.Current.Type == Consts.COMMA) {
                    parser.NextToken(ast);

                    if (parser.Current.Type != Consts.ID) {
                        return ast.Fail(new TError(
                            parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                            "Expected identifier"
                        ));
                    }
                    arg_name_toks.Add(parser.Current);
                    parser.NextToken(ast);
                }
                if (parser.Current.Type != Consts.RPAR) {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected ',' Or ')'"
                    ));
                }
		    } else{
                if (parser.Current.Type != Consts.RPAR) {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected identifier Or ')'"
                    ));
                }
            }
		    parser.NextToken(ast);
            /* Multilines */
            if (parser.Current.Type == Consts.ARROW){
              parser.NextToken(ast);

              Visitor false_body = ast.Registry(new Exp().Rule(parser));
              if (ast.Error!=null) return ast;

              return ast.Success(new NoFuncDef(
                var_name_tok,
                arg_name_toks.ToArray(),
                false_body,
                true
              ));
            }
            if (parser.Current.Type != Consts.NEWLINE){
              return ast.Fail(new TError(
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" +Consts.ARROW+"' Or "+Consts.NEWLINE
              ));
            }
            parser.NextToken(ast);

            //var Tok = parser.Current;//apagar
            Visitor body = ast.Registry(new Statements().Rule(parser));
            if (ast.Error!=null) return ast;

            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
              return ast.Fail(new TError(
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
              ));
            }
            parser.NextToken(ast);

            return ast.Success(new NoFuncDef(
              var_name_tok,
              arg_name_toks.ToArray(),
              body,
              false
            ));
        }
    }
}
