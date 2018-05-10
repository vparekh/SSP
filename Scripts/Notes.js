

//loads notes in sortable list
function loadnoteslist(selectedNote) {

    //if selectedNote is not null, that note is highlighted
    //if selecteNote is null, the first note is highlighted
    
    
    if ($("#hdnProtocol").val() == "")
        return;
   
    var version = $('#ddDraftVersions').val();

    //get notes
    $.getJSON("../api/ProtocolNotes/GetNotes?ProtocolID=" + $("#hdnProtocol").val() +  '&Mode=' + $('#hdnEditMode').val() + '&DraftVersion=' + version ,
     function (data) {
        
         // build sortable notes list
         var notes = "<ul style='list-style: none;padding-left:4px;margin-top:10px' id='sortablenotes';>";
         
         var fn = " onclick = 'SetSelectedNote(event, this.id, true)'>";
       
         //light purple : #dbd4e9
         //white : #e9edf7
         var indexRef = 0;
         $.each(data, function (key, val) {

             if ($('#hdnEditMode').val() == 1) {
                 notes = notes + "<li style='border-bottom:solid thin' value=" + "n_" + val.CKey.replace(".", "_") + ">" +
                                     "<div style='display:inline;width:200px;'><span class='reference' id=" + "n_" + val.CKey.replace(".", "_") + fn + val.Title + "</span></div></li>";

             }
             else {
                 notes = notes + "<li class='nosort' style='border-bottom:solid thin' value=" + "n_" + val.CKey.replace(".", "_") + ">" +
                                     "<div style='display:inline;width:200px;'><span class='reference' id=" + "n_" + val.CKey.replace(".", "_") + fn + val.Title + "</span></div></li>";

             }
            
             
             indexRef++;
         }); //each
         notes = notes + "</ul>";
        
         $('#noteslist').html(notes);     

         //Highlight the selected note. 
         //The 2nd parameter is the id of the list item to be highlighted

         if (selectedNote != null) {
             SetSelectedNote(null, 'n_' + selectedNote.replace(".", "_"), false);
         }
         else {
             var firstnoteid = $('#sortablenotes li:first').attr('value');
             SetSelectedNote(null, firstnoteid, false);

         }

         //initialize sortable control
         $("#sortablenotes").sortable({
             items: 'li:not(.nosort)',
             update: function (event, ui) {
                 var ckey = ui.item.attr('value').substring(2).replace("-", ".");
                 var index = ui.placeholder.index();
                 var ckeys = '';
                 $.each($('#sortablenotes li'), function (index, val) {
                     type = typeof $(this).attr('value');
                     if (type != 'undefined')
                         ckeys = ckeys + ',' + $(this).attr('value').substring(2).replace("_", ".");
                 })
                 
                 var obj = new Object();
         
                 obj.CKeys = ckeys;
                 $.ajax({
                     url: '../api/ProtocolNotes/Resequence',
                     type: 'PUT',
                     dataType: 'json',
                     data: obj,
                     success: function (data, textStatus, xhr) {
                         console.log(data);
                         var firstnoteid = $('#sortablenotes li:first').attr('value');
                         loadnoteslist();  //reload notes list and open the first note
                     },
                     error: function (xhr, textStatus, errorThrown) {
                         console.log('Error in Operation');
                     }
                 });


             }
         });

         $("#sortablenotes").disableSelection();
     });
}


//creates a new note with specified title
function CreateNote(title)
{
   
    data = new Object();
    data.ProtocolCKey = $("#txtProtocolVersion").val();  //empty string is sent as null
    data.UserCKey = $('#hdnUserCKey').val();
    data.Title = title;
   
    $.ajax({
        url: '../api/ProtocolNotes/CreateUpdateNote',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data, textStatus, xhr) {

            //load notes that includes the new note and select the new note
            loadnoteslist(data);
            
            //also load references for the new note
            console.log('CreateNote');
            loadReferences();

        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });

    richEditor.PerformCallback("new|" + $("#txtProtocolVersion").val()); //clear editor
}

function UpdateNoteTitle(title)
{
    
    data = new Object();
    data.ProtocolCKey = $("#txtProtocolVersion").val();  //empty string is sent as null
    data.UserCKey = $('#hdnUserCKey').val();
    data.Title = title;
    data.NoteCKey = $('#hdnNoteCKey').val();
   
    $.ajax({
        url: '../api/ProtocolNotes/CreateUpdateNote',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data, textStatus, xhr) {

            //load notes that includes the new note and select the new note
            loadnoteslist($('#hdnNoteCKey').val());

            console.log('UpdateNoteTitle');
            //also load references for the new note
            loadReferences();

        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
}

function ShowNoteTitle()
{
    // $('#ddDraftVersions').val(999);
    //$('#hdnEditMode').val()
    $.get("../Service/SSPService.asmx/NoteTitle", { parm: $('#hdnNoteCKey').val() + '|' + $('#hdnEditMode').val() + '|' + $('#ddDraftVersions').val() }, function (data) {

        var $xml = $(data);
        
        var title = ($xml.find("string").text());
        $('#notetitle').html(title);
       
    });
}

function SetSelectedNote(evt, id, showCmd) {
   
    
    if (id == undefined) id = "0.100004300";
    var orgid = id;
    
    

    id = id.substring(2).replace("_", ".");
    
    $('#hdnNoteCKey').val(id);
    
    $('.reference').removeClass('activereference');
    

    $('#' + orgid).addClass('activereference');
    $('.commandtoolbar').hide();

    if (showCmd == true) {
        $('#t_' + orgid.substring(2)).show();
    }
    
    
    richEditor.PerformCallback("open|" + id);

    console.log("SetSelectedNote");
   loadReferences();

   ShowNoteTitle();
    return false;
   
}


function loadReferences() {

    console.log("loadReferences");

    if ($("#hdnProtocol").val() == "")
        return;
    var version = $('#ddDraftVersions').val();

    $.getJSON("../api/reference/GetReference?NoteCKey=" + $("#hdnNoteCKey").val() + "&Mode=" + $('#hdnEditMode').val() + "&DraftVersion=" + version,
                function (data) {

                    var fn = " onclick = 'SetSelectedReference(event, this.id, true)'>";
                    var i = 1;
                    var references = '';

                    references = "<ul style='list-style: none;padding-left:4px;margin-top:10px' id='sortablereferences'>";
                  

                    var i = 0;
                    $.each(data, function (key, val) {
                        i++;
                        references = references + "<li style='border-bottom:solid thin'  class='nosort' value=" + "r_" + val.ReferenceCKey.replace(".", "_") + ">" +
                                                     "<div style='display:inline;width:200px'><span>" + i + ". </span><span draggable='true' ondragstart='drag(event)' reference' id=" + "r_" + val.ReferenceCKey.replace(".", "_") + fn + val.ReferencesContent + "</span></div></li>";

                    }); //each
                    references = references + "</ul>";

                    $('#referencelist').html(references);
                   
                });  //data
}
function ResetSelectedReference(e) {
    
    
    if (e.target.nodeName == 'DIV') {
        
        $('#hdnSelectedReference').val('0');
        $('.reference').removeClass('activereference');
        $('.commandtoolbar').hide();

    }
}

function ResetSelectedNote(e) {
    if (e.target.nodeName == 'DIV') {
        $('#hdnNoteCKey').val('0');
        $('.reference').removeClass('activereference');
        $('.commandtoolbar').hide();

    }
}

function GetReferenceOrder(referencevalue) {
    //reference value - r_2_1.00004300
    var i = 1;
    var foundat = 1;
    $.each($('#sortablereferences li'), function (index, val) {
        if (($(this).attr('value')) == referencevalue) {
            foundat = i;
            return;  //return from anonymous function works like break 
        }
        else {
            i++;
        }
    })

    return foundat;
}

function SetSelectedReference(e, id, showCommands) {
    //when showCommands = true edit commands (delete and edit) show
    //hidden control has just the ckey while id has the value from list item

    $('.activereference').removeClass('activereference');

    $('#hdnSelectedReference').val(id.substring(2).replace("_", "."));

    if ($('#' + id).length == 0) {
        alert("Error Reference Id = " + id + " not found.")
        return;
    }


    $('#' + id).addClass('activereference');
    

    return false;
    //stop click event from bubbling up
    //e.stopPropagation();
}

function DeleteHTMLReference() {
    
    var apiUrl = "../api/Reference/DeleteReference?NoteReferenceCKey={0}";
    var refer = $("#hdnSelectedReference").val();

    apiUrl = apiUrl.replace("{0}", refer);

    

    $.confirm({
        title: 'Confirm delete!',
        content: 'Delete this reference?',
        boxWidth: '500px',
        useBootstrap: false,
        buttons: {
            confirm: function () {
                $.ajax({
                    url: apiUrl,
                    type: 'DELETE',
                    cache: false,
                    statusCode: {
                        200: function (data) {
                           
                           
                            referenceEditor.SetHtml("");
                          
                            $.get("../Service/SSPService.asmx/getBookmarks", { parm: $("#hdnNoteCKey").val() + '|' + $('#hdnEditMode').val() + '|' + $('#hdnDraftVersion').val() }, function (data) {

                                var $xml = $(data);
                                var allref = $xml.find("string").text();
                                
                                $('#dlgInsertReference').empty();

                                $('#dlgInsertReference').append(allref);

                                console.log("DeleteHTMLReference");

                                //load references
                                loadReferences();

                                //highlight the selected note again
                                SetSelectedNote(null, 'n_' + $('#hdnNoteCKey').val().replace('.', '_'), false);

                            });

                            //OpenSelectedNote();
                        }, // Successful DELETE
                        404: function (data) {
                            alert("Notes.js - not found: " + data);
                        }, // 404 Not Found
                        400: function (data) {
                            alert("Notes.js - Bad Request " + data);

                        } // 400 Bad Request
                    } // statusCode


                }); // ajax call
            },
            cancel: function () {

            },

        }
    });



}


function DeleteNote() {
    var apiUrl = "../api/ProtocolNotes/Delete/{0}";
    
    apiUrl = apiUrl.replace("{0}", $("#hdnNoteCKey").val());

    $.confirm({
        
        title: 'Confirm delete!',
        content: 'Delete the note? Please note that deleting this note will remove references to it from case summary.',
        boxWidth: '500px',
        useBootstrap: false,
        buttons: {
            confirm: function () {
                $.ajax({
                    url: apiUrl,
                    type: 'DELETE',
                    cache: false,
                    statusCode: {
                        200: function (data) {
                            //after DELETE is complete, set hdnSelecteNote to the first note in the list                           
                            var firstnoteid = $('#sortablenotes li:first').attr('value');
                            //get CKey from id 
                            firstnoteid = firstnoteid.substring(2).replace("_", ".");
                            
                            //refresh new list after DELETE
                            loadnoteslist(firstnoteid);                                                        
                            
                        }, // Successful DELETE
                        404: function (data) {
                            alert('Notes.js ' + apiUrl + " ... Not Found");
                        }, // 404 Not Found
                        400: function (data) {
                            alert("Notes.js Bad Request O");
                        } // 400 Bad Request
                    } // statusCode
                }); // ajax call
            },
            cancel: function () {

            },

        }
    });


}

function addMouseOverToReferences()
{
    $('span[title*="referenceckey"]').addClass('referenceClass');
    var spanList = $("span[class='referenceClass']");
    
    spanList.each(function (index) {
        
        var thisSpan = $(this);
        var titletext = $(this).attr('title');
        var referenceckey = titletext.replace("referenceckey:", "");
        
           
            var nestedSpan = $(thisSpan).find("span")[0];
            $(nestedSpan).mouseover(function () {
                $.get("../Service/SSPService.asmx/getReferenceText", { parm: referenceckey + '|' + $('#hdnEditMode').val() + '|' + $('#hdnDraftVersion').val() }, function (data) {

                    var $xml = $(data);
                    var result = $xml.find("string").text();
                    $('#divReferencePreview').html(result);
                    result = $('#divReferencePreview').text();
                    thisSpan.attr('title',result);
                });
          
            });       
    });
    
}

//event attached to the comment inside
function addOnClickToComments() {
    $('span[title*="commentckey"]').addClass('commentClass');
    var spanList = $("span[class='commentClass']");
    
    spanList.each(function (index) {

        var thisSpan = $(this);
        var titletext = $(this).attr('title');
        var commentckey = titletext.replace("commentckey:", "");

        var nestedSpan = $(thisSpan).find("span")[0];
        $(nestedSpan).click(function () {

            
            $.ajax({
                url: '../api/NoteComment/GetComment?NoteCommentCKey=' + commentckey + "&DraftVersion=" + $("#ddDraftVersions").val(),
                success: function (data, textStatus, xhr) {
                    
                    ShowAddCommentDlg(data.UserComment, data.NoteCommentCKey);
                   

                },

                error: function (xhr, textStatus, errorThrown) {
                    console.log('Error in Operation');
                }

            });

        });
    });

}

function stripHTML(htmlstring) {
    return $("<div/>").html(htmlstring).text();
   
}

function addMouseOverToComments() {
    $('span[title*="commentckey"]').addClass('commentClass');
    var spanList = $("span[class='commentClass']");
    
    spanList.each(function (index) {

        var thisSpan = $(this);
        var titletext = $(this).attr('title');
        var commentckey = titletext.replace("commentckey:", "");

        var nestedSpan = $(thisSpan).find("span")[0];
        $(nestedSpan).mouseover(function () {
          
            //var point = richEditor.core.canvasManager.getLayoutPoint(evt, false);
            //var hitTestResult = richEditor.core.hitTestManager.calculate(point, __aspxRichEdit.DocumentLayoutDetailsLevel.Character, s.core.model.activeSubDocument);
            //var modelPosition = hitTestResult.getPosition();
            //var interval = new ASPx.Interval();
            //interval.start = modelPosition;
            //interval.length = 0;
            //richEditor.selection.intervals = [interval];

            $.ajax({
                url: '../api/NoteComment/GetComment?NoteCommentCKey=' + commentckey + "&DraftVersion=" + $("#ddDraftVersions").val(),
                success: function (data, textStatus, xhr) {
                    var comment = data.UserComment;
                    //strip html markup
                    comment = stripHTML(comment);
                    thisSpan.attr('title',comment );
                    $('#divReferencePreview').html(data.UserComment);

                },

                error: function (xhr, textStatus, errorThrown) {
                    console.log('Error in Operation');
                }

            });

        });
    });

}

function onSave()
{
   //callback event from richedit
    loadReferences();
}
function OnNoteSaveClick(s, e) {
    //only allow authors to save
    var role = $("#hdnRole").val();
    if (role.indexOf("1") < 0 && role.indexOf("5") < 0 && role.indexOf("99") < 0)
    {
        
        alert("Only authors are allowed to save changes");
        return;
    }
    richEditor.PerformCallback("save|" + $("#txtProtocolVersion").val() + "|" + $("hdnNoteCKey").val());

}


/*
function selectReferenceTab() {
    selectedTab.val('references');
    $("#maintab a[href='#references']").tab('show');
    selectedTab.val('references');
    activateDevExpressControls('references');
}
*/

/*10/10/2017 - gets reference text for dispaly in reference dialogbox*/
function showReferenceText(ddl) {

    $('#hdnSelectedReference').val($("#marks option:selected").val());
    $("#editreferenceckey").val($("#marks option:selected").val());

    $.get("../Service/SSPService.asmx/getReferenceText", { parm: $("#marks option:selected").val() + '|' + $('#hdnEditMode').val() + '|' + $('#hdnDraftVersion').val() }, function (data) {
        
        var $xml = $(data);
        result = $xml.find("string").text();
        $("#referencepreview").html(result);


    });  //get bookmarks


}


$(document).ready(function () {

    //custom dialogbox to ask reference
    /*10/10/2017- this dialogbox is used to display/update reference when reference link is ctrl_clicked*/ 
    $("#dlgInsertReference").dialog({
        autoOpen: false,
        height: 400,
        width: 450,
        modal: true,
        buttons: {
            "insert": function () {
                var selval = $("#marks option:selected").val();
                var seltext = $("#marks option:selected").text();
                var index = $("#marks").prop('selectedIndex');
                //remove numbers
                var index = seltext.indexOf('-');
                if (index > 0 & index < 3)
                    seltext = seltext.substring(index + 1);
                
                if (selval == '') {
                    alert("Please a select a reference to insert");
                    return;
                }
                $(this).dialog("close");

                //selval is ckey and seltext is the text
                
                //dlgInsertReferenceCallback(selval, '&#160;', seltext);
                dlgInsertReferenceCallback(selval, 'referenceckey:' + selval);
               
            },
            "edit": function () {
               
                //r_3_1_00004100
                var id = $("#editreferenceckey").val().replace(".", "_");
                id = "r_" + id;
                ShowAddReferenceDlg(id);

            },
            "delete": function () {
                              
                DeleteHTMLReference();
               
               

            },
            "new": function() {
                
                ShowAddReferenceDlg();  //null id
               
            },
           
           
            "cancel": function () {
                $(this).dialog("close");
            }
        },
        close: function () {

        }
    });

    

    function dlgInsertReferenceCallback(value, tooltip) {
        //tooltip has referenceckey:nn.100004300
        //reference = index.toString();
        //richEditor.commands.insertHyperlink.execute({ text: reference, url: value, tooltip: "Click to show reference" });

        
        richEditor.commands.insertHyperlink.execute({ text: '\u0087', url: value, tooltip: tooltip });
    }

  
})

/*
window.ShowReference = function (reference) {


    selectReferenceTab();

    selected = 0;
    current = 0;

    //populate references list
    PopulateReferenceTags();

    $("#ddReference > option").each(function () {

        if (this.text == reference) {

            selected = current;
        }
        else {
            current++;
        }

    });

    var curentProtocol = $('#hdnProtocol').find(":selected").val();

    $('#ddReference')[0].selectedIndex = selected;



}
*/

$(document).ready(function () {
    $("#dlgAddReference").dialog({
        autoOpen: false,
        height: 300,
        width: 700,
        modal: true,
        buttons: {
            'Remove Markups': function () {
               
                referenceEditor.SetHtml(stripHTML(referenceEditor.GetHtml()));
            },
            Save: function () {

                SaveReference($("#editreferenceckey").val(), $("#editreferenceorder").val(), referenceEditor.GetHtml());
              

                $(this).dialog("close"); //closing on Ok click
            },
            Cancel: function () {
                $(this).dialog("close");
            },
           
        }
    });

    $("#dlgInsertComment").dialog({
        autoOpen: false,
        height: 550,
        width: 600,
        modal: true,
        buttons: {
            Save: function () {
                var comment = CommentEditor.GetHtml();
                if ($('#hdnNoteCommentCKey').val()=='')
                    AddNoteComment(comment);
                else
                {   
                    UpdateNoteComment(comment, $('#hdnNoteCommentCKey').val());
                }

                $(this).dialog("close"); //closing on Ok click
            },
            Delete: function () {
                UpdateNoteComment('', $('#hdnNoteCommentCKey').val());
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dlgNewNote").dialog({
        autoOpen: false,
        height: 200,
        width: 500,
        modal: true,
        buttons: {
            Ok: function () {
                var title = $('#NewNoteTitle').val();
                
                if ($('#hdnNoteCKey').val() == '' || $('#hdnNoteCKey') == '0')
                {
                    CreateNote(title);
                }
                else {
                   
                    UpdateNoteTitle(title);
                }
                
                $(this).dialog("close");
            }
        }
    })

});


function ShowAddNoteDlg()
{
   
   $('#hdnNoteCKey').val('');
   $('#NewNoteTitle').val('');
   $("#dlgNewNote").dialog('open');
        
}

function ShowUpdateNoteDlg() {

    if ($('#hdnNoteCKey').val() != '' || $('#hdnNoteCKey').val() != '0') {
        //populate the tile
        $.get("../Service/SSPService.asmx/NoteTitle", { parm: $('#hdnNoteCKey').val() + '|1|999' }, function (data) {

            var $xml = $(data);

            $('#NewNoteTitle').val($xml.find("string").text());

        });

        $("#dlgNewNote").dialog('open');
    }
    else {
        alert("Selected note not found");
    }
}

function UpdateNoteComment(comment, commentckey)
{
    
    var CommentObject = new Object();
    CommentObject.NoteCKey = $('#hdnNoteCKey').val();
    CommentObject.UserCKey = $('#hdnUserCKey').val();
    CommentObject.UserComment = comment;
    CommentObject.NoteCommentCKey = commentckey;
    CommentObject.Version = $('#hdnDraftVersion').val();
    $.ajax({
        url: '../api/NoteComment/UpdateComment',
        type: 'PUT',
        dataType: 'json',
        data: CommentObject,
        success: function (successMessageObject, textStatusString, xhr) {
            console.log(successMessageObject.StatusCode + successMessageObject.ReasonPhrase);
            //Save comment as part of text
            richEditor.PerformCallback("updatecomment|" + $("#txtProtocolVersion").val() + "|" + $("hdnNoteCKey").val());

        },

        error: function (xhr, textStatus, errorThrown) {
            if (xhr.getResponseHeader('Content-Type').indexOf('application/json') > -1) {
                // only parse the response if you know it is JSON
                var error = $.parseJSON(xhr.responseText);
                console.log('error:' + errorThrown + ", status: " + textStatus + ", message = " + error.Message);
                 alert('Notes.js - ' + error.Message);
            } else {
                alert('Notes.js - Fatal error');
            }

            
            
        }

    });
}

function AddNoteComment(comment)
{
    
    var CommentObject = new Object();
    CommentObject.NoteCKey = $('#hdnNoteCKey').val();
    CommentObject.UserCKey = $('#hdnUserCKey').val();
    CommentObject.UserComment = comment;
    CommentObject.Version = $('#hdnDraftVersion').val();

    $.ajax({
        url: '../api/NoteComment/AddComment',
        type: 'POST',
        dataType: 'json',
        data: CommentObject,
        success: function (ckey, textStatus, xhr) {
            console.log(ckey);
            //insert 
            //richEditor.commands.insertHyperlink.execute({ text: '~', url: key, tooltip: 'commentckey:' + data });
            var userid = $('#hdnUserID').val();
            richEditor.commands.insertHyperlink.execute({ text: '+' + userid, url: ckey, tooltip: 'commentckey:' + ckey });

            //Save comment
            richEditor.PerformCallback("updatecomment|" + $("#txtProtocolVersion").val() + "|" + $("hdnNoteCKey").val());
        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });

}
function SaveReference(refckey, reforder, html) {

    var reference = new Object();

    reference.ReferenceCKey = refckey;
    reference.Number = reforder;
    reference.ReferenceContent = html; // 'test';
    reference.ProtocolVersionCKey = $('#txtProtocolVersion').val();
    reference.UserCKey = $('#hdnUserCKey').val();
    reference.NoteCKey = $('#hdnNoteCKey').val();


    $.ajax({
        url: '../api/reference/SaveReference',
        type: 'POST',
        dataType: 'json',
        data: reference,
        success: function (data, textStatus, xhr) {
            console.log(data);
            console.log("SaveReference");
            loadReferences();
           
            //refresh dropdown
            $.get("../Service/SSPService.asmx/getBookmarks", { parm: $("#hdnNoteCKey").val() + '|' + $('#hdnEditMode').val() + '|' + $('#hdnDraftVersion').val() }, function (data) {

                var $xml = $(data);
                var allref = $xml.find("string").text();
              
                $('#dlgInsertReference').empty();
               
                $('#dlgInsertReference').append(allref);
               

            });
        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
}


/*10/10/2017 - This function called when reference link inside document is ctrl+clicked*/
function ShowInsertReferenceDlg(NoteCKey) {
    
    
    $.get("../Service/SSPService.asmx/getBookmarks", { parm: $("#hdnNoteCKey").val() + '|' + $('#hdnEditMode').val() + '|' + $('#hdnDraftVersion').val() }, function (data) {

        var $xml = $(data);
        var allref = $xml.find("string").text();
        
        $('#dlgInsertReference').empty();
        $('#dlgInsertReference').append(allref);
        $('#dlgInsertReference').css('visibility', 'visible');
        $('#dlgInsertReference').dialog('open');
        if(NoteCKey!=null)
        {
            
            $('#marks').val(NoteCKey);
            showReferenceText();
        }

    });  //get bookmarks

}


function ShowAddCommentDlg(comment, commentckey) {
    //comment and commentckey will be null if called from the button on the toolbar
    //these will have values when called from the click event of the comment itself
    var role = $('#hdnRole').val();
    //alert(role);
    //allow only reviewers and authors to add comment

    if (role.indexOf("1") < 0 && role.indexOf("2") < 0 && role.indexOf("3") < 0)
        {
    //if (role != "3" & role != "1") {
        alert("Only reviewers and authors are allowed to add comments");
        return;
    }

    console.log(commentckey);
    
    if (commentckey != null)
        $('#hdnNoteCommentCKey').val(commentckey);
    else
        $('#hdnNoteCommentCKey').val('');

    //$('#txtNoteComment').val('');
    if (comment == null) {
        CommentEditor.SetHtml('');
    }
    else
    {
        CommentEditor.SetHtml(comment);
    }
        

    $('#dlgInsertComment').css('visibility', 'visible');
    $('#dlgInsertComment').dialog('open');  
}

function ShowAddReferenceDlg(id) {
    //id is in the form - r_3_1.00004100
    
    
    if (id == null)  //add new
    {

        $("#hdnSelectedReference").val('0');
        $("#editreferenceckey").val("0");

        //1-base order
        $("#editreferenceorder").val($("#sortablereferences li").length + 1);
        referenceEditor.SetHtml('');
        $('#dlgAddReference').dialog('open');

    }
    else {
        //convert id to CKeys
        $("#hdnSelectedReference").val(id.substring(2).replace("_", "."));
        $("#editreferenceckey").val(id.substring(2).replace("_", "."));

       
        
        i = GetReferenceOrder(id);
       

        $("#editreferenceorder").val(i);

        //html = $("#" + id).text();
        html = $("#" + id).html();
    
        referenceEditor.SetHtml(html);
        $('#dlgAddReference').dialog('open');
    }

    
}