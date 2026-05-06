using Common.Data;

namespace Common.Nodes {
    public class NoBreak : Visitor {
        public NoBreak(JSource scIni, JSource scEnd) {
            this.NOIni = scIni;
            this.NOEnd = scEnd;
        }
        public override DataFlow Visit(JMemory memory) {
            return new DataFlow().BreakOk();
        }
    }
}

