using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;

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
            bool moreStatements = true; // Adicionar mais stms/exp separados por newline
            while (true) {
                int newlines = 0; // Quantidade de novas linhas
                while (parser.Current.Type == Consts.NEWLINE) {
                    parser.NextToken(ast);
                    newlines += 1;
                }
                if (newlines == 0) {
                    moreStatements = false;
                }
                if (!moreStatements) { break; }
                statement = ast.TryRegister(new Stm().Rule(parser));// Se nao statement entao parar (moreStatements==false)
                if (statement == null) { //se nao statement
                    parser.Reverse(ast.ToReverseCount);
                    moreStatements = false;
                    continue;
                }
                statements.Add(statement);
            }
            return ast.Success(new NoList( // Uma lista de retornos
              statements,
              scIni,
              parser.Current.NOEnd.Copy()
            ));
        }
    }
}
