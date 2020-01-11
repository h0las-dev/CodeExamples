namespace Interpolation
{
    using System;

    internal class SplineInterpolation : CommonInterpolator
    {
        private double[] k;
        private double[] s;
        private double[] args;
        private double[,] matrix;

        public SplineInterpolation(double[] values) : base(values)
        {
            // nothing to do.
        }

        public override double CalculateValue(double x)
        {
            this.InitializeMatrix();
            this.SolveMatrix();

            if (this.Values.Length < 2)
            {
                throw new ArgumentException();
            }

            var xleft = 0.0;
            var xRight = 0.0;
            var leftIndex = 0;
            var rightIndex = 0;

            for (var i = 0; i < this.Values.Length - 1; i++)
            {
                if ((x < this.args[i + 1]) && (x > this.args[i]))
                {
                    xleft = this.args[i];
                    leftIndex = i;
                    xRight = this.args[i + 1];
                    rightIndex = i + 1;
                }
            }

            if (xleft == xRight)
            {
                throw new ArgumentException();
            }

            var t = (x - xleft) / (xRight - xleft);
            var a = (this.k[leftIndex] * (xRight - xleft)) - (this.Values[rightIndex] - this.Values[leftIndex]);
            var b = (-this.k[rightIndex] * (xRight - xleft)) + (this.Values[rightIndex] - this.Values[leftIndex]);
            var q = ((1 - t) * this.Values[leftIndex]) + (t * this.Values[rightIndex]) + ((t * (1 - t)) * ((a * (1 - t)) + (b * t)));
            return q;
        }

        private void InitializeMatrix()
        {
            this.args = new double[this.Values.Length];
            for (var i = 0; i < this.Values.Length; i++)
            {
                this.args[i] = i;
            }

            this.s = new double[this.Values.Length];

            this.matrix = new double[this.Values.Length, this.Values.Length];

            for (var i = 0; i < this.Values.Length; i++)
            {
                if (i == 0)
                {
                    this.matrix[0, 0] = 2 / (this.args[1] - this.args[0]);
                    this.matrix[0, 1] = 1 / (this.args[1] - this.args[0]);
                    this.s[0] = 3 * (this.Values[1] - this.Values[0]) / Math.Pow(this.args[1] - this.args[0], 2);
                }
                else if (i == this.Values.Length - 1)
                {
                    this.matrix[this.Values.Length - 1, this.Values.Length - 2] =
                        1 / (this.args[this.Values.Length - 1] - this.args[this.Values.Length - 2]);
                    this.matrix[this.Values.Length - 1, this.Values.Length - 1] =
                        2 / (this.args[Values.Length - 1] - this.args[Values.Length - 2]);
                    this.s[this.Values.Length - 1] = 3 * (this.Values[this.Values.Length - 1] - this.Values[this.Values.Length - 2]) / Math.Pow(this.args[this.Values.Length - 1] - this.args[this.Values.Length - 2], 2);
                }
                else
                {
                    for (var j = 0; j < this.Values.Length; j++)
                    {
                        if ((j > i + 1) || (j < i - 1))
                        {
                            this.matrix[i, j] = 0;
                        }
                        else
                        {
                            if (j == i - 1)
                            {
                                this.matrix[i, j] = 1 / (this.args[i] - this.args[i - 1]);
                            }
                            else if (j == i + 1)
                            {
                                this.matrix[i, j] = 1 / (this.args[i + 1] - this.args[i]);
                            }
                            else
                            {
                                // if i == j.
                                this.matrix[i, j] = 2 * ((1 / (this.args[i] - this.args[i - 1])) + (1 / (this.args[i + 1] - this.args[i])));
                            }
                        }
                    }

                    this.s[i] = 3 * (((this.Values[i] - this.Values[i - 1]) / Math.Pow(this.args[i] - this.args[i - 1], 2)) +
                                ((this.Values[i + 1] - this.Values[i]) / Math.Pow(this.args[i + 1] - this.args[i], 2)));
                }
            }
        }

        private void SolveMatrix()
        {
            this.k = new double[this.Values.Length];

            var matrixP = new double[this.Values.Length];
            var matrixQ = new double[this.Values.Length];

            for (var i = 0; i < this.Values.Length; i++)
            {
                matrixP[i] = 0;
                matrixQ[i] = 0;
            }

            matrixP[0] = this.matrix[0, 1] / (-this.matrix[0, 0]);
            matrixQ[0] = this.s[0] / this.matrix[0, 0];

            for (int i = 1; i < this.Values.Length - 1; i++)
            {
                matrixP[i] = this.matrix[i, i + 1] / (-this.matrix[i, i] - (this.matrix[i, i - 1] * matrixP[i - 1]));
                matrixQ[i] = (-this.s[i] + (this.matrix[i, i - 1] * matrixQ[i - 1])) /
                             (-this.matrix[i, i] - (this.matrix[i, i - 1] * matrixP[i - 1]));
            }

            matrixQ[this.Values.Length - 1] =
                (this.s[this.Values.Length - 1] - (this.matrix[this.Values.Length - 1, this.Values.Length - 2] * matrixQ[this.Values.Length - 2])) /
                (this.matrix[this.Values.Length - 1, this.Values.Length - 1] +
                 (this.matrix[this.Values.Length - 1, this.Values.Length - 2] * matrixP[this.Values.Length - 2]));

            this.k[this.Values.Length - 1] = matrixQ[this.Values.Length - 1];

            for (var i = this.Values.Length - 2; i >= 0; i--)
            {
                this.k[i] = (matrixP[i] * this.k[i + 1]) + matrixQ[i];
            }
        }
    }
}