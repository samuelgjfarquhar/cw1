using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace CW1
{
    public class AuctioneerAgent : Agent
    {
        private struct Bid
        {
            public string Bidder { get; set; }
            public int BidValue { get; set; }

            public Bid(string bidder, int bidValue)
            {
                Bidder = bidder;
                BidValue = bidValue;
            }
        }

        private List<Bid> _bids;
        private int _turnsToWait;

        public AuctioneerAgent()
        {
            _bids = new List<Bid>();
        }

        public override void Setup()
        {
            Broadcast("start");
            _turnsToWait = 2;
        }

        public override void Act(Message message)
        {
            try
            {
                Console.WriteLine($"\t{message.Format()}");
                message.Parse(out string action, out string parameters);

                switch (action)
                {
                    case "bid":
                        HandleBid(message.Sender, Convert.ToInt32(parameters));
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

        public override void ActDefault()
        {
            if (--_turnsToWait <= 0)
                HandleFinish();
        }

        private void HandleBid(string sender, int price)
        {
            _bids.Add(new Bid(sender, price));
        }

        private void HandleFinish()
        {
            string highestBidder = "";
            int highestBid = int.MinValue;
            int[] bidValues = new int[_bids.Count];

            for (int i = 0; i < _bids.Count; i++)
            {
                int b = _bids[i].BidValue;
                if (b > highestBid && b >= EnvironmentAgent.MinDemand)
                {
                    highestBid = b;
                    highestBidder = _bids[i].Bidder;
                }
                bidValues[i] = b;
            }

            if (highestBidder == "") // no bids above reserve price
            {
                Console.WriteLine("[auctioneer]: Auction finished. No winner.");
                Broadcast("winner none");
            }
            else
            {
                Array.Sort(bidValues);
                Array.Reverse(bidValues);
                int winningPrice = bidValues[0]; // first price
                Console.WriteLine($"[auctioneer]: Auction finished. Sold to {highestBidder} for price {winningPrice}.");
            }
            Stop();
        }
    }
}
