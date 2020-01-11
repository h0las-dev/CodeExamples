namespace Interpolation
{
    internal class LinearInterpolator : CommonInterpolator
    {
        public LinearInterpolator(double[] values) : base(values)
        {
            // nothing to do.
        }

        public override double CalculateValue(double x)
        {
            var nmax = Values.Length - 1;
            if (nmax < 0)
            {
                return base.CalculateValue(x);
            }

            if (x < 0)
            {
                return this.Values[0];
            }

            var n = (int)x;

            if (n >= nmax)
            {
                return this.Values[nmax];
            }
            else
            {
                return this.Values[n] + ((this.Values[n + 1] - this.Values[n]) * (x - n));
            }
        }
    }
}
