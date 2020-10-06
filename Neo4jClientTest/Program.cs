using System;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neo4jClient;

namespace Neo4jClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new GraphClient(new Uri("http://localhost:7474"), "neo4j", "123456");

            await client.ConnectAsync();

            var node = await client.Cypher
                .Match("(n:SQL)")
                .Return(n => n.As<Sql>())
                .ResultsAsync;

            var nodeList = node.ToList();

            Console.ReadLine();

            var newUser = new { Id = 456, Name = "Jim" };
            await client.Cypher
                .Create("(user:User {newUser})")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResultsAsync();

            Console.ReadLine();

        }
    }
}