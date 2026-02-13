using System.Collections.Generic;
using Common.Errors;
using Common.Data;
using Common.Helpers;

namespace FrontEnd.Lexing {
    public class Lexer {
        private char[] Code { get; set; }
        private JSource Source { get; set; }
        private char? Current { get; set; }
        public Lexer(string _fileName, string _fileContent) {
            this.Code = _fileContent.ToCharArray();
            this.Source = new JSource(-1, 0, -1, _fileName, _fileContent);
            this.Current = null;
            this.Next();
        }
        private void Next() {
            this.Source.Next(this.Current);
            this.Current = null;
            this.Current = Source.Idx < Code.Length ? Code[Source.Idx] : this.Current;
        }
        public LexerTokens MakeTokens() {
            List<Token> tokens = new List<Token>();

            while (this.Current != null) {
                if (Util.Em(this.Current, " \t")) {
                    this.Next();
                } else if (this.Current == '#') {
                    this.Comments();
                } else if (Util.Em(this.Current, ";\n\r\n")) {
                    tokens.Add(new Token(Consts.NEWLINE, null, this.Source));
                    this.Next();
                } else if (Util.DIGITS(this.Current)) { //Util.Em(this.Current, Consts.DIGITS)) {
                    tokens.Add(this.MakeNumber());
                } else if (Util.LETTERS(this.Current)){ //Util.Em(this.Current, Consts.LETTERS)) {
                    tokens.Add(this.MakeIdentifier());
                } else if (this.Current == '"') {
                    tokens.Add(this.MakeString());
                } else if (this.Current == '+') {
                    tokens.Add(new Token(Consts.PLUS, null, this.Source));
                    this.Next();
                } else if (this.Current == '-') {// - ou ->
                    tokens.Add(this.MakeMinusOrArrow());
                } else if (this.Current == '*') {
                    tokens.Add(new Token(Consts.MUL, null, this.Source));
                    this.Next();
                } else if (this.Current == '.') {
                    tokens.Add(new Token(Consts.DOT, null, this.Source));
                    this.Next();
                } else if (this.Current == '|') {
                    Token par = this.MakePar();
                    tokens.Add(par);
                    if (par.Error != null)
                        return new LexerTokens(new List<Token>(), par.Error);
                } else if (this.Current == '/') {
                    tokens.Add(new Token(Consts.DIV, null, this.Source));
                    this.Next();
                } else if (this.Current == '^') {
                    tokens.Add(new Token(Consts.POW, null, this.Source));
                    this.Next();
                } else if (this.Current == '(') {
                    tokens.Add(new Token(Consts.LPAR, null, this.Source));
                    this.Next();
                } else if (this.Current == ')') {
                    tokens.Add(new Token(Consts.RPAR, null, this.Source));
                    this.Next();
                } else if (this.Current == '[') {
                    tokens.Add(new Token(Consts.LSQR, null, this.Source));
                    this.Next();
                } else if (this.Current == ']') {
                    tokens.Add(new Token(Consts.RSQR, null, this.Source));
                    this.Next();
                } else if (this.Current == '!') {
                    Token token = this.MakeNotEquals();
                    if (token.Error != null) return new LexerTokens(new List<Token>(), token.Error);
                    tokens.Add(token);
                } else if (this.Current == '=') {
                    tokens.Add(this.MakeEquals());
                } else if (this.Current == '<') {
                    tokens.Add(this.MakeLessThan());
                } else if (this.Current == '>') {
                    tokens.Add(this.MakeGreaterThan());
                } else if (this.Current == ',') {
                    tokens.Add(new Token(Consts.COMMA, null, this.Source));
                    this.Next();
                } else if (this.Current == 65279) {// TODO: Erro de char: Unicode Character 'ZERO WIDTH VAL-BREAK SPACE'(U + FEFF)
                    this.Next();
                } else {
                    JSource NoIni = this.Source.Copy();
                    char? _char = this.Current;
                    this.Next();
                    TError erro = new TError(NoIni, this.Source, "Char not supported", "'" + _char + "'");
                    return new LexerTokens(new List<Token>(), erro);
                }
            }
            tokens.Add(new Token(Consts.EOF, null, this.Source));//, null));
            return new LexerTokens(tokens, null);
        }
        private Token MakeMinusOrArrow() {
		    string typeTOK = Consts.MINUS;
		    JSource NoIni = this.Source.Copy();
		    this.Next();

		    if (this.Current == '>'){
			    this.Next();
			    typeTOK = Consts.ARROW;
            }
            return new Token(typeTOK, null, NoIni, this.Source);
        }
        private Token MakeNotEquals() {
		    JSource NoIni = this.Source.Copy();
		    this.Next();

		    if (this.Current == '='){
			    this.Next();
                return new Token(Consts.NE, null, NoIni, this.Source);
            }
		    this.Next();
            TError err = new TError(NoIni, this.Source, "Expected Character", "'=' (after '!')");
            Token tk = new Token(null, null, this.Source); //null, null);
            tk.Error = err;
            return tk;
        }
        private Token MakeEquals() {
		    string typeTOK = Consts.EQ;
		    JSource NoIni = this.Source.Copy();
		    this.Next();

            if (this.Current == '=') {
                this.Next();
                typeTOK = Consts.EE;
            }
            return new Token(typeTOK, null, NoIni, this.Source);
        }
        private Token MakeLessThan() {
            string typeTOK = Consts.LT;
            JSource NoIni = this.Source.Copy();
            this.Next();

            if (this.Current == '='){
                this.Next();
                typeTOK = Consts.LTE;
            }
            return new Token(typeTOK, null, NoIni, this.Source);
        }
        private Token MakeGreaterThan() {
            string typeTOK = Consts.GT;
            JSource NoIni = this.Source.Copy();
            this.Next();

            if (this.Current == '='){
                this.Next();
                typeTOK = Consts.GTE;
            }
            return new Token(typeTOK, null, NoIni, this.Source);
        }
        private Token MakeIdentifier() {
            string str = "";
            JSource NoIni = this.Source.Copy();
            while (this.Current != null && (Util.LETTERS_DIGITS_UNDERSCORE(this.Current))) {
                str += this.Current;
                this.Next();
            }
            string tokType = "";
            if (Util.Em(str, Consts.KEYS))
                tokType = Consts.KEY;
            else
                tokType = Consts.ID;

            return new Token(tokType, str, NoIni, this.Source);
        }
        private Token MakeNumber() {
            string numStr = "";
            int dotCount = 0;
            JSource NoIni = this.Source.Copy();

            while (this.Current != null && Util.LETTERS_DIGITS_DOT(this.Current)){
                if (Util.Em(this.Current, ".")) {
                    if (dotCount == 1) break;
                    dotCount += 1;
                    numStr += ".";
                } else {
                    numStr += this.Current;
                }
                this.Next();
            }
            if (dotCount == 0) {
                return new Token(Consts.INT, numStr, NoIni, this.Source);
            } else
                return new Token(Consts.FLOAT, numStr, NoIni, this.Source);
        }
        private Token MakeString() {
		    string str = "";
		    JSource NoIni = this.Source.Copy();
		    bool specialChar = false;
		    this.Next();

            var specialChars = new Dictionary<char?, char?>();
            specialChars['n'] = '\n'; specialChars['t'] = '\t';

            while (this.Current != null && (this.Current != '"' || specialChar)) {
			    if (specialChar) {
                    if (specialChars.TryGetValue(this.Current, out char? c)==false ){ c = this.Current; } 
                    str += c; 
                    specialChar = false; 
                } else {
				    if (this.Current == '\\') {
                        specialChar = true;
				    } else{
					    str += this.Current;
                    }
			    }
                this.Next();
            }
            this.Next();
		    return new Token(Consts.STRING, str, NoIni, this.Source);
        }
        private void Comments() {
            this.Next();
            while (this.Current!=null && this.Current != '\n')
              this.Next();
            this.Next();
        }
        private Token MakePar() {
            string valor;
            string typeTOK = Consts.KEY;
            JSource noIni = this.Source.Copy();
            this.Next();
            Token t = new Token(null, null, noIni, this.Source);
            if (this.Current == '|') {
                typeTOK = Consts.KEY;
                valor = Consts.KEYS[Consts.IDX.PFUN];
                this.Next();
                t = new Token(typeTOK, valor, noIni, this.Source);
                return t;
            }
            t.Error = new TError(noIni, this.Source, Consts.KEY, "'|', expected ||");
            return t;
        }
    }
}
