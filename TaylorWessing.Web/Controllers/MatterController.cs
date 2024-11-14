using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using TaylorWessing.Models;

namespace TaylorWessing.Web.Controllers
{
    public class MatterController : Controller
    {
        string baseUri;
        string XApiKey;
        public MatterController(IConfiguration config)
        {
            baseUri = config.GetValue<string>("ApiUrl");
            XApiKey = config.GetValue<string>("XApiKey");

        }
        public IActionResult Index()
        {
            return View(new List<Matter>());
        }
        public IActionResult SearchIndex()
        {
            return View("Search", new List<Matter>());
        }
        public async Task<ActionResult> Search()
        {
            var resMatter = new List<Matter>();
            string Uri = "/api/ClientData/mattersearch";
            string results = string.Empty;
            //
            string requesturl = BuildRequestUrl(Uri, Request.Form["clientid"], Request.Form["sortcolumn"], Request.Form["sortby"], Request.Form["PageIndex"], Request.Form["PageOffset"]);
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("XApiKey", XApiKey);
                HttpResponseMessage Res = await client.GetAsync(requesturl.ToString());
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    resMatter = JsonConvert.DeserializeObject<List<Matter>>(EmpResponse);
                }
                return View("Search", resMatter);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Get()
        {
            var matters = new List<Matter>();
            string Uri = "/api/ClientData/matter?matterid=";
            string results = string.Empty;

            ViewBag.matterid = Request.Form["Matterid"];
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("XApiKey", XApiKey);
                //
                HttpResponseMessage Res = await client.GetAsync(Uri + Request.Form["Matterid"]);
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    matters = JsonConvert.DeserializeObject<List<Matter>>(EmpResponse);
                }
                return View("Index", matters);
            }
        }
        private string BuildRequestUrl(string Uri, string clientid, string sortcolumn, string sortby, string PageIndex, string PageOffset)
        {
            StringBuilder requesturl = new StringBuilder();
            //
            StoretoView(clientid, sortcolumn, sortby, PageIndex, PageOffset);
            requesturl.Append(Uri);
            requesturl.Append("?clientid=");
            requesturl.Append(clientid);
            requesturl.Append("&columnorder=");
            requesturl.Append(sortcolumn);
            requesturl.Append("&sort=");
            requesturl.Append(sortby);
            requesturl.Append("&index=");
            requesturl.Append(PageIndex);
            requesturl.Append("&offset=");
            requesturl.Append(PageOffset);
            return requesturl.ToString();
        }
        private void StoretoView(string clientid, string sortcolumn, string sortby, string PageIndex, string PageOffset)
        {
            ViewBag.clientid = clientid;
            ViewBag.sortcolumn = sortcolumn;
            ViewBag.sortby = sortby;
            ViewBag.PageIndex = PageIndex;
            ViewBag.PageOffset = PageOffset;
        }
    }
}
