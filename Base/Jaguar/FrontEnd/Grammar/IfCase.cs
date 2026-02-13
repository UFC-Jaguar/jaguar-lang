using System.Collections.Generic;
using Common.Data;
using Common.Errors;
using Common.Nodes;
using FrontEnd.Parsing;

namespace FrontEnd.Grammar {
    public class IfCase : Grammar {
        public Visitor Condition { get; set; }
        public List<NoIF.NoDataIFs> ConditionsCase { get; set; }
        public IfCase(Visitor condition, List<NoIF.NoDataIFs> conditionsCase) {
            this.Condition = condition;
            this.ConditionsCase = conditionsCase;
        }
        public AstInfo Rule(Parser parser) {
            return this.IFNODE(parser);
        }
        private AstInfo IFNODE(Parser parser) {
            AstInfo ast = new AstInfo();
            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DO])) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + Consts.KEYS[Consts.IDX.DO] + "'"
                ));
            }
            parser.NextToken(ast);

            bool needReturnNull = true;
            Visitor statements = ast.Registry((new Statements()).Rule(parser));
            if (ast.Error != null)
                return ast;
            this.ConditionsCase.Add(NoIF.DataIFInstance(this.Condition, statements, needReturnNull));

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                parser.NextToken(ast);
                return ast.Success(new NoIF(this.ConditionsCase, null));
            }
            // Case ELIF_Else
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELIF])) { // elif
                return new IfElif(this.ConditionsCase).Rule(parser);
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) { // else
                return new IfElse(this.ConditionsCase).Rule(parser);
            }
            return ast.Fail(new TError( // Falha
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + "'" + Consts.KEYS[Consts.IDX.ELIF] + "' or " + Consts.KEYS[Consts.IDX.ELSE]
            ));
        }
    }
}
