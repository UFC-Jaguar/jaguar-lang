using System.Collections.Generic;
using FrontEnd.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;
using Common.Nodes;

namespace FrontEnd.Grammar {
    public class PAtomCall : Grammar {
        public AstInfo Rule(Parser parser) {
            return ATOM(parser, new AstInfo());
        }
        public AstInfo ATOM(Parser parser, AstInfo ast) {
            if (parser.Current.Type != Consts.ID)
                return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax, 
                    "Invalid Syntax PAtomCall Rule. It's should a identifier!"));
            NoVarAccess atom = (NoVarAccess) ast.Registry(new Atom().Rule(parser));
            if (ast.Error != null) return ast;

            if (parser.Current.Type == Consts.LPAR) {
                parser.NextToken(ast);
                var argsNode = new List<Visitor>();
                if (parser.Current.Type == Consts.RPAR) {
                    parser.NextToken(ast);
                } else {
                    argsNode.Add(ast.Registry(new Exp().Rule(parser)));
                    if (ast.Error != null) {
                        return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                            "Invalid Syntax Expected ')'|'LET'|'IF'|'FOR'|'WHILE'|'DEFFUN'|int|float|identifier|'OPERATOR'|'('|'NOT'"
                        ));
                    }
                    while (parser.Current.Type == Consts.COMMA) {
                        parser.NextToken(ast);

                        argsNode.Add(ast.Registry(new Exp().Rule(parser)));
                        if (ast.Error != null) return ast;
                    }
                    if (parser.Current.Type != Consts.RPAR) {
                        return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                            "Invalid Syntax Expected ',' Or ')'"
                        ));
                    }
                    parser.NextToken(ast);
                }
                if (atom.GetType() == typeof(NoVarAccess)) {
                    string metodo = atom.VarNameTOK.Value;
                    return ast.Success(new NoPAtomCall(atom, argsNode.ToArray(), metodo));
                } else {
                    return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax, 
                        "Invalid Syntax PAtomCall Rule"));
                }
            }
            return ast.Success(atom);
        }
    }
}
