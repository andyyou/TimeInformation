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
using System.Xml.Serialization;

namespace TimeInformation.Controllers
{
    public class RailwayController : ApiController
    {
        // GET api/railway
        public IEnumerable<TimeTable> Get(string fromStation, string toStation, DateTime getinDate)
        {
            // Verify GetinDate, tra only provide 45 days timetable
            if ((DateTime.Compare(DateTime.Today, getinDate) > 0) ||
                (DateTime.Compare(getinDate, DateTime.Today.AddDays(44)) > 0))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            }

            # region Deal XML File

            string xmlFolder = System.Web.HttpContext.Current.Server.MapPath("~/Xml/");
            string localXmlPath = xmlFolder + getinDate.ToString("yyyyMMdd") + ".xml";
            string localZipPath = localXmlPath.Replace(".xml", ".zip");

            // Delete expired xml file
            DateTime dtToday = DateTime.Today;
            DirectoryInfo di = new DirectoryInfo(xmlFolder);
            FileInfo[] xmlFiles = di.GetFiles("*.xml");
            foreach (var file in xmlFiles)
            {
                DateTime dtFile = DateTime.ParseExact(file.Name.Substring(0, 8), new string[] { "yyyyMMdd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                if (DateTime.Compare(dtToday, dtFile) > 0)
                {
                    File.Delete(file.FullName);
                }
            }

            // Download xml file if file not exist
            if (!File.Exists(localXmlPath))
            {
                string remoteZipPath = @"http://163.29.3.98/XML/" + getinDate.ToString("yyyyMMdd") + ".zip";
                // Download zip file from remote server
                using (var client = new WebClient())
                {
                    client.DownloadFile(remoteZipPath, localZipPath);
                }
                // Unzip file(Supported by .NET Framework 4.5)
                System.IO.Compression.ZipFile.ExtractToDirectory(localZipPath, xmlFolder);
                // Delete zip file
                File.Delete(localZipPath);
            }

            #endregion

            #region Parse Data

            //string stationFilePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/StationList.json");

            // Deserialize json file to list
            //List<StationList> stationList = JsonConvert.DeserializeObject<List<StationList>>(
            //    File.ReadAllText(stationFilePath)).Select(c => (StationList)c).ToList();

            // Get station code from list
            //string fromStationCode = stationList.Where(s => s.zh_TW == FromStation || s.en_US == FromStation).SingleOrDefault().Station;
            //string toStationCode = stationList.Where(s => s.zh_TW == ToStation || s.en_US == ToStation).SingleOrDefault().Station;

            // Parse XML

            XmlSerializer ser = new XmlSerializer(typeof(TaiTrainList));
            TaiTrainList allTrain = ser.Deserialize(new StreamReader(localXmlPath)) as TaiTrainList;
            List<TrainInfo> trainInfo = allTrain.TrainInfo.Where(t => t.TimeInfo != null).ToList();

            var fromTrainList = trainInfo.SelectMany(t => t.TimeInfo, (t, i) =>
                new
                {
                    // TrainInfo
                    Train = t.Train,
                    CarClass = t.CarClass,
                    Line = t.Line,
                    Origin = t.TimeInfo.First().Station,
                    Dest = t.TimeInfo.Last().Station,
                    Note = t.Note,
                    // TimeInfo
                    Order = Convert.ToInt32(i.Order),
                    Station = i.Station,
                    ARRTime = i.ARRTime,
                    DEPTime = i.DEPTime
                })
                .Where(i => i.Station == fromStation).ToList();
            var toTrainList = trainInfo.SelectMany(t => t.TimeInfo, (t, i) =>
                new
                {
                    // TrainInfo
                    Train = t.Train,
                    CarClass = t.CarClass,
                    Note = t.Note,
                    // TimeInfo
                    Order = Convert.ToInt32(i.Order),
                    Station = i.Station,
                    ARRTime = i.ARRTime,
                    DEPTime = i.DEPTime
                })
                .Where(i => i.Station == toStation).ToList();

            List<TimeTable> timeTable = new List<TimeTable>();

            foreach (var fromTrain in fromTrainList)
            {
                foreach (var toTrain in toTrainList)
                {
                    if (fromTrain.Train == toTrain.Train && fromTrain.Order < toTrain.Order)
                    {
                        timeTable.Add(new TimeTable
                        {
                            Train = fromTrain.Train,
                            CarClass = fromTrain.CarClass,
                            Line = fromTrain.Line,
                            Origin = fromTrain.Origin,
                            Dest = fromTrain.Dest,
                            DEPTime = fromTrain.DEPTime,
                            ARRTime = toTrain.ARRTime,
                            Note = fromTrain.Note
                        });
                    }
                }
            }

            return timeTable.OrderBy(t => t.DEPTime);
            #endregion
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
