using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entity
{
    public class User
    {
        public static String DBConnectionString = "Data Source=DESKTOP-0H95GT0\\MSSQLSERVER02; Initial Catalog = Bank;" +
            " User ID = BankCashiers; Password=BankCashiersSecur1ty;Trusted_Connection=True;TrustServerCertificate=True;";
        public int userId { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public User(string email, string password)
        {
            this.login = email;
            this.password = password;
        }
        public User(int id, string email, string password)
        {
            this.userId = id;
            this.login = email;
            this.password = password;
        }
        public User()
        {
        }
    }
}
