<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="default.aspx.cs" Inherits="JetParts.Web.Com._Default" %>

<asp:Content runat="server" ContentPlaceHolderID="ViewPortContent">
    <div class="HomePageHeaderDiv" id="pageHeaderDiv">
        <object width="660" height="250">
            <param name="movie" value="smartbuyer link.swf" />
            <embed height="250" src="smartbuyer link.swf" width="660" />
        </object>
        <div class="pageHeadlinesH3" style="width:100%; text-align:right; text-decoration:none;">
        <a href="http://www.jpesmartbuyer.com" target="_blank" border="0" style="text-decoration: none; font-size:110%;">
                JPE SmartBuyer &gt;&gt;</a></div>
    </div>
    <p class="pageBody">
        The goal of Jet Parts Engineering has always been to provide our MRO partners an accessible 
        and easy alternative to OEMs. With few exceptions, we carry all of our PMA parts in stock 
        and can ship within twenty four hours.</p>
    <p class="pageBodyPar1">
        Since 1994, we have built a solid aerospace industry reputation for supplying high quality 
        FAA-PMA parts. We pride ourselves on providing excellent customer service serving such clients 
        as Delta, Lufthansa, and United Airlines, as well as repair stations all across the globe. Our 
        standards in quality control and assurance are elevated to the highest degree. Our engineers 
        and technicians are chosen from the best and brightest in their fields. They have fine-tuned 
        our reverse engineering process to guarantee only the safest and most reliable parts possible. 
        Our employees are here to serve you, our customer. We are certain you will find your experience 
        with Jet Parts Engineering to be enjoyable and satisfying.</p>
    <br />
    <asp:HyperLink ID="supplementsLink" NavigateUrl="~/supplements.aspx" ImageUrl="~/images/approvals.png"
        ToolTip="New FAA Approvals" Height="120" Width="325" Style="margin-right: 5px;
        float: left;" runat="server" />
    <asp:HyperLink ID="newsLink" NavigateUrl="~/news.aspx" ImageUrl="~/images/events.png"
        ToolTip="News and Events" Height="120" Width="325" runat="server" />
    <div style="clear: both;">
    </div>
</asp:Content>
