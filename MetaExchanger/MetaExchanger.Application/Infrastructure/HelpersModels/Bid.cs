namespace MetaExchanger.Application.Infrastructure.HelpersModels
{
    /// <summary>
    /// Class helper for deserialization procces during DB initialize. Contains order.
    /// </summary>
    public class Bid
    {        
        public Order Order { get; set; }
    }
}