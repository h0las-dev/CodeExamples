namespace Autocomplete
{
    using System.Threading;

    public class AutoThread
    {
        private Thread thread;
        private EventWaitHandle handleThread;

        public AutoThread(Thread tr, EventWaitHandle wh)
        {
            this.thread = tr;
            this.handleThread = wh;
        }

        public Thread Thread
        {
            get { return this.thread; }
            set { this.thread = value; }
        }

        public EventWaitHandle HandleThread
        {
            get { return this.handleThread; }
            set { this.handleThread = value; }
        }
    }
}