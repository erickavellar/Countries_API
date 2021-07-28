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
    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;

        private CurrencyDataService currencyDataService;

        private LanguageDataService languageDataService;

        private RegionalBlocDataService regionalBlocsDataService;

        private TranslationsDataService translationsDataService;

        private TimezoneDataService timezoneDataService;

        #region Create Data Service       
        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists countries( name varchar(50), " +
                    "alpha2Code varchar(2), " +
                    "alpha3Code varchar(3), " +
                    "capital varchar(50), " +
                    "region varchar(50), " +
                    "subregion varchar(50), " +
                    "population int, " +
                    "demonym varchar(30), " +
                    "area varchar(10), " +
                    "gini varchar(10), " +
                    "nativeName varchar(50), " +
                    "numericCode varchar(50), " +
                    "cioc varchar(50))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                dialogService.ShowMessage("Erro", e.Message);
            }
        }
        #endregion

        #region Save Data
        public async Task SaveData(List<Country> countries, IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport();
            await Task.Run(() =>
            {
                try
                {
                    foreach (var country in countries)
                    {
                        if (country.name != "Portugal" && country.name != "Espanha")
                        {
                            string sql = string.Format("insert or ignore into countries (name, alpha2Code, alpha3Code, capital," +
                                                " region, subregion, population, demonym, area, gini, nativeName, numericCode, cioc) " +
                                                "values (\"{0}\", '{1}', '{2}', \"{3}\", '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " +
                                                "\"{10}\", '{11}', '{12}')", country.name, country.alpha2Code, country.alpha3Code,
                                                country.capital, country.region, country.subregion, country.population, country.demonym,
                                                country.area, country.gini, country.nativeName, country.numericCode, country.cioc);

                            command = new SQLiteCommand(sql, connection);
                            command.ExecuteNonQuery();

                            //currencyDataService = new CurrencyDataService();
                            //currencyDataService.SaveData(country.currencies, country.alpha3Code);

                            //languageDataService = new LanguageDataService();
                            //languageDataService.SaveData(country.languages, country.alpha3Code);

                            //regionalBlocsDataService = new RegionalBlocDataService();
                            //regionalBlocsDataService.SaveData(country.regionalBlocs, country.alpha3Code);

                            //translationsDataService = new TranslationsDataService();
                            //translationsDataService.SaveData(country.translations, country.alpha3Code);

                            //timezoneDataService = new TimezoneDataService();
                            //timezoneDataService.SaveData(countries, country.alpha3Code);

                            report.SitesDownloaded.Add(country);
                            report.PercentageComplete = (report.SitesDownloaded.Count * 100) / countries.Count;
                            progress.Report(report); 
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {

                    dialogService.ShowMessage("Erro", e.Message);
                }
            });
            
        }
        #endregion

        #region Get Data
        public async Task<List<Country>> GetData()
        {
            List<Country> countries = new List<Country>();

            try
            {
                string sql = "select name, alpha2Code, alpha3Code, capital, region, subregion, population, " +
                    "demonym, area, gini, nativeName, numericCode, cioc from countries";

                command = new SQLiteCommand(sql, connection);

                //Lê cada registo
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    currencyDataService = new CurrencyDataService();
                    await Task.Run(() => countries.Add(new Country
                    {
                        name = reader["name"].ToString(),
                        alpha2Code = reader["alpha2Code"].ToString(),
                        alpha3Code = reader["alpha3Code"].ToString(),
                        capital = reader["capital"].ToString(),
                        region = reader["region"].ToString(),
                        subregion = reader["subregion"].ToString(),
                        population = Convert.ToInt32(reader["population"]),
                        demonym = reader["demonym"].ToString(),
                        area = reader["area"].ToString(),
                        gini = reader["gini"].ToString(),
                        nativeName = reader["nativeName"].ToString(),
                        numericCode = reader["numericCode"].ToString(),
                        cioc = reader["cioc"].ToString(),
                        currencies = currencyDataService.GetCurrenciesByCountryCode(reader["alpha3Code"].ToString()),
                    }));
                    
                    
                }

                connection.Close();

                return countries;
            }
            catch (Exception e)
            {

                dialogService.ShowMessage("Erro", e.Message);
                return null;
            }
        }
        #endregion

        #region Delete Data
        public void DeleteData()
        {
            try
            {
                string sql = "delete from Countries";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();

                //currencyDataService = new CurrencyDataService();
                //currencyDataService.DeleteData();
                //languageDataService = new LanguageDataService();
                //languageDataService.DeleteData();
                //regionalBlocsDataService = new RegionalBlocDataService();
                //regionalBlocsDataService.DeleteData();
                //translationsDataService = new TranslationsDataService();
                //translationsDataService.DeleteData();
                //timezoneDataService = new TimezoneDataService();
                //timezoneDataService.DeleteData();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
        #endregion


    }
}
