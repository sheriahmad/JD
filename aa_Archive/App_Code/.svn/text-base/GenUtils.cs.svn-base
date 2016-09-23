using System;
using System.Collections.Generic;
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


/// <summary>
/// Summary description for Class1
/// </summary>
public class GenUtils
{
   

	public GenUtils()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Compute the distance between two strings.
    /// </summary>
    

    public static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Step 1
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Step 2
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Step 3
        for (int i = 1; i <= n; i++)
        {
            //Step 4
            for (int j = 1; j <= m; j++)
            {
                // Step 5
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                // Step 6
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        // Step 7
        return d[n, m];
    }

    public static DataTable loadGlobalAdmin(int useGlobalLine)
    {

        DataTable dtTemp = null;

        DataSet dsGlobal = new DataSet();
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        string queryString = "SELECT * from ecom_GlobalAdmin WHERE pkID = " + useGlobalLine + ";";

        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;

        SqlDataAdapter globalDA = new SqlDataAdapter();
        globalDA.SelectCommand = selectCMD;

        try
        {
            sqlConn.Open();
            globalDA.Fill(dsGlobal, "Global");
            dtTemp = dsGlobal.Tables["Global"];
        }
        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }
        return dtTemp;
    }

    public static DataTable loadSmartStockHistory(int this_CMP_AUTO_KEY)
    {
        string conn = string.Empty;
        DataTable dtTemp = null;
        DataSet dsTemp = new DataSet();
        conn = ConfigurationManager.ConnectionStrings["SQL2005_623673_jetConnectionString"].ConnectionString;
      
        SqlConnection stockQueueConn = new SqlConnection(conn);
        string queryString = string.Empty;
        queryString = "SELECT TOP(50) * FROM dbo.ecom_StockScanQueue  WHERE CompanyKey = " + this_CMP_AUTO_KEY + " AND ScanReversed = 0 ORDER BY DateTime DESC;";
        SqlCommand cmd = new SqlCommand(queryString, stockQueueConn);
        SqlDataAdapter tempDA = new SqlDataAdapter();
        tempDA.SelectCommand = cmd;
        try
        {
            stockQueueConn.Open();
            tempDA.Fill(dsTemp, "Temp");
            dtTemp = dsTemp.Tables["Temp"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error reading the Invoice File: " + ex.ToString());
        }
        finally
        {
            stockQueueConn.Close();
        }
        return dtTemp;
    }

    public static DataTable loadAdminUsersTable(int salesPersonAutoKey)
    {
        string conn = string.Empty;
        DataTable dtTemp = null;
        DataSet dsTemp = new DataSet();
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;

        SqlConnection AdminConn = new SqlConnection(conn);
        string queryString = string.Empty;
        queryString ="select accounts.pkAccountKey, accounts.Company, accounts.FirstName, accounts.JobTitle, accounts.EmailAddress, accounts.Active, accounts.JPEApproved, accounts.AccountType, accounts.RolodexKey, accounts.QuantumCompanyCode, hits.Logins, hits.Accumulator, login.AttemptDateTime, accounts.MySalesPersonKey ";

        queryString += " from ecom_UserAccounts  accounts ";
        queryString += " LEFT OUTER JOIN ecom_HitsCounter hits ON   accounts.pkAccountKey = hits.UserKey ";
        queryString += " LEFT OUTER JOIN ecom_loginQueue login ON   accounts.pkAccountKey = login.fkAccountKey ";
        queryString += " WHERE NOT EXISTS (SELECT AttemptDateTime from ecom_LoginQueue where fkAccountKey = accounts.pkAccountKey AND pkLogin > login.pkLogin)  ";
        if (salesPersonAutoKey > 0)
        {
            queryString += " AND MySalesPersonKey = " + salesPersonAutoKey + " ";
        }
        queryString += " order by Company, FirstName ";

        SqlCommand cmd = new SqlCommand(queryString, AdminConn);
        SqlDataAdapter tempDA = new SqlDataAdapter();
        tempDA.SelectCommand = cmd;
        try
        {
            AdminConn.Open();
            tempDA.Fill(dsTemp, "Temp");
            dtTemp = dsTemp.Tables["Temp"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error reading the Admin Users File: " + ex.ToString());
        }
        finally
        {
            AdminConn.Close();
        }
        return dtTemp;
    }

    public static DataTable loadWebsiteDown(int useWSLine)
    {

        DataTable dtTemp = null;

        DataSet dsGlobal = new DataSet();
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        string queryString = "SELECT * from ecom_WebsiteDown WHERE [key] = " + useWSLine + ";";

        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;

        SqlDataAdapter globalDA = new SqlDataAdapter();
        globalDA.SelectCommand = selectCMD;

        try
        {
            sqlConn.Open();
            globalDA.Fill(dsGlobal, "Global");
            dtTemp = dsGlobal.Tables["Global"];
        }

        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }

        return dtTemp;
    }

    public static void FocusControlOnPageLoad(string ClientID,
                                      System.Web.UI.Page page)
    {
        page.RegisterClientScriptBlock("CtrlFocus",

        @"<script> 
      function ScrollView()
      {
         var el = document.getElementById('" + ClientID + @"')
         if (el != null)
         {        
            el.scrollIntoView();
            el.focus();
         }
      }
      window.onload = ScrollView;
      </script>");

    }
    public static String MakeUCaseNumChar(string strThisString)
    {
        string strTemp = string.Empty;
        int intStrEnum;
        int counter = 0;
        if (strThisString == null)
        {
            return string.Empty;
        }

        CharEnumerator charEnum = strThisString.GetEnumerator();
        while (charEnum.MoveNext())
        {
            intStrEnum = Convert.ToInt32(strThisString[counter]);
            //  numbers 48 - 57  upper 65 - 90 lower case 97 - 122
            if ((intStrEnum >= 97) && (intStrEnum <= 122))
            {
                strTemp += (char)(intStrEnum - 32) ;
            }
            
            else if ((intStrEnum >= 48) && (intStrEnum <= 57)  || (intStrEnum >= 65) && (intStrEnum <= 90))
            {
                strTemp += (char)intStrEnum;
            }
            counter += 1;
        }
        return strTemp;
    }

    public static String MakeUniformPartNumber(string strThisString)
    {
        string strTemp = strThisString.Trim().ToUpper();
        return MakeUCaseNumChar(strTemp);
    }

    public static String MakeUniformQtyString(string strThisString)
    {
        string strTemp = strThisString.Trim();
        strTemp = MakeNumbersOnly(strTemp);
        if (strTemp == string.Empty)
        {
            strTemp = "0";
        }
        return strTemp;
    }

    public static String MakeNumbersOnly(string strThisString)
    {
        string strTemp = string.Empty;
        int intStrEnum;
        int counter = 0;

        CharEnumerator charEnum = strThisString.GetEnumerator();
        while (charEnum.MoveNext())
        {
            intStrEnum = Convert.ToInt32(strThisString[counter]);
            //  numbers 48 - 57  upper 65 - 90 lower case 97 - 122
            if ((intStrEnum >= 48) && (intStrEnum <= 57))
            {
                strTemp += (char)intStrEnum;
            }
            counter += 1;
        }
        if (strTemp == string.Empty)
        {
            strTemp = "0";
        }
        return strTemp;
    }

    public static String StripQTYidString(string thisString)
    {
        if (thisString.IndexOf("#") > 0)
        {
            thisString = thisString.Substring(0, thisString.IndexOf("#"));
        }
        return thisString;
    }

    public static DataTable FindParts(string strWhereString, string TopCount)
    {
        string queryString = "SELECT  ";
        
        queryString +=  " U.UOM_AUTO_KEY, P.UOM_AUTO_KEY AS P_UOM_AUTO_KEY, P.PNM_AUTO_KEY AS P_PNM_AUTO_KEY, P.PN, P.PN_STRIPPED, P.DESCRIPTION, ALT.ALT_PNM_AUTO_KEY, P2.PN AS ALT_PN, P2.PN_STRIPPED AS ALT_PN_STRIPPED, P2.DESCRIPTION AS ALT_DESCRIPTION, P2.LIST_PRICE AS ALT_LIST_PRICE, P2.STC_AUTO_KEY AS ALT_STC_AUTO_KEY, P2.QTY_AVAILABLE AS ALT_QTY_AVAILABLE, U.DESCRIPTION AS UNIT_OF_MEASURE, P.IC_UDF_015 AS ECCN, P.STC_AUTO_KEY AS P_STC_AUTO_KEY, P.IFC_AUTO_KEY, P.LIST_PRICE, P.EXCH_LIST_PRICE, P.QTY_AVAILABLE, SC.DESCRIPTION AS STOCK_DESC ";
        queryString += " FROM parts_master p ";
        queryString += " LEFT OUTER JOIN UOM_CODES u on u.UOM_AUTO_KEY = p.UOM_AUTO_KEY ";
        queryString += " LEFT OUTER JOIN STOCK_CATEGORY_CODES sc ON sc.STC_AUTO_KEY = p.STC_AUTO_KEY ";
        queryString += " LEFT OUTER JOIN QCTL.ALTERNATES_PARTS_MASTER ALT ON ALT.PNM_AUTO_KEY = P.PNM_AUTO_KEY ";
        queryString += " LEFT OUTER JOIN QCTL.PARTS_MASTER P2 ON ALT.ALT_PNM_AUTO_KEY =  P2.PNM_AUTO_KEY";
        queryString += strWhereString;

        queryString += " AND (ALT.ATC_AUTO_KEY != 6 OR ALT.ATC_AUTO_KEY IS NULL) ";

        queryString += " AND (P.STC_AUTO_KEY = 2 OR P.STC_AUTO_KEY = 3 OR P.STC_AUTO_KEY = 4 OR P.STC_AUTO_KEY = 6 OR   P.STC_AUTO_KEY = 10  OR P.STC_AUTO_KEY = 11 OR P.STC_AUTO_KEY = 12 OR P.STC_AUTO_KEY = 20  ) ";

        queryString += TopCount;


        queryString += " ORDER BY p.PN ";

        return GenUtils.buildOracleTable(queryString);
}


    public static DataTable GetCacheforSessionID(string thisSessionID)
    {
        //-------------------------- Retrieve a dataTable with the current Session Cache Information

        DataTable dtTemp = null;
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        string queryString = "SELECT * FROM ecom_ActiveCache WHERE SessionID = '" + thisSessionID + "';";
        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;
        SqlDataAdapter cacheDA = new SqlDataAdapter();
        cacheDA.SelectCommand = selectCMD;
        DataSet cacheDS = new DataSet();
        try
        {
            sqlConn.Open();
            cacheDA.Fill(cacheDS, "sessionCache");
            dtTemp = cacheDS.Tables["sessionCache"];
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

    public static string findImageLink(int intCurRow, DataTable dtCacheMaster)
    {
        if (dtCacheMaster.Rows[intCurRow]["image_IMG1_KEY"].ToString() != string.Empty && dtCacheMaster.Rows[intCurRow]["image_IMG1_KEY"].ToString() != "0")
        {
            return "http://jetserve1.jetpartsengineering.com/qimg/" + dtCacheMaster.Rows[intCurRow]["image_IMG1_KEY"].ToString() + ".qid";
        }
        else
        {
            return "images\\noimage.png";
        }

    }

    public static int FindRowforStrippedPartNumber(string strippedPartNumber, DataTable dtCacheMaster)
    {
        //----- returns a list of the NHA associated parts for a given part
        string temp = string.Empty;
       
        
        int dataRowEnum = -1;

        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
        {
            dataRowEnum++;
            //temp = myMasterRow["partNumberStripped"].ToString();
            if (myMasterRow["partNumberStripped"].ToString() == MakeUniformPartNumber(strippedPartNumber))
            {
                return dataRowEnum;
            }
        }
        return dataRowEnum;

    }

    public static DataTable FindNHAItems(string strJPEPartNumber, string strOEMPartNumber)
    {

        System.Data.DataTable dtTemp = null;


        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_CTDB_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);


        string queryString = "Select pall.*, n.NHAId, nha.ATAChapter, n.PartsPerNHA, nha.NHA from Part p JOIN Part2NHA n ON n.PartID = p.PartID JOIN Part2NHA nall ON nall.NHAID = n.NHAId JOIN Part pall ON pall.PartID = nall.PartID Join NHA nha ON nha.NHAID = nall.NHAId ";

        
        switch (strJPEPartNumber)
        {
            case "NHA":
                {
                    queryString += "WHERE  REPLACE(nha.NHA, '-', '') = '" + MakeUniformPartNumber(strOEMPartNumber) + "' ";


                   

                    break;
                }
            default:
                {

                    queryString += "WHERE REPLACE(p.JPEPN, '-', '') = '" + MakeUniformPartNumber(strJPEPartNumber) + "' ";
                    if (strOEMPartNumber != string.Empty)
                    {
                        queryString += " or REPLACE(p.OEMPN, '-', '') = '" + MakeUniformPartNumber(strOEMPartNumber) + "' ";
                    }


                    break;
                }
        }
        queryString += " ORDER BY nha.NHA; ";



        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;

        SqlDataAdapter nhaDA = new SqlDataAdapter();
        nhaDA.SelectCommand = selectCMD;



        DataSet mynhaDS = new DataSet();


        try
        {
            sqlConn.Open();
            nhaDA.Fill(mynhaDS, "NHA");
            dtTemp = mynhaDS.Tables["NHA"];
        }

        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);
            //lblLoginError.Text = "Login Error: " + ex.ToString();
        }
        finally
        {
            sqlConn.Close();
        }

        return dtTemp;


    }

    public static int[] FindAltPartNumber(int intCurRow, DataTable dtCacheMaster, string thisNHA)
    {
        //----- returns a list of the NHA's associated

        if (thisNHA == string.Empty)
        {
            thisNHA = dtCacheMaster.Rows[intCurRow]["nha_NHA"].ToString();
        }
       
        int[] temp = new int[1000];
        temp[0] = - 1;
        int tempCounter = -1;
        int dataRowEnum = -1;

        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
        {
            if (myMasterRow["nha_NHA"].ToString() == thisNHA && ++dataRowEnum != intCurRow)
            {
                temp[++tempCounter] = dataRowEnum;
            }
        }
        return temp;

    }

    public static string RemoveReservedChars(string strtoClean)
    {
        strtoClean = strtoClean.Replace("'", string.Empty);
        strtoClean = strtoClean.Replace("\"", string.Empty);


        return strtoClean;
    }

    public static string BuildSalespersonPanel(DataTable dtCompany)
    {
        string temp = string.Empty;
        string thisEmail = "Sales@JetPartsEngineering.com";
        string thisPicture = "images/SalesPersons/SW.jpg";
        string thisPhone = "(206) 281-0963";
        string thisFax = "(206) 838-8487";
        string thisMobile = string.Empty;
        string thisName = "Shawn Wallace";
        if (dtCompany != null)
        {


            if (dtCompany.Rows[0]["EMAIL_ADDRESS"].ToString() != string.Empty)
            {
                thisEmail = dtCompany.Rows[0]["EMAIL_ADDRESS"].ToString();
                thisPicture = "images/SalesPersons/" + dtCompany.Rows[0]["SP_IMAGE"].ToString();
                thisPhone = dtCompany.Rows[0]["PHONE_NUMBER"].ToString();
                thisFax = dtCompany.Rows[0]["FAX_NUMBER"].ToString();
                thisMobile = dtCompany.Rows[0]["MOBILE_PHONE"].ToString();
                thisName = dtCompany.Rows[0]["SALESPERSON_NAME"].ToString();
            }
        }

        temp = "<table class='GenViewPortTable'><tr><td>";
        temp += "<a href='mailto:" + thisEmail + "' border='0'><img src='images/email.png' ID='ibEmailUs1' border = '0' ></a>";
        temp += "</td><td class='salespersonLeftTD'>";
        temp += "<div class='divCallUs'>";
        temp += "Can I help you?</div>";

        if (thisName != string.Empty)
        {
            temp += thisName + "<br />";
        }
        if (thisPhone != string.Empty)
        {
            temp += "Phone: " + thisPhone + "<br />";
        }
        if (thisMobile != string.Empty)
        {
            temp += "Mobile: " + thisMobile + "<br />";
        }
        if (thisFax != string.Empty)
        {
            temp += "Facsimile: " + thisFax + "<br />";
        }
        temp += "<a href='mailto:" + thisEmail + "' >" + thisEmail + "</a><br />";
        temp += "</td><td class='salespersonRightTD'><img src='" + thisPicture + "' class='partsImage' />";

        temp += "</td></tr></table>";

      
        return temp;

    }

    public static DataTable CompanySitesLookup(string thisCompany_Auto_key, int intKeyOverRide)
    {


        //DataTable dtTemp = null;

        string queryString = " select si.SITE_DESCRIPTION, si.ADDRESS1, si.ADDRESS2, si.ADDRESS3, si.CITY, si.STATE, si.ZIP_CODE, si.COUNTRY, co.COMPANY_NAME ";
        queryString += " FROM COMPANY_SITES si ";
        queryString += " RIGHT OUTER JOIN COMPANIES co ON co.CMP_AUTO_KEY = si.CMP_AUTO_KEY ";

        if (intKeyOverRide > -1)
        {
            queryString += " WHERE CO.CMP_AUTO_KEY = " + intKeyOverRide + " ";

        }
        else
        {
            queryString += " WHERE co.CMP_AUTO_KEY = '" + thisCompany_Auto_key + "' ";

        }
       
        


        return GenUtils.buildOracleTable(queryString);

    }

    public static DataTable CompaniesforSalesperson(int spn_auto_key)
    {
        string queryString;
        if (spn_auto_key < 0)
        {

            //queryString = " select company_name, cmp_auto_key from companies where customer_flag ='T' AND (CV_UDF_001 = 'T' OR CV_UDF_002 = 'T') ORDER BY company_name";
            queryString = " select company_name, cmp_auto_key from companies where customer_flag ='T' ORDER BY company_name";


        }
        else
        {
            queryString = "select company_name, cmp_auto_key from companies where spn_auto_key = " + spn_auto_key + " ORDER BY company_name ";

        }
      
        return GenUtils.buildOracleTable(queryString);
   
    }

    public static DataTable RoloDexLookup(string thisRoloDexKey)
    {


        //DataTable dtTemp = null;

        string queryString = " SELECT COMPANIES.COMPANY_NAME, ROLODEX.RDX_CONTACT_NAME, ROLODEX.TITLE, ROLODEX.PHONE_NUMBER, ROLODEX.FAX_NUMBER, ROLODEX.EMAIL_ADDRESS, ROLODEX.ADDRESS1, ROLODEX.ADDRESS2, ROLODEX.ADDRESS3, ROLODEX.CITY, ROLODEX.STATE, ROLODEX.ZIP_CODE, ROLODEX.COUNTRY, COMPANIES.COMPANY_CODE_UPPER, COMPANIES.CMP_AUTO_KEY ";
        queryString += " FROM QCTL.COMPANIES COMPANIES, QCTL.COMPANY_ROLODEX COMPANY_ROLODEX, QCTL.ROLODEX ROLODEX ";
        queryString += " WHERE COMPANIES.CMP_AUTO_KEY = COMPANY_ROLODEX.CMP_AUTO_KEY AND rolodex.rdx_auto_key = company_rolodex.rdx_auto_key ";
        queryString += " AND rolodex.rdx_auto_key = " + thisRoloDexKey + " ";




        return GenUtils.buildOracleTable(queryString);



    }

    public static DataTable CompanyLookup(string Quantum_Auto_Key, string strSalesPictureRootPath, int intKeyOverRide)
    {

        //----------------- Lookup the company information from Quantum for this user

        DataTable dtTemp = null;

        string queryString = "Select CO.CMP_AUTO_KEY, CO.COMPANY_CODE, CO.COMPANY_CODE_UPPER,  CO.CITY, CO.COMPANY_NAME, CO.COUNTRY, CO.SHIP_ADDRESS1, CO.SHIP_ADDRESS2, CO.SHIP_ADDRESS3, CO.SHIP_CITY, CO.SHIP_STATE, CO.SHIP_COUNTRY, CO.SHIP_ZIP_CODE, SA.SALESPERSON_CODE, SA.SALESPERSON_NAME, CO.ADDRESS1, CO.ADDRESS2, CO.ADDRESS3, CO.STATE, CO.ZIP_CODE, CO.CV_UDF_001, CO.CV_UDF_002, CO.CV_UDF_003, CO.CV_UDF_004, CO.CV_UDF_005, ";

        queryString += "SU.EMAIL_ADDRESS, SU.PHONE_NUMBER, SU.MOBILE_PHONE, su.fax_number, CO.MAX_CREDIT, TC.TMC_AUTO_KEY, TC.DESCRIPTION, SA.SALESPERSON_CODE as SP_IMAGE, ";


        queryString += "CO.ADDRESS1, CO.ADDRESS2, CO.ADDRESS3, CO.CITY, CO.STATE, CO.COUNTRY, CO.ZIP_CODE ";

        

        queryString += " FROM COMPANIES CO ";
        queryString += " FULL OUTER JOIN SALESPERSON SA ON COALESCE(CO.SPN_AUTO_KEY, 19) = SA.SPN_AUTO_KEY ";
        queryString += " FULL OUTER JOIN SYS_USERS SU ON SU.SPN_AUTO_KEY = SA.SPN_AUTO_KEY ";
        queryString += " FULL OUTER JOIN TERM_CODES TC on TC.TMC_AUTO_KEY = CO.TMC_AUTO_KEY ";

        if (intKeyOverRide > -1)
        {
            queryString += " WHERE CO.CMP_AUTO_KEY = " + intKeyOverRide + " ";

        }
        else
        {
            queryString += " WHERE CO.CMP_AUTO_KEY = '" + Quantum_Auto_Key + "' ";

        }








       dtTemp = GenUtils.buildOracleTable(queryString);

      
        try
        {
            if (File.Exists(strSalesPictureRootPath + "\\images\\salespersons\\" + dtTemp.Rows[0]["SP_IMAGE"] + ".jpg"))
            {
                dtTemp.Rows[0]["SP_IMAGE"] = dtTemp.Rows[0]["SP_IMAGE"] + ".jpg";
            }
            else if (dtTemp.Rows[0]["SP_IMAGE"].ToString() == "XX")
            {
                dtTemp.Rows[0]["SP_IMAGE"] = "SW.jpg";
            }
            else
            {
                dtTemp.Rows[0]["SP_IMAGE"] = "NoPix.jpg";
            }


        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return dtTemp;

    }

    public static DataTable CompanyShippingLookup(int thisCompanyKey)
    {


        //DataTable dtTemp = null;

        string queryString = " SELECT CODES.SHIP_VIA_CODE, CODES.DESCRIPTION, ACCTS.ACCOUNT_NUMBER, ACCTS.REMARKS ";
        queryString += " FROM SHIP_VIA_ACCOUNTS ACCTS ";
        queryString += " RIGHT OUTER JOIN SHIP_VIA_CODES CODES ON ACCTS.SVC_AUTO_KEY = CODES.SVC_AUTO_KEY ";

        queryString += " WHERE ACCTS.CMP_AUTO_KEY = " + thisCompanyKey + " ";




        return GenUtils.buildOracleTable(queryString);

    }

    public static DataTable ecom_AccountLookup(string myUserName, int pkAccountKey)
    {

        DataTable dtTemp = null;
        DataSet accountDS;
        accountDS = new DataSet();


        string conn = string.Empty;
        string queryString = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        bool useEVerify = false;


        if (myUserName != string.Empty)
        {
            if (myUserName.Length > 8)
            {
                if (myUserName.Substring(0, 8) == "EVERIFY:")
                {
                    useEVerify = true;
                }
            }
        }


        if (useEVerify)
        {
         queryString = "SELECT * FROM ecom_UserAccounts WHERE  LOWER(eMailValidationString) = '" + myUserName.Substring(8).ToLower() + "'; ";
         }
          
        else if (myUserName != string.Empty)
        {

         queryString = "SELECT * FROM ecom_UserAccounts WHERE  LOWER(EmailAddress) = '" + myUserName.ToLower() + "'; ";
         }
        else
        {
            queryString = "SELECT * FROM ecom_UserAccounts WHERE  pkAccountKey = " + pkAccountKey + " ; ";
        }



        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;

        SqlDataAdapter accountDA = new SqlDataAdapter();
        accountDA.SelectCommand = selectCMD;

        try
        {
            sqlConn.Open();
            accountDA.Fill(accountDS, "Accounts");
            dtTemp = accountDS.Tables["Accounts"];
        }

        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);

        }
        finally
        {
            sqlConn.Close();
        }


        return dtTemp;

    }

    public static string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
        {
            return builder.ToString().ToLower();
            
        }
        else
        {
            return builder.ToString();
        }
    }


    
    public static string RandomString(int size, string type)
    {

        StringBuilder builder = new StringBuilder();
        Random random = new Random( );
        char ch;
        for (int i = 0; i < size; i++)
        {
            if (type == "numbers")
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48)));
            }
            else
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));

            }
            builder.Append(ch);
        }

       
        if (type == "lowerCase")
        {
            return builder.ToString().ToLower();

        }
        else
        {
            return builder.ToString();
        }
    }

    /// <summary>
    /// Validates a credit card number using the standard Luhn/mod10
    /// validation algorithm.
    /// </summary>
    /// <param name="cardNumber">Card number, with or without
    ///        punctuation</param>
    /// <returns>True if card number appears valid, false if not
    /// </returns>
    public static bool IsCreditCardValid(string cardNumber)
    {
        const string allowed = "0123456789";
        int i;

        StringBuilder cleanNumber = new StringBuilder();
        for (i = 0; i < cardNumber.Length; i++)
        {
            if (allowed.IndexOf(cardNumber.Substring(i, 1)) >= 0)
                cleanNumber.Append(cardNumber.Substring(i, 1));
        }
        if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
            return false;

        for (i = cleanNumber.Length + 1; i <= 16; i++)
            cleanNumber.Insert(0, "0");

        int multiplier, digit, sum, total = 0;
        string number = cleanNumber.ToString();

        for (i = 1; i <= 16; i++)
        {
            multiplier = 1 + (i % 2);
            digit = int.Parse(number.Substring(i - 1, 1));
            sum = digit * multiplier;
            if (sum > 9)
                sum -= 9;
            total += sum;
        }
        return (total % 10 == 0);
    }


    public static DataTable BuildMyQuotesHistory(int companyAutoKey, string fromDate, string toDate, string CustRefNumber, string findTopX, string HxDaysAllowed)
    {

        //DataTable dtTemp = null;
        string queryString = "SELECT ";


        DateTime NoDatesBefore = DateTime.Now.AddDays(0 - int.Parse(HxDaysAllowed));

        DateTime myFromDate;
        DateTime myToDate;

        if (IsDate(fromDate))
        {
            myFromDate = DateTime.Parse(fromDate);
            if (DateTime.Compare(myFromDate, NoDatesBefore) < 0)
            {
                myFromDate = NoDatesBefore;
            }
        }
        else
        {
            myFromDate = NoDatesBefore;
        }

        if (IsDate(toDate))
        {
            myToDate = DateTime.Parse(toDate);
            if (DateTime.Compare(myToDate, myFromDate) < 0)
            {
                myToDate = DateTime.Now;
            }
        }
        else
        {
            myToDate = DateTime.Now;
        }
        


        queryString += " COMPANIES.COMPANY_NAME, CQ_HEADER.CQ_NUMBER, CQ_HEADER.COMPANY_REF_NUMBER, CQ_HEADER.ENTRY_DATE, CQ_HEADER.EXPIRATION_DATE, PARTS_MASTER.PN, PARTS_MASTER.DESCRIPTION AS PDESCRIPTION, CQ_DETAIL.ITEM_NUMBER, CQ_DETAIL.QTY_QUOTED, CQ_DETAIL.CUSTOMER_PRICE, CQ_STATUS.DESCRIPTION, CQ_HEADER.CQH_AUTO_KEY, CQ_HEADER.TOTAL_PRICE  ";

        queryString += " FROM QCTL.COMPANIES COMPANIES, QCTL.CQ_DETAIL CQ_DETAIL, QCTL.CQ_HEADER CQ_HEADER, QCTL.CQ_STATUS CQ_STATUS, QCTL.PARTS_MASTER PARTS_MASTER ";
        queryString += " WHERE COMPANIES.CMP_AUTO_KEY = CQ_HEADER.CMP_AUTO_KEY ";
        queryString += " AND CQ_DETAIL.CQH_AUTO_KEY = CQ_HEADER.CQH_AUTO_KEY ";
        queryString += " AND PARTS_MASTER.PNM_AUTO_KEY = CQ_DETAIL.PNM_AUTO_KEY ";
        queryString += " AND CQ_STATUS.CQS_AUTO_KEY = CQ_HEADER.CQS_AUTO_KEY ";
 


        //if (fromDate != string.Empty && toDate != string.Empty)
        //{
        //    if (IsDate(fromDate) && IsDate(toDate))
        //    {
              
        //        queryString += " AND (CQ_HEADER.ENTRY_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND CQ_HEADER.ENTRY_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY')) ";
        //    }
        //}



        queryString += " AND (CQ_HEADER.ENTRY_DATE >= to_date('" + string.Format("{0:MM/d/yyy}", myFromDate) + "', 'MM/DD/YY') AND CQ_HEADER.ENTRY_DATE <= to_date('" + string.Format("{0:MM/d/yyy}", myToDate) + "', 'MM/DD/YYYY')) ";



        //if (fromDate != string.Empty || toDate != string.Empty)
        //{
        //    if (IsDate(fromDate) || IsDate(toDate))
        //    {
        //        if (IsDate(fromDate) && IsDate(toDate))
        //        {
        //            queryString += " AND (CQ_HEADER.ENTRY_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND CQ_HEADER.ENTRY_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY')) ";
        //        }
        //        else if (IsDate(fromDate))
        //        {
        //            toDate = string.Format("{0:MM/d/yyy}", DateTime.Now);
        //            queryString += " AND (CQ_HEADER.ENTRY_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND CQ_HEADER.ENTRY_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY')) ";
        //        }
        //    }
        //}

        if (fromDate != string.Empty && toDate != string.Empty)
        {
            queryString += " AND (SO_HEADER.ENTRY_DATE >= to_date('" + fromDate + "', 'MM/DD/YY') AND SO_HEADER.ENTRY_DATE <= to_date('" + toDate + "', 'MM/DD/YYYY')) ";
        }

        short shLp;
        bool firstplaced = false;

        if (CustRefNumber != String.Empty)
        {
            string[] tmp = CustRefNumber.Split(',');
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
                queryString += " LOWER(CQ_HEADER.COMPANY_REF_NUMBER) like '%" + tmp[shLp].Trim().ToLower() + "%' OR LOWER(PARTS_MASTER.PN) like '%" + tmp[shLp].Trim().ToLower() + "%' ";

            }
            queryString += ") ";
        }

        //if (findTopX != string.Empty)
        if (fromDate.Trim() + toDate.Trim() + CustRefNumber.Trim() == string.Empty)
        {
            //queryString += " AND ROWNUM <= 20 ";
        }

        queryString += " AND CQ_HEADER.CMP_AUTO_KEY = " + companyAutoKey + " ";
        queryString += " ORDER BY CQ_HEADER.ENTRY_DATE DESC ";  //, INVC_HEADER.INH_AUTO_KEY ";




        return GenUtils.buildOracleTable(queryString);

    }


    public static string BODateToString(string thisDate)
    {
        string strTemp = string.Empty;


        strTemp = thisDate;

        return strTemp;

    }

    public static DataTable RetrieveBackOrderPOs(string thisPN)
    {

 
        //--------check the purchase order table for incoming
        //string queryString = "select ps.POS_AUTO_KEY, ps.QTY_ORDERED as psQtyOrdered, pn.PN, so.qty_ordered, pod.next_delivery_date, pod.qty_rec, pod.qty_ordered ";
        //queryString += " from purchase_sales ps, parts_master pn, so_detail so, po_detail pod ";
        //queryString += " where ps.sod_auto_key = " + strSOD_AUTO_KEY + " ";
        //queryString += " and so.sod_auto_key = ps.sod_auto_key ";
        //queryString += " and so.PNM_AUTO_KEY = pn.PNM_AUTO_KEY ";
        //queryString += " and pod.pod_auto_key = ps.pod_auto_key ";

        //queryString += " Order By  pod.next_delivery_date ";

        string queryString = "select pod.poh_auto_key, pod.pod_auto_key, pod.entry_date, pn.PN, pod.next_delivery_date, pod.qty_rec, pod.qty_ordered, pod.qty_back_order ";

        queryString += " from  parts_master pn, po_detail pod, po_header poh ";
        queryString += " where pn.pn_stripped = '" + thisPN + "' ";
        queryString += " and pod.pnm_auto_key = pn.pnm_auto_key";
        queryString += " and poh.poh_auto_key = pod.poh_auto_key ";
        queryString += " and poh.open_flag = 'T' ";
        queryString += " and pod.qty_rec < pod.qty_ordered ";
        queryString += " order by NEXT_DELIVERY_DATE ";


        return GenUtils.buildOracleTable(queryString);


    }

    public static DataTable RetrieveBackOrderWoos(string thisPN)
    {
        string queryString = "select  woo.manual_ecd, woo.open_flag, woo.entry_date, woo.due_date, woo.active_flag, woo.kit_qty";
        queryString += " from  parts_master pn, wo_operation woo ";
        queryString += " where pn.PN_STRIPPED = '" + thisPN + "' ";
        queryString += " and woo.Open_flag = 'T' ";
        queryString += " and woo.pnm_auto_key = pn.pnm_auto_key ";
        queryString += " order by Manual_ECD  ";

        return GenUtils.buildOracleTable(queryString);
    }

    public static DataTable BuildWebsiteOrderHistory(int companyAutoKey, string fromDate, string toDate, string CustRefNumber, string findTopX, string HxDaysAllowed)
    {
        //--------------------------Quantum lookup of prior order
        DataTable dtTemp = null;

        DataSet dsws = new DataSet();
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);


        string queryString = "SELECT ";

        //if (findTopX != string.Empty)
        if (fromDate.Trim() + toDate.Trim() + CustRefNumber.Trim() == string.Empty)
        {
            //queryString += " TOP 20 ";
        }
        
        queryString += " * from ecom_OrdersEmailed WHERE ";

        DateTime NoDatesBefore = DateTime.Now.AddDays(0 - int.Parse(HxDaysAllowed));
        DateTime myFromDate;
        DateTime myToDate;

        if (IsDate(fromDate))
        {
            myFromDate = DateTime.Parse(fromDate);
            if (DateTime.Compare(myFromDate, NoDatesBefore) < 0)
            {
                myFromDate = NoDatesBefore;
            }
        }
        else
        {
            myFromDate = NoDatesBefore;
        }

        if (IsDate(toDate))
        {
            myToDate = DateTime.Parse(toDate);
            if (DateTime.Compare(myToDate, myFromDate) < 0)
            {
                myToDate = DateTime.Now;
            }
        }
        else
        {
            myToDate = DateTime.Now;
        }

        queryString += " (DateTime >= '" + myFromDate.ToString() + "' AND DateTime <= '" + myToDate.ToString() + "') ";

        short shLp;
        bool firstplaced = false;

        if (CustRefNumber != String.Empty)
        {
            string[] tmp = CustRefNumber.Split(',');
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
                queryString += " LOWER(PONumber) like '%" + tmp[shLp].Trim().ToLower() + "%' OR LOWER(OrderDetails) like '%" + tmp[shLp].Trim().ToLower() + "%' ";

            }
            queryString += ") ";
        }
      
        queryString += " AND CompanyKey = " + companyAutoKey + " ";
        queryString += " ORDER BY DateTime DESC ";

        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;

        SqlDataAdapter globalDA = new SqlDataAdapter();
        globalDA.SelectCommand = selectCMD;

        try
        {
            sqlConn.Open();
            globalDA.Fill(dsws, "ws");
            dtTemp = dsws.Tables["ws"];
        }

        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }
        return dtTemp;
    }


    public static DataTable BuildCompleteOrderHistory(int companyAutoKey, string fromDate, string toDate, string CustRefNumber, string findTopX, string HxDaysAllowed)
    {
        //--------------------------Quantum lookup of prior order
        string queryString = "SELECT ";

        DateTime NoDatesBefore = DateTime.Now.AddDays(0 - int.Parse(HxDaysAllowed));
        DateTime myFromDate;
        DateTime myToDate;

        if (IsDate(fromDate))
        {
            myFromDate = DateTime.Parse(fromDate);
            if (DateTime.Compare(myFromDate, NoDatesBefore) < 0)
            {
                myFromDate = NoDatesBefore;
            }
        }
        else
        {
            myFromDate = NoDatesBefore;
        }

        if (IsDate(toDate))
        {
            myToDate = DateTime.Parse(toDate);
            if (DateTime.Compare(myToDate, myFromDate) < 0)
            {
                myToDate = DateTime.Now;
            }
        }
        else
        {
            myToDate = DateTime.Now;
        }

        queryString = "SELECT  SO_HEADER.SOH_AUTO_KEY, SO_DETAIL.SOD_AUTO_KEY, SO_HEADER.TOTAL_PRICE, SO_HEADER.BILL_NAME, SO_HEADER.COMPANY_REF_NUMBER, PARTS_MASTER.PN, SO_HEADER.ENTRY_DATE, SO_DETAIL.QTY_ORDERED,";
        queryString += " INVC_DETAIL.UNIT_PRICE, SO_DETAIL.QTY_INVOICED, INVC_DETAIL.IND_AUTO_KEY, INVC_DETAIL.SHIP_DATE, INVC_DETAIL.QTY_SHIP, INVC_DETAIL.QTY_BACK_ORDER, ";
        queryString += " INVC_HEADER.INVC_NUMBER, INVC_HEADER.AIRWAY_BILL, INVC_HEADER.INH_AUTO_KEY, PARTS_MASTER.STC_AUTO_KEY ";
        queryString += " FROM QCTL.PARTS_MASTER PARTS_MASTER, QCTL.SO_DETAIL SO_DETAIL, QCTL.SO_HEADER SO_HEADER, QCTL.STOCK_RESERVATIONS STOCK_RESERVATIONS ";
        queryString += " LEFT OUTER JOIN QCTL.INVC_DETAIL INVC_DETAIL ON  STOCK_RESERVATIONS.IND_AUTO_KEY = INVC_DETAIL.IND_AUTO_KEY ";
        queryString += " LEFT OUTER JOIN QCTL.INVC_HEADER INVC_HEADER ON INVC_HEADER.INH_AUTO_KEY = INVC_DETAIL.INH_AUTO_KEY ";
        queryString += " WHERE SO_HEADER.SOH_AUTO_KEY = SO_DETAIL.SOH_AUTO_KEY ";
        queryString += " AND PARTS_MASTER.PNM_AUTO_KEY = SO_DETAIL.PNM_AUTO_KEY  ";
        queryString += " AND STOCK_RESERVATIONS.SOD_AUTO_KEY = SO_DETAIL.SOD_AUTO_KEY ";
        queryString += " AND (SO_HEADER.ENTRY_DATE >= to_date('" + string.Format("{0:MM/d/yyy}", myFromDate) + "', 'MM/DD/YY') AND SO_HEADER.ENTRY_DATE <= to_date('" + string.Format("{0:MM/d/yyy}", myToDate) + "', 'MM/DD/YYYY')) ";
        short shLp;
        bool firstplaced = false;

        if (CustRefNumber != String.Empty)
        {
            string[] tmp = CustRefNumber.Split(',');
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
                queryString += " LOWER(SO_HEADER.COMPANY_REF_NUMBER) like '%" + tmp[shLp].Trim().ToLower() + "%' OR LOWER(PARTS_MASTER.PN) like '%" + tmp[shLp].Trim().ToLower() + "%' ";
            }
            queryString += ") ";
        }
        queryString += " AND (SO_DETAIL.PCC_AUTO_KEY != 9) ";
        //------- this excludes the Borrowed returns

        //if (findTopX != string.Empty)
        if (fromDate.Trim() + toDate.Trim() + CustRefNumber.Trim() == string.Empty)
        {
            //queryString += " AND ROWNUM <= 20 ";
        }
        queryString += " AND SO_HEADER.CMP_AUTO_KEY = " + companyAutoKey + " ";
        queryString += " ORDER BY SO_HEADER.ENTRY_DATE DESC ";  
        return GenUtils.buildOracleTable(queryString);
    }


    public static DataTable BuildCompanyWebsiteUseHistory(int companyAutoKey)
    {
        DataTable dtTemp = null;

        DataSet dsws = new DataSet();
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);

        string queryString = "SELECT ";
        queryString += " hits.userKey, login.fkAccountKey, search.SearchWord, search.DateTime, login.pkLogin, hits.UserName, hits.CompanyName, hits.Logins, hits.Accumulator, login.AttemptDateTime from ecom_HitsCounter hits ";
        queryString += "  LEFT JOIN  ecom_loginQueue login ON  login.fkAccountKey = hits.UserKey ";
        queryString += " LEFT JOIN ecom_SearchQueue search ON search.fkUserKey = hits.UserKey ";
        queryString += " WHERE hits.companyKey = " + companyAutoKey + " ";
        queryString += " AND NOT EXISTS (SELECT AttemptDateTime from ecom_LoginQueue where fkAccountKey = hits.userKey AND pkLogin > login.pkLogin) ";
        queryString += " order by hits.UserName, search.DateTime DESC ";

        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;

        SqlDataAdapter globalDA = new SqlDataAdapter();
        globalDA.SelectCommand = selectCMD;

        try
        {
            sqlConn.Open();
            globalDA.Fill(dsws, "ws");
            dtTemp = dsws.Tables["ws"];
        }

        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            sqlConn.Close();
        }

        return dtTemp;
    }

    
    //********************************************** Database connectivity
    public static DataTable buildOracleTable(string queryString)
    {

        DataTable dtTemp = null;
        string oracleOLE = "OraOLEDB.Oracle";
        OleDbConnection oracleConn = new OleDbConnection();
        oracleConn.ConnectionString = "Provider=" + oracleOLE + "; User ID=crystal;Password=report;Data Source=173.10.78.33:1521/CCTL;OLEDB.NET=True";
        DataSet dsTemp = new DataSet();
        OleDbDataAdapter daTemp = new OleDbDataAdapter();
        daTemp.SelectCommand = new OleDbCommand(queryString, oracleConn);

        try
        {
 
            daTemp.Fill(dsTemp, "OraTable");
            if (dsTemp.Tables.Count > 0)
            {
                dtTemp = dsTemp.Tables["OraTable"];
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            daTemp.Dispose();
            dsTemp.Dispose();
            oracleConn.Dispose();
        }
        return dtTemp;
    }


    public static Decimal oracleGetCount(string queryString)
    {
        decimal decTemp = 0m;
        DataTable dtTemp = null;

        dtTemp = GenUtils.buildOracleTable(queryString);

        if (dtTemp != null)
        {
            if (dtTemp.Rows[0][0] != DBNull.Value)
            {
                if (GenUtils.IsDecimal(dtTemp.Rows[0][0].ToString()))
                {
                    decTemp = Convert.ToDecimal(dtTemp.Rows[0][0].ToString());
                }
            }
        }
        return decTemp;
    }
    // *********************************** Debugging Utilities
    public static void PrintTableOrView(DataTable dt, string label)
    {
        if (dt == null)
        {
            return;
        }
        string temp;
        int count = -1;
        foreach (DataRow myRow in dt.Rows)
        {
            temp = "DataTable " + count++ + "-->";
            foreach (DataColumn myCol in dt.Columns)
            {
                temp += label + "--" + myCol.ColumnName + " " + myRow[myCol] + "\r\n";
            }
            Debug.WriteLine(temp);
        }
    }

    public static void PrintTableOrView(DataView dv, string label)
    {
        if (dv == null)
        {
            return;
        }
        string temp;
        int count = -1;
        foreach (DataRow myRow in dv.Table.Rows)
        {
            temp = "DataView " + count++ + "-->";
            foreach (DataColumn myCol in dv.Table.Columns)
            {
                temp += myRow[myCol] + " ";
            }
            Debug.WriteLine(temp);
        }
    }

    public static void showForm()
    {
        HttpApplication ht = HttpContext.Current.ApplicationInstance;
        foreach (string name in ht.Request.Form)
        {
            Debug.WriteLine( name + " = " + ht.Request.Form[name]);
        }
    }

    public static void showQString()
    {
        HttpApplication ht = HttpContext.Current.ApplicationInstance;
        foreach (string name in ht.Request.QueryString)
        {
            Debug.WriteLine("---------- QString: " + name + " = " + ht.Request.QueryString[name]);
        }
    }

    public static bool IsDate(string sdate)
    {	
        DateTime dt;
        bool isDate = true;
        try {
            dt = DateTime.Parse(sdate);
        }	catch {
            isDate = false;
            }
        
        return isDate;
    }

    public static bool IsNumber(string iNumber)
    {
        int myint;
        bool isNumber = true;
        try {
            myint = int.Parse(iNumber);
        } catch {
            isNumber = false;
        }
        return isNumber;

    }

    public static decimal returnDecimalforString(string thisString)
    {
        decimal decTemp = 0m;
        if (thisString == null)
            return 0m;
        thisString = thisString.Replace("$", "");
        if (GenUtils.IsDecimal(thisString))
        {
            decTemp = Convert.ToDecimal(thisString);
        }
        return decTemp;
    }

    public static bool IsDecimal(string iNumber)
    {
        decimal mydecimal;
        bool isDecimal = true;
        try
        {
            mydecimal = decimal.Parse(iNumber);
        }
        catch
        {
            isDecimal = false;
        }
        return isDecimal;
    }


    public static string CharacterstoHTML(string thisValue)
    {
        string temp = thisValue.Replace("'", "&#39;");
        temp = temp.Replace("\"", "&#34;");
        temp = temp.Replace("*", "&#42;");
        temp = temp.Replace("\\", "&#92;");
        return temp;
    }

    public static string HTMLtoCharacters(string thisValue)
    {
        string temp = thisValue.Replace("'", "&#39;");
        temp = temp.Replace("\"", "&#34;");
        temp = temp.Replace("#", "&#35;");
        temp = temp.Replace("*", "&#42;");
        temp = temp.Replace("\\", "&#92;");
        return temp;    
    }
}


public class ArrayComparer : System.Collections.IComparer
{
    int ix;
    public ArrayComparer(int SortFieldIndex)
    {
        ix = SortFieldIndex;
    }

    public int Compare(object x, object y)
    {
        IComparable cx = (IComparable)((Array)x).GetValue(ix);
        IComparable cy = (IComparable)((Array)y).GetValue(ix);
        return cx.CompareTo(cy);
    }
}