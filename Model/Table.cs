using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Table
    {
        private int _tableID;
        private string _tableName;
        private int _tableBuyIn;
        private int _tablePlayerLimit;

        public Table(int tableID, string tableName, int tableBuyIn, int tablePlayerLimit) 
        {
            _tableID = tableID;
            _tableName = tableName;
            _tableBuyIn = tableBuyIn;
            _tablePlayerLimit = tablePlayerLimit;
        }

        public int TableID { get { return _tableID;} set { _tableID = value; } }
        public string TableName { get { return _tableName; } set { _tableName = value; } }
        public int TableBuyIn {  get { return _tableBuyIn; } set { _tableBuyIn = value; } }
        public int TablePlayerLimit {  get { return _tablePlayerLimit; } set { _tablePlayerLimit = value; } }
    }
}
