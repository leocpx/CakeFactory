namespace Core.Interfaces
{
    public interface IOrder
    {
        long id { get; set; }
        long _finishedGoodId { get; set; }
        long _startTime { get; set; }
        long _workerId { get; set; }
        bool _completed { get; set; }
    }
}
