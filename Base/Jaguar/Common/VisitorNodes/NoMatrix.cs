using System.Collections.Generic;
using Common.Data;

namespace Common.Nodes {
    public class NoMatrix : Visitor {
        public List<Visitor> Elements { get; set; }
        public NoMatrix(List<Visitor> _elementNodes, JSource scIni, JSource scEnd){
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
            int[] ints = new int[elements.Count];
            int i = 0;
            foreach (TValue v in elements) {
                try { 
                double? d = ((double?) v.Value);
                ints[i++] = (int)d;
                } catch {
                    ints = new int[0];
                    break;
                }
            }
            var arr = new TMatrixNumber(ints);
            this.Value = arr;
            return manager.SetDefaultAndNewTValue(
              arr.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd)
            );
        }
    }
}

