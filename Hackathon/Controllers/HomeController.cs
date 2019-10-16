using Hackathon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Nest;
using Amazon;
using Amazon.Runtime;
using System.Web.Configuration;

namespace Hackathon.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        class DocumentAttributes
        {
            public string s3link { get; set; }
        }

        private HackathonDBEntities db = new HackathonDBEntities();
        public ViewResult Index(string SearchString,string SearchString1,string SearchString2)
        {
            string esDomain = WebConfigurationManager.AppSettings["EsEndpoint"];

            var credentials = new EnvironmentAWSCredentials();
            var httpConnection = new AwsHttpConnection(credentials, RegionEndpoint.USEast1);

            var pool = new SingleNodeConnectionPool(new Uri(esDomain));
            var config = new ConnectionSettings(pool, httpConnection);
            var client = new ElasticClient(config);

            var getTableData = client.Search<DocumentAttributes>(s => s.Index("resume")); // you can get maximum 10000 records at a time  

            List<DocumentAttributes> lstData = new List<DocumentAttributes>();

            foreach (var hit in getTableData.Hits)
            {
                lstData.Add(hit.Source);
            }
           
            return View(lstData);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Report()
        {
            return View();
        }
    }
}