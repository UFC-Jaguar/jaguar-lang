using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class AtomCall : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.PFUN])) {
                return PCALL(parser, ast);
            }
            return ATOM(parser, ast);
        }
        public AstInfo ATOM(Parser parser, AstInfo ast) {
            Visitor atom = ast.Registry(new Atom().Rule(parser));
		    if (ast.Error!=null) return ast;

            if (parser.Current.Type == Consts.LPAR) {
		        parser.NextToken(ast);
		        var argsNode = new List<Visitor>();
		        if (parser.Current.Type == Consts.RPAR){
			        parser.NextToken(ast);
                } else {
                    argsNode.Add(ast.Registry(new Exp().Rule(parser)));
			        if (ast.Error!=null){
				        return ast.Fail(new TError(
					        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                            "Expected ')', '" + 
                            Consts.KEYS[Consts.IDX.LET] +"', '"+ 
                            Consts.KEYS[Consts.IDX.IF] +"', '"+
                            Consts.KEYS[Consts.IDX.FOR] +"', '"+
                            Consts.KEYS[Consts.IDX.WHILE]+"', '"+
                            Consts.KEYS[Consts.IDX.DEF]+"', int, float, identifier, '+', '-', '(', '[' Or '"+
                            Consts.KEYS[Consts.IDX.NOT]+"'"
                        ));
                    }
			        while (parser.Current.Type == Consts.COMMA){
				        parser.NextToken(ast);

                        argsNode.Add(ast.Registry(new Exp().Rule(parser)));
				        if (ast.Error!=null) return ast;
                    }
			        if (parser.Current.Type != Consts.RPAR){
				        return ast.Fail(new TError(
					        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax, "Expected ',' Or ')'"
				        ));
                    }
			        parser.NextToken(ast);
                }
		        return ast.Success(new NoAtomCall(atom, argsNode.ToArray()));
            }
	        return ast.Success(atom);
        }
        public AstInfo PCALL(Parser parser, AstInfo ast) {
            parser.NextToken(ast);
            if (parser.Current.Type == Consts.DOT) {
                parser.NextToken(ast);
                Visitor atom = ast.Registry(new PAtomCall().Rule(parser));
                if (ast.Error != null) return ast;
                return ast.Success(atom);
            }
            return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax, "Invalid Syntax Expected '.'"));
        }
    }
}
