using System;
using FrontEnd.Parsing;
using FrontEnd.Grammar;
using FrontEnd.Lexing;
using Common.Helpers;
using Common.Data;
using Common.Errors;

namespace Common.Nodes {
    public class NoOpBinaria: Visitor {
        public Visitor Left { get; set; }
        public Token OpTok { get; set; }
        public Visitor Right { get; set; }
        public NoOpBinaria(Visitor left, Token opTok, Visitor right) {
            this.Left = left;
            this.OpTok = opTok;
            this.Right = right;
            this.NOIni = this.Left.NOIni;
            this.NOEnd = this.Left.NOEnd;
        }
        public override string ToString() {
            return "(" + this.Left + ", " + OpTok + ", " + Right + ")";
        }
        public static AstInfo Perform(Parser parser, Grammar stma, params string[] ops) {
            return Perform(parser, stma, stma, ops);
        }
        public static AstInfo Perform(Parser parser, Grammar stma, Grammar stmb, params string[] ops) {
            AstInfo ast = new AstInfo();
            Visitor left = ast.Registry(stma.Rule(parser));
            if (ast.Error != null) return ast;
            while (Util.Em(parser.Current.Type, ops)) {
                Token opTok = parser.Current;
                parser.NextToken(ast);
                Visitor right = ast.Registry(stmb.Rule(parser));
                if (ast.Error != null) return ast;
                left = new NoOpBinaria(left, opTok, right);
            }
            return ast.Success(left);
        }

        public static AstInfo Perform(Parser parser, Grammar stma, params Tuple<string,string>[] ops) {
            return Perform(parser, stma, stma, ops);
        }
        public static AstInfo Perform(Parser parser, Grammar stma, Grammar stmb, params Tuple<string, string>[] ops) {
            AstInfo ast = new AstInfo();
            Visitor left = ast.Registry(stma.Rule(parser));
            if (ast.Error != null) return ast;
            while (Util.Em(parser.Current.Type, parser.Current.Value, ops)) {
                Token op = parser.Current;
                parser.NextToken(ast);
                Visitor right = ast.Registry(stmb.Rule(parser));
                if (ast.Error != null) return ast;
                left = new NoOpBinaria(left, op, right);
            }
            return ast.Success(left);
        }

        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            TValue left = manager.Registry(this.Left.Visit(memory));
            if (manager.NeedReturn) return manager;
            TValue right = manager.Registry(this.Right.Visit(memory));
            if (manager.NeedReturn) return manager;
            TValue result = null;
            if (this.OpTok.Type == Consts.PLUS)
                result = left.Add(right);
            else if (this.OpTok.Type == Consts.MINUS)
                result = left.Sub(right);
            else if (this.OpTok.Type == Consts.MUL)
                result = left.Multiply(right);
            else if (this.OpTok.Type == Consts.DIV)
                result = left.Divide(right);
            else if (this.OpTok.Type == Consts.POW)
                result = left.Pow(right);
            else if (this.OpTok.Type == Consts.EE)
                result = left.ComparisonEq(right);
            else if (this.OpTok.Type == Consts.NE)
                result = left.ComparisonNe(right);
            else if (this.OpTok.Type == Consts.LT)
                result = left.ComparisonLt(right);
            else if (this.OpTok.Type == Consts.GT)
                result = left.ComparisonGt(right);
            else if (this.OpTok.Type == Consts.LTE)
                result = left.ComparisonLte(right);
            else if (this.OpTok.Type == Consts.GTE)
                result = left.ComparisonGte(right);
            else if (this.OpTok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.AND]))
                result = left.And(right);
            else if (this.OpTok.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.OR]))
                result = left.Or(right);

            if (result == null) return manager.Fail(new TRunTimeError(left.NOIni, right.NOEnd, "RuntimeError", memory));
            if (result.Error != null) return manager.Fail(result.Error);
            result.SetMemory(memory);
            result.SetLocation(this.NOIni, this.NOEnd);
            return manager.Success(result);
        }
    }
}
