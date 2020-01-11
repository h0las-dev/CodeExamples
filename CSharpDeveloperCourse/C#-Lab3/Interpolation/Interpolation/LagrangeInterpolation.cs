namespace Interpolation
{
    internal class LagrangeInterpolation : CommonInterpolator
    {
        public LagrangeInterpolation(double[] values) : base(values)
        {
            // nothing to do.
        }

        public override double CalculateValue(double x)
        {
            var resultValue = 0.0;

            for (var i = 0; i < this.Values.Length; i++)
            {
                resultValue += this.Values[i] * this.GetBasePolynomialValue(i, this.Values.Length, x);
            }

            return resultValue;
        }

        private double GetBasePolynomialValue(int index, int dimension, double x)
        {
            double polynomValue = 1.0;

            for (var i = 0; i < dimension; i++)
            {
                if (index == i)
                {
                    continue;
                }
                else
                {
                    polynomValue *= (x - i) / (index - i);
                }
            }

            return polynomValue;
        }
    }
}
