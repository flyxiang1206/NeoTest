using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Neo4jTest
{
    public class Neo4jHandler : IDisposable
    {
        //Neo4j連線基礎
        private readonly IDriver _driver;

        public Neo4jHandler(string uri, string user, string password)
        {
            //建立基礎連線
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        //改法 1
        public async Task<string> HelloWord1(string message)
        {
            string sql = $"CREATE (n:Greeting{{message:'{message}'}}) RETURN n.message + ' from node : '+ id(n)";

            //建立交談
            var session = _driver.AsyncSession();

            //開始交談
            var transaction = await session.BeginTransactionAsync();

            //執行 cypher 語法
            var echo = await transaction.RunAsync(sql);

            //取得回傳
            var result = await echo.ToListAsync(r => r[0].As<string>());

            return result[0];
        }

        //改法 2
        public async Task<string> HelloWord2(string message)
        {
            string sql = $"CREATE (n:Greeting{{message:'{message}'}}) RETURN n.message + ' from node : '+ id(n)";

            var session = _driver.AsyncSession();

            var greeting = await session.WriteTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);
                return await result.ToListAsync(r => r[0].As<string>());
            });

            return greeting[0];
        }

        //繼承自 IDisposable
        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
