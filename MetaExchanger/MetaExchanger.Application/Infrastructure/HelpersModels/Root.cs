namespace MetaExchanger.Application.Infrastructure.HelpersModels
{
    /// <summary>
    /// Class helper for deserialization procces during DB initialize. Contains bids that contains a order.
    /// </summary>
    public class Root
    {
        public DateTime AcqTime { get; set; }
        public List<Bid> Bids { get; set; }
    }
}