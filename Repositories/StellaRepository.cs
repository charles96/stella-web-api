using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using stella_web_api.Models;

namespace stella_web_api.Repositories
{
    public class StellaRepository
    {

        public async Task<IEnumerable<Compliment>> ComplimentAsync()
        {
            IEnumerable<Compliment> ret = null;

            using (var conn = new MySqlConnection("Data Source=20.194.5.138;port=3306;User ID=charles;Password=96Hic121@@;Initial Catalog=navertalk;SslMode=None;"))
            {
                ret = await conn.QueryAsync<Compliment>("Select * FROM tb_compliment");
            }

            return ret;
        }
    }
}
