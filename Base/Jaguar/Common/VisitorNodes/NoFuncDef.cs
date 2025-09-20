using BackEnd;
using FrontEnd.Lexing;
using Common.Data;

namespace Common.Nodes { // Rever Tipos
    public class NoFuncDef : Visitor {
        private Token NameFunction { get; set; }//= null;
        private Token[] ArgsFunction { get; set; }//= null;
        private Visitor BodyFunction { get; set; }//= null;
        private bool NeedReturn { get; set; }//= false;
        public NoFuncDef(Token nameFunction, Token[] argsFunction, Visitor bodyFunction, bool needReturn) {
            this.NameFunction = nameFunction;
            this.ArgsFunction = argsFunction;
            this.BodyFunction = bodyFunction;
            this.NeedReturn = needReturn;

            if (this.NameFunction!=null)
                this.NOIni = this.NameFunction.NOIni;
            else if (this.ArgsFunction.Length > 0)
                this.NOIni = this.ArgsFunction[0].NOIni;
            else
                this.NOIni = this.BodyFunction.NOIni;
            this.NOEnd = this.BodyFunction.NOEnd;
        }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();

            string nameFuncValue = this.NameFunction != null ? this.NameFunction.Value : null;
            var bodyNode = this.BodyFunction;

            string[] argsNames = new string[this.ArgsFunction.Length];
            for (int i = 0; i < this.ArgsFunction.Length; i++)
                argsNames[i] = this.ArgsFunction[i].Value;

            TFunction funcInstance = new TFunction(nameFuncValue, bodyNode, argsNames, this.NeedReturn);
            funcInstance.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd);

            if (this.NameFunction != null)
                memory.SymbolTable.Set(nameFuncValue, funcInstance);

            return manager.Success(funcInstance);
        }
    }
}
