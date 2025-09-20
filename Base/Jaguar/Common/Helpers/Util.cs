using Common.Data;
using Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Common.Helpers {
    public class Util {
        public static bool Em(char? c, string str) {
    	    foreach (char x in str.ToCharArray()) {
			    if(x==c) return true;
		    }
    	    return false;
        }
        public static bool Em(string s, params string[] strs) {
    	    foreach (string x in strs) {
			    if(x.Equals(s)) return true;
		    }
    	    return false;
        }

        public static bool Em(string k, string v, params Tuple<string, string>[] tps) {
            foreach (Tuple<string, string> t in tps) {
                if (t.Item1.Equals(k) && t.Item2.Equals(v)) return true;
            }
            return false;
        }
        private static int ToNumber(char? n) {
            switch (n) {
                case null:
                    return 0;
                default:
                    return (int)n;
            }
        }
        public static bool DIGITS(char? n) {
            int i = Util.ToNumber(n);
            return (i > 47 && i < 58);
        }
        public static bool LETTERS(char? n) {
            int i = Util.ToNumber(n);
            return (i > 64 && i < 91) || (i > 96 && i < 123);
        }
        public static bool LETTERS_DIGITS(char? n) {
            int i = Util.ToNumber(n);
            return (i > 47 && i < 58) || (i > 64 && i < 91) || (i > 96 && i < 123);
        }
        public static bool LETTERS_DIGITS_UNDERSCORE(char? n) {
            int i = Util.ToNumber(n);
            return i==95 || ((i > 47 && i < 58) || (i > 64 && i < 91) || (i > 96 && i < 123));
        }
        public static bool LETTERS_DIGITS_DOT(char? n) {
            int i = Util.ToNumber(n);
            return i == 46 || (i > 47 && i < 58);
        }
        public static string StringsDataLocation(string code, JSource scIni, JSource scEnd) {
            string result = "";
            //Calculate indices
            int idxIni;
            try {
                idxIni = Math.Max(code.LastIndexOf('\n', scIni.Idx), 0);
            } catch {
                idxIni = 0;
            }
            int idxEnd = code.IndexOf('\n', idxIni + 1);

            if (idxEnd < 0) idxEnd = code.Length;

            //Generate each line;
            int lineCount = scEnd.Line - scIni.Line + 1;
            for (int i = 0; i < lineCount; i++) {
                //Calculate line columns;
                string line = code.Substring(idxIni, idxEnd - idxIni);
                int colIni = i == 0 ? scIni.Col : 0;

                int colEnd = i == lineCount - 1 ? scEnd.Col : line.Length - 1;

                //Append to result;
                result += line + '\n';
                string space = "", space2="";
                for (int k = 0; k < colIni; k++) 
                    space += ' ';
                for (int k = 0; k < (colEnd - colIni); k++) 
                    space2 += '^';
                result += space + space2;

                //Re-calculate indices
                idxIni = idxEnd;

                try {
                    idxEnd = code.IndexOf('\n', idxIni + 1);
                } catch {
                    if (idxEnd < 0) idxEnd = code.Length;
                }
            }
            return result.Replace("\t", "");
        }
        public static string ClassName(object o) {
            if (o == null) return "Object 'o' in ClassName Not found";
            string[] names = o.GetType().ToString().Split('.');
            if (names.Length > 0)
                return names[names.Length - 1];
            return "";
        }
        public static IList<MethodInfo> MethodNames(object o) {
            IList<MethodInfo> list = new List<MethodInfo>();
            IList<MethodInfo> methods = new List<MethodInfo>(o.GetType().GetMethods());
            for (int i = 0; i < methods.Count; i++) {
                MethodInfo m = methods.ElementAt(i);
                list.Add(m);
            }
            return list;
        }
        /* Reflexao */
        private static string SplitMethodName(string meth) { return meth.Split(' ', '(')[1]; }
        public static MethodInfo SelectMethod(object o, string start_name, string end_name) {
            var list_method = Util.MethodNames(o);
            foreach (var m in list_method) {
                if (Util.SplitMethodName(m.ToString()) == start_name + end_name) {
                    return m;
                }
            }
            return null;
        }
        public static TError NoVisitMethod(TValue node, JMemory memory, string ini, string end) {
            return new TRunTimeError(node.NOIni, node.NOEnd, "No "+ ini + end + " method defined", memory);
        }
        public static string DefaultMessage() {
            string msg = "********************************** Jaguar ******************************************\n";
            msg += ":> Jaguar is a programming language, target to parallel type system And methods.\n";
            msg += ":> Thank you for using, report errors, And contribute for.\n";
            msg += ":> If you need, send reports to computabilidade@gmail.com, with subject 'Jaguar-Lang'\n";
            msg += "************************************************************************************\n";
            return msg + HelpMsg;
        }
        private readonly static string grammar = System.IO.File.ReadAllText("Grammar.txt");
        private readonly static string helpMsgDefault = "************************************** Versão: V1.0.0.7 **************************************\n" +
                    "To start, take a look at the attached source file (fonte.c).\n" +
                    "After that, type in the terminal below expressions like:\n" +
                    "  include(\"fonte.c\")\n" +
                    "  def Add(a, b) -> a + b\n" +
                    "  Add(2, 3)^2\n" +
                    "  allprint(pid)\n" +
                    "  print(Size)\n" +
                    "  ||.sum(pid)\n" +
                    "  allprint(||.scatter([10,20,30,40]))\n" + HelpMsg;
        public static string HelpMsg { get { return "Type: (grammar, to print the grammar | ?, for examples And help | :q, to exit).\n"; } }
        public static string ReadText(string prompt) {
            Console.Write(prompt + " ");
            string s = Console.ReadLine();
            return s;
        }
        public static string ReplText(string prompt) {
            string s = Util.ReadText(prompt);
            if (s == "grammar") {
                Console.WriteLine(grammar);
                s = "";
            }
            if (s == "?") {
                Console.WriteLine(helpMsgDefault);
                s = "";
            }
            return s;
        }
        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
