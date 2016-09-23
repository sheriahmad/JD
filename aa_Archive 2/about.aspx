<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="about.aspx.cs" Inherits="JetParts.Web.Com.about" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_about_bw.png" MouseOverImage="images/banner_about_color.png"
        Alt="About Jet Parts Engineering" runat="server" />
    <p class="pageBodyPar1">
        <img src="images/jpe_px1.png" align="right" hspace="20" vspace="20" alt="" />Jet
        Parts Engineering is an engineering-intensive company that manufactures and sells
        FAA-approved PMA parts. Our PMAs go through an intensive approval process with the
        FAA to ensure that they are equal to or better than their OEM counterparts. We understand
        how important it is to offer the safest and most reliable parts to our customers,
        and we are able to offer them at a price dramatically lower than the OEMs.</p>
    <p class="pageBody">
        Our ability to deliver premier PMA parts comes from a highly focused approach that
        allows us to design, produce, inspect, and ship parts in an efficient, effective,
        and timely manner. We try to keep all of our parts in stock, and in doing so, we
        offer greatly reduced lead times.</p>
    <p class="pageBody">
        We have established a reputation in the aviation industry for supplying the highest
        possible quality of parts while providing exceptional service to our customers.</p>
    <p class="pageBody">
        Jet Parts Engineering, your PMA Solution since 1994.</p>
</asp:Content>
