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
using System.Threading;

using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

using System.Drawing;
using System.Drawing.Imaging;

//http://jetserve1.jetpartsengineering.com/

//\\jetserve1\hostedwebsite




public partial class ecom_prodSearch : System.Web.UI.Page
{

    //********************************* Global Variables
    bool blnshowDebuggerOutput;
    public int intPlacedPartNumberCounter;
    string MySessionId;
    public string strFileRootPath;
    public string strSalesPictureRootPath;
    public int seed;

    public Label lblSmartBuyerPanel;

    string strOurIPs = string.Empty;
    string singleNHAMatch;
    string singleNHAMatchtoPrint;
    int CacheCounter;
    int AddPartsCounter;
    string strSearchList;

    int intNHAPopUpMatchesFound;
    Boolean blnNHADetailPopupSearch;

    Boolean blnLogLogin;
    Boolean blnUserCookiesRequired;
    Boolean blnSmartStockFound;

    Boolean blnEmailSearchWords;
       
    DataTable dtGlobalAdmin;
    DataTable dtWebsiteDown;
    Boolean blnOnDevServer;
    Boolean blnOnProductionServer;
    Boolean PartsPopupUpdate;
    Boolean closePartsPopup;
    Boolean closeAccountHistoryPopup;
    Boolean blnShortSearchWord;
    Boolean blnSuggestion;

    Boolean blncheckAvlScrape;
    Boolean blnAvlButtonclick;
    Boolean blnAddToCart;

    Boolean blnShowDevelopmentBlueCode;
    public Boolean blnRequireSearchQty;
    Boolean blnThisisNewSearch;

    DataTable dtAccounts;
    DataTable dtCompany;
    DataTable dtSites;
    DataTable dtShipVia;
    DataTable dtRolodex;
    DataTable dtOrderHistory;
    DataTable dtRFQHistory;
    DataTable dtWebsiteOrderHistory;
    DataTable dtCompanyUsageStats;
    DataTable dtAdminUsers;

    Boolean blnExcelLimitExceeded;

    public string[,] searchValues;

    DataTable dtCacheMaster;
    public string[,] cacheDefine;
    int cacheColumnCount;
    //string[,] clientSearchList;


    string[,] SPBmasterSEARCHList;
    string[,] SPBmasterSCRAPEList;
    string[,] SPBmasterUNIFIEDList;
    int SPBmasterSEARCHCnt;
    int SPBmasterSCRAPECnt;
    int SPBmasterUNIFIEDCnt;
    int showDidYouMeanDepth;

    int OrderHistoryRowCounter;
    int AdminUsersPanelCounter;

    public int cartTlQty;
    public decimal cartTlAmount;

    bool blnUserLoaded;
    bool blnPartsCacheLoaded;
    bool blnGlobalLoaded;

    int useGlobalLine;
    bool blnItemsOnPage;
    bool blnItemsInCart;

    public int intlimitExcelParts;

    public string strReportsPath;

    //string QuantumConnectType;
    
    //"C:\Program Files\Internet Explorer\IEXPLORE.EXE" http*//www.we_site.com/datafeed/sffdatafeed.asp?ID=41



    //than one line. But I haven't had time to play with it.


    //****************************************** Page Load

    protected void Page_Load(object sender, EventArgs e)
    {

       



        blnshowDebuggerOutput = false;
        
        if (blnshowDebuggerOutput)
            GenUtils.showForm();

        // me 127.0.0.1
        // JPE 192.168.10.1 

        strOurIPs = "127.0.0.1|192.168.10.1|";

        //--------------------- old email Roles
        //approvalEmailTo
        //changeEmailTo
        //newAccountEmailFrom
        //defaultEmailTo
        //defaultEmailFrom
        //recoverPWFrom
        //InternalEmailFrom

        //---------------------- New Email Roles in GlobalAdmin
        //emailFrom_Default
        //emailTo_Orders
        //emailFrom_Orders
        //emailTo_RFQs
        //emailFrom_RFQs
        //emailTo_NewUsers
        //emailFrom_NewUsers
        //emailTo_UserChanges
        //emailFrom_UserChanges
        //emailFrom_RecoverPassword
        //emailFrom_VerificaitonLink
        //emailFrom_IPLink



        if (blnshowDebuggerOutput)
            GenUtils.showQString();

        MySessionId = Session.SessionID;
        Page.Session["MySessionId"] = MySessionId;

        Page.Session["MMenu"] = "products";
        Page.Session["SMenu"] = "smartbuyer";


        //  NEWWS      iddenField hdnSessionID = (HiddenField)FindControl("hdn_SessionID");
        //hdnSessionID.Value = MySessionId;
        hdn_SessionID.Value = MySessionId;

        if (Page.Session["AccountType"] != null)
        {
            hdn_MyAccountType.Value = Page.Session["AccountType"].ToString();
        }


        blnEmailSearchWords = true;
        blnShowDevelopmentBlueCode = false;
        blnRequireSearchQty = false;
        blnUserCookiesRequired = true;

        showDidYouMeanDepth = 5;

        //lblSteveDebug.Text = string.Empty;

        searchValues = new string[7, 2];
   
        //------------------ set the server dependent variables
        string thisPathTranslated = Request.ServerVariables["PATH_TRANSLATED"].ToLower();

        blnOnDevServer = false;
        blnOnProductionServer = false;
        if (thisPathTranslated.Substring(0, 10) == "c:\\inetpub")
        {
            blnOnProductionServer = true;
        }
        if (thisPathTranslated.Substring(0, 12) == "c:\\documents")
        {
            blnOnDevServer = true;
            blnOnProductionServer = false;
        }
        else if (thisPathTranslated.Substring(0, 20) == "d:\\hostedwebsitetest")
        {
            blnOnProductionServer = false;

        }


        Page.Session["blnOnDevServer"] = blnOnDevServer.ToString();
        Page.Session["blnOnProductionServer"] = blnOnProductionServer.ToString();


        useGlobalLine = 1;
        Page.Session["blnOnDevServer"] = blnOnDevServer;
       
        
        //strFileRootPath =  "C:\\Inetpub\\www";
        //strReportsPath = "http://www.jpesmartbuyer.com/reports/";

        strFileRootPath = "c:\\inetpub\\wwwroot";
        strSalesPictureRootPath = "c:\\inetpub\\wwwroot";
        blnItemsInCart = false;

        strReportsPath = "http://jetpartsengineering.com/reports/";

        if (blnOnProductionServer)
        {
            strFileRootPath = "D:\\HostedWebsite";
        }

        else if (blnOnDevServer)
        {
            strFileRootPath = "c:\\Documents and Settings\\user\\My Documents\\Visual Studio 2008\\Websites\\JetParts";
            strReportsPath = "http://localhost/JetParts/reports/";
            useGlobalLine = 2;
        }

        Page.Session["strFileRootPath"] = strFileRootPath;
        Page.Session["strSalesPictureRootPath"] = strSalesPictureRootPath;


      //------------------------- load the global variables if not loaded
        if (!blnGlobalLoaded)
        {
           dtGlobalAdmin =  GenUtils.loadGlobalAdmin(useGlobalLine);
           dtWebsiteDown = GenUtils.loadGlobalAdmin(0);

          blnGlobalLoaded = true;
        }


        if (Page.Session["LoggedInUserID"] == null || int.Parse(Page.Session["LoggedInUserID"].ToString()) == -1)
        {
            if (IsPostBack)
                SetSessionOnLogout();

            Page.Session["PanelMode"] = "login";
        }

        // -------------------- Handle post back form commands
        if (IsPostBack)
        {
             switch (Request.Form["cmd"])
            {
                case "LostPassword":
                    {
                        break;
                    }
            }
        }


        //--------------------------- Handle QueryString Post Back requests

        if (Request.QueryString["v1"] != null)
        {
            if (Request.QueryString["v1"].ToString().Length > 10)
            {
                verifyEmail(Request.QueryString["v1"].ToString(), "emailVer");
            }
        }
        else if (Request.QueryString["v2"] != null)
        {
            if (Request.QueryString["v2"].ToString().Length > 10)
            {
                verifyEmail(Request.QueryString["v2"].ToString(), "CookieSet");
            }
        }

        else if (Request.QueryString["a"] != null)
        {
            PublishSalesReport(Request.QueryString["a"].ToString().ToLower(), sender, e);
        }

        //------------------- Limit the number of Excel Drop in Parts
        intlimitExcelParts = 25;


        //-------- Debugger stuff
        if (blnshowDebuggerOutput)
            VBCLass1.showSessionVariables();


        //-- insert the client side javascript event handlers
        btnHistorySearch.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnHxSearch_HDN');");
        btnPartsSearch.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnPartsSearch');");

        btnBuildSRPReport.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnBuildSRPReport');");
        btnLogin.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnLogin');");
        btnLogOut.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnLogOut');");
        btnUpdatePartsOnQty.Attributes.Add("onclick", "javascript: return qtyUpdate('btnShowThrobber', 'btnUpdatePartsOnQty');");
        btnPartsSearchfromCart2.Attributes.Add("onclick", "javascript: return qtyUpdate('btnShowThrobber', 'btnPartsSearchfromCart2');");
        txtLoginUserName.Attributes.Add("onkeyup", "javascript: return focusOnEnter(event, 'txtLoginPassword');");
        txtLoginUserName.Attributes.Add("onkeypress", "javascript: return capLock(event);");
        txtLoginPassword.Attributes.Add("onkeyup", "javascript: return submitOnEnter(event, 'btnLogin');");
        txtLoginPassword.Attributes.Add("onkeypress", "javascript: return capLock(event);");
        txtForgotEmail.Attributes.Add("onkeyup", "javascript: return submitOnEnter(event, 'btnRecoverPassword');");
        btnRecoverPassword.Attributes.Add("onclick", "javascript: return ReplaceThrobber('behaveThrobber', 'btnRecoverPassword', 'behaveForgotten');");
        txtCardNumber.Attributes.Add("onkeypress", "javascript: return checkIt(event);");
        txtExpireMonth.Attributes.Add("onkeypress", "javascript: return checkIt(event);");
        txtExpireYear.Attributes.Add("onkeypress", "javascript: return checkIt(event);");
        txtCVVCode.Attributes.Add("onkeypress", "javascript: return checkIt(event);");
        btnPlaceOrder.Attributes.Add("onclick", "javascript: return ChoosePostBack();");
        btnAcctCancel2.Attributes.Add("onclick", "javascript: return doButtonClick('btnAcctCancel1');");


        //TextBox myTB = (TextBox)FindControl("test1_48023");
        //myTB.Attributes.Add("onblur", "javascript: return alignQuantities('');");





       // test1_83923.Attributes.Add("onblur", "javascript: return doButtonClick('btnAcctCancel1');");


        //   NEWWS  Button myHintsChange = (Button)FindControl("btnChangeUseHints");
        //myHintsChange.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnChangeUseHints');");
        btnChangeUseHints.Attributes.Add("onclick", "javascript: return showThrobber('btnShowThrobber', 'btnChangeUseHints');");

       cbUsesBillingAddress.Attributes.Add("onclick", "javascript: changeCardBillingAddress();");

        //  IDMissingLinks();
        //-----ftpfile("/temp/ftptest.gif", "C:/temp/ftptest.gif");
        //-----ListControlCollections();

        //string strTest = GenUtils.MakeUniformPartNumber("32384701");
        //string strWhereString;
        //string[] sortArray = new string[1000];
        //int cnt = -1;
        //string thisCompare = string.Empty;
        ////string thisLookup = string.Empty;
        //string trimmedPartNumber = string.Empty;
        ////---speed up the 3's
        //string leadLookup = strTest.Substring(0, 1) == "3" ? strTest.Substring(0, 2) : strTest.Substring(0, 1);
        //if (strTest.Length > 1000)
        //{
        //    strWhereString = " WHERE p.PN_STRIPPED Like'" + GenUtils.MakeUCaseNumChar(leadLookup) + "%'";
        //    //--- OK No Match so load the parts that start with 
        //    DataTable lowTemp = GenUtils.FindParts(strWhereString, string.Empty);

        //    foreach (DataRow myRow in lowTemp.Rows)
        //    {
        //        trimmedPartNumber = myRow["PN_STRIPPED"].ToString().Length > strTest.Length ? myRow["PN_STRIPPED"].ToString().Substring(0,strTest.Length)  : myRow["PN_STRIPPED"].ToString();

        //        thisCompare = GenUtils.LevenshteinDistance(trimmedPartNumber, strTest).ToString();
        //        while (thisCompare.Length < 3)
        //        {
        //            thisCompare = "0" + thisCompare; 
        //        }
        //        sortArray[++cnt] = thisCompare +  "-" + myRow["PN"].ToString();
        //        Debug.WriteLine(sortArray[cnt]);
        //    }

        //    int x;
        //    System.Array.Sort(sortArray);
        //    for (x = 1000 - cnt; x < 1000; x++)
        //    {
        //        Debug.WriteLine(sortArray[x]);
        //    }

        //}
    }

    protected void IncrementHitCounter()
    {

        int userKey = -1;
        int companyKey = -1;
        string companyName = string.Empty;
        string userName = string.Empty;
        string userType = string.Empty;
        Boolean blnLogin = false;

        WriteHits(-1, "", -1, "","GLOBAL", false);

        if (Page.Session["LoggedInUserID"] != null)
        {
            if ((int)Page.Session["LoggedInUserID"] > -1)
            {
                userKey = (int)Page.Session["LoggedInUserID"];
                userName = Page.Session["UserName"].ToString();
                userType = Page.Session["AccountType"].ToString();

                if (blnLogLogin)
                    {
                        blnLogin = true;
                    }
               
                if (Page.Session["Quantum_CMP_AUTO_KEY"] != null)
                {
                    if ((int)Page.Session["Quantum_CMP_AUTO_KEY"] > -1)
                    {
                        companyName = Page.Session["Company"].ToString();
                        companyKey = (int)Page.Session["Quantum_CMP_AUTO_KEY"];
                    }
                }
                WriteHits(userKey, userName, companyKey, companyName, userType, blnLogin);
            }
        }
    }

    protected void WriteHits(int userKey, string userName, int companyKey, string companyName, string userType, Boolean blnLogin)
    {
        string queryString = string.Empty;

        string conn = string.Empty;

        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        SqlConnection cacheQueueConn = new SqlConnection(conn);
        //SqlDataReader veReader;
        int intRecCount = -1;
        SqlCommand cmd;
        
        if (userType == "GLOBAL")
        {
            queryString = "UPDATE dbo.ecom_HitsCounter SET Accumulator = Accumulator + 1, LastDateTime = '" + DateTime.Now + "' where pkHitsKey = 1;";
        }
        else
        {
            //------- see if the key and type exist
            queryString = "Select Count(*) from dbo.ecom_HitsCounter WHERE userKey = " + userKey + ";";
            cmd = new SqlCommand(queryString, cacheQueueConn);
            try
            {
                cacheQueueConn.Open();
                intRecCount = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error HITS Counter scalar read attempt: " + ex.ToString());
            }
            finally
            {
                cacheQueueConn.Close();
            }

            if (intRecCount > 0)
            {
                queryString = "UPDATE dbo.ecom_HitsCounter SET Accumulator = Accumulator + 1, LastDateTime = '" + DateTime.Now + "' ";

                if (blnLogin)
                {
                    queryString += ", Logins = Logins + 1 ";
                }
                queryString += "  WHERE userKey = " + userKey + ";";
            }
            else
            {
                queryString = "INSERT INTO dbo.ecom_HitsCounter (userKey, companyKey, userName, companyName, userType, Accumulator, LastDateTime, Logins)  VALUES (" + userKey + ", " + companyKey + ", '" + userName + "', '" + companyName + "', '" + userType + "', 1, '" + DateTime.Now + "', 1);";
            }
        }
        cmd = new SqlCommand(queryString, cacheQueueConn);
        try
        {
            cacheQueueConn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error HITS Counter attempt: " + ex.ToString());
        }
        finally
        {
            cacheQueueConn.Close();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Page.Session["PanelMode"] != null)
        {
            SetPanels(Page.Session["PanelMode"].ToString());
        }
        //------------------------------------------- Increment the hit counter
        IncrementHitCounter();
    }

    private void PublishSalesReport(string qstring, object sender, EventArgs e)
    {

        HyperLink hl = null;
        mktg_Reporting myReport = new mktg_Reporting();
        //hlshowSRPReport.Text = "page load";
        if (qstring == "asr")
        {
            hl = myReport.distributeReport("distributionList", string.Empty, string.Empty, string.Empty, dtGlobalAdmin.Rows[0]["internalEmailFrom"].ToString(), string.Empty, string.Empty, strFileRootPath, strReportsPath, sender, e, string.Empty, string.Empty, false, string.Empty, string.Empty, blnOnDevServer);
        }
        else if (qstring == "test")
        {
            hl = myReport.distributeReport("distributionList", string.Empty, string.Empty, string.Empty, dtGlobalAdmin.Rows[0]["internalEmailFrom"].ToString(), "TEST", string.Empty, strFileRootPath, strReportsPath, sender, e, string.Empty, string.Empty, false, string.Empty, string.Empty, blnOnDevServer);

        }
    }

    private void ListControlCollections()
    {
        ArrayList controlList = new ArrayList();
        AddControls(Page.Controls, controlList); 
        foreach (string str in controlList) 
            { 
            Debug.WriteLine(str + "\n\n"); 
            } 
        Debug.WriteLine("Total Controls:" + controlList.Count); 
    }    
    
    private void AddControls(ControlCollection page, ArrayList controlList)
    {
        foreach (Control c in page) 
        { 
            if (c.ID != null) 
            { 
                controlList.Add(c.ID + " container: " +  c.NamingContainer.ToString() +  " page: " + c.Page.ToString() + " Parent: " + c.Parent.ToString() + " Unique ID: " + c.UniqueID + " " ); 
            } 
            if (c.HasControls()) 
            { 
                AddControls(c.Controls, controlList); 
            } 
        } 
    }

    protected void IDMissingLinks()
{
    string conn = string.Empty;
    conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
    SqlConnection sqlConn = new SqlConnection(conn);

    System.Data.DataTable dtTemp = null;
    string queryString = "Select ID, PN from dbo.JPEPN";
    //queryString += " WHERE PN = '3181634JP-1' OR PN = '3233531JP-1' OR PN = '5863224JP-101' OR PN = 'APM3864070-503'  ";
    queryString += " ORDER BY PN; ";

    SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
    selectCMD.CommandTimeout = 30;
    SqlDataAdapter nhaDA = new SqlDataAdapter();
    nhaDA.SelectCommand = selectCMD;
    DataSet mynhaDS = new DataSet();

    try
    {
        sqlConn.Open();
        nhaDA.Fill(mynhaDS, "JPN");
        dtTemp = mynhaDS.Tables["JPN"];
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
    int ORAKey =0;
    string description = string.Empty;
    int ifc_auto_key = 0;
    int stc_auto_key = 0;
    int AlternateKey1 = 0;
    int AlternateKey2 = 0;
    string Alt1PN = string.Empty;
    string Alt2PN = string.Empty;
    int Alt1PNAltBackKey1 = 0;
    int ALt2PNAltBackKey2 = 0;
    string  Alt1BackPN = string.Empty;
    string Alt2BackPN = string.Empty;
    int intRows = 0;
    Decimal ListPrice = 0m;
    Decimal ExchangePrice = 0m;
    DataTable dtOraclePartsMaster = null;
    DataTable dtAlternates = null;
    string strWhereString = string.Empty;
    int JPNCounter = -1;

    foreach (DataRow myRow in dtTemp.Rows)
    {
        JPNCounter++;
        ORAKey = 0;
        description = string.Empty;
        AlternateKey1 = 0;
        AlternateKey2 = 0;
        ifc_auto_key = 0;
        stc_auto_key = 0;
        Alt1PN = string.Empty;
        Alt2PN = string.Empty;
        Alt1PNAltBackKey1 = 0;
        ALt2PNAltBackKey2 = 0;
        Alt1BackPN = string.Empty;
        Alt2BackPN = string.Empty;
        ListPrice = 0m;
        ExchangePrice = 0m;
        intRows = 0;
        strWhereString = " WHERE p.PN_STRIPPED = '" + GenUtils.MakeUCaseNumChar(myRow["PN"].ToString()) + "'";
        dtOraclePartsMaster = GenUtils.FindParts(strWhereString, string.Empty);

        if (dtOraclePartsMaster != null)
        {
            //---- get the PMA Part Number Key
            dtOraclePartsMaster = getOraPart(GenUtils.MakeUCaseNumChar(myRow["PN"].ToString()), -1);
            intRows = dtOraclePartsMaster.Rows.Count;
            ORAKey = int.Parse(dtOraclePartsMaster.Rows[0]["PNM_AUTO_KEY"].ToString());
            description = dtOraclePartsMaster.Rows[0]["DESCRIPTION"].ToString().Replace("'", "");
            ifc_auto_key = int.Parse(dtOraclePartsMaster.Rows[0]["IFC_AUTO_KEY"].ToString());
            stc_auto_key = int.Parse(dtOraclePartsMaster.Rows[0]["STC_AUTO_KEY"].ToString());

            if (dtOraclePartsMaster.Rows[0]["LIST_PRICE"] != null)
            {
                if (GenUtils.IsDecimal(dtOraclePartsMaster.Rows[0]["LIST_PRICE"].ToString()))
                {
                    ListPrice = decimal.Parse(dtOraclePartsMaster.Rows[0]["LIST_PRICE"].ToString());
                }
            }
            if (dtOraclePartsMaster.Rows[0]["EXCH_LIST_PRICE"] != null)
            {
                if (GenUtils.IsDecimal(dtOraclePartsMaster.Rows[0]["EXCH_LIST_PRICE"].ToString()))
                {
                    ExchangePrice = decimal.Parse(dtOraclePartsMaster.Rows[0]["EXCH_LIST_PRICE"].ToString());
                }
            }

            //------- Get a list of the Alternates
            dtAlternates = getAlternates(ORAKey);
            if (dtAlternates != null)
            {
                if (dtAlternates.Rows.Count > 0)
                {
                    AlternateKey1 = int.Parse(dtAlternates.Rows[0]["ALT_PNM_AUTO_KEY"].ToString());
                    if (dtAlternates.Rows.Count > 1)
                    {
                        AlternateKey2 = int.Parse(dtAlternates.Rows[1]["ALT_PNM_AUTO_KEY"].ToString());
                    }
                }


                //------- Get the alternate Record(s)
                if (AlternateKey1 > 0)
                {
                    dtOraclePartsMaster = getOraPart("", AlternateKey1);
                    Alt1PN = dtOraclePartsMaster.Rows[0]["PN"].ToString();

                    dtAlternates = getAlternates(AlternateKey1);
                    if (dtAlternates != null)
                    {
                        if (dtAlternates.Rows.Count > 0)
                        {
                            Alt1PNAltBackKey1 = int.Parse(dtAlternates.Rows[0]["ALT_PNM_AUTO_KEY"].ToString());
                            if (dtAlternates.Rows.Count > 1)
                            {
                                ALt2PNAltBackKey2 = int.Parse(dtAlternates.Rows[1]["ALT_PNM_AUTO_KEY"].ToString());
                            }
                        }

                        if (Alt1PNAltBackKey1 > 0)
                        {
                            dtOraclePartsMaster = getOraPart("", Alt1PNAltBackKey1);
                            Alt1BackPN = dtOraclePartsMaster.Rows[0]["PN"].ToString();
                        }
                        if (ALt2PNAltBackKey2 > 0)
                        {
                            dtOraclePartsMaster = getOraPart("", ALt2PNAltBackKey2);
                            Alt2BackPN = dtOraclePartsMaster.Rows[0]["PN"].ToString();
                        }
                    }
                }
                if (AlternateKey2 > 0)
                {
                    dtOraclePartsMaster = getOraPart("", AlternateKey2);
                    Alt2PN = dtOraclePartsMaster.Rows[0]["PN"].ToString();
                }
            }

            //--------------write the data to the SQL Table
            queryString = "Update dbo.JPEPN  SET ORAKey =" + ORAKey + ", Description='" + description + "',  IFC_AUTO_KEY='" + ifc_auto_key + "', STC_AUTO_KEY='" + stc_auto_key + "', AlternateKey1=" + AlternateKey1 + ", AlternateKey2=" + AlternateKey2 + ", Alt1PN='" + Alt1PN + "', Alt2PN='" + Alt2PN + "', Alt1PNAltBackKey1=" + Alt1PNAltBackKey1 + ", ALt2PNAltBackKey2=" + ALt2PNAltBackKey2 + ", Alt1BackPN='" + Alt1BackPN + "', Alt2BackPN='" + Alt2BackPN + "', ListPrice = " + ListPrice + ", ExchangePrice = " + ExchangePrice + ", rows = " + intRows + " WHERE ID = " + myRow["ID"].ToString() + ";";

            SqlCommand cmd = new SqlCommand(queryString, sqlConn);
            try
            {
                sqlConn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }
}

    protected DataTable getOraPart(string partNumber, int partKey)
    {
        string queryString = string.Empty;
        if (partKey > -1)
        {
              queryString = " SELECT PN, PNM_AUTO_KEY, LIST_PRICE, EXCH_LIST_PRICE, IFC_AUTO_KEY, STC_AUTO_KEY, DESCRIPTION from parts_master where PNM_AUTO_KEY = " + partKey + " ";
        }
        else
        {
            queryString = " SELECT PN, PNM_AUTO_KEY, LIST_PRICE, EXCH_LIST_PRICE, IFC_AUTO_KEY, STC_AUTO_KEY, DESCRIPTION from parts_master where PN_STRIPPED = '" + GenUtils.MakeUCaseNumChar(partNumber) + "' ";
        }

        return GenUtils.buildOracleTable(queryString);
      
    }

    protected DataTable getAlternates(int thisPartKey)
    {
        string queryString = " SELECT ALT_PNM_AUTO_KEY from ALTERNATES_PARTS_MASTER where PNM_AUTO_KEY = " + thisPartKey + " ";
        return GenUtils.buildOracleTable(queryString);
    }

    // **************************** Global Navigation

    protected void SetPanels(string mode)
    {
        if (closeAccountHistoryPopup)
        {
            ModalPopupExtenderAccountHistory.Hide();
            return;
        }
        ModalPopupExtenderAccountHistory.Y = 20;
        ModalPopupExtenderOrderReceived.Hide();
        if (!blnSuggestion)
        {
            lblSuggestionMessage.Text = string.Empty;
        }
        lblSteveDebug.Text = string.Empty;
        //---------Page.Session["PanelMode"] = "login", "search", "checkout";

        if (blnShowDevelopmentBlueCode)
            BuildstrDebug(-1); //------- show the company info

        pnlJPECompanyDDL.Visible = false;
        pnlSmartStock.Visible = false;
        pnlRememberMe.Visible = true;
        cbRememberMe.Checked = false;
        pnlLogin.Visible = false;
        pnlEmtpyCartMessage.Visible = false;
        pnlAdminControls.Visible = false;
        pnlCheckoutCartApproved.Visible = false;
        pnlSearchVideoGame.Visible = false;
        pnlCanWeHelpYou.Visible = false;
        pnlSearchForParts.Visible = false;
        lblsearchTitle.Text = "Search for Parts";
        pnlShippingandPayment.Visible = false;
        pnlCreditCardInfo.Visible = false;
        //  NEWWS   Panel mySearchMorePanel = (Panel)FindControl("pnlSearchMore");
        //mySearchMorePanel.Visible = false;
        pnlSearchMore.Visible = false;
        pnlShippingInstructions.Visible = false;

         //----------------------------- check for website down
        pnlWebsiteDown.Visible = false;
        int intSiteDownStatus = 0;  //---- set to 1 for coming site down, 2 to site down
        DataTable dtsiteDown = GenUtils.loadWebsiteDown(0);
        DateTime fromTime = (DateTime)dtsiteDown.Rows[0]["fromTime"];
        DateTime toTime = (DateTime)dtsiteDown.Rows[0]["toTime"];
        int cmpFrom = DateTime.Now.CompareTo( fromTime);
        int cmpTo = DateTime.Now.CompareTo( toTime);
        if (cmpFrom == -1 && cmpTo == -1)
        {
            intSiteDownStatus = 1; // will be down
            lclWebsiteDown.Text = dtsiteDown.Rows[0]["messageBefore"].ToString();
        }
        else if (cmpFrom == 1 && cmpTo == -1)
        {
            intSiteDownStatus = 2; //---- down now
            lclWebsiteDown.Text = dtsiteDown.Rows[0]["messageDuring"].ToString();
        }

        if (intSiteDownStatus > 0)
        {
            pnlWebsiteDown.Visible = true;
            lclWebsiteDown.Text = lclWebsiteDown.Text.Replace("@toTime", dtsiteDown.Rows[0]["toTime"].ToString());
            lclWebsiteDown.Text = lclWebsiteDown.Text.Replace("@fromTime", dtsiteDown.Rows[0]["fromTime"].ToString());

            if (intSiteDownStatus == 2)
            {
                if (closePartsPopup)
                {
                    ModalPopupExtenderPartsDetail.Hide();
                }
                upPnlSmartPartsBuyer.Update();
                upHeader.Update();
                ModalPopupExtenderThrobber.Hide();
                SetSessionOnLogout();
                upHeader.Visible = false;
                return;
            }
        }
        string thisShipToAddressSel = string.Empty;

        if (Request.Form["ddlMyShipToAddress"] != null)
        {
            thisShipToAddressSel = Request.Form["ddlMyShipToAddress"].ToString();
        }

        string strThisMyShipper = string.Empty;
        if (Request.Form["ddlMyShipperID"] != null)
        {
            strThisMyShipper = Request.Form["ddlMyShipperID"].ToString();
        }
        DataTable dtAccounts;

        lblOrderPlaced.Text = string.Empty;
        //---- look me up and log 
        if (Page.Session["LoggedInUserID"] != null)
        {
            if ((int)Page.Session["LoggedInUserID"] > -1)
            {
                dtAccounts = GenUtils.ecom_AccountLookup("", Convert.ToInt32(Page.Session["LoggedInUserID"].ToString()));
                switch (dtAccounts.Rows[0]["useAutoCheck"].ToString())
                {
                    case "True":
                        Page.Session["useAutoCheck"] = " checked ";
                        break;
                    default:
                        Page.Session["useAutoCheck"] = string.Empty;
                        break;
                }
                switch (dtAccounts.Rows[0]["useHints"].ToString())
                {
                    case "True":
                        Page.Session["useHints"] = " checked ";
                        break;
                    default:
                        Page.Session["useHints"] = string.Empty;
                        break;
                }
                LoadUserVariables(thisShipToAddressSel, strThisMyShipper);
                switch (mode)
                {
                    case "clearCache":
                        mode = "search";
                        Page.Session["PanelMode"] = "search";
                        cartTlQty = 0;
                        cartTlAmount = 0;
                        lblCheckoutCart.Text = BuildCheckoutCart("search", false, false);
                        phSmartBuyerPanel.Controls.Clear();
                        break;
                    case "OrderPlaced":
                        mode = "search";
                        Page.Session["PanelMode"] = "search";
                        cartTlQty = 0;
                        cartTlAmount = 0;
                        phSmartBuyerPanel.Controls.Clear();
                        lblCheckoutCart.Text = BuildCheckoutCart("search", false, false);
                        ModalPopupExtenderOrderReceived.Show();
                        break;
                    case "login":
                        Page.Session["PanelMode"] = "search";
                        lblCheckoutCart.Text = BuildCheckoutCart("search", false, false);
                        mode = "search";
                       
                        break;
                    case "searchClick":
                        Page.Session["LastButton"] = "SearchButton";
                        mode = "search";
                        Page.Session["PanelMode"] = "search";
                        PartsCacheManager("Update", null);
                        BuildTheDisplay();
                        Page.Session["LastButton"] = string.Empty;
                        lblCheckoutCart.Text = BuildCheckoutCart("search", blnItemsOnPage, blnItemsInCart);
                        break;
                    case "search":
                        PartsCacheManager("Update", null);
                        BuildTheDisplay();
                        Page.Session["PanelMode"] = "search";
                        lblCheckoutCart.Text = BuildCheckoutCart("search", blnItemsOnPage, blnItemsInCart);
                        break;
                    case "search-noUpdate":
                        mode = "search";
                        Page.Session["PanelMode"] = "search";
                        break;
                    case "checkout":
                        PartsCacheManager("Update", null);
                        BuildTheDisplay();
                        lblCheckoutCart.Text = BuildCheckoutCart("checkout", blnItemsOnPage, blnItemsInCart);
                        break;
                }
            }
        }
        else
        {
            SetSessionOnLogout();
            mode = "login";
        }

        hdnpkAccountKey.Value = Page.Session["pkAccountKey"].ToString();

        //   NEWWS   HiddenField myUseHints = (HiddenField)FindControl("hdnUseHints");
        //myUseHints.Value = Page.Session["useHints"].ToString();
        hdnUseHints.Value = Page.Session["useHints"].ToString();

        Page.Session["LastPanelMode"] = Page.Session["PanelMode"].ToString();
        mpOrderReceived.BackImageUrl = "images/ordreceived.png";

        if (!(bool)Page.Session["JPEapprovedForPricing"])
        {
        mpOrderReceived.BackImageUrl = "images/rfqreceived.png";
        }
        upOrderThankYou.Update();

        Page.Session["blnAllowNetTerms"] = true;

        switch (mode)
        {
           
            case "login":
                pnlLogin.Visible = true;
                //------ show the remember me button
                HttpCookie myAccessCookie = Request.Cookies["JPEPreferences"];
                txtLoginUserName.Focus();
                if (myAccessCookie != null)
                {
                    if (myAccessCookie["JPEUserName"] != null)
                    {
                        txtLoginUserName.Text = myAccessCookie["JPEUserName"].ToString();
                        if (txtLoginUserName.Text != string.Empty)
                        {
                            cbRememberMe.Checked = true;
                            txtLoginPassword.Focus();
                        }
                    }
                }
                break;
            case "search":
                pnlSearchVideoGame.Visible = true;
                pnlSearchForParts.Visible = true;
                pnlCheckoutCartApproved.Visible = true;
                if (lblSmartBuyerPanel != null)
                {
                    if (lblSmartBuyerPanel.Text == string.Empty && Page.Session["useHints"].ToString() == " checked ")
                    {
                        pnlEmtpyCartMessage.Visible = true;
                    }
                    else
                    {
                        pnlSearchMore.Visible = true;
                        //----- NEWWS
                    }
                }
  
                if (blnSmartStockFound)
                {
                    pnlSmartStock.Visible = true;
                }

                pnlCanWeHelpYou.Visible = true;
                lblsearchTitle.Text = "Search for Parts";
                lblModalHelp.Text = buildHelpPanel("modalHelp");
                break;
            case "checkout":
                switch (Page.Session["TMC_AUTO_KEY"].ToString())
                {
                    case "11":  // TBD
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "16":  // Credit
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "5":  // Credit Card Order
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "10":  // Net
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "15":  // Net Cash
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "30":  // Pay Upon Receipt
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "4":  // PrePaid
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "12":  // Prepay only
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "18":  // Cash
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                    case "38":  // Cash in Advance
                        pnlCreditCardInfo.Visible = true;
                        Page.Session["blnAllowNetTerms"] = false;
                        break;
                }

                string thisShipperCarrier = string.Empty;
                if (Request.Form["ddlShipperCarrier"] != null)
                {
                    thisShipperCarrier = Request.Form["ddlShipperCarrier"].ToString();
                }
                SetshippingCarrierDDL(thisShipperCarrier);

                string thisCardType = string.Empty;
                if (Request.Form["ddlCardType"] != null)
                {
                    thisCardType = Request.Form["ddlCardType"].ToString();
                }
                SetMyCardTypeDDL(thisCardType);
                lblAccountError1.Text = string.Empty;
                pnlCheckoutCartApproved.Visible = true;
                pnlCanWeHelpYou.Visible = true;
                pnlShippingandPayment.Visible = true;
                lblModalHelp.Text = buildHelpPanel("modalHelp");
                if ((bool)Page.Session["JPEapprovedForPricing"])
                {
                    pnlShippingInstructions.Visible = true;
                    btnPlaceOrder.Text = "Place Order";
                }
                else
                {
                    btnPlaceOrder.Text = "Submit RFQ";
                }
                break;
        }

        if (Page.Session["AccountType"] != null)
        {
            if ((Page.Session["AccountType"].ToString().ToLower() == "admin" || (Page.Session["AccountType"].ToString().ToLower() == "jpe" && (int)Page.Session["salesPersonAutoKey"] > 0 )) &&  (int)Page.Session["DEMO_CMP_AUTO_KEY"] < 1)
            {
                if (mode == "search")
                {
                    pnlAdminControls.Visible = true;
                }
            }
        }

        if ((Page.Session["AccountType"].ToString().ToLower() == "jpe" || Page.Session["AccountType"].ToString().ToLower() == "admin") && (int)Page.Session["DEMO_CMP_AUTO_KEY"] < 1)
            {
                 pnlJPECompanyDDL.Visible = true;
            }
       
        if (closePartsPopup)
        {
            ModalPopupExtenderPartsDetail.Hide();
        }

        upPnlSmartPartsBuyer.Update();
        upHeader.Update();
        ModalPopupExtenderThrobber.Hide();
    }

    protected void SetSessionOnLogout()
    {

        // ------- set on page load
        // Page.Session["strFileRootPath"] = String.Empty;
        // Page.Session["blnOnDevServer"] = TRUE OR FALSE 

        // Page.Session["MySessionId"]

        blnLogLogin = false;

        //-------------------- Navigation Session
        Page.Session["PanelMode"] = "login";
        Page.Session["LastPanelMode"] = "reset";
        Page.Session["strSearchList"] = string.Empty;

        Page.Session["strAssemblySearch"] = string.Empty;
        Page.Session["strNHAPartialMatches"] = string.Empty;
        Page.Session["strLastNHASearchString"] = string.Empty;




        //------------- set on Login

        Page.Session["LoggedInUserID"] = -1;
        Page.Session["salesPersonAutoKey"] = -1;



        Page.Session["UserName"] = string.Empty;
        Page.Session["Password"] = string.Empty;

        Page.Session["JobTitle"] = string.Empty;
        Page.Session["Company"] = string.Empty;
        Page.Session["AccountType"] = string.Empty; // user or admin
        //Page.Session["MySalesPersonAutoKey"] = string.Empty;
        Page.Session["EmailAddress"] = string.Empty;
        Page.Session["RDXPhoneNumber"] = string.Empty;

        Page.Session["pkAccountKey"] = -1;
        Page.Session["JPEapprovedForPricing"] = false;

        Page.Session["useAutoCheck"] = " ";
        Page.Session["useHints"] = " ";

        Page.Session["blnAllowNetTerms"] = false;

        Page.Session["RolodexKey"] = string.Empty;

        Page.Session["QuantumCompanyCode"] = string.Empty;
        Page.Session["JPECompanyCode"] = string.Empty;
        

        Page.Session["Quantum_CMP_AUTO_KEY"] = -1;
        Page.Session["JPE_CMP_AUTO_KEY"] = -10;
        Page.Session["DEMO_CMP_AUTO_KEY"] = -10;



       
        Page.Session["RDX_CONTACT_NAME"] = string.Empty;
        Session["SalesPersonPanel"] = string.Empty; //holds the salesperson contact panel
        Page.Session["TMC_AUTO_KEY"] = string.Empty;
        Page.Session["TERM_DESCRIPTION"] = string.Empty;

        Page.Session["Company_Type_for_Price"] = string.Empty;

        Page.Session["BILL_ADDRESS1"] = string.Empty;
        Page.Session["BILL_ADDRESS2"] = string.Empty;
        Page.Session["BILL_ADDRESS3"] = string.Empty;
        Page.Session["BILL_CITY"] = string.Empty;
        Page.Session["BILL_STATE"] = string.Empty;
        Page.Session["BILL_COUNTRY"] = string.Empty;
        Page.Session["BILL_ZIP_CODE"] = string.Empty;

        hdn_txtCardAddress1.Value = string.Empty;
        hdn_txtCardAddress2.Value = string.Empty;
        hdn_txtCardCity.Value = string.Empty;
        hdn_txtCardState.Value = string.Empty;
        hdn_txtCardZip.Value = string.Empty;

        Page.Session["hxFromDate"] = string.Empty;
        Page.Session["hxToDate"] = string.Empty;
        Page.Session["hxPartNo"] = string.Empty;

        blnUserLoaded = false;
        blnPartsCacheLoaded = false;
        blnGlobalLoaded = false;


        clearPartsCache();
        phSmartBuyerPanel.Controls.Clear();

    }

  //*********************************** JPE Smart Buyer

    protected string OpenPod(string strTitle, string strMatch)
    {
        string tmp = string.Empty;
        string lclTitle1 = strTitle;
        string lclTitle2 = string.Empty;

        tmp = "<div class='box'><b class='tc'><b class='L1'></b><b class='L2'></b><b class='L3'></b></b><div class='content'>";
        tmp += "<table>";
        if (strTitle != string.Empty)
        {
            if (strTitle == "multi")
            {
                lclTitle1 = "Matches starting with: ";
            }
            else if (strTitle == "didyoumean")
            {
                lclTitle1 = string.Empty;
                lclTitle2 = " - Did you mean:";
            }
            else if (strTitle == "nhamulti")
            {
                lclTitle1 = "Assemblies starting with: ";
            }
            else if (strTitle == "nomatch")
            {
                lclTitle1 = "No Matches Found";
            }
   
            tmp += "<tr><td class='SBpodTitleBar'><table><tr><td class='SBpodTitleBar' style='width:80%; white-space:nowrap;'>" + lclTitle1 + "<span class='SBpodSearchWord'>" + strMatch + "</span>" + lclTitle2 + "</td><td style='margin-right:5px;text-align:right;width:100%;white-space:nowrap;font:verdana;color:#666666;font-size:9px;'><!-- a name='' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQty');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.</div>')" + (char)34 + "><img src='images/save_button.gif' height='19' width='37' border='0' alt=''></a --></td></tr></table></td></tr>"; 
        }
        return tmp;

    }

    protected string closePod()
    {
        string tmp = string.Empty;
        tmp += "</table>";
        tmp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";
        return tmp;

    }

    protected string BuildFullMatchPanel(string strPartNumber, string strReplaces, string strDescription, decimal decCost, int intQtyOrdered, decimal decExtend, decimal decAvl, int intBO, string strImageName, string strLinkID, string strDebug, bool blnAvlSearchItem, int intCacheOrdered, int intCacheAvlCheck, decimal rawQtyAvailable, int intStockCategoryKey)
    {
        string tmp = string.Empty;
        Boolean blnShowPricing = true;
        if (decExtend == -999)
        {
            blnShowPricing = false;
        }
        else if (Page.Session["Company_Type_for_Price"].ToString() == "CUSTOM" && decCost == 0)
        {
            blnShowPricing = false;
        }

        if (!((bool)Page.Session["JPEapprovedForPricing"]))
        {
            blnShowPricing = false;
        }
        
        tmp += "<tr><td><table class='SBSingleMatchTable' style='width:535px;margin-left:3px;margin-right:3px;'>";
        tmp += "<tr><td class='SBpartImageTD' rowspan='2'>";
        tmp += "<a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">";
        tmp += "<img src='" + strImageName + "' width='75' alt='Click to view detailed information about this Part' onmouseover=" + (char)34 + "this.className='SBshowBorder';" + (char)34 + " onmouseout=" + (char)34 + "this.className='SBnoBorderSingleMatch';" + (char)34 + " class='SBnoBorderSingleMatch' /></a>";
        tmp += "</td><!-- top row table --><td class='partTopLine1'>";
        tmp += "<table>";
        tmp += "<tr><td class='SBpartNumberTD'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + strPartNumber + "</a></td>";
        tmp += "<td class='SBreplacesTD'><span class='SBreplacesLabel'>Replaces: </span>" + strReplaces + "</td></tr>";
        if (!blnShowPricing)
        {
            tmp += "<tr><td class='SBdescriptionTD' >" + strDescription + strDebug  + "</td>";
            tmp += "<td style='width:200px;padding-left:8px;'>";
            tmp += "<table border='1' bordercolor='#cccccc' cellpadding='0' cellspacing='0' >";
            tmp += "<tr><td class='SBqtyLabelTD'>Qty for Quote</td><td class='SBqtyLabelTD'>Avl</td><td class='SBqtyLabelTD'>B/O</td></tr>";
            tmp += "<tr><td class='SBqtyTD'>";

            if ((intQtyOrdered > 0 && !blncheckAvlScrape) || intCacheOrdered > 0)
            {
                tmp += intQtyOrdered;
            }
            else
            {
                tmp += "<input type='text' name='txtQTY_Search-SingleMatch_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_Search-SingleMatch_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)' Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_Search-SingleMatch_" + strLinkID + "');" + (char)34 + " value='" + intQtyOrdered + "' onFocus ='javascript:this.select();'>";
                
                
                tmp += "<input type='hidden'ID='hidden_txtQTY_Search-SingleMatch_" + strLinkID + "' name='hidden_txtQTY_Search-SingleMatch_" + strLinkID + "' value = '" + intQtyOrdered + "'></td>";
            }
            if (decAvl > 0)
            {
                tmp += "<td class='SBavailTD'>" + string.Format("{0:#####}", decAvl) + "</td>";
            }
            else if (intQtyOrdered < 1)
            {
                if (rawQtyAvailable > 0)
                {
                    tmp += "<td class='SBavailTD'><img src='images/instock5.gif' alt='' /></td>";
                }
                else
                {
                    tmp += "<td class='SBavailTD'><img src='images/callstock.gif' alt='' /></td>";
                }
            }
            else
            {
                tmp += "<td class='SBavailTD'>&nbsp;</td>";
            }
            if (intBO > 0)
            {
                string strThisBo = "<" + string.Format("{0:#####}", intBO);
                strThisBo = buildDeliveryDate("PNM", "", Convert.ToDecimal(intBO), strPartNumber, intStockCategoryKey);
                tmp += "<td class='SBboTD'" + strThisBo + "</td>";
            }
            else
            {
                tmp += "<td class='SBboTD'>&nbsp;</td>";
            }
            tmp += "</tr></table>";
        }
        else
        {
            //tmp += "<tr><td class='SBdescriptionTD' colspan='2'>" + strDescription + strDebug +  strLinkID + "</td>";
            tmp += "<tr><td class='SBdescriptionTD' colspan='2'>" + strDescription + strDebug + "</td>";
        }
        tmp += "</tr>";
        tmp += "</table>";
        tmp += "</td>";
        tmp += "<td class='SBmoreInfoTD' rowspan='2'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + "><img src='images/moreinfo.png' alt='Click to view detailed information about this Part'  border='0' /><br />Click for Details and NHA Parts List</a></td>";
        tmp += "</tr><!-- end of top row table -->";

        if (blnShowPricing)
        {
            tmp += "<td>";
            tmp += "<table border='1' bordercolor='#cccccc' cellpadding='0' cellspacing='0'>";
            tmp += "<tr>";
            tmp += "<td class='SBcostLabelTD'>Price</td>";
            tmp+= "<td class='SBqtyLabelTD'>Qty</td><td class='SBextendLabelTD'>Extend</td><td class='SBavailLabelTD'>Avail</td><td class='SBboLabelTD'>B/0</td></tr>";
            tmp += "<tr><td class='SBcostTD'>" + String.Format("{0:C}", decCost) + "</td><td class='SBqtyTD' ><table><tr><td>";
            if ((intQtyOrdered > 0 && !blnAvlSearchItem) || intCacheOrdered > 0)
            {
                tmp += intQtyOrdered;
            }
            else
            {
                tmp += "<input type='text' name='txtQTY_Search-SingleMatch_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_Search-SingleMatch_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)' Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_Search-SingleMatch_" + strLinkID + "');" + (char)34 + " value='" + intQtyOrdered + "' onFocus ='javascript:this.select();'>";
                tmp += "<input type='hidden'ID='hidden_txtQTY_Search-SingleMatch_" + strLinkID + "' name='hidden_txtQTY_Search-SingleMatch_" + strLinkID + "' value = '" + intQtyOrdered + "'>";
            }
            tmp += "</td></tr></table>";
            tmp += "</td><td class='SBextendTD'>" + String.Format("{0:C}", decExtend) + "</td>";
        
            if (decAvl > 0)
            {
                tmp += "<td class='SBavailTD'>" + string.Format("{0:#####}", decAvl) + "</td>";
            }
            else if (intQtyOrdered < 1)
            {
                if (rawQtyAvailable > 0)
                {
                    tmp += "<td class='SBavailTD'><img src='images/instock5.gif' alt='' /></td>";
                }
                else
                {
                    tmp += "<td class='SBavailTD'><img src='images/callstock.gif' alt='' /></td>";
                }
            }
            else
            {
                tmp += "<td class='SBavailTD'>&nbsp;</td>";
            }

            if (intBO > 0)
            {
            string strThisBo = "<" + string.Format("{0:#####}", intBO);
            strThisBo = buildDeliveryDate("PNM", "", Convert.ToDecimal(intBO), strPartNumber, intStockCategoryKey);
            tmp += "<td class='SBboTD'" + strThisBo + "</td>";
            }
            else
            {
                tmp += "<td class='SBboTD'>&nbsp;</td>";
            }
        tmp += "</tr></table>";
        }
        tmp += "</td></tr></table></td></tr>";
        return tmp;
    }

    protected string addLabelBar(string strLabel)
    {
        string tmp = string.Empty;
        tmp += "<tr><td class = 'SBpodTitleBar' ><table><tr><td class='SBpodTitleBar' style='width:80%; white-space:nowrap;'>" + strLabel + "</td><td style='margin-right:5px;text-align:right;width:100%;white-space:nowrap;font:verdana;color:#666666;font-size:9px; '><!-- a name='' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQty');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.</div>')" + (char)34 + "><img src='images/save_button.gif' height='19' width='37' border='0' alt=''></a --></td></tr></table></td></tr>";
        return tmp;
    }

    protected string BuildOEMPanel(string strPartNumber, string strDescription, int intQtyOrdered, decimal decCost, decimal decAvl, string strLinkID, string strDebug)
    {
        string tmp = string.Empty;
        string strQtyOrdered = string.Empty;

        tmp += " <tr><td>  <table class='SBOEMMatchTable' style='width:535px;margin-left:3px;	margin-right:3px;'>";
        tmp += "<tr><!-- top row table --><td class='partTopLine1'>";
        tmp += "<table><tr><td class='SBpartNumberOEMTD'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut999';" + (char)34 + ">" + strPartNumber + "</a></td><td class='SBdescriptionOEMTD'>" + strDescription + strDebug + "</td>";
        
        tmp += "<td><img src='images/instock5.gif' alt='' /></td>";
        tmp += "<td><table><tr>";

        if (((bool)Page.Session["JPEapprovedForPricing"]))
        {
            tmp += "<td class='SBcostLabelTD'>Price</td>";
        }
        else
        {
            tmp += "<td class='SBcostLabelTD'></td>";
        }
        
        tmp += "<td class='SBqtyLabelTD'>Qty</td><td class='SBboLabelTD'>&nbsp;</td><td class='SBavailLabelTD'>Avail</td></tr>";
        tmp += "<tr>";

        if (((bool)Page.Session["JPEapprovedForPricing"]))
        {
            tmp += "<td class='SBcostTD'>" + String.Format("{0:C}", decCost) + "</td>";
        }
        else
        {
            tmp += "<td class='SBcostTD'></td>";
        }
       
        tmp += "<td class='SBqtyTD'><table><tr><td><input type='text' name='txtQTY_Search-OEM_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_Search-OEM_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_Search-OEM_" + strLinkID + "' onFocus ='javascript:this.select();');" + (char)34 + " ";


        if (intQtyOrdered > 0)
        {
            strQtyOrdered = intQtyOrdered.ToString();
        }

        tmp += "  value='" + strQtyOrdered + "'>";
        tmp += "<td style='margin-right:5px auto;text-align:right;font:verdana;color:#666666;font-size:9px;white-space:nowrap;'></td></tr></table>";
        tmp += "<input type='hidden'ID='hidden_txtQTY_Search-OEM_" + strLinkID + "' name='hidden_txtQTY_Search-OEM_" + strLinkID + "' value = '" + strQtyOrdered + "'>";
        tmp += "</td><td class='SBboTD'>&nbsp;</td><td class='SBavailTD'>" + decAvl + "</td></tr></table></td></tr></table>";
        tmp += "</td><td class='SBmoreInfoTD' rowspan='2'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Click for Details</a></td></tr></table>";
        tmp += "</td></tr>";

        return tmp;
    }

    protected string openNHATable()
    {
        string tmp = string.Empty;
        tmp += "<tr><td><table class='SBNHAMatchTable' style='width:535px;margin-left:3px;	margin-right:3px;margin-top:1px;'>";
        return tmp;
    }
    protected string closeNHATable()
    {
        string tmp = string.Empty;
        tmp += "</table></td></tr>";
        return tmp;
    }

    protected string BuildAssociatedLineItem(string strPartNumber, string strDescription, decimal decCost, int intQtyOrdered, string strLinkID, string strDebug, decimal rawQtyAvailable)
    {
        string tmp = string.Empty;
        string strQtyOrdered = string.Empty;

        tmp += "<tr><td class='SBpartNumberNHATD'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + strPartNumber + "</a></td>";
        //tmp += "<td class='SBdescriptionNHATD'>" + strDescription + strDebug + strLinkID +  "</td>";
        tmp += "<td class='SBdescriptionNHATD'>" + strDescription + strDebug  + "</td>";

        if (rawQtyAvailable > 0)
        {
            tmp += "<td style='vertical-align:bottom;padding-right:5px;padding-bottom:6px;' ><img src='images/instock5.gif' alt='' /></td>";
        }
        else
        {
            tmp += "<td style='vertical-align:bottom;padding-right:5px;padding-bottom:6px;' ><img src='images/callstockr.gif' alt='' /></td>";
        }

        if (((bool)Page.Session["JPEapprovedForPricing"]))
        {
            tmp += "<td class='SBcostTD'>" + String.Format("{0:C}", decCost) + "</td>";
        }
        else
        {
            tmp += "<td class='SBcostTD'></td>";
        }
        
        //tmp += "<td class='SBqtyNHATD'><table><tr><td><input type='text' name='txtQTY_Search-NHA_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_Search-NHA_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_Search-NHA_" + strLinkID + "' );" + (char)34 + " ";

        tmp += "<td class='SBqtyNHATD'><table><tr><td><input type='text' name='txtQTY_Search-NHA_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_Search-NHA_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)'  Onblur=" + (char)34 + "javascript: return alignQtyBoxes('txtQTY_Search-NHA_" + strLinkID + "' );" + (char)34 + " ";

        if (intQtyOrdered > 0)
        {
            strQtyOrdered = intQtyOrdered.ToString();
        }

        tmp += "  value='" + strQtyOrdered + "' onFocus ='javascript:this.select();' >";
        tmp += "</td></tr></table>";
        tmp += "<input type='hidden'ID='hidden_txtQTY_Search-NHA_" + strLinkID + "' name='hidden_txtQTY_Search-NHA_" + strLinkID + "' value = '" + strQtyOrdered + "'>";
        tmp += "</td><td class='SBmoreInfoNHATD'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Click for Details</a></td></tr>";
        return tmp;
    }

    protected string BuildNHAShowMore(string strNHAAssembly)
    {
        string tmp = string.Empty;
        tmp += "<tr><td class='SBpartNumberNHATD' colspan='4'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|" + strNHAAssembly + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Show all NHA Parts</td>";
        tmp += "<td class='SBmoreInfoNHATD' style='text-align:right; margin:0px auto; padding-right:4px;'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|" + strNHAAssembly + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + "><img src='images/Open-32.png'  alt='Click to view all NHA Parts' onmouseover=" + (char)34 + "this.className='SBshowBorder';" + (char)34 + "  onmouseout=" + (char)34 + "this.className='SBnoBorderSingleMatch';" + (char)34 + " class='SBnoBorderSingleMatch' /></a></td></tr>";
        return string.Empty;

    }

    protected string BuildMultiMatchPanel(string strPartNumber, string strDescription, decimal decCost, int intQtyOrdered, string strImageName, string strLinkID, string strDebug, string strSearchKey, int andMore, decimal rawQtyAvailable, string callType)
    {
        string tmp = string.Empty;
        string strQtyOrdered = string.Empty;

        //callType - "didyoumean" "multi"
        
        string highlightedPN = VBCLass1.Highlighter(strPartNumber, strSearchKey);
        string tableCSS = callType == "didyoumean" ? "SBDidYouMeanTable" : "SBMultipleMatchTable";
       
        tmp += "<tr><td>";
        tmp += "<table class='" + tableCSS + "' style='width:535px;margin-left:3px;	margin-right:3px;margin-top:5px;'>";
        tmp += "<tr><!-- top row table --><td class='partTopLine1'>";
        tmp += "<table><tr><td class='SBmoreInfoTD' rowspan='2'><img src='" + strImageName + "' alt=''  width='50' /></td>";
        tmp += "<td class='SBpartNumberMultiMatchTD'>" + highlightedPN + "</a></td>";
        tmp += "<td class='SBdescriptionMultiMatchTD'>" + strDescription + strDebug + "</td>";

        if (rawQtyAvailable > 0)
        {
            tmp += "<td class='SBavailTD'><img src='images/instock5.gif' alt='' /></td>";
        }
        else
        {
            tmp += "<td class='SBavailTD'><img src='images/callstock.gif' alt='' /></td>";
        } 
        tmp += "<td><table><tr>";

        if (((bool)Page.Session["JPEapprovedForPricing"]))
        {
            tmp += "<td class='SBcostLabelTD'>Price</td>";
        }
        else
        {
            tmp += "<td class='SBcostLabelTD'></td>";
        }

        tmp += "<td class='SBqtyLabelTD'>Qty</td>";
        tmp += "</tr><tr>";

        if (((bool)Page.Session["JPEapprovedForPricing"]))
        {
            tmp += "<td class='SBcostTD'>" + String.Format("{0:C}", decCost) + "</td>";
        }
        else
        {
            tmp += "<td class='SBcostTD'></td>";
        }

        tmp += "<td class='SBqtyTD'><table><tr><td><input type='text' name='txtQTY_Search-MultiMatch_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_Search-MultiMatch_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_Search-MultiMatch_" + strLinkID + "');" + (char)34 + "  onFocus ='javascript:this.select();' ";

        if (intQtyOrdered > 0)
        {
            strQtyOrdered = intQtyOrdered.ToString();
        }
        tmp += "  value='" + strQtyOrdered + "'>";
        tmp += "</td></tr></table>";
        tmp += "<input type='hidden'ID='hidden_txtQTY_Search-MultiMatch_" + strLinkID + "' name='hidden_txtQTY_Search-MultiMatch_" + strLinkID + "' value = '" + strQtyOrdered + "'>";

        tmp += "</td>";
        tmp += "</tr></table></td>";
        tmp+= " </tr>";
        if (andMore > 0)
        {
            tmp += "<tr><td colspan = '3' class='SBMultiAndMore'>" + andMore + " more items begin with " + strSearchKey + ".  Please refine your search.</td></tr>";
        }
        tmp += "</table></td>";
        tmp += "<td class='SBmoreInfoTD' rowspan='2'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Click for Details</a></td></tr>";
        tmp += "</table></td></tr>";
        return tmp;
    }

    protected string BuildNoMatchPanel(string[] strNoMatches, int intNoMatchCount)
    {
        string tmp = string.Empty;
        bool blnfirstplaced = false;

        if (strNoMatches[0] == string.Empty && intNoMatchCount == 0)
        {
            return string.Empty;
        }

        for (int lp = 0; lp <= intNoMatchCount; lp++)
        {
            if (strNoMatches[lp] != "DUPLICATE")
            {
                if (!blnfirstplaced)
                {
                    tmp += OpenPod("nomatch", "");
                    tmp += "<tr><td>";
                    tmp += "<table class='SBNoMatchTable' style='margin-top:3px;'>";
                    blnfirstplaced = true;
                }
            }
            tmp += "<tr><td class='SBpartNumberTD'>" + strNoMatches[lp] + "</td><td>Not found in our database</td></tr>";
        }

        if (blnfirstplaced)
        {
            tmp += "</table><!-- end of top row table --></td></tr>";
            tmp += closePod();
        }
        return tmp;
    }

     protected string BuildDidYouMeanPanel(string[] sortArray, int startEl, DataTable lowTemp, string searchTag, string searchLabel)
    {
        string tmp = string.Empty;
        int CurCacheRow = -1;
         Decimal decListPrice = 0m;
         Decimal decExchangePrice = 0m;
         int intPartKey = 0;
         int intStockCategoryKey = 0;
         decimal rawQtyAvailable = 0m;
         int intQtyOrdered =0;
         decimal decCost = 0m;
         string strImage = string.Empty;
         string AddParts = string.Empty;
         bool blnDidYouMeanAdded = false;

         //----- add the parts to the cache
         int lp;
         for (lp = startEl; lp <= startEl + showDidYouMeanDepth - 1; lp++)
         {
             AddParts += sortArray[lp].Substring(4) + "|";
         }
         PartsCacheManager("AddParts", AddParts);

         //--- add all did you mean items to the cache before this is called


         //----- open the pod 
         tmp += OpenPod("didyoumean", searchLabel);

        for (lp = startEl; lp <= startEl + showDidYouMeanDepth - 1; lp++)
        {
            //--CurCacheRow = GenUtils.FindRowforStrippedPartNumber(sortArray[lp].Substring(4), dtCacheMaster);
            //string dsajdsajda = sortArray[lp].Substring(4);
            CurCacheRow = GenUtils.FindRowforStrippedPartNumber(sortArray[lp].Substring(4), dtCacheMaster);
            //---- XRef to the proper row for OEM Part numbers
            CurCacheRow = findJPNRow(CurCacheRow, dtCacheMaster);
            //----- find the DataTable Element for the 000-106711JP
            decListPrice = decimal.Parse(dtCacheMaster.Rows[CurCacheRow]["pmast_LIST_PRICE"].ToString());
            decExchangePrice = decimal.Parse(dtCacheMaster.Rows[CurCacheRow]["pmast_EXCH_LIST_PRICE"].ToString());
            intPartKey = int.Parse(dtCacheMaster.Rows[CurCacheRow]["pmast_P_PNM_AUTO_KEY"].ToString());
            if (dtCacheMaster.Rows[CurCacheRow]["pmast_P_STC_AUTO_KEY"].ToString() != string.Empty)
            {
                intStockCategoryKey = int.Parse(dtCacheMaster.Rows[CurCacheRow]["pmast_P_STC_AUTO_KEY"].ToString());
            }

            rawQtyAvailable = (decimal)dtCacheMaster.Rows[CurCacheRow]["calc_QTY_Available"];
            if (blncheckAvlScrape && (int)dtCacheMaster.Rows[CurCacheRow]["cur_QTY_AvailableSearch"] > 0)
            {
                intQtyOrdered = (int)dtCacheMaster.Rows[CurCacheRow]["cur_QTY_AvailableSearch"];
            }
            else
            {
                intQtyOrdered = (int)dtCacheMaster.Rows[CurCacheRow]["cur_QTY_ORDERED"];
            }
            decCost = (decimal)dtCacheMaster.Rows[CurCacheRow]["calc_SELL_PRICE"];
            strImage = GenUtils.findImageLink(CurCacheRow, dtCacheMaster);

           
            if (decCost > 0 )
            {
                       tmp += BuildMultiMatchPanel(dtCacheMaster.Rows[CurCacheRow]["pmast_PN"].ToString(), dtCacheMaster.Rows[CurCacheRow]["pmast_Description"].ToString(), decCost, 0, strImage, dtCacheMaster.Rows[CurCacheRow]["pmast_PN"].ToString(), BuildstrDebug(CurCacheRow), searchTag, 0, rawQtyAvailable, "didyoumean");
                       blnDidYouMeanAdded = true;
             }

        }

        if (blnDidYouMeanAdded)
        {
            tmp += closePod();
            return tmp;
        }
        else
        {
            return string.Empty;
        }

    }

    protected string BuildNHAMultiMatchPanel(string[,] strNHAMatches, int intNHAMatchCount, string strSearchKey)
    {
        string tmp = string.Empty;
        bool blnfirstplaced = false;
        string highlightedNHA;

        if (strNHAMatches[0,0] == string.Empty && intNHAMatchCount == 0)
        {
            return string.Empty;
        }

        for (int lp = 0; lp <= intNHAMatchCount; lp++)
        {
            if (!blnfirstplaced)
            {
                tmp += "<tr><td>";
                tmp += "<table class='SBNHANoMatchTable' style='margin-top:3px;'>";
                blnfirstplaced = true;
            }

            highlightedNHA = VBCLass1.Highlighter(strNHAMatches[lp, 0], strSearchKey);

            tmp += "<tr><td class='SBpartNumberTD'><a name='' onclick=" + (char)34 + "javascript:showThisNHA('" + strNHAMatches[lp, 0] + "');" + (char)34 + "  onmouseover=" + (char)34 + "this.style.color = 'red'; this.style.cursor = 'hand';" + (char)34 + " onmouseout=" + (char)34 + "this.style.color = '#021238' ; this.style.cursor='default';" + (char)34 + ";>" + highlightedNHA + "</a></td></tr>";
        }
                  
        if (blnfirstplaced)
        {
            tmp += "</table><!-- end of top row table --></td></tr>";
        }
        return tmp;
    }

    private int FindPartsTableRow(string strThisPartNumber)
    {
        int intTemp = -1;
        string strThisPN = string.Empty;
        int intSearchCount = -1;
        string strThisSearch = string.Empty;

        foreach (DataRow myRow in dtCacheMaster.Rows)
        {
            intSearchCount++;
            strThisPN = myRow["PN_STRIPPED"].ToString();
            strThisSearch = GenUtils.MakeUCaseNumChar(strThisPartNumber);
            if (strThisPN == strThisSearch)
            {
                intTemp = intSearchCount;
            }
        }
        return intTemp;
    }

    //****************************************** Checkout


    private string BuildCheckoutCart(string cartType, bool blnItemsOnPage, bool blnItemsInCart)
    {
        string tmp = string.Empty;
        bool blnEmptyCart = true;

        // cartType search or checkout

        if (dtCacheMaster != null)
        {
            dtCacheMaster.Rows.Clear();
        }
        blnItemsInCart = false;


        //-------------------------------- SPEED - Is this needed or is the Cache loaded
        dtCacheMaster = GenUtils.GetCacheforSessionID(Page.Session["MySessionId"].ToString());
        if (dtCacheMaster == null)
        {
            return "ERROR WITH CACHE LOOKUP";
        }

        int intColPad = 8;
        if ((bool)Page.Session["JPEapprovedForPricing"])
        {
            intColPad = 5;
        }

        if (dtCacheMaster.Rows.Count > 0)
        {
            foreach (DataRow myControlRow in dtCacheMaster.Rows)
            {
                if (myControlRow["cur_QTY_ORDERED"] != DBNull.Value)
                {
                    if ((int)myControlRow["cur_QTY_ORDERED"] > 0)
                    {
                        blnEmptyCart = false;
                        break;
                    }
                }
            }
        }

        tmp = "<table class='cartTable'  style='width:560px;'>";
        tmp += "<tr><td colspan='" + intColPad + 5 + "' class='cartRowSpacer' style='height:3px;'></td></tr>";

        if (blnEmptyCart)
        {
            tmp += "<tr><td class='cartColSpacer'></td>";
            tmp += "<td class='cartEmptyCartHeader' colspan = '" + intColPad + 3 + "' style='width:560px;' >Your cart is Empty</td>";
            tmp += "<td class='cartColSpacer'></td>";
            tmp += "</tr>";
            tmp += "<tr><td colspan='" + intColPad + 5 + "' class='cartRowSpacer'></td></tr>";
        }
        else
        {
            tmp += "<!-- Header Cart Row -->";
            tmp += "<tr>";
            tmp += "<td class='cartColSpacer'></td>";
            tmp += "<td class='cartJPENumberHeader'>JPE Part No.</td>";
            tmp += "<td class='cartColSpacer'></td>";
            tmp += "<td class='cartDescriptionHeader'>Description</td>";
            tmp += "<td class='cartColSpacer'></td>";
            tmp += "<td class='cartQtyHeader'>Qty</td>";
            tmp += "<td class='cartColSpacer'></td>";
            tmp += "<td class='cartBOHeader'>B/O</td>";
            tmp += "<td class='cartColSpacer'></td>";

            if ((bool)Page.Session["JPEapprovedForPricing"])
            {
            tmp += "<td class='cartPriceHeader'>Price</td>";
            tmp += "<td class='cartColSpacer'></td>";
            tmp += "<td class='cartExtensionHeader'>Extension</td>";
            tmp += "<td class='cartColSpacer'></td>";
            }

            tmp += "</tr>";
            tmp += " <tr><td colspan='" + intColPad + 5 + "' class='cartRowSpacer'></td></tr>";
        }

        //--------------------- line item

        string strPartNumber = string.Empty;
        string strOEMPartNumber = string.Empty;
        string strDescription = string.Empty;
        string strLinkID = string.Empty;
        string strthisQty = string.Empty;
        decimal decCost = 0m;
        string NextShipDate = string.Empty;
        int intQtyOrdered = 0;
        decimal decExt = 0m;
        decimal decAvl = 0m;
        int intBO = 0;
        int intPartKey = 0;
        int intStockCategoryKey = 0;
        decimal decOrderTotal = 0m;

        if (dtCacheMaster.Rows.Count > 0)
        {
            foreach (DataRow myControlRow in dtCacheMaster.Rows)
            {
                if (myControlRow["cur_QTY_ORDERED"] != DBNull.Value)
                {
                    if ((int)myControlRow["cur_QTY_ORDERED"] > 0)
                    {
                        blnItemsInCart = true;
                        strPartNumber = myControlRow["pmast_PN"].ToString();
                        strOEMPartNumber = myControlRow["pmast_ALT_PN"].ToString();
                        strDescription = myControlRow["pmast_DESCRIPTION"].ToString();
                        strLinkID = myControlRow["pmast_PN"].ToString();
                        decCost = (decimal)myControlRow["calc_SELL_PRICE"];
                        intQtyOrdered = (int)myControlRow["cur_QTY_ORDERED"];
                        intStockCategoryKey = (int)myControlRow["pmast_P_STC_AUTO_KEY"];
                        strthisQty = intQtyOrdered.ToString();
                        decExt = decCost * intQtyOrdered;
                        decOrderTotal += decExt;
                        intBO = 0;
                        decAvl = (decimal)myControlRow["calc_QTY_AVAILABLE"] - intQtyOrdered;
                        if (decAvl >= intQtyOrdered)
                        {
                            decAvl = Convert.ToDecimal(intQtyOrdered);
                        }
                        else
                        {
                            decAvl = (decimal)myControlRow["calc_QTY_AVAILABLE"];
                            intBO = intQtyOrdered - Convert.ToInt32(decAvl);
                        }
                        if (intBO < 0)
                        {
                            intBO = 0;
                        }
                        string strThisBo = string.Empty;

                        if (intBO > 0)
                        {
                            strThisBo = buildDeliveryDate("PNM", "", Convert.ToDecimal(intBO), strPartNumber, intStockCategoryKey);
                            NextShipDate = "Roll your mouse over the 'B/O' number for information on back-ordered items";
                        }

                        intPartKey = int.Parse(myControlRow["pmast_P_PNM_AUTO_KEY"].ToString());

                        tmp += "<!-- Cart Row -->";
                        tmp += "<tr>";
                        tmp += "<td class='cartColSpacer'></td>";
                        tmp += "<td class='cartJPENumber'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + strPartNumber + "</a></td>";
                        tmp += "<td class='cartColSpacer'></td>";
                        tmp += "<td class='cartDescription'>" + strDescription + "</td>";
                        tmp += "<td class='cartColSpacer'></td>";
                        tmp += "<td class='cartQty'>";
 
                        if (cartType == "checkout")
                         {
                             tmp += "<input type='text' name='txtQTY_CheckoutCart_" + strPartNumber + "' class='SBtxtQtyCntrl' id='txtQTY_CheckoutCart_" + strPartNumber + "' maxlength='5' onkeypress='return checkIt(event);'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_CheckoutCart_" + strPartNumber + "');" + (char)34 + " value='" + strthisQty + "'  onFocus ='javascript:this.select();'>";
                              tmp += "<input type='hidden'ID='hidden_txtQTY_CheckoutCart_" + strPartNumber + "' name='hidden_txtQTY_CheckoutCart_" + strPartNumber + "' value = '" +  strthisQty + "'>";
                        }
                        else
                        {
                            tmp += "<input type='text' name='txtQTY_CheckoutCart_" + strPartNumber + "' class='SBtxtQtyCntrl' id='txtQTY_CheckoutCart_" + strPartNumber + "' maxlength='5' onkeypress='return checkIt(event);'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQty', 'txtQTY_CheckoutCart_" + strPartNumber + "');" + (char)34 + " value='" + strthisQty + "'  onFocus ='javascript:this.select();'>";
                            tmp += "<input type='hidden'ID='hidden_txtQTY_CheckoutCart_" + strPartNumber + "' name='hidden_txtQTY_CheckoutCart_" + strPartNumber + "' value = '" + strthisQty + "'>";
                        } 
                         tmp += "</td>";
                         tmp += "<td class='cartColSpacer'></td>";
                         tmp += "<td class='cartBO'" + strThisBo + "</td>";
                         tmp += "<td class='cartColSpacer'></td>";

                         if ((bool)Page.Session["JPEapprovedForPricing"])
                         {
                             tmp += "<td class='cartPrice'>" + String.Format("{0:C}", decCost) + "</td>";
                             tmp += "<td class='cartColSpacer'></td>";
                             tmp += "<td class='cartExtension'>" + String.Format("{0:C}", decExt) + "</td>";
                             tmp += "<td class='cartColSpacer'></td>";
                         }

                        tmp += "</tr>";
                        tmp += "<tr>";
                        tmp += "<td colspan='" + intColPad + 5 + "' class='cartRowSpacer'></td>";
                        tmp += "</tr>";
                    }
                }
            }
        }

        tmp += "<tr>";
        if (blnEmptyCart)
        {
            tmp += "<td class='cartColSpacer'></td><td class='cartNeutral' colspan='" + intColPad + 3 + "' style='text-align:center;'>";
        }
        else
        {
                tmp += "<td class='cartColSpacer'></td><td class='cartNeutral' colspan='8' style='text-align:right;margin-right:5px auto;'>";
        }

        tmp += "<table>";
        tmp += "<tr>";
        string strCheckoutTitle = "View Checkout Page";
        string strCheckoutExplain = "Click this button to view the JPE Smart Buyer Checkout Page.";
        string strUpdateCartButton = "Recalculate";
        string strUpdateCartExpl = "Click this button to update the order total when you change the quantity of any item in yout shopping cart.  To remove an item from your cart change the quantity to zero.";
        if (!(bool)Page.Session["JPEapprovedForPricing"])
        {
            strCheckoutTitle = "View RFQ Page";
            strCheckoutExplain = "Click this button to view the JPE Smart Buyer RFQ page.";
            strUpdateCartButton = "Update Quantities";
            strUpdateCartExpl = "Click this button to change the quantities for the RFQ.  To remove an item from your cart change the quantity to zero.";
        }
        
        if (cartType == "search")
        {
            if (blnItemsInCart)
            {
                tmp += "<td style='padding-left:5px;padding-right:5px;' ><input type='button' ID='btnEmptyMyCart'  OnClick=" + (char)34 + "javascript: return qtyUpdate('btnShowThrobber', 'btnMTSPBPanel_HDN');" + (char)34 + " value='Empty Cart' onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click this button to remove all items from your shopping cart and from the JPE Smart Buyer search panel.</div>')" + (char)34 + "  /></td>";
                tmp += "<td style='padding-left:5px;padding-right:5px;' ><input type = 'button' id='btnSPBSaveTop' value = 'Update Cart' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQty');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click the UPDATE CART button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.</div>')" + (char)34 + "></td>";

            strCheckoutTitle = "Proceed to Checkout";
            strCheckoutExplain = "Click this button to proceed to the checkout page and complete your order.";
            if (!(bool)Page.Session["JPEapprovedForPricing"])
            {
                strCheckoutTitle = "Proceed with RFQ";
                strCheckoutExplain = "Click this button to proceed to the RFQ page and complete your request.";
            }
          
            }
            else if (blnItemsOnPage)
            {
                tmp += "<td style='padding-left:5px;padding-right:5px;' ><input type='button' ID='btnEmptyMyCart'  OnClick=" + (char)34 + "javascript: return qtyUpdate('btnShowThrobber', 'btnMTSPBPanel_HDN');" + (char)34 + " value='Clear Search' onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click this button to clear the search results from the JPE Smart Buyer search panel.</div>')" + (char)34 + "  /></td>";
            }
            tmp += "<td style='padding-left:5px;padding-right:5px;'><input type='button' ID='btnCheckoutNowTop'  OnClick=" + (char)34 + "javascript: return qtyUpdate('btnShowThrobber', 'btnCheckoutNow_HDN');" + (char)34 + " value='" + strCheckoutTitle + "'  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>" + strCheckoutExplain + "</div>')" + (char)34 + "  /></td>";
        }
        else
            {
                tmp += "<td style='padding-left:5px;padding-right:5px;'><input type='button' ID='btnPartsSearchfromCart1'  OnClick=" + (char)34 + "doButtonClick('btnPartsSearchfromCart2');" + (char)34 + " value='Back'  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click this button to return to the search page.</div>')" + (char)34 + "   /></td>";
             if (!blnEmptyCart)
             {
                 tmp += "<td style='padding-left:5px; padding-right:5px;'><input type='button' ID='btnRecalcCart1'  OnClick=" + (char)34 + "javascript: return qtyUpdate('btnShowThrobber', 'btnUpdatePartsOnQty');" + (char)34 + " value='" + strUpdateCartButton + "' onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>" + strUpdateCartExpl + "</div>')" + (char)34 + "    /></td>";
             }
            }

        tmp += "</tr></table>";
        tmp += "</td>";

        if (!blnEmptyCart && (bool)Page.Session["JPEapprovedForPricing"])
        {
                  tmp += "<td colspan='1' class='cartNeutral'><span style='font:verdana;color:#666666;font-size:9px; width:100%;white-space:nowrap;'>";
                  if (Page.Session["useAutoCheck"].ToString() == " checked ")
                  {
                      tmp += "<input type ='hidden' name='hdn_chkAutoUp' id='hdn_chkAutoUp' value = 'true' ></span>";
                  }    
                  tmp += " Total</td>";
                tmp += "<td class='cartColSpacer'></td>";
                tmp += "<td class='cartTotal'  onmouseover=" + (char)34 + "fadeBox.showTooltip(event,'The total price for your order is " + String.Format("{0:C}", decOrderTotal) + "')" + (char)34 + "  >" + String.Format("{0:C}", decOrderTotal) + "</td>";
              }
        tmp += "<td class='cartColSpacer'></td>";
        tmp += "</tr>";

        if (NextShipDate != string.Empty)
        {
            tmp += "<tr><td class='cartColSpacer'></td><td class='ordNextDateBar' colspan = '" + intColPad + 3 + "' style='font-size:12px;'>&nbsp;<img src='images/alert_icon.png' alt= '' border='0' />&nbsp;&nbsp;" + NextShipDate + "</td><td class='cartColSpacer'></td>";
            tmp += "</tr>";
        }
        tmp += "<tr><td colspan='" + intColPad + 6 + "' class='cartNeutral' style='height:3px;'></td></tr>";
        tmp += "</table>";
              
        return tmp;
    }

   
    //***************************************** Order History

    protected string BuildOrderHistoryPanel(DataTable dtOrderHistory, DataTable dtRFQHistory, DataTable dtWebsiteOrderHistory)
    {
        string temp = string.Empty;
        bool blnValidOrder = true;
        bool blnValidRFQ = true;
        bool blnValidWebsite = true;

        if(dtOrderHistory == null)
        {
            blnValidOrder = false;
        }
        else if (dtOrderHistory.Rows.Count < 1)
        {
            blnValidOrder = false;
        }

        if (dtRFQHistory == null)
        {
            blnValidRFQ = false;
        }
        else if (dtRFQHistory.Rows.Count < 1)
        {
            blnValidRFQ = false;
        }

        if (dtWebsiteOrderHistory == null)
        {
            blnValidWebsite = false;
        }
        else if (dtWebsiteOrderHistory.Rows.Count < 1)
        {
            blnValidWebsite = false;
        }

        if (!blnValidOrder && !blnValidRFQ && !blnValidWebsite)
        {
            return string.Empty;
        }

        temp += "<!-- Header Row -->";
        temp += "<tr><td class='ordHistoryHeader' >Date</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td  class='ordHistoryHeader'>Ref. Number</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryHeader' >Total</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryHeader' >Status</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryHeader' >Details</td>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";

        string curSORefNo = string.Empty;
        int SORefNoCounter = -1;
        //------ build an object array of all sales orders and dates
        string[,] SORefNoOrders = new string[1000, 3];
        if (dtOrderHistory != null)
        {
            foreach (DataRow myRow in dtOrderHistory.Rows)
            {
                if (curSORefNo != myRow["COMPANY_REF_NUMBER"].ToString())
                {
                    SORefNoOrders[++SORefNoCounter, 0] = myRow["COMPANY_REF_NUMBER"].ToString();
                    SORefNoOrders[SORefNoCounter, 1] = myRow["ENTRY_DATE"].ToString();
                    SORefNoOrders[SORefNoCounter, 2] = myRow["SOH_AUTO_KEY"].ToString();
                    curSORefNo = myRow["COMPANY_REF_NUMBER"].ToString();
                }
            }
        }

        string curRFQItem = string.Empty;
        int RFQArrayCounter = - 1;
        //------ build an object array of all sales orders and dates
        string[,] RFQItems = new string[500, 2];
        if (dtRFQHistory != null)
        {
            foreach (DataRow myRow in dtRFQHistory.Rows)
            {
                if (curRFQItem != myRow["CQH_AUTO_KEY"].ToString())
                {
                    RFQItems[++RFQArrayCounter, 0] = myRow["CQH_AUTO_KEY"].ToString();
                    RFQItems[RFQArrayCounter, 1] = myRow["ENTRY_DATE"].ToString();
                    curRFQItem = myRow["CQH_AUTO_KEY"].ToString();
                }
            }
        }

        int lp;
        if (blnshowDebuggerOutput)
        {
            //---------- show the Sales Order Table
            for (lp = 0; lp <= SORefNoCounter; lp++)
            {
                Debug.WriteLine(lp + " QUANTUM SALES ORDERS " + SORefNoOrders[lp, 0] + " " + SORefNoOrders[lp, 1] + " " + SORefNoOrders[lp,2]);
            }
            //------ do the same for quotes to be mixed in 
            for (lp = 0; lp <= RFQArrayCounter; lp++)
            {
                Debug.WriteLine(lp + " RFQ ITEM" + RFQItems[lp, 0] + " " + RFQItems[lp, 1]);
            }
        }
        int SOLpCounter = -1;
        int RFQLpCounter = -1;
        DateTime nextSODate;
        DateTime nextRFQDate;
        OrderHistoryRowCounter = -1;
        DateTime noDate = new DateTime(2000,1,1,12,0,0);
        DateTime lastDatePlaced = noDate;
        DateTime lastWebsiteDatePlaced = DateTime.Now.AddDays(1);

        //--------------- this has to be modified to mix the arrays
        for (lp = 0; lp <= 5000; lp++)
        {
            nextSODate = DateTime.Parse("1/1/2000");
            nextRFQDate = DateTime.Parse("1/1/2000");
            if (SOLpCounter + 1 <= SORefNoCounter)
            {
                nextSODate = DateTime.Parse(SORefNoOrders[SOLpCounter + 1, 1]);
            }

            if (RFQLpCounter + 1 <= RFQArrayCounter)
            {
                nextRFQDate = DateTime.Parse(RFQItems[RFQLpCounter + 1, 1]);
            }
    
            if (DateTime.Compare(nextRFQDate, noDate) > 0 && DateTime.Compare(nextSODate, noDate) > 0)
            {
                if (DateTime.Compare(nextRFQDate, nextSODate) > 0)
                {
                    RFQLpCounter++;
                    lastDatePlaced = nextRFQDate;
                    temp += PlaceWebsiteOrder(lastDatePlaced, lastWebsiteDatePlaced, dtWebsiteOrderHistory, lp, SORefNoOrders, SORefNoCounter);
                    lastWebsiteDatePlaced = lastDatePlaced;
                    temp += AddRFQLine(RFQItems[RFQLpCounter, 0], dtRFQHistory, RFQLpCounter, lp);
                }
                else
                {
                    lastDatePlaced = nextSODate;
                    SOLpCounter++;
                    temp += PlaceWebsiteOrder(lastDatePlaced, lastWebsiteDatePlaced, dtWebsiteOrderHistory, lp, SORefNoOrders, SORefNoCounter);
                    lastWebsiteDatePlaced = lastDatePlaced;
                    temp += AddSalesOrder(SORefNoOrders[SOLpCounter, 2], dtOrderHistory, SOLpCounter, lp);
                }
            }
            else if (DateTime.Compare(nextRFQDate, noDate) > 0)
            {
                RFQLpCounter++;
                lastDatePlaced = nextRFQDate;
                temp += PlaceWebsiteOrder(lastDatePlaced, lastWebsiteDatePlaced, dtWebsiteOrderHistory, lp, SORefNoOrders, SORefNoCounter);
                lastWebsiteDatePlaced = lastDatePlaced;
                temp += AddRFQLine(RFQItems[RFQLpCounter, 0], dtRFQHistory, RFQLpCounter, lp);
            }
            else if (DateTime.Compare(nextSODate, noDate) > 0)
            {
                SOLpCounter++;
                lastDatePlaced = nextSODate;
                temp += PlaceWebsiteOrder(lastDatePlaced, lastWebsiteDatePlaced, dtWebsiteOrderHistory, lp, SORefNoOrders, SORefNoCounter);
                lastWebsiteDatePlaced = lastDatePlaced;
                temp += AddSalesOrder(SORefNoOrders[SOLpCounter, 2], dtOrderHistory, SOLpCounter, lp);
            }
        }
        return temp;
    }

    protected string BuildAdminPanel(DataTable dtAdminUsers)
    {
        string temp = string.Empty;
        bool blnAdminTable = true;
     
        if (dtAdminUsers == null)
        {
            blnAdminTable = false;
        }
        else if (dtAdminUsers.Rows.Count < 1)
        {
            blnAdminTable = false;
        }
        if (!blnAdminTable)
        {
            return string.Empty;
        }

        // new  
        // denied  
        // guest
        // admin - user - jpe


        AdminUsersPanelCounter = -1;
        temp = "<table class='SBecomTable'>";
        temp += "<tr>";
        temp += "<td class='SBHeaderBarTD' colspan='3'>";
        if (Page.Session["AccountType"].ToString().ToLower() == "admin")
        {
            temp += "Website Users Admin Panel";
        }
        else
        {
            temp += "Website Users Panel";
        }
        temp += "</td>";
        temp += "</tr>";
        temp += "<tr>";
        temp += "<td class='SBViewPortBorderTD'>";
        temp += "</td>";
        temp += "<td class='SBViewPortContentTD'>";
        temp += "<!-- Begin Table Viewport Area -->";
        temp += "<table class='GenViewPortTable'>";
        temp += "<tr><td>";
        temp += "<table class='cartTable'  style='width:560px;'>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- Header Row -->";
        temp += "<tr><td class='ordHistoryHeader' style='width:345px;' >Company</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryHeader' style='width:30px;' >Logins</td>";
        temp += "<td class='cartColSpacer'></td>"; 
        temp += "<td class='ordHistoryHeader' style='width:30px;' >Hits</td>";
        temp += "<td class='cartColSpacer'></td>"; 
        temp += "<td class='ordHistoryHeader' style='width:80px;' >Last Date</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryHeader' style='width:70px;'>Details</td>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";

        int x;
        bool blnMatchFound = false;
        int AdminCompanyKeysCount = -1;
        //------ build an object array of all sales orders and dates
        string[] AdminCompanyKeys = new string[1000];
        if (dtAdminUsers != null)
        {
            foreach (DataRow myRow in dtAdminUsers.Rows)
            {
                blnMatchFound = false;
                for (x = 0; x <= AdminCompanyKeysCount; x++)
                {
                    if (myRow["QuantumCompanyCode"].ToString() == AdminCompanyKeys[x])
                    {
                        blnMatchFound = true;
                        break;
                    }
                }
                if (!blnMatchFound && myRow["QuantumCompanyCode"].ToString().Trim() != String.Empty)
                {
                    AdminCompanyKeys[++AdminCompanyKeysCount] = myRow["QuantumCompanyCode"].ToString();
                }
            }
        }


        //------- Place the Pending Users

        temp += AddCompanyUsersPod("new");
        for (x = 0; x <= AdminCompanyKeysCount; x++)
        {
            temp += AddCompanyUsersPod(AdminCompanyKeys[x]);
        }
        if (Page.Session["AccountType"].ToString().ToLower() == "admin")
        {
            temp += AddCompanyUsersPod("nocompany");
            temp += AddCompanyUsersPod("denied");
        }
       
        temp += "</td>";
        temp += "</tr>";
        temp += "</table>";
        temp += "</td>";
        temp += "</tr>";
        temp += "</table>";
        temp += "<!-- End  Table Viewport Area  -->";
        temp += "</td>";
        temp += "<td class='SBViewPortBorderTD'>";
        temp += "</td>";
        temp += "</tr>";
        temp += "<tr><td class='SBViewPortFooterTD' colspan='3'>";
        //temp += "<asp:Label runat="server" ID = "lblOrderHxSearchError" />
        temp += "</td>";
        temp += "</tr>";
        temp += "</table>";


        return temp;
    }

    protected string AddCompanyUsersPod(string thisCompany)
    {
        string temp = string.Empty;

        // new  
        // denied  
        // guest
        // admin - user - jpe
        bool blnMatchFound = false;
        bool includeThisOne = false;
        bool tableOpened = false;
        string strThisTitle = string.Empty;
        bool firstPlaced = false;
        string thisLink = string.Empty;
        string thisBarCSS = "AdminUsersDataLeft";
        string companyHeader = string.Empty;
        int coTotalLogins = 0;
        int coTotalHits = 0;
        DateTime coLastDate = Convert.ToDateTime("1/1/2000");

        foreach (DataRow myRow in dtAdminUsers.Rows)
        {
            switch (thisCompany)
            {
                case "new":
                    {
                        strThisTitle = "New Users Requests";
                        thisBarCSS = "AdminUsersDataLeftNew";
                        if (myRow["AccountType"].ToString().ToLower() == "new")
                        {
                            blnMatchFound = true;
                        }
                    break;
                    }
                case "denied":
                    {
                        strThisTitle = "Denied Account Requests";
                        thisBarCSS = "AdminUsersDataLeftOther";
                        if (myRow["AccountType"].ToString().ToLower() == "closed")
                        {
                            blnMatchFound = true; 
                        }
                        break;
                    }
                case "nocompany":
                    {
                        strThisTitle = "Guests with No Company";
                        thisBarCSS = "AdminUsersDataLeftOther";
                        if (myRow["QuantumCompanyCode"].ToString().ToLower() == string.Empty && myRow["AccountType"].ToString().ToLower() != "closed" && myRow["AccountType"].ToString().ToLower() != "new")
                        {
                            blnMatchFound = true;
                        }
                        break;
                    }
                default:
                    {
                    blnMatchFound = true;
                    if (myRow["QuantumCompanyCode"].ToString().ToLower() == thisCompany)
                    {
                        if (myRow["Company"].ToString().Length > strThisTitle.Length)
                        {
                            strThisTitle = myRow["Company"].ToString();
                        }
                    }
                    break;
                }
            }
        }

        if (blnMatchFound)
        {
            companyHeader += "<!-- Cart Row -->";
            companyHeader += "<tr><td class='" + thisBarCSS + "'  style='width:345px;'>" + strThisTitle + "</td>";
            companyHeader += "<td class='cartColSpacer'></td>";
            companyHeader += "<td class='" + thisBarCSS + "'  style='width:30px;'>PUT_LOGINS_HERE</td>";
            companyHeader += "<td class='cartColSpacer'></td>";
            companyHeader += "<td class='" + thisBarCSS + "'  style='width:30px;'>PUT_HITS_HERE</td>";
            companyHeader += "<td class='cartColSpacer'></td>";
            companyHeader += "<td class='" + thisBarCSS + "'  style='width:80px;'>PUT_LAST_DATE_HERE</td>";
            companyHeader += "<td class='cartColSpacer'></td>";
            companyHeader += "<td class='" + thisBarCSS + "'><a name='' onclick=" + (char)34 + "javascript:switchVue('adminUsersRowL" + ++AdminUsersPanelCounter + "');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + " onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ")><label id='adminUsersRowL" + AdminUsersPanelCounter + "_Label'>Show</label></td>";
            companyHeader += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";


            foreach (DataRow myRow in dtAdminUsers.Rows)
            {
                includeThisOne = false;
                switch (thisCompany)
                {
                    case "new":
                        {
                            if (myRow["AccountType"].ToString().ToLower() == "new")
                            {
                                includeThisOne = true;
                            }
                            break;
                        }
                    case "denied":
                        {
                            if (myRow["AccountType"].ToString().ToLower() == "closed")
                            {
                                includeThisOne = true;
                            }
                            break;
                        }
                    case "nocompany":
                        {
                            if (myRow["QuantumCompanyCode"].ToString().ToLower() == string.Empty && myRow["AccountType"].ToString().ToLower() != "closed" && myRow["AccountType"].ToString().ToLower() != "new")
                            {
                                includeThisOne = true;
                            }
                            break;
                        }
                    default:
                        {
                            if (myRow["QuantumCompanyCode"].ToString().ToLower() == thisCompany)
                            {
                                includeThisOne = true;
                            }
                            break;
                        }
                }


                if (includeThisOne)
                {
                    if (!tableOpened)
                    {
                        temp += "<!-- -------- Users for Company Detail ------------- -->";
                        temp += "<tr class='WSordDetailsTR' id='adminUsersRowL" + AdminUsersPanelCounter + "' style='display:none;background-color:#ffffff;'><td colspan='9' >";
                        temp += "<div class='box'><b class='tc'><b class='L1'></b><b class='L2'></b><b class='L3'></b></b><div class='content'>";
                        temp += "<table class='WSordDetailsTable' style='width:98%;' cellpadding='2' cellspacing='2'>";
                        temp += "<tr><td class='WSordDetailsHeaders'>Name</td>";
                        temp += "<td class='WSordDetailsHeaders'>Email</td>";
                        temp += "<td class='WSordDetailsHeaders'>Type</td>";
                        temp += "<td class='WSordDetailsHeaders'>Log</td>";
                        temp += "<td class='WSordDetailsHeaders'>Hits</td>";
                        temp += "<td class='WSordDetailsHeaders'>Last Date</td>";
                        if (Page.Session["AccountType"].ToString().ToLower() == "admin")
                        {
                            temp += "<td class='WSordDetailsHeaders'>Link</td></tr>";
                        }
                        else
                        {
                            temp += "<td class='WSordDetailsHeaders'>&nbsp;</td></tr>";
                        }
                        tableOpened = true;
                    }

                    if (firstPlaced)
                    {
                        temp += "<tr><td colspan='9' class='WSordDetailRowSpacer'></td>";
                        temp += "</tr>";
                    }
                    else
                    {
                        firstPlaced = true;
                    }

                    temp += "<tr><td class='AdminUsersDetailsData'  style='text-align:left;white-space:nowrap;'>" + myRow["FirstName"] + "</td>";
                    temp += "<td class='AdminUsersDetailsData' ><a href='mailto:" + myRow["EmailAddress"] + "'>" + myRow["EmailAddress"]  + "</a></td>";
                    temp += "<td class='AdminUsersDetailsData' >" + myRow["AccountType"] + "</td>";
                    temp += "<td class='AdminUsersDetailsData' >" + myRow["Logins"].ToString() + "</td>";
                    temp += "<td class='AdminUsersDetailsData' >" + myRow["Accumulator"].ToString() + "</td>";
                    temp += "<td class='AdminUsersDetailsData' >" + String.Format("{0:MM/dd/yyyy}", myRow["AttemptDateTime"]) + "</td>";

                    if (myRow["logins"] != DBNull.Value)
                    {
                        coTotalLogins += (int)myRow["Logins"];
                    }

                    if (myRow["Accumulator"] != DBNull.Value)
                    {
                        coTotalHits += (int)myRow["Accumulator"];
                    }
                    
                    if (myRow["AttemptDateTime"] != DBNull.Value)
                    {
                    if (DateTime.Compare((DateTime)myRow["AttemptDateTime"],coLastDate) > 0)
                        {
                            coLastDate = (DateTime)myRow["AttemptDateTime"];
                        }
                    }

                    thisLink = "&nbsp;";
                    if (Page.Session["AccountType"].ToString().ToLower() == "admin")
                    {
                        thisLink = "  <a name='' onclick=" + (char)34 + "javascript:ShowMyAccountModalPopupEvt('" + Page.Session["AccountType"].ToString() + "|" + myRow["pkAccountKey"] + "|ADMINGRIDVIEW|||');" + (char)34 + "   onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Edit</a>";
                    }
                    temp += "<td class='AdminUsersDetailsData' >" + thisLink + "</td>";
                    temp += "</tr>";
                }
            }
            temp += "</table>";
            temp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";
            temp += "</td></tr>";
            temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
            temp += "<!-- --------------- Order detail div  ------- -->";
        }

        if (!blnMatchFound)
        {
            return string.Empty;
        }
        else
        {
            companyHeader = companyHeader.Replace("PUT_LOGINS_HERE", coTotalLogins.ToString());
            companyHeader = companyHeader.Replace("PUT_HITS_HERE", coTotalHits.ToString());
            if (coLastDate != Convert.ToDateTime("1/1/2000"))
            {
                companyHeader = companyHeader.Replace("PUT_LAST_DATE_HERE", String.Format("{0:MM/dd/yyyy}", coLastDate));
            }
            else
            {
                companyHeader = companyHeader.Replace("PUT_LAST_DATE_HERE", string.Empty);
            }
            return companyHeader + temp;
        }
    }


    protected string BuildCurrentCompanyUsagePanel(DataTable dtCompanyUsageStats)
    {

        lblCoTotals.Text = string.Empty;
        string temp = string.Empty;
        bool blnValidHistory = true;
 
        if (dtCompanyUsageStats == null)
        {
            blnValidHistory = false;
        }
        else if (dtCompanyUsageStats.Rows.Count < 1)
        {
            blnValidHistory = false;
        }

        if (!blnValidHistory)
        {
            return string.Empty;
        }

        string strCurUser = string.Empty;  //----dtCompanyUsageStats.Rows[0]["UserName"].ToString();
        bool blnFirstPlaced = false;
        int coTotalLogins = 0;
        int coTotalHits = 0;
        DateTime coLastLogin = Convert.ToDateTime("1/1/2000");
        string thisEmail = string.Empty;
        string thisDate = string.Empty;

        temp = "<table border='1' style='width:250px;'>";
        temp += "<tr><td class='custUsageHeader'>User eMail</td><td class='custUsageHeader'>Logins</td><td class='custUsageHeader'>Hits</td><td class='custUsageHeader'>Last Login</td><td class='custUsageHeader'>Search History</td></tr>";
        foreach (DataRow myRow in dtCompanyUsageStats.Rows)
            {
                if (strCurUser != myRow["UserName"].ToString())
                {
                   if (blnFirstPlaced)
                   {
                        temp += "</select> </td></tr>";
                   }
                   strCurUser = myRow["UserName"].ToString();
                   thisEmail = myRow["UserName"].ToString().Replace( "@", " @ ");
                   thisEmail = thisEmail.Replace(".", " . ");
                   thisDate = String.Format("{0:MM/dd/yyyy}", (DateTime)myRow["AttemptDateTime"]);
                   temp += " <tr><td class='custUsageDataLeft' style='width:150px;'>" + thisEmail + "</td><td  class='custUsageDataLeft'>" + myRow["Logins"] + "</td><td  class='custUsageDataLeft'>" + myRow["Accumulator"] + "</td><td  class='custUsageDataLeft'  style='width:98%;' >" + thisDate + "</td><td>";
                   temp += "<select size='5'  class='custUsageDataLeft' style='width:100px;'>";
                   coTotalHits += (int)myRow["Accumulator"];
                   coTotalLogins += (int)myRow["Logins"];
                   if (DateTime.Compare(coLastLogin, (DateTime)myRow["AttemptDateTime"]) < 0)
                   {
                       coLastLogin = (DateTime)myRow["AttemptDateTime"];
                   }
                }
                //---- add the current search word
                temp += "<option>" + myRow["SearchWord"] + "</option>";
            }
        //------ close the last row
        temp += "</select> </td></tr>";
        temp += "</table>";
        lblCoTotals.Text = "<div style='text-align:left;color:#999999;font-family:Verdana;font-size:10px;'>";
        lblCoTotals.Text += "<b>Total Logins: </b>" + coTotalLogins.ToString() + "<br />";
        lblCoTotals.Text += "<b>Total Page Hits: </b>" + coTotalHits.ToString() + "<br />";
        lblCoTotals.Text += "<b>Last Login: </b>" + String.Format("{0:MM/dd/yyyy}", coLastLogin) + "<br />";
        lblCoTotals.Text += "</div>";

        return temp;
    }


    protected String PlaceWebsiteOrder(DateTime nextDateToPlace, DateTime LastWebsiteDatePlaced, DataTable dt, int plcdLp, string[,] SORefNoOrders, int SORefNoCounter)
    {

        String temp = string.Empty;
        bool blnMatchFound = false;
        int lp;

        foreach (DataRow myRow in dt.Rows)
        {
            if (DateTime.Compare((DateTime)myRow["DateTime"], LastWebsiteDatePlaced) < 0 && DateTime.Compare((DateTime)myRow["DateTime"], nextDateToPlace) > 0)
            {
                blnMatchFound = false;
                for (lp = 0; lp <= SORefNoCounter; lp++)
                {
                    if (myRow["PONumber"].ToString().Trim().ToLower() == SORefNoOrders[lp, 0].Trim().ToLower())
                    {
                        blnMatchFound = true;
                        break;
                    }
                }
                if (!blnMatchFound)
                {
                    temp += AddWebsiteOrder(myRow, plcdLp);
                }
            }
        }
        return temp;
    }


    protected string AddWebsiteOrder(DataRow myRow, int plcLp)
    {
        string temp = string.Empty;
        string thisPartLine = myRow["OrderDetails"].ToString();
        if (thisPartLine.Trim() == string.Empty)
        {
            return string.Empty;
        }

        string curPartNumber = string.Empty;
        string WSCompanyReferenceNumber = string.Empty;
        Decimal WSTotalPrice = 0m;
        string WSEntryDate = string.Empty;

        WSCompanyReferenceNumber = myRow["PONumber"].ToString();
        WSTotalPrice = decimal.Parse(myRow["OrderTotal"].ToString());
        WSEntryDate = string.Format("{0:MM/dd/yyyy}", myRow["DateTime"]);

        temp += "<!-- Cart Row -->";
        temp += "<tr><td class='WSordHistoryDataLeft'>" + WSEntryDate + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='WSordHistoryDataLeft'>" + WSCompanyReferenceNumber + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='WSordHistoryDataRight'>" + String.Format("{0:C}", WSTotalPrice) + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='WSordHistoryDataLeft'>Website order received</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='WSordHistoryDataLeft'><a name='' onclick=" + (char)34 + "javascript:switchVue('orderDetailRowL" + ++OrderHistoryRowCounter + "');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + " onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ")><label id='orderDetailRowL" + OrderHistoryRowCounter + "_Label'>Show</label></td>";

        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- -------- Order Detail ------------- -->";
        temp += "<tr class='WSordDetailsTR' id='orderDetailRowL" + OrderHistoryRowCounter + "' style='display:none;background-color:#ffffff;'><td colspan='9' >";
        temp += "<div class='box'><b class='tc'><b class='L1'></b><b class='L2'></b><b class='L3'></b></b><div class='content'>";
        temp += "<table class='WSordDetailsTable' style='width:98%;' cellpadding='2' cellspacing='2'>";
        temp += "<tr><td class='WSordDetailsHeaders'>Part Number</td>";
        temp += "<td class='WSordDetailsHeaders'>Description</td>";
        temp += "<td class='WSordDetailsHeaders'>Qty</td>";
        temp += "<td class='WSordDetailsHeaders'>Avl</td>";
        temp += "<td class='WSordDetailsHeaders'>BO</td>";
        temp += "<td class='WSordDetailsHeaders'>Price</td>";
        temp += "<td class='WSordDetailsHeaders'>Extend</td></tr>";
       
        bool firstPlaced = false;
 
        if (thisPartLine.Trim() != string.Empty)
        {
            string[] PartLines = thisPartLine.Split('~');

            foreach (string s in PartLines)
            {
                if (s != string.Empty)
                {
                string[] lineDetail = s.Split('|');
                if (firstPlaced)
                {
                    temp += "<tr><td colspan='7' class='WSordDetailRowSpacer'></td>";
                    temp += "</tr>";
                }
                else
                {
                    firstPlaced = true;
                }

                temp += "<tr><td class='WSordDetailsData'  style='text-align:left;white-space:nowrap;'>" + lineDetail[0] + "</td>";
                temp += "<td class='WSordDetailsDataLeft' >" + string.Format("{0:#####}", lineDetail[1]) + "</td>";
                temp += "<td class='WSordDetailsData' >" + string.Format("{0:#####}", lineDetail[2]) + "</td>";
                temp += "<td class='WSordDetailsData' >" + string.Format("{0:#####}", lineDetail[3]) + "</td>";
                temp += "<td class='WSordDetailsData'>" + string.Format("{0:#####}", lineDetail[4]) + "</td>";
                temp += "<td class='WSordDetailsDataRight' >" + string.Format("{0:C}", Decimal.Parse(lineDetail[5])) + "</td>";
                temp += "<td class='WSordDetailsDataRight' >" + string.Format("{0:C}", Decimal.Parse(lineDetail[6])) + "</td>";
                temp += "</tr>";
            }
            }
        }

        temp += "</table>";
        temp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";
        temp += "</td></tr>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- --------------- Order detail div  ------- -->";

        return temp;
    }


    protected string AddRFQLine(string thisRFQNumber, DataTable dtRFQHistory, int SORowNo, int plcdLp)
    {
        string temp = string.Empty;
        int PartsArrayCounter = -1;
        string curPartNumber = string.Empty;
        string QuoteValidDate = string.Empty;
        decimal TlqtyQuoted = 0;
        string RFQCompanyReferenceNumber = string.Empty;
        Decimal RFQTotalCost = 0m;
        string RFQEntryDate = string.Empty;
        string RFQExpirationDate = string.Empty;
        string strQuoteValidDate = string.Empty;
        string QuoteStatus = string.Empty;
        //------ build an object array of all sales orders and dates
        object[,] PartsforRFQ = new object[200, 4];
        int rowCounter = -1;
        foreach (DataRow myRow in dtRFQHistory.Rows)
        {
            rowCounter++;
            if (thisRFQNumber == myRow["CQH_AUTO_KEY"].ToString())
            {
                RFQCompanyReferenceNumber = myRow["COMPANY_REF_NUMBER"].ToString();
                RFQTotalCost = decimal.Parse(myRow["TOTAL_PRICE"].ToString());
                RFQEntryDate = string.Format("{0:MM/dd/yyyy}", myRow["ENTRY_DATE"]);
                QuoteValidDate = string.Format("{0:MM/dd/yyyy}",  myRow["EXPIRATION_DATE"]);
                QuoteStatus = myRow["DESCRIPTION"].ToString();
                if (curPartNumber != myRow["PN"].ToString())
                {
                    PartsforRFQ[++PartsArrayCounter, 0] = rowCounter;
                    PartsforRFQ[PartsArrayCounter, 1] = 1;  /// rows
                    PartsforRFQ[PartsArrayCounter, 2] = decimal.Parse(myRow["QTY_QUOTED"].ToString());
                    PartsforRFQ[PartsArrayCounter, 3] = myRow["EXPIRATION_DATE"].ToString();
                    curPartNumber = myRow["PN"].ToString();
                    TlqtyQuoted = decimal.Parse(myRow["QTY_QUOTED"].ToString());
                }
                else
                {
                    PartsforRFQ[PartsArrayCounter, 1] = (int)PartsforRFQ[PartsArrayCounter, 1] + 1;
                    PartsforRFQ[PartsArrayCounter, 2] = (decimal)PartsforRFQ[PartsArrayCounter, 2] + decimal.Parse(myRow["QTY_QUOTED"].ToString());
                    TlqtyQuoted = TlqtyQuoted + decimal.Parse(myRow["QTY_QUOTED"].ToString());
                }
            }
        }

        int lp;
        
        strQuoteValidDate = "Quote Valid through " + QuoteValidDate;
        temp += "<!-- Cart Row -->";
        temp += "<tr><td class='rfqHistoryDataLeft'>" + RFQEntryDate + "</td>";
        temp += "<td class='cartColSpacer'></td><td class='rfqHistoryDataLeft'>" + RFQCompanyReferenceNumber + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='rfqHistoryDataRight'>" + String.Format("{0:C}", RFQTotalCost) + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='rfqValidDateBar'>" + QuoteStatus + "</td>";      
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='rfqHistoryDataLeft'><a name='' onclick=" + (char)34 + "javascript:switchVue('orderDetailRowL" + ++OrderHistoryRowCounter + "');" + (char)34 + ") onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + " onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + " ><label id='orderDetailRowL" + OrderHistoryRowCounter + "_Label'>Show</label>";
        temp += "</td></tr>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- -------- Order Detail ------------- -->";
        temp += "<tr class='ordDetailsTR' id='orderDetailRowL" + OrderHistoryRowCounter + "' style='display:none;background-color:#ffffff; width:98%;'><td colspan='9' >";
        temp += "<div class='box'><b class='tc'><b class='L1'></b><b class='L2'></b><b class='L3'></b></b><div class='content'>";
        temp += "<table class='ordDetailsTable' style='width:98%;'>";
        temp += "<tr><td class='rfqDetailsHeaders'>Part Number</td>";
        temp += "<td class='rfqDetailsHeaders' style='width:60%;'>Description</td>";
        temp += "<td class='rfqDetailsHeaders'>Qty</td>";
        temp += "<td class='rfqDetailsHeaders'>Unit</td>";
        temp += "<td class='rfqDetailsHeaders'>Ext</td>";
        temp += "</tr>";

        bool firstPlaced = false;
        decimal unitPrice = 0m;
        decimal Extension = 0m;
        int curDtRow = -1;
        decimal qtyQuoted = -1;
  
        for (lp = 0; lp <= PartsArrayCounter; lp++)
        {
            if (firstPlaced)
            {
                temp += "<tr><td colspan='7' class='ordDetailRowSpacer'></td>";
                temp += "</tr>";
            }
            else
            {
                firstPlaced = true;
            }

            curDtRow = int.Parse(PartsforRFQ[lp, 0].ToString());
            qtyQuoted = decimal.Parse(PartsforRFQ[lp, 2].ToString());
            unitPrice = decimal.Parse(dtRFQHistory.Rows[curDtRow]["CUSTOMER_PRICE"].ToString());
            Extension = unitPrice * qtyQuoted;
           
            temp += "<tr><td class='rfqDetailsData' style='text-align:left;white-space:nowrap;'>" + dtRFQHistory.Rows[curDtRow]["PN"] + "</td>";
            temp += "<td class='rfqDetailsData'  style='white-space:nowrap; text-align:left;'>" + dtRFQHistory.Rows[curDtRow]["PDESCRIPTION"] + "</td>";
            temp += "<td class='rfqDetailsData'>" + string.Format("{0:#####}", qtyQuoted) + "</td>";
            temp += "<td class='rfqDetailsData'>" + string.Format("{0:c}", unitPrice) +"</td>";
            temp += "<td class='rfqDetailsData'>" + string.Format("{0:c}", Extension) + "</td>";
            temp += "</tr>";

            if (QuoteValidDate != string.Empty)
            {
                temp += "<tr><td class='rfqValidDateBar' colspan = '6'>" + strQuoteValidDate + "</td>";
                temp += "</tr>";
            }
        }

        temp += "</table>";
        temp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";
        temp += "</td></tr>";
        temp += "<tr>";
        temp += "<td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- --------------- RFQ div  ------- -->";

        return temp;
    }

    protected string AddSalesOrder(string thisSalesOrderNumber, DataTable dtOrderHistory, int SORowNo, int plcdLp)
    {
        string temp = string.Empty;
        int PartsArrayCounter = - 1;
        int InvoiceArrayCount = -1;
        string curPartNumber = string.Empty;
        bool blnMatchFound = false;
        decimal TlqtyOrdered = 0m;
        decimal TlqtyInvoiced = 0m;
        string SOCompanyReferenceNumber = string.Empty;
        Decimal SOTotalPrice = 0m;
        string SOEntryDate = string.Empty;
        string buildInvoiceNumber = string.Empty;
        bool blnNonInvoiceItems = false;
        int slp;
        string strIND_AUTO_KEY = string.Empty;
        //------ build an object array of all sales orders and dates
        object[,] PartsforSO = new object[200, 6];
        string[] InvoicesForSO = new string[200];
        int rowCounter = -1;
        foreach (DataRow myRow in dtOrderHistory.Rows)
        {
            rowCounter++;
          if (thisSalesOrderNumber ==  myRow["SOH_AUTO_KEY"].ToString())
          {
              SOCompanyReferenceNumber = myRow["COMPANY_REF_NUMBER"].ToString();
              SOTotalPrice = decimal.Parse(myRow["TOTAL_PRICE"].ToString());
              SOEntryDate = string.Format("{0:MM/dd/yyyy}", myRow["ENTRY_DATE"]);

              //-------- see add the invoice Number to the
              blnMatchFound = false;
              string logThisInvoice = string.Empty;
              if (myRow["INVC_NUMBER"].ToString() == string.Empty)

              {
                  logThisInvoice = "";
                  blnNonInvoiceItems = true;
              }
              else
              {
                  logThisInvoice = myRow["INVC_NUMBER"].ToString();
              }
              for (slp = 0; slp <= InvoiceArrayCount; slp++)
              {
                  if (InvoicesForSO[slp] == logThisInvoice)
                  {
                      blnMatchFound = true;
                      break;
                  }
              }
              if (!blnMatchFound && logThisInvoice != String.Empty)
              {
                  InvoicesForSO[++InvoiceArrayCount] = logThisInvoice;
              }

              if (curPartNumber != myRow["PN"].ToString())
              {
                  PartsforSO[++PartsArrayCounter, 0] = rowCounter;
                  PartsforSO[PartsArrayCounter, 1] = 1;  /// rows
                  PartsforSO[PartsArrayCounter, 2] = Convert.ToDecimal(myRow["QTY_ORDERED"].ToString());
                  PartsforSO[PartsArrayCounter, 3] = Convert.ToDecimal(myRow["QTY_INVOICED"].ToString());
                  PartsforSO[PartsArrayCounter, 4] = myRow["INVC_NUMBER"].ToString();
                  PartsforSO[PartsArrayCounter, 5] = myRow["STC_AUTO_KEY"].ToString();
                  curPartNumber = myRow["PN"].ToString();
                  strIND_AUTO_KEY = myRow["IND_AUTO_KEY"].ToString();
                  TlqtyOrdered = Convert.ToDecimal(myRow["QTY_ORDERED"].ToString());
                  TlqtyInvoiced = Convert.ToDecimal(myRow["QTY_INVOICED"].ToString());
              }
            else if (strIND_AUTO_KEY != myRow["IND_AUTO_KEY"].ToString())
            {
            PartsforSO[PartsArrayCounter,1] = (int)PartsforSO[PartsArrayCounter,1] + 1;  
            PartsforSO[PartsArrayCounter,2] = (decimal)PartsforSO[PartsArrayCounter,2] + Convert.ToDecimal(myRow["QTY_ORDERED"].ToString());
            PartsforSO[PartsArrayCounter,3] = (decimal)PartsforSO[PartsArrayCounter,3] + Convert.ToDecimal(myRow["QTY_INVOICED"].ToString());
            TlqtyOrdered = TlqtyOrdered + Convert.ToDecimal(myRow["QTY_ORDERED"].ToString());
            TlqtyInvoiced = TlqtyInvoiced + Convert.ToDecimal(myRow["QTY_INVOICED"].ToString());
            }
          }
        }

        int lp;
        string orderStatus = "Open";
        if (TlqtyInvoiced == TlqtyOrdered)
        {
            orderStatus = "Shipped";
        }
        else if (TlqtyInvoiced > 0)
        {
            orderStatus = "Partially Shipped";
        }
        //----- Place the Sales Order Row
        temp += "<!-- Cart Row -->";
        temp += "<tr><td class='ordHistoryDataLeft'>" + SOEntryDate + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryDataLeft'>" + SOCompanyReferenceNumber + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryDataRight'>" + String.Format("{0:C}", SOTotalPrice) + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryDataLeft'>" + orderStatus + "</td>";
        temp += "<td class='cartColSpacer'></td>";
        temp += "<td class='ordHistoryDataLeft'><a name='' onclick=" + (char)34 + "javascript:switchVue('orderDetailRowL" + ++OrderHistoryRowCounter + "');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + " onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ")><label id='orderDetailRowL" + OrderHistoryRowCounter + "_Label'>Show</label></td>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- -------- Order Detail ------------- -->";
        temp += "<tr class='ordDetailsTR' ID='orderDetailRowL" + OrderHistoryRowCounter + "' style='display:none;background-color:#ffffff;'><td colspan='9' >";


        if (blnNonInvoiceItems)
        {
            temp += buildSalesOrderPod(PartsArrayCounter, PartsforSO, string.Empty);
        }

        for (lp = 0; lp <= InvoiceArrayCount; lp++)
        {
            temp += buildSalesOrderPod(PartsArrayCounter, PartsforSO, InvoicesForSO[lp]);
        }

        temp += "</td></tr>";
        temp += "<tr><td colspan='9' class='cartRowSpacer'></td></tr>";
        temp += "<!-- --------------- Order detail div  ------- -->";

        return temp;

    }

    protected string buildSalesOrderPod(int PartsArrayCounter, object[,] PartsforSO, string showInvoiceNumber)
    {

        string temp = string.Empty;
        int lp;
        temp += "<div class='box'><b class='tc'><b class='L1'></b><b class='L2'></b><b class='L3'></b></b><div class='content'>";
        temp += "<table class='ordDetailsTable' style='width:98%;' cellpadding='2' cellspacing='2'>";
        temp += "<tr><td class='ordDetailsHeaders'>Part Number</td>";
        temp += "<td class='ordDetailsHeaders'>Order</td>";
        temp += "<td class='ordDetailsHeaders'>Pending</td>";
        temp += "<td class='ordDetailsHeaders'>Shipped</td>";
        temp += "<td class='ordDetailsHeaders'>Date</td>";
        temp += "<td class='ordDetailsHeaders'>Invoice</td>";
        temp += "<td class='ordDetailsHeaders'>Carrier</td>";
        temp += "<td class='ordDetailsHeaders'>Tracking Link</td></tr>";

        bool firstPlaced = false;
        int rowsthisPart = 0;
        int curDtRow = -1;
        decimal qtyOrdered = -1m;
        decimal qtyShipped = -1;
        string thisShippedDate = string.Empty;
        string thisCarrier = string.Empty;
        string thisTrackingNumber = string.Empty;
        string thisTrackingLink = string.Empty;
        string NextShipDate = String.Empty;
        string LastTrackingLink = string.Empty;
        string thisInvoiceNumber = string.Empty;
        string LastInvoiceNumber = string.Empty;
        string strPrintThisInvoice = string.Empty;
        decimal decqtyBO;
        int thisSTCKey = 0;

        for (lp = 0; lp <= PartsArrayCounter; lp++)
        {
            thisInvoiceNumber = PartsforSO[lp, 4].ToString();
            if (thisInvoiceNumber == showInvoiceNumber)
            {
                if (firstPlaced)
                {
                    temp += "<tr><td colspan='8' class='ordDetailRowSpacer'></td>";
                    temp += "</tr>";
                }
                else
                {
                    firstPlaced = true;
                }

                rowsthisPart = int.Parse(PartsforSO[lp, 1].ToString());
                curDtRow = int.Parse(PartsforSO[lp, 0].ToString());
                qtyShipped = decimal.Parse(PartsforSO[lp, 3].ToString());
                qtyOrdered = decimal.Parse(PartsforSO[lp, 2].ToString());
                thisSTCKey = int.Parse(PartsforSO[lp, 5].ToString());
                thisShippedDate = string.Format("{0:MM/dd/yyyy}", dtOrderHistory.Rows[curDtRow]["SHIP_DATE"]);
                thisTrackingNumber = dtOrderHistory.Rows[curDtRow]["AIRWAY_BILL"].ToString();
                thisCarrier = string.Empty;
                if (thisTrackingNumber != null)
                {
                    if (thisTrackingNumber.Length > 5)
                    {
                        if (thisTrackingNumber.Length > 18)
                        {
                            thisCarrier = "USPS";
                        }
                        else if (thisTrackingNumber.Length == 18)
                        {
                            thisCarrier = "UPS";
                        }
                        else if (thisTrackingNumber.Length == 14)
                        {
                            thisCarrier = "DHL";
                        }
                        else if (thisTrackingNumber.Length == 12)
                        {
                            thisCarrier = "Fedex";
                        }
                    }
                }
                NextShipDate = string.Empty;
                //--------------- do we have next delivery information?
                if ((qtyOrdered - qtyShipped) > 0)
                {
                    NextShipDate = "Roll your mouse over the 'Pending' number for information on back-ordered items";
                    rowsthisPart++;
                }
                string closeA = string.Empty;

                switch (thisCarrier)
                {
                    case "UPS":
                        thisTrackingLink = "<a href='http://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=3D=status&tracknums_displayed=3D1&TypeOfInquiryNumber=3DT&loc=3D=en_US&InquiryNumber1=" + thisTrackingNumber + "&track.x=3D0&track.y==3D0'  target = '_new'>";
                        closeA = "</a>";
                        break;
                    case "Fedex":
                        thisTrackingLink = "<a href='http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&tracknumbers=" + thisTrackingNumber + "' target = '_new'>";
                        closeA = "</a>";
                        break;
                    case "DHL":
                        thisTrackingLink = "<a href='http://track.dhl-usa.com/TrackByNbr.asp?ShipmentNumber=" + thisTrackingNumber + "' target = '_new'>";
                        closeA = "</a>";
                        break;
                    case "USPS":
                        thisTrackingLink = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?strOrigTrackNum=" + thisTrackingNumber + "' target = '_new'>";
                        closeA = "</a>";
                        break;
                    default:
                        thisTrackingLink = "Not Available";
                        break;
                }

                if (thisTrackingLink == LastTrackingLink)
                {
                    thisTrackingLink = string.Empty;
                    thisTrackingNumber = "<span style='color:#d3d3d3'>same as above</span>";
                }
                else
                {
                    LastTrackingLink = thisTrackingLink;
                }
                if (thisInvoiceNumber == LastInvoiceNumber)
                {
                    strPrintThisInvoice = "<span style='color:#d3d3d3'>same</span>"; ;
                }
                else
                {
                    strPrintThisInvoice = thisInvoiceNumber;
                    LastInvoiceNumber = thisInvoiceNumber;
                }

                rowsthisPart = 1;  // -- for multi row for one part number

                temp += "<tr><td class='ordDetailsData' rowspan = '" + rowsthisPart + "'  style='text-align:left;white-space:nowrap;'>" + dtOrderHistory.Rows[curDtRow]["PN"] + "</td>";

                temp += "<td class='ordDetailsData' >" + string.Format("{0:#####}", qtyOrdered) + "</td>";

                decqtyBO = qtyOrdered - qtyShipped;
                string strThisBo = "<" + string.Format("{0:#####}", decqtyBO);
                if (decqtyBO > 0)
                {
                    strThisBo = buildDeliveryDate("SO", dtOrderHistory.Rows[curDtRow]["SOD_AUTO_KEY"].ToString(), decqtyBO, dtOrderHistory.Rows[curDtRow]["PN"].ToString(), thisSTCKey);
                }

                temp += "<td class='ordDetailsData' " + strThisBo + "</td>";
                temp += "<td class='ordDetailsData'>" + string.Format("{0:#####}", qtyShipped) + "</td>";
                temp += "<td class='ordDetailsData'>" + thisShippedDate + "</td>";
                temp += "<td class='ordDetailsData'>" + strPrintThisInvoice + "</td>";
                temp += "<td class='ordDetailsData'>" + thisCarrier + "</td>";
                temp += "<td class='ordDetailsData'>" + thisTrackingLink + " " + thisTrackingNumber + closeA + "</td>";
                temp += "</tr>";
                if (NextShipDate != string.Empty)
                {
                    temp += "<tr><td class='ordNextDateBar' colspan = '11'>&nbsp;<img src='images/alert_icon.png' alt= '' border='0' />&nbsp;&nbsp;" + NextShipDate + "</td>";
                    temp += "</tr>";
                }
            }
        }

        temp += "</table>";
        temp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";

        return temp;

    }

    protected string buildDeliveryDate(string thisType, string thisKey, decimal decqtyBO, string thisPN, int stcKey)
    {

        DataTable dtBOPOs = null;
        DataTable dtBOWoos = null;
        string strThisBo = string.Format("{0:#####}", decqtyBO);
        string thisSearchPN = GenUtils.MakeUniformPartNumber(thisPN);
        string strThisDelList = string.Empty;

        if (stcKey == 2 || stcKey == 6 || stcKey == 12 || stcKey == 26) //------ woo
        {
            dtBOWoos = GenUtils.RetrieveBackOrderWoos(thisSearchPN);
            if (dtBOWoos != null)
            {
                if (dtBOWoos.Rows.Count > 0)
                {
                    if (dtBOWoos.Rows[0]["manual_ecd"] != DBNull.Value)
                    {
                        if (GenUtils.IsDate(dtBOWoos.Rows[0]["manual_ecd"].ToString()))
                        {
                            strThisDelList = buildShowDate((DateTime)dtBOWoos.Rows[0]["manual_ecd"]);
                        }
                    }
                }
            }
        }
        else if (stcKey == 3 || stcKey == 10 || stcKey == 20)
        {
            dtBOPOs = GenUtils.RetrieveBackOrderPOs(thisSearchPN);
            if (dtBOWoos != null)
            {
                if (dtBOPOs.Rows.Count > 0)
                {
                    if (dtBOPOs.Rows[0]["next_delivery_date"] != DBNull.Value)
                    {
                        if (GenUtils.IsDate(dtBOPOs.Rows[0]["next_delivery_date"].ToString()))
                        {
                        strThisDelList = buildShowDate((DateTime)dtBOPOs.Rows[0]["next_delivery_date"]);
                        }
                    }
                }
            }
        }

        //GenUtils.PrintTableOrView(dtBOWoos, "");


        if (strThisDelList != string.Empty)
        {
            strThisBo = " onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>We are currently back-ordered for " + string.Format("{0:#####}", decqtyBO) + " units of for part number " + thisPN + ".  Our internal system tracks the manufacturing process and we are currently expecting the following delivery schedule.  Please be aware that this information is subject to change.  This panel is automatically updated as we learn of schedule changes. <br><ul>" + strThisDelList + "</ul><br /><br /><b>If you need this part sooner, please call us. We can sometimes obtain parts sooner then the regularly scheduled date to meet increased or unanticipated demand.</div>');" + (char)34 + " >" + strThisBo + " ";
        }
        else //----- call your rep message
        {
            strThisBo = " onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>We are currently back-ordered for " + string.Format("{0:####}", decqtyBO) + " units of for part number " + thisPN + ".  Our automated back order tracking system was not able to retrieve data for this item.  Please call your Jet Parts Engineering Sales Rep for more info.</div>');" + (char)34 + " >" + strThisBo + "";
        }

        return strThisBo;
    }

    private string buildShowDate(DateTime thisDate)
    {
        string temp = string.Empty;
        thisDate = thisDate.AddDays(5);
        if (thisDate.Day < 11)
        {
            temp = "Early ";
        }
        else if (thisDate.Day < 21)
        {
            temp = "Mid ";
        }
        else
        {
            temp = "Late ";
        }

        temp += String.Format("{0:MMMM yyyy}", thisDate);
        return temp;
    }

    //****************************************** Build Smaller Panels

    protected string buildHelpPanel(string target)
    {
        string temp = string.Empty;
        DataTable dtHelp = null;
        string orderBy = string.Empty;
        DataSet dsHelp = new DataSet();
       
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        string queryString = "SELECT * FROM ecom_HelpText WHERE  (";

        switch (target)
        {
            case "HomePage":
                queryString += " displayOrderHomePage > 0 ";
                orderBy = " displayOrderHomePage ";
                break;
            case "modalHelp":
                queryString += " displayOrdermodalHelp > 0 ";
                orderBy = " displayOrdermodalHelp ";
                break;
            default:
                return string.Empty;
        }

        queryString += ") ORDER BY " + orderBy + ";";

        SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
        selectCMD.CommandTimeout = 30;
        SqlDataAdapter helpDA = new SqlDataAdapter();
        helpDA.SelectCommand = selectCMD;

        try
        {
            sqlConn.Open();
            helpDA.Fill(dsHelp, "Help");
            dtHelp = dsHelp.Tables["Help"];
        }

        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.WriteLine(ex.Message);
            return string.Empty;
        }
        finally
        {
            sqlConn.Close();
        }

        int lpCounter = -1;

        foreach (DataRow myHelpRow in dtHelp.Rows)
        {
            temp += " <p class='ecom_SubHeadline'><a name onclick=" + (char)34 + "javascript: return switchMenu('" + target + (++lpCounter).ToString() + "');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 +  " onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + myHelpRow["Headline"] + "&nbsp;<img src='images/Expand_Icon.jpg' border = '0' alt='Click to Expand Section.'  id='" + target + lpCounter.ToString() + "Img' /></a></p>";
            temp += "<div  id='" + target + lpCounter.ToString() + "' style='display:none;'>";
            temp += "<p class='ecom_BodyText'>" + myHelpRow["Body"] + "</p></div>";
        }

        if (target == "modalHelp" && Page.Session["AccountType"] != null)
        {
            if (Page.Session["AccountType"].ToString().ToLower() == "admin")
            {
                dsHelp.Clear();
                if (dtHelp != null)
                {
                    dtHelp.Clear();
                }
              
               queryString = "SELECT * FROM ecom_HelpText WHERE  (displayOrderAdminUG > 0) ORDER BY  displayOrderAdminUG;";

               SqlCommand selectCMD2 = new SqlCommand(queryString, sqlConn);
               selectCMD.CommandTimeout = 30;
               helpDA.SelectCommand = selectCMD2;

               try
               {
                   sqlConn.Open();
                   helpDA.Fill(dsHelp, "Help");
                   dtHelp = dsHelp.Tables["Help"];
               }

               catch (Exception ex)
               {
                   sqlConn.Close();
                   Debug.WriteLine(ex.Message);
                   return string.Empty;
               }
               finally
               {
                   sqlConn.Close();
               }

               temp += "<br /><br /><br /><p class='ecom_SubHeadline' style='color:#330000;'><img src='images/guide.gif' align='left' border='0' hspace='20'>Admin Users Guide Admins Only<br /><br /><br /></p>";

               foreach (DataRow myHelpRow in dtHelp.Rows)
               {
                   temp += " <p class='ecom_SubHeadline'><a name onclick=" + (char)34 + "javascript: return switchMenu('" + target + (++lpCounter).ToString() + "');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + " onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + myHelpRow["Headline"] + "&nbsp;<img src='images/Expand_Icon.jpg' border = '0' alt='Click to Expand Section.'  id='" + target + lpCounter.ToString() + "Img' /></a></p>";
                   temp += "<div  id='" + target + lpCounter.ToString() + "' style='display:none;'>";
                   temp += "<p class='ecom_BodyText'>" + myHelpRow["Body"] + "</p></div>";
               }
            }
        }
        return temp;
    }

    //************************************* Data Interactions and Event Logging

    protected void logSearchWordUsed(string[,] searchWords, string nhaSearch)
    {
        int lp;
        string conn = string.Empty;
        string wordsList = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        SqlConnection cacheQueueConn = new SqlConnection(conn);
        string queryString = string.Empty;
        wordsList = "<ul>";
        for (lp = 0; lp <= 999; lp++)
        {
            if (searchWords[lp,0] == null || searchWords[lp,0] == string.Empty)
            {
                break;
            }
            wordsList += "<li>" + searchWords[lp, 0];
            if (string.IsNullOrEmpty(searchWords[lp, 1]) != true)
            {
                if (searchWords[lp, 1] != "0")
                {
                    wordsList += " qty: " + searchWords[lp, 1];
                }
            }
            wordsList += "</li>";
            queryString = "INSERT INTO dbo.ecom_SearchQueue (fkUserKey, SearchWord, Quantity, DateTime, SourcePanel) VALUES (" + Page.Session["LoggedInUserID"] + ", '" + searchWords[lp, 0] + "', '" + searchWords[lp, 1] + "', '" + DateTime.Now + "', '" + searchWords[lp, 2]  + "');";
            SqlCommand cmd = new SqlCommand(queryString, cacheQueueConn);
            try
            {
                cacheQueueConn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Updating login attempt: " + ex.ToString());
            }
            finally
            {
                cacheQueueConn.Close();
            }
        }


        if (nhaSearch != string.Empty)
        {
            wordsList += "<li> NHA Search: " + nhaSearch + "</li>";
        }
        wordsList += "</ul>";
        //-------------------------------------------- Email the search to the salesperson
        if (blnEmailSearchWords && (Page.Session["AccountType"].ToString().ToLower() == "guest" ||  Page.Session["AccountType"].ToString().ToLower() == "user" ))
        {
            emailSearchWords(wordsList);
        }
    }


    protected void emailSearchWords(string wordList)
    {
        string conn = string.Empty;
        DataTable dtTemp = null;
        DataSet dsTemp = new DataSet();
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection stockQueueConn = new SqlConnection(conn);
        string queryString = string.Empty;
        queryString = "SELECT EmailAddress FROM dbo.ecom_UserAccounts WHERE salesPersonAutoKey  = " + Page.Session["salesPersonAutoKey"].ToString() + ";";
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

        string dbEmail = string.Empty;
        string emailBody = string.Empty;
        if (dtTemp != null)
        {
            if (dtTemp.Rows.Count > 0)
            {
                dbEmail = dtTemp.Rows[0]["EmailAddress"].ToString();
            }
        }
        if (dbEmail != string.Empty)
        {
            emailBody = "User: " +  Page.Session["RDX_CONTACT_NAME"] + "<br />";
            if (Page.Session["JobTitle"].ToString() != string.Empty)
            {
                emailBody += "Title: " + Page.Session["JobTitle"] + "<br />";
            }
            emailBody += "Company: " + Page.Session["Company"] + "<br /><br />";
            emailBody += "Searched for:";
            emailBody += wordList + "<br />";
            sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), dbEmail, "eCommerce Search Notice", emailBody, true, false, "");
        }
    }

    protected void savePartsDetailPanel()
    {
        Page.Validate("vgParts");
        ModalPopupExtenderPartsDetail.Hide();
        upPnlSmartPartsBuyer.Update();
    }

    protected void handleForgottenPassword()
    {

        string mlSendReult = string.Empty;
        string dbUserName = string.Empty;
        string dbFirstName = string.Empty;
        string dbLastName = string.Empty;
        string dbPW = string.Empty;
        string dbEmail = string.Empty;
        string emailBody = string.Empty;

        lblPWrecoveryMsg.Text = string.Empty;
        btnForgottenCancel.Text = "Cancel";
        btnRecoverPassword.Visible = true;

        DataSet dsPW;
        dsPW = new DataSet();

        if (txtForgotEmail.Text.Trim().ToLower() == string.Empty )
        {
            lblPWrecoveryMsg.Text = "Please enter your email address.";
        }
        else
        {
            string conn = string.Empty;
            conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conn);
            string queryString = "SELECT UserName, FirstName, LastName, Password, EmailAddress FROM ecom_UserAccounts WHERE ";
            
            queryString += " LOWER(EmailAddress) = '" + txtForgotEmail.Text.Trim().ToLower() + "' ";
            SqlCommand selectCMD = new SqlCommand(queryString, sqlConn);
            selectCMD.CommandTimeout = 30;

            SqlDataAdapter pwDA = new SqlDataAdapter();
            pwDA.SelectCommand = selectCMD;

            try
            {
                sqlConn.Open();
                pwDA.Fill(dsPW, "PW");
                if (dsPW.Tables["PW"].Rows.Count > 0)
                {
                    //dbUserName = dsPW.Tables["PW"].Rows[0]["UserName"].ToString();
                    dbPW = dsPW.Tables["PW"].Rows[0]["Password"].ToString();
                    dbEmail = dsPW.Tables["PW"].Rows[0]["EmailAddress"].ToString();
                    dbFirstName = dsPW.Tables["PW"].Rows[0]["FirstName"].ToString();
                    dbLastName = dsPW.Tables["PW"].Rows[0]["LastName"].ToString();
                }
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
            if (dsPW.Tables["PW"].Rows.Count > 0)
            {
                emailBody = "Hello " + dbFirstName + ";<br /><br />";
                emailBody += "Your Login ID is your email address: " + dbEmail + "<br />";
                emailBody += "Your Password: " + dbPW + "<br /><br /><br />";
                emailBody += dtGlobalAdmin.Rows[0]["JPEName"].ToString() + "<br />";
                emailBody += "Phone " + dtGlobalAdmin.Rows[0]["JPEPhone"].ToString() + "<br />";
                emailBody += "Fax " + dtGlobalAdmin.Rows[0]["JPEFax"].ToString() + "<br />";
                mlSendReult = sendMail(dtGlobalAdmin.Rows[0]["recoverPWFrom"].ToString(), dbEmail, "Password Recovery", emailBody, true, false, "");
            }
            else
            {
                lblPWrecoveryMsg.Text = "Sorry, we could not locate your account using the email address you entered.  Please try again or call your sales representative for more help.";
            }
        }

        if (lblPWrecoveryMsg.Text == string.Empty)
        {
            lblPWrecoveryMsg.Text = "Thank you.  Your information has been emailed to " + dbEmail + ".";
            btnForgottenCancel.Text = "Close";
            btnRecoverPassword.Visible = false;
            btnForgottenCancel.Focus();
        }
        else
        {
            btnForgottenCancel.Text = "Cancel";
            txtForgotEmail.Text = string.Empty;
            //txtForgotUserName.Text = string.Empty;
            txtForgotEmail.Focus();
        }
        ModalPopupExtenderThrobber.Hide();
        ModalPopupExtenderForgottenPassword.Show();
        upForgottenPW.Update();
    }

    protected string sendMail(string From, string To, string Subject, string Body, bool useHTML, bool useImage, string AttachFile)
    {
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
        string server = dtGlobalAdmin.Rows[0]["smtpServer"].ToString();

        MailMessage message = new MailMessage(From, To, Subject, Body);
        message.IsBodyHtml = useHTML;

        if (AttachFile != string.Empty)
        {
            Attachment data = new Attachment(AttachFile, MediaTypeNames.Application.Octet);
            message.Attachments.Add(data);
        }

        SmtpClient client = new SmtpClient(server);
        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(dtGlobalAdmin.Rows[0]["smtpUserName"].ToString(), dtGlobalAdmin.Rows[0]["smtpPassword"].ToString());



        //if ((bool)Page.Session["blnOnDevServer"])
        //{
            client.UseDefaultCredentials = false;
            client.Credentials = SMTPUserInfo;
            client.Port = int.Parse(dtGlobalAdmin.Rows[0]["smtpPort"].ToString());
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

    protected void saveAccountEditPanel()
    {

        string AccountKey = Request.Form["hdn_AccountKey"].ToString();
        string AccountUserType = Request.Form["hdn_UserType"].ToString().ToLower();
        bool blnUpdateNeeded = false;
        bool blnactivateEmailSent = false;
        bool blnAdminSendEmailChecked = false;

        string MyUserType = string.Empty;

        if (Page.Session["AccountType"] != null)
        {
            MyUserType = Page.Session["AccountType"].ToString().ToLower();
        }

        string VerifiedNewPassword = string.Empty;
        string VerifiedNewEmail = string.Empty;

        bool deleteAccount = false;
        string queryString = string.Empty;
        string randomValidString = string.Empty;
        //int chkActive = 0;
        //int chkApproved = 0;

        string changeList = string.Empty;
        string listToCustomer = string.Empty;

        string strErrorMessage = string.Empty;
        lblAccountError1.Text = string.Empty;
        int mtFieldCounter = 0;

        Label myAckLabelTop = (Label)FindControl("lblAcctAckTop");
        myAckLabelTop.Text = "Thank you.<br><br>  Your sales rep has been notified of the changes you entered.";
        Label myAckLabelBottom = (Label)FindControl("lblAcctAckBottom");
        myAckLabelBottom.Text = "Please allow up to two business days for changes to be reflected on the website.";

        if (AccountKey == "-1")
        {


            if (Request.Form["txt_eMail"].ToString().Trim() == string.Empty)
            {
                strErrorMessage += "<li>Please enter your eMail Address.</li>";
                mtFieldCounter++;
            }
            else
            {
                string regexString = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                RegexStringValidator myRegexValidator = new RegexStringValidator(regexString);
                try
                {
                    myRegexValidator.Validate(Request.Form["txt_eMail"].ToString().Trim());
                }
                catch
                {
                    strErrorMessage += "<li>Your Email address does not appear to be valid.</li>";
                }
            }

            if (Request.Form["txt_eMail"].ToString().Trim().ToLower() != Request.Form["txt_eMailVerify"].ToString().Trim().ToLower())
            {
                strErrorMessage += "<li>Your Verify Email address does not match the Email address you entered.</li>";
            }
            else
            {
                //----------------------- see if the email address is already used

                DataTable dtTestUName = GenUtils.ecom_AccountLookup(Request.Form["txt_eMailVerify"].ToString().Trim(), -1);
                if (dtTestUName != null)
                {
                    if (dtTestUName.Rows.Count > 0)
                    {
                        strErrorMessage += "<li>The email address you entered is already contained in our database.  Please use the Password Recovery link on the login page to retrieve login information for your account.</li>";

                    }
                }
            }


        if (AccountUserType != "back")
        {
            if (Request.Form["txt_NewPassword"].ToString().Trim().Length < 6)
            {
                strErrorMessage += "<li>Your Password must be at least 6 characters long.</li>";
                if (Request.Form["txt_NewPassword"].ToString().Trim() == string.Empty)
                {
                    mtFieldCounter++;
                }
            }
            
            if (Request.Form["txt_NewPassword"].ToString().Trim() != Request.Form["txt_Verify"].ToString().Trim())
            {
                strErrorMessage += "<li>The new password you entered does not match the verification entry.</li>";
                
            }
        }


            if (Request.Form["txt_FirstName"].ToString().Trim() == string.Empty)
            {
                strErrorMessage += "<li>Please enter your First Name.</li>";
                mtFieldCounter++;
            }

   

            if (Request.Form["txt_Company"].ToString().Trim() == string.Empty)
            {
                strErrorMessage += "<li>Please enter you Company Name.</li>";
                mtFieldCounter++;
            }
            if (Request.Form["txt_JobTitle"].ToString().Trim() == string.Empty)
            {
                strErrorMessage += "<li>Please enter your Job Title.</li>";
                mtFieldCounter++;
            }

            if (Request.Form["txt_Phone"].ToString().Trim() == string.Empty)
            {
                strErrorMessage += "<li>Please enter your Phone Number.</li>";
                mtFieldCounter++;
            }

            if (Request.Form["txt_Notes"].ToString().Trim() == string.Empty)
            {
                mtFieldCounter++;
            }

            //string dsdsd = Request.Form["txt_CAPTCHA"].ToString().Trim();
            //string dsdsad = Page.Session["last_CaptchaText"].ToString();

           myAckLabelTop.Text = "Thank you.<br><br>  Your request for an account on JPE Smart Buyer has been sent to our sales department for activation.";
            myAckLabelBottom.Text = "Please allow up to two business days for activation.  We will send you an email when your account is ready to go.";
            if (AccountUserType == "back")
            {
                myAckLabelTop.Text = "Welcome Back.<br><br>  Your request for an account on JPE Smart Buyer has been sent to our sales department for activation.";
                myAckLabelBottom.Text = "Please allow up to two business days for activation.  We will send you an email when your account is ready to go.";
            }

            if (AccountUserType != "back")
            {

                if (Request.Form["txt_CAPTCHA"].ToString().Trim() != Page.Session["CaptchaImageText"].ToString())
                {
                    strErrorMessage += "<li>The 'Human Verification' information you entered was incorrect.</li>";
                    
                }
            }


            if ((mtFieldCounter > 8 && AccountUserType != "back") || (mtFieldCounter > 4 && AccountUserType == "back"))
            {
                ModalPopupExtenderAccountUpdate.Hide();
                upPnlSmartPartsBuyer.Update();
                return;
            }
            else if (strErrorMessage != string.Empty)
            {
                lblAccountError1.Text = makenwErrorPanel(strErrorMessage);
                upAccountDetail.Update();
                return;
            }
            else
            {

                randomValidString = GenUtils.RandomString(20, false);

                queryString = "INSERT INTO ecom_UserAccounts (FirstName,  JobTitle, Phone, Company, EmailAddress, FAX, Password, MyAddress1, MyAddress2, MyAddress3, MyCity, MyState, MyZip, MyCountry, AccountCreated, Active, JPEApproved, AccountType, eMailValidationString, custLastComment, useAutoCheck, useHints) VALUES (";
               
                queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_FirstName"].ToString().Trim()) + "', ";
                queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_JobTitle"].ToString().Trim()) + "', ";
                queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_Phone"].ToString().Trim()) + "', ";
                queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_Company"].ToString().Trim()) + "', ";
                queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_eMail"].ToString().Trim()) + "', ";



                if (AccountUserType == "back")
                {
                    queryString += "'', ";
                    queryString += "'', ";

                    queryString += "'', ";
                    queryString += "'', ";
                    queryString += "'', ";
                    queryString += "'', ";
                    queryString += "'', ";
                    queryString += "'', ";
                    queryString += "'', ";
                }
                else
                {


                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_Fax"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_NewPassword"].ToString().Trim()) + "', ";

                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyAddress1"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyAddress2"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyAddress3"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyCity"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyState"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyZip"].ToString().Trim()) + "', ";
                    queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_MyCountry"].ToString().Trim()) + "', ";

                }


                queryString += "'" + DateTime.Now + "', ";
                queryString += "0, ";
                queryString += "0, ";

           
               queryString += "'new', ";
                


                queryString += "'" + randomValidString + "', ";

                queryString += "'" + GenUtils.CharacterstoHTML(Request.Form["txt_Notes"].ToString().Trim()) + "', ";
                queryString += "0, ";
                queryString += "1 ";

                queryString += " );";

                changeList = "Name||" + Request.Form["txt_FirstName"].ToString() + "~";
                changeList += "job Title||" + Request.Form["txt_JobTitle"].ToString() + "~";
                changeList += "Phone||" + Request.Form["txt_Phone"].ToString() + "~";
                changeList += "Company||" + Request.Form["txt_Company"].ToString() + "~";
                changeList += "Email Address||" + Request.Form["txt_eMail"].ToString() + "~";

                if (AccountUserType != "back")
                {


                    changeList += "Fax||" + Request.Form["txt_Fax"].ToString() + "~";

                    changeList += "Personal Address 1||" + Request.Form["txt_MyAddress1"].ToString() + "~";
                    changeList += "Personal Address 2||" + Request.Form["txt_MyAddress2"].ToString() + "~";
                    changeList += "Personal Address 3||" + Request.Form["txt_MyAddress3"].ToString() + "~";
                    changeList += "Personal City||" + Request.Form["txt_MyCity"].ToString() + "~";
                    changeList += "Personal State||" + Request.Form["txt_MyState"].ToString() + "~";
                    changeList += "Personal Zip||" + Request.Form["txt_MyZip"].ToString() + "~";
                    changeList += "Personal Country||" + Request.Form["txt_MyCountry"].ToString() + "~";
                }


                //changeList += "User Name|new|" + Request.Form["txt_UserName"].ToString() + "~";


                changeList += "Customer Notes|note|" + Request.Form["txt_Notes"].ToString() + "~";

                changeList += "Customer Key|note|~";
                changeList += "Website Updated|note|~";

            }


        }
        else
        {


            //------ Password
            if (Request.Form["txt_NewPassword"].ToString().Trim() != string.Empty)
            {
                if (Request.Form["txt_OldPassword"].ToString().Trim() != Page.Session["Password"].ToString())
                {
                    strErrorMessage += "<li>You must correctly enter your old password before you can specify a new password.  Your password has not been changed.</li>";
                }
                else if (Request.Form["txt_NewPassword"].ToString().Trim() != Request.Form["txt_Verify"].ToString().Trim())
                {
                    strErrorMessage += "<li>The new password you entered does not match the verification entry.  Your password has not been changed.</li>";
                }
                else if (Request.Form["txt_NewPassword"].ToString().Trim().Length < 6)
                {
                    strErrorMessage += "<li>Your password must be at least 6 characters long.  Your password has not been changed.</li>";
                }
                else if (Request.Form["txt_NewPassword"].ToString().Trim() == Request.Form["txt_Verify"].ToString().Trim())
                {
                    VerifiedNewPassword = Request.Form["txt_NewPassword"].ToString().Trim();
                    changeList += "Password Changed|****|****~";
                    blnUpdateNeeded = true;
                }
            }

            ////----------------------- email change
            //if (Request.Form["txt_eMail"].ToString() != Request.Form["hdn_eMail"].ToString())
            //{
            //    blnUpdateNeeded = true;


            //    changeList += "eMail|" + Request.Form["hdn_eMail"].ToString() + "|" + Request.Form["txt_eMail"].ToString() + "~";


            //}
            bool emailError = false;

            if (Request.Form["txt_eMail"].ToString() != Request.Form["hdn_eMail"].ToString())
            {
                if (Request.Form["txt_eMail"].ToString().Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your eMail Address.</li>";
                    emailError = true;
                    mtFieldCounter++;
                }
                else
                {
                    string regexString = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                    RegexStringValidator myRegexValidator = new RegexStringValidator(regexString);
                    try
                    {
                        myRegexValidator.Validate(Request.Form["txt_eMail"].ToString().Trim());
                    }
                    catch
                    {
                        strErrorMessage += "<li>Your Email address does not appear to be valid.</li>";
                        emailError = true;

                    }
                }

                if (Request.Form["txt_eMail"].ToString().Trim().ToLower() != Request.Form["txt_eMailVerify"].ToString().Trim().ToLower())
                {
                    strErrorMessage += "<li>Your Verify Email address does not match the Email address you entered.</li>";
                    emailError = true;
                }
                else if (!emailError)
                {
                    VerifiedNewEmail = Request.Form["txt_eMail"].ToString().Trim();
                    changeList += "eMail Address|" + Request.Form["hdn_eMail"].ToString() + "|" + Request.Form["txt_eMail"].ToString() + "~";
                    blnUpdateNeeded = true;
                }

            }


            //------- profile
            if (Request.Form["txt_FirstName"].ToString() != Request.Form["hdn_FirstName"].ToString())
            {
                blnUpdateNeeded = true;
                changeList += "First Name|" + Request.Form["hdn_FirstName"].ToString() + "|" + Request.Form["txt_FirstName"].ToString() + "~";
            }
            //if (Request.Form["txt_LastName"].ToString() != Request.Form["hdn_LastName"].ToString())
            //{
            //    blnUpdateNeeded = true;
            //    changeList += "Last Name|" + Request.Form["hdn_LastName"].ToString() + "|" + Request.Form["txt_LastName"].ToString() + "~";
            //}
            if (Request.Form["txt_Company"].ToString() != Request.Form["hdn_Company"].ToString())
            {
                blnUpdateNeeded = true;
                //chgProfile = true;
                changeList += "Company| " + Request.Form["hdn_Company"].ToString() + "|" + Request.Form["txt_Company"].ToString() + "~";
            }
            if (Request.Form["txt_JobTitle"].ToString() != Request.Form["hdn_JobTitle"].ToString())
            {
                blnUpdateNeeded = true;
                //chgProfile = true;
                changeList += "Job Title|" + Request.Form["txt_JobTitle"].ToString() + "|" + Request.Form["txt_JobTitle"].ToString() + "~";
            }
            if (Request.Form["txt_Phone"].ToString() != Request.Form["hdn_Phone"].ToString())
            {
                blnUpdateNeeded = true;
                changeList += "Phone|" + Request.Form["hdn_Phone"].ToString() + "|" + Request.Form["txt_Phone"].ToString() + "~";
            }






            if (Page.Session["AccountType"].ToString() != "guest")
            {





                if (Request.Form["txt_Fax"].ToString() != Request.Form["hdn_Fax"].ToString())
                {
                  
                    changeList += "FAX|" + Request.Form["hdn_Fax"].ToString() + "|" + Request.Form["txt_Fax"].ToString() + "~";
                }

                //----- My Address
                if (Request.Form["txt_MyAddress1"].ToString() != Request.Form["hdn_MyAddress1"].ToString())
                {
                
                    changeList += "Personal Address 1|" + Request.Form["hdn_MyAddress1"].ToString() + "|" + Request.Form["txt_MyAddress1"].ToString() + "~";
                }
                if (Request.Form["txt_MyAddress2"].ToString() != Request.Form["hdn_MyAddress2"].ToString())
                {
                    
                    changeList += "Personal Address 2|" + Request.Form["hdn_MyAddress2"].ToString() + "|" + Request.Form["txt_MyAddress2"].ToString() + "~";
                }
                if (Request.Form["txt_MyAddress3"].ToString() != Request.Form["hdn_MyAddress3"].ToString())
                {
                    
                    changeList += "Personal Address 3|" + Request.Form["hdn_MyAddress3"].ToString() + "|" + Request.Form["txt_MyAddress3"].ToString() + "~";
                }
                if (Request.Form["txt_MyCity"].ToString() != Request.Form["hdn_MyCity"].ToString())
                {
                   
                    changeList += "Personal Address City|" + Request.Form["hdn_MyCity"].ToString() + "|" + Request.Form["txt_MyCity"].ToString() + "~";
                }
                if (Request.Form["txt_MyState"].ToString() != Request.Form["hdn_MyState"].ToString())
                {
                  
                    changeList += "Personal Address State|" + Request.Form["hdn_MyState"].ToString() + "|" + Request.Form["txt_MyState"].ToString() + "~";
                }
                if (Request.Form["txt_MyZip"].ToString() != Request.Form["hdn_MyZip"].ToString())
                {
                    
                    changeList += "Personal Address Zip|" + Request.Form["hdn_MyZip"].ToString() + "|" + Request.Form["txt_MyZip"].ToString() + "~";
                }
                if (Request.Form["txt_MyCountry"].ToString() != Request.Form["hdn_MyCountry"].ToString())
                {
                   
                    changeList += "Personal Address Country|" + Request.Form["hdn_MyCountry"].ToString() + "|" + Request.Form["txt_MyCountry"].ToString() + "~";
                }

                //----- Billing Address
                if (Request.Form["txt_Address1"].ToString() != Request.Form["hdn_Address1"].ToString())
                {
                   
                    changeList += "Billing Address 1|" + Request.Form["hdn_Address1"].ToString() + "|" + Request.Form["txt_Address1"].ToString() + "~";
                }
                if (Request.Form["txt_Address2"].ToString() != Request.Form["hdn_Address2"].ToString())
                {
                   
                    changeList += "Billing Address 2|" + Request.Form["hdn_Address2"].ToString() + "|" + Request.Form["txt_Address2"].ToString() + "~";
                }
                if (Request.Form["txt_Address3"].ToString() != Request.Form["hdn_Address3"].ToString())
                {
                    changeList += "Billing Address 3|" + Request.Form["hdn_Address3"].ToString() + "|" + Request.Form["txt_Address3"].ToString() + "~";
                }
                if (Request.Form["txt_City"].ToString() != Request.Form["hdn_City"].ToString())
                {
                    changeList += "Billing Address City|" + Request.Form["hdn_City"].ToString() + "|" + Request.Form["txt_City"].ToString() + "~";
                }
                if (Request.Form["txt_State"].ToString() != Request.Form["hdn_State"].ToString())
                {
                    changeList += "Billing Address State|" + Request.Form["hdn_State"].ToString() + "|" + Request.Form["txt_State"].ToString() + "~";
                }
                if (Request.Form["txt_Zip"].ToString() != Request.Form["hdn_Zip"].ToString())
                {
                    changeList += "Billing Address Zip|" + Request.Form["hdn_Zip"].ToString() + "|" + Request.Form["txt_Zip"].ToString() + "~";
                }
                if (Request.Form["txt_Country"].ToString() != Request.Form["hdn_Country"].ToString())
                {
                    changeList += "Billing Address Country|" + Request.Form["hdn_Country"].ToString() + "|" + Request.Form["txt_Country"].ToString() + "~";
                }

                //----- Add a Ship To Address
                if (Request.Form["txt_shipAddress1"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping Address 1|add|" + Request.Form["txt_shipAddress1"].ToString() + "~";
                }

                if (Request.Form["txt_shipAddress2"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping Address 2|add|" + Request.Form["txt_shipAddress2"].ToString() + "~";
                }
                if (Request.Form["txt_shipAddress3"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping Address 3|add|" + Request.Form["txt_shipAddress3"].ToString() + "~";
                }
                if (Request.Form["txt_shipCity"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping City|add|" + Request.Form["txt_City"].ToString() + "~";
                }
                if (Request.Form["txt_shipState"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping State|add|" + Request.Form["txt_shipState"].ToString() + "~";
                }
                if (Request.Form["txt_shipCountry"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping Country|add|" + Request.Form["txt_shipCountry"].ToString() + "~";
                }
                if (Request.Form["txt_shipZip"].ToString().Trim() != string.Empty)
                {
                    changeList += "Shipping Zip|add|" + Request.Form["txt_shipZip"].ToString() + "~";
                }

                //-------------------- Add a shippers account
                if (Request.Form["txt_NewShipperAccount"].ToString().Trim() != string.Empty)
                {
                    if (Request.Form["ddl_NewShipperCarrier"].ToString().Trim() != string.Empty)
                    {
                        changeList += "Shipping Carrier|add|" + Request.Form["ddl_NewShipperCarrier"].ToString() + "~";
                    }

                    changeList += "Shipping Account Number|add|" + Request.Form["txt_NewShipperAccount"].ToString() + "~";
                }

            }

            //-------------------- Add a Note to JPE
            if (Request.Form["txt_Notes"].ToString().Trim() != string.Empty)
            {
                changeList += "Notes to JPE|note|" + Request.Form["txt_Notes"].ToString() + "~";
            }

            //----- update the file if there are changelist items
            if (changeList != string.Empty)
            {
                blnUpdateNeeded = true;
            }

            //---------------------- Admin Changes
            if (MyUserType == "admin")
            {
                if (Request.Form["chk_DeleteAccount"] != null)
                {

                    if (Request.Form["chk_DeleteAccount"].ToString() == "on")
                    {
                        deleteAccount = true;
                    }
                }

                if (Request.Form["chk_SendEmail"] != null)
                {
                    if (Request.Form["chk_SendEmail"].ToString() == "on")
                    {
                        blnAdminSendEmailChecked = true;
                    }
                }



                //chkApproved = 0;
                //if (Request.Form["chk_Approved"] != null)
                //{
                //    if (Request.Form["chk_Approved"].ToString() != Request.Form["hdn_Approved"].ToString())
                //    {
                //        blnUpdateNeeded = true;
                //        listToCustomer += "Your account with Jet Parts Engineering has been approved.";
                //        chkApproved = 1;
                //    }
                //    else if (Request.Form["chk_Approved"].ToString() == "on")
                //    {
                //        chkApproved = 1;
                //    }
                //    else if (Request.Form["hdn_Approved"].ToString() != "on")
                //    {
                //        blnUpdateNeeded = true;
                //    }
                //}
                //else if(Request.Form["hdn_Approved"].ToString() == "on")
                //{
                //    blnUpdateNeeded = true;
                //}

                //chkActive = 0;
                //if (Request.Form["hdn_Approved"].ToString() == "on")
                //{
                //    blnUpdateNeeded = true;
                //}
                //if (Request.Form["chk_Active"] != null)
                //{
                //    if (Request.Form["chk_Active"].ToString() != Request.Form["hdn_Active"].ToString())
                //    {
                //        blnUpdateNeeded = true;
                //        chkActive = 1;
                //        blnactivateEmailSent = true;
                //    }
                //    else if (Request.Form["chk_Active"].ToString() == "on")
                //    {
                //        chkActive = 1;
                //        blnactivateEmailSent = true;
                //    }
                //    else if (Request.Form["hdn_Active"].ToString() != "on")
                //    {
                //        blnUpdateNeeded = true;
                //    }
                //}
                //else if (Request.Form["hdn_Active"].ToString() == "on")
                //{
                //    blnUpdateNeeded = true;
                //}



                if (Request.Form["txt_RolodexKey"].ToString() != Request.Form["hdn_RolodexKey"].ToString())
                {
                    blnUpdateNeeded = true;
                }
                if (Request.Form["txt_QuantumCompanyCode"].ToString() != Request.Form["hdn_QuantumCompanyCode"].ToString())
                {
                    blnUpdateNeeded = true;
                }
                if (Request.Form["txt_MySalesPersonKey"].ToString() != Request.Form["hdn_MySalesPersonKey"].ToString())
                {
                    blnUpdateNeeded = true;
                }
                if (Request.Form["txt_AccountType"].ToString() != Request.Form["hdn_AccountType"].ToString())
                {
                    blnUpdateNeeded = true;
                }

                if (Request.Form["txt_JPENotes"].ToString().Trim() != Request.Form["hdn_JPENotes"].ToString().Trim())
                {
                    blnUpdateNeeded = true;
                }
            }

            if (strErrorMessage != string.Empty)
            {
                lblAccountError1.Text = makenwErrorPanel(strErrorMessage);
                upAccountDetail.Update();
                return;
            }

            else if (deleteAccount)
            {
                queryString = "DELETE FROM ecom_UserAccounts WHERE pkAccountKey = " + AccountKey + ";";
            }
            else if (blnUpdateNeeded)
            {
               
                
                queryString = " UPDATE ecom_UserAccounts SET ";
                queryString += " FirstName = '" + GenUtils.CharacterstoHTML(Request.Form["txt_FirstName"].ToString()) + "', ";
                //queryString += " LastName = '" + Request.Form["txt_LastName"].ToString() + "', ";
                queryString += " JobTitle = '" + GenUtils.CharacterstoHTML(Request.Form["txt_JobTitle"].ToString()) + "', ";
                queryString += " Phone = '" + GenUtils.CharacterstoHTML(Request.Form["txt_Phone"].ToString()) + "', ";
                queryString += " Company = '" + GenUtils.CharacterstoHTML(Request.Form["txt_Company"].ToString()) + "', ";
                
                queryString += " FAX = '" + GenUtils.CharacterstoHTML(Request.Form["txt_Fax"].ToString()) + "', ";

                
                if (VerifiedNewPassword != string.Empty)
                    queryString += " Password = '" + GenUtils.CharacterstoHTML(Request.Form["txt_NewPassword"].ToString()) + "', ";

                if ( VerifiedNewEmail != string.Empty)
                    queryString += " EmailAddress = '" + GenUtils.CharacterstoHTML(VerifiedNewEmail) + "', ";
            

                if (Page.Session["AccountType"].ToString() != "guest")
                {

                    
                    queryString += " MyAddress1 = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyAddress1"].ToString()) + "', ";
                    queryString += " MyAddress2 = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyAddress2"].ToString()) + "', ";
                    queryString += " MyAddress3 = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyAddress3"].ToString()) + "', ";
                    queryString += " MyCity = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyCity"].ToString()) + "', ";
                    queryString += " MyState = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyState"].ToString()) + "', ";
                    queryString += " MyZip = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyZip"].ToString()) + "', ";
                    queryString += " MyCountry = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MyCountry"].ToString()) + "', ";
                }


                if (MyUserType == "admin")
                {
                    //queryString += "  JPEApproved = " + chkApproved + ", Active = " + chkActive + ", ";


                    queryString += "  RolodexKey = '" + GenUtils.CharacterstoHTML(Request.Form["txt_RolodexKey"].ToString()) + "', ";

                    queryString += " QuantumCompanyCode  = '" + GenUtils.CharacterstoHTML(Request.Form["txt_QuantumCompanyCode"].ToString()) + "', ";

                    queryString += " MySalesPersonKey  = '" + GenUtils.CharacterstoHTML(Request.Form["txt_MySalesPersonKey"].ToString()) + "', ";

                    queryString += " AccountType  = '" + GenUtils.CharacterstoHTML(Request.Form["txt_AccountType"].ToString()) + "', ";
                   
                    queryString += "  JPENotes = '" + GenUtils.CharacterstoHTML(Request.Form["txt_JPENotes"].ToString()) + "', ";
                    queryString += "  LastAdminUpdate = '" + DateTime.Now + "', ";
                }

                queryString += "  custLastComment = '" + GenUtils.CharacterstoHTML(Request.Form["txt_Notes"].ToString()) + "' ";

                queryString += " WHERE pkAccountKey = " + AccountKey + " ;";
            }
        } 


        lblAccountError1.Text = string.Empty;
        upAccountDetail.Update();

        string conn = string.Empty;

        if (queryString != string.Empty)
        {
            conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conn);

            SqlConnection cacheQueueConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(queryString, cacheQueueConn);

            try
            {

                cacheQueueConn.Open();
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Error Updating User Account File: " + ex.ToString());
            }
            finally
            {
                cacheQueueConn.Close();
            }

            
            //gvAdminTable.DataBind();
        }

        string emailBody = string.Empty;


        if (AccountKey == "-1")
        {
            changeList = makeEmailTable(changeList, "NewAccount");
        }
        else
        {
            changeList = makeEmailTable(changeList, "AccountChanges");
        }

        string verifBody = string.Empty;
        if (listToCustomer != string.Empty)
        {
            //----- notification to customer that account activated


            //emailBody = "Hello " + dbFirstName + ";<br /><br />";
            //emailBody += "Your Username: " + dbUserName + "<br />";
            //emailBody += "Your Password: " + dbPW + "<br /><br /><br />";
            //emailBody += dtGlobalAdmin.Rows[0]["JPEName"].ToString() + "<br />";
            //emailBody += "Phone " + dtGlobalAdmin.Rows[0]["JPEPhone"].ToString() + "<br />";
            //emailBody += "Fax " + dtGlobalAdmin.Rows[0]["JPEFax"].ToString() + "<br />";

            //dtGlobalAdmin.Rows[0][""].ToString()

            //sendMail(dtGlobalAdmin.Rows[0]["recoverPWFrom"].ToString(), dbEmail, "Password Recovery", emailBody, true, false, "");

        }

        blnactivateEmailSent = true;
        

        string thisSubject = string.Empty;
        if (blnactivateEmailSent && blnAdminSendEmailChecked)
        {

            thisSubject = "JPESmartBuyer Account Activation";
            emailBody = "Hello " + Request.Form["txt_FirstName"].ToString() + ";<br /><br />";
            emailBody += "Thank you for requesting an account on our new JPE Smart Buyer ecommerce system.  Your account has been successfully activated.<br /><br />";

            emailBody += "Visit us at <a href='http://JetPartsEngineering.com'>http://JetPartsEngineering.com</a> for general information.  You may also access the eCommerce website directly by going to <a href='http://JPESmartBuyer.com'>http://JPESmartBuyer.com</a>.<br /> <br />";

            emailBody += "Your login email address is: " + Request.Form["txt_eMail"].ToString() + "<br /><br />";
            
            emailBody += "If you have forgotten your password, please go to <a href = 'http://JPESmartBuyer.com'>http://JPESmartBuyer.com</a> and click on the ‘Forgot your password’ link.  Your password will be emailed to you. <br /><BR />";

            emailBody += "The first time you attempt to log into JPE Smart Buyer you may be asked to verify your computer.  If this happens you will receive an email that contains a verification link.  Please click the link or copy and paste it into your browser’s address bar.<br /><br />";

            emailBody += "If you have any questions, comments or suggestions please give us a call at (206) 281-0963 and tell us that you would like website support or email <a href='bcourtright@jetpartsengineering.com'>bcourtright@jetpartsengineering.com</a> .<br/><br/>";

            emailBody += "We look forward to serving your PMA Parts needs.<br/><br/>";


            emailBody += "<i><br />";
            emailBody += "<font size='+2'><b>Jet Parts Engineering, Inc.</font></b><br />";
            emailBody += "220 West Mercer Street Suite 100<br />";
            emailBody += "Seattle, WA 98119<br />";
            emailBody += "P: (206) 281-0963<br />";
            emailBody += "F: (206) 838-8487<br />";
            emailBody += "<a href='WebSupport@jetpartsengineering.com '>WebSupport@jetpartsengineering.com </a><br /></i>";

            emailBody += "<br /><br /><font size=-2>This email message is for the sole use of the intended recipient(s) and may contain confidential and privileged information. Any unauthorized review, use, disclosure or distribution is prohibited. If you are not the intended recipient, please contact the sender by reply email and destroy all copies of the original message.</font>";

            //emailBody += "<br /><br />Time: " + DateTime.Now + "<br /><br />";
            //emailBody += dtGlobalAdmin.Rows[0]["JPEName"].ToString() + "<br />";
            //emailBody += "Phone " + dtGlobalAdmin.Rows[0]["JPEPhone"].ToString() + "<br />";
            //emailBody += "Fax " + dtGlobalAdmin.Rows[0]["JPEFax"].ToString() + "<br />";

            sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), Request.Form["txt_eMail"].ToString(), thisSubject, emailBody, true, false, "");
        }
       

       
        bool emailMe = false;
        if (Request.Form["chk_emailMeACopy"] != null)
        {
            emailMe = true;
        }

        if (emailMe || AccountKey == "-1")
        {
            if (AccountKey == "-1")
            {
                thisSubject = "JPE Smart Buyer New eCommerce Account request";
                emailBody = "Hello " + Request.Form["txt_FirstName"].ToString() + ";<br /><br />";
                emailBody += "We have received your request to open a JPE Smart Buyer eCommerce account.<br /><br /> ";
                emailBody += "In order to activate your account you will need to verify the email address you entered.  Please click on the link below or copy it and paste the entire string into the address bar of your browser.<br/> <br/>";
                emailBody += "<a href='http://jpesmartbuyer.com/ecom_prodSearch.aspx?v1=" + randomValidString + "'>http://jpesmartbuyer.com/ecom_prodSearch.aspx?v1=" + randomValidString + "</a><br/><br/>";
                emailBody += "We will email you when it is ready to go.<br /><br />";


                emailBody += "We look forward to serving your PMA Parts needs.<br/><br/>";


                emailBody += "<i><br />";
                emailBody += "<font size='+2'><b>Jet Parts Engineering, Inc.</font></b><br />";
                emailBody += "220 West Mercer Street Suite 100<br />";
                emailBody += "Seattle, WA 98119<br />";
                emailBody += "P: (206) 281-0963<br />";
                emailBody += "F: (206) 838-8487<br />";
                emailBody += "<a href='WebSupport@jetpartsengineering.com '>WebSupport@jetpartsengineering.com </a><br /></i>";

                emailBody += "<br /><br /><font size=-2>This email message is for the sole use of the intended recipient(s) and may contain confidential and privileged information. Any unauthorized review, use, disclosure or distribution is prohibited. If you are not the intended recipient, please contact the sender by reply email and destroy all copies of the original message.</font>";


            }

            else
            {
                thisSubject = "JPE Smart Buyer eCommerce Account Changes";
                emailBody = "Hello " + Request.Form["txt_FirstName"].ToString() + ";<br /><br />";

                emailBody += "Jet Parts Engineering has been notified of the following changes to your eCommerce profile.  Changes should be reflected in your account within one business day.<br /><br />";
            }
            emailBody += changeList;
            emailBody += "<br /><br />Time: " + DateTime.Now + "<br /><br />";
            emailBody += dtGlobalAdmin.Rows[0]["JPEName"].ToString() + "<br />";
            emailBody += "Phone " + dtGlobalAdmin.Rows[0]["JPEPhone"].ToString() + "<br />";
            emailBody += "Fax " + dtGlobalAdmin.Rows[0]["JPEFax"].ToString() + "<br /><br /><br />";




            emailBody += "We look forward to serving your PMA Parts needs.<br/><br/>";


            emailBody += "<i><br />";
            emailBody += "<font size='+2'><b>Jet Parts Engineering, Inc.</font></b><br />";
            emailBody += "220 West Mercer Street Suite 100<br />";
            emailBody += "Seattle, WA 98119<br />";
            emailBody += "P: (206) 281-0963<br />";
            emailBody += "F: (206) 838-8487<br />";
            emailBody += "<a href='WebSupport@jetpartsengineering.com '>WebSupport@jetpartsengineering.com </a><br /></i>";

            emailBody += "<br /><br /><font size=-2>This email message is for the sole use of the intended recipient(s) and may contain confidential and privileged information. Any unauthorized review, use, disclosure or distribution is prohibited. If you are not the intended recipient, please contact the sender by reply email and destroy all copies of the original message.</font>";


            sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), Request.Form["txt_eMail"].ToString(), thisSubject, emailBody, true, false, "");

            //---- send customer of copy of        changeList
        }

        if (changeList != string.Empty  && MyUserType != "admin")
        {
            //---------------- New account request
            if (AccountKey == "-1")
            {
                thisSubject = "JPE Smart Buyer New eCommerce Account request";
                emailBody = Request.Form["txt_FirstName"].ToString()  + " has requested a JPE Smart Buyer account:<br /><br />";

            }
            else
            {
                thisSubject = "JPE Smart Buyer eCommerce Account Changes";
                emailBody = Request.Form["txt_FirstName"].ToString() + " made the following changes to his or her profile on the JPE Smart Buyer website:<br /><br />";
            }
            emailBody += changeList;
            emailBody += "<br /><br />Time: " + DateTime.Now + "<br />";
            sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), dtGlobalAdmin.Rows[0]["changeEmailTo"].ToString(), thisSubject, emailBody, true, false, "");



        }

        //string strAckLabel = "<table class='accountAckTable'>";
        //strAckLabel += "<tr><td class='accountAckTop'>Thank you.<br><br>  Your sales rep has been notified of the changes you entered.</td></tr>";
        //strAckLabel += "<tr><td class='accountAckBottom'>Please allow up to two business days for changes to be reflected on the website.</td></tr>";
        //strAckLabel += "</table>";

        //Label myAckLabelTop = (Label)FindControl("lblAcctAckTop");
        //myAckLabelTop.Text = "Thank you.<br><br>  Your sales rep has been notified of the changes you entered.";
        //Label myAckLabelBottom = (Label)FindControl("lblAcctAckBottom");
        //myAckLabelBottom.Text = "Please allow up to two business days for changes to be reflected on the website.";


        ModalPopupExtenderAccountUpdate.Hide();

        if (MyUserType != "admin")
        {
            upAccountUpdateAck.Update();
            ModalPopupExtenderAccountAck.Show();
        }

        upPnlSmartPartsBuyer.Update();

      
    }

    private void writeLoginQueue(string UserNameEntered, string myIP, DateTime AttemptDateTime, int fkAccountKey, DateTime LockedOutDateTime, string loginStatus)
    {
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection loginQueueConn = new SqlConnection(conn);

        string queryString = "INSERT INTO ecom_LoginQueue (UserNameEntered, IP, AttemptDateTime, fkAccountKey, status) VALUES ('" + UserNameEntered + "', '" + myIP + "', '" + AttemptDateTime + "', " + fkAccountKey + ", '" + loginStatus + "'); ";

        SqlCommand cmd = new SqlCommand(queryString, loginQueueConn);

        try
        {
            loginQueueConn.Open();
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            lblLoginError.Text = "Error Updating login attempt: " + ex.ToString();
        }
        finally
        {
            loginQueueConn.Close();
        }
    }

    private void writeSearchQueue(DateTime SearchDateTime, int fkAccountKey, string SearchWords, int Qty, char Results)
    {
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection searchQueueConn = new SqlConnection(conn);

        string queryString = "INSERT INTO ecom_SearchQueue (SearchWords, fkAccountKey, Quantity, DateTime,,Results) VALUES ('" + SearchWords + "', " + fkAccountKey + "', " + Qty + ", '" + SearchDateTime + "', '" + Results + "'); ";

        SqlCommand cmd = new SqlCommand(queryString, searchQueueConn);

        try
        {
            searchQueueConn.Open();
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            lblLoginError.Text = "Error Updating login attempt: " + ex.ToString();
        }
        finally
        {
            searchQueueConn.Close();
        }
    }


    protected Boolean PlaceTheOrder()
    {
        lblShippingError.Text = string.Empty;
        upShippingError.Update();

        string strErrorMessage = string.Empty;
        string selectedCarrier = string.Empty;
        TextBox myTextBox3 = (TextBox)FindControl("txtNewShipAddress3");
        bool blnWriteCCToSQL = false;



        if ((bool)Page.Session["JPEapprovedForPricing"])
        {



            switch (Request.Form["ddlShipperCarrier"].ToString())
            {
                case "ups":
                    selectedCarrier = "U.P.S";
                    break;
                case "fedex":
                    selectedCarrier = "Federal Express";
                    break;
                case "dhl":
                    selectedCarrier = "DHL";
                    break;
                case "usps":
                    selectedCarrier = "U.S.P.S";
                    break;
                case "other":
                    selectedCarrier = "Other";
                    break;
            }

            bool ddlMyShipperIdSet = false;
            if (Request.Form["ddlMyShipperID"] != null)
            {
                if (Request.Form["ddlMyShipperID"].ToString() != "0")
                {
                    ddlMyShipperIdSet = true;
                }
            }
            string djsahjhas = Request.Form["ddlMyShipToAddress"].ToString();

            bool ddlMyShipAddressSet = false;
            if (Request.Form["ddlMyShipToAddress"] != null)
            {
                if (Request.Form["ddlMyShipToAddress"].ToString() != "0")
                {
                    ddlMyShipAddressSet = true;
                }
            }




            if (!ddlMyShipperIdSet)
            {
                if (Request.Form["ddlShipperCarrier"].ToString() == string.Empty && txtNewShipperID.Text == string.Empty)
                {
                    strErrorMessage += "<li>Please enter shipping carrier and account information.  If you want JPE to choose, please select 'Best Method' from the 'Select Carrier' drop down list.</li>";
                }
                else if (Request.Form["ddlShipperCarrier"].ToString() != "4" && txtNewShipperID.Text.Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your " + selectedCarrier + " account number.  If you want JPE to prepay shipping, please enter PrePay in the 'Shipper ID' control.</li>";
                }
            }


            if (!ddlMyShipAddressSet)
            {
                if (Request.Form["txtNewShipCompanyName"].ToString().Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your Company Name.</li>";
                }
                if (Request.Form["txtNewShipAddress1"].ToString().Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your ship to address.</li>";
                }

                if (Request.Form["txtNewShipCity"].ToString().Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your ship to city.</li>";
                }
                if (Request.Form["txtNewShipState"].ToString().Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your ship to state.</li>";
                }
                if (Request.Form["txtNewShipZip"].ToString().Trim() == string.Empty)
                {
                    strErrorMessage += "<li>Please enter your ship to zip code.</li>";
                }
            }


            if (Request.Form["txtPONumber"].ToString() == string.Empty)
            {
                strErrorMessage += "<li>Please enter your purchase order number.</li>";
            }


            if (Request.Form["hdnCCRequired"] != null)
            {
                if (Request.Form["hdnCCRequired"] == "true")
                {
                    if (Request.Form["ddlCardType"].ToString() == string.Empty)
                    {
                        strErrorMessage += "<li>Please select your payment method type.</li>";
                    }

                    if (Request.Form["ddlCardType"].ToString() != "COD")
                    {

                        if (Request.Form["txtCardName"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your name as it appears on the credit card.</li>";
                        }

                        if (Request.Form["txtCardNumber"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your credit card number.</li>";
                        }
                        else if (!GenUtils.IsCreditCardValid(Request.Form["txtCardNumber"].ToString()))
                        {
                            strErrorMessage += "<li>The credit card number you entered does not appear to be valid.  Please make corrections.</li>";
                        }

                        if (Request.Form["txtExpireMonth"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your card's expiration month.</li>";
                        }

                        if (Request.Form["txtExpireYear"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your card's expiration year.</li>";
                        }



                        if (Request.Form["txtCVVCode"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your card's CVV Code.</li>";
                        }


                        if (Request.Form["txtCardAddress1"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your credit card billing address.</li>";
                        }

                        if (Request.Form["txtCardCity"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your credit card billing city.</li>";
                        }


                        if (Request.Form["txtCardZip"].ToString() == string.Empty)
                        {
                            strErrorMessage += "<li>Please enter your credit card billing zip code.</li>";
                        }
                    }
                }
            }
            else //--- they must select net or cod
            {
                if (Request.Form["ddlCardType"].ToString() == string.Empty)
                {
                    strErrorMessage += "<li>Please select your payment method type.</li>";
                }
            }

            if (Request.Form["cbIAgreeToTOS"] == null)
            {
                strErrorMessage += "<li>You must agree to the terms and conditions.  Please check the box at the bottom of the checkout panel.</li>";

            }



        } //---------- if order instead of RFQ
         string orderTbl = string.Empty;
         string strPartNumber = string.Empty;
         string strDescription = string.Empty;
         string strthisQty = string.Empty;
         decimal decCost = 0m;
         int intQtyOrdered = 0;
         decimal decExt = 0m;
         decimal decAvl = 0m;
         int intBO = 0;
         int stcKey = 0;

         decimal decOrderTotal = 0m;

         if (dtCacheMaster != null)
         {
             dtCacheMaster.Rows.Clear();
         }

        //------------ SPEED  is this needed or is the cache loaded----
         dtCacheMaster = GenUtils.GetCacheforSessionID(Page.Session["MySessionId"].ToString());

         if (dtCacheMaster == null)
         {
             strErrorMessage += "<li>Your Shopping Cart is Empty.</li>";
         }
         if (dtCacheMaster.Rows.Count > 0)
         {
             foreach (DataRow myControlRow in dtCacheMaster.Rows)
             {
                 if (myControlRow["cur_QTY_ORDERED"] != DBNull.Value)
                 {
                     if ((int)myControlRow["cur_QTY_ORDERED"] > 0)
                     {

                         strPartNumber = myControlRow["pmast_PN"].ToString();
                         strDescription = myControlRow["pmast_DESCRIPTION"].ToString();
                         decCost = (decimal)myControlRow["calc_SELL_PRICE"];
                         intQtyOrdered = (int)myControlRow["cur_QTY_ORDERED"];
                         stcKey = (int)myControlRow["pmast_P_STC_AUTO_KEY"];
                         strthisQty = intQtyOrdered.ToString();
                         decExt = decCost * intQtyOrdered;
                         decOrderTotal += decExt;
                         intBO = 0;
                         decAvl = (decimal)myControlRow["calc_QTY_AVAILABLE"] - intQtyOrdered;
                         if (decAvl > 0)
                         {
                             decAvl = Convert.ToDecimal(intQtyOrdered);
                         }
                         else
                         {
                             decAvl = (decimal)myControlRow["calc_QTY_AVAILABLE"];
                             intBO = intQtyOrdered - Convert.ToInt32(decAvl);
                         }


                         orderTbl += strPartNumber + "|" + strDescription + "|" + intQtyOrdered + "|" + decAvl + "|" + intBO + "|" + decCost  + "|" + decExt + "|" + stcKey + "~";
                     }
                 }
             }
         }
         else
         {
             strErrorMessage += "<li>Your Shopping Cart is Empty.</li>";
         }

         //3183387JP-1|Poppet and Guide Assembly|1|1|0|677.00|677.00~3750806JP-1|Ring, Piston|1|1|0|725.00|725.00~3750807JP-1|Ring Set|1|1|0|419.00|419.00~3750808JP-1|Ring, Piston|1|1|0|793.00|793.00~3750809JP-1|Ring, Guide|1|1|0|299.00|299.00~

         if (strErrorMessage != string.Empty)
            {
                lblShippingError.Text = makenwErrorPanel(strErrorMessage);
                upShippingError.Update();
                return false;
            }

         string fileToAttachLocation = string.Empty;
         string fileToAttach = string.Empty;

         string fn = string.Empty;
         if ((fileUploadPO.PostedFile != null) && (fileUploadPO.PostedFile.ContentLength > 0))
         {
             fn = System.IO.Path.GetFileName(fileUploadPO.PostedFile.FileName);
             fn = GenUtils.RandomString(5, false) + "_" +fn;
             fileToAttachLocation = Server.MapPath("poUpload") + "\\" + fn;
             fileToAttach = fn;
             
             try
             {
                 fileUploadPO.PostedFile.SaveAs(fileToAttachLocation);
             }
             catch (Exception ex)
             {
                 Debug.WriteLine("Error: " + ex.Message);
             }
         }

         DateTime orderDateTime = DateTime.Now;

         string emailTbl = string.Empty;
        //-------------- Meta
         emailTbl += "User|" + Page.Session["RDX_CONTACT_NAME"] + "|~";
         emailTbl += "Job Title|" + Page.Session["JobTitle"] + "|~";
         emailTbl += "Company|" + Page.Session["Company"] + "|~";
         emailTbl += "Email Address|" + Page.Session["EmailAddress"] + "|~";
         emailTbl += "Phone Number|" + Page.Session["RDXPhoneNumber"] + "|~";
         emailTbl += "Rolodex Key|" + Page.Session["RolodexKey"] + "|~";
         string myShipperID = string.Empty;
         string myShipToAddress = string.Empty;

         if ((bool)Page.Session["JPEapprovedForPricing"])
         {
             emailTbl += "Order Total|$" + String.Format("{0:C}", decOrderTotal.ToString()) + "|~";
             if (Request.Form["ddlCardType"] != null)
             {
                 switch (Request.Form["ddlCardType"].ToString())
                 {
                     case "COD":
                         emailTbl += "Payment Type|Cash On Delivery|~";
                         break;
                     case "NET":
                         emailTbl += "Payment Type|" + Page.Session["TERM_DESCRIPTION"] + "|~";
                         break;
                     case "AX":
                         emailTbl += "Payment Type|AMEX|~";
                         blnWriteCCToSQL = true;
                         break;
                     case "VI":
                         emailTbl += "Payment Type|VISA|~";
                         blnWriteCCToSQL = true;
                         break;
                     case "MC":
                         emailTbl += "Payment Type|MC|~";
                         blnWriteCCToSQL = true;
                         break;
                 }
             }

             //------- profile
             if (Request.Form["ddlMyShipperID"] != null)
             {
                 if (Request.Form["ddlMyShipperID"].ToString() != string.Empty)
                 {
                     emailTbl += "Cur Shipping Account|" + Request.Form["ddlMyShipperID"].ToString() + "|~";
                     myShipperID = Request.Form["ddlMyShipperID"].ToString();
                 }
             }
             if (Request.Form["ddlShipperCarrier"] != null)
             {
                 if (Request.Form["ddlShipperCarrier"].ToString() != "0")
                 {
                     emailTbl += "New Shipping Carrier|" + Request.Form["ddlShipperCarrier"].ToString() + "|~";
                 }
             }

             if (txtNewShipperID.Text != string.Empty)
             {
                 emailTbl += "New Shipper ID|" + txtNewShipperID.Text + "|~";
                 emailTbl += "Save New Shipper Account|" + cbSaveNewShipperID.Checked + "|~";
                 emailTbl += "New Shipper Service to User|" + txtNewShipperServicetoUse.Text + "|~";
             }

             if (Request.Form["ddlMyShipToAddress"].ToString() != null)
             {
                 if (Request.Form["ddlMyShipToAddress"].ToString() != string.Empty)
                 {
                     emailTbl += "Cur Shipping Address|" + Request.Form["ddlMyShipToAddress"].ToString() + "|~";
                     myShipToAddress = Request.Form["ddlMyShipToAddress"].ToString();
                 }
             }
             if (txtNewShipAddress1.Text != string.Empty)
             {
                 emailTbl += "New Shipper Address|" + txtNewShipCompanyName.Text + "|~";
                 emailTbl += "|" + txtNewShipAddress1.Text + "|~";
                 emailTbl += "|" + txtNewShipAddress2.Text + "|~";
                 emailTbl += "|" + myTextBox3.Text + "|~";
                 emailTbl += "City|" + txtNewShipCity.Text + "|~";
                 emailTbl += "State|" + txtNewShipState.Text + "|~";
                 emailTbl += "Country|" + txtNewShipCountry.Text + "|~";
                 emailTbl += "Zip|" + txtNewShipZip.Text + "|~";
                 emailTbl += "Save New Address|" + cbSaveNewShipToAddress.Checked + "|~";
             }
         }
         if (txtFutureShipCalendar.Text != string.Empty)
         {
             emailTbl += "Customer Required Date|" + txtFutureShipCalendar.Text + "|~";
         }
         if (Request.Form["txtPONumber"].ToString() != string.Empty)
         {
             emailTbl += "PO Number|" + Request.Form["txtPONumber"].ToString() + "|~";
         }
         emailTbl += "fileToAttachLocation|" + fileToAttachLocation + "|~";
         emailTbl += "fileToAttach|" + fileToAttach + "|~";
        emailTbl = makeEmailTable(emailTbl, "OrderCompany");
        string thisSubject = string.Empty;
        string emailBody = string.Empty;

        if ((bool)Page.Session["JPEapprovedForPricing"])
        {
            thisSubject = "JPE Smart Buyer eCommerce Order Entered";
            emailBody += "The following order has been placed on the eCommerce Website<br /><br />";
        }
        else
        {
            thisSubject = "JPE Smart Buyer Request for Quote";
            emailBody += "The following RFQ has been submitted on the eCommerce Website<br /><br />";
        }
         emailBody += emailTbl;
         emailBody += "<br /><br />";
         emailBody += makeEmailTable(orderTbl, "OrderCART");
         emailBody += "<br /><br />Time: " + orderDateTime + "<br /><br />";
         emailBody += dtGlobalAdmin.Rows[0]["JPEName"].ToString() + "<br />";
         emailBody += "Phone " + dtGlobalAdmin.Rows[0]["JPEPhone"].ToString() + "<br />";
         emailBody += "Fax " + dtGlobalAdmin.Rows[0]["JPEFax"].ToString() + "<br />";

         sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), dtGlobalAdmin.Rows[0]["defaultEmailTo"].ToString(), thisSubject, emailBody, true, false, fileToAttachLocation);

         string strReqType = "Order";
         if (!(bool)Page.Session["JPEapprovedForPricing"])
         {
             strReqType = "RFQ";
         }
        //---------------- write the order to a file
         string conn = string.Empty;
         conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
         SqlConnection ordersConn = new SqlConnection(conn);
         string queryString = string.Empty;

         if ((bool)Page.Session["JPEapprovedForPricing"])
         {
             queryString = "INSERT INTO ecom_OrdersEmailed (DateTime, reqType, PlacedBy, JobTitle, Company, Email, RoloDexKey, OrderTotal, ShipAccount, ShipCarrier, ShipperID, ShipperService, CustomerRequiredDate, ShipToCompanyName, ShipToAddress, NewShipAddress1, NewShipAddress2, NewShipAddress3, City, State, Country, Zip, PONumber, FileAttachment, OrderDetails, CompanyKey) VALUES ('" + orderDateTime + "', '" + strReqType + "', '" + Page.Session["RDX_CONTACT_NAME"] + "', '" + Page.Session["JobTitle"] + "', '" + Page.Session["Company"] + "', '" + Page.Session["EmailAddress"] + "', '" + Page.Session["RolodexKey"] + "', '" + String.Format("{0:C}", decOrderTotal.ToString()) + "', '" + myShipperID + "','" + Request.Form["ddlShipperCarrier"].ToString() + "', '" + txtNewShipperID.Text + "', '" + txtNewShipperServicetoUse.Text + "', '" + txtFutureShipCalendar.Text + "', '" + myShipToAddress + "', '" + txtNewShipCompanyName.Text + "', '" + txtNewShipAddress1.Text + "', '" + txtNewShipAddress2.Text + "', '" + myTextBox3.Text + "', '" + txtNewShipCity.Text + "', '" + txtNewShipState.Text + "', '" + txtNewShipCountry.Text + "', '" + txtNewShipZip.Text + "', '" + Request.Form["txtPONumber"].ToString() + "', '" + fileToAttach + "', '" + orderTbl + "', '" + Page.Session["Quantum_CMP_AUTO_KEY"] + "'); ";
         }
         else
         {
             queryString = "INSERT INTO ecom_OrdersEmailed (DateTime, reqType, PlacedBy, JobTitle, Company, Email, RoloDexKey, OrderTotal, ShipAccount, ShipCarrier, ShipperID, ShipperService, CustomerRequiredDate, ShipToCompanyName, ShipToAddress, NewShipAddress1, NewShipAddress2, NewShipAddress3, City, State, Country, Zip, PONumber, FileAttachment, OrderDetails, CompanyKey) VALUES ('" + orderDateTime + "', '" + strReqType + "', '" + Page.Session["RDX_CONTACT_NAME"] + "', '" + Page.Session["JobTitle"] + "', '" + Page.Session["Company"] + "', '" + Page.Session["EmailAddress"] + "', '" + Page.Session["RolodexKey"] + "', '0', '" + myShipperID + "','', '', '', '" + txtFutureShipCalendar.Text + "', '', '', '', '', '', '', '', '', '', '" + Request.Form["txtPONumber"].ToString() + "', '" + fileToAttach + "', '" + orderTbl + "', '" + Page.Session["Quantum_CMP_AUTO_KEY"] + "'); ";

         }
         SqlCommand cmd = new SqlCommand(queryString, ordersConn);
         try
         {
             ordersConn.Open();
             cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
             lblLoginError.Text = "Error Inserting the Order: " + ex.ToString();
         }
         finally
         {
             ordersConn.Close();
         }

        //----  Write Credit Card Info to the SQL Table
         if (blnWriteCCToSQL)
         {
                 SqlConnection cardConn = new SqlConnection(conn);
                 queryString = "INSERT INTO ecom_CreditCardQueue (DateTime, PlacedBy, Company, Email, PONumber, OrderTotal, CardType, CardNumber, NameonCard, ExpirationDate, CVVCode, Address1, Address2, City, State, Zip) VALUES ('" + orderDateTime + "', '" + Page.Session["RDX_CONTACT_NAME"] + "', '" + Page.Session["Company"] + "', '" + Page.Session["EmailAddress"] + "', '" + Request.Form["txtPONumber"].ToString() + "', '" + String.Format("{0:C}", decOrderTotal.ToString()) + "', '" + Request.Form["ddlCardType"].ToString() + "', '" + Request.Form["txtCardNumber"].ToString() + "', '" + Request.Form["txtCardName"].ToString() + "', '" + Request.Form["txtExpireMonth"].ToString() + "/" + Request.Form["txtExpireYear"].ToString() + "', '" + Request.Form["txtCVVCode"].ToString() + "', '" + Request.Form["txtCardAddress1"].ToString() + "', '" + Request.Form["txtCardAddress2"].ToString() + "', '" + Request.Form["txtCardCity"].ToString() + "', '" + Request.Form["txtCardState"].ToString() + "', '" + Request.Form["txtCardZip"].ToString()  + "'); ";
                 SqlCommand cardcmd = new SqlCommand(queryString, cardConn);
                 try
                 {
                     cardConn.Open();
                     cardcmd.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {
                     lblLoginError.Text = "Error Inserting the Credit Card: " + ex.ToString();

                 }
                 finally
                 {
                     cardConn.Close();
                 }
         }
         return true;
    }

    //************ Button click events stack

    protected void btnPartsSearchfromCart_Click(object sender, EventArgs e)
    {
        lblShippingError.Text = string.Empty;
        upShippingError.Update();
        Page.Session["PanelMode"] = "search";
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnRecalcCart2_Click(object sender, EventArgs e)
    {
        lblShippingError.Text = string.Empty;
        upShippingError.Update();
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnCheckoutNow_Click(object sender, EventArgs e)
    {
        lblShippingError.Text = string.Empty;
        upShippingError.Update();
        Page.Session["PanelMode"] = "checkout";
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnCloseVerify_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderEmailVerify.Hide();
        Response.Redirect("ecom_prodSearch.aspx");
    }

    protected void btnRecoverPassword_Click(object sender, EventArgs e)
    {
        handleForgottenPassword();
    }

    protected void btnForgottenCancel_Click(object sender, EventArgs e)
    {
        lblPWrecoveryMsg.Text = string.Empty;
        txtForgotEmail.Text = string.Empty;
        btnForgottenCancel.Text = "Cancel";
        btnRecoverPassword.Visible = true;
        ModalPopupExtenderForgottenPassword.Hide();
        txtLoginUserName.Focus();
    }

    protected void ibEditMyAccount_Click(object sender, EventArgs e)
    {
    }

    protected void btnAccountEditPanelSave_Click(object sender, EventArgs e)
    {
        saveAccountEditPanel();
    }

    protected void btnPartsPanelSave2_Click(object sender, EventArgs e)
    {
        savePartsDetailPanel();
    }

    protected void btnPartsPanelSave1_Click(object sender, EventArgs e)
    {
        savePartsDetailPanel();
    }
   
    protected void btnUpdateSmartBuyer_Click(object sender, EventArgs e)
    {
        blnAddToCart = true;
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnUpdateSmartBuyerPopup_Click(object sender, EventArgs e)
    {
        PartsPopupUpdate = true;
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnCheckAvailable_Click(object sender, EventArgs e)
    {
        blncheckAvlScrape = true;
        blnAvlButtonclick = true;
        ModalPopupExtenderThrobber.Hide();
    }

    protected void JPEonChangeCompanyDDL(object sender, EventArgs e)
    {
    //----event handler for JPE company selector changed
    Page.Session["JPE_CMP_AUTO_KEY"] = Convert.ToInt16(ddlCompaniesforSalesperson.SelectedValue);

    if (dtCacheMaster != null)
    {
        dtCacheMaster.Rows.Clear();
    }
    Page.Session["strSearchList"] = string.Empty;
    Page.Session["strAssemblySearch"] = string.Empty;
    Page.Session["strNHAPartialMatches"] = string.Empty;
    Page.Session["strLastNHASearchString"] = string.Empty;
    updateTheCache(dtCacheMaster, Page.Session["MySessionId"].ToString(), "clearAll", string.Empty);
    Page.Session["PanelMode"] = "clearCache";

    }

    protected void btnUpdatePartsClosePopup_Click(object sender, EventArgs e)
    {
        PartsPopupUpdate = true;
        closePartsPopup = true;
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnPartsSearch_Click(object sender, EventArgs e)
    {
        Page.Session["PanelMode"] = "searchClick";
        blncheckAvlScrape = true;
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnNHADetailPopup_Click(object sender, EventArgs e)
    {
        Page.Session["PanelMode"] = "searchClick";
        blnNHADetailPopupSearch = true;
        ModalPopupExtenderThrobber.Hide();
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        UserLogin("btnLogin_Click");
    }
  
    protected void btnPlaceOrder_Click(object sender, EventArgs e)
    {
        if (PlaceTheOrder())
        {
            if (dtCacheMaster != null)
            {
                dtCacheMaster.Rows.Clear();
            }
            updateTheCache(dtCacheMaster, Page.Session["MySessionId"].ToString(), "clearAll", string.Empty);

            Page.Session["PanelMode"] = "OrderPlaced";

            ModalPopupExtenderThrobber.Hide();

            cbSaveNewShipperID.Checked = false;
            txtNewShipperServicetoUse.Text = string.Empty;
            txtFutureShipCalendar.Text = string.Empty;
            txtNewShipCompanyName.Text = string.Empty;
            cbSaveNewShipToAddress.Checked = false;
            txtNewShipAddress1.Text = string.Empty;
            txtNewShipAddress2.Text = string.Empty;

            txtNewShipCity.Text = string.Empty;
            txtNewShipCountry.Text = string.Empty;
            txtNewShipZip.Text = string.Empty;
            txtPONumber.Text = string.Empty;
            txtCardName.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
            txtExpireMonth.Text = string.Empty;
            txtExpireYear.Text = string.Empty;
            txtCVVCode.Text = string.Empty;
            cbIAgreetoTOS.Checked = false;
        }
    }

    protected void btnLogOut_Click(object sender, EventArgs e)
    {
        UserLogOut();
    }

    protected void btnBrowserBackFwdButton_Click(object sender, EventArgs e)
    {
        string thisState = lblBrowserBackFwdState.Text;
    }

    protected void btnMTSPBPanel_Click(object sender, EventArgs e)
    {

        //----------------------------------- SPEED is the cache master already loaded
        if (dtCacheMaster != null)
        {
            dtCacheMaster.Rows.Clear();
        }
        Page.Session["strSearchList"] = string.Empty;
        Page.Session["strNHAPartialMatches"] = string.Empty;
        Page.Session["strAssemblySearch"] = String.Empty;
        Page.Session["strLastNHASearchString"] = string.Empty;
        updateTheCache(dtCacheMaster, Page.Session["MySessionId"].ToString(), "clearAll", string.Empty);
        Page.Session["PanelMode"] = "clearCache";
        ModalPopupExtenderThrobber.Hide();
    }

    //protected void btnLoadandShowPartsPopUp_Click(object sender, EventArgs e)
    //{












    //}

    protected void btnSuggestionSubmit_Click(object sender, EventArgs e)
    {
        blnSuggestion = true;
        handleSuggestion();
    }

    protected void btnChangeUseHints_Click(object sender, EventArgs e)
    {
        blncheckAvlScrape = true;
        Thread.Sleep(100);
    }

    protected void btnHistorySearch_Click(object sender, EventArgs e)
    {
        int thisCompanyKey = -1;
        if (Page.Session["Quantum_CMP_AUTO_KEY"] != null)
        {
            if( int.TryParse(Page.Session["Quantum_CMP_AUTO_KEY"].ToString(), out thisCompanyKey) == true)
            {
                DateTime fromDate;
                DateTime toDate;
                string hxDateError = string.Empty;  
                String strfromDate = string.Empty;
                string strtoDate = string.Empty;

                if (GenUtils.IsDate(txtOrdersFromDate.Text))
                {
                    fromDate = DateTime.Parse(txtOrdersFromDate.Text);
                    TimeSpan span = DateTime.Now - fromDate;
                    if (span.TotalDays > int.Parse(dtGlobalAdmin.Rows[0]["HistoryShowDays"].ToString()))
                    {
                        fromDate = DateTime.Now.AddDays(0 - int.Parse(dtGlobalAdmin.Rows[0]["HistoryShowDays"].ToString()));
                        hxDateError = "History searches are limited to the past " + dtGlobalAdmin.Rows[0]["HistoryShowDays"].ToString() + " days.";
                    }
                    strfromDate = string.Format("{0:MM/d/yyy}", fromDate);
                }
                else
                {
                    strfromDate = string.Empty;
                }

                if (GenUtils.IsDate(txtOrdersToDate.Text))
                {
                    toDate = DateTime.Parse(txtOrdersToDate.Text);
                    strtoDate = string.Format("{0:MM/d/yyy}", toDate);
                }
                else
                {
                    strtoDate = string.Empty;
                }

                txtOrdersFromDate.Text = strfromDate;
                Page.Session["hxFromDate"] = txtOrdersFromDate.Text;
                txtOrdersToDate.Text = strtoDate;
                Page.Session["hxToDate"] = txtOrdersToDate.Text;
                Page.Session["hxPartNo"] = txtOrdersPONumber.Text;
                lblOrderHxSearchError.Text = hxDateError;
            }
        }
        ModalPopupExtenderThrobber.Hide();
        ModalPopupExtenderAccountHistory.Y = 20;
    }

    protected void btnCloseAccountHistory_Click(object sender, EventArgs e)
    {
        closeAccountHistoryPopup = true;
    }

    protected void btnBuildSRPReport_Click(object sender, EventArgs e)
    {
        mktg_Reporting myReport = new mktg_Reporting();
        hlshowSRPReport = myReport.distributeReport("adminPanel", txtSRPFromDate.Text, txtSRPToDate.Text, txtSRPToDate.Text, "", txtSRPSalesReps.Text, txtSRPCompanyCode.Text, strFileRootPath, strReportsPath, sender, e, cbIncludeCompanies.Checked.ToString(), cbIncludeParts.Checked.ToString(), false, string.Empty, string.Empty, blnOnDevServer);
        ModalPopupExtenderThrobber.Hide();
    }

    protected void clearPartsCache()
    {
        if (dtCacheMaster != null)
        {
            dtCacheMaster.Rows.Clear();
        }
        Page.Session["strSearchList"] = string.Empty;
        Page.Session["strNHAPartialMatches"] = string.Empty;
        Page.Session["strAssemblySearch"] = String.Empty;
        Page.Session["strLastNHASearchString"] = string.Empty;
        updateTheCache(dtCacheMaster, Page.Session["MySessionId"].ToString(), "clearAll", string.Empty);
        phSmartBuyerPanel.Controls.Clear();
    }

    //************ Application Utilities

    protected void handleSuggestion()
    {
        string emailBody = string.Empty;
        string emailSubject = string.Empty;

        if ((int)Page.Session["LoggedInUserID"] > 0)
        {
            if (Request.Form["txtSuggestionBox"].Trim() == string.Empty)
            {
                lblSuggestionMessage.Text = "No information was entered.  No action was taken.<br/>";
                return;
            }
        }
        else
        {
            if (Request.Form["txtSuggestionName"].Trim() != string.Empty)
            {
                emailBody += "Name: " + Request.Form["txtSuggestionName"].Trim() + "<br />";
            }
            if (Request.Form["txtEmailAddress"].Trim() != string.Empty)
            {
                emailBody += "Email and/or Phone: " + Request.Form["txtEmailAddress"].Trim() + "<br />";
            }

            if (Request.Form["txtEmailAddress"] + Request.Form["txtSuggestionName"].Trim() + Request.Form["txtSuggestionBox"].Trim() == string.Empty)
            {
                lblSuggestionMessage.Text = "No information was entered.  No action was taken.<br/>";
                return;
            }
        }
       
        if ((int)Page.Session["LoggedInUserID"] > 0)
        {
            emailSubject += Page.Session["RDX_CONTACT_NAME"] + " [logged in user] has sent a suggestion ";
            emailBody += "Name: " + Page.Session["RDX_CONTACT_NAME"] + "<br />";
            emailBody += "Job Title: " + Page.Session["JobTitle"] + "<br />";
            emailBody += "Company: " + Page.Session["Company"] + "<br />";
            emailBody += "Email: " + Page.Session["EmailAddress"] + "<br />";
            emailBody += "Phone: " + Page.Session["RDXPhoneNumber"] + "<br />";
        }
        else
        {
             if (Request.Form["txtSuggestionName"].Trim() != string.Empty)
            {
                emailSubject += Request.Form["txtSuggestionName"].Trim() + " [guest] ";
            }
             else
             {
                 emailSubject += "An anonymous guest ";
             }
             emailSubject += "has sent a suggestion";
        }
        emailBody += "<br/><br/>";
        emailBody += Request.Form["txtSuggestionBox"];

        sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), dtGlobalAdmin.Rows[0]["defaultEmailTo"].ToString(), emailSubject, emailBody, true, false, "");

        //----  Record to the Data Table
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection loginQueueConn = new SqlConnection(conn);
        string queryString = string.Empty;
        if ((int)Page.Session["LoggedInUserID"] > 0)
        {
            queryString = "INSERT INTO ecom_Suggestions (dateTime, UserID, Name, Phone, Email, Company, JobTitle, Suggestion) VALUES ('" + DateTime.Now + "', " + Page.Session["LoggedInUserID"] + ", '" + Page.Session["RDX_CONTACT_NAME"] + "', '" + Page.Session["RDXPhoneNumber"] + "', '" + Page.Session["EmailAddress"] + "', '" + Page.Session["Company"] + "', '" + Page.Session["JobTitle"] + "', '" + GenUtils.CharacterstoHTML(Request.Form["txtSuggestionBox"]) + "'); ";
        }
        else
        {
            queryString = "INSERT INTO ecom_Suggestions (dateTime, UserID, Name, Email, Suggestion) VALUES ('" + DateTime.Now + "', " + Page.Session["LoggedInUserID"] + ", '" + GenUtils.CharacterstoHTML(Request.Form["txtSuggestionName"]) + "', '" + GenUtils.CharacterstoHTML(Request.Form["txtEmailAddress"]) + "', '" + GenUtils.CharacterstoHTML(Request.Form["txtSuggestionBox"]) + "'); ";
        }
        SqlCommand cmd = new SqlCommand(queryString, loginQueueConn);
        try
        {
            loginQueueConn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            lblLoginError.Text = "Error Updating suggestion table: " + ex.ToString();
        }
        finally
        {
            loginQueueConn.Close();
        }
        lblSuggestionMessage.Text = "Thank you. Your suggestions have been sent to the JPE Smart Buyer team.<br/>";
    }

    protected string makeEmailTable(string inputList, string headerType)
    {
        string newTbl = string.Empty;
        int lp = -1;
        if (inputList != string.Empty)
        {
            if (headerType == "NewAccount")
            {
                newTbl = "<Table border='1' cellpadding='5'><tr><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;'></td><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>&nbsp;</td></tr>";
            }
            else if (headerType == "OrderCART")
            {
                newTbl = "<Table border='1' cellpadding='5'><tr>";
                newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;'>Part Number</td>";
                newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Description</td>";
                if ((bool)Page.Session["JPEapprovedForPricing"])
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Ordered</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Avl</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>B/O</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Price</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Ext</td>"; 
                }
                else
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Required</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>Avl</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>B.O.</td>"; 
                }
                    newTbl += "</tr>";
            }
            else if (headerType == "OrderCompany")
            {
                newTbl = "<Table border='1' cellpadding='5'><tr><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;'></td><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;' coslpan='2'>&nbsp;</td></tr>";
            }
            else   //   AccountChanges
            {
                newTbl = "<Table border='1' cellpadding='5'><tr><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;'></td><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3;'>Changed From</td><td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:center;background-color:#f3f3f3'>Changed to</td></tr>";
            }

            string[] tLines = inputList.Split('~');
            for (lp = 0; lp < tLines.GetUpperBound(0); lp++)
            {
                string[] thisLine = tLines[lp].Split('|');
                newTbl += "<tr>";
                if (headerType == "OrderCART")
                {
                    if ((bool)Page.Session["JPEapprovedForPricing"])
                    {
                        newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;'>" + thisLine[1] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>" + thisLine[2] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>" + thisLine[3] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>" + thisLine[4] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>$" + String.Format("{0:C}", thisLine[5]) + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>$" + String.Format("{0:C}", thisLine[6]) + "</td>";
                    }
                    else
                    {
                        newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;'>" + thisLine[1] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>" + thisLine[2] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>" + thisLine[3] + "</td>";
                        newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:right;'>" + thisLine[4] + "</td>";
                    }
                }
                else if (headerType == "OrderCompany")
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + "</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;'>" + thisLine[1] + "</td>";
                }
                else if (thisLine[1] == "new")
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + " [new]</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;' colspan = '2'>" + thisLine[2] + "</td>";
                }
                else if (thisLine[1] == "add")
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + " [add]</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;' colspan = '2'>" + thisLine[2] + "</td>";
                }
                else if (thisLine[1] == "note")
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + " [note]</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;' colspan = '2'>" + thisLine[2] + "</td>";
                }
                else
                {
                    newTbl += "<td style='font-family:verdana; font-size:12px;font-weight:600;color:#666666;text-align:right;background-color:#f3f3f3'>" + thisLine[0] + "</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;600;color:#021238;text-align:left;'>" + thisLine[1] + "</td>";
                    newTbl += "<td style='font-family:verdana; font-size:12px;color:#021238;text-align:left;'>" + thisLine[2] + "</td>";
                }
                newTbl += "</tr>";
            }
            newTbl += "</table>";
        }
        return newTbl;

    }

    protected void verifyEmail(string thisVerifyString, string thisRequestType)
    {
        string queryString = string.Empty;
        string conn = string.Empty;
        string conn2 = string.Empty;
        DataTable dttemp = GenUtils.ecom_AccountLookup("EVERIFY:" + thisVerifyString, -1);
        string myIP = Request.ServerVariables["REMOTE_ADDR"];
        if (dttemp.Rows.Count > -1)
        {
            queryString = "UPDATE ecom_UserAccounts SET emailValidated = '" + DateTime.Now + "' WHERE LOWER(eMailValidationString) = '" + thisVerifyString.ToLower() + "';";
            conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
            SqlConnection cacheQueueConn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(queryString, cacheQueueConn);

            try
            {
                cacheQueueConn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Updating Account File for Email Verification File : " + ex.ToString());
            }
            finally
            {
                cacheQueueConn.Close();
            }

            queryString = "INSERT INTO  ecom_VerificationEvents (Event, UserName, IP, DateTime) VALUES ('" + thisRequestType + "', '" + dttemp.Rows[0]["UserName"].ToString() + "', '" + myIP + "', '" + DateTime.Now + "');";
            conn2 = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
            SqlConnection cacheQueueConn2 = new SqlConnection(conn2);
            SqlCommand cmd2 = new SqlCommand(queryString, cacheQueueConn2);
            try
            {
                cacheQueueConn2.Open();
                cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Updating Cookie / Email Events File: " + ex.ToString());
            }
            finally
            {
                cacheQueueConn2.Close();
            }
            switch (thisRequestType)
            {
            case "emailVer":
                 lblEmailVerifyResults.Text = "Your email address has been verified.  Thank you.";
                break;
            default:
                lblEmailVerifyResults.Text = "This computer has been verified.  Thank you.";
                break;
            }
             HttpCookie cookie = Request.Cookies["JPEPreferences"];
              if (cookie == null)
              {
                  cookie = new HttpCookie("JPEPreferences");
              }
              cookie["JPEAccessCode"] = thisVerifyString;
              cookie.Expires = DateTime.Now.AddYears(5);
              Response.Cookies.Add(cookie);
        }
        else
        {
            lblEmailVerifyResults.Text = "Sorry, we were unable to verify that email address.";
        }
        ModalPopupExtenderEmailVerify.Show();
    }

    protected string makenwErrorPanel(string thisErrorList)
    {
        return  "<div class='nwAcctErrorDiv'><ul>" + thisErrorList + "</ul></div>";
    }

    public void SetshippingCarrierDDL(string strThisValue)
    {
        ddlShipperCarrier.Items.Clear();
        ddlShipperCarrier.Items.Add(new ListItem("U.P.S.", "ups"));
        ddlShipperCarrier.Items.Add(new ListItem("Federal Express", "fedex"));
        ddlShipperCarrier.Items.Add(new ListItem("DHL", "dhl"));
        ddlShipperCarrier.Items.Add(new ListItem("U.S.P.S.", "usps"));
        ddlShipperCarrier.Items.Add(new ListItem("Other", "other"));
        ddlShipperCarrier.Items.Add(new ListItem("Best Method", "best"));
        ddlShipperCarrier.Items.Insert(0, "Select Carrier");
        ddlShipperCarrier.Items[0].Value = "0"; 
        switch (strThisValue)
        {
            case "ups":
                ddlShipperCarrier.SelectedIndex = 1;
                break;
            case "fedex":
                ddlShipperCarrier.SelectedIndex = 2;
                break;
            case "dhl":
                ddlShipperCarrier.SelectedIndex = 3;
                break;
            case "usps":
                ddlShipperCarrier.SelectedIndex = 4;
                break;
            case "other":
                ddlShipperCarrier.SelectedIndex = 5;
                break;
            case "best":
                ddlShipperCarrier.SelectedIndex = 6;
                break;
            default:
                ddlShipperCarrier.SelectedIndex = 0;
                break;
        }
    }

    public void SetCompaniesforSalespersonDDL(string thisCompany, DataTable dtCompaniesforSalesperson)
    {
        ddlCompaniesforSalesperson.Items.Clear();
        ddlCompaniesforSalesperson.Items.Insert(0, "Standard Overhaul Pricing");
        ddlCompaniesforSalesperson.Items[0].Value = "-1";
        ddlCompaniesforSalesperson.Items.Insert(1, "Standard Airline Pricing");
        ddlCompaniesforSalesperson.Items[1].Value = "-2";
        int lpCounter = 1;
        foreach (DataRow myCompanyRow in dtCompaniesforSalesperson.Rows)
        {
            ddlCompaniesforSalesperson.Items.Add(new ListItem( myCompanyRow["COMPANY_NAME"].ToString(), myCompanyRow["CMP_AUTO_KEY"].ToString()));

            lpCounter++;
            if (myCompanyRow["CMP_AUTO_KEY"].ToString() == thisCompany)
            {
                ddlCompaniesforSalesperson.SelectedIndex = lpCounter;
            }
        }
        if (thisCompany == "-1")
        {
            ddlCompaniesforSalesperson.SelectedIndex = 0;
        }
        else if (thisCompany == "-2")
        {
            ddlCompaniesforSalesperson.SelectedIndex = 1;
        }
    }

    public void SetMyShipperidDDL(string strThisValue, DataTable dtShipVia)
    {
        ddlMyShipperID.Items.Clear();
        int lpCounter = -1;
        foreach (DataRow myShipperRow in dtShipVia.Rows)
        {
            lpCounter++;
            ddlMyShipperID.Items.Add(new ListItem(myShipperRow["DESCRIPTION"] + " " + myShipperRow["ACCOUNT_NUMBER"], myShipperRow["DESCRIPTION"] + " " + myShipperRow["ACCOUNT_NUMBER"]));
            if (myShipperRow["DESCRIPTION"] + " " + myShipperRow["ACCOUNT_NUMBER"] == strThisValue)
            {
                ddlMyShipperID.SelectedIndex = lpCounter;
            }
        } 
        ddlMyShipperID.Items.Insert(0, "Select My Shipper ID");
        ddlMyShipperID.Items[0].Value = "0";
    }

    public void SetMyShipToAddressDDL(DataTable dtCompany, DataTable dtSites, string thisSelected)
    {
        ddlMyShipToAddress.Items.Clear();
        pnlMyShipAddressDDL.Visible = false;
        string bldAddress = string.Empty;
        int matchCtr = 0;
        int intCtr = 1;
        

        if (dtCompany.Rows[0]["SHIP_ADDRESS1"].ToString() != string.Empty)
        {
            bldAddress = dtCompany.Rows[0]["SHIP_ADDRESS1"].ToString();
            if (dtCompany.Rows[0]["SHIP_ADDRESS2"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["SHIP_ADDRESS2"].ToString();

            }
            if (dtCompany.Rows[0]["SHIP_ADDRESS3"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["SHIP_ADDRESS3"].ToString();

            }
            bldAddress += ", " + dtCompany.Rows[0]["SHIP_CITY"].ToString();
            bldAddress += ", " + dtCompany.Rows[0]["SHIP_STATE"].ToString();
            if (dtCompany.Rows[0]["SHIP_COUNTRY"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["SHIP_COUNTRY"].ToString();

            }
            if (dtCompany.Rows[0]["SHIP_ZIP_CODE"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["SHIP_ZIP_CODE"].ToString();

            }
        }
        else
        {
            bldAddress = dtCompany.Rows[0]["ADDRESS1"].ToString();
            if (dtCompany.Rows[0]["ADDRESS2"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["ADDRESS2"].ToString();

            }
            if (dtCompany.Rows[0]["ADDRESS3"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["ADDRESS3"].ToString();

            }
            bldAddress += ", " + dtCompany.Rows[0]["CITY"].ToString();
            bldAddress += ", " + dtCompany.Rows[0]["STATE"].ToString();
            if (dtCompany.Rows[0]["COUNTRY"].ToString() != string.Empty)
            {
                bldAddress += ", " + dtCompany.Rows[0]["COUNTRY"].ToString();

            }
            if (dtCompany.Rows[0]["ZIP_CODE"].ToString() != string.Empty)
            {

                bldAddress += ", " + dtCompany.Rows[0]["ZIP_CODE"].ToString();

            }
        }

        ddlMyShipToAddress.Items.Add(new ListItem(bldAddress, bldAddress));
        if (bldAddress == thisSelected)
        {
            matchCtr = 1;
            
        }

        foreach (DataRow mySiteRow in dtSites.Rows)
        {
            bldAddress = mySiteRow["ADDRESS1"].ToString();
            if (mySiteRow["ADDRESS2"].ToString() != string.Empty)
            {
                bldAddress += ", " + mySiteRow["ADDRESS2"].ToString();

            }
            if (mySiteRow["ADDRESS3"].ToString() != string.Empty)
            {
                bldAddress += ", " + mySiteRow["ADDRESS3"].ToString();

            }
            bldAddress += ", " + mySiteRow["CITY"].ToString();
            bldAddress += ", " + mySiteRow["STATE"].ToString();
            if (mySiteRow["COUNTRY"].ToString() != string.Empty)
            {
                bldAddress += ", " + mySiteRow["COUNTRY"].ToString();

            }
            if (mySiteRow["ZIP_CODE"].ToString() != string.Empty)
            {
                bldAddress += ", " + mySiteRow["ZIP_CODE"].ToString();

            }
            ddlMyShipToAddress.Items.Add(new ListItem(bldAddress, bldAddress));
            intCtr++;
            if (bldAddress == thisSelected)
            {
                matchCtr = intCtr;
            }
        }

        ddlMyShipToAddress.Items.Insert(0, "Select My Ship To Address");
        ddlMyShipToAddress.Items[0].Value = "0";
        pnlMyShipAddressDDL.Visible = true;
        ddlMyShipToAddress.SelectedIndex = matchCtr;
    }
    
    public void SetMyCardTypeDDL(string strThisValue)
    {

        //MasterCard 51-55
        //Visa 4
        //AmEx 34,37

        ddlCardType.Items.Clear();

        if (Page.Session["blnAllowNetTerms"] == null)
        {
            Page.Session["blnAllowNetTerms"] = false;
        }

        if ((bool)Page.Session["blnAllowNetTerms"])
        {
            ddlCardType.Items.Add(new ListItem(Page.Session["TERM_DESCRIPTION"].ToString(), "NET"));
            ddlCardType.Items.Add(new ListItem("Cash On Delivery", "COD"));
            switch (strThisValue)
            {
                case "NET":
                    ddlCardType.SelectedIndex = 1;
                    break;
                case "COD":
                    ddlCardType.SelectedIndex = 2;
                    break;
                default:
                    break;
            }

        }
        else
        {
            ddlCardType.Items.Add(new ListItem("American Express", "AX"));
            ddlCardType.Items.Add(new ListItem("MasterCard", "MC"));
            ddlCardType.Items.Add(new ListItem("Visa", "VI"));
            ddlCardType.Items.Add(new ListItem("Cash On Delivery", "COD"));
            switch (strThisValue)
            {
                case "AX":
                    ddlCardType.SelectedIndex = 1;
                    break;
                case "MC":
                    ddlCardType.SelectedIndex = 2;
                    break;
                case "VI":
                    ddlCardType.SelectedIndex = 3;
                    break;
                case "COD":
                    ddlCardType.SelectedIndex = 4;
                    break;
                default:
                    break;
            }
        }
        ddlCardType.Items.Insert(0, "Select Payment Method");
        ddlCardType.Items[0].Value = string.Empty;
    }

    protected int findUpperElement(string[,] thisArray)
    {
        int upperElement = -1;
        int lp;
        for (lp = 0; lp <= 999; lp++)
        {
            if (thisArray[lp, 0] == null)
            {
                break;
            }
            else
            {
                upperElement++;
            }
        }
        return upperElement;
    }

    protected int findUpperElement(string[] thisArray)
    {
        int upperElement = -1;
        int lp;
        for (lp = 0; lp <= 999; lp++)
        {
            if (thisArray[lp] == null)
            {
                break;
            }
            else
            {
                upperElement++;
            }
        }
        return upperElement;
    }

    //************ Login Algorithms

    protected void UserLogin(string mode)
    {
        int minPasswordLength = 4;
        int minUserNameLength = 6;
        bool blnActiveAccount = false;
        int intMatchAccountKey = -1;
        bool blnSetRememberMe = false;

        Page.Session["AccountType"] = string.Empty;
        lblLoginError.Text = string.Empty;

        string myPassword = txtLoginPassword.Text.Trim();
        string myUserName = txtLoginUserName.Text.Trim();
        myUserName = myUserName.Replace(" ", "");
        string strDemoCo = string.Empty;
        Page.Session["DEMO_CMP_AUTO_KEY"] = -10;
        //-------- separate out the Company Code for JPE's and Admins
        if (myUserName.IndexOf("[") > 0 && myUserName.IndexOf("]") > 0)
        {
            if (myUserName.IndexOf("]") > myUserName.IndexOf("["))
            {
                strDemoCo = myUserName.Substring(myUserName.IndexOf("[") + 1, myUserName.IndexOf("]") - myUserName.IndexOf("[") - 1);
                if (GenUtils.IsNumber(strDemoCo))
                {
                    Page.Session["DEMO_CMP_AUTO_KEY"] = Convert.ToInt32(strDemoCo);
                    myUserName = myUserName.Substring(0, myUserName.IndexOf("["));
                }
            }
        }

        Page.Session["LoggedInUserID"] = -1;
        Page.Session["salesPersonAutoKey"] = -1;

        string myIP = Request.ServerVariables["REMOTE_ADDR"];
        DateTime currentDate = DateTime.Now;
        string strUserName = myUserName;
        DateTime noDate = DateTime.Parse("1/1/1900");
        if (myUserName.Length < minUserNameLength)
        {
            lblLoginError.Text = "Your User Name is your email address and must be at least " + minUserNameLength + " characters long.";
        }
        if (myPassword.Length < minPasswordLength)
        {
            if (lblLoginError.Text.Length > 1)
                lblLoginError.Text = "Your User Name is your email address and must be at least " + minUserNameLength + " characters long, and your Password must be at least " + minPasswordLength + " long.";
            else
                lblLoginError.Text = "Your Password must be at least " + minPasswordLength + " characters long.";
        }
        
        if (lblLoginError.Text.Length > 1)
        {
            lblLoginError.Text += " Please try again.";
            ModalPopupExtenderThrobber.Hide();
            return;
        }

           dtAccounts = GenUtils.ecom_AccountLookup(myUserName, -1);

            if (dtAccounts == null)
            {
                writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-NoUser");
                lblLoginError.Text += "The email address you entered is not valid.";
                ModalPopupShowBadEmail.Show();
            }
            else if (dtAccounts.Rows.Count < 1)
            {
                writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-NoUser");
                lblLoginError.Text += "The email address you entered is not valid.";
                ModalPopupShowBadEmail.Show();
            }
            else
            {
                DataRow r = dtAccounts.Rows[0];
                bool blnGoodRolodexKey = false;
                if (r["RoloDexKey"] != DBNull.Value)
                {
                    if (int.Parse(r["RoloDexKey"].ToString()) > 0)
                    {
                        blnGoodRolodexKey = true;
                    }
                }
               
                bool goodCookie = false;
                HttpCookie myAccessCookie = Request.Cookies["JPEPreferences"];
                if (myAccessCookie != null)
                {
                    if (myAccessCookie["JPEAccessCode"] != null)
                    {
                        if (myAccessCookie["JPEAccessCode"].ToString() == r["eMailValidationString"].ToString())
                        {
                            goodCookie = true;
                        }
                    }
                }

                if (!blnUserCookiesRequired)
                {
                    goodCookie = true;
                }

                string strMyAccountType = r["AccountType"].ToString().ToLower();
                if (strMyAccountType == "user" || strMyAccountType == "guest" ||
strMyAccountType == "jpe" || strMyAccountType == "admin")
                {
                    blnActiveAccount = true;
                }

                if (myPassword == r["Password"].ToString())
                {
                    if (blnActiveAccount && (blnGoodRolodexKey || strMyAccountType == "guest"))
                    {
                        Page.Session["LoggedInUserID"] = r["pkAccountKey"];
                        if (r["salesPersonAutoKey"] != DBNull.Value)
                        {
                            if ((int)r["salesPersonAutoKey"] > 0)
                            {
                                Page.Session["salesPersonAutoKey"] = r["salesPersonAutoKey"]; 
                            }
                        }
                        
                        Page.Session["UserName"] = r["EmailAddress"].ToString();
                        Page.Session["Password"] = r["Password"].ToString();
                        Page.Session["AccountType"] = r["AccountType"].ToString().ToLower();
                        Page.Session["pkAccountKey"] = r["pkAccountKey"];

                        Page.Session["JPEapprovedForPricing"] = true;
                        hdnpkAccountKey.Value = Page.Session["pkAccountKey"].ToString();

                        Page.Session["useHints"] = r["useHints"];
                        //   NEWWS    HiddenField myUseHints = (HiddenField)FindControl("hdnUseHints");
                        //myUseHints.Value = Page.Session["useHints"].ToString();

                        hdnUseHints.Value = Page.Session["useHints"].ToString();

                        intMatchAccountKey = (int)Page.Session["pkAccountKey"];

                        Page.Session["QuantumCompanyCode"] = string.Empty;
                        Page.Session["JPECompanyCode"] = string.Empty;
                        Page.Session["Quantum_CMP_AUTO_KEY"] = -1;
                        Page.Session["JPE_CMP_AUTO_KEY"] = -10;
                        Page.Session["RDX_CONTACT_NAME"] = string.Empty;
                        Page.Session["TMC_AUTO_KEY"] = string.Empty;
                        Page.Session["TERM_DESCRIPTION"] = string.Empty;
                        Page.Session["RolodexKey"] = r["RoloDexKey"].ToString();
                        Page.Session["Company_Type_for_Price"] = string.Empty;
                        Page.Session["BILL_ADDRESS1"] = string.Empty;
                        Page.Session["BILL_ADDRESS2"] = string.Empty;
                        Page.Session["BILL_ADDRESS3"] = string.Empty;
                        Page.Session["BILL_CITY"] = string.Empty;
                        Page.Session["BILL_STATE"] = string.Empty;
                        Page.Session["BILL_COUNTRY"] = string.Empty;
                        Page.Session["BILL_ZIP_CODE"] = string.Empty;

                        hdn_txtCardAddress1.Value = string.Empty;
                        hdn_txtCardAddress2.Value = string.Empty;
                        hdn_txtCardCity.Value = string.Empty;
                        hdn_txtCardState.Value = string.Empty;
                        hdn_txtCardZip.Value = string.Empty;


                        switch (r["useAutoCheck"].ToString())
                        {
                            case "True":
                                Page.Session["useAutoCheck"] = " checked ";
                                break;
                            default:
                                Page.Session["useAutoCheck"] = string.Empty;
                                break;
                        }

                        switch (r["useHints"].ToString())
                        {
                            case "True":
                                Page.Session["useHints"] = " checked ";
                                break;
                            default:
                                Page.Session["useHints"] = string.Empty;
                                break;
                        }
                        Page.Session["hxFromDate"] = string.Empty;
                        Page.Session["hxToDate"] = string.Empty;
                        Page.Session["hxPartNo"] = string.Empty;

                        if (cbRememberMe.Checked)
                        {
                            blnSetRememberMe = true;
                        }

                        if (strMyAccountType == "guest")
                        {
                             Page.Session["RDX_CONTACT_NAME"] = r["FirstName"].ToString();
                             Page.Session["JobTitle"] = r["JobTitle"].ToString();
                             Page.Session["RDXPhoneNumber"] = r["Phone"].ToString();
                             Page.Session["EmailAddress"] = r["EmailAddress"].ToString();
                             Page.Session["Company"] = r["Company"].ToString() + "[Guest]";
                             Page.Session["Company_Type_for_Price"] = "OVERHAUL";
                            if (r["PricingView"] != DBNull.Value)
                            {
                                Page.Session["Company_Type_for_Price"] = r["PricingView"].ToString().ToUpper();
                            }
                            if (r["PricingView"].ToString().ToUpper() != "CUSTOM" && r["PricingView"].ToString().ToUpper() != "AIRLINE" && r["PricingView"].ToString().ToUpper() != "OVERHAUL")
                            {
                                Page.Session["JPEapprovedForPricing"] = false;
                            }
                        }
                        writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-Success");
                        //--------------------------- Load the User Variables
                        LoadUserVariables(string.Empty, string.Empty);
                        if (blnshowDebuggerOutput)
                            GenUtils.PrintTableOrView(dtCompany, "dtCompany");

                        Page.Session["PanelMode"] = "login";
                        blnLogLogin = true;
                    }
                    else // ---------------- account not active
                    {

                        if (blnActiveAccount && !blnGoodRolodexKey && Page.Session["AccountType"].ToString() != "guest")
                        {
                            lblLoginError.Text += "Please call JPE at (206) 281-0963.  There is no Rolodex key for your account.  We apologize for the inconvenience.";
                            writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-NoRolodex");
                        }
                        else
                        {
                            if (strMyAccountType == "closed")
                            {
                                lblLoginError.Text += "<B>Your account has been denied.</B><br><br>  If you feel that this is inappropriate please give us a call at (206) 281-0963.";
                                writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-Closed");
                            }
                            else
                            {
                                lblLoginError.Text += "<B>Your account is not active at this time.</B><br><br>  If you have recently requested the account, please allow one to two business days for setup; otherwise please give us a call at (206) 281-0963.";
                                writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-AcctInactive");
                            }
                        }
                        ModalPopupExtenderThrobber.Hide();
                        SetSessionOnLogout();
                        blnSetRememberMe = false;
                        return;
                    }
                }
                else
                {
                    lblLoginError.Text = "The password you entered is incorrect. ";
                    writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-WrongPW");
                    lblLoginError.Text += " Please try again.";
                    ModalPopupExtenderThrobber.Hide();
                    return;
                }

                bool blnTestingIP = false;
                string[] strIps = strOurIPs.Split('|');
                int lp = 0;
                for (lp = 0; lp <= strIps.GetUpperBound(0); lp++)
                {
                    if (Request.ServerVariables["REMOTE_ADDR"].ToString() == strIps[lp])
                    {
                        blnTestingIP = true;
                        break;
                    }
                }

                if (!goodCookie && r["EmailAddress"].ToString() != string.Empty && r["AccountType"].ToString().ToLower() != "admin" && !blnTestingIP)
                {
                    //---------- bad cookie routine
                    string thisSubject, emailBody;
                    lblLoginError.Text = "You need to verify your User Name for this computer. We have emailed a link to at " + dtRolodex.Rows[0]["EMAIL_ADDRESS"].ToString() + ".  Please access your email and either click the link in the email, or paste the link into the address bar of your browser.  This will place a small cookie code onto your computer allowing you to aceess your Smart Part Buyers account from this computer.";
                    writeLoginQueue(strUserName, myIP, currentDate, intMatchAccountKey, noDate, "login-NoCookie");
                    thisSubject = "JPE Smart Buyer Computer Validation";
                    emailBody = "Hello " + dtRolodex.Rows[0]["RDX_CONTACT_NAME"].ToString() + ";<br /><br />";
                    emailBody += "In order to access the Jet Parts Engineering Smart Buyer eCommerce system, you must register your computer.<br /><br /> ";
                    emailBody += "Please click the link below, or copy and past it into the address bar of your browser.  This will place a small cookie code onto your computer allowing you to access your Smart Part Buyers account from this computer.<br/> <br/>";
                    emailBody += "<a href='http://jpesmartbuyer.com/ecom_prodSearch.aspx?v2=" + r["eMailValidationString"] + "'>http://jpesmartbuyer.com/ecom_prodSearch.aspx?v2=" + r["eMailValidationString"] + "</a><br/><br/>";
                    sendMail(dtGlobalAdmin.Rows[0]["defaultEmailFrom"].ToString(), dtRolodex.Rows[0]["EMAIL_ADDRESS"].ToString(), thisSubject, emailBody, true, false, "");
                    SetSessionOnLogout();
                    blnSetRememberMe = false;
                    string strCompanyCode = string.Empty;
                    Page.Session["PanelMode"] = "login";
                }

                HttpCookie cookie = Request.Cookies["JPEPreferences"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("JPEPreferences");
                }
                cookie["JPEUserName"] = blnSetRememberMe ? Page.Session["UserName"].ToString() : string.Empty;
                cookie.Expires = DateTime.Now.AddYears(5);
                Response.Cookies.Add(cookie);
                ModalPopupExtenderThrobber.Hide();
            }

            if (lblLoginError.Text.Length > 1)
            {
                ModalPopupExtenderThrobber.Hide();
                SetSessionOnLogout();
                blnSetRememberMe = false;
                string strCompanyCode = string.Empty;
                Page.Session["PanelMode"] = "login";
            }
            else
            {
                if ((int)Page.Session["DEMO_CMP_AUTO_KEY"] > 0)
                {
                    //----event handler for JPE company selector changed
                    Page.Session["JPE_CMP_AUTO_KEY"] = Page.Session["DEMO_CMP_AUTO_KEY"];

                    if (dtCacheMaster != null)
                    {
                        dtCacheMaster.Rows.Clear();
                    }
                }
            }
    }

    protected void LoadUserVariables(string strMyAddressSel, string strMyShipperID)
    {
        DataTable dtCompaniesforSalesperson = null;
        int intKeyOverride = -10;
        if (Page.Session["AccountType"].ToString() == "jpe" || Page.Session["AccountType"].ToString() == "admin")
        {
            if (GenUtils.IsNumber(Page.Session["JPE_CMP_AUTO_KEY"].ToString()))
            {
                if (Convert.ToInt16(Page.Session["JPE_CMP_AUTO_KEY"].ToString()) > -10)
                {
                    intKeyOverride = Convert.ToInt16(Page.Session["JPE_CMP_AUTO_KEY"]);
                }
            }
        }

        if (Page.Session["AccountType"].ToString() == "guest")
        {
            Page.Session["JPE_CMP_AUTO_KEY"] = -10;
            Page.Session["DEMO_CMP_AUTO_KEY"] = -10;
            Page.Session["Quantum_CMP_AUTO_KEY"] = -1;
            Page.Session["TMC_AUTO_KEY"] = -1;
            Page.Session["TERM_DESCRIPTION"] = "C.O.D.";
            Page.Session["Company"] = "Guest";
            Page.Session["BILL_ADDRESS1"] = string.Empty;
            Page.Session["BILL_ADDRESS2"] = string.Empty;
            Page.Session["BILL_ADDRESS3"] =string.Empty;
            Page.Session["BILL_CITY"] = string.Empty;
            Page.Session["BILL_STATE"] = string.Empty;
            Page.Session["BILL_COUNTRY"] = string.Empty;
            Page.Session["BILL_ZIP_CODE"] = string.Empty;

            hdn_txtCardAddress1.Value = string.Empty;
            hdn_txtCardAddress2.Value = string.Empty;
            hdn_txtCardCity.Value = string.Empty;
            hdn_txtCardState.Value = string.Empty;
            hdn_txtCardZip.Value = string.Empty;
            Page.Session["BILL_TO_STRING"] = string.Empty;
            lblSalesPersonTable.Text = GenUtils.BuildSalespersonPanel(dtCompany);
            Session["SalesPersonPanel"] = lblSalesPersonTable.Text;

            return;
        }

        if (Page.Session["AccountType"].ToString() == "jpe")
        {
                dtCompaniesforSalesperson = GenUtils.CompaniesforSalesperson((int)Page.Session["salesPersonAutoKey"]);
                SetCompaniesforSalespersonDDL(Page.Session["JPE_CMP_AUTO_KEY"].ToString(), dtCompaniesforSalesperson);
        }
        else if (Page.Session["AccountType"].ToString() == "admin")
        {
            dtCompaniesforSalesperson = GenUtils.CompaniesforSalesperson(-1);
            SetCompaniesforSalespersonDDL(Page.Session["JPE_CMP_AUTO_KEY"].ToString(), dtCompaniesforSalesperson);
        }
        blnUserLoaded = true;
      
        // Load a company record to the dt for pricing
        dtRolodex = GenUtils.RoloDexLookup(Page.Session["RolodexKey"].ToString());

        if (dtRolodex != null)
        {
            if (dtRolodex.Rows[0]["COMPANY_CODE_UPPER"].ToString() != string.Empty)
            {
             Page.Session["QuantumCompanyCode"] = dtRolodex.Rows[0]["COMPANY_CODE_UPPER"].ToString();
             Page.Session["CMP_AUTO_KEY"] = dtRolodex.Rows[0]["CMP_AUTO_KEY"].ToString();
                Page.Session["RDX_CONTACT_NAME"] = dtRolodex.Rows[0]["RDX_CONTACT_NAME"].ToString();
                Page.Session["JobTitle"] = dtRolodex.Rows[0]["TITLE"].ToString();
                Page.Session["RDXPhoneNumber"] = dtRolodex.Rows[0]["PHONE_NUMBER"].ToString();
                Page.Session["EmailAddress"] = dtRolodex.Rows[0]["EMAIL_ADDRESS"].ToString();
                dtCompany = GenUtils.CompanyLookup(Page.Session["CMP_AUTO_KEY"].ToString(), Page.Session["strSalesPictureRootPath"].ToString(), intKeyOverride);
                dtSites = GenUtils.CompanySitesLookup(Page.Session["CMP_AUTO_KEY"].ToString(), intKeyOverride);
                pnlMyShipAddressDDL.Visible = false;
                pnlMyShipperDDL.Visible = false;
                if (dtCompany.Rows.Count > 0 && dtSites.Rows.Count > 0)
                {
                    SetMyShipToAddressDDL(dtCompany, dtSites, strMyAddressSel);
                    Page.Session["Company_Type_for_Price"] = "CUSTOM";
                    // custom pricing only for rfq customers, and competitors
                    if (dtCompany.Rows[0]["CV_UDF_002"].ToString() == "T") 
                    {
                        Page.Session["Company_Type_for_Price"] = "OVERHAUL";
                    }
                    else  if (dtCompany.Rows[0]["CV_UDF_001"].ToString() == "T")  //---- 001 True for airlines
                    {
                        Page.Session["Company_Type_for_Price"] = "AIRLINE";
                    }

                    //--------------------------- JPE Employees Override
                    if (intKeyOverride == -1)
                    {
                         Page.Session["Company_Type_for_Price"] = "OVERHAUL";
                    }
                    else if (intKeyOverride == -2)
                    {
                        Page.Session["Company_Type_for_Price"] = "AIRLINE";
                    }

                    // CV_UDF_001 true for airlines
                    // CV_UDF_002 true for overhaul shop
                    // CV_UDF_003 true for competitor
                    // CV_UDF_004 true for broker / distributor
                    // CV_UDF_005 true for other

                    //------------------20100430
                    Page.Session["QuantumCompanyCode"] = dtCompany.Rows[0]["COMPANY_CODE"].ToString();
                    int thisCompanyKey = int.Parse(dtCompany.Rows[0]["CMP_AUTO_KEY"].ToString());
                    Page.Session["Quantum_CMP_AUTO_KEY"] = int.Parse(dtCompany.Rows[0]["CMP_AUTO_KEY"].ToString());
                    Page.Session["TMC_AUTO_KEY"] = dtCompany.Rows[0]["TMC_AUTO_KEY"].ToString();
                    Page.Session["TERM_DESCRIPTION"] = dtCompany.Rows[0]["DESCRIPTION"].ToString();
                    Page.Session["Company"] = dtCompany.Rows[0]["COMPANY_NAME"].ToString();
                    Page.Session["BILL_ADDRESS1"] = dtCompany.Rows[0]["ADDRESS1"].ToString();
                    Page.Session["BILL_ADDRESS2"] = dtCompany.Rows[0]["ADDRESS2"].ToString();
                    Page.Session["BILL_ADDRESS3"] = dtCompany.Rows[0]["ADDRESS3"].ToString();
                    Page.Session["BILL_CITY"] = dtCompany.Rows[0]["CITY"].ToString();
                    Page.Session["BILL_STATE"] = dtCompany.Rows[0]["STATE"].ToString();
                    Page.Session["BILL_COUNTRY"] = dtCompany.Rows[0]["COUNTRY"].ToString();
                    Page.Session["BILL_ZIP_CODE"] = dtCompany.Rows[0]["ZIP_CODE"].ToString();

                    hdn_txtCardAddress1.Value = dtCompany.Rows[0]["ADDRESS1"].ToString();
                    hdn_txtCardAddress2.Value = dtCompany.Rows[0]["ADDRESS2"].ToString();
                    hdn_txtCardCity.Value = dtCompany.Rows[0]["CITY"].ToString();
                    hdn_txtCardState.Value = dtCompany.Rows[0]["STATE"].ToString();
                    hdn_txtCardZip.Value = dtCompany.Rows[0]["ZIP_CODE"].ToString();
                    Page.Session["BILL_TO_STRING"] = Page.Session["Company"].ToString() + "<br />" + Page.Session["BILL_ADDRESS1"].ToString();
                  
                    if (Page.Session["BILL_ADDRESS2"].ToString().Trim() != string.Empty)
                    {
                        Page.Session["BILL_TO_STRING"] += ", " + Page.Session["BILL_ADDRESS2"].ToString();
                    }
                    if (Page.Session["BILL_ADDRESS3"].ToString().Trim() != string.Empty)
                    {
                        Page.Session["BILL_TO_STRING"] += ", " + Page.Session["BILL_ADDRESS3"].ToString();
                    }
                    if (Page.Session["BILL_CITY"].ToString().Trim() != string.Empty)
                    {
                        Page.Session["BILL_TO_STRING"] += "<br />" + Page.Session["BILL_CITY"].ToString();
                    }
                    if (Page.Session["BILL_STATE"].ToString().Trim() != string.Empty)
                    {
                        Page.Session["BILL_TO_STRING"] += ", " + Page.Session["BILL_STATE"].ToString();
                    }

                    if (Page.Session["BILL_ZIP_CODE"].ToString().Trim() != string.Empty)
                    {
                        Page.Session["BILL_TO_STRING"] += ", " + Page.Session["BILL_ZIP_CODE"].ToString();
                    }

                    dtShipVia = GenUtils.CompanyShippingLookup(thisCompanyKey);

                    if (dtShipVia.Rows.Count > 0)
                    {
                        SetMyShipperidDDL(strMyShipperID, dtShipVia);
                        pnlMyShipperDDL.Visible = true;
                    }
                    lblSalesPersonTable.Text = GenUtils.BuildSalespersonPanel(dtCompany);
                    Session["SalesPersonPanel"] = lblSalesPersonTable.Text;

                    //-------- Build the 
                    txtOrdersFromDate.Text = Page.Session["hxFromDate"].ToString();
                    txtOrdersToDate.Text = Page.Session["hxToDate"].ToString();
                    txtOrdersPONumber.Text = Page.Session["hxPartNo"].ToString();
                    string temp = string.Empty;

                    if (lblOrderHistoryTable.Text == string.Empty)
                    {
                        temp += "<!-- Begin Table Viewport Area -->";
                        temp += "<table class='GenViewPortTable'>";
                        temp += "<tr><td>";
                        temp += "<table class='cartTable'  style='width:550px;'>";
                        temp += "<tr><td colspan='11' class='cartRowSpacer'></td></tr>";
                        temp += "<!-- Header Row -->";
                        temp += "<tr><td style='font-family:verdana; color:#999999;size:11pt;font-weight:600;text-align:center;width:100%;background-color:white;' colspan = '9'>Click Search to view recent orders / rfq's, or enter search criteria</td></tr>";
                        lblOrderHistoryTable.Text = temp;
                    }
                    dtOrderHistory = GenUtils.BuildCompleteOrderHistory(thisCompanyKey, Page.Session["hxFromDate"].ToString(), Page.Session["hxToDate"].ToString(), Page.Session["hxPartNo"].ToString(), "", dtGlobalAdmin.Rows[0]["HistoryShowDays"].ToString());

                    dtWebsiteOrderHistory = GenUtils.BuildWebsiteOrderHistory(thisCompanyKey, Page.Session["hxFromDate"].ToString(), Page.Session["hxToDate"].ToString(), Page.Session["hxPartNo"].ToString(), "", dtGlobalAdmin.Rows[0]["HistoryShowDays"].ToString());

                    dtRFQHistory = GenUtils.BuildMyQuotesHistory(thisCompanyKey, Page.Session["hxFromDate"].ToString(), Page.Session["hxToDate"].ToString(), Page.Session["hxPartNo"].ToString(), "", dtGlobalAdmin.Rows[0]["HistoryShowDays"].ToString());

                    lblOrderHistoryTable.Text = BuildOrderHistoryPanel(dtOrderHistory, dtRFQHistory, dtWebsiteOrderHistory);
                    upAccountHistory.Update();
                    lblCurrentCompanyUsage.Text = string.Empty;
                    //-----if user is admin mode 
                    if ((Page.Session["AccountType"].ToString() == "jpe" || Page.Session["AccountType"].ToString() == "admin") && thisCompanyKey.ToString() != "776")
                    {
                        dtCompanyUsageStats = GenUtils.BuildCompanyWebsiteUseHistory(thisCompanyKey);
                        lblCurrentCompanyUsage.Text = BuildCurrentCompanyUsagePanel(dtCompanyUsageStats);
                    }
                    if (Page.Session["AccountType"].ToString() == "admin")
                    {
                        dtAdminUsers = GenUtils.loadAdminUsersTable(0);
                        lblAdminTable.Text = BuildAdminPanel(dtAdminUsers);
                    }
                    else if (Page.Session["AccountType"].ToString() == "jpe" && (int)Page.Session["SalesPersonAutoKey"] > 0)
                    {
                        dtAdminUsers = GenUtils.loadAdminUsersTable((int)Page.Session["SalesPersonAutoKey"]);
                        lblAdminTable.Text = BuildAdminPanel(dtAdminUsers);
                    }
                    //------------------------ Look for Smart Stock
                    string thisStatus = string.Empty;
                    lblSmartStock.Text = "<TABLE border='1' style='padding-left:5px;padding-right:5px;font-family:verdana;font-dize:9px;color:#999999;'>";
                    lblSmartStock.Text += "<tr><td colspan='3' style='background-color:#666666;color:white;text-align:center;font-family:verdana;font-weight:600;font-size:11px;'>JPE Smart Stock Scanned Invoice Status</td></tr>";
                    lblSmartStock.Text += "<tr><td style='background-color:#999999;color:white;text-align:center;font-family:verdana;font-size:10px;text-align:center;'>Invoice No.</td><td style='background-color:#999999;color:white;text-align:center;font-family:verdana;font-size:10px;text-align:center;'>Date / Time</td><td style='background-color:#999999;color:white;text-align:center;font-family:verdana;font-size:10px;text-align:center;'>Status</td></tr>";
                    DataTable dtSmartStock = GenUtils.loadSmartStockHistory(thisCompanyKey);
                    if (dtSmartStock != null)
                    {
                        if (dtSmartStock.Rows.Count > 0)
                        {
                            foreach (DataRow mySSRow in dtSmartStock.Rows)
                            {
                                thisStatus = string.Empty;
                                if (mySSRow["eMailSent"] != null)
                                {
                                    if (GenUtils.IsDate(mySSRow["eMailSent"].ToString()))
                                    {
                                        thisStatus = "Posted on " + string.Format("{0:MM/dd/yyyy}", mySSRow["eMailSent"]);
                                    }
                                    else
                                    {
                                        thisStatus = "Scanned but Not Posted";
                                    }
                                }
                                else
                                {
                                    thisStatus = "Scanned but Not Posted";
                                }
                                lblSmartStock.Text += "<tr><td style='padding-left:5px;padding-right:5px;'>" + mySSRow["InvoiceNumber"].ToString() + "</td><td style='padding-left:5px;padding-right:5px;'>" + string.Format("{0:MM/dd/yyyy}", mySSRow["DateTime"]) + "</td><td style='white-space:nowrap; padding-left:5px;padding-right:5px;''>" + thisStatus + "</td></tr>";
                            }
                            blnSmartStockFound = true;
                            lblSmartStock.Text += "</TABLE>";
                        }
                    }
                }
            }
        }
        lclCurrentCompany.Text = string.Empty;

        if (intKeyOverride == -1 || intKeyOverride == -2)
        {
            lclCurrentCompany.Text = "<ul>";
            lclCurrentCompany.Text += "<li>Pricing Mode: ";
            if (intKeyOverride == -2)
            {
                lclCurrentCompany.Text += " Airline </li>";
            }
            else
            {
                lclCurrentCompany.Text += " Overhaul </li>";
            }
            lclCurrentCompany.Text += "</ul>";
        }
            
        else if (intKeyOverride > 1)
        {
            lclCurrentCompany.Text = "<ul>";
            lclCurrentCompany.Text += "<li>Company: " + Page.Session["Company"] + "</li>";
            lclCurrentCompany.Text += "<li>Terms: " + Page.Session["TERM_DESCRIPTION"] + "</li>";
            lclCurrentCompany.Text += "<li>BillAddress: " + Page.Session["BILL_TO_STRING"] + "</li>";
            lclCurrentCompany.Text += "<li>Pricing Type: " + Page.Session["Company_Type_for_Price"] + "</li>";
            lclCurrentCompany.Text += "</ul>";
        }
    }

    protected void UserLogOut()
    {
        SetSessionOnLogout();
        string strCompanyCode = string.Empty;
        Page.Session["PanelMode"] = "login";
        ModalPopupExtenderThrobber.Hide();
    }

    //************ New Data Cache Objects

    protected string[,] PartsCacheManager(string CallMode, string AddPartsList)
    {

       
        string scrapeType = "base";
        bool blnPopThisNHA = false;
        int swCounter = -1;

        // ------ definitions for these three  arrays
        //      0   part number TRIMMED, STRIPPED and ToUpper
        //      1   Qunatity ordered -- trimmed and run through isNumber 
        //      2   Source (SingleMatch, NHAMatch, SearchPanel, List)
        //      5 the original search string for re-display


        //----- search values is an public array used to repopulate the controls an an aaborted search



        if (CallMode == "AddParts" )
        {
            AddPartsCounter = -1;
            SPBScreenScraper("AddParts", AddPartsList, scrapeType, blnThisisNewSearch);
            CacheCounter += AddPartsCounter;

        }
        else
        {



            strSearchList = string.Empty;
            cartTlQty = 0;
            cartTlAmount = 0m;
            lblSteveDebug.Text = string.Empty;


            SPBmasterSEARCHList = new string[1000, 7];
            SPBmasterSCRAPEList = new string[1000, 7];
            SPBmasterUNIFIEDList = new string[1000, 7];
            SPBmasterSEARCHCnt = -1;
            SPBmasterSCRAPECnt = -1;
            SPBmasterUNIFIEDCnt = -1;

            searchValues = new string[7, 2];
           
            blnThisisNewSearch = false;

            blnExcelLimitExceeded = false; //---- for the warning label
            blnShortSearchWord = false; //--- set if any search word is too short
            
            




        //--------------------- is this postback from the details popup or the main panel
       
        if (PartsPopupUpdate && CallMode != "AddParts")
        {
            scrapeType = "detailPopup";
        }

        //----------- is this postback triggered by clicking the search button (a new search)
       
        if (Request.Form["txtSearch_PartNo_1"] != null || Request.Form["txtSearch_Assembly"] != null)
        {
            if ((Request.Form["txtSearch_PartNo_1"].Trim() != string.Empty || Request.Form["txtSearch_PartNo_2"].Trim() != string.Empty || Request.Form["txtSearch_PartNo_3"].Trim() != string.Empty || Request.Form["txtSearch_PartNo_4"].Trim() != string.Empty || Request.Form["txtSearch_PartNo_5"].Trim() != string.Empty || listSearchBox.InnerText != string.Empty || Request.Form["txtSearch_Assembly"].Trim() != string.Empty) && scrapeType != "detailPopup")
            {
                blnThisisNewSearch = true;
            }
        }

        //------- flag set when NHA popup is called from thebase page
        if (blnNHADetailPopupSearch)
        {
            blnThisisNewSearch = false;
        }

        // ------- scrape the screen  and build 2 lists
        //------- this method takes the postback info and populates it into the SCRAPEList and
        //      SEARCHList

               SPBScreenScraper("ScreenScrape", AddPartsList, scrapeType, blnThisisNewSearch);


        //---------------- Excel Limit Exceded Error Panel
        if (blnExcelLimitExceeded)
        {
            lblSteveDebug.Text = makenwErrorPanel("We're sorry, but pasted list part searches are limited to " + intlimitExcelParts + " and the list you pasted exceeded that number.  Only the first " + intlimitExcelParts + " parts on your list are being displayed");
        }

        //----- clear the last search if this is an add to cart action
        //------ true if the Add to Cart Button clicked
        if (blnAddToCart)
        {
            Page.Session["strSearchList"] = string.Empty;
            Page.Session["strNHAPartialMatches"] = string.Empty;
            Page.Session["strAssemblySearch"] = string.Empty;
            Page.Session["strLastNHASearchString"] = string.Empty;
        }


        //-------- build a Page Session string with the current key words if this isn't a new search
        //      this assures that out unified array always includes the current scrape and the 
        //      search words and the Search List has the search words
       
       
        int i, s;


        if (blnThisisNewSearch)
        {
            while (SPBmasterSEARCHList[++swCounter, 0] != null)
            {
                for (i = 0; i < 7; i++)
                {
                    strSearchList += SPBmasterSEARCHList[swCounter, i] + "|";
                }
                strSearchList += "~";
            }
            Page.Session["strSearchList"] = strSearchList;
            //---- if new search log the strings searched for
            if (blnThisisNewSearch)
            {
                logSearchWordUsed(SPBmasterSEARCHList, Request.Form["txtSearch_Assembly"].ToString());
            }
            if (!blnNHADetailPopupSearch)
            {
                Page.Session["strNHAPartialMatches"] = string.Empty;
                Page.Session["strAssemblySearch"] = string.Empty;
                Page.Session["strLastNHASearchString"] = string.Empty;
            }
        }
        else if (!blnAddToCart && !blnAvlButtonclick)
        //--------- otherwise reconstrunct the SEARCH array from the Page Session
        {
            if (string.IsNullOrEmpty(Page.Session["strSearchList"].ToString()) != true)
            {
                strSearchList = Page.Session["strSearchList"].ToString();
                string[] swtemp = strSearchList.Split('~');
                for (i = 0; i <= swtemp.GetUpperBound(0); i++)
                {
                    string[] swcols = swtemp[i].Split('|');
                    if (swcols.GetUpperBound(0) > 4)
                    {
                        for (s = 0; s < 7; s++)
                        {
                            SPBmasterSEARCHList[i, s] = swcols[s];
                        }
                    }
                }
            }
        }

       
        //----------------------- Now look for information in the NHA popup control then set variables to display the popup
        if (string.IsNullOrEmpty((string)Request.Form["txtSearch_Assembly"]) != true)
        {
            if (Request.Form["txtSearch_Assembly"].ToString().Length > 4)
            {
                if (Request.Form["txtSearch_Assembly"].ToString().Substring(0, 4) == "POP:")
                {
                    blnPopThisNHA = true;
                    Page.Session["strAssemblySearch"] = Request.Form["txtSearch_Assembly"].ToString().Trim().Substring(4);
                }
                else
                {
                    Page.Session["strAssemblySearch"] = Request.Form["txtSearch_Assembly"].ToString().Trim();
                    Page.Session["strLastNHASearchString"] = Request.Form["txtSearch_Assembly"].ToString().Trim();
                }
            }
            else
            {
                Page.Session["strAssemblySearch"] = Request.Form["txtSearch_Assembly"].ToString().Trim();
                Page.Session["strLastNHASearchString"] = Request.Form["txtSearch_Assembly"].ToString().Trim();
            }
        }
        //---------------------- end of NHA Pop up area

        CacheCounter += swCounter;

        } // addParts




        AddPartstoCache("search", CacheCounter, blnPopThisNHA, strSearchList);



        //---------- Write the the cache array to the DBase for use by modal processes
        updateTheCache(dtCacheMaster, Page.Session["MySessionId"].ToString(), "", strSearchList);




        //------------- empty the search box
        listSearchBox.Value = string.Empty;
        return null;
    }

    protected void BuildTheDisplay()
    {
        
        //------ Build the Smart Buyer display panel and attach it to the PlaceHolder
        lblSmartBuyerPanel = new Label();
        lblSmartBuyerPanel.Text = buildSmartBuyerPanel(dtCacheMaster, SPBmasterUNIFIEDList, CacheCounter);
        phSmartBuyerPanel.Controls.Add(lblSmartBuyerPanel);

    }

    protected string AddPartstoCache(string callMode, int swCounter, bool blnPopThisNHA, string strSearchList)
    {



        string temp = string.Empty;



        string[,] nhaList = new string[1000, 6];
        int[] imageList = new int[1000];
        string[,] MasterPartsArray = new string[1000, 6];

        DataTable dtNHA = null;
        DataTable dtOraclePartsMaster = null;

        int lp;
        bool blnqtyChanged1 = false;  // does the original scrape have qty changes from the cache




        //------- merge the SEARCHList and teh SCRAPEList into the Unified List
        SPBmasterUNIFIEDList = BuildUNIFIEDList(SPBmasterSEARCHList, SPBmasterSCRAPEList, blnThisisNewSearch);


        //-------- the scraper for the search panel can return a quantity error if qty required (not currently used)
        lblSearchQtyError.Text = string.Empty;
        if (SPBmasterSEARCHList[0, 0] == "QTY_ERROR")
        {
            lblSearchQtyError.Text = "<span style='color:red;'>Please enter a quantity for: <ul>";

            for (lp = 0; lp < 10; lp++)
            {
                if (SPBmasterSEARCHList[lp, 1] == null)
                {
                    break;
                }
                else
                {
                    searchValues[lp, 0] = SPBmasterSEARCHList[lp, 1];
                    searchValues[lp, 1] = SPBmasterSEARCHList[lp, 2];
                    lblSearchQtyError.Text += "<li>" + SPBmasterSEARCHList[lp, 1] + "</li>";
                }
            }
            searchValues[5, 0] = Request.Form["txtSearch_Assembly"];
            lblSearchQtyError.Text += "</ul></span>";
            return "NoTable";;
        }
        else if (blnShortSearchWord)
        {
            lblSearchQtyError.Text = "<span style='color:red;'>All Part Numbers must be at least 4 characters in length.  Please correct: <ul>";
            for (lp = 0; lp < 10; lp++)
            {
                if (SPBmasterSEARCHList[lp, 1] == null)
                {
                    break;
                }
                else
                {
                    if (SPBmasterSEARCHList[lp, 2] == "SearchPanel")
                    {
                        searchValues[lp, 0] = SPBmasterSEARCHList[lp, 0];
                        searchValues[lp, 1] = SPBmasterSEARCHList[lp, 1];
                    }
                }

            }
            searchValues[5, 0] = Request.Form["txtSearch_Assembly"];
            lblSearchQtyError.Text += "</span>";
            return "NoTable";
        }
        //--------- end of qty required exit method


        //------------------------ Clear the Cache   --- SPEED - this is being done too many times
        if (dtCacheMaster != null)
        {
            dtCacheMaster.Rows.Clear();
        }
        //-----------------------recall the current Cache from the ActiveCache Table
        dtCacheMaster = GenUtils.GetCacheforSessionID(Page.Session["MySessionId"].ToString());
        if (dtCacheMaster == null)  //--------- define the cache if it is null
        {
            dtCacheMaster = defineCacheMaster(true);
        }
        else
        {
            defineCacheMaster(false);
        }

        //------------------- boolean - are any of the scraped quantities different from the Cache
        //   SPEED   ---  is this needed - this is not needed if I am updating the cache every time anyway
        blnqtyChanged1 = updateQtyFromScraper(SPBmasterUNIFIEDList);


        //--------------- Remove duplicates from the list (is this really needed?)
        //      and if it is needed - isn't it needed for all thre arrays?
        SPBmasterUNIFIEDList = RemoveDuplicates(SPBmasterUNIFIEDList);


        //----------------------- determine the upper populated element of the UNIFIED List
        swCounter = findUpperElement(SPBmasterUNIFIEDList);



        intNHAPopUpMatchesFound = 0;
        singleNHAMatch = string.Empty;
        singleNHAMatchtoPrint = string.Empty;

        //------ Locate the NHA Parts to the UNIFIED List
        dtNHA = FindAllNHAItems(SPBmasterUNIFIEDList, blnPopThisNHA);

        //------- Add the NHA Records to the Master Cache table
        nhaList = addNHAtoMasterTable(dtNHA, Page.Session["MySessionId"].ToString());

        //------- produce a clean master parts array for the query
        MasterPartsArray = partsListRemoveDups(SPBmasterUNIFIEDList, nhaList);



        //------- set the stage for the NHA Assembly modal popup to open after the
        //  SmartBuyer panel has been updated


        //   NEWWS    HiddenField hdnAssembly = (HiddenField)FindControl("hdn_AssemblySearch");
        //hdnAssembly.Value = string.Empty;
        hdn_AssemblySearch.Value = string.Empty;

        if (string.IsNullOrEmpty(Page.Session["strAssemblySearch"].ToString()) != true)
        {
            if (intNHAPopUpMatchesFound == 1)
            {
                hdn_AssemblySearch.Value = singleNHAMatchtoPrint;
            }
            else if (intNHAPopUpMatchesFound > 1)
            {
                hdn_AssemblySearch.Value = string.Empty;
            }
            else //----- no popup matches found
            {
                hdn_AssemblySearch.Value = string.Empty;
            }
        }
        //-------- end of NHA event trigger setup





        //--------SPEED  - is this working -- Determine if there are new items to be looked up from Oracle
        bool firstplaced = false;
        string strWhereString = " WHERE (";
        for (lp = 0; lp <= 999; lp++)
        {
            if (MasterPartsArray[lp, 0] != null)
            {
                if (MasterPartsArray[lp, 3] != "ORA+")
                {
                    if (GenUtils.MakeUCaseNumChar(MasterPartsArray[lp, 0]).Length > 0)
                    {
                        if (firstplaced)
                        {
                            strWhereString += " OR ";
                        }
                        else
                        {
                            firstplaced = true;
                        }
                        strWhereString += "p.PN_STRIPPED like  '" + GenUtils.MakeUCaseNumChar(MasterPartsArray[lp, 0]) + "%'";
                    }
                }
            }
            else
            {
                break;
            }
        }
        strWhereString += ") ";
        //------------------------- if there are items to be looked up and added
        if (firstplaced)
        {
            //Debug.WriteLine("should be getting smaller: " + strWhereString);
            dtOraclePartsMaster = GenUtils.FindParts(strWhereString, string.Empty);

            //------------------------ Now expand the datatable and move the oracle data into it.
            addOracleParstsMastertoMasterTable(dtOraclePartsMaster, Page.Session["MySessionId"].ToString(), "OR_PART");

            //---------- Now loop through the table and produce a list of all keys for the image lookup
            //  and a list of all Alternate parts which are not OEM Loaded
            //  load the list of unloaded Alternate narts from Oracle and return the list is
            //  PNM_AUTO_KEYS for the Image Lookup
            imageList = LoadAlternatesandImages(Page.Session["MySessionId"].ToString());
            loadImagestoDT(imageList);

            // -------------------------- set the parts prices
            priceTheParts();
        }

        loadScrapeQuantities(dtCacheMaster, SPBmasterUNIFIEDList, swCounter, blnThisisNewSearch);

        //GenUtils.PrintTableOrView(dtCacheMaster, "After");

        //---------- Check the stocklines to determine the available quantities and add to the Cache Master
        UpdateStockLines();

        //------ Check each part to determine if there is custom pricing for this company
        UpdateCustomPricing(Convert.ToInt32(Page.Session["Quantum_CMP_AUTO_KEY"].ToString()));

        //------- apply the proper pricing for this user to the parts
        priceTheParts();

        return temp;

    }

    protected string[,] BuildUNIFIEDList(string[,] SPBmasterSEARCHList, string[,] SPBmasterSCRAPEList, bool blnThisisNewSearch)
    {
        int lpSearch, lpScrape, subLp;
        int cntUnified = -1;
        string[,] strTemp = new string[1000, 7];
        bool blnMatchFound = false;

        if (blnThisisNewSearch)
        {
            for (lpSearch = 0; lpSearch <= 1000; lpSearch++)
            {
                if (SPBmasterSEARCHList[lpSearch, 0] == null)
                {
                    break;
                }
                cntUnified++;
                for (subLp = 0; subLp < 7; subLp++)
                {
                    strTemp[cntUnified, subLp] = SPBmasterSEARCHList[lpSearch, subLp];
                }
                strTemp[cntUnified, 4] = "search";
            }

            for (lpScrape = 0; lpScrape <= 1000; lpScrape++)
            {
                if (SPBmasterSCRAPEList[lpScrape, 0] == null)
                {
                    break;
                }
                blnMatchFound = false;
                for (subLp = 0; subLp < 7; subLp++)
                {
                    if (strTemp[subLp, 0] == SPBmasterSCRAPEList[lpScrape, 0])
                    {
                        blnMatchFound = true;
                        break;
                    }
                }
                if (!blnMatchFound)
                {
                    cntUnified++;
                    for (subLp = 0; subLp < 7; subLp++)
                    {
                        strTemp[cntUnified, subLp] = SPBmasterSCRAPEList[lpScrape, subLp];
                    }
                    strTemp[cntUnified, 4] = "scrape";
                }
            }
        }
        else
        {
            for (lpScrape = 0; lpScrape <= 1000; lpScrape++)
            {
                if (SPBmasterSCRAPEList[lpScrape, 0] == null)
                {
                    break;
                }
                cntUnified++;
                for (subLp = 0; subLp < 7; subLp++)
                {
                    strTemp[cntUnified, subLp] = SPBmasterSCRAPEList[lpScrape, subLp];
                }
                strTemp[cntUnified, 4] = "scrape";
            }

            for (lpSearch = 0; lpSearch <= 1000; lpSearch++)
            {
                if (SPBmasterSEARCHList[lpSearch, 0] == null)
                {
                    break;
                }
                blnMatchFound = false;
                for (subLp = 0; subLp < 7; subLp++)
                {
                    if (strTemp[subLp, 0] == SPBmasterSEARCHList[lpSearch, 0])
                    {
                        blnMatchFound = true;
                        strTemp[subLp, 4] = "search";
                        break;
                    }
                }
                if (!blnMatchFound)
                {
                    cntUnified++;
                    for (subLp = 0; subLp < 7; subLp++)
                    {
                        strTemp[cntUnified, subLp] = SPBmasterSEARCHList[lpSearch, subLp];
                    }
                    strTemp[cntUnified, 4] = "search";
                }
            }
        }
        return strTemp;
    }

    protected string[,] SPBScreenScraper(string mode, string AddPartsList, string scrapeType, bool blnThisisNewSearch)
    {
        //----- Scrape the current Request Form post for all part numbers and associated Quantities
        string[,] csTemp = new string[1000, 6];
        int csCounter = -1;
        string myName, myValue, myPanel, myPartNumber, myFormType;
        string myQty;
        string[,] csQtyError = new string[10, 3];
        int csQtyCounter = -1;
        bool blnUsethisValue = false;

        if (mode == "AddParts")
        {
            if (AddPartsList != "")
            {
                string[] splitAP = AddPartsList.Split('|');
                foreach (string s in splitAP)
                {
                    if (s.Trim() != string.Empty)
                    {
                        string[] tmp = s.Split('~');
                        csCounter++;
                        csTemp[csCounter, 0] = tmp[0].Trim();
                        if (csTemp[csCounter, 0].Length < 4)
                        {
                            blnShortSearchWord = true;
                        }
                        csTemp[csCounter, 1] = "0";
                        if (tmp.GetUpperBound(0) > 0)
                        {
                            csTemp[csCounter, 1] = tmp[1];
                        }
                        csTemp[csCounter, 2] = "IntAdd";
                        SPBmasterSEARCHList[++SPBmasterSEARCHCnt, 0] = GenUtils.MakeUniformPartNumber(tmp[0]);
                        SPBmasterSEARCHList[SPBmasterSEARCHCnt, 0] = tmp[0];
                        SPBmasterSEARCHList[SPBmasterSEARCHCnt, 1] = "0";
                        if (tmp.GetUpperBound(0) > 0)
                        {
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 1] = GenUtils.MakeUniformQtyString(tmp[1]);
                        }
                        SPBmasterSEARCHList[SPBmasterSEARCHCnt, 2] = "IntADD";
                        AddPartsCounter++;
                    }
                }
            }
        }
        else
        {


            foreach (string name in Request.Form)
            {
                blnUsethisValue = false;
                if (name != null)
                {
                    myName = name.Trim();
                    myValue = Request.Form[name].Trim();
                    myFormType = string.Empty;
                    string[] temp = myName.Split('_');
                    if (temp.GetUpperBound(0) > 1)
                    {
                        temp[2] = GenUtils.StripQTYidString(temp[2]);
                    }
                    if (myName.Length > 17)
                    {
                        if (myName.Substring(0, 17).ToLower() == "txtsearch_partno_")
                            myFormType = "SEARCH";
                        blnUsethisValue = true;
                    }

                    if (myName.Length > 7)
                    {
                        if (myName.Substring(0, 7).ToLower() == "txtqty_")
                        {
                            myFormType = "Base_QTY";
                        }
                        blnUsethisValue = true;
                        if (temp.GetUpperBound(0) > 1)
                        {
                            if (temp[1].ToLower() == "detailpanel" && scrapeType != "detailPopup")
                            {
                                blnUsethisValue = false;
                                myFormType = string.Empty;
                            }
                        }
                        blnUsethisValue = true;
                        //--------- if it is in the search panel let that override
                        if (temp.GetUpperBound(0) > 1)
                        {
                            if (Request.Form["txtSearch_PartNo_1"] != null)
                            {
                                if (Request.Form["txtSearch_PartNo_1"].ToLower() == temp[2].ToLower() || Request.Form["txtSearch_PartNo_2"].ToLower() == temp[2].ToLower() || Request.Form["txtSearch_PartNo_3"].ToLower() == temp[2].ToLower() || Request.Form["txtSearch_PartNo_4"].ToLower() == temp[2].ToLower() || Request.Form["txtSearch_PartNo_5"].ToLower() == temp[2].ToLower())
                                {
                                    if (myFormType == "Base_QTY")
                                    {
                                        blnUsethisValue = false;
                                    }
                                }
                            }
                        }
                    }

                    //------ if a popup is open and the Form contains a 
                    if (scrapeType == "detailPopup")
                    {
                        if (temp.GetUpperBound(0) > 1)
                        {
                            if (Request.Form["txtQTY_DetailPanel_" + temp[2]] != null)
                            {
                                if (myFormType == "Base_QTY")
                                {
                                    blnUsethisValue = false;
                                }
                            }
                        }
                        if (myName.Length > 18)
                        {
                            if (myName.Substring(0, 18) == "txtQTY_DetailPanel")
                            {
                                myFormType = "PopupQty_QTY";
                                if (scrapeType == "detailPopup")
                                {
                                    blnUsethisValue = true;
                                }
                            }
                        }
                    }

                    if ((myFormType == "PopupQty_QTY" || myFormType == "Base_QTY") && blnUsethisValue)
                    {
                        string[] tmp = myName.Split('_');
                        myPanel = tmp[1];
                        myPartNumber = GenUtils.StripQTYidString(tmp[2]);
                        myQty = Request.Form[name];
                        myQty = GenUtils.MakeNumbersOnly(myQty);
                        csCounter++;
                        csTemp[csCounter, 0] = myPartNumber.Trim();
                        csTemp[csCounter, 1] = myQty;
                        csTemp[csCounter, 2] = "QTY-" + myPanel;
                        if (myPartNumber.Length < 4)
                        {
                            blnShortSearchWord = true;
                        }
                        SPBmasterSCRAPEList[++SPBmasterSCRAPECnt, 0] = GenUtils.MakeUniformPartNumber(myPartNumber);
                        SPBmasterSCRAPEList[SPBmasterSCRAPECnt, 5] = myPartNumber;
                        SPBmasterSCRAPEList[SPBmasterSCRAPECnt, 1] = GenUtils.MakeUniformQtyString(myQty);
                        SPBmasterSCRAPEList[SPBmasterSCRAPECnt, 2] = "QTY-" + myPanel;
                    }
                    if (myValue != string.Empty && blnUsethisValue)
                    {
                        // ---- add the items form the search new items panel
                        if (myFormType == "SEARCH")
                        {
                            string[] tmp = myName.Split('_');
                            csCounter++;
                            csTemp[csCounter, 0] = Request.Form[name].Trim();
                            csTemp[csCounter, 1] = Request.Form["txtSearch_Qty_" + tmp[2]].Trim();
                            csTemp[csCounter, 2] = "SearchPanel";

                            if (csTemp[csCounter, 0].Length < 4)
                            {
                                blnShortSearchWord = true;
                            }

                            SPBmasterSEARCHList[++SPBmasterSEARCHCnt, 0] = GenUtils.MakeUniformPartNumber(Request.Form[name]);
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 1] = GenUtils.MakeUniformQtyString(Request.Form["txtSearch_Qty_" + tmp[2]]);
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 2] = "SearchPanel";
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 5] = Request.Form[name];

                            if (blnRequireSearchQty)
                            {
                                if (csTemp[csCounter, 0].Trim() != string.Empty)
                                {
                                    if (csTemp[csCounter, 1].ToString().Trim() == string.Empty)
                                    {
                                        csQtyError[++csQtyCounter, 0] = "QTY_ERROR";
                                        csQtyError[csQtyCounter, 1] = csTemp[csCounter, 0];
                                        csQtyError[csQtyCounter, 2] = csTemp[csCounter, 1];
                                    }
                                    else if (int.Parse(csTemp[csCounter, 1].ToString()) < 1)
                                    {
                                        csQtyError[++csQtyCounter, 0] = "QTY_ERROR";
                                        csQtyError[csQtyCounter, 1] = csTemp[csCounter, 0];
                                        csQtyError[csQtyCounter, 2] = csTemp[csCounter, 1];
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (scrapeType == "base")
            {
                int intExcelPartCounter = 0;
                // ------ add the items in the listSearchBox
                string tbText = listSearchBox.InnerText;
                if (tbText.Trim() != string.Empty)
                {
                    tbText = tbText.Replace("\r", "");
                    tbText = tbText.Replace("\t", "~");
                    string[] split1 = tbText.Split('\n');
                    foreach (string s in split1)
                    {
                        if (s.Trim() != string.Empty)
                        {
                            if (intExcelPartCounter++ >= intlimitExcelParts)
                            {
                                blnExcelLimitExceeded = true;
                                break;
                            }
                            string[] tmp = s.Split('~');
                            csCounter++;
                            csTemp[csCounter, 0] = tmp[0].Trim();
                            if (csTemp[csCounter, 0].Length < 4)
                            {
                                blnShortSearchWord = true;
                            }

                            csTemp[csCounter, 1] = "0";
                            if (tmp.GetUpperBound(0) > 0)
                            {
                                csTemp[csCounter, 1] = tmp[1];
                            }
                            csTemp[csCounter, 2] = "List";
                            SPBmasterSEARCHList[++SPBmasterSEARCHCnt, 0] = GenUtils.MakeUniformPartNumber(tmp[0]);
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 5] = tmp[0];
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 1] = "0";
                            if (tmp.GetUpperBound(0) > 0)
                            {
                                SPBmasterSEARCHList[SPBmasterSEARCHCnt, 1] = GenUtils.MakeUniformQtyString(tmp[1]);
                            }
                            SPBmasterSEARCHList[SPBmasterSEARCHCnt, 2] = "List";
                        }
                    }
                }




            }
        }  // else addparts

       
           
        

        if (csQtyCounter > -1)
        {
            return csQtyError;
        }
        else
        {
            return csTemp;
        }
    }

    protected bool UpdateStockLines()
    {
        bool avlChanged = false;
        bool firstplaced = false;
        DataTable dtStock = null;
        string[] theseSet = new string[1000];
        int intsetCounter = -1;
        string queryString = " SELECT STOCK.PN, STOCK.PNM_AUTO_KEY, STOCK.QTY_AVAILABLE, STOCK.HOLD_LINE FROM STOCK STOCK WHERE ( ";
        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
        {
            if (myMasterRow["calc_AVL_SET"].ToString() != "True" && myMasterRow["OracleLoaded"].ToString() == "True")
            {
                if (!firstplaced)
                {
                    firstplaced = true;
                }
                else
                {
                    queryString += " OR ";
                }
                theseSet[++intsetCounter] = myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString();

                queryString += " STOCK.PNM_AUTO_KEY = " + myMasterRow["pmast_P_PNM_AUTO_KEY"] + " ";
            }
        }
        queryString += " ) ORDER BY STOCK.PNM_AUTO_KEY";

        if (firstplaced)
        {
            avlChanged = true;
            dtStock = GenUtils.buildOracleTable(queryString);
            foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                if (myMasterRow["calc_AVL_SET"].ToString() != "True")
                {
                    if (dtStock != null)
                    {
                        foreach (DataRow myStockRow in dtStock.Rows)
                        {
                            if (myMasterRow["pmast_P_PNM_AUTO_KEY"] != DBNull.Value)
                            {
                                if ((int.Parse(myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString()) == int.Parse(myStockRow["PNM_AUTO_KEY"].ToString())) && (myStockRow["HOLD_LINE"].ToString() != "T"))
                                {
                                    if (GenUtils.IsDecimal(myMasterRow["calc_QTY_AVAILABLE"].ToString()) && GenUtils.IsDecimal(myStockRow["QTY_AVAILABLE"].ToString()))
                                    {
                                        myMasterRow["calc_QTY_AVAILABLE"] = decimal.Parse(myMasterRow["calc_QTY_AVAILABLE"].ToString()) + decimal.Parse(myStockRow["QTY_AVAILABLE"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int lp;
            for (lp = 0; lp < 999; lp++)
            {
                if (theseSet[lp] == string.Empty)
                {
                    break;
                }
                foreach (DataRow myMasterRow in dtCacheMaster.Rows)
                {
                    if (myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString() == theseSet[lp])
                    {
                        myMasterRow["calc_AVL_SET"] = true;
                        break;
                    }
                }
            }
        }
        return avlChanged;
    }

    protected bool UpdateCustomPricing(int curCompanyCode)
    {
        bool priceChanged = false;
        bool firstplaced = false;
        DataTable dtPrices = null;
        string[] theseSet = new string[1000];
        int intsetCounter = -1;
        string queryString = " SELECT PNM_AUTO_KEY, UNIT_PRICE FROM PRICES WHERE CMP_AUTO_KEY = " + curCompanyCode  + "  AND ( ";
        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
        {
            if (myMasterRow["calc_PRICE_SET"].ToString() != "True")
            {
                if (!firstplaced)
                {
                    firstplaced = true;
                }
                else
                {
                    queryString += " OR ";
                }
                theseSet[++intsetCounter] = myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString();
                queryString += " PNM_AUTO_KEY = " + myMasterRow["pmast_P_PNM_AUTO_KEY"] + " ";
            }
        }
        queryString += " ) ORDER BY PNM_AUTO_KEY";
        if (firstplaced)
        {
            priceChanged = true;
            dtPrices = GenUtils.buildOracleTable(queryString);
            foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                if (myMasterRow["calc_PRICE_SET"].ToString() != "True")
                {
                    if (dtPrices != null)
                    {
                        foreach (DataRow myPriceRow in dtPrices.Rows)
                        {
                            if ((int.Parse(myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString()) == int.Parse(myPriceRow["PNM_AUTO_KEY"].ToString())))
                            {
                                myMasterRow["calc_CUSTOM_UNIT_PRICE"] = myPriceRow["UNIT_PRICE"];
                            }
                        }
                    }
                }
            }
            int lp;
            for (lp = 0; lp < 999; lp++)
            {
                if (theseSet[lp] == string.Empty)
                {
                    break;
                }
                foreach (DataRow myMasterRow in dtCacheMaster.Rows)
                {
                    if (myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString() == theseSet[lp])
                    {
                        myMasterRow["calc_PRICE_SET"] = true;
                        break;
                    }
                }
            }

        }
        return priceChanged;
    }

    protected void priceTheParts()
    {

        decimal decList = 0m;
        decimal decCustom = 0m;
        decimal decExchangeListPrice = 0m;
        decimal thisSell = 0m;
        cartTlQty = 0;
        cartTlAmount = 0m;

        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
        {
            decList = 0m;
            if (myMasterRow["pmast_LIST_PRICE"] != null)
            {
                if (myMasterRow["pmast_LIST_PRICE"].ToString() != string.Empty)
                {
                    decList = decimal.Parse(myMasterRow["pmast_LIST_PRICE"].ToString());
                }
            }
            decExchangeListPrice = 0m;
            if (myMasterRow["pmast_EXCH_LIST_PRICE"] != null)
            {
                if (myMasterRow["pmast_EXCH_LIST_PRICE"].ToString() != string.Empty)
                {
                    decExchangeListPrice = decimal.Parse(myMasterRow["pmast_EXCH_LIST_PRICE"].ToString());
                }
            }
            decCustom = 0m;
            if (myMasterRow["calc_CUSTOM_UNIT_PRICE"] != null)
            {
                if (myMasterRow["calc_CUSTOM_UNIT_PRICE"].ToString() != string.Empty)
                {
                    decCustom = decimal.Parse(myMasterRow["calc_CUSTOM_UNIT_PRICE"].ToString());
                }
            }
            switch (Page.Session["Company_Type_for_Price"].ToString())
            {
                 case "OVERHAUL":
                      thisSell = decList;
                      myMasterRow["calc_PRICE_TYPE_USED"] = "OH"; //---- Overhaul / default pricing
                     break;
                 case "AIRLINE":
                     thisSell = decExchangeListPrice;
                     myMasterRow["calc_PRICE_TYPE_USED"] = "AL";  //---- Airline Pricing
                     break;
                 default:
                     myMasterRow["calc_PRICE_TYPE_USED"] = "PL";  //---- Custom Pricing Only
                     thisSell = 0;
                     break;
             }
             if (decCustom > 0)
             {
                thisSell = decCustom;
             }
             myMasterRow["calc_SELL_PRICE"] = thisSell;
             cartTlQty += (int)myMasterRow["cur_QTY_ORDERED"];
             cartTlAmount += ((int)myMasterRow["cur_QTY_ORDERED"] * thisSell);
        }
    }

    protected bool updateQtyFromScraper(string[,] scraperList)
    {
        int lp = -1;
        bool qtyChanged = false;
        for (lp = 0; lp < 999; lp++)
        {
            if (scraperList[lp, 0] == null)
            {
                return qtyChanged;
            }
            else if (scraperList[lp, 0] == string.Empty)
            {
                return qtyChanged;
            }
         foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                if (GenUtils.MakeUCaseNumChar(scraperList[lp, 0]) == myMasterRow["partNumberStripped"].ToString())
                {
                    if (scraperList[lp, 1] != string.Empty)
                    {
                        if (blncheckAvlScrape)
                        {
                            if ((int)myMasterRow["cur_QTY_AvailableSearch"] != int.Parse(scraperList[lp, 1]))
                            {
                                myMasterRow["cur_QTY_AvailableSearch"] = int.Parse(scraperList[lp, 1]);
                            }
                        }
                        else
                        {
                            if ((int)myMasterRow["cur_QTY_ORDERED"] != int.Parse(scraperList[lp, 1]))
                            {
                                myMasterRow["cur_QTY_ORDERED"] = int.Parse(scraperList[lp, 1]);
                            }
                        }
                        qtyChanged = true;
                        break;
                    }
                }
            }
        }
        return qtyChanged;
    }

    protected Boolean loadScrapeQuantities(DataTable dtCacheMaster, string[,] searchWords, int swCounter, bool blnThisisNewSearch)
    {

    int lp;
    int intCurRow;
    int dtRowCounter = -1;
    bool qtyChanged = false;
    
    foreach (DataRow myMTRow in dtCacheMaster.Rows)
    {
        dtCacheMaster.Rows[++dtRowCounter]["CurrentSearchMatch"] = string.Empty;
    }
        for (lp = 0; lp <= swCounter; lp++)
        {
            dtRowCounter = -1;
            foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                dtRowCounter++;
                string temp1 = myMasterRow["pmast_PN_STRIPPED"].ToString();
                string temp2 = GenUtils.MakeUCaseNumChar(searchWords[lp, 0]);

                if (myMasterRow["pmast_PN_STRIPPED"].ToString() == GenUtils.MakeUCaseNumChar(searchWords[lp, 0]) )
                {
                    intCurRow = findJPNRow(dtRowCounter, dtCacheMaster);
                    if (searchWords[lp, 2] == "SearchPanel" || searchWords[lp, 2] == "List")
                    {
                        dtCacheMaster.Rows[intCurRow]["CurrentSearchMatch"] = GenUtils.MakeUniformPartNumber(searchWords[lp, 0]);
                        if (GenUtils.MakeNumbersOnly(searchWords[lp, 1]) != string.Empty)
                        {
                            if (blncheckAvlScrape)
                            {
                                if (dtCacheMaster.Rows[intCurRow]["cur_QTY_AvailableSearch"].ToString() != GenUtils.MakeNumbersOnly(searchWords[lp, 1]))
                                {
                                    dtCacheMaster.Rows[intCurRow]["cur_QTY_AvailableSearch"] = Int32.Parse(GenUtils.MakeNumbersOnly(searchWords[lp, 1]));
                                    qtyChanged = true;
                                }
                            }
                            else
                            {
                                if (dtCacheMaster.Rows[intCurRow]["cur_QTY_ORDERED"].ToString() != GenUtils.MakeNumbersOnly(searchWords[lp, 1]))
                                {
                                    dtCacheMaster.Rows[intCurRow]["cur_QTY_ORDERED"] = Int32.Parse(GenUtils.MakeNumbersOnly(searchWords[lp, 1]));
                                    qtyChanged = true;
                                }
                            }
                        }
                    }
                    dtCacheMaster.Rows[intCurRow]["cur_LAST_LIST_PANEL"] = searchWords[lp, 2];
                    break;
                }
            }
        }
        return qtyChanged;
    }

    protected void loadImagestoDT(int[] imageList)
    {

        int[] loadedKeys = new int[1000];
        int loadedKeyCounter = -1;
        int lp, slp;
        bool firstplaced = false;
        bool matchFound = false;
        DataTable dtImages = null;

        string strWhereString = " WHERE SOURCE_TABLE = 'PARTS_MASTER' AND (";

        for (lp = 0; lp <= 999; lp++)
        {
            if ((lp > 0) && (imageList[lp] == 0))
            {
                break;
            }
            matchFound = false;
            for (slp = 0; slp <= 999; slp++)
            {
                if (loadedKeys[slp] == imageList[lp])
                {
                    matchFound = true;
                    break;
                }
                if ((slp > 0) && (imageList[slp] == 0))
                {
                    break;
                }
            }

            if (!matchFound)
            {
                foreach (DataRow myMasterRow in dtCacheMaster.Rows)
                {
                    if (myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString() == imageList[lp].ToString()  && myMasterRow["imagesLoaded"].ToString() == "True")
                    {
                        matchFound = true;
                        break;
                    }
                }
            }
            
            if (!matchFound)
            {
                loadedKeys[++loadedKeyCounter] = imageList[lp];

                if (firstplaced)
                {
                    strWhereString += " OR ";
                }
                else
                {
                    firstplaced = true;
                }
                strWhereString += " SOURCE_PK =" + imageList[lp] + " ";
            }
        }
        strWhereString += ") ";

        if (firstplaced)
        {
            string queryString = "Select i.IMAGE_KEY, i.FILE_NAME, i.FILE_EXT, p.PNM_AUTO_KEY, i.SOURCE_PK, i.IMC_AUTO_KEY  FROM image_list i JOIN PARTS_MASTER p ON i.SOURCE_PK = p.PNM_AUTO_KEY ";
            queryString += strWhereString;
            queryString += " ORDER BY p.PN";

            if (blnshowDebuggerOutput)
                Debug.WriteLine(queryString);

            dtImages = GenUtils.buildOracleTable(queryString);

            if (dtImages != null)
            {
                foreach (DataRow imageRow in dtImages.Rows)
                {
                    foreach (DataRow myMasterRow in dtCacheMaster.Rows)
                    {
                        if (myMasterRow["pmast_P_PNM_AUTO_KEY"] != DBNull.Value && myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString() != string.Empty && imageRow["SOURCE_PK"] != DBNull.Value && imageRow["SOURCE_PK"].ToString() != string.Empty)
                        {
                            if (myMasterRow["pmast_P_PNM_AUTO_KEY"].ToString() == imageRow["SOURCE_PK"].ToString())
                            {
                                if (imageRow["IMC_AUTO_KEY"].ToString() == "88")
                                {
                                    myMasterRow["image_IMG1_FILE_NAME"] = imageRow["FILE_NAME"];  
                                    myMasterRow["image_IMG1_FILE_EXT"] = imageRow["IMC_AUTO_KEY"];
                                    myMasterRow["image_IMG1_KEY"] = imageRow["IMAGE_KEY"];
                                }
                                else if ((myMasterRow["image_IMG2_FILE_NAME"] == DBNull.Value || myMasterRow["image_IMG2_FILE_NAME"].ToString() == string.Empty) && imageRow["IMC_AUTO_KEY"].ToString() == "113")
                                {
                                    myMasterRow["image_IMG2_FILE_NAME"] = imageRow["FILE_NAME"]; 
                                    myMasterRow["image_IMG2_FILE_EXT"] = imageRow["IMC_AUTO_KEY"];
                                    myMasterRow["image_IMG2_KEY"] = imageRow["IMAGE_KEY"];
                                }
                                else if (myMasterRow["image_IMG3_FILE_NAME"] == DBNull.Value || myMasterRow["image_IMG3_FILE_NAME"].ToString() == string.Empty)
                                {
                                    myMasterRow["image_IMG3_FILE_NAME"] = imageRow["FILE_NAME"]; 
                                    myMasterRow["image_IMG3_FILE_EXT"] = imageRow["IMC_AUTO_KEY"];
                                    myMasterRow["image_IMG3_KEY"] = imageRow["IMAGE_KEY"];
                                }
                                else if (myMasterRow["image_IMG4_FILE_NAME"] == DBNull.Value || myMasterRow["image_IMG4_FILE_NAME"].ToString() == string.Empty)
                                {
                                    myMasterRow["image_IMG4_FILE_NAME"] = imageRow["FILE_NAME"]; 
                                    myMasterRow["image_IMG4_FILE_EXT"] = imageRow["IMC_AUTO_KEY"];
                                    myMasterRow["image_IMG4_KEY"] = imageRow["IMAGE_KEY"];
                                }
                                myMasterRow["imagesLoaded"] = true;
                                break;
                            }
                        }
                        myMasterRow["imagesLoaded"] = true;
                    }
                }
            }
        }
    }

    protected int[] LoadAlternatesandImages(string MySessionId)
    {
        int[] tempImages = new int[1000];
        int imageCounter = -1;
        int[] tempNotLoadedKeys = new int[1000];
        int notLoadedCounter = - 1;
        DataTable oemPartsLookup;
        string[,] oemList = new string[1000, 6];
        bool matchFound = false;

        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
        {
            if (myMasterRow["pmast_ALT_PNM_AUTO_KEY"] != null && myMasterRow["pmast_ALT_PNM_AUTO_KEY"] != DBNull.Value)
            {
                if ((int)myMasterRow["pmast_ALT_PNM_AUTO_KEY"] > 0)
                {
                    tempImages[++imageCounter] = (int)myMasterRow["pmast_ALT_PNM_AUTO_KEY"];
                }
            }
            if (myMasterRow["pmast_P_PNM_AUTO_KEY"] != null && myMasterRow["pmast_P_PNM_AUTO_KEY"] != DBNull.Value)
            {
                if ((int)myMasterRow["pmast_P_PNM_AUTO_KEY"] > 0)
                {
                    tempImages[++imageCounter] = (int)myMasterRow["pmast_P_PNM_AUTO_KEY"];
                }
            }
            matchFound = false;

            foreach (DataRow myTestRow in dtCacheMaster.Rows)
            {
                if (myTestRow["pmast_P_PNM_AUTO_KEY"] != null && myTestRow["pmast_P_PNM_AUTO_KEY"] != DBNull.Value && myMasterRow["pmast_ALT_PNM_AUTO_KEY"] != null && myMasterRow["pmast_ALT_PNM_AUTO_KEY"] != DBNull.Value)
                {
                    if ((int)myTestRow["pmast_P_PNM_AUTO_KEY"] == (int)myMasterRow["pmast_ALT_PNM_AUTO_KEY"])
                    {
                        matchFound = true;
                        break;
                    }
                }
                else
                {
                    matchFound = true;
                }
            }
            if (!matchFound && myMasterRow["pmast_ALT_PNM_AUTO_KEY"] != null && myMasterRow["pmast_ALT_PNM_AUTO_KEY"] != DBNull.Value && (int)myMasterRow["pmast_ALT_PNM_AUTO_KEY"] > 0 )
           
            {
                tempNotLoadedKeys[++notLoadedCounter] = (int)myMasterRow["pmast_ALT_PNM_AUTO_KEY"];
            }
        }
        int lp;
        bool firstplaced = false;
        string strWhereString = " WHERE (";
        if (notLoadedCounter > -1)
        {
            for (lp = 0; lp <= 999; lp++)
            {
                if (tempNotLoadedKeys[lp] > 0)
                {
                    if (firstplaced)
                    {
                        strWhereString += " OR ";
                    }
                    else
                    {
                        firstplaced = true;
                    }
                    strWhereString += "p.PNM_AUTO_KEY =" + tempNotLoadedKeys[lp] + " ";
                }
                else
                {
                    break;
                }
            }
            strWhereString += ") ";
            oemPartsLookup = GenUtils.FindParts(strWhereString, string.Empty);
            oemList = addOracleParstsMastertoMasterTable(oemPartsLookup, Page.Session["MySessionId"].ToString(), "OR_ALTERNATE");
        }
        return tempImages;
    }

    protected void updateTheCache(DataTable dtCacheMaster, string thisSessionID, string clearAll, string strSearchWords)
    {
        string conn = string.Empty;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_JETSERVE3_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        SqlConnection cacheQueueConn = new SqlConnection(conn);
        string queryString = string.Empty;

        if (clearAll == "clearAll")
        {
            queryString = "DELETE FROM ecom_ActiveCache WHERE SessionID = '" +  thisSessionID + "' ; ";
        }
        else
        {
        queryString = "DELETE FROM ecom_ActiveCache WHERE createDateTime < '" + DateTime.Now.AddSeconds(-1000) + "' OR SessionID = '" +  thisSessionID + "' ; ";
        }
        SqlCommand cmd = new SqlCommand(queryString, cacheQueueConn);

        try
        {
            cacheQueueConn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error Updating login attempt: " + ex.ToString());
        }
        finally
        {
            cacheQueueConn.Close();
        }
        if (clearAll == "clearAll")
        {
            return;
        }

        int slp;
        string strTitleList = string.Empty;
        string strValueList = string.Empty;
        bool blnFirstplaced = false;
        string strThisValue;

        try
        {
            cacheQueueConn.Open();
            foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                if (myMasterRow["partNumber"] == null)
                {
                    break;
                }
               
                else
                {
                    strTitleList = string.Empty;
                    strValueList = string.Empty;
                    blnFirstplaced = false;
                    for (slp = 1; slp <= cacheColumnCount; slp++)
                    {
                        if (blnFirstplaced)
                        {
                            strTitleList += ", ";
                            strValueList += ", ";
                        }
                        else
                        {
                            blnFirstplaced = true;
                        }
                        strTitleList += cacheDefine[slp, 0];
                        switch (cacheDefine[slp, 1])
                        {
                            case "String":
                                {
                                    strValueList += "'";
                                    break;
                                }
                            case "DateTime":
                                {
                                    strValueList += "'";
                                    break;
                                }
                        }
                        strThisValue = myMasterRow[cacheDefine[slp, 0]].ToString();
                        switch (cacheDefine[slp, 1])
                        {
                            case "Boolean":
                                {
                                    if (strThisValue == "True")
                                    {
                                        strValueList += "1";
                                    }
                                    else
                                    {
                                        strValueList += "0";
                                    }
                                    break;
                                }
                            case "Int32":
                                {
                                    if (strThisValue == string.Empty || strThisValue == null)
                                    {
                                        strValueList += "0";
                                    }
                                    else
                                    {
                                        strValueList += strThisValue;
                                    }
                                    break;
                                }
                            case "Decimal":
                                {
                                    if (strThisValue == string.Empty || strThisValue == null)
                                    {
                                        strValueList += "0";
                                    }
                                    else
                                    {
                                        strValueList += strThisValue;
                                    }
                                    break;
                                }
                            default:
                                {
                                    strValueList += GenUtils.RemoveReservedChars(strThisValue);
                                    break;
                                }
                        }
                        switch (cacheDefine[slp, 1])
                        {
                            case "String":
                                {
                                    strValueList += "'";
                                    break;
                                }
                            case "DateTime":
                                {
                                    strValueList += "'";
                                    break;
                                }
                        }
                    }
                    queryString = "INSERT INTO ecom_ActiveCache ( " + strTitleList + ", meta_SearchWords) VALUES (" + strValueList + ", '" + strSearchWords + "');";
                    cmd = new SqlCommand(queryString, cacheQueueConn);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error Updating login attempt: " + ex.ToString());
        }
        finally
        {
            cacheQueueConn.Close();
        }
    }

    protected string[,] addNHAtoMasterTable(DataTable dtNHA, string MySessionId)
    {
        string [,] arTemp = new string[1000, 6];
        int arCounter = -1;
        if (dtNHA != null)
        {
            foreach (DataRow myNHARow in dtNHA.Rows)
            {
                arTemp[++arCounter, 0] = myNHARow["JPEPN"].ToString();
                arTemp[arCounter, 1] = "0";
                arTemp[arCounter, 2] = "NHA_JPE";
                arTemp[++arCounter, 0] = myNHARow["OEMPN"].ToString();
                arTemp[arCounter, 1] = "0";
                arTemp[arCounter, 2] = "NHA_OEM";
                if (dtCacheMaster != null)
                {
                    addeditNHANumber(myNHARow, "JPEPN", Page.Session["MySessionId"].ToString());
                    addeditNHANumber(myNHARow, "OEMPN", Page.Session["MySessionId"].ToString());
                }
            }
        }
        return arTemp;
    }

    protected void addeditNHANumber(DataRow myNHARow, string fieldToCompare, string MySessionID)
    {
        bool blnMatchFound = false;
        DataRow newRow;
        foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                if (myNHARow[fieldToCompare].ToString() == myMasterRow["partNumber"].ToString())
                {
                    blnMatchFound = true;
                    myMasterRow["NHALoaded"] = true;
                    myMasterRow["nha_NHA"] = myNHARow["NHA"];
                    myMasterRow["nha_ATAChapter"] = myNHARow["ATAChapter"];
                    myMasterRow["nha_PartsPerNHA"] = myNHARow["PartsPerNHA"];
                    myMasterRow["nha_OEMPN"] = myNHARow["OEMPN"];
                    myMasterRow["nha_JPEPN"] = myNHARow["JPEPN"];
                    myMasterRow["nha_OEMPrice"] = myNHARow["OEMPrice"];
                    myMasterRow["nha_JPEPrice"] = myNHARow["JPEPrice"];
                    myMasterRow["nha_Description"] = myNHARow["Description"];
                    myMasterRow["nha_NHAName"] =   myNHARow["NHA"] ;
                    break;
                }
            }
         if (!blnMatchFound)  // -- add it
         {
             newRow = dtCacheMaster.NewRow();
             newRow["recordSource"] = "NHA_" + fieldToCompare;
             newRow["partNumber"] = myNHARow[fieldToCompare];
             newRow["partNumberStripped"] = GenUtils.MakeUCaseNumChar(myNHARow[fieldToCompare].ToString());
             newRow["createDateTime"] = DateTime.Now;
             newRow["SessionID"] = Page.Session["MySessionId"];
             newRow["NHALoaded"] = true;
             newRow["nha_NHA"] = myNHARow["NHA"];
             newRow["nha_ATAChapter"] = myNHARow["ATAChapter"];
             newRow["nha_PartsPerNHA"] = myNHARow["PartsPerNHA"];
             newRow["nha_OEMPN"] = myNHARow["OEMPN"];
             newRow["nha_JPEPN"] = myNHARow["JPEPN"];
             newRow["nha_OEMPrice"] = myNHARow["OEMPrice"];
             newRow["nha_JPEPrice"] = myNHARow["JPEPrice"];
             newRow["nha_Description"] = myNHARow["Description"];
             newRow["nha_NHAName"] = myNHARow["NHA"];
             //---- set some defaults
             newRow["cur_QTY_ORDERED"] = 0;
             newRow["cur_QTY_AvailableSearch"] = 0;
             newRow["cur_LAST_LIST_PANEL"] = string.Empty;
             newRow["calc_CUSTOM_UNIT_PRICE"] = 0m;
             newRow["calc_QTY_AVAILABLE"] = 0m;
             newRow["calc_NEXT_SHIP_QTY"] = 0;
             newRow["calc_SELL_PRICE"] = 0m;
             dtCacheMaster.Rows.Add(newRow);
         }
    }

    protected string[,] addOracleParstsMastertoMasterTable(DataTable dtOraclePartsMaster, string MySessionId, string addType)
    {
        bool blnMatchFound = false;
        DataRow newRow;
        string[,] pmTemp = new string[1000, 6];
        int pmCounter = -1;
        if (dtOraclePartsMaster == null)
        {
            return null;
        }
        foreach (DataRow myOrPartRow in dtOraclePartsMaster.Rows)
        {
            blnMatchFound = false;
            pmTemp[++pmCounter, 0] = myOrPartRow["PN"].ToString();
            pmTemp[pmCounter, 1] = "0";
            pmTemp[pmCounter, 2] = "ORPART_PN";
            if (dtCacheMaster != null)
            {
                foreach (DataRow myMasterRow in dtCacheMaster.Rows)
                {
                    if (myOrPartRow["PN_STRIPPED"].ToString() == myMasterRow["partNumberStripped"].ToString())
                    {
                        blnMatchFound = true;
                        myMasterRow["OracleLoaded"] = true;
                        myMasterRow["pmast_PN"] = myOrPartRow["PN"];
                        myMasterRow["pmast_PN_STRIPPED"] = myOrPartRow["PN_STRIPPED"];
                        myMasterRow["pmast_DESCRIPTION"] = myOrPartRow["DESCRIPTION"];
                        myMasterRow["pmast_ALT_PNM_AUTO_KEY"] = myOrPartRow["ALT_PNM_AUTO_KEY"];
                        myMasterRow["pmast_ALT_PN"] = myOrPartRow["ALT_PN"];
                        myMasterRow["pmast_ALT_PN_STRIPPED"] = myOrPartRow["ALT_PN_STRIPPED"];
                        myMasterRow["pmast_ALT_DESCRIPTION"] = myOrPartRow["ALT_DESCRIPTION"];
                        myMasterRow["pmast_ALT_LIST_PRICE"] = myOrPartRow["ALT_LIST_PRICE"];
                        myMasterRow["pmast_ALT_STC_AUTO_KEY"] = myOrPartRow["ALT_STC_AUTO_KEY"];
                        myMasterRow["pmast_ALT_QTY_AVAILABLE"] = myOrPartRow["ALT_QTY_AVAILABLE"];
                        myMasterRow["pmast_ECCN"] = myOrPartRow["ECCN"];
                        myMasterRow["pmast_UNIT_OF_MEASURE"] = myOrPartRow["UNIT_OF_MEASURE"];
                        myMasterRow["pmast_P_PNM_AUTO_KEY"] = myOrPartRow["P_PNM_AUTO_KEY"];
                        myMasterRow["pmast_P_STC_AUTO_KEY"] = myOrPartRow["P_STC_AUTO_KEY"];
                        myMasterRow["pmast_LIST_PRICE"] = myOrPartRow["LIST_PRICE"];
                        myMasterRow["pmast_EXCH_LIST_PRICE"] = myOrPartRow["EXCH_LIST_PRICE"];
                        myMasterRow["pmast_QTY_AVAILABLE"] = myOrPartRow["QTY_AVAILABLE"];
                        myMasterRow["pmast_STOCK_DESC"] = myOrPartRow["STOCK_DESC"];
                        break;
                    }
                }
            }
            if (!blnMatchFound)  // -- add it
            {
                newRow = dtCacheMaster.NewRow();
                newRow["recordSource"] = addType; //---"OR_PART";
                newRow["createDateTime"] = DateTime.Now;
                newRow["partNumber"] = myOrPartRow["PN"];
                newRow["partNumberStripped"] = myOrPartRow["PN_STRIPPED"];
                newRow["SessionID"] = Page.Session["MySessionId"].ToString();
                newRow["OracleLoaded"] = true;
                newRow["pmast_PN"] = myOrPartRow["PN"];
                newRow["pmast_PN_STRIPPED"] = myOrPartRow["PN_STRIPPED"];
                newRow["pmast_DESCRIPTION"] = myOrPartRow["DESCRIPTION"];
                newRow["pmast_ALT_PNM_AUTO_KEY"] = myOrPartRow["ALT_PNM_AUTO_KEY"];
                newRow["pmast_ALT_PN"] = myOrPartRow["ALT_PN"];
                newRow["pmast_ALT_PN_STRIPPED"] = myOrPartRow["ALT_PN_STRIPPED"];
                newRow["pmast_ALT_DESCRIPTION"] = myOrPartRow["ALT_DESCRIPTION"];
                newRow["pmast_ALT_LIST_PRICE"] = myOrPartRow["ALT_LIST_PRICE"];
                newRow["pmast_ALT_STC_AUTO_KEY"] = myOrPartRow["ALT_STC_AUTO_KEY"];
                newRow["pmast_ALT_QTY_AVAILABLE"] = myOrPartRow["ALT_QTY_AVAILABLE"];
                newRow["pmast_UNIT_OF_MEASURE"] = myOrPartRow["UNIT_OF_MEASURE"];
                newRow["pmast_ECCN"] = myOrPartRow["ECCN"];
                newRow["pmast_P_PNM_AUTO_KEY"] = myOrPartRow["P_PNM_AUTO_KEY"];
                newRow["pmast_P_STC_AUTO_KEY"] = myOrPartRow["P_STC_AUTO_KEY"];
                newRow["pmast_P_IFC_AUTO_KEY"] = myOrPartRow["IFC_AUTO_KEY"];
                newRow["pmast_LIST_PRICE"] = myOrPartRow["LIST_PRICE"];
                newRow["pmast_EXCH_LIST_PRICE"] = myOrPartRow["EXCH_LIST_PRICE"];
                newRow["pmast_QTY_AVAILABLE"] = myOrPartRow["QTY_AVAILABLE"];
                newRow["pmast_STOCK_DESC"] = myOrPartRow["STOCK_DESC"];

                //---- set some defaults
                newRow["cur_QTY_ORDERED"] = 0;
                newRow["cur_QTY_AvailableSearch"] = 0;
                newRow["cur_LAST_LIST_PANEL"] = string.Empty;
                newRow["calc_CUSTOM_UNIT_PRICE"] = 0m;
                newRow["calc_QTY_AVAILABLE"] = 0m;
                newRow["calc_NEXT_SHIP_QTY"] = 0;
                newRow["calc_NEXT_SHIP_DATE"] = string.Empty;
                newRow["calc_SELL_PRICE"] = 0m;
                newRow["calc_PRICE_TYPE_USED"] = string.Empty;

                dtCacheMaster.Rows.Add(newRow);
            }
        }
        return pmTemp;
    }

    protected DataTable defineCacheMaster(Boolean buildTable)
    {
        DataTable dtTemp = new DataTable();
        cacheDefine = new string[100, 3];
        cacheColumnCount = -1;
        int lp;
        cacheDefine[++cacheColumnCount, 0] = "pkCache"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "SessionID"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "CurrentSearchMatch"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "partNumber"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "partNumberStripped"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "recordSource"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "createDateTime"; cacheDefine[cacheColumnCount, 1] = "DateTime";
        cacheDefine[++cacheColumnCount, 0] = "OracleLoaded"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "NHALoaded"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "OEMNumberLoaded"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "OEMPartLoaded"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "imagesLoaded"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "nha_NHA"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "nha_ATAChapter"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "nha_PartsPerNHA"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "nha_OEMPN"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "nha_JPEPN"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "nha_OEMPrice"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "nha_JPEPrice"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "nha_Description"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "nha_NHAName"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_PN"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_PN_STRIPPED"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_DESCRIPTION"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_PNM_AUTO_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_PN"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_PN_STRIPPED"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_DESCRIPTION"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_LIST_PRICE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_STC_AUTO_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ALT_QTY_AVAILABLE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "pmast_UNIT_OF_MEASURE"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_ECCN"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "pmast_P_PNM_AUTO_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "pmast_P_STC_AUTO_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "pmast_P_IFC_AUTO_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "pmast_LIST_PRICE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "pmast_EXCH_LIST_PRICE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "pmast_QTY_AVAILABLE"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "pmast_STOCK_DESC"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG1_FILE_NAME"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG1_FILE_EXT"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG1_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG2_FILE_NAME"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG2_FILE_EXT"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG2_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG3_FILE_NAME"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG3_FILE_EXT"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG3_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG4_FILE_NAME"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG4_FILE_EXT"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "image_IMG4_KEY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "cur_QTY_ORDERED"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "cur_QTY_AvailableSearch"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "cur_LAST_LIST_PANEL"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "calc_CUSTOM_UNIT_PRICE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "calc_PRICE_SET"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "calc_PRICE_TYPE_USED"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "calc_QTY_AVAILABLE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        cacheDefine[++cacheColumnCount, 0] = "calc_AVL_SET"; cacheDefine[cacheColumnCount, 1] = "Boolean";
        cacheDefine[++cacheColumnCount, 0] = "calc_NEXT_SHIP_QTY"; cacheDefine[cacheColumnCount, 1] = "Int32";
        cacheDefine[++cacheColumnCount, 0] = "calc_NEXT_SHIP_DATE"; cacheDefine[cacheColumnCount, 1] = "String";
        cacheDefine[++cacheColumnCount, 0] = "calc_SELL_PRICE"; cacheDefine[cacheColumnCount, 1] = "Decimal";
        if (buildTable)
        {
            for (lp = 0; lp <= cacheColumnCount; lp++)
            {
                dtTemp.Columns.Add(addMyColumn(cacheDefine[lp, 0], cacheDefine[lp, 1]));
            }
        }
        return dtTemp;
    }

    protected DataColumn addMyColumn(string thisID, string thisType)
    {
        DataColumn col;
        col = new DataColumn();
        col.DataType = System.Type.GetType("System." + thisType);
        col.ColumnName = thisID;
        return col;
    }
        
    protected string[,] partsListRemoveDups(string[,] ckListToClean1, string[,] ckListToClean2)
    {
        string[,] ckTempRet = new string[1000, 6];
        string[,] ckCombinedList = new string[1000, 6];
        int lp, slp;
        string slpQty = string.Empty;
        string lpQty = string.Empty;
        int ckTempCounter = -1;
        int ckCombinedCounter = -1;
        for (lp = 0; lp <= 1999; lp++)
        {
            if (ckListToClean1[lp, 0] != null)
            {
                ++ckCombinedCounter;
                for (slp = 0; slp <= 5; slp++)
                {
                    ckCombinedList[ckCombinedCounter, slp] = ckListToClean1[lp, slp];
                }
            }
            else
            {
                break;
            }
        }
        for (lp = 0; lp <= 1999; lp++)
        {
            if (ckListToClean2[lp, 0] != null)
            {
                ++ckCombinedCounter;
                for (slp = 0; slp <= 5; slp++)
                {
                    ckCombinedList[ckCombinedCounter, slp] = ckListToClean2[lp, slp];
                }
            }
            else
            {
                break;
            }
        }
        //---------- Remove any duplicated
        for (lp = 0; lp <= ckCombinedCounter; lp++)
        {
            for (slp = 0; slp <= ckCombinedCounter; slp++)
            {
                if ((slp != lp) && (ckCombinedList[lp, 0] == ckCombinedList[slp, 0]))
                {
                    lpQty = GenUtils.MakeNumbersOnly(ckCombinedList[lp, 1].ToString());
                    slpQty = GenUtils.MakeNumbersOnly(ckCombinedList[slp, 1].ToString());
                    if (int.Parse(lpQty) > int.Parse(slpQty))
                    {
                        ckCombinedList[lp, 0] = "DUPLICATE";
                    }
                    else
                    {
                        ckCombinedList[slp, 0] = "DUPLICATE";
                    }
                }
            }
        }
        //----- produce a return array without duplicates
        for (lp = 0; lp <= ckCombinedCounter; lp++)
        {
            if (ckCombinedList[lp, 0] != "DUPLICATE")
            {
                ckTempCounter++;
                for (slp = 0; slp <= 5; slp++)
                {
                    ckTempRet[ckTempCounter, slp] = ckCombinedList[lp, slp];
                }
            }
        }
        //---------------------- Now add to col 3 the Oracle Lookup Status
        for (lp = 0; lp <= ckTempCounter; lp++)
        {
            foreach (DataRow myMasterRow in dtCacheMaster.Rows)
            {
                if (GenUtils.MakeUCaseNumChar(ckTempRet[lp, 0]) == myMasterRow["partNumberStripped"].ToString())
                {
                    if (myMasterRow["OracleLoaded"] != null && myMasterRow["OracleLoaded"] != DBNull.Value)
                    {
                        if ((Boolean)myMasterRow["OracleLoaded"])
                        {
                            ckTempRet[lp, 3] = "ORA+";
                        }
                    }
                    break;
                }
            }
        }
        return ckTempRet;
    }

    protected string[,] RemoveDuplicates(string[,] ckList)
    {
        int lp = -1;
        int slp = -1;
        int upperEl = findUpperElement(ckList);
        string lpQty = string.Empty;
        string slpQty = string.Empty;
        //---------- Remove any duplicated
        for (lp = 0; lp <= upperEl; lp++)
        {
            if ((ckList[lp, 2].Substring(0, 4) == "QTY-") && (int.Parse(ckList[lp, 1]) == 0))
            {
                ckList[lp, 0] = "ZERO";
            }
            for (slp = 0; slp <= upperEl; slp++)
            {
                if ((slp != lp) && (ckList[lp, 0] == ckList[slp, 0]))
                {
                    lpQty = GenUtils.MakeNumbersOnly(ckList[lp, 1].ToString());
                    slpQty = GenUtils.MakeNumbersOnly(ckList[slp, 1].ToString());
                    if (int.Parse(lpQty) > int.Parse(slpQty))
                    {
                        ckList[lp, 0] = "DUPLICATE";
                    }
                    else
                    {
                        ckList[slp, 0] = "DUPLICATE";
                    }
                }
            }
        }
        int ckTempCounter = -1;
        string[,] ckTempRet = new string[1000, 6];

        //----- produce a return array without duplicates
        for (lp = 0; lp <= upperEl; lp++)
        {
            if (ckList[lp, 0] != "DUPLICATE"  && ckList[lp, 0] != "ZERO")
            {
                ckTempCounter++;
                for (slp = 0; slp <6 ; slp++)
                {
                    ckTempRet[ckTempCounter, slp] = ckList[lp, slp];
                }
            }
        }
        return ckTempRet;
    }

    protected DataTable FindAllNHAItems(string[,] partList, Boolean blnPopthisNHA)
    {
        DataTable dtTemp = null;
        DataTable dtTemp2 = null;
        DataTable dtTemp3 = null;
        string conn = string.Empty;
        intNHAPopUpMatchesFound = 0;
        SqlCommand selectCMD;
        conn = ConfigurationManager.ConnectionStrings["SQL2005_CTDB_ConnectionString"].ConnectionString;
        SqlConnection sqlConn = new SqlConnection(conn);
        string queryString = "Select pall.*, n.NHAId, nha.ATAChapter, n.PartsPerNHA, nha.NHA from Part p JOIN Part2NHA n ON n.PartID = p.PartID JOIN Part2NHA nall ON nall.NHAID = n.NHAId JOIN Part pall ON pall.PartID = nall.PartID Join NHA nha ON nha.NHAID = nall.NHAId ";
        bool blnfirstplaced = false;
        queryString += " WHERE (";
        int lp;
        for (lp = 0; lp <= 999; lp++)
        {
            if (partList[lp, 0] == null)
            {
                break;
            }
            else
            {
                if (blnfirstplaced)
                {
                    queryString += " OR ";
                }
                else
                {
                    blnfirstplaced = true;
                }
                queryString += " REPLACE(p.OEMPN, '-', '') = '" + partList[lp, 0] + "' OR REPLACE(p.JPEPN, '-', '') = '" + partList[lp, 0] + "' ";
            }
        }
        queryString += ")  ORDER BY pall.JPEPN; ";

        if (blnfirstplaced)
        {
            selectCMD = new SqlCommand(queryString, sqlConn);
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
            }
            finally
            {
                sqlConn.Close();
            }
        }
        string strThisNHA = string.Empty;

        if (string.IsNullOrEmpty((string)Request.Form["txtSearch_Assembly"]) != true)
        {
            strThisNHA = Page.Session["strAssemblySearch"].ToString().ToLower();
                strThisNHA = strThisNHA.Replace("-", "");
                queryString = "Select nha.NHA  from NHA nha  WHERE (REPLACE(lower(nha.NHA), '-', '') like '" + strThisNHA + "%' ) ORDER BY  nha.NHA; ";
                SqlCommand selectCMD3 = new SqlCommand(queryString, sqlConn);
                SqlDataAdapter nhaDA3 = new SqlDataAdapter();
                nhaDA3.SelectCommand = selectCMD3;
                DataSet mynhaDS3 = new DataSet();
                try
                {
                    sqlConn.Open();
                    nhaDA3.Fill(mynhaDS3, "NHA3");
                    dtTemp3 = mynhaDS3.Tables["NHA3"];
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

                intNHAPopUpMatchesFound = dtTemp3.Rows.Count;

                if (!blnPopthisNHA)
                    Page.Session["strNHAPartialMatches"] = string.Empty;
                if (dtTemp3.Rows.Count > 1)
                {
                    foreach (DataRow mynhaRow in dtTemp3.Rows)
                    {
                        if (!blnPopthisNHA)
                            Page.Session["strNHAPartialMatches"] += mynhaRow["NHA"].ToString() + "|";
                    }
                }
                else if (dtTemp3.Rows.Count == 1)
                {
                    singleNHAMatch = dtTemp3.Rows[0]["NHA"].ToString();
                    singleNHAMatchtoPrint = singleNHAMatch;
                    singleNHAMatch = singleNHAMatch.Replace("-", "");
                    queryString = "Select p.*, n.NHAId, n.ATAChapter, n.PartsPerNHA, nha.NHA ";
                    queryString += " from Part p ";
                    queryString += " JOIN Part2NHA n ON n.PartID = p.PartID ";
                    queryString += " Join NHA nha ON nha.NHAID = n.NHAId  ";
                    queryString += " WHERE (REPLACE(lower(nha.NHA), '-', '') = '" + singleNHAMatch + "' AND n.NHAID = nha.NHAID ) ";
                    queryString += " ORDER BY p.JPEPN; ";
                    SqlCommand selectCMD2 = new SqlCommand(queryString, sqlConn);
                    selectCMD2.CommandTimeout = 30;
                    SqlDataAdapter nhaDA2 = new SqlDataAdapter();
                    nhaDA2.SelectCommand = selectCMD2;
                    DataSet mynhaDS2 = new DataSet();
                    try
                    {
                        sqlConn.Open();
                        nhaDA2.Fill(mynhaDS2, "NHA2");
                        if (dtTemp != null)
                        {
                            dtTemp2 = mynhaDS2.Tables["NHA2"];
                            dtTemp.Merge(dtTemp2);
                        }
                        else
                        {
                            dtTemp = mynhaDS2.Tables["NHA2"];
                        }
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
                }
        }
        return dtTemp;
    }

    protected int findJPNRow(int thisRow, DataTable dtCacheMaster)
    { 
        //-------- check a Cache Row to determine if it is the JPN Part Number.  Return the Row Number of the
        //      JPN Part if not or the current row if it is

        string findAltPN = string.Empty;
        int rowCounter = - 1;
        if (dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "2" && dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "20" && dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "4" && dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "6" && dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "12" && dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "30" && dtCacheMaster.Rows[thisRow]["pmast_P_STC_AUTO_KEY"].ToString() != "26")
        {
            findAltPN = dtCacheMaster.Rows[thisRow]["pmast_ALT_PN_STRIPPED"].ToString();
            foreach (DataRow myaltRow in dtCacheMaster.Rows)
            {
                rowCounter++;
                if (myaltRow["pmast_PN_STRIPPED"].ToString() == findAltPN)
                {
                    return rowCounter;
                }
            }
        }
        return thisRow;       
    }

    //************ Build the SmartBuyer Panel

    protected string buildSmartBuyerPanel(DataTable dtCacheMaster, string[,] searchWords, int swCounter)
    {
        string tmp = string.Empty;
        int lp;
        string strCartBtnTitle = "Add to Cart";
        if (!((bool)Page.Session["JPEapprovedForPricing"]))
        {
            strCartBtnTitle = "Add to RFQ";
        }

        //------------------------ Main View Port Header
        tmp = "<table class='SBecomTable'>";
        tmp += "<tr><td class='SBHeaderBarTD' colspan='3'style='width:100%'>";
        tmp += "<table><tr><td class='SBHeaderBarTD' style='width:50%;white-space:nowrap;'>JPE Smart Buyer</TD>";
        tmp += "<td style='padding-right:20px;text-align:right;font:verdana;color:#666666;font-size:9px; width:50%;white-space:nowrap;'><input type = 'button' id='btnSPBSaveTop' value = 'Check Available' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnCheckAvailable');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click the CHECK AVAILABLE button to check to see if the quantity indicated is currently available.</div>')" + (char)34 + ">&nbsp;&nbsp;<input type = 'button' id='btnSPBSaveTop' value = '" + strCartBtnTitle+ "' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQty');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click the " + strCartBtnTitle.ToUpper() + " button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.</div>')" + (char)34 + ">";
        tmp += "</td></tr></table></td></tr>";
        tmp += "<td class='SBViewPortBorderTD'></td>";
        tmp += "<td class='SBViewPortContentTD'>";

        // ----- step one: find exact matches and look for 
        string strPartNumber, strOEMPartNumber, strDescription, strOEMDescription, strImage, strLinkID;
        string strOEMLinkID;
        decimal decCost, decExt, decOEMCost;
        int intQtyOrdered, intOEMQtyOrdered, intBO, intPartKey, intIFCKey;
        int intCacheOrdered, intCacheAvlCheck;
        int intCurRow;
        decimal decAvl, decOEMAvl;
        string strSearchList = string.Empty;
        string strThisPN = string.Empty;
        string strThisSearch = string.Empty;
        int intStockCategoryKey, intOEMStockCategoryKey; 
        string[] strTemp;
        int intNHAThisSingleMatch = 0;
        bool blnNHAshowMore = false;
        int controlRowCounter = - 1;
        int tempRow = 0;
        bool showthisRow = false;
        string strDebug = string.Empty;
        bool blnShowDidYouMean = false;
        bool blnSingleMatchFound = false;
        decimal rawQtyAvailable = 0m;
        bool panelActive = false;
        bool blnThisisSearchRow = false;
        string strThisPartNumberStripped = string.Empty;
        string strThisOEMPartNumberStripped = string.Empty;
        bool blnPodOpened = false;
        bool thisPartisSellable;
        bool thisJPEPartisSellable;
        bool thisOEMPartisSellable;
        bool thisPartisSingleMatch;
        bool blnAvlSearchItem;
        int nhaRowMatch = -1;
        int intShowNHARows = 100;
        string[,] strPlacedPartNumbers = new string[1000, 10];
            //-- 0 part number
            //-- 1 stripped part number
            //-- 2 price
            //-- 3 qty
            //-- 4 panel type

      //----- Iterate through the Cach master table and place the items based on their status
      if (dtCacheMaster.Rows.Count > 0)
        {
            foreach (DataRow myControlRow in dtCacheMaster.Rows)
            {
                controlRowCounter++;
                showthisRow = false;

                blnSingleMatchFound = false;
                blnPodOpened = false;
                blnThisisSearchRow = false;
                //----- Identify the rows that match search criteria
                if (myControlRow["CurrentSearchMatch"] != DBNull.Value)
                {
                    if (myControlRow["CurrentSearchMatch"].ToString() != string.Empty)
                    {
                        showthisRow = true;
                        blnThisisSearchRow = true;
                        strSearchList += myControlRow["CurrentSearchMatch"] + " ";
                        strThisSearch = myControlRow["CurrentSearchMatch"].ToString();
                    }
                }

                //---- Identify the Items that have Available sotck
                blnAvlSearchItem = false;
                if (myControlRow["cur_QTY_AvailableSearch"] != DBNull.Value && blncheckAvlScrape)
                {
                   if  ((int)myControlRow["cur_QTY_AvailableSearch"] > 0 )
                    {
                        showthisRow = true;
                        blnAvlSearchItem = true;
                    }
                }

                //---- Identify the Items that have a current order quantity
                if (myControlRow["cur_QTY_ORDERED"] != DBNull.Value)
                {
                    if ((int)myControlRow["cur_QTY_ORDERED"] > 0)
                    {
                        showthisRow = true;
                        blnAvlSearchItem = false;
                    }
                }

                //---- set some local variables
                strThisPN = myControlRow["pmast_PN_STRIPPED"].ToString();
                intCurRow = controlRowCounter;
                //---- XRef to the prope row for OEM Part numbers
                tempRow = findJPNRow(controlRowCounter, dtCacheMaster);
                if (tempRow > 0)
                    intCurRow = tempRow;



                strThisPN = dtCacheMaster.Rows[intCurRow]["pmast_PN_STRIPPED"].ToString();
                if (intCurRow > -1 && showthisRow)
                {
                    strPartNumber = dtCacheMaster.Rows[intCurRow]["pmast_PN"].ToString();
                    strThisPartNumberStripped = dtCacheMaster.Rows[intCurRow]["pmast_PN_STRIPPED"].ToString();
                    strOEMPartNumber = dtCacheMaster.Rows[intCurRow]["pmast_ALT_PN"].ToString();
                    strDescription = dtCacheMaster.Rows[intCurRow]["pmast_DESCRIPTION"].ToString();
                    strLinkID = dtCacheMaster.Rows[intCurRow]["pmast_PN"].ToString() + "#" + RandomString2(6);

                    if (dtCacheMaster.Rows[intCurRow]["pmast_P_PNM_AUTO_KEY"] != DBNull.Value)
                    {
                        intPartKey = int.Parse(dtCacheMaster.Rows[intCurRow]["pmast_P_PNM_AUTO_KEY"].ToString());
                    }
                    strImage = GenUtils.findImageLink(intCurRow, dtCacheMaster);

                    //---- Set some local variables
                    //intStockCategoryKey = -1;
                    intIFCKey = -1;
                    decCost = 0m;
                    decAvl = 0m;
                    intBO = 0;
                    decOEMCost = 0m;
                    decOEMAvl = 0m;

                    decCost = (decimal)dtCacheMaster.Rows[intCurRow]["calc_SELL_PRICE"];
                    intCacheOrdered = (int)dtCacheMaster.Rows[intCurRow]["cur_QTY_ORDERED"];
                    intCacheAvlCheck = (int)dtCacheMaster.Rows[intCurRow]["cur_QTY_AvailableSearch"];
                    rawQtyAvailable = (decimal)dtCacheMaster.Rows[intCurRow]["calc_QTY_Available"];
                    intQtyOrdered = blncheckAvlScrape && (int)dtCacheMaster.Rows[intCurRow]["cur_QTY_AvailableSearch"] > 0 ? intQtyOrdered = (int)dtCacheMaster.Rows[intCurRow]["cur_QTY_AvailableSearch"] : 0;
                    intQtyOrdered = (int)dtCacheMaster.Rows[intCurRow]["cur_QTY_ORDERED"] > 0 ? (int)dtCacheMaster.Rows[intCurRow]["cur_QTY_ORDERED"] : intQtyOrdered;
                    decExt = decCost * intQtyOrdered;
                    intStockCategoryKey = dtCacheMaster.Rows[intCurRow]["pmast_P_STC_AUTO_KEY"].ToString() != string.Empty ? int.Parse(dtCacheMaster.Rows[intCurRow]["pmast_P_STC_AUTO_KEY"].ToString()) : -1;
                    intIFCKey = dtCacheMaster.Rows[intCurRow]["pmast_P_IFC_AUTO_KEY"] != DBNull.Value ? int.Parse(dtCacheMaster.Rows[intCurRow]["pmast_P_IFC_AUTO_KEY"].ToString()) : -1;

                    intBO = 0;
                    decAvl = (decimal)dtCacheMaster.Rows[intCurRow]["calc_QTY_AVAILABLE"] - intQtyOrdered;
                    if (decAvl > 0)
                    {
                        decAvl = Convert.ToDecimal(intQtyOrdered);
                    }
                    else
                    {
                        decAvl = (decimal)dtCacheMaster.Rows[intCurRow]["calc_QTY_AVAILABLE"];
                        intBO = intQtyOrdered - Convert.ToInt32(decAvl);
                    }



                    //------- Business Logic: Is this Part Sellable?

                    thisJPEPartisSellable = false;
                    if (decCost > 0 && (intStockCategoryKey == 2 || intStockCategoryKey == 4 || intStockCategoryKey == 6 || intStockCategoryKey == 12 || intStockCategoryKey == 20 || intStockCategoryKey == 30 || intStockCategoryKey == 26 || ((intStockCategoryKey == 3 && intCurRow == controlRowCounter) || (intStockCategoryKey == 29 && intCurRow == controlRowCounter))))
                    {
                        thisJPEPartisSellable = true;
                    }

                    //---------- determine if we have the OEM available
                    strTemp = strOEMPartNumber.Split(',');
                    int intOEMMatchRow = FindOEMItems(strTemp[0], dtCacheMaster);
                    thisOEMPartisSellable = false;
                    strOEMDescription = string.Empty;
                    intOEMQtyOrdered = 0;
                    strOEMLinkID = string.Empty;
                    
                    if (intOEMMatchRow > -1)
                    {
                        strOEMPartNumber = dtCacheMaster.Rows[intOEMMatchRow]["pmast_PN"].ToString();
                        strThisOEMPartNumberStripped = dtCacheMaster.Rows[intOEMMatchRow]["pmast_PN_STRIPPED"].ToString();
                        strOEMDescription = dtCacheMaster.Rows[intOEMMatchRow]["pmast_DESCRIPTION"].ToString();
                        strOEMLinkID = dtCacheMaster.Rows[intOEMMatchRow]["pmast_PN"].ToString() + "#" + RandomString2(6);
                        intOEMStockCategoryKey = dtCacheMaster.Rows[intOEMMatchRow]["pmast_P_STC_AUTO_KEY"].ToString() != string.Empty ? intOEMStockCategoryKey = Convert.ToInt32(dtCacheMaster.Rows[intOEMMatchRow]["pmast_P_STC_AUTO_KEY"].ToString()) : -1;

                        intOEMQtyOrdered = blncheckAvlScrape && (int)dtCacheMaster.Rows[intOEMMatchRow]["cur_QTY_AvailableSearch"] > 0 ? (int)dtCacheMaster.Rows[intOEMMatchRow]["cur_QTY_AvailableSearch"] : (int)dtCacheMaster.Rows[intOEMMatchRow]["cur_QTY_ORDERED"];
                        decOEMAvl = (decimal)dtCacheMaster.Rows[intOEMMatchRow]["calc_QTY_AVAILABLE"];
                        decOEMCost = (decimal)dtCacheMaster.Rows[intOEMMatchRow]["calc_SELL_PRICE"];

                        //---- Is the  OEM Sellable
                        if (decOEMCost > 0 && (intOEMStockCategoryKey == 10 || intOEMStockCategoryKey == 3) && decOEMAvl > 0)
                        {
                            thisOEMPartisSellable = true;
                        }
                        //---- end of OEM Setup
                    }

                    //---- Identify a sellable OEM with no sellable JPE part
                    if (thisOEMPartisSellable && !thisJPEPartisSellable)
                    {
                        decExt = -999m; //------ JPE part not sellable
                    }

                    if ((PartNumberOnScreen(strPartNumber, strThisPartNumberStripped, decCost.ToString(), intQtyOrdered.ToString(), "SingleMatch", strPlacedPartNumbers, "test") == -1 && thisJPEPartisSellable) || thisOEMPartisSellable) 
                    {
                        tmp += OpenPod(string.Empty, string.Empty);
                        blnPodOpened = true;
                        tmp += BuildFullMatchPanel(strPartNumber, strOEMPartNumber, strDescription, decCost, intQtyOrdered, decExt, decAvl, intBO, strImage, strLinkID, BuildstrDebug(intCurRow), blnAvlSearchItem, intCacheOrdered, intCacheAvlCheck, rawQtyAvailable, intStockCategoryKey);
                        PartNumberOnScreen(strPartNumber, strThisPartNumberStripped, decCost.ToString(), intQtyOrdered.ToString(), "SingleMatch", strPlacedPartNumbers, "Placed");
                        panelActive = true;
                        blnItemsOnPage = true;
                        intNHAThisSingleMatch = 0;
                        blnNHAshowMore = false;
                        blnSingleMatchFound = true;

                        //---- Is there an OEM Row to Place?
                        if (intOEMMatchRow > -1 && blnThisisSearchRow)
                        {
                            if (thisOEMPartisSellable)
                            {
                                tmp += addLabelBar("OEM Part");
                                tmp += BuildOEMPanel(strOEMPartNumber, strOEMDescription, intOEMQtyOrdered, decOEMCost, decOEMAvl, strOEMLinkID, BuildstrDebug(intOEMMatchRow));
                            }
                            PartNumberOnScreen(strOEMPartNumber, strThisOEMPartNumberStripped, decOEMCost.ToString(), intOEMQtyOrdered.ToString(), "OEMMatch", strPlacedPartNumbers, "Placed");
                            blnItemsOnPage = true;
                        }

                        //**** NHA ROWS
                        string strNHAPartNumber, strNHADescription, strNHAAssembly, intNHAQtyPerAssembly, strATAChapter;
                        Decimal decNHACost = 0m;
                        System.Data.DataTable dtThisNHA = GenUtils.FindNHAItems(strPartNumber, strOEMPartNumber);
                        bool blnNHATableOpened = false;
                        if (dtThisNHA != null && blnThisisSearchRow)
                        {
                            if (dtThisNHA.Rows.Count > 0)
                            {
                                string strLastNHA = dtThisNHA.Rows[0]["NHA"].ToString();
                                strATAChapter = dtThisNHA.Rows[0]["ATAChapter"].ToString();
                                foreach (DataRow mynhaRow in dtThisNHA.Rows)
                                {
                                    strNHAPartNumber = mynhaRow["JPEPN"].ToString();
                                    strNHAAssembly = mynhaRow["NHA"].ToString();
                                    intNHAQtyPerAssembly = mynhaRow["PartsPerNHA"].ToString();
                                    nhaRowMatch = GenUtils.FindRowforStrippedPartNumber(GenUtils.MakeUCaseNumChar(strNHAPartNumber), dtCacheMaster);
                                    strNHADescription = dtCacheMaster.Rows[nhaRowMatch]["pmast_Description"].ToString();
                                    rawQtyAvailable = (decimal)dtCacheMaster.Rows[nhaRowMatch]["calc_QTY_Available"];
                                    intQtyOrdered = blncheckAvlScrape && (int)dtCacheMaster.Rows[nhaRowMatch]["cur_QTY_AvailableSearch"] > 0 ? (int)dtCacheMaster.Rows[nhaRowMatch]["cur_QTY_AvailableSearch"] : (int)dtCacheMaster.Rows[nhaRowMatch]["cur_QTY_ORDERED"];
                                    decNHACost = (decimal)dtCacheMaster.Rows[nhaRowMatch]["calc_SELL_PRICE"];
                                    intStockCategoryKey = -1;
                                    if (dtCacheMaster.Rows[nhaRowMatch]["pmast_P_STC_AUTO_KEY"].ToString() != string.Empty)
                                    {
                                        intStockCategoryKey = int.Parse(dtCacheMaster.Rows[nhaRowMatch]["pmast_P_STC_AUTO_KEY"].ToString());
                                    }

                                    thisPartisSellable = false;
                                    if (decNHACost > 0 && (intStockCategoryKey == 2 || intStockCategoryKey == 4 || intStockCategoryKey == 6 || intStockCategoryKey == 12 || intStockCategoryKey == 20 || intStockCategoryKey == 12 || intStockCategoryKey == 20 || intStockCategoryKey == 26 || intStockCategoryKey == 30))
                                    {
                                        thisPartisSellable = true;
                                    }
                                    thisPartisSingleMatch = false;

                                    if (dtCacheMaster.Rows[nhaRowMatch]["CurrentSearchMatch"] != DBNull.Value)
                                    {
                                        if (dtCacheMaster.Rows[nhaRowMatch]["CurrentSearchMatch"].ToString() != string.Empty)
                                        {
                                            thisPartisSingleMatch = true;
                                        }
                                    }

                                    //if ( thisPartisSellable && !thisPartisSingleMatch)
                                    if (thisPartisSellable && !thisPartisSingleMatch)
                                    {
                                        //if (PartNumberOnScreen(strNHAPartNumber, "", decNHACost.ToString(), intQtyOrdered.ToString(), "NHA", strPlacedPartNumbers, "Placed") == -1 )
                                        PartNumberOnScreen(strNHAPartNumber, "", decNHACost.ToString(), intQtyOrdered.ToString(), "NHA", strPlacedPartNumbers, "Placed");
                                        //{
                                            blnItemsOnPage = true;
                                            if (intNHAThisSingleMatch <= intShowNHARows)
                                            {
                                                if (!blnNHATableOpened)
                                                {
                                                    tmp += addLabelBar(" <a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|" + strLastNHA + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '');" + (char)34 + "  onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Associated Parts - NHA: " + strLastNHA + "</a>");
                                                    tmp += openNHATable();
                                                    blnNHATableOpened = true;
                                                }
                                                else if (strNHAAssembly != strLastNHA)
                                                {
                                                    tmp += closeNHATable();
                                                    tmp += addLabelBar("<a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|" + strNHAAssembly + "|" + Page.Session["MySessionId"].ToString() + "|" + Page.Session["AccountType"].ToString() + "', event, '');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className='SBpodTitleBarMOMut';" + (char)34 + ">Associated Parts - NHA: " + strNHAAssembly + "</a>");
                                                    tmp += openNHATable();
                                                }
                                                strLastNHA = strNHAAssembly;
                                                tmp += BuildAssociatedLineItem(strNHAPartNumber, strNHADescription, decNHACost, 0, strNHAPartNumber + "#" + RandomString2(6), BuildstrDebug(nhaRowMatch), rawQtyAvailable);
                                                panelActive = true;
                                                intNHAThisSingleMatch++;
                                            }
                                            else if (!blnNHAshowMore)//-- place a show more row
                                            {
                                                blnNHAshowMore = true;
                                                tmp += BuildNHAShowMore(strNHAAssembly);
                                            }
                                        //}
                                    }
                                }
                                if (blnNHATableOpened)
                                    tmp += closeNHATable();
                            }
                        }
                    }
                    if  (blnPodOpened)
                        tmp += closePod();
                  }
                }
        }


        //************ Multi Match Section
        string tmpMulti = string.Empty;
        string tmpDidYouMean = string.Empty;
        string tmpNoMatch = string.Empty;
        string strThisSearchItem = string.Empty;
        string strThisAssigned = string.Empty;
        bool blnPartialMatchFound = false;
        bool blnDidYouMeanMatchFound = false;
        int intThisSearchLength = 0;
        int intdataRowCounter = 0;
        string[] strNoMatches = new string[1000];
        int intNoMatchCounter = -1;
        Hashtable MultiMatch = new Hashtable();
        intStockCategoryKey = -1;
        string showMax = dtGlobalAdmin.Rows[0]["PartialMatchShowMax"].ToString();
        int showMaxPerKey = Convert.ToInt16(showMax);

        for (lp = 0; lp <= swCounter; lp++)
        {
            if (SPBmasterUNIFIEDList[lp, 2] == "SearchPanel" || SPBmasterUNIFIEDList[lp, 2] == "List")
            {
                strThisSearchItem = GenUtils.MakeUniformPartNumber(SPBmasterUNIFIEDList[lp, 0]);
                strThisAssigned = SPBmasterUNIFIEDList[lp, 3];
                intThisSearchLength = strThisSearchItem.Length;
                int intTemp;
                string str1, str2, str3;
                if (intThisSearchLength > 0 && (strThisAssigned == string.Empty || strThisAssigned == null))
                {
                    if ((strThisAssigned == null || strThisAssigned == string.Empty) && dtCacheMaster != null)
                    {
                        blnPartialMatchFound = false;
                        if (dtCacheMaster.Rows.Count > 0)
                        {
                            intdataRowCounter = -1;
                            foreach (DataRow myfinalRow in dtCacheMaster.Rows)
                            {
                                intdataRowCounter++;
                                strThisPartNumberStripped = myfinalRow["pmast_PN_STRIPPED"].ToString();
                                if (strThisPartNumberStripped.Length >= intThisSearchLength)
                                {
                                    decCost = (decimal)myfinalRow["calc_SELL_PRICE"];
                                    str1 = strThisPartNumberStripped.Substring(0, intThisSearchLength);
                                    str2 = strThisSearchItem;
                                    str3 = myfinalRow["pmast_P_STC_AUTO_KEY"].ToString();
                                    if (str1 == str2 && decCost > 0 && (str3 == "2" || str3 == "4" || str3 == "6" || str3 == "12" || str3 == "20" || str3 == "26" || str3 == "30"))
                                    {
                                        intTemp = PartNumberOnScreen(strThisPartNumberStripped, strThisPartNumberStripped, "0", "0", "MULTI", strPlacedPartNumbers, "test");
                                        if (intTemp == -1 || intTemp == -2)
                                        {
                                            MultiMatch[strThisSearchItem] += intdataRowCounter.ToString() + ",";
                                            panelActive = true;
                                            blnPartialMatchFound = true;
                                            blnItemsOnPage = true;
                                        }
                                        else
                                        {
                                            blnPartialMatchFound = true;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                //************ Did You Mean
                blnDidYouMeanMatchFound = false;
                if (!blnPartialMatchFound && !blnSingleMatchFound)
                {
                    string strTest = GenUtils.MakeUniformPartNumber(strThisSearchItem);
                    string strWhereString;
                    string tryDYM = string.Empty;
                    string[] sortArray = new string[1000];
                    int cnt = -1;
                    string thisCompare = string.Empty;
                    int startEl;
                    string trimmedPartNumber = string.Empty;
                    //---speed up the 3's
                    string leadLookup = strTest.Substring(0, 1) == "3" ? strTest.Substring(0, 2) : strTest.Substring(0, 1);
                    if (strTest.Length > 2)
                    {
                        strWhereString = " WHERE p.PN_STRIPPED Like'" + GenUtils.MakeUCaseNumChar(leadLookup) + "%' AND ( P.STC_AUTO_KEY = 2 OR P.STC_AUTO_KEY = 4 OR P.STC_AUTO_KEY = 6 OR P.STC_AUTO_KEY = 12 OR P.STC_AUTO_KEY = 20 OR P.STC_AUTO_KEY = 26 OR P.STC_AUTO_KEY = 30 ) ";
                        //--- OK No Match so load the parts that start with 
                        DataTable lowTemp = GenUtils.FindParts(strWhereString, "  AND (ROWNUM < 998) ");
                        foreach (DataRow myRow in lowTemp.Rows)
                        {
                            trimmedPartNumber = myRow["PN_STRIPPED"].ToString().Length > strTest.Length ? myRow["PN_STRIPPED"].ToString().Substring(0, strTest.Length) : myRow["PN_STRIPPED"].ToString();


                            thisCompare = GenUtils.LevenshteinDistance(trimmedPartNumber, strTest).ToString();
                            while (thisCompare.Length < 3)
                            {
                                thisCompare = "0" + thisCompare;
                            }
                            sortArray[++cnt] = thisCompare + "-" + myRow["PN"].ToString();
                            Debug.WriteLine(sortArray[cnt]);
                            
                        }
                        System.Array.Sort(sortArray);
                        startEl = 1000 - cnt;
                        if (startEl < 1000)
                        {
                            tryDYM = BuildDidYouMeanPanel(sortArray, startEl, lowTemp, strTest, SPBmasterUNIFIEDList[lp, 5]);

                            if (tryDYM != string.Empty)
                            {
                                tmpDidYouMean += tryDYM;

                                blnDidYouMeanMatchFound = true;
                                panelActive = true;
                                blnShowDidYouMean = true;
                                //----- reload the Cache Table
                                dtCacheMaster = GenUtils.GetCacheforSessionID(Page.Session["MySessionId"].ToString());
                            }

                           

                            //for (int y = 1000 - cnt; y < 1000; y++)
                            //{
                            //    Debug.WriteLine(sortArray[y]);
                            //}
                        }
                    }
                }

                if (!blnPartialMatchFound && !blnDidYouMeanMatchFound)
                {
                    if (PartNumberOnScreen(strThisSearchItem, strThisSearchItem, "0", "0", "NOMATCH", strPlacedPartNumbers, "Placed") == -1)
                    {
                        strNoMatches[++intNoMatchCounter] = SPBmasterUNIFIEDList[lp, 5]; //00000000000
                        panelActive = true;
                    }
                }
            }
        }

        string strThisValue = string.Empty;
        int intThisPartsRow = -1;
        decimal decListPrice = 0m;
        decimal decExchangePrice = 0m;
        int PartialMatchesFound = -1;
        int PartialMatchCounter = 0;
        int PartialNHAMatchCount = 0;
        int andMore = -1;
        string tmpNHAMulti = string.Empty;



        string AddParts = string.Empty;

        foreach (string key in MultiMatch.Keys)
        {
            strThisValue = MultiMatch[key].ToString();
            string[] ntemp = strThisValue.Split(',');
            PartialMatchesFound = ntemp.GetUpperBound(0) + 1;
            //------ add a multiple match pod
            tmpMulti += OpenPod("multi", key);
            PartialMatchCounter = 0;
            andMore = - 1;
            for (int tlp = 0; tlp <= ntemp.GetUpperBound(0); tlp++)
            {
                if (ntemp[tlp] != string.Empty)
                {
                    if (++PartialMatchCounter >= showMaxPerKey)
                    {
                        andMore = PartialMatchesFound - PartialMatchCounter;
                    }
                    intThisPartsRow = int.Parse(ntemp[tlp].ToString());
                    decListPrice = decimal.Parse(dtCacheMaster.Rows[intThisPartsRow]["pmast_LIST_PRICE"].ToString());
                    decExchangePrice = decimal.Parse(dtCacheMaster.Rows[intThisPartsRow]["pmast_EXCH_LIST_PRICE"].ToString());
                    intPartKey = int.Parse(dtCacheMaster.Rows[intThisPartsRow]["pmast_P_PNM_AUTO_KEY"].ToString());
                    if (dtCacheMaster.Rows[intThisPartsRow]["pmast_P_STC_AUTO_KEY"].ToString() != string.Empty)
                    {
                        intStockCategoryKey = int.Parse(dtCacheMaster.Rows[intThisPartsRow]["pmast_P_STC_AUTO_KEY"].ToString());
                    }
                    rawQtyAvailable = (decimal)dtCacheMaster.Rows[intThisPartsRow]["calc_QTY_Available"];
                    intQtyOrdered = blncheckAvlScrape && (int)dtCacheMaster.Rows[intThisPartsRow]["cur_QTY_AvailableSearch"] > 0 ? (int)dtCacheMaster.Rows[intThisPartsRow]["cur_QTY_AvailableSearch"] : (int)dtCacheMaster.Rows[intThisPartsRow]["cur_QTY_ORDERED"]; 
                    decCost = (decimal)dtCacheMaster.Rows[intThisPartsRow]["calc_SELL_PRICE"];
                    strImage = GenUtils.findImageLink(intThisPartsRow, dtCacheMaster);

                    thisPartisSellable = false;

                    if (decCost > 0 && (intStockCategoryKey == 2 || intStockCategoryKey == 4 || intStockCategoryKey == 6 || intStockCategoryKey == 12 || intStockCategoryKey == 20))
                    {
                        thisPartisSellable = true;
                    }

                    if (thisPartisSellable)
                    {
                        tmpMulti += BuildMultiMatchPanel(dtCacheMaster.Rows[intThisPartsRow]["pmast_PN"].ToString(), dtCacheMaster.Rows[intThisPartsRow]["pmast_Description"].ToString(), decCost, 0, strImage, dtCacheMaster.Rows[intThisPartsRow]["pmast_PN"].ToString(), BuildstrDebug(intThisPartsRow), key, andMore, rawQtyAvailable, "multi");
                        panelActive = true;
                        AddParts += dtCacheMaster.Rows[intThisPartsRow]["pmast_PN_STRIPPED"].ToString() + "|";
                    }
                }
                if (andMore > 0)
                {
                    blnPartialMatchFound = true;
                    break;
                }
            }
            tmpMulti += closePod();
        }
        if (AddParts != string.Empty)
        {
            PartsCacheManager("AddParts", AddParts);
        }
       
        //---- Add the Multi Match Panel to the display
        if (MultiMatch.Count > 0)
        {
            tmp += tmpMulti;
        }

        //---- Add the Did You Mean Panel to the display
        if (blnShowDidYouMean)
        {
            tmp += tmpDidYouMean;
        }

        string[,] nhaPartialMatches = new string[1000, 1];
        string[] nhaPartials = Page.Session["strNHAPartialMatches"].ToString().Split('|');

        int ct;
        PartialNHAMatchCount = nhaPartials.GetUpperBound(0);
        for (ct = 0; ct <= PartialNHAMatchCount; ct++)
        {
            nhaPartialMatches[ct, 0] = nhaPartials[ct];
        }

        if (nhaPartials != null)
        {
            if (string.IsNullOrEmpty(nhaPartials[0]) != true)
            {
                tmpNHAMulti = OpenPod("nhamulti", Page.Session["strAssemblySearch"].ToString());
                tmpNHAMulti += BuildNHAMultiMatchPanel(nhaPartialMatches, PartialNHAMatchCount, Page.Session["strLastNHASearchString"].ToString());
                panelActive = true;
                tmpNHAMulti += closePod();
                tmp += tmpNHAMulti;
            }
        }

        //---- add the no matchess
        if (intNoMatchCounter > -1 )
        {
            tmp += BuildNoMatchPanel(strNoMatches, intNoMatchCounter);
        }

        //------------------------- Nothing Indicated
        if (dtCacheMaster.Rows.Count == 0 && MultiMatch.Count == 0 && intNoMatchCounter == 0 && !panelActive && PartialNHAMatchCount == 0)
        {
            tmp += OpenPod(string.Empty, string.Empty);
            tmp += "<tr><td>It does not appear that you entered any items to search for.</td></tr>";
            tmp += closePod();
        }

        //---------------------- Close the main Viewport table and add footer
        tmp += "<!-- End  Table Viewport Area  -->";
        tmp += "</td><td class='SBViewPortBorderTD'>";
        tmp += "</td></tr>";
        tmp += "<tr><td class='SBViewPortFooterTD' colspan='3'>";
        tmp += "<table><tr><td style='padding-right:0px;text-align:right;font:verdana;color:#666666;font-size:9px; width:100%;white-space:nowrap;'><input type = 'button' id='btnSPBSaveTop' value = 'Check Available' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnCheckAvailable');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click the CHECK AVAILABLE button to check to see if the quantity indicated is currently available.</div>')" + (char)34 + ">&nbsp;&nbsp;<input type = 'button' id='btnSPBSaveBottom' value = '" + strCartBtnTitle + "' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQty');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'<div style=&quot;text-align:left;&quot;>Click the " + strCartBtnTitle.ToUpper() + " button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.</div>')" + (char)34 + "></td></tr></table>"; 
        tmp += "</td></tr></table>";

        if (panelActive)
        {
            return tmp;
        }
        else
        {
            return string.Empty;
        }
    }

    protected string BuildstrDebug(int thisCacheRow)
    {
        string temp = string.Empty;
        if (!blnShowDevelopmentBlueCode)
        {
            return string.Empty;
        }

        if (thisCacheRow > -1)
        {
            temp = "<br><font style='color:blue; font-size:9px; font-weight:normal;'>[Avl: " + dtCacheMaster.Rows[thisCacheRow]["calc_QTY_AVAILABLE"] + " List: " + dtCacheMaster.Rows[thisCacheRow]["pmast_LIST_PRICE"] + " Exch [airline]: " + dtCacheMaster.Rows[thisCacheRow]["pmast_EXCH_LIST_PRICE"] + "  Custom: " + dtCacheMaster.Rows[thisCacheRow]["calc_CUSTOM_UNIT_PRICE"] + " IFC: " + dtCacheMaster.Rows[thisCacheRow]["pmast_P_IFC_AUTO_KEY"].ToString() + "  Stock Categ: " + dtCacheMaster.Rows[thisCacheRow]["pmast_P_STC_AUTO_KEY"].ToString() + " PNM_AUTO_KEY: " + dtCacheMaster.Rows[thisCacheRow]["pmast_P_PNM_AUTO_KEY"] + " ALT_PN: " + dtCacheMaster.Rows[thisCacheRow]["pmast_ALT_PN"] + " ALT_STC: " + dtCacheMaster.Rows[thisCacheRow]["pmast_ALT_STC_AUTO_KEY"] + " Image File: " + dtCacheMaster.Rows[thisCacheRow]["image_IMG1_FILE_NAME"] + "  Image 2 - PMA " + dtCacheMaster.Rows[thisCacheRow]["image_IMG2_FILE_NAME"] + " Part Key: " + dtCacheMaster.Rows[thisCacheRow]["pmast_P_PNM_AUTO_KEY"]  + "]</font>";
        }

        if (Page.Session["UserName"] != null)
        {
            lblSteveDebug.Text = "  <br><font style='color:blue; font-size:9px; font-weight:normal;'>[Panel Mode: " + Page.Session["PanelMode"] + " User Name: " + Page.Session["UserName"] + " User ID: " + Page.Session["LoggedInUserID"] + " Company: " + Page.Session["Company"] + "  Account Type: " + Page.Session["AccountType"] + " RoloDex Key: " + Page.Session["RolodexKey"] + " Quantum Company Code:  " + Page.Session["QuantumCompanyCode"] + " useGlobalLine: " + useGlobalLine + " CMP_AUTO_KEY: " + Page.Session["Quantum_CMP_AUTO_KEY"] + " HxFrom: " + Page.Session["hxFromDate"] + " HxTo: " + Page.Session["hxToDate"] + " HxPO: " + Page.Session["hxPartNo"] + " TERM CODE: " + Page.Session["TMC_AUTO_KEY"].ToString() + " Term Description: " + Page.Session["TERM_DESCRIPTION"] + " Pricing Type: " + Page.Session["Company_Type_for_Price"].ToString() + "]</font>";
        }
        return temp;
    }

    protected int FindOEMItems(string strJPEPartNumber, DataTable dtCacheMaster)
    {
        int intLclMatchRow = -1;
        int intRowCounter = -1;
        if (strJPEPartNumber.Trim() == string.Empty)
        {
            return -1;
        }
       
        foreach (DataRow myPNRow in dtCacheMaster.Rows)
        {
            intRowCounter++;
            if (strJPEPartNumber == myPNRow["pmast_PN"].ToString())
            {
                intLclMatchRow = intRowCounter;
                break;
            }
        }  
        return intLclMatchRow;
    }

    protected int PartNumberOnScreen(string thisPartNumber, string thisPartNumberStripped, string thisPrice, string thisQty, string thisPanelType, string[,] strPlacedPartNumbers, string thisFunction)
    {
        int intAlreadyPlacedElement = -1;
        int lp;
        if ((thisPanelType == "NHA") && (int.Parse(thisQty) > 0))
        {
            return -2;
        }
        for (lp = 0; lp <= intPlacedPartNumberCounter; lp++)
        {
            if (thisPartNumber == strPlacedPartNumbers[lp, 0] || ((thisPartNumberStripped == strPlacedPartNumbers[lp, 1] && thisPartNumberStripped != string.Empty)))
            {
                intAlreadyPlacedElement = lp;
            }
        }
        if (intAlreadyPlacedElement == -1 && thisFunction == "Placed")
        {
            strPlacedPartNumbers[++intPlacedPartNumberCounter, 0] = thisPartNumber;
            strPlacedPartNumbers[intPlacedPartNumberCounter, 1] = thisPartNumberStripped;
            strPlacedPartNumbers[intPlacedPartNumberCounter, 2] = thisPrice;
            strPlacedPartNumbers[intPlacedPartNumberCounter, 3] = thisQty;
            strPlacedPartNumbers[intPlacedPartNumberCounter, 3] = thisPanelType;
        }

        //string[,] strPlacedPartNumbers = new string[500, 10];
        //-- 0 part number
        //-- 1 stripped part number
        //-- 2 price
        //-- 3 qty
        //-- 4 panel type

        return intAlreadyPlacedElement;
    }

    public void ftpfile(string ftpfilepath, string inputfilepath)   
    {   
    string ftphost = "96.31.41.94";  
    //here correct hostname or IP of the ftp server to be given   

    string ftpfullpath = "ftp://" + ftphost + ftpfilepath;
    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);   
    ftp.Credentials = new NetworkCredential("jetpartseng", "sell737ng");      
    //userid and password for the ftp server to given   

    ftp.KeepAlive = true;   
    ftp.UseBinary = true;   
    ftp.Method = WebRequestMethods.Ftp.UploadFile;   
    FileStream fs = File.OpenRead(inputfilepath);   
    byte[] buffer = new byte[fs.Length];   
    fs.Read(buffer, 0, buffer.Length);   
    fs.Close();

    ftp.Proxy = null;

    Stream ftpstream = ftp.GetRequestStream();   
    ftpstream.Write(buffer, 0, buffer.Length);   
    ftpstream.Close();   
    } 

    protected string RandomString2(int thisSize)
    {
        Random r = new Random(seed++);
        string legalChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEGHIJKLMNOPQRSTUVWXYZ";
        StringBuilder sb = new StringBuilder();
        for (int i=0; i < thisSize; i++)
        {
            sb.Append(legalChars.Substring(r.Next(0,legalChars.Length - 1), 1));
        }

         Debug.WriteLine("--" +  sb.ToString());
        return sb.ToString();


    }

}



