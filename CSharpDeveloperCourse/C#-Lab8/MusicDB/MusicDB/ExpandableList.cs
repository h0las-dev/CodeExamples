namespace MusicDB
{
    using System;
    using System.Text;

    public sealed class ExpandableList
    {
        private readonly StringBuilder buffer = new StringBuilder();
        private readonly int bufferWidth = Console.BufferWidth;
        private readonly int dataHeight;
        private readonly string ellipsisLine;

        private readonly MusicListModel listModel;
    
        private int offset;
        private int selectedIndex;
        private int totalCount;
    
        public ExpandableList(MusicListModel listModel)
        {
            this.listModel = listModel;

            this.buffer.Append(' ');
            while (this.buffer.Length <= this.bufferWidth - 2)
            {
                this.buffer.Append("^ ");
            }

            this.buffer.Append(' ', this.bufferWidth - this.buffer.Length);
            
            this.ellipsisLine = this.buffer.ToString();
            this.buffer.Clear();

            this.offset = 0;
            this.selectedIndex = -1;
            
            if (listModel != null)
            {
                listModel.ChangeEvent += this.ChangeHandle;
            }

            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            this.dataHeight = Console.BufferHeight - 2;

            this.ChangeHandle();
        }

        public object SelectedItem
        {
            get
            {
                var index = this.SelectedIndex;

                foreach (var album in this.listModel.Albums)
                {
                    if (index-- == 0)
                    {
                        return album;
                    }

                    foreach (var song in album.Songs)
                    {
                        if (index-- == 0)
                        {
                            return song;
                        }
                    }
                }

                return null;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                if (value <= -1)
                {
                    this.selectedIndex = -1;
                    this.offset = 0;
                }
                else
                {
                    this.selectedIndex = (value >= this.totalCount) ? this.totalCount - 1 : value;

                    this.offset = this.offset > this.selectedIndex
                                 ? this.selectedIndex
                                 : (this.offset <= this.selectedIndex - this.dataHeight ? this.selectedIndex - this.dataHeight + 1 : this.offset);
                }

                this.Draw();
            }
        }

        public void ChangeHandle()
        {
            this.totalCount = this.listModel.Albums.Count;

            foreach (var album in this.listModel.Albums)
            {
                this.totalCount += album.Songs.Count;
            }

            this.selectedIndex = this.selectedIndex >= this.totalCount ? this.totalCount - 1 : this.selectedIndex;

            this.Draw();
        }

        public void Run()
        {
            this.ChangeHandle();

            while (true)
            {
                var keyInfo = Console.ReadKey();
                var item = this.SelectedItem;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        return;

                    case ConsoleKey.DownArrow:
                        this.SelectedIndex++;
                        break;

                    case ConsoleKey.PageDown:
                        this.SelectedIndex += this.dataHeight;
                        break;

                    case ConsoleKey.UpArrow:
                        if (this.SelectedIndex > 0)
                        {
                            this.SelectedIndex--;
                        }

                        break;

                    case ConsoleKey.PageUp:
                        this.SelectedIndex = this.SelectedIndex > this.dataHeight ? this.SelectedIndex - this.dataHeight : 0;
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Enter:
                        var selectedAlbum = item as Album;
                        if (selectedAlbum != null)
                        {
                            this.listModel.CollapseAll();
                            if (keyInfo.Key != ConsoleKey.LeftArrow)
                            {
                                this.listModel.ExpandAlbum(selectedAlbum);
                            }
                        }

                        break;

                    case ConsoleKey.Delete:
                        if (item is Album)
                        {
                            this.listModel.Delete(item as Album);
                        }

                        if (item is Song)
                        {
                            this.listModel.Delete(item as Song);
                        }

                        break;

                    case ConsoleKey.A:
                        var albumData = ConsoleDialog.CreateAlbum();
                        if (albumData != null)
                        {
                            this.listModel.AddAlbum(albumData.Item1, albumData.Item2);
                            this.SelectedIndex = this.totalCount - 1;
                        }
                        else
                        {
                            this.listModel.CollapseAll();
                        }

                        break;

                    case ConsoleKey.S:
                        if (item != null)
                        {
                            var songData = ConsoleDialog.CreateSong();
                            if (songData != null)
                            {
                                this.listModel.AddSong(
                                    songData.Item1, songData.Item2, item is Song ? (item as Song).Alb : item as Album);
                            }
                            else
                            {
                                this.listModel.CollapseAll();
                            }
                        }

                        break;
                }

                // We have to clear unused input from the console buffer
                Console.Write("\b \b");
            }
        }

        private void Draw()
        {
            var bufferHeight = Console.BufferHeight;
            var contentHeight = bufferHeight - 2;

            // One empty line at the top
            this.buffer.Clear();
            if (this.offset > 0)
            {
                this.AddEllipsis();
            }
            else
            {
                this.AddSpace();
            }

            // Albums and songs
            var lineIndex = 0;
            var tail = false;
            foreach (var album in this.listModel.Albums)
            {
                if (lineIndex++ >= this.offset)
                {
                    if (lineIndex > this.offset + contentHeight)
                    {
                        tail = true;
                        break;
                    }

                    this.DrawAlbum(album);
                }

                var order = 0;
                foreach (var song in album.Songs)
                {
                    if (lineIndex++ >= this.offset)
                    {
                        if (lineIndex > this.offset + contentHeight)
                        {
                            tail = true;
                            break;
                        }

                        this.DrawSong(song, ++order);
                    }
                }
            }

            // Trailing lines
            while (this.buffer.Length < bufferHeight * this.bufferWidth)
            {
                if (tail)
                {
                    this.AddEllipsis();
                }
                else
                {
                    this.AddSpace();
                }
            }

            // For slow console, it could be less flicker to draw evertyng first 
            Console.CursorVisible = false;
            this.buffer.Length = (bufferHeight * this.bufferWidth) - 1;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Console.Write(this.buffer.ToString());

            // And than, redraw selected item in is's highlighted state
            if (this.selectedIndex >= this.offset && this.selectedIndex < this.offset + contentHeight)
            {
                var bufferIndex = this.selectedIndex - this.offset + 1;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, bufferIndex);
                Console.Write(this.buffer.ToString(bufferIndex * this.bufferWidth, this.bufferWidth));
            }

            // This is the safe cursof position, preventing the scrolling
            Console.SetCursorPosition(0, 0);
            Console.ResetColor();
        }

        private void AddEllipsis()
        {
            this.buffer.Append(this.ellipsisLine);
        }

        private void AddSpace()
        {
            this.buffer.Append(' ', this.bufferWidth);
        }

        private void DrawAlbum(Album album)
        {
            var displayTitle = album.Title.Length < (this.bufferWidth - 10) ? album.Title : album.Title.Substring(0, this.bufferWidth - 10);
            this.buffer.Append(
                string.Format(
                    "{0} {1,-" + (this.bufferWidth - 3) + "} ",
                    album.Expanded ? '-' : '+',
                    displayTitle + "(" + album.AlbumDate.Year + ")"));
        }

        private void DrawSong(Song song, int order)
        {
            var displayTitle = song.Title.Length < (this.bufferWidth - 20) ? song.Title : song.Title.Substring(0, this.bufferWidth - 20);
            this.buffer.Append(
                string.Format(
                "{0,5}.{1,-" + (this.bufferWidth - 17) + "} {2:c} s", 
                order, 
                displayTitle, 
                song.Duration));
        }
    }
}
