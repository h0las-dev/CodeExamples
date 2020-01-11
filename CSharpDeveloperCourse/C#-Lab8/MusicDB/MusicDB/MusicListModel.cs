namespace MusicDB
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class MusicListModel
    {
        public readonly List<Album> Albums = new List<Album>();

        private readonly string connectionString;

        public MusicListModel(string connectionString)
        {
            this.connectionString = connectionString;

            this.ReadAlbumsList();
        }

        public event Action ChangeEvent;

        public void CollapseAll()
        {
            foreach (var album in this.Albums)
            {
                album.Songs.Clear();
                album.Expanded = false;
            }

            this.Changed();
        }

        public void ExpandAlbum(Album album)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM [songs] WHERE albumId = @AlbumId", connection);
                command.Parameters.AddWithValue("@AlbumId", album.Id);
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        album.Songs.Add(
                            new Song(
                                (int)dataReader["songId"],
                                album,
                                (string)dataReader["title"],
                                (TimeSpan)dataReader["duration"]));
                    }
                }
            }
            
            album.Expanded = true;
            this.Changed();
        }

        public void ReadAlbumsList()
        {
            this.Albums.Clear();

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM [albums]", connection);
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        this.Albums.Add(
                            new Album(
                                (int)dataReader["albumId"], (DateTime)dataReader["date"], (string)dataReader["title"]));
                    }
                }
            }

            this.Changed();
        }

        public void Delete(Song song)
        {
            var album = song.Alb;
            
            if (this.Albums.IndexOf(album) >= 0)
            {
                if (album != null)
                {
                    album.Songs.Remove(song);
                }

                using (var connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("DELETE FROM [songs] WHERE songId = @SongId", connection);
                    command.Parameters.AddWithValue("@SongId", song.Id);
                    command.ExecuteNonQuery();
                }
                
                this.Changed();
            }
        }

        public void Delete(Album album)
        {
            if (this.Albums.Remove(album))
            {
                using (var connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("DELETE FROM [albums] WHERE albumId = @AlbumId", connection);
                    command.Parameters.AddWithValue("@AlbumId", album.Id);
                    command.ExecuteNonQuery();
                }

                this.Changed();
            }
        }

        public void AddSong(string songTitle, TimeSpan songDuration,  Album album)
        {
            if (album == null)
            {
                return;
            }

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("uspAddSong", connection)
                    { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@Title", songTitle);
                command.Parameters.AddWithValue("@Duration", songDuration.ToString());
                command.Parameters.AddWithValue("@AlbumId", album.Id);
                var returnParam = command.Parameters.Add("@NewSongId", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;
                var rowCount = command.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    var songId = (int)returnParam.Value;
                    album.Songs.Add(new Song(songId, album, songTitle, songDuration));
                    album.Expanded = true;
                    this.Changed();
                }
            }
        }

        public void AddAlbum(string albumTitle, DateTime albumDate)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand("uspAddAlbum", connection)
                    { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@Title", albumTitle);
                command.Parameters.AddWithValue("@Date", albumDate.ToString());
                var returnParam = command.Parameters.Add("@NewAlbumId", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;
                var rowCount = command.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    var albumId = (int)returnParam.Value;
                    this.Albums.Add(new Album(albumId, albumDate, albumTitle));
                    this.Changed();
                }
            }
        }

        private void Changed()
        {
            if (this.ChangeEvent != null)
            {
                this.ChangeEvent();
            }
        }
    }
}
