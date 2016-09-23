using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LongRunningProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Padding to circumvent IE's buffer*
        Response.Write(new string('*', 256));
        Response.Flush();

        // Initialization
        UpdateProgress(0, "Initializing task.");
        System.Threading.Thread.Sleep(10000);

        // Gather data.
        UpdateProgress(25, "Gathering data.");
        System.Threading.Thread.Sleep(6000);

        // Process data.
        UpdateProgress(40, "Processing data.");
        System.Threading.Thread.Sleep(20000);

        // Clean up.
        UpdateProgress(90, "Cleaning up.");
        System.Threading.Thread.Sleep(4000);

        // Task completed.
        UpdateProgress(100, "Task completed!");
    }

    protected void UpdateProgress(int PercentComplete, string Message)
    {
        // Write out the parent script callback.
        Response.Write(String.Format(
          "<script>parent.UpdateProgress({0}, '{1}');</script>",
          PercentComplete, Message));
        // To be sure the response isn't buffered on the server.
        Response.Flush();
    }
}
