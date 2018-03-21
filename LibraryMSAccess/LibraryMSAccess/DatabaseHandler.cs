using System;
using System.Data;
using System.Data.OleDb;

namespace LibraryMSAccess
{
    public partial class DatabaseHandler
    {
        private string provider {get; } = @"Provider = Microsoft.ACE.OLEDB.12.0;Data Source = ";
        private string filePath {get; } = System.IO.Directory.GetCurrentDirectory();
        private string fileName {get; }

        private OleDbConnection conn;

        private OleDbCommand cmd;


        // overloaded constructor - only needs the file name
        public DatabaseHandler(string pFileName)
        {
            fileName = pFileName;
            SetUp();
        }

        // overloaded constructor - needs file path and file name
        public DatabaseHandler(string pFilePath, string pFileName)
        {
            filePath = pFilePath;
            fileName = pFileName;
            SetUp();
        }

        // overloaded constructor - take all three in
        public DatabaseHandler(string pProvider, string pFilePath, string pFileName)
        {
            provider = pProvider;
            filePath = pFilePath;
            fileName = pFileName;
            SetUp();
        }

        private void SetUp()
        {
            var connString = provider + filePath + fileName;
            conn = new OleDbConnection(connString);
        }


        // good for inserts updates and deletes 
        private void ExecuteNonQueryRawSQL(string pSQL)
        {
            cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = pSQL;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Exception: LibraryMSAccess.DatabaseHandler.ExecuteNonQueryRawSQL: query - " + 
                                        pSQL + " - : e.Message - " + e.Message);
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                    conn.Close();
            } 
        }




        private void ExecuteReaderRawSQL(ref OleDbDataReader pReader, string pSQL)
        {
            cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = pSQL;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                pReader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new Exception("Exception: LibraryMSAccess.DatabaseHandler.ExecuteReaderRawSQL: query - " +
                                        pSQL + " - : e.Message - " + e.Message);
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                    conn.Close();
            }
        }

        /*
       
        the command based query needs to be set up on as a parameterised OleDBCommand 
        and passed into the method will work with update, insert and delete
        set up will look something like


        INSERT
          
        var vCmd = new OleDbCommand();
        vCmd.CommandType = CommandType.Text;
        vCmd.CommandText = "INSERT INTO GAME ([pid], [player_choice], [computer_choice]) VALUES (?, ?, ?)";
        vCmd.Parameters.AddWithValue("@pid", pGame.getPlayer().getPid());
        vCmd.Parameters.AddWithValue("@player_choice", pGame.getPlayerChoice().ToString());
        vCmd.Parameters.AddWithValue("@computer_choice", pGame.getAiChoice().ToString());

        UPDATE

        var vCmd = new OleDbCommand();
        vCmd.CommandType = CommandType.Text;
        vCmd.CommandText = "UPDATE PLAYER SET [player_name] = ? WHERE [pid] = ?";
        vCmd.Parameters.AddWithValue("@player_name", pNewName);
        vCmd.Parameters.AddWithValue("@pid", pPid);

        DELETE

        var vCmd = new OleDbCommand();
        vCmd.CommandType = CommandType.Text;
        vCmd.CommandText = "DELETE FROM PLAYER WHERE ([pid]) = (?)";
        vCmd.Parameters.AddWithValue("@pid", pPid);

        */

        private void ExecuteNonQueryCommand(ref OleDbCommand pCmd)
        {
            try
            {        
                conn.Open();
                pCmd.Connection = conn;
                pCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Exception: LibraryMSAccess.DatabaseHandler.ExecuteNonQueryCommand: query - " +
                                        pCmd.CommandText + " - : e.Message - " + e.Message);
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                    conn.Close();
            }
        }

        /*
        the command based query needs to be set up on as a parameterised OleDBCommand 
        and passed into the method will work with update, insert and delete
        set up will look something like

         SELECT

         var vCmd = new OleDbCommand();
         vCmd.CommandType = CommandType.Text;
         vCmd.CommandText = "UPDATE PLAYER SET [player_name] = ? WHERE [pid] = ?";
         vCmd.Parameters.AddWithValue("@player_name", pNewName);
         vCmd.Parameters.AddWithValue("@pid", pPid);

        */

        private void ExecuteReaderCommand(ref OleDbDataReader pReader, ref OleDbCommand pCmd)
        {
            try
            {
                conn.Open();
                pCmd.Connection = conn;
                pReader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new Exception("Exception: LibraryMSAccess.DatabaseHandler.ExecuteReaderCommand: query - " +
                                pCmd.CommandText + " - : e.Message - " + e.Message);
            }
            finally
            {
                if (conn.State.Equals(ConnectionState.Open))
                    conn.Close();
            }
        }
    }
}
