using System;

namespace stella_web_api.Models
{
    public class CompetitorResponse : BaseResponse
    {
        public string luna_email { get; set; }
        public string content { get; set; }
        public DateTime reg_dt { get; set; }
    }
}
