using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;

namespace FrontEnd.Grammar {
    public class Term : Grammar {
        public AstInfo Rule(Parser parser) {
            return NoOpBinaria.Perform(parser, new Factor(), Consts.MUL, Consts.DIV);
        }
    }
}
