using System.Collections.Generic;
using FrontEnd.Lexing;
using Common.Errors;
using FrontEnd.Grammar;
using Common.Data;

namespace FrontEnd.Parsing {
    public class Parser {
        private List<Token> Tokens { get; set; }// = null;
        private int TokIdx = -1;

        private Token CurrentTOKEN = null;
        public Token Current { get { return CurrentTOKEN;  } } // = null;

        public Parser(List<Token> tokens) {
            this.CurrentTOKEN = null;
            this.Tokens = tokens;
            this.TokIdx = -1;
            this.NextToken(); // nao SetAhead
        }
        private Token NextToken() {
            this.TokIdx += 1;
            this.UpdateCurrentTok();
            return this.CurrentTOKEN;
        }
        public Token NextToken(AstInfo ast) {
            ast.SetAhead();
            return this.NextToken();
        }
        public Token Reverse() { return this.Reverse(1); }
        public Token Reverse(int amount){
          this.TokIdx -= amount;
          this.UpdateCurrentTok();
          return this.CurrentTOKEN;
        }
        public void UpdateCurrentTok() {
            if (this.TokIdx >= 0 && this.TokIdx < this.Tokens.Count)
                this.CurrentTOKEN = this.Tokens[this.TokIdx];
        }
        public AstInfo Parsing() {
            AstInfo ast = new Statements().Rule(this);
            if (ast.Error == null && this.CurrentTOKEN.Type != Consts.EOF) {
                return ast.Fail(
                    new TError(this.CurrentTOKEN.NOIni, this.CurrentTOKEN.NOEnd, TError.ESyntax,
                        "Did you think about number '+', '-', '*' Or '/'?"
                    )
                );
            }
            return ast;
        }
        public Token Lookahead(int _nEsimo) { 
            int nEsimo = this.TokIdx + _nEsimo; 
            return this.Tokens[nEsimo < this.Tokens.Count ? nEsimo : this.Tokens.Count - 1];
        }
    }
}
