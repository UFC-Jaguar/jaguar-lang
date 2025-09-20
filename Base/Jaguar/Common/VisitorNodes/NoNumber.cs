using FrontEnd.Lexing;
using Common.Data;

// TODO: talvez podemos gerenciar melhor o NOIni/NOEnd, que é quem nos informa onde está um erro.
namespace Common.Nodes {
    public class NoNumber: Visitor {
        public Token Tok { get; set; }
        public NoNumber(Token tok) { 
            this.Tok = tok;
            this.NOIni = tok.NOIni;
            this.NOEnd = tok.NOEnd;
            this.Value = new TNumber(float.Parse(this.Tok.Value));
        }
        public override string ToString() {
            return Tok.ToString();
        }
        public override MemoryManager Visit(JMemory memory) {
            this.Value.SetMemory(memory);
            this.Value.SetLocation(this.NOIni, this.NOEnd);
            MemoryManager manager = new MemoryManager();
            manager.Success(this.Value);
            return manager;
        }
    }
}
