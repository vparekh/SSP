$(document).ready(function () {

    var jsonTypes;
    var jsonRequired;
    var jsonNotes;
    var jsonUnits;

    
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
                //var condition = $("#txtCondition").val();
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

    function test() {
        
    }

    window.toggleParentNewItem = function (e) {
        if (e.value == 'selected' && e.checked) {
            $('#vistxt').show();
        }
        else {
            $('#vistxt').hide();
        }
    }
   
    window.UpdateChecklistItem = function () {
        
        var obj = new Object();
        obj.CKey = $('#hdnCurrentItem').val();
        obj.VisibleText = $('#txtVisibleText').val();
        obj.ItemType = $('#myItemTypes').val();
        obj.AnswerUnit = $('#myUnitTypes').val();
        obj.SelectionDisablesChildren = ($('#chkSDC').is(':checked')?1:0);
        obj.SelectionDisablesSiblings = ($('#chkSDS').is(':checked')?1:0);
        obj.MustImplement = ($('#chkMustImplement').is(':checked')?1:0);
        obj.MustAnswer = ($('#chkMustAnswer').is(':checked')?1:0);
        obj.ReportOption = $('#myReportOptions').val();
        obj.ProtocolVersionCKey = $('#hdnProtocol').val();

        var noteckeys = '';
        $("#myNotesList > option:selected").each(function () {
            
            noteckeys = noteckeys + ";" + this.value;
        });

        obj.NoteCKeys = noteckeys.substring(1);
       

       

        
        $.ajax({
            url: '../api/Checklist/Update',
            type: 'PUT',
            dataType: 'json',
            data: obj,
            success: function (data, textStatus, xhr) {
                //console.log(data);
                //reload tree
                
                mytreeList.PerformCallback('open|' + $('#ddChecklists').val());
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

        $('#myUnitTypes')
            .find('option')
            .remove()
            .end();

        $('#myNotesList')
           .find('option')
           .remove()
           .end();

        $('#myReportOptions')
            .find('option')
            .remove()
            .end();


        $.each(jsonTypes, function (val, text) {
            $('#myItemTypes').append(
                $('<option></option>').val(val).html(text)
            );
        });

        

        $.each(jsonUnits, function (val, text) {
            $('#myUnitTypes').append(
                $('<option></option>').val(val).html(text)
            );
        });

        $('#myReportOptions').append($('<option></option>').val('1').html('Always'));
        $('#myReportOptions').append($('<option></option>').val('2').html('Never'));
        $('#myReportOptions').append($('<option></option>').val('3').html('Optional'));

        
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


            $.getJSON('../api/Checklist/GetChecklistItem?ItemCKey=' + checklistitem,
                function (checklistdata) {

                    $('#txtVisibleText').val(checklistdata.VisibleText);
                    if (checklistdata.ShowInReport == 1)
                        $('#myReportOptions').prop('checked', true);
                    if (checklistdata.MustAnswer == 1)
                        $('#chkMustAnswer').prop('checked', true);
                    if (checklistdata.MustImplement == 1)
                        $('#chkMustImplement').prop('checked', true);
                    if (checklistdata.SelectionDisablesChildren == 1)
                        $('#chkSDC').prop('checked', true);
                    if (checklistdata.SelectionDisablesSiblings == 1) 
                        $('#chkSDS').prop('checked', true);

                    $('#myItemTypes').val(checklistdata.ItemType);
                    $('#myUnitTypes').val(checklistdata.AnswerUnit);
                    $('#myReportOptions').val(checklistdata.ReportOption);
                    
                    $.each(checklistdata.NoteCKeys.split(";"), function (i, e) {
                        $("#myNotesList option[value='" + e + "']").prop("selected", true);
                    });



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

            $('#myUnitTypes')
                .find('option')
                .remove()
                .end();

            //$('#myRequiredStatus')
            //   .find('option')
            //   .remove()
            //   .end();

            $('#myNotesList')
               .find('option')
               .remove()
               .end();

            $('#myReportOptions')
                .find('option')
                .remove()
                .end();
           

            $.each(myTypes, function (val, text) {
                $('#myItemTypes').append(
                    $('<option></option>').val(val).html(text)
                );
            });

            $.each(myUnits, function (val, text) {                
                $('#myUnitTypes').append(
                    $('<option></option>').val(val).html(text)
                );
            });

            $('#myReportOptions').append($('<option></option>').val('1').html('Always'));
            $('#myReportOptions').append($('<option></option>').val('2').html('Never'));
            $('#myReportOptions').append($('<option></option>').val('3').html('Optional'));

            //$.each(myRequired, function (val, text) {
            //    $('#myRequiredStatus').append(
            //        $('<option></option>').val(val).html(text)
            //    );
            //});

            //$("#myRequiredStatus").change(function () {

            //    // jQuery
            //    var selectedVal = $(this).find(':selected').val();
            //    var selectedText = $(this).find(':selected').text();
            //    if (selectedVal == '2') {
            //        $("#txtCondition").val("required only if applicable");

            //    }
            //    else {
            //        $("#txtCondition").val("");

            //    }
            //});

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


function deleteNode(node) {

    keyValue = $('#hdnCurrentItem').val();
    if (keyValue == '') {
        $.confirm({
            title: 'Select an item',
            content: 'Please select an item to delete.',
            type: 'red',
            boxWidth: '500px',
            useBootstrap: false,
            typeAnimated: true,
            buttons: {

                close: function () {
                }
            }
        });
    }

    $.get("../Service/SSPService.asmx/CanDelete", { parm: keyValue }, function (data) {
        var $xml = $(data);
        //var xmlString = (new XMLSerializer()).serializeToString(data);

        data = $xml.find("boolean").text();
        if (data == "false") {


            $.confirm({
                title: 'Cannot delete!',
                content: 'Selected item cannot be deleted. You may deprecate it by dragging to the Trash icon.',
                type: 'red',
                boxWidth: '500px',
                useBootstrap: false,
                typeAnimated: true,
                buttons: {

                    close: function () {
                    }
                }
            });

            return;
        }


        $.get("../Service/SSPService.asmx/GetVisibleText", { parm: keyValue }, function (data) {
            var $xml = $(data);
            data = $xml.find("string").text();

            $.confirm({
                title: 'Confirm delete!',
                content: 'Delete the selected node?',
                boxWidth: '500px',
                useBootstrap: false,
                buttons: {
                    confirm: function () {
                        mytreeList.PerformCallback('DELETE|' + keyValue);
                    },
                    cancel: function () {

                    },

                }
            });


        });
    });
}

function LoadSelectedChecklist() {
    
    $('#hdnChecklistCKey').val($('#ddChecklists').val());
    mytreeList.PerformCallback('open|' + $('#ddChecklists').val());
    //CaseSummaryCallbackPanel.PerformCallback();
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

function OnGetSelectedNodeValuesCallback(data) {
    var test = '';
    for (var i = 0; i < data.length; i++) {
        test = test + ',' + data[i];
    }
}

//multi-select dropdown functions frim treelist
var textSeparator = ";";
function OnListBoxSelectionChanged(listBox, args) {
   
    try {

        UpdateText();
    }
    catch (err) {
        alert("Error in OnListBoxSelectionChanged - " + err.message);
    }
}

function UpdateText() {
    try {
        var selectedItems = checkListBox.GetSelectedItems();
        //replace all <br/> with ; since <br/> causses validation error
        var notes = GetSelectedItemsText(selectedItems).replace(new RegExp('<br/>', 'g'), ';');     
        checkComboBox.SetText(notes);       
        document.getElementById("checklistnotes").value = notes;
    }
    catch (err) {
        alert("Error in UpdateText() - " + err.message);
    }

}

function SynchronizeListBoxValues(dropDown, args) {
    
    try {        
        checkListBox.UnselectAll();
        var texts = dropDown.GetText().split(textSeparator);
        var values = GetValuesByTexts(texts);        
    
        checkListBox.SelectValues(values);
        UpdateText(); // for remove non-existing texts

    }
    catch (err) {
        alert("error in SynchronizeListBoxValues - " + err.message);
    }

}
function GetSelectedItemsText(items) {
   
    try {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            texts.push(items[i].text);
        return texts.join(textSeparator);
    }
    catch (err) {
        alert('GetSelectedItemsText:' + err.message);
    }

}
function GetValuesByTexts(texts) {
    
    try {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = checkListBox.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
            //else
            //    alert(texts[i] + ' not found');
        }
        return actualValues;
    }
    catch (err) {
        alert("Error in GetValuesByTexts - " + err.message);
    }

}

function CheckConditionSelected(s) {

    if (s.value == "Conditional") {
        cltConditionText.SetText("required only if applicable");  //add item dialog
    }
    else {
        cltConditionText.SetText("");  //add item dialog
    }
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
        loadReferences();
    });

}

//editor for visible text
function OnMemoInit(s, e) {
    //doesn't adjust height exactly but pretty close
    //adding 5 to make sure the height is big enough
    s.SetHeight(s.GetInputElement().scrollHeight+5);
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
var copyNodeKey = null;
var currentNodeKey = null;
function OnCaseSummaryContextMenu(s, e) {
    if (e.objectType != 'Node') return;
    s.SetFocusedNodeKey(e.objectKey);
    var mouseX = ASPxClientUtils.GetEventX(e.htmlEvent);
    var mouseY = ASPxClientUtils.GetEventY(e.htmlEvent);
    ShowCaseSummaryMenu(e.objectKey, mouseX, mouseY);
}
var currentNodeKey = -1;
function ShowCaseSummaryMenu(nodeKey, x, y) {
    clientPopupMenu.ShowAtPos(x, y);
    currentNodeKey = nodeKey;
    var menu = ASPxClientPopupMenu.Cast(clientPopupMenu);
    menu.GetItemByName("PASTECHILD").SetEnabled(copyNodeKey != null);
    menu.GetItemByName("PASTENEXT").SetEnabled(copyNodeKey != null);

}
function OnCaseSummaryEndCallback(s, e) {
    if (myCallback) {
        ResetCaseSummaryValues();
        
    }
}
function ResetCaseSummaryValues() {
    currentNodeKey = null; copyNodeKey = null;
}
function ProcessCaseSummaryNodeClick(itemName) {
    switch (itemName) {
        case "CUT":
            {
                if (copyNodeKey != currentNodeKey) {
                    $("tr[rowKey='" + copyNodeKey + "']").removeClass("cutRow");
                    $("tr[rowKey='" + currentNodeKey + "']").addClass("cutRow");
                    copyNodeKey = currentNodeKey;
                }
                break;
            }
        case "PASTECHILD":
            {
                if (copyNodeKey == null) {
                    alert("There is nothing to paste");
                    return;
                }
                var parameter = "PASTECHILD|" + currentNodeKey + "|" + copyNodeKey;
               
                mytreeList.PerformCallback(parameter);

                break;
            }
        case "PASTENEXT":
            {
                if (copyNodeKey == null) {
                    alert("There is nothing to paste");
                    return;
                }
                var parameter = "PASTENEXT|" + currentNodeKey + "|" + copyNodeKey;
                mytreeList.PerformCallback(parameter);

                break;
            }
    }
}
