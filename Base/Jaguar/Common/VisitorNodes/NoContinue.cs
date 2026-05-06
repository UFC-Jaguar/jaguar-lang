using Common.Data;

namespace Common.Nodes {
    public class NoContinue : Visitor {
        public NoContinue(JSource scIni, JSource scEnd) {
            this.NOIni = scIni;
            this.NOEnd = scEnd;
        }
        public override DataFlow Visit(JMemory memory) {
            return new DataFlow().ContinueOk();
        }
    }
}


