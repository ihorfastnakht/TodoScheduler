namespace TodoScheduler.Models
{
    public enum TodoPriority
    {
        Low = 0,
        Normal,
        High
    }

    public enum TodoStatus
    {
        Failed = -1,
        InProcess = 0,
        Completed
    }
}
