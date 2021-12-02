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

        public HouseholdAgent(int val)
        {
            _valuation = val;
        }

        public override void Setup()
        {
            Console.WriteLine($"[{Name}]: My valuation is {_valuation}");
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
                        break;

                    case "winner":
                        HandleWinner(parameters);
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
            Send("auctioneer", $"bid {_valuation}");
        }

        private void HandleWinner(string winner)
        {
            if (winner == Name)
                Console.WriteLine($"[{Name}]: I have won.");

            Stop();
        }
    }
}