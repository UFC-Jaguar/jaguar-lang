using FrontEnd.Parsing;

namespace FrontEnd.Grammar {
    public interface Grammar {
        AstInfo Rule(Parser parser);
    }
}
