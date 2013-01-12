namespace ClearLines.FsiControl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using ClearLines.FsiRunner;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class EditorViewModel : ViewModelBase
    {
        private FsiSession session;
        private RelayCommand run;
        private RelayCommand addCodeBlock;
        private string feedback;
        private ObservableCollection<CodeBlock> codeBlocks;

        public EditorViewModel()
        {
            this.feedback = "";
            this.run = new RelayCommand(OnRun, CanRun);
            this.addCodeBlock = new RelayCommand(OnAddCodeBlock);

            var fsiPath = @"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
            this.session = new FsiSession(fsiPath);

            this.Session.Start();
            this.Session.OutputReceived += OnOutputReceived;
            this.Session.ErrorReceived += OnErrorReceived;

            this.codeBlocks = new ObservableCollection<CodeBlock>();
            this.CodeBlocks.Add(new CodeBlock(this.Session));
        }

        public ObservableCollection<CodeBlock> CodeBlocks
        {
            get { return this.codeBlocks; }
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

        public ICommand AddCodeBlock
        {
            get { return this.addCodeBlock; }
        }

        private void OnRun()
        {
            // TODO: implement as run-all
        }

        private bool CanRun()
        {
            return false;
        }

        private void OnAddCodeBlock()
        {
            this.CodeBlocks.Add(new CodeBlock(this.Session));
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
