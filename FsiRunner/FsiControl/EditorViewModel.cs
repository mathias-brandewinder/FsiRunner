namespace ClearLines.FsiControl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Input;
    using ClearLines.FsiRunner;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class EditorViewModel : ViewModelBase
    {
        private FsiSession session;
        private string code;
        private RelayCommand run;
        private string feedback;

        public EditorViewModel()
        {
            this.code = "Hello World";
            this.run = new RelayCommand(OnRun, CanRun);
            this.feedback = "";

            var fsiPath = @"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
            this.session = new FsiSession(fsiPath);

            this.Session.Start();
            this.Session.OutputReceived += OnOutputReceived;
            this.Session.ErrorReceived += OnErrorReceived;

        }

        public string CodeBlock
        {
            get { return this.code; }
            set
            {
                if (this.code != value)
                {
                    this.code = value;
                    base.RaisePropertyChanged("CodeBlock");
                }
            }
        }

        public string Feedback
        {
            get { return this.feedback; }
            set
            {
                if (this.feedback != value)
                {
                    this.feedback = value;
                    base.RaisePropertyChanged("Feedback");
                }
            }
        }

        private FsiSession Session
        {
            get { return this.session; }
        }

        public ICommand Run
        {
            get { return this.run; }
        }

        private void OnRun()
        {
            var lines = this.BreakLines(this.CodeBlock);
            foreach (var line in lines)
            {
                this.Session.AddLine(line);
            }

            this.Session.Evaluate();
        }

        private bool CanRun()
        {
            return true;
        }

        private void OnOutputReceived(object sender, DataReceivedEventArgs e)
        {

            this.Feedback = this.Feedback + Environment.NewLine + e.Data;
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            this.Feedback = this.Feedback + Environment.NewLine + e.Data;
        }

        private IEnumerable<string> BreakLines(string text)
        {
            return text.Split((char)10);
        }
    }
}
