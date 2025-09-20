using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class ListExp : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            var element_nodes = new List<Visitor>();
            JSource scIni = parser.Current.NOIni.Copy();

            if (parser.Current.Type != Consts.LSQR) { // Ja checamos isso Em Atom
              return ast.Fail(new TError(
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '['"
              ));
            }
            parser.NextToken(ast);

            if (parser.Current.Type == Consts.RSQR){ // TList vazia
              parser.NextToken(ast);
            } else{
              element_nodes.Add(ast.Registry(new Exp().Rule(parser)));
              if (ast.Error!=null){
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected ']', '" + 
                  Consts.KEYS[Consts.IDX.LET] + "', '" + 
                  Consts.KEYS[Consts.IDX.IF] + "', '" + 
                  Consts.KEYS[Consts.IDX.FOR] + "', '" + 
                  Consts.KEYS[Consts.IDX.WHILE] + "', '" + 
                  Consts.KEYS[Consts.IDX.DEF] + "', int, float, identifier, '+', '-', '(', '[' Or '" + 
                  Consts.KEYS[Consts.IDX.NOT] + "'"
                ));
              }
              while (parser.Current.Type == Consts.COMMA){
                parser.NextToken(ast);

                element_nodes.Add(ast.Registry(new Exp().Rule(parser)));
                if (ast.Error!=null) return ast;
              }
              if (parser.Current.Type != Consts.RSQR){
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected ',' Or ']'"
                ));
              }
              parser.NextToken(ast);
            }
            return ast.Success(new NoList(
              element_nodes,
              scIni,
              parser.Current.NOEnd.Copy()
            ));
        }
    }
}

