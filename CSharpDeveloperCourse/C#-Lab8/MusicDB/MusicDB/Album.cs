namespace MusicDB
{
    using System;
    using System.Collections.Generic;

    public sealed class Album
    {
        public readonly int Id;

        public readonly string Title;

        public readonly DateTime AlbumDate;

        public readonly List<Song> Songs = new List<Song>();

        public Album(int id, DateTime date, string title)
        {
            this.Id = id;
            this.AlbumDate = date;
            this.Title = title;
        }

        public bool Expanded { get; set; }
    }
}
