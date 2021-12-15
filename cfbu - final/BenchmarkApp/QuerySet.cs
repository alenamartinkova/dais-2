using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AspNetExampleApp.Database;

namespace AspNetExampleApp.Database
{
    class QuerySet
    {
        Collection<DbTable> mTableObjects;
        Database mDatabase;
        Random mRandom = new Random();

        public QuerySet(Database database)
        {
            mTableObjects = new Collection<DbTable>();
            mDatabase = database;
        }

        public void AddTableObject(DbTable tableObject)
        {
            mTableObjects.Add(tableObject);
        }

        public DbTable GetTableObject(out int index)
        {
            int num = mRandom.Next(mTableObjects.Count);
            index = num;
            return mTableObjects[num];
        }
    }
}
