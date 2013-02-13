using System.IO;
using System.Reflection;
using System.Xaml;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

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
        private RelayCommand increaseFontSize;
        private RelayCommand decreaseFontSize;

        private string feedback;
        private ObservableCollection<CodeBlock> codeBlocks;
        private ObservableCollection<FeedbackBlock> feedbackBlocks;
        private readonly IConfiguration configuration;
        private double fontSize;
        private IHighlightingDefinition syntaxHighlighting;

        public EditorViewModel(IConfiguration configuration)
        {
            this.fontSize = 13.0;
            this.configuration = configuration;
            DispatcherHelper.Initialize();

            this.feedback = "";
            this.run = new RelayCommand(OnRun, CanRun);
            this.addCodeBlock = new RelayCommand(OnAddCodeBlock);

            this.increaseFontSize = new RelayCommand(OnIncreaseFont);
            this.decreaseFontSize = new RelayCommand(OnDecreaseFont);

            var fsiPath = this.configuration.FsiLocation; //@"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
            this.session = new FsiSession(fsiPath);

            this.Session.Start();
            this.Session.OutputReceived += OnOutputReceived;
            this.Session.ErrorReceived += OnErrorReceived;

            var editorLocation = Assembly.GetExecutingAssembly().Location;
            var editorPath = Path.GetDirectoryName(editorLocation);
            var highlightFileName = "FSharpHighlighting.xshd";
            var highlightFileLocation = Path.Combine(editorPath, highlightFileName);

            using (var stream = File.OpenRead(highlightFileLocation))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    this.syntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }

            this.codeBlocks = new ObservableCollection<CodeBlock>();
            this.CodeBlocks.Add(new CodeBlock(this.Session, this.syntaxHighlighting));

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

        public double FontSize
        {
            get { return this.fontSize; }
            set
            {
                if (value < 5.0)
                {
                    return;
                }

                if (value != this.fontSize)
                {
                    this.fontSize = value;
                    base.RaisePropertyChanged("FontSize");
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

        public ICommand AddCodeBlock
        {
            get { return this.addCodeBlock; }
        }

        public ICommand IncreaseFontSize
        {
            get { return this.increaseFontSize; }
        }

        public ICommand DecreaseFontSize
        {
            get { return this.decreaseFontSize; }
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
            var codeBlock = new CodeBlock(this.Session, this.syntaxHighlighting) { FontSize = this.FontSize };
            this.CodeBlocks.Add(codeBlock);
        }

        private void OnIncreaseFont()
        {
            this.FontSize += 0.5;

            foreach (var codeblock in this.CodeBlocks)
            {
                codeblock.FontSize = this.FontSize;
            }

            foreach (var feedback in this.FeedbackBlocks)
            {
                feedback.FontSize = this.FontSize;
            }
        }

        private void OnDecreaseFont()
        {
            this.FontSize -= 0.5;

            foreach (var codeblock in this.CodeBlocks)
            {
                codeblock.FontSize = this.FontSize;
            }

            foreach (var feedback in this.FeedbackBlocks)
            {
                feedback.FontSize = this.FontSize;
            }
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
                    feedback.FontSize = this.FontSize;
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
