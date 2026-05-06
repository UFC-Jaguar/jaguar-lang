using System.Collections.Generic;
using Common.Data;
using Common.Errors;
using Common.Nodes;
using FrontEnd.Parsing;

namespace FrontEnd.Grammar {
    public class IfBody : Grammar {
        public Visitor Condition { get; set; }
        public List<NoIF.NoDataIFs> BufferNoIF { get; set; }
        public IfBody(Visitor condition, List<NoIF.NoDataIFs> buffer) {
            this.Condition = condition;
            this.BufferNoIF = buffer;
        }
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            //if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DO])) {
            //    return ast.Fail(new TError(
            //      parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
            //      "Expected '" + Consts.KEYS[Consts.IDX.DO] + "'"
            //    ));
            //}
            //parser.NextToken(ast);

            //bool needReturnNull = true;
            Visitor statements = ast.Registry((new Statements()).Rule(parser));
            if (ast.Error != null)
                return ast;
            this.BufferNoIF.Add(NoIF.DataIFInstance(this.Condition, statements));

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                parser.NextToken(ast);
                return ast.Success(new NoIF(this.BufferNoIF, null));
            }
            // Case ELIF_Else
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELIF])) { // elif
                return new IfExp(Consts.KEYS[Consts.IDX.ELIF], this.BufferNoIF).Rule(parser);
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) { // else
                return new IfElse(this.BufferNoIF).Rule(parser);
            }
            return ast.Fail(new TError( // Falha
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + "'" + Consts.KEYS[Consts.IDX.ELIF] + "' or " + Consts.KEYS[Consts.IDX.ELSE]
            ));
        }
    }
}
