using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;


namespace LOMCoreServer
{
    public class LOMCoreServer : BaseScript
    {
        /** //DB CONNECTIONS
        private const String SERVER = "cyclom.ddns.net";
        private const String DATABASE = "fivem";
        private const String UID = "root";
        private const String PASSWORD = "";

        private static MySqlConnection dbConn;
            **/




        public static String MOTD = "Willkommen!";
        public static String prefix = "[RayvoGC // Server]";

        public LOMCoreServer()
        {
            EventHandlers.Add("playerConnecting", new Action<Player, string, CallbackDelegate>(OnPlayerConnecting));
            EventHandlers.Add("onPlayerDied", new Action<Player, string, Vector3, CallbackDelegate>(OnPlayerDied));
            EventHandlers.Add("baseevents:leftVehicle", new Action<Player, object, int, string>(OnPlayerLeftVehicle));
            EventHandlers.Add("baseevents:enteringVehicle", new Action<Player, object, int, string>(OnPlayerLeftVehicle));
        }

   
        /**public static void InitializeDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;

            String connString = builder.ToString();

            builder = null;

            Debug.WriteLine(connString);

            dbConn = new MySqlConnection(connString);
        }

        public static void InsertPlayer(Player player)
        {
            String query = string.Format($"INSERT INTO table_listnames (steamid, police_rank, police_status, staff) VALUES({player.Identifiers["steam"]}, 0, '10-7', 0)");

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();

            dbConn.Close();

        }**/



        private void OnPlayerConnecting([FromSource] Player player, string playerName, CallbackDelegate kickReason)
        {
            String _steamID = player.Identifiers["steam"];
            TriggerClientEvent(player, "checkMysql", _steamID);
            Debug.WriteLine($"{prefix} {player.Name} versucht mit folgender IP zu verbinden: {player.EndPoint.ToString()} (Steam: {player.Identifiers["steam"]})");
        }

        private void OnPlayerDied(Player player, string deathReason, Vector3 position, CallbackDelegate wtf)
        {
            Debug.WriteLine($"{prefix} -DEATH - {player} ist bei Position {position.ToString()} aufgrund von {deathReason} gestorben. wtf is {wtf}");
        }

        private void OnPlayerLeftVehicle([FromSource]Player player, object vehicle, int seat, string displayName)
        {
            TriggerClientEvent(player, "playerLeftVehicle");
        }

    }
}
