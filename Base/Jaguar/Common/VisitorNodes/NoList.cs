using System.Collections.Generic;
using Common.Data;

namespace Common.Nodes {
    public class NoList : Visitor {
        public List<Visitor> Elements { get; set; }
        public NoList(List<Visitor> _elementNodes, JSource scIni, JSource scEnd){
            this.Elements = _elementNodes;

            this.NOIni = scIni;
            this.NOEnd = scEnd;
        }
        public override DataFlow Visit(JMemory memory) {
            DataFlow manager = new DataFlow();
            var elements = new List<TValue>();

            foreach (Visitor visitorNode in this.Elements) {
                elements.Add(manager.update_and_get_value(visitorNode.Visit(memory)));
                if (manager.NeedReturn) return manager;
            }
            var list = new TList(elements);
            this.Value = list;
            return manager.SetDefaultAndNewTValue(
              list.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd)
            );
        }
    }
}

