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
    public class GrossNode
    {
        public double Coef;
        public int Power;
        private static double TOLERANCE = 1e-15;

        public GrossNode()
        {
            Coef = 0.0;
            Power = 0;
        }
        public static bool operator >(GrossNode g1, GrossNode g2)
        {
            return (g1.Power == g2.Power)?(g1.Coef>g2.Coef):(g1.Power>g2.Power);
        }
        public static bool operator <(GrossNode g1, GrossNode g2)
        {
            return (g1.Power == g2.Power) ? (g1.Coef < g2.Coef) : (g1.Power < g2.Power);
        }

        public static bool operator ==(GrossNode g1, GrossNode g2)
        {
            return (g1.Power == g2.Power) && (Math.Abs(g1.Coef - g2.Coef) < TOLERANCE);
        }
        public static bool operator !=(GrossNode g1, GrossNode g2)
        {
            return (g1.Power != g2.Power) || (Math.Abs(g1.Coef - g2.Coef) > TOLERANCE);
        }
        public static GrossNode operator +(GrossNode g1, GrossNode g2)
        {
            GrossNode result;
            if (g1.Power != g2.Power) throw new FormatException();
            {
                result = new GrossNode();
                result.Power = g1.Power;
                result.Coef = g1.Coef + g2.Coef;
            }
            return result;
        }
        public static GrossNode operator -(GrossNode g1, GrossNode g2)
        {
            GrossNode result;
            if (g1.Power != g2.Power) throw new FormatException();
            {
                result = new GrossNode();
                result.Power = g1.Power;
                result.Coef = g1.Coef - g2.Coef;
            }
            return result;
        }

        public static GrossNode operator -(GrossNode arg)
        {
            arg.Coef = -arg.Coef;
            return arg;
        }
        public static GrossNode operator +(GrossNode arg)
        {
            arg.Coef = +arg.Coef;
            return arg;
        }

        public static GrossNode operator *(GrossNode g1, GrossNode g2)
        {
            var result = new GrossNode();
            result.Power = g1.Power + g2.Power;
            result.Coef = g1.Coef + g2.Coef;
            return result;
        }
    }
    public class Gross
    {
        public List<GrossNode> Container;
        public int Size;
        private char g = '\u2460';
        public GrossNode this[int index1]   // Indexer declaration
        {
            get { return Container[index1]; }
            set { Container[index1] = value; }
        }

        public Gross()
        {
            Size = 0;
            Container = new List<GrossNode>();
        }

        public void Add(GrossNode arg)
        {

            if (Container.Exists(m => m.Power == arg.Power))
            {
                Container[Container.FindIndex(m => m.Power == arg.Power)] += arg;
            }
            else
            {
                Container.Add(arg);
                Container.OrderBy(x => x.Power);
            }
            Size = Container.Count;
        }
        public static Gross operator +(Gross A, Gross B)
        {
            Gross result = new Gross();
            int i = 0, j = 0;
            while (i < A.Size || j < B.Size)
            {
                if (i != A.Size && j != B.Size)
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
                if (i==A.Size)
                {
                    result.Add(B[j]);
                    j++;
                }
                else if (j == B.Size)
                {
                    result.Add(A[i]);
                    j++;
                }
            }
            return result;
        }
        public static Gross operator -(Gross A, Gross B)
        {
            Gross result = new Gross();
            int i = 0, j = 0;
            while (i < A.Size || j < B.Size)
            {
                if (i != A.Size && j != B.Size)
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
                if (i == A.Size)
                {
                    result.Add(-B[j]);
                    j++;
                }
                else if (j == B.Size)
                {
                    result.Add(A[i]);
                    j++;
                }
            }
            return result;
        }
    }
}
