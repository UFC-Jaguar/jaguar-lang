using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class Statements : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            List<Visitor> statements = new List<Visitor>();
            JSource scIni = parser.Current.NOIni.Copy();
            while (parser.Current.Type == Consts.NEWLINE) {
                parser.NextToken(ast);
            }
            Visitor statement = ast.Registry(new Stm().Rule(parser));
            if (ast.Error != null) return ast;
            statements.Add(statement);
            //bool moreStatements = true; // Adicionar mais stms/exp separados por newline
            bool abort = true;
            while (true) {
                //int newlines = 0; // Quantidade de novas linhas
                while (parser.Current.Type == Consts.NEWLINE) {
                    parser.NextToken(ast);
                    //newlines += 1;
                    abort = false;
                }
                //if (newlines == 0) {
                //    moreStatements = false;
                //}
                //if (!moreStatements) { break; }
                if (parser.Current.MatchesValues(Consts.KEY, Consts.BYPASS_STATEMENTS)){
                    break; // Ignore control keys;
                }
                if (parser.Current.Type == Consts.EOF){
                    break; // Ignore IOF;
                }
                if (abort) break;// remove multi stm inline (ex: 1 2 3): TODO
                statement = ast.getNullIfError_YouCanBackTraking(new Stm().Rule(parser));// Se nao statement entao parar (moreStatements==false)
                if (ast.Error != null || statement == null) { //se nao statement
                    //parser.Reverse(ast.ToReverseCount);
                    //moreStatements = false;
                    //continue;
                    string msg = "";
                    if (ast.Error != null)
                        msg = ast.Error.ToString();
                    return ast.Fail(new TError(parser.Current.NOIni, parser.Current.NOEnd, "Syntax", msg + "\n    Statements Error"));
                }
                statements.Add(statement);
                abort = true;
            }
            return ast.Success(new NoList( // Uma lista de retornos
              statements,
              scIni,
              parser.Current.NOEnd.Copy()
            ));
        }
    }
}
