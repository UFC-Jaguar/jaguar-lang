using System.Collections.Generic;
using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;
using Common.Errors;

namespace FrontEnd.Grammar {
    public class IfExp : Grammar {
        public AstInfo Rule(Parser parser) {
            AstInfo ast = new AstInfo();
            Node ifNode = ast.Registry(this.IFNODE(parser, Consts.KEYS[Consts.IDX.IF]));
            if (ast.Error != null) return ast;
            var cases = ((NoIF)ifNode).IFCases;
            var elseStatement = ((NoIF)ifNode).Else;
            return ast.Success(new NoIF(cases, elseStatement));
        }
        private AstInfo IFNODE(Parser parser, string keyIF_ELIF) {
            AstInfo ast = new AstInfo();
            List<NoIF.NoDataIFs> dataIfs = new List<NoIF.NoDataIFs>();
            NoIF.NoElse noElse = null;
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

            //
            bool needReturnNull = true;
            Visitor statements = ast.Registry((new Statements()).Rule(parser));
            if (ast.Error != null)
                return ast;
            dataIfs.Add(NoIF.DataIFInstance(condition, statements, needReturnNull));
            //

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                parser.NextToken(ast);
                return ast.Success(new NoIF(dataIfs, noElse));
            }
            // Case ELIF_Else
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELIF])) { // elif
                Node noIF = ast.Registry(this.Call_Elif(parser));
                if (ast.Error != null)
                    return ast;
                dataIfs.AddRange(((NoIF)noIF).IFCases);
                return ast.Success(new NoIF(dataIfs, ((NoIF)noIF).Else));
            }
            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) { // else
                Node noIF = ast.Registry(this.Call_Else(parser));
                if (ast.Error != null)
                    return ast;
                dataIfs.AddRange(((NoIF)noIF).IFCases);
                return ast.Success(new NoIF(dataIfs, ((NoIF)noIF).Else));
            }
            return ast.Fail(new TError( // Falha
                parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                "Expected '" + Consts.KEYS[Consts.IDX.IF] + "'" + Consts.KEYS[Consts.IDX.ELIF]+"' or " + Consts.KEYS[Consts.IDX.ELSE]
            ));
        }
        private AstInfo Call_Elif(Parser parser) {
            AstInfo ast = new AstInfo();
            NoIF.NoElse noElse;

            Node noIF = ast.Registry(this.IFNODE(parser, Consts.KEYS[Consts.IDX.ELIF]));
            if (ast.Error != null) return ast;
            var dataIfs = ((NoIF)noIF).IFCases;
            noElse = ((NoIF)noIF).Else;
            return ast.Success(new NoIF(dataIfs, noElse));
        }
        private AstInfo Call_Else(Parser parser) {
            AstInfo ast = new AstInfo();
            var dataIfs = new List<NoIF.NoDataIFs>();
            NoIF.NoElse noElse = null;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) {
                parser.NextToken(ast);

                Visitor statements = ast.Registry(new Statements().Rule(parser));
                if (ast.Error != null)
                    return ast;
                noElse = NoIF.ElseInstance(statements, true);

                if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                    parser.NextToken(ast);
                } else {
                    return ast.Fail(new TError(
                        parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                        "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                    ));
                }
            }
            Visitor no = ast.Registry(ast.Success(noElse));
            if (ast.Error != null)
                return ast;
            noElse = (no != null && no.GetType() == typeof(NoIF.NoElse)) ? (NoIF.NoElse)no : null;
            return ast.Success(new NoIF(dataIfs, noElse));
        }
    }
}


