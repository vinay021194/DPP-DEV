// public static List<SupplierModel> GetSupplierDataModel(List<SupplierModel> datalistSupplier,

//             List<costSeriesModel> seriesName)



// {

//     List<SupplierModel> SupplierList = new List<SupplierModel>();

//     try

//     {

//         if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

//         {

//             connectionString = SQLconnectionString;

//         }

//         if (datalistSupplier != null)

//         {

//             foreach (var item in datalistSupplier)//for supplier

//             {

//                 if (item.Formula.All(char.IsDigit))//check if it is digit

//                 {

//                     var supplierInformation = new SupplierModel

//                     {

//                         Name = item.Name,

//                         OptimizeName = "Forecasted Price",

//                         Nov = Convert.ToDouble(item.Formula),

//                         Dec = Convert.ToDouble(item.Formula),

//                         JAN = Convert.ToDouble(item.Formula),

//                         FEB = Convert.ToDouble(item.Formula),

//                         MARCH = Convert.ToDouble(item.Formula),

//                         APRIL = Convert.ToDouble(item.Formula),

//                     };

//                     var leatInformation = new SupplierModel

//                     {

//                         Name = item.Name,

//                         OptimizeName = "Lead Time",

//                         Nov = Convert.ToDouble(item.Time),

//                         Dec = Convert.ToDouble(item.Time),

//                         JAN = Convert.ToDouble(item.Time),

//                         FEB = Convert.ToDouble(item.Time),

//                         MARCH = Convert.ToDouble(item.Time),

//                         APRIL = Convert.ToDouble(item.Time)

//                     };

//                     var CapacityInformation = new SupplierModel

//                     {

//                         Name = item.Name,

//                         OptimizeName = "Supplier Max. Capacity",

//                         Nov = Convert.ToDouble(item.Capacity),

//                         Dec = Convert.ToDouble(item.Capacity),

//                         JAN = Convert.ToDouble(item.Capacity),

//                         FEB = Convert.ToDouble(item.Capacity),

//                         MARCH = Convert.ToDouble(item.Capacity),

//                         APRIL = Convert.ToDouble(item.Capacity)

//                     };

//                     SupplierList.Add(supplierInformation);

//                     SupplierList.Add(leatInformation);

//                     SupplierList.Add(CapacityInformation);



//                 }

//                 else

//                 { //   2*[1]+[2]

//                     Dictionary<string, string> FormulaList = new Dictionary<string, string>();

//                     List<Double> finalResult = new List<Double>();

//                     var index = item.Formula;

//                     var FormulaForIndex = index;

//                     char search = '[';

//                     var result = index.Select((b, i) => b.Equals(search) ? i : -1).Where(i => i != -1).ToList();

//                     foreach (var resultrow in result)//take out the index from formula

//                     {

//                         var indexdata = resultrow;

//                         indexdata = indexdata + 1;

//                         var s = Convert.ToInt32(index.Substring(indexdata, 1));

//                         s = s - 1;

//                         var seriesdata = seriesName[s];

//                         var series = seriesdata.series;

//                         var material = seriesdata.costDriver;

//                         try

//                         {

//                             using (var conn = new OdbcConnection(connectionString))

//                             {

//                                 conn.Open();

//                                 using (var cmd = conn.CreateCommand())/// material -1 series -3

//                                 {



//                                     cmd.CommandText = @"SELECT * from proc_pp_inter.icis_forecast_sid  where material_name = '" + material + "' and" +

//                                         " serial_name ='" + series + "' and " +

//                                         " icis_forecast_sid.`date` >= from_unixtime(unix_timestamp()) and icis_forecast_sid.`date` < add_months(from_unixtime(unix_timestamp()), 6)";

//                                     if (ConfigurationManager.AppSettings["ExecutionMode"].Contains("SQL"))

//                                         cmd.CommandText = OpenQueryMSSQL(cmd);

//                                     using (OdbcDataReader reader = cmd.ExecuteReader())

//                                     {

//                                         List<String> months = new List<string>();

//                                         var dataTable = new DataTable();

//                                         dataTable.Load(reader);

//                                         DataTable dest = new DataTable();

//                                         foreach (DataRow itemdata in dataTable.Rows)//Take data for material and serial

//                                         {

//                                             var currentdate = DateTime.Parse(itemdata["date"].ToString());

//                                             var currentMonth = currentdate.ToString("MMM");

//                                             var month = months

//                                                         .FirstOrDefault(stringToCheck => stringToCheck.Contains(currentMonth));

//                                             if (month == null)

//                                             {

//                                                 months.Add(currentMonth);

//                                                 var currentCount = dataTable.AsEnumerable()//Take the month

//                                                                         .Where(r => r.Field<DateTime>("Date").ToString("MMM") ==

//                                                                         (currentdate.ToString("MMM"))).Count();

//                                                 DataTable innertable = dataTable.AsEnumerable()//Take data for particular month

//                                                                             .Where(r => r.Field<DateTime>("Date").ToString("MMM") == (currentdate.ToString("MMM"))).CopyToDataTable();

//                                                 var sum = 0.0;

//                                                 foreach (DataRow itemrow in innertable.Rows)//Data for a particular month

//                                                 {

//                                                     var value = itemrow["actual_values"].ToString();

//                                                     if (!string.IsNullOrEmpty(value))

//                                                     {

//                                                         sum = sum + Convert.ToDouble(itemrow["actual_values"].ToString());

//                                                     }

//                                                     else

//                                                     {

//                                                         sum = sum + Convert.ToDouble(itemrow["predicted_values"].ToString());

//                                                     }

//                                                 }

//                                                 var totalsum = sum;

//                                                 var avrage = sum / currentCount;

//                                                 if (FormulaList.ContainsKey(currentMonth))//if list have current month

//                                                 {

//                                                     var currentmonthindex = FormulaList[currentMonth];

//                                                     var aStringBuilder = new StringBuilder(currentmonthindex);

//                                                     var newIndexPosition = currentmonthindex.IndexOf(search);

//                                                     aStringBuilder.Remove(newIndexPosition, 3);

//                                                     aStringBuilder.Insert(newIndexPosition, avrage);

//                                                     currentmonthindex = aStringBuilder.ToString();

//                                                     FormulaList[currentMonth] = currentmonthindex;

//                                                 }

//                                                 else//If list dont have current month

//                                                 {

//                                                     var aStringBuilder = new StringBuilder(index);

//                                                     aStringBuilder.Remove(resultrow, 3);

//                                                     aStringBuilder.Insert(resultrow, avrage);

//                                                     var newindex = aStringBuilder.ToString();

//                                                     FormulaList.Add(currentMonth, newindex);

//                                                 }

//                                             }

//                                         }

//                                     }

//                                 }

//                                 conn.Close();

//                             }

//                         }

//                         catch (Exception e)

//                         {

//                             Logging.LogError(e);

//                             return null;



//                         }

//                     }

//                     foreach (var formulalists in FormulaList.ToArray())

//                     {

//                         var finalValue = formulalists.Value.Trim();

//                         DataTable dt = new DataTable();//calculate formula

//                         var v = dt.Compute(finalValue, "");

//                         v = Convert.ToDouble((String.Format("{0:n0}", v)));

//                         finalResult.Add(Math.Round((Convert.ToDouble(v)), 2));

//                     }

//                     var supplierInformation = new SupplierModel

//                     {

//                         Name = item.Name,

//                         OptimizeName = "Forecasted Price",

//                         Nov = finalResult[0],

//                         Dec = finalResult[1],

//                         JAN = finalResult[2],

//                         FEB = finalResult[3],

//                         MARCH = finalResult[4],

//                         APRIL = finalResult[5]

//                     };

//                     var leatInformation = new SupplierModel

//                     {

//                         Name = item.Name,

//                         OptimizeName = "Lead Time",

//                         Nov = Convert.ToDouble(item.Time),

//                         Dec = Convert.ToDouble(item.Time),

//                         JAN = Convert.ToDouble(item.Time),

//                         FEB = Convert.ToDouble(item.Time),

//                         MARCH = Convert.ToDouble(item.Time),

//                         APRIL = Convert.ToDouble(item.Time)

//                     };

//                     var CapacityInformation = new SupplierModel

//                     {

//                         Name = item.Name,

//                         OptimizeName = "Supplier Max. Capacity",

//                         //OptimizeName = "Capacity",

//                         Nov = Convert.ToDouble(item.Capacity),

//                         Dec = Convert.ToDouble(item.Capacity),

//                         JAN = Convert.ToDouble(item.Capacity),

//                         FEB = Convert.ToDouble(item.Capacity),

//                         MARCH = Convert.ToDouble(item.Capacity),

//                         APRIL = Convert.ToDouble(item.Capacity)

//                     };

//                     SupplierList.Add(supplierInformation);

//                     SupplierList.Add(leatInformation);

//                     SupplierList.Add(CapacityInformation);





//                 }

//             }

//         }

//         return SupplierList;



//     }

//     catch (Exception ex)

//     {

//         Logging.LogError(ex);

//         return null;





//     }

// }