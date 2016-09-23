using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class letter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        Page.Session["MMenu"] = "about";
        Page.Session["SMenu"] = "letter";

    }
}
