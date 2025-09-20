using Common.Nodes;
using System;
using System.Collections.Generic;

namespace Common.Data {
    [Serializable()]
    public class TFun : TBaseFunction {
        private Visitor BodyNode { get; set; }//= null;
        private string[] ArgNames { get; set; }//= null;
        private bool NeedAutoReturn { get; set; }//= false;
        
        private static Dictionary<int, TValue> mem = new Dictionary<int, TValue>();

        public TFun(string _name, Visitor _bodyNode, string[] _argNames, bool needAutoReturn) : base(_name) {
            BodyNode = _bodyNode;
            ArgNames = _argNames;
            this.NeedAutoReturn = needAutoReturn;
            this.Name = _name;
        }
        private string Name { get; set; }
        private int hashCode = -1;
        public int GenCode(string _name, TValue[] args) {
            foreach (TValue _arg in args) {
                _name += _arg.Value.GetHashCode();
            }
            this.hashCode = _name.GetHashCode();
            return this.hashCode;
        }


        public override MemoryManager Run(TValue[] args) {
            MemoryManager managerRunner = new MemoryManager();
            JMemory newMemory = this.GenerateNewMemory();
            managerRunner.Registry(this.CheckAndPopulateArgs(this.ArgNames, args, newMemory));
            if (managerRunner.NeedReturn) return managerRunner;

            TValue value;
            this.GenCode(this.Name, args);
            if (!TFun.mem.TryGetValue(this.hashCode, out value)) {
                value = managerRunner.Registry(this.BodyNode.Visit(newMemory));
                TFun.mem.Add(this.hashCode, value);
            }
            this.Value = value;
            if ((managerRunner.NeedReturn) && managerRunner.FuncReturn == null) return managerRunner;

            TValue retValue = this.NeedAutoReturn ? value :
                (managerRunner.FuncReturn != null ? managerRunner.FuncReturn : Consts.Number.Null);
            this.Value = retValue;
            return managerRunner.Success(retValue);
        }
        public override TValue Copy() {
            TFun copy = new TFun(this.EmbeddedFunctionName, this.BodyNode, this.ArgNames, this.NeedAutoReturn);
            copy.SetMemory(this.memory);
            copy.SetLocation(this.NOIni, this.NOEnd);
            return copy;
        }

        public override string ToString() {
            return "<function " + this.EmbeddedFunctionName + ">";
        }
    }
}

