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

        private struct ElectricSellerBid
        {
            public string ElectricSellerBidder { get; set; }
            public int ElectricSellerBidValue { get; set; }

            public ElectricSellerBid(string bidder, int bidValue)
            {
                ElectricSellerBidder = bidder;
                ElectricSellerBidValue = bidValue;
            }
        }

        private struct ElectricBuyerBid
        {
            public string ElectricBuyerBidder { get; set; }
            public int ElectricBuyerBidValue { get; set; }

            public ElectricBuyerBid(string bidder, int bidValue)
            {
                ElectricBuyerBidder = bidder;
                ElectricBuyerBidValue = bidValue;
            }
        }

        private List<SellerBid> _sellerbids;
        private List<BuyerBid> _buyerbids;
        private List<Bid> _bids;
        private List<ElectricBuyerBid> _electricbuyerbids;
        private List<ElectricSellerBid> _electricsellerbids;

        private int _turnsToWait;

        public AuctioneerAgent()
        {
            _sellerbids = new List<SellerBid>();
            _buyerbids = new List<BuyerBid>();
            _bids = new List<Bid>();
            _electricbuyerbids = new List<ElectricBuyerBid>();
            _electricsellerbids = new List<ElectricSellerBid>();
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
                int param = Convert.ToInt32(parameters);

                switch (action)
                {
                    case "auction":
                        HandleBid(message.Sender, Convert.ToInt32(parameters));
                        break;
                    case "buying":
                        if (param < 0)
                        {
                            param = param * (-1);
                        }
                        HandleBuying(message.Sender, Convert.ToInt32(parameters));
                        Console.WriteLine($"\t\t[{message.Sender}]: buying {param} kw/h");
                        break;
                    case "selling":
                        HandleSelling(message.Sender, Convert.ToInt32(parameters));
                        Console.WriteLine($"\t\t[{message.Sender}]: selling {param} kw/h");
                        break;
                    case "buyElectric":
                        HandleBuyingElectric(message.Sender, Convert.ToInt32(parameters));
                        Console.WriteLine($"\t\t[{message.Sender}]: buying utility electric for {param} kw/h");
                        break;
                    case "sellElectric":
                        HandleSellingElectric(message.Sender, Convert.ToInt32(parameters));
                        Console.WriteLine($"\t\t[{message.Sender}]: selling utility electric for {param} kw/h");
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

        private void HandleBuyingElectric(string sender, int price)
        {
            _electricbuyerbids.Add(new ElectricBuyerBid(sender, price));

        }

        private void HandleSellingElectric(string sender, int price)
        {
            _electricsellerbids.Add(new ElectricSellerBid(sender, price));

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
            int[] electricsellerbidValues = new int[_electricsellerbids.Count];
            int[] electricbuyerbidValues = new int[_electricbuyerbids.Count];
            Console.WriteLine("\t\t\tsellers " + _sellerbids.Count);
            Console.WriteLine("\t\t\tbuyers " + _buyerbids.Count);
            Console.WriteLine("\t\t\tutil sellers " + _electricsellerbids.Count);
            Console.WriteLine("\t\t\tutil buyers " + _electricbuyerbids.Count);
            if (_sellerbids.Count > 0)
            {
                for (int i = 0; i < _sellerbids.Count; i++)
                {
                    int b = _sellerbids[i].SellerBidValue;
                    int sel = _electricsellerbids[i].ElectricSellerBidValue;
                    if (b > highestBid && b >= -10)
                    {
                        highestBid = b;
                        highestBidder = _sellerbids[i].SellerBidder;

                    }
                    sellerbidValues[i] = b;
                    electricsellerbidValues[i] = sel;
                }
            }

            if (_buyerbids.Count > 0)
            {
                for (int i = 0; i < _buyerbids.Count; i++)
                {
                    int buy = _buyerbids[i].BuyerBidValue;
                    int bee = _electricbuyerbids[i].ElectricBuyerBidValue;
                    buy = buy * -1;
                    if (buy > highestBidBuyer && buy <= 10)
                    {
                        highestBidBuyer = buy;
                        highestBidderBuyer = _buyerbids[i].BuyerBidder;

                    }
                    buyerbidValues[i] = buy;
                    electricbuyerbidValues[i] = bee;

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
                int kwhRemaining = kwhNeeded - kwhSell;


                if (kwhRemaining > 0)
                {
                    int utilKwhSell = electricsellerbidValues[0] * kwhRemaining;
                    Console.WriteLine($"[auctioneer]: Auction finished. {highestBidder} sold {kwhNeeded}kw/h to {highestBidderBuyer} who had {kwhSell}kw/h and sold the remianing {kwhRemaining} to utlity company for {utilKwhSell}p");// sell utility

                }
                if (kwhRemaining == 0)
                {
                    //remove agent
                    Console.WriteLine($"removed agent {highestBidder} and {highestBidderBuyer}");
                }
                if (kwhRemaining < 0)
                {
                    kwhRemaining = kwhRemaining * -1;

                    int utilKwhBuy = electricbuyerbidValues[0] * kwhRemaining;
                    Console.WriteLine($"[auctioneer]: Auction finished. {highestBidderBuyer} sold {kwhNeeded}kw/h to {highestBidder} who needed {kwhSell}kw/h and bought a remaining {kwhRemaining}kw/h from utlity company for {utilKwhBuy}p");// buy utility
                }
                //Console.WriteLine($"[auctioneer]: Auction finished. {highestBidder} sold {kwhSell}kw/h to {highestBidderBuyer} who needed {kwhNeeded}kw/h");

            }

            Stop();

        }

    }
}
