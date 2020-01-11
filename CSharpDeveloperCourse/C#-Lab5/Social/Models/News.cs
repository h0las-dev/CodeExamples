namespace Social.Models
{
    using System.Collections.Generic;

    public struct News
    {
        public int AuthorId;

        public string AuthorName;

        public List<int> Likes;

        public string Text;

        public News(int id, string name, List<int> likes, string text)
        {
            this.AuthorId = id;
            this.Likes = likes;
            this.AuthorName = name;
            this.Text = text;
        }
    }
}