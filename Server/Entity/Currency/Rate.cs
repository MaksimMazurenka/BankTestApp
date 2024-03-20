using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entity
{
    class Rate
    {
        public String CurrencyFrom { get; set; }
        public String CurrencyTo { get; set; }
        public Decimal ExchangeRate { get; set; }
        public int Scale { get; set; }
        public DateTime Date { get; set; }
        public Rate(String currencyFrom, String currencyTo, Decimal exchangeRate, int scale)
        {
            this.CurrencyFrom = currencyFrom;
            this.CurrencyTo = currencyTo;
            this.ExchangeRate = exchangeRate;
            this.Scale = scale;
        }

        public Rate(String currencyFrom, String currencyTo, Decimal exchangeRate, int scale, DateTime date)
        {
            this.CurrencyFrom = currencyFrom;
            this.CurrencyTo = currencyTo;
            this.ExchangeRate = exchangeRate;
            this.Scale = scale;
            this.Date = date;
        }

        public Rate(String currencyFrom, String currencyTo)
        {
            this.CurrencyFrom = currencyFrom;
            this.CurrencyTo = currencyTo;
        }

        public async static Task<Rate> getRate(String currency1, String currency2)
        {
            await DBController.Connect();
            string sqlExpression = "SELECT ExchangeRate, Scale FROM ExchangeRates as a WHERE TransactionDateTime = (SELECT MAX(TransactionDateTime) FROM ExchangeRates as b WHERE a.CurrencyFrom = b.CurrencyFrom AND a.CurrencyTo = b.CurrencyTo) AND CurrencyFrom = @currency1 AND  CurrencyTo = @currency2";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);

            cmd.Parameters.Add("@currency1", System.Data.SqlDbType.NVarChar,3);
            cmd.Parameters["@currency1"].Value = currency1;
            cmd.Parameters.Add("@currency2", System.Data.SqlDbType.NVarChar,3);
            cmd.Parameters["@currency2"].Value = currency2;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(currency1, currency2, dr.GetDecimal(0), dr.GetInt32(1));
                    dr.Close();
                    DBController.connection.Close();
                    return rate;
                }
            }
            else
            {
                Rate currency = new Rate(null,null);
                dr.Close();
                DBController.connection.Close();
                return currency;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRates()
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            //SELECT CurrencyFrom, CurrencyTo,ExchangeRate, TransactionDateTime  FROM ExchangeRates as a WHERE TransactionDateTime = (SELECT MAX(TransactionDateTime)    FROM ExchangeRates as b WHERE a.CurrencyFrom = b.CurrencyFrom AND a.CurrencyTo = b.CurrencyTo)
            string sqlExpression = "SELECT CurrencyFrom, CurrencyTo ,ExchangeRate, Scale FROM ExchangeRates as a WHERE TransactionDateTime = (SELECT MAX(TransactionDateTime)    FROM ExchangeRates as b WHERE a.CurrencyFrom = b.CurrencyFrom AND a.CurrencyTo = b.CurrencyTo)";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesWithDate()
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByDateCur1Cur2(DateTime date, string cur1, string cur2)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CurrencyFrom = @CurrencyFrom AND CurrencyTo=@CurrencyTo AND CONVERT(date,TransactionDateTime) <= @Date GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date);
            cmd.Parameters["@Date"].Value = date;
            cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyFrom"].Value = cur1;
            cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyTo"].Value = cur2;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByCur1Cur2(string cur1, string cur2)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CurrencyFrom = @CurrencyFrom AND CurrencyTo=@CurrencyTo  GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyFrom"].Value = cur1;
            cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyTo"].Value = cur2;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByDateCur1(DateTime date, string cur1)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CurrencyFrom = @CurrencyFrom AND CONVERT(date,TransactionDateTime) <= @Date GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date);
            cmd.Parameters["@Date"].Value = date;
            cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyFrom"].Value = cur1;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByDateCur2(DateTime date, string cur2)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CurrencyTo=@CurrencyTo AND CONVERT(date,TransactionDateTime) <= @Date GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date);
            cmd.Parameters["@Date"].Value = date;
            cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyTo"].Value = cur2;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByCur1(string cur1)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CurrencyFrom = @CurrencyFrom GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyFrom"].Value = cur1;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByCur2(string cur2)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CurrencyTo=@CurrencyTo GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@CurrencyTo"].Value = cur2;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<List<Rate>> getAllRatesByDate(DateTime date)
        {
            List<Rate> rates = new List<Rate>();
            await DBController.Connect();
            string sqlExpression = "SELECT c.CurrencyFrom, c.CurrencyTo, c.ExchangeRate, c.Scale, c.TransactionDateTime FROM ExchangeRates c INNER JOIN (SELECT CurrencyFrom, CurrencyTo, MAX(TransactionDateTime) AS max_date FROM ExchangeRates WHERE CONVERT(date,TransactionDateTime) <= @Date GROUP BY CurrencyFrom, CurrencyTo) max_dates ON c.CurrencyFrom = max_dates.CurrencyFrom AND c.CurrencyTo = max_dates.CurrencyTo AND c.TransactionDateTime = max_dates.max_date; ";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
            cmd.Parameters.Add("@Date", System.Data.SqlDbType.Date);
            cmd.Parameters["@Date"].Value = date;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Rate rate = new Rate(dr.GetString(0), dr.GetString(1), dr.GetDecimal(2), dr.GetInt32(3), dr.GetDateTime(4));
                    rates.Add(rate);
                }
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            else
            {
                rates = new List<Rate>();
                dr.Close();
                DBController.connection.Close();
                return rates;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }
    }  
}
