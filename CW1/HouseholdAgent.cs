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
        private int _valuation;
        

        public HouseholdAgent()
        {
           
        }

        public override void Setup()
        {
                          
        }

        public override void Act(Message message)
        {
            try
            {
                Console.WriteLine($"\t{message.Format()}");
                message.Parse(out string action, out string parameters);

                switch (action)
                {
                   case "start":
                        HandleStart();
                        string[] inform = parameters.Split(' ');
                        int energyDemand = Int32.Parse(inform[0]);
                        int energyGenerated = Int32.Parse(inform[1]);
                        int utilityBuy = Int32.Parse(inform[2]);
                        int utilitySell = Int32.Parse(inform[3]);
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
            Send("env", "start", "start");
            if (_valuation <= 0)
            {
                Console.WriteLine($"[{Name}]: im buying {_valuation} kw/h");
            }
            else
            {
                Console.WriteLine($"[{Name}]: im selling {_valuation} kw/h");
            }
            //Send("auctioneer", $"bid {_valuation}");
        }

        private void HandleWinner(string winner)
        {
            if (winner == Name)
                Console.WriteLine($"[{Name}]: I have won.");

            Stop();
        }
    }
}