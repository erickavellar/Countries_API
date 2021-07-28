using Library.Models;
using Library.Services;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Countries_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Country> _result;
        private readonly List<Language> _language;
        private DialogService dialogService;
        private readonly List<Covid> _result2;
        private readonly bool _load;

        public MainWindow(List<Country> result, List<Covid> result2, bool load)
        {
            InitializeComponent();

            dialogService = new DialogService();
            _result = result;
            _result2 = result2;
            _load = load;

            tbCountries.ItemsSource = result;
            tbCountries.SelectedItem = "Portugal";
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            foreach (var country in _result)
            {
                if (tbCountries.Text == country.name)
                {
                    lblCountry.Content = UnavailableInfo(country.name);
                    textName.Text = UnavailableInfo(country.name);
                    textCapital.Text = UnavailableInfo(country.capital);
                    textRegion.Text = UnavailableInfo(country.region);
                    textSubRegion.Text = UnavailableInfo(country.subregion);
                    textPopulation.Text = UnavailableInfo(country.population.ToString());
                    textArea.Text = UnavailableInfo(country.area);
                    textGini.Text = UnavailableInfo(country.gini);
                    textLanguage.Text = string.Empty;
                    textCurrency.Text = string.Empty;
                    
                    

                    if (_load != false)
                    {

                        if (country.languages != null || country.languages.Count != 0)
                        {
                            foreach (var language in country.languages)
                            {
                                textLanguage.Text += UnavailableInfo(language.name);
                            } 
                        }

                        if (country.currencies != null || country.currencies.Count != 0)
                        {
                            foreach (var currency in country.currencies)
                            {
                                textCurrency.Text += UnavailableInfo(currency.symbol);
                            } 
                        }

                        if (country.timezones != null || country.timezones.Count != 0)
                        {
                            lvUtc.ItemsSource = country.timezones;
                        }


                        var find = _result2.Find(x => x.Country == country.name);
                        if (find != null)
                        {
                            if (find.TotalCases != null)
                            {
                                textTotalCases.Text = UnavailableInfo(find.TotalCases.ToString());
                            }

                            if (find.TotalDeths != null)
                            {
                                textTotalDeaths.Text = UnavailableInfo(find.TotalDeths.ToString());
                            }

                            if (find.TotalRecovery != null)
                            {
                                textTotalRecovery.Text = UnavailableInfo(find.TotalRecovery.ToString());
                            }

                            if (find.TotalTests != null)
                            {
                                textTotalTests.Text = UnavailableInfo(find.TotalTests.ToString());
                            }
                        }

                        //-------------------------MAP VIEW------------------------------

                        Map.Mode = new AerialMode(true);
                        if (country.latlng.Count == 0 || country.latlng == null)
                        {
                            dialogService.ShowMessage("Error", "LOCATION NOT FOUND");
                            Map.Center = new Microsoft.Maps.MapControl.WPF.Location(0, 0);
                            Point.Location = new Microsoft.Maps.MapControl.WPF.Location(0, 0);
                            return;
                        }
                        else
                        {
                            Map.Center = new Microsoft.Maps.MapControl.WPF.Location(country.latlng[0], country.latlng[1]);
                            Point.Location = new Microsoft.Maps.MapControl.WPF.Location(country.latlng[0], country.latlng[1]);

                        }
                    }

                    //-------------------------FLAGS VIEW------------------------------

                    
                    string fileNameFlags = Environment.CurrentDirectory + "/Flags" + $"/{country.alpha3Code.ToLower()}.jpg";

                    
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    if (File.Exists(fileNameFlags))
                    {
                        img.UriSource = new Uri(fileNameFlags);
                    }
                    else
                    {
                        img.UriSource = new Uri(Environment.CurrentDirectory + "/ImageUnavailable.png");//mando carregar trazendo ela do meu diretorio
                                                                                                        //imageFlag.Stretch = Stretch.None;//depois mando identificar
                    }
                    img.EndInit();
                    imageFlag.Source = img;
                }
            }

        }

        private string UnavailableInfo(string info)
        {
            if (string.IsNullOrEmpty(info))
            {
                return "N/A";
            }
            else if (info == "0")
            {
                return "N/A";
            }
            else
            {
                return info;
            }
        }

        private void btnEngland_Click(object sender, RoutedEventArgs e)
        {
            btnSearch.Content = "Search";
            lblLanguage.Content = "Choose Language:";
            tbName.Text = "Name:";
            tbCapital.Text = "Capital:";
            tbRegion.Text = "Region:";
            tbSubRegion.Text = "Sub-Region:";
            tbPopulation.Text = "Population:";
            tbArea.Text = "Area:";
            tbGini.Text = "GINI:";
            tbLanguage.Text = "Language:";
            tbCurrency.Text = "Currency:";
            btnAbout.Content = "About";
            tbTotalCases.Text = "Total Cases:";
            tbTotalDeaths.Text = "Total Deaths:";
            tbTotalRecovery.Text = "Total Recovery:";
            tbTotalTests.Text = "Total Tests:";
            textCovid.Text = "Covid-19 Worldwide Cases";
        }

        private void btnSpanish_Click(object sender, RoutedEventArgs e)
        {
            btnSearch.Content = "Buscar";
            lblLanguage.Content = "Elige Lengua:";
            tbName.Text = "Nombre:";
            tbCapital.Text = "Capital:";
            tbRegion.Text = "Región:";
            tbSubRegion.Text = "Subregión:";
            tbPopulation.Text = "Población:";
            tbArea.Text = "Área:";
            tbGini.Text = "GINI:";
            tbLanguage.Text = "Lengua:";
            tbCurrency.Text = "Divisa:";
            btnAbout.Content = "Acerca de";
            tbTotalCases.Text = "Total de casos:";
            tbTotalDeaths.Text = "Muertes totales:";
            tbTotalRecovery.Text = "Recuperación total:";
            tbTotalTests.Text = "Pruebas totales:";
            textCovid.Text = "COVID-19 Casos Mundiales";
        }

        private void btnPortuguese_Click(object sender, RoutedEventArgs e)
        {
            btnSearch.Content = "Procurar";
            lblLanguage.Content = "Escolha o Idioma:";
            tbName.Text = "Nome:";
            tbCapital.Text = "Capital:";
            tbRegion.Text = "Região:";
            tbSubRegion.Text = "Sub-Região:";
            tbPopulation.Text = "População:";
            tbArea.Text = "Area:";
            tbGini.Text = "GINI:";
            tbLanguage.Text = "Idioma:";
            tbCurrency.Text = "Moeda:";
            btnAbout.Content = "Sobre";
            tbTotalCases.Text = "Total de Casos:";
            tbTotalDeaths.Text = "Total de Mortes:";
            tbTotalRecovery.Text = "Recuperação Total:";
            tbTotalTests.Text = "Total de Testes:";
            textCovid.Text = "Covid-19 Casos Mundiais";
        }

        private void btnFrance_Click(object sender, RoutedEventArgs e)
        {
            btnSearch.Content = "Rechercher";
            lblLanguage.Content = "Choisissez la langue:";
            tbName.Text = "Nom:";
            tbCapital.Text = "Capitale:";
            tbRegion.Text = "Région:";
            tbSubRegion.Text = "Sous-région:";
            tbPopulation.Text = "Population:";
            tbArea.Text = "Surface:";
            tbGini.Text = "GINI:";
            tbLanguage.Text = "Langue:";
            tbCurrency.Text = "Pièce de monnaie:";
            btnAbout.Content = "À propos";
            tbTotalCases.Text = "Nombre total de cas:";
            tbTotalDeaths.Text = "Nombre total de décès:";
            tbTotalRecovery.Text = "Récupération totale:";
            tbTotalTests.Text = "Tests Totaux:";
            textCovid.Text = "Covid-19 Cas Dans le Monde";
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow main = new AboutWindow();
            main.Show();
        }
    }
}
