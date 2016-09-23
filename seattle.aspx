<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="seattle.aspx.cs" Inherits="JetParts.Web.Com.seattle" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
   

    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_seattle_bw.png" MouseOverImage="images/banner_seattle_color.png"
        Alt="Jet Parts Engineering Seattle Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>North American Headquarters</h2>
        <p class="address">
            Jet Parts Engineering<br />
            4772 Ohio Ave. S. <br />
            Seattle, WA 98134
        </p>
        <p class="address">
           
            
            <iframe src="https://www.google.com/maps/embed?pb=!1m5!3m3!1m2!1s0x54901542fa33630b%3A0xc31acab2f8eb0aa4!2sJet+Parts+Engineering+Inc%2C+Seattle%2C+WA!5e0!3m2!1sen!2sus!4v1387234988607" width="600" height="350" frameborder="0" style="border:0"></iframe>
            
            
            
            <br />
            <br />
            
            <iframe width="600" height="338" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/maps?layer=c&panoid=uCwh0U8iFIUAAAQXDQ_4Ug&ie=UTF8&source=embed&output=svembed&cbp=13%2C39.74%2C%2C0%2C0"></iframe><br /><small><a href="https://www.google.com/maps/@47.558376,-122.337846,3a,75y,39.74h,90t/data=!3m5!1e1!3m3!1suCwh0U8iFIUAAAQXDQ_4Ug!2e0!3e2">View Larger Map</a></small>
        </p>
        <p class="address">
            Contact: Sales Department<br />
            <dl>
            <dt>Telephone</dt><dd>(206) 281-0963</dd>
            <dt>Facsimile</dt><dd>(206) 838-8487</dd>
            <dt>E-mail</dt><dd><a href="mailto:sales@jetpartsengineering.com?subject=North American Sales Office">sales@jetpartsengineering.com</a></dd>
            <dt>Accounting</dt><dd><a href="mailto:accounting@jetpartsengineering.com m?subject=Accounting">accounting@jetpartsengineering.com</a></dd>
            </dl>
        </p>
        </div>
</asp:Content>
