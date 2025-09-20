using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;

namespace FrontEnd.Grammar {
    public class ArithExp : Grammar {
        public AstInfo Rule(Parser parser) {
            return NoOpBinaria.Perform(parser, new Term(), Consts.PLUS, Consts.MINUS);
        }
    }
}
