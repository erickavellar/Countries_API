
using Library.Models;
using Library.Services;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Countries_API
{

    public partial class LoadWindow : Window
    {
        #region Atributos        
        private NetworkService networkService;
        private List<Country> Countries;
        private ApiService apiService;
        private DialogService dialogService;
        private DataService dataService;
        private List<Covid> covidCases;
        #endregion

        public LoadWindow()
        {
            InitializeComponent();

            networkService = new NetworkService();
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            Countries = new List<Country>();


            LoadInfoAsync();
        }

        private void ReportProgress(object sender, ProgressReport e)
        {
            ProgressBarLoad.Value = e.PercentageComplete;
        }

        bool load;
        private async Task LoadInfoAsync()
        {
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;

            var connection = networkService.CheckConnection();


            if (!connection.IsSuccess)
            {
                lblStatus.Content = "Loading...";

                load = false;

                await LoadLocalCountries();
                lblStatus.Content = "Loading Complete! Only local data base info.";


            }
            else
            {
                dataService.DeleteData();
                lblStatus.Content = "Loading...";
                await LoadApiCountriesAsync();
                await GetFlagsAsync(progress);
                await LoadApiCovidAsync();
                load = true;
                lblStatus.Content = "Loading Complete";


            }

            if (Countries.Count == 0)
            {
                lblStatus.Content = "Not Connection to the Internet" + Environment.NewLine +
                    "and the information was not previously uploaded." + Environment.NewLine + "Try again later!!!";
                lblStatus.Content = "First initialization must have Internet Connection!";

                return;
            }

            if (load)
            {
                lblStatus.Content = "Saving...";
                await dataService.SaveData(Countries, progress);
                lblStatus.Content = "Saving Complete";
                lblStatus.Content = "Loaded Successfully" + Environment.NewLine + "          Online";

            }
            else
            {
                lblStatus.Content = "Loaded Successfully" + Environment.NewLine + "        Offline";

            }
            MainWindow main = new MainWindow(Countries, covidCases, load);
            main.Show();
            this.Close();
        }

        private async Task LoadLocalCountries()
        {
            Countries = await dataService.GetData();
        }


        #region COUNTRIES

        /// <summary>
        /// Redirects to the ApiService Class to make the API Call to the Countries Rest API asynchronously.
        /// </summary>
        /// <returns>Task</returns>
        private async Task LoadApiCountriesAsync()
        {
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;

            var response = await apiService.GetCountry("http://restcountries.eu", "/rest/v2/all", progress);

            Countries = (List<Country>)response.Result;
        }
        #endregion

        #region Covid        
        private async Task LoadApiCovidAsync()
        {
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;

            var response = await apiService.GetCovid("http://covidcase.somee.com", "/api/covid", progress);

            covidCases = (List<Covid>)response.Result;
        }
        #endregion

        #region FLAGS
        private async Task GetFlagsAsync(IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport();

            if (!Directory.Exists("Flags"))
            {
                Directory.CreateDirectory("Flags");
            }

            foreach (var country in Countries)
            {
                var fileNameSVG = Environment.CurrentDirectory + "/Flags" + $"/{country.alpha3Code.ToLower()}.svg";//Path to save the image as SVG
                var fileNameJPG = Environment.CurrentDirectory + "/Flags" + $"/{country.alpha3Code.ToLower()}.jpg";
                var pathBackup = Environment.CurrentDirectory + "/FlagsBackup" + $"/{country.alpha3Code.ToLower()}.jpg";
                FileInfo imageFile = new FileInfo(fileNameSVG);

                if (!File.Exists(fileNameJPG))
                {
                    try
                    {
                        //Save the image as SVG from the URL
                        string svgFileName = "https://restcountries.eu" + $"/data/{country.alpha3Code.ToLower()}.svg";

                        using (WebClient webClient = new WebClient())
                        {
                            await webClient.DownloadFileTaskAsync(svgFileName, fileNameSVG);
                        }


                        //Read SVG Document from file system
                        var svgDocument = SvgDocument.Open(fileNameSVG);

                        try
                        {
                            var bitmap = svgDocument.Draw(); //If the Bitmap it's unable to be created, it will go to catch

                            bitmap.Save(fileNameJPG, ImageFormat.Jpeg);

                            File.Delete(fileNameSVG);
                        }
                        catch
                        {
                            if (File.Exists(pathBackup))
                            {
                                imageFile = new FileInfo(pathBackup);
                                File.Delete(fileNameSVG);
                                imageFile.CopyTo(fileNameJPG);
                            }
                        }


                    }
                    catch
                    {
                        if (File.Exists(pathBackup))
                        {
                            imageFile = new FileInfo(pathBackup);
                            File.Delete(fileNameSVG);
                            imageFile.CopyTo(fileNameJPG);
                        }
                        continue;
                    }
                }
                report.SitesDownloaded.Add(country);
                report.PercentageComplete = report.SitesDownloaded.Count * 100 / Countries.Count;
                progress.Report(report);


            }


        }
        #endregion



    }
}