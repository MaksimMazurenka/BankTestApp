using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entity
{
    class Currency
    {
        public string currencyCode { get; set; }
        public string currencyName { get; set; }
        public decimal count { get; set; }

        public Currency(String currencyCode, String currencyName, decimal count)
        {
            this.currencyCode = currencyCode;
            this.currencyName = currencyName;
            this.count = count;
        }
        public Currency()
        {
        }

        public async static Task<List<Currency>> getCurrencies()
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT CurrencyCode, CurrencyName, Amount FROM Currencies";
                await DBController.Connect();

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    List<Currency> currencies = new List<Currency>();
                    while (dr.Read())
                    {
                        Currency currency = new Currency(dr.GetString(0), dr.GetString(1),dr.IsDBNull(2) ? 0 : dr.GetDecimal(2));

                        currencies.Add(currency);
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return currencies;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<Currency>();
                }
                dr.Close();
                DBController.connection.Close();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;

        }

        public async static Task<Currency> getCurrencieByCode(String code)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT CurrencyCode, CurrencyName, Amount FROM Currencies WHERE CurrencyCode = @code";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@code", System.Data.SqlDbType.NVarChar, 3);
                cmd.Parameters["@code"].Value = code;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    Currency currency = new Currency();
                    while (dr.Read())
                    {
                        currency = new Currency(dr.GetString(0), dr.GetString(1), dr.IsDBNull(2) ? 0 : dr.GetDecimal(2));

                    }
                    dr.Close();
                    DBController.connection.Close();
                    return currency;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new Currency();
                }
                dr.Close();
                DBController.connection.Close();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;

        }
    }
}
