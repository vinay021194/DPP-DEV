using AdProvider.Utility;

using DPP.FilterAttributes;

using DPP.Interfaces;

using DPP.Logic.Business.DemandAnalysis;

using DPP.Models.DemandAnalysis;

using Kendo.Mvc.Extensions;

using Kendo.Mvc.UI;

using Newtonsoft.Json;

using Newtonsoft.Json.Linq;

using System;

using System.Collections.Generic;

using System.Data;

using System.IO;

using System.Linq;

using System.Net;

using System.Text;

using System.Web.Mvc;

using System.Web.Script.Serialization;

 

namespace DPP.Controllers

{

    [AuthenticatedUserFilter]

    public class DemandAnalysisController : Controller

    {

        [AuthorizeUser]

        public ActionResult DemandAnalysis()

        {

            var user = User as ICustomPrincipal;

            HttpContext.Items.Add("TableauOneParam", tableauParam(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("TableauOneParamsID"))).ConfigurationValue);

            return PartialView("Landing", new List<InventoryAnalysis>());

        }

 

        [AuthorizeUser]

        public ActionResult Index()

        {

            var user = User as ICustomPrincipal;

            HttpContext.Items.Add("TableauOneParam", tableauParam(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("TableauOneParamsID"))).ConfigurationValue);

            return PartialView("Landing", new List<InventoryAnalysis>());

        }

 

        [AuthorizeUser]

        public ActionResult Home()

        {

           

            var user = User as ICustomPrincipal;

            return PartialView("Home", new List<InventoryAnalysis>());

        }

 

        [HttpPost]

        public ActionResult Home(string materialID)

        {

            var user = User as ICustomPrincipal;

           // Session["MaterialID"] = materialID.ToString();

            return Json(materialID, JsonRequestBehavior.AllowGet);

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult getinventoryanalysisdetails([DataSourceRequest] DataSourceRequest request, int materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.InventoryAnalysis> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.InventoryAnalysis>();

            List<string> plantlist = new List<string>();

            int id = materialId;

            DataSourceResult datResult = null;

 

            //if (Session["MaterialID"] != null)

            //{

            //    id = Convert.ToInt32(Session["MaterialID"].ToString());

            //}

            try

            {

                if (id > 0)

                {

                    objdisclaimerdetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetInventoryAnalysisLogicDetails(id);

 

                    if (objdisclaimerdetails != null)

                    {

                        datResult = objdisclaimerdetails.ToDataSourceResult(request);

                    }

                }

                else

                {

                    datResult = null;

                }

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

            }

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetMaterialDetail([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.MateriaModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.MateriaModel>();

            double id = 0;

        if (materialId != 0)

            {

                id = Convert.ToDouble(materialId.ToString());

            }

            DataSourceResult datResult = null;

            try

            {

                if (id > 0)

                {

                    objdisclaimerdetails.Add(DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetMaterialDetails(id.ToString()));

 

                    if (objdisclaimerdetails != null)

                    {

                        datResult = objdisclaimerdetails.ToDataSourceResult(request);

                    }

                }

                else

                {

                    datResult = null;

 

                }

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

            }

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetDemandPredicitonsData([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.DemandModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.DemandModel>();

            double id = 0;

            DataSourceResult datResult = null;

            //validate

            id = materialId;

            try

            {

                if (id > 0)

                {

                    objdisclaimerdetails = (DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetDemandPredicitonDetails(id));

 

                    if (objdisclaimerdetails != null)

                    {

                        datResult = objdisclaimerdetails.ToDataSourceResult(request);

                    }

                }

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                // throw;

            }

 

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

        [HttpGet]

        [AuthorizeUser]

        public ActionResult MaterialOptimization(double materialId = 0)

        {

            try

            {

                var user = ((ICustomPrincipal)User);

                string Language = user.Language;

                ModelState.Clear();

                HttpContext.Items.Add("TableauTwoParam", tableauParam(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("TableauTwoParamsID"))).ConfigurationValue);

                return PartialView("DemandandIInventoryAnalysis", new List<DPP.Models.DemandAnalysis.MateriaModel>());

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

              //  throw;

            }

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetMaterialOptimizationData([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.MateriaModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.MateriaModel>();

            DataSourceResult datResult = null;

            var id ="";

            id = materialId.ToString();

            try

            {

                if (id != null)

                {

                    objdisclaimerdetails.Add(DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetMaterialDetails(id.ToString()));

 

                    if (objdisclaimerdetails != null)

                    {

                        datResult = objdisclaimerdetails.ToDataSourceResult(request);

                    }

               }

            }

            catch (Exception ex)

            {

                datResult = null;

                return Utility.Logging.LogError(ex);

                //throw;

            }

 

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetCostDriverOutputData([DataSourceRequest] DataSourceRequest request, List<costSeriesModel> materialobj)

        {

            List<DPP.Models.DemandAnalysis.CostDriverOutputModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.CostDriverOutputModel>();

            DataSourceResult datResult = null;

           

            try

            {

              

                    objdisclaimerdetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetCostDriverOutput(materialobj);

 

                    if (objdisclaimerdetails != null)

                    {

                        datResult = objdisclaimerdetails.ToDataSourceResult(request);

                    }

               

            }

            catch (Exception ex)

            {

                datResult = null;

                return Utility.Logging.LogError(ex);

                //throw;

            }

 

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetPlantData([DataSourceRequest] DataSourceRequest request, List<PlantModel> plantData, double materialId = 0, string plantid = null)

        {

            List<DPP.Models.DemandAnalysis.PlantModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.PlantModel>();

            DataSourceResult datResult = null;

            var id = "";

            var plant = plantid;

            id = materialId.ToString();

            try

            {

                if (id != null)

                {

                    objdisclaimerdetails = (DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetPlantData(id.ToString(), plant.ToString()));

                    if (objdisclaimerdetails != null)

                    {

                        if(plantData ==  null)

                        datResult = objdisclaimerdetails.ToDataSourceResult(request);

                        else datResult = plantData.ToDataSourceResult(request);

                    }

                    else datResult = plantData.ToDataSourceResult(request);

                }

            }

            catch (Exception ex)

            {

                datResult = null;

                return Utility.Logging.LogError(ex);

              ////  throw;

            }

            return Json(datResult, JsonRequestBehavior.AllowGet);  

        }

 

        [HttpPost]//update supplier result

        [ValidateCustomAntiForgeryToken]

        public ActionResult EditingInlinePlantData_update([DataSourceRequest] DataSourceRequest request, PlantModel datalist)

        {

            //List<PlantModel> datalistSupplier = new List<PlantModel>();

           

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetPeriodData([DataSourceRequest] DataSourceRequest request, List<PeriodModel> purchaseRequisitionsDatasoure, double materialId = 0, string plantid = null)

        {

            List<DPP.Models.DemandAnalysis.PeriodModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.PeriodModel>();

            DataSourceResult datResult = null;

            var id = "";

            var plant = plantid;

            id = materialId.ToString();

            try

            {

                if (id != null)

                {

                    objdisclaimerdetails = (DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetPeriodData(id.ToString(), plant.ToString()));

                    if (objdisclaimerdetails != null)

                    {

                        var resultlist = objdisclaimerdetails.Where(x => x.Period == "On Route").ToList();

                        if (purchaseRequisitionsDatasoure == null)

                        {

                            datResult = objdisclaimerdetails.ToDataSourceResult(request);

                        }

                        else

                        {

                            datResult = purchaseRequisitionsDatasoure.ToDataSourceResult(request);

 

                        }

                    }

                    else

                    {

                        datResult = purchaseRequisitionsDatasoure.ToDataSourceResult(request);

 

                    }

                }

            }

            catch (Exception ex)

            {

                datResult = null;

                return Utility.Logging.LogError(ex);

               // throw;

            }

 

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetCostOptimizationData([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            List<DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.CostDriverPredictionOutputModel>();

            DataSourceResult datResult = null;

            var id = 0.0;

            id = materialId;

            try

            {

                if (id > 0)

                    objdisclaimerdetails = (DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetCostOptimizationData(id));

 

                if (objdisclaimerdetails != null)

                {

                    datResult = objdisclaimerdetails.ToDataSourceResult(request);

                }

            }

            catch (Exception ex)

            {

                datResult = null;

                return Utility.Logging.LogError(ex);

            }

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

        [HttpGet]

        [AuthorizeUser]

        public ActionResult CostAnalysis([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

 

            try

            {

                var user = ((ICustomPrincipal)User);

                string Language = user.Language;

               var materialData =  new MateriaModel();

                ModelState.Clear();

                return PartialView("CostAnalysis", materialData);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

               // throw;

            }

 

        }

 

        [HttpGet]

        [AuthorizeUser]

        public ActionResult ShowForecast([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

 

            try

            {

                var user = ((ICustomPrincipal)User);

                string Language = user.Language;

                //ViewBag.ChartdataModel = supplierchartdataModel;

                ModelState.Clear();

                return PartialView("ShowForecast", new DPP.Models.DemandAnalysis.supplierchartdataModel());

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                //throw;

            }

 

        }

 

 

 

        [HttpPost]

        [AuthorizeUser]

        public ActionResult ShowForecastdata([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            try

            {

                var user = ((ICustomPrincipal)User);

                string Language = user.Language;

                ModelState.Clear();

                return PartialView(new List<DPP.Models.DemandAnalysis.MateriaModel>());

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

               // throw;

            }

 

        }

 

 

        [HttpGet]

        [AuthorizeUser]

        public ActionResult EditOptimize([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            try

            {

                var materialData = new MateriaModel();

                var user = ((ICustomPrincipal)User);

                string Language = user.Language;

                ModelState.Clear();

                return PartialView("EditOptimize", materialData);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                //throw;

            }

        }

 

        [HttpGet]

        [AuthorizeUser]

        public ActionResult FinalResult([DataSourceRequest] DataSourceRequest request, double materialId = 0)

        {

            try

            {

                var materialData = new MateriaModel();

                var user = ((ICustomPrincipal)User);

                string Language = user.Language;

               // materialData.PlantID = Session["Plant"].ToString();

                ModelState.Clear();

                return PartialView("FinalResult", materialData);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                //throw;

            }

 

        }

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetMaterialIds(string materialId)

        {

            List<string> objDisclaimerDetails = new List<string>();

            try

           {

                try

                {

                    if (materialId != "0")

                    {

                        objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetDropDownData(materialId);

                    }

                }

                catch (Exception e)

                {

                    return Utility.Logging.LogError(e);

                    //throw;

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

                var data = e.Message;

                objDisclaimerDetails = null;

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

            }

        }

 

        [HttpPost]    

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetPlantIds(string materialId)

        {

 

            List<DPP.Models.DemandAnalysis.InventoryAnalysis> objdisclaimerdetails = new List<DPP.Models.DemandAnalysis.InventoryAnalysis>();

            List<string> plantlist = new List<string>();

            int id = 0;

            id = Convert.ToInt32(materialId.ToString());

            try

            {

                if (id > 0)

                {

                    objdisclaimerdetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetInventoryAnalysisLogicDetails(id);

 

                    if (objdisclaimerdetails != null)

                    {

                        foreach (var data in objdisclaimerdetails)

                        {

                            plantlist.Add(data.plant);

                        }

 

                    }

                }

                else

                {

                    plantlist = null;

                }

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

            }

            return Json(plantlist, JsonRequestBehavior.AllowGet);

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetCostDriverDropDown(string materialId)

        {

 

            List<string> objDisclaimerDetails = new List<string>(); ;

            try

            {

                try

                {

 

                    if (!string.IsNullOrEmpty(materialId))

                    {

                        objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetCostDriverDropDown(materialId);

 

                    }

                }

                catch (Exception e)

                {

                    return Utility.Logging.LogError(e);

                 

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

 

                return Utility.Logging.LogError(e);

              

            }

 

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetCostDriverMuiltiDropDown()

        {

 

            List<string> objDisclaimerDetails = new List<string>(); ;

            try

            {

                try

                {

                    objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetCostDrivermultiDropDown();

                }

                catch (Exception e)

                {

                    return Utility.Logging.LogError(e);

 

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

 

                return Utility.Logging.LogError(e);

 

            }

 

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetSeriesNameMultiDropDown(List<string> materialId)

        {

            List<string> objDisclaimerDetails = new List<string>(); ;

            try

            {

                try

                {

 

                    if (materialId != null)

                    {

                        objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetSeriesNameMultiData(materialId);

 

                    }

                }

                catch (Exception e)

                {

 

                    objDisclaimerDetails = null;

                    return Utility.Logging.LogError(e);

                    //throw;

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

 

                return Utility.Logging.LogError(e);

                throw;

            }

 

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetCountryMultiDropDown(string materialId,string plantid)

        {

            List<string> objDisclaimerDetails = new List<string>(); ;

            try

            {

                try

                {

 

                    if (materialId != null)

                    {

                        objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetCountryNameMultiData(materialId, plantid);

 

                    }

                }

                catch (Exception e)

                {

 

                    objDisclaimerDetails = null;

                    return Utility.Logging.LogError(e);

                    //throw;

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

 

                return Utility.Logging.LogError(e);

                throw;

            }

 

        }

 

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetVendorMultiDropDown(string materialId, string plantid)

        {

            List<string> objDisclaimerDetails = new List<string>(); ;

            try

            {

                try

                {

 

                    if (materialId != null)

                    {

                        objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetVendorNameMultiData(materialId, plantid);

 

                    }

                }

                catch (Exception e)

                {

 

                    objDisclaimerDetails = null;

                    return Utility.Logging.LogError(e);

                    //throw;

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

 

                return Utility.Logging.LogError(e);

                throw;

            }

 

        }

 

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetSeriesNameDropDown(string materialId)

        {

            List<string> objDisclaimerDetails = new List<string>(); ;

            try

            {

                try

                {

 

                    if (materialId != null)

                    {

                       objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetSeriesNameData(materialId);

 

                    }

                }

                catch (Exception e)

                {

 

                    objDisclaimerDetails = null;

                    return Utility.Logging.LogError(e);

                    //throw;

                }

 

                return Json(objDisclaimerDetails, JsonRequestBehavior.AllowGet);

 

            }

            catch (Exception e)

            {

 

                return Utility.Logging.LogError(e);

                throw;

            }

 

        }

 

 

        [AcceptVerbs(HttpVerbs.Post)]

        [ValidateCustomAntiForgeryToken]

        public ActionResult CreateSupplierList([DataSourceRequest] DataSourceRequest request, SupplierModel datalist)

        {

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

        [AcceptVerbs(HttpVerbs.Post)]//update supplier result

        [ValidateCustomAntiForgeryToken]

        public ActionResult UpdateSupplierList([DataSourceRequest] DataSourceRequest request, SupplierModel datalist)

        {

            List<SupplierModel> datalistSupplier = new List<SupplierModel>();

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

        [AcceptVerbs(HttpVerbs.Post)]//update supplier result

        [ValidateCustomAntiForgeryToken]

        public ActionResult DeleteSupplierList([DataSourceRequest] DataSourceRequest request, SupplierModel datalist)

        {

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

        [AcceptVerbs(HttpVerbs.Post)]//update supplier result

        [ValidateCustomAntiForgeryToken]

        public ActionResult EditingInline_update([DataSourceRequest] DataSourceRequest request, SupplierModel datalist)

        {

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

        [AcceptVerbs(HttpVerbs.Post)]//update supplier result

        public ActionResult EditingInlineOptimize_update([DataSourceRequest] DataSourceRequest request, SupplierModel datalist)

        {

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetAllSupplier([DataSourceRequest] DataSourceRequest request, List<costSeriesModel> materialobj,

            List<SupplierModel> supplierdata) //UserSearchDetails model

        {

            try

            {

                var datalistSupplier = supplierdata;

                return Json(datalistSupplier.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                // throw;

            }

        }

 

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult SupplierDataModelData([DataSourceRequest] DataSourceRequest request, List<SupplierModel> supplierobject,

            List<SupplierDataModel> supplierDataModel,

            List<costSeriesModel> materialobj)

        {

            DataSourceResult datResult = null;

            try

            {

                List<SupplierDataModel> supplierDetailedDataModels = new List<SupplierDataModel>();

                if (supplierDataModel == null)

                {

                    supplierDetailedDataModels = new DemandAnalysisController().GetSupplierOptimizeDataModelTest(request, supplierobject, materialobj);// DemandAnalysisLogic.GetSupplierDataModel();

                }

                else

                {

                     supplierDetailedDataModels = supplierDataModel;

                }

                List<SupplierDataModel> supplierDataModels = new List<SupplierDataModel>();

                if (supplierDetailedDataModels != null)

                {

                    supplierDataModels = supplierDetailedDataModels;

                }

                if (supplierDataModels != null)

                {

                    datResult = supplierDataModels.ToDataSourceResult(request);

                }

            }

            catch (Exception ex)

            {

                datResult = null;

                return Utility.Logging.LogError(ex);

                // throw;

            }

 

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult SupplierDataModelForcast([DataSourceRequest] DataSourceRequest request, List<costSeriesModel> materialobj,

            List<SupplierModel> supplierdata)

        {

            var datalistSupplier = supplierdata;

            var seriesName = materialobj;

            List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

            DataSourceResult datResult = null;

            try

            {

                objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetSupplierDataModel(datalistSupplier, seriesName);

                if (objDisclaimerDetails != null)

                {

                    objDisclaimerDetails = (List<SupplierModel>)objDisclaimerDetails.Where(s => s.OptimizeName == "Forecasted Price").ToList();

                    datResult = objDisclaimerDetails.ToDataSourceResult(request);

                }

            }

            catch (Exception e)

            {

                return Utility.Logging.LogError(e);

                // throw;

                // datResult = null;

            }

 

            return Json(datResult, JsonRequestBehavior.AllowGet);

        }

 

 

        //        [HttpPost]

        //[ValidateCustomAntiForgeryToken]

        //public ActionResult costDriverAnalysisGraph([DataSourceRequest] DataSourceRequest request, List<String> materialobj,

        //    List<String> supplierdata , DateTime dateRange)

        //{

        //    var datalistSupplier = supplierdata;

        //    var seriesName = materialobj;

        //    List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

        //    DataSourceResult datResult = null;

        //    try

        //    {

        //        objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.costDriverAnalysisGraphdata(materialobj, supplierdata);

        //        if (objDisclaimerDetails != null)

        //        {

        //            objDisclaimerDetails = (List<SupplierModel>)objDisclaimerDetails.Where(s => s.OptimizeName == "Forecasted Price").ToList();

        //            datResult = objDisclaimerDetails.ToDataSourceResult(request);

        //        }

        //    }

        //    catch (Exception e)

        //    {

        //        return Utility.Logging.LogError(e);

        //        // throw;

        //        // datResult = null;

        //    }

 

        //    return Json(datResult, JsonRequestBehavior.AllowGet);

        //}

 

 

 

        public List<SupplierModel> SupplierDataModelForcastChart([DataSourceRequest] DataSourceRequest request, List<SupplierModel> supplierobject,

          List<costSeriesModel> materialobj)

        {

            var datalistSupplier = supplierobject;//System.Web.HttpContext.Current.Session["supplierInformationList"] as List<SupplierModel>;

            var seriesName = materialobj;//System.Web.HttpContext.Current.Session["costSeries"] as List<costSeriesModel>;

            List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

            try

            {

                {

                    objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetSupplierDataModel(datalistSupplier, seriesName);

                }

            }

            catch (Exception e)

            {

                Logging.LogError(e);

                return null;

 

            }

            return objDisclaimerDetails;

        }

 

 

        public List<SupplierDataModel> GetSupplierOptimizeDataModelTest([DataSourceRequest] DataSourceRequest request, List<SupplierModel> supplierobject, List<costSeriesModel> materialobj)

        {

            //if (System.Web.HttpContext.Current.Session["SupplierAvgResult"] == null || System.Web.HttpContext.Current.Session["SupplierAvgResult"].ToString() == "0")

            //{

            List<SupplierModel> s1 = SupplierDataModelForcastChart(request, supplierobject, materialobj);

            //System.Web.HttpContext.Current.Session["SupplierAvgResult"] = s1;

 

            //}

            var suppliers = s1 as List<SupplierModel>;

            List<SupplierDetailedDataModel> supplierDetailedDataModels = new List<SupplierDetailedDataModel>();

            List<SupplierDataModel> supplierDataModels = new List<SupplierDataModel>();

 

            int counter = 1;

            List<String> existingSupplier = new List<String>();

 

            if (suppliers != null)

            {

                foreach (var supplier in suppliers)

                {

                    var supplierdata = suppliers.Select(s => s.Name).Distinct();

                    foreach (var data in supplierdata)

                    {

                        var duplicateSupplier = existingSupplier.FirstOrDefault(stringToCheck => stringToCheck.Contains(data));

                        if (duplicateSupplier == null)

                        {

                            existingSupplier.Add(data);

                            var newList = suppliers.Where(item => item.Name == data).ToList();

                            foreach (var datasupplier in newList)

                            {

                                supplierDataModels.Add(new SupplierDataModel()

                                {

                                    SupplierId = counter,

                                    TimePeriod = datasupplier.OptimizeName,

                                    SupplierName = datasupplier.Name,

                                    Month1 = Convert.ToString(datasupplier.Nov),// DateTime.Now.ToString("MMM"),

                                    Month2 = Convert.ToString(datasupplier.Dec),// DateTime.Now.AddMonths(1).ToString("MMM"),

                                    Month3 = Convert.ToString(datasupplier.JAN),// DateTime.Now.AddMonths(2).ToString("MMM"),

                                    Month4 = Convert.ToString(datasupplier.FEB),// DateTime.Now.AddMonths(3).ToString("MMM"),

                                    Month5 = Convert.ToString(datasupplier.MARCH),//DateTime.Now.AddMonths(4).ToString("MMM"),

                                    Month6 = Convert.ToString(datasupplier.APRIL),// DateTime.Now.AddMonths(5).ToString("MMM"),   

                                });

                                counter++;

                            }

                        }

                    }

 

 

                }

            }

            return supplierDataModels;//supplierDetailedDataModels;//supplierDataModels;//

        }

 

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult EditingSupplierDataModelData([DataSourceRequest] DataSourceRequest request, SupplierDataModel datalist)

        {

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult EditingPeriodDataModelData([DataSourceRequest] DataSourceRequest request, PeriodModel datalist)

        {

           // List<PeriodModel> periodDetailedDataModels = (List<PeriodModel>)Session["OnRoute"];

           

            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));

        }

 

 

        [HttpPost]

        public ActionResult GetPlantDetails(string Plant) //UserSearchDetails model

        {

            try

            {

                //Session["Plant"] = Plant;

                return Json(Plant, JsonRequestBehavior.AllowGet);

 

            }

           catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                //throw;

            }

        }

 

 

        //[HttpPost]

        //public ActionResult GetIndexInformation([DataSourceRequest] DataSourceRequest request) //UserSearchDetails model

        //{

        //    try

        //    {

        //        //var IndexInfoData = Session["supplierInformationList"] as List<IndexInfoModel>;

        //        return Json(IndexInfoData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        //    }

        //    catch (Exception ex)

        //    {

        //        return Utility.Logging.LogError(ex);

        //    }

       //}

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult getForcastChart([DataSourceRequest] DataSourceRequest request,

            List<costSeriesModel> materialobj, List<SupplierModel> supplierdata)

        {

            supplierchartdataModel supplierchartdataModel = new supplierchartdataModel();

            List<supplierChartModel> result = new List<supplierChartModel>();

            List<SupplierDataModel> supplierDetailedDataModels = new DemandAnalysisController().GetSupplierOptimizeDataModelTest(request, supplierdata,

                     materialobj);// DemandAnalysisLogic.GetSupplierDataModel();

            supplierDetailedDataModels = supplierDetailedDataModels.Where(x => x.TimePeriod == "Forecasted Price").ToList();

            var chartdataModel = new supplierchartdataModel();

            foreach (var Supplier in supplierDetailedDataModels)

            {

                var chartmodel = new supplierChartModel();

                double[] values = new double[] { Convert.ToDouble(Supplier.Month1), Convert.ToDouble(Supplier.Month2), Convert.ToDouble(Supplier.Month3),

                    Convert.ToDouble(Supplier.Month4), Convert.ToDouble(Supplier.Month5), Convert.ToDouble(Supplier.Month6) };

                chartmodel.avrageResult = values;

                chartmodel.name = Supplier.SupplierName;

                //chartdataModel.Add(chartmodel);

                result.Add(chartmodel);

            }

 

            List<string> months = new List<string>();

            for (int i = 0; i < 6; i++)

            {

                var chartmodel = new supplierChartModel();

                var formattedMonth = (DateTime.Now.AddMonths(i).ToString("MMM-yy"));

                months.Add(formattedMonth);

            }

            chartdataModel.Categories = months.ToList();

            supplierchartdataModel.Series = result;

            supplierchartdataModel.Categories = months;

            return Json(supplierchartdataModel, JsonRequestBehavior.AllowGet);

        }

 

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult getMaterialChart([DataSourceRequest] DataSourceRequest request, string materialId, string plantid,

            string vendor,string country)

        {

            List<supplierChartModel2> result = new List<supplierChartModel2>();

            var material = "";

            var plant = plantid;

            material = materialId.ToString();

            try

            {

                if (vendor != "" && country != "")

                {

                    result = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetMaterialChartData(material, plant, vendor, country);

                }

             }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

               // throw;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

 

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult getAlertAnalysisChart([DataSourceRequest] DataSourceRequest request,

            string material, string series, DateTime startingDate , DateTime endDate , string timeInterwal)

        {

            List<alertAnalysisChart> result = new List<alertAnalysisChart>();

            try

            {

                result = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.getAlertAnalysisData(material, series,

                    startingDate,  endDate , timeInterwal);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                // throw;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

 

        }

 

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetFinalResult([DataSourceRequest] DataSourceRequest request,

            List<SupplierDataModel> supplierDataModel, List<PlantModel> plantDataInput, List<PeriodModel> PeriodListInput,

            List<PeriodModel> predictedConsumtionInput, List<SupplierModel> supplierobject,

            List<costSeriesModel> materialobj) //UserSearchDetails model

           

        {

            List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

            List<SupplierDataModel> SupplierDataModeldatalist = new List<SupplierDataModel>();

            DataSourceResult datResult = null;

            try

            {

                if (supplierDataModel == null)

                {

                    SupplierDataModeldatalist = new DemandAnalysisController().GetSupplierOptimizeDataModelTest(request, supplierobject, materialobj);// demandanalysislogic.getsupplierdatamodel();

 

                }

                else

                {

                    SupplierDataModeldatalist = supplierDataModel;//(List<SupplierDataModel>)Session["SupplierDetailedDataModels"];}

                }

                List<PlantModel> Plantdata = new List<PlantModel>();

                List<PeriodModel> PeriodList = new List<PeriodModel>();

                PeriodList = PeriodListInput;

                Plantdata = plantDataInput;

                var predictedConsumtion = predictedConsumtionInput;//Session["Forecasted"] as List<PeriodModel>;

                objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetFinalResultData(

                    SupplierDataModeldatalist, Plantdata, PeriodList, predictedConsumtion);

                if (objDisclaimerDetails != null)

                {

                   // Session["FinalResultList"] = objDisclaimerDetails;

                    datResult = objDisclaimerDetails.ToDataSourceResult(request);

                }

                return Json(datResult, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                // throw;

            }

        }

      

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetFinalResult2([DataSourceRequest] DataSourceRequest request,

            List<SupplierDataModel> supplierDataModel, List<PlantModel> plantDataInput, List<PeriodModel> PeriodListInput,

            List<PeriodModel> predictedConsumtionInput, List<SupplierModel> supplierobject,

            List<costSeriesModel> materialobj)

        {

            List<SupplierDataModel2> objDisclaimerDetails = new List<SupplierDataModel2>();

            List<SupplierDataModel> SupplierDataModeldatalist = new List<SupplierDataModel>();

            DataSourceResult datResult = null;

            try

            {

                if (supplierDataModel == null)

                {

                    SupplierDataModeldatalist = new DemandAnalysisController().GetSupplierOptimizeDataModelTest(request, supplierobject, materialobj);// demandanalysislogic.getsupplierdatamodel();

 

                }

                else

                {

                    SupplierDataModeldatalist = supplierDataModel;//(List<SupplierDataModel>)Session["SupplierDetailedDataModels"];}

                }

                List<PlantModel> Plantdata = new List<PlantModel>();

                List<PeriodModel> PeriodList = new List<PeriodModel>();

                PeriodList = PeriodListInput;

                Plantdata = plantDataInput;

                var predictedConsumtion = predictedConsumtionInput;//Session["Forecasted"] as List<PeriodModel>;

                objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetFinalResultData2(SupplierDataModeldatalist, Plantdata, PeriodList, predictedConsumtion);

 

                datResult = objDisclaimerDetails.ToDataSourceResult(request);

                return Json(datResult, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

            }

        }

      

        [HttpPost]

        [ValidateCustomAntiForgeryToken]

        public ActionResult GetFinalResult3([DataSourceRequest] DataSourceRequest request,

            List<SupplierDataModel> supplierDataModel, List<PlantModel> plantDataInput, List<PeriodModel> PeriodListInput,

            List<PeriodModel> predictedConsumtionInput, List<SupplierModel> supplierobject,

            List<costSeriesModel> materialobj)

        {

            List<SupplierDataModel3> objDisclaimerDetails = new List<SupplierDataModel3>();

            List<SupplierDataModel> SupplierDataModeldatalist = new List<SupplierDataModel>();

            DataSourceResult datResult = null;

            try

            {

                if (supplierDataModel == null)

                {

                    SupplierDataModeldatalist = new DemandAnalysisController().GetSupplierOptimizeDataModelTest(request, supplierobject, materialobj);// demandanalysislogic.getsupplierdatamodel();

 

                }

                else

                {

                    SupplierDataModeldatalist = supplierDataModel;//(List<SupplierDataModel>)Session["SupplierDetailedDataModels"];}

                }

                List<PlantModel> Plantdata = new List<PlantModel>();

                List<PeriodModel> PeriodList = new List<PeriodModel>();

                PeriodList = PeriodListInput;

                Plantdata = plantDataInput;

                var predictedConsumtion = predictedConsumtionInput;//Session["Forecasted"] as List<PeriodModel>;

                objDisclaimerDetails = DPP.Logic.Business.DemandAnalysis.DemandAnalysisLogic.GetFinalResultData3(SupplierDataModeldatalist, Plantdata, PeriodList, predictedConsumtion);

 

                datResult = objDisclaimerDetails.ToDataSourceResult(request);

                return Json(datResult, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                // throw;

            }

        }

        public ActionResult GetSeriesName(List<costSeriesModel> materialobj) //UserSearchDetails model

        {

            try

            {

                List<costSeriesModel> costSeries = materialobj;

                return Json(costSeries, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)

            {

                return Utility.Logging.LogError(ex);

                // throw;

            }

        }

 

 

        public ActionResult excel_export_read([DataSourceRequest] DataSourceRequest request)

        {

            List<SupplierModel> objDisclaimerDetails = new List<SupplierModel>();

            DataSourceResult datResult = null;

           // objDisclaimerDetails = Session["FinalResultList"] as List<SupplierModel>;

            datResult = objDisclaimerDetails.ToDataSourceResult(request);

            return Json(datResult, JsonRequestBehavior.AllowGet);

 

        }

 

 

        public Models.Configure.GetConfigurationModel tableauParam(int Id)

        {

            return Logic.Business.Configure.ConfigureLogic.GetConfigurationById(Id) ?? new Models.Configure.GetConfigurationModel();

        }

 

        [HttpPost]

 

        public ActionResult GetTableauParams(int configureId)

        {

            Models.Configure.GetConfigurationModel Result = Logic.Business.Configure.ConfigureLogic.GetConfigurationById(configureId) ?? new Models.Configure.GetConfigurationModel();

            HttpContext.Items.Add("TableauOneParam", Result.ConfigurationValue);

            return Json(Result, JsonRequestBehavior.AllowGet);

 

        }

 

    }

}