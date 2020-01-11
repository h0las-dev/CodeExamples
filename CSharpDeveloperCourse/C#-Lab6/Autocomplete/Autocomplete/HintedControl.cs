namespace Autocomplete
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public delegate void HintedControlEventHandler(HintedControl sender);

    public sealed class HintedControl
    {
        private readonly StringBuilder text = new StringBuilder();
        private string hint = string.Empty;
        private int lastWordLength;

        public event HintedControlEventHandler TypingEvent;

        public string Hint
        {
            get
            {
                return this.hint;
            }

            set
            {
                if (value != this.Hint)
                {
                    this.SetHint(value);
                }
            }
        }

        public string Text
        {
            get
            {
                return this.text.ToString();
            }

            set
            {
                if (value != this.Text)
                {
                    this.SetText(value);
                }
            }
        }

        public string LastWord
        {
            get
            {
                return this.text.ToString(this.text.Length - this.lastWordLength, this.lastWordLength);
            }

            set
            {
                if (value != this.LastWord)
                {
                    this.SetLastWord(value);
                }
            }
        }

        public void Update()
        {
            while (this.ProcessInput())
            {
                this.TypingEvent?.Invoke(this);
            }
        }

        private void DetectLastWord()
        {
            var match = Regex.Match(this.text.ToString(), @"\w+$", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            this.lastWordLength = match.Length;
        }

        private void SetText(string value)
        {
            this.text.Clear();
            this.text.Append(value);
            this.DetectLastWord();
            this.Display();
        }

        private void SetHint(string value)
        {
            this.hint = value;
            this.Display();
        }

        private void SetLastWord(string value)
        {
            if (this.lastWordLength > 0)
            {
                this.text.Remove(Math.Max(0, this.text.Length - this.lastWordLength), this.lastWordLength);
            }

            this.lastWordLength = value.Length;
            this.text.Append(value);
            this.Display();
        }

        private void Display()
        {
            lock (this)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(this.text);

                var left = Console.CursorLeft;
                var top = Console.CursorTop;

                Console.SetCursorPosition(
                    Math.Max(
                        0,
                        Math.Min(left - this.lastWordLength, Console.BufferWidth - (this.hint ?? string.Empty).Length)),
                    top + 1);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(this.hint);

                Console.SetCursorPosition(left, top);
            }
        }

        private bool ProcessInput()
        {
            var key = Console.ReadKey(false);

            if (char.IsControl(key.KeyChar))
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        this.LastWord = this.Hint;
                        return true;
                    
                    case ConsoleKey.Enter:
                        this.text.AppendLine();
                        break;
                    
                    case ConsoleKey.Backspace:
                        if (this.text.Length > 0)
                        {
                            this.text.Length--;
                        }

                        break;
                    
                    case ConsoleKey.Escape:
                        return false;
                    
                    default:
                        this.text.Clear();
                        break;
                }
            }
            else
            {
                this.text.Append(key.KeyChar);
            }

            this.DetectLastWord();
            this.Display();
            return true;
        }        
    }
}