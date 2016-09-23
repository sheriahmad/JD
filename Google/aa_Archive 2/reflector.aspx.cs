using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class reflector : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        HttpApplication ht = HttpContext.Current.ApplicationInstance;

        lblReflector.Text = "~~" + ht.Request.QueryString.ToString() + "~~";


    }
}
