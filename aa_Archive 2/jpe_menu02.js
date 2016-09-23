//----------DHTML Menu Created using AllWebMenus PRO ver 4.2-#656---------------
//E:\Documents and Settings\scottmo\My Documents\000-PERS\001-Mile 13\001-Clients\JPE\jjpe000b.awm
var awmMenuName='jpe_menu02';
var awmLibraryBuild=656;
var awmLibraryPath='/awmdata';
var awmImagesPath='/awmdata';
var awmSupported=(navigator.appName + navigator.appVersion.substring(0,1)=="Netscape5" || document.all || document.layers || navigator.userAgent.indexOf('Opera')>-1 || navigator.userAgent.indexOf('Konqueror')>-1)?1:0;
if (awmAltUrl!='' && !awmSupported) window.location.replace(awmAltUrl);
if (awmSupported){
var nua=navigator.userAgent,scriptNo=(nua.indexOf('Safari')>-1)?7:(nua.indexOf('Gecko')>-1)?2:((document.layers)?3:((nua.indexOf('Opera')>-1)?4:((nua.indexOf('Mac')>-1)?5:1)));
var mpi=document.location,xt="";
var mpa=mpi.protocol+"//"+mpi.host;
var mpi=mpi.protocol+"//"+mpi.host+mpi.pathname;
if(scriptNo==1){oBC=document.all.tags("BASE");if(oBC && oBC.length) if(oBC[0].href) mpi=oBC[0].href;}
while (mpi.search(/\\/)>-1) mpi=mpi.replace("\\","/");
mpi=mpi.substring(0,mpi.lastIndexOf("/")+1);
var e=document.getElementsByTagName("SCRIPT");
for (var i=0;i<e.length;i++){if (e[i].src){if (e[i].src.indexOf(awmMenuName+".js")!=-1){xt=e[i].src.split("/");if (xt[xt.length-1]==awmMenuName+".js"){xt=e[i].src.substring(0,e[i].src.length-awmMenuName.length-3);if (e[i].src.indexOf("://")!=-1){mpi=xt;}else{if(xt.substring(0,1)=="/")mpi=mpa+xt; else mpi+=xt;}}}}}
while (mpi.search(/\/\.\//)>-1) {mpi=mpi.replace("/./","/");}
var awmMenuPath=mpi.substring(0,mpi.length-1);
while (awmMenuPath.search("'")>-1) {awmMenuPath=awmMenuPath.replace("'","&#39;");}
document.write("<SCRIPT SRC='"+awmMenuPath+awmLibraryPath+"/awmlib"+scriptNo+".js'><\/SCRIPT>");
var n=null;
awmzindex=1000;
}

var awmSubmenusFrame='';
var awmSubmenusFrameOffset;
var awmOptimize=0;
var awmComboFix=1;
var awmUseTrs=0;
var awmSepr=["0","","",""];
function awmBuildMenu(){
if (awmSupported){
awmImagesColl=["main-header.gif",8,41,"main-footer.gif",8,41,"menu_home_off.gif",0,0,"menu_home_on.gif",0,0,"bkg_chrome_bar.gif",0,0,"menu_about-us_off.gif",0,0,"menu_about-us_on.gif",0,0,"menu_products_off.gif",0,0,"menu_products_on.gif",0,0,"menu_support_off.gif",0,0,"menu_support_on.gif",0,0,"menu_contact_off.gif",0,0,"menu_contact_on.gif",0,0];
awmCreateCSS(0,1,0,n,n,n,n,n,'none',0,'#000000',0,0);
awmCreateCSS(1,2,0,'#FFFFFF',n,4,'bold 11px Verdana',n,'none',0,'#000000','0px 30px 0px 30',1);
awmCreateCSS(0,2,0,'#D41421','#FFFFFF',n,'bold 11px Verdana',n,'none',0,'#000000','0px 30px 0px 30',1);
awmCreateCSS(0,2,0,'#FF0000',n,4,'bold 11px Verdana',n,'none',0,'#000000','0px 30px 0px 30',1);
awmCreateCSS(0,1,0,n,'#FFFFFF',n,n,n,'solid',1,'#000000',0,0);
awmCreateCSS(1,2,0,'#000000',n,n,'11px Verdana',n,'none',0,'#000000','5px 10px 5px 10',1);
awmCreateCSS(0,2,0,'#D41421',n,n,'11px Verdana',n,'none',0,'#000000','5px 10px 5px 10',1);
awmCreateCSS(0,2,0,'#FF0000',n,n,'11px Verdana',n,'none',0,'#000000','5px 10px 5px 10',1);
var s0=awmCreateMenu(0,0,0,0,1,0,0,0,0,1,1,0,1,0,0,0,1,n,n,100,1,0,1,1,0);
it=s0.addItemWithImages(1,2,3,"","","","",2,3,2,3,3,3,n,n,n,"default.aspx",n,n,n,"default.aspx",n,152,31,2,n,n,n,n,n,n,1,0,1,0);
it=s0.addItemWithImages(1,2,3,"","","","",5,6,5,3,3,3,n,n,n,"about_us.aspx",n,n,n,"about_us.aspx",n,152,31,2,n,n,n,n,n,n,1,0,1,0);
var s1=it.addSubmenu(0,0,0,0,0,0,0,4,0,1,0,n,n,80,0,1,0);
it=s1.addItem(5,6,7,"Letter from the President",n,n,"","about_us-president_letter.aspx",n,n,n,"about_us-president_letter.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Company History",n,n,"","about_us-comp_history.aspx",n,n,n,"about_us-comp_history.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Capabilities",n,n,"","about_us-capabilities.aspx",n,n,n,"about_us-capabilities.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Quality Control",n,n,"","about_us-quality_control.aspx",n,n,n,"about_us-quality_control.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"JPE News",n,n,"","about_us-news.stm",n,n,n,"about_us-news.aspx",n,0,0,2,0);
it = s0.addItemWithImages(1, 2, 3, "", "", "", "", 7, 8, 7, 3, 3, 3, n, n, n, "http://JPESmartBuyer.com/ecom_prodSearch.aspx", n, n, n, "http://JPESmartBuyer.com/ecom_prodSearch.aspx", "new", 152, 31, 2, n, n, n, n, n, n, 1, 0, 1, 0);
var s1 = it.addSubmenu(0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 0, n, n, 80, 0, 2, 0);
it = s1.addItem(5, 6, 7, "Buy Parts Online", n, n, "", "http://JPESmartBuyer.com/ecom_prodSearch.aspx", n, n, n, "http://JPESmartBuyer.com/ecom_prodSearch.aspx", "new", 0, 0, 2, 0);
it = s1.addItem(5, 6, 7, "Why Go PMA", n, n, "", "products-why_pma.aspx", n, n, n, "products-why_pma.aspx", n, 0, 0, 2, 0);
it = s0.addItemWithImages(1, 2, 3, "", "", "", "", 9, 10, 9, 3, 3, 3, n, n, n, "ecom_Support.aspx", n, n, n, "ecom_Support.aspx", n, 152, 31, 2, n, n, n, n, n, n, 1, 0, 1, 0);
var s1=it.addSubmenu(0,0,0,0,0,0,0,4,0,1,0,n,n,80,0,3,0);
//it=s1.addItem(5,6,7,"eCommerce Support",n,n,"","ecom_Support.aspx",n,n,n,"ecom_Support.aspx",n,0,0,2,0);
it = s0.addItemWithImages(1, 2, 3, "", "", "", "", 11, 12, 11, 3, 3, 3, n, n, n, "contact.aspx", n, n, n, "contact.aspx", n, 152, 31, 2, n, n, n, n, n, n, 1, 0, 1, 0);
var s1=it.addSubmenu(0,0,0,0,0,0,0,4,0,1,0,n,n,80,0,4,0);
it=s1.addItem(5,6,7,"Seattle Office",n,n,"","contact-n_amer.aspx",n,n,n,"contact-n_amer.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Daphne Office",n,n,"","contact-alabama.aspx",n,n,n,"contact-alabama.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Beijing Office",n,n,"","contact-asia.aspx",n,n,n,"contact-asia.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Amsterdam Office",n,n,"","contact-europe.aspx",n,n,n,"contact-europe.aspx",n,0,0,2,0);
it=s1.addItem(5,6,7,"Careers",n,n,"","contact-careers.aspx",n,n,n,"contact-careers.aspx",n,0,0,2,0);
s0.pm.buildMenu();
}}
