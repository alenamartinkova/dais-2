﻿using System;
namespace AspNetExampleApp.Database
{
    public class DbTable
    {
        protected Database mDatabase;
        protected String mTableName;

        public DbTable(Database database, String tableName)
        {
            mDatabase = database;
            mTableName = tableName;
        }      
    }
}
