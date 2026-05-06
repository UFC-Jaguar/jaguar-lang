using Common.Data;

namespace Common.Nodes {
    public class NoReturn : Visitor {
        public Visitor nodeToReturn = null;
        public NoReturn(Visitor node_to_return, JSource scIni, JSource scEnd) {
            this.nodeToReturn = node_to_return;
            this.NOIni = scIni;
            this.NOEnd = scEnd;
        }
        public override DataFlow Visit(JMemory memory) {
            DataFlow manager = new DataFlow();

            TValue value = Consts.Number.Null;
            if (this.nodeToReturn != null) {
                value = manager.update_and_get_value(this.nodeToReturn.Visit(memory));
                if (manager.NeedReturn) return manager;
            }
            return manager.ReturnOk(value);
        }
    }
}
