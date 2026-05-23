using System;
using FrontEnd.Lexing;
using FrontEnd.Parsing;
using Common.Environment;

namespace Compiler {
    public class AppMain {
        public static bool Par = true;
        public static void Main(string[] args) {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            if (Par)
                PRunner.RunEnvironment(args);
            else
                SRunner.RunEnvironment(args);
        }
    }
}
