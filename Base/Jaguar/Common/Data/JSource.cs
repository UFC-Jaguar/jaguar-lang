using System;

namespace Common.Data {
    //[Serializable()]
    public class JSource : ICloneable {
        public int Idx { get; set; }
        public int Line { get; set; }
        public int Col { get; set; }
        public string FileName { get; set; }
        public string Code { get; set; }

        public JSource(int idx, int line, int col, string fileName, string _fileContent) {
            this.Idx = idx;
            this.Line = line;
            this.Col = col;
            this.FileName = fileName;
            this.Code = _fileContent;
        }
        public JSource Next() {
            return Next(null);
        }
        public JSource Next(Char? _currentChar) {
            this.Idx += 1;
            this.Col += 1;
            if (_currentChar != null)
                if (_currentChar == '\n') {
                    this.Line += 1;
                    this.Col = 0;
                }
            return this;
        }
        public JSource Copy() {
            return new JSource(Idx, Line, Col, FileName, Code);
        }
        public virtual object Clone() {
            return this.Copy();
        }
        public override string ToString() {
            return "[Idx: "+Idx+", Line: "+Line+", Col: "+Col+", file: "+FileName+", Code: "+ Code+"]";
        }
        public string GetCode() {
            return this.Code;
        }
    }
}
