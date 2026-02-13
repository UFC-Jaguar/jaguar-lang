using System.Collections.Generic;
using Common.Data;
using Common.Nodes;
using Common.Errors;
using FrontEnd.Parsing;

namespace FrontEnd.Grammar {
    public class IfElif : Grammar {
        public List<NoIF.NoDataIFs> ConditionsCase { get; set; }
        public IfElif(List<NoIF.NoDataIFs> cc) { this.ConditionsCase = cc; }
        public AstInfo Rule(Parser parser) {
            var ast = this.IFNODE(parser, Consts.KEYS[Consts.IDX.ELIF]);
            Visitor condition = ast.Node;
            return (new IfCase(condition, this.ConditionsCase)).Rule(parser);
        }
        private AstInfo IFNODE(Parser parser, string _key) {
            AstInfo ast = new AstInfo();
            if (!parser.Current.Matches(Consts.KEY, _key)) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + _key + "'"
                ));
            }
            parser.NextToken(ast);

            Visitor condition = ast.Registry(new Exp().Rule(parser));
            if (ast.Error != null)
                return ast;
            return ast.Success(condition);
        }
    }
}