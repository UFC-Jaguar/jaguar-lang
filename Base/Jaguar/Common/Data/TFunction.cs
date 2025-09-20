using Common.Nodes;
using System;

namespace Common.Data {
    [Serializable()]
    public class TFunction : TBaseFunction {
        private Visitor BodyNode { get; set; }//= null;
        private string[] ArgNames { get; set; }//= null;
        private bool NeedAutoReturn { get; set; }//= false;
        public TFunction(string _name, Visitor _bodyNode, string[] _argNames, bool needAutoReturn) :base(_name) {
		    BodyNode = _bodyNode;
		    ArgNames = _argNames;
            this.NeedAutoReturn = needAutoReturn;
        }
        public override MemoryManager Run(TValue[] args){
            MemoryManager managerRunner = new MemoryManager();
            JMemory newMemory = this.GenerateNewMemory();
            managerRunner.Registry(this.CheckAndPopulateArgs(this.ArgNames, args, newMemory));
            if (managerRunner.NeedReturn) return managerRunner;
            TValue value = managerRunner.Registry(this.BodyNode.Visit(newMemory));
            if ((managerRunner.NeedReturn) && managerRunner.FuncReturn == null) return managerRunner;

            TValue retValue = this.NeedAutoReturn ? value:
                (managerRunner.FuncReturn!=null? managerRunner.FuncReturn : Consts.Number.Null);
            this.Value = retValue;
            return managerRunner.Success(retValue);
        }
	    public override TValue Copy() {
            TFunction copy = new TFunction(this.EmbeddedFunctionName, this.BodyNode, this.ArgNames, this.NeedAutoReturn);
            copy.SetMemory(this.memory);
            copy.SetLocation(this.NOIni, this.NOEnd);
            return copy;
        }

        public override string ToString() {
            return "<function " + this.EmbeddedFunctionName + ">";
        }
    }
}

