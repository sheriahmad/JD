using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.Data.Odbc;
using System.ServiceModel;
using System.Data.OracleClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;


using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

/// <summary>
/// Summary description for mktg_Reporting
/// </summary>
public partial class mktg_Reporting : System.Web.UI.Page
{

    //  http://www.computing.net/answers/programming/batch-files/14003.html



    Excel.Application xlApp;
    Excel.Workbook xlWorkBook;




    Excel.Worksheet xlWorkSheet;
    Excel.Worksheet xlWorkSheet2;
    Excel.Worksheet xlWorkSheet3;
    Excel.Worksheet xlWorkSheet4;
    Excel.Worksheet xlWorkSheet5;

    string[,] strMailingList;
    int mailListCounter;

    object misValue = System.Reflection.Missing.Value;
    Excel.Range chartRange;

    public string strLastSalesReps;

    DataTable dtGlobalAdmin;

    //int intOrderedTodayQtyRow;
    int intOrderedTodayDollarsRow;
    //int intShippedTodayQtyRow;
    int intShippedTodayDollarsRow;



    public HyperLink distributeReport(string type, string fromDate, string toDate, string sendTo, string sendFrom, string SalesReps, string CompanyCodes, string strFileRootPath, string strReportsPath, object sender, EventArgs e, string strIncludeCompanies, string strIncludeParts, Boolean emailResults, string includePages, string slspCode, bool blnOnDevServer)
    {
        HyperLink hlTemp = new HyperLink();




       


        //kill EXCEL com processes
        System.Diagnostics.Process[] PROC2 = Process.GetProcessesByName("EXCEL");
        foreach (System.Diagnostics.Process PK2 in PROC2)
        {//User excel process always have window name
            //COM process do not.
            if (PK2.MainWindowTitle.Length == 0)
                PK2.Kill();
        }


        dtGlobalAdmin = GenUtils.loadGlobalAdmin(1);

        strMailingList = new string[100, 5];
        mailListCounter = -1;
        int i;

        //intOrderedTodayQtyRow = 0;
        intOrderedTodayDollarsRow = 0;
        //intShippedTodayQtyRow = 0;
        intShippedTodayDollarsRow = 0;

        string thisResult = string.Empty;
  

        xlApp = new Excel.ApplicationClass();
        xlWorkBook = xlApp.Workbooks.Add(misValue);
        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        xlWorkSheet2 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
        xlWorkSheet3 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(3);
        xlWorkSheet4 = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, xlWorkSheet3, Type.Missing, Type.Missing);
        xlWorkSheet5 = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, xlWorkSheet4, Type.Missing, Type.Missing);

        xlApp.DisplayAlerts = false;

        DataTable dtDistList = null;

        switch (type)
        {
            //case "test":

            //    hlTemp = BuildSalesReport(type, fromDate, toDate, sendTo, sendFrom, SalesReps, CompanyCodes, strFileRootPath, strReportsPath, sender, e, "False", "False", false, "", "Pnl");

            //    thisResult = sendMail("TestTo@phatsky.com", "TestTo@phatsky.com", "Subject Line 10 ", string.Empty, true, false, string.Empty);


            //    //Response.Write("the result: " + thisResult);



            //    break;

           

            case "distributionList":

                //------------------ load the distribution list

                if (blnOnDevServer  || SalesReps == "TEST")
                {
                   
                    dtDistList = loadDistributionList("blnOnDevServer");
                    if (SalesReps == "TEST")
                    {
                        SalesReps = string.Empty;
                    }

                }
                else
                {

                    dtDistList = loadDistributionList("DailySR");

                }





                string strSalesReps = string.Empty;
                string strReportPages = string.Empty;

                foreach (DataRow distRow in dtDistList.Rows)
                {
                    if (distRow["Frequency"].ToString().ToLower() == "daily")
                    {
                        strSalesReps = string.Empty;

                        if (distRow["Parameter1"].ToString() != "all")
                        {
                            strSalesReps = distRow["Parameter1"].ToString();
                        }


                        //--------------------- set to today for 5 pm



                        BuildSalesReport(type, fromDate, toDate, distRow["emailList"].ToString(), sendFrom, strSalesReps, CompanyCodes, strFileRootPath, strReportsPath, sender, e, strIncludeCompanies, strIncludeParts, false, distRow["Parameter2"].ToString(), distRow["SlspCode"].ToString());

                    }
                }

                break;
            case "adminPanel":

                hlTemp = BuildSalesReport(type, fromDate, toDate, sendTo, sendFrom, SalesReps, CompanyCodes, strFileRootPath, strReportsPath, sender, e, strIncludeCompanies, strIncludeParts, false, "", "Pnl");

                break;

            
        }







        // Need all following code to clean up and extingush all references!!!
        xlWorkBook.Close(null, null, null);
        xlApp.Workbooks.Close();
        xlApp.Quit();
        System.Runtime.InteropServices.Marshal.ReleaseComObject(chartRange);
        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
        xlWorkSheet = null;
        xlWorkBook = null;
        xlApp = null;
        GC.Collect(); // force final cleanup!


        ////kill EXCEL com processes
        //System.Diagnostics.Process[] PROC = Process.GetProcessesByName("EXCEL");
        //foreach (System.Diagnostics.Process PK in PROC)
        //{//User excel process always have window name
        //    //COM process do not.
        //    //if (PK.MainWindowTitle.Length == 0)
        //        PK.Kill();
        //}





        //----------------------------------------- Mail the sheets
        if (type == "distributionList")
        {
           
                for (i = 0; i <= mailListCounter; i++)
                {
                    thisResult = sendMail(strMailingList[i, 0], strMailingList[i, 1], strMailingList[i, 2], string.Empty, true, false, strMailingList[i, 4]);
                }
           

            //kill MSIE com processes
            System.Diagnostics.Process[] PROC = Process.GetProcessesByName("IEXPLORE");
            foreach (System.Diagnostics.Process PK in PROC)
            {//User excel process always have window name
                //COM process do not.
                if (PK.MainWindowTitle.IndexOf("Engineering") > -1)
                    PK.Kill();
            }
        }


          

       

       //  taskkill /f im iexplore.exe



        return hlTemp;

    }


    public mktg_Reporting()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //************************************************************************


    public HyperLink BuildSalesReport(string type, string fromDate, string toDate, string sendTo, string sendFrom, string SalesReps, string CompanyCodes, string strFileRootPath, string strReportsPath, object sender, EventArgs e, string strIncludeCompanies, string strIncludeParts, Boolean emailResults, string includePages, string slspCode)
    {



        

        ////kill EXCEL com processes
        //System.Diagnostics.Process[] PROC2 = Process.GetProcessesByName("EXCEL");
        //foreach (System.Diagnostics.Process PK2 in PROC2)
        //{//User excel process always have window name
        //    //COM process do not.
        //    if (PK2.MainWindowTitle.Length == 0)
        //        PK2.Kill();
        //}
        
        
        int i;


        bool blIncludeDailySales = true;
        bool blIncludeSnapShot = true;
        bool blIncludeWeeklyRecap = true;


        Excel.Range myCell = null;


        bool blIncludeCompanies = false;
        if (strIncludeCompanies == "True")
        {
            blIncludeCompanies = true;
        }

        bool blIncludeParts = false;
        if (strIncludeParts == "True")
        {
            blIncludeParts = true;
        }


        if (includePages != string.Empty)
        {

            blIncludeDailySales = false;
            blIncludeSnapShot = false;
            blIncludeWeeklyRecap = false;
            blIncludeCompanies = false;
            blIncludeParts = false;

            string[] incPages = includePages.Trim().Split(',');
            for (i = 0; i <= incPages.GetUpperBound(0); i++)
            {
                switch (incPages[i])
                {
                    case "1":
                        blIncludeDailySales = true;
                        break;
                    case "2":
                        blIncludeSnapShot = true;
                        break;
                    case "3":
                        blIncludeWeeklyRecap = true;
                        break;
                    case "4":
                        blIncludeCompanies = true;
                        break;
                    case "5":
                        blIncludeParts = true;
                        break;
                }
            }
        }

        HyperLink hlTemp = new HyperLink();

        //Excel.Application xlApp;
        //Excel.Workbook xlWorkBook;


       
        //Excel.Worksheet xlWorkSheet;
        //Excel.Worksheet xlWorkSheet2;
        //Excel.Worksheet xlWorkSheet3;
        //Excel.Worksheet xlWorkSheet4;
        //Excel.Worksheet xlWorkSheet5;

        //object misValue = System.Reflection.Missing.Value;
        //Excel.Range chartRange;

        int rowCounter = 1;
        short colCnt = 1;
        short colSalesRep = colCnt++;
        short colCompany = colCnt++;
        short colPartNo = colCnt++;
        short colQtyOrdered = colCnt++;
        short colQtyShipped = colCnt++;
        short colUnitPrice = colCnt++;
        short colTotalOrdered = colCnt++;
        short colTotalShipped = colCnt++;
        short colCustomerDueDate = colCnt++;

        short colCompanyYearForecast = colCnt++;
        short colCompanyYTDForecast = colCnt++;
        short colCompanyYTDSales = colCnt++;
        short colCompanyYTDVariance = colCnt++;



        short colCompanyRolling12Sales = colCnt++;
        int colCompanyListRolling12_24Sales = colCnt++;
        int colCompanyListRolling24_36Sales = colCnt++;
        int colCompanyListRollingTrend = colCnt++;


        //short colCompanyYearVariance = colCnt++;



        short colPartYearForecastQty = colCnt++;
        short colPartYearForecastDollars = colCnt++;
        short colPartYTDForecast = colCnt++;
        short colPartYTDSales = colCnt++;
        short colPartYTDVariance = colCnt++;




        short colPartRolling12Qty = colCnt++;
        short colPartRolling12Dollars = colCnt++;

        string strSelectedReps = string.Empty;
        bool blnRepsFirstPlaced = false;

        string strCompanyCodes = string.Empty;
        bool blnCompanyCodesPlaced = false;

        string strCellData = string.Empty;
        Excel.Range range;


        //xlApp = new Excel.ApplicationClass();
        //xlWorkBook = xlApp.Workbooks.Add(misValue);
        //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //xlWorkSheet2 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
        //xlWorkSheet3 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(3);
        //xlWorkSheet4 = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, xlWorkSheet3, Type.Missing, Type.Missing);
        //xlWorkSheet5 = (Excel.Worksheet)xlWorkBook.Worksheets.Add(Type.Missing, xlWorkSheet4, Type.Missing, Type.Missing);


        ((Excel.Worksheet)xlApp.ActiveWorkbook.Sheets[1]).Select(Type.Missing); //activates first sheet

        //---------------- get sales reps keys
        //DataTable dtSalesReps = null;
        string[] nSalesReps = SalesReps.Trim().Split(',');

        if (SalesReps.Trim() != string.Empty)
        {
            //dtSalesReps = getSalesRepsKeys(SalesReps);
 
        }


        //xlWorkSheet.Name = "Daily Sales Report";
        //xlWorkSheet2.Name = "Current Positions";


        //chartRange = xlWorkSheet.get_Range("A1", "Z1");
        //chartRange.EntireColumn.ColumnWidth = 14;

        //chartRange = xlWorkSheet.get_Range("B1", "b1");
        //chartRange.EntireColumn.ColumnWidth = 26;

        //chartRange = xlWorkSheet.get_Range("c1", "c1");
        //chartRange.EntireColumn.ColumnWidth = 20;


        //chartRange.EntireColumn.Font.Size = 11.0;


        //chartRange.EntireColumn.Font.Size = 11.0;


        DateTime reportFromDate = DateTime.Now.AddDays(-1);
        DateTime reportToDate = DateTime.Now.AddDays(-1);
        
        
        string strReportFromDate = string.Empty;
        string strReportToDate = string.Empty;


        bool blnMultiDayReport = false;
       



        string strFileName = "DSR";


        if (type == "distributionList")
        {
            //fromDate = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
            //reportFromDate = Convert.ToDateTime(fromDate); 
            //toDate = string.Format("{0:MM/dd/yyyy}",DateTime.Now);

            reportFromDate = DateTime.Now;
           reportToDate = DateTime.Now;


        }


        else if (GenUtils.IsDate(toDate) && GenUtils.IsDate(fromDate)  && fromDate != toDate)
        {
            blnMultiDayReport = true;
            reportFromDate = Convert.ToDateTime(fromDate);
            strReportToDate = string.Format("{0:MM/dd/yyyy}", toDate);
            xlWorkSheet.Cells[rowCounter, 1] = "Sales Recap";
            strFileName = "SR";
            //blIncludeSnapShot = t

        }

        else if (GenUtils.IsDate(fromDate)) //----------- valid from date for single day report
        {
            reportFromDate = Convert.ToDateTime(fromDate);
            strFileName = "1DaySR";
        }

        else //------- today modified for weekends - default mode
        {

            switch (reportFromDate.DayOfWeek.ToString())
            {
                case "Sunday":
                    reportFromDate = reportFromDate.AddDays(-2);
                    break;
                case "Saturday":
                    reportFromDate = reportFromDate.AddDays(-1);
                    break;
            }

        }

        strReportFromDate = string.Format("{0:MM/dd/yyyy}", reportFromDate);




        //--------------------------------- remember we can't cross a fiscal year
        int dayofYear = reportFromDate.DayOfYear;
        Decimal decTodaysFactor = dayofYear / 365m;

        string strDateRange = string.Format("{0:dddd, MMMM dd, yyyy}", reportFromDate);

        if (type == "distributionList")
        {
            strDateRange = string.Format("{0:dddd, MMMM dd, yyyy}", reportFromDate)  + " as of 5:00 P.M. Pacific Time";
        }
         else if (blnMultiDayReport)
        {
            strDateRange = string.Format("{0:MM/dd/yyyy}", strReportFromDate) + " through " + string.Format("{0:MM/dd/yyyy}", strReportToDate);
        }
        else
        
        {
            strDateRange = string.Format("{0:dddd, MMMM dd, yyyy}", reportFromDate);
        }




        

        

        string thisQuarter = "1st Quarter";
        int intQuarter = 1;

        string fiscalYear = reportFromDate.Year.ToString();


        if (reportFromDate.Month > 3 && reportFromDate.Month < 7)
        {
            thisQuarter = "2nd Quarter";
            intQuarter = 2;
        }
        else if (reportFromDate.Month > 6 && reportFromDate.Month < 10)
        {
            thisQuarter = "3rd Quarter";
            intQuarter = 3;

        }
        else if (reportFromDate.Month > 9 && reportFromDate.Month < 13)
        {
            thisQuarter = "4th Quarter";
            intQuarter = 4;

        }






        

        string strRolling12FromDate = reportFromDate.AddYears(-1).Month + "/" + reportFromDate.AddYears(-1).Day + "/" + reportFromDate.AddYears(-1).Year;
        string strRolling12ToDate = strReportFromDate;



        string strRolling12_24FromDate = reportFromDate.AddYears(-2).Month + "/" + reportFromDate.AddYears(-2).Day + "/" + reportFromDate.AddYears(-2).Year;


        string strRolling24_36FromDate = reportFromDate.AddYears(-3).Month + "/" + reportFromDate.AddYears(-3).Day + "/" + reportFromDate.AddYears(-3).Year; 


        ////int intOrderedTodayQtyRow = 0;
        //int intOrderedTodayDollarsRow = 0;
        ////int intShippedTodayQtyRow = 0;
        //int intShippedTodayDollarsRow = 0;


        //********************************************************** SnapShopt Page 2
        xlWorkSheet2.Select(Missing.Value);

        
        range = xlWorkSheet2.get_Range("A1", "A1");
        strCellData = range.Text.ToString();
   


        if (blIncludeSnapShot && strCellData != string.Empty)
        {
            //-------- this page should be good to go
        }
        else if (blIncludeSnapShot)
        {

            xlWorkSheet2.Cells[rowCounter, 1] = "Current Positions";

            xlWorkSheet2.Name = "Current Positions";


            chartRange = xlWorkSheet2.get_Range("A1", "Z1");
            chartRange.EntireColumn.ColumnWidth = 14;

            chartRange = xlWorkSheet2.get_Range("B1", "b1");
            chartRange.EntireColumn.ColumnWidth = 26;

            chartRange = xlWorkSheet2.get_Range("c1", "c1");
            chartRange.EntireColumn.ColumnWidth = 20;





            chartRange = xlWorkSheet2.get_Range("a" + rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.Size = 24.0;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

            xlWorkSheet2.Cells[++rowCounter, 1] = strDateRange;
            chartRange = xlWorkSheet2.get_Range("a" + rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.Size = 16.0;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            chartRange.Interior.ColorIndex = 37;




            xlWorkSheet2.Cells[rowCounter, 20] = thisQuarter;




            xlWorkSheet2.Cells[++rowCounter, 1] = "SnapShot";
            chartRange = xlWorkSheet2.get_Range("a" + rowCounter, "c" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Size = 16.0;
            chartRange.Font.ColorIndex = 2;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            chartRange.Interior.ColorIndex = 11;



            //add totals


            decimal decWeekInvoiced = CalculateSalesForPeriod("AllSalesByPeriod", "week", reportFromDate, string.Empty, reportToDate);
            decimal decWeekOrdered = CalculateSalesForPeriod("AllOrdersByPeriod", "week", reportFromDate, string.Empty, reportToDate);




            //xlWorkSheet.Cells[++rowCounter, 1] = "Ordered Today Qty [" + string.Format("{0:MM/dd/yyyy}", strReportFromDate) + "]";
            //intOrderedTodayQtyRow = rowCounter;


            xlWorkSheet2.Cells[++rowCounter, 1] = "Ordered Today Total";
            intOrderedTodayDollarsRow = rowCounter;


            //xlWorkSheet.Cells[++rowCounter, 1] = "Shipped Today Qty";
            //intShippedTodayQtyRow = rowCounter;


            xlWorkSheet2.Cells[++rowCounter, 1] = "Shipped Today Total";
            intShippedTodayDollarsRow = rowCounter;


            rowCounter++;



            xlWorkSheet2.Cells[++rowCounter, 1] = "Week To Date Ordered Total";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decWeekOrdered);




            xlWorkSheet2.Cells[++rowCounter, 1] = "Week To Date Shipped Total";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decWeekInvoiced);

            decimal decWeekForecast = loadForecast("week", string.Empty, fiscalYear, reportFromDate);
            decimal decNumberofDays = loadForecast("week-workdays", string.Empty, fiscalYear, reportFromDate);

            int dow = ((int)reportFromDate.DayOfWeek);


           


            decimal decWeekWTDVariance = 0m;
            decimal prorateFactor = Convert.ToDecimal(dow) / decNumberofDays;

            if (decWeekForecast > 1 && prorateFactor > 0)
            {
                //decWeekWTDVariance = 100 - ((decWeekInvoiced / (decWeekForecast * prorateFactor)) * 100);
                decWeekWTDVariance = 100 - ((decWeekOrdered / (decWeekForecast * prorateFactor)) * 100);
                decWeekWTDVariance = Math.Round(0 - decWeekWTDVariance, 2);
            }

          


            xlWorkSheet2.Cells[++rowCounter, 1] = "Forecast for the Week";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decWeekForecast);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Prorated Forecast";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decWeekForecast * prorateFactor);

            xlWorkSheet2.Cells[++rowCounter, 1] = "Variance";
            xlWorkSheet2.Cells[rowCounter, 3] = decWeekWTDVariance + "%";
            if (decWeekWTDVariance < 0)
            {
                chartRange = xlWorkSheet2.get_Range("c" + rowCounter, "c" + rowCounter);
                chartRange.Font.ColorIndex = 3;
            }
            else
            {
                chartRange = xlWorkSheet2.get_Range("c" + rowCounter, "c" + rowCounter);
                chartRange.Font.ColorIndex = 10;
            }




            decimal decQrtrInvoiced = CalculateSalesForPeriod("AllSalesByPeriod", "quarter", reportFromDate, string.Empty, reportToDate);
            decimal decQrtrOrdered = CalculateSalesForPeriod("AllOrdersByPeriod", "quarter", reportFromDate, string.Empty, reportToDate);

            decimal decQtrForecast = loadForecast("quarter", intQuarter.ToString(), fiscalYear, reportFromDate);

            decimal decQrtrVariance = 0m;
            if (decQtrForecast > 1)
            {
                //decQrtrVariance = 100 - ((decQrtrInvoiced / decQtrForecast) * 100);
                decQrtrVariance = 100 - ((decQrtrOrdered / decQtrForecast) * 100);
                decQrtrVariance = Math.Round(0 - decQrtrVariance, 2);
            }



            string qtrBeg = BeginningofQuarter(reportFromDate);
            DateTime dtqtrBeg = Convert.ToDateTime(qtrBeg);
            DateTime lastqtrmonth = dtqtrBeg.AddMonths(3);
            DateTime lastdayofqtr = lastqtrmonth.AddDays(-1);
            //lastdayofqtr = lastdayofqtr.AddDays(-1);
            TimeSpan daysinQuarter = lastdayofqtr - dtqtrBeg;
            TimeSpan daysused = reportFromDate - dtqtrBeg;

            decimal thisdaysUsed = Convert.ToDecimal(daysused.Days);
            decimal thisDaysinQtr = Convert.ToDecimal(daysinQuarter.Days);


            prorateFactor = (thisdaysUsed / thisDaysinQtr);
            decimal decAdjustedQtrForecast = decQtrForecast * prorateFactor;


            decimal decQrtrQTDVariance = 0m;
            if (decQtrForecast > 1)
            {
                //decQrtrQTDVariance = 100 - ((decQrtrInvoiced / decAdjustedQtrForecast) * 100);
                decQrtrQTDVariance = 100 - ((decQrtrOrdered / decAdjustedQtrForecast) * 100);
                decQrtrQTDVariance = Math.Round(0 - decQrtrQTDVariance, 2);
            }



            rowCounter++;
            xlWorkSheet2.Cells[++rowCounter, 1] = "Quarter to Date Ordered";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decQrtrOrdered);



            xlWorkSheet2.Cells[++rowCounter, 1] = "Quarter to Date Shipped";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decQrtrInvoiced);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Prorated Forecast";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decQtrForecast * prorateFactor);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Forecast for the Quarter";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decQtrForecast);




            xlWorkSheet2.Cells[++rowCounter, 1] = "Variance";
            xlWorkSheet2.Cells[rowCounter, 3] = decQrtrQTDVariance + "%";
            if (decQrtrQTDVariance < 0)
            {
                chartRange = xlWorkSheet2.get_Range("c" + rowCounter, "c" + rowCounter);
                chartRange.Font.ColorIndex = 3;
            }
            else
            {
                chartRange = xlWorkSheet2.get_Range("c" + rowCounter, "c" + rowCounter);
                chartRange.Font.ColorIndex = 10;
            }



            decimal decYearInvoiced = CalculateSalesForPeriod("AllSalesByPeriod", "year", reportFromDate, string.Empty, reportToDate);
            decimal decYearOrdered = CalculateSalesForPeriod("AllOrdersByPeriod", "year", reportFromDate, string.Empty, reportToDate);

            decimal decYearBudget = loadForecast("year", "", fiscalYear, reportFromDate);

            decimal decYearVariance = 0m;
            if (decYearBudget > 1)
            {
                //decYearVariance = 100 - ((decYearInvoiced / decYearBudget) * 100);
                decYearVariance = 100 - ((decYearOrdered / decYearBudget) * 100);
                decYearVariance = Math.Round(0 - decYearVariance, 2);

            }



            decimal decYearADJVariance = 0m;
            if (decYearBudget > 1)
            {
                //decYearADJVariance = 100 - ((decYearInvoiced / (decYearBudget * decTodaysFactor) * 100));
                decYearADJVariance = 100 - ((decYearOrdered / (decYearBudget * decTodaysFactor) * 100));
                decYearADJVariance = Math.Round(0 - decYearADJVariance, 2);

            }


            rowCounter++;
            xlWorkSheet2.Cells[++rowCounter, 1] = "Year to Date Ordered";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decYearOrdered);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Year to Date Shipped";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decYearInvoiced);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Prorated Forecast";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decYearBudget * decTodaysFactor);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Yearly Forecast";
            xlWorkSheet2.Cells[rowCounter, 3] = String.Format("{0:C}", decYearBudget);


            xlWorkSheet2.Cells[++rowCounter, 1] = "Annualized Variance";
            xlWorkSheet2.Cells[rowCounter, 3] = decYearADJVariance + "%";
            if (decYearADJVariance < 0)
            {
                chartRange = xlWorkSheet2.get_Range("c" + rowCounter, "c" + rowCounter);
                chartRange.Font.ColorIndex = 3;
            }
            else
            {
                chartRange = xlWorkSheet2.get_Range("c" + rowCounter, "c" + rowCounter);
                chartRange.Font.ColorIndex = 10;
            }






            chartRange = xlWorkSheet2.get_Range("a4", "a" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Size = 10.0;
            chartRange.Font.Underline = true;

        } //----- end of include snapshot page 2


    




        //---------------------- Build the Weekly Sheet
      
        xlWorkSheet3.Select(Missing.Value);
        range = xlWorkSheet3.get_Range("A1", "A1");
        strCellData = range.Text.ToString();

        if (blIncludeWeeklyRecap && strCellData != string.Empty)
        {
            //-------- this page should be good to go
        }
      
        else if (blIncludeWeeklyRecap)
        {
            buildWeeklyRecap(xlWorkSheet3, fiscalYear);



            myCell = null;


            xlWorkSheet3.Select(Missing.Value);
            myCell = (Excel.Range)xlWorkSheet3.Cells[3, 1];
            myCell.Activate();
            myCell.Application.ActiveWindow.FreezePanes = true;


        }







        Decimal decshippedExt = 0m;
        Decimal decorderdExt = 0m;
        Decimal thisPrice = 0;
        int thisShipped = 0;
        int thisOrdered = 0;


        Decimal decCompanyYTDSales = 0m;
        Decimal decCompanyYearForecast = 0m;
        Decimal decCompanyYTDForecast = 0m;
        Decimal decCompanyYearVariance = 0m;
        Decimal decCompanyYTDVariance = 0m;

        Decimal decPartYTDSales = 0m;
        Decimal decCompanyRolling12Sales = 0m;
        Decimal decPartYearForecastQty = 0m;
        Decimal decPartYearForecastDollars = 0m;
        Decimal decPartYearVariance = 0m;
        Decimal decPartYTDForecastDollars = 0m;
        Decimal decPartYTDVariance = 0m;

        Decimal decRolling12Qty = 0m;
        Decimal decRolling12Dollars = 0m;
        Decimal decCompanyRolling12_24Sales = 0m;
        Decimal decCompanyRolling24_36Sales = 0m;
        Decimal decCompanyRollingtrend = 0m;


        Decimal decQtyOrderedTotal = 0m;
        Decimal decQtyShippedTotal = 0m;
        Decimal decTotalOrderedTotal = 0m;
        Decimal decTotalShippedTotal = 0m;

        Decimal decMyQtyOrderedTotal = 0m;
        Decimal decMyQtyShippedTotal = 0m;
        Decimal decMyTotalOrderedTotal = 0m;
        Decimal decMyTotalShippedTotal = 0m;


        int intRepsCount = 0;
        int intCompanies = 0;
        int intUniquePartNumbers = 0;

        int intMyCompanies = 0;
        int intMyUniquePartNumbers = 0;

        string[] strPlacedPartNumbers = new string[1000];


        string strCurRep = string.Empty;
        string strCurCompany = string.Empty;
        string strCurCompanyKey = string.Empty;
        bool blnIncrementrow = false;
        bool showColorBar = false;
        int intDailyTopRow = rowCounter;


        bool blthisPartMatch = false;
        int thisPartMatchTop = -1;

        bool blnPrintThisRow = false;
        i = 0;




        rowCounter = 1;

        xlWorkSheet.Select(Missing.Value);
     
        range = xlWorkSheet.get_Range("A1", "A1");
        strCellData = range.Text.ToString();

        //******************************************** build the Daily Sales Report
        if (blIncludeDailySales && strCellData != string.Empty && strLastSalesReps == SalesReps)
        {
            //-------- this page should be good to go
        }
        else if (blIncludeDailySales)
        {
            chartRange = xlWorkSheet.get_Range("A1", "Z1000");
            chartRange.Rows.Delete(Missing.Value);


            

            xlWorkSheet.Name = "Daily Sales Report";


            chartRange = xlWorkSheet.get_Range("A1", "Z1");
            chartRange.EntireColumn.ColumnWidth = 14;

            chartRange = xlWorkSheet.get_Range("B1", "b1");
            chartRange.EntireColumn.ColumnWidth = 26;

            chartRange = xlWorkSheet.get_Range("c1", "c1");
            chartRange.EntireColumn.ColumnWidth = 20;



            chartRange = xlWorkSheet.get_Range("a" + rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.Size = 24.0;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

            xlWorkSheet.Cells[++rowCounter, 1] = strDateRange;
            chartRange = xlWorkSheet.get_Range("a" + rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.Size = 16.0;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            chartRange.Interior.ColorIndex = 37;


            //------------------------------------- Label Row

            xlWorkSheet.Cells[++rowCounter, 1] = "Daily Sales Details By Rep / Customer";
            chartRange = xlWorkSheet.get_Range("a" + rowCounter, "i" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.Size = 16.0;
            chartRange.Font.ColorIndex = 2;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            chartRange.Interior.ColorIndex = 15;


            xlWorkSheet.Cells[rowCounter, 10] = "Customer Performance";
            chartRange = xlWorkSheet.get_Range("j" + rowCounter, "q" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.Size = 16.0;
            chartRange.Font.ColorIndex = 2;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            chartRange.Interior.ColorIndex = 16;


            xlWorkSheet.Cells[rowCounter, 18] = "Parts Performance";
            chartRange = xlWorkSheet.get_Range("r" + rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = false;
            chartRange.Font.ColorIndex = 2;
            chartRange.Font.Size = 16.0;
            chartRange.Merge(false);
            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            chartRange.Interior.ColorIndex = 15;



            //column titles

            xlWorkSheet.Cells[++rowCounter, colSalesRep] = "Sales Rep";
            xlWorkSheet.Cells[rowCounter, colCompany] = "Company Name";
            xlWorkSheet.Cells[rowCounter, colPartNo] = "Part Number";
            xlWorkSheet.Cells[rowCounter, colQtyOrdered] = "Order";





            xlWorkSheet.Cells[rowCounter, colQtyShipped] = "Ship";
            xlWorkSheet.Cells[rowCounter, colUnitPrice] = "Unit Price";
            xlWorkSheet.Cells[rowCounter, colTotalOrdered] = "Total Ordered";
            xlWorkSheet.Cells[rowCounter, colTotalShipped] = "Total Shipped";



            xlWorkSheet.Cells[rowCounter, colCustomerDueDate] = "Due Date";



            xlWorkSheet.Cells[rowCounter, colCompanyYTDSales] = "YTD Sales";

            xlWorkSheet.Cells[rowCounter, colCompanyListRolling24_36Sales] = "24 to 36  Sales";

            xlWorkSheet.Cells[rowCounter, colCompanyListRolling12_24Sales] = "12 to 24  Sales";


            xlWorkSheet.Cells[rowCounter, colCompanyRolling12Sales] = "Last 12 Sales";


            xlWorkSheet.Cells[rowCounter, colCompanyListRollingTrend] = "Trend";



            xlWorkSheet.Cells[rowCounter, colCompanyYearForecast] = "Year Forecast";

            xlWorkSheet.Cells[rowCounter, colCompanyYTDForecast] = "YTD Forecast";
            xlWorkSheet.Cells[rowCounter, colCompanyYTDVariance] = "YTD Variance";

            xlWorkSheet.Cells[rowCounter, colPartYTDSales] = "YTD Sales $";

            xlWorkSheet.Cells[rowCounter, colPartYearForecastQty] = "Year Frcst QTY";
            xlWorkSheet.Cells[rowCounter, colPartYearForecastDollars] = "Year Frcst Sales";

            xlWorkSheet.Cells[rowCounter, colPartRolling12Qty] = "Rolling 12 Qty [comp]";
            xlWorkSheet.Cells[rowCounter, colPartRolling12Dollars] = "Rolling 12 Sales [comp]";


            xlWorkSheet.Cells[rowCounter, colPartYTDForecast] = "YTD Forecast";
            xlWorkSheet.Cells[rowCounter, colPartYTDVariance] = "YTD Variance";



            chartRange = xlWorkSheet.get_Range("a" + rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Underline = true;
            chartRange.Font.Size = 9.0;

            chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter; ;




            //----the data
            DataTable dtSales = loadSalesOrders(strReportFromDate, strReportToDate, "", "", "reportForDate");



            strCurCompanyKey = dtSales.Rows[0]["CMP_AUTO_KEY"].ToString();
            string strCurCompanyCode = string.Empty;

            GenUtils.PrintTableOrView(dtSales, "");

            //int LPCNTER = 0;





            foreach (DataRow myRow in dtSales.Rows)
            {



                //Debug.WriteLine(LPCNTER++);

                thisShipped = Convert.ToInt16(myRow["QTY_INVOICED"]);
                thisOrdered = Convert.ToInt16(myRow["QTY_ORDERED"]);
                thisPrice = Convert.ToDecimal(myRow["UNIT_PRICE"]);
                decshippedExt = thisShipped * thisPrice;
                decorderdExt = thisOrdered * thisPrice;

                decQtyOrderedTotal += thisOrdered;
                decQtyShippedTotal += thisShipped;
                decTotalOrderedTotal += decorderdExt;
                decTotalShippedTotal += decshippedExt;


                blnIncrementrow = false;
                showColorBar = false;
                decCompanyYearForecast = 0m;
                decCompanyYTDForecast = 0m;
                decCompanyYTDVariance = 0m;
                decCompanyYearVariance = 0m;



                blnPrintThisRow = true;

                if (SalesReps.Trim() != string.Empty)
                {
                    blnPrintThisRow = false;
                    for (i = 0; i <= nSalesReps.GetUpperBound(0); i++)
                    {
                        if (nSalesReps[i].Trim().ToLower() == myRow["SALESPERSON_CODE"].ToString().Trim().ToLower())
                        {
                            blnPrintThisRow = true;
                            break;
                        }
                    }

                }




                //---------------- check if unique part number
                int plp = 0;
                blthisPartMatch = false;
                while (strPlacedPartNumbers[plp] != null)
                {

                    if (myRow["PN"].ToString() == strPlacedPartNumbers[plp])
                    {
                        blthisPartMatch = true;
                        break;
                    }
                    plp++;

                }




                if (!blthisPartMatch)
                {
                    strPlacedPartNumbers[++thisPartMatchTop] = myRow["PN"].ToString();
                    intUniquePartNumbers++;

                    //intCompanies++;
                    //if (blnPrintThisRow)
                    //{
                    intMyUniquePartNumbers++;
                    //}
                }




                if (strCurRep != myRow["SALESPERSON_NAME"].ToString() || strCurRep == string.Empty)
                {

                    intRepsCount++;

                    if (strCurRep != string.Empty)
                    {
                        blnIncrementrow = true;
                    }
                    showColorBar = true;


                    //---------------- print the total bar for the sales rep





                    //------------------- Print My totals
                    //if (SalesReps.Trim() != string.Empty)
                    //{
                    if (blnPrintThisRow && strCurRep != string.Empty)
                    {
                        chartRange = xlWorkSheet.get_Range("a" + ++rowCounter, "x" + rowCounter);
                        chartRange.Font.Bold = true;
                        chartRange.Font.Size = 9.0;
                        chartRange.Interior.ColorIndex = 19;
                        chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

                        xlWorkSheet.Cells[rowCounter, colSalesRep] = strCurRep + " Totals";
                        xlWorkSheet.Cells[rowCounter, colCompany] = String.Format("{0:g}", intMyCompanies + " Companies");
                        xlWorkSheet.Cells[rowCounter, colPartNo] = String.Format("{0:g}", intMyUniquePartNumbers + " Unique Part Numbers");



                        xlWorkSheet.Cells[rowCounter, colQtyOrdered] = String.Format("{0:g}", decMyQtyOrderedTotal);
                        xlWorkSheet.Cells[rowCounter, colQtyShipped] = String.Format("{0:g}", decMyQtyShippedTotal);
                        xlWorkSheet.Cells[rowCounter, colTotalOrdered] = String.Format("{0:C}", decMyTotalOrderedTotal);
                        xlWorkSheet.Cells[rowCounter--, colTotalShipped] = String.Format("{0:C}", decMyTotalShippedTotal);



                        decMyQtyOrderedTotal = 0m;
                        decMyQtyShippedTotal = 0m;
                        decMyTotalOrderedTotal = 0m;
                        decMyTotalShippedTotal = 0m;
                        intMyCompanies = 0;
                        intMyUniquePartNumbers = 0;


                        // }
                    }





                }







                if (strCurCompany != myRow["BILL_NAME"].ToString() && strCurCompany != string.Empty)
                {


                    if (intCompanies == 1)
                    {
                        strCompanyCodes += strCurCompanyCode;
                        blnCompanyCodesPlaced = true;
                    }


                    intCompanies++;


                    //if (blnPrintThisRow)
                    //{
                    intMyCompanies++;

                    if (blnCompanyCodesPlaced)
                    {
                        strCompanyCodes += ",";
                    }
                    else
                    {
                        blnCompanyCodesPlaced = true;
                    }

                    strCompanyCodes += myRow["COMPANY_CODE"].ToString();



                    //}



                    decCompanyYTDSales = CalculateSalesForPeriod("SalesByCompany", "year", reportFromDate, strCurCompanyKey, reportToDate);

                    decCompanyYearForecast = loadForecast("company", strCurCompany, fiscalYear, reportFromDate);


                    decCompanyRolling12Sales = CalculateSalesForPeriod("SalesByCompany", "specifiedRange", Convert.ToDateTime(strRolling12FromDate), strCurCompanyKey, Convert.ToDateTime(strRolling12ToDate));


                    decCompanyRolling12_24Sales = CalculateSalesForPeriod("SalesByCompany", "specifiedRange", Convert.ToDateTime(strRolling12_24FromDate), strCurCompanyKey, Convert.ToDateTime(strRolling12FromDate));

                    decCompanyRolling24_36Sales = CalculateSalesForPeriod("SalesByCompany", "specifiedRange", Convert.ToDateTime(strRolling24_36FromDate), strCurCompanyKey, Convert.ToDateTime(strRolling12_24FromDate));





                    if (decCompanyRolling12Sales != 0m)
                    {
                        xlWorkSheet.Cells[rowCounter, colCompanyRolling12Sales] = string.Format("{0:C}", decCompanyRolling12Sales);
                    }





                    if (decCompanyRolling12_24Sales != 0m)
                    {
                        xlWorkSheet.Cells[rowCounter, colCompanyListRolling12_24Sales] = string.Format("{0:C}", decCompanyRolling12_24Sales);
                    }

                    if (decCompanyRolling24_36Sales != 0m)
                    {
                        xlWorkSheet.Cells[rowCounter, colCompanyListRolling24_36Sales] = string.Format("{0:C}", decCompanyRolling24_36Sales);
                    }

                    decimal dec2Trend = -9999m;
                    decimal dec1Trend = -9999m;

                    if (decCompanyRolling24_36Sales != 0 && decCompanyRolling12_24Sales != 0)
                    {
                        dec2Trend = decCompanyRolling12_24Sales / decCompanyRolling24_36Sales;
                    }


                    if (decCompanyRolling12Sales != 0m && decCompanyRolling12_24Sales != 0m)
                    {
                        dec1Trend = decCompanyRolling12Sales / decCompanyRolling12_24Sales;
                    }

                    if (dec2Trend != -9999 && dec1Trend != -9999)
                    {
                        decCompanyRollingtrend = (((dec1Trend * 2) + dec2Trend) / 3);
                    }
                    else if (dec1Trend != -9999)
                    {
                        decCompanyRollingtrend = dec1Trend;
                    }





                    if (decCompanyRollingtrend != 0m)
                    {
                        xlWorkSheet.Cells[rowCounter, colCompanyListRollingTrend] = string.Format("{0:g}", Math.Round(decCompanyRollingtrend, 2));


                        if (decCompanyRollingtrend < 1m)
                        {
                            chartRange = xlWorkSheet.get_Range("q" + rowCounter, "q" + rowCounter);
                            chartRange.Font.ColorIndex = 3;
                        }
                        else
                        {
                            chartRange = xlWorkSheet.get_Range("q" + rowCounter, "q" + rowCounter);
                            chartRange.Font.ColorIndex = 10;
                        }
                    }


















                    decCompanyYTDForecast = 0m;
                    if (decCompanyYearForecast > 0)
                    {
                        decCompanyYTDForecast = Math.Round(decCompanyYearForecast * decTodaysFactor, 2);
                    }

                    decCompanyYTDVariance = 0m;
                    if (decCompanyYTDForecast > 1)
                    {
                        decCompanyYTDVariance = 100 - ((decCompanyYTDSales / decCompanyYTDForecast) * 100);
                        decCompanyYTDVariance = Math.Round(0 - decCompanyYTDVariance, 1);
                    }


                    decCompanyYearVariance = 0m;
                    if (decCompanyYearForecast > 1)
                    {
                        decCompanyYearVariance = 100 - ((decCompanyYTDSales / decCompanyYearForecast) * 100);
                        decCompanyYearVariance = Math.Round(0 - decCompanyYearVariance, 1);

                    }



                    xlWorkSheet.Cells[rowCounter, colCompanyYTDSales] = string.Format("{0:C}", decCompanyYTDSales);
                    xlWorkSheet.Cells[rowCounter, colCompanyYearForecast] = string.Format("{0:C}", decCompanyYearForecast);



                    xlWorkSheet.Cells[rowCounter, colCompanyYTDForecast] = string.Format("{0:C}", decCompanyYTDForecast);
                    xlWorkSheet.Cells[rowCounter, colCompanyYTDVariance] = string.Format("{0:g}", decCompanyYTDVariance) + "%";
                    if (decCompanyYTDVariance < 0)
                    {
                        chartRange = xlWorkSheet.get_Range("m" + rowCounter, "m" + rowCounter);
                        chartRange.Font.ColorIndex = 3;
                    }
                    else
                    {
                        chartRange = xlWorkSheet.get_Range("m" + rowCounter, "m" + rowCounter);
                        chartRange.Font.ColorIndex = 10;
                    }

                    if (strCurCompany != string.Empty)
                    {
                        blnIncrementrow = true;
                    }
                }



                if (blnIncrementrow && blnPrintThisRow)
                {
                    rowCounter++;
                }





                strCurCompanyKey = myRow["CMP_AUTO_KEY"].ToString();
                strCurCompanyCode = myRow["COMPANY_CODE"].ToString();

                if (blnPrintThisRow)
                {

                    if (strCurRep == string.Empty || strCurRep != myRow["SALESPERSON_NAME"].ToString())
                    {
                        xlWorkSheet.Cells[++rowCounter, 1] = myRow["SALESPERSON_NAME"].ToString();
                        strCurRep = myRow["SALESPERSON_NAME"].ToString();

                        if (blnRepsFirstPlaced)
                        {
                            strSelectedReps += ", ";
                        }
                        else
                        {
                            blnRepsFirstPlaced = true;
                        }

                        strSelectedReps += myRow["SALESPERSON_NAME"].ToString();

                    }
                    else
                    {
                        xlWorkSheet.Cells[++rowCounter, colSalesRep] = string.Empty;
                    }
                    if (strCurCompany == string.Empty || strCurCompany != myRow["BILL_NAME"].ToString())
                    {

                        xlWorkSheet.Cells[rowCounter, colCompany] = myRow["BILL_NAME"].ToString();
                        strCurCompany = myRow["BILL_NAME"].ToString();
                        strCurCompanyCode = myRow["COMPANY_CODE"].ToString();



                    }


                    if (showColorBar)
                    {

                        chartRange = xlWorkSheet.get_Range("A" + rowCounter, "x" + rowCounter);
                        chartRange.Interior.ColorIndex = 15;
                    }

                }


                decPartYTDSales = CalculateSalesForPeriod("PartsSoldByPeriod", "year", reportFromDate, myRow["PNM_AUTO_KEY"].ToString(), reportToDate);

                decPartYearForecastQty = loadForecast("part", myRow["PN"].ToString(), fiscalYear, reportFromDate);
                decPartYearForecastDollars = decPartYearForecastQty * Convert.ToDecimal(myRow["UNIT_PRICE"].ToString());
                decPartYearForecastDollars = Math.Round(decPartYearForecastDollars, 2);






                decRolling12Qty = getPartSalesPerPeriod(myRow["PN"].ToString(), strCurCompanyKey, strRolling12FromDate, strRolling12ToDate, "qty");

                decRolling12Dollars = getPartSalesPerPeriod(myRow["PN"].ToString(), strCurCompanyKey, strRolling12FromDate, strRolling12ToDate, "dollars");




                //--------------- get the rolling 12 month sales for part and part / this company


                decPartYTDForecastDollars = 0m;
                if (decPartYearForecastDollars > 0)
                {
                    decPartYTDForecastDollars = Math.Round(decPartYearForecastDollars * decTodaysFactor, 2);
                }


                decPartYTDVariance = 0m;
                if (decPartYTDForecastDollars > 1)
                {
                    decPartYTDVariance = 100 - ((decPartYTDSales / decPartYTDForecastDollars) * 100);
                    decPartYTDVariance = Math.Round(0 - decPartYTDVariance, 1);

                }


                decPartYearVariance = 0m;
                if (decPartYearForecastDollars > 1)
                {
                    decPartYearVariance = 100 - ((decPartYTDSales / decPartYearForecastDollars) * 100);
                    decPartYearVariance = Math.Round(0 - decPartYearVariance, 1);

                }









                if (blnPrintThisRow)
                {


                    //------ do MyTotals
                    decMyQtyOrderedTotal += thisOrdered;
                    decMyQtyShippedTotal += thisShipped;
                    decMyTotalOrderedTotal += decorderdExt;
                    decMyTotalShippedTotal += decshippedExt;





                    xlWorkSheet.Cells[rowCounter, colPartNo] = myRow["PN"].ToString();
                    xlWorkSheet.Cells[rowCounter, colQtyOrdered] = myRow["QTY_ORDERED"].ToString();
                    xlWorkSheet.Cells[rowCounter, colQtyShipped] = myRow["QTY_INVOICED"].ToString();
                    xlWorkSheet.Cells[rowCounter, colUnitPrice] = String.Format("{0:C}", Convert.ToDecimal(myRow["UNIT_PRICE"].ToString()));
                    xlWorkSheet.Cells[rowCounter, colTotalOrdered] = String.Format("{0:C}", decorderdExt);
                    xlWorkSheet.Cells[rowCounter, colTotalShipped] = String.Format("{0:C}", decshippedExt);
                    xlWorkSheet.Cells[rowCounter, colCustomerDueDate] = String.Format("{0:MM/dd/yyyy}", myRow["DELIVERY_DATE"]);



                    xlWorkSheet.Cells[rowCounter, colPartYTDSales] = String.Format("{0:C}", decPartYTDSales);
                    xlWorkSheet.Cells[rowCounter, colPartYearForecastQty] = String.Format("{0:g}", decPartYearForecastQty);


                    xlWorkSheet.Cells[rowCounter, colPartYearForecastDollars] = String.Format("{0:C}", decPartYearForecastDollars);

                    xlWorkSheet.Cells[rowCounter, colPartRolling12Qty] = String.Format("{0:g}", decRolling12Qty);

                    xlWorkSheet.Cells[rowCounter, colPartRolling12Dollars] = String.Format("{0:C}", decRolling12Dollars);



                    xlWorkSheet.Cells[rowCounter, colPartYTDForecast] = String.Format("{0:C}", decPartYTDForecastDollars);
                    xlWorkSheet.Cells[rowCounter, colPartYTDVariance] = String.Format("{0:g}", decPartYTDVariance) + "%";
                    if (decPartYTDVariance < 0)
                    {
                        chartRange = xlWorkSheet.get_Range("v" + rowCounter, "v" + rowCounter);
                        chartRange.Font.ColorIndex = 3;
                    }
                    else
                    {
                        chartRange = xlWorkSheet.get_Range("v" + rowCounter, "v" + rowCounter);
                        chartRange.Font.ColorIndex = 10;
                    }

                }




            } //---------------------- if print this row





            decCompanyYearForecast = 0m;
            decCompanyYTDForecast = 0m;
            decCompanyYTDVariance = 0m;
            decCompanyYearVariance = 0m;
            //----- show the last totals
            if (strCurCompanyKey != string.Empty)
            {
                decCompanyYTDSales = CalculateSalesForPeriod("SalesByCompany", "year", reportFromDate, strCurCompanyKey, reportToDate);
                decCompanyYearForecast = loadForecast("company", strCurCompany, fiscalYear, reportFromDate);

                decCompanyYTDForecast = 0m;
                if (decCompanyYearForecast > 0)
                {
                    decCompanyYTDForecast = Math.Round(decCompanyYearForecast * decTodaysFactor, 2);
                }

                decCompanyYTDVariance = 0m;
                if (decCompanyYTDForecast > 1)
                {
                    decCompanyYTDVariance = 100 - ((decCompanyYTDSales / decCompanyYTDForecast) * 100);
                    decCompanyYTDVariance = Math.Round(0 - decCompanyYTDVariance, 1);
                }


                decCompanyYearVariance = 0m;
                if (decCompanyYearForecast > 1)
                {
                    decCompanyYTDVariance = 100 - ((decCompanyYTDSales / decCompanyYTDForecast) * 100);
                    decCompanyYTDVariance = Math.Round(0 - decCompanyYTDVariance, 1);

                }



                xlWorkSheet.Cells[rowCounter, colCompanyYTDSales] = string.Format("{0:C}", decCompanyYTDSales);
                xlWorkSheet.Cells[rowCounter, colCompanyYearForecast] = string.Format("{0:C}", decCompanyYearForecast);

                xlWorkSheet.Cells[rowCounter, colCompanyYTDForecast] = string.Format("{0:C}", decCompanyYTDForecast);
                xlWorkSheet.Cells[rowCounter, colCompanyYTDVariance] = string.Format("{0:g}", decCompanyYTDVariance) + "%";
                if (decCompanyYTDVariance < 0)
                {
                    chartRange = xlWorkSheet.get_Range("m" + rowCounter, "m" + rowCounter);
                    chartRange.Font.ColorIndex = 3;
                }
                else
                {
                    {
                        chartRange = xlWorkSheet.get_Range("m" + rowCounter, "m" + rowCounter);
                        chartRange.Font.ColorIndex = 10;
                    }
                }


            }








            chartRange = xlWorkSheet.get_Range("j" + intDailyTopRow, "j" + rowCounter);
            chartRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
            chartRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlBorderWeight.xlThick;


            chartRange = xlWorkSheet.get_Range("q" + intDailyTopRow, "q" + rowCounter);
            chartRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
            chartRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlBorderWeight.xlThick;



            //------------------- Print My totals
            //if (SalesReps.Trim() != string.Empty)
            //{

            //if (blnPrintThisRow)
            //{
            chartRange = xlWorkSheet.get_Range("a" + ++rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Size = 9.0;
            chartRange.Interior.ColorIndex = 19;
            chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

            xlWorkSheet.Cells[rowCounter, colSalesRep] = strCurRep + "Totals";
            xlWorkSheet.Cells[rowCounter, colCompany] = String.Format("{0:g}", intMyCompanies + " Companies");
            xlWorkSheet.Cells[rowCounter, colPartNo] = String.Format("{0:g}", intMyUniquePartNumbers + " Unique Part Numbers");



            xlWorkSheet.Cells[rowCounter, colQtyOrdered] = String.Format("{0:g}", decMyQtyOrderedTotal);
            xlWorkSheet.Cells[rowCounter, colQtyShipped] = String.Format("{0:g}", decMyQtyShippedTotal);
            xlWorkSheet.Cells[rowCounter, colTotalOrdered] = String.Format("{0:C}", decMyTotalOrderedTotal);
            xlWorkSheet.Cells[rowCounter, colTotalShipped] = String.Format("{0:C}", decMyTotalShippedTotal);


            //}
            //}



            //------------------------------------- Totals
            chartRange = xlWorkSheet.get_Range("a" + ++rowCounter, "x" + rowCounter);
            chartRange.Font.Bold = true;
            chartRange.Font.Size = 9.0;
            chartRange.Interior.ColorIndex = 6;
            chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

            xlWorkSheet.Cells[rowCounter, colSalesRep] = String.Format("{0:g}", intRepsCount + " Reps");
            xlWorkSheet.Cells[rowCounter, colCompany] = String.Format("{0:g}", intCompanies + " Companies");
            xlWorkSheet.Cells[rowCounter, colPartNo] = String.Format("{0:g}", intUniquePartNumbers + " Unique Part Numbers");



            xlWorkSheet.Cells[rowCounter, colQtyOrdered] = String.Format("{0:g}", decQtyOrderedTotal);
            xlWorkSheet.Cells[rowCounter, colQtyShipped] = String.Format("{0:g}", decQtyShippedTotal);
            xlWorkSheet.Cells[rowCounter, colTotalOrdered] = String.Format("{0:C}", decTotalOrderedTotal);
            xlWorkSheet.Cells[rowCounter, colTotalShipped] = String.Format("{0:C}", decTotalShippedTotal);



        } //------------------------------ end of include daily sales report





        xlWorkSheet.Select(Missing.Value);
        myCell = (Excel.Range)xlWorkSheet.Cells[5, 4];
        myCell.Activate();
        myCell.Application.ActiveWindow.FreezePanes = true;


        //chartRange = xlWorkSheet.get_Range("f18", "f" + rowCounter);
        //chartRange.Cells.NumberFormat = "$0.00";

        xlWorkSheet2.Select(Missing.Value);
       
        range = xlWorkSheet2.get_Range("A1", "A1");
        strCellData = range.Text.ToString();





        //if (blIncludeSnapShot && strCellData != string.Empty)
        //{
        //    //-------- this page should be good to go
        //}
       if (blIncludeSnapShot)
        {
            

            //xlWorkSheet.Cells[intOrderedTodayQtyRow, 3] = String.Format("{0:g}", decQtyOrderedTotal);
            xlWorkSheet2.Cells[intOrderedTodayDollarsRow, 3] = String.Format("{0:C}", decTotalOrderedTotal);
            //xlWorkSheet.Cells[intShippedTodayQtyRow, 3] = String.Format("{0:g}", decQtyShippedTotal);
            xlWorkSheet2.Cells[intShippedTodayDollarsRow, 3] = String.Format("{0:C}", decTotalShippedTotal);

        }



        string thisTitle = "Daily Sales Recap";


        
        if (SalesReps != string.Empty)
        {
            thisTitle += " for " + strSelectedReps;
        }

        xlWorkSheet.Cells[1, 1] = thisTitle;
               



        //xlWorkSheet.Cells[++rowCounter, 1] = "Ordered Today Qty [" + string.Format("{0:MM/dd/yyyy}", strReportFromDate) + "";
        //int intOrderedTodayQtyRow = rowCounter;


        //xlWorkSheet.Cells[++rowCounter, 1] = "Ordered Today Extend";
        //int intOrderedTodayDollarsRow = rowCounter;


        //xlWorkSheet.Cells[++rowCounter, 1] = "Shipped Today Qty";
        //int intShippedTodayQtyRow = rowCounter;


        //xlWorkSheet.Cells[++rowCounter, 1] = "Shipped Today Extend";
        //int intShippedTodayDollarsRow = rowCounter;



        int intLevelThreeBeginRow = rowCounter;

        intLevelThreeBeginRow += 5;



       



        //---------------------------------------------- Include the Company run
        xlWorkSheet4.Select(Missing.Value);
        
        range = xlWorkSheet4.get_Range("A1", "A1");
        strCellData = range.Text.ToString();

        if (blIncludeCompanies && strCellData != string.Empty)
        {
            //-------- this page should be good to go
        }
        else if (blIncludeCompanies)
        {



            buildCompanySheet(xlWorkSheet4, fiscalYear, decTodaysFactor, reportFromDate, strRolling12FromDate, strRolling12ToDate, reportToDate);

           


            xlWorkSheet4.Select(Missing.Value);
            myCell = (Excel.Range)xlWorkSheet4.Cells[3, 3];
            myCell.Activate();
            myCell.Application.ActiveWindow.FreezePanes = true;



        }

        //------------------------------------------------ Include the Parts Run
        xlWorkSheet5.Select(Missing.Value);
      
        range = xlWorkSheet5.get_Range("A1", "A1");
        strCellData = range.Text.ToString();

        if (blIncludeParts && strCellData != string.Empty)
        {
            //-------- this page should be good to go
        }
        else if (blIncludeParts)
        {



            buildPartsSheet(xlWorkSheet5, fiscalYear, CompanyCodes, reportFromDate, strRolling12FromDate, strRolling12ToDate, decTodaysFactor, reportToDate, strCompanyCodes);



            xlWorkSheet5.Select(Missing.Value);
            myCell = (Excel.Range)xlWorkSheet5.Cells[3, 3];
            myCell.Activate();
            myCell.Application.ActiveWindow.FreezePanes = true;


        }











        string strReps = string.Empty;
        string shortDate = string.Format("{0:yyyyMMdd}", reportFromDate);
        shortDate += "_" + string.Format("{0:hhmm}", DateTime.Now);

        if (blnMultiDayReport)
        {
            shortDate += "-" + string.Format("{0:MMdd}",  Convert.ToDateTime(strReportToDate));
        }
        if (SalesReps.Trim() != string.Empty)
        {
            strReps = SalesReps.Replace(",", "_");
            strReps = strReps.Replace(" ", string.Empty);

            shortDate += "_" + strReps;
        }


        shortDate += "_" + slspCode;


        string xlsFile = strFileRootPath + "\\Reports\\SalesReports\\" + strFileName + shortDate + ".xls";

        //if (File.Exists(xlsFile))
        //{
        //    File.Delete(xlsFile);
        //}

        xlWorkSheet.Select(Missing.Value);


        hlTemp.NavigateUrl = strReportsPath + "SalesReports/" + strFileName + shortDate + ".xls";
        hlTemp.Text = shortDate + ".xls";

        string thisResult = string.Empty;
        string variables = " xlsFile: " + xlsFile + " ";






        xlWorkBook.SaveAs(xlsFile, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

        //thisResult = sendMail("StevenRush@Cyberwiz.com", "StevenRush@Cyberwiz.com", "Debug", variables, true, false, string.Empty);




        //xlWorkBook.Close(true, misValue, misValue);



        //// Need all following code to clean up and extingush all references!!!
        //xlWorkBook.Close(null, null, null);
        //xlApp.Workbooks.Close();
        //xlApp.Quit();
        //System.Runtime.InteropServices.Marshal.ReleaseComObject(chartRange);
        //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
        //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
        //xlWorkSheet = null;
        //xlWorkBook = null;
        //xlApp = null;
        //GC.Collect(); // force final cleanup!


        ////kill EXCEL com processes
        //System.Diagnostics.Process[] PROC = Process.GetProcessesByName("EXCEL");
        //foreach (System.Diagnostics.Process PK in PROC)
        //{//User excel process always have window name
        //    //COM process do not.
        //    if (PK.MainWindowTitle.Length == 0)
        //        PK.Kill();
        //}






        //GC.GetTotalMemory(false);
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //xlApp.Quit();
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //GC.GetTotalMemory(true);


        //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
        //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
        //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);


        //xlWorkSheet = null;
        //xlWorkBook = null;
        //xlApp = null;


        //----- add to the mailing list

        strMailingList[++mailListCounter, 0] = sendFrom;
        strMailingList[mailListCounter, 1] = sendTo;
        strMailingList[mailListCounter, 2] = "Daily Sales Report";
        strMailingList[mailListCounter, 4] = xlsFile;





        //string thisResult = string.Empty;
        if (emailResults)
        {

            thisResult = sendMail(sendFrom, sendTo, "Daily Sales Report", string.Empty, true, false, xlsFile);

            //lblSteveDebug.Text = thisResult;
     
        }




        return hlTemp;

    }




    private static void buildCompanySheet(Excel.Worksheet xlWorkSheet4, string fiscalYear, Decimal decTodaysFactor, DateTime reportFromDate,  string strRolling12FromDate, string strRolling12ToDate, DateTime reportToDate)
    {




        string strRolling12_24FromDate = reportFromDate.AddYears(-2).Month + "/" + reportFromDate.AddYears(-2).Day + "/" + reportFromDate.AddYears(-2).Year;


        string strRolling24_36FromDate = reportFromDate.AddYears(-3).Month + "/" + reportFromDate.AddYears(-3).Day + "/" + reportFromDate.AddYears(-3).Year; 

        int intColCnt = 0;
      
        Excel.Range chartRange;

        xlWorkSheet4.Name = "Company Performance Report";


        chartRange = xlWorkSheet4.get_Range("B1", "Z1");
        chartRange.EntireColumn.ColumnWidth = 14;


        chartRange = xlWorkSheet4.get_Range("A1", "A1");
        chartRange.EntireColumn.ColumnWidth = 35;



        Decimal decCompanyYTDSales = 0m;

        Decimal decCompanyRolling12Sales = 0m;
        Decimal decCompanyRolling12_24Sales = 0m;
        Decimal decCompanyRolling24_36Sales = 0m;
        Decimal decCompanyRollingtrend = 0m;

        Decimal dec24_36Total = 0m;
        Decimal dec12_24Total = 0m;




        Decimal decCompanyYearForecast = 0m;
        Decimal decCompanyYTDForecast = 0m;
        Decimal decTotalYTDSales = 0m;
        Decimal decCompanyYTDVariance = 0m;

        decimal decRollingTotal = 0m;
        decimal decYearForecast = 0m;
        decimal decYTDForecast = 0m;
        decimal decYTDVariance = 0m;
        int intYTDDivisor = 0;

        decimal decTrendAccumulator = 0m;
        int intTrendCounter = 0;

     
        string strCurRep = string.Empty;
        string strCurCompany = string.Empty;
        string strCurCompanyKey = string.Empty;


        intColCnt = 1;
        int colCompanyName = intColCnt++;
        int colSalesRep = intColCnt++;


        int colCompanyListRolling24_36Sales = intColCnt++;

        int colCompanyListRolling12_24Sales = intColCnt++;

        int colCompanyListRolling12Sales = intColCnt++;

        int colCompanyListRollingTrend = intColCnt++;


        int colCompanyListYearForecast = intColCnt++;

        int colCompanyListAdjustedForecast = intColCnt++;

        int colCompanyListYTDSales = intColCnt++;

        int colCompanyListAdjustedYTDVariance = intColCnt++;

        int rowCounter = 0;



        xlWorkSheet4.Cells[++rowCounter, 1] = "Performance By Customer";
        chartRange = xlWorkSheet4.get_Range("a" + rowCounter, "j" + rowCounter);
        chartRange.Font.Bold = true;
        chartRange.Font.Size = 16.0;
        chartRange.Font.ColorIndex = 1;
        chartRange.Merge(false);
        chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
        chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
        chartRange.Interior.ColorIndex = 15;



        xlWorkSheet4.Cells[++rowCounter, colCompanyName] = "Company Name";
        xlWorkSheet4.Cells[rowCounter, colSalesRep] = "Sales Rep";
        xlWorkSheet4.Cells[rowCounter, colCompanyListYTDSales] = "YTD Sales";

        xlWorkSheet4.Cells[rowCounter, colCompanyListRolling12Sales] = "Last 12 Sales";
        xlWorkSheet4.Cells[rowCounter, colCompanyListRolling12_24Sales] = "12 to 24 Sales";
        xlWorkSheet4.Cells[rowCounter, colCompanyListRolling24_36Sales] = "24 to 36 Sales";
        xlWorkSheet4.Cells[rowCounter, colCompanyListRollingTrend] = "Trend";




        xlWorkSheet4.Cells[rowCounter, colCompanyListYearForecast] = "Year Forecast";


        xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedForecast] = "YTD Forecast";
        xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedYTDVariance] = "YTD Variance";



        chartRange = xlWorkSheet4.get_Range("i" + (rowCounter + 1).ToString(), "i1000");
        chartRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;







        DataTable dtCompanylist = loadCompaniesList();
        DataTable dtCompanyForecasts = loadCompanyForecasts(fiscalYear);
        //Decimal thisSalesGoal = 0m;

        foreach (DataRow myRow in dtCompanylist.Rows)
        {





            xlWorkSheet4.Cells[++rowCounter, colCompanyName] = myRow["COMPANY_NAME"].ToString();

            if (myRow["SALESPERSON_NAME"].ToString() != "Unknown")
            {
                xlWorkSheet4.Cells[rowCounter, colSalesRep] = myRow["SALESPERSON_NAME"].ToString();
            }

            //string dfsfsdds = myRow["COMPANY_NAME"].ToString();

            decCompanyYearForecast = 0m;
            decCompanyYTDForecast = 0m;
            decCompanyYTDVariance = 0m;
            decCompanyRolling12Sales = 0m;
            decCompanyRolling12_24Sales = 0m;
            decCompanyRolling24_36Sales = 0m;
            decCompanyRollingtrend = 0m;
        

            strCurCompanyKey = myRow["CMP_AUTO_KEY"].ToString();

            decCompanyYTDSales = CalculateSalesForPeriod("SalesByCompany", "year", reportFromDate, strCurCompanyKey, reportToDate);

            //decCompanyRolling12Sales = getPartSalesPerPeriod(string.Empty, strCurCompanyKey, strRolling12FromDate, strRolling12ToDate, "dollars");


            //decCompanyRolling12_24Sales = getPartSalesPerPeriod(string.Empty, strCurCompanyKey, strRolling12_24FromDate, strRolling12FromDate, "dollars");

            //decCompanyRolling24_36Sales = getPartSalesPerPeriod(string.Empty, strCurCompanyKey, strRolling24_36FromDate, strRolling12_24FromDate, "dollars");




            decCompanyRolling12Sales = CalculateSalesForPeriod("SalesByCompany", "specifiedRange", Convert.ToDateTime(strRolling12FromDate), strCurCompanyKey, Convert.ToDateTime(strRolling12ToDate));


            decCompanyRolling12_24Sales = CalculateSalesForPeriod("SalesByCompany", "specifiedRange", Convert.ToDateTime(strRolling12_24FromDate), strCurCompanyKey, Convert.ToDateTime(strRolling12FromDate));

            decCompanyRolling24_36Sales = CalculateSalesForPeriod("SalesByCompany", "specifiedRange", Convert.ToDateTime(strRolling24_36FromDate), strCurCompanyKey, Convert.ToDateTime(strRolling12_24FromDate));



            dec24_36Total += decCompanyRolling24_36Sales;
            dec12_24Total += decCompanyRolling12_24Sales;



            if (decCompanyRolling12Sales != 0)
            {
                xlWorkSheet4.Cells[rowCounter, colCompanyListRolling12Sales] = string.Format("{0:C}", decCompanyRolling12Sales);
            }

            if (decCompanyRolling12_24Sales != 0)
            {
                xlWorkSheet4.Cells[rowCounter, colCompanyListRolling12_24Sales] = string.Format("{0:C}", decCompanyRolling12_24Sales);
            }

            if (decCompanyRolling24_36Sales != 0)
            {
                xlWorkSheet4.Cells[rowCounter, colCompanyListRolling24_36Sales] = string.Format("{0:C}", decCompanyRolling24_36Sales);
            }

            decimal dec2Trend = -9999m;
            decimal dec1Trend = -9999m;

            if (decCompanyRolling24_36Sales != 0 && decCompanyRolling12_24Sales !=0)
            {
                dec2Trend = decCompanyRolling12_24Sales / decCompanyRolling24_36Sales;
            }


            if (decCompanyRolling12Sales != 0 && decCompanyRolling12_24Sales != 0)
            {
                dec1Trend = decCompanyRolling12Sales /decCompanyRolling12_24Sales;
            }

            if (dec2Trend != -9999 && dec1Trend != -9999)
            {
                decCompanyRollingtrend = (((dec1Trend * 2) + dec2Trend) / 3);
            }
            else if (dec1Trend != -9999)
            {
                decCompanyRollingtrend = dec1Trend;
            }



            if (decCompanyRollingtrend != 0)
            {

                decTrendAccumulator += decCompanyRollingtrend;
                intTrendCounter++;

                xlWorkSheet4.Cells[rowCounter, colCompanyListRollingTrend] = string.Format("{0:g}", Math.Round(decCompanyRollingtrend, 2));


                if (decCompanyRollingtrend < 1m)
                {
                    chartRange = xlWorkSheet4.get_Range("f" + rowCounter, "f" + rowCounter);
                    chartRange.Font.ColorIndex = 3;
                }
                else
                {
                    chartRange = xlWorkSheet4.get_Range("f" + rowCounter, "f" + rowCounter);
                    chartRange.Font.ColorIndex = 10;
                }
            }


           

            if (decCompanyYTDSales != 0)
            {
                xlWorkSheet4.Cells[rowCounter, colCompanyListYTDSales] = string.Format("{0:C}", decCompanyYTDSales);
            }

         

            decTotalYTDSales += decCompanyYTDSales;
            decRollingTotal += decCompanyRolling12Sales;
           

            foreach (DataRow forecastRow in dtCompanyForecasts.Rows)
            {
                //string dfttt = forecastRow["CompanyName"].ToString();
                if (forecastRow["CompanyName"].ToString() == myRow["COMPANY_NAME"].ToString())
                {
                    if (forecastRow["SalesGoal"] != null)
                    {
                        if (GenUtils.IsDecimal(forecastRow["SalesGoal"].ToString()));

                        {


                            decCompanyYearForecast = (Decimal)forecastRow["SalesGoal"]; ;


                            decCompanyYTDForecast = 0m;
                            if (decCompanyYearForecast > 0)
                            {
                                decCompanyYTDForecast = Math.Round(decCompanyYearForecast * decTodaysFactor, 2);
                            }

                            decCompanyYTDVariance = 0m;
                            if (decCompanyYTDForecast > 1)
                            {
                                decCompanyYTDVariance = 100 - ((decCompanyYTDSales / decCompanyYTDForecast) * 100);
                                decCompanyYTDVariance = Math.Round(0 - decCompanyYTDVariance, 1);
                            }






                            xlWorkSheet4.Cells[rowCounter, colCompanyListYearForecast] = string.Format("{0:C}", decCompanyYearForecast);



                            xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedForecast] = string.Format("{0:C}", decCompanyYTDForecast);


                            if (decCompanyYTDSales == 0)
                            {
                                xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedYTDVariance] = "";
                            }
                            else
                            {
                                xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedYTDVariance] = string.Format("{0:g}", decCompanyYTDVariance) + "%";
                            }

                            if (decCompanyYTDVariance < 0)
                            {
                                chartRange = xlWorkSheet4.get_Range("j" + rowCounter, "j" + rowCounter);
                                chartRange.Font.ColorIndex = 3;
                            }
                            else
                            {
                                chartRange = xlWorkSheet4.get_Range("j" + rowCounter, "j" + rowCounter);
                                chartRange.Font.ColorIndex = 10;
                            }


                            decYearForecast += decCompanyYearForecast;
                            decYTDForecast += decCompanyYTDForecast;
                            decYTDVariance += decCompanyYTDVariance;
                            intYTDDivisor++;


                        }
                    }
                    break;
                }

            }




        }





        //------------------------------------- Totals
        chartRange = xlWorkSheet4.get_Range("a" + ++rowCounter, "j" + rowCounter);
        chartRange.Font.Bold = true;
        chartRange.Font.Size = 9.0;
        chartRange.Interior.ColorIndex = 6;
        chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);


  

        xlWorkSheet4.Cells[rowCounter, colCompanyListRolling24_36Sales] = String.Format("{0:C}", dec24_36Total);
        xlWorkSheet4.Cells[rowCounter, colCompanyListRolling12_24Sales] = String.Format("{0:C}", dec12_24Total);


        xlWorkSheet4.Cells[rowCounter, colCompanyListYTDSales] = String.Format("{0:C}", decTotalYTDSales);
        xlWorkSheet4.Cells[rowCounter, colCompanyListRolling12Sales] = String.Format("{0:C}", decRollingTotal);
        xlWorkSheet4.Cells[rowCounter, colCompanyListYearForecast] = String.Format("{0:C}", decYearForecast);


        xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedForecast] = String.Format("{0:C}", decYTDForecast);


        xlWorkSheet4.Cells[rowCounter, colCompanyListAdjustedYTDVariance] = String.Format("{0:g}", Math.Round(decYTDVariance / intYTDDivisor, 2)) + "%";


        xlWorkSheet4.Cells[rowCounter, colCompanyListRollingTrend] = String.Format("{0:g}", Math.Round(decTrendAccumulator / intTrendCounter, 2)) + "%";


  


        // + (rowCounter -1).ToString()


        //Microsoft.Office.Excel.NamedRange NamedRange1 =
        //       this.Controls.AddNamedRange(this.Range["A1", "A7"],
        //       "NamedRange1");

        //NamedRange1.AutoFilter(1, "Apple",
        //   Excel.XlAutoFilterOperator.xlAnd, missing, true);


        chartRange = xlWorkSheet4.get_Range("a2", "j" + (rowCounter - 3).ToString());
        chartRange.AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Missing.Value, true);






    }


    private static void buildPartsSheet(Excel.Worksheet xlWorkSheet5, string fiscalYear, string CompanyCodes, DateTime reportFromDate, string strRolling12FromDate, string strRolling12ToDate, Decimal decTodaysFactor, DateTime reportToDate, string strCompanyCodes)
    {

        int intColCnt = 0;
        string allCompanyCodes = CompanyCodes;
        
        if (strCompanyCodes != string.Empty)
        {
            if (allCompanyCodes != string.Empty)
            {
                allCompanyCodes += ",";
            }
            allCompanyCodes += strCompanyCodes;
        }


        Excel.Range chartRange;

        xlWorkSheet5.Name = "Parts Performance Report";


        chartRange = xlWorkSheet5.get_Range("A1", "Z1");
        chartRange.EntireColumn.ColumnWidth = 14;

         int asciiRow = 0;
        decimal decPartsYTDSales = 0m;
        decimal decPartsYearForecast = 0m;
        decimal decPartsYTDForecast = 0m;
        decimal decPartsYTDVariance = 0m;
        decimal decPartsYearVariance = 0m;
        string strCurPartKey = string.Empty;
        decimal decQtyLast12 = 0m;
        decimal decDollarsLast12 = 0m;
        decimal decCustomPrice = 0m;

        decimal thisListPrice = 0m;
        decimal thisExchPrice = 0m;



        decimal decRoll12QtyTotal = 0m;
        decimal decRolling12DollarsTotal = 0m;

        decimal decYTDSalesTotal = 0m;
        decimal decYearForecastTotal = 0m;
        decimal decYTDForecastTotal = 0m;
        decimal decVarianceTotal = 0m;
        int intVarianceDivisor = 0;


        //----------------- load the custom pricing Table
        DataTable[] dtCustomPricing = new DataTable[20];
        int intCustomPriceCount = 0;




        intColCnt = 1;
        int colPartListPN = intColCnt++;
     
        int colPartListDescription = intColCnt++;
        
        int colPartListListPrice = intColCnt++;
        int colPartListExchPrice = intColCnt++;
       

        int[] colPartListCustomArray = new int[20];
        string[] nCompanies = allCompanyCodes.Trim().Split(',');

        if (allCompanyCodes != string.Empty)
        {
            intCustomPriceCount = nCompanies.GetUpperBound(0) + 1;
            for (int tlp = 0; tlp <= nCompanies.GetUpperBound(0); tlp++)
            {
                dtCustomPricing[tlp] = lookupCustomPricing(nCompanies[tlp].Trim().ToLower());
                colPartListCustomArray[tlp] = intColCnt++;
            }

        }


        //chartRange = xlWorkSheet5.get_Range("B1", "c1");
        //chartRange.EntireColumn.ColumnWidth = 22;

        int colPartListYearForecast = intColCnt++;
        int colPartListAdjustedForecast = intColCnt++;
        int colPartListYTDSales = intColCnt++;
        int colPartListAdjustedYTDVariance = intColCnt++;



        int colPartListRolling12Qty = intColCnt++;
        int colPartListRolling12Dollars = intColCnt++;
        //int colCompanyListRolling12_24Sales = intColCnt++;
        //int colCompanyListRolling24_36Sales = intColCnt++;
        //int colCompanyListRollingTrend = intColCnt++;

      

        int rowCounter = 0;

        asciiRow = 106 + intCustomPriceCount;

        xlWorkSheet5.Cells[++rowCounter, 1] = "Performance By Part Number";
        chartRange = xlWorkSheet5.get_Range("a" + rowCounter, (char)asciiRow + (rowCounter).ToString());
        chartRange.Font.Bold = true;
        chartRange.Font.Size = 16.0;
        chartRange.Font.ColorIndex = 1;
        chartRange.Merge(false);
        chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
        chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
        chartRange.Interior.ColorIndex = 15;



     

        xlWorkSheet5.Cells[++rowCounter, colPartListPN] = "Part Number";
        xlWorkSheet5.Cells[rowCounter, colPartListDescription] = "Description";
        xlWorkSheet5.Cells[rowCounter, colPartListListPrice] = "Overhaul List";
        xlWorkSheet5.Cells[rowCounter, colPartListExchPrice] = "Airlines List";



        if (allCompanyCodes != string.Empty)
        {
            for (int tlp = 0; tlp <= nCompanies.GetUpperBound(0); tlp++)
            {
                xlWorkSheet5.Cells[rowCounter, colPartListCustomArray[tlp]] = nCompanies[tlp].Trim() + " Price";
                
            }
        }
   



        xlWorkSheet5.Cells[rowCounter, colPartListRolling12Qty] = "Rolling 12 Qty";
        xlWorkSheet5.Cells[rowCounter, colPartListRolling12Dollars] = "Rolling 12 Shipped";


        xlWorkSheet5.Cells[rowCounter, colPartListYTDSales] = "YTD Sales";
        xlWorkSheet5.Cells[rowCounter, colPartListYearForecast] = "Year Forecast";

        xlWorkSheet5.Cells[rowCounter, colPartListAdjustedForecast] = "YTD Forecast";
        xlWorkSheet5.Cells[rowCounter, colPartListAdjustedYTDVariance] = "YTD Variance";


        chartRange = xlWorkSheet5.get_Range("a" + rowCounter, (char)asciiRow + (rowCounter).ToString());
        chartRange.Font.Bold = true;
        chartRange.Font.Underline = true;
        chartRange.Font.Size = 9.0;
        chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter; ;


        chartRange = xlWorkSheet5.get_Range("a" + rowCounter + 1, "b" + 1000);
        chartRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;


        chartRange = xlWorkSheet5.get_Range("f" + (rowCounter + 1).ToString(), "f1000");
        chartRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;





        chartRange = xlWorkSheet5.get_Range("A1", "B1");
        chartRange.EntireColumn.ColumnWidth = 30;






        DataTable dtPartsList = loadPartsList();
        DataTable dtPartsForecasts = loadPartsForecasts(fiscalYear);
        //Decimal thisSalesGoal = 0m;
        
        int intDataTop = rowCounter + 1;

        foreach (DataRow myRow in dtPartsList.Rows)
        {




            thisListPrice = (Decimal)myRow["LIST_PRICE"];
            thisExchPrice = (Decimal)myRow["EXCH_LIST_PRICE"];



            xlWorkSheet5.Cells[++rowCounter, colPartListPN] = myRow["PN"].ToString();


            xlWorkSheet5.Cells[rowCounter, colPartListDescription] = myRow["DESCRIPTION"].ToString();



            xlWorkSheet5.Cells[rowCounter, colPartListListPrice] = string.Format("{0:C}", thisListPrice);




            if (thisExchPrice != 0)
            {
                xlWorkSheet5.Cells[rowCounter, colPartListExchPrice] = string.Format("{0:C}", thisExchPrice);
            }



            decCustomPrice = 0m;


            int prcLp = 0;


            if (allCompanyCodes != string.Empty)
            {
                for (int tlp = 0; tlp <= nCompanies.GetUpperBound(0); tlp++)
                {
                   

                    prcLp = 0;

                    for (prcLp = 0; prcLp <= dtCustomPricing[tlp].Rows.Count - 1; prcLp++)
                    {
                        decCustomPrice = 0m;
                        if (myRow["PNM_AUTO_KEY"].ToString() == dtCustomPricing[tlp].Rows[prcLp]["PNM_AUTO_KEY"].ToString())
                        {
                            decCustomPrice = (decimal)dtCustomPricing[tlp].Rows[prcLp]["UNIT_PRICE"];
                            break;
                        }
                    }
                    if (decCustomPrice != 0)
                    {
                        xlWorkSheet5.Cells[rowCounter, colPartListCustomArray[tlp]] = string.Format("{0:C}", decCustomPrice);
                    }

                }
            }
   



            decPartsYearForecast = 0m;
            decPartsYTDForecast = 0m;
            decPartsYTDVariance = 0m;
            decPartsYearVariance = 0m;
            decQtyLast12 = 0m;
            decDollarsLast12 = 0m;

            strCurPartKey = myRow["PNM_AUTO_KEY"].ToString();
            //thisSalesGoal = (Decimal)forecastRow["SalesGoal"] * thisListPrice;


            decPartsYTDSales = CalculateSalesForPeriod("PartsSoldByPeriod", "year", reportFromDate, strCurPartKey, reportToDate);



            decQtyLast12 = getPartSalesPerPeriod(myRow["PN"].ToString(), string.Empty, strRolling12FromDate, strRolling12ToDate, "qty");

            decDollarsLast12 = getPartSalesPerPeriod(myRow["PN"].ToString(), string.Empty, strRolling12FromDate, strRolling12ToDate, "dollars");



            if (decPartsYTDSales != 0)
            {
                xlWorkSheet5.Cells[rowCounter, colPartListYTDSales] = string.Format("{0:C}", decPartsYTDSales);
            }


            if (decQtyLast12 != 0)
            {
                xlWorkSheet5.Cells[rowCounter, colPartListRolling12Qty] = string.Format("{0:g}", decQtyLast12);
            }
            if (decDollarsLast12 != 0)
            {

                xlWorkSheet5.Cells[rowCounter, colPartListRolling12Dollars] = string.Format("{0:C}", decDollarsLast12);
            }



            decRoll12QtyTotal += decQtyLast12;
            decRolling12DollarsTotal += decDollarsLast12;
            decYTDSalesTotal += decPartsYTDSales;
         

            foreach (DataRow forecastRow in dtPartsForecasts.Rows)
            {
                if (forecastRow["PN"].ToString() == myRow["PN"].ToString())
                {
                    if (forecastRow["SalesGoal"] != null)
                    {
                        if (GenUtils.IsDecimal(forecastRow["SalesGoal"].ToString()));

                        {
                            decPartsYearForecast = (Decimal)forecastRow["SalesGoal"] * thisListPrice;

                            decPartsYTDForecast = 0m;
                            if (decPartsYearForecast > 0)
                            {
                                decPartsYTDForecast = Math.Round(decPartsYearForecast * decTodaysFactor, 2);
                            }

                            decPartsYTDVariance = 0m;
                            if (decPartsYTDForecast > 1)
                            {
                                decPartsYTDVariance = 100 - ((decPartsYTDSales / decPartsYTDForecast) * 100);
                                decPartsYTDVariance = Math.Round(0 - decPartsYTDVariance, 1);
                            }


                            decPartsYearVariance = 0m;
                            if (decPartsYearForecast > 1)
                            {
                                decPartsYearVariance = 100 - ((decPartsYTDSales / decPartsYearForecast) * 100);
                                decPartsYearVariance = Math.Round(0 - decPartsYearVariance, 1);

                            }

                            xlWorkSheet5.Cells[rowCounter, colPartListYearForecast] = string.Format("{0:C}", decPartsYearForecast);

                            xlWorkSheet5.Cells[rowCounter, colPartListAdjustedForecast] = string.Format("{0:C}", decPartsYTDForecast);

                            if (decPartsYTDSales == 0)
                            {
                                xlWorkSheet5.Cells[rowCounter, colPartListAdjustedYTDVariance] = "";
                            }
                            else
                            {
                                xlWorkSheet5.Cells[rowCounter, colPartListAdjustedYTDVariance] = string.Format("{0:g}", decPartsYTDVariance) + "%";
                            }
                            asciiRow = 104 + intCustomPriceCount;

                            if (decPartsYTDVariance < 0)
                            {
                                chartRange = xlWorkSheet5.get_Range((char)asciiRow + (rowCounter).ToString(), (char)asciiRow + (rowCounter).ToString());
                                chartRange.Font.ColorIndex = 3;
                            }
                            else
                            {
                                chartRange = xlWorkSheet5.get_Range((char)asciiRow + (rowCounter).ToString(), (char)asciiRow + (rowCounter).ToString());
                                chartRange.Font.ColorIndex = 10;
                            }

                            decYearForecastTotal += decPartsYearForecast;
                            decYTDForecastTotal += decPartsYTDForecast;
                            decVarianceTotal += decPartsYTDVariance;
                            
                            intVarianceDivisor++;




                        }
                    }
                    break;
                }

            }

        }



        chartRange = xlWorkSheet5.get_Range("a" + intDataTop, "a" + (rowCounter + 1).ToString());
        chartRange.Interior.ColorIndex = 35;
        chartRange.Borders.ColorIndex = 43;
        chartRange.Borders.LineStyle = Excel.XlLineStyle.xlDot;
        chartRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;




      

        if (allCompanyCodes != string.Empty)
        {

            asciiRow =  100 + intCustomPriceCount;
            chartRange = xlWorkSheet5.get_Range("e" + intDataTop, (char)asciiRow + (rowCounter + 1).ToString());
            chartRange.Interior.ColorIndex = 19;
            chartRange.Borders.ColorIndex = 43;
            chartRange.Borders.LineStyle = Excel.XlLineStyle.xlDot;
            


         
        }


        asciiRow = 106 + intCustomPriceCount;

        //------------------------------------- Totals
        chartRange = xlWorkSheet5.get_Range("a" + ++rowCounter, (char)asciiRow + (rowCounter).ToString());
        chartRange.Font.Bold = true;
        chartRange.Font.Size = 9.0;
        chartRange.Interior.ColorIndex = 6;
        chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

        xlWorkSheet5.Cells[rowCounter, colPartListRolling12Qty] = String.Format("{0:g}", decRoll12QtyTotal);
        xlWorkSheet5.Cells[rowCounter, colPartListRolling12Dollars] = String.Format("{0:C}", decRolling12DollarsTotal);
        xlWorkSheet5.Cells[rowCounter, colPartListYTDSales] = String.Format("{0:C}", decYTDSalesTotal);
        xlWorkSheet5.Cells[rowCounter, colPartListYearForecast] = String.Format("{0:C}", decYearForecastTotal);
        xlWorkSheet5.Cells[rowCounter, colPartListAdjustedForecast] = String.Format("{0:C}", decYTDForecastTotal);
        xlWorkSheet5.Cells[rowCounter, colPartListAdjustedYTDVariance] = String.Format("{0:g}", Math.Round(decVarianceTotal / intVarianceDivisor, 2)) + "%";


         asciiRow =  106 + intCustomPriceCount;
        chartRange = xlWorkSheet5.get_Range("a2", (char)asciiRow + (rowCounter - 1).ToString());
        chartRange.AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Missing.Value, true);



    }

    private static void buildWeeklyRecap(Excel.Worksheet xlWorkSheet3, string fiscalYear)
    {



        //------------------------------ Actual Sales Weekly Recap
        int recapRowCounter = 0;
        Excel.Range chartRange;

        xlWorkSheet3.Name = "Weekly Sales Report";


        chartRange = xlWorkSheet3.get_Range("A1", "Z1");
        chartRange.EntireColumn.ColumnWidth = 14;



     
        xlWorkSheet3.Cells[++recapRowCounter, 1] = "Actual Sales Weekly Recap";
        chartRange = xlWorkSheet3.get_Range("a" + recapRowCounter, "l" + recapRowCounter);
        chartRange.Font.Bold = true;
        chartRange.Font.Size = 16.0;
        chartRange.Font.ColorIndex = 1;
        chartRange.Merge(false);
        chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
        chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
        chartRange.Interior.ColorIndex = 15;


        string[,] strSalesRecap = new string[53, 10];
        strSalesRecap = loadSalesRecapArray(fiscalYear);

        int shlp;
        int intTopElement = Convert.ToInt16(strSalesRecap[0,0]);
        int intBottomElement = intTopElement -10;
        if (intBottomElement < 1)
        {
            intBottomElement = 1;
        }

        int intColCnt = 1;
        int colFiscalWeek = intColCnt++;
        int colFromDate = intColCnt++;
        int colToDate = intColCnt++;
        int colDiscount = intColCnt++;
        int colAVSOEM = intColCnt++;
        int colAVSPMA = intColCnt++;
        int colDist = intColCnt++;
        int colJPEPMA = intColCnt++;
        int colOEM = intColCnt++;
        int colOOP = intColCnt++;
        int colPMADist = intColCnt++;
        int colTotal = intColCnt++;


        decimal decDiscountTotal = 0m;
        decimal decAVSOEMTotal = 0m;
        decimal decAVSPMATotal = 0m;
        decimal decDistTotal = 0m;
        decimal decJPEPMATotal = 0m;
        decimal decOEMTotal = 0m;
        decimal decOOPTotal = 0m;
        decimal decColPMADistGT = 0m;
        decimal decColGTGT = 0m;

        decimal decPMADistTotal = 0m;
        decimal decGrantTotal = 0m;



        xlWorkSheet3.Cells[++recapRowCounter, colFiscalWeek] = "Week";
        xlWorkSheet3.Cells[recapRowCounter, colFromDate] = "From Date";
        xlWorkSheet3.Cells[recapRowCounter, colToDate] = "To Date";
        xlWorkSheet3.Cells[recapRowCounter, colDiscount] = "Discount";
        xlWorkSheet3.Cells[recapRowCounter, colAVSOEM] = "AVS OEM";
        xlWorkSheet3.Cells[recapRowCounter, colAVSPMA] = "AVS PMA";
        xlWorkSheet3.Cells[recapRowCounter, colDist] = "Dist";
        xlWorkSheet3.Cells[recapRowCounter, colJPEPMA] = "JPE PMA";
        xlWorkSheet3.Cells[recapRowCounter, colOEM] = "OEM";
        xlWorkSheet3.Cells[recapRowCounter, colOOP] = "Owner Operator";
        xlWorkSheet3.Cells[recapRowCounter, colPMADist] = "PMA & Dist Total";
        xlWorkSheet3.Cells[recapRowCounter, colTotal] = "Grand Total";

        chartRange = xlWorkSheet3.get_Range("a" + recapRowCounter, "l" + recapRowCounter);
        chartRange.Font.Bold = true;
        chartRange.Font.Underline = true;
        chartRange.Font.Size = 9.0;
        chartRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;





        for (shlp = 1; shlp <= intTopElement; shlp++)
        {



            xlWorkSheet3.Cells[++recapRowCounter, colFiscalWeek] = shlp.ToString();
            xlWorkSheet3.Cells[recapRowCounter, colFromDate] = string.Format("{0:MM/dd/yyy}", strSalesRecap[shlp, 2]);
            xlWorkSheet3.Cells[recapRowCounter, colToDate] = string.Format("{0:MM/dd/yyy}", strSalesRecap[shlp, 3]);
            xlWorkSheet3.Cells[recapRowCounter, colDiscount] = string.Format("{0:C}", strSalesRecap[shlp, 4]);
            xlWorkSheet3.Cells[recapRowCounter, colAVSOEM] = string.Format("{0:C}", strSalesRecap[shlp, 5]);
            xlWorkSheet3.Cells[recapRowCounter, colAVSPMA] = string.Format("{0:C}", strSalesRecap[shlp, 6]);
            xlWorkSheet3.Cells[recapRowCounter, colDist] = string.Format("{0:C}", strSalesRecap[shlp, 7]);
            xlWorkSheet3.Cells[recapRowCounter, colJPEPMA] = string.Format("{0:C}", strSalesRecap[shlp, 8]);
            xlWorkSheet3.Cells[recapRowCounter, colOEM] = string.Format("{0:C}", strSalesRecap[shlp, 9]);
            xlWorkSheet3.Cells[recapRowCounter, colOOP] = string.Format("{0:C}", strSalesRecap[shlp, 10]);



            decPMADistTotal = 0m;
            decPMADistTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 6]);
            decPMADistTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 7]);
            decPMADistTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 8]);
            decPMADistTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 10]);

            decColPMADistGT += decPMADistTotal;
            

            decGrantTotal = 0m;
            decGrantTotal -= GenUtils.returnDecimalforString(strSalesRecap[shlp, 4]);
            decGrantTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 5]);
            decGrantTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 6]);
            decGrantTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 7]);
            decGrantTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 8]);
            decGrantTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 9]);
            decGrantTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 10]);

            decColGTGT += decGrantTotal;


            xlWorkSheet3.Cells[recapRowCounter, colPMADist] = string.Format("{0:C}", decPMADistTotal);
            xlWorkSheet3.Cells[recapRowCounter, colTotal] = string.Format("{0:C}", decGrantTotal);



            decDiscountTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 4]);
            decAVSOEMTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 5]);
            decAVSPMATotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 6]);
            decDistTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 7]);
            decJPEPMATotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 8]);
            decOEMTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 9]);
            decOOPTotal += GenUtils.returnDecimalforString(strSalesRecap[shlp, 10]);
        }



        xlWorkSheet3.Cells[++recapRowCounter, colFiscalWeek] = string.Empty;
        xlWorkSheet3.Cells[recapRowCounter, colFromDate] = string.Empty;
        xlWorkSheet3.Cells[recapRowCounter, colToDate] = string.Empty;
        xlWorkSheet3.Cells[recapRowCounter, colDiscount] = string.Format("{0:C}", decDiscountTotal);
        xlWorkSheet3.Cells[recapRowCounter, colAVSOEM] = string.Format("{0:C}", decAVSOEMTotal);
        xlWorkSheet3.Cells[recapRowCounter, colAVSPMA] = string.Format("{0:C}", decAVSPMATotal);
        xlWorkSheet3.Cells[recapRowCounter, colDist] = string.Format("{0:C}", decDistTotal);
        xlWorkSheet3.Cells[recapRowCounter, colJPEPMA] = string.Format("{0:C}", decJPEPMATotal);
        xlWorkSheet3.Cells[recapRowCounter, colOEM] = string.Format("{0:C}", decOEMTotal);
        xlWorkSheet3.Cells[recapRowCounter, colOOP] = string.Format("{0:C}", decOOPTotal);
        xlWorkSheet3.Cells[recapRowCounter, colPMADist] = string.Format("{0:C}", decColPMADistGT);
        xlWorkSheet3.Cells[recapRowCounter, colTotal] = string.Format("{0:C}", decColGTGT);


        chartRange = xlWorkSheet3.get_Range("a" + recapRowCounter, "l" + recapRowCounter);
        chartRange.Font.Bold = true;
        chartRange.Font.Size = 9.0;
        chartRange.Interior.ColorIndex = 6;
        chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic); 


        //-- cols
        //  1   week
        //  2   from date
        //  3   to date
        //  4   discount  -------
        //  5   AVS OEM  STC Key 10  ------
        //  6   AVS PMA  20 ------
        //  7   DIST  4  ------
        //  8   JPE PMA 2 ----
        //  9   OEM  3 ----
        //  10   Owner Operator 12 ----
        //  11  PMA and Distributor Total
        //  12  Grand Total for Week



        //Excel._Workbook oWB;
        //Excel.Series oSeries;
        //Excel.Range oResizeRange;
        //Excel._Chart oChart;
        //String sMsg;
        //int iNumQtrs;

        ////Determine how many quarters to display data for.
        //for (iNumQtrs = 4; iNumQtrs >= 2; iNumQtrs--)
        //{
        //    sMsg = "Enter sales data for ";
        //    sMsg = String.Concat(sMsg, iNumQtrs);
        //    sMsg = String.Concat(sMsg, " quarter(s)?");

        //    DialogResult iRet = MessageBox.Show(sMsg, "Quarterly Sales?",
        //        MessageBoxButtons.YesNo);
        //    if (iRet == DialogResult.Yes)
        //        break;
        //}

        //sMsg = "Displaying data for ";
        //sMsg = String.Concat(sMsg, iNumQtrs);
        //sMsg = String.Concat(sMsg, " quarter(s).");

        //MessageBox.Show(sMsg, "Quarterly Sales");

        ////Starting at E1, fill headers for the number of columns selected.
        //oResizeRange = oWS.get_Range("E1", "E1").get_Resize(Missing.Value, iNumQtrs);
        //oResizeRange.Formula = "=\"Q\" & COLUMN()-4 & CHAR(10) & \"Sales\"";

        ////Change the Orientation and WrapText properties for the headers.
        //oResizeRange.Orientation = 38;
        //oResizeRange.WrapText = true;

        ////Fill the interior color of the headers.
        //oResizeRange.Interior.ColorIndex = 36;

        ////Fill the columns with a formula and apply a number format.
        //oResizeRange = oWS.get_Range("E2", "E6").get_Resize(Missing.Value, iNumQtrs);
        //oResizeRange.Formula = "=RAND()*100";
        //oResizeRange.NumberFormat = "$0.00";

        ////Apply borders to the Sales data and headers.
        //oResizeRange = oWS.get_Range("E1", "E6").get_Resize(Missing.Value, iNumQtrs);
        //oResizeRange.Borders.Weight = Excel.XlBorderWeight.xlThin;

        ////Add a Totals formula for the sales data and apply a border.
        //oResizeRange = oWS.get_Range("E8", "E8").get_Resize(Missing.Value, iNumQtrs);
        //oResizeRange.Formula = "=SUM(E2:E6)";
        //oResizeRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle
        //    = Excel.XlLineStyle.xlDouble;
        //oResizeRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).Weight
        //    = Excel.XlBorderWeight.xlThick;

        ////Add a Chart for the selected data.
        //oWB = (Excel._Workbook)oWS.Parent;
        //oChart = (Excel._Chart)oWB.Charts.Add(Missing.Value, Missing.Value,
        //    Missing.Value, Missing.Value);

        ////Use the ChartWizard to create a new chart from the selected data.
        //oResizeRange = oWS.get_Range("E2:E6", Missing.Value).get_Resize(
        //    Missing.Value, iNumQtrs);
        //oChart.ChartWizard(oResizeRange, Excel.XlChartType.xl3DColumn, Missing.Value,
        //    Excel.XlRowCol.xlColumns, Missing.Value, Missing.Value, Missing.Value,
        //    Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //oSeries = (Excel.Series)oChart.SeriesCollection(1);
        //oSeries.XValues = oWS.get_Range("A2", "A6");
        //for (int iRet = 1; iRet <= iNumQtrs; iRet++)
        //{
        //    oSeries = (Excel.Series)oChart.SeriesCollection(iRet);
        //    String seriesName;
        //    seriesName = "=\"Q";
        //    seriesName = String.Concat(seriesName, iRet);
        //    seriesName = String.Concat(seriesName, "\"");
        //    oSeries.Name = seriesName;
        //}

        //oChart.Location(Excel.XlChartLocation.xlLocationAsObject, oWS.Name);

        ////Move the chart so as not to cover your data.
        //oResizeRange = (Excel.Range)oWS.Rows.get_Item(10, Missing.Value);
        //oWS.Shapes.Item("Chart 1").Top = (float)(double)oResizeRange.Top;
        //oResizeRange = (Excel.Range)oWS.Columns.get_Item(2, Missing.Value);
        //oWS.Shapes.Item("Chart 1").Left = (float)(double)oResizeRange.Left;






    }


    private static string[,] loadSalesRecapArray(string fiscalYear)
    {

        string[,] strTemp = new string[70, 14];

      
        DateTime dtCurBeginSunday;
        DateTime dtCurEndSaturday;
        DateTime  firstdayofyear= Convert.ToDateTime("1/1/" + fiscalYear);

 
        int enumDOW = -1;

        switch (firstdayofyear.DayOfWeek.ToString())
        {
            case "Sunday":
                enumDOW = 7;
                break;
            case "Monday":
                enumDOW = 6;
                break;
            case "Tuesday":
                enumDOW = 5;
                break;
            case "Wednesday":
                enumDOW = 4;
                break;
            case "Thursday":
                enumDOW = 3;
                break;
            case "Friday":
                enumDOW = 2;
                break;
            case "Saturday":
                enumDOW = 1;
                break;

        }


        dtCurEndSaturday = firstdayofyear.AddDays(enumDOW - 1);
        dtCurBeginSunday = firstdayofyear;
        
        //int DayCounter = 0;
        decimal[] decTemp = new decimal[14];
        decimal[] decWeeks = new decimal[14];
        decimal[] decTotals = new decimal[14];


        int weekCounter = 0;
       

        while (DateTime.Compare(dtCurEndSaturday, DateTime.Now) < 0)
        {

        //----- set the initial
        strTemp[++weekCounter, 1] = "1";
        strTemp[weekCounter, 2] = string.Format("{0:MM/dd/yyyy}", dtCurBeginSunday);
        strTemp[weekCounter, 3] = string.Format("{0:MM/dd/yyyy}", dtCurEndSaturday);
        string dfsdsaf = strTemp[weekCounter, 3];
        strTemp[weekCounter, 5] = "$0.00";
        
        decTemp[5] = getWeekSalesperSTCCode(strTemp[weekCounter, 2], strTemp[weekCounter, 3], " AND (STC_AUTO_KEY = 10) ");
        strTemp[weekCounter, 5] = string.Format("{0:C}", decTemp[5]);
        decTotals[5] += decTemp[5];

        decTemp[6] = getWeekSalesperSTCCode(strTemp[weekCounter, 2], strTemp[weekCounter, 3], " AND (STC_AUTO_KEY = 20) ");
        strTemp[weekCounter, 6] = string.Format("{0:C}", decTemp[6]);
        decTotals[6] += decTemp[6];

        decTemp[7] = getWeekSalesperSTCCode(strTemp[weekCounter, 2], strTemp[weekCounter, 3], " AND (STC_AUTO_KEY = 4) ");
        strTemp[weekCounter, 7] = string.Format("{0:C}", decTemp[7]);
        decTotals[7] += decTemp[7];

        decTemp[8] = getWeekSalesperSTCCode(strTemp[weekCounter, 2], strTemp[weekCounter, 3], " AND (STC_AUTO_KEY = 2) ");
        strTemp[weekCounter, 8] = string.Format("{0:C}", decTemp[8]);
        decTotals[8] += decTemp[8];

        decTemp[9] = getWeekSalesperSTCCode(strTemp[weekCounter, 2], strTemp[weekCounter, 3], " AND (STC_AUTO_KEY = 3) ");
        strTemp[weekCounter, 9] = string.Format("{0:C}", decTemp[9]);
        decTotals[9] += decTemp[9];



        decTemp[10] = getWeekSalesperSTCCode(strTemp[weekCounter, 2], strTemp[weekCounter, 3], " AND (STC_AUTO_KEY = 12) ");
        strTemp[weekCounter, 10] = string.Format("{0:C}", decTemp[10]);
        decTotals[10] += decTemp[10];



        //strTemp[weekCounter, 10] = string.Format("{0:C}", decTemp[6] + decTemp[7] + decTemp[8]);
        //decTotals[10] += decTemp[10];

        //strTemp[weekCounter, 11] = string.Format("{0:C}", decTemp[5], decTemp[6] + decTemp[7] + decTemp[8] + decTemp[9] + decTemp[10]);
        //decTotals[11] += decTemp[11];


        //----- increment the date 6 days
        dtCurBeginSunday = dtCurEndSaturday.AddDays(1);
        dtCurEndSaturday = dtCurEndSaturday.AddDays(7);

        strTemp[0, 0] = weekCounter.ToString();
        
        //intRange = DateTime.Compare(dtCurEndSaturday, DateTime.Now);

       


    }




        //AND (STC_AUTO_KEY = 20)

        //-- cols
        //  1   week
        //  2   from date
        //  3   to date
        //  4   discount  -------
        //  5   AVS OEM  STC Key 10  ------
        //  6   AVS PMA  20 ------
        //  7   DIST  4  ------
        //  8   JPE PMA 2 ----
        //  9   OEM  3 ----
        //  10   Owner Operator 12 ----
        //  11  PMA and Distributor Total
        //  12  Grand Total for Week



       





        //temp[weekCounter, 0] == 


        return strTemp;

      
    }



    private static Decimal getPartSalesPerPeriod(string strPN, string strCMP_AUTO_KEY, string fromDate, string toDate, string typeCall)
    {
        //DataTable dtTemp = null;
        //decimal decTemp = 0m;
        string queryString = "SELECT ";

       



        switch (typeCall)
        {
            case "qty":
                queryString += " SUM(QTY_SHIP)  ";
                break;
            default:
                queryString += " SUM(TOTAL_PRICE) ";
                break;
        }


        queryString += " FROM  QCTL.INVC_HEADER INVC_HEADER ";
        queryString += " FULL OUTER JOIN QCTL.INVC_DETAIL INVC_DETAIL ON INVC_DETAIL.INH_AUTO_KEY = INVC_HEADER.INH_AUTO_KEY ";
        queryString += " FULL OUTER JOIN QCTL.PARTS_MASTER PARTS_MASTER ON PARTS_MASTER.PNM_AUTO_KEY = INVC_DETAIL.PNM_AUTO_KEY ";

        queryString += " WHERE INVC_HEADER.INVOICE_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND INVC_HEADER.INVOICE_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY') ";

        if (strPN != string.Empty)
        {
            queryString += " AND PARTS_MASTER.PN = '" + strPN + "' ";
        }


      
        
        if (strCMP_AUTO_KEY != string.Empty)
        {

            queryString += " AND INVC_HEADER.CMP_AUTO_KEY = " + strCMP_AUTO_KEY + "  ";
        }
        

        


        return  GenUtils.oracleGetCount(queryString);



    }

    private static DataTable lookupCustomPricing(string strCompanyCode)
    {

      
        //DataTable dtTemp = null;

        string queryString = " SELECT PNM_AUTO_KEY, UNIT_PRICE FROM PRICES ";

        queryString += " FULL OUTER JOIN companies on companies.cmp_auto_key = prices.cmp_auto_key ";

        queryString += " WHERE Lower(companies.COMPANY_CODE) = '" + strCompanyCode + "' " ;







        return GenUtils.buildOracleTable(queryString);

  
    }

    private static Decimal getWeekSalesperSTCCode(string fromDate, string toDate, string whereClause)
    {

        //DataTable dtTemp = null;
        //decimal decTemp = 0m;
        string queryString = "SELECT ";


        queryString += " SUM(TOTAL_PRICE) FROM  QCTL.INVC_HEADER INVC_HEADER ";
        queryString += " FULL OUTER JOIN QCTL.INVC_DETAIL INVC_DETAIL ON INVC_DETAIL.INH_AUTO_KEY = INVC_HEADER.INH_AUTO_KEY ";
        queryString += " FULL OUTER JOIN QCTL.PARTS_MASTER PARTS_MASTER ON PARTS_MASTER.PNM_AUTO_KEY = INVC_DETAIL.PNM_AUTO_KEY ";

        queryString += " WHERE INVC_HEADER.INVOICE_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND INVC_HEADER.INVOICE_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY') ";

        queryString += whereClause;   //AND (STC_AUTO_KEY = 20)


        queryString += " AND INVC_HEADER.POST_DESC = 'Posted'  ";

       

        return GenUtils.oracleGetCount(queryString);

    }


    private static DataTable loadCompaniesList()
    {

        //DataTable dtTemp = null;
        string queryString = "SELECT ";

       

        queryString += "  company_name, COMPANIES.CMP_AUTO_KEY, COMPANY_CODE, SALESPERSON_NAME ";
        queryString += " FROM COMPANIES  ";
        queryString += " FULL OUTER JOIN SALESPERSON ON COALESCE(COMPANIES.SPN_AUTO_KEY, 19) = SALESPERSON.SPN_AUTO_KEY ";
        queryString += " where Companies.customer_flag = 'T'  ";
        queryString += " ORDER BY Companies.COMPANY_NAME ";

       

        return GenUtils.buildOracleTable(queryString);

    }

   


    private static DataTable loadPartsList()
    {


        //DataTable dtTemp = null;
        string queryString = "SELECT ";


        queryString += " PN, DESCRIPTION, LIST_PRICE, EXCH_LIST_PRICE, PNM_AUTO_KEY from PARTS_MASTER p";

        queryString += " where  (p.STC_AUTO_KEY = 2  OR  p.STC_AUTO_KEY = 4   OR p.STC_AUTO_KEY = 6 OR p.STC_AUTO_KEY = 12  OR p.STC_AUTO_KEY = 20 /*  OR p.STC_AUTO_KEY = 10 */) ";

        queryString += " AND IFC_AUTO_KEY = 1 or IFC_AUTO_KEY = 4 ";

        queryString += " AND (LIST_PRICE > 0 OR EXCH_LIST_PRICE > 0 ) ";


        queryString += " ORDER BY PN ";

  

       

        return GenUtils.buildOracleTable(queryString);

    }









    private static DataTable loadSalesOrders(string fromDate, string toDate, string SalesReps, string CompanyCodes, string runType)
    {
        //DataTable dtTemp = null;
       
        
        string queryString = "SELECT ";

        string companyKeys = string.Empty;
        string salesRepKeys = string.Empty;


        if (runType != "CalculateAllTotals")
        {
            //--------------- if Sales Reps or CompanyCodes != string.Empty - we need to get a list of the sales reps
        }


        queryString += " SO_DETAIL.SOD_AUTO_KEY, COMP.COMPANY_CODE, SLSP.SALESPERSON_CODE, SLSP.SALESPERSON_NAME, SO_HEADER.SOH_AUTO_KEY, SO_HEADER.TOTAL_COST, SO_HEADER.BILL_NAME, SO_HEADER.COMPANY_REF_NUMBER, SO_HEADER.TOTAL_PRICE, SO_DETAIL.UNIT_PRICE, PARTS_MASTER.PN, SO_HEADER.ENTRY_DATE, SO_DETAIL.QTY_ORDERED,   SO_DETAIL.QTY_INVOICED, SO_DETAIL.DELIVERY_DATE, SO_HEADER.CMP_AUTO_KEY , PARTS_MASTER.PNM_AUTO_KEY    ";

        queryString += " FROM  QCTL.PARTS_MASTER PARTS_MASTER, QCTL.SO_DETAIL SO_DETAIL, QCTL.SO_HEADER SO_HEADER, QCTL.COMPANIES COMP, QCTL.SALESPERSON SLSP ";

        //queryString += " FULL OUTER JOIN  QCTL.SALESPERSON SLSP ON SLSP.SPN_AUTO_KEY = COMP.SPN_AUTO_KEY ";

        queryString += " WHERE SO_HEADER.SOH_AUTO_KEY = SO_DETAIL.SOH_AUTO_KEY ";
        queryString += " AND PARTS_MASTER.PNM_AUTO_KEY = SO_DETAIL.PNM_AUTO_KEY ";

        queryString += " AND COMP.CMP_AUTO_KEY = SO_HEADER.CMP_AUTO_KEY ";

        queryString += " AND SLSP.SPN_AUTO_KEY = COALESCE(SO_HEADER.SPN_AUTO_KEY, 19) ";



        if (fromDate != string.Empty && toDate != string.Empty)
        {
            if (GenUtils.IsDate(fromDate) && GenUtils.IsDate(toDate))
            {
                queryString += " AND (SO_HEADER.ENTRY_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND SO_HEADER.ENTRY_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY')) ";
            }
        }
        else
        {
            queryString += " AND (SO_HEADER.ENTRY_DATE = to_date('" + fromDate + "' , 'MM/DD/YY')) ";
        }


        if (runType != "CalculateAllTotals")
        {

            short shLp;
            bool firstplaced = false;

            if (companyKeys != String.Empty)
            {
                string[] tmp = companyKeys.Split(',');
                queryString += " AND (";
                for (shLp = 0; shLp <= tmp.GetUpperBound(0); shLp++)
                {
                    if (firstplaced)
                    {
                        queryString += " OR ";
                    }
                    else
                    {
                        firstplaced = true;
                    }
                    queryString += " SO_HEADER.CMP_AUTO_KEY = " + tmp[shLp].Trim().ToLower() + " ";

                }
                queryString += ") ";

            }

            firstplaced = false;

            if (salesRepKeys != String.Empty)
            {
                string[] tmp = salesRepKeys.Split(',');
                queryString += " AND (";
                for (shLp = 0; shLp <= tmp.GetUpperBound(0); shLp++)
                {
                    if (firstplaced)
                    {
                        queryString += " OR ";
                    }
                    else
                    {
                        firstplaced = true;
                    }
                    queryString += " SO_HEADER.CMP_AUTO_KEY = " + tmp[shLp].Trim().ToLower() + " ";

                }
                queryString += ") ";

            }


        }

        queryString += " ORDER BY SLSP.SALESPERSON_CODE, BILL_NAME, PN, SO_HEADER.ENTRY_DATE  ";


     

        return GenUtils.buildOracleTable(queryString);

    }


    private static Decimal CalculateSalesForPeriod(string salesType, string salesPeriod, DateTime ReportFromDate, string strParameter , DateTime ReportToDate) 
    {

        string fromDate = string.Empty;
        string toDate = string.Empty;
        DateTime dtFromDate;
        int subtractDays = 0;


        toDate = string.Format("{0:MM/dd/yyyy}", ReportToDate);

        switch (salesPeriod)
        {
            case "year":
                {
                    fromDate = "1/1/" + ReportFromDate.Year.ToString();
                    break;
                }
            case "quarter":
                {
                    fromDate = BeginningofQuarter(ReportFromDate);
                    break;
                }
            case "week":
                {
                    subtractDays = 0 - ReportToDate.DayOfWeek;
                    subtractDays++;
                    dtFromDate = ReportFromDate.AddDays(subtractDays);
                    fromDate = string.Format("{0:MM/dd/yyyy}", dtFromDate);
                    break;
                }
            case "specifiedRange":
                fromDate = string.Format("{0:MM/dd/yyyy}", ReportFromDate);
                toDate = string.Format("{0:MM/dd/yyyy}", ReportToDate);
                break;

            default:
                {
                    return 0m;
                }
        }


        string queryString = string.Empty;
        switch (salesType)
        {
            case "SalesByCompany":
                {
                    queryString = "SELECT  SUM(TOTAL_PRICE)  FROM  QCTL.INVC_HEADER INVC_HEADER ";
                    queryString += " WHERE INVC_HEADER.INVOICE_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND INVC_HEADER.INVOICE_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY') ";
                    queryString += " AND INVC_HEADER.CMP_AUTO_KEY = " + strParameter + " ";
                    queryString += " AND POST_DESC = 'Posted' ";
                    //queryString += " ORDER BY INVC_HEADER.INVOICE_DATE ";
                    break;

                }

            case "AllSalesByPeriod":

                queryString = "SELECT  SUM(TOTAL_PRICE)  FROM  QCTL.INVC_HEADER INVC_HEADER ";
                queryString += " WHERE INVC_HEADER.INVOICE_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND INVC_HEADER.INVOICE_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY') ";
                queryString += " AND POST_DESC = 'Posted' ";
                //queryString += " ORDER BY INVC_HEADER.INVOICE_DATE ";
                break;

            case "AllOrdersByPeriod":

                queryString = "SELECT  SUM(TOTAL_PRICE)  FROM  QCTL.SO_HEADER SOH_HEADER ";
                queryString += " WHERE SOH_HEADER.ENTRY_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND SOH_HEADER.ENTRY_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY') ";
                //queryString += " ORDER BY SOH_HEADER.ENTRY_DATE ";
                break;

            case "PartsSoldByPeriod":


                queryString = "SELECT SUM(INVC_DETAIL.UNIT_PRICE * INVC_DETAIL.QTY_SHIP)   ";
                queryString += " FROM QCTL.INVC_DETAIL INVC_DETAIL ";
                queryString += " FULL OUTER JOIN  QCTL.INVC_HEADER INVC_HEADER ON INVC_HEADER.INH_AUTO_KEY = INVC_DETAIL.INH_AUTO_KEY ";
                queryString += " FULL OUTER JOIN QCTL.PARTS_MASTER PARTS_MASTER ON PARTS_MASTER.PNM_AUTO_KEY = INVC_DETAIL.PNM_AUTO_KEY ";
                queryString += " WHERE INVC_DETAIL.PNM_AUTO_KEY = '" + strParameter +"' ";
                queryString += " AND INVC_HEADER.INVOICE_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND INVC_HEADER.INVOICE_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY') ";

                queryString += " AND POST_DESC = 'Posted' ";
                //queryString += " ORDER BY INVC_DETAIL.IND_AUTO_KEY ";
                break;

        }


        return GenUtils.oracleGetCount(queryString);


    }

    private static string BeginningofQuarter(DateTime ReportFromDate)
    {

        string fromDate = string.Empty;
        switch (ReportFromDate.Month)
        {
            case 1:
                fromDate = "1/1/" + ReportFromDate.Year.ToString();
                break;
            case 2:
                fromDate = "1/1/" + ReportFromDate.Year;
                break;
            case 3:
                fromDate = "1/1/" + ReportFromDate.Year;
                break;
            case 4:
                fromDate = "4/1/" + ReportFromDate.Year;
                break;
            case 5:
                fromDate = "4/1/" + ReportFromDate.Year;
                break;
            case 6:
                fromDate = "4/1/" + ReportFromDate.Year;
                break;
            case 7:
                fromDate = "7/1/" + ReportFromDate.Year;
                break;
            case 8:
                fromDate = "8/1/" + ReportFromDate.Year;
                break;
            case 9:
                fromDate = "9/1/" + ReportFromDate.Year;
                break;
            case 10:
                fromDate = "10/1/" + ReportFromDate.Year;
                break;
            case 11:
                fromDate = "10/1/" + ReportFromDate.Year;
                break;
            case 12:
                fromDate = "10/1/" + ReportFromDate.Year;
                break;

        }

        return fromDate;

    }

    private static DataTable loadCompanyForecasts(string fiscalYear)
    {

        DataTable dtTemp = null;
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);

        string queryString = string.Empty;


            queryString = " SELECT CompanyName, SalesGoal FROM mktg_CompanyGoals WHERE FiscalYear = '" + fiscalYear + "';";
       


        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;
        SqlDataAdapter cacheDA = new SqlDataAdapter();
        cacheDA.SelectCommand = selectCMD;
        DataSet cacheDS = new DataSet();
        try
        {
            sqlConn.Open();
            cacheDA.Fill(cacheDS, "budgetTotals");
            dtTemp = cacheDS.Tables["budgetTotals"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }


        return dtTemp;


    }



    private static DataTable getSalesRepsKeysxxxxxxxx(string SalesReps)
    {
        
        string[] nSalesReps = SalesReps.Trim().Split(',');

        string queryString = "SELECT ";



        queryString += "  spn_auto_Key ";
        queryString += "  from SALESPERSON ";
        queryString += " where ( ";

        int i;
        bool firstPlaced = false;

        for (i=0; i <= nSalesReps.GetUpperBound(0); i++)
        {
            if (firstPlaced)
            {
                queryString += " OR ";
            }
            else
            {
                firstPlaced = true;
            }

            queryString += " lower(SALESPERSON_CODE) = '" + nSalesReps[i].Trim().ToLower() + "' ";



        }

        queryString += " )";

        
       

        return GenUtils.buildOracleTable(queryString);

      

    }

    private static DataTable loadPartsForecasts(string fiscalYear)
    {

        DataTable dtTemp = null;
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);

        string queryString = string.Empty;


        queryString = " SELECT PN, SalesGoal FROM mktg_PartGoals WHERE FiscalYear = '" + fiscalYear + "';";



        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;
        SqlDataAdapter cacheDA = new SqlDataAdapter();
        cacheDA.SelectCommand = selectCMD;
        DataSet cacheDS = new DataSet();
        try
        {
            sqlConn.Open();
            cacheDA.Fill(cacheDS, "budgetTotals");
            dtTemp = cacheDS.Tables["budgetTotals"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }


        return dtTemp;


    }


    private static Decimal loadForecast(string forecastType, string forecastParameter, string fiscalYear, DateTime ReportFromDate)
    {
        decimal decTemp = 0m;

        DataTable dtTemp = null;
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);

        string queryString = string.Empty;

        if (forecastType == "quarter")
        {
            queryString = " SELECT SUM(weeklyGoal) FROM mktg_SalesGoals WHERE FiscalYear = '" + fiscalYear + "' AND effective = 1 AND qtr = " + forecastParameter + ";";
        }
        else if (forecastType == "year")
        {
            queryString = " SELECT SUM(weeklyGoal) FROM mktg_SalesGoals WHERE FiscalYear = '" + fiscalYear + "' AND effective = 1 ;";
        }
        else if (forecastType == "week")
        {
            queryString = " SELECT weeklyGoal FROM mktg_SalesGoals WHERE ('" + ReportFromDate + "' BETWEEN calendarFrom AND  calendarTo) AND effective = 1 ;";
        }
        else if (forecastType == "week-workdays")
        {
            queryString = " SELECT workDays FROM mktg_SalesGoals WHERE ('" + ReportFromDate + "' BETWEEN calendarFrom AND  calendarTo) AND effective = 1 ;";
        }
        else if (forecastType == "company")
        {

            queryString = " SELECT SalesGoal FROM mktg_CompanyGoals WHERE FiscalYear = '" + fiscalYear + "' AND CompanyName = '" + forecastParameter + "' ;";
        }

        else if (forecastType == "part")
        {
            queryString = " SELECT SalesGoal FROM mktg_PartGoals WHERE FiscalYear = '" + fiscalYear + "' AND PN = '" + forecastParameter + "' ;";
        }


        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;
        SqlDataAdapter cacheDA = new SqlDataAdapter();
        cacheDA.SelectCommand = selectCMD;
        DataSet cacheDS = new DataSet();
        try
        {
            sqlConn.Open();
            cacheDA.Fill(cacheDS, "budgetTotals");
            dtTemp = cacheDS.Tables["budgetTotals"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }


        if (dtTemp != null)
        {
            if (dtTemp.Rows.Count > 0)
            {
                decTemp = Convert.ToDecimal(dtTemp.Rows[0][0].ToString());
            }

        }

        return decTemp;


    }


    protected string sendMail(string From, string To, string Subject, string Body, bool useHTML, bool useImage, string AttachFile)
    {

        //string file = "data.xls";
        string thisResult = string.Empty;
        string bodyBuilder = string.Empty;

        if (useHTML)
        {
            bodyBuilder = "<HTML><BODY style='font-family:verdana;font-size:14px;color:#333333;'>";
            if (useImage)
            {
                bodyBuilder += "<div style='margin-right:5px auto; text-align:right;padding-right:5px;'><img src='http://jpesmartbuyer.com/images/logo-JPE_000.gif' border='0' hspace='20' vspace='20'></div>";

            }


            bodyBuilder += Body + "</BODY></HTML>";

            Body = bodyBuilder;

        }


        string server = dtGlobalAdmin.Rows[0]["smtpServer"].ToString();  //---"192.168.10.2";

        //---string server = "smtp.comcast.net";


        MailMessage message = new MailMessage(From, To, Subject, Body);
        message.IsBodyHtml = useHTML;

        if (AttachFile != string.Empty)
        {

            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(AttachFile, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            //ContentDisposition disposition = data.ContentDisposition;
            //disposition.CreationDate = System.IO.File.GetCreationTime(AttachFile);
            //disposition.ModificationDate = System.IO.File.GetLastWriteTime(AttachFile);
            //disposition.ReadDate = System.IO.File.GetLastAccessTime(AttachFile);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);
        }

        SmtpClient client = new SmtpClient(server);

        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(dtGlobalAdmin.Rows[0]["smtpUserName"].ToString(), dtGlobalAdmin.Rows[0]["smtpPassword"].ToString());

        //-----System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential("cyberwiz@comcast.net", "att31227");  


        //if ((bool)Page.Session["blnOnDevServer"] || !(bool)Page.Session["blnOnDevServer"])
        //{


            client.UseDefaultCredentials = false;
            client.Credentials = SMTPUserInfo;
            client.Port = Convert.ToInt32(dtGlobalAdmin.Rows[0]["smtpPort"].ToString());  //---- 25;  //----587

            try
            {
                client.Send(message);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                      ex.ToString());
            }


        //}
        //else
        //{

        //    thisResult = VBCLass1.Dosend(To, From, Subject, Body, AttachFile);


        //}


        return thisResult;


    }





    private static DataTable loadDistributionList(string ReportID)
    {

        DataTable dtTemp = null;
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);

        string queryString = string.Empty;

        switch (ReportID)
        {
            case "blnOnDevServer": 
                queryString = " SELECT * FROM mktg_ReportDistributionList WHERE ReportID = 'DailySR' AND slspCode = 'SR' ORDER BY Parameter1;";
                break;
            default:
                queryString = " SELECT * FROM mktg_ReportDistributionList WHERE ReportID = '" + ReportID + "' ORDER BY Parameter1;";
                break;
        }




        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;
        SqlDataAdapter cacheDA = new SqlDataAdapter();
        cacheDA.SelectCommand = selectCMD;
        DataSet cacheDS = new DataSet();
        try
        {
            sqlConn.Open();
            cacheDA.Fill(cacheDS, "distList");
            dtTemp = cacheDS.Tables["distList"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }


        return dtTemp;


    }


}
