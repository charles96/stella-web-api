namespace stella_web_api.Models
{
    public class StatisticsAlimtalkMonthly
    {
        public string send_month { get; set; }
        public int auto_send_count { get; set; }
        public int manual_send_count { get; set; }
        public int api_send_count { get; set; }
    }
}
