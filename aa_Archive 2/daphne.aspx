<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="daphne.aspx.cs" Inherits="JetParts.Web.Com.daphne" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_daphne_bw.png" MouseOverImage="images/banner_daphne_color.png"
        Alt="Jet Parts Engineering Daphne Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>
            Daphne, Alabama Office</h2>
        <p class="address">
            Jet Parts Engineering<br />
            25303 Profit Drive<br />
            Daphne, AL 36526
        </p>
        <p class="address">
            Map & Directions (Links will open in new window):<br />
            <br />
            <a href="http://maps.google.com/maps?f=q&hl=en&q=25303+Profit+Drive,+Daphne,+AL+36526&ie=UTF8&z=16&om=1&iwloc=A"
                target="_new">Google Maps</a>
        </p>
        <p class="address">
            Contact: Rafa Laredo<br />
            <dl>
                <dt>Telephone</dt><dd>(251) 625-1666</dd>
                <dt>Facsimile</dt><dd>(251) 625-1666</dd>
                <dt>E-mail</dt><dd><a href="mailto:sales@jetpartsengineering.com?subject=Alabama Sales Office">sales@jetpartsengineering.com</a></dd>
            </dl>
        </p>
    </div>
</asp:Content>
