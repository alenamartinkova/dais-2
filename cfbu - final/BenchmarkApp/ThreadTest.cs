using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using AspNetExampleApp.Database;
using BenchmarkApp.Database.Entities;
using ProjektORM.Databse;

namespace BenchmarkApp
{
    class ThreadTest
    {
        QuerySet mQuerySet;
        QuerySetPool mQuerySetPool;
        int mThreadId;

        public ThreadTest(int threadId, QuerySet querySet, QuerySetPool querySetPool)
        {
            mQuerySet = querySet;
            mThreadId = threadId;
            mQuerySetPool = querySetPool;
        }

        public void Run()
        {
            Console.Write("#" + mThreadId + " : Start \t");

            Random rnd = new Random(mThreadId);
            int i = 0;

            while (true)
            {
                int index, queryNumber;
                DbTable tableObject = mQuerySet.GetTableObject(out index); // get table object of a random query

                // check when the query be executed
                if (mQuerySetPool.CanIExecuteQuery(index, out queryNumber) == false)
                {
                    continue;
                }

                // use the instance to call the method
                // prepare parameters
                int count = mQuerySetPool.GetMethodParameters(index).Length;

                object[] parameters = new object[count];
                for (int j = 0; j < count; j++)
                {
                    MethodInfo m = mQuerySetPool.getMethod(index);

                    if (m.DeclaringType.Name.Equals("TicketTable"))
                    {
                        if (m.Name.Equals("GetTicketsForMatch"))
                        {
                            if (mQuerySetPool.GetMethodParameters(index)[j] == System.Type.GetType("System.Int32"))
                            {
                                parameters[j] = rnd.Next(QuerySetPool.TEAM_MATCH_MAX_T);
                            }
                        }
                    }

                    if (m.DeclaringType.Name.Equals("PlayerTransferHistoryTable"))
                    {
                        if (m.Name.Equals("GetPlayerTransferHistoriesForPlayer"))
                        {
                            if (mQuerySetPool.GetMethodParameters(index)[j] == System.Type.GetType("System.Int32"))
                            {
                                parameters[j] = rnd.Next(QuerySetPool.PLAYER_MAX_PTH_S);
                            }
                        }
                    }


                    if (m.DeclaringType.Name.Equals("StatisticTable"))
                    {
                        if (m.Name.Equals("UpdateS2"))
                        {
                            if (mQuerySetPool.GetMethodParameters(index)[j] == System.Type.GetType("System.Int32"))
                            {
                                parameters[j] = rnd.Next(QuerySetPool.MAX_S);
                            }
                        }

                        if (m.Name.Equals("SelectPlayerStats"))
                        {
                            if (mQuerySetPool.GetMethodParameters(index)[j] == System.Type.GetType("System.Int32"))
                            {
                                parameters[j] = rnd.Next(QuerySetPool.PLAYER_MAX_PTH_S);
                            }
                        }
                    }


                    if (m.DeclaringType.Name.Equals("PlayerTable"))
                    {
                        if (m.Name.Equals("UpdateP9"))
                        {
                            if (mQuerySetPool.GetMethodParameters(index)[j] == System.Type.GetType("System.Int32"))
                            {
                                parameters[j] = rnd.Next(QuerySetPool.TEAM_MAX_P);
                            }
                        }
                    }

                    if (m.DeclaringType.Name.Equals("PitchTable"))
                    {
                        if (m.Name.Equals("Insert"))
                        {
                            parameters[j] = Pitch.FakeData();
                            j = count;
                        }
                    }
                }

                mQuerySetPool.GetQueryMethod(index).Invoke(tableObject, parameters);

                // print only sometimes, for performance reason
                if (i++ % 10 == 0)
                {
                    Console.Write("#" + mThreadId + " (" + index + "): " + queryNumber + "\t");
                }

                if (mQuerySetPool.CanIFinish())
                {
                    break;
                }
            }
            Console.Write("#" + mThreadId + " : End \t");
        }
    }
}
