namespace Calculator
{
    public class Operation
    {
        private string sign;
        private double[] parameters;

        public string Sign
        {
            get
            {
                return this.sign;
            }

            set
            {
                this.sign = value;
            }
        }

        public double[] Parameters
        {
            get
            {
                return this.parameters;
            }

            set
            {
                this.parameters = value;
            }
        }
    }
}