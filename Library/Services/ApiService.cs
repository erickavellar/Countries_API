using Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class ApiService
    {
        private DialogService dialogService;
        private List<Country> countries;
        private List<Covid> covidCases;

        public async Task<Response> GetCountry(string urlBase, string controller, IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport();
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                var response = await client.GetAsync(controller);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,

                    };
                }

                var country = JsonConvert.DeserializeObject<List<Country>>(result);
                report.SitesDownloaded = country;
                report.PercentageComplete = (report.SitesDownloaded.Count * 100) / country.Count;
                progress.Report(report);

                return new Response
                {
                    IsSuccess = true,
                    Result = country
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetCovid(string urlBase, string controller, IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport();
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                var response = await client.GetAsync(controller);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,

                    };
                }

                var covidCases = JsonConvert.DeserializeObject<List<Covid>>(result);
                report.SitesDownloaded2 = covidCases;
                report.PercentageComplete = (report.SitesDownloaded.Count * 100) / covidCases.Count;
                progress.Report(report);

                return new Response
                {
                    IsSuccess = true,
                    Result = covidCases
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
