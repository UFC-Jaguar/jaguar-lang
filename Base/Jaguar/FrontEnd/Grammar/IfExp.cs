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
            var cases = ((NoIF)ifNode).QueueIFs;
            var elseStatement = ((NoIF)ifNode).Else;
            return ast.Success(new NoIF(cases, elseStatement));
        }
        private AstInfo IFNODE(Parser parser, string keyIF_ELIF) {// Default is Inline. IF First
            AstInfo ast = new AstInfo();
            var dataIfs = new List<NoIF.NoDataIFs>();
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

            if (parser.Current.Type == Consts.NEWLINE) { // If new line
                parser.NextToken(ast);

                if (!NewCondition(ref ast, condition, new Statements(), dataIfs, true, parser)) 
                    return ast;

                if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                    parser.NextToken(ast);
                    return ast.Success(new NoIF(dataIfs, noElse));
                }
                if (!More_IFs_Else(ref ast, dataIfs, ref noElse, parser)) 
                    return ast;
                return ast.Success(new NoIF(dataIfs, noElse));
            } // Inline:
            if (!NewCondition(ref ast, condition, new Stm(), dataIfs, false, parser)) 
                return ast;

            if (!More_IFs_Else(ref ast, dataIfs, ref noElse, parser)) 
                return ast;

            return ast.Success(new NoIF(dataIfs, noElse));
        }
        private AstInfo ElifOrElse(Parser parser) {// Default is __ELSE
            AstInfo ast = new AstInfo();
            var dataIfs = new List<NoIF.NoDataIFs>();
            NoIF.NoElse noElse;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELIF])) {
                Node noIF = ast.Registry(this.IFNODE(parser, Consts.KEYS[Consts.IDX.ELIF]));
                if (ast.Error != null) return ast;
                dataIfs = ((NoIF)noIF).QueueIFs;
                noElse = ((NoIF)noIF).Else;
                return ast.Success(new NoIF(dataIfs, noElse));
            }
            Node no = ast.Registry(this.ElseExp(parser));
            if (ast.Error != null) return ast;
            noElse = (no != null && no.GetType() == typeof(NoIF.NoElse)) ? (NoIF.NoElse)no : null;
            return ast.Success(new NoIF(dataIfs, noElse));
        }
        private bool More_IFs_Else(ref AstInfo ast, List<NoIF.NoDataIFs> dataIFs, ref NoIF.NoElse noElse, Parser parser) {
            Node noIF = ast.Registry(this.ElifOrElse(parser));
            if (ast.Error != null) return false;
            List<NoIF.NoDataIFs> newDataIF = ((NoIF)noIF).QueueIFs;
            noElse = ((NoIF)noIF).Else;
            dataIFs.AddRange(newDataIF);
            return true;
        }
        private bool NewCondition(ref AstInfo ast, 
                                      Visitor condition, Grammar grammar, 
                                      List<NoIF.NoDataIFs> dataIFs, 
                                      bool needReturnNull, Parser parser) {

            Visitor expOrStatements = ast.Registry(grammar.Rule(parser));// TODO: Dá para melhorar ????
            if (ast.Error != null) 
                return false;
            dataIFs.Add(NoIF.DataIFInstance(condition, expOrStatements, needReturnNull));
            return true;
        }
        private AstInfo ElseExp(Parser parser) { // TODO: avaliar
            AstInfo ast = new AstInfo();
            NoIF.NoElse noElse = null;

            if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.ELSE])) {
                parser.NextToken(ast);

                if (parser.Current.Type == Consts.NEWLINE) { // New line
                    parser.NextToken(ast);

                    Visitor statements = ast.Registry(new Statements().Rule(parser));
                    if (ast.Error != null) return ast;
                    noElse = NoIF.ElseInstance(statements, true);

                    if (parser.Current.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.END])) {
                        parser.NextToken(ast);
                    } else {
                        return ast.Fail(new TError(
                          parser.Current.NOIni, parser.Current.NOEnd, TError.ESyntax,
                          "Expected '" + Consts.KEYS[Consts.IDX.END] + "'"
                        ));
                    }
                } else { // In line
                    Visitor stm = ast.Registry(new Stm().Rule(parser));
                    if (ast.Error != null) return ast;
                    noElse = NoIF.ElseInstance(stm, false);
                }
            }
            return ast.Success(noElse);
        }
    }
}


