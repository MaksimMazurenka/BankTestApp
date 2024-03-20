using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entity
{
    class Cashier : User
    {
        public static String DBConnectionString = "Data Source=DESKTOP-0H95GT0\\MSSQLSERVER02; Initial Catalog = Bank;" +
            " User ID = BankCashiers; Password=BankCashiersSecur1ty;Trusted_Connection=True;TrustServerCertificate=True;";
        public Cashier(string login, string password) :base(login, password)
        {
            this.login = login;
            this.password = password;
        }
        public Cashier()
        {
        }
        public float Convert(int ammount, float rate, int count)
        {
            return ammount * rate * count;
        }

        public static async Task<bool> setExchangeAsync(int userId, String CurrencyFrom, decimal AmountFrom, String CurrencyTo, decimal AmountTo, decimal ExchangeRate, int ExchangeScale, decimal Change)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "INSERT INTO ExchangeTransactions (userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change) VALUES (@userId, @CurrencyFrom, @AmountFrom, @CurrencyTo, @AmountTo, @ExchangeRate, @ExchangeScale, @Change)";
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@userId", SqlDbType.Int, 3).Value = userId;
                cmd.Parameters.Add("@CurrencyFrom", SqlDbType.NVarChar, 3).Value = CurrencyFrom;
                cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = AmountFrom;
                cmd.Parameters.Add("@CurrencyTo", SqlDbType.NVarChar,3).Value = CurrencyTo;
                cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = AmountTo;
                cmd.Parameters.Add("@ExchangeRate", SqlDbType.Decimal).Value = ExchangeRate;
                cmd.Parameters.Add("@ExchangeScale", SqlDbType.Int).Value = ExchangeScale;
                cmd.Parameters.Add("@Change", SqlDbType.Decimal).Value = Change;
                cmd.ExecuteNonQuery();
                DBController.connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DBController.connection.Close();
                return false;
            }
        }
    }
}
