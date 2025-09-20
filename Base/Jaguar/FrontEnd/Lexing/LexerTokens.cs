using System.Collections.Generic;
using Common.Errors;

namespace FrontEnd.Lexing {
    public class LexerTokens {
        public List<Token> Tokens { get; set; }
        public TError Error { get; set; }
        public LexerTokens(List<Token> _tokens, TError _erro) {
            this.Tokens = _tokens;
            this.Error = _erro;
        }
        public override string ToString() {
            string sb = "[";
            if (Tokens != null) {
                var it = Tokens.GetEnumerator();
                if (it.MoveNext())
                    sb += it.Current;
                while (it.MoveNext()) {
                    sb = sb + ", " + it.Current;
                }
            }
            sb += "]\n";
            if (Error != null) {
                sb += Error;
            }
            return sb;
        }
    }
}
