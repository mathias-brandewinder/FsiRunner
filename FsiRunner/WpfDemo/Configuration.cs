namespace WpfDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FsiControl;

    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            this.FsiLocation = @"C:\Program Files (x86)\Microsoft SDKs\F#\3.0\Framework\v4.0\fsi.exe";
        }

        public string FsiLocation { get; set; }
    }
}
