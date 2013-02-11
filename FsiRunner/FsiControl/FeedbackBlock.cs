namespace FsiControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GalaSoft.MvvmLight;

    public class FeedbackBlock : ViewModelBase
    {
        private double fontSize;

        public FeedbackBlock(string result)
        {
            this.Result = result;
        }

        public double FontSize
        {
            get { return this.fontSize; }
            set
            {
                this.fontSize = value;
                base.RaisePropertyChanged("FontSize");
            }
        }

        public string Result { get; private set; }
    }
}
