using Common.Helpers;
using Common.Data;
/* TODO: Teria como eliminar 'Memory' desta class, sem prejudicar os detalhes do erro? */
namespace Common.Errors {
    public class TRunTimeError: TError {
        private JMemory Memory { get; set; } 

        public TRunTimeError(JSource _NoIni, JSource _NoEnd, string _info, JMemory _memory) :
            base(_NoIni, _NoEnd, TError.ERun, _info) {
            this.Memory = _memory;
        }

        private string Tracing() {
            string tracing = "", space = "    ";
            JSource pos = this.NoIni;
            JMemory mem = this.Memory;
            while (mem != null) {
                tracing = space+"On " + pos.FileName + ", line " + (pos.Line + 1) + ", in "+ mem.Name + "\n" + space+tracing;
                pos = mem.ParentLocation;
                mem = mem.Parent;
            }
            return "  Tracing the stack memory for find/view calls:\n" + tracing;
        }

        public override string ToString() {
            string ret = this.Tracing();
		    ret += this.Kind+": "+this.Info;
            ret += "\n\n" + Util.StringsDataLocation(this.NoIni.GetCode(), this.NoIni, this.NoEnd);
		    return ret;
        }
    }
}
