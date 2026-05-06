using System.Collections.Generic;
using Common.Data;
using Common.Nodes;
using Common.Errors;
using FrontEnd.Parsing;

namespace FrontEnd.Grammar {
    public class IfExp : Grammar {
        protected string if_elsif_name = "";
        public List<NoIF.NoDataIFs> BufferNoIF { get; set; }
        public IfExp(string if_elsif_name, List<NoIF.NoDataIFs> buffer) { 
            this.if_elsif_name= if_elsif_name;
            this.BufferNoIF = buffer;
        }
        private AstInfo GetCondition(Parser parser) {
            AstInfo ast = new AstInfo();
            if (!parser.Current.Matches(Consts.KEY, this.if_elsif_name)) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + this.if_elsif_name + "'"
                ));
            }
            parser.NextToken(ast);

            Visitor condition = ast.Registry(new Exp().Rule(parser));
            ast.Node = condition;
            return ast;
        }
        public AstInfo Rule(Parser parser) {
            var ast = this.GetCondition(parser);
            if (ast.Error != null) return ast;
            Visitor condition = ast.Node;
            return (new IfBody(condition, this.BufferNoIF)).Rule(parser);
        }
    }
}


/*
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
    public class IfExp : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            Visitor if_expression = ast.update_and_get_value(this.GetCondition(parser, Consts.KEYS[Consts.IDX.IF]));
            if (ast.Error != null)
                return ast;
            return ast.SetDefaultAndNewTValue(if_expression);
        }
        private AstInfo GetCondition(Parser parser, string keyIF_ELIF) {
            AstInfo ast = new AstInfo();
            List<NoIF.NoDataIFs> conditions_case = new List<NoIF.NoDataIFs>();
            if (!parser.Current.Matches(Consts.KEY, keyIF_ELIF)) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + keyIF_ELIF + "'"
                ));
            }
            parser.NextToken(ast);

            Visitor condition = ast.update_and_get_value(new Exp().Rule(parser));
            if (ast.Error != null)
                return ast;
            if (!parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.DO])) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + Consts.KEYS[Consts.IDX.DO] + "'"
                ));
            }
            parser.NextToken(ast);

            bool needReturnNull = true;
            Visitor statements = ast.update_and_get_value((new Statements()).Rule(parser));
            if (ast.Error != null)
                return ast;
            conditions_case.Add(NoIF.DataIFInstance(condition, statements, needReturnNull));

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                parser.NextToken(ast);
                return ast.SetDefaultAndNewTValue(new NoIF(conditions_case, null));
            }
            // Case ELIF_Else
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELIF])) { // elif
                Visitor noElif = ast.update_and_get_value(this.GetCondition(parser, Consts.KEYS[Consts.IDX.ELIF]));
                if (ast.Error != null)
                    return ast;
                conditions_case.AddRange(((NoIF)noElif).IFCases);
                return ast.SetDefaultAndNewTValue(new NoIF(conditions_case, ((NoIF)noElif).Else));
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) { // else
                Visitor noElse = ast.update_and_get_value(this.Call_Else(parser));
                if (ast.Error != null)
                    return ast;
                conditions_case.AddRange(((NoIF)noElse).IFCases);
                return ast.SetDefaultAndNewTValue(new NoIF(conditions_case, ((NoIF)noElse).Else));
            }
            return ast.Fail(new TError( // Falha
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + Consts.KEYS[Consts.IDX.IF] + "'" + Consts.KEYS[Consts.IDX.ELIF] + "' or " + Consts.KEYS[Consts.IDX.ELSE]
            ));
        }
        private AstInfo Call_Else(Parser parser) {
            AstInfo ast = new AstInfo();
            NoIF.NoElse noElse = null;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) {
                parser.NextToken(ast);

                Visitor statements = ast.update_and_get_value(new Statements().Rule(parser));
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
            return ast.SetDefaultAndNewTValue(new NoIF(new List<NoIF.NoDataIFs>(), noElse));
        }
    }
}

 */