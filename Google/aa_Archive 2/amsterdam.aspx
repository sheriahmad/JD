<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="amsterdam.aspx.cs" Inherits="JetParts.Web.Com.amsterdam" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_amsterdam_bw.png" MouseOverImage="images/banner_amsterdam_color.png"
        Alt="Jet Parts Engineering Amsterdam Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>Europe Headquarters</h2>
        <p class="address">
                    Jet Parts Engineering, Europe<br />
                    Het Oerd 59<br />
                    2134 ZE Hoofddorp<br />
                    The Netherlands
        </p>
        <p class="address">
            Map & Directions (Links will open in new window):<br />
            <br />
            <a href="http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=Het+Oerd+59,+Hoofddorp,+Nederland&aq=0&sll=52.132633,5.291266&sspn=2.512054,8.453979&g=Netherlands&ie=UTF8&hq=&hnear=Het+Oerd+59,+Hoofddorp,+Haarlemmermeer,+Noord-Holland,+The+Netherlands&ll=52.318763,4.647689&spn=0.019543,0.066047&z=15"
                target="_new">Google Maps</a>
        </p>
        <p class="address">
            Contact: Manodj Soedhwa<br />
            <dl>
            <dt>Telephone</dt><dd>+ 31 (0)6 146 926 24</dd>
            <dt>E-mail</dt><dd><a href="mailto:sales@jetpartsengineering.com?subject=Europe Sales Office">sales@jetpartsengineering.com</a></dd>
            </dl>
        </p>
        </div>
</asp:Content>
