using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class IfExp : Grammar {
        private readonly List<NoIF.NoDataIFs> conditions_case = new List<NoIF.NoDataIFs>();
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            Visitor if_expression = ast.Registry(this.IFNODE(parser, Consts.KEYS[Consts.IDX.IF]));
            if (ast.Error != null) return ast;
            return ast.Success(if_expression);
        }
        private AstInfo IFNODE(Parser parser, string keyIF_ELIF) {
            AstInfo ast = new AstInfo();
            if (!parser.Current.Matches(Consts.KEY, keyIF_ELIF)) {
                return ast.Fail(new TError(
                  parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                  "Expected '" + keyIF_ELIF + "'"
                ));
            }
            parser.NextToken(ast);

            Visitor condition = ast.Registry(new Exp().Rule(parser));
            if (ast.Error != null) return ast;
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
            conditions_case.Add(NoIF.DataIFInstance(condition, statements, needReturnNull));

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                parser.NextToken(ast);
                return ast.Success(new NoIF(conditions_case, null)); 
            }
            // Case ELIF_Else
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELIF])) { // elif
                Visitor noElif = ast.Registry(this.IFNODE(parser, Consts.KEYS[Consts.IDX.ELIF]));
                if (ast.Error != null)
                    return ast;
                return ast.Success(new NoIF(conditions_case, ((NoIF)noElif).Else));
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) { // else
                Visitor noElse = ast.Registry(this.Call_Else(parser));
                if (ast.Error != null)
                    return ast;
                return ast.Success(new NoIF(conditions_case, ((NoIF)noElse).Else));
            }
            return ast.Fail(new TError( // Falha
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + Consts.KEYS[Consts.IDX.IF] + "'" + Consts.KEYS[Consts.IDX.ELIF]+"' or " + Consts.KEYS[Consts.IDX.ELSE]
            ));
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
            return ast.Success(new NoIF(conditions_case, noElse));
        }
    }
}


