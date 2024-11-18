using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaylorWessing.Models;
using TaylorWessing.DAL;
using TaylorWessing.Log;
using System.Net;

namespace TaylorWessing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientDataController : ControllerBase
    {
        private ILog logger = new Log.Log(typeof(ClientDataController));
        
        /// <summary>
        /// This is and endpoint used to search for clients:
        /// </summary>
        /// <param name="searchterm">is the filter you wish to apply to search by client's name</param>
        /// <param name="columnorder">this is which column you wish to perform as sort against NAME or DATE. The default value if not supplied is NAME.</param>
        /// <param name="sort">this is how you wish to sort the column described in {column order} ASCENDING or DESCENDING</param>
        /// <param name="index">this is the point in the result set you wish to take you offset records from. This must be 0 or greater. (Used for building pagination)</param>
        /// <param name="offset">the count of records you wish to return from the index. This must be a value of between 1 and 50. (Used for building pagination)</param>
        /// <returns></returns>
        [HttpGet("clientsearch")]
        public IEnumerable<Client> ClientSearch(string searchterm, string columnorder,string sort, int index, int offset)
        {
            try
            {
                using (var context = new TaylorWessingContext())
                {
                    var b = context.Clients.Include(c => c.Matters).Where(c => c.Name.Contains(searchterm)).AsQueryable();
                    switch (sort.Trim().ToLower())
                    {
                        case "ascending":
                            return b.OrderBy(p => EF.Property<object>(p, columnorder)).ToList().Skip(index).Take(offset);
                        case "descending":
                            return b.OrderByDescending(p => EF.Property<object>(p, columnorder)).ToList().Skip(index).Take(offset);
                    }
                    return b.ToList().Skip(index).Take(offset);
                }
            }
            catch (Exception ex) 
            {
                logger.Error("error on api/ClientSearch ", ex.GetBaseException());
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                throw ex;
            }
        }
        /// <summary>
        /// This is the endpoint to fetch client data by a given client id:
        /// </summary>
        /// <param name="clientid"> is returned when a successful search has been performed in the Client Search endpoint.</param>
        /// <returns></returns>
        [HttpGet("client")]
        public IEnumerable<Client>  Client(int clientid)
        {
            try
            {
                using (var context = new TaylorWessingContext())
                {
                    return context.Clients.Include(c => c.Matters).Where(c => c.ClientId == clientid).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error("error on api/get Client ", ex.GetBaseException());
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                throw ex;
            }
        }
        /// <summary>
        /// This is an endpoint to fetch a specific matter by its matter id:
        /// </summary>
        /// <param name="matterid">is returned when a successful search has been performed in the Matter Search Endpoint.</param>
        /// <returns></returns>
        [HttpGet("matter")]
        public IEnumerable<Matter> Matter(int matterid)
        {
            try
            {
                using (var context = new TaylorWessingContext())
                {
                    return context.Matters.Include(c => c.Client).Where(c => c.MatterId == matterid).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error("error on api/get Matter ", ex.GetBaseException());
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                throw ex;
            }
        }
        /// <summary>
        /// This is the endpoint used to return matters for a given client Id:
        /// </summary>
        /// <param name="clientid">is used to return matters for the given client id as returned from the initial Client Search endpoint.</param>
        /// <param name="columnorder">this is which column you wish to perform as sort against NAME or DATE. The default value if not supplied is DATE.</param>
        /// <param name="sort">this is how you wish to sort the column described in {column order} ASCENDING or DESCENDING</param>
        /// <param name="index">this is the point in the result set you wish to take you offset records from. This must be 0 or greater. (Used for building pagination)</param>
        /// <param name="offset">the count of records you wish to return from the index. This must be a value of between 1 and 50. (Used for building pagination)</param>
        /// <returns></returns>
        [HttpGet("mattersearch")]
        public IEnumerable<Matter> MatterSearch(int clientid, string columnorder, string sort, int index, int offset)
        {
            try
            {
                using (var context = new TaylorWessingContext())
                {
                    var b = context.Matters.Where(c => c.ClientId == clientid).AsQueryable();
                    switch (sort.Trim().ToLower())
                    {
                        case "ascending":
                            return b.OrderBy(p => EF.Property<object>(p, columnorder)).ToList().Skip(index).Take(offset);
                        case "descending":
                            return b.OrderByDescending(p => EF.Property<object>(p, columnorder)).ToList().Skip(index).Take(offset);
                    }
                    return b.ToList().Skip(index).Take(offset);
                }
            }            
            catch (Exception ex)
            {
                logger.Error("error on api/MatterSearch ", ex.GetBaseException());
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                throw ex;
            }
        }
    }
}