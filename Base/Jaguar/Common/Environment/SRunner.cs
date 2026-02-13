using System;
using Common.Data;
using Common.Helpers;
using FrontEnd.Lexing;
using FrontEnd.Parsing;
using BackEnd;
using System.Runtime.InteropServices;

namespace Common.Environment {
    public class SRunner {
        public static void RunEnvironment(string[] args) {
            
            JMemory memory = new JMemory("<S" + Runner.prompt + ">");
            
            string line = InitEnv();

            while (line != ":q") {
                if (line != "") {
                    DateTime inicio = DateTime.Now;
                    LexerTokens lr = Runner.RunLexer("\"terminal " + Runner.prompt + "\"", line);
                    if (lr.Error != null) {
                        Console.WriteLine(lr);
                    } else {
                        //Console.WriteLine(lr);
                        AstInfo pr = Runner.RunParser(lr);
                        if (pr.Error != null) {
                            Console.WriteLine(pr);
                        } else {
                            //Console.WriteLine(pr);
                            Interpreter interpreter = new Interpreter();
                            MemoryManager resultado = interpreter.Visit(pr.Node, memory);//var resultado = interpreter.Visit(pr.VAL);
                            if (resultado.Error != null) {
                                Console.WriteLine("Result: " + resultado.Error);
                            } else {
                                if (resultado.Value != null && resultado.Value.GetType() == typeof(TList)) {
                                    TList v = (TList)resultado.Value;
                                    string s = v.Shell();
                                    if (s.Length > 0) {
                                        Console.WriteLine(s);
                                    }
                                } else {
                                    Console.WriteLine(resultado);
                                }
                            }
                        }
                    }
                    TimeSpan duracao = DateTime.Now - inicio;
                    Console.WriteLine($"Time spent: {duracao.TotalMilliseconds} ms");
                }
                line = ReadLine();
            }
        }
        public static string InitEnv() {
            //Console.WriteLine("************************************************************************************");
            Console.WriteLine("                         Jaguar (SERIAL ENVIRONMENT)                                ");
            Console.WriteLine("   P.S: SERIAL ENVIRONMENT DON'T PROVIDE: '||' OR EMBEDDED FUNCTION 'allprint()'");
            Console.WriteLine(Util.DefaultMessage());
            return ReadLine();
        }
        public static string ReadLine() {
            /* DOC:
             * O propósito da linguagem é ser paralela.
             * Esta função é apenas para permitir testes Em ambiente serial.
             * Portanto, não queremos avaliar || Em ambiente serial.
            */
            string line = Util.ReplText("S"+Runner.prompt + ">").Trim();
            if (line != "") {
                string not_provide = "\"It isn't include on a serial version\"";
                if (line.ToCharArray()[0] == '|') return not_provide;
                if (line.Length >= 8 && line.Substring(0, 8) == "allprint") return not_provide;
            }
            return line;
        }
    }
}
