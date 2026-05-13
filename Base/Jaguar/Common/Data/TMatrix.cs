using Common.Errors;
using System;
//using System.Collections.Generic; // |matrix = [2,3]|

namespace Common.Data {
    [Serializable()]
    public class TMatrixNumber : TValue {
        public double?[] MAT = null;//default;
        public int Row = 0;
        public int Col = 0;
        public override object Value { get { return this.MAT; } set { this.MAT = (double?[]) value; } }

        public TMatrixNumber(int[] v):base() {
            int n = v.Length;
            int len = 0;
            this.Row = 0;
            this.Col = 0;
            if (n > 0) {
                len = 1;
                this.Row = v[0]!=0?v[0]:1;
                for (int i = 1; i < n; i++) 
                    len = v[i]!=0?len*v[i]:len;
                this.Col = len;
                len = this.Row * this.Col;
            }
            this.MAT = new double?[len];
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
        public override TValue Add(TValue other) {
            if (other.GetType() == typeof(TMatrixNumber)) {
                TMatrixNumber o = (TMatrixNumber) other;
                if (this.Row == o.Row && this.Col == o.Col) {
                    int l1 = this.Row;
                    int c1 = this.Col;
                    int[] dim = { l1, c1 };
                    TMatrixNumber c = new TMatrixNumber(dim);
                    for (int i = 0; i< l1; i++) {
                        for(int j = 0; j< c1; j++) {
                            c.MAT[i * c1 + j] = this.MAT[i * c1 + j] + o.MAT[i * c1 + j];
                        }
                    }
                    return c;
                }
            }
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
                    for (int i = 0; i < l1; i++) {
                        for (int j = 0; j < c1; j++) {
                            c.MAT[i * c1 + j] = this.MAT[i * c1 + j] - o.MAT[i * c1 + j];
                        }
                    }
                    return c;
                }
            }
            return new TMatrixNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Multiply(TValue other) {
            if (other.GetType() == typeof(TMatrixNumber)) {
                TMatrixNumber o = (TMatrixNumber)other;
                if (this.Col == o.Row) {
                    int step = 1;
                    int l1 = this.Row;
                    int c1 = this.Col;
                    int c2 = o.Col;
                    int[] dim = { l1, c2 };
                    TMatrixNumber res = new TMatrixNumber(dim);// l1_A_c1_B_c2 == l1_C_c2
                    for (int i = 0; i < l1; i = i + step) {
                        for (int j = 0; j < c1; j = j + step) {
                            for (int k = 0; k < c2; k = k + step) {
                                res.MAT[i * c2 + k] += this.MAT[i * c1 + j] * o.MAT[j * c2 + k];
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
            int l1 = this.Row;
            int c1 = this.Col;
            for (int i = 0; i < l1; i++) {
                for (int j = 0; j < c1; j++) {
                    sb += this.MAT[i*c1+j]+((j+1)==c1?"\n ":", ");
                }
            }
            return sb + " " + l1 + "x" + c1;
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