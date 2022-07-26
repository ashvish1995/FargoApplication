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
using System.Data;
using System.Configuration;


namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class MPesaTransactionAPIController : ApiController
    {
        [HttpPost]
        [Route("api/MPesaTransactionAPI/MPesaValidation")]
        public HttpResponseMessage MPesaValidation([FromBody] MPesaValidation mPesaValidation)
        {
          
            try
            {
                int result = 0;
                string BasicAuthenticationCredentials = ConfigurationManager.AppSettings["BasicAuthenticationCredentials"].ToString();
                string BusinessShortCode = ConfigurationManager.AppSettings["BusinessShortCode"].ToString();
                string PasswordKey = ConfigurationManager.AppSettings["PasswordKey"].ToString();

                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    DataTable dataTable = MPesaTransactionManager.MPesaValidation(mPesaValidation);
                    if (dataTable != null && dataTable.Rows.Count>0)
                    {
                        string MPESA_TRANSACTION_ID = dataTable.Rows[0]["MPESA_TRANSACTION_ID"].ToString();
                        string CUSTOMER_NAME = dataTable.Rows[0]["CUSTOMER_NAME"].ToString();
                        string CUSTOMER_MOBILE = dataTable.Rows[0]["CUSTOMER_MOBILE"].ToString();
                        double MPESA_AMOUNT = Convert.ToDouble(dataTable.Rows[0]["MPESA_AMOUNT"].ToString());
                        string CREATED_ON = dataTable.Rows[0]["CREATED_ON"].ToString();
                        string TIMESTAMP = dataTable.Rows[0]["TIMESTAMP"].ToString();
                       ;
                        string accessToken = MPesaTransactionManager.GenerateAccessToken(BasicAuthenticationCredentials);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            result= MPesaTransactionManager.MPesaProcess(MPESA_TRANSACTION_ID, CUSTOMER_MOBILE, MPESA_AMOUNT, TIMESTAMP, accessToken, BusinessShortCode, PasswordKey);
                        }

                        if ((!string.IsNullOrEmpty(accessToken)) && (result > 0))
                        {
                            responseModel.Status = "Success";
                            responseModel.Message = "Transaction successfully made.";
                            responseModel.Description = "Transaction successfully made.";
                            return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                        }
                        else
                        {
                            responseModel.Status = "Failed";
                            responseModel.Message = "Transaction not done.";
                            responseModel.Description = "Transaction not done.";
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                        }                       
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Transaction not done.";
                        responseModel.Description = "Transaction not done.";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }


        [HttpPost]
        [Route("api/MPesaTransactionAPI/MPesaTransactionRequest")]
        public HttpResponseMessage MPesaTransactionRequest([FromBody] MPesaTransactionModel mPesaTransactionModel)
        {

            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    //int result = MPesaTransactionManager.MPesaTransactionRequest(mPesaTransactionModel);
                    //if (result > 0)
                    //{
                    //    if (mPesaTransactionModel.ResultDesc.ToLower().Contains("success"))
                    //    {
                    //        responseModel.Status = "Success";
                    //        responseModel.Message = "Transaction successfully made.";
                    //        responseModel.Description = "The service request is processed successfully.";
                    //        return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                    //    }
                    //    else
                    //    {
                    //        responseModel.Status = "Failed";
                    //        responseModel.Message = "Transaction not made.";
                    //        responseModel.Description = "The service request is not processed.";
                    //        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    //    }                       
                    //}
                    //else
                    //{
                    //    responseModel.Status = "Failed";
                    //    responseModel.Message = "Transaction not made.";
                    //    responseModel.Description = "The service request is not processed.";
                    //    return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    //}
                    if (!string.IsNullOrEmpty(mPesaTransactionModel.ResultDesc) && mPesaTransactionModel.ResultDesc.ToLower().Contains("success"))
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "Transaction successfully made.";
                        responseModel.Description = "The service request is processed successfully.";
                        return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Transaction not made.";
                        responseModel.Description = "The service request is not processed.";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    } 
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }

    }
}
