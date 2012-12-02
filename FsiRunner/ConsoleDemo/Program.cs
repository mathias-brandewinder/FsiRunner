namespace ConsoleDemo
{
    using System;
    using System.Diagnostics;
    using ClearLines.FsiRunner;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Beginning");
            // This is the path to FSI.EXE on my machine, adjust accordingly
            var fsiPath = @"C:\Program Files (x86)\Microsoft F#\v4.0\fsi.exe";
            var session = new FsiSession(fsiPath);

            // start the session and hook the listeners
            session.Start();
            session.OutputReceived += OnOutputReceived;
            session.ErrorReceived += OnErrorReceived;

            // Send some trivial code to FSI and evaluate
            var code = @"let x = 42";
            session.AddLine(code);
            session.Evaluate();

            // Send a code block of 4 lines, using whitespace
            // note how x, which was declared previously,
            // is used in f as a closure, and still available.
            var line1 = @"let f y = x + y";
            var line2 = @"let z =";
            var line3 = @"   [ 1; 2; 3]";
            var line4 = @"   |> List.map (fun e -> f e)";

            session.AddLine(line1);
            session.AddLine(line2);
            session.AddLine(line3);
            session.AddLine(line4);
            session.Evaluate();

            // random "code" which is definitely not F#
            // nothing crashes but we don't get any output?
            var error1 = "Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn";
            session.AddLine(error1);
            session.Evaluate();

            // In spite of invoking Cthulhu before,
            // our session is still healthy and evaluates this
            var code2 = @"let c = 123";
            session.AddLine(code2);
            session.Evaluate();

            // wait for user to type [ENTER] to close
            Console.ReadLine();
        }

        private static void OnOutputReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("FSI has happy news:");
            Console.WriteLine(e.Data);
        }

        private static void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("FSI has bad news:");
            Console.WriteLine(e.Data);
        }
    }
}
