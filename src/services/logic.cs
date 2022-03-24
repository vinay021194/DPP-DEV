using AdProvider.Utility;

using DPP.Logic.Application;

using DPP.Models.DemandAnalysis;

using System;

using System.Collections.Generic;

using System.Data;

using System.Data.Odbc;

using System.Linq;

using System.Text;

using System.Web;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using System.Data.Odbc;

using System.Net.Http;

using System.Net.Http.Headers;

using System.Security.Cryptography.Xml;

using System.IO;

using System.Net;

using System.Text;

using System.Data;

using Newtonsoft.Json.Linq;

using System.Web.Script.Serialization;

using System.Net.Http;

using System.Collections;

using Newtonsoft.Json;

using System.Configuration;

using DPP.Models;

 

namespace DPP.Logic.Business.DemandAnalysis

{

    public class DemandAnalysisLogic

    {

        public static string connectionString = ConfigurationManager.AppSettings.Get("ImpalaConnection");//"DSN=impaladev32";

        public static string SQLconnectionString = ConfigurationManager.AppSettings.Get("SQLConnection");

        public static string DataikuAPIURL = ConfigurationManager.AppSettings.Get("DataikuAPI");//"DataikuAPI";

        public static string SQLASSP = ConfigurationManager.AppSettings.Get("SQLASSP");

 

        public static DPP.Models.DemandAnalysis.MateriaModel GetMaterialDetails(string materialId = null)

        {

            List<DPP.Models.DemandAnalysis.MateriaModel> numberList = new List<DPP.Models.DemandAnalysis.MateriaModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            return db.SP_BD_GetMaterialDetails(materialId).Select(m => new DPP.Models.DemandAnalysis.MateriaModel

                            {

                                materialID = m.material.ToString(),

                                type = m.material_type,

                                Description = m.material_description_1,

                                Group = m.material_group,

                                CLass = m.mdrm_class,

                                Criticality = @DPP.Utility.AppConstants.NullValueRefrence,

                                UOM = m.base_unit_of_measure

                            }).ToList().FirstOrDefault();

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

 

                }

 

 

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT material,material_type,material_description_1,material_group,mdrm_class,base_unit_of_measure

                                            from proc_pp_inter.alternate_material_info where material='" + materialId + "' limit 1";//limit 1

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var m = new DPP.Models.DemandAnalysis.MateriaModel();

                                m.materialID = !string.IsNullOrEmpty(item["material"].ToString()) ? item["material"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.type = !string.IsNullOrEmpty(item["material_type"].ToString()) ? item["material_type"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.Description = !string.IsNullOrEmpty(item["material_description_1"].ToString()) ? item["material_description_1"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.Group = !string.IsNullOrEmpty(item["material_group"].ToString()) ? item["material_group"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.CLass = !string.IsNullOrEmpty(item["mdrm_class"].ToString()) ? item["mdrm_class"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.Criticality = @DPP.Utility.AppConstants.NullValueRefrence;

                                m.UOM = !string.IsNullOrEmpty(item["base_unit_of_measure"].ToString()) ? item["base_unit_of_measure"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                numberList.Add((m));

                            }

                            conn.Close();

                            return numberList.FirstOrDefault();

                        }

                    }

                }

            }

            catch (Exception e)

            {

                Utility.Logging.LogError(e);

                throw;

            }

 

        }

 

        public static List<DPP.Models.DemandAnalysis.InventoryAnalysis> GetInventoryAnalysisLogicDetails(int materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.InventoryAnalysis> numberList = new List<DPP.Models.DemandAnalysis.InventoryAnalysis>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            return db.SP_BD_GetInventoryAnalysisLogicDetails(materialId).Select(m => new DPP.Models.DemandAnalysis.InventoryAnalysis

                            {

                                plant = m.plant,

                                SafetyStock = m.safety_stock,

                                WarehouseCapacity = m.warehouse_capacity,

                                UnristrictedStock = m.opening_stock,

                                DeliveryMonth = @DPP.Utility.AppConstants.NullValueRefrence

 

                            }).ToList();

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                   

                   

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT plant,safety_stock,warehouse_capacity,opening_stock from

proc_pp_inter.inventory_info where material=" + materialId;

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var m = new DPP.Models.DemandAnalysis.InventoryAnalysis();

                                m.plant = !string.IsNullOrEmpty(item["plant"].ToString()) ? item["plant"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.SafetyStock = !string.IsNullOrEmpty(item["safety_stock"].ToString()) ? item["safety_stock"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.WarehouseCapacity = !string.IsNullOrEmpty(item["warehouse_capacity"].ToString()) ? item["warehouse_capacity"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.UnristrictedStock = !string.IsNullOrEmpty(item["opening_stock"].ToString()) ? item["opening_stock"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.DeliveryMonth = @DPP.Utility.AppConstants.NullValueRefrence;

                                numberList.Add((m));

                            }

                        }

                        conn.Close();

                        return numberList;

                    }

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                throw;

            }

 

        }

 

 

        public static List<DPP.Models.DemandAnalysis.PlantModel> GetPlantData(string materialId = null, string plant = null)

        {

            List<DPP.Models.DemandAnalysis.PlantModel> numberList = new List<DPP.Models.DemandAnalysis.PlantModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            return db.SP_BD_GetPlantData(Convert.ToInt64(materialId), plant).Select(m => new DPP.Models.DemandAnalysis.PlantModel

                            {

                                SaftyStock = m.safety_stock,

                                WarehouseCapacity = m.warehouse_capacity,

                                CLosingStock = m.opening_stock

 

                            }).ToList();

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT safety_stock,warehouse_capacity,opening_stock from proc_pp_inter.inventory_info where material= " + materialId + " and plant = '" + plant + "'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var m = new DPP.Models.DemandAnalysis.PlantModel();

                                m.SaftyStock = !string.IsNullOrEmpty(item["safety_stock"].ToString()) ? item["safety_stock"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.WarehouseCapacity = !string.IsNullOrEmpty(item["warehouse_capacity"].ToString()) ? item["warehouse_capacity"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                m.CLosingStock = !string.IsNullOrEmpty(item["opening_stock"].ToString()) ? item["opening_stock"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                numberList.Add((m));

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                throw;

            }

 

        }

 

        public static List<DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel> GetCostOptimizationData(double materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel> numberList = new List<DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            return db.SP_BD_GetCostOptimizationData(Convert.ToInt64(materialId)).Select(m => new DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel

                            {

                                Material = Convert.ToInt64(m.materialcode),

                                Composition = m.item_composition_in_english,

                                Percentage = m.concentration_percentage

 

                            }).ToList();

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT materialcode,item_composition_in_english,concentration_percentage from

                                            proc_pp_inter.material_cost_driver_output where materialcode= " + materialId;//+ " and plant = '" + plant + "'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var m = new DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel();

                                m.Material = Convert.ToDouble(item["materialcode"].ToString());

                                m.Composition = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item["item_composition_in_english"].ToString().ToLower());

                                m.Percentage = !string.IsNullOrEmpty(item["concentration_percentage"].ToString()) ? item["concentration_percentage"].ToString() : @DPP.Utility.AppConstants.NullValueRefrence;

                                numberList.Add((m));

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                throw;

            }

 

        }

 

        public static List<DPP.Models.DemandAnalysis.PeriodModel> GetPeriodData(string materialId = null, string plant = null)

        {

            List<DPP.Models.DemandAnalysis.PeriodModel> numberList = new List<DPP.Models.DemandAnalysis.PeriodModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    connectionString = SQLconnectionString;

                }

 

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT * from proc_pp_inter.demand_ui_table where Material=" + materialId + " and plant='" + plant + "'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText =OpenQueryMSSQL(cmd);

                       

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

 

                            if (dataTable != null && dataTable.Rows.Count > 0)

                            {

                                foreach (DataRow item in dataTable.Rows)

                                {

                                    var m = new DPP.Models.DemandAnalysis.PeriodModel();

 

                                    m.Month1 = !string.IsNullOrEmpty(item[DateTime.Now.ToString("yyyy-MM-01")].ToString())

                                        ? Convert.ToDouble(Math.Round(Convert.ToDecimal(item[DateTime.Now.ToString("yyyy-MM-01").ToString()]), 2)) : 0.0;

 

                                    m.Month2 = !string.IsNullOrEmpty(item[DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")].ToString())

                                        ? Convert.ToDouble(Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")]), 2)) : 0.0;

 

                                    m.Month3 = !string.IsNullOrEmpty(item[DateTime.Now.AddMonths(2).ToString("yyyy-MM-01")].ToString()) ?

                                        Convert.ToDouble(Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(2).ToString("yyyy-MM-01")]), 2)) : 0.0;

 

                                    m.Month4 = !string.IsNullOrEmpty(item[DateTime.Now.AddMonths(3).ToString("yyyy-MM-01")].ToString()) ?

                                        Convert.ToDouble(Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(3).ToString("yyyy-MM-01")]), 2)) : 0.0;

 

                                    m.Month5 = !string.IsNullOrEmpty(item[DateTime.Now.AddMonths(4).ToString("yyyy-MM-01")].ToString()) ?

                                        Convert.ToDouble(Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(4).ToString("yyyy-MM-01")]), 2)) : 0.0;

 

                                    m.Month6 = !string.IsNullOrEmpty(item[DateTime.Now.AddMonths(5).ToString("yyyy-MM-01")].ToString()) ?

                                        Convert.ToDouble(Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(5).ToString("yyyy-MM-01")].ToString()), 2)) : 0.0;

 

                                    m.Period = "Forecasted Demand";

                                    numberList.Add((m));

                                }

                            }

                            else

                            {

                                var p = new DPP.Models.DemandAnalysis.PeriodModel();

                                p.Month1 = 0.0;

                                p.Month2 = 0.0;

                                p.Month3 = 0.0;

                                p.Month4 = 0.0;

                                p.Month5 = 0.0;

                                p.Month6 = 0.0;

                                p.Period = "Forecasted Demand";

                                numberList.Add((p));

                            }

                        }

                        cmd.CommandText = @"SELECT * from proc_pp_inter.on_route_info where Material=" + materialId + " and plant='" + plant + "'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var p = new DPP.Models.DemandAnalysis.PeriodModel();

                            var now = DateTime.Now.AddMonths(-1);

                            var Months = Enumerable.Range(1, 6).Select(i => now.AddMonths(+i).ToString("MM"));

                            var myArray = Months.ToArray();

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            if (dataTable != null && dataTable.Rows.Count > 0)

                            {

                                var data = dataTable.Select("month=" + myArray[0]);

                                p.Month1 = (data != null) ? Convert.ToDouble(data[0].ItemArray[4]) : 0.0;

 

                                var data1 = dataTable.Select("month=" + myArray[1]);

                                p.Month2 = (data1 != null) ? Convert.ToDouble(data1[0].ItemArray[4]) : 0.0;

 

                                var data2 = dataTable.Select("month=" + myArray[2]);

                                p.Month3 = (data2 != null) ? Convert.ToDouble(data2[0].ItemArray[4]) : 0.0;

 

                                var data3 = dataTable.Select("month=" + myArray[3]);

                                p.Month4 = (data3 != null) ? Convert.ToDouble(data3[0].ItemArray[4]) : 0.0;

 

                                var data4 = dataTable.Select("month=" + myArray[4]);

                                p.Month5 = (data4 != null) ? Convert.ToDouble(data4[0].ItemArray[4]) : 0.0;

 

                                var data5 = dataTable.Select("month=" + myArray[5]);

                                p.Month6 = (data5 != null) ? Convert.ToDouble(data5[0].ItemArray[4]) : 0.0;

 

                                p.Period = "On Route";

                                numberList.Add((p));

                            }

                            else

                            {

                                p.Month1 = 0.0;

                                p.Month2 = 0.0;

                                p.Month3 = 0.0;

                                p.Month4 = 0.0;

                                p.Month5 = 0.0;

                                p.Month6 = 0.0;

                                p.Period = "On Route";

                                numberList.Add((p));

                            }

 

                        }

                        return numberList;

 

                    }

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                throw;

            }

 

        }

 

        private static string OpenQueryMSSQL(OdbcCommand cmd)

        {

            string doublecomma = "''";

            cmd.CommandText = cmd.CommandText.Replace("'", doublecomma);

            return  string.Format(@"SELECT * FROM OPENQUERY(Impala,'{0}')", cmd.CommandText);

 

        }

 

        public static List<DPP.Models.DemandAnalysis.CostDriverOutputModel> GetCostDriverOutput(List<costSeriesModel> seriesName)

        {

            List<DPP.Models.DemandAnalysis.CostDriverOutputModel> numberList = new List<DPP.Models.DemandAnalysis.CostDriverOutputModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    connectionString = SQLconnectionString;

                }

                using (var conn = new OdbcConnection(connectionString))

               {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        if (seriesName != null && seriesName.Count > 0)

                        {

                            foreach (var seriesdata in seriesName)

                            {

 

                                var series = seriesdata.series;

                                var material = seriesdata.costDriver;

                                cmd.CommandText = @"SELECT first_month_accuracy, second_month_accuracy, third_month_accuracy,

                                fourth_month_accuracy, fifth_month_accuracy,sixth_month_accuracy, test_accuracy,* FROM proc_pp_inter.icis_forecast_error_info_updated

                                where material ='" + material + "' and series = '" + series + "' ORDER BY executed_on desc limit 1";

                                //"SELECT * from proc_pp_inter.demand_ui_table where Material=" + materialId + " and plant='" + plant + "'";

                                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                                    cmd.CommandText = OpenQueryMSSQL(cmd);

                                using (OdbcDataReader reader = cmd.ExecuteReader())

                                {

                                    var dataTable = new DataTable();

                                    dataTable.Load(reader);

                                    foreach (DataRow item in dataTable.Rows)

                                    {

                                        var m = new DPP.Models.DemandAnalysis.CostDriverOutputModel();

 

                                        m.Month1 = !string.IsNullOrEmpty(item["first_month_accuracy"].ToString())

                                            ? Convert.ToDouble(Math.Round(Convert.ToDecimal(item["first_month_accuracy"]), 2)) : 0.0;

 

                                        m.Month2 = !string.IsNullOrEmpty(item["second_month_accuracy"].ToString())

                                            ? Convert.ToDouble(Math.Round(Convert.ToDecimal(item["second_month_accuracy"]), 2)) : 0.0;

 

                                        m.Month3 = !string.IsNullOrEmpty(item["third_month_accuracy"].ToString()) ?

                                            Convert.ToDouble(Math.Round(Convert.ToDecimal(item["third_month_accuracy"]), 2)) : 0.0;

 

                                        m.Month4 = !string.IsNullOrEmpty(item["fourth_month_accuracy"].ToString()) ?

                                            Convert.ToDouble(Math.Round(Convert.ToDecimal(item["fourth_month_accuracy"]), 2)) : 0.0;

 

                                        m.Month5 = !string.IsNullOrEmpty(item["fifth_month_accuracy"].ToString()) ?

                                            Convert.ToDouble(Math.Round(Convert.ToDecimal(item["fifth_month_accuracy"]), 2)) : 0.0;

 

                                        m.Month6 = !string.IsNullOrEmpty(item["sixth_month_accuracy"].ToString()) ?

                                            Convert.ToDouble(Math.Round(Convert.ToDecimal(item["sixth_month_accuracy"].ToString()), 2)) : 0.0;

 

 

                                        m.CostDriver = material;

                                        m.series = series;

 

 

                                        numberList.Add((m));

                                    }

                                }

 

                            }

                        }

                        else

                        {

                            var p = new DPP.Models.DemandAnalysis.CostDriverOutputModel();

                            p.Month1 = 0.0;

                            p.Month2 = 0.0;

                            p.Month3 = 0.0;

                            p.Month4 = 0.0;

                            p.Month5 = 0.0;

                            p.Month6 = 0.0;

                            p.CostDriver = @DPP.Utility.AppConstants.NullValueRefrence;

                            p.series = @DPP.Utility.AppConstants.NullValueRefrence;

                            numberList.Add((p));

                        }

                    }

 

                    return numberList;

                }

 

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                throw;

            }

 

        }

 

 

        public static List<DPP.Models.DemandAnalysis.DemandModel> GetDemandPredicitonDetails(double materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.DemandModel> numberList = new List<DPP.Models.DemandAnalysis.DemandModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                   connectionString = SQLconnectionString;

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT * from proc_pp_inter.demand_ui_table where material = " + materialId;

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var m = new DPP.Models.DemandAnalysis.DemandModel();

                                m.Plant = item["plant"].ToString();

                                if (!string.IsNullOrEmpty(item["prediction_error"].ToString()))

                                {

                                    m.predictionerror = Math.Round(100 - (double)Math.Round(Convert.ToDecimal(item["prediction_error"].ToString()), 2), 2);

                                }

                                m.AvgCon = item["avg_total_consumption"].ToString();

                                m.Nov = (double)Math.Round(Convert.ToDecimal(item[DateTime.Now.ToString("yyyy-MM-01")].ToString()), 2);

                                m.Dec = (double)Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")].ToString()), 2);

                                m.JAN = (double)Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(2).ToString("yyyy-MM-01")].ToString()), 2);

                                m.FEB = (double)Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(3).ToString("yyyy-MM-01")].ToString()), 2);

                                m.MARCH = (double)Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(4).ToString("yyyy-MM-01")].ToString()), 2);

                                m.APRIL = (double)Math.Round(Convert.ToDecimal(item[DateTime.Now.AddMonths(5).ToString("yyyy-MM-01")].ToString()), 2);

 

                                numberList.Add((m));

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                throw;

            }

 

        }

 

 

 

 

        public static List<String> GetDropDownData(string materialId)

        {

            List<String> numberList = new List<String>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            var GetDropDownDatalist = db.SP_BD_GetDropDownData(Convert.ToInt64(materialId)).ToList();

                            foreach (var item in GetDropDownDatalist)

                            {

                                numberList.Add((String)(item.material));

                            }

                            return numberList;

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT  distinct top 10 material FROM proc_pp_inter.alternate_material_info where material like '%" + materialId + "%'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            while (reader.Read())

                            {

                                numberList.Add((String)(reader["material"]));

                            }

                        }

 

                    }

                    conn.Close();

                    return numberList;

                }

            }

           catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<String> GetDropDownPlantData(string plant)

        {

            List<String> numberList = new List<String>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            var GetDropDownDatalist = db.SP_BD_GetDropDownPlantData(plant);

                            numberList.Add(GetDropDownDatalist.ToString());

                            return numberList;

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT distinct top 10 plant from proc_pp_inter.inventory_info where plant like '%" + plant + "%'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            while (reader.Read())

                            {

                                numberList.Add((String)(reader["plant"]));

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<string> GetCostDriverDropDown(string material_name)

        {

            List<String> numberList = new List<String>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            var GetCostDriverDropDownlist = db.SP_BD_GetCostDriverDropDown(material_name).ToList();

                            foreach (var item in GetCostDriverDropDownlist)

                           {

                                numberList.Add((String)(item.material_name));

                            }

                            return numberList;

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT distinct top 20 material_name FROM proc_pp_inter.icis_forecast_sid where

                                        LOWER(material_name) like LOWER('" + material_name + "%')";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                numberList.Add(item["material_name"].ToString());

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<string> GetSeriesNameData(string material_name)

        {

            List<String> numberList = new List<String>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            var GetCostDriverDropDownlist = db.SP_BD_GetCostDriverDropDown(material_name).ToList();

                            foreach (var item in GetCostDriverDropDownlist)

                            {

                                numberList.Add((String)(item.material_name));

                            }

                            return numberList;

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT distinct top 10 serial_name FROM proc_pp_inter.icis_forecast_sid

                                    where material_name = '" + material_name + "'";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                numberList.Add(item["serial_name"].ToString());

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<supplierChartModel2> costDriverAnalysisGraphdata(List<String> materalName , List<String> seriesName)

        {

            List<supplierChartModel2> result = new List<supplierChartModel2>();

            try

            {

                var sqlQuery = "SELECT * FROM proc_pp_inter.icis_forecast_sid,proc_pp_inter.icis_forecast_error_info_updated where ";

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        {

                            if (materalName != null)

                            {

                                Console.WriteLine(materalName[0]);

                                sqlQuery = sqlQuery + "material in (";

                                var materalNamelastelement = materalName[materalName.Count - 1];

                                foreach (var materials in materalName)

                                {

                                    Console.WriteLine("inside foreach");

                                    if (materials == materalNamelastelement)

                                    {

                                        sqlQuery = sqlQuery + "'" + materials + "')";

                                    }

                                    else

                                    {

                                        sqlQuery = sqlQuery + "'" + materials + "',";

                                    }

                                }

                            }

                            if (seriesName != null)

                            {

                                sqlQuery = sqlQuery + " and series in (";

                                var seriesNamelastelement = seriesName[seriesName.Count - 1];

                                foreach (var materials in seriesName)

                                {

                                    if (materials == seriesNamelastelement)

                                    {

                                        sqlQuery = sqlQuery + "'" + materials + "')";

                                    }

                                    else

                                    {

                                        sqlQuery = sqlQuery + "'" + materials + "',";

 

                                    }

                                }

                            }

                        }

                        cmd.CommandText = sqlQuery;

                           if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            var Maxvalue = 0.0;

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var chartmodel = new supplierChartModel2();

                                var currentdate = DateTime.Parse(item["posting_date"].ToString());

                                var currentMonth = currentdate.ToString("MMM-yy");

                                chartmodel.Categories = currentMonth;

                                chartmodel.avrageResult = Convert.ToDouble(item["unit_price_usd"].ToString());

                                result.Add(chartmodel);

                            }

                            Maxvalue = result.Max(m => m.avrageResult);

                        }

                    }

                    conn.Close();

                    return result;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<string> GetCostDrivermultiDropDown()

        {

            List<String> numberList = new List<String>();

            try

            {

 

                using (var conn = new OdbcConnection(connectionString))

                {

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = @"SELECT distinct top 20 material_name FROM proc_pp_inter.icis_forecast_sid";

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                numberList.Add(item["material_name"].ToString());

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<string> GetSeriesNameMultiData(List<String> material_name)

        {

            List<String> numberList = new List<String>();

            try

            {

                var sqlQuery = "SELECT distinct top 50 serial_name FROM proc_pp_inter.icis_forecast_sid where ";

                if (material_name != null)

                {

                    sqlQuery = sqlQuery + " material_name in (";

                    var lastElement = material_name[material_name.Count - 1];

                    foreach (var mat in material_name)

                    {

                        if (mat == lastElement)

                        {

                            sqlQuery = sqlQuery + "'" + mat + "')";

                        }

                        else

                        {

                            sqlQuery = sqlQuery + "'" + mat + "',";

 

                        }

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

 

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = sqlQuery;

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                numberList.Add(item["serial_name"].ToString());

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

 

        public static List<string> GetCountryNameMultiData(String materialId, String plantid)

        {

            List<String> numberList = new List<String>();

            try

            {

                var sqlQuery = "select distinct top 50 vendor_country from proc_pp_inter.vendor_mapping_summary_table " +

                    "where material='"+ materialId + "' and plant= '"+ plantid  + "'";

              

                using (var conn = new OdbcConnection(connectionString))

                {

 

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = sqlQuery;

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                       {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                numberList.Add(item["vendor_country"].ToString());

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

 

        public static List<string> GetVendorNameMultiData(String materialId, String plantid)

        {

            List<String> numberList = new List<String>();

            try

            {

                var sqlQuery = "select distinct top 50 vendor_code from proc_pp_inter.vendor_mapping_summary_table " +

                    "where material='" + materialId + "' and plant= '" + plantid + "'";

 

                using (var conn = new OdbcConnection(connectionString))

                {

 

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = sqlQuery;

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            foreach (DataRow item in dataTable.Rows)

                            {

                                numberList.Add(item["vendor_code"].ToString());

                            }

                        }

                    }

                    conn.Close();

                    return numberList;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

        public static List<alertAnalysisChart> getAlertAnalysisData(string materia_name, string series_name,

            DateTime startingDate, DateTime endDate, string timeInterwal)

        {

            List<alertAnalysisChart> result = new List<alertAnalysisChart>();

            var sDate = startingDate.ToString("yyyy-MM-dd HH:mm:ss");

            var eDate = endDate.ToString("yyyy-MM-dd HH:mm:ss");

 

            using (var conn = new OdbcConnection(connectionString))

            {

                conn.Open();

                using (var cmd = conn.CreateCommand())

                {

                    cmd.CommandText = @"SELECT actual_values,weekly_returns,percentile_of_monthly_returns,percentile_of_weekly_returns,

                    monthly_returns,`date` FROM proc_pp_inter.icis_alert_info where material_name = '"+ materia_name +"' and " +

                    "serial_name = '"+ series_name + "' and  `date`>'"+ sDate + "' and `date`< '"+ eDate + "' ";

                    if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                        cmd.CommandText = OpenQueryMSSQL(cmd);

                    using (OdbcDataReader reader = cmd.ExecuteReader())

                    {

                        var dataTable = new DataTable();

                        dataTable.Load(reader);

                        foreach (DataRow item in dataTable.Rows)

                        {

                            var chartmodel = new alertAnalysisChart();

                            var currentdate = DateTime.Parse(item["date"].ToString());

                            var currentMonth = currentdate.ToString("yyyy,MM,dd");

                            //chartmodel.Categories = currentMonth;(2010,01,11)

                            chartmodel.dayofdate = currentMonth;

                            chartmodel.actualValues = !string.IsNullOrEmpty(item["actual_values"].ToString())?

                                Convert.ToDouble(item["actual_values"].ToString()): 0;

                            chartmodel.weeklyPercentageReturn = !   string.IsNullOrEmpty(item["percentile_of_weekly_returns"].ToString())?

                                Convert.ToDouble(item["percentile_of_weekly_returns"].ToString()):0;

                            chartmodel.monthlyPercentageReturn = !string.IsNullOrEmpty(item["percentile_of_monthly_returns"].ToString()) ?

                                Convert.ToDouble(item["percentile_of_monthly_returns"].ToString()) : 0;

                            chartmodel.weeklyReturn = !string.IsNullOrEmpty(item["weekly_returns"].ToString()) ?

                                Convert.ToDouble(item["weekly_returns"].ToString()):0;

                            chartmodel.monthly_returns = !string.IsNullOrEmpty(item["monthly_returns"].ToString())?

                                Convert.ToDouble(item["monthly_returns"].ToString()):0;

                            result.Add(chartmodel);

                        }

                    }

                }

                conn.Close();

                return result;

            }

        }

        public static List<supplierChartModel2> GetMaterialChartData(string material_name, string plant, string vendor,string country)

        {

            List<supplierChartModel2> result = new List<supplierChartModel2>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    var Maxvalue = 0.0;

                    if (ConfigurationManager.AppSettings["SQLASSP"].Contains("True"))

                    {

                        using (var db = new ProjectDBDataContext())

                        {

                            var GetCostDriverDropDownlist = db.SP_BD_GetMaterialChartData(material_name, plant).Select(m => new DPP.Models.DemandAnalysis.supplierChartModel2

                            {

                                Categories = DateTime.Parse(m.posting_date.ToString()).ToString("MMM-yy"),

                                avrageResult = Convert.ToDouble(m.unit_price_usd)

 

                            }).ToList();

                            Maxvalue = result.Max(m => m.avrageResult);

                            return GetCostDriverDropDownlist;

                        }

                    }

                    else

                    {

                        connectionString = SQLconnectionString;

                    }

                }

                using (var conn = new OdbcConnection(connectionString))

                {

                    var sqlQuery = "";

                    if (vendor != "" && country != "") {

                         sqlQuery =

                        "select month(posting_date) as months,avg(unit_price_usd) as price from proc_pp_inter.vendor_mapping_summary_table " +

                         "  where  material = '" + material_name + "' and plant = '" + plant + "'"+

                         "and posting_date <= from_unixtime(unix_timestamp()) and posting_date > add_months(from_unixtime(unix_timestamp()), -12)" +

                         " group by months order by months desc";      

                         }

                    conn.Open();

                    using (var cmd = conn.CreateCommand())

                    {

                        cmd.CommandText = sqlQuery;

                        if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                            cmd.CommandText = OpenQueryMSSQL(cmd);

                        using (OdbcDataReader reader = cmd.ExecuteReader())

                        {

                            var dataTable = new DataTable();

                            dataTable.Load(reader);

                            var Maxvalue = 0.0;

                            foreach (DataRow item in dataTable.Rows)

                            {

                                var chartmodel = new supplierChartModel2();

                                var currentdate = DateTime.Today;

                                var currentMonth = currentdate.ToString("yyyy");

                                chartmodel.Categories = (item["months"].ToString())+"-"+ currentMonth;

                                chartmodel.avrageResult = Math.Round(Convert.ToDouble(item["price"].ToString()),2);

                                result.Add(chartmodel);

                            }

                            Maxvalue = result.Max(m => m.avrageResult);

                        }

                    }

                    conn.Close();

                    return result;

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

            }

        }

 

 

 

 

 

        public static List<SupplierModel> GetSupplierDataModel(List<SupplierModel> datalistSupplier,

            List<costSeriesModel> seriesName)

 

        {

            List<SupplierModel> SupplierList = new List<SupplierModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    connectionString = SQLconnectionString;

                }

                if (datalistSupplier != null)

                {

                    foreach (var item in datalistSupplier)//for supplier

                    {

                        if (item.Formula.All(char.IsDigit))//check if it is digit

                        {

                            var supplierInformation = new SupplierModel

                            {

                                Name = item.Name,

                                OptimizeName = "Forecasted Price",

                                Nov = Convert.ToDouble(item.Formula),

                                Dec = Convert.ToDouble(item.Formula),

                                JAN = Convert.ToDouble(item.Formula),

                                FEB = Convert.ToDouble(item.Formula),

                                MARCH = Convert.ToDouble(item.Formula),

                                APRIL = Convert.ToDouble(item.Formula),

                            };

                            var leatInformation = new SupplierModel

                            {

                                Name = item.Name,

                                OptimizeName = "Lead Time",

                                Nov = Convert.ToDouble(item.Time),

                                Dec = Convert.ToDouble(item.Time),

                                JAN = Convert.ToDouble(item.Time),

                                FEB = Convert.ToDouble(item.Time),

                                MARCH = Convert.ToDouble(item.Time),

                                APRIL = Convert.ToDouble(item.Time)

                            };

                            var CapacityInformation = new SupplierModel

                            {

                                Name = item.Name,

                                OptimizeName = "Supplier Max. Capacity",

                                Nov = Convert.ToDouble(item.Capacity),

                                Dec = Convert.ToDouble(item.Capacity),

                                JAN = Convert.ToDouble(item.Capacity),

                                FEB = Convert.ToDouble(item.Capacity),

                                MARCH = Convert.ToDouble(item.Capacity),

                                APRIL = Convert.ToDouble(item.Capacity)

                            };

                            SupplierList.Add(supplierInformation);

                            SupplierList.Add(leatInformation);

                            SupplierList.Add(CapacityInformation);

 

                        }

                        else

                        { //   2*[1]+[2]

                            Dictionary<string, string> FormulaList = new Dictionary<string, string>();

                            List<Double> finalResult = new List<Double>();

                            var index = item.Formula;

                            var FormulaForIndex = index;

                            char search = '[';

                            var result = index.Select((b, i) => b.Equals(search) ? i : -1).Where(i => i != -1).ToList();

                            foreach (var resultrow in result)//take out the index from formula

                            {

                                var indexdata = resultrow;

                                indexdata = indexdata + 1;

                                var s = Convert.ToInt32(index.Substring(indexdata, 1));

                                s = s - 1;

                                var seriesdata = seriesName[s];

                                var series = seriesdata.series;

                                var material = seriesdata.costDriver;

                                try

                                {

                                    using (var conn = new OdbcConnection(connectionString))

                                    {

                                        conn.Open();

                                        using (var cmd = conn.CreateCommand())/// material -1 series -3

                                        {

 

                                            cmd.CommandText = @"SELECT * from proc_pp_inter.icis_forecast_sid  where material_name = '" + material + "' and" +

                                                " serial_name ='" + series + "' and " +

                                                " icis_forecast_sid.`date` >= from_unixtime(unix_timestamp()) and icis_forecast_sid.`date` < add_months(from_unixtime(unix_timestamp()), 6)";

                                            if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                                                cmd.CommandText = OpenQueryMSSQL(cmd);

                                            using (OdbcDataReader reader = cmd.ExecuteReader())

                                            {

                                                List<String> months = new List<string>();

                                                var dataTable = new DataTable();

                                                dataTable.Load(reader);

                                                DataTable dest = new DataTable();

                                                foreach (DataRow itemdata in dataTable.Rows)//Take data for material and serial

                                                {

                                                    var currentdate = DateTime.Parse(itemdata["date"].ToString());

                                                    var currentMonth = currentdate.ToString("MMM");

                                                    var month = months

                                                                .FirstOrDefault(stringToCheck => stringToCheck.Contains(currentMonth));

                                                    if (month == null)

                                                    {

                                                        months.Add(currentMonth);

                                                        var currentCount = dataTable.AsEnumerable()//Take the month

                                                                                .Where(r => r.Field<DateTime>("Date").ToString("MMM") ==

                                                                                (currentdate.ToString("MMM"))).Count();

                                                        DataTable innertable = dataTable.AsEnumerable()//Take data for particular month

                                                                                    .Where(r => r.Field<DateTime>("Date").ToString("MMM") == (currentdate.ToString("MMM"))).CopyToDataTable();

                                                        var sum = 0.0;

                                                        foreach (DataRow itemrow in innertable.Rows)//Data for a particular month

                                                        {

                                                            var value = itemrow["actual_values"].ToString();

                                                            if (!string.IsNullOrEmpty(value))

                                                            {

                                                                sum = sum + Convert.ToDouble(itemrow["actual_values"].ToString());

                                                            }

                                                            else

                                                            {

                                                                sum = sum + Convert.ToDouble(itemrow["predicted_values"].ToString());

                                                            }

                                                        }

                                                        var totalsum = sum;

                                                        var avrage = sum / currentCount;

                                                        if (FormulaList.ContainsKey(currentMonth))//if list have current month

                                                        {

                                                            var currentmonthindex = FormulaList[currentMonth];

                                                            var aStringBuilder = new StringBuilder(currentmonthindex);

                                                            var newIndexPosition = currentmonthindex.IndexOf(search);

                                                            aStringBuilder.Remove(newIndexPosition, 3);

                                                            aStringBuilder.Insert(newIndexPosition, avrage);

                                                            currentmonthindex = aStringBuilder.ToString();

                                                            FormulaList[currentMonth] = currentmonthindex;

                                                        }

                                                        else//If list dont have current month

                                                        {

                                                            var aStringBuilder = new StringBuilder(index);

                                                            aStringBuilder.Remove(resultrow, 3);

                                                            aStringBuilder.Insert(resultrow, avrage);

                                                            var newindex = aStringBuilder.ToString();

                                                            FormulaList.Add(currentMonth, newindex);

                                                        }

                                                    }

                                                }

                                            }

                                        }

                                        conn.Close();

                                    }

                                }

                                catch (Exception e)

                                {

                                    Logging.LogError(e);

                                    return null;

 

                                }

                            }

                            foreach (var formulalists in FormulaList.ToArray())

                            {

                                var finalValue = formulalists.Value.Trim();

                                DataTable dt = new DataTable();//calculate formula

                                var v = dt.Compute(finalValue, "");

                                v = Convert.ToDouble((String.Format("{0:n0}", v)));

                                finalResult.Add(Math.Round((Convert.ToDouble(v)), 2));

                            }

                            var supplierInformation = new SupplierModel

                            {

                                Name = item.Name,

                                OptimizeName = "Forecasted Price",

                                Nov = finalResult[0],

                                Dec = finalResult[1],

                                JAN = finalResult[2],

                                FEB = finalResult[3],

                                MARCH = finalResult[4],

                                APRIL = finalResult[5]

                            };

                            var leatInformation = new SupplierModel

                            {

                                Name = item.Name,

                                OptimizeName = "Lead Time",

                                Nov = Convert.ToDouble(item.Time),

                                Dec = Convert.ToDouble(item.Time),

                                JAN = Convert.ToDouble(item.Time),

                                FEB = Convert.ToDouble(item.Time),

                                MARCH = Convert.ToDouble(item.Time),

                                APRIL = Convert.ToDouble(item.Time)

                            };

                            var CapacityInformation = new SupplierModel

                            {

                                Name = item.Name,

                                OptimizeName = "Supplier Max. Capacity",

                                //OptimizeName = "Capacity",

                                Nov = Convert.ToDouble(item.Capacity),

                               Dec = Convert.ToDouble(item.Capacity),

                                JAN = Convert.ToDouble(item.Capacity),

                                FEB = Convert.ToDouble(item.Capacity),

                                MARCH = Convert.ToDouble(item.Capacity),

                                APRIL = Convert.ToDouble(item.Capacity)

                            };

                            SupplierList.Add(supplierInformation);

                            SupplierList.Add(leatInformation);

                            SupplierList.Add(CapacityInformation);

 

 

                        }

                    }

                }

                return SupplierList;

 

            }

            catch (Exception ex)

            {

                Logging.LogError(ex);

                return null;

 

 

            }

        }

 

        public static string GetPredictedData(List<SupplierDataModel> supplierDetailedDataModels, List<PlantModel> plantdata,

           List<PeriodModel> PeriodList1, List<PeriodModel> predictedConsumtion2) //UserSearchDetails model

        {

            string result = null;

            List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

            try

            {

                if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

                {

                    connectionString = SQLconnectionString;

                }

                List<SupplierDataModel> datalist = supplierDetailedDataModels;

                var Plantdata = plantdata;

                List<PeriodModel> PeriodList = PeriodList1;

                string[] months = new String[6];

                Double[] routes = new Double[6];

                if (PeriodList.Any())

                {

                    routes[0] = PeriodList[0].Month1;

                    routes[1] = PeriodList[0].Month2;

                    routes[2] = PeriodList[0].Month3;

                    routes[3] = PeriodList[0].Month4;

                    routes[4] = PeriodList[0].Month5;

                    routes[5] = PeriodList[0].Month6;

                }

                else

                {

                    routes[0] = 0.0;

                    routes[1] = 0.0;

                    routes[2] = 0.0;

                    routes[3] = 0.0;

                    routes[4] = 0.0;

                    routes[5] = 0.0;

                }

                for (int i = -1; i < 5; i++)

                {

                    var m = (DateTime.Now.AddMonths(i).ToString("MMMMMMMM"));

                    months[i + 1] = (m);

                }

                var predictedConsumtion = predictedConsumtion2;

                double[] predicteddata = new Double[6];

                if (predictedConsumtion.Any())

                {

                    var predictedConsumtion1 = predictedConsumtion.ToArray();

                    predicteddata[0] = predictedConsumtion1[0].Month1;

                    predicteddata[1] = predictedConsumtion1[0].Month2;

                    predicteddata[2] = predictedConsumtion1[0].Month3;

                    predicteddata[3] = predictedConsumtion1[0].Month4;

                    predicteddata[4] = predictedConsumtion1[0].Month5;

                    predicteddata[5] = predictedConsumtion1[0].Month6;

                }

                else

                {

                    predicteddata[0] = 0.0;

                    predicteddata[1] = 0.0;

                    predicteddata[2] = 0.0;

                    predicteddata[3] = 0.0;

                    predicteddata[4] = 0.0;

                    predicteddata[5] = 0.0;

                }

                var finaldata = new DPP.Models.DemandAnalysis.FinalDataModel()

                {

                    Plant_data = new Plantdata()

                    {

                        Period = months.ToArray(),

                        WarehouseCapacity = Convert.ToDouble(Plantdata[0].WarehouseCapacity),

                        SafetyStock = Convert.ToDouble(Plantdata[0].SaftyStock),

                        predicted_consumption_mp_level = predicteddata,

                        OpeningStock = Convert.ToDouble(Plantdata[0].CLosingStock),

                        on_route = routes.ToArray(),

                    },

                    Suppliers = new Supplier()

                    {

                        Spplier_Name = new Spplier_Name()

                        {

                            Name = datalist,

                        }

                    }

                };

                string result1 = JsonConvert.SerializeObject(finaldata);

                var jsoninput = result1.ToString();

                string urlAddress = DataikuAPIURL + jsoninput;

                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(urlAddress);

                HttpWebResponse response = (HttpWebResponse)request1.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)

                {

                    Stream receiveStream = response.GetResponseStream();

                    StreamReader readStream = null;

                    if (response.CharacterSet == null)

                        readStream = new StreamReader(receiveStream);

                    else

                        readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    result = readStream.ReadToEnd();

                    readStream.Close();

                    return result;

                }

                else

                {

                    return null;

                }

            }

            catch (Exception ex)

            {

                Logging.LogError(ex);

                return null;

 

            }

        }

        public static List<SupplierModel> GetFinalResultData(List<SupplierDataModel> supplierDetailedDataModels, List<PlantModel> plantdata,

            List<PeriodModel> PeriodList1, List<PeriodModel> predictedConsumtion2) //UserSearchDetails model

        {

            List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

            try

            {

                var result = GetPredictedData(supplierDetailedDataModels, plantdata, PeriodList1, predictedConsumtion2);

                JavaScriptSerializer jjs = new JavaScriptSerializer();

                var jsonresult = jjs.Deserialize<dynamic>(result);

                var output = jsonresult["response"][1];

                var doutput = jjs.Deserialize<dynamic>(output);

                var i = 0;

                foreach (var item in doutput)

                {

                    if (i != 0)

                    {

                        var m = new DPP.Models.DemandAnalysis.SupplierModel();

                        var outputarray = item.Value;

                        m.Name = item.Key;

                        m.Dec = Math.Round(Convert.ToDouble(outputarray["0"].ToString()), 2);

                        m.JAN = Math.Round(Convert.ToDouble(outputarray["1"].ToString()), 2);

                        m.FEB = Math.Round(Convert.ToDouble(outputarray["2"].ToString()), 2);

                        m.MARCH = Math.Round(Convert.ToDouble(outputarray["3"].ToString()), 2);

                        m.APRIL = Math.Round(Convert.ToDouble(outputarray["4"].ToString()), 2);

                        m.TotalSum = Math.Round((m.JAN + m.Dec + m.FEB + m.MARCH + m.APRIL), 2);

                        objDisclaimerDetails.Add(m);

                    }

                    i++;

                }

 

                return objDisclaimerDetails;

            }

            catch (Exception ex)

            {

                Logging.LogError(ex);

                return null;

 

            }

        }

 

 

 

        public static List<SupplierDataModel2> GetFinalResultData2(List<SupplierDataModel> supplierDetailedDataModels, List<PlantModel> plantdata,

           List<PeriodModel> PeriodList1, List<PeriodModel> predictedConsumtion2) //UserSearchDetails model

       {

            List<SupplierDataModel2> objDisclaimerDetails = new List<SupplierDataModel2>();

            try

            {

                var result = GetPredictedData(supplierDetailedDataModels, plantdata, PeriodList1, predictedConsumtion2);

                JavaScriptSerializer jjs = new JavaScriptSerializer();

                var d = jjs.Deserialize<dynamic>(result);

                var output = d["response"][0];

                var doutput = jjs.Deserialize<dynamic>(output);

                var i = 0;

                foreach (var item in doutput)

                {

 

                    var m = new DPP.Models.DemandAnalysis.SupplierDataModel2();

                    var outputarray = item.Value;

                    if (item.Key != "Period")

                    {

                        m.name = item.Key;

                        m.month1 = Math.Round(Convert.ToDouble(outputarray["0"].ToString()), 2);

                        m.month2 = Math.Round(Convert.ToDouble(outputarray["1"].ToString()), 2);

                        m.month3 = Math.Round(Convert.ToDouble(outputarray["2"].ToString()), 2);

                        m.month4 = Math.Round(Convert.ToDouble(outputarray["3"].ToString()), 2);

                        m.month5 = Math.Round(Convert.ToDouble(outputarray["4"].ToString()), 2);

                        objDisclaimerDetails.Add(m);

                    }

                }

                return objDisclaimerDetails;

            }

            catch (Exception ex)

            {

                Logging.LogError(ex);

                return null;

 

            }

        }

 

 

        public static List<SupplierDataModel3> GetFinalResultData3(List<SupplierDataModel> supplierDetailedDataModels, List<PlantModel> plantdata,

            List<PeriodModel> PeriodList1, List<PeriodModel> predictedConsumtion2) //UserSearchDetails model

        {

            List<SupplierDataModel3> objDisclaimerDetails = new List<SupplierDataModel3>();

            try

            {

                var result = GetPredictedData(supplierDetailedDataModels, plantdata, PeriodList1, predictedConsumtion2);

                JavaScriptSerializer jjs = new JavaScriptSerializer();

                var d = jjs.Deserialize<dynamic>(result);

                var output = d["response"][2];

                var doutput = jjs.Deserialize<dynamic>(output);

                var ErrorInfo = doutput["Error_info"];

                foreach (var item in ErrorInfo)

                {

                    var m = new DPP.Models.DemandAnalysis.SupplierDataModel3();

                    var outputarray = item.Value;

                    m.name = item.Key;

                    m.Error_info = ErrorInfo[m.name].ToString();

                    objDisclaimerDetails.Add(m);

                }

                return objDisclaimerDetails;

            }

            catch (Exception ex)

            {

                Logging.LogError(ex);

                return null;

 

            }

        }

 

 

    }

 

}