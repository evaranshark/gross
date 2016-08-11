using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace GrossArithmetik
{
    /// <summary>
    /// GrossNode exposes "simple" gross-number with only one power of gross (e.g. 10.5g^12).
    ///Tolerance of equality is 1e-15.
    /// </summary>
    public class GrossNode: IEquatable<GrossNode>, IComparable<GrossNode>
    {

        public double Coef { get; private set; }
        public int Power { get; private set; }
        private static readonly double TOLERANCE = 1e-15;
        /// <summary>
        /// Empty constructor. Initializes 0g0;
        /// </summary>
        public GrossNode()
        {
            Coef = 0.0;
            Power = 0;
        }
        /// <summary>
        /// Constructor from real number (double).
        /// </summary>
        /// <param name="val">Value to convert.</param>
        public GrossNode(double val)
        {
            Power = 0;
            Coef = val;
        }
        /// <summary>
        /// Constructor on existing values.
        /// </summary>
        /// <param name="Coef">Coefficient of GrossNode.</param>
        /// <param name="Power">Power of GrossNode.</param>
        public GrossNode(double Coef, int Power)
        {
            this.Coef = Coef;
            this.Power = Power;
        }
        //HashCodes for IEquatable.
        public override int GetHashCode()
        {
            return (Coef.GetHashCode() ^ Power).GetHashCode();
        }
        public override bool Equals(Object obj)
        {
            GrossNode o = (GrossNode) obj;
            return (Math.Abs(Coef - o.Coef) <= TOLERANCE) & Power == o.Power;
        }
        public bool Equals(GrossNode obj)
        {
            if (obj == null) return false;
            return Coef == obj.Coef & Power == obj.Power;
        }
        //Method for IComparable. Used for sorting.
        public int CompareTo(GrossNode obj)
        {
            if (obj == null) return 1;
            return Power.CompareTo(obj.Power);
        }

        //Operator for implicit conversation from double.
        public static implicit operator GrossNode(double val)
        {
            return new GrossNode(val);
        }
        public static GrossNode operator +(GrossNode donor,GrossNode val)
        {
            if (val.Power != donor.Power) throw new FormatException("op+");
            return new GrossNode(donor.Coef + val.Coef, donor.Power);
        }
        public static GrossNode operator -(GrossNode donor, GrossNode val)
        {
            if (val.Power != donor.Power) throw new FormatException("op-");
            return new GrossNode(donor.Coef - val.Coef, donor.Power);
        }
        public static GrossNode operator -(GrossNode arg)
        {
            GrossNode result = new GrossNode(-arg.Coef, arg.Power);
            //arg.Coef = -arg.Coef;
            return result;
        }
        public static GrossNode operator +(GrossNode arg)
        {
            GrossNode result = new GrossNode(+arg.Coef, arg.Power);
            //arg.Coef = +arg.Coef;
            return result;
        }
        public static GrossNode operator *(GrossNode g1, GrossNode g2)
        {
            var result = new GrossNode();
            result.Power = g1.Power + g2.Power;
            result.Coef = g1.Coef * g2.Coef;
            return result;
        }

        public static GrossNode operator /(GrossNode g1, GrossNode g2)
        {
            if (g2.Coef == 0) throw new DivideByZeroException("op/");

            var result = new GrossNode();
            result.Power = g1.Power - g2.Power;
            result.Coef = g1.Coef / g2.Coef;
            return result;
        }
    }
    public class Gross: List<GrossNode>
    {
        private char g = '\u2460'; // Char "grossone"
        private static readonly double TOLERANCE = 1e-15;
        public new void Add(GrossNode arg)
        {

            if (Exists(m => m.Power == arg.Power))
            {
                this[FindIndex(m => m.Power == arg.Power)] += arg;
            }
            else
            {
                ((List<GrossNode>)this).Add(arg);
                this.Sort();
                this.Reverse();
            }
        }
        public static Gross operator +(Gross A, Gross B)
        {
            Gross result = new Gross();
            if (A == null || B == null)
                throw new ArgumentNullException();
            if (A.Count == 0) return B;
            if (B.Count == 0) return A;
            int i = 0, j = 0;
            while (i < A.Count || j < B.Count)
            {
                if (i != A.Count && j != B.Count)
                {
                    if (A[i].Power == B[j].Power)
                    {
                        result.Add(A[i] + B[j]);
                        i++;
                        j++;
                    }
                    else if (A[i].Power > B[j].Power)
                    {
                        result.Add(B[j]);
                        j++;
                    }
                    else
                    {
                        result.Add(A[i]);
                        i++;
                    }
                }
                else if (i == A.Count)
                {
                    result.Add(B[j]);
                    //result.AddRange(B.SkipWhile(x => x == B[j]));
                    j++;
                    //return result;
                }
                else if (j == B.Count)
                {
                    result.Add(A[i]);
                    i++;
                }
            }
            result.Clear();
            return result;
        }
        public static Gross operator -(Gross A, Gross B)
        {
            Gross result = new Gross();
            int i = 0, j = 0;
            while (i < A.Count || j < B.Count)
            {
                if (i != A.Count && j != B.Count)
                {
                    if (A[i].Power == B[j].Power)
                    {
                        result.Add(A[i] - B[j]);
                        i++;
                        j++;
                    }
                    else if (A[i].Power > B[j].Power)
                    {
                        result.Add(-B[j]);
                        j++;
                    }
                    else
                    {
                        result.Add(A[i]);
                        i++;
                    }
                }
                else if (i == A.Count)
                {
                    result.Add(-B[j]);
                    j++;
                }
                else if (j == B.Count)
                {
                    result.Add(A[i]);
                    i++;
                }
            }
            result.Clear();
            return result;
        }
        public static Gross operator *(Gross A, Gross B)
        {
            var result = new Gross();
            foreach (var a in A)
                foreach (var b in B)
                    result.Add(new GrossNode(a.Coef * b.Coef, a.Power + b.Power));
            result.Clear();
            return result;
        }
        public static Gross operator /(Gross A, Gross B)
        {
            var result = new Gross();
            var residue = A;
            Gross akk;
            while (residue.Count > 0 && residue[0].Power > -3)
            {
                akk = new Gross() { residue[0] / B[0] };
                result.Add(akk[0]);
                residue = residue - akk * B;
            }

            result.Clear();
            return result;
        }
        public static implicit operator Gross(double val)
        {
            Gross res = new Gross();
            res.Add(val);
            return res;
        }
        public String Show()
        {
            if (this == null)
                throw new ArgumentNullException("Gross is not initialized.");
            if (Count == 0)
                return "0";
            String result = "";
            foreach (var a in this)
            {
                result += a.Coef.ToString() + g + a.Power.ToString() + '_';
            }
            result = result.Remove(result.LastIndexOf('_'));
            return result;
        }
        public new void Clear()
        {
            RemoveAll(m => Math.Abs(m.Coef - 0.0) < TOLERANCE);
           // if (Count == 0)
               // Add(0);
        }
    }
    public static class StringExt
    {
        public static Gross ParseG(this String arg)
        {
            List<String> sGrossNodes = new List<string>();
            sGrossNodes = arg.Split('_').ToList();
            Gross result = new Gross();
            foreach (var item in sGrossNodes)
            {

                result.Add(new GrossNode(Convert.ToDouble(item.Split('g')[0].ToString()), Convert.ToInt32(item.Split('g')[1].ToString())));
            }
            //result = result.OrderByDescending(x => x.Power) as Gross;
            return result;
        }
    }
}

