using System;
using Common.Helpers;
using Common.Data;

namespace Common.Errors {
    public class TError: Exception {
        public static string ESyntax = "Syntax Error";
        public static string ELexer = "Lexer Error";
        public static string ERun = "Runtime Error<MemoryManager>";
        public JSource NoIni { get; set; }
        public JSource NoEnd { get; set; }
        public string Kind { get; set; }
        public string Info { get; set; }

        public TError(JSource _NoIni, JSource _NoEnd, string _kind, string _info) {
            this.NoIni = _NoIni;
            this.NoEnd = _NoEnd;
            this.Kind = _kind;
            this.Info = _info;
        }
        public override string ToString() {
		    string ret  = this.Kind + ": " + this.Info+"\n";
		    ret += "On "+this.NoIni.FileName+", line "+(this.NoIni.Line + 1);
            ret += "\n\n" + Util.StringsDataLocation(this.NoIni.GetCode(), this.NoIni, this.NoEnd);
		    return ret;
        }
    }
}
