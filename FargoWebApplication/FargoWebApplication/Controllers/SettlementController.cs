using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApplication.Controllers
{
    [UserAuthorization]
    public class SettlementController : Controller
    {
        // GET: Settlement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SettlementModel settlementModel)
        {
            List<SettlementModel> LstSettlement = null;
            //LstSettlement = SettlementManager.SettlementInfo(settlementModel);

            if (LstSettlement != null && LstSettlement.Count > 0)
                ViewBag.IsData = '1';
            else
                ViewBag.IsData = '0';

            ViewBag.LstSettlement = LstSettlement;
            return View();
        }
    }
}