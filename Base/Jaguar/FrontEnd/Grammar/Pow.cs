using Common.Nodes;
using FrontEnd.Parsing;
using Common.Data;

namespace FrontEnd.Grammar {
    public class Pow: Grammar {
        public AstInfo Rule(Parser parser) {
            return NoOpBinaria.Perform(parser, new AtomCall(), new Factor(), Consts.POW);
        }
    }
}
