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

namespace FargoWebApplication.Manager
{
    public class SettlementManager
    {
        public static List<SettlementModel> SettlementInfo(SettlementModel settlementModel)
        {
            List<SettlementModel> LstSettlement = new List<SettlementModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spSettlement", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        SettlementModel _settlementModel = new SettlementModel();
                        //_settlementModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        //_settlementModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        //_settlementModel.DAY_IN_TIME = sqlDataReader["DAY_IN_TIME"].ToString();
                        //_settlementModel.TOTAL_DAY_IN_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_DAY_IN_AMOUNT"].ToString());
                        //_settlementModel.TOTAL_CASH_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CASH_AMOUNT"].ToString());
                        //_settlementModel.TOTAL_MPESA_AMOUNT =Convert.ToDouble(sqlDataReader["TOTAL_MPESA_AMOUNT"].ToString();
                        //_settlementModel.TOTAL_CREDIT_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_CREDIT_AMOUNT"].ToString();
                        //_settlementModel.NO_OF_CASH_TRANSACTION = Convert.ToDouble(sqlDataReader["NO_OF_CASH_TRANSACTION"].ToString());
                        //_settlementModel.NO_OF_MPESA_TRANSACTION = Convert.ToDouble(sqlDataReader["NO_OF_MPESA_TRANSACTION"].ToString();
                        //_settlementModel.NO_OF_CREDIT_TRANSACTION = Convert.ToDouble(sqlDataReader["NO_OF_CREDIT_TRANSACTION"].ToString();
                        //_settlementModel.TOTAL_DAY_END_AMOUNT = sqlDataReader["TOTAL_DAY_END_AMOUNT"].ToString();
                        //_settlementModel.DAY_END_TIME = sqlDataReader["DAY_END_TIME"].ToString();
                        //_settlementModel.MANAGER_ID = Convert.ToDouble(sqlDataReader["MANAGER_ID"].ToString();
                        //_settlementModel.STATUS = sqlDataReader["STATUS"].ToString();
                        //_settlementModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        //_settlementModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        //_settlementModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        //LstSettlement.Add(_settlementModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstSettlement;
        }
    }
}