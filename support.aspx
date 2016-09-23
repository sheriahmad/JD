<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="support.aspx.cs" Inherits="JetParts.Web.Com.support" %>

     
<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_support_bw.png" MouseOverImage="images/banner_support_color.png"
        Alt="Customer Support" runat="server" />
    <div style="width: 660px;">
    <h3 class="pageHeadlinesH3">
        Jet Parts Engineering provides stellar customer service and satisfaction.</h3>
    <p class="pageBody">
        <img src="images/jpe_px6.png" align="right" hspace="20" vspace="20" alt="" />We
        are known for our high caliber of customer service, and strive to ensure that your
        experience with us is a personalized and fulfilling one. Not only is it important
        for us to help you in every way we can, we also take the time to understand your
        company's modes and needs so that we can better serve you every time. We value your
        time, and strive to answer and return every phone call personally within 24 hours,
        if not immediately. We are proactive in communicating, so you'll never be left in
        the dark.
    </p>
    <p class="pageBody">
        One of our company directives is to maintain stocking levels that enable
        us to ship same day in most cases, or next day on late arriving orders. We understand
        that turnaround time is very important with today's MRO and we attempt to set ourselves
        apart by employing this "Just in Time" inventory capability. Consignment stock can
        be made available in some cases, in order to make it even easier to meet your supply
        needs. Our corporate office is connected to our warehouse, providing us with immediate
        access to our products and a direct handle on all of our inventory.</p> 
    <p class="pageBody">
        Moreover, all of our employees are friendly, professional, and focused on your needs. Delta Air
        Lines recognized us with their <asp:Hyperlink ID="deltastar" NavigateUrl="deltastar.aspx" runat="server">
        Star Award</asp:Hyperlink> in 2004 and 2006 as Supplier of the Year. Become a part of the 
        Jet Parts Engineering fabric, and let us show you how business can be done.</p>
    <p class="pageBody">
        Please see a description of the <asp:Hyperlink ID="deltastar1" NavigateUrl="deltastar.aspx" runat="server">
        Delta awards</asp:Hyperlink> that we are honored to present as a
        customer testimonial to our service.</p>

    <h3 class="pageHeadlinesH3">
        Contact: Sales Department</h3>
    <p class="pageBody">
     Telephone (206) 281-0963<br />
     Facsimile (206) 838-8487<br />
     E-mail <a href="mailto:sales@jetpartsengineering.com">sales@jetpartsengineering.com</a>
        </p>
    </div>
</asp:Content>
