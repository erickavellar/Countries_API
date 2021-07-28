using Library.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class CurrencyDataService
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private DialogService dialogService;

        #region Currency Data Service
        public CurrencyDataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source =" + path);
                connection.Open();

                string sqlCommand = "create table if not exists currencies (code varchar(30), name varchar(30), " +
                    "symbol varchar(5), countryCode varchar(50), foreign key(countryCode) references countries(numericCode))";

                command = new SQLiteCommand(sqlCommand, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }
        #endregion

        #region Save Data Currency
        public void SaveData(List<Currency> currencies, string alpha3Code)
        {
            try
            {
                foreach (var currency in currencies)
                {
                    string sql = string.Format("insert into currencies (code, name, symbol, countryCode) " +
                        "values ('{0}', \"{1}\", '{2}', '{3}')", currency.code, currency.name, currency.symbol, alpha3Code);

                    command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }
        #endregion

        #region Get Data Currency
        public List<Currency> GetData()
        {
            List<Currency> currencies = new List<Currency>();

            try
            {
                string sql = "select code, name, symbol, countryCode from currencies";

                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    currencies.Add(new Currency
                    {
                        code = reader["code"].ToString(),
                        name = reader["name"].ToString(),
                        symbol = reader["symbol"].ToString(),
                        countryCode = reader["countryCode"].ToString(),
                    });
                }
                connection.Close();
                return currencies;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
                return null;
            }
        }
        #endregion

        #region Delete Data Currency
        public void DeleteData()
        {
            try
            {
                string sql = "delete from currencies";
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }
        #endregion

        public List<Currency> GetCurrenciesByCountryCode(string alpha3Code)
        {
            List<Currency> currencies = new List<Currency>();

            try
            {
                string sql = $"select code, name, symbol, countryCode from currencies where countryCode = '{alpha3Code}'";

                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    currencies.Add(new Currency
                    {
                        code = reader["code"].ToString(),
                        name = reader["name"].ToString(),
                        symbol = reader["symbol"].ToString(),
                        countryCode = reader["countryCode"].ToString(),
                    });
                }
                connection.Close();
                return currencies;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
                return null;
            }
        }
    }
}
