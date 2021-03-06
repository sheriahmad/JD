﻿// ecom.js
// Javascript and AJAX Support for JPE Smart Buyer


//--------------------------- detect the browser type

var BrowserDetect = {
    init: function() {
        this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
        this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
        this.OS = this.searchString(this.dataOS) || "an unknown OS";
    },
    searchString: function(data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            var dataProp = data[i].prop;
            this.versionSearchString = data[i].versionSearch || data[i].identity;
            if (dataString) {
                if (dataString.indexOf(data[i].subString) != -1)
                    return data[i].identity;
            }
            else if (dataProp)
                return data[i].identity;
        }
    },
    searchVersion: function(dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },
    dataBrowser: [
		{
		    string: navigator.userAgent,
		    subString: "Chrome",
		    identity: "Chrome"
		},
		{ string: navigator.userAgent,
		    subString: "OmniWeb",
		    versionSearch: "OmniWeb/",
		    identity: "OmniWeb"
		},
		{
		    string: navigator.vendor,
		    subString: "Apple",
		    identity: "Safari",
		    versionSearch: "Version"
		},
		{
		    prop: window.opera,
		    identity: "Opera"
		},
		{
		    string: navigator.vendor,
		    subString: "iCab",
		    identity: "iCab"
		},
		{
		    string: navigator.vendor,
		    subString: "KDE",
		    identity: "Konqueror"
		},
		{
		    string: navigator.userAgent,
		    subString: "Firefox",
		    identity: "Firefox"
		},
		{
		    string: navigator.vendor,
		    subString: "Camino",
		    identity: "Camino"
		},
		{		// for newer Netscapes (6+)
		    string: navigator.userAgent,
		    subString: "Netscape",
		    identity: "Netscape"
		},
		{
		    string: navigator.userAgent,
		    subString: "MSIE",
		    identity: "Explorer",
		    versionSearch: "MSIE"
		},
		{
		    string: navigator.userAgent,
		    subString: "Gecko",
		    identity: "Mozilla",
		    versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
		    string: navigator.userAgent,
		    subString: "Mozilla",
		    identity: "Netscape",
		    versionSearch: "Mozilla"
		}
	],
    dataOS: [
		{
		    string: navigator.platform,
		    subString: "Win",
		    identity: "Windows"
		},
		{
		    string: navigator.platform,
		    subString: "Mac",
		    identity: "Mac"
		},
		{
		    string: navigator.userAgent,
		    subString: "iPhone",
		    identity: "iPhone/iPod"
		},
		{
		    string: navigator.platform,
		    subString: "Linux",
		    identity: "Linux"
		}
	]

};


BrowserDetect.init();

//-------------------------------- Determine if caps lock is on
function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divMayus').style.visibility = 'visible';
    else
        document.getElementById('divMayus').style.visibility = 'hidden';
}


//-------------------- Choose the postback method

function ChoosePostBack() {
    var fileUploadPO = window.document.getElementById('fileUploadPO');
    if (fileUploadPO != null && fileUploadPO.value.length > 0) {
        Sys.WebForms.PageRequestManager.getInstance()._originalDoPostBack('btnPlaceOrder', '');
        return false;
    }
    return true;
}


//------------------------ Client side numbers only field

function checkIt(evt) {
    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        status = "This field accepts numbers only."
        return false;
    }
    return true;
}


//----------------------------- Submit a form on enter key
function submitOnEnter(event, theSubmitButton) {
    if (event.keyCode == 13) {
        document.getElementById(theSubmitButton).click();
    }
    return true;
}

//----------------------- Move focus on enter key
function focusOnEnter(event, thisControl) {
    if (event.keyCode == 13) {
        document.getElementById(thisControl).focus();
    }
    return true;
}


//------------------------------------ Perform a button click event
function doButtonClick(thisButtonID) {
    document.getElementById(thisButtonID).click();
}

//---------------------------------------------- This was the attempt to use the back button
//var lastHash = "";
//var NavMethod = "";
//var lastiFrameState = "";
//var iFrameState = "";
//var iFrameNav = "";

//window.onload = function() {

//    var label = document.getElementById('lblMyBrowser');
//    if (label != null) {
//        label.innerHTML = "Browser: " + BrowserDetect.browser + " Version: " + BrowserDetect.version + " OS: " + BrowserDetect.OS
//    }

//}
//function pollHash() {
//    switch (BrowserDetect.browser) {
//        case "Explorer":
//            NavMethod = "iFrame";
//            break;
//        case "Chrome":
//            NavMethod = "hash";
//            break;
//        case "Safari":
//            NavMethod = "hash";
//            break;
//        case "Firefox":
//            NavMethod = "hash";
//            break;
//        case "Opera":
//            NavMethod = "hash";
//            break;
//        default:
//            NavMethod = "hash";
//    }

//    if (NavMethod == "iFrame") {

//        iFrameNav = $get("iFrameNav");
//        var iFrameBody = iFrameNav.contentWindow.document.body.innerHTML;
//        var bracket1 = iFrameBody.indexOf("~~");
//        var bracket2 = iFrameBody.indexOf("~~", bracket1 + 2);
//        var iFrameState = iFrameBody.substring(bracket1 + 2, bracket2);

//        if (iFrameState == lastiFrameState) {
//            return;
//        }
//        lastiFrameState = iFrameState;
//        $get('lblBrowserBackFwdState').Text = iFrameState;
//    }
//    else {
//        if (window.location.hash == lastHash) {
//            return;
//        }
//        lastHash = window.location.hash;
//        $get('lblBrowserBackFwdState').Text = lastHash;
//    }
//}





//function NavVueChange(thisNavSource) {
//    var thisNavVue = "";
//    var iFrameSrclabel = document.getElementById('lbliFrameNavSrc');
//    switch (thisNavSource) {
//        case "test":
//            thisNavVue = "test3";
//            break;
//        default:
//            thisNavVue = thisNavSource;
//    }
//    switch (BrowserDetect.browser) {
//        case "Explorer":
//            NavMethod = "iFrame";
//            break;
//        case "Chrome":
//            NavMethod = "hash";
//            break;
//        case "Safari":
//            NavMethod = "hash";
//            break;
//        case "Firefox":
//            NavMethod = "hash";
//            break;
//        case "Opera":
//            NavMethod = "hash";
//            break;
//        default:
//            NavMethod = "hash";
//    }
//    if (NavMethod == "iFrame") {
//        lastiFrameState = thisNavVue;
//        lastHash = "#" + thisNavVue;
//        var iFrameNav = $get("iFrameNav");
//        iFrameNav.src = "reflector.aspx?" + thisNavVue;
//    }
//    else {
//        window.location.hash = thisNavVue;
//    }
//}



//----------------------------- show the throbber modal on mode switch
function ReplaceThrobber(throbberPanel, theSubmitButton, openPanel) {
    $find(openPanel).hide();
    $find(throbberPanel).show();
    document.getElementById(theSubmitButton).click();
}

//--------------------------- Auto populate the billing address on the credit card form
function changeCardBillingAddress() {


    if (document.getElementById("cbUsesBillingAddress").checked) {

        document.getElementById("txtCardAddress1").value = document.getElementById("hdn_txtCardAddress1").value;
        document.getElementById("txtCardAddress2").value = document.getElementById("hdn_txtCardAddress2").value;
        document.getElementById("txtCardCity").value = document.getElementById("hdn_txtCardCity").value;
        document.getElementById("txtCardState").value = document.getElementById("hdn_txtCardState").value;
        document.getElementById("txtCardZip").value = document.getElementById("hdn_txtCardZip").value;
    }

    else {
        document.getElementById("txtCardAddress1").value = "";
        document.getElementById("txtCardAddress2").value = "";
        document.getElementById("txtCardCity").value = "";
        document.getElementById("txtCardState").value = "";
        document.getElementById("txtCardZip").value = "";
    }
}


//------------------------------------- change the contents on the history panel

function switchVue(obj) {
    var el = document.getElementById(obj);
    var elLbl = document.getElementById(obj + "_Label");
    if (el.style.display != 'none') {
        el.style.display = 'none';
        elLbl.innerHTML = 'Show';
    }
    else {
        el.style.display = '';
        elLbl.innerHTML = 'Hide';
    }
}


//--------------------------- switch the view on  the Help Menu
function switchMenu(obj) {
    var el = document.getElementById(obj);
    var elImg = document.getElementById(obj + "Img");
    if (el.style.display != 'none') {
        el.style.display = 'none';
        elImg.style.display = '';
    }
    else {
        el.style.display = '';
        elImg.style.display = 'none';
    }
}

//-------------------------------------- show the throbber then click a button
function showThrobber(theThrobberButton, theSubmitButton) {


    document.getElementById(theThrobberButton).click();
    document.getElementById(theSubmitButton).click();

}

//----------------------------------- show the throbber on a qty update

function qtyUpdate(theThrobberButton, theSubmitButton) {
    document.getElementById(theThrobberButton).click();
    document.getElementById(theSubmitButton).click();

}



//----------------------------------- scroll the window to 0 0 
function jumpScroll() {
    window.scroll(0, 0); // horizontal and vertical scroll targets
}


//------------------------------------- scroll the specified control into view then focus there
function controlAndFocus(scrollControl, focusControl) {
    document.getElementById(scrollControl).scrollIntoView(true);
    $get(focusControl).focus();
}


//----------------------------- Called from qty fields to locate qty changes
function qtyOnBlurIf(theThrobberButton, theSubmitButton, thisControl) {




    var hiddenValue = document.getElementById("hidden_" + thisControl).value;
    var curValue = document.getElementById(thisControl).value;
    var AutoOn = false;

    if (document.getElementById("chkAutoUp") != null) {
        AutoOn = document.getElementById("chkAutoUp").checked;
    }
    else if (document.getElementById("hdn_chkAutoUp") != null) {
        AutoOn = true;
    }

    if (curValue != hiddenValue && AutoOn) {
        document.getElementById(theThrobberButton).click();
        document.getElementById(theSubmitButton).click();
    }
}

//---------------------- Align the Quantity
function alignQtyBoxes(thisPN) {

    var elem = document.getElementById('form1').elements;
    var srchDelim = thisPN.indexOf("#");
    var startPN = thisPN.lastIndexOf("_");
    var srchPN = thisPN.substring(startPN + 1, srchDelim);
    var newValue = document.getElementById(thisPN).value;

    var testDelim = -1;
    var teststartPN = -1;
    var testPN = '';
    
    //alert('startPN: ' + startPN + ' --srchDelim: ' + srchDelim + ' --srchPN:   ' +  srchPN + "  --PN:  " + thisPN + " ----New Value: " + newValue);

    for (var i = 0; i < elem.length; i++) {
        testDelim = elem[i].name.indexOf("#");
        if (testDelim > 0) {
            teststartPN = elem[i].name.lastIndexOf("_");
            testPN = elem[i].name.substring(teststartPN + 1, testDelim);
            //alert("testPN: " + testPN + " - srchPN: " + srchPN);
            if (testPN == srchPN) {
                elem[i].value = newValue;
            }

        }
    }
     
}