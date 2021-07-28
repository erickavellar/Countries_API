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
    public class TimezoneDataService
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private DialogService dialogService;

        public TimezoneDataService()
        {
            dialogService = new DialogService();
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path); //cria BD se ainda nao existir
                connection.Open();

                string sqlcommand = "create table if not exists timezone(Cod varchar(3)," +
                    "Info varchar(25))";

                command = new SQLiteCommand(sqlcommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }

        }

        public void SaveData(List<Country> countries, string alpha3code)
        {
            try
            {
                foreach (var item in countries)
                {
                    foreach (var times in item.timezones)
                    {
                        string sql = string.Format("insert into timezone (Cod, Info) " +
                       "values ('{0}', '{1}')",
                       alpha3code, times);

                        command = new SQLiteCommand(sql, connection);
                        command.ExecuteNonQuery();

                    }
                }

                connection.Close();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public List<Timezones> GetData()
        {
            List<Timezones> timezone = new List<Timezones>();

            try
            {
                string sql = "select Cod, Info from timezone";

                command = new SQLiteCommand(sql, connection);

                //le cada registo
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    timezone.Add(new Timezones
                    {
                        Cod = (string)reader["Cod"],
                        Info = (string)reader["Info"]
                    });
                }

                connection.Close();
                return timezone;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
                return null;
            }

        }

        public void DeleteData()
        {
            try
            {
                string sql = "Delete from timezone";
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
    }
}
