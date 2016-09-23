function tryAssociated() {

    var el = document.getElementById('hdn_AssemblySearch').value;
    var sessionID = document.getElementById('hdn_SessionID').value;
    var AccountType = document.getElementById('hdn_MyAccountType').value;


    if (el != '') {
        ShowMyModalPopupEvt('ModalPopupExtenderPartsDetail', 'NHA|' + el + '|' + sessionID + '|' + AccountType, event, '');

    }

}

//------------------------------ show the NHA popup window from the locate NHA control

function showThisNHA(thisNHA) {

    //document.getElementById('btnShowThrobber').click();
    document.getElementById('txtSearch_Assembly').value = "POP:" + thisNHA;
    document.getElementById('btnNHADetailPopup').click();


}



//---------------------------

//http://gmod.org/wiki/Popup_Balloons 

var fadeBox = new Box;
BalloonConfig(fadeBox, 'GBox');
fadeBox.bgColor = 'black';
fadeBox.fontColor = 'white';
fadeBox.borderStyle = 'none';
fadeBox.delayTime = 200;
fadeBox.allowFade = true;
fadeBox.fadeIn = 750;
fadeBox.fadeOut = 200;

var balloonSPB = new Balloon;
BalloonConfig(balloonSPB, 'balloonSPB');
balloonSPB.fontColor = 'black';
balloonSPB.fontFamily = 'Arial, sans-serif';
balloonSPB.fontSize = '10pt';
balloonSPB.minWidth = 100;
balloonSPB.maxWidth = 500;
balloonSPB.padding = 20;
balloonSPB.shadow = 0;
balloonSPB.stemHeight = 39;
balloonSPB.stemOverlap = 1;
balloonSPB.opacity = 0.9;
balloonSPB.images = 'images/blue1';
balloonSPB.balloonImage = 'balloon.png';
balloonSPB.upLeftStem = 'up_left.png';
balloonSPB.upRightStem = 'up_right.png';
balloonSPB.downLeftStem = 'down_left.png';
balloonSPB.downRightStem = 'down_right.png';
balloonSPB.closeButton = 'close.png';
balloonSPB.closeButtonWidth = 20;
balloonSPB.ieImage = null;
balloonSPB.vOffset = 18;
balloonSPB.hOffset = -10;
balloonSPB.delayTime = 25;

// up  right blue  white round 20 60 shadow checked blue1



//------------------------------------- refresh the image in the CATCHA display
//function refreshImage(image) {
//    tmp = new Date();
//    tmp = "?a=" + tmp.getTime()
//    document.images["CaptchaImage"].src = image + tmp;
//}



//------------------------------ turn the Tool Tips on and off
function showTooltipOnOff(evt, caption, sticky, width, height) {
    var UseHints = document.getElementById("hdnUseHints").value;
    if (UseHints == " checked ") {
        balloonSPB.showTooltip(event, caption, sticky, width, height);
    }
}



//----------------- Open the parts detail modal popup
ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);

function HideModalPopup(thisModalPanel) {
    var modal = $find(thisModalPanel);
    modal.hide();
    ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);
}



function ShowMyModalPopupEvt(thisModalPanel, PartID, evt, load) {

    var modal = $find(thisModalPanel);
    var panel = $get('mpPartsDetail');

    ecom_PartDetailModal.BuildDetailPanel(PartID, BuildDisplayPanel);
    ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);
    modal.show();
    panel.style.top = '50px';
    scrollTo(0, 0);
    
}
//------------------------- Switch between the detail panel and the NHA lookup panel
function ModalSwitchEvt(thisModalPanel, PartID, evt, backPartID) {


    ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);


    ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);
    var modal = $find(thisModalPanel);
    var panel = $get('mpPartsDetail');
    var label = document.getElementById('lblPartsPopupAJAX');

    ecom_PartDetailModal.BuildDetailPanel(PartID, BuildDisplayPanel);

    modal.show();
    panel.style.top = '50px';
    scrollTo(0, 0);
}
function BuildDisplayPanel(result) {
    var label = document.getElementById('lblPartsPopupAJAX');
    if (label != null) {
        label.innerHTML = result;
    }
}



//--------------Javascript for the Accounts Management Popup
ecom_EditAccountModal.DeconAccountPanel("", BuildAccountPanel);

function HideAccountModalPopup(thisModalPanel) {
    var modal = $find(thisModalPanel);
    modal.hide();
    ecom_EditAccountModal.DeconAccountPanel("", BuildAccountPanel);
}
function ShowMyAccountModalPopupEvt(AccountID) {
    var modal = $find('ModalPopupExtenderAccountUpdate');
    var panel = $get('mpAccountUpdate');
    var label = document.getElementById('lblAccountAJAX');
    if (label != null) {
        label.innerHTML = "";
    }
    ecom_EditAccountModal.BuildAccountPopup(AccountID, BuildAccountPanel);
    modal.show();
    panel.style.top = '50px';
    scrollTo(0, 0);
}

function BuildAccountPanel(result) {
    var label = document.getElementById('lblAccountAJAX');
    if (label != null) {
        label.innerHTML = result;
    }
}


//--------------------------------------- closes the parts detail modal panel
function closePartsPopup(theSubmitButton, theClosePanel) {

    var modal = $find(theClosePanel);
    modal.hide();
    ecom_PartDetailModal.DeconDetailPanel("", BuildDisplayPanel);

    if (theSubmitButton != "cancel") {
        document.getElementById('btnShowThrobber').click();
        document.getElementById(theSubmitButton).click();
    }



}

//------------------------------ change the status when the autocheck checkbox is changed
function autoCheckChanged() {

    var AutoOn = false;
    var CurUserpkKey = document.getElementById("hdnpkAccountKey").value;
    if (document.getElementById("chkAutoUp") != null) {
        AutoOn = document.getElementById("chkAutoUp").checked;
    }
    ecom_AJAXInterface.MyAccountUpdate("QTYAUTO|" + AutoOn + "|" + CurUserpkKey + "|");
}

//------------------------------ change the status of the use hints checkbox

function useHintsChanged() {

    var useHintsOn = false;
    var CurUserpkKey = document.getElementById("hdnpkAccountKey").value;
    if (document.getElementById("cbUseHints") != null) {

        useHintsOn = document.getElementById("cbUseHints").checked;
    }

    ecom_AJAXInterface.MyAccountUpdate("USEHINTS|" + useHintsOn + "|" + CurUserpkKey + "|");

    document.getElementById("btnChangeUseHints").click();



}