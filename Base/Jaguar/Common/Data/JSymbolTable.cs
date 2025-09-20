using System;
using System.Collections.Generic;

namespace Common.Data {
    //[Serializable()]
    public class JSymbolTable {
        private IDictionary<string, TValue> Symbols { get; set; }//get { return _symbols; } } //private readonly IDictionary<string, TValue> _symbols = new Dictionary<string, TValue>();
        private JSymbolTable Parent { get; set; }//get { return _parent;  } set { _parent = value; } } //private JSymbolTable _parent = null;
        public JSymbolTable() {
            this.Parent = null;
            this.Symbols = new Dictionary<string, TValue>(); 
        }
        public JSymbolTable(JSymbolTable _parentSymbolTable) :this() {
            this.Parent = _parentSymbolTable;
        }
        public TValue Get(string name){
            if (this.Symbols.TryGetValue(name, out TValue value))
                return value;
            if (value == null && this.Parent != null)
                return this.Parent.Get(name);
            return value;
        }
	    public void Set(string name, TValue value){
		    this.Symbols[name] = value;
        }
	    public bool Remove(string name){
            return this.Symbols.Remove(name);
        }
    }
}
