namespace Social.Models
{
    using System;

    public struct User
    {
        public DateTime DateofBirth;

        public int Gender;

        public DateTime LastVisir;

        public string Name;

        public int Online;

        public int UserId;

        public User(DateTime db, int gender, DateTime lv, string name, int status, int id)
        {
            this.DateofBirth = db;
            this.Gender = gender;
            this.LastVisir = lv;
            this.Name = name;
            this.Online = status;
            this.UserId = id;
        }
    }
}