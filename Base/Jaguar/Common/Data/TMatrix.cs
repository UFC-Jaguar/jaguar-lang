using Common.Errors;
using System;
using Common.Environment;
using MPI;
//using System.Collections.Generic; // |matrix = [2,3]|

namespace Common.Data {
    [Serializable()]
    public class TMatrixNumber : PValue {
        public double?[][] MAT = null;//default;
        public int Row = 0;
        public int Col = 0;
        public int PLen = 0;
        public override object Value { get { return this.MAT; } set { this.MAT = (double?[][]) value; } }

        public TMatrixNumber(int[] v, bool _par = false):base() {
            //Console.WriteLine("Parallel: " + _par);
            this.Par = _par;
            int n = v.Length;
            this.PLen = 0;
            this.Row = 0;
            this.Col = 0;
            int size = MPIEnv.Size;
            if (n > 0) { // Há argumento #[ai]
                this.PLen = this.Row = v[0]>0?v[0]:1;
                for (int i = 1; i < n; i++)
                    this.PLen = v[i]>0? this.PLen * v[i]: this.PLen;

                int col = this.PLen / this.Row;
                bool unidimensional = col == 1;
                this.Col = unidimensional? this.Row:col;
                this.Row = unidimensional? 1 : this.Row;
                this.PLen = this.Row * this.Col / size;
            }
            this.MAT = new double?[size][];
            for (int k = 0; k < size; k++) {
                this.MAT[k] = new double?[this.PLen];
            }

            int l1 = 1;//this.Row;
            int c1 = this.PLen;//this.Row*this.Col;//this.Col;
            for (int k = 0; k < size; k++) {
                for (int i = 0; i < l1; i++) {
                    for (int j = 0; j < c1; j++) {
                        this.MAT[k][i * c1 + j] = 0;
                    }
                }
            }
        }
        public TMatrixNumber(int[] v, JMemory _memory) : this(v) {
            this.memory = _memory;
        }
        public TMatrixNumber(TError e):base() {
            this.MAT = null;
            this.Error = e;
        }
        public TMatrixNumber(TError e, JMemory _memory):this(e) {
            this.memory = _memory;
        }
        public override DataFlow Run(TValue[] args) {
            DataFlow manager = new DataFlow();
            if (args.Length<2) return manager.Fail(this.IllegalOp(null));
            int idx = 0;
            double? dv = 0;
            int n = this.MAT[MPIEnv.Rank].GetLength(0);
            int n_args = args.Length;
            int[] idxs = new int[n_args-1];
            bool ok = true;
            for (int k = 0; k < n_args - 1; k++) {
                try {
                    ok = args[k].GetType() == typeof(TNumber) && ok;
                    idxs[k] = (int)(double?)args[k].Value;
                } catch (Exception ex) {
                    return manager.Fail(this.IllegalOp(null));
                }
            }
            try {
                ok = args[n_args-1].GetType() == typeof(TNumber) && ok;
                dv = (double?)args[n_args-1].Value;
            } catch (Exception ex) {
                return manager.Fail(this.IllegalOp(null));
            }
            idx = idxs[0];
            if (ok && args.Length >= 3) { idx = idxs[0] * this.Col + idxs[1]; }
            if (ok && idx < n && idx >= 0) {
                this.MAT[MPIEnv.Rank][idx] = dv;
                return manager.SetDefaultAndNewTValue(this);
            }
            return manager.Fail(this.IllegalOp(null));// m(1,3,4)
        }
        public override TValue Add(TValue other) {
            if (other.GetType() == typeof(TMatrixNumber)) {
                TMatrixNumber o = (TMatrixNumber) other;
                if (this.Row == o.Row && this.Col == o.Col) {
                    int l1 = this.Row;
                    int c1 = this.Col;
                    int[] dim = { l1, c1 };
                    TMatrixNumber c = new TMatrixNumber(dim);
                    c.Par = this.Par;
                    for (int i = 0; i< l1; i++) {
                        for(int j = 0; j< c1; j++) {
                            c.MAT[MPIEnv.Rank][i * c1 + j] = this.MAT[MPIEnv.Rank][i * c1 + j] + o.MAT[MPIEnv.Rank][i * c1 + j];
                        }
                    }
                    return c;
                }
            }
            //if (other.GetType() == typeof(TNumber)) {
            //    return new TMatrixNumber(this.VAL + ((TNumber)other).VAL, this.memory);
            //}
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Sub(TValue other) {
            if (other.GetType() == typeof(TMatrixNumber)) {
                TMatrixNumber o = (TMatrixNumber)other;
                if (this.Row == o.Row && this.Col == o.Col) {
                    int l1 = this.Row;
                    int c1 = this.Col;
                    int[] dim = { l1, c1 };
                    TMatrixNumber c = new TMatrixNumber(dim);
                    c.Par = this.Par;
                    for (int i = 0; i < l1; i++) {
                        for (int j = 0; j < c1; j++) {
                            c.MAT[MPIEnv.Rank][i * c1 + j] = this.MAT[MPIEnv.Rank][i * c1 + j] - o.MAT[MPIEnv.Rank][i * c1 + j];
                        }
                    }
                    return c;
                }
            }
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Multiply(TValue other) {
            if (this.Par) return this.PMultiply(other);
            if (other.GetType() == typeof(TMatrixNumber)) {
                TMatrixNumber o = (TMatrixNumber)other;
                if (this.Col == o.Row) {
                    int step = 1;
                    int l1 = this.Row;
                    int c1 = this.Col;
                    int c2 = o.Col;
                    int[] dim = { l1, c2 };
                    TMatrixNumber res = new TMatrixNumber(dim);// l1_A_c1_B_c2 == l1_C_c2
                    res.Par = this.Par;
                    for (int i = 0; i < l1; i = i + step) {
                        for (int j = 0; j < c1; j = j + step) {
                            for (int k = 0; k < c2; k = k + step) {
                                res.MAT[MPIEnv.Rank][i * c2 + k] += this.MAT[MPIEnv.Rank][i * c1 + j] * o.MAT[MPIEnv.Rank][j * c2 + k];
                            }
                        }
                    }
                    return res;
                }
            }
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public TValue PMultiply(TValue other) {
            if (other.GetType() == typeof(TMatrixNumber)) {
                TMatrixNumber o = (TMatrixNumber)other;
                if (this.Col == o.Row) {
                    int step = 1;
                    int l1 = this.Row;
                    int c1 = this.Col;
                    int c2 = o.Col;
                    int[] dim = { l1, c2 };
                    TMatrixNumber res = new TMatrixNumber(dim);// l1_A_c1_B_c2 == l1_C_c2
                    res.Par = this.Par;
                    for (int i = 0; i < l1; i = i + step) {
                        for (int j = 0; j < c1; j = j + step) {
                            for (int k = 0; k < c2; k = k + step) {
                                res.MAT[MPIEnv.Rank][i * c2 + k] += this.MAT[MPIEnv.Rank][i * c1 + j] * o.MAT[MPIEnv.Rank][j * c2 + k];
                            }
                        }
                    }
                    return res;
                }
            }
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Divide(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Pow(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonEq(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonNe(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonLt(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonGt(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonLte(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonGte(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue And(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Or(TValue other) {
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        private double? testing(double? f) { return f != null && f != 0 ? f : 0; }

	    public override TValue Not(){
            return new TMatrixNumber(this.IllegalOp(this), this.memory);
        }
        public override bool IsTrue() {
            return true;
        }

        public override TValue Copy(){
            //TMatrixNumber clone = new TMatrixNumber(this.MAT);
            //clone.SetLocation(this.NOIni, this.NOEnd);
            //clone.SetMemory(this.memory);
            return this;//
        }
        public override string ToString() {
            string sb = " ";
            string s = "";
            int l1 = 1;//this.Row;
            int c1 = this.PLen;//this.Row*this.Col;
            int x = 0;
            for (int k = 0; k < MPIEnv.Size; k++) {
                for (int i = 0; i < l1; i++) {
                    for (int j = 0; j < c1; j++) {
                        x = x+1==this.Col?0:x+1;
                        s = x == 0 ? "\n " : ", ";
                        sb += this.MAT[k][i * c1 + j] + s;
                    }
                }
            }
            return sb + " " + this.Row + "x" + this.Col;
        }
        public override string ToStr() {
            return this.ToString();
        }
    }
}
/*
	int l1 = 2; int c1 = 3; int l2 = c1; int c2 = 4;
	int[] A = new int[l1*c1]; int[] B = new int[l2*c2]; int[] C = new int[l1*c2]; 
	for(int i = 0; i < l1; i++) for(int j=0; j< c1; j++) A[i*c1+j] = i*c1+j;
	for(int i = 0; i < l2; i++) for(int j=0; j< c2; j++) B[i*c2+j] = i*c2+j;
	string s = " ";
	for(int i = 0; i < l1; i++) for(int j=0; j< c1; j++) s = s + (""+A[i*c1+j]+((j+1)==c1?"\n ":", "));
	Console.WriteLine(s+" "+l1+"x"+c1); s = " ";
	for(int i = 0; i < l2; i++) for(int j=0; j< c2; j++) s = s + (""+B[i*c2+j]+((j+1)==c2?"\n ":", "));
	Console.WriteLine(s+" "+l2+"x"+c2); s = " ";
    int step = 1;
    for (int i = 0; i < l1; i=i+step) {
        for (int j = 0; j < c1; j = j + step) {
            for (int k = 0; k < c2; k = k + step) {
                C[i*c2+k] += A[i*c1+j] * B[j*c2+k];
            }
        }
    }
	for(int i = 0; i < l1; i++) for(int j=0; j< c2; j++) s = s + (""+C[i*c2+j]+((j+1)==c2?"\n ":", "));
	Console.WriteLine(s+" "+l1+"x"+c2); s = " ";
 */