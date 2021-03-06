﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task<string> HelloWordBegin(string message)
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

            //提交變更
            await transaction.CommitAsync();

            await session.CloseAsync();

            return result[0];
        }

        //改法 2
        public async Task<string> HelloWordWrite(string message)
        {
            string sql = $"CREATE (n:Greeting{{message:'{message}'}}) RETURN n.message + ' from node : '+ id(n)";

            var session = _driver.AsyncSession();

            var greeting = await session.WriteTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);

                return await result.ToListAsync(r => r[0].As<string>());
            });

            await session.CloseAsync();

            return greeting[0];
        }


        public async Task<string> Read(string message)
        {
            string sql = $"MATCH (n:Greeting{{message:'{message}'}}) RETURN n.message + ' from node : '+ id(n)";

            var session = _driver.AsyncSession();

            var greeting = await session.ReadTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);

                var aa = await result.FetchAsync();

                return await result.ToListAsync(r => r[0].As<string>());
            });

            return greeting[0];
        }

        public async Task<IList<INode>> GetNode()
        {
            string sql = $"MATCH (n:SQL) RETURN n";

            var session = _driver.AsyncSession();

            var greeting = await session.ReadTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);

                return await result.ToListAsync(r => r[0].As<INode>());
            });

            return greeting;
        }

        public async Task<IList<IRelationship>> GetRelationship()
        {
            string sql = "MATCH (:SQL)-[r:have]->(:SQL) RETURN r";

            var session = _driver.AsyncSession();

            var greeting = await session.ReadTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);

                return await result.ToListAsync(r => r[0].As<IRelationship>());
            });

            return greeting;
        }

        public async Task<IList<IPath>> GetAll()
        {
            string sql = "MATCH p=(:SQL)-[:have]->(:SQL) RETURN p";

            var session = _driver.AsyncSession();

            var greeting = await session.ReadTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);

                return await result.ToListAsync(r => r[0].As<IPath>());
            });

            return greeting;
        }

        public async Task<IList<IRecord>> GetMix()
        {
            string sql = "MATCH (n:SQL)-[r:have]->(n1:SQL) RETURN n,r,n1";

            var session = _driver.AsyncSession();

            var greeting = await session.ReadTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(sql);

                return await result.ToListAsync();
            });

            foreach (var record in greeting)
            {
                var node = record["n"].As<INode>();
                var node1 = record["n1"].As<INode>();
                var relationship = record["r"].As<IRelationship>();
            }

            return greeting;
        }


        //繼承自 IDisposable
        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
