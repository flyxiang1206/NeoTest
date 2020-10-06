using System;
using System.Threading.Tasks;

namespace Neo4jTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //基本宣告，這個應該不用解釋
            using (var greeter = new Neo4jHandler("bolt://localhost:7687", "neo4j", "123456"))
            {
                //var result = await greeter.HelloWordBegin("hello, world");

                //Console.WriteLine(result);

                //var result2 = await greeter.Read("hello, world");

                //Console.WriteLine(result2);

                var resultNode = await greeter.GetNode();
                var resultRelationship = await greeter.GetRelationship();
                var resultAll = await greeter.GetAll();
                var resultMix = await greeter.GetMix();

                Console.ReadLine();
            }
        }
    }
}

