using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entity
{
    class ExchangeHistory
    {
        public int userId { get; set; }
        public String CurrencyFrom { get; set; }
        public decimal AmountFrom { get; set; }
        public String CurrencyTo { get; set; }
        public decimal AmountTo { get; set; }
        public decimal ExchangeRate { get; set; }
        public int ExchangeScale { get; set; }
        public decimal Change { get; set; }
        public DateTime date { get; set; }

        public ExchangeHistory()
        {

        }

        public ExchangeHistory(int userId, String CurrencyFrom, decimal AmountFrom, String CurrencyTo, decimal AmountTo, decimal ExchangeRate, int ExchangeScale, decimal Change, DateTime date)
        {
            this.userId = userId;
            this.CurrencyFrom = CurrencyFrom;
            this.AmountFrom = AmountFrom;
            this.CurrencyTo = CurrencyTo;
            this.AmountTo = AmountTo;
            this.ExchangeRate = ExchangeRate;
            this.ExchangeScale = ExchangeScale;
            this.Change = Change;
            this.date = date;
        }

        public async static Task<List<ExchangeHistory>> getTodayExchangeByUser(int userId)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE UserId = @UserId AND CAST(TransactionDateTime AS DATE) = CAST(GETDATE() AS DATE);";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0),dr.GetString(1),dr.GetDecimal(2),dr.GetString(3),dr.GetDecimal(4),dr.GetDecimal(5),dr.GetInt32(6),dr.GetDecimal(7),dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByUser(int userId)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE UserId = @UserId;";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDate(DateTime firstDate, DateTime secondDate)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByCur1(String cur1)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE CurrencyFrom=@CurrencyFrom";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByCur2(String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateAndUser(DateTime firstDate, DateTime secondDate, int userId)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateUserAndCurrency1(DateTime firstDate, DateTime secondDate, int userId, String cur1)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND UserId = @UserId AND CurrencyFrom=@CurrencyFrom";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateUserAndCurrency2(DateTime firstDate, DateTime secondDate, int userId, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND UserId = @UserId AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateUserAndCur1Cur2(DateTime firstDate, DateTime secondDate, int userId,String cur1, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND UserId = @UserId AND CurrencyFrom=@CurrencyFrom AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByUserAndCur1Cur2(int userId, String cur1, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE UserId = @UserId AND CurrencyFrom=@CurrencyFrom AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByUserAndCur1(int userId, String cur1)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE UserId = @UserId AND CurrencyFrom=@CurrencyFrom";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByUserAndCur2(int userId, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE UserId = @UserId AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.Int);
                cmd.Parameters["@UserId"].Value = userId;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByCur1Cur2(String cur1, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE CurrencyFrom=@CurrencyFrom AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateCur1(DateTime firstDate, DateTime secondDate, String cur1)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND CurrencyFrom=@CurrencyFrom";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateCur2(DateTime firstDate, DateTime secondDate, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getExchangeByDateCur1Cur2(DateTime firstDate, DateTime secondDate, String cur1, String cur2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions WHERE TransactionDateTime BETWEEN @FirstDate AND @SecondtDate AND CurrencyFrom=@CurrencyFrom AND CurrencyTo=@CurrencyTo";

                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@FirstDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@FirstDate"].Value = firstDate;
                cmd.Parameters.Add("@SecondtDate", System.Data.SqlDbType.Date);
                cmd.Parameters["@SecondtDate"].Value = secondDate;
                cmd.Parameters.Add("@CurrencyFrom", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyFrom"].Value = cur1;
                cmd.Parameters.Add("@CurrencyTo", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@CurrencyTo"].Value = cur2;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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

        public async static Task<List<ExchangeHistory>> getAllExchangeHistory()
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "SELECT userId, CurrencyFrom, AmountFrom, CurrencyTo, AmountTo, ExchangeRate, ExchangeScale, Change, TransactionDateTime FROM ExchangeTransactions;";
                await DBController.Connect();
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    List<ExchangeHistory> exchangeHistory = new List<ExchangeHistory>();
                    while (dr.Read())
                    {
                        exchangeHistory.Add(new ExchangeHistory(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3), dr.GetDecimal(4), dr.GetDecimal(5), dr.GetInt32(6), dr.GetDecimal(7), dr.GetDateTime(8)));
                    }
                    dr.Close();
                    DBController.connection.Close();
                    return exchangeHistory;
                }
                else
                {
                    dr.Close();
                    DBController.connection.Close();
                    return new List<ExchangeHistory>();
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