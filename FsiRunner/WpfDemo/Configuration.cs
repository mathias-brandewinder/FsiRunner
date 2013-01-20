// -----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WpfDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FsiControl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            this.FsiLocation = @"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
        }

        public string FsiLocation { get; set; }
    }
}
