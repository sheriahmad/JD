<%@ Page Title="" Language="C#" MasterPageFile="~/Code/Site.Master" AutoEventWireup="true" CodeBehind="calverton.aspx.cs" Inherits="JetParts.Web.Com.calverton" %>
<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_calverton_bw.png" MouseOverImage="images/banner_calverton_color.png"
        Alt="Jet Parts Engineering Calverton, New York Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>
            Calverton, New York Office</h2>
        <p class="address">
            Jet Parts Engineering<br />
            335 Burman Blvd<br />
            Calverton, NY 11933
        </p>
        <p class="address">
            Map & Directions (Links will open in new window):<br />
            <br />
            <a href="http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=335+Burman+Blvd,+Calverton,+NY+11933&aq=1&sll=40.912896,-72.79644&sspn=0.045015,0.051928&ie=UTF8&hq=&hnear=335+Burman+Blvd,+Calverton,+NY+11933&ll=40.915394,-72.797213&spn=0.045013,0.051928&z=14"
                target="_new">Google Maps</a>
        </p>
        <p class="address">
            Contact: Daryl Porter<br />
            <dl>
                <dt>Telephone</dt><dd>(631) 208-0727</dd>
                <dt>E-mail</dt><dd><a href="mailto:sales@jetpartsengineering.com?subject=New York Sales Office">sales@jetpartsengineering.com</a></dd>
            </dl>
        </p>
    </div>
</asp:Content>
