using FrontEnd.Lexing;
using Common.Data;

namespace Common.Nodes {
    public class NoOpUnaria: Visitor {
        public Token OP { get; set; }
        public Visitor NO { get; set; }
        public NoOpUnaria(Token op, Visitor node) {
            this.OP = op;
            this.NO = node;
            this.NOIni = node.NOIni;
            this.NOEnd = node.NOEnd;
        }
        public override string ToString() {
            return "(" + OP + ", " + NO + ")";
        }
        public override DataFlow Visit(JMemory memory) {
            DataFlow manager = new DataFlow();
            TValue num = manager.update_and_get_value(this.NO.Visit(memory));
            if (manager.NeedReturn)
                return manager;

            if (this.OP.Type == Consts.MINUS) {
                num = num.Multiply(new TNumber(-1));
            } else if (this.OP.Matches(Consts.KEY, Consts.KEYS[Consts.IDX.NOT])) {
                num = num.Not();
            }
            if (num.Error != null)
                return manager.Fail(num.Error);

            num.SetMemory(memory);
            num.SetLocation(this.NOIni, this.NOEnd);
            return manager.SetDefaultAndNewTValue(num);
        }
    }
}
