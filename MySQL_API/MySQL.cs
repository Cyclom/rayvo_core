using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQL_API
{
    public class MySQL
    {
        MySqlConnection conn;

      public void connectToMySQL()
        {
            string connString = "datasource=cyclom.ddns.net;port=3306;username=root;password=;database=fivem;";

            conn = new MySqlConnection(connString);
        }

    }
}
