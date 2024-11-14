using Microsoft.AspNetCore.Mvc;
using TaylorWessing.Models;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using TaylorWessing.Log;
using System.Net;

namespace TaylorWessing.Web.Controllers
{
    public class ClientController : Controller
    {
        private ILog logger = new Log.Log(typeof(ClientController));
        string baseUri;
        string XApiKey;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public ClientController(IConfiguration config) 
        {
            baseUri = config.GetValue<string>("ApiUrl");
            XApiKey = config.GetValue<string>("XApiKey");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(new List<Client>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult SearchIndex()
        {
            return View("Search",new List<Client>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Get()
        {
            var clients = new List<Client>();
            string Uri = "/api/ClientData/client?clientid=";
            string results = string.Empty;

            try
            {
                ViewBag.Clientid = Request.Form["ClientId"];
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("XApiKey", XApiKey);
                    //
                    HttpResponseMessage Res = await client.GetAsync(Uri + Request.Form["ClientId"]);
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        clients = JsonConvert.DeserializeObject<List<Client>>(EmpResponse);
                    }
                    else
                    {
                        logger.Fatal(Res.ReasonPhrase);
                    }
                    return View("Index", clients);
                }
            }
            catch (Exception ex)
            {
                logger.Error("error on web/ClientController/Get ", ex.GetBaseException());
                return View("Index", clients);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]        
        public async Task<ActionResult> Search()
        {
            var clients = new List<Client>();
            string Uri = "/api/ClientData/ClientSearch";
            string results = string.Empty;

            try
            {
                string requesturl = BuildRequestUrl(Uri, Request.Form["Name"], Request.Form["sortcolumn"], Request.Form["sortby"], Request.Form["PageIndex"], Request.Form["PageOffset"]);
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
                        clients = JsonConvert.DeserializeObject<List<Client>>(EmpResponse);
                    }
                    return View("Search", clients);
                }
            }            
            catch (Exception ex)
            {
                logger.Error("error on web/ClientController/Search ", ex.GetBaseException());
                return View("Search", clients);
            }

}
/// <summary>
/// 
/// </summary>
/// <param name="Uri"></param>
/// <param name="name"></param>
/// <param name="sortcolumn"></param>
/// <param name="sortby"></param>
/// <param name="PageIndex"></param>
/// <param name="PageOffset"></param>
/// <returns></returns>
 private string BuildRequestUrl(string Uri, string name, string sortcolumn, string sortby, string PageIndex, string PageOffset)
        {
            StringBuilder requesturl = new StringBuilder();
            //
            StoretoView(name, sortcolumn, sortby, PageIndex, PageOffset);
            requesturl.Append(Uri);
            requesturl.Append("?searchterm=");
            requesturl.Append(name);
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
        private void StoretoView(string name, string sortcolumn, string sortby, string PageIndex, string PageOffset)
        {
            ViewBag.Name = name;
            ViewBag.sortcolumn = sortcolumn;
            ViewBag.sortby = sortby;
            ViewBag.PageIndex = PageIndex;
            ViewBag.PageOffset = PageOffset;
        }
      }
}
