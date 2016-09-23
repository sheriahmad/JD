<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ecom_prodSearch.aspx.cs" Inherits="ecom_prodSearch" MasterPageFile="JPESite.Master" Theme="eCommerceTheme" EnableEventValidation="false"  viewstateencryptionmode="Never"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="nk" Namespace="nStuff.AtlasControls" %>




<asp:Content ID="Content1" runat = "server" ContentPlaceHolderID="ViewPortContent">
  
<style type="text/css" > 
.modalBackground {
	background-color:Gray;
	filter:alpha(opacity=70);
	opacity:0.7;
}
</style>
  
  
<script type="text/javascript" language="javascript">
    //<![CDATA[
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        scrollTo(0, 0);
        tryAssociated();
    }
 //]]>
</script>




<script src="js/ecom_BodyFunctions.js" type="text/javascript"></script>



    <div>
    
  <div id="header">
		<table style="height:80px;" >
			<tr>
				<td style="vertical-align:middle;">
					<a href="default.aspx"><img id="logo" src="images/logo-JPE_2010JL2.gif" title="Jet Parts Engineering" width="201" height="57" /></a>
					
					  
<%--<iframe ID="iFrameNav" name="iFrameNav" src="reflector.aspx?" style="height:60px; width:250px; visibility:hidden;"></iframe>--%>

				</td>
				<td style="margin-right: 5px auto; text-align:right;">
						
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

 <table style="width:200px;"><tr>
 
 <% 
     if ((int)Page.Session["LoggedInUserID"] > 0)
     {
     %>
          <td style="font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;width:75px;height:80px;">
 
    <a name onclick="javascript:ShowMyAccountModalPopupEvt('<% =Page.Session["AccountType"] %>|<% =Page.Session["pkAccountKey"] %>|MYACCOUNT|<% =Page.Session["AccountType"] %>||');" onmouseover="this.className='SBpodTitleBarOMOver';"  onmouseout="this.className = 'SBpodTitleBarOMOut';"><img src="images/editmyaccount.png" border="0" alt="Manage My Account" /><br />Manage My Account</a>
         
     </td>
    
     <td style="font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;width:75px;">
 
    <a name onclick="javascript: return doButtonClick('btnSHowHistoryModal');" onmouseover="this.className='SBpodTitleBarOMOver';"  onmouseout="this.className = 'SBpodTitleBarOMOut';"><img src="images/orderhist2.png" border="0" alt="My Order History and Tracking" /><br />Order History and Tracking</a>
         
     </td>
     <td style="font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;width:75px;">
        <a name onclick="javascript: return doButtonClick('btnLogOut');" onmouseover="this.className='SBpodTitleBarOMOver';"  onmouseout="this.className = 'SBpodTitleBarOMOut'; "><img src="images/logout.png" border="0" alt="Logout" /><br />Log Out</a>
        <asp:Button runat="server" ID="btnLogOut" Text="" style="visibility:hidden;" OnClick="btnLogOut_Click" /></td>

    <%
     }
     else
     {
      %>
            
<%--      <td style="width:75px;height:80px;font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;">
       <a name onclick="javascript:ShowMyAccountModalPopupEvt('BACK|-1|BACK|||');"><img src="images/welcome_icon.gif" border="0" alt="Welcome to the NEW JPE Smart Buyer" onmouseover="this.className='SBpodTitleBarOMOver'; javascript: showTooltipOnOff(event,'<div style=\'text-align:left;\'>If you were a registered user on our old website, please tell us your name, company, email and phone, and your sales rep will do the rest.</div>')"  onmouseout="this.className = 'SBpodTitleBarOMOut';" /><br />Returning Users</a>
      
      </td>--%>
      <td style="width:300px;height:80px;font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;"></td>
      <td style="font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;width:75px;">
       <a name onclick="javascript:ShowMyAccountModalPopupEvt('NONE|-1|NEW|||');"><img src="images/new-account.jpg" border="0" alt="Create An Account on JPE Smart Buyer" onmouseover="this.className='SBpodTitleBarOMOver'; javascript: showTooltipOnOff(event,'<div style=\'text-align:left;\'>Click this button to obtain credentials for your company to use the Jet Parts Engineering Smart Buyer on-line system.</div>')"  onmouseout="this.className = 'SBpodTitleBarOMOut';" /><br />Create an Account</a>
       </td>
      <%
     }
       %>
     
     <td style="width:100px;">&nbsp;</td>
     
    <%-- 
    
 <td style="font:verdana;color:#999999;font-size:9px;margin: 5px auto; text-align:center;width:75px;">
   <a name onclick="javascript: return doButtonClick('btnShowHelp')" onmouseover="this.className='SBpodTitleBarOMOver';"  onmouseout="this.className = 'SBpodTitleBarOMOut';"><img src="images/help.gif" border="0" alt="Open Help Panel" /><br />Help</a>
   
   </td>
--%>

 </tr></table>

        </ContentTemplate>
        <Triggers>
              
        </Triggers>
        </asp:UpdatePanel>
     	
				</td>
			</tr>
			<tr id="slogan_stripe">
				<td colspan="2" style="background-color: #242E6a;" height="18px">
					<img src="images/header_sloganblue.gif" style="border: 0px; margin: 0px;"/>
				</td>
			</tr>
		</table>
	</div>


	<div id="main" style="border-bottom:solid 2px;border-top:solid 2px;border-left:solid 2px;border-right:solid 2px;">
	
		<table border="0" width="758" id="layout_grid" style="border-collapse: collapse" cellpadding="0">
<!-- ROW 01: -->
			<tr id="menu">
<!-- NAV: GLOBAL -->
				<td id="awmAnchor-jpe_menu02" bgcolor="#666666" colspan="7" width="758">
				</td>
			</tr>
<!-- ROW 02: -->
			<tr id="focus"> 
				<td rowspan="8" width="1" id="left_nav" style="vertical-align:top;">
				</td>
				<td class="border"width="1"></td>
<!-- INTRO -->
				<td  colspan="5" width="758" >
 <center>

<div class='ecomDiv' >
    
    <!-- JPE Smart Buyer Main Update Panel - wraps other layer 1 panels  -->			 
<asp:UpdatePanel ID="upPnlSmartPartsBuyer" runat="server" UpdateMode="Conditional" >
<ContentTemplate>

 <table><tr><td><img src="images/spb7.png" border = "0" alt="Jet Parts Engineering's Smart Buyer B2B eCommerce System for your FAA-PMA needs." /></td>

<td style="text-align:right; margin-right:5px auto;vertical-align:top; font-family:Verdana;font-size:9px;font-weight:600;color:#0B2B75;">
 <% 
     if ((int)Page.Session["LoggedInUserID"] > 0)
     {
     %>
     Welcome Back<br /> <% =Page.Session["RDX_CONTACT_NAME"]%>
     
     <% if ((int)Page.Session["DEMO_CMP_AUTO_KEY"] > 0) 
        {%>
       <br /> Demo As: <% =Page.Session["Company"] %>

    <% } 
     }

     else
     {
      %>
      
      <!-- img src="images/iPadTop.png" border = "0" alt="Signup for JPE Smart Buyer by May 7th and WIN a FREE iPad!" / -->
      
      
      
      <% }  %>
      
      
      
      
      
      
      
     <br /><br /><br />
     <asp:Panel ID="pnlSearchMore" runat="server" visible = "false" >
  
     
     <a name="" onclick="javascript: return controlAndFocus('lblsearchTitle', 'txtSearch_PartNo_1');">
     <img height="27" width="32" src="images/sm3.gif" alt = ""/><br />
     Search</a>
     <br />
    </asp:Panel>
     
     
     

     
     
 </td>
 
 </tr></table>
 </p></p>
 
 
 
 
 
 <asp:Label ID="lblOrderPlaced" runat = "server" />

 
 
 
 
 <asp:panel ID="pnlJPECompanyDDL" runat = "server" >
 
 <div style="padding-top:10px; padding-bottom:20px; text-align:right; margin-right:10px auto;">
 


 <table style="width:600px;">
 

<tr><td rowspan = '2' style='width:300px;vertical-align:top;'>


<asp:Label runat="server" ID="lblCoTotals" />

<asp:Label runat="server" ID="lblCurrentCompanyUsage" />





</td>
 
 
 
 <td style='width:250px;vertical-align:top;'> <asp:DropDownList ID="ddlCompaniesforSalesperson" runat="server" style="font-size:10px;"  OnSelectedIndexChanged = "JPEonChangeCompanyDDL"  AutoPostBack="true" />
 
</td></tr>
<tr><td style="color:#999999; font-size:10px;text-align:left;vertical-align:top;">

 <asp:Label runat="server" ID="lclCurrentCompany" />
 
</td></tr>
</table>

 
 </div>
 
 </asp:Panel>
 
 
  
  
  
  
 <asp:panel ID="pnlSmartStock" runat = "server">
 <div style="padding-top:10px; padding-bottom:20px; text-align:right; margin-right:10px auto;">
 <table style="width:30%;">
 <tr><td>
  <asp:Label runat="server" ID="lblSmartStock" />
</td></tr>
<%--<tr><td style="color:#999999; font-size:10px;text-align:left;";>
</td></tr--%>
</table>
 </div>
 </asp:Panel>
  
  
   <asp:Panel runat="server" ID="pnlWebsiteDown" >
 
<div class="divwebsiteDown"><center></center><asp:Label runat="server" ID="lclWebsiteDown" /></center></div>
 
 </asp:Panel>
 
 
  
  
  
  
  
  
  
  
	<!-- LOGIN PANEL -->
	
<asp:Panel ID="pnlLogin" runat = "server" >	
<center>   
     <table class='SBecomTable' style="width:400px;" >
     <tr><td class='SBHeaderBarTD' colspan='3' style="height:2px;">
     </td></tr>
    
     <tr><td class='SBViewPortBorderTD' style="width:1px;"></td>
     <td class='SBViewPortContentTD' >
     
     <!-- Begin Table Viewport Area -->
              
	<table class='GenViewPortTable'>
	<tr><td>
				<h3 style="color:#0B2B75;">Login</h3 >
				
			<table class="ecomLoginTable">
			    <tr> 
			    <td class= "login_titles" style="white-space:nowrap;"><asp:Label id="lblUserName" name="lblUserName" Text="eMail Address" runat="server" />&nbsp;&nbsp;</td>
			    <td class = "login_controls">
			    
			     <table><tr><td WIDTH="200px;">
                <asp:TextBox ID="txtLoginUserName" runat="server"
                    Width = "200px"
                    CssClass = "ecomTextBoxes"
                    TabIndex="1"
                    MaxLength = "50"  />
                    </td>
                    <td>
                    
                 
                    
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                    ControlToValidate="txtLoginUserName"
                    ValidationGroup="vgLogin"
                    Display="Dynamic"
                    SetFocusOnError="True"
                    TabIndex = "1"
                    Text="Please Enter Your Email Address"
                    ErrorMessage="Please Enter Your Email Address" />
                    
                  </td></tr></table>
			    
			    </td>
			    </tr>
			    
			    
			    
			     <tr> 
			    <td class= "login_titles">&nbsp;</td>
			   
			    <td style="width:100px;white-space:nowrap;">
                   
                   <asp:panel runat="server" ID="pnlRememberMe" style="white-space:nowrap;"><asp:CheckBox runat="server" ID="cbRememberMe" />Remember My Email Address</asp:Panel></td><td>
                    
			    </td>
			    
			    </tr>
			    
			    
			    
			    
			    <tr> 
			    
			    
			    <td class= "login_titles"><asp:Label ID="lblPassword" name="lblPassword" Text="Password" runat="server" /></td>
			    <td class = "login_controls"> 
			    
			    <table><tr><td WIDTH="200px;">
			    <asp:TextBox ID="txtLoginPassword" runat="server"
                    Width = "200px"
                    TextMode = "Password"
                    TabIndex = "2"
                    OnKeyUp="submitOnEnter(event, 'btnLogin');"
                    CssClass = "ecomTextBoxes"
                    MaxLength = "30"  /></td><td style="width:100px;"><span id="divMayus" style="visibility:hidden;"  class="spanCapsLockOn"><!-- img src="images/Caps-Lock-On_icon.gif" border = "0" alt="Your caps lock is on." / -->CAPS LOCK ON</span></td><td>
                 
                    
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                    ControlToValidate="txtLoginPassword"
                    ValidationGroup="vgLogin"
                    Display="Dynamic"
                    SetFocusOnError="True"
                    Text="Please Enter Your Password"
                    ErrorMessage="Please Enter Your Password" />
                    
                    </td></tr></table>
                    
                    
                    </td>
			    </tr>
			    
			     
			     <tr> 
			    <td class= "login_titles">&nbsp;</td>
			   
			    <td style="width:100px;white-space:nowrap;">
               </td><td>
                    
			    </td>
			    
			    </tr>
			    
			    
			    
			    
			 </table>

			<table><tr><td><a name onclick ="javascript: doButtonClick('btnForgottenPassword');" onmouseover="this.className='SBpodTitleBarOMOver';"  onmouseout="this.className = 'SBpodTitleBarOMOut';"><img src="images/forgotten.gif" border = "0" alt="Forgotten username or password? Click Here." /></a></td><td><a name onclick ="javascript: doButtonClick('btnForgottenPassword');" onmouseover="this.className='SBpodTitleBarOMOver';"  onmouseout="this.className = 'SBpodTitleBarOMOut';">Forget your password?  No Problem.</a></td><td> <asp:Button ID="btnLogin" TabIndex = "3" runat="server" Text="Login" onclick="btnLogin_Click"   /> </td></tr></table>
			
			
            
            <asp:Label ID="lblLoginError" runat="server" Text="" CssClass="modalMessages" />
            
         

		</td></tr>
		</table>
     <!-- End  Table Viewport Area  -->
     </td><td class='SBViewPortBorderTD' style="width:1px;"></td></tr>
     <tr><td class='SBViewPortFooterTD' colspan = '3' style="height:2px;"></td></tr>
     </table>
     </center>

     <br /><br />
     

        <br />
        
        <p class="ecom_Headline">Welcome to the New JPE Smart Buyer eCommerce System</p>
        
        <br />
     
        <p class="ecom_BodyText">The NEW <b>JPE Smart Buyer</b> business-to-business eCommerce website uses an exciting new concept to empower Jet Parts Engineering's customers to become active participants in the purchasing and fulfillment process.  <b>JPE Smart Buyer</b> directly interfaces with our in-house production and inventory management system to provide you with real-time information about your orders.</p>
        
        <br />
        
        <p class="ecom_BodyText"  style="font-size:14px;"><b>New Users</b></p>
       
        <p class="ecom_BodyText">Please click the <a name onclick="javascript:ShowMyAccountModalPopupEvt('NONE|-1|NEW|||');" onmouseover="this.style.color = 'red'; this.style.cursor = 'hand';" onmouseout="this.style.color = '#666666' ; this.style.cursor='default';"><b><u>Create an Account</u></b></a> icon at the top of this page to fill out a New User sign-up form.
         </p>
     
         <br />
     
       <table><tr><td>
         <p class="ecom_BodyText"  style="font-size:14px;"><b>Returning Users</b></p>
       
         <p class="ecom_BodyText">If you have used our old parts lookup website, please click the  <a name onclick="javascript:ShowMyAccountModalPopupEvt('BACK|-1|BACK|||');" onmouseover="this.style.color = 'red'; this.style.cursor = 'hand';" onmouseout="this.style.color = '#666666' ;this.style.cursor = 'default';"><b><u>Returning Users</u></b></a> icon at the top of this page to fill out the 'Returning User' sign-up form.
         </p>
         
         <br />
         <br />
     
              <p class="ecom_BodyText">Once you submit your sign-up form, your Jet Parts Engineering sales rep will verify your account information and our IT folks will activate your JPE Smart Buyer account. Your account will be activated in 1-2 business days [please allow additional time during the MRO show due to the anticipated volume of new sign ups] and we will send you a confirmation email when your account is activated.</p>
 <br />
 <br />
      
     
      </td><td><!-- a href="" / --><img src="images/focus05.jpg" border = "0" alt=""  hspace="10" /><!-- /a --></td></tr></table>

    
       <p class="ecom_Headline">About JPE Smart Buyer</font></p>
        <br />
       
              <p class="ecom_BodyText"><b>JPE Smart Buyer</b> is ready to help streamline your FAA-PMA parts ordering procedures today. <b>Getting started is easy.</b>  Use the link at the top of this page and complete the form.  Your JPE sales rep will activate your account and notify you as soon as you are set up.</b></p>
       
        <br />
       
         <p class="ecom_BodyText">You know you can count on <b>Jet Parts Engineering</b> to exceed your expectations in parts reliability and quality customer service. Now you can count on JPE to use the latest information management technologies to empower you to better manage your supply chain needs.
         </p>
         <br />
         <p><b>Sign up for JPE Smart Buyer today!</b></p>
       
       
        <br />
       

	
	     

     
          <div style="width:580px;text-align:right;">
     <table border="0" style="width:360px;">
     <tr><td><a href="http://www.pmamarpa.com/index.html" target = "_new" border="0"><img src="images/marpalogo.jpg" border="0"/></a></td>
     <td><a href="http://www.asa2fly.com/" target = "_new" border="0"><img src="images/V2AsaLogoSm.gif" border="0" /></a></td>
    <td style="padding-left:10px;"><a href="http://quegroup.camp7.org/" target = "_new" border="0"><img src="images/que_member_icon.gif" border="0" /></a></td>
   </tr></table>
   </div>


     
     <asp:Label ID="lblHomePageHelpPanel" runat="server" />
     
</asp:Panel> 
     
     <!-- End of Login Panel -->
    
 

 
 
 
 
 
 
 
 
 
 

<asp:Panel ID="pnlCheckoutCartApproved" runat="server">
<table class='ecomTable'>
<tr>
    <td class='SBHeaderBarTD' colspan='3'>
    <table><tr><td class='SBHeaderBarTD'>
        Cart
        
        
        <td style="white-space:nowrap;font-family:Verdana; font-size:9px; color:#666666; width:50%;text-align:right;">Use hints&nbsp;<input type ='checkbox' name='cbUseHints' id='cbUseHints' <% =Session["useHints"].ToString() %> onClick = "javascript: return useHintsChanged();" />
        
        </td></tr></table>
        
        
    </td>
</tr>
<tr>
    <td class='SBViewPortBorderTD'>
    </td>
    <td class='GenViewPortContentTD'>
        <!-- Begin Table Viewport Area -->
       
        <table class='GenViewPortTable'>
            <tr>
                <td>
                   <asp:Label runat="server" ID="lblCheckoutCart" />
                </td>
            </tr>
        </table>
        <!-- End  Table Viewport Area  -->
    </td>
    <td class='SBViewPortBorderTD'>
    
    </td>
</tr>
<tr>
    <td class='SBViewPortFooterTD' colspan='3' style="height:5px;">
        
    </td>
</tr>
</table>
<br />

</asp:Panel>


<asp:Panel ID="pnlEmtpyCartMessage" runat = "server" Visible="false"  >

    <table>
    <tr><td class="ecom_Headline" style="color:#999999;white-space:nowrap;">
    Ready to get started?
     </td><td style="white-space:nowrap;font-family:Verdana; font-size:9px; color:#666666; width:50%;text-align:right;"><%--Show this Introduction&nbsp;<input type ='checkbox' name='cbUseHints' id='cbUseHints' <% =Session["useHints"].ToString() %> onClick = "javascript: return useHintsChanged();"> --%>
     
     </td></tr>
     <tr><td colspan = "2">
     <p>
     <ul style="font-family:Verdana; font-size:11px; color:#666666;">
     <li><b>Let's go.</b>  Enter the OEM part numbers or partial part numbers and the quantity you are looking for in the text boxes to the left, or copy and paste a list of up to <% =intlimitExcelParts %> parts into the text area to the right, then click <b>Search</b>.</li>
     <li><b>JPE Smart Buyer</b> will search for parts that match your needs and allow you to view other parts often used when servicing the assembly each part is associated with.</li>
      <li>As you navigate the system you can change the 'quantity ordered' for any part at any time.  Please be sure to click one of the <b>save</b> buttons when you make changes to one or more quantity fields before you navigate away from the page to update your shopping cart.</li>
     <li>The small <b>shopping cart</b> that appears at the top of this panel shows the items currently in your cart.  The <b>JPE Smart Buyer</b> panel allows you to navigate parts and associated parts.  You can search for new parts by entering more part numbers in the <b>search panel</b> on the lower part of this screen</b></li>
     <li><b>Can we help?</b>  If you have questions about our new eCommerce system, or about PMA parts, please contact your sales rep; contact information is listed at the bottom of this panel.</li>
     
     </p>
     
     </td></tr></table>

</asp:Panel>


      <asp:Label ID="lblSteveDebug" runat="server" Text="" />
      

<asp:Panel ID="pnlSearchVideoGame" runat="server" >
    
         <asp:PlaceHolder runat="server" ID="phSmartBuyerPanel" />
         
<br /> <br />
    <%-- <asp:PlaceHolder runat="server" ID="phSmartBuyerPanelDemo" />
--%>   
    <asp:Label runat="server" ID="lblControlList" />
</asp:Panel>




<asp:Panel ID="pnlShippingandPayment" runat="server">
<table class='SBecomTable'>
<tr>
<td class='SBHeaderBarTD' colspan='3'>
<%  if ((bool)Page.Session["JPEapprovedForPricing"])
    { %>
     Shipping and Payment Panel
    
    <% } else { %>
      Send Request for Quote
   <%  } %>
  
</td>
</tr>
<tr>
<td class='SBViewPortBorderTD'>
</td>
<td class='SBViewPortContentTD'>



    <asp:UpdatePanel ID="upShippingError" runat="server" UpdateMode="Conditional"     >
    <ContentTemplate>
    <asp:Label ID="lblShippingError" runat = "server"  />
    </ContentTemplate>   
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnPlaceOrder" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>  

    <!-- Begin Table Viewport Area -->
    <table class='GenViewPortTable'>
        <tr>
            <td>
                <table class='checkoutTable' style='width:538px;'>
                    
                    
                    
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    
                    
                    
                    <tr>
                        <td class='checkoutSectionRowTD' colspan='2'>
                        <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                        { %>
                         Purchase Order Information
                        
                        <% } else { %>
                          Customer Reference Information
                       <%  } %>
                           
                        </td>
                    </tr>
                    
                    
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            
                        <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                        { %>
                         PO Number
                        
                        <% } else { %>
                          Customer Reference No
                       <%  } %>
                        </td>
                        <td class='checkoutRowValueTD'>
                        
                        
                        
                        
                        <table>
                        
                       
                        <tr>
                       
                            
                        <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                        { %>
                         <td class="checkoutThisorThatInstrTD" style="color:#990000;">
                         Enter your PO Number [required]
                        
                        <% } else { %>
                         <td class="checkoutThisorThatInstrTD" >
                          Enter your Reference Number [optional]
                       <%  } %>
                       
                        
                        </td>
                        <td class="checkoutThisorThatDataTD"> 
                        
                          
                            <asp:TextBox ID='txtPONumber' runat='server' class='txtBoxControls' MaxLength='20' Style="width: 100px;" />
                        </td>
                        </tr>
                        
                          <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                        <tr >
                        <td class="checkoutThisorThatInstrTD" >
                             
                        <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                        { %>
                         Upload a copy of your PO [optional]
                        
                        <% } else { %>
                          Upload a copy of your Specifications [optional]
                       <%  } %>
                       
                        
                        
                        </td>
                        <td class="checkoutThisorThatDataAltTD" > 
                        
                          <ASP:FileUpload ID="fileUploadPO"  name="fileUploadPO"  runat="server" />
                        </td>
                        </tr>
                      
                        
                        </table>
                       
                            
                        </td>
                    </tr>
                    
                    
                    
                    
                    
                    
                    
                    
                    
                      <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class='checkoutSectionRowTD' colspan='2'>
                            Delivery Date
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Customer Required Date
                        </td>
                        <td class='checkoutRowValueTD'>
                        
                        
                        
                        
                        <table>
                        
                       
                        <tr>
                        <td class="checkoutThisorThatInstrTD">Indicate the target delivery date if appropriate</td>
                        <td class="checkoutThisorThatDataTD"> 
                        
                          <asp:TextBox ID="txtFutureShipCalendar" runat="server" />
                            <asp:ImageButton ID="imgBtnFutureCalendar" runat="server" ImageUrl="images/smallCalendar.png" />
                            <cc1:CalendarExtender ID="celExtFuturDate" runat="server" TargetControlID="txtFutureShipCalendar"
                                Format="MMMM d, yyyy" PopupButtonID="imgBtnFutureCalendar" PopupPosition="Right" />
                        
                        </td>
                        </tr>
                      
                        
                        </table>
                        
                      
                     
                           
                        </td>
                    </tr>
                    
       
                           <asp:Panel ID="pnlShippingInstructions" runat="server" Visible = "false"> 
      
                    
                   
     

                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutSectionRowTD' colspan='2'>
                            Shipping Instructions
                        </td>
                    </tr>
                    
     
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    
                    
                    
                    
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Shipping Carrier Information for this Delivery
                        </td>
                        <td class='checkoutRowValueTD'>
                        
                        
                        
                        
                        
                        <table>
                        
                        
                        <asp:Panel ID="pnlMyShipperDDL" runat="server"  >
                        <tr>
                        <td class="checkoutThisorThatInstrTD">Select a Shipper ID from your profile</td>
                        <td class="checkoutThisorThatDataTD"> 
                        
                         <asp:DropDownList ID="ddlMyShipperID" runat="server" style="font-size:10px;"  />
                        
                        </td>
                        </tr>
                        </asp:Panel>
                        
                        <tr><td colspan = "2"  class="checkoutORTD" style="color:#990000;">- OR -</td></tr>
                        
                        <tr>
                        <td class="checkoutThisorThatInstrTD" >Enter New shipping information</td>
                        <td class="checkoutThisorThatDataTD"> 
                        <table border="0">
                        <tr><td><asp:DropDownList ID="ddlShipperCarrier" runat="server"  /></td><td>Select Shipping Carrier</td></tr>
                        <tr class="checkoutThisorThatDataAltTD"><td> <asp:TextBox ID='txtNewShipperID' runat='server' class='txtBoxControls' MaxLength='20' /></td><td>Enter your Shipper ID</td></tr>
                        <tr><td > <asp:TextBox ID='txtNewShipperServicetoUse' runat='server' class='txtBoxControls' MaxLength='20' /></td><td>Indicate the type of service [i.e. standard, overnight]</td></tr>
                         <tr class="checkoutThisorThatDataAltTD"><td ><asp:CheckBox ID='cbSaveNewShipperID' runat='server' /></td><td>Check to Save this to your profile</td></tr>
                        
                        </table>
                        
                      
                        </td>
                        </tr>
                        </table>
                        
                        
                        
                        
                        </td>
                    </tr>
                    
                    
                    
                    
                 
                    
                       
                    
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Ship To Address for this Order
                           
                            
                        </td>
                        <td class='checkoutRowValueTD'>
                        
                        
                     
                        
                        <table>
                        
                        
                        <asp:Panel ID="pnlMyShipAddressDDL" runat="server" >
                        <tr>
                        <td class="checkoutThisorThatInstrTD">Select Ship To Address</td>
                        <td class="checkoutThisorThatDataTD"> 
                        
                        <asp:DropDownList ID="ddlMyShipToAddress" runat="server" style="font-size:10px;"  />
                        
                        </td>
                        </tr>
                        </asp:Panel>
                        
                        <tr><td colspan = "2"  class="checkoutORTD" style="color:#990000;">- OR -</td></tr>
                        
                        <tr>
                        <td class="checkoutThisorThatInstrTD" >Enter New shipping information</td>
                        <td class="checkoutThisorThatDataTD"> 
                        
                        <table border="0">
                        <tr><td><asp:TextBox ID='txtNewShipCompanyName' runat='server' class='txtBoxControls' MaxLength='50' Style="width: 200px;" /></td><td>Company Name</td></tr>
                        
                        
                        
                        
                        <tr class="checkoutThisorThatDataAltTD"><td> <asp:TextBox ID='txtNewShipAddress1' runat='server' class='txtBoxControls' MaxLength='50' Style="width: 200px;" /></td><td>Address 1</td></tr>
                        
                        
                        
                        
                        <tr><td> <asp:TextBox ID='txtNewShipAddress2' runat='server' class='txtBoxControls' MaxLength='50' Style="width: 200px;" /></td><td>Address 2</td></tr>
                        
                      
                       
                        <tr><td> <asp:TextBox ID='txtNewShipAddress3' runat='server' class='txtBoxControls' MaxLength='50' Style="width: 200px;" /></td><td>Address 3</td></tr>
                        
                        
                         <tr class="checkoutThisorThatDataAltTD"><td><asp:TextBox ID='txtNewShipCity' runat='server' class='txtBoxControls' MaxLength='20' Style="width: 150px;" /></td><td>City</td></tr>
                        
                         <tr><td > <asp:TextBox ID='txtNewShipState' runat='server' class='txtBoxControls' MaxLength='20' Style="width: 100px;" /></td><td>State</td></tr>
                         
                          <tr class="checkoutThisorThatDataAltTD"><td><asp:TextBox ID='txtNewShipCountry' runat='server' class='txtBoxControls' MaxLength='20' Style="width: 100px;" /></td><td>Country</td></tr>
                          
                            <tr><td ><asp:TextBox ID='txtNewShipZip' runat='server' class='txtBoxControls' MaxLength='50' Style="width: 100px;" /></td><td>Postal Code</td></tr>
                        
                        
                         <tr class="checkoutThisorThatDataAltTD"><td ><asp:CheckBox ID='cbSaveNewShipToAddress' runat='server' /></td><td>Check to Save this to your profile</td></tr>
                        
                        </table>
                        
                        
                      
                        </td>
                        </tr>
                        </table>
                           
                        
                        
                        
                        
                        
                            
                            
                            
                        </td>
                    </tr>
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                         
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutSectionRowTD' colspan='2'>
                            Payment Information
                        </td>
                    </tr>
                    
                    
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    <tr>
                        <td class='checkoutSectionRowTD' colspan='2'>
                        
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutRowLabelTD' style="color:#990000;">
                            Payment Method
                        </td>
                        <td class='checkoutRowValueTD' >
                        <table><tr><td style="vertical-align:top;">
                            <asp:DropDownList ID="ddlCardType" runat="server" />
                            </td>
                            <td>
                            
                            <%
                                if (pnlCreditCardInfo.Visible)
                                {
                                 %>
                           <img src="images/credit-cards-fan.gif" border="0" alt="Credit Cards Accepted" />
            <%
                                }
                                else
                                {
                 %>
                
                <b>Terms:  </b><% = Page.Session["TERM_DESCRIPTION"].ToString()%>
                <% }  %>
                
                            </td>
                            
                            </tr></table>
                        </td>
                    </tr>
                    
                    
                    
                                        
                    <asp:Panel ID="pnlCreditCardInfo" runat="server"  visible="false" >

                    
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        
                            
                            <asp:HiddenField ID="hdnCCRequired" runat="server" Value="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Name on Card
                        </td>
                        <td class='checkoutRowValueTD, checkoutThisorThatDataAltTD'><asp:TextBox ID="txtCardName" runat="server" MaxLength="50" CssClass="txtBoxControls"
                                Style="width: 200px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    
                    
                    
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Card Number
                        </td>
                        <td class='checkoutRowValueTD'><asp:TextBox ID="txtCardNumber" runat="server" MaxLength="20" CssClass="txtBoxControls" Style="width: 200px;" />
                        
                            
                        </td>
                    </tr>
                    
                      <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                     
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Credit Card Billing Address
                        </td>
                        <td class='checkoutRowValueTD, checkoutThisorThatDataAltTD'>
                        
                         <table>
                     <%
                         if (Page.Session["BILL_TO_STRING"] != null)
                         {
                             if (Page.Session["BILL_TO_STRING"].ToString().Trim() != string.Empty)
                             {
                          %>
                      
                      <tr><td style="vertical-align:top;">
                        <asp:CheckBox runat = "server" ID="cbUsesBillingAddress" /> Use: </td><td><% =Page.Session["BILL_TO_STRING"]%>
                        
                        
                        <asp:HiddenField runat="server" ID="hdn_txtCardAddress1" />
                        <asp:HiddenField runat="server" ID="hdn_txtCardAddress2" />
                        <asp:HiddenField runat="server" ID="hdn_txtCardCity" />
                        <asp:HiddenField runat="server" ID="hdn_txtCardState" />
                        <asp:HiddenField runat="server" ID="hdn_txtCardZip" />

                        
                        </td></tr>
                        
                    

                        
                        
                             <%  
                            }
                         }
                         %>
                    
                        <tr><td>Address</td><td class = "checkoutThisorThatDataAltTD">  <asp:TextBox ID="txtCardAddress1" runat="server" MaxLength="50" CssClass="txtBoxControls" Style="width: 200px;" />
                        </td></tr>
                        
                          
                        <tr><td></td><td>  <asp:TextBox ID="txtCardAddress2" runat="server" MaxLength="50" CssClass="txtBoxControls" Style="width: 200px;" />
                        </td></tr>
                       
                          <tr><td>City</td><td class = "checkoutThisorThatDataAltTD">  <asp:TextBox ID="txtCardCity" runat="server" MaxLength="20" CssClass="txtBoxControls" Style="width: 100px;" /> &nbsp; <asp:TextBox ID="txtCardState" runat="server" MaxLength="2" CssClass="txtBoxControls" Style="width: 30px;" /> &nbsp; <asp:TextBox ID="txtCardZip" runat="server" MaxLength="15" CssClass="txtBoxControls" Style="width: 50px;" />
                        </td></tr>
                       
                        </table>
                      
                        
                            
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    
                    
                    
                    
                    <tr>
                        <td class='checkoutRowLabelTD'>
                            Expiration Date
                        </td>
                        <td class='checkoutRowValueTD'>
                             
                        <asp:TextBox ID="txtExpireMonth" runat="server" MaxLength="2" CssClass="txtBoxControls" Style="width:50px;" />
                           
                        <asp:TextBox ID="txtExpireYear" runat="server" MaxLength="4" CssClass="txtBoxControls" Style="width:50px;" />
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutRowLabelTD, checkoutThisorThatDataAltTD'>
                            CVV Code
                        </td>
                        <td class='checkoutRowValueTD'>
                               <asp:TextBox ID="txtCVVCode" runat="server" MaxLength="4" CssClass="txtBoxControls" Style="width:50px;" />
                           
                            [VISA / MC: 3 number code on the back of the card; AMEX: the 4 number code on the front]
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    </ASP:Panel>
           </ASP:Panel>
                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutSectionRowTD' colspan='2'>
                         <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                            { %>
                             Agreement
                            
                            <% } else { %>
                              Send Request to JPE
                           <%  } %>
                            
                        </td>
                    </tr>

                    <tr>
                        <td colspan='2' class='checkoutRowBorder'>
                        </td>
                    </tr>
                    <tr>
                        <td class='checkoutRowLabelTD'>
                        </td>
                        <td class='checkoutRowValueTD'>
                        <table><tr><td colspan="2">
                           <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                               { %>
                            
                            I understand that by clicking the 'Place Order' button I am agreeing to the terms of this order as stated above. If I have not entered my shipper ID, I understand that Jet Parts Engineering will also charge carrier shipping rates. If I am paying by credit card, I understand that shipping will be charged to my credit card. 
                            <% } %>
                            </td></tr>
                            <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                               { %>
                            <tr><td class="checkoutThisorThatDataAltTD" style="padding-top:10px; padding-bottom:5px;color:#990000;" colspan="2"> 
                             <asp:CheckBox runat = "server" ID="cbIAgreetoTOS" />&nbsp;I understand the terms outlined above.
                            </td></tr>
                         <% } %>
                            <tr><td>
                             <%  if ((bool)Page.Session["JPEapprovedForPricing"])
                                 { %>
                            <img src="images/global_delivery_icon.jpg" border="0" alt="Global Delivery" />
                            <% } else { %>
                            <img src="images/rfq.jpg" border="0" alt="Global Delivery" />
                            <% } %>
                            </td>
                            <td style="text-align: right; margin-right: 0px auto; padding-right: 10px.">
                             <asp:Button ID="btnPlaceOrder" TabIndex = "3" runat="server" Text="Place Order" onclick="btnPlaceOrder_Click"   />  
                            </td>
                            </table>
                          
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    

    <!-- End  Table Viewport Area  -->
    

    
</td>
<td class='SBViewPortBorderTD'>
</td>
</tr>
<tr>
<td class='SBViewPortFooterTD' colspan='3'>
    &nbsp;
</td>
</tr>
</table>
<br />
<br />
</asp:Panel>

<asp:Panel ID="pnlSearchForParts" runat = "server" >
<table class='SBecomTable'>
<tr>
    <td class='SBHeaderBarTD' colspan='3'>
        <asp:Label ID="lblsearchTitle" runat="server" />
    </td>
</tr>
<tr>
    <td class='SBViewPortBorderTD'>
    </td>
    <td class='SBViewPortContentTD'>
        <!-- Begin Table Viewport Area -->
        <table class='GenViewPortTable'>
            <tr>
                <td class='SearchPNLeftTD'>
                    <table class='SearchPNLeftTable'>
                        <tr>
                            <td class='SearchLineNumbers'>
                            </td>
                </td>
                <td class='SearchColHeaders'>
                    Part Number
                </td>
                <td class='SearchColHeaders'>
                    Qty <% if (blnRequireSearchQty)
                           { %><span style="color:Red;">*</span><% } %>
                </td>
            </tr>
            <tr>
                <td class='SearchLineNosTD'>
                    1
                </td>
                <td class='SearchPNcontrolTD'>
                    <input type='text' name='txtSearch_PartNo_1' class='txtPartNoCntrl'
                        id='txtSearch_PartNo_1' maxlength='50' onkeyup="return submitOnEnter(event, 'btnPartsSearch');" value="<% = searchValues[0,0] %>">
                </td>
                <td class='SearchQTYcontrolTD'>
                    <input type='text' name='txtSearch_Qty_1' class='SearchtxtQtyCntrl' id='txtSearch_Qty_1' maxlength='5' onkeypress='return checkIt(event)' onkeyup="return submitOnEnter(event, 'btnPartsSearch');" value="<% = searchValues[0,1] %>">
                </td>
            </tr>
            <tr>
                <td class='SearchLineNosTD'>
                    2
                </td>
                <td class='SearchPNcontrolTD'>
                    <input type='type='text' name='txtSearch_PartNo_2' class='txtPartNoCntrl'
                        id='txtSearch_PartNo_2' maxlength='50' value="<% = searchValues[1,0] %>">
                </td>
                <td class='SearchQTYcontrolTD'>
                    <input type='text' name='txtSearch_Qty_2' class='SearchtxtQtyCntrl' id='txtSearch_Qty_2' maxlength='5'  onkeypress='return checkIt(event)' value="<% = searchValues[1,1] %>">
                </td>
            </tr>
            <tr>
                <td class='SearchLineNosTD'>
                    3
                </td>
                <td class='SearchPNcontrolTD'>
                    <input type='type='text' name='txtSearch_PartNo_3' class='txtPartNoCntrl'
                        id='txtSearch_PartNo_3' maxlength='50' value="<% = searchValues[2,0] %>">
                </td>
                <td class='SearchQTYcontrolTD'>
                    <input type='text' name='txtSearch_Qty_3' class='SearchtxtQtyCntrl' id='txtSearch_Qty_3'  maxlength='5' onkeypress='return checkIt(event)' value="<% = searchValues[2,1] %>">
                </td>
            </tr>
            <tr>
                <td class='SearchLineNosTD'>
                    4
                </td>
                <td class='SearchPNcontrolTD'>
                    <input type='type='text' name='txtSearch_PartNo_4' class='txtPartNoCntrl'
                        id='txtSearch_PartNo_4' maxlength='50' value="<% = searchValues[3,0] %>">
                </td>
                <td class='SearchQTYcontrolTD'>
                    <input type='text' name='txtSearch_Qty_4' class='SearchtxtQtyCntrl' id='txtSearch_Qty_4' maxlength='5' onkeypress='return checkIt(event)' value="<% = searchValues[3,1] %>">
                </td>
            </tr>
            <tr>
                <td class='SearchLineNosTD'>
                    5
                </td>
                <td class='SearchPNcontrolTD'>
                    <input type='type='text' name='txtSearch_PartNo_5' class='txtPartNoCntrl'
                        id='txtSearch_PartNo_5' maxlength='50' value="<% = searchValues[4,0] %>">
                </td>
                <td class='SearchQTYcontrolTD'>
                    <input type='text' name='txtSearch_Qty_5' class='SearchtxtQtyCntrl' id='txtSearch_Qty_5' maxlength='5' onkeypress='return checkIt(event)' value="<% = searchValues[4,1] %>">
                </td>
            </tr>
             <tr>
                <td class='SearchLineNosTD'>
                    
                </td>
                <td class='SearchPNcontrolTD' colspan = '2'>
                   <% if (blnRequireSearchQty)
                      { %><span style="color:Red;">*</span> Quantity is a required field <% } %>
                </td>
                
            </tr>

            <asp:Panel runat="server" ID="pnlTempCloseNHA" visible = "true" >

            <tr>
            <td colspan="3" style="padding-top:10px;">&nbsp;
            </td>    
            </tr>

              <tr>
                <td class='SearchLineNumbers'>
                </td>
                </td>
                <td class='SearchColHeaders' >
                   Assembly Search
                </td>
                <td class='SearchColHeaders'>
                   
                </td>
            </tr>

             <tr>
                <td class='SearchLineNosTD'> 
                </td>
                <td class='SearchPNcontrolTD'>
                    <input type='text' name='txtSearch_Assembly' class='txtPartNoCntrl'
                        ID='txtSearch_Assembly' maxlength='50'   visible='false'  value="<% = searchValues[5,0] %>"    onmouseover = "javascript: showTooltipOnOff(event,'<div style=\'text-align:left;\'>Enter an assembly number to see a list of the PMA parts that Jet Parts Engineering sells for that assembly.</div>');" >

                      <asp:HyperLink runat="server" ID="hlLastAssembly" Text="" />
                </td>
                <td class='SearchQTYcontrolTD'>
                <asp:Button ID="btnLastAssembly" runat="server" Text=""   Visible= "false"   />
                </td>
            </tr>
            </asp:Panel>
        </table>
    </td>
    <td class='SearchPNRightTD'>
        <table class='SearchPNRightTable'>
            <tr>
                <td class='excelTD'>
           
                    <textarea name="listSearchBox" id="listSearchBox" class="excelTextArea" runat="server" onmouseover = "javascript: showTooltipOnOff(event,'<div style=\'text-align:left;\'>Copy and paste a list of up to 25 parts.  You can also drop in an Excel list that has the part number in column A and the quantity desired in column B.</div>');" style="height:80px;" ></textarea>
                  
                   <br />
                    Paste your Parts list here.
                </td>
            </tr>
            <tr><td style='text-align:left; padding-left:15px;'><asp:Label ID="lblSearchQtyError" runat = "server" /></td></tr>
            
            <tr>
                <td class='searchGoTD'>
                <asp:Button ID="btnPartsSearch" runat="server" Text="Search"   OnClick="btnPartsSearch_Click"   />
                
                </td>
                </td>
            </tr>
        </table>
    </td>
</tr>
</table>
<!-- End  Table Viewport Area  -->
</td><td class='SBViewPortBorderTD'>
</td>
</tr>
<tr>
<td class='SBViewPortFooterTD' colspan='3'>
   &nbsp;
</td>
</tr>
</table>
<br />
<br />
</asp:Panel>


 <!-- Can We Help You Panel -->     
<asp:Panel ID="pnlCanWeHelpYou" runat = server >
    <table class='SBecomTable'>
        <tr>
            <td class='SBHeaderBarTD' colspan='3' style="height:2px;">
            </td>
        </tr>
        <tr>
            <td class='SBViewPortBorderTD'  style="width:2px;">
            </td>
            <td class='SBViewPortContentTD'>
                <!-- Begin Table Viewport Area -->
                <asp:Label ID="lblSalesPersonTable" runat="server" />
                <!-- End  Table Viewport Area  -->
            </td>
            <td class='SBViewPortBorderTD'  style="width:2px;">
            </td>
        </tr>
        <tr>
            <td class='SBViewPortFooterTD' colspan='3'  style="height:2px;">
            </td>
        </tr>
    </table>
    </br></br>
    </asp:Panel>
    
    
 <!-- Suggestion Box Panel -->     
<asp:Panel ID="pnlSuggestionBox" runat = server >
      <% 
          if ((int)Page.Session["LoggedInUserID"] < 1)
          {
     %>
     
     <br /><br />
     <hr style="color:#cccccc;width:100%;" />
     <br /><br />
     <%
          }
          %>
     
     <table class='SBecomTable'>
        <tr>
            <td class='SBHeaderBarTD' colspan='3' style="height:2px;">
            </td>
        </tr>
        <tr>
            <td class='SBViewPortBorderTD'  style="width:2px;">
            </td>
            <td class='SBViewPortContentTD'>
          <table class=''>
          <tr>
            <td class='tdSuggestionBoxHeadline' colspan='2'>
            Suggestions / Feedback / Bug Reports
            </td>
        </tr>
        
        <tr>
        <td class='tdSuggestionBoxText'>
            Do you like what JPE Smart Buyer does? Wish it could do more? We're working on it, but we need your help too. What direction should we go next? What features would you like to see? Let us know if you have an idea for something we're not doing. We want to hear what people have to say. We need the feedback if we're going to make this thing better.</p>
                  <% 
                      if ((int)Page.Session["LoggedInUserID"] < 1)
                      {
     %>
     In case we have any questions and if it would be convenient for us to contact you, please please provide your name and either your email address or phone number.</p>
     
     <%
                      }
          %>
            
            </td>
            <td class='tdSuggestionBoxImage' style="text-align:center;">
            <img src='images/suggestionbox.jpg' alt='suggestion box' />
            </td>
        </tr>
        
      <% 
          if ((int)Page.Session["LoggedInUserID"] < 1)
          {
     %>

     <tr><td colspan = "2" style="padding-top:10px;padding-bottom:10px;">
     <table>
            <td class="tdSuggestionBoxNameTitle">Your Name [optional]</td>
            <td class="tdSuggestionBoxNamecontrol"><input type="text" id="txtSuggestionName" name="txtSuggestionName" class="txtSuggestionNameControl" /></td>
        </tr>
         <tr>
            <td class="tdSuggestionBoxNameTitle">Your Email and/or Phone [optional]</td>
            <td class="tdSuggestionBoxNamecontrol"><input type="text" id="txtEmailAddress"  name="txtEmailAddress"  class="txtSuggestionNameControl" </td>
        </tr>
     </table>
     </td></tr>
     
     
     <%
          }
          %>
          
           <tr>
            <td class='' colspan='2'> 
            <textarea id='txtSuggestionBox' name='txtSuggestionBox' class='textareaSuggestionBox' > </textarea>
            </td>
        </tr>
        
         <tr>
            <td class='tdSuggestionBoxSubmit' colspan='2'>
            <asp:Label ID="lblSuggestionMessage" runat="server"  CssClass="spanSuggestionResult" />
                <asp:Button ID="btnSuggestionSubmit" runat="server" Text="Send Suggestion"  OnClick="btnSuggestionSubmit_Click" />
            </td>
        </tr>
            </table>
        </td>
            <td class='SBViewPortBorderTD'  style="width:2px;">
            </td>
        </tr>
        <tr>
            <td class='SBViewPortFooterTD' colspan='3'  style="height:2px;">
            </td>
        </tr>
    </table>

</br>
</asp:Panel>
    
    
 
<asp:Panel runat = "server" ID = "pnlAdminControls" visile = "False" >
<asp:Label runat="server" ID="lblAdminTable" />
	<br />
	
	
	<%
	    if (Page.Session["AccountType"].ToString().ToLower() == "admin")
    { 
    %>

<table border = "1" class="adminTable" style="width:50%;">
<tr><td class="adminHeadertd" colspan = "2">Sales Report</td></tr>

<tr>
<td class="adminBodytd" >Date || From Date</td>
<td  class="adminBodytd"><asp:TextBox runat = "server" ID = "txtSRPFromDate"  CssClass="ordTxtDates" />
      
    <asp:ImageButton ID="ibSRPFromDate" runat="server" ImageUrl="images/smallCalendar.png" />
    <cc1:CalendarExtender ID="CalendarExtenderSRPFromDate"
         runat="server" 
         TargetControlID="txtSRPFromDate"
         Format="MM/dd/yyyy" 
         PopupButtonID="ibSRPFromDate" 
         PopupPosition="Right" />
</td>
</tr>


<tr>
<td  class="adminBodytd">To Date</td>
<td  class="adminBodytd"><asp:TextBox runat = "server" ID = "txtSRPToDate"  CssClass="ordTxtDates" />
      
    <asp:ImageButton ID="ibSRPToDate" runat="server" ImageUrl="images/smallCalendar.png" />
    <cc1:CalendarExtender ID="CalendarExtenderSRPToDate"
         runat="server" 
         TargetControlID="txtSRPToDate"
         Format="MM/dd/yyyy" 
         PopupButtonID="ibSRPToDate" 
         PopupPosition="Right" />
</td>
</tr>

<tr>
<td  class="adminBodytd">Include</td>
<td  class="adminBodytd">Companies:&nbsp;<asp:CheckBox runat="server"  ID= "cbIncludeCompanies" /> &nbsp; &nbsp; Parts&nbsp;<asp:CheckBox runat = "server" ID = "cbIncludeParts" />
</td>
</tr>

<tr>
<td  class="adminBodytd">Company Codes</td>
<td  class="adminBodytd"><asp:TextBox runat = "server" ID = "txtSRPCompanyCode"  CssClass="ordTxtDates" style="width:180px;" />
</td> 
</tr>

<tr>
<td  class="adminBodytd">Sales Rep(s)</td>
<td  class="adminBodytd"><asp:TextBox runat = "server" ID = "txtSRPSalesReps"  CssClass="ordTxtDates" style="width:180px;" />
</td>
</tr>

<tr>
<td  class="adminBodytd"></td>
<td  class="adminBodytd">

<asp:Button runat = "server" ID="btnBuildSRPReport"  OnClick = "btnBuildSRPReport_Click" Text="Build Report"   />
<br /><br />

  <asp:HyperLink runat="server" ID="hlshowSRPReport"  Text = ""  Visible = "true" Target="_New" />    
</td>
</tr>

</table>

<% }  %>


</asp:Panel>

<asp:HiddenField ID="hdnpkAccountKey" runat="server"  />
<asp:HiddenField ID="hdnUseHints" runat="server"  />

<SCRIPT LANGUAGE="JavaScript" language="javascript">
    //----------------- lets run the throbber setup for the popup onLoad
    ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);
</SCRIPT>


        <asp:Button ID="btnPartsSearchfromCart2" 
        runat="server" 
        onclick="btnPartsSearchfromCart_Click"
        style="visibility:hidden;"  />

        <asp:Button ID="btnRecalcCart2" 
        runat="server" 
        onclick="btnRecalcCart2_Click"
        style="visibility:hidden;"  />

        <asp:Button runat="server" ID="btnUpdatePartsOnQty" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnUpdateSmartBuyer_Click" />

        <asp:Button runat="server" ID="btnNHADetailPopup" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnNHADetailPopup_Click" />

        <asp:Button runat="server" ID="btnCheckAvailable" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnCheckAvailable_Click" />

        <asp:Button runat="server" ID="btnUpdatePartsOnQtyPopup" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnUpdateSmartBuyerPopup_Click" />


        <asp:Button runat="server" ID="btnUpdatePartsClosePopup" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnUpdatePartsClosePopup_Click"  />

        <asp:Button runat="server" ID="btnMTSPBPanel_HDN" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnMTSPBPanel_Click" />

        <asp:Button runat="server" ID="btnCheckoutNow_HDN" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnCheckoutNow_Click" />

        <asp:Button runat="server" ID="btnUpdateAll_HDN" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnUpdateSmartBuyer_Click" />

        <asp:HiddenField ID="hdn_SessionID"
        runat= "server"
        value= ""
        />

        <asp:HiddenField ID="hdn_MyAccountType"
        runat= "server"
        value= ""
        />

        <asp:Button runat="server" ID="btnChangeUseHints" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnChangeUseHints_Click" />

        <asp:HiddenField ID="hdn_AssemblySearch"
        value=""
        runat= "server" />  

    </ContentTemplate>
    
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnPartsSearch" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnNHADetailPopup" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnBrowserBackFwdButton" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnMTSPBPanel_HDN" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnPartsSearchfromCart2" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnUpdatePartsClosePopup" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnRecalcCart2" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnUpdatePartsOnQty" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnAccountEditPanelSave2" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnAccountEditPanelSave1" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnChangeUseHints" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnCheckoutNow_HDN" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnUpdateAll_HDN" EventName="Click" />

      </Triggers>
    </asp:UpdatePanel>
    
     <!-- Order History and Tracking Update Panel-->     

    <asp:Panel ID="mpAccountHistory" runat="server" CssClass="modalPopupAccountHistory" >       
        <asp:UpdatePanel ID="upAccountHistory" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
        
        
        	<table><tr><td><img src="images/logo-JPE_000.gif" border="0" alt="JPE" /></td>
    <td style='padding:20px; margin-right:10px auto; text-align:right; font-size:10px; color:#999999;'>
          <asp:Button ID="btnCloseAccountHistory2" runat="server"  Text="Close History Panel" onClick = "btnCloseAccountHistory_Click"  UseSubmitBehavior="false" /> 
         </td>
	 	</tr></table>
        
         <table class='SBecomTable'>
         <tr><td class='SBHeaderBarTD' colspan='3' style="height:2px;"></td></tr>
         <tr><td class='SBViewPortBorderTD' style="width:2px;"></td>
         <td class='SBViewPortContentTD'>
         <!-- Begin Table Viewport Area -->  
         
		<table class='GenViewPortTable' style="width:400px;">
		<tr><td>	
		

        <table class='SBecomTable'>
        <tr>
        <td class='SBHeaderBarTD' colspan='3'>
            Order History
        </td>
        </tr>
        <tr>
        <td class='SBViewPortBorderTD'>
        </td>
        <td class='SBViewPortContentTD'>
            
            <!-- Begin Table Viewport Area -->
            <table class='GenViewPortTable'>
            <tr><td>
            <table class='cartTable'  style='width:650px;'>
            <tr><td colspan='11' class='cartRowSpacer'></td></tr>

             <asp:Label ID="lblOrderHistoryTable" runat="server" />
               <!-- --------------- Order detail div  ------- -->  
               <tr><td colspan='9'  class='cartNeutral'>
               <table>
               <tr>
               <td style="white-space:nowrap; padding-left:2px;padding-right:2px;font-size:10px;">PO /Part No</td>
               <td>
               <asp:TextBox ID="txtOrdersPONumber" runat="server" CssClass="ordTxtPO" />
               </td>
               <td  style="white-space:nowrap; padding-left:2px;padding-right:2px;font-size:10px;">From Date</td>
               <td  style="white-space:nowrap; padding-left:2px;padding-right:2px;font-size:10px;"> 
               <asp:TextBox ID="txtOrdersFromDate" runat="server" CssClass="ordTxtDates" />
            <asp:ImageButton ID="ibOrdersFromDate" runat="server" ImageUrl="images/smallCalendar.png" />
            <cc1:CalendarExtender ID="CalendarExtenderOrdersFromDate"
                 runat="server" 
                 TargetControlID="txtOrdersFromDate"
                 Format="MM/dd/yyyy" 
                 PopupButtonID="ibOrdersFromDate" 
                 PopupPosition="Right" />
               </td>
        <td  style="white-space:nowrap; padding-left:2px;padding-right:2px;font-size:10px;">To Date</td>
               <td  style="white-space:nowrap; padding-left:2px;padding-right:2px;font-size:10px;"> 
               <asp:TextBox ID="txtOrdersToDate" runat="server" CssClass="ordTxtDates" />
            <asp:ImageButton ID="ibOrdersToDate" runat="server" ImageUrl="images/smallCalendar.png" />
            <cc1:CalendarExtender ID="CalendarExtenderOrdersToDate"
                 runat="server" 
                 TargetControlID="txtOrdersToDate"
                 Format="MM/dd/yyyy" 
                 PopupButtonID="ibOrdersToDate" 
                 PopupPosition="Right" />
                 
               </td>
               <td  style="white-space:nowrap; padding-left:2px;padding-right:2px;font-size:10px;">
                <asp:Button ID="btnHistorySearch" runat="server" Text="Search"   onClick = "btnHistorySearch_Click"   />
               </td>
               </tr>         
               </table>                     
         </td>
                 </tr>   
               </table>
                </td>
            </tr>
            </table>
            <!-- End  Table Viewport Area  -->
        </td>
        <td class='SBViewPortBorderTD'>
        </td>
        </tr>
        <tr>
        <td class='SBViewPortFooterTD' colspan='3'>
           <asp:Label runat="server" ID = "lblOrderHxSearchError" />
        </td>
        </tr>
        </table>
                 <p>
                 
                 <table border = "1" style="width:200px;margin-top:5px;margin-left:5px;"><tr><td style="background-color:#e3e3e3;width:100px;">Sales Orders</td><td style="background-color:#E2FAD7;width:100px;">RFQ's</td></tr></table>&nbsp;&nbsp;&nbsp;
                 
                 <div style="margin-right:0px auto;text-align:right; padding-right:5px;padding-bottom:5x;padding-top:5px;white-space:nowrap;">
            <asp:Button ID="btnCloseAccountHistory" runat="server"  Text="Close History Panel" onClick = "btnCloseAccountHistory_Click"  /> </div>
                </p>
               
			</td></tr>
			</table>
         <!-- End  Table Viewport Area  -->
         </td><td class='SBViewPortBorderTD' style="width:2px;"></td></tr>
         <tr><td class='SBViewPortFooterTD' colspan = '3' style="height:2px;"></td></tr>
         </table>
         <asp:Label runat="server" ID="lblHiddenDrag7"  style="visibility:hidden"; />
          </ContentTemplate>   
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnHxSearch_HDN" EventName="Click">
          </asp:AsyncPostBackTrigger>
          </Triggers>
          </asp:UpdatePanel>         
          </asp:Panel>   
        <asp:Button runat="server" ID="btnSHowHistoryModal" Text=""\ style="visibility:hidden;"   />
        <asp:Button runat="server" ID="btnHxSearch_HDN"  Text=""  style="visibility:hidden;"  onClick = "btnHistorySearch_Click"   />
          <cc1:ModalPopupExtender ID="ModalPopupExtenderAccountHistory" runat="server"
                TargetControlID="btnSHowHistoryModal"
                PopupControlID="mpAccountHistory"
                BackgroundCssClass="modalBackground"
                RepositionMode="None"
                BehaviorID="behaveAccountHistory" 
                PopupDragHandleControlID="lblHiddenDrag7"
                DropShadow="False"
                Y = "20"
                dynamicServicePath=""
                Enabled="True" />



      <!-- Forgotten Password Modal Panel -->     
    <asp:Panel ID="mpForgottenPassword" runat="server" CssClass="modalPopupForgottenPassword" >       
        <asp:UpdatePanel ID="upForgottenPW" runat="server" UpdateMode="Conditional"     >
        <ContentTemplate>
         <table class='SBecomTable'>
         <tr><td class='SBHeaderBarTD' colspan='3' style="height:2px;"></td></tr>
         <tr><td class='SBViewPortBorderTD' style="width:2px;"></td>
         <td class='SBViewPortContentTD'>
         <!-- Begin Table Viewport Area -->  
		<table class='GenViewPortTable' style="width:400px;">
		<tr><td>	
	    <table><tr><td><h3 style="color:#0B2B75;">Password Retrieval</h3 >
	    <p>If you have misplaced your password for the <b>JPE Smart Buyer</b> system, please enter the email address currently associated with your account.</p><p><b>Your account details will be emailed to you at that email address.</p></td><td><img src="images/forgotten.gif" border = "0" alt="Forgotten" /></td></tr></table>
					<br />
				<table style="ecomLoginTable">
				    <tr> 
				    <td style= "login_titles"><asp:Label id="Label1" name="lblUserName" Text="eMail Address" runat="server" /></td>
				    <td style = "login_controls">
                    <asp:TextBox ID="txtForgotEmail" runat="server"
                        Width = "250px"
                        CssClass = "ecomTextBoxes"
                        MaxLength = "50"  />
				    </td>
				    </tr>
				    <tr> 
			
				 </table>
                 <p>
                 <div style="margin-right:0px auto;text-align:right; padding-right:5px;padding-bottom:5x;padding-top:5px;">
                   <asp:Button ID="btnForgottenCancel" runat="server"  CausesValidation= "false" Text="Cancel"   OnClick="btnForgottenCancel_Click"  /> &nbsp; <asp:Button ID="btnRecoverPassword" runat="server"  CausesValidation= "false" Text="Submit" OnClick="btnRecoverPassword_Click"   /> </div>
                </p>
                <asp:Label ID="lblPWrecoveryMsg" runat="server"  CssClass="modalMessages" />
			</td></tr>
			</table>
         <!-- End  Table Viewport Area  -->
         </td><td class='SBViewPortBorderTD' style="width:2px;"> <asp:Label runat="server" ID="lblHiddenDrag2"  style="visibility:hidden"; />
          </td></tr>
         <tr><td class='SBViewPortFooterTD' colspan = '3' style="height:2px;"></td></tr>
         </table>
        
          </ContentTemplate>   
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnRecoverPassword" EventName="Click"></asp:AsyncPostBackTrigger>
          </Triggers>
          </asp:UpdatePanel>         
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderForgottenPassword" runat="server"
            TargetControlID="btnForgottenPassword"
            PopupControlID="mpForgottenPassword"
             CancelControlID= "btnForgottenCancel"
            BackgroundCssClass="modalBackground"
            RepositionMode="None"
            BehaviorID="behaveForgotten" 
            PopupDragHandleControlID="lblHiddenDrag2"
            DropShadow="True"
            dynamicServicePath=""
            Enabled="True" />
      <asp:Button runat="server" ID="btnForgottenPassword" Text="" style="visibility:hidden;"  />
    
    
      <!-- Parts and NHA Detail Modal Panel  -->     
        <asp:Panel ID="mpPartsDetail" runat="server" CssClass="modalPopupParts" >
        <asp:Label runat = "server" ID="lblPartsPopupAJAX" />
        <asp:Label runat="server" ID="lblNoDragDummy1"  style="visibility:hidden"; />
        
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderPartsDetail" runat="server"
            TargetControlID="btnEditModalParts"
            PopupControlID="mpPartsDetail"
            BackgroundCssClass="modalBackground"
            RepositionMode="None"
            Y = 50
            PopupDragHandleControlID="lblNoDragDummy1"
            DropShadow="False"
            Enabled="True" />
       <asp:Button runat="server" ID="btnEditModalParts" Text="" style="visibility:hidden;"  />
 
 
  <!-- Account Admin Table  -->
	<asp:Panel ID="mpAccountUpdate" runat="server" CssClass="modalPopupEditAccount">
	<asp:Panel ID="accountScrollPanel" runat="server" ScrollBars="auto" class="accountScrollPanel" >
	 	<table><tr><td><img src="images/logo-JPE_000.gif" border="0" alt="JPE" /></td>
    <td style='padding:20px; margin-right:10px auto; text-align:right; font-size:10px; color:#999999;'>
         <asp:Button ID="btnAcctCancel1" runat="server"   Text="Cancel"  />&nbsp;&nbsp; <asp:Button ID="btnAccountEditPanelSave1" runat="server"  CausesValidation = "False" Text="Save & Close" OnClick="btnAccountEditPanelSave_Click"   />
         </td>
	 	</tr></table>
	 	<asp:UpdatePanel ID="upAccountDetail" runat="server" UpdateMode="Conditional"     >
        <ContentTemplate>
        <asp:Label ID="lblAccountError1" runat = "server"  />
  </ContentTemplate>   
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnAccountEditPanelSave2" EventName="Click" />
          <asp:AsyncPostBackTrigger ControlID="btnAccountEditPanelSave1" EventName="Click" />
          </Triggers>
          </asp:UpdatePanel>  
         <asp:Label ID="lblAccountAJAX" runat="server" />
        <div style='padding:20px; margin-right:10px auto; text-align:right; font-size:10px;color:#999999;'>
         <asp:Button ID="btnAcctCancel2" runat="server" Text="Cancel"  UseSubmitBehavior="false" />&nbsp;&nbsp; <asp:Button ID="btnAccountEditPanelSave2" runat="server"  CausesValidation = "False" Text="Save & Close" OnClick="btnAccountEditPanelSave_Click"   />
           </div>
	<asp:Label runat="server" ID="lblHiddenDrag3"  style="visibility:hidden"; />
	</asp:Panel>
    </asp:Panel>
    
    
        <br />
        <cc1:ModalPopupExtender ID="ModalPopupExtenderAccountUpdate" runat="server"
            TargetControlID="btnEditAccount"
            PopupControlID="mpAccountUpdate"
            CancelControlID = "btnAcctCancel1"
            BackgroundCssClass="modalBackground"
            RepositionMode="None"
            Y = 50
            PopupDragHandleControlID="lblHiddenDrag3"
            DropShadow="False"
            Enabled="True" />
    <asp:Button runat="server" ID="btnEditAccount" Text="" style="visibility:hidden;" />
 
 
    <!-- Throbber Modal Popup  -->
        <asp:UpdatePanel ID="upPnlThrobber" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
         <asp:Panel ID="mpThrobber" runat="server" CssClass="modalPopupThrobber" >
        <center>
        <asp:Image ID="imgThrobber" runat="server"  ImageUrl="images/progressbar.gif" />
		</center>
		</asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderThrobber" runat="server"
            TargetControlID="btnShowThrobber"
            PopupControlID="mpThrobber"
            BackgroundCssClass="modalBackground"
            RepositionMode="None"
            PopupDragHandleControlID="none"
            DropShadow="False"
            dynamicServicePath="" 
            BehaviorID="behaveThrobber"
            Enabled="True" />
        <asp:Button ID="btnShowThrobber" runat="server" style="visibility:hidden;"   /> 
         <asp:Button ID="btnShowThrobberDummy" runat="server" style="visibility:hidden;"   /> 
       </ContentTemplate>
       </asp:UpdatePanel>
       <!-- DASP   No Dummy on TargetControlID -->
        
        
       <!-- Email Verified Popup Modal Popup  -->
        <asp:UpdatePanel ID="upEmailVerifiy" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
         <asp:Panel ID="mpEmailVerify" runat="server" CssClass="modalPopupEmailVerify" >
        <table style='margin-top:20px;margin-bottom:20px;margin-left:20px;margin-right:20px;width:260px;'><tr><td>
        <asp:Label ID="lblEmailVerifyResults" runat="server" />
      </td><td>
        <asp:Image ID="imgChecked" runat="server"  ImageUrl="images/checked.png" />
         </td></tr>
         <tr><td style="margin-ght:5px auto; text-align:right;padding-right:10px;" coslpan='2'>
          <asp:Button ID="btnCloseVerify" runat="server"  Text="Close"  OnClick="btnCloseVerify_Click"  />
         </td></tr>
         </table>
        <%-- <asp:Label runat="server" ID="Label3"  style="visibility:hidden"; />--%>
		</asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderEmailVerify" runat="server"
            TargetControlID="btnEmailVerify"
            PopupControlID="mpEmailVerify"
            BackgroundCssClass="modalBackground"
            RepositionMode="None"
            PopupDragHandleControlID="none"
            DropShadow="False"
            dynamicServicePath=""
            Enabled="True" />
        <asp:Button ID="btnEmailVerify" runat="server" style="visibility:hidden;"  /> 
       </ContentTemplate>
       </asp:UpdatePanel>

       <!-- User Interaction Acknowledgement -->
           
        <asp:UpdatePanel ID="upAccountUpdateAck" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:Panel ID="mpAccountUpdateAck" runat="server" CssClass="modalPopupAccountAck" BackImageUrl = "images/accountAck.jpg" >
     
      <table style='margin-top:20px;margin-bottom:20px;margin-left:30px;margin-right:20px;width:400px;'>
      <tr><td class="accountAckTop">
         <asp:Label runat="server" ID="lblAcctAckTop" />
      </td></tr>
          <tr><td class="accountAckBottom">
         <asp:Label runat="server" ID="lblAcctAckBottom" />
      </td></tr>
         <tr><td style="margin-right:5px auto; text-align:right;padding-right:10px;" coslpan='2'>
         <asp:Button ID="btnCloseAcctAck" runat="server"  Text="Close"  />
         </td></tr>
         </table>
	</asp:Panel>
    <cc1:ModalPopupExtender ID="ModalPopupExtenderAccountAck" runat="server"
        TargetControlID="btnShowAccountAck"
        PopupControlID="mpAccountUpdateAck"
        BackgroundCssClass="modalBackground"
        RepositionMode="None"
         CancelControlID = "btnCloseAcctAck"
        PopupDragHandleControlID="none"
        DropShadow="True"
        dynamicServicePath=""
        Enabled="True" />
        <asp:Button ID="btnShowAccountAck" runat="server" style="visibility:hidden;"   /> 
           </ContentTemplate>
       </asp:UpdatePanel>
       
       <!-- Bad email address entered - returning user? -->
           
        <asp:UpdatePanel ID="mpBadEmailLogin" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:Panel ID="pnlBadEmailLogin" runat="server" CssClass="modalPopupBadEmail" BackImageUrl = "images/emailnotfound.png" >
    <div style='height:333px; width:500px; margin-top:300px; text-align:right; margin-right:25px;'>
      <asp:Button ID="btnCloseBadEmail" runat="server"  Text="Close"  />
     </div>
	</asp:Panel>
    <cc1:ModalPopupExtender ID="ModalPopupShowBadEmail" runat="server"
        TargetControlID="btnShowBadEmail"
        PopupControlID="mpBadEmailLogin"
        BackgroundCssClass="modalBackground"
        RepositionMode="None"
        CancelControlID = "btnCloseBadEmail"
        PopupDragHandleControlID="none"
        DropShadow="False"
        dynamicServicePath=""
        Enabled="True" />
        <asp:Button ID="btnShowBadEmail" runat="server" style="visibility:hidden;"   /> 
           </ContentTemplate>
       </asp:UpdatePanel>

       <!-- Order Received Popup  -->
           
        <asp:UpdatePanel ID="upOrderThankYou" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:Panel ID="mpOrderReceived" runat="server" CssClass="modalPopupOrderReceived" BackImageUrl = "images/ordreceived.png" >
    <div style='height:333px; width:500px; margin-top:300px; text-align:right; margin-right:25px;'>
      <asp:Button ID="btnCloseOrdReceived" runat="server"  Text="Close"  />
     </div>
	</asp:Panel>
    <cc1:ModalPopupExtender ID="ModalPopupExtenderOrderReceived" runat="server"
        TargetControlID="btnShowOrderReceived"
        PopupControlID="mpOrderReceived"
        BackgroundCssClass="modalBackground"
        RepositionMode="None"
         CancelControlID = "btnCloseOrdReceived"
        PopupDragHandleControlID="none"
        DropShadow="True"
        dynamicServicePath=""
        Enabled="True" />
        <asp:Button ID="btnShowOrderReceived" runat="server" style="visibility:hidden;"   /> 
           </ContentTemplate>
       </asp:UpdatePanel>

	</div>
  </center>

  </td></tr>
		</table>
	</div>
    
	<!--#include file="footer.inc"-->


 <SCRIPT LANGUAGE="JavaScript" language="javascript">
     //<![CDATA[
     function BeginProcess() {
         // Create an iframe.
         var iframe = document.createElement("iframe");

         // Point the iframe to the location of
         //  the long running process.
         iframe.src = "LongRunningProcess.aspx";

         // Make the iframe invisible.
         iframe.style.display = "none";

         // Add the iframe to the DOM.  The process
         //  will begin execution at this point.
         document.body.appendChild(iframe);
     }

     function UpdateProgress(PercentComplete, Message) {
         document.getElementById('trigger').value =
    PercentComplete + '%: ' + Message;
     }
     //]]>
</SCRIPT>


<!--    Modal Panel Code Area       -->
<div class="ecomDiv">
    
       <!-- Help PopUp  -->
	<asp:Panel ID="mpHelp" runat="server" CssClass="modalPopupEditAccount">
	<asp:Panel ID="scrollPanelHelp" runat="server" ScrollBars="Vertical" class="helpScrollPanel" >
        <table style="width:100%;vertical-align:top;width:90%;"><tr><td><img src="images/logo-JPE_000.gif" border="0" alt="JPE" /</td><td style="margin-right: 5px auto; text-align:right;font:verdana;color:#999999;font-size:9px;padding-bottom:15px;"><asp:Button ID="ibCloseHelp" runat="server" Text="Close"  /></td></tr>
        <tr><td colspan = "2">
           <hr color="#999999" width = "100%" />
         <p class="ecom_BodyText">We are proud to introduce <b>JPE Smart Buyer</b>, our innovative, easy-to-use, intuitive B2B <i>[business to business]</i> eCommerce system designed by our engineers from the ground up to give you the best possible on-line buying experience. </p>
         <p class="ecom_BodyText">Click on the section heads below to read more about How to best use your new JPE Smart Buyer system.</p>
         <hr color="#999999" width = "100%" />
        <asp:Label ID="lblModalHelp" runat="server" />
        </td> 
        </tr></table>
	<asp:Label runat="server" ID="lblHiddenDrag4"  style="visibility:hidden"; />

	</asp:Panel>
    </asp:Panel>
     <cc1:ModalPopupExtender ID="ModalPopupExtenderHelp" runat="server"
            TargetControlID="btnShowHelp"
            PopupControlID="mpHelp"
            BackgroundCssClass="modalBackground" 
            CancelControlID="ibCloseHelp"
            RepositionMode="None"
            PopupDragHandleControlID="lblHiddenDrag4"
            DropShadow="True"
            Y = "50"
            dynamicServicePath=""
            Enabled="True" />
	<asp:Button runat="server" ID="btnShowHelp" Text="" style="visibility:hidden;"   />


      <!-- TOU - Terms of Use PopUp  -->
	<asp:Panel ID="mpTOU" runat="server" CssClass="modalPopupEditAccount">
	
        <table style="width:100%;vertical-align:top;width:90%;"><tr><td><img src="images/logo-JPE_000.gif" border="0" alt="JPE" /</td><td style="margin-right: 5px auto; text-align:right;font:verdana;color:#999999;font-size:9px;padding-bottom:15px;"><asp:Button ID="ibCloseTOU" runat="server" Text="Close"  /></td></tr>
        
        <tr><td colspan = "2">
        
        <br /><br /><br />
          
           <p class="ecom_Headline">Jet Parts Engineering Smart Buyer E-Business Web Site Use</p>
          
         <p class="ecom_BodyText">By clicking on accept, you hereby certify that you are the person to whom the username and password entered as assigned to by Jet Parts Engineering (JPE), and that are still employed by the company that you were employed by when the username and password was assigned to you by JPE.</p>
         
          <p class="ecom_Headline">GENERAL TERMS</p>
          
         <p class="ecom_BodyText">The JPE Smart Buyer e-business web site (hereinafter referred to as “Site”) is provided by Jet Parts Engineering, Inc (hereinafter referred to as “JPE”)
as an online information and communication service. Use of this Site is subject to your acceptance of the terms and conditions set forth within this agreement (hereinafter referred to as “Agreement”).</p>

  <p class="ecom_Headline">USE</p>
          
         <p class="ecom_BodyText">The use of this Site is SOLELY for the purpose of conducting business between JPE and its Customers. The User ID and Password assigned to you has been provided in confidence and you may NOT reveal your User ID and Password to anyone under any
circumstance. </p>

<p class="ecom_BodyText"> Your coworkers may request their own User ID and Password if necessary.  If you are no longer employed by JPE’s customer, you must discontinue using this site immediately.  If you accept employment with another JPE customer, you must notify JPE as soon as possible so that access is updated to reflect your new employer’s data. Do not access the Site until the appropriate
changes have been made to your access and authority levels. Continued use of this Site shall be deemed as your continued acceptance of this Agreement.</p>

 <p class="ecom_Headline">TERMINATION</p>
          
         <p class="ecom_BodyText">JPE reserves the right to terminate Site access to you at its sole discretion without notice. JPE will pursue a civil lawsuit or criminal prosecution for any alleged or actual illegal activities involving this Site or any of the services.  JPE may terminate, change, suspend, or discontinue any aspect of this Site at any time, including the availability of any features of this Site. JPE may also impose limits on certain features and services or restrict access to parts, or all, of this Site without notice or liability.</p>
         
       <p class="ecom_Headline">PRICE AND AVAILABILITY</p>
          
         <p class="ecom_BodyText">The prices and availability indicated in the Site are quotations only and do not constitute legally binding offers.</p>   
         
       <p class="ecom_Headline">PROPRIETARY / NON-DISCLOSURE NOTICE</p>
          
         <p class="ecom_BodyText">The information contained in this Site is the property of JPE or its subsidiary and contains trade secrets.  It is delivered to you in confidence and is not to be disclosed to others, copied (in whole or in part); and it shall not be used for any purpose except in the determination by you to purchase items from JPE or its subsidiary.</p>   
        
        </td> 
        </tr></table>
	<asp:Label runat="server" ID="lblHiddenDragTOU"  style="visibility:hidden"; />


    </asp:Panel>
     <cc1:ModalPopupExtender ID="ModalPopupExtenderTOU" runat="server"
            TargetControlID="btnShowTOU"
            PopupControlID="mpTOU"
            BackgroundCssClass="modalBackground" 
            CancelControlID="ibCloseTOU"
            RepositionMode="None"
            PopupDragHandleControlID="lblHiddenDragTOU"
            DropShadow="True"
            Y = "50"
            dynamicServicePath=""
            Enabled="True" />
	<asp:Button runat="server" ID="btnShowTOU" Text="" style="visibility:hidden;"  />
     
      <!-- Forward Back Navigation Update Panel  -->
     
   <asp:UpdatePanel ID="upFwdBackPanel" runat="server" UpdateMode="Conditional"     >
        <ContentTemplate>                    
    <asp:Button runat="server" ID="btnBrowserBackFwdButton" 
        Text="" 
        style="visibility:hidden;" 
        onClick = "btnBrowserBackFwdButton_Click" 
        UseSubmitBehavior="false" />
   
    <asp:Label runat = "server" ID="lblBrowserBackFwdState" style="visibility:hidden;" />
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnBrowserBackFwdButton" EventName="Click" />    
    </Triggers>
    </asp:UpdatePanel>

</div>
<%--
 
 <input type="submit" value="Start Long Running Process" 
  id="trigger" onclick="BeginProcess(); return false;" />
  
  <% gradient.Text = VBCLass1.GradientMeter(400, 20, 75, 50, "", "", ""); %>

<asp:Label ID="gradient" runat="server" />
 
 --%>
  
<%--<asp:Label ID="lblMyBrowser" runat="server" Text="My Browser" />  
<asp:Label ID="lblCookieCode" runat="server" Text="" />
<asp:Label ID="lblMyIP" runat="server" Text="" />
<asp:Label ID="lblMyIP" runat="server" Text="" />
--%>
      



</asp:Content>
    
    
    
    
