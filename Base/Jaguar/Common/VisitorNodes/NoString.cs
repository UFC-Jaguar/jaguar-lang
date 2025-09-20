using FrontEnd.Lexing;
using Common.Data;

namespace Common.Nodes {
    public class NoString : Visitor {
        public Token Tok { get; set; }
        public NoString(Token tok) {
            this.Tok = tok;
            this.NOIni = tok.NOIni;
            this.NOEnd = tok.NOEnd;
            this.Value = new TString(this.Tok.Value);
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

