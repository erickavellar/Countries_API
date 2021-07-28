using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_COVID.Controllers
{
    

    public class CovidController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();

        // GET: api/Covid
        public List<Covid_Case> Get()
        {
            var list = from Covid_Case in dc.Covid_Cases select Covid_Case;

            return list.ToList();
        }

        // GET: api/Covid/Country
        /// <summary>
        /// Get all the Countries
        /// </summary>
        /// <param name="country">Nome</param>
        /// <returns>List off all countries</returns>
        [Route("api/Covid/{country}")]
        public IHttpActionResult Get(string country)
        {
            var covid = dc.Covid_Cases.SingleOrDefault(c => c.Country == country); //Procurar se meu country foi introduzida no url por defoult

            if (covid != null)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, covid));
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Country not found"));
        }

        // POST: api/Covid
        /// <summary>
        /// Create a country
        /// </summary>
        /// <param name="newCovidCase">Country</param>
        /// <returns>new country</returns>
        public IHttpActionResult Post([FromBody]Covid_Case newCovidCase)
        {
            Covid_Case covid = dc.Covid_Cases.FirstOrDefault(c => c.Country == newCovidCase.Country);

            if (covid != null)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Conflict,
                    "There is already a country registered with that name."));
            }

            dc.Covid_Cases.InsertOnSubmit(newCovidCase);

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.ServiceUnavailable, e));
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        // PUT: api/Covid/5        
        public IHttpActionResult Put([FromBody]Covid_Case CovidCaseChanged)
        {
            Covid_Case covid = dc.Covid_Cases.FirstOrDefault(c => c.Country == CovidCaseChanged.Country);

            if (covid == null)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound,
                    "There is not country with that name to be changed."));
            }
            covid.Country = CovidCaseChanged.Country;
            covid.TotalCases = CovidCaseChanged.TotalCases;
            covid.TotalDeths = CovidCaseChanged.TotalDeths;
            covid.TotalRecovery = CovidCaseChanged.TotalRecovery;
            covid.TotalTests = CovidCaseChanged.TotalTests;

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.ServiceUnavailable, e));
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }

        // DELETE: api/Covid/5
        [Route("api/Covid/{country}")]
        public IHttpActionResult Delete(string country)
        {
            Covid_Case covid = dc.Covid_Cases.FirstOrDefault(c => c.Country == country);

            if (covid == null)
            {
                dc.Covid_Cases.DeleteOnSubmit(covid);

                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.ServiceUnavailable, e));
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "There is not country with that name to be deleted."));
        }
    }
}
