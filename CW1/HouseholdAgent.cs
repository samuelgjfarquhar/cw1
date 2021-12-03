using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace CW1
{
    public class HouseholdAgent : Agent
    {
        public HouseholdAgent()
        {
           
        }

        public override void Setup()
        {
            Send("env", "start");
        }

        public override void Act(Message message)
        {
            try
            {
                Console.WriteLine($"\t{message.Format()}");
                message.Parse(out string action, out string parameters);

                switch (action)
                {
                   case "inform":
                        string[] inform = parameters.Split(' ');
                        int energyDemand = Int32.Parse(inform[0]);                        
                        int energyGenerated = Int32.Parse(inform[1]);
                        int utilityBuy = Int32.Parse(inform[2]);
                        int utilitySell = Int32.Parse(inform[3]);
                        int energyRemaining = energyGenerated - energyDemand;
                        if (energyRemaining <= 0)
                        {
                            //Console.WriteLine($"[{Name}]: im buying {energyRemaining} kw/h");
                            Send("auctioneer", $"buying {energyRemaining}");
                        }
                        else
                        {
                            //Console.WriteLine($"[{Name}]: im selling {energyRemaining} kw/h");
                            Send("auctioneer", $"selling {energyRemaining}");
                        }
                        //Send("auctioneer", $"auction {energyRemaining}");
                        break;
                   default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleStart()
        {
            
        }

        private void HandleWinner(string winner)
        {
            if (winner == Name)
                Console.WriteLine($"[{Name}]: I have won.");

            Stop();
        }
    }
}