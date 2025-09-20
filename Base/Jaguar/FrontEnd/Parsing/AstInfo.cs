using Common.Nodes;
using Common.Errors;

namespace FrontEnd.Parsing {
    public class AstInfo {
        private int LookAhead { get; set; }
        public TError Error { get; set; }
        public Visitor Node { get; set; }
        public int ToReverseCount { get; set; }
        public AstInfo() {
            this.Error = null;
            this.Node = null;
            this.LookAhead = 0;
            this.ToReverseCount = 0;
        }
        public void SetAhead() {
            this.LookAhead += 1;
        }
        public Visitor Registry(AstInfo ast) {
            this.LookAhead += ast.LookAhead;
            if (ast.Error != null)
                this.Error = ast.Error;
            return ast.Node;
        }
        public Visitor TryRegister(AstInfo ast){ // Um lookahead para statements 
          if (ast.Error!=null){
            this.ToReverseCount = ast.LookAhead; // retornar a contagem que foi avancada
            return null;
          }
          return this.Registry(ast);
        }
        public AstInfo Success(Visitor _node) { 
            this.Node = _node;
            return this;
        }
        public AstInfo Fail(TError _error) { 
            if (this.Error==null || this.LookAhead == 0)
			    this.Error = _error;
            return this;
        }
        public override string ToString() {
            string sb = "";
            if (this.Error != null)
                sb += this.Error.ToString();
            if(this.Node!=null)
                sb += this.Node.ToString();
            return sb;
        }
    }
}
