<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteMenu.ascx.cs" Inherits="JetParts.Web.Com.Code.SiteMenu" %>
<%@ Import Namespace="JetParts.Web.Com.Code" %>
<div id="menus" onmouseout="JPEmenuReset();">
    <asp:Repeater ID="MainMenu" runat="server">
        <HeaderTemplate>
            <div id="leftNavBarMainMenu" class="WS_LeftNavBar_MainMenu_div">
        </HeaderTemplate>
        <ItemTemplate>
            <a href="<%# ((JpeSiteMapNode)Container.DataItem).Url %>" target="<%# ((JpeSiteMapNode)Container.DataItem).Target %>" onmouseover="JPEmenuOver('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>');"
                onmouseout="JPEmenuOut('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>');">
                <img src="<%# ((JpeSiteMapNode)Container.DataItem).Image%>" id="<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>"
                    name="<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>" alt="<%# ((JpeSiteMapNode)Container.DataItem).Description%>" /></a><br />
        </ItemTemplate>
        <FooterTemplate>
                <div style="margin-top: 50px">
                    <!-- Facebook -->
                    <div id="fb-root"></div>
                    <script>    
                        (function (d, s, id) {
                            var js, fjs = d.getElementsByTagName(s)[0];
                            if (d.getElementById(id)) return;
                            js = d.createElement(s); js.id = id;
                            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
                            fjs.parentNode.insertBefore(js, fjs);
                        } (document, 'script', 'facebook-jssdk'));
                    </script>
                    <div class="fb-like" data-href="https://www.facebook.com/pages/Jet-Parts-Engineering-Inc/246562272027658?ref=hl" data-send="true" data-layout="button_count" data-width="450" data-show-faces="true" data-font="lucida grande"></div>
                    <br />
                    <!-- Twitter -->
                    <a href="https://twitter.com/JetPartsEng" class="twitter-follow-button" data-show-count="false">Follow @JetPartsEng</a>
                    <script>                !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>
                    <br />
                    <!-- LinkedIn -->
                    <script src="//platform.linkedin.com/in.js" type="text/javascript"></script>
                    <script type="IN/FollowCompany" data-counter="right" data-id="2526328"></script>
                </div>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="SubMenus" runat="server">
        <HeaderTemplate>
            <div id="leftNavBarSubMenu" class="WS_LeftNavBar_SubMenu_div">
        </HeaderTemplate>
        <ItemTemplate>
            <div id="<%# ((JpeSiteMapNode)Container.DataItem).SubMenuName%>" name="<%# ((JpeSiteMapNode)Container.DataItem).SubMenuName%>"
                class="WS_SubMenu_<%# ((JpeSiteMapNode)Container.DataItem).Title%>_Container_div"
                onmouseover="JPEmenuOver('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>');"
                onmouseout="JPEmenuOut('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>');">
                <asp:Repeater ID="SubMenu" DataSource='<%# ((JpeSiteMapNode)Container.DataItem).ChildNodes %>'
                    runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href="<%# ((JpeSiteMapNode)Container.DataItem).Url %>" onmouseover="JPEsubmenuOver('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>','<%# ((JpeSiteMapNode)Container.DataItem).Title%>');"
                            onmouseout="JPEsubmenuOut('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>','<%# ((JpeSiteMapNode)Container.DataItem).Title%>');">
                            <img src="<%# ((JpeSiteMapNode)Container.DataItem).Image%>" id="<%# ((JpeSiteMapNode)Container.DataItem).ParentTitle%>menu_<%# ((JpeSiteMapNode)Container.DataItem).Title%>"
                                name="<%# ((JpeSiteMapNode)Container.DataItem).SubMenuItemName%>%>" alt="<%# ((JpeSiteMapNode)Container.DataItem).Description%>" /></a><br />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    

    <script type="text/javascript">
    var JPEMenuArray = [];
    var currentMain = '<%= CurrentMain %>';
    var currentSubMain = '<%= CurrentSub %>';
    function JPEMenuItem(name, img_src, img_over, sub_name) {
        this.name = name;
        this.img_src = img_src;
        this.img_over = img_over;
        this.sub_name = sub_name;
        this.SubMenus = [];
    }
<asp:Repeater ID="JSMenus" runat="server">
    <HeaderTemplate>
    function JPEMenuInit() {
    </HeaderTemplate>
        <ItemTemplate>JPEMenuArray['<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>'] = new JPEMenuItem('<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>', '<%# ((JpeSiteMapNode)Container.DataItem).Image%>', '<%# ((JpeSiteMapNode)Container.DataItem).ImageOver%>', '<%# ((JpeSiteMapNode)Container.DataItem).SubMenuName%>');
        <asp:Repeater ID="JSMenus2" DataSource='<%# ((JpeSiteMapNode)Container.DataItem).ChildNodes %>' runat="server">
        <ItemTemplate>JPEMenuArray['<%# ((JpeSiteMapNode)Container.DataItem).TopLevelName%>'].SubMenus['<%# ((JpeSiteMapNode)Container.DataItem).Title%>'] = new JPEMenuItem('<%# ((JpeSiteMapNode)Container.DataItem).SubMenuItemName%>', '<%# ((JpeSiteMapNode)Container.DataItem).Image%>', '<%# ((JpeSiteMapNode)Container.DataItem).ImageOver%>', '');
        </ItemTemplate>
        </asp:Repeater></ItemTemplate>
    <FooterTemplate>
        if (document.images)
        {
          preload_image_object = new Image();
          for(menu in JPEMenuArray){
             preload_image_object.src = JPEMenuArray[menu].img_over;
             for(sub in JPEMenuArray[menu].SubMenus){
                preload_image_object.src = JPEMenuArray[menu].SubMenus[sub].img_over;
             }
          }
        }
      }
    </FooterTemplate>
</asp:Repeater>

    function JPEsubmenuOver(menu_name, sub_name)
    {
        var currentSub = JPEMenuArray[menu_name].SubMenus[sub_name];
        if(currentSub != undefined){
            document[currentSub.name].src = currentSub.img_over;
            if(sub_name != currentSubMain) JPEsubmenuOut(currentMain, currentSubMain);

            if(document.getElementById(currentSub.sub_name) != null){
                document.getElementById(currentSub.sub_name).style.display = "inline";
            }
        }
    }
    function JPEsubmenuOut(menu_name, sub_name)
    {
        var currentSub = JPEMenuArray[menu_name].SubMenus[sub_name];
        if(currentSub != undefined){
            document[currentSub.name].src = currentSub.img_src;
        }
        if(sub_name != currentSubMain) JPEsubmenuOver(currentMain, currentSubMain);
    }
    function JPEmenuOver(menu_name) {
        closeSubs();
        var currentMenu = JPEMenuArray[menu_name];
        if(currentMenu != undefined){
            document[currentMenu.name].src = currentMenu.img_over;
            if(menu_name != currentMain) JPEmenuOut(currentMain);

            if(document.getElementById(currentMenu.sub_name) != null){
                document.getElementById(currentMenu.sub_name).style.display = "inline";
            }
        }
    }
    function JPEmenuOut(menu_name) {
        var currentMenu = JPEMenuArray[menu_name];
        if(currentMenu != undefined){
            document[currentMenu.name].src = currentMenu.img_src;
        }
        if(menu_name != currentMain) JPEmenuReset();
    }
    function closeSubs() {
        for (menu in JPEMenuArray) {
            document[JPEMenuArray[menu].name].src = JPEMenuArray[menu].img_src;
            if (document.getElementById(JPEMenuArray[menu].sub_name) != null) {
                document.getElementById(JPEMenuArray[menu].sub_name).style.display = "none";
            }
        }
    }
    JPEMenuInit();
    function JPEmenuReset() {
        JPEmenuOver(currentMain);
        JPEsubmenuOver(currentMain, currentSubMain);
    }
    JPEmenuReset();
    </script>
</div>
