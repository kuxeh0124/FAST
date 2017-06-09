using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CoffeeBeanReporter
{
    public class DatabaseConnector
    {
        //Server IP Address
        private String serverIpAdd;

        //Server IP Port
        private String serverIpPort;

        //Database Schema Name
        private String schemaName;

        //Database Username
        private String schemaUser;

        //Database Password
        private String schemaPass;

        //Character Delimiter
        private String chrEqual = "=";
        private String chrComma = ";";

        private MySqlConnection conn;


        public DatabaseConnector()
        {
            this.serverIpAdd = "Server" + chrEqual + ConfigurationManager.AppSettings.Get("mysqlDbServer");
            this.serverIpPort = "Port" + chrEqual + ConfigurationManager.AppSettings.Get("mysqlDbPort");
            this.schemaName = "Database" + chrEqual + ConfigurationManager.AppSettings.Get("mysqlDbSchema");
            this.schemaUser = "UID" + chrEqual + ConfigurationManager.AppSettings.Get("mysqlDbUser");
            this.schemaPass = "Password" + chrEqual + ConfigurationManager.AppSettings.Get("mysqlDbPass");
        }

        /**
         * Gather parameters for the database 
         */
        public DatabaseConnector(String serverIpAdd, String serverIpPort,
            String schemaName, String schemaUser, String schemaPass)
        {
            this.serverIpAdd = "Server" + chrEqual + serverIpAdd;
            this.serverIpPort = "Port" + chrEqual + serverIpPort;
            this.schemaName = "Database" + chrEqual + schemaName;
            this.schemaUser = "UID" + chrEqual + schemaUser;
            this.schemaPass = "Password" + chrEqual + schemaPass;
        }

        /**
         * Database connectivity
         */
        public void connect()
        {
            conn = new MySqlConnection(serverIpAdd + chrComma +
                serverIpPort + chrComma +
                schemaName + chrComma +
                schemaUser + chrComma +
                schemaPass);
            Console.WriteLine("MySQL connection created..." + conn.ConnectionString);
        }

        /**
         * Opens the connectivty of the database
         */
        public void open()
        {
            try
            {
                connect();
                Console.WriteLine(conn.ConnectionString);
                conn.Open();
                Console.WriteLine("MySQL connection opened...");
            }
            catch (MySqlException connEx)
            {
                Console.WriteLine("Error encountered for mysql connection opened..." + connEx.ToString());
            }
        }

        /**
         * Close the connectivty of the database
         */
        public void close()
        {
            try
            {
                conn.Close();
                conn.Dispose();
                Console.WriteLine("MySQL connection closed...");
            }
            catch (MySqlException connEx)
            {
                Console.WriteLine("Error encountered for mysql connection closed..." + connEx.ToString());
            }
        }

        /**
         * Query for the database 
         */
        public List<object[]> query(String cmdText)
        {
            DatabaseDataReader read = new DatabaseDataReader();
            read.init();
            open();

            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = cmdText;

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //MessageBox.Show(reader.GetString(0));
                                read.addRowInf(reader.Read());
                            }
                        }
                    }
                    catch (Exception dataEx)
                    {
                        Console.WriteLine(dataEx.ToString());
                    }

                }
                catch (MySqlException connEx)
                {
                    Console.WriteLine(connEx.ToString());
                }


            }

            close();
            return read.getRowInfo();
        }

        /**
         * Insert the data
         */
        public void insert(String cmdText)
        {
            open();

            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = cmdText;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception dataEx)
                    {
                        Console.WriteLine(dataEx.ToString());
                    }

                }
                catch (MySqlException connEx)
                {
                    Console.WriteLine(connEx.ToString());
                }

            }

            close();
        }


        public void Log(string formatString, params object[] parameters)
        {
            Console.WriteLine(String.Format(formatString, parameters));
        }

        public void testResultsInsert(String filename, String testModule, String testCase, String stepNumber,
            String description, String results)
        {
            String cmdText = String.Format("insert into test_results values (0,'{0}','{1}','{2}','{3}','{4}','{5}',NOW())", filename,
                testModule, testCase, stepNumber, description, results);
            try
            {
                Console.WriteLine(cmdText);
                insert(cmdText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }

    /**
     *  This class will return you the data from the data reader
     */
    public class DatabaseDataReader
    {
        private List<object[]> rowData;

        public void init()
        {
            rowData = new List<object[]>();
        }

        public void addRowInf(params object[] columnData)
        {
            rowData.Add(columnData);
        }

        public List<object[]> getRowInfo()
        {
            return rowData;
        }
    }
}
