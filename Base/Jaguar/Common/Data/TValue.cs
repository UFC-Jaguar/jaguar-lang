using System;
using Common.Errors;
// Avaliar se esta classe vai para o BackEnd, pois usa MemoryManager Em Run(args)

namespace Common.Data {
    [Serializable()]
    public abstract class TValue {
        [NonSerialized()] public JMemory memory = null; /* TODO: avaliar vantagens de por aqui!! */
        public TValue() {
            //this.SetLocation(null, null); // Nao deve setar null para JSource
            this.SetMemory(null);
            this.Value = null;
        }
        public virtual object Value { get; set; }

        [NonSerialized()] private JSource _NoIni = default; // = null;
        [NonSerialized()] private JSource _NoEnd = default; // = null;
        [NonSerialized()] private TError __error = null;

        public JSource NOIni { get { return _NoIni; } set { _NoIni = value; } }

        public JSource NOEnd { get { return _NoEnd; } set { _NoEnd = value; } }

        public TError Error { get { return __error; } set { __error = value; } }

        public virtual string ToStr() {
            return this.ToString();
        }

        public TValue SetLocation(JSource _ds_ini, JSource _ds_end) {
            this.NOIni = _ds_ini;
            this.NOEnd = _ds_end;
            return this;
        }
        public TValue SetMemory(JMemory _memory) {
            this.memory = _memory;
            return this;
        }
        public virtual TValue Add(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue Sub(TValue other) {
            this.IllegalOp(null);
            return null;
        }

        public virtual TValue Multiply(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue Divide(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue Pow(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue ComparisonEq(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue ComparisonNe(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue ComparisonLt(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue ComparisonGt(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue ComparisonLte(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue ComparisonGte(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue And(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue Or(TValue other) {
            this.IllegalOp(null);
            return null;
        }
        public virtual TValue Not() {
            this.IllegalOp(null);
            return null;
        }
        /*
          Função Run sera chamada sempre que uma funcao for executada. Isso eh sobrescrito Em TFunction, que eh um TValue
          TODO: Quais args ?, Avaliar o "porque" de MemoryManager Em FrontEnd ? Seria melhor criar a ideia de BackEnd?
        */
        public virtual MemoryManager Run(TValue[] args) { 
            MemoryManager manager = new MemoryManager();
            return manager.Fail(this.IllegalOp(null));
        }
        public virtual TValue Copy() {
            throw new Exception("No Copy method defined");
        }
        public virtual bool IsTrue() {
            return false;
        }
        public virtual TError IllegalOp(TValue other) {
            if (other == null) other = this;
            return new TRunTimeError(
                this.NOIni, other.NOEnd,
                "Illegal operation",
                this.memory
            );
        }
        public override string ToString() {
            return TNull.Get().ToString(); // Deixar para o polimorfismo resolver. Subclasses devem sobrescrever
        }
        public bool Null { get { return this.Value == null; } }
    }
    public class TNull : TValue { 
        public static TNull SINGVALUE = null;
        public TNull() : base() {  
            if (TNull.SINGVALUE != null) throw new Exception("TNull is a singleton and has already been instantiated.");
            TNull.SINGVALUE = this;
            this.Value = null;
        }
        public static TNull Get() {
            if (TNull.SINGVALUE == null) {
                new TNull();
            }
            return TNull.SINGVALUE;
        }
        public override string ToString() { return "null"; }
        public override bool IsTrue() { return false; }
        public override TValue Copy() {
            return this;
            /*TNull clone = new TNull();
            clone.SetLocation(this.NOIni, this.NOEnd);
            clone.SetMemory(this.memory);
            return clone;*/
        }
        public override bool Equals(object other) {
            if (other is TValue value) {
                return value.Value == null;
            }
            return false;
        }
        public override int GetHashCode() {/*
            int hashCode = 1525335648;
            hashCode = hashCode * -1521134295 + EqualityComparer<JMemory>.Default.GetHashCode(memory);
            hashCode = hashCode * -1521134295 + EqualityComparer<JSource>.Default.GetHashCode(NOIni);
            hashCode = hashCode * -1521134295 + EqualityComparer<JSource>.Default.GetHashCode(NOEnd);
            hashCode = hashCode * -1521134295 + EqualityComparer<TError>.Default.GetHashCode(Error);
            hashCode = hashCode * -1521134295 + NULL.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value); */
            return 0;// hashCode;
        }
    }
}

