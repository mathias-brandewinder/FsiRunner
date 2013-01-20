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
    using GalaSoft.MvvmLight.Threading;
    using global::FsiControl;

    public class EditorViewModel : ViewModelBase
    {
        private FsiSession session;
        private RelayCommand run;
        private RelayCommand addCodeBlock;
        private string feedback;
        private ObservableCollection<CodeBlock> codeBlocks;
        private ObservableCollection<FeedbackBlock> feedbackBlocks;
        private readonly IConfiguration configuration;

        public EditorViewModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            DispatcherHelper.Initialize();

            this.feedback = "";
            this.run = new RelayCommand(OnRun, CanRun);
            this.addCodeBlock = new RelayCommand(OnAddCodeBlock);

            var fsiPath = this.configuration.FsiLocation; //@"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
            this.session = new FsiSession(fsiPath);

            this.Session.Start();
            this.Session.OutputReceived += OnOutputReceived;
            this.Session.ErrorReceived += OnErrorReceived;

            this.codeBlocks = new ObservableCollection<CodeBlock>();
            this.CodeBlocks.Add(new CodeBlock(this.Session));

            this.feedbackBlocks = new ObservableCollection<FeedbackBlock>();
        }

        public ObservableCollection<CodeBlock> CodeBlocks
        {
            get { return this.codeBlocks; }
        }

        public ObservableCollection<FeedbackBlock> FeedbackBlocks
        {
            get { return this.feedbackBlocks; }
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
            this.AddFeedbackBlock(e);
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            this.AddFeedbackBlock(e);
        }

        private void AddFeedbackBlock(DataReceivedEventArgs e)
        {
            if (e.Data.Length == 0)
            {
                return;
            }

            if (e.Data.StartsWith("> "))
            {
                return;
            }

            if (e.Data.Length > 0)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var feedback = new FeedbackBlock(e.Data);
                    this.FeedbackBlocks.Add(feedback);
                });
            }
        }

        private IEnumerable<string> BreakLines(string text)
        {
            return text.Split((char)10);
        }
    }
}
