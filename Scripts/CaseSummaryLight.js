
window.isitemdirty = false;
$(document).ready(function () {

    
    
    var jsonTypes;
    var jsonRequired;
    var jsonNotes;
    var jsonUnits;

    $('#optionaltext').hide();
    $('#optionalrow').hide();
    $.get("../Service/SSPService.asmx/ItemTypes", function (data) {

        
        var $xml = $(data);
        myItemTypes = $xml.find("string").text();
        
        jsonTypes = jQuery.parseJSON(myItemTypes);


    }); //get itemtype

    $.get("../Service/SSPService.asmx/UnitTypes", function (data) {

        var $xml = $(data);
        myUnitTypes = $xml.find("string").text();
        jsonUnits = jQuery.parseJSON(myUnitTypes);

       

    }); //get unittypes

    $.get("../Service/SSPService.asmx/RequiredStatus", function (data) {

        var $xml = $(data);
        myRequiredStatus = $xml.find("string").text();
        jsonRequired = jQuery.parseJSON(myRequiredStatus);

    }); //get required status

 
 

$("#dlgInsertChecklistItem").dialog({
        autoOpen: false,
        height: 670,
        width: 580,
        modal: true,
        buttons: {
            "Add": function () {
                var visibletext = $("#txtVisibleText").val();
                var itemtype = $("#myItemTypes option:selected").val();
                var unittype = $("#myUnitTypes option:selected").val();

                //var required = $("#myRequiredStatus option:selected").val();
                var parent = $("#txtParentItem").val();
                var sdc = $('#chkSDC').prop('checked');
                var sds = $('#chkSDS').prop('checked');
                var reportType = $('#myReportOptions option:selected').val();
                var mustImplement = $('#chkMustImplement').prop('checked');
                var mustAnswer = $('#chkMustAnswer').prop('checked');

                var notes = '';
                $('#myNotesList :selected').each(function (i, selected) {
                    if (notes == '') {
                        notes = $(selected).val();
                    }
                    else {
                        notes = notes + ';' + $(selected).val();
                    }
                });
              
                if ($('#rootParent').prop('checked'))
                {
                  
                    parent = '';
                }
                $(this).dialog("close");
                dlgInsertChecklistItemCallback(visibletext, itemtype, unittype, notes, parent, sdc, sds, mustImplement, mustAnswer, reportType);
            },
            "cancel": function () {
                $(this).dialog("close");
            }
        },
        close: function () {

        }


});   //dialogbox initialize

    function dlgInsertChecklistItemCallback(visibletext, itemtype, unittype, notes, parent, sdc, sds, mustImplement, mustAnswer, reportType) {

        mytreeList.PerformCallback('INSERT|' + visibletext + '|' + itemtype + '|' + unittype + '|' + notes + '|' + parent + '|' + sdc + '|' + sds + '|' + mustImplement + '|' + mustAnswer + '|' + reportType);

    }

 
    window.toggleParentNewItem = function (e) {
        if (e.value == 'selected' && e.checked) {
            $('#vistxt').show();
        }
        else {
            $('#vistxt').hide();
        }
    }
   
    window.SetDirty = function () {
        window.isitemdirty = true;
    }
    window.UpdateChecklistItem = function (e) {

        var focuskey
        if (e != null)
          focuskey = e.nodeKey;
        //CustomCallback retains the position in treelist,
        //therefore use CustomCallback instead of Ajax to save and reload the tree
        
        if (!window.isitemdirty) return;

        var obj = new Object();
        obj.CKey = $('#hdnCurrentItem').val();
        obj.VisibleText = $('#txtVisibleText').val();
        obj.ItemType = $('#myItemTypes').val();
        //obj.AnswerUnit = $('#myUnitTypes').val();
        //obj.SelectionDisablesChildren = ($('#chkSDC').is(':checked')?1:0);
        //obj.SelectionDisablesSiblings = ($('#chkSDS').is(':checked')?1:0);
        //obj.MustImplement = ($('#chkMustImplement').is(':checked')?1:0);
        //obj.MustAnswer = ($('#chkMustAnswer').is(':checked')?1:0);
        //obj.ReportOption = $('#myReportOptions').val();
        obj.ProtocolVersionCKey = $('#hdnProtocol').val();
        obj.Comments = $('#txtItemComment').val();
        obj.RequiredStatus = $('#myRequiredStatus').val();
        obj.Condition = $('#optionaltext').val();
        
        if (obj.ItemType == null)
        {
            alert('An item type is required. Please select an item type from the dropdown.');
            return false;
        }

        var noteckeys = '';
        $("#myNotesList > option:selected").each(function () {
            
            noteckeys = noteckeys + ";" + this.value;
        });

        obj.NoteCKeys = noteckeys.substring(1);

        if (e != null)
        {
            
            mytreeList.PerformCallback('UPDATE|' + $('#hdnProtocol').val() + '|' + $('#hdnCurrentItem').val() + '|'
          + $('#txtVisibleText').val() + '|' + $('#myItemTypes').val() + '|' + $('#txtItemComment').val()
          + '|' + $('#myRequiredStatus').val() + '|' + $('#optionaltext').val() + '|' + noteckeys.substring(1) + '|' + focuskey);


        }
        else {

            
            mytreeList.PerformCallback('UPDATE|' + $('#hdnProtocol').val() + '|' + $('#hdnCurrentItem').val() + '|'
          + $('#txtVisibleText').val() + '|' + $('#myItemTypes').val() + '|' + $('#txtItemComment').val()
          + '|' + $('#myRequiredStatus').val() + '|' + $('#optionaltext').val() + '|' + noteckeys.substring(1) + '|');

        }
       
        window.isitemdirty = false;
        return;
        
        //do not call ajax update and load since the position is lost upon reloading the tree

        $.ajax({
            url: '../api/Checklist/Update',
            type: 'PUT',
            dataType: 'json',
            data: obj,
            success: function (data, textStatus, xhr) {
                //save focused node
                var keyValue = mytreeList.GetFocusedNodeKey();
                
                //reload tree                
                mytreeList.PerformCallback('open|' + $('#ddChecklists').val());
                //now focus on the node
                mytreeList.SetFocusedNodeKey(keyValue);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
        
    }

    window.PopulateChecklistItemDetails = function (checklistitem) {
        
        $('#hdnCurrentItem').val(checklistitem);
        //populate dropdowns
        $('#myItemTypes')
            .find('option')
            .remove()
            .end();

        //$('#myUnitTypes')
        //    .find('option')
        //    .remove()
        //    .end();

        $('#myNotesList')
           .find('option')
           .remove()
           .end();

        //$('#myReportOptions')
        //    .find('option')
        //    .remove()
        //    .end();


        $('#myRequiredStatus')
           .find('option')
           .remove()
           .end();

        $.each(jsonTypes, function (index, type) {
            
            $('#myItemTypes').append(
                $('<option></option>').val(type.key).html(type.label)
            );
        });

        //$('#myRequiredStatus').append($('<option></option>').val('1').html('Core'));
        //$('#myRequiredStatus').append($('<option></option>').val('2').html('Conditional'));
        //$('#myRequiredStatus').append($('<option></option>').val('3').html('Optional'));

        //use existing authorityrequired field - bit needs to be converted to tinyint

        $('#myRequiredStatus').append($('<option></option>').val('0').html('Optional'));
        $('#myRequiredStatus').append($('<option></option>').val('1').html('Core'));
        $('#myRequiredStatus').append($('<option></option>').val('2').html('Conditional'));
        

        $('#optionaltext').val('');
        $('#txtVisibleText').val('');
        $('#txtComment').val('');
        //$.each(jsonUnits, function (val, text) {
        //    $('#myUnitTypes').append(
        //        $('<option></option>').val(val).html(text)
        //    );
        //});

        //$('#myReportOptions').append($('<option></option>').val('1').html('Always'));
        //$('#myReportOptions').append($('<option></option>').val('2').html('Never'));
        //$('#myReportOptions').append($('<option></option>').val('3').html('Optional'));

        
        $.get("../Service/SSPService.asmx/NotesList", { parm: $("#hdnProtocol").val() },

        function (data) {
            
            var $xml = $(data);
            myNotesList = $xml.find("string").text();
            jsonNotes = jQuery.parseJSON(myNotesList);

            $.each(jsonNotes, function (val, text) {
                
                $('#myNotesList').append(
                    $('<option></option>').val(val).html(text)
                );
            });
                      

            $('#myNotesList').prepend(
                    $('<option></option>').val(0).html("")
            );


         

            
            $.getJSON('../api/Checklist/GetChecklistItem?ItemCKey=' + checklistitem + "&Mode=" + $('#hdnEditMode').val() + "&DraftVersion=" + $('#hdnDraftVersion').val(),
                function (checklistdata) {

                    
                    if (checklistdata == null)
                        return;

                    //alert(checklistdata.RequiredStatus);
                    $('#txtVisibleText').val(checklistdata.VisibleText);
                    $('#txtItemComment').val(checklistdata.Comments);
                    if (checklistdata.ShowInReport == 1)
                        $('#myReportOptions').prop('checked', true);
                    else
                        $('#myReportOptions').prop('checked', false);

                    if (checklistdata.MustAnswer == 1)
                        $('#chkMustAnswer').prop('checked', true);
                    else
                        $('#chkMustAnswer').prop('checked', false);

                    if (checklistdata.MustImplement == 1)
                        $('#chkMustImplement').prop('checked', true);
                    else
                        $('#chkMustImplement').prop('checked', false);

                    if (checklistdata.SelectionDisablesChildren == 1)
                        $('#chkSDC').prop('checked', true);
                    else
                        $('#chkSDC').prop('checked', false);

                    if (checklistdata.SelectionDisablesSiblings == 1) 
                        $('#chkSDS').prop('checked', true);
                    else
                        $('#chkSDS').prop('checked', false);

                    $('#myItemTypes').val(checklistdata.ItemType);
                   // $('#myUnitTypes').val(checklistdata.AnswerUnit);
                    //$('#myReportOptions').val(checklistdata.ReportOption);
                    $('#myRequiredStatus').val(checklistdata.RequiredStatus);

                    $.each(checklistdata.NoteCKeys.split(";"), function (i, e) {
                        $("#myNotesList option[value='" + e + "']").prop("selected", true);
                    });

                    $('#optionaltext').val(checklistdata.Condition);
                    if ($('#myRequiredStatus').val()==2)
                    {
                        
                        $('#optionaltext').show();
                    }
                    else{
                        $('#optionaltext').hide();
                        //$('#optionalrow').hide();
                    }
                });
        })

    }

    window.addChecklistItem = function () {


        $.get("../Service/SSPService.asmx/NotesList", { parm: $("#hdnProtocol").val() },

        function (data) {


            $("#txtVisibleText").val('');
            $("#txtCondition").val('');
            var $xml = $(data);
            myNotesList = $xml.find("string").text();
            jsonNotes = jQuery.parseJSON(myNotesList);

            $('#dlgInsertChecklistItem').css('visibility', 'visible');


            var keyValue = mytreeList.GetFocusedNodeKey();
            $("#txtParentItem").val(keyValue);


            var myTypes = jQuery.parseJSON(myItemTypes);

            var myRequired = jQuery.parseJSON(myRequiredStatus);

            var myNotes = jQuery.parseJSON(myNotesList);

            var myUnits = jQuery.parseJSON(myUnitTypes);

            $('#myItemTypes')
                .find('option')
                .remove()
                .end();

            //$('#myUnitTypes')
            //    .find('option')
            //    .remove()
            //    .end();

          
            $('#myNotesList')
               .find('option')
               .remove()
               .end();

            //$('#myReportOptions')
            //    .find('option')
            //    .remove()
            //    .end();
           

            $.each(myTypes, function (val, text) {
                $('#myItemTypes').append(
                    $('<option></option>').val(val).html(text)
                );
            });

            //$.each(myUnits, function (val, text) {                
            //    $('#myUnitTypes').append(
            //        $('<option></option>').val(val).html(text)
            //    );
            //});

            //$('#myReportOptions').append($('<option></option>').val('1').html('Always'));
            //$('#myReportOptions').append($('<option></option>').val('2').html('Never'));
            //$('#myReportOptions').append($('<option></option>').val('3').html('Optional'));

            $('#myRequiredStatus')
                .find('option')
                .remove()
                .end();

            $('#myRequiredStatus').append($('<option></option>').val('1').html('Core'));
            $('#myRequiredStatus').append($('<option></option>').val('2').html('Conditional'));
            $('#myRequiredStatus').append($('<option></option>').val('3').html('Optional'));

            $('#myNotesList').append(
                $('<option></option>').val('').html(''));

            //trim text at 50 chars
            $.each(myNotes, function (val, text) {
                $('#myNotesList').append(
                    $('<option></option>').val(val).html(text)
                );
            });

            $.get("../Service/SSPService.asmx/GetVisibleText", { parm: keyValue }, function (data) {
                var $xml = $(data);
                data = $xml.find("string").text();

                $('#vistxt').text(data);

                $('#dlgInsertChecklistItem').dialog('open');
            });


        });  //get notes list


    }

})




var editingnode;

var current_item_visible_text;

//function deleteNode(node) {

//    keyValue = $('#hdnCurrentItem').val();
//    if (keyValue == '') {
//        $.confirm({
//            title: 'Select an item',
//            content: 'Please select an item to delete.',
//            type: 'red',
//            boxWidth: '500px',
//            useBootstrap: false,
//            typeAnimated: true,
//            buttons: {

//                close: function () {
//                }
//            }
//        });
//    }

//    $.get("../Service/SSPService.asmx/CanDelete", { parm: keyValue }, function (data) {
//        var $xml = $(data);
//        //var xmlString = (new XMLSerializer()).serializeToString(data);

//        data = $xml.find("boolean").text();
//        if (data == "false") {


//            $.confirm({
//                title: 'Cannot delete!',
//                content: 'Selected item cannot be deleted. You may deprecate it by dragging to the Trash icon.',
//                type: 'red',
//                boxWidth: '500px',
//                useBootstrap: false,
//                typeAnimated: true,
//                buttons: {

//                    close: function () {
//                    }
//                }
//            });

//            return;
//        }


//        $.get("../Service/SSPService.asmx/GetVisibleText", { parm: keyValue }, function (data) {
//            var $xml = $(data);
//            data = $xml.find("string").text();
            
//            $.confirm({
//                title: 'Confirm delete!',
//                content: 'Delete ' + data + '?',
//                boxWidth: '500px',
//                useBootstrap: false,
//                buttons: {
//                    confirm: function () {
//                        mytreeList.PerformCallback('DELETE|' + keyValue);
//                    },
//                    cancel: function () {

//                    },

//                }
//            });


//        });
//    });
//}

function LoadSelectedChecklist() {
   

    $('#hdnChecklistCKey').val($('#ddChecklists').val());
    
    mytreeList.PerformCallback('open|' + $('#ddChecklists').val());
    
}

function LoadCopyChecklist() {    

    copytree.PerformCallback('open|' + $('#ddCopyChecklists').val());
    
}

function LoadSelectedChecklistView() {
    
    $('#hdnChecklistCKeyView').val($('#ddChecklistsView').val());
   
    mytreeListView.PerformCallback('open|' + $('#ddChecklistsView').val());
    //CaseSummaryCallbackPanel.PerformCallback(); //needed to enable/disable drag-drop
}
function setCurrentItemKey(key) {
    $('#hdnCurrentItem').val(key);
}

function selectNotesTab() {
    selectedTab.val('notes');
    $("#maintab a[href='#notes']").tab('show');
    selectedTab.val('notes');
    activateDevExpressControls('notes');
}

function LoadCaseSummaryView() {

    var selectedprotocol = $('#hdnProtocol').val();
   
    $.getJSON("../api/Checklist/Get?ProtocolCKey=" + selectedprotocol, function (data) {
        
        $('#ddChecklistsView').empty();
        $.each(data, function (key, val) {

            $('#ddChecklistsView').append('<option value=' + val.ChecklistCKey + '>' + val.VisibleText + '</option>');
        })

        if ($('#ddChecklistsView > option').length == 0)
        {
            alert("No checklist found for this protocol");
            return;
        }


        $('#ddChecklistsView')[0].selectedIndex = 0;
        $('#hdnChecklistCKeyView').val($('#ddChecklistsView').val());
       
        //test($('#ddChecklistsView').val());
        mytreeListView.PerformCallback('open|' + $('#ddChecklistsView').val());
        ShowTab('lnkCaseSummaryView', 'checklistView');
    });
}



//note link clicked from Tree
window.OnNoteClick = function (note) {

    
    PopulateNoteTags();

    selectNotesTab();

    selected = 0;
    current = 0;


    $("#lstTags > option").each(function () {

        if (this.text == note) {
            selected = current;
        }
        else {
            current++;
        }

    });


    var curentProtocol = $('#hdnProtocol').find(":selected").val();



    $.get("../Service/SSPService.asmx/NoteTitle", { parm: curentProtocol + '|' + note }, function (data) {

        var $xml = $(data);

        $('#txtNoteTitle').val($xml.find('string').text())
        $('#lstTags')[0].selectedIndex = selected;

        var currentNoteCKey = $("#lstTags").val();

        //load note and references
        richEditor.PerformCallback("open|" + currentNoteCKey);
        $('#hdnNoteCKey').val(currentNoteCKey);
        console.log("window.OnNoteClick");
        loadReferences();
    });

}

function GoToNote(note)
{
    
    selectNotesTab();
    SetSelectedNote(null,'n_'+note.replace('.','_'),false)
    
}

function ExpandNodes() {
    mytreeList.ExpandAll();
}


function OnCaseSummaryCallbackError(s, e) {
    // if (e.message == "You're trying to move a parent node to its child") - uncomment this line to reset styles only in certain cases
    myCallback = false;

}
var myCallback = false;
function OnCaseSummaryBeginCallback(s, e) {
    if (e.command == "CustomCallback") {
        myCallback = true;
    }

}
var cutNodeKey = null;
var currentNodeKey = null;
var copyNodeKey = null;
function OnCaseSummaryContextMenu(s, e) {
    //fires before ShowCaseSummaryMenu
    //use this event to show/not show menu
    //alert('OnCaseSummaryContextMenu - ' + e.objectKey);
    if (e.objectKey.indexOf('*') >= 0) return;
    if ($('#hdnEditMode').val() == '0') return;
    if (e.objectType != 'Node') return;
    s.SetFocusedNodeKey(e.objectKey);
    var mouseX = ASPxClientUtils.GetEventX(e.htmlEvent);
    var mouseY = ASPxClientUtils.GetEventY(e.htmlEvent);
    ShowCaseSummaryMenu(e.objectKey, mouseX, mouseY);
}
var currentNodeKey = -1;
function ShowCaseSummaryMenu(nodeKey, x, y) {
    //fires when before me u is shown - use this even enable/disable menu items
    //alert('ShowCaseSummaryMenu - ' + nodeKey);
    clientPopupMenu.ShowAtPos(x, y);
    currentNodeKey = nodeKey;
    var menu = ASPxClientPopupMenu.Cast(clientPopupMenu);
    menu.GetItemByName("PASTE_FIRST_CHILD").SetEnabled(cutNodeKey != null || copyNodeKey != null);
    //menu.GetItemByName("PASTE_LAST_CHILD").SetEnabled(copyNodeKey != null);
    menu.GetItemByName("PASTE_NEXT").SetEnabled(cutNodeKey != null || copyNodeKey != null);
    
    //if(currentNodeKey.substring(0,1)=="*")
    //{
    //    menu.GetItemByName("TRASH").SetEnabled(false);
    //}
    //else {
    //    menu.GetItemByName("TRASH").SetEnabled(true);
    //}
}
function OnCaseSummaryEndCallback(s, e) {
    if (myCallback) {
        ResetCaseSummaryValues();
    }
    
    
    PopulateChecklistItemDetails(mytreeList.GetFocusedNodeKey());
    if (mytreeList.GetVisibleNodeKeys().length > 0) {
        $('#AddRootItem').hide();
        $('#Expand').show();
        $('#Collapse').show();
        
    }
    else {
        $('#AddRootItem').show();
        $('#Expand').hide();
        $('#Collapse').hide();
    }
}
function ResetCaseSummaryValues() {
    currentNodeKey = null; cutNodeKey = null; copyNodeKey = null;
}



function OnCaseSummaryInit(s, e) {
    
    ASPxClientUtils.AttachEventToElement(
    s.GetMainElement(),
    "keydown",
    function (evt) {
        switch (evt.keyCode) {
            //F2               
            case 113:
                
                //var key = s.GetFocusedNodeKey();
                //s.StartEdit(key);
                break;
            case 46:
                //alert('delete');
                
                DeleteNode($('#hdnCurrentItem').val());
            
        }
    }
    );
}
function OnDelete() {
    
}

function AddRootNode()
{
   var parameter = "ADD_ROOT";

    
   mytreeList.PerformCallback(parameter);

}

function ProcessCaseSummaryNodeClick(itemName) {
    $('#hdnNewItem').val(0);
    
    switch (itemName) {
        case "CUT":
            {
                if (cutNodeKey != currentNodeKey) {
                    $("tr[rowKey='" + cutNodeKey + "']").removeClass("cutRow");
                    $("tr[rowKey='" + currentNodeKey + "']").addClass("cutRow");
                    cutNodeKey = currentNodeKey;
                }
                break;
            }
        case "COPY":
            {
                //alert("in copy");
                if (copyNodeKey != currentNodeKey) {
                    //$("tr[rowKey='" + copyNodeKey + "']").removeClass("cutRow");
                    //$("tr[rowKey='" + currentNodeKey + "']").addClass("cutRow");
                    //alert('in test');
                    copyNodeKey = currentNodeKey;
                    cutNodeKey = null;
                }
                break;
            }
        case "PASTE_FIRST_CHILD":
            {
                if (cutNodeKey == null & copyNodeKey==null) {
                    alert("There is nothing to paste");
                    return;
                }
                var parameter;

                //alert(currentNodeKey);
                //alert(copyNodeKey);
                if(cutNodeKey!=null)
                    parameter = "PASTE_FIRST_CHILD|" + currentNodeKey + "|" + cutNodeKey;
               
                if (copyNodeKey != null)
                    parameter = "COPY_PASTE_FIRST_CHILD|" + currentNodeKey + "|" + copyNodeKey;

                cutNodeKey = null;
                copyNodeKey = null;

                mytreeList.PerformCallback(parameter);

                break;
            }
        case "PASTE_LAST_CHILD":
            {
                if (cutNodeKey == null && copyNodeKey == null) {
                    alert("There is nothing to paste");
                    return;
                }
                var parameter;
                if (cutNodeKey != null)
                    parameter = "PASTE_LAST_CHILD|" + currentNodeKey + "|" + cutNodeKey;
                if (copyNodeKey != null)
                    parameter = "COPY_PASTE_LAST_CHILD|" + currentNodeKey + "|" + copyNodeKey;

                cutNodeKey = null;
                copyNodeKey = null;

                mytreeList.PerformCallback(parameter);

                break;
            }
        case "PASTE_NEXT":
            {
                if (cutNodeKey == null && copyNodeKey == null) {
                    alert("There is nothing to paste");
                    return;
                }
                var parameter;

                if (cutNodeKey != null)
                    parameter = "PASTE_NEXT|" + currentNodeKey + "|" + cutNodeKey;
                if (copyNodeKey != null)
                    parameter = "COPY_PASTE_NEXT|" + currentNodeKey + "|" + copyNodeKey;

                cutNodeKey = null;
                copyNodeKey = null;

                mytreeList.PerformCallback(parameter);

                break;
            }
        case "ADD_FIRST_CHILD":
            {
                
                var parameter = "ADD_FIRST_CHILD|" + currentNodeKey;
                mytreeList.PerformCallback(parameter);
                $('#hdnNewItem').val(1);
                break;
            }
        case "ADD_LAST_CHILD":
            {
                
                var parameter = "ADD_LAST_CHILD|" + currentNodeKey;
                mytreeList.PerformCallback(parameter);
                $('#hdnNewItem').val(1);
                break;
            }
        case "ADD_NEXT":
            {
                
                var parameter = "ADD_NEXT|" + currentNodeKey;
                mytreeList.PerformCallback(parameter);
                $('#hdnNewItem').val(1);
                break;
            }
        case "TRASH":
            {
                var parameter = "TRASH|" + currentNodeKey;
                mytreeList.PerformCallback(parameter);

                break;
            }
        case "DELETE":
            {

               
                DeleteNode(currentNodeKey);
                

                break;
            }
    }
}

function DeleteNode(NodeKey)
{
    $.get("../Service/SSPService.asmx/CanDelete", { parm: NodeKey }, function (data) {
        var $xml = $(data);

        data = $xml.find("boolean").text();
        if (data == "false") {

            //this is an existing item and you can only deprecate it
            //$.confirm({
            //    title: 'Cannot delete!',
            //    content: 'Selected item cannot be deleted. You may deprecate it by dragging to the Trash icon or by selecting Move to Recycle option from the context menu.',
            //    type: 'red',
            //    boxWidth: '500px',
            //    useBootstrap: false,
            //    typeAnimated: true,
            //    buttons: {

            //        close: function () {
            //        }
            //    }
            //});
            $.get("../Service/SSPService.asmx/GetVisibleText", { parm: NodeKey }, function (data) {
                var $xml = $(data);
                data = $xml.find("string").text();
                $.confirm({
                    title: 'Confirm delete!',
                    
                    content: 'Delete ' + data + '?' + '<br/>Note: This is an existing data item and will be deprecated upon delete. You will be able to find it in the Recycle bin.',
                    boxWidth: '500px',
                    useBootstrap: false,
                    buttons: {
                        confirm: function () {
                            var parameter = "TRASH|" + NodeKey;
                            mytreeList.PerformCallback(parameter);
                        },
                        cancel: function () {

                        },

                    }
                });
            });

            
            
        }
        else {

            $.get("../Service/SSPService.asmx/GetVisibleText", { parm: NodeKey }, function (data) {
                var $xml = $(data);
                data = $xml.find("string").text();
                $.confirm({
                    title: 'Confirm delete!',
                    content: 'Delete ' + data + '?',
                    boxWidth: '500px',
                    useBootstrap: false,
                    buttons: {
                        confirm: function () {
                            var parameter = "DELETE|" + NodeKey;
                            mytreeList.PerformCallback(parameter, OnDelete);
                        },
                        cancel: function () {

                        },

                    }
                });
            });

        }
    });

}