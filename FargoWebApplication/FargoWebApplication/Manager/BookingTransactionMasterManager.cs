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
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Security;

namespace FargoWebApplication.Manager
{
    public class BookingTransactionMasterManager
    {
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public static List<BookingTransactionMasterModel> TransactionReport(BOOKING_TRANSACTION_MASTER _BOOKING_TRANSACTION_MASTER)
        {
            List<BookingTransactionMasterModel> LstBookingTransactionMaster = new List<BookingTransactionMasterModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@DATE", _BOOKING_TRANSACTION_MASTER.DATE);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", _BOOKING_TRANSACTION_MASTER.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReport", sp1, sp2, sp3);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        BookingTransactionMasterModel bookingTransactionMaster = new BookingTransactionMasterModel();

                        bookingTransactionMaster.IMEI_NUMBER = sqlDataReader["IMEI_NUMBER"].ToString();
                       //bookingTransactionMaster.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        bookingTransactionMaster.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        bookingTransactionMaster.CUSTOMER_CONTACT = sqlDataReader["CUSTOMER_CONTACT"].ToString();
                        bookingTransactionMaster.COURIER_ADDRESS = sqlDataReader["COURIER_ADDRESS"].ToString();
                        bookingTransactionMaster.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_AMOUNT"].ToString());
                        //bookingTransactionMaster.PAYMENT_MODE = sqlDataReader["PAYMENT_MODE"].ToString();
                        //bookingTransactionMaster.REFERENCE_NUMBER = sqlDataReader["REFERENCE_NUMBER"].ToString();
                        bookingTransactionMaster.DATE = sqlDataReader["DATE"].ToString();
                        bookingTransactionMaster.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();

                        LstBookingTransactionMaster.Add(bookingTransactionMaster);
                    }
                }
            }
            catch (Exception exception)
            {
              string ErrorMessage=  ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingTransactionMaster;
        }

     

        public static int SubmitBookingTransaction(BookingTransactionMasterModel bookingTransactionMaster)
        {
            int result = 0; 
            double? totalCashAmount = 0; double? totalMPesaAmount = 0; double? totalCreditAmount = 0;
            int NoOfCashTransaction = 0; int NoOfMPesaTransaction = 0; int NoOfCreditTransaction = 0;
            try
            {          
                SqlParameter sp1 = new SqlParameter("@USER_ID", bookingTransactionMaster.USER_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", bookingTransactionMaster.CASHIER_ID);
                SqlParameter sp3 = new SqlParameter("@STORE_ID", bookingTransactionMaster.STORE_ID);
                SqlParameter sp4 = new SqlParameter("@CUSTOMER_ID", bookingTransactionMaster.CUSTOMER_ID);
                SqlParameter sp5 = new SqlParameter("@IMEI_NUMBER", bookingTransactionMaster.IMEI_NUMBER);
                SqlParameter sp6 = new SqlParameter("@CUSTOMER_NAME", bookingTransactionMaster.CUSTOMER_NAME);
                SqlParameter sp7 = new SqlParameter("@CUSTOMER_CONTACT", bookingTransactionMaster.CUSTOMER_CONTACT);
                SqlParameter sp8 = new SqlParameter("@COURIER_ADDRESS", bookingTransactionMaster.COURIER_ADDRESS);
                SqlParameter sp9 = new SqlParameter("@TOTAL_AMOUNT", bookingTransactionMaster.TOTAL_AMOUNT);
                SqlParameter sp10 = new SqlParameter("@DATE", bookingTransactionMaster.DATE);
                SqlParameter sp11 = new SqlParameter("@TIME", bookingTransactionMaster.TIME);
                SqlParameter sp12 = new SqlParameter("@CREATED_BY", bookingTransactionMaster.USER_ID);
                SqlParameter sp13 = new SqlParameter("@CREATED_ON", DateTime.Now);
                SqlParameter sp14 = new SqlParameter("@FLAG", "1");

                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spBookingTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, sp11, sp12, sp13, sp14);
                if (dataTable != null) {
                    string BOOKING_TRANSACTION_ID = dataTable.Rows[0][0].ToString();
                    DataTable _DTPayment = DTPayment(bookingTransactionMaster.BOOKING_PAYMENT_DETAILS, BOOKING_TRANSACTION_ID, bookingTransactionMaster.USER_ID.ToString(), bookingTransactionMaster.CUSTOMER_ID, bookingTransactionMaster.TOTAL_AMOUNT, out totalCashAmount, out totalMPesaAmount, out totalCreditAmount, out NoOfCashTransaction, out NoOfMPesaTransaction, out NoOfCreditTransaction);
                    DataTable _DTOrder = DTOrder(bookingTransactionMaster.BOOKING_ORDER_DETAILS, BOOKING_TRANSACTION_ID, bookingTransactionMaster.USER_ID.ToString());

                    SqlParameter _sp1 = new SqlParameter("@BOOKING_TRANSACTION_ID", BOOKING_TRANSACTION_ID);
                    SqlParameter _sp2 = new SqlParameter("@tblPayment", _DTPayment);
                    SqlParameter _sp3 = new SqlParameter("@tblOrder", _DTOrder);
                    SqlParameter _sp4 = new SqlParameter("@TOTAL_CASH_AMOUNT", totalCashAmount);
                    SqlParameter _sp5 = new SqlParameter("@TOTAL_MPESA_AMOUNT", totalMPesaAmount);
                    SqlParameter _sp6 = new SqlParameter("@TOTAL_CREDIT_AMOUNT", totalCreditAmount);
                    SqlParameter _sp7 = new SqlParameter("@NO_OF_CASH_TRANSACTION", NoOfCashTransaction);
                    SqlParameter _sp8 = new SqlParameter("@NO_OF_MPESA_TRANSACTION", NoOfMPesaTransaction);
                    SqlParameter _sp9 = new SqlParameter("@NO_OF_CREDIT_TRANSACTION", NoOfCreditTransaction);
                    SqlParameter _sp10 = new SqlParameter("@CASHIER_ID", bookingTransactionMaster.CASHIER_ID);
                    SqlParameter _sp11 = new SqlParameter("@STORE_ID", bookingTransactionMaster.STORE_ID);
                    SqlParameter _sp12 = new SqlParameter("@FLAG", "2");

                    result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spBookingTransaction", _sp1, _sp2, _sp3, _sp4, _sp5, _sp6, _sp7, _sp8, _sp9, _sp10, _sp11, _sp12);
                    if (result > 0)
                    {
                        string _transactionId = "";
                        Int32 _initialTransactionId = 100000000;
                        _transactionId = (_initialTransactionId + Convert.ToInt32(BOOKING_TRANSACTION_ID)).ToString();

                        ETRTransactionBuyerModel _ETRTransactionBuyerModel = new ETRTransactionBuyerModel()
                        {
                            registrationName = "Domestic Customer",
                            taxIdentificationNumber = "PSK12121989"
                        };
                        List<ETRTransactionItemModel> _ETRTransactionItemModel = new List<ETRTransactionItemModel>()
                        {
                            new  ETRTransactionItemModel
                            {
                                code = "10000262",
                                description = "Air freight",
                                discount = 500.00,
                                hs="",
                                invoicedQuantity=1,
                                price = 5220.46,
                                taxCode = "A",
                                total = 4720.46
                            }                           
                        };
                        ETRTransactionTaxModel _ETRTransactionTaxModel = new ETRTransactionTaxModel()
                        {
                            vatNetAmount = 4069.36,
                            vatTaxAmount = 651.10                         
                        };
                        ETRTransactionModel _ETRTransactionModel = new ETRTransactionModel()
                        {
                            buyer = _ETRTransactionBuyerModel,
                            cashier1= "",
                            currencyCode= "KSH",
                            discountAmount= 0,
                            invoiceDocumentReference= "",
                            issueDate= "2022-07-15",
                            issueTime= "11:08:13",
                            items = _ETRTransactionItemModel,
                            posID= "pos01",
                            registrationID= "91KRA0030010073",
                            tax=_ETRTransactionTaxModel,
                            taxExclusiveAmount= 4069.36,
                            taxInclusiveAmount= 4720.46,
                            transactionID = _transactionId,
                            transactionTypeCode= 1
                        };

                        string JSONResponse = "{";
                        string JSONString = JsonConvert.SerializeObject(_ETRTransactionModel);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        ServicePointManager.ServerCertificateValidationCallback = new
                                                                                RemoteCertificateValidationCallback
                                                                                (
                                                                                   delegate { return true; }
                                                                                );

                        string URL = String.Format("https://52.168.16.149:8010/api/v2.0/transaction/new");
                        WebRequest webRequest = WebRequest.Create(URL);
                        webRequest.Method = "POST";
                        webRequest.Headers["clientid"] = "OiZqm01q9S51y5J";
                        webRequest.Headers["accessKey"] = "3ZixXmuHFk7qyXO+2sfxPxFmEROn4m13mir+gRjFFfk=";
                        webRequest.ContentType = "application/json";

                        using (var stramWriter = new StreamWriter(webRequest.GetRequestStream()))
                        {
                            stramWriter.Write(JSONString);
                            stramWriter.Flush();
                            stramWriter.Close();
                            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                            string responseString = streamReader.ReadLine();
                            JSONResponse = responseString;
                        }
                        string Base64EndcodedJSONResponse = EncryptDecryptString.EncodeBase64(JSONResponse);
                        ETRTransactionResponseModel _ETRTransactionResponseModel = JsonConvert.DeserializeObject<ETRTransactionResponseModel>(JSONResponse);

                        SqlParameter sp1_ = new SqlParameter("BOOKING_TRANSACTION_ID", BOOKING_TRANSACTION_ID);
                        SqlParameter sp2_ = new SqlParameter("TRANSACTION_ID", _ETRTransactionResponseModel.transactionID);
                        SqlParameter sp3_ = new SqlParameter("CU_NUMBER", _ETRTransactionResponseModel.signature.cuNumber);
                        SqlParameter sp4_ = new SqlParameter("TIMESTAMP", _ETRTransactionResponseModel.signature.timestamp);
                        SqlParameter sp5_ = new SqlParameter("FISCAL_TRANSACTION_NUMBER", _ETRTransactionResponseModel.signature.fiscalTransactionNumber);
                        SqlParameter sp6_ = new SqlParameter("QR", _ETRTransactionResponseModel.qr);
                        SqlParameter sp7_ = new SqlParameter("IS_DUPLICATE", _ETRTransactionResponseModel.isDuplicate);
                        SqlParameter sp8_ = new SqlParameter("SUCCESS", _ETRTransactionResponseModel.success);
                        SqlParameter sp9_ = new SqlParameter("ERROR_CODE", _ETRTransactionResponseModel.errorCode);
                        SqlParameter sp10_ = new SqlParameter("ERROR_MESSAGE", _ETRTransactionResponseModel.errorMessage);
                        SqlParameter sp11_ = new SqlParameter("CREATED_BY", bookingTransactionMaster.USER_ID);
                        SqlParameter sp12_ = new SqlParameter("FLAG", "1");
                        int ETRResult = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spETRTransaction", sp1_, sp2_, sp3_, sp4_, sp5_, sp6_, sp7_, sp8_, sp9_, sp10_, sp11_, sp12_);
                    }            
                }                    
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        private static DataTable DTPayment(List<BookingPaymentDetailsModel> _BOOKING_PAYMENT_DETAILS, string BOOKING_TRANSACTION_ID, string USER_ID,long CUSTOMER_ID, double? TOTAL_AMOUNT, out double? totalCashAmount, out double? totalMPesaAmount, out double? totalCreditAmount, out int NoOfCashTransaction, out int NoOfMPesaTransaction, out int NoOfCreditTransaction)
        {
            totalCashAmount=0; totalMPesaAmount=0;  totalCreditAmount=0;
            NoOfCashTransaction = 0; NoOfMPesaTransaction=0;  NoOfCreditTransaction = 0;

            DataTable _DTPayment = new DataTable();
            _DTPayment.Columns.Add("BOOKING_TRANSACTION_ID");
            _DTPayment.Columns.Add("REFERENCE_NUMBER");
            _DTPayment.Columns.Add("PAYMENT_MODE");
            _DTPayment.Columns.Add("AMOUNT");
            _DTPayment.Columns.Add("STATUS");
            _DTPayment.Columns.Add("DESCRIPTION");
            _DTPayment.Columns.Add("DATA_SOURCE");
            _DTPayment.Columns.Add("IS_ACTIVE");
            _DTPayment.Columns.Add("CREATED_BY");
            _DTPayment.Columns.Add("CREATED_ON");
            _DTPayment.AcceptChanges();
            try
            {
                if (CUSTOMER_ID > 0)
                {
                    totalCreditAmount = TOTAL_AMOUNT;
                    NoOfCreditTransaction = 1;

                    _DTPayment.Rows.Add(BOOKING_TRANSACTION_ID, null, "CREDIT", TOTAL_AMOUNT, null, "Full payment made via Credit.", "M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                    _DTPayment.AcceptChanges();
                }
                else 
                {
                    foreach (BookingPaymentDetailsModel BookingPaymentDetails in _BOOKING_PAYMENT_DETAILS)
                    {
                        if (BookingPaymentDetails.AMOUNT > 0)
                        {
                            if (BookingPaymentDetails.PAYMENT_MODE == "CASH")
                            {
                                totalCashAmount += BookingPaymentDetails.AMOUNT == null ? 0 : BookingPaymentDetails.AMOUNT;
                                NoOfCashTransaction = NoOfCashTransaction + 1;
                            }
                            if (BookingPaymentDetails.PAYMENT_MODE == "MPESA")
                            {
                                totalMPesaAmount += BookingPaymentDetails.AMOUNT == null ? 0 : BookingPaymentDetails.AMOUNT;
                                NoOfMPesaTransaction = NoOfMPesaTransaction + 1;
                            }
                            _DTPayment.Rows.Add(BOOKING_TRANSACTION_ID, BookingPaymentDetails.REFERENCE_NUMBER, BookingPaymentDetails.PAYMENT_MODE, BookingPaymentDetails.AMOUNT, BookingPaymentDetails.STATUS, BookingPaymentDetails.DESCRIPTION, "M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                            _DTPayment.AcceptChanges();
                        }                       
                    }                                                
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return _DTPayment;
        }

        private static DataTable DTOrder(List<BookingOrderDetailsModel> _BOOKING_ORDER_DETAILS, string BOOKING_TRANSACTION_ID, string USER_ID)
        {
            DataTable _DTOrder = new DataTable();
            _DTOrder.Columns.Add("BOOKING_TRANSACTION_ID");
            _DTOrder.Columns.Add("TRACKING_NUMBER");
            _DTOrder.Columns.Add("STATUS");
            _DTOrder.Columns.Add("DESCRIPTION");
            _DTOrder.Columns.Add("DATA_SOURCE");
            _DTOrder.Columns.Add("IS_ACTIVE");
            _DTOrder.Columns.Add("CREATED_BY");
            _DTOrder.Columns.Add("CREATED_ON");
            _DTOrder.AcceptChanges();
            try
            {
                foreach (BookingOrderDetailsModel BookingOrderDetails in _BOOKING_ORDER_DETAILS)
                {
                    _DTOrder.Rows.Add(BOOKING_TRANSACTION_ID, BookingOrderDetails.TRACKING_NUMBER, BookingOrderDetails.STATUS, BookingOrderDetails.DESCRIPTION,"M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                    _DTOrder.AcceptChanges();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return _DTOrder;
        }


        public static List<StoreModel> LstStores()
        {
            List<StoreModel> LstStores = new List<StoreModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportBookingTransaction", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        StoreModel storeModel = new StoreModel();
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        LstStores.Add(storeModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstStores;
        }

        public static List<BookingTransactionMasterModel> BookingTransactionReport(BookingTransactionMasterModel bookingTransactionModel)
        {
            List<BookingTransactionMasterModel> LstBookingTransactionReport = new List<BookingTransactionMasterModel>();
            try
            {
                 string fromDate = "01-01-2022"; string toDate = DateTime.Now.AddDays(1).ToString("MM-dd-yyyy");
                 SqlParameter sp1 = null; SqlParameter sp2 = null; SqlParameter sp3 = null; SqlParameter sp4 = null;

                 if (string.IsNullOrEmpty(bookingTransactionModel.FROM_DATE) && string.IsNullOrEmpty(bookingTransactionModel.TO_DATE))
                 {
                     sp1 = new SqlParameter("@STORE_ID", bookingTransactionModel.STORE_ID);
                     sp2 = new SqlParameter("@FROM_DATE", fromDate);
                     sp3 = new SqlParameter("@TO_DATE", toDate);
                     sp4 = new SqlParameter("@FLAG", bookingTransactionModel.STORE_ID==null?"4":"3");
                 }
                 else
                 {
                     sp1 = new SqlParameter("@STORE_ID", bookingTransactionModel.STORE_ID);
                     sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.FROM_DATE));
                     sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(bookingTransactionModel.TO_DATE) ?ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(bookingTransactionModel.TO_DATE));
                     sp4 = new SqlParameter("@FLAG", bookingTransactionModel.STORE_ID == null ? "4" : "3");
                 }
                 SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportBookingTransaction", sp1, sp2, sp3, sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        BookingTransactionMasterModel bookingTransactionMaster = new BookingTransactionMasterModel();

                        bookingTransactionMaster.BOOKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["BOOKING_TRANSACTION_ID"].ToString());
                        bookingTransactionMaster.CASHIER_NAME = sqlDataReader["FIRST_NAME"].ToString() + " " + sqlDataReader["LAST_NAME"].ToString();
                        bookingTransactionMaster.IMEI_NUMBER = sqlDataReader["IMEI_NUMBER"].ToString();
                        bookingTransactionMaster.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        bookingTransactionMaster.CUSTOMER_CONTACT = sqlDataReader["CUSTOMER_CONTACT"].ToString();
                        bookingTransactionMaster.TOTAL_AMOUNT = Convert.ToInt64(sqlDataReader["TOTAL_AMOUNT"].ToString());
                        bookingTransactionMaster.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        bookingTransactionMaster.DATE = sqlDataReader["ENTRY_DATE"].ToString();

                        LstBookingTransactionReport.Add(bookingTransactionMaster);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingTransactionReport;
        }
   
    }
}