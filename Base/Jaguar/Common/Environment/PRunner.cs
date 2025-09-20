using System;
using Common.Data;
using Common.Helpers;
using FrontEnd.Lexing;
using FrontEnd.Parsing;
using BackEnd;
using MPI;

namespace Common.Environment {
    public class PRunner {
        public static void RunEnvironment(string[] args) {
            MPIEnv.mpi_start(args);
            JMemory memory = new JMemory("<" + Runner.prompt + ">");

            string line = InitEnv();
            while (line != ":q") {
                if (line != "") {
                    LexerTokens lr = Runner.RunLexer("\"terminal " + Runner.prompt + "\"", line);
                    if (lr.Error != null) {
                        if (MPIEnv.Rank == MPIEnv.Root)
                            Console.WriteLine(lr);
                    } else {
                        //Console.WriteLine(lr);
                        AstInfo pr = Runner.RunParser(lr);
                        if (pr.Error != null) {
                            if (MPIEnv.Rank == MPIEnv.Root)
                                Console.WriteLine(pr);
                        } else {
                            //Console.WriteLine(pr);
                            Interpreter interpreter = new Interpreter();
                            MemoryManager resultado = interpreter.Visit(pr.Node, memory);//var resultado = interpreter.Visit(pr.VAL);
                            if (resultado.Error != null) {
                                if (MPIEnv.Rank == MPIEnv.Root)
                                    Console.WriteLine("VAL(" + MPIEnv.Rank + "): " + resultado.Error);
                            } else {
                                if (resultado.Value != null && resultado.Value.GetType() == typeof(TList)) {
                                    TList v = (TList)resultado.Value;
                                    string s = v.Shell();
                                    if (s.Length > 0) {
                                        if (MPIEnv.Rank == MPIEnv.Root)
                                            Console.WriteLine(s);
                                    }
                                } else {
                                    if (MPIEnv.Rank == MPIEnv.Root)
                                        Console.WriteLine(resultado);
                                }
                            }
                        }
                    }
                }
                line = ReadBroadcast();
            }
            MPIEnv.mpi_stop();
        }
        public static string InitEnv() {
            if (MPIEnv.Rank == MPIEnv.Root) {
                Console.WriteLine(Util.DefaultMessage());
            }
            MPIEnv.Comm_world.Barrier();
            string valores = "\"ROOT pid: \"+(pid = " + MPIEnv.Rank + ")+\", Size: \"+(Size = " + MPIEnv.Size + ")";
            Console.WriteLine("Hello, my parallel pid is: " + MPIEnv.Rank);
            MPIEnv.Comm_world.Barrier();
            return ReadBroadcast();
        }
        public static string ReadBroadcast() {
            string valores = "";
            if (MPIEnv.Rank == MPIEnv.Root) {
                valores = Util.ReplText(Runner.prompt + ">").Trim();
            }
            MPIEnv.Comm_world.Broadcast<string>(ref valores, MPIEnv.Root);
            MPIEnv.Comm_world.Barrier();
            return valores;
        }
    }
}
