using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entity
{
    class Admin : User
    {
        public static String DBConnectionString = "Data Source=DESKTOP-0H95GT0\\MSSQLSERVER02; Initial Catalog = Bank;" +
            " User ID = BankAdmin; Password=BankAdminSecur1ty;Trusted_Connection=True;TrustServerCertificate=True;";

        public Admin(String login, String password) : base(login, password)
        {
            this.login = login;
            this.password = password;
        }
  

        public static async Task<bool> setRateAsync(String currrency1, String currency2, decimal rate, int scale)
        {
            try
            {
            await DBController.Connect();
            string sqlExpression = "INSERT INTO ExchangeRates (CurrencyFrom, CurrencyTo, ExchangeRate, Scale) VALUES (@currrency1,@currency2,@rate,@scale)";
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@currrency1", SqlDbType.NVarChar, 3).Value = currrency1;
                cmd.Parameters.Add("@currency2", SqlDbType.NVarChar, 3).Value = currency2;
                cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = rate;
                cmd.Parameters.Add("@scale", SqlDbType.Int).Value = scale;
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

        public static async Task<bool> setUser(String currrency1, String currency2)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "INSERT INTO Users (Email, Password) VALUES (@currrency1,@currency2)";
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@currrency1", SqlDbType.NVarChar).Value = currrency1;
                cmd.Parameters.Add("@currency2", SqlDbType.NVarChar).Value = currency2;

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

        public static async Task<bool> setCurrencyAsync(String abr, String name)
        {
            Currency currency = await Currency.getCurrencieByCode(abr);
            if (currency != null && currency.currencyCode == null)
            {
                try
                {
                    await DBController.Connect();
                    string sqlExpression = "INSERT INTO Currencies (CurrencyCode, CurrencyName, Amount) VALUES (@currrency1, @currency2, @ammount)";
                    SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                    cmd.Parameters.Add("@currrency1", SqlDbType.NVarChar, 3).Value = abr;
                    cmd.Parameters.Add("@currency2", SqlDbType.NVarChar, 50).Value = name;
                    cmd.Parameters.Add("@ammount", SqlDbType.Int).Value = 0;
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
            return false;
        }


        public static async Task<bool> updateRateAsync(String currrency1, String currency2, decimal rate, int scale)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "Update ExchangeRates Set ExchangeRate = @rate, Scale = @scale Where CurrencyFrom = @currrency1 AND CurrencyTo = @currency2";
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = rate;
                cmd.Parameters.Add("@scale", SqlDbType.Int).Value = scale;
                cmd.Parameters.Add("@currrency1", SqlDbType.NVarChar, 3).Value = currrency1;
                cmd.Parameters.Add("@currency2", SqlDbType.NVarChar, 3).Value = currency2;
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

        public static async Task<bool> updateCurrencyAsync(String currrency1, int count)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "Update Currencies Set Amount = @count Where CurrencyCode = @currrency1";
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@count", SqlDbType.Decimal).Value = count;
                cmd.Parameters.Add("@currrency1", SqlDbType.NVarChar).Value = currrency1;
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

        public async static Task<List<User>> getAllUsers()
        {
            await DBController.Connect();
            string sqlExpression = "SELECT Id, Email, Password FROM Users";
            List<User> users = new List<User>();

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    User user = new User(dr.GetInt32(0), dr.GetString(1),dr.GetString(2));
                    users.Add(user);
                }
                dr.Close();
                DBController.connection.Close();
                return users;
            }
            else
            {
                users = new List<User>();
                dr.Close();
                DBController.connection.Close();
                return users;
            }
            dr.Close();
            DBController.connection.Close();
            return null;
        }
        //public async static Task<String> CashiersInfo(int id)
        //{
        //    await Connect();
        //    string sqlExpression = "SELECT (Resume) FROM Candidates " +
        //                           "WHERE UserId='" + id + "'";

        //    SqlCommand cmd = new SqlCommand(sqlExpression, connection);
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //            String empCode = dr.GetString(0);
        //            dr.Close();
        //            connection.Close();
        //            return empCode;
        //        }
        //    }
        //    else
        //    {
        //        dr.Close();
        //        connection.Close();
        //        return "No data found";
        //    }
        //    dr.Close();
        //    connection.Close();
        //    return "Ошибка";

        //}
    }
}
