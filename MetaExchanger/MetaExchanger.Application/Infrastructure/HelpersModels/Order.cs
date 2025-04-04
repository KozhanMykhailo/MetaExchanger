namespace MetaExchanger.Application.Infrastructure.HelpersModels
{
    /// <summary>
    /// Class helper for deserialization procces during DB initialize. Main entity
    /// </summary>
    public class Order
    {
        public object Id { get; set; }

        public DateTime Time { get; set; }

        public string Type { get; set; }

        public string Kind { get; set; }

        public decimal Amount { get; set; }

        public decimal Price { get; set; }
    }
}