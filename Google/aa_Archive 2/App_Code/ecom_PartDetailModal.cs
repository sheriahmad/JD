using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web.Services;
using System.Threading;
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
using System.Web.Services.Protocols;
using System.Web.SessionState;


/// <summary>
/// Summary description for ecom_PartDetailModal
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ecom_PartDetailModal : System.Web.Services.WebService {

    public ecom_PartDetailModal () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod] 
    public string FetchOneCustomer(string customerid) 
    { 
      string sql = "select * from Customers where cus_code=" + customerid; 
      //SqlDataAdapter da = new SqlDataAdapter(sql, “Your connection string”); 
      //DataSet ds = new DataSet(); 
      //da.Fill(ds); 
      //return ds.GetXml();
      return string.Empty;
    }



    [WebMethod]
    public string BuildDetailPanel(string strContext)
    {
        string[] temp = strContext.Split('|');
        string strPanel = string.Empty;


        switch (temp[0])
        {
            case "NHA":
                strPanel = BuildNHAPanel(temp[1], temp[2], temp[3]);
                break;
            case "PN":
                strPanel = BuildPartsPanel(temp[1], temp[2], temp[3]);
                break;
        }

        Thread.Sleep(2000);

        return strPanel;

    }

    [WebMethod]
    public string DeconDetailPanel(string strPartNumber)
    {
        string temp = string.Empty;

        temp += "<div style='margin:0px auto;text-align:center;vertical-align:middle;' >"; 
        temp += "<image src ='images\\progressbar.gif'  >";
        temp += "</div>";

        return temp;
    }



    public string BuildPartsPanel(string strPartNumber, string MySessionId, string MyAccountType)
    {
        string temp = string.Empty;

        DataTable dtCacheMaster = null;

        //DataTable dtNHA = null;



        //------------- Lookup the Part

        string strImage = string.Empty;
        string strAltPartNumber = string.Empty;
        string strPrice = string.Empty;
        string strUnitofMeasure = string.Empty;
        string strCondition = string.Empty;
        string strPMA1 = string.Empty;
        string strPMA2 = string.Empty;

        string strECCN = string.Empty;
        string strQtyControl = string.Empty;
        string strAvailable = string.Empty;
        string strBO = string.Empty;
        string strEXT = string.Empty;
        string strthisNHA = string.Empty;
        //int[] thisNHAList;
        string strPMAFileName1;
        //string strPMAFileName2;
        decimal rawQtyAvailable;

        string strTechDataSheet;
        string strTechDataFileName;
       

        int thisQtyOrdered;
        decimal decAvl;
        decimal thisPrice;
        int intBO;

        string[,] strPlacedPartNumbers = new string[1000, 10];
       


        string strCurrentPN = "PN|" + strPartNumber + "|" + MySessionId ;



        int intPartRow = - 1;

        dtCacheMaster = GenUtils.GetCacheforSessionID(MySessionId);


        if (dtCacheMaster != null)
        {
           
            intPartRow = GenUtils.FindRowforStrippedPartNumber(GenUtils.MakeUCaseNumChar(strPartNumber), dtCacheMaster);

            //--------------- This part is not in the cache
            if (intPartRow == -1)
            {
                temp = BuildPanelHeader("Parts Detail Panel");
                temp += "ERROR ---- ERROR";
                temp += BuildPanelFooter(0);
                return temp;
            }



           //------ load the image
            strImage = GenUtils.findImageLink(intPartRow, dtCacheMaster);

            //------- Look for the alternate part nyumber
            //thisNHAList = new int[1000];
            //thisNHAList = GenUtils.FindAltPartNumber(intPartRow, dtCacheMaster, string.Empty);
            //if (thisNHAList[0] > -1)
            //{
            strAltPartNumber = dtCacheMaster.Rows[intPartRow]["pmast_ALT_PN"].ToString();
            //}
            
            
            
              //----- Condition
            strCondition = "New";
            //----- Unit of Measure
            strUnitofMeasure = dtCacheMaster.Rows[intPartRow]["pmast_UNIT_OF_MEASURE"].ToString();

            thisQtyOrdered = 0;
            //-------- Set the price
            if ((int)dtCacheMaster.Rows[intPartRow]["cur_QTY_ORDERED"] > 0 )
            {
                thisQtyOrdered = (int)dtCacheMaster.Rows[intPartRow]["cur_QTY_ORDERED"];
            }
            else if ((int)dtCacheMaster.Rows[intPartRow]["cur_QTY_AvailableSearch"] > 0)
            {
                thisQtyOrdered = (int)dtCacheMaster.Rows[intPartRow]["cur_QTY_AVAILABLE"];

            }


            rawQtyAvailable = (decimal)dtCacheMaster.Rows[intPartRow]["calc_QTY_AVAILABLE"];

            thisPrice = decimal.Parse(dtCacheMaster.Rows[intPartRow]["calc_SELL_PRICE"].ToString());


            strPrice = String.Format("{0:C}", thisPrice);
            strEXT = String.Format("{0:C}", (thisPrice * thisQtyOrdered));

            //------- TEch Data Sheet  IMage2
            strTechDataSheet = string.Empty;
            strTechDataFileName = string.Empty;

            if (Convert.ToInt16(dtCacheMaster.Rows[intPartRow]["image_IMG2_KEY"].ToString()) > 0 )
            {
                strTechDataFileName = dtCacheMaster.Rows[intPartRow]["image_IMG2_FILE_NAME"].ToString();
                strTechDataSheet = "<a href='http://jetserve1.jetpartsengineering.com/qimg/" + dtCacheMaster.Rows[intPartRow]["image_IMG2_KEY"].ToString() + ".qid' target='_new'>" + strTechDataFileName + "</a> [click to view]</a>";
            }


            //--- PMA Supplement
            short shLp;
            strPMA1 = string.Empty;
            strPMAFileName1 = "";
            bool firstPlaced = false;
            string thisFileType = string.Empty;

            for (shLp = 3; shLp <= 4; shLp++)
            {
                
                if (dtCacheMaster.Rows[intPartRow]["image_IMG" + shLp + "_KEY"].ToString() != string.Empty)
                {
                    strPMAFileName1 = dtCacheMaster.Rows[intPartRow]["image_IMG" + shLp+ "_FILE_NAME"].ToString();
                    //strPMA1 = string.Empty;
                    thisFileType = dtCacheMaster.Rows[intPartRow]["image_IMG" + shLp + "_FILE_EXT"].ToString();
                    if (strPMAFileName1.IndexOf(".") > 0 && thisFileType == "2" )
                    {
                        if (firstPlaced)
                        {
                            strPMA1 += "</br>";
                        }
                        else
                        {
                            firstPlaced = true;
                        }
                        strPMAFileName1 = strPMAFileName1.Substring(0, strPMAFileName1.IndexOf("."));
                        strPMA1 += "<a href='http://jetserve1.jetpartsengineering.com/qimg/" + dtCacheMaster.Rows[intPartRow]["image_IMG" + shLp + "_KEY"].ToString() + ".qid' target='_new'>" + strPMAFileName1 + "</a> [click to view]</a>";

                    }
                }


            }



         

                 
            //----- ECCN
            strECCN = dtCacheMaster.Rows[intPartRow]["pmast_ECCN"].ToString();

            //---- Quantity COntrol
            strQtyControl = "<input type='text' name='txtQTY_DetailPanel_" + GenUtils.MakeUCaseNumChar(strPartNumber) + "' class='SBtxtQtyCntrl' id='txtQTY_DetailPanel_" + GenUtils.MakeUCaseNumChar(strPartNumber) + "' maxlength='5' onKeyPress='return checkIt(event)'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQtyPopup', 'txtQTY_DetailPanel_" + GenUtils.MakeUCaseNumChar(strPartNumber) + "');" + (char)34 + " value='" + thisQtyOrdered.ToString() + "'  onFocus ='javascript:this.select();'>&nbsp;&nbsp;";

            strQtyControl += "<input type='hidden'ID='hidden_txtQTY_DetailPanel_" + GenUtils.MakeUCaseNumChar(strPartNumber) + "' name='hidden_txtQTY_DetailPanel_" + GenUtils.MakeUCaseNumChar(strPartNumber) + "' value = '" + thisQtyOrdered.ToString() + "'>";

            //----- log the PN as beuing onb screen
            PartNumberOnScreen(strPartNumber, "", "0", "0", "SingleMatch", strPlacedPartNumbers);


            //------Avaialble Back Order
           decAvl = (decimal)dtCacheMaster.Rows[intPartRow]["calc_QTY_AVAILABLE"];
           strAvailable = "0";
           if (dtCacheMaster.Rows[intPartRow]["calc_QTY_AVAILABLE"].ToString() != string.Empty)
           {
               strAvailable = string.Format("{0:#####}", Convert.ToDecimal(dtCacheMaster.Rows[intPartRow]["calc_QTY_AVAILABLE"].ToString()));
           }



           intBO = 0;

           if (decAvl >= thisQtyOrdered)
           {
               decAvl = Convert.ToDecimal(thisQtyOrdered);
           }
           else if (thisQtyOrdered > decAvl)
           {
               decAvl = (decimal)dtCacheMaster.Rows[intPartRow]["calc_QTY_AVAILABLE"];
               intBO = thisQtyOrdered - Convert.ToInt32(decAvl);
           }

           strBO =  string.Format("{0:#####}", intBO);




        }
        else
        {
           temp = BuildPanelHeader("Parts Detail Panel");
           temp += "ERROR ---- ERROR";
           temp += BuildPanelFooter(0);
           return temp;
        }

        temp = BuildPanelHeader("Parts Detail Panel");

        temp += "<!-- Begin Table Viewport Area -->";


        
        temp += "<table>";
        temp += "<tr><td class='partsImageTD' rowspan='3' >";
        temp += "<img src='" + strImage + "' class='partsImage'/></td>";
        temp += "<td class='partsPartNumberTD' colspan='2'><span class='replacesLabel'>JPE Part Number: </span> " + strPartNumber + "</td></tr>";
        temp += "<tr><td class='partsReplacesTD' colspan='2'><span class='replacesLabel'>Replaces: </span>" + strAltPartNumber + "</td></tr>";
       
        temp += "<tr><td class='partsDescriptionTD'>" + dtCacheMaster.Rows[intPartRow]["pmast_DESCRIPTION"] + "</td><td class='partsReplacesTD'><input type='button' ID='btnPartsPanelSave1'   onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsClosePopup');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.')" + (char)34 + "  value='Save & Close' />&nbsp;<input type='button' ID='btnPartsPaneClose'   OnClick=" + (char)34 + "javascript: return closePartsPopup('cancel', 'ModalPopupExtenderPartsDetail' );" + (char)34 + "  value='Close Window' /><br><span style='font-size:10px; color:#999999; text-aign:right;padding-right:5px;'>Save changes before you navigate away from window.</span></td></tr>";

        temp += "</table>";


        temp += "</td></tr>";

         temp += "<tr><td colspan='2'>";
         temp += "<table class='partsContainerTable'>";
         temp += "<tr><td class = 'partsSpacer'colspan='2'>";
         temp += "</td></tr>";

         temp += "<tr><td class='partsLabels'>Condition</td>";
         temp += "<td class='partsValues'>" + strCondition + "</td></tr>";
         temp += "<tr><td class='partsSpacer' colspan='2'></td>";

         temp += "<tr><td class='partsLabels'>Unit of Measure</td>";
         temp += "<td class='partsValues'>" + strUnitofMeasure + "</td></tr>";



         if (MyAccountType != "guest")
         {
             temp += "<tr><td class='partsSpacer' colspan='2'></td>";
             temp += "<tr><td class='partsLabels'>Price</td>";
             temp += "<td class='partsValues'>" + strPrice + "</td></tr>";

         }

         temp += "<tr><td class='partsSpacer' colspan='2'></td>";
         temp += "<tr><td class='partsLabels'>Current Order Quantity</td>";
         temp += "<td class='partsValues'>" + strQtyControl + " " ;
         
        if (rawQtyAvailable > 0)
         {
             temp += "<img src='images/instock5.gif' alt='' />";
         }
         else
         {
             temp += "<img src='images/callstock.gif' alt='' />";
         }
            
         temp += "</td></tr>";


         if (MyAccountType != "guest")
         {
             temp += "<tr><td class='partsSpacer' colspan='2'></td>";
             temp += "<tr><td class='partsLabels'>Extend</td>";
             temp += "<td class='partsValues'>" + strEXT + "</td></tr>";
         }


         temp += "<tr><td class='partsSpacer' colspan='2'></td>";
         temp += "<tr><td class='partsLabels'>PMA Supplement</td>";

         temp += "<td class='partsValues'>" + strPMA1;
         
             
             
         temp += "</td></tr>";

         if (strTechDataSheet != string.Empty)
         {
             temp += "<tr><td class='partsSpacer' colspan='2'></td>";
             temp += "<tr><td class='partsLabels'>Tech Data Sheet</td>";

             temp += "<td class='partsValues'>" + strTechDataSheet;

             temp += "</td></tr>";
         }




        // temp += "<tr><td class='partsSpacer' colspan='2'></td>";

    

        // temp += "<tr><td class='partsLabels'>Available</td>";
        // temp += "<td class='partsValues'>" + strAvailable + "</td></tr>";

        //if (strBO != string.Empty)
        //{
        // temp += "<tr><td class='partsSpacer' colspan='2'></td>";

        // temp += "<tr><td class='partsLabels'>Back Ordered</td>";
        // temp += "<td class='partsValues'>" + strBO + "</td></tr>";
        //}











        int nhaRowMatch = -1;
        int intStockCategoryKey;
        bool thisPartisSellable;
        int NHACount = 0;

        int intQtyOrdered;



        // ***************************************** NHA ROWS


        string strNHAPartNumber, strNHADescription, strNHAAssembly, intNHAQtyPerAssembly, strATAChapter;
        Decimal decNHACost = 0m;

        System.Data.DataTable dtThisNHA = null;
        dtThisNHA = GenUtils.FindNHAItems(strPartNumber, "");


        //GenUtils.PrintTableOrView(dtThisNHA, "NHA Table");

        bool blnNHATableOpened = false;
        if (dtThisNHA != null)
        {
            if (dtThisNHA.Rows.Count > 0)
            {
                temp += "<tr><td class = 'partsSpacer'colspan='2'>";
                temp += "</td></tr>";
                temp += "<tr><td colspan='2'>";
                temp += OpenPod(string.Empty, string.Empty);

                string strLastNHA = dtThisNHA.Rows[0]["NHA"].ToString();
                strATAChapter = dtThisNHA.Rows[0]["ATAChapter"].ToString();


                    foreach (DataRow mynhaRow in dtThisNHA.Rows)
                    {
                        strNHAPartNumber = mynhaRow["JPEPN"].ToString();
                        strNHAAssembly = mynhaRow["NHA"].ToString();
                       
                        intNHAQtyPerAssembly = mynhaRow["PartsPerNHA"].ToString();

                        nhaRowMatch = GenUtils.FindRowforStrippedPartNumber(GenUtils.MakeUCaseNumChar(strNHAPartNumber), dtCacheMaster);

                        strNHADescription = dtCacheMaster.Rows[nhaRowMatch]["pmast_Description"].ToString();
                        intQtyOrdered = (int)dtCacheMaster.Rows[nhaRowMatch]["cur_QTY_ORDERED"];

                        rawQtyAvailable = (decimal)dtCacheMaster.Rows[nhaRowMatch]["calc_QTY_AVAILABLE"];

                        decNHACost = (decimal)dtCacheMaster.Rows[nhaRowMatch]["calc_SELL_PRICE"];

                        intStockCategoryKey = -1;
                        if (dtCacheMaster.Rows[nhaRowMatch]["pmast_P_STC_AUTO_KEY"].ToString() != string.Empty)
                        {
                            intStockCategoryKey = int.Parse(dtCacheMaster.Rows[nhaRowMatch]["pmast_P_STC_AUTO_KEY"].ToString());
                        }

                        thisPartisSellable = false;

                        if (decNHACost > 0 && (intStockCategoryKey == 2 || intStockCategoryKey == 4 || intStockCategoryKey == 6 || intStockCategoryKey == 12 || intStockCategoryKey == 20))
                        {
                            thisPartisSellable = true;
                        }


                        if (PartNumberOnScreen(strNHAPartNumber, "", decNHACost.ToString(), intQtyOrdered.ToString(), "NHAMatch", strPlacedPartNumbers) == -1 && thisPartisSellable && decNHACost > 0)
                        {                                    
                            if (!blnNHATableOpened)
                        {
                            temp += addLabelBar(" <a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|" + strLastNHA + "|" + MySessionId + "|" + MyAccountType + "', event, '');" + (char)34 + "  onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Associated Parts - NHA: " + strLastNHA + "</a>");
                            temp += openNHATable();
                            blnNHATableOpened = true;
                        }
                         else if (strNHAAssembly != strLastNHA)
                        {
                            temp += closeNHATable();
                            temp += addLabelBar("<a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|" + strNHAAssembly + "|" + MySessionId + "|" + MyAccountType + "', event, '');" + (char)34 + " onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className='SBpodTitleBarMOMut';" + (char)34 + ">Associated Parts - NHA: " + strNHAAssembly + "</a>");
                            temp += openNHATable();
                        }
                        strLastNHA = strNHAAssembly;




                        temp += BuildAssociatedLineItem(strNHAPartNumber, strNHADescription, decNHACost, intQtyOrdered, strNHAPartNumber, "", MySessionId, MyAccountType, rawQtyAvailable);
                      
                        //Debug.WriteLine("NHA" + strNHAPartNumber);

                          
                        }

                    }
                    if (blnNHATableOpened)
                    {
                        temp += closeNHATable();
                        temp += closePod();
                        temp += "</td></tr>";
                        //temp += "</table>";
                    }
            }
                //temp += closePod();
                //temp += "</td></tr>"; 
            NHACount = dtThisNHA.Rows.Count;
        }
        temp += "</table>";


        temp += "<tr><td class = 'partsSpacer'colspan='2'>";
        temp += "</td></tr>";


        temp += BuildPanelFooter(NHACount);




        return temp;


    }


    protected string BuildAssociatedLineItem(string strPartNumber, string strDescription, decimal decCost, int intQtyOrdered, string strLinkID, string strDebug, string MySessionId, string MyAccountType, decimal rawQtyAvailable)
    {
        string tmp = string.Empty;
        string strQtyOrdered = string.Empty;

        tmp += "<tr><td class='SBpartNumberNHATD'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + MySessionId + "|" + MyAccountType + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + strPartNumber + "</a></td><td class='SBdescriptionNHATD'>" + strDescription + strDebug + "</td>";

        if (rawQtyAvailable > 0)
        {
            tmp += "<td class='SBavailTD'><img src='images/instock5.gif' alt='' /></td>";
        }
        else
        {
            tmp += "<td class='SBavailTD'><img src='images/callstock.gif' alt='' /></td>";
        }
            


        if (MyAccountType != "guest")
        {
            tmp += "<td class='SBcostTD'>" + String.Format("{0:C}", decCost) + "</td>";
        }
        else
        {
            tmp += "<td class='SBcostTD'></td>";
        }
        

        tmp += "<td class='SBqtyNHATD'><input type='text' name='txtQTY_DetailPanel_" + strLinkID + "' class='SBtxtQtyCntrl' id='txtQTY_DetailPanel_" + strLinkID + "' maxlength='5' onkeypress='return checkIt(event)'  Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQtyPopup', 'txtQTY_DetailPanel_" + strLinkID + "');" + (char)34 + " ";


        if (intQtyOrdered > 0)
        {
            strQtyOrdered = intQtyOrdered.ToString();
        }

        tmp += "  value='" + strQtyOrdered + "'  onFocus ='javascript:this.select();' >";

        tmp += "<input type='hidden'ID='hidden_txtQTY_DetailPanel_" + strLinkID + "' name='hidden_txtQTY_DetailPanel_" + strLinkID + "' value = '" + strQtyOrdered + "'></td>";





        tmp += "<td class='SBmoreInfoNHATD'><a name='' onclick=" + (char)34 + "javascript:ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + MySessionId + "|" + MyAccountType + "', event, '')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">Click for Details</a></td></tr>";


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



    protected string addLabelBar(string strLabel)
    {
        string tmp = string.Empty;
       

        tmp += "<tr><td class = 'SBpodTitleBar'><table><tr><td class='SBpodTitleBar' style='width:80%; white-space:nowrap;'>" + strLabel + "</td><td style='margin-right:5px;text-align:right;width:100%;white-space:nowrap;font:verdana;color:#666666;font-size:9px;'><!-- <a name='' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQtyPopup');" + (char)34 + "  onmouseover=" + (char)34 + "showTooltipOnOff(event,'Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.')" + (char)34 + "><img src='images/save_button.gif' height='19' width='37' border='0' alt=''></a> --></td></tr></table></td></tr>";


        return tmp;
    }


    protected string OpenPod(string strTitle, string strMatch)
    {
        string tmp = string.Empty;
        string lclTitle = strTitle;

        tmp = "<div class='box'><b class='tc'><b class='L1'></b><b class='L2'></b><b class='L3'></b></b><div class='content'>";
        tmp += "<table>";
        if (strTitle != string.Empty)
        {
            if (strTitle == "multi")
            {
                lclTitle = "Matches containing: ";

            }
            else if (strTitle == "nomatch")
            {
                lclTitle = "No Matches Found";
            }
           

            tmp += "<tr><td class='SBpodTitleBar'><table><tr><td class='SBpodTitleBar' style='width:80%; white-space:nowrap;'>" + lclTitle + "<span class='SBpodSearchWord'>" + strMatch + "</span></td><td style='margin-right:5px;text-align:right;width:100%;white-space:nowrap;''><!-- <a name='' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsOnQtyPopup');" + (char)34 + "  onmouseover=" + (char)34 + "balloonSPB.showTooltip(event,'Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.')" + (char)34 + "><img src='images/save_button.gif' height='19' width='37' border='0' alt=''></a> --></td></tr></table></td></tr>";

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

    protected string BuildPanelHeader(string strThisTitle)
    {
        string temp = string.Empty;

        temp += "<table class='SBecomTable'>";
        temp += "<tr><td colspan='3' class='SBHeaderBarTD'>";
       

        temp += "</td></tr>";
        temp += "<tr><td class='SBViewPortBorderTD'></td>";
        temp += "<td class='SBViewPortContentTD'>";
        temp += "<table class='ViewPortTable'>";
        temp += "<tr><td colspan = '2' style='width:100%;'>";

        return temp;
 
        	

    }

    protected string BuildPanelFooter(int rowCount)
    {

        string temp = string.Empty;
        
        temp = "<tr>";
        temp += "<td class='partsJPElogo'><img src='images/logo-JPE_000.gif' alt='Jet Parts Engineering - Your PMA Solution' border='0' /></td>";
        temp += "<td colspan='1' class='partsBackTD'>";
        
    if (rowCount > 0 )
    {
        temp += "<input type='button' ID='btnPartsPanelSave2' onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsClosePopup');" + (char)34 + "  onmouseover=" + (char)34 + "balloonSPB.showTooltip(event,'Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.')" + (char)34 + "  value='Save & Close' />";
    }
        
        temp += "&nbsp;<input type='button' ID='btnPartsPanelClose2'   OnClick=" + (char)34 + "javascript: return closePartsPopup('cancel', 'ModalPopupExtenderPartsDetail' );" + (char)34 + "  value='Close Window' />";




        temp += "<br />";
        temp += "<span style='font-size:10px; color:#999999; text-aign:right;padding-right:5px;'>Save changes before you navigate away from window.</span>";
        temp += "</td></tr>";
        temp += "</table>";


        temp += "</td><td class='SBViewPortBorderTD'></td></tr>";
        temp += "<tr><td class='SBViewPortFooterTD' colspan = '3'>&nbsp;</td></tr>";
        temp += "</table>";

        return temp;
    }



    public string BuildNHAPanel(string strNHA, string MySessionId, string MyAccountType)
    {
        string temp = string.Empty;
        DataTable dtNHA = null;
        DataTable dtNHADetails = null;

        strNHA = strNHA.ToUpper();

        //------------- Lookup the Part

        string strImage = string.Empty;
        string strAltPartNumber = string.Empty;
        string strPrice = string.Empty;
        string strQtyControl = string.Empty;
        string strAvailable = string.Empty;
        string strBO = string.Empty;
        string strEXT = string.Empty;
        string strCurrentNHA = string.Empty;
        int thisCacheRow = - 1;
        string strPartNumber = string.Empty;
        string strWholePartNumber = string.Empty;
        DataTable dtCacheMaster;
        decimal rawQtyAvailable;
        decimal thisPrice;

       


        string[,] strPlacedPartNumbers = new string[1000, 10];



        int thisQtyOrdered;
        decimal decAvl;
        //decimal thisPrice;
        int intBO;
       


        //int intPartKey = -1;
  
        dtCacheMaster = GenUtils.GetCacheforSessionID(MySessionId);


        dtNHA = GenUtils.FindNHAItems("NHA", strNHA);
        temp = BuildPanelHeader("NHA Parts List");

        strCurrentNHA = "NHA|" + strNHA + "|" + MySessionId;


        temp += "<!-- Begin Table Viewport Area -->";
        temp += "<table>";
        temp += "<tr><td class='partsImageTD'  style='font-size:25px; color:#cccccc; font-weight:600;white-space:nowrap;' >";
        temp += "Assembly Parts List for:</td>";
        temp += "<td class='partsPartNumberTD'>" + strNHA + "</td></tr>";
     

        temp += "<tr><td colspan='2' class='partsPartNumberTD'>";


        if (dtNHA.Rows.Count > 0)
        {

            temp += "<input type='button' ID='btnPartsPanelSave1'    onclick=" + (char)34 + "javascript: return showThrobber('btnShowThrobber', 'btnUpdatePartsClosePopup');" + (char)34 + "  onmouseover=" + (char)34 + "balloonSPB.showTooltip(event,'Click on any SAVE button to update your quantity field changes.  To remove an item from your shopping cart set the Qty to zero.')" + (char)34 + "  value='Save & Close' />";
        }
        
        temp += "&nbsp;<input type='button' ID='btnPartsPanelClose1'   OnClick=" + (char)34 + "javascript: return closePartsPopup('cancel', 'ModalPopupExtenderPartsDetail' );" + (char)34 + "  value='Close Window' /><br><span style='font-size:10px; color:#999999; text-aign:right;padding-right:5px;'>Save changes before you navigate away from window.</span></td></tr>";



        temp += "</table>";

        temp += "</td></tr>";

        temp += "<tr><td colspan='2'>";
        temp += "<table class='partsContainerTable'>";
        temp += "<tr><td class = 'partsSpacer'colspan='2'>";
        temp += "</td></tr>";



        if (dtNHA.Rows.Count > 0)
        {

            string strWhere = " WHERE (";
            bool blnFirstPlaced = false;


            foreach (DataRow mynhaRow in dtNHA.Rows)
            {
                if (blnFirstPlaced)
                {
                    strWhere += "OR ";
                }
                else
                {
                    blnFirstPlaced = true;
                }

                strWhere += " P.PN_STRIPPED = '" + GenUtils.MakeUCaseNumChar(mynhaRow["JPEPN"].ToString()) + "' ";

            }
            strWhere += " )";


            //------ Retrieve the NHA Data
            dtNHADetails = GenUtils.FindParts(strWhere, string.Empty);





            temp += "<tr><td colspan = '2'>";

            //bool blnTitlePlaced = false;



            temp += "<div class='box'><b class='tc'><b class='L1'></b><b Sclass='L2'></b><b class='L3'></b></b><div class='content'>";

            foreach (DataRow mynhaRow in dtNHADetails.Rows)
            {


                strPartNumber = GenUtils.MakeUCaseNumChar(mynhaRow["PN"].ToString());
                strWholePartNumber = mynhaRow["PN"].ToString();
                thisCacheRow = GenUtils.FindRowforStrippedPartNumber(strPartNumber, dtCacheMaster);
                //------ load the image
                strImage = GenUtils.findImageLink(thisCacheRow, dtCacheMaster);

                strAltPartNumber = dtCacheMaster.Rows[thisCacheRow]["pmast_ALT_PN"].ToString();

                //-------- Set the price
                //strPrice = String.Format("{0:C}", decimal.Parse(dtCacheMaster.Rows[thisCacheRow]["calc_SELL_PRICE"].ToString()));


                thisPrice = decimal.Parse(dtCacheMaster.Rows[thisCacheRow]["calc_SELL_PRICE"].ToString());




                strEXT = String.Format("{0:C}", 0);

                //---- Quantity COntrol
                strQtyControl = "<input type='text' name='txtQTY_DetailPanel_" + strPartNumber + "' class='SBtxtQtyCntrl' id='txtQTY_DetailPanel_" + strPartNumber + "' maxlength='5' onkeypress='return checkIt(event)'   Onblur=" + (char)34 + "javascript: return qtyOnBlurIf('btnShowThrobber', 'btnUpdatePartsOnQtyPopup', 'txtQTY_DetailPanel_" + strPartNumber + "');" + (char)34 + "  value='" + dtCacheMaster.Rows[thisCacheRow]["cur_QTY_ORDERED"].ToString() + "' onFocus ='javascript:this.select();'>";


                strQtyControl += "<input type='hidden'ID='hidden_txtQTY_DetailPanel_" + strPartNumber + "' name='hidden_txtQTY_DetailPanel_" + strPartNumber + "' value = '" + dtCacheMaster.Rows[thisCacheRow]["cur_QTY_ORDERED"].ToString() + "'>";

                //------Avaialble Back Order
                strBO = "";
                decAvl = (decimal)dtCacheMaster.Rows[thisCacheRow]["calc_QTY_AVAILABLE"];

                rawQtyAvailable = (decimal)dtCacheMaster.Rows[thisCacheRow]["calc_QTY_AVAILABLE"];



                thisQtyOrdered = (int)dtCacheMaster.Rows[thisCacheRow]["cur_QTY_ORDERED"];

                intBO = 0;


                if (decAvl >= thisQtyOrdered)
                {
                    decAvl = Convert.ToDecimal(thisQtyOrdered);
                }
                else if (thisQtyOrdered > decAvl)
                {
                    decAvl = (decimal)dtCacheMaster.Rows[thisCacheRow]["calc_QTY_AVAILABLE"];
                    intBO = thisQtyOrdered - Convert.ToInt32(decAvl);
                }
                else
                {
                    decAvl = 0;
                    intBO = 0;
                }


                strAvailable = "0";
                if (dtCacheMaster.Rows[thisCacheRow]["calc_QTY_AVAILABLE"].ToString() != string.Empty)
                {
                    strAvailable = string.Format("{0:#####}", decAvl.ToString());
                }

                strBO = string.Format("{0:#####}", intBO);


                strPrice = String.Format("{0:C}", thisPrice);
                strEXT = String.Format("{0:C}", (thisPrice * thisQtyOrdered));

                if (PartNumberOnScreen(strPartNumber, "", "", thisQtyOrdered.ToString(), "SingleMatch", strPlacedPartNumbers) == -1  && decimal.Parse(dtCacheMaster.Rows[thisCacheRow]["calc_SELL_PRICE"].ToString()) > 0 )
                {

                    temp += "<table class='SBNHAMatchTable' style='width:535px;margin-left:3px;	margin-right:3px;margin-top:5px;'>";


                    temp += "<tr>";


                    temp += "<!-- top row table -->";
                    temp += "<td class='partTopLine1'>";
                    temp += "<table>";
                    temp += "<tr>";
                    temp += "<td class='SBpartNumberTD'><a name='' onclick=" + (char)34 + "javascript:ModalSwitchEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + MySessionId + "|" + MyAccountType + "', event, '" + strCurrentNHA + "')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + ">" + strWholePartNumber + "</a></td>";


                    temp += "<td class='SBreplacesTD'><span class='SBreplacesLabel'>Replaces: </span>" + dtCacheMaster.Rows[thisCacheRow]["pmast_ALT_PN"] + "</td></tr>";
                    temp += "<tr><td class='SBdescriptionTD' colspan='2'>" + mynhaRow["DESCRIPTION"] + "</td></tr>";

                    temp += "</table>";
                    temp += "</td>";
                    temp += "<td class='SBmoreInfoTD' rowspan='2'>";




                    temp += "<a name='' onclick=" + (char)34 + "javascript:ModalSwitchEvt('ModalPopupExtenderPartsDetail', 'PN|" + strPartNumber + "|" + MySessionId + "|" + MyAccountType + "', event, '" + strCurrentNHA + "')" + (char)34 + "    onmouseover=" + (char)34 + "this.className='SBpodTitleBarOMOver';" + (char)34 + "  onmouseout=" + (char)34 + "this.className = 'SBpodTitleBarOMOut';" + (char)34 + "><img src='" + strImage + "' alt='Click to view detailed information about this Part' width='50'  onmouseover=" + (char)34 + "this.className='SBshowBorder';" + (char)34 + " onmouseout=" + (char)34 + "this.className='SBnoBorderNHA';" + (char)34 + " class='SBnoBorderNHA' />";

                    temp += "<br />Click for Details</a></td></tr>";
                    temp += "<!-- end of top row table -->";
                    temp += "<td>";
                    temp += "<table>";
                    temp += "<tr>";
                    if (MyAccountType != "guest")
                    {
                        temp += "<td class='SBcostLabelTD'>Price</td>";
                    }
                    else
                    {
                        temp += "<td class='SBcostLabelTD'></td>";
                    }
                    
                    temp += "<td class='SBqtyLabelTD'>Qty</td>";

                    if (MyAccountType != "guest")
                    {
                        temp += "<td class='SBextendLabelTD'>EXT</td>";
                    }
                    else
                    {
                        temp += "<td class='SBextendLabelTD'></td>";
                    }
                    temp += "<td class='SBboLabelTD'>Avail</td>";
                    temp += "<td class='SBavailLabelTD'>B/O</td>";
                    temp += "</tr>";
                    temp += "<tr>";

                    if (MyAccountType != "guest")
                    {
                        temp += "<td class='SBcostTD'>" + strPrice + "</td>";
                    }
                    else
                    {
                        temp += "<td class='SBcostTD'></td>";
                    }
                    temp += "<td class='SBqtyTD'>" + strQtyControl + "</td>";

                    if (MyAccountType != "guest")
                    {
                        if ((thisPrice * thisQtyOrdered) > 0)
                        {
                            temp += "<td class='SBextendTD'>" + strEXT + "</td>";
                        }
                        else
                        {
                            temp += "<td class='SBextendTD'>&nbsp;</td>";
                        }
                    }
                    else
                    {
                        temp += "<td class='SBextendTD'></td>";
                    }


                    if (thisQtyOrdered > 0)
                    {
                        temp += "<td class='SBboTD'>" + strAvailable + "</td>";
                    }
                    else
                    {
                        if (rawQtyAvailable > 0)
                        {
                            temp += "<td class='SBavailTD'><img src='images/instock5.gif' alt='' /></td>";
                        }
                        else
                        {
                            temp += "<td class='SBavailTD'><img src='images/callstock.gif' alt='' /></td>";
                        }
            

                       
                    }

                    
                   


                    temp += "<td class='SBavailTD'>" + strBO + "</td>";

                    temp += "</tr>";
                    temp += "</table>";
                    temp += "</td></tr></table>";

                    temp += "<div class='partsNAHSpacer'></div>";
                }

            }


            temp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";


            temp += "</td></tr>";


          


        }
        else //------- No NHA information found
        {
            temp += "<tr><td colspan = '2'>";

            temp += "<div class='box'><b class='tc'><b class='L1'></b><b Sclass='L2'></b><b class='L3'></b></b><div class='content'>";

            temp += "Sorry, we were unable to locate any NHA information for the assembly " + strNHA;

            temp += "</div><b class='bc'><b class='L3'></b><b class='L2'></b><b class='L1'></b></b></div>";


            temp += "</td></tr>";

        }

        temp += "</td></tr>";
    
        temp += "<table class='partsContainerTable'>";

        temp += "<tr><td class = 'partsSpacer'colspan='2'>";
        temp += "</td></tr>";

        temp += BuildPanelFooter(dtNHA.Rows.Count);


        return temp;

        }


    int intPlacedPartNumberCounter;

    protected int PartNumberOnScreen(string thisPartNumber, string thisPartNumberStripped, string thisPrice, string thisQty, string thisPanelType, string[,] strPlacedPartNumbers)
    {


        int intAlreadyPlacedElement = -1;
        int lp;

 
        for (lp = 0; lp <= intPlacedPartNumberCounter; lp++)
        {
            if (thisPartNumber == strPlacedPartNumbers[lp, 0] || ((thisPartNumberStripped == strPlacedPartNumbers[lp, 1] && thisPartNumberStripped != string.Empty)))
            {
                intAlreadyPlacedElement = lp;
            }
        }


        if (intAlreadyPlacedElement == -1)
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

    }





