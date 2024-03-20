using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.Entity
{
    class nbrbAPI
    {
        public int cur_id { get; set; }
        public string date { get; set; }
        public string cur_abbreviation { get; set; }
        public int cur_scale { get; set; }
        public string cur_name { get; set; }
        public float cur_officialRate { get; set; }

        public static async Task<nbrbAPI> GetCatFactAsync(String name)
        {
            StringBuilder sb = new StringBuilder("https://api.nbrb.by/exrates/rates/");
            sb.Append(name);
            sb.Append("?parammode=2");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(sb.ToString());
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            nbrbAPI cure = JsonConvert.DeserializeObject<nbrbAPI>(responseBody);
            return cure;
        }

    }
}
