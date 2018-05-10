$(document).ready(function () {

    xhr = null;

    $("#btnSubmit").hide();
    $('#btnSaveAll').hide();
    $('#btnSubmitReview').hide();
    $('#admin').hide();

    $("#test").dialog({
        autoOpen: false
    });

    $("#dlgCopyChecklist").dialog({
        autoOpen: false,
        height: 540,
        width: 500,
        modal: true,
        buttons: {
            "Copy": function () {
                alert("copy - " + copytree.GetFocusedNodeKey());
                copyNodeKey = copytree.GetFocusedNodeKey();
                $('#hdnCopyChecklist').val($('#ddCopyChecklists').val());
            },
            "Close": function () {
                $(this).dialog("close");
            }
        },
        close: function () {

        }  
});
    
$("#dlgAddComment").dialog({
        autoOpen: false,
        height: 540,
        width: 580,
        modal: true,
        buttons: {
            "Add": function () {
                var comment = $('#txtComment').val();

                $(this).dialog("close");
                dlgAddCommentCallback(comment);


            },
            "cancel": function () {
                $(this).dialog("close");
            }
        },
        close: function () {

        }


    });
});   //$(document).ready

function showcopytree()
{
    
    $('#dlgCopyChecklist').dialog('open');
    xhr = $.getJSON("../api/Checklist/GetChecklists", function (data) {
        $('#ddCopyChecklists').empty();

        $.each(data, function (key, val) {
            $('#ddCopyChecklists').append('<option value=' + val.ChecklistCKey + '>' + val.VisibleText + '</option>');

        })

        //select the first checklist by default
        $('#ddCopyChecklists')[0].selectedIndex = 0;

        copytree.PerformCallback('open|' + $('#ddCopyChecklists').val());
       
    });

   
}
function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function loadBreadCrumbs()
{
    $.getJSON("../api/Protocols/GetProtocolName/?ProtocolVersionCKey=" + $("#hdnProtocol").val(), function (data) {
        var tab = $('#hdnSelectedTab').val();
        //var tab = $("#<%=hfTab.ClientID%>").val();
        $('#hdnProtocolName').val(data);
        $('#breadcrumbs').html('Protocols');
        $('#breadcrumbs').html($('#breadcrumbs').html() + ' >> ' + $('#hdnProtocolName').val());
        if (tab == 'headers')
            $('#breadcrumbs').html($('#breadcrumbs').html() + ' >> Title');
        if (tab == 'procedures')
            $('#breadcrumbs').html($('#breadcrumbs').html() + ' >> Cover Page');
        if (tab == 'authors')
            $('#breadcrumbs').html($('#breadcrumbs').html() + ' >> Authors');
        if (tab == 'checklist')
            $('#breadcrumbs').html($('#breadcrumbs').html() + ' >> Case Summary');
        if (tab == 'notes')
            $('#breadcrumbs').html($('#breadcrumbs').html() + ' >> Explanatory Notes');
       
       
    });
}

function AllowEdit(role)
{
    if (data == '-1' || data > '9')
        return false;

    if (data.indexOf("1") >= 0 ) {
        return true;
    }
    else if (data.indexOf("5") >= 0) {
        return true;
    }
    else if (data = '99') {
        return true;
    }
    return false;
}

function LoadContents(protocolid, version) {

    //cancel any ajax
    //if (xhr != null)
    //{
    //    if (xhr && xhr.readyState != 4) {
    //        alert('aborting');
    //        xhr.abort();
    //    }
    //}
        

    //first check if hdnUserCKey still exists
    if ($('#hdnUserCKey').val() == "") {
        alert('Session value (UserCKey) lost. Please log back in.');
        window.location.replace('login.aspx');
    }

    $('#hdnNewItem').val(0);
    var selectedprotocol = 0;
    if (protocolid == null)
        selectedprotocol = $('#hdnProtocol').val(); // $('#ddlProtocols :selected').val();
    else
    {
        selectedprotocol = protocolid;
        $('#hdnProtocol').val(selectedprotocol);
    }
    
    loadBreadCrumbs();

    $('#hdnSelectedNote').val('A');
    
    $('#btnViewComments').hide();
    $('#btnSubmitReview').hide();
   
    var readonly = 0;

    console.log("LoadContents");
    //set allowable actions
    xhr = $.getJSON("../api/Role/GetRole?UserCKey=" + $('#hdnUserCKey').val() + "&ProtocolCKey=" + $('#hdnProtocol').val(), function (data) {
        
        $('#hdnRole').val(data);

        
       
        if (data.indexOf("1") >= 0 & data != '-1' & data.indexOf("16") < 0 & data.indexOf("11")<0) {
            $('#hdnEditMode').val(1);  //allow editing
            SetEditMode(true);           
            readonly = 0;
            $('#lblmode').text("Edit");
            $('#btnViewComments').show();
        }
        else if (data.indexOf("5") >= 0 & data != '-1') {
            SetEditMode(true);
            $('#hdnEditMode').val(1);  //allow editing
            readonly = 0;
            $('#lblmode').text("Edit");
            $("li#admin").show();
            $('#btnViewComments').show();
        }
        else if (data.indexOf("99") >= 0) {  //admin
            $('#hdnEditMode').val(1);  //allow editing
            SetEditMode(true);
            readonly = 0;
            $('#lblmode').text("Edit");
            $('#btnViewComments').show();
            $('#admin').show();
        }
        else {
           
            SetEditMode(false);
            readonly = 1;
            $('#hdnEditMode').val(0);  //do not alow editing
            $('#lblmode').text("View");
        }

       
        //alert(data);
        if (!data.indexOf("3") >= 0) {
            $('#showAddProcedureComment').hide();
            $('#showAddReferencesComment').hide();
            $('#showAddAuthorComment').hide();
            $('#showAddNoteComment').hide();
            $('#showAddSummaryComment').hide();
            $('#showAddCommentBased').hide();
            $('#showAddCommentTitle').hide();
            $('#showAddCommentSubtitle').hide();
           
        }

        if (data.indexOf("3") >= 0) {

            $('#showAddProcedureComment').show();
            $('#showAddReferencesComment').show();
            $('#showAddAuthorComment').show();
            $('#showAddNoteComment').show();
            $('#showAddSummaryComment').show();
            $('#showAddCommentBased').show();
            $('#showAddCommentTitle').show();
            $('#showAddCommentSubtitle').show();
            $('#btnSubmitReview').show();
            $('#lblmode').text("Review");
            $('#btnViewComments').show();

        }

        
        /*reload note in order to fire the Init event on the RichEdit control
        required to lock keys for users who are not authors.
        Cannot just make this control read-only for non-authors since reviewers need to be able 
        to insert inline comments
        ----------------------------------------------------------------------------------
        Be careful this may have increased the severity of "key not found in the dictionary"
        error as this error is thrown from the server callback event upon changing RichEdit's ReadOnly 
        property
        ----------------------------------------------------------------------------------
        */

        //test for key not found error - this is not the solution though since keys need to be locked
        panelNote.PerformCallback();

        //get draft versions
        xhr = $.getJSON("../api/Protocols/GetDraftVersions/?ProtocolVersion=" + $("#hdnProtocol").val(), function (data) {
            var max = 0;
            $('#ddDraftVersions').empty();
            $.each(data, function (key, val) {
                $('#ddDraftVersions').append('<option value=' + val.Version + '>' + "Draft " + val.Version + '</option>');
                if (parseFloat(val.Version) > parseFloat(max))
                    max = val.Version;
            })


            if ($('#hdnEditMode').val() == 1) {
               
                $('#ddDraftVersions').append('<option value=999>Work Version</option>');
                $('#ddDraftVersions').val(999);
                $('#hdnDraftVersion').val(999);
            }
            else {
                $('#ddDraftVersions').val(max);
                $('#hdnDraftVersion').val(max);
            }

            if (version != null) {
                $('#ddDraftVersions').val(version);
                $('#hdnDraftVersion').val(version);
                
                if(version.trim()!="999")
                {  
                    $('#hdnEditMode').val(0);
                    SetEditMode(0);                    
                }
                else {
                    
                    $('#hdnEditMode').val(1);
                    SetEditMode(1);
                }

            }

            var selectedtab = $('#hfTab');
            if (selectedtab.val() == "authors") {
                
                if ($('#hdnEditMode').val() == 0 || $('#hdnDraftVersion').val() != "999") {
                    PopulateReadonlyAuthor();
                }
                else {
                    PopulateEditableAuthor();
                }
            }

            xhr = $.getJSON("../api/ProtocolHeader/GetHeader?ProtocolCKey=" + selectedprotocol
            + "&Mode=" + $('#hdnEditMode').val() + "&DraftVersion=" + $('#hdnDraftVersion').val(),
                function (data) {

                    $('#txtTitle').val(data.Title);
                    //$('#txtSubtitle').val(data.Subtitle);
                    $('#txtProtocol').val(data.ProtocolCKey);
                    $('#txtProtocolValue').val(data.ProtocolName);
                    $('#txtProtocolGroup').val(data.ProtocolGroupCKey);
                    $('#txtProtocolGroupValue').val(data.ProtocolGroup);
                    $('#txtProtocolVersion').val(data.ProtocolVersionCKey);
                    $('#txtProtocolVersionValue').val(data.ProtocolVersion);
                    var versions = data.BaseVersions;
                    var arr = versions.split(',');
                    $.each(arr, function (key, code) {
                        $("#ajcc option[value='" + code + "']").attr("selected", "true");
                    })


                    
                    var coverpagedata = data.CoverPageData;
                    initializecoverpagedata();
                    
                    $.each(coverpagedata, function (index, val) {
                        var categoryckey = val.CategoryCKey;
                        var usageckey = val.UsageCKey;
                        var category = val.Category;
                        var usage = val.Usage;

                        var categorytype = val.CategoryType;
                        var categorytypecomment = val.CategoryTypeComment;

                        addusage(categoryckey, category, usageckey);

                        usageid = categoryckey.toString().replace('.', '_') + "__"
                            + usageckey.toString().replace('.', '_');
                        addrow("usage_" + usageid, categorytype, categorytypecomment);

                    })                    

                    //the circle and rowcommand will need to be invisible if the user is not an author
                    if ($('#hdnEditMode').val() == "0") {
                        $('.circle').hide();
                        $('.rowcommand').hide();
                    }
                    else {
                        $('.circle').show();
                        $('.rowcommand').show();
                    }
                });
                
            //load procedure
            xhr = $.getJSON("../api/Procedure/GetProcedure/?ProtocolCKey=" + $("#hdnProtocol").val()
                + "&Mode=" + $('#hdnEditMode').val() + "&DraftVersion=" + $('#hdnDraftVersion').val(), function (data) {
                
                    myprocedureEditor.SetHtml(data);

                    //set enabled here?
                    
                        if ($('#hdnEditMode').val() == 0) {
                            myprocedureEditor.SetEnabled(false);
                            
                        }
                        else {
                            myprocedureEditor.SetEnabled(true);
                            
                        }

            });

            //IMP---10/10/2017 - moving loadnoteslist() to OnCallback event of pnlNptes in NoteEditorTest.ascx
            //loadnoteslist();

            

            xhr = $.getJSON("../api/Checklist/Get?ProtocolCKey=" + selectedprotocol, function (data) {
                $('#ddChecklists').empty();
                
                $.each(data, function (key, val) {
                    $('#ddChecklists').append('<option value=' + val.ChecklistCKey + '>' + val.VisibleText + '</option>');

                })

                //select the first checklist by default
                $('#ddChecklists')[0].selectedIndex = 0;
                $('#hdnChecklistCKey').val($('#ddChecklists').val());

                //clear
                $('#txtVisibleText').val("");
                $('#txtItemComment').val("");
                $('#myReportOptions').prop('checked', false);
                $('#chkMustAnswer').prop('checked', false);
                $('#chkMustImplement').prop('checked', false);
                $('#chkSDC').prop('checked', false);
                $('#chkSDS').prop('checked', false);
                $('#myItemTypes').val("");
                $('#myUnitTypes').val("");
                $('#myReportOptions').val("");
                $('#hdnCurrentItem').val("");

                //IMP 10/10/2017
                //needed to reload treelist to enable/disable drag-drop
                CaseSummaryCallbackPanel.PerformCallback();  

                //this call is now moved to CaseSummaryLight.ascx on 
                //EndCallback event of CaseSummaryCallbackPanel

                //mytreeList.PerformCallback('open|' + $('#ddChecklists').val());

            })
        });

        
        //load list versions list
        xhr = $.getJSON("../api/BaseVersion/GetVersions", function (data) {
            $('#ajcc').empty();
            //add options
            $.each(data, function (key, val) {
                $('#ajcc').append('<option value=' + val.Code + '>' + val.Label + '</option>');

            }); //each              

        });

    }).fail(function (jqxhr, textStatus, error)
        /*call to GetRole failed - most likely it timed out. 
             However, getJSON does not seem to return the actual 
             error message since it tries to read it as JSON object,
             so the error message seems related to data conversion
              - better to use $.ajax instead of $.getJSON
        */
        {
            var err = textStatus + ", " + error;
            console.log("Ajax call failed while loading data in LoadContents: " + err);
            window.location.replace('login.aspx');
        }
    );

    
    //
    return false;
}

function activateDevExpressControls(tabId) {

    
    if (document.getElementById('tabbeddialog')) {
        ASPxClientControl.AdjustControls(document.getElementById(tabId));
    }

}

function LoadProtocols() {
    alert('loadprotocols');
    return false;
    $('hdnProtocolGroup').val($('#ddlProtocolGroups :selected').val());
    $('#ddlProtocols').empty();
    $.getJSON("../api/Protocols?ProtocolGroup=" + $('#ddlProtocolGroups :selected').val(),
    function (data) {
        $.each(data, function (key, val) {

            $('#ddlProtocols').append('<option value=' + val.code + ">" + val.label + '</option>');

        }); //

        $("#ddlProtocols > option").each(function () {
            var item = $(this).text();
           
            if (item !== "Select") {
               
                $(this).attr("title", 'test');
            }
        });
    })
}

function SetEditMode(mode) {
    
    if (mode == false) {
        $('#btnNewNote').hide();
        $('#btnDeleteNote').hide();
        $('#btnSaveNote1').hide();
        $('#btnNewReference').hide();
        $('#btnDeleteReference').hide();
        $('#btnSaveReference').hide();
        $('#btnNewItem').hide();
        $('#btnDeleteItem').hide();
        $('#btnAddAuthorRole').hide();
        $('#divAddAuthor').hide();
        $('#allowtreeedit').val('false');

        $("#btnSubmit").hide();
        $('#btnSaveAll').hide();

        $('#savesummary').hide();
       
        $('#saveprocedure').hide();
        $('#saveheader').hide();
        $('#txtTitle').attr("readonly", true);
        $('#txtSubtitle').attr("readonly", true);
        $('#txtProtocol').attr("readonly", true);
        $('#txtProtocolValue').attr("readonly", true);
        $('#ajcc').attr("disabled", true);
       

        $('#txtVisibleText').attr('readonly', true);
        $('#txtVisibleText').addClass('input-disabled');
        $('#myReportOptions').attr('disabled', true);
        $('#myItemTypes').attr('disabled', true);
        $('#myUnitTypes').attr('disabled', true);

        $('#chkSDC').attr('disabled', true);
        $('#chkSDS').attr('disabled', true);
        $('#chkMustImplement').attr('disabled', true);
        $('#chkMustAnswer').attr('disabled', true);
        $('#myNotesList').attr('disabled', true);
        $('#txtItemComment').attr('readonly', true);
       
        $('#btnAddReference').hide();
        $('#btnDeleteReference').hide();
        $('#btnUpdateReference').hide();

        $('#btnAddNote').hide();
        $('#btnDeleteNote').hide();
        $('#btnUpdateNote').hide();

        //this statement seems to generate errors if htmleditor is not loaded already
        //move to Init event?
        //myprocedureEditor.SetEnabled(false);
    }
    else {
        $('#btnNewNote').show();
        $('#btnDeleteNote').show();
        $('#btnSaveNote1').show();
        $('#btnNewReference').show();
        $('#btnDeleteReference').show();
        $('#btnSaveReference').show();
        $('#btnNewItem').show();
        $('#btnDeleteItem').show();
        $('#btnAddAuthorRole').show();
        $('#divAddAuthor').show();
        $('#allowtreeedit').val('true');
        $("#btnSubmit").show();
        $('#btnSaveAll').show();
        $('#saveprocedure').show();
        $('#btnAddNote').show();
        $('#btnAddReference').show();
        $('#savesummary').show();
        $('#saveheader').show();
      
        $('#txtTitle').attr("readonly", false);
        $('#txtSubtitle').attr("readonly", false);
        $('#txtProtocol').attr("readonly", false);
        $('#txtProtocolValue').attr("readonly", false);
        $('#ajcc').attr("disabled", false);

        $('#txtVisibleText').attr('readonly', false);
        $('#txtVisibleText').removeClass('input-disabled');
        $('#myReportOptions').attr('disabled', false);
        $('#myItemTypes').attr('disabled', false);
        $('#myUnitTypes').attr('disabled', false);

        $('#chkSDC').attr('disabled', false);
        $('#chkSDS').attr('disabled', false);
        $('#chkMustImplement').attr('disabled', false);
        $('#chkMustAnswer').attr('disabled', false);
        $('#myNotesList').attr('disabled', false);
        $('#txtItemComment').attr('readonly', false);
        
        $('#btnAddReference').show();
        $('#btnDeleteReference').show();
        $('#btnUpdateReference').show();

        $('#btnAddNote').show();
        $('#btnDeleteNote').show();
        $('#btnUpdateNote').show();

        //myprocedureEditor.SetEnabled(true);
    }
}

function Preview()
{
    
    window.open("preview.aspx?TemplateVersion=" + $('#hdnProtocol').val() + "&version=" + $('#ddDraftVersions').val(), '_blank', 'location=yes,height=570,width=520,scrollbars=yes,status=yes,menubar=yes');
}
function UpdateReviewStatus(status) {
    var submitreq = new Object();

    submitreq.Status = status;
    submitreq.ProtocolCKey = $("#txtProtocolVersion").val(); 
    submitreq.ReviewerCKey = $("#hdnUserCKey").val();
    
    $.ajax({
        url: '../api/ReviewStatus/UpdateStatus',
        type: 'PUT',
        dataType: 'json',
        data: submitreq,
        success: function (data, textStatus, xhr) {
            console.log(data);
        },


        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
}

function UpdateEditStatus(status) {
    var submitreq = new Object();

    submitreq.Status = status;
    submitreq.ProtocolCKey = $("#txtProtocolVersion").val();
    submitreq.AuthorCKey = $("#hdnUserCKey").val();
    
    $.ajax({
        url: '../api/EditStatus/UpdateStatus',
        type: 'PUT',
        dataType: 'json',
        data: submitreq,
        success: function (data, textStatus, xhr) {
            console.log(data);
        },


        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
}

function GetEditStatus() {
    
    //first check if the protocol edits is done
    $.getJSON("../api/EditStatus/?ProtocolCKey=" + $("#txtProtocolVersion").val(),
     function (data) {
         var comments = '';
         return data.status;
     })
}

function saveAll(auto) {

    //check if autosave enabled
    var autosaveenabled = $('#hdnAutosave').val();
    var save = true;

    if (auto && autosaveenabled == 0)
        save = false;

    //save header
    if (save)
    {
        saveHeader();

        //save coverpage
        SaveProcedure();

        //save current note
        richEditor.PerformCallback("save|" + $("#txtProtocolVersion").val() + "|" + $("hdnNoteCKey").val());
    }
    

    //no need to save author
    // nor checklist since they are saved automatically after each update.
    if (!auto)
    {
        $.confirm({
            title: 'Success',
            content: 'All your changes are saved. These changes are made to your work copy only and will not be visible to reviewers until you Submit Draft.',
            type: 'green',
            boxWidth: '500px',
            useBootstrap: false,
            typeAnimated: true,
            buttons: {
                close: function () {
                }
            }
        });
    }
    
    
    
}

function SubmitDraft() {

    
    //ask user to confirm submit
    $.confirm({
        title: 'Submit Draft?',
        content: 'You are about to release a new draft with all your changes. Reviewers will be able to view these changes and will be able to comment.<br/><br/>Do you want to proceed?',
        type: 'orange',
        boxWidth: '500px',
        useBootstrap: false,
        typeAnimated: true,
        buttons: {
            yes: function () {
                var submitreq = new Object();

                submitreq.Status = 'C';
                submitreq.ProtocolCKey = $("#txtProtocolVersion").val();
                submitreq.AuthorCKey = $("#hdnUserCKey").val();

                $.ajax({
                    url: '../api/Workflow/SubmitDraft',
                    type: 'PUT',
                    dataType: 'json',
                    data: submitreq,
                    success: function (data, textStatus, xhr) {
                        $.confirm({
                            title: 'Success',
                            content: 'You have successfully submitted all your draft version.',
                            type: 'green',
                            boxWidth: '500px',
                            useBootstrap: false,
                            typeAnimated: true,
                            buttons: {
                                close: function () {
                                }
                            }
                        });
                        console.log(data);
                    },

                    error: function (xhr, textStatus, errorThrown) {
                        console.log('Error in Operation');
                    }

                });

            },
            no: function () { },

        }
    });
}

function SubmitReview() {
    var submitreq = new Object();

    submitreq.Status = 'C';
    submitreq.ProtocolCKey = $("#txtProtocolVersion").val();
    submitreq.DraftVersion = $('#hdnDraftVersion').val();
    submitreq.ReviewerCKey = $("#hdnUserCKey").val();
    // url: '../api/ReviewStatus/UpdateStatus',
    $.ajax({
        url: '../api/Workflow/SubmitReview',
        type: 'PUT',
        dataType: 'json',
        data: submitreq,
        success: function (data, textStatus, xhr) {
            console.log(data);

            $.confirm({
                title: 'Success',
                content: 'Thank you for your review! Your review has been submitted.',
                type: 'green',
                boxWidth: '500px',
                useBootstrap: false,
                typeAnimated: true,
                buttons: {

                    close: function () {
                    }
                }
            });
        },


        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in SubmitReview - ' + textStatus + ', ' + errorThrown);
        }

    });
}