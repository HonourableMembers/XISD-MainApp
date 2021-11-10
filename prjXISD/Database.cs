using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace prjXISD
{
    public class Database
    {
        public string connetionString = null;
        public SqlConnection cnn;

        public Database()
        {
            connetionString = "Data Source=PAUWLAPTOP\\SQLEXPRESS;Initial Catalog=DevHub;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
        }
    }
}
