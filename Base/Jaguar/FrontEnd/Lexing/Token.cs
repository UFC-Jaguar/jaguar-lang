using Common.Errors;
using Common.Data;

namespace FrontEnd.Lexing {
    public class Token : Node {
        public string Type { get; set; }
        public string Value { get; set; }

        private Token(string _type):this(_type, null) { }
        private Token(string _type, string _value) {
            this.Type = _type;
            this.Value = _value;
        }
        public Token(string _type, string _value, JSource _NoIni) : this(_type, _value) {
            this.NOIni = _NoIni.Copy();
            this.NOEnd = _NoIni.Copy();
            this.NOEnd.Next();
        }
        public Token(string _type, string _value, JSource _NoIni, JSource _NoEnd) : this(_type, _value) {
            this.NOIni = _NoIni.Copy();
            this.NOEnd = _NoEnd.Copy();
        }
        public Token(string _type, string _value, JSource _NoIni, JSource _NoEnd, TError _erro): this(_type, _value, _NoIni, _NoEnd) {
            this.Error = _erro;
        }
        public bool Matches(string _type, string _value) {
            return this.Type == _type && this.Value == _value;
        }
        public override string ToString() {
            if (this.Value != null)
                return "" + this.Type + ":" + this.Value + "";
            return this.Type;
        }
    }
}
