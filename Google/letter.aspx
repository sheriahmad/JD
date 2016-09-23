<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="letter.aspx.cs" Inherits="JetParts.Web.Com.letter" %>
<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
   

<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_letter_bw.png" MouseOverImage="images/banner_letter_color.png" Alt="Letter From the President" runat="server" />
    <div style="width: 660px;">
    <h3 class="pageHeadlinesH3">
        A Note from Anu Goel</h3>
    <%--<p class="pageBody">
        <img src="images/jpe_px2.png" align="right" hspace="20" vspace="20" alt="" />Jet
        Parts Engineering is a quickly growing company, currently operating out
        of three continents, with offices in Seattle, Munich, and Beijing.</p>--%>
    <p class="pageBody">
        This year, Jet Parts Engineering celebrates our 20th Anniversary and I, along with our entire staff, would like to thank you for contributing to our success. Since its inception in 1994, Jet Parts has balanced growth with a steadfast foundation of consistency and professional integrity. We at Jet Parts are dedicated to providing excellent customer service, satisfaction, and quality. From greatly reduced lead times and a competitive pricing structure, to an ever-expanding product line of FAA-approved PMA parts, we continually strive to help our customers reduce their overall cost of doing business.</p>
    <p class="pageBody">
        In complementing and sustaining our business objectives, Jet Parts Engineering is home to an excellent support team. Composed of some of the best and brightest within their field, our staff believes in attentiveness, faithfulness and responsibility to both our customers and fellow employees. Our commitment and expertise is unparalleled by any other within the aerospace industry.</p>
    <p class="pageBody">
        I would like to take this opportunity to thank you for visiting our website. The goal is simple: We want to help you drive your business to new heights. We look forward to enhancing our existing relationships and are eager to forge new bonds. If you are a current customer, thank you for your continued support. If you are a prospective customer, I encourage you to contact someone from our team and let us know how we can fulfill your PMA needs. I have no doubt you will find our staff to be amiable, professional, and knowledgeable.</p>
    <p class="pageBody">
        Thank you for considering Jet Parts Engineering as a part of your business plan. We are proud to be your PMA solution with 20 years of success, innovating for the future.</p>
    <p class="LetterSig">
        <b>Anu Goel</b><br />
        President,<br />
        Jet Parts Engineering Inc.</p>
    </div>
</asp:Content>
