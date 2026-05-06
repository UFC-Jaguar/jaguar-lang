using Common.Data;
using Common.Nodes;

namespace BackEnd {
    public class Interpreter {
        public DataFlow Visit(Visitor node, JMemory memory){
            return node.Visit(memory);
        }
    }
}
