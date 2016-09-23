using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class ImageGen : System.Web.UI.Page
{
    private void Page_Load(object sender, System.EventArgs e)
    {
        // Create a CAPTCHA image using the text stored in the Session object.
       
        //CaptchaImage.CaptchaImage ci = new CaptchaImage.CaptchaImage(Page.Session["CaptchaImageText"].ToString(), 200, 50, "Century Schoolbook");

        Page.Session["CaptchaImageText"] = GenUtils.RandomString(5, "numbers");


        CaptchaImage.CaptchaImage ci = new CaptchaImage.CaptchaImage(Page.Session["CaptchaImageText"].ToString(), 200, 50, "Cooper Black");


        Page.Session["last_CaptchaText"] = Page.Session["CaptchaImageText"].ToString();


        //CaptchaImage.CaptchaImage ci = new CaptchaImage.CaptchaImage("lljhjkas", 200, 50, "Century Schoolbook");

        // Change the response headers to output a JPEG image.
        this.Response.Clear();
        this.Response.ContentType = "image/jpeg";

        // Write the image to the response stream in JPEG format.
        ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

        // Dispose of the CAPTCHA image object.
        ci.Dispose();
    }



}
