using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;

namespace FargoWebApplication.Manager
{
    public class StoreManager
    {

        public static int Submit(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_NAME", storeModel.STORE_NAME);
                SqlParameter sp2 = new SqlParameter("@STORE_CODE", storeModel.STORE_CODE);
                SqlParameter sp3 = new SqlParameter("@DESCRIPTION", storeModel.DESCRIPTION);
                SqlParameter sp4 = new SqlParameter("@CREATED_BY", storeModel.USER_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "2");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int Update(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", storeModel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_NAME", storeModel.STORE_NAME);
                SqlParameter sp3 = new SqlParameter("@STORE_CODE", storeModel.STORE_CODE);
                SqlParameter sp4 = new SqlParameter("@DESCRIPTION", storeModel.DESCRIPTION);
                SqlParameter sp5 = new SqlParameter("@MODIFIED_BY", storeModel.USER_ID);
                SqlParameter sp6 = new SqlParameter("@FLAG", "4");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3, sp4, sp5, sp6);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int Delete(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", storeModel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@DELETED_BY", storeModel.USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<StoreModel> LstStores()
        {
            List<StoreModel> LstStores = new List<StoreModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spStore", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        StoreModel storeModel = new StoreModel();
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        storeModel.STORE_CODE = sqlDataReader["STORE_CODE"].ToString();
                        storeModel.FROM_TRACKING_NO = sqlDataReader["FROM_TRACKING_NO"].ToString();
                        storeModel.TO_TRACKING_NO = sqlDataReader["TO_TRACKING_NO"].ToString();
                        storeModel.CURRENT_TRACKING_NO = sqlDataReader["CURRENT_TRACKING_NO"].ToString();
                        storeModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
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

        public static StoreModel Edit(long STORE_ID)
        {
            StoreModel storeModel = new StoreModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", STORE_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spStore", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        storeModel.STORE_CODE = sqlDataReader["STORE_CODE"].ToString();
                        storeModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return storeModel;
        }




        public static int EditStoreTrackingNo(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", storeModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", storeModel.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@FROM_TRACKING_NO", storeModel.FROM_TRACKING_NO);
                SqlParameter sp4 = new SqlParameter("@TO_TRACKING_NO", storeModel.TO_TRACKING_NO);
                SqlParameter sp5 = new SqlParameter("@CURRENT_TRACKING_NO", storeModel.CURRENT_TRACKING_NO);                
                SqlParameter sp6 = new SqlParameter("@FLAG", "6");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3, sp4, sp5, sp6);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int SubmitSlot(SlotModel slotModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", slotModel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@SLOT_NAME", slotModel.SLOT_NAME);
                SqlParameter sp3 = new SqlParameter("@SLOT_CODE", slotModel.SLOT_CODE);
                SqlParameter sp4 = new SqlParameter("@DESCRIPTION", slotModel.DESCRIPTION);
                SqlParameter sp5 = new SqlParameter("@CREATED_BY", slotModel.USER_ID);
                SqlParameter sp6 = new SqlParameter("@FLAG", "2");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spSlot", sp1, sp2, sp3, sp4, sp5, sp6);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int UpdateSlot(SlotModel slotModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@SLOT_ID", slotModel.SLOT_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", slotModel.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@SLOT_NAME", slotModel.SLOT_NAME);
                SqlParameter sp4 = new SqlParameter("@SLOT_CODE", slotModel.SLOT_CODE);
                SqlParameter sp5 = new SqlParameter("@DESCRIPTION", slotModel.DESCRIPTION);
                SqlParameter sp6 = new SqlParameter("@MODIFIED_BY", slotModel.USER_ID);
                SqlParameter sp7 = new SqlParameter("@FLAG", "4");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spSlot", sp1, sp2, sp3, sp4, sp5, sp6, sp7);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static SlotModel Modify(long SLOT_ID)
        {
            SlotModel slotModel = new SlotModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@SLOT_ID", SLOT_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spSlot", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        slotModel.SLOT_ID = Convert.ToInt64(sqlDataReader["SLOT_ID"].ToString());
                        slotModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        slotModel.SLOT_NAME = sqlDataReader["SLOT_NAME"].ToString();
                        slotModel.SLOT_CODE = sqlDataReader["SLOT_CODE"].ToString();
                        slotModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return slotModel;
        }

        public static int Remove(SlotModel slotModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@SLOT_ID", slotModel.SLOT_ID);
                SqlParameter sp2 = new SqlParameter("@DELETED_BY", slotModel.USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spSlot", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static DataSet SlotInfo(out List<StoreModel> LstStores, out List<SlotModel> LstSlotes)
        {
            DataSet dataSet = new DataSet();
            LstStores = null; LstSlotes = null;
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spSlot", sp1);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                        {
                            LstStores = (from DataRow dataRow in dataSet.Tables[0].Rows
                                                select new StoreModel()
                                                {
                                                    STORE_ID = Convert.ToInt64(dataRow["STORE_ID"]),
                                                    STORE_NAME = dataRow["STORE_NAME"].ToString()
                                                }).ToList();
                        }
                    }
                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                    {
                        LstSlotes = (from DataRow dataRow in dataSet.Tables[1].Rows
                                            select new SlotModel()
                                            {
                                                SLOT_ID = Convert.ToInt64(dataRow["SLOT_ID"]),
                                                SLOT_NAME = dataRow["SLOT_NAME"].ToString(),
                                                SLOT_CODE = dataRow["SLOT_CODE"].ToString(),
                                                DESCRIPTION = dataRow["DESCRIPTION"].ToString(),
                                                STORE_NAME = dataRow["STORE_NAME"].ToString(),
                                            }).ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataSet;
        }
    }
}