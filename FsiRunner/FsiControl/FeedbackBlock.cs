namespace FsiControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GalaSoft.MvvmLight;

    public class FeedbackBlock : ViewModelBase
    {
        public FeedbackBlock(string result)
        {
            this.Result = result;
        }

        public string Result { get; private set; }
    }
}
