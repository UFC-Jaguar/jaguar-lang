using System.Collections.Generic;
using Common.Data;

namespace Common.Nodes { /* TODO: Rever Tipos */
    public class NoAtomCall : Visitor {
        private Visitor NodeToCall { get; set; }//= null;
        private Visitor[] ArgNodes { get; set; }//= null;
        public NoAtomCall(Visitor node_to_call, Visitor[] arg_nodes) {
		    this.NodeToCall = node_to_call;
            this.ArgNodes = arg_nodes;

            this.NOIni = this.NodeToCall.NOIni;

		    if (this.ArgNodes.Length > 0)
                this.NOEnd = this.ArgNodes[this.ArgNodes.Length - 1].NOEnd;
		    else
                this.NOEnd = this.NodeToCall.NOEnd;
        }
        public override DataFlow Visit(JMemory memory) {
            DataFlow manager = new DataFlow();
            List<TValue> args = new List<TValue>();

            TValue valueToCall = manager.update_and_get_value(this.NodeToCall.Visit(memory));
            if (manager.NeedReturn) return manager;
            valueToCall = valueToCall.Copy().SetLocation(this.NOIni, this.NOEnd);

            foreach (var arg_node in this.ArgNodes) {
                args.Add(manager.update_and_get_value(arg_node.Visit(memory)));
                if (manager.NeedReturn) return manager;
            }
            TValue returnValue = manager.update_and_get_value(valueToCall.Run(args.ToArray()));
            if (manager.NeedReturn) return manager;
            returnValue = returnValue.Copy().SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            this.Value = returnValue;
            return manager.SetDefaultAndNewTValue(returnValue);
        }
    }
}

