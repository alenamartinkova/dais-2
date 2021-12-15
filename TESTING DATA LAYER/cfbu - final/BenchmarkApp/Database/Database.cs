using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ProjektORM.Databse.SQLS;

namespace AspNetExampleApp.Database
{
    public class Database
    {
        public static string DbServer = @"dbsys.cs.vsb.cz\STUDENT";
        public static string DbUser = "mar0702";
        public static string DbPassword = "tq71ge9681Iw9JRA";
        // private static int TIMEOUT = 240;
        private SqlConnection mConnection;
        uint mConnectNumber = 0;
        bool mTransactionFlag = false;
        SqlTransaction mSqlTransaction;
        private String mLanguage = "en";
        //private static String CONNECTION_STRING = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";
        private static String CONNECTION_STRING = string.Format("Data Source={0};User={1};Password={2};Initial Catalog={1};MultipleActiveResultSets=true", DbServer, DbUser, DbPassword);

        public LeagueTable LeagueTable;
        public TeamMatchTable TeamMatchTable;
        public TeamTable TeamTable;
        public PitchTable PitchTable;
        public PlayerTable PlayerTable;
        public PlayerTransferHistoryTable PlayerTransferHistoryTable;
        public StatisticTable StatisticTable;
        public TicketTable TicketTable;

        public Database() 
        {
            mConnection = new SqlConnection();
            LeagueTable = new LeagueTable(this);
            TeamMatchTable = new TeamMatchTable(this);
            TeamTable = new TeamTable(this);
            PitchTable = new PitchTable(this);
            PlayerTable = new PlayerTable(this);
            PlayerTransferHistoryTable = new PlayerTransferHistoryTable(this);
            StatisticTable = new StatisticTable(this);
            TicketTable = new TicketTable(this);
        }

        /**
         * Connect.
         **/
        public bool Connect(String conString)
        {
            if (!mTransactionFlag)
            {
                mConnectNumber++;
            }
            if (mConnection.State != System.Data.ConnectionState.Open)
            {
                mConnection.ConnectionString = conString;
                mConnection.Open();
            }
            return true;
        }

        /**
         * Connect.
         **/
        public bool Connect()
        {
            bool ret = true;

            if (mConnection.State != System.Data.ConnectionState.Open)
            {
                ret = Connect(CONNECTION_STRING);
                // ret = Connect(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            }
            else if (!mTransactionFlag)
            {
                mConnectNumber++;
            }

            return ret;
        }

        /**
         * Close.
         **/
        public bool Close()
        {
            if (!mTransactionFlag)
            {
                if (mConnectNumber > 0)
                {
                    mConnectNumber--;
                }
            }

            if (mConnectNumber == 0)
            {
                mConnection.Close();
            }
            return true;
        }

        /**
         * Begin a transaction.
         **/
        public void BeginTransaction()
        {
            mSqlTransaction = mConnection.BeginTransaction(IsolationLevel.Serializable);
            mTransactionFlag = true;
        }

        /**
         * End a transaction.
         **/
        public void EndTransaction()
        {
            // command.Dispose()
            mSqlTransaction.Commit();
            mTransactionFlag = false;
            mConnection.Close();
            Close();
        }

        /**
         * If a transaction is failed call it.
         **/
        public void Rollback()
        {
            mSqlTransaction.Rollback();
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Close();
            }
            return rowNumber;
        }

        /**
         * Update a record encapulated in the command.
         **/
        public int Update(SqlCommand command)
        {
            // ...
            return 0;
        }

        /**
         * Insert a record encapulated in the command.
         **/
        public int Insert(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                //command.Prepare();
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Close();
            }
            return rowNumber;
        }

        /**
         * Create command.
         **/
        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, mConnection);

            if (mTransactionFlag)
            {
                command.Transaction = mSqlTransaction;
            }
            return command;
        }

        /**
         * Select encapulated in the command.
         **/
        public SqlDataReader Select(SqlCommand command)
        {
            command.Prepare();
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }

        /**
         * Delete encapulated in the command.
         **/
        public int Delete(SqlCommand command)
        {
            // ...
            return 0;
        }

        public String Language
        {
            get
            {
                return mLanguage;
            }
            set
            {
                mLanguage = value;
            }
        }

    }
}

