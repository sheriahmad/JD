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
public class ecom_AJAXInterface : System.Web.Services.WebService
{

    public ecom_AJAXInterface()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }





    [WebMethod]
    public string MyAccountUpdate(string strContext)
    {
        string[] temp = strContext.Split('|');
        string strPanel = string.Empty;
        string queryString = string.Empty;


        switch (temp[0])
        {
            case "QTYAUTO":



                queryString = " UPDATE ecom_UserAccounts SET useAutoCheck = ";
                if (temp[1] == "true")
                {
                    queryString += "1";
                }
                else
                {
                    queryString += "0";
                }

                queryString += " WHERE pkAccountKey = " + temp[2] + " ";


                strPanel = UpdateecomAccount(queryString);
                break;

            case "USEHINTS":



                queryString = " UPDATE ecom_UserAccounts SET useHints = ";
                if (temp[1] == "true")
                {
                    queryString += "1";
                }
                else
                {
                    queryString += "0";
                }

                queryString += " WHERE pkAccountKey = " + temp[2] + " ";


                strPanel = UpdateecomAccount(queryString);
                break;


            //case "PN":
            //    strPanel = BuildPartsPanel(temp[1], temp[2]);
            //    break;
        }

        return strPanel;



    }




    public string UpdateecomAccount(string queryString)
    {
        string temp = string.Empty;
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


        }



        return temp;


    }



}