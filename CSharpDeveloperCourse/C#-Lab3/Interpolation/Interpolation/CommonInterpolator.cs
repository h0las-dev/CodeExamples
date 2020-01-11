namespace Interpolation
{
    internal class CommonInterpolator
    {
        protected double[] Values;

        public CommonInterpolator(double[] values)
        {
            this.Values = values;
        }

        public virtual double CalculateValue(double x)
        {
            return 0;
        }
    }
}
