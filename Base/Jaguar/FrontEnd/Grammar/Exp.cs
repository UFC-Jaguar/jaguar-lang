using System;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;
using FrontEnd.Lexing;

namespace FrontEnd.Grammar {
    public class Exp: Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
		    if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.LET])) {
			    parser.NextToken(ast);

			    if (parser.Current.Type != Consts.ID){
				    return ast.Fail(new TError(
					    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected identifier"
				    ));
                }
                Token var_name = GetCurrentIdentifierGoToEQ(parser, ast);
                if (parser.Current.Type != Consts.EQ) {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected '='"
                    ));
                }
                return VarAssign(parser, ast, var_name);
            }
            if (parser.Current.Type == Consts.ID) {
                if (parser.Lookahead(1).Type == Consts.EQ)
                    return VarAssign(parser, ast, GetCurrentIdentifierGoToEQ(parser, ast));
            }
            Tuple<string, string>[] tps = {
                new Tuple<string, string>(Consts.KEY, Consts.KEYS[Consts.IDX.AND]),
                new Tuple<string, string>(Consts.KEY, Consts.KEYS[Consts.IDX.OR]) };
            Visitor node = ast.Registry(NoOpBinaria.Perform(parser, new LogicExp(), tps));
            if (ast.Error != null) {
                return ast.Fail(new TError(
				    parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                    "Expected '" +
                    Consts.KEYS[Consts.IDX.LET]+"', '"+
                    Consts.KEYS[Consts.IDX.IF]+"', '"+ 
                    Consts.KEYS[Consts.IDX.FOR] + "', '" + 
                    Consts.KEYS[Consts.IDX.WHILE] + "', '" + 
                    Consts.KEYS[Consts.IDX.DEF] + "', int, float, identifier, '+', '-', '(', '[' Or '" + 
                    Consts.KEYS[Consts.IDX.NOT] + "'"
                ));
            }
            return ast.Success(node);
        }
        private AstInfo VarAssign(Parser parser, AstInfo ast, Token var_name) {
            parser.NextToken(ast);
            Visitor expr = ast.Registry(new Exp().Rule(parser));
            if (ast.Error != null) return ast;
            return ast.Success(new NoVarAssign(var_name, expr));
        }
        private Token GetCurrentIdentifierGoToEQ(Parser parser, AstInfo ast) {
            Token var_name = parser.Current;
            parser.NextToken(ast);
            return var_name;
        }
    }
}