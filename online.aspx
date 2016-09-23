<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="online.aspx.cs" Inherits="JetParts.Web.Com.online" %>
<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
   

    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_order_bw.png" MouseOverImage="images/banner_order_color.png" Alt="JPE Smart Buyer" runat="server" />
    <h3 class="pageHeadlinesH3">
        Your Online PMA Solution</h3>
    <p class="pageBody">
        At Jet Parts Engineering we recognize that your time is extremely valuable, and
        the ability to have information and parts readily available is paramount to your
        operation. By introducing our ecommerce solution: JPE Smart Buyer, and our consignment
        inventory program: JPE Smart Stock, we are confident in helping you achieve your
        goals.</p>
    <p class="pageBody">
        Our customers have said that the business of supply chain management and procurement
        is getting more complicated everyday. Jet Parts Engineering is proud to promote
        this suite of tools, called JPE Smart Solutions - designed to help minimize the
        challenges you face daily as a supply chain specialist.</p>
    <asp:HyperLink ID="SmartBuyer" NavigateUrl="http://www.jpesmartbuyer.com" ImageUrl="~/images/login_med.png"
        Target="_blank" ToolTip="JPE Smart Buyer" runat="server" Style="float: left;
        padding: 30px 20px 0 0;" />
    <div>
        <h3 class="pageHeadlinesH3" style="text-decoration: none;">
            Whether you are a current customer or a potential partner,
            <a href="http://www.jpesmartbuyer.com" target="_blank" border="0" style="text-decoration: none; font-size:110%;">
                &lt;&lt; Sign-up Today!</a></h3>
        <p class="pageBody">
            Simply go to jpesmartbuyer.com to start placing and tracking your orders online! 
            On the login screen, click on the “Create an Account” icon and tell us who you are. 
            You will receive an email with your login information once your account is ready.</p>
    </div>
    <div style="clear: both;"></div>
    <h3 class="pageHeadlinesH3">Smart Solution</h3>
    <p class="pageBody">
        Discover an easier way to manage your PMA parts supply requirements. We are proud
        to introduce JPE Smart Buyer, our new and easy to use ecommerce system. Linked directly
        to our order and shipping systems, JPE Smart Buyer is the fastest way to order and
        track your PMA parts in real time.</p>
    <h3 class="pageHeadlinesH3">Smart Search</h3>
    <asp:Image ID="SearchShot" ImageUrl="~/images/search_small.png" runat="server" Style="float: right;
        padding: 35px 110px 10px 20px;" />
    <p class="pageBody">
        JPE Smart Buyer allows you to:
        <ul>
        <li><p class="pageBody">Search for parts by PMA or OEM part number</p></li>
        <li><p class="pageBody">Search one part at a time</p></li>
        <li><p class="pageBody">Copy and paste your entire parts list</p></li>
        <li><p class="pageBody">Search by NHA relationships</p></li>
        </ul><br /><br /></p>
    <div style="clear: both;">
    </div>
    <asp:Image ID="ResultsShot" ImageUrl="~/images/search_result_small.png" runat="server"
        Style="float: right; padding: 35px 110px 10px 20px;" />
    <h3 class="pageHeadlinesH3">
        Smart Results</h3>
    <ul>
        <li><p class="pageBody">View all your search results on one easy to use interface.</p></li>
        <li><p class="pageBody">Parts related by NHA are conveniently displayed with each result.</p></li>
        <li><p class="pageBody">Get further details, price quotes and even order parts directly from your search page!</p></li>
    </ul>
    <div style="clear: both;">
    </div>
</asp:Content>
