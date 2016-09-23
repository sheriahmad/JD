<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="careers.aspx.cs" Inherits="JetParts.Web.Com.careers" %>


<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_careers_bw.png" MouseOverImage="images/banner_careers_color.png"
        Alt="Creers at Jet Parts Engineering" runat="server" />
    <div style="width: 660px;">
        <h3 class="pageHeadlinesH3">
            Careers at Jet Parts Engineering</h3>

          <!-- 
              IF THERE ARE NO OPEN POSITIONS THEN USE THIS CODE AND COMMENT OUT THE OTHER CODE. 
              <p class="pageBody">Unfortunately we don't have any current positions.</p>  -->
        <p class="pageBody">
            <img src="images/jpe_px8.png" align="right" hspace="20" vspace="20" alt="" />Come join the team at Jet Parts Engineering. 
            We are an Equal Opportunity Employer, and offer many opportunities and great benefits to our staff. Even though our largest 
            presence is at our headquarters in Seattle, Washington, we also have offices in the Netherlands, Germany
            and various parts of the United States.
            </p>
        <p class="pageBody">Below are our current open positions. 
            <br />To apply for an open position, please click the link for the job you are interested in;  you will be able to view the job description and apply directly in the link.</p>
        <br />
        
            
        

            
         <p class="pageBody">
            <a  href=" https://workforcenow.adp.com/jobs/apply/posting.html?client=jetparts&jobId=85273&lang=en_US&source=CC3">JPE Staff Accountant II - WA </a> </p> 	

		<p class="pageBody">
            <a  href=" https://workforcenow.adp.com/jobs/apply/posting.html?client=jetparts&jobId=85439&lang=en_US&source=CC3">JPE Outside Sales Representative - GA </a> </p> 		
		
		<p class="pageBody">
            <a  href=" https://workforcenow.adp.com/jobs/apply/posting.html?client=jetparts&jobId=83176&lang=en_US&source=CC2">JPE Procurement Agent II - WA </a> </p>

     
        <br />
        <p class="pageBody">
            Even if you didn't see your ideal job, we are always looking to add high energy, dynamic people to our team. If you are
            interested in a career with Jet Parts Engineering,<br /> please email your resume to <a
                href="mailto:hr@jetpartsengineering.com?subject=Careers">hr@jetpartsengineering.com</a>
            or mail it to:</p>
        <br />
        <br />
        <p class="address" style="text-align: left; padding-left: 30px;">
            Jet Parts Engineering<br />
            Att: HR Administrator<br />
            4772 Ohio Ave. S.<br />
            Seattle, WA 98134
        </p>
    </div>
</asp:Content>
