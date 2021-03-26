namespace stella_web_api.Models
{
    public class CompetitorResponse : BaseResponse
    {
        public Competitor data { get; set; } = new Competitor();
    }
}
