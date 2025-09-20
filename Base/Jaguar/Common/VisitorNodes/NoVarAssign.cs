using FrontEnd.Lexing;
using Common.Data;

namespace Common.Nodes {
    public class NoVarAssign : Visitor {
        public Token VarNameTOK { get; set; }
        public Visitor ExpValue { get; set; }
        public NoVarAssign(Token token, Visitor no) {
            this.VarNameTOK = token;
            this.ExpValue = no;
            this.NOIni = this.VarNameTOK.NOIni;
            this.NOEnd = this.ExpValue.NOEnd;
            //this.Value = no.Value;
        }
        public override string ToString() {
            return "("+this.VarNameTOK.ToString() + ", = ," + this.ExpValue.ToString()+")";
        }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            string varName = this.VarNameTOK.Value;
            TValue value = manager.Registry(this.ExpValue.Visit(memory));
            if (manager.NeedReturn) return manager;

            memory.SymbolTable.Set(varName, value);
            this.Value = value;
            return manager.Success(value);
        }
    }
}
