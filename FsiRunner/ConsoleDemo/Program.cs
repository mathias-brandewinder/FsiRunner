namespace ConsoleDemo
{
    using System;
    using ClearLines.FsiRunner;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Beginning");

            var fsiPath = @"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
            var session = new FsiSession(fsiPath);

            session.Start();
            session.OutputReceived += OnOutputReceived;
            session.ErrorReceived += OnErrorReceived;

            var code = @"let x = 42";
            session.AddLine(code);
            session.Evaluate();

            var line1 = @"let f y = x + y";
            var line2 = @"let z =";
            var line3 = @"   [ 1; 2; 3]";
            var line4 = @"   |> List.map (fun e -> f e)";

            session.AddLine(line1);
            session.AddLine(line2);
            session.AddLine(line3);
            session.AddLine(line4);
            session.Evaluate();

            var error1 = "Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn";
            session.AddLine(error1);
            session.Evaluate();

            var code2 = @"let c = 123";
            session.AddLine(code2);
            session.Evaluate();

            Console.ReadLine();
        }

        static void OnOutputReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine("FSI has happy news:");
            Console.WriteLine(e.Data);
        }

        static void OnErrorReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine("FSI has bad news:");
            Console.WriteLine(e.Data);
        }
    }
}
