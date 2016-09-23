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
        Since its inception in 1994, Jet Parts has balanced growth with a steadfast
        foundation of consistency and professional integrity. We at Jet Parts are dedicated
        to providing excellent customer service and satisfaction. From greatly reduced lead
        times and a competitive pricing structure, to an ever-expanding product line of
        FAA-approved PMA parts, we have helped our customers reduce their overall cost of
        doing business.</p>
    <p class="pageBody">
        In complementing and sustaining our business objectives, Jet Parts Engineering is
        proud to have an excellent support team. Our staff is composed of some of the best
        and brightest in their fields. We believe in attentiveness, faithfulness and responsiblity
        to two parties: our customers and our employees. You will find our commitment and expertise 
        to be without equal in the aerospace industry.</p>
    <p class="pageBody">
        I would like to take this opportunity to thank you for visiting our website. We
        look forward to enhancing the great relationships we have built so far. Additionally,
        we look forward to forging new bonds. We want to help you drive your business to
        new heights. If you are a current customer, we thank you for your continued support.
        If you are a prospective customer, we encourage you to contact us and speak with
        someone from our team. You will find our staff to be amiable, professional, and
        knowledgeable.</p>
    <p class="pageBody">
        Thank you for considering Jet Parts Engineering as a part of your business plan.
        We are proud to be your PMA solution.</p>
    <p class="LetterSig">
        <b>Anu Goel</b><br />
        President,<br />
        Jet Parts Engineering Inc.</p>
    </div>
</asp:Content>
