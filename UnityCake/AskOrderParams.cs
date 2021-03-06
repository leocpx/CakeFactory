using DBManager.Tables;

namespace UnityCake.Events
{
    public class AskOrderParams
    {
        public Users worker { get; set; }
        public long startTime { get; set; }
        public FinishedGoodsInfo OrderRecipe { get; set; }
        public ProductionOrders ProductionOrder { get; set; }
        public PackagingOrders PackagingOrder { get; set; }
    }
}
