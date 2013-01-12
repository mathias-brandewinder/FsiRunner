namespace ClearLines.FsiControl
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using FsiRunner;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class CodeBlock : ViewModelBase
    {
        private readonly FsiSession session;
        private string code;
        private RelayCommand run;

        public CodeBlock(FsiSession session)
        {
            this.code = "";
            this.session = session;
            this.run = new RelayCommand(OnRun, CanRun);
        }

        public string Code
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

        public ICommand Run
        {
            get { return this.run; }
        }

        private void OnRun()
        {
            var lines = this.BreakLines(this.Code);
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

        private FsiSession Session
        {
            get { return this.session; }
        }

        private IEnumerable<string> BreakLines(string text)
        {
            return text.Split((char)10);
        }
    }
}