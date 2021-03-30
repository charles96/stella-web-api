namespace stella_web_api.Models
{
    public class StatisticAlimtalkMonthly
    {
        public int ManualSendCount { get; set; }
        public int AutoSendCount { get; set; }
        public int ApiSendCount { get; set; }
        public int TotalSendCount { get; set; }
        public float Gap { get; set; }
        public float Increase { get; set; }
    }
}
