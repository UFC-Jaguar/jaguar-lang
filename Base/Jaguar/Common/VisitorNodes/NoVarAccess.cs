using FrontEnd.Lexing;
using Common.Data;
using Common.Errors;

namespace Common.Nodes {
    public class NoVarAccess: Visitor {
        public Token VarNameTOK { get; set; }
        public NoVarAccess(Token token){
            this.VarNameTOK = token;
		    this.NOIni = this.VarNameTOK.NOIni;
		    this.NOEnd = this.VarNameTOK.NOEnd;
        }
        public override string ToString() {
            return this.VarNameTOK.ToString();
        }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            string varName = this.VarNameTOK.Value;
            TValue value = memory.SymbolTable.Get(varName);

            if (value == null) {
                return manager.Fail(new TRunTimeError(
                    this.NOIni, this.NOEnd,
                    "'" + varName + "' is Not defined",
                    memory
                ));
            }
            value = value.Copy();
            value.SetLocation(this.NOIni, this.NOEnd);
            value.SetMemory(memory);
            this.Value = value;
            return manager.Success(value);
        }
    }
}
