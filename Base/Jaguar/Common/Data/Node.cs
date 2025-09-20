using Common.Errors;
using System;

namespace Common.Data {
    //[Serializable()]
    public abstract class Node {

        private JSource _NoIni = default; // = null;
        private JSource _NoEnd = default; // = null;
        private TError __error = null;

        public JSource NOIni { get{ return _NoIni; } set { _NoIni = value; } }

        public JSource NOEnd { get { return _NoEnd; } set { _NoEnd = value; } }

        public TError Error { get { return __error; } set { __error = value; } }

        public virtual string ToStr() {
            return this.ToString();
        }
    }
}
