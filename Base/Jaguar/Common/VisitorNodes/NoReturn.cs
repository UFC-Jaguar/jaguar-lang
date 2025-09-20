using Common.Data;

namespace Common.Nodes {
    public class NoReturn : Visitor {
        public Visitor nodeToReturn = null;
        public NoReturn(Visitor node_to_return, JSource scIni, JSource scEnd) {
            this.nodeToReturn = node_to_return;
            this.NOIni = scIni;
            this.NOEnd = scEnd;
        }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();

            TValue value = Consts.Number.Null;
            if (this.nodeToReturn != null) {
                value = manager.Registry(this.nodeToReturn.Visit(memory));
                if (manager.NeedReturn) return manager;
            }
            return manager.SuccessReturn(value);
        }
    }
}
