namespace SecretBook
{
    using System;

    public class Note
    {
        public Note()
        {
        }

        public Note(string newHeader, string newBody, DateTime newDate)
        {
            this.Header = newHeader;
            this.Body = newBody;
            this.Date = newDate;
        }

        public string Header
        {
            get;

            set; 
        }

        public string Body
        {
            get;

            set;
        }

        public DateTime Date
        {
            get;

            set; 
        }
    }
}