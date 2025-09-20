using Common.Data;
using Common.Errors;

namespace Common.Nodes {
    public abstract class Visitor: Node {
        public abstract MemoryManager Visit(JMemory memory);
        //public abstract object Val();
        public object Val() {
            return this.Value.Value;
        }        
        private TValue _value = TNull.Get();

        public TValue Value { get { return _value;  } set { _value = value;  } }
    }
}
