using System.Collections.Generic;
using Common.Environment;
namespace Common.Data {
    public sealed class Consts {
        public readonly static string INT  	     = "INT",     FLOAT 	 = "FLOAT",      STRING     = "STRING",    
                                      PLUS       = "PLUS",    MINUS      = "MINUS",      MUL        = "MUL",       DIV        = "DIV",     
                                      ID         = "ID",      KEY        = "KEY",        POW        = "POW",       EQ         = "EQ",     
                                      LPAR       = "LPAR",    RPAR       = "RPAR",       LSQR       = "LSQR",      RSQR       = "RSQR",
                                      EE         = "EE",      NE         = "NE",         LT         = "LT",        
                                      GT         = "GT",      LTE        = "LTE",        GTE        = "GTE",       
                                      COMMA      = "COMMA",   ARROW      = "ARROW",      NEWLINE    = "NEWLINE",   EOF		 = "EOF",
                                      DOT        = ".";

        private readonly static string[] _KEYS = new string[IDX.SIZE_KEY];
        public static string[] KEYS { get { return _KEYS;  } }

        static Consts() {
            KEYS[IDX.LET]           = "LET".ToLower();
            KEYS[IDX.AND]           = "AND".ToLower();
            KEYS[IDX.OR]            = "OR".ToLower();
            KEYS[IDX.NOT]           = "NOT".ToLower();
            KEYS[IDX.IF]            = "IF".ToLower();
            KEYS[IDX.ELIF]          = "ELIF".ToLower();
            KEYS[IDX.ELSE]          = "ELSE".ToLower();
            KEYS[IDX.FOR]           = "FOR".ToLower();
            KEYS[IDX.TO]            = "TO".ToLower();
            KEYS[IDX.STEP]          = "STEP".ToLower();
            KEYS[IDX.WHILE]         = "WHILE".ToLower();
            KEYS[IDX.DEF]           = "DEF".ToLower();
            KEYS[IDX.FUN]           = "FUN".ToLower();
            KEYS[IDX.DO]            = "DO".ToLower();
            KEYS[IDX.END]           = "END".ToLower();
            KEYS[IDX.RETURN]        = "RETURN".ToLower();
            KEYS[IDX.CONTINUE]      = "CONTINUE".ToLower();
            KEYS[IDX.BREAK]         = "BREAK".ToLower();
            KEYS[IDX.PFUN]          = "||";
        }

        public static class IDX { // Reserved Identifier
            public static int SIZE_KEY = 0; //18;
            public readonly static int
                // Reserved Native Words
                LET                 = IDX.SIZE_KEY++,
                AND                 = IDX.SIZE_KEY++,
                OR                  = IDX.SIZE_KEY++,
                NOT                 = IDX.SIZE_KEY++,
                IF                  = IDX.SIZE_KEY++,
                DO                  = IDX.SIZE_KEY++,
                ELIF                = IDX.SIZE_KEY++,
                ELSE                = IDX.SIZE_KEY++,
                FOR                 = IDX.SIZE_KEY++,
                TO                  = IDX.SIZE_KEY++,
                STEP                = IDX.SIZE_KEY++,
                WHILE               = IDX.SIZE_KEY++,
                DEF                 = IDX.SIZE_KEY++,
                FUN                 = IDX.SIZE_KEY++,
                END                 = IDX.SIZE_KEY++,
                RETURN              = IDX.SIZE_KEY++,
                CONTINUE            = IDX.SIZE_KEY++,
                BREAK               = IDX.SIZE_KEY++,
                PFUN                = IDX.SIZE_KEY++;
        }
        //public static TNull Null { get { return TNull.Get(); } } // Verificar se eh interessante trocar (TNumber Null por TNull Null)?
        public abstract class Number {
            // Statics objects
            private readonly static TNumber _null   = new TNumber((float?)null);        public static TNumber Null  { get { return _null;   } }
            private readonly static TNumber _false  = new TNumber(0);                   public static TNumber False { get { return _false;  } }
            private readonly static TNumber _true   = new TNumber(1);                   public static TNumber True  { get { return _true;   } }
            private readonly static TNumber _pi     = new TNumber(3.1415926535897931f); public static TNumber PI    { get { return _pi;     } }
            private readonly static TNumber _rank   = new TNumber(MPIEnv.Rank);         public static TNumber Rank  { get { return _rank;   } }
            private readonly static TNumber _size   = new TNumber(MPIEnv.Size);         public static TNumber Size  { get { return _size;   } }
        }
        public abstract class EmbeddedFunction {
            public static readonly string INI = "Run_";
            // TODO: Evaluate False, True, Null on Keywords
            public static readonly string FALSE            = "FALSE".ToLower();
            public static readonly string TRUE             = "TRUE".ToLower();
            public static readonly string NULL             = "NULL".ToLower();

            // EMBEDDED Native Functions And Symbols
            public static readonly string PI               = "PI".ToLower();
            public static readonly string PRINT            = "PRINT".ToLower();
            public static readonly string ALLPRINT         = "ALLPRINT".ToLower();
            public static readonly string STR              = "STR".ToLower();
            public static readonly string INPUT            = "INPUT".ToLower();
            public static readonly string INPUT_INT        = "INPUT_INT".ToLower();
            public static readonly string CLEAR            = "CLEAR".ToLower();
            public static readonly string IS_NUMBER        = "IS_NUMBER".ToLower();
            public static readonly string IS_STRING        = "IS_STRING".ToLower();
            public static readonly string IS_LIST          = "IS_LIST".ToLower();
            public static readonly string IS_FUNCTION      = "IS_FUNCTION".ToLower();
            public static readonly string PUSH             = "PUSH".ToLower();
            public static readonly string POP              = "POP".ToLower();
            public static readonly string CONCAT_LIST      = "CONCAT_LIST".ToLower();
            public static readonly string LEN              = "LEN".ToLower();
            public static readonly string INCLUDE          = "INCLUDE".ToLower();
            public static readonly string MPI_RANK         = "PID".ToLower();
            public static readonly string MPI_SIZE         = "SIZE".ToLower();
            public static readonly string MPI_SUM          = "MPI_SUM".ToLower();
            //public static readonly string CLS            = "CLS".ToLower();

            // Statics objects
            public static readonly TEmbeddedFunction E_PRINT        = new TEmbeddedFunction(Consts.EmbeddedFunction.PRINT);
            public static readonly TEmbeddedFunction E_ALLPRINT     = new TEmbeddedFunction(Consts.EmbeddedFunction.ALLPRINT);
            public static readonly TEmbeddedFunction E_INCLUDE      = new TEmbeddedFunction(Consts.EmbeddedFunction.INCLUDE);
            public static readonly TEmbeddedFunction E_MPI_SUM      = new TEmbeddedFunction(Consts.EmbeddedFunction.MPI_SUM);
            public static readonly TEmbeddedFunction E_STR          = new TEmbeddedFunction(Consts.EmbeddedFunction.STR);
            public static readonly TEmbeddedFunction E_INPUT        = new TEmbeddedFunction(Consts.EmbeddedFunction.INPUT);
            public static readonly TEmbeddedFunction E_INPUT_INT    = new TEmbeddedFunction(Consts.EmbeddedFunction.INPUT_INT);
            public static readonly TEmbeddedFunction E_CLEAR        = new TEmbeddedFunction(Consts.EmbeddedFunction.CLEAR);
            public static readonly TEmbeddedFunction E_IS_NUMBER    = new TEmbeddedFunction(Consts.EmbeddedFunction.IS_NUMBER);
            public static readonly TEmbeddedFunction E_IS_STRING    = new TEmbeddedFunction(Consts.EmbeddedFunction.IS_STRING);
            public static readonly TEmbeddedFunction E_IS_LIST      = new TEmbeddedFunction(Consts.EmbeddedFunction.IS_LIST);
            public static readonly TEmbeddedFunction E_IS_FUNCTION  = new TEmbeddedFunction(Consts.EmbeddedFunction.IS_FUNCTION);
            public static readonly TEmbeddedFunction E_PUSH         = new TEmbeddedFunction(Consts.EmbeddedFunction.PUSH);
            public static readonly TEmbeddedFunction E_POP          = new TEmbeddedFunction(Consts.EmbeddedFunction.POP);
            public static readonly TEmbeddedFunction E_LEN          = new TEmbeddedFunction(Consts.EmbeddedFunction.LEN);
            //public static readonly TEmbeddedFunction E_CONCAT_LIST= new TEmbeddedFunction(Consts.EmbeddedFunction.CONCAT_LIST);
        }
        public abstract class ArgNames {// TODO: Sempre verificar o Max, se 16 key/values datas in _values
            private static readonly IDictionary<string, string[]> _values = new Dictionary<string, string[]>(16);
            private static readonly KeysEmbed _keys = new KeysEmbed();
            private static IDictionary<string, string[]> Values { get { return _values; } }
            /*
             * Essas chaves (Keys) devem ser verificadas em EmbeddedFunction
             * De fato, Keys aqui são chaves genéricas acessíveis em memória filha (JMemory), retornando o valor (parâmetro).
             */
            public static KeysEmbed Keys { get { return _keys; } }
            public static string[] Get(string key) { return Values[key]; }
            static ArgNames() { // Isto aqui diz quantos argumentos uma função embarcada possui.
                Values[Consts.EmbeddedFunction.INPUT]           = new string[0]; // OBS: vetor vazio
                Values[Consts.EmbeddedFunction.INPUT_INT]       = new string[0]; // OBS: vetor vazio
                Values[Consts.EmbeddedFunction.CLEAR]           = new string[0]; // OBS: vetor vazio
                Values[Consts.EmbeddedFunction.PRINT]           = new string[1] { Keys.KValue }; // Exemplo: print("oi")
                Values[Consts.EmbeddedFunction.ALLPRINT]        = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.MPI_SUM]         = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.STR]             = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.IS_NUMBER]       = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.IS_STRING]       = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.IS_LIST]         = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.IS_FUNCTION]     = new string[1] { Keys.KValue };
                Values[Consts.EmbeddedFunction.LEN]             = new string[1] { Keys.KList };
                Values[Consts.EmbeddedFunction.PUSH]            = new string[2] { Keys.KList, Keys.KValue };
                Values[Consts.EmbeddedFunction.POP]             = new string[2] { Keys.KList, Keys.KIndex };
                //Values[Consts.EmbeddedFunction.CONCAT_LIST]   = new string[2] { Keys.KListA, Keys.KListB }; // Exemplo: 2 listas. A linguagem já prevê isso: [1]+[2]==[1,2].
                Values[Consts.EmbeddedFunction.INCLUDE]         = new string[1] { Keys.KFileName };
            }
            public class KeysEmbed {
                public string KValue { get { return "Value"; } }
                public string KFileName { get { return "FileName"; } }
                public string KList { get { return "list"; } }
                public string KIndex { get { return "index"; } }
                public string KListA { get { return "listA"; } }
                public string KListB { get { return "listB"; } }
            }
        }
    }
}
