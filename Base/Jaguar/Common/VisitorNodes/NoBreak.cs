using Common.Data;

namespace Common.Nodes {
    public class NoBreak : Visitor {
        public NoBreak(JSource scIni, JSource scEnd) {
            this.NOIni = scIni;
            this.NOEnd = scEnd;
        }
        public override MemoryManager Visit(JMemory memory) {
            return new MemoryManager().SuccessBreak();
        }
    }
}

