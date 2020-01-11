namespace Interpolation
{
    using System;

    public class Program
    {
        private const double SamplePoint = 1.75;

        public static void Main()
        {
            double[] values = { 0, 2, 1, 4 };
            CommonInterpolator[] interpolators = 
            {
                new StepInterpolator(values),
                new LinearInterpolator(values), new LagrangeInterpolation(values), new SplineInterpolation(values), 
            };

            Console.WriteLine("Calculating value at sample point: {0}", SamplePoint);

            foreach (var interpolator in interpolators)
            {
                Console.WriteLine("Class {0}: Interpolated value is {1}", interpolator.GetType().Name, interpolator.CalculateValue(SamplePoint));
            }

            Console.ReadLine();
        }
    }
}
