using System;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using stella_web_api.Models;

namespace stella_web_api.Repositories
{
    public class StellaRepository
    {

        public static async Task<int> ComplimentWriteAsync(ComplimentRequest complimentRequest)
        {
            int ret = 0;

            using (var conn = new MySqlConnection("Data Source=20.194.5.138;port=3306;User ID=charles;Password=96Hic121@@;Initial Catalog=stella;SslMode=None;"))
            {
                ret = await conn.ExecuteAsync
                    (@"
                        INSERT INTO 
                            tb_compliment 
                            (
                                from_luna_email,
                                to_luna_email,
                                compliment_contents, 
                                compliment_de
                            ) VALUES
                            (
                                @from_luna_email,
                                @to_luna_email,
                                @compliment_contents,
                                @compliment_de
                            )"
                , new
                {
                    from_luna_email = complimentRequest.from_luna_email,
                    to_luna_email = complimentRequest.to_luna_email,
                    compliment_contents = complimentRequest.compliment_contents,
                    compliment_de = DateTime.Now
                });
            }

            return ret;
        }

        public static async Task<ComplimentRequest> ComplimentReadAsync()
        {
            ComplimentRequest ret = null;

            using (var conn = new MySqlConnection("Data Source=20.194.5.138;port=3306;User ID=charles;Password=96Hic121@@;Initial Catalog=stella;SslMode=None;"))
            {
                ret = await conn.QueryFirstOrDefaultAsync<ComplimentRequest>("SELECT * FROM tb_compliment LIMIT 1");
            }

            return ret;
        }

        public static async Task<CompetitorResponse> CompetitorReadAsync()
        {
            CompetitorResponse ret = null;

            using (var conn = new MySqlConnection("Data Source=20.194.5.138;port=3306;User ID=charles;Password=96Hic121@@;Initial Catalog=stella;SslMode=None;"))
            {
                ret = await conn.QueryFirstOrDefaultAsync<CompetitorResponse>("SELECT * FROM tb_competitor ORDER BY reg_dt DESC LIMIT 1");
            }

            return ret;
        }

    }
}
