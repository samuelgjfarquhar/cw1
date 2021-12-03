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

        private struct BuyerBid
        {
            public string BuyerBidder { get; set; }
            public int BuyerBidValue { get; set; }

            public BuyerBid(string bidder, int bidValue)
            {
                BuyerBidder = bidder;
                BuyerBidValue = bidValue;
            }
        }

        private struct SellerBid
        {
            public string SellerBidder { get; set; }
            public int SellerBidValue { get; set; }

            public SellerBid(string bidder, int bidValue)
            {
                SellerBidder = bidder;
                SellerBidValue = bidValue;
            }
        }

        private List<SellerBid> _sellerbids;
        private List<BuyerBid> _buyerbids;
        private List<Bid> _bids;
        private int _turnsToWait;

        public AuctioneerAgent()
        {
            _sellerbids = new List<SellerBid>();
            _buyerbids = new List<BuyerBid>();
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
                //Console.WriteLine($"\t{message.Format()}");
                message.Parse(out string action, out string parameters);
                int kwh = Convert.ToInt32(parameters);
                
                switch (action)
                {
                    case "auction":
                        HandleBid(message.Sender, Convert.ToInt32(parameters));
                        break;
                    case "buying":
                        if (kwh < 0)
                        {
                            kwh = kwh * (-1);
                        }
                        HandleBuying(message.Sender, Convert.ToInt32(parameters));
                        Console.WriteLine($"[{message.Sender}]: buying {kwh}");
                        break;
                    case "selling":
                        HandleSelling(message.Sender, Convert.ToInt32(parameters));
                        Console.WriteLine($"[{message.Sender}]: selling {kwh}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
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

        private void HandleSelling(string sender, int price)
        {
            _sellerbids.Add(new SellerBid(sender, price));
            
        }

        private void HandleBuying(string sender, int price)
        {
            _buyerbids.Add(new BuyerBid(sender, price));
            
        }

        private void HandleFinish()
        {
            string highestBidder = "";
            string highestBidderBuyer = "";
            int highestBid = int.MinValue;
            int highestBidBuyer = int.MinValue;
            int[] bidValues = new int[_bids.Count];
            int[] sellerbidValues = new int[_sellerbids.Count];
            int[] buyerbidValues = new int[_buyerbids.Count];
            Console.WriteLine("sellers " + _sellerbids.Count);
            Console.WriteLine("buyers " + _buyerbids.Count);
            
            if(_sellerbids.Count > 0)
            {
                for (int i = 0; i < _sellerbids.Count; i++)
                {
                    int b = _sellerbids[i].SellerBidValue;
                    if (b > highestBid && b >= -10)
                    {
                        highestBid = b;
                        highestBidder = _sellerbids[i].SellerBidder;
                    }
                    sellerbidValues[i] = b;
                }
            }

            if(_buyerbids.Count > 0)
            {
                for(int i = 0; i < _buyerbids.Count; i++)
                {
                    int buy = _buyerbids[i].BuyerBidValue;
                    buy = buy * -1;
                    if (buy > highestBidBuyer && buy <= 10)
                    {
                        highestBidBuyer = buy;
                        highestBidderBuyer = _buyerbids[i].BuyerBidder;
                    }
                    buyerbidValues[i] = buy;

                }
                
            }

            if (highestBidder == "") // no bids above reserve price
            {
                Console.WriteLine("[auctioneer]: Auction finished. No winner.");
                Broadcast("winner none");
            }
            else
            {
                Array.Sort(sellerbidValues);
                Array.Reverse(sellerbidValues);
                Array.Sort(buyerbidValues);
                Array.Reverse(buyerbidValues);
                int kwhSell = sellerbidValues[0]; // first price
                int kwhNeeded = buyerbidValues[0];

                Console.WriteLine($"[auctioneer]: Auction finished. {kwhSell}kw/h sold to {highestBidderBuyer} buying {kwhNeeded}");
            }

            Stop();
           
        }
        
    }
}
