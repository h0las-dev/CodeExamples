namespace Interpolation
{
    using System;

    internal class StepInterpolator : CommonInterpolator
    {
        public StepInterpolator(double[] values) : base(values)
        {
            // Nothing to do
        }

        public override double CalculateValue(double x)
        {
            var nmax = this.Values.Length - 1;
            var f = Math.Round(x);
            f = (int)Math.Round(x);
            if (nmax < 0)
            {
                return base.CalculateValue(x);
            }
            else
            {
                return this.Values[Math.Max(0, Math.Min((int)Math.Round(x), nmax))];
            }
        }
    }
}
