using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Helpers;
using Common.Errors;
using FrontEnd.Lexing;

namespace FrontEnd.Grammar {
    public class Atom: Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            Token tok = parser.Current;
            if (Util.Em(tok.Type, Consts.INT, Consts.FLOAT)) {
                parser.NextToken(ast);
                return ast.Success(new NoNumber(tok));
            } else if(tok.Type == Consts.STRING){
			    parser.NextToken(ast);
			    return ast.Success(new NoString(tok));
            } else if (tok.Type == Consts.ID){
                parser.NextToken(ast);
                return ast.Success(new NoVarAccess(tok));
            } else if (tok.Type == Consts.LPAR) {
                parser.NextToken(ast);
                Visitor expr = ast.Registry(new Exp().Rule(parser));
                if (ast.Error!=null) return ast;
                if (parser.Current.Type == Consts.RPAR) {
                    parser.NextToken(ast);
                    return ast.Success(expr);
                } else {
                    return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Did you think about ')'?"
                    ));
                }
            } else if (tok.Type == Consts.LSQR){
                Visitor listExp = ast.Registry(new ListExp().Rule(parser)); 
              if (ast.Error!=null) return ast;
              return ast.Success(listExp);
            } else if (tok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.IF])){
                Visitor ifExp = ast.Registry(new IfExp().Rule(parser));
			    if (ast.Error!=null) return ast;
			    return ast.Success(ifExp);
            } else if (tok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.FOR])){
                Visitor forExp = ast.Registry(new ForExp().Rule(parser));
			    if (ast.Error!=null) return ast;
			    return ast.Success(forExp);
            } else if (tok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.WHILE])) {
                Visitor whileExp = ast.Registry(new WhileExp().Rule(parser));
			    if (ast.Error!=null) return ast;
			    return ast.Success(whileExp);
            } else if (tok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DEF])) {
                Visitor funcDef = ast.Registry(new FuncDef().Rule(parser));
			    if (ast.Error!=null) return ast;
			    return ast.Success(funcDef);
            } else if (tok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.FUN])) {
                Visitor funcFun = ast.Registry(new FuncFun().Rule(parser));
                if (ast.Error != null) return ast;
                return ast.Success(funcFun);
            }
            return ast.Fail(new TError(
			    tok.NOIni, tok.NOEnd, TError.ESyntax,
                "Did you think about number, identifier, '+', '-', '(', '[', '" + 
                Consts.KEYS[Consts.IDX.IF] +"', '"+
                Consts.KEYS[Consts.IDX.FOR] +"', '"+ 
                Consts.KEYS[Consts.IDX.WHILE] +"', '"+ 
                Consts.KEYS[Consts.IDX.DEF] +"'?"
            ));
        }
    }
}
