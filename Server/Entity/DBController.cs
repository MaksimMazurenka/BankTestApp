using MongoDB.Driver.Core.Configuration;
using System;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Entity
{
    class DBController
    {
        public static SqlConnection connection;
        static DBController()
        {
            if(Login.user is Admin)
            {
                connection = new SqlConnection(Admin.DBConnectionString);
            }
            else
            {
                connection = new SqlConnection(Cashier.DBConnectionString);
            }
        }

        public static async Task Connect()
        {
            while (connection.State == ConnectionState.Connecting)
            {
                Thread.Sleep(500);
            }
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Подключение открыто");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static async Task Disconnect()
        {
            try
            {
                await connection.CloseAsync();
            }
            catch (Exception e)
            {

            }
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
                Console.WriteLine("Подключение закрыто...");
            }
        }

        public static async Task<Boolean> Execute(String expression)
        {
            await Connect();
            try
            {
                SqlCommand command = new SqlCommand(expression, connection);
                int number = await command.ExecuteNonQueryAsync();
                await Disconnect();
                return true;
            }
            catch (Exception e)
            {
                await Disconnect();
                return false;
            }
        }


        public async static Task<User> getUserAndRole(String currency1, String currency2)
        {
            await DBController.Connect();
            string sqlExpression = "SELECT Id, Email, Password FROM Users WHERE Email = @currency1 AND  Password = @currency2";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);

            cmd.Parameters.Add("@currency1", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@currency1"].Value = currency1;
            cmd.Parameters.Add("@currency2", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@currency2"].Value = currency2;

            SqlDataReader dr = cmd.ExecuteReader();
            User user = new User(null,null);
            int id = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Login.curUserId = dr.GetInt32(0);
                    id = dr.GetInt32(0);
                    user = new User(dr.GetString(1), dr.GetString(2));
                }
                dr.Close();
            }
            else
            {
                User currency = new User(null, null);
                dr.Close();
                DBController.connection.Close();
                return currency;
            }

            sqlExpression = "SELECT Id, UserId FROM Admins WHERE UserId = @currency1";
            cmd = new SqlCommand(sqlExpression, DBController.connection);

            cmd.Parameters.Add("@currency1", System.Data.SqlDbType.Int);
            cmd.Parameters["@currency1"].Value = id;

            dr = cmd.ExecuteReader();
            Admin admin;
            Cashier cashier;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    admin = new Admin(user.login, user.password);
                    dr.Close();
                    DBController.connection.Close();
                    return admin;
                }
            }
            else
            {
                cashier = new Cashier(user.login, user.password);
                dr.Close();
                DBController.connection.Close();
                return cashier;
            }

            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public async static Task<User> getUser(String currency1)
        {
            await DBController.Connect();
            string sqlExpression = "SELECT Id, Email, Password FROM Users WHERE Email = @currency1";

            SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);

            cmd.Parameters.Add("@currency1", System.Data.SqlDbType.NVarChar);
            cmd.Parameters["@currency1"].Value = currency1;

            SqlDataReader dr = cmd.ExecuteReader();
            User user = new User(null, null);
            int id = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    user = new User(dr.GetString(1), dr.GetString(2));
                }
                dr.Close();
                DBController.connection.Close();
                return user;
            }
            else
            {
                User currency = new User(null, null);
                dr.Close();
                DBController.connection.Close();
                return currency;
            }

            dr.Close();
            DBController.connection.Close();
            return null;
        }

        public static async Task<bool> writeLog(String log, int user)
        {
            try
            {
                await DBController.Connect();
                string sqlExpression = "INSERT INTO UserActionLogs (UserId, ActionDescription) VALUES (@UserId,@ActionDescription)";
                SqlCommand cmd = new SqlCommand(sqlExpression, DBController.connection);
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = user;
                cmd.Parameters.Add("@ActionDescription", SqlDbType.NVarChar).Value = log;
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
/*
Use Bank
CREATE TABLE Users
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL
);
CREATE TABLE Admins
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
CREATE TABLE Cashiers
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
CREATE TABLE Currencies
(
    CurrencyCode NVARCHAR(3) PRIMARY KEY,
    CurrencyName NVARCHAR(50) NOT NULL
);
CREATE TABLE ExchangeRates
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    CurrencyFrom NVARCHAR(3) NOT NULL,
    CurrencyTo NVARCHAR(3) NOT NULL,
    ExchangeRate DECIMAL(18, 6) NOT NULL,
	Scale INT NOT NULL,
    TransactionDateTime DATETIME,
    FOREIGN KEY (CurrencyFrom) REFERENCES Currencies(CurrencyCode),
    FOREIGN KEY (CurrencyTo) REFERENCES Currencies(CurrencyCode)
);
CREATE TABLE ExchangeTransactions
(
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CurrencyFrom NVARCHAR(3) NOT NULL,
    AmountFrom DECIMAL(18, 2) NOT NULL,
    CurrencyTo NVARCHAR(3) NOT NULL,
    AmountTo DECIMAL(18, 2) NOT NULL,
    ExchangeRate DECIMAL(18, 6) NOT NULL,
    Change DECIMAL(18, 2) NOT NULL,
    TransactionDateTime DATETIME,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (CurrencyFrom) REFERENCES Currencies(CurrencyCode),
    FOREIGN KEY (CurrencyTo) REFERENCES Currencies(CurrencyCode)
);

 */ 