using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TSMC_Employee_Query
{
    class MySqlCommand
    {
        private object Del;
        private object myConnection;

        public MySqlCommand(object Del, object myConnection)
        {
            // TODO: Complete member initialization
            this.Del = Del;
            this.myConnection = myConnection;
        }

        public object Connection { get; set; }

        internal void ExecuteNonQuery()
        {
           // throw new NotImplementedException();
        }
    }
}
