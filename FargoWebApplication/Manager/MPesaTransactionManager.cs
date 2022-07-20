using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;
using FargoWebApplication.Filter;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace FargoWebApplication.Manager
{
    public class MPesaTransactionManager
    {
        public static int MPesaTransactionRequest(MPesaTransactionModel mPesaTransactionModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@MerchantRequestID", mPesaTransactionModel.MerchantRequestID);
                SqlParameter sp2 = new SqlParameter("@CheckoutRequestID", mPesaTransactionModel.CheckoutRequestID);
                SqlParameter sp3 = new SqlParameter("@ResultCode", mPesaTransactionModel.ResultCode);
                SqlParameter sp4 = new SqlParameter("@ResultDesc", mPesaTransactionModel.ResultDesc);
                SqlParameter sp5 = new SqlParameter("@CallbackMetadata", mPesaTransactionModel.CallbackMetadata);
                SqlParameter sp6 = new SqlParameter("@USER_ID", mPesaTransactionModel.USER_ID);
                SqlParameter sp7 = new SqlParameter("@CREATED_ON", DateTime.Now);
                SqlParameter sp8 = new SqlParameter("@FLAG", "1");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static DataTable MPesaValidation(MPesaValidation mPesaValidation)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlParameter sp1 = new SqlParameter("@CUSTOMER_NAME", mPesaValidation.CUSTOMER_NAME);
                SqlParameter sp2 = new SqlParameter("@CUSTOMER_MOBILE", mPesaValidation.CUSTOMER_MOBILE);
                SqlParameter sp3 = new SqlParameter("@MPESA_AMOUNT", mPesaValidation.MPESA_AMOUNT);
                SqlParameter sp4 = new SqlParameter("@CREATED_BY", mPesaValidation.USER_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "1");
                dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2, sp3, sp4, sp5);
                
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataTable;
        }

        public static string GenerateAccessToken(string BasicAuthenticationCredentials)
        {
            string accessToken = string.Empty;
            try
            {
                string JSONResponse = "{";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string URL = String.Format("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials");
                WebRequest webRequest = WebRequest.Create(URL);
                webRequest.Method = "GET";
                webRequest.Headers["Authorization"] = "Basic " + BasicAuthenticationCredentials;
                webRequest.ContentType = "application/json";
                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseString = streamReader.ReadLine();
                while (responseString != null)
                {
                    Console.WriteLine(responseString);
                    responseString = streamReader.ReadLine();
                    JSONResponse = JSONResponse + responseString;
                }               
                GenerateTokenModel generateTokenModel = JsonConvert.DeserializeObject<GenerateTokenModel>(JSONResponse);
                accessToken = generateTokenModel.access_token;
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return accessToken;
        }

        public static int MPesaProcess(string MPESA_TRANSACTION_ID, string CUSTOMER_MOBILE, double MPESA_AMOUNT, string TIMESTAMP, string accessToken, string BusinessShortCode, string PasswordKey)
        {
            int result=0;
            try
            {
                string JSONResponse = "{";
                MPesaProcessModel mPesaProcessModel = new MPesaProcessModel();
                mPesaProcessModel.BusinessShortCode = BusinessShortCode;
                mPesaProcessModel.Password = EncryptDecryptString.EncodeBase64(BusinessShortCode + PasswordKey + TIMESTAMP);
                mPesaProcessModel.Timestamp = TIMESTAMP;
                mPesaProcessModel.TransactionType = "CustomerPayBillOnline";
                mPesaProcessModel.Amount = MPESA_AMOUNT;
                mPesaProcessModel.PartyA = "254708374149";//CUSTOMER_MOBILE;
                mPesaProcessModel.PartyB = BusinessShortCode;
                mPesaProcessModel.PhoneNumber = "254708374149";//CUSTOMER_MOBILE;
                mPesaProcessModel.CallBackURL = "https://fargo.speed18.com/api/MPesaTransactionAPI/MPesaTransactionRequest";
                mPesaProcessModel.AccountReference = "Test";
                mPesaProcessModel.TransactionDesc = "Test";
                string JSONString = JsonConvert.SerializeObject(mPesaProcessModel);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string URL = String.Format("https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest");
                WebRequest webRequest = WebRequest.Create(URL);
                webRequest.Method = "POST";
                webRequest.Headers["Authorization"] = "Bearer " + accessToken;
                webRequest.ContentType = "application/json";

                using (var stramWriter= new StreamWriter(webRequest.GetRequestStream()))
                {
                    stramWriter.Write(JSONString);
                    stramWriter.Flush();
                    stramWriter.Close();
                    HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    string responseString = streamReader.ReadLine();
                    while (responseString != null)
                    {
                        responseString = streamReader.ReadLine();
                        JSONResponse = JSONResponse + responseString;
                    } 
                }
                string Base64EndcodedJSONResponse = EncryptDecryptString.EncodeBase64(JSONResponse);
                SqlParameter sp1 = new SqlParameter("@FLAG", "2");
                SqlParameter sp2 = new SqlParameter("@MPESA_TRANSACTION_ID", MPESA_TRANSACTION_ID);
                SqlParameter sp3 = new SqlParameter("@MPESA_PROCESS_RESPONSE", Base64EndcodedJSONResponse);
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spMPesaTransaction", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}