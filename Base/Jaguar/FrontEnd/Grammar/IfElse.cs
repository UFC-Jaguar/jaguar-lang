//using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Common.Nodes.NoIF;
using Common.Data;
using Common.Errors;
using Common.Nodes;
using FrontEnd.Parsing;
using System.Collections.Generic;

namespace FrontEnd.Grammar {
    public class IfElse : Grammar {
        public List<NoIF.NoDataIFs> ConditionsCase { get; set; }
        public IfElse(List<NoIF.NoDataIFs> cc) {
            this.ConditionsCase = cc;
        }
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            Visitor noElse = ast.Registry(this.Call_Else(parser));
            if (ast.Error != null)
                return ast;
            return ast.Success(new NoIF(this.ConditionsCase, ((NoIF)noElse).Else));
        }
        private AstInfo Call_Else(Parser parser) {
            AstInfo ast = new AstInfo();
            NoIF.NoElse noElse = null;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) {
                parser.NextToken(ast);

                Visitor statements = ast.Registry(new Statements().Rule(parser));
                if (ast.Error != null)
                    return ast;
                noElse = NoIF.ElseInstance(statements, true);

                if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                    ));
                }
                parser.NextToken(ast);
            }
            return ast.Success(new NoIF(this.ConditionsCase, noElse));
        }
    }
}
