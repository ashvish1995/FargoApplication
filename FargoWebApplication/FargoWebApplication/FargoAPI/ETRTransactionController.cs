using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fargo_DataAccessLayers;
using FargoWebApplication.Filter;
using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Manager;
using System.Threading;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class ETRTransactionController : ApiController
    {
        [HttpGet]
        [Route("api/ETRTransaction/ETRFileLocation")]
        public IHttpActionResult ETRFileLocation(string TRANSACTION_ID)
        {
            ETRFileLocationModel ETRFileLocation = new ETRFileLocationModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    ETRFileLocation = ETRTransactionManager.ETRFileLocation(TRANSACTION_ID);
                    return Ok(ETRFileLocation);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return InternalServerError();
            }
        }
    }
}
