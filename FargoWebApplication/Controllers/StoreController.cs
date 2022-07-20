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
    public class StoreController : Controller
    {

        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();

        public ActionResult Index()
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            ViewBag.UserId = SessionInformation.USER_ID; ViewBag.Message = null;

            List<StoreModel> LstStores = StoreManager.LstStores();
            ViewBag.LstStores = LstStores;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(StoreModel storeModel, string Submit, string Update)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                TempData["Message"] = null;
                if (!string.IsNullOrEmpty(Submit))
                {
                    storeModel.USER_ID = SessionInformation.USER_ID;
                    int result = StoreManager.Submit(storeModel);
                    if (result > 0)
                        TempData["Message"] = "Records successfully added.";
                    else
                        TempData["Message"] = "Records not added.";
                }
                if (!string.IsNullOrEmpty(Update))
                {
                    storeModel.USER_ID = SessionInformation.USER_ID;
                    int result = StoreManager.Update(storeModel);
                    if (result > 0)
                        TempData["Message"] = "Records successfully updated.";
                    else
                        TempData["Message"] = "Records not updated.";
                }
                return RedirectToAction("Index", "Store");
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }

        public ActionResult Edit(long STORE_ID)
        {
            try
            {
                StoreModel storeModel = StoreManager.Edit(STORE_ID);
                return Json(storeModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }


        public ActionResult Delete(long STORE_ID)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                string Message = null;
                StoreModel storeModel = new StoreModel();
                storeModel.STORE_ID = STORE_ID;
                storeModel.USER_ID = SessionInformation.USER_ID;
                int result = StoreManager.Delete(storeModel);
                if (result > 0)
                    Message = "Record deleted.";
                else
                    Message = "Record not deleted.";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }





        public ActionResult Slot()
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            ViewBag.UserId = SessionInformation.USER_ID; ViewBag.Message = null;

            List<StoreModel> LstStores = null; List<SlotModel> LstSlotes = null;

            DataSet dataSet = StoreManager.SlotInfo(out LstStores, out LstSlotes);
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");

            ViewBag.LstSlotes = LstSlotes;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Slot(SlotModel slotModel, string Submit, string Update)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                TempData["Message"] = null;
                if (!string.IsNullOrEmpty(Submit))
                {
                    slotModel.USER_ID = SessionInformation.USER_ID;
                    int result = StoreManager.SubmitSlot(slotModel);
                    if (result > 0)
                        TempData["Message"] = "Records successfully added.";
                    else
                        TempData["Message"] = "Records not added.";
                }
                if (!string.IsNullOrEmpty(Update))
                {
                    slotModel.USER_ID = SessionInformation.USER_ID;
                    int result = StoreManager.UpdateSlot(slotModel);
                    if (result > 0)
                        TempData["Message"] = "Records successfully updated.";
                    else
                        TempData["Message"] = "Records not updated.";
                }
                return RedirectToAction("Slot", "Store");
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }

        public ActionResult Modify(long SLOT_ID)
        {
            try
            {
                SlotModel slotModel = StoreManager.Modify(SLOT_ID);
                return Json(slotModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }


        public ActionResult Remove(long SLOT_ID)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                string Message = null;
                SlotModel slotModel = new SlotModel();
                slotModel.SLOT_ID = SLOT_ID;
                slotModel.USER_ID = SessionInformation.USER_ID;
                int result = StoreManager.Remove(slotModel);
                if (result > 0)
                    Message = "Record deleted.";
                else
                    Message = "Record not deleted.";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }



        //**************************************~FOR STORE TRACKING NO MAPPING~**********************************************
        public ActionResult Tracking()
        {
            ViewBag.LstStores = StoreManager.LstStores();
            return View();
        }


        [HttpPost]
        public ActionResult EditStoreTrackingNo(StoreModel storeModel)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                storeModel.USER_ID = SessionInformation.USER_ID; string Message=null;
                int result = StoreManager.EditStoreTrackingNo(storeModel);
                if (result > 0)
                    Message = "Data updated.";
                else
                    Message = "Data not updated.";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }
    }    
}