using Common.Data;
using Common.Nodes;

namespace BackEnd {
    public class Interpreter {
        public MemoryManager Visit(Visitor node, JMemory memory){
            return node.Visit(memory);
        }
    }
}
