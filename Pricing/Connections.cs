using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Pricing
{
    class Connections
    {
        string oracledb = "DATA SOURCE = DBOLAP; USER ID = DWS1; Password=DWS1";
        public string cmd = null;
        public string querytask = null;
        public string readerData = null;
       
        public void databaseconnect()
        {
            using (OracleConnection oradb = new OracleConnection(oracledb))
            using (OracleCommand cmd = new OracleCommand(querytask,oradb))

                if (oradb.State == ConnectionState.Closed)
                {
                    oradb.Open();
                   
                }
                {

                }
                try
                {

                }
                catch (Exception)
                {

                    throw;
                }


                

        }

      
    }
}
