namespace SecretBook
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public class DataProducer
    {
        private List<Note> notes;

        public DataProducer()
        {
            this.notes = new List<Note>();
        }

        public string DecryptData { get; set; }

        public string UserName { get; set; }

        public string DataHash { get; set; }

        public void ParseNotes()
        {
            this.ParseDataHash();

            if (!this.CheckDataHash())
            {
                throw new Exception("The data file was broken!");
            }

            var userRegex = new Regex(@"<Username>(.|\n|\t|\s)+?<\/Username>");
            var noteRegex = new Regex(@"<Note>(.|\n|\t|\s)+?<\/Note>");
            var headRegex = new Regex(@"<Header>(.|\n|\t|\s)+?<\/Header>");
            var bodyRegex = new Regex(@"<Body>(.|\n|\t|\s)+?<\/Body>");
            var dateRegex = new Regex(@"<Date>(.|\n|\t|\s)+?<\/Date>");

            this.UserName = Regex.Replace(Regex.Replace(userRegex.Match(this.DecryptData).Value, @"(<Username>|</Username>)", string.Empty), @"\s+", string.Empty).Trim();

            var note = noteRegex.Match(this.DecryptData);
            if (note != Match.Empty)
            {
                while (note.Success)
                {
                    var currentNote = new Note();

                    currentNote.Header = Regex.Replace(headRegex.Match(note.Value).Value, @"(<Header>|</Header>)", string.Empty);
                    currentNote.Body = Regex.Replace(bodyRegex.Match(note.Value).Value, @"(<Body>|</Body>)", string.Empty);
                    currentNote.Date = Convert.ToDateTime(Regex.Replace(dateRegex.Match(note.Value).Value, @"(<Date>|</Date>)", string.Empty));

                    this.notes.Add(currentNote);

                    note = note.NextMatch();
                }
            }
            else
            {
                Console.WriteLine("Sorry, your book is empty!\n");
            }
        }

        public void ShowMenu()
        {
            if (this.DecryptData == string.Empty || this.DecryptData == null)
            {
                Console.WriteLine("Hello, user! Please, enter your username: ");
                this.UserName = Console.ReadLine();

                this.DecryptData = "<Username>\n" + this.UserName + "\n</Username>\n";
            }

            Console.Clear();

            var inputKey = 0;

            Console.WriteLine("Hello, " + this.UserName + "!\n");

            do
            {
                Console.WriteLine("1. Show all notes");
                Console.WriteLine("2. Get note by index");
                Console.WriteLine("3. Change header by index");
                Console.WriteLine("4. Delete note by index");
                Console.WriteLine("5. Add new note");

                Console.WriteLine("0. Save and exit");
                Console.WriteLine();
                Console.Write(">>> ");
                var inputStr = Console.ReadLine();
                if (Int32.TryParse(inputStr, out inputKey))
                {
                    switch (inputKey)
                    {
                        case 0:
                            Console.Clear();
                            Console.WriteLine("Save...");
                            this.Save();
                            Console.WriteLine("Exit...");
                            Console.WriteLine("Bye!");
                            break;
                        case 1:
                            Console.Clear();
                            this.ShowAllNotes();
                            break;
                        case 2:
                            Console.WriteLine("Enter index of note: ");
                            Console.Write(">>> ");
                            var noteID = Convert.ToInt32(Console.ReadLine());
                            this.ShowNoteByNumber(noteID);
                            break;
                        case 3:
                            Console.WriteLine("Enter index of note: ");
                            Console.Write(">>> ");
                            noteID = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter new header of note: ");
                            Console.Write(">>> ");
                            var newHeader = Console.ReadLine();
                            this.ChangeName(noteID, newHeader);
                            Console.Clear();
                            break;
                        case 4:
                            Console.WriteLine("Enter index of note: ");
                            Console.Write(">>> ");
                            noteID = Convert.ToInt32(Console.ReadLine());
                            this.RemoveNoteByIndex(noteID);
                            Console.Clear();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("Enter head of note: ");
                            Console.Write(">>> ");
                            var head = Console.ReadLine();
                            Console.WriteLine("Enter new note: ");
                            Console.Write(">>> ");
                            var body = Console.ReadLine();
                            this.AddNoteToNotes(head, body);
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Incorrect input! Try again: ");
                            continue;
                    }
                }
                else
                {
                    inputKey = -1;
                    Console.Clear();
                    continue;
                }
            }
            while (inputKey != 0);
        }

        private void ShowAllNotes()
        {
            Console.Write("     note ID\t\t\t\tnote header\n\n");

            if (this.notes.Count == 0)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("    " + "Your book is empty!" + "\n");
                Console.WriteLine("--------------------------------------------------------------------------------");
            }

            foreach (var note in this.notes)
            {
                this.ShowHeaderOnly(note);
            }
        }

        private void ShowNoteByNumber(int noteID)
        {
            if (noteID < 0 || noteID >= this.notes.Count)
            {
                throw new ArgumentException("incorrect noteID");
            }

            var note = this.notes[noteID];

            this.ShowNote(note);
        }

        private void ShowNote(Note note)
        {
            Console.Clear();
            var noteID = 0;
            Console.Write("     note ID\t\t\t\tnote header\n\n");
            Console.WriteLine("--------------------------------------------------------------------------------");
            foreach (var currentNote in this.notes)
            {
                if (currentNote.Header == note.Header)
                {
                    noteID = this.notes.IndexOf(note);
                    Console.Write("\t{0}.\t\t\t", noteID);
                    Console.WriteLine("*********/ " + note.Header + " /*********\n");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    Console.WriteLine("    " + note.Body + "\n");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    Console.WriteLine("                                          " + note.Date + "\n");
                    Console.WriteLine("--------------------------------------------------------------------------------\n");
                    break;
                }
            }
        }

        private void ShowHeaderOnly(Note note)
        {
            var noteID = 0;

            foreach (var currentNote in this.notes)
            {
                if (currentNote.Header == note.Header)
                {
                    noteID = this.notes.IndexOf(note);
                    Console.Write("\t{0}.\t\t\t", noteID);
                    Console.WriteLine("*********/ " + note.Header + " /*********\n");
                }
            }
        }

        private void AddNoteToNotes(string head, string body)
        {
            var currentDate = DateTime.Now;
            var newNote = new Note();
            newNote.Header = head;
            newNote.Body = body;
            newNote.Date = currentDate;

            this.notes.Add(newNote);

            this.DecryptData += this.GetFormatNote(head, body, currentDate);
        }

        private void RemoveNoteByIndex(int noteID)
        {
            if (noteID >= 0 && noteID < this.notes.Count)
            {
                this.notes.Remove(this.notes[noteID]);
            }
            else
            {
                 throw new ArgumentException("incorrect noteID");
            }

            this.DecryptData = string.Empty;
            this.DecryptData = "<Username>\n" + "\t" + this.UserName + "\n</Username>\n";

            foreach (var note in this.notes)
            {
                this.DecryptData += this.GetFormatNote(note.Header, note.Body, note.Date);
            }
        }

        private void ChangeName(int noteID, string newHeader)
        {
            if (noteID < 0 || noteID >= this.notes.Count)
            {
                throw new ArgumentException("incorrect noteID");
            }

            var oldHeader = this.notes[noteID].Header;

            var noteRegex = new Regex(@"<Note>(.|\n|\t|\s)+?<\/Note>");

            var headRegex = new Regex(@"<Header>(.|\n|\t|\s)+?<\/Header>");

            var note = noteRegex.Match(this.DecryptData);
            if (note != Match.Empty)
            {
                while (note.Success)
                {
                    var currentNote = new Note();

                    currentNote.Header = Regex.Replace(headRegex.Match(note.Value).Value, @"(<Header>|</Header>)", string.Empty);
                    if (currentNote.Header == oldHeader)
                    {
                        this.notes[noteID].Header = newHeader;

                        var newNote = Regex.Replace(note.Value, @"(<Header>" + oldHeader + @"</Header>)", @"(<Header>" + newHeader + @"</Header>)");

                        this.DecryptData = this.DecryptData.Replace(note.Value, newNote);

                        break;
                    }

                    note = note.NextMatch();
                }
            }
            else
            {
                Console.WriteLine("Your book is empty.");
            }
        }

        private void Save()
        {
            this.DecryptData = "<Username>\n" + "\t" + this.UserName + "\n</Username>\n";

            foreach (var note in this.notes)
            {
                this.DecryptData += this.GetFormatNote(note.Header, note.Body, note.Date);
            }

            this.DataHash = this.GetDataHash();
            this.DecryptData += "<Hash>\n" + this.DataHash + "\n</Hash>";
        }

        private string GetFormatNote(string head, string body, DateTime currentDate)
        {
            var str = "<Note>\n" + "\t" + "<Header>" + head + "</Header>\n" + "\t" + "<Body>" + body + "</Body>\n" + "\t" + "<Date>" + currentDate + "</Date>\n" + "</Note>\n";

            return str;
        }

        private string GetDataHash()
        {
            var decryptDataNoHash = Regex.Replace(this.DecryptData, @"<Hash>(.|\n|\t|\s)+?<\/Hash>", string.Empty);
            byte[] bytes = Encoding.UTF8.GetBytes(decryptDataNoHash);

            MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider();

            byte[] byteHash = cryptoProvider.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }

            return hash;
        }

        private bool CheckDataHash()
        {
            string hash = this.GetDataHash();

            return hash == this.DataHash;
        }

        private void ParseDataHash()
        {
            var hashRegex = new Regex(@"<Hash>(.|\n|\t|\s)+?<\/Hash>");

            this.DataHash = Regex.Replace(Regex.Replace(hashRegex.Match(this.DecryptData).Value, @"(<Hash>|</Hash>)", string.Empty), @"\s+", string.Empty);
        }
    }
}