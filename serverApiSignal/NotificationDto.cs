namespace serverApiSignal
{
    public class NotificationDto
    {

        public string Title { get; set; }
        public string Message { get; set; }
        public string data { get; set; }
        public EventType type { get; set; }
    }


    public enum EventType
    {
        DataBaseUpdate,
        Notification
    }
}
