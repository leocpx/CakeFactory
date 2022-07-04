using DBManager.Tables;

namespace UnityCake.Events
{
    public class AskOrderParams
    {
        public Users worker { get; set; }
        public long startTime { get; set; }
        public FinishedGoodsInfo OrderRecipe { get; set; }
    }
}
