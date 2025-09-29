using System;

namespace Common.Data {
    //[Serializable()]
    public class JMemory {
        private readonly string memoryName;
        private JMemory _parent = null;
        private JSource _parentLocation = default;
        private JSymbolTable _symbolTable = new JSymbolTable();

        public string Name { get { return this.memoryName; } }

        public JMemory Parent { get { return _parent;  } set { _parent = value;  } }

        public JSource ParentLocation { get { return _parentLocation; } set { _parentLocation = value; } }

        public JSymbolTable SymbolTable { get { return _symbolTable; } set { _symbolTable = value; } }

        /* TODO:
            * Definiremos aqui algumas funções nativas (embarcadas) na linguagem;
            * Quais seriam importantes. Verificadores? Conversores? Paralelas? Etc...
            * O True/False não estão como palavra reservada. Talvez seria importante considerar.
        */
        public JMemory(string _name) {
            this.memoryName = _name;
            SymbolTable.Set(Consts.EmbeddedFunction.FALSE, Consts.Number.False);
            SymbolTable.Set(Consts.EmbeddedFunction.TRUE, Consts.Number.True);
            SymbolTable.Set(Consts.EmbeddedFunction.NULL, Consts.Number.Null);
            SymbolTable.Set(Consts.EmbeddedFunction.PI, Consts.Number.PI);
            SymbolTable.Set(Consts.EmbeddedFunction.MPI_RANK, Consts.Number.Rank);
            SymbolTable.Set(Consts.EmbeddedFunction.MPI_MANAGER, Consts.Number.Manager);
            SymbolTable.Set(Consts.EmbeddedFunction.MPI_SIZE, Consts.Number.Size);

            SymbolTable.Set(Consts.EmbeddedFunction.PRINT, Consts.EmbeddedFunction.E_PRINT);
            SymbolTable.Set(Consts.EmbeddedFunction.ALLPRINT, Consts.EmbeddedFunction.E_ALLPRINT);
            SymbolTable.Set(Consts.EmbeddedFunction.INCLUDE, Consts.EmbeddedFunction.E_INCLUDE);
            SymbolTable.Set(Consts.EmbeddedFunction.MPI_SUM, Consts.EmbeddedFunction.E_MPI_SUM);
            SymbolTable.Set(Consts.EmbeddedFunction.STR, Consts.EmbeddedFunction.E_STR);
            SymbolTable.Set(Consts.EmbeddedFunction.INPUT, Consts.EmbeddedFunction.E_INPUT);
            SymbolTable.Set(Consts.EmbeddedFunction.INPUT_INT, Consts.EmbeddedFunction.E_INPUT_INT);
            SymbolTable.Set(Consts.EmbeddedFunction.CLEAR, Consts.EmbeddedFunction.E_CLEAR);
            SymbolTable.Set(Consts.EmbeddedFunction.IS_NUMBER, Consts.EmbeddedFunction.E_IS_NUMBER);
            SymbolTable.Set(Consts.EmbeddedFunction.IS_STRING, Consts.EmbeddedFunction.E_IS_STRING);
            SymbolTable.Set(Consts.EmbeddedFunction.IS_LIST, Consts.EmbeddedFunction.E_IS_LIST);
            SymbolTable.Set(Consts.EmbeddedFunction.IS_FUNCTION, Consts.EmbeddedFunction.E_IS_FUNCTION);
            SymbolTable.Set(Consts.EmbeddedFunction.PUSH, Consts.EmbeddedFunction.E_PUSH);
            SymbolTable.Set(Consts.EmbeddedFunction.GET, Consts.EmbeddedFunction.E_GET);
            SymbolTable.Set(Consts.EmbeddedFunction.LEN, Consts.EmbeddedFunction.E_LEN);

            //SymbolTable.Set(Consts.EmbeddedFunction.CLS, Consts.EmbeddedFunction.E_CLEAR);
            //SymbolTable.Set(Consts.EmbeddedFunction.CONCAT_LIST, Consts.EmbeddedFunction.E_CONCAT_LIST);
        }
        public JMemory(string _name, JMemory __parent, JSource __parentLocation) : this(_name) {
            this.Parent = __parent;
            this.ParentLocation = __parentLocation;
        }
    }
}
