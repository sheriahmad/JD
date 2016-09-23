<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="careers.aspx.cs" Inherits="JetParts.Web.Com.careers" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_careers_bw.png" MouseOverImage="images/banner_careers_color.png"
        Alt="Creers at Jet Parts Engineering" runat="server" />
    <div style="width: 660px;">
        <h3 class="pageHeadlinesH3">
            Careers at Jet Parts Engineering</h3>
        <p class="pageBody">
            <img src="images/jpe_px8.png" align="right" hspace="20" vspace="20" alt="" />Come
            join the team at Jet Parts Engineering. We are an Equal Opportunity Employer, and
            offer many opportunities and benefits to our staff. The majority of our employees
            work at our headquarters in Seattle, Washington, but we also have offices in Europe,
            Asia, and various parts of the United States.</p>
        <p class="pageBody">
            We are always looking to add high energy, dynamic people to our team. If you are
            interested in a career with Jet Parts Engineering, please email your resume to <a
                href="mailto:hr@jetpartsengineering.com?subject=Careers">hr@jetpartsengineering.com</a>
            or mail it to:</p>
        <br />
        <br />
        <p class="address" style="text-align: left; padding-left: 30px;">
            Jet Parts Engineering<br />
            Att: HR Administrator<br />
            220 West Mercer Street, Suite 100<br />
            Seattle, WA 98119
        </p>
    </div>
</asp:Content>
