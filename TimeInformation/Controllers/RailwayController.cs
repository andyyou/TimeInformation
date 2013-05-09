using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimeInformation.Models;

namespace TimeInformation.Controllers
{
    public class RailwayController : ApiController
    {
        // GET api/railway
        public IEnumerable<string> Get(string FromStation, string ToStation, DateTime GetinDate)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/StationList.json");

            // Deserialize json file to list
            List<StationList> stationList = JsonConvert.DeserializeObject<List<StationList>>(
                File.ReadAllText(path)).Select(c => (StationList)c).ToList();
            
            // Get station code from list
            string fromStationCode = stationList.Where(s => s.zh_TW == FromStation || s.en_US == FromStation).SingleOrDefault().Station;
            string toStationCode = stationList.Where(s => s.zh_TW == ToStation || s.en_US == ToStation).SingleOrDefault().Station;


            return new string[] { fromStationCode, toStationCode, GetinDate.ToShortDateString() };
        }

        // GET api/railway/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/railway
        public void Post([FromBody]string value)
        {
        }

        // PUT api/railway/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/railway/5
        public void Delete(int id)
        {
        }
    }
}
