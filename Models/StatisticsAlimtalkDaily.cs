namespace stella_web_api.Models
{
    public class StatisticsAlimtalkDaily
    {
        public string send_day { get; set; }
        public int auto_send_count { get; set; }
        public int manual_send_count { get; set; }
        public int api_send_count { get; set; }
    }
}
