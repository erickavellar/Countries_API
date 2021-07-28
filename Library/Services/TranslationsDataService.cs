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
    public class TranslationsDataService
    {
        private DialogService dialogService;
        private SQLiteConnection connection;
        private SQLiteCommand command;

        /// <summary>
        /// SQL Table creation for Translations
        /// (Constroctor)
        /// </summary>
        public TranslationsDataService()
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

                string sqlCommand = "create table if not exists translations(de char(2), es char(2), fr char(2), ja char(2)," +
                    " it char(2), br char(2), pt char(2), nl char(2), hr char(2), fa char(2), countryCode)";

                command = new SQLiteCommand(sqlCommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }

        /// <summary>
        /// Saving data of translations in SQL with insert command
        /// </summary>
        /// <param name="translations"></param>
        /// <param name="countryCode"></param>
        public void SaveData(Translations translations, string countryCode)
        {
            try
            {
                string sqlComand = string.Format("insert into translations(de, es, fr, ja, it, br, pt, nl, hr, fa, " +
                    "countryCode) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", 
                    translations.de, translations.es, translations.fr, translations.ja, translations.it, translations.br, 
                    translations.pt, translations.nl, translations.hr, translations.fa, countryCode);
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
                throw;
            }
        }


        //Verificar se esta bem feito
        /// <summary>
        /// Get some data from SQL table Translations
        /// </summary>
        /// <returns>language list</returns>
        public Translations GetTranslations()
        {
            Translations translations = new Translations();

            try
            {
                string sql = "select de, es, fr, ja, it, br, pt, nl, hr, fa, countryCode";

                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    translations = new Translations
                    {
                        de = (string)reader["de"],
                        es = (string)reader["es"],
                        fr = (string)reader["fr"],
                        ja = (string)reader["ja"],
                        it = (string)reader["it"],
                        br = (string)reader["br"],
                        pt = (string)reader["pt"],
                        nl = (string)reader["nl"],
                        hr = (string)reader["hr"],
                        fa = (string)reader["fa"],
                        countryCode = (string)reader["countryCode"],
                    };
                }
                connection.Close();
                return translations;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Delete data from SQL table Translations
        /// </summary>
        public void DeleteData()
        {
            try
            {
                string sql = "delete from translations";
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }
    }
}
