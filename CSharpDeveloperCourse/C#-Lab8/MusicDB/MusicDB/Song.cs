namespace MusicDB
{
    using System;

    public sealed class Song
    {
        public readonly int Id;

        public readonly Album Alb;

        public readonly string Title;

        public readonly TimeSpan Duration;

        public Song(int id, Album album, string title, TimeSpan duration)
        {
            this.Id = id;
            this.Alb = album;
            this.Title = title;
            this.Duration = duration;
        }
    }
}
