using System.Collections.Generic;
using Common.Data;

namespace Common.Nodes {
    public class NoWhile : Visitor {
        private readonly Visitor ConditionVisitor = null;
        private readonly Visitor BodyVisitor = null;
        private readonly bool NeedReturnNull = false;
        public NoWhile(Visitor _conditionVisitor, Visitor _bodyVisitor, bool needReturnNull) {
            this.ConditionVisitor = _conditionVisitor;
            this.BodyVisitor = _bodyVisitor;
            this.NeedReturnNull = needReturnNull;

            this.NOIni = this.ConditionVisitor.NOIni;
            this.NOEnd = this.BodyVisitor.NOEnd;
        }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            var elements = new List<TValue>();

            while (true) {
                TValue condition = manager.Registry(this.ConditionVisitor.Visit(memory));
                if (manager.NeedReturn) return manager;

                if (!condition.IsTrue()) break;

                TValue theValue = manager.Registry(this.BodyVisitor.Visit(memory));
                //this.Value = theValue;
                if (manager.NeedReturn && manager.LoopContinue == false && manager.LoopBreak == false) return manager;

                if (manager.LoopContinue) continue;

                if (manager.LoopBreak) break;

                elements.Add(theValue);
            }
            TList list = new TList(elements);
            TValue value = this.NeedReturnNull ? Consts.Number.Null : list.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd);
            return manager.Success(value); 
        }
    }
}
