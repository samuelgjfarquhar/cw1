using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace CW1
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new EnvironmentMas();
            var settings = new EnvironmentAgent();
            var rand = new Random();
            
            var envAgent = new EnvironmentAgent(); env.Add(envAgent, "env");
            var auctioneerAgent = new AuctioneerAgent(); env.Add(auctioneerAgent, "auctioneer");
            for (int i = 1; i <= EnvironmentAgent.NumberOfHouseholds; i++)
            {
                var hsAgent = new HouseholdAgent();
                env.Add(hsAgent, $"household{i:D2}");
            }
           
            env.Start();

            Console.ReadKey();

        }
    }
}
