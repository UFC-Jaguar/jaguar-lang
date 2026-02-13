using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class Stm : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            JSource scIni = parser.Current.NOIni.Copy();

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.RETURN])) {
              parser.NextToken(ast);
              
              Visitor expReturn = ast.TryRegister(new Exp().Rule(parser));
              if (expReturn==null){
                parser.Reverse(ast.ToReverseCount);
              }
              return ast.Success(new NoReturn(expReturn, scIni, parser.Current.NOIni.Copy()));
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.CONTINUE])) {
              parser.NextToken(ast);
              return ast.Success(new NoContinue(scIni, parser.Current.NOIni.Copy()));
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.BREAK])) {
              parser.NextToken(ast);
              return ast.Success(new NoBreak(scIni, parser.Current.NOIni.Copy()));
            }
            Visitor exp = ast.Registry(new Exp().Rule(parser));
            if (ast.Error!=null){
              return ast; /*ast.Fail(new TError(
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + 
                Consts.KEYS[Consts.IDX.RETURN] + "', '" + 
                Consts.KEYS[Consts.IDX.CONTINUE] + "', '" + 
                Consts.KEYS[Consts.IDX.BREAK] + "', '" + 
                Consts.KEYS[Consts.IDX.LET] + "', '" + 
                Consts.KEYS[Consts.IDX.IF] + "', '" + 
                Consts.KEYS[Consts.IDX.FOR] + "', '" + 
                Consts.KEYS[Consts.IDX.WHILE] + "', '" + 
                Consts.KEYS[Consts.IDX.DEF] + "', int, float, identifier, '+', '-', '(', '[' Or '" + 
                Consts.KEYS[Consts.IDX.NOT] + "'"
              ));*/
            }
            return ast.Success(exp);
        }
    }
}
