<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="seattle.aspx.cs" Inherits="JetParts.Web.Com.seattle" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_seattle_bw.png" MouseOverImage="images/banner_seattle_olor.png"
        Alt="Jet Parts Engineering Seattle Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>North American Headquarters</h2>
        <p class="address">
            Jet Parts Engineering<br />
            220 West Mercer Street, Suite 100<br />
            Seattle, WA 98119
        </p>
        <p class="address">
            Map & Directions (Links will open in new window):<br />
            <br />
            <a href="http://maps.google.com/maps?f=q&hl=en&q=220+W.+Mercer+St.,+seattle,+wa&ie=UTF8&z=15&om=1&iwloc=A"
                target="_new">Google Maps</a>
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
