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

            for (int i = 1; i <= EnvironmentAgent.NumberOfHouseholds; i++)
            {
                int agentValuation = EnvironmentAgent.MinDemand + rand.Next(EnvironmentAgent.MaxDemand - EnvironmentAgent.MinDemand);
                var householdAgent = new HouseholdAgent(agentValuation);
                env.Add(householdAgent, $"bidder{i:D2}");
            }
            var auctioneerAgent = new AuctioneerAgent();
            env.Add(auctioneerAgent, "auctioneer");
            env.Start();

            Console.ReadKey();

        }
    }
}
