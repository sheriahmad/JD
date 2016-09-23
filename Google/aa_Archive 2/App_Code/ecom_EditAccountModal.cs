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



//http://173.10.78.33/images/curschedad.png 


/// <summary>
/// Summary description for ecom_EditAccountModal
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ecom_EditAccountModal : System.Web.Services.WebService
{

    public ecom_EditAccountModal()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }





    [WebMethod]
    public string BuildAccountPopup(string strContext)
    {
        string[] temp = strContext.Split('|');
        string strPanel = string.Empty;

        strPanel = BuildAccountPanel(temp[0], temp[1], temp[2], temp[3]);

        // none/ admin/user  |  account key  |  ADMINGRIDVIEW / MYACCOUNT / NEWACCOUNT
        return strPanel;

    }

    [WebMethod]
    public string DeconAccountPanel(string strPartNumber)
    {
        string temp = string.Empty;

        temp += "<div style='margin:0px auto;text-align:center;vertical-align:middle;' >";
        temp += "<image src ='images\\progressbar.gif'  >";
        temp += "</div>";

        return temp;
    }




    public string BuildAccountPanel(string UserType, string AccountKey, string CalledFrom, string strCurUserType)
    {
        string temp = string.Empty;


        DataTable dtAccounts;
        DataTable dtCompany;
        DataTable dtSites;
        DataTable dtShipVia;
        DataTable dtRolodex;


        int thisCompanyKey;




        string firstName = string.Empty;
        string lastName = string.Empty;
        string userName = string.Empty;
        string Company = string.Empty;
        string jobTitle = string.Empty;
        string Phone = string.Empty;
        string Fax = string.Empty;
        string eMail = string.Empty;

        string MyAddress1 = string.Empty;
        string MyAddress2 = string.Empty;
        string MyAddress3 = string.Empty;
        string MyCity = string.Empty;
        string MyState = string.Empty;
        string MyCountry = string.Empty;
        string MyZip = string.Empty;


        string Address1 = string.Empty;
        string Address2 = string.Empty;
        string Address3 = string.Empty;
        string City = string.Empty;
        string State = string.Empty;
        string Country = string.Empty;
        string Zip = string.Empty;
        string JPENotes = string.Empty;
        string RolodexKey = "0";
        string CompanyKey = "0";
        string SalesPersonKey = "0";
        string ddlUserType = string.Empty;
      
        string shipAddressOL = string.Empty;
        string shipperIDUL = string.Empty;

        string AccountCreated = string.Empty;
        string emailValidated = string.Empty;
        string AccountType = string.Empty;
        string LastUserUpdate = string.Empty;
        string LastAdminUpdate = string.Empty;
        string custLastComment = string.Empty;
        string PricingView = string.Empty;
        string UserPassword = string.Empty;

        string approved_checked = string.Empty;
        string active_checked = string.Empty;

        string hdn_approved_checked = string.Empty;
        string hdn_active_checked = string.Empty;

        string QuantumCode = string.Empty;
        bool blnnewAccout = false;
        bool blnReturningUser = false;
        int thisAccountKey = -1;


       

        if (CalledFrom == "NEW")
        {
            blnnewAccout = true;
        }
        else if (CalledFrom == "BACK")
        {
            blnnewAccout = true;
            blnReturningUser = true;
        }
        else
        {
            thisAccountKey = int.Parse(AccountKey);

            dtAccounts = GenUtils.ecom_AccountLookup("", thisAccountKey);

            if (dtAccounts.Rows.Count > 0)
            {
                DataRow r = dtAccounts.Rows[0];
                // Load a company record to the dt for pricing

                userName = r["UserName"].ToString();
                firstName = r["FirstName"].ToString();
                lastName = r["LastName"].ToString();
                Company = r["Company"].ToString();  
                jobTitle = r["JobTitle"].ToString();
                Phone = r["Phone"].ToString();
                Fax = r["FAX"].ToString();
                eMail = r["EmailAddress"].ToString();

              


                AccountCreated = r["AccountCreated"].ToString();
                emailValidated = r["emailValidated"].ToString();
                AccountType = r["AccountType"].ToString().ToLower();
                LastUserUpdate = r["LastUserUpdate"].ToString();
                LastAdminUpdate = r["LastAdminUpdate"].ToString();
                custLastComment = r["custLastComment"].ToString();
                PricingView = r["PricingView"].ToString();

                if (r["AccountType"].ToString().ToLower() == "user") 
                {
                    UserPassword = r["Password"].ToString();
                }

                //if (r["JPEApproved"].ToString() == "True")
                //{
                //    approved_checked = "CHECKED";
                //    hdn_approved_checked = "on";
                //}
                //if (r["Active"].ToString() == "True")
                //{
                //    active_checked = "CHECKED";
                //    hdn_active_checked = "on";
                //}

                QuantumCode = r["QuantumCompanyCode"].ToString();
                JPENotes = r["JPENotes"].ToString();

                RolodexKey = r["RolodexKey"].ToString();
                CompanyKey = r["QuantumCompanyCode"].ToString();
                SalesPersonKey = r["MySalesPersonKey"].ToString();



                if (RolodexKey != string.Empty && int.Parse(RolodexKey) > 0)
                {
                    dtRolodex = GenUtils.RoloDexLookup(RolodexKey);
                    if (dtRolodex != null)
                    {
                        if (dtRolodex.Rows[0]["COMPANY_CODE_UPPER"].ToString() != string.Empty)
                        {

                            MyAddress1 = dtRolodex.Rows[0]["ADDRESS1"].ToString();
                            MyAddress2 = dtRolodex.Rows[0]["ADDRESS2"].ToString();
                            MyAddress3 = dtRolodex.Rows[0]["ADDRESS3"].ToString();
                            MyCity = dtRolodex.Rows[0]["CITY"].ToString();
                            MyState = dtRolodex.Rows[0]["STATE"].ToString();
                            MyCountry = dtRolodex.Rows[0]["COUNTRY"].ToString();
                            MyZip = dtRolodex.Rows[0]["ZIP_CODE"].ToString();

                            //userName = r["UserName"].ToString();
                            firstName = dtRolodex.Rows[0]["RDX_CONTACT_NAME"].ToString();
                            lastName = string.Empty;
                            Company = dtRolodex.Rows[0]["COMPANY_NAME"].ToString();
                            jobTitle = dtRolodex.Rows[0]["TITLE"].ToString();
                            Phone = dtRolodex.Rows[0]["PHONE_NUMBER"].ToString();
                            Fax = dtRolodex.Rows[0]["FAX_NUMBER"].ToString();
                            eMail = dtRolodex.Rows[0]["EMAIL_ADDRESS"].ToString();


                            dtCompany = GenUtils.CompanyLookup(dtRolodex.Rows[0]["CMP_AUTO_KEY"].ToString(), "", -10);

                            dtSites = GenUtils.CompanySitesLookup(dtRolodex.Rows[0]["CMP_AUTO_KEY"].ToString(), -10);
                            ///SetMyShipToAddressDDL(dtCompany, dtSites);

                            thisCompanyKey = int.Parse(dtCompany.Rows[0]["CMP_AUTO_KEY"].ToString());
                            dtShipVia = GenUtils.CompanyShippingLookup(thisCompanyKey);



                            Address1 = dtCompany.Rows[0]["ADDRESS1"].ToString();
                            Address2 = dtCompany.Rows[0]["ADDRESS2"].ToString();
                            Address3 = dtCompany.Rows[0]["ADDRESS3"].ToString();
                            City = dtCompany.Rows[0]["CITY"].ToString();
                            State = dtCompany.Rows[0]["STATE"].ToString();
                            Country = dtCompany.Rows[0]["COUNTRY"].ToString();
                            Zip = dtCompany.Rows[0]["ZIP_CODE"].ToString();




                            //------------------- Build the shipping address lines



                            string bldAddress = string.Empty;

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

                            if (bldAddress != string.Empty)
                            {
                                shipAddressOL = "<ol><li>" + bldAddress + "</li>";
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
                                if (mySiteRow["CITY"].ToString() != string.Empty)
                                {
                                    bldAddress += ", " + mySiteRow["CITY"].ToString();
                                }
                                if (mySiteRow["STATE"].ToString() != string.Empty)
                                {
                                    bldAddress += ", " + mySiteRow["STATE"].ToString();
                                }
                                if (mySiteRow["COUNTRY"].ToString() != string.Empty)
                                {
                                    bldAddress += ", " + mySiteRow["COUNTRY"].ToString();

                                }
                                if (mySiteRow["ZIP_CODE"].ToString() != string.Empty)
                                {
                                    bldAddress += ", " + mySiteRow["ZIP_CODE"].ToString();

                                }

                                if (bldAddress != string.Empty)
                                {
                                    shipAddressOL += "<li>" + bldAddress + "</li>";
                                }


                            }

                            if (shipAddressOL != String.Empty)
                            {
                                shipAddressOL += "</ol>";
                            }


                            //------------- set the shipper numbers
                            if (dtShipVia.Rows.Count > 0)
                            {
                                shipperIDUL = "<OL>";
                                foreach (DataRow myShipperRow in dtShipVia.Rows)
                                {
                                    shipperIDUL += "<LI>" + myShipperRow["DESCRIPTION"] + " " + myShipperRow["ACCOUNT_NUMBER"] + "</LI>";

                                }
                                shipperIDUL += "</OL>";
                            }

                        }
                    }
                }
            }


            else
            {
                return "ERROR ON ACCOUNT LOOKUP";
            }
        }
        string thisSelected = string.Empty;

        ddlUserType = "<select id='txt_AccountType' name = 'txt_AccountType'>";
        thisSelected = (AccountType == "new" || AccountType == string.Empty) ? "selected" : string.Empty;
        ddlUserType += "<option value='new' " + thisSelected + ">New Account Request</option>";


        thisSelected = (AccountType == "guest") ? "selected" : string.Empty;
        ddlUserType += "<option value='guest' " + thisSelected + ">Guest - No pricing</option>";

        thisSelected = (AccountType == "user") ? "selected" : string.Empty;
        ddlUserType += "<option value='user' " + thisSelected + ">User</option>";

        thisSelected = (AccountType == "jpe") ? "selected" : string.Empty;
        ddlUserType += "<option value='user' " + thisSelected + ">JPE Salesperson</option>";

        thisSelected = (AccountType == "closed") ? "selected" : string.Empty;
        ddlUserType += "<option value='closed' " + thisSelected + ">Closed Account</option>";

        thisSelected = (AccountType == "admin") ? "selected" : string.Empty;
        ddlUserType += "<option value='admin' " + thisSelected + ">JPESmartBuyer Admin</option>";
        ddlUserType += "</Select>";



        temp = "<!-- Begin Table Viewport Area -->";

        temp += "<table class='nwAccountTable' cellpadding='15'>";

        if (blnReturningUser)
        {

            temp += "<tr><td class='nwAccountLeftTD'>Welcome Back</td>";
            temp += "<td class='nwAccountRightTD'>";



            temp += "<div class='nwAcctInstructionDiv'><p><b>JPE Smart Buyer</b> is a new leading edge business-to-business eCommerce website that is directly connected to our in-house inventory management and order tracking program.  Use this form if you were registered on the old Jet Parts Engineering website and we will transfer your account information to <b>JPE Smart Buyer</b>.  Simply fill in the fields below and then click 'Save & Close.' </p><br><p> <b>Please allow 1 to 2 business days</b> for our IT staff to activate your new account.</p><p><b>We will send you an email when it is ready to go!</b></p></div>";
  
        }
        else if (blnnewAccout)
        {

        temp += "<tr><td class='nwAccountLeftTD'>Welcome</td>";
        temp += "<td class='nwAccountRightTD'>";



        temp += "<div class='nwAcctInstructionDiv'><p><b>JPE Smart Buyer</b> is a new leading edge business-to-business eCommerce website that is directly connected to our in-house inventory management and order tracking program.  Use this form if you are a new customer to Jet Parts Engineering, or if you are an existing JPE customer that did not use the old website system.  Simply fill in the fields below and then click 'Save & Close.' </p><br><p> <b>Please allow 1 to 2 business days</b> for our IT staff to activate your new account.  If your company is new to JPE we may require additional information in order to set your account up. </p><p><b>We will send you an email when it is ready to go!</b></p></div>";
  

}

        
        temp += "<tr><td class='nwAccountLeftTD'>eMail Address</td>";
        temp += "<td class='nwAccountRightTD'>";

        temp += "<table>";

        temp += "<tr><td class='nwSubTDLabels'>Email Address</td><td class='nwSubTdContent'><input type='text' ID='txt_eMail' name='txt_eMail' style='width:200px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + eMail + "'/></td><td class='nwSubTDLabels' rowspan='2' style='vertical-align:top;'><div class='nwAcctInstructionDiv'>You will use your email address to log into Smart Buyer.</div></td></tr><td class='nwSubTDLabels'>Verify Email</td><td class='nwSubTdContent'><input type='text' ID='txt_eMailVerify' name='txt_eMailVerify' style='width:200px;' Class = 'ecomTextBoxes'  MaxLength = '50' value=''/></td><tr>";


        //if (thisAccountKey > 0)
        //{
        //    temp += "<tr><td class='nwSubTdContent'>" + userName + "</td></tr>";
        //}
        //else
        //{

        //    temp += "<tr><td class='nwSubTdContent'><input type='text' ID='txt_UserName' name='txt_UserName' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' value='' /> </td><td class='nwSubTDLabels'><div class='nwAcctInstructionDiv'>Your user name must be between 6 and 20 characters in length.  It is not case sensitive</div></td></tr>";
        //}


        temp += "</table>";
        temp += "</td></tr>";





      
        temp += "<tr><td class='nwAccountLeftTD'>Profile</td>";
        temp += "<td class='nwAccountRightTD'>";

        temp += "<table>";




        temp += "<tr><td class='nwSubTDLabels'>Name</td><td class='nwSubTdContent' colspan='3'><input type='text' ID='txt_FirstName' name='txt_FirstName' style='width:406px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + firstName + "' /> </td><!-- td class='nwSubTDLabels'>Last Name</td><td class='nwSubTdContent'><input type='text' ID='txt_LastName' name='txt_LastName' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + lastName + "' /> </td --></tr>";

        temp += "<tr><td class='nwSubTDLabels'>Company</td><td class='nwSubTdContent'><input type='text' ID='txt_Company' name='txt_Company' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + Company + "' /></td><td class='nwSubTDLabels'>Job Title</td><td class='nwSubTdContent'><input type='text' ID='txt_JobTitle' name='txt_JobTitle' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '50'  value = '" + jobTitle + "'/>";
        temp += "<tr><td class='nwSubTDLabels'>Phone</td><td class='nwSubTdContent'><input type='text' ID='txt_Phone' name='txt_Phone' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + Phone + "' /></td>";
         if (!blnReturningUser)
        {
        temp += "<td class='nwSubTDLabels'>FAX</td><td class='nwSubTdContent'><input type='text' ID='txt_Fax' name='txt_Fax' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + Fax + "'/>";
        }

        //temp += "<tr><td class='nwSubTDLabels'>eMail Address</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_eMail' name='txt_eMail' style='width:406px;' Class = 'ecomTextBoxes'  MaxLength = '50' value='" + eMail + "'/>";


        temp += "<input type = 'hidden' id = 'hdn_FirstName'  name = 'hdn_FirstName'  value ='" + firstName + "' />";
        temp += "<input type = 'hidden' id = 'hdn_LastName' name = 'hdn_LastName'  value ='" + lastName + "' />";
        temp += "<input type = 'hidden' id = 'hdn_Company' name = 'hdn_Company' value ='" + Company + "' />";
        temp += "<input type = 'hidden' id = 'hdn_JobTitle' name = 'hdn_JobTitle' value ='" + jobTitle + "' />";
        temp += "<input type = 'hidden' id = 'hdn_Phone' name = 'hdn_Phone' value ='" + Phone + "' />";
        temp += "<input type = 'hidden' id = 'hdn_Fax' name = 'hdn_Fax' value ='" + Fax + "' />";
        temp += "<input type = 'hidden' id = 'hdn_eMail' name = 'hdn_eMail' value ='" + eMail + "' />";

        temp += "<input type = 'hidden' id = 'hdn_AccountKey' name = 'hdn_AccountKey' value ='" + AccountKey + "' />";
        temp += "<input type = 'hidden' id = 'hdn_UserType'  name = 'hdn_UserType' value ='" + UserType + "' />";



        temp += "</td></tr>";

        temp += "</table>";
        temp += "</td></tr>";



        if (!blnReturningUser)
        {

            if (thisAccountKey > 0)
            {

                temp += "<tr><td class='nwAccountLeftTD'>Password Change</td>";
                temp += "<td class='nwAccountRightTD'>";
                temp += "<table>";

                temp += "<tr><td class='nwSubTDLabels'>Old<br /> Password</td><td class='nwSubTdContent'><input type='password' ID='txt_OldPassword' name='txt_OldPassword' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td><td class='nwSubTDLabels'>New<br />Password</td><td class='nwSubTdContent'><input type='password' ID='txt_NewPassword' name='txt_NewPassword' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td><td class='nwSubTDLabels'>Verify</td><td class='nwSubTdContent'><input type='password' ID='txt_Verify' name='txt_Verify' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td></tr>";

            }
            else
            {
                temp += "<tr><td class='nwAccountLeftTD'>Password</td>";
                temp += "<td class='nwAccountRightTD'>";
                temp += "<table>";

                temp += "<tr><td class='nwSubTDLabels'>Password</td><td class='nwSubTdContent'><input type='password' ID='txt_NewPassword' name='txt_NewPassword' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td><td class='nwSubTDLabels' rowspan='2'><div class='nwAcctInstructionDiv'>Your password must be between 6 and 20 characters in length and is case sensitive</div></td></tr><tr><td class='nwSubTDLabels'>Verify</td><td class='nwSubTdContent'><input type='password' ID='txt_Verify' name='txt_Verify' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td></tr>";

            }



            temp += "<tr><td colspan= '6'>";

            temp += "</td></tr>";
            temp += "</table>";
            temp += "</td></tr>";











            if (strCurUserType != "guest")
            {









                temp += "<tr><td class='nwAccountLeftTD'>My Address</td>";
                temp += "<td class='nwAccountRightTD'>";
                temp += "<table>";
                temp += "<tr><td class='nwSubTDLabels'>Address 1</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_MyAddress1' name='txt_MyAddress1' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + MyAddress1 + "'/></td></tr>";
                temp += "<tr><td class='nwSubTDLabels'>Address 2</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_MyAddress2' name='txt_MyAddress2' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + MyAddress2 + "' /></td></tr>";

                temp += "<tr><td class='nwSubTDLabels'>Address 3</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_MyAddress3' name='txt_MyAddress3' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + MyAddress3 + "'  /></td></tr>";

                temp += "<tr><td class='nwSubTDLabels'>City</td><td class='nwSubTdContent'><input type='text' ID='txt_MyCity' name='txt_MyCity' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' value = '" + MyCity + "' /></td>    <td class='nwSubTDLabels'>State / Province</td><td class='nwSubTdContent' ><input type='text' ID='txt_MyState' name='txt_MyState' style='width:50px;' Class = 'ecomTextBoxes'  MaxLength = '10' value = '" + MyState + "' /></td>";

                temp += "<tr><td class='nwSubTDLabels'>Country</td><td class='nwSubTdContent'><input type='text' ID='txt_MyCountry' name='txt_MyCountry' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' value = '" + MyCountry + "' /></td>";

                temp += "<td class='nwSubTDLabels'>Postal Code</td><td class='nwSubTdContent' ><input type='text' ID='txt_MyZip' name='txt_MyZip' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '10' value='" + MyZip + "' />";

                temp += "<input type = 'hidden' id = 'hdn_Address1'  name = 'hdn_MyAddress1' value ='" + MyAddress1 + "' />";
                temp += "<input type = 'hidden' id = 'hdn_Address2'  name = 'hdn_MyAddress2' value ='" + MyAddress2 + "' />";
                temp += "<input type = 'hidden' id = 'hdn_Address3' name = 'hdn_MyAddress3' value ='" + MyAddress3 + "' />";
                temp += "<input type = 'hidden' id = 'hdn_City' name = 'hdn_MyCity' value ='" + MyCity + "' />";
                temp += "<input type = 'hidden' id = 'hdn_State'  name = 'hdn_MyState' value ='" + MyState + "' />";
                temp += "<input type = 'hidden' id = 'hdn_Country' name = 'hdn_MyCountry' value ='" + MyCountry + "' />";
                temp += "<input type = 'hidden' id = 'hdn_Zip' name = 'hdn_MyZip' value ='" + MyZip + "' />";

                temp += "</td>";
                temp += "</tr>";

                temp += "</table>";
                temp += "</td></tr>";
































                if (!blnnewAccout)
                {


                    temp += "<tr><td class='nwAccountLeftTD'>Billing Address</td>";
                    temp += "<td class='nwAccountRightTD'>";
                    temp += "<table>";
                    temp += "<tr><td class='nwSubTDLabels'>Address 1</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_Address1' name='txt_Address1' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + Address1 + "'/></td></tr>";
                    temp += "<tr><td class='nwSubTDLabels'>Address 2</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_Address2' name='txt_Address2' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + Address2 + "' /></td></tr>";

                    temp += "<tr><td class='nwSubTDLabels'>Address 3</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_Address3' name='txt_Address3' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + Address3 + "'  /></td></tr>";

                    temp += "<tr><td class='nwSubTDLabels'>City</td><td class='nwSubTdContent'><input type='text' ID='txt_City' name='txt_City' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' value = '" + City + "' /></td>    <td class='nwSubTDLabels'>State</td><td class='nwSubTdContent' ><input type='text' ID='txt_State' name='txt_State' style='width:50px;' Class = 'ecomTextBoxes'  MaxLength = '10' value = '" + State + "' /></td>";

                    temp += "<tr><td class='nwSubTDLabels'>Country</td><td class='nwSubTdContent'><input type='text' ID='txt_Country' name='txt_Country' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' value = '" + Country + "' /></td>";

                    temp += "<td class='nwSubTDLabels'>Postal Code</td><td class='nwSubTdContent' ><input type='text' ID='txt_Zip' name='txt_Zip' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '10' value='" + Zip + "' />";

                    temp += "<input type = 'hidden' id = 'hdn_Address1'  name = 'hdn_Address1' value ='" + Address1 + "' />";
                    temp += "<input type = 'hidden' id = 'hdn_Address2'  name = 'hdn_Address2' value ='" + Address2 + "' />";
                    temp += "<input type = 'hidden' id = 'hdn_Address3' name = 'hdn_Address3' value ='" + Address3 + "' />";
                    temp += "<input type = 'hidden' id = 'hdn_City' name = 'hdn_City' value ='" + City + "' />";
                    temp += "<input type = 'hidden' id = 'hdn_State'  name = 'hdn_State' value ='" + State + "' />";
                    temp += "<input type = 'hidden' id = 'hdn_Country' name = 'hdn_Country' value ='" + Country + "' />";
                    temp += "<input type = 'hidden' id = 'hdn_Zip' name = 'hdn_Zip' value ='" + Zip + "' />";

                    temp += "</td>";
                    temp += "</tr>";

                    temp += "</table>";
                    temp += "</td></tr>";

                    temp += "<tr><td class='nwAccountLeftTD'>Shipping Addresses</td>";
                    temp += "<td class='nwAccountRightTD'>";
                    temp += "<div class='nwAcctInstructionDiv'>Current Shipping address[s]:</div>";
                    temp += shipAddressOL;

                    temp += "<div class='nwAcctInstructionDiv'>Add a ship To Address:</div>";
                    temp += "<table>";

                    temp += "<tr><td class='nwSubTDLabels'>Address 1</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_shipAddress1' name='txt_shipAddress1' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' /></td></tr>";

                    temp += "<tr><td class='nwSubTDLabels'>Address 2</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_shipAddress2' name='txt_shipAddress2' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' /></td></tr>";

                    temp += "<tr><td class='nwSubTDLabels'>Address 3</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_shipAddress3' name='txt_shipAddress3' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' /></td></tr>";

                    temp += "<tr><td class='nwSubTDLabels'>City</td><td class='nwSubTdContent'><input type='text' ID='txt_shipCity' name='txt_shipCity' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td>";

                    temp += "<td class='nwSubTDLabels'>State</td><td class='nwSubTdContent' ><input type='text' ID='txt_shipState' name='txt_shipState' style='width:50px;' Class = 'ecomTextBoxes'  MaxLength = '10' /></td>";

                    temp += "<tr><td class='nwSubTDLabels'>Country</td><td class='nwSubTdContent'><input type='text' ID='txt_shipCountry' name='txt_shipCountry' style='width:150px;' Class = 'ecomTextBoxes'  MaxLength = '20' /></td>";
                    temp += "<td class='nwSubTDLabels'>Postal Code</td><td class='nwSubTdContent' ><input type='text' ID='txt_shipZip' name='txt_shipZip' style='width:80px;' Class = 'ecomTextBoxes'  MaxLength = '10' />";
                    temp += "</td>";
                    temp += "</tr>";
                    temp += "</table>";

                    temp += "</td></tr>";


                    temp += "<tr><td class='nwAccountLeftTD'>Shipping Account IDs</td><td class='nwAccountRightTD'>";
                    temp += "<div class='nwAcctInstructionDiv'>Current Shipping Account IDs:</div>";
                    temp += shipperIDUL;


                    temp += "<div class='nwAcctInstructionDiv'>Add a New Shipper ID:</div>";

                    temp += "<table>";
                    temp += "<tr><td class='nwSubTDLabels'>Company</td><td class='nwSubTdContent' colspan = '3'><Select id='ddl_NewShipperCarrier' name='ddl_NewShipperCarrier'><option value=''>Select Carrier</Option><option value='UPS'>U.P.S.</Option><option value='FedEx'>FedEx</Option><option value='DHL'>DHL</Option></Select></td>";
                    temp += "<tr><td class='nwSubTDLabels'>Account Number</td><td class='nwSubTdContent' colspan = '3'><input type='text' ID='txt_NewShipperAccount' name='txt_NewShipperAccount' style='width:300px;' Class = 'ecomTextBoxes'  MaxLength = '50' /></td></tr>";

                    temp += "</td></tr></table>";

                    temp += "</td></tr>";

                    //temp += "<tr><td class='nwAccountLeftTD'>Account Terms</td>";

                    //temp += "<td class='nwAccountRightTD'>";
                    //temp += System.Web.HttpContext.Current.Session["TERM_DESCRIPTION"];
                    //temp += "</td></tr>";

                } // if not bknNewAccount


                //---------------- CAPTCHA for New Accounts
                if (blnnewAccout)
                {

                    //System.Web.HttpContext.Current.Session("adminKeepAliveTime") = 300
                    //SessionState

                    //System.Web.HttpContext.Current.Session["CaptchaImageText"] = GenUtils.RandomString(5, "numbers");

                    //string dfassad = System.Web.HttpContext.Current.Session["CaptchaImageText"].ToString();



                    temp += "<tr><td class='nwAccountLeftTD'>Human Verification</td>";
                    temp += "<td class='nwAccountRightTD'>";

                    temp += "<div class='nwAcctInstructionDiv'><p>Please enter the numbers in the text box.</p></div>";

                    temp += "<span style='padding-left:10px;'><img src='ImageGen.aspx?a=" + DateTime.Now + "' name='CaptchaImage' ></span>";

                    //temp += "&nbsp;&nbsp;<button  onClick=" + (char)34 + "JavaScript:refreshImage('ImageGen.aspx')" + (char)34 + ">Refresh Image</button><br />";

                    temp += "<div style='padding-left:10px;padding-top:10px;'><input type='text' ID='txt_CAPTCHA' name='txt_CAPTCHA' style='width:200px;' Class = 'ecomTextBoxes'  MaxLength = '10' value = ''  /></div>";














                    temp += "</td></tr>";

                }





            }  //---- if not back

       }

        temp += "<tr><td class='nwAccountLeftTD'>Note to Jet Parts Engineering</td>";
        temp += "<td class='nwAccountRightTD'>";
            if (blnnewAccout)
            {
                temp += "<div class='nwAcctInstructionDiv'><p>Please feel free to add any additional information that may be helpful or send us questions,  comments or suggestions about <b>the New JPE Smart Buyer</b> website.  Our software developers are working to make <b>JPE Smart Buyer</b> the easiest-to-use and most useful resource available in the PMA marketplace.  <b>We appreciate your input.</b></p></div>";
            }
            else
            {

        temp += "<div class='nwAcctInstructionDiv'><p>When change your profile information, billing address, add a ship to address or shipper ID, your sales representative at Jet Parts Engineering is notified of the change so that out in-house order control and delivery system can be updated.  Please allow up to one business day for changes to take effect.  You can also enter a new ship to address or shipper ID when you place an order.</p><p>Please use the textbox below to add any comments or clarifications regarding the changes you made.</p></p></div>";
                }

        temp += "<textarea id='txt_Notes' name='txt_Notes' style='height:150px;width:500px'> </textarea>";




        if (blnReturningUser)
        {

            temp += "<div style='width:100%;text-align:right; margin-right:5px auto;padding-top:5px; margin-bottom:5px;'><img src='images/welcome_icon.gif' alt='' /></div>";

        }


        temp += "</td></tr>";




        if (!blnnewAccout && UserType.ToLower() == "admin")
        {
        temp += "<tr><td class='nwAccountLeftTD'>JPE Office Use</td>";
        temp += "<td class='nwAccountRightTD'>";

        temp += "<div class='nwAcctInstructionDiv'>Account Information:</div>";
        temp += "<ul>";
        temp += "<li>Account type: " + AccountType + "</li>";
        temp += "<li>Account Created: " + AccountCreated + "</li>";
        temp += "<li>eMail Validated: " + emailValidated + "</li>";
        temp += "<li>Last User Update: " + LastUserUpdate + "</li>";
        temp += "<li>Last Admin Update: " + LastUserUpdate + "</li>";
        temp += "<li>Last Customer Comment: " + custLastComment + "</li>";
        temp += "<li>Pricing View: " + PricingView + "</li>";
        if (AccountType.ToLower() == "user")
        {
            temp += "<li>User Password: " + UserPassword + "</li>";

        }


        temp += "</ul>";
        temp += "<table>";

        //temp += "<tr><td class='nwSubTDLabels' style='width:80px;'>Approved</td><td class='nwSubTdContent' ><input type='checkbox' ID='chk_Approved' name='chk_Approved' " + approved_checked + "  /></td><td class='nwSubTDLabels'>Active</td><td class='nwSubTdContent'><input type='checkbox' ID='chk_Active' name='chk_Active' " + active_checked + " /></td>";

        //temp += "<td class='nwSubTDLabels'>Send eMail</td><td class='nwSubTdContent'><input type='checkbox' ID='chk_SendEmail' name='chk_SendEmail' /></td> <td class='nwSubTDLabels'>Delete Account</td><td class='nwSubTdContent'><input type='checkbox' ID='chk_DeleteAccount' name='chk_DeleteAccount' /></td>";


        //temp += "<td class='nwSubTDLabels'>Send eMail</td><td class='nwSubTdContent'><input type='checkbox' ID='chk_SendEmail' name='chk_SendEmail' /></td> <td class='nwSubTDLabels'>Delete Account</td><td class='nwSubTdContent'><input type='checkbox' ID='chk_DeleteAccount' name='chk_DeleteAccount' /></td>";
       // temp += "</tr>";
  

        temp += "<tr><td class='nwSubTDLabels' style='width:80px;'>Rolodex Key</td><td class='nwSubTdLabels' colspan = '7'><input type='text' ID='txt_RolodexKey' name='txt_RolodexKey' style='width:100px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + RolodexKey + "'/>&nbsp;Company Key&nbsp;<input type='text' ID='txt_QuantumCompanyCode' name='txt_QuantumCompanyCode' style='width:100px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + CompanyKey + "'/></td></tr>";

        temp += "<tr><td class='nwSubTDLabels' style='width:80px;'>Salesperson Key </td><td class='nwSubTdLabels' colspan = '7'><input type='text' ID='txt_MySalesPersonKey' name='txt_MySalesPersonKey' style='width:100px;' Class = 'ecomTextBoxes'  MaxLength = '50' value = '" + SalesPersonKey + "'/>&nbsp;AccountType&nbsp;" + ddlUserType + "</td></tr>";




        temp += "<tr><td class='nwSubTDLabels' style='width:80px;'>Send Email</td><td class='nwSubTdContent' ><input type='checkbox' ID='chk_SendEmail' name='chk_SendEmail' /></td><td class='nwSubTDLabels'>Delete Account</td><td class='nwSubTdContent'><input type='checkbox' ID='chk_DeleteAccount' name='chk_DeleteAccount' /></td><td></td></tr>";



        temp += "<tr><td class='nwSubTDLabels'  style='width:80px;'>JPE Quick Note</td><td class='nwSubTdContent' colspan = '7'>";

        temp += " <textarea id='txt_JPENotes' name='txt_JPENotes' style='height:150px;width:400px'>" +  JPENotes + "</textarea>";

        //temp += "<input type = 'hidden' id = 'hdn_Approved' name = 'hdn_Approved' value ='" + hdn_approved_checked + "' />";
        //temp += "<input type = 'hidden' id = 'hdn_Active'  name = 'hdn_Active'  value ='" + hdn_active_checked + "' />";
        temp += "<input type = 'hidden' id = 'hdn_RolodexKey' name = 'hdn_RolodexKey' value ='" + RolodexKey + "' />";
        temp += "<input type = 'hidden' id = 'hdn_QuantumCompanyCode' name = 'hdn_QuantumCompanyCode' value ='" + CompanyKey + "' />";
        temp += "<input type = 'hidden' id = 'hdn_MySalesPersonKey' name = 'hdn_MySalesPersonKey' value ='" + SalesPersonKey + "' />";

            temp += "<input type = 'hidden' id = 'hdn_AccountType' name = 'hdn_AccountType' value ='" + AccountType + "' />";


        temp += "<input type = 'hidden' id = 'hdn_JPENotes' name = 'hdn_JPENotes' value ='" + JPENotes + "' />";









        temp += "</td>";

        temp += "</table>";

        } // user type is admin


        temp += "</td></tr>";
        if (!blnnewAccout)
        {
            temp += "<tr><td colspan = '2'><div style='padding:20px; margin-right:10px auto; text-align:right; font-size:10px;color:#999999;'><input type='checkbox' ID='chk_emailMeACopy' name='chk_emailMeACopy' />Email Me a Copy of Changes</div>";

        }






        temp += "</table>";
   



        return temp;


    }



}

