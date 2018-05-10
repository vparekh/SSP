var references = "";
$(document).ready(function () {

    /*
    jQuery.get( url [, data ] [, success ] [, dataType ] )
     url: A string containing the URL to which the request is sent.
    data: A plain object or string that is sent to the server with the request.
    dataType: The type of data expected from the server. Default: Intelligent Guess (xml, json, script, text, html).

    longer form:
    ----------------------------
    $.ajax({
        url: url,
        data: data,
        success: success,
        dataType: dataType
    });

   
    */

    $.get("../Service/SSPService.asmx/ItemTypes", function (data) {
        
        var $xml = $(data);
        myItemTypes = $xml.find("string").text();


    });

    $.get("../Service/SSPService.asmx/RequiredStatus", function (data) {

        var $xml = $(data);
        myRequiredStatus = $xml.find("string").text();
        

    });

    $.get("../Service/SSPService.asmx/NotesList", { parm: $("#txtProtocolVersion").val() }, function (data) {

        var $xml = $(data);
        myNotesList = $xml.find("string").text();
        

    });

    //$("#dlgInsertChecklistItem").dialog({
    //    autoOpen: false,
    //    height: 540,
    //    width: 580,
    //    modal: true,
    //    buttons: {
    //        "Add": function () {
    //            var visibletext = $("#txtVisibleText").val();
    //            var condition = $("#txtCondition").val();
    //            var itemtype = $("#myItemTypes option:selected").val();
    //            var required = $("#myRequiredStatus option:selected").val();
    //            var parent = $("#txtParentItem").val();
    //            var notes = '';
    //            $('#myNotesList :selected').each(function (i, selected) {
    //                if (notes == '') {
    //                    notes = $(selected).val();
    //                }
    //                else {
    //                    notes = notes + ';' + $(selected).val();
    //                }
    //            });

    //            $(this).dialog("close");
    //            dlgInsertChecklistItemCallback(visibletext, itemtype, required, notes, parent, condition);
    //        },
    //        "cancel": function () {
    //            $(this).dialog("close");
    //        }
    //    },
    //    close: function () {

    //    }
    //});



    //custom dialogbox to ask reference
    $("#dlgInsertReference").dialog({
        autoOpen: false,
        height: 350,
        width: 400,
        modal: true,
        buttons: {
            "select": function () {
                var selval = $("#marks option:selected").val();
                var seltext = $("#marks option:selected").text();

                $(this).dialog("close");
                dlgInsertReferenceCallback(selval, seltext);
            },
            "cancel": function () {
                $(this).dialog("close");
            }
        },
        close: function () {

        }
    });


    function dlgInsertReferenceCallback(value, text) {

        reference = text.substring(10);
        richEditor.commands.insertHyperlink.execute({ text: reference, url: value, tooltip: "Click to show reference" });

    }


    //call asmx service - note that response is an xml which is then wrapped inside jquery object
    $.get("../Service/SSPService.asmx/getBookmarks", { parm: $("#txtProtocolVersion").val() + '|' + $('#hdnEditMode').val() + '|' + $('#hdnDraftVersion').val()  }, function (data) {

        references = $(data).text();     //wrap data in a jquery object, and call text() method
        

    }, "html");

    function saveHeaders() {
        //console.log('in save headers');
        var title = $("#txtTitle").val();
        var subtitle = $("#txtSubtitle").val();
        var ajcc = '';
        $('#ajcc :selected').each(function (i, selected) {
            if (ajcc == '') {
                ajcc = $(selected).val();
            }
            else {
                ajcc = ajcc + ',' + $(selected).val();
            }
        });
        var protocolversionckey = $("#txtProtocolVersion").val();
        var userckey = $("#hdnUserCKey").val();
        var headerckey = "0";

        //console.log('read variables');

        
        $.post("../Service/SSPService.asmx/UpdateHeaders",
        {
                    Title: title,
                    Subtitle: subtitle,
                    ProtocolVersionCKey: protocolversionckey,
                    ProtocolHeaderCKey: headerckey,
                    AJCC_UICC_Version: ajcc,
                    UserCKey: userckey

                },
        function (data, status) {
            console.log("Data: " + data + "\nStatus: " + status);
        });
    }

    var timeoutId;
    $('#headers textarea').on('input propertychange change', function () {
        console.log('Textarea Change');

        clearTimeout(timeoutId);
        timeoutId = setTimeout(function () {
            // Runs 1 second (1000 ms) after the last change    
            
            saveHeaders();
        }, 1000);
    });

    function saveToDB() {
        console.log('Saving to the db');
    }

   
}



)


//case summary
var editingnode;

function setCurrentItemKey(key) {
    $('#hdnCurrentItem').val(key);
}

function deleteNode(node) {

    keyValue = $('#hdnCurrentItem').val();

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


function validateAddAuthor() {
   
    //return false;
    if ($('#lstAddAuthors').val() == 'Please select' | $('#lstAddRoles').val() == 'Please select') {
        
        $.confirm({
            title: 'Cannot add!',
            content: 'Please select an author and a role from dropdown boxes.',
            type: 'red',
            boxWidth: '500px',
            useBootstrap: false,
            typeAnimated: true,
            buttons: {
                  
                close: function () {
                }
            }
        });
        return false;
    }
        return false;
    }




//rich edit save events
function OnNoteSaveClick(s, e) {
    richEditor.PerformCallback("save|" + $("#txtProtocolVersion").val());
}
function OnReferencesSaveClick(s, e) {
    myReferences.PerformCallback("save|" + $("#txtProtocolVersion").val());
}
function OnProcedureSaveClick(s, e) {
    myProcedures.PerformCallback("save|" + $("#txtProtocolVersion").val());
}

/*
function showReferenceText(ddl) {
    $.get("../Service/SSPService.asmx/getReferenceText",
        { parm: $("#txtProtocolVersion").val() + "|" + $("#marks option:selected").val() },
        function (data) {
            var $xml = $(data);
            var preview = $xml.find("string").html();
            preview = $('<div/>').html(preview).text();
            $("#referencepreview").html(preview);
        }
    );  //end of get
}
*/


//note link clicked from Tree
window.OnNoteClick = function (note) {
    $("#currentNote").val(note);

    selected = 0;
    current = 0;
    window.location.href = "explanatorynotes.aspx?note=" + note;

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

    UpdateText();
}

function UpdateText() {
    var selectedItems = checkListBox.GetSelectedItems();
    checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    document.getElementById("checklistnotes").value = GetSelectedItemsText(selectedItems);
}
function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    var texts = dropDown.GetText().split(textSeparator);
    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);

    UpdateText(); // for remove non-existing texts
}
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        texts.push(items[i].text);
    return texts.join(textSeparator);
}
function GetValuesByTexts(texts) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = checkListBox.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}



//insert checklist
window.addChecklistItem = function () {
    $('#dlgInsertChecklistItem').css('visibility', 'visible');


    var keyValue = mytreeList.GetFocusedNodeKey();
    $("#txtParentItem").val(keyValue);


    var myTypes = jQuery.parseJSON(myItemTypes);

    var myRequired = jQuery.parseJSON(myRequiredStatus);

    var myNotes = jQuery.parseJSON(myNotesList);



    $('#myItemTypes')
        .find('option')
        .remove()
        .end();

    $('#myRequiredStatus')
       .find('option')
       .remove()
       .end();

    $('#myNotesList')
       .find('option')
       .remove()
       .end();


    $.each(myTypes, function (val, text) {
        $('#myItemTypes').append(
            $('<option></option>').val(val).html(text)
        );
    });

    $.each(myRequired, function (val, text) {
        $('#myRequiredStatus').append(
            $('<option></option>').val(val).html(text)
        );
    });


    $("#myRequiredStatus").change(function () {


        // jQuery
        var selectedVal = $(this).find(':selected').val();
        var selectedText = $(this).find(':selected').text();
        if (selectedVal == '2') {

            $("#txtCondition").val("required only if applicable");

            $('#dlgInsertChecklistItem').css('height', '500');

        }
        else {
            $("#txtCondition").val("");

            $('#dlgInsertChecklistItem').css('height', '500');
        }
    });

    $('#myNotesList').append(
            $('<option></option>').val('').html(''));

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

    //PageMethods.GetVisibleText(keyValue, OnVisualTextReceived);
    $('#txtVisibleText').val('');
    $('#txtCondition').val('');

}

//function OnVisualTextReceived(data) {
//    $('#vistxt').text(data);
//    $('#dlgInsertChecklistItem').dialog('open');
//}



function dlgInsertChecklistItemCallback(visibletext, itemtype, required, notes, parent, sdc, sds) {

    mytreeList.PerformCallback('INSERT|' + visibletext + '|' + itemtype + '|' + required + '|' + notes + '|' + parent + '|' + sdc + '|' + sds);

}




