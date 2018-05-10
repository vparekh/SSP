
$(document).ready(function () {
    $("#dlgViewComments").dialog({
        autoOpen: false,
        height: 540,
        width: 580,
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close"); //closing on Ok click
            }
        }
    });
});

function dlgAddCommentCallback(usercomment) {


    var comment = new Object();
    comment.UserComment = usercomment;
    comment.ReviewItem = $('#hdnReviewItem').val();
    comment.UserCKey = $('#hdnUserCKey').val();
    comment.ReviewItemCKey = $('#hdnReviewItemCKey').val();
    comment.ProtocolVersionCKey = $('#txtProtocolVersion').val();
    comment.DraftVersion = $('#ddDraftVersions').val();
    $.ajax({
        url: '../api/comments/AddComment',
        type: 'POST',
        dataType: 'json',
        data: comment,
        success: function (data, textStatus, xhr) {

            //update review status

            //UpdateReviewStatus('S');

            console.log(data);
        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
}

function OpenCommentBox(reviewitem, reviewitemckey) {
    
    //$.getJSON("../api/Workflow/GetWorkflowStatus?ProtocolCKey=" + $("#txtProtocolVersion").val()
    //first check if the protocol edits is done
    
    $.getJSON("../api/Workflow/GetWorkflowReviewStatus?ProtocolCKey=" + $("#txtProtocolVersion").val()
        + "&DraftVersion=" + $('#hdnDraftVersion').val() + "&ReviewerCKey=" + $('#hdnUserCKey').val(),
     function (data) {
         
         var comments = '';
         if (data.Status != 2.100004300) {
             var msg = ''
             if (data.Status == 3.100004300)
             {
                 msg = 'You have already submitted your reviews. You can view but not add any more comments.';
             }
             else
             {
                 msg = 'Primary author has not submitted this draft for your review.'
             }

             $.confirm({
                 title: 'Not available!',
                 content: msg,
                 type: 'red',
                 boxWidth: '500px',
                 useBootstrap: false,
                 typeAnimated: true,
                 buttons: {

                     close: function () {
                     }
                 }
             });
             
             //if (data.Status != 5.100004300)
                return;
         };
                
         if (data.Status == 2.100004300)
         {
             $('#txtComment').val('');     
             
             $('#dlgAddComment').dialog('open');

             $('#hdnReviewItem').val(reviewitem);
             $('#hdnReviewItemCKey').val(reviewitemckey);

             var role = $('#hdnRole').val()
             if ((role.indexOf("1") >= 0 & role != '-1') || (role.indexOf("99") >= 0)
                    || (role.indexOf("5") >= 0)) {
                 reviewerckey = "";
             }

             console.log('in here');
             $.getJSON("../api/comments/GetComments?protocolckey=" + $("#txtProtocolVersion").val()
                 + "&draftversion=" + $('#ddDraftVersions').val()
                 + "&reviewitem=" + reviewitem + "&reviewitemckey="
                 + reviewitemckey + "&reviewerckey=" + $("#hdnUserCKey").val(),
            function (data) {
                var comments = '';

                $.each(data, function (key, val) {
                    comments = comments + "<p><b>" + val.DateAdded + ' - ' + val.UserId + '</b> :' + val.UserComment + '</p>';
                }); //each

                if (comments != '')
                    comments = "<p><b>Previous Comments</b></p>" + comments;

                $('#prevcomments').html(comments);
            });  //data
         }
        
     });
}

function ViewComment(reviewitem, reviewitemckey, reviewerckey) {
    
    $('#dlgViewComments').dialog('open');

    var protocol = $("#txtProtocolVersion").val()
    //if user is the author she should be able to view all reviewers' comments

    data = $('#hdnRole').val()
    //$.getJSON("../api/Role/GetRole?UserCKey=" + $('#hdnUserCKey').val() + "&ProtocolCKey=" + $('#hdnProtocol').val(), function (data) {
    if ((data.indexOf("1") >= 0 & data != '-1') || (data.indexOf("99") >= 0) 
        || (data.indexOf("5") >= 0))
        {
            reviewerckey = "";
        }

        //if (data.indexOf("99") >= 0) {
        //    reviewerckey = "";
        //}

        //if (data.indexOf("5") >= 0) {
        //    reviewerckey = "";
        //}
        console.debug('in comments');
        $.getJSON("../api/comments/GetComments?protocolckey=" + protocol + "&draftversion=" + $('#ddDraftVersions').val()
            + "&reviewitem="
            + reviewitem + "&reviewitemckey=" + reviewitemckey + "&reviewerckey=" + reviewerckey,
         function (data) {

             //remove all rows except the first
             $("#usercomments").find("tr:gt(0)").remove();

             //add rows
             $.each(data, function (key, val) {
                 $('#usercomments > tbody:last-child').append('<tr><td style="padding:2px">'
                     + val.UserId + '</td><td style="padding:2px">'
                     + val.DateAdded + '</td><td style="padding:2px">'
                     + val.ReviewItem + '</td><td style="padding:2px">'
                     //+ val.ReviewItemCKey + '</td><td style="padding:2px">'
                     + val.UserComment + '</td></tr>');
             }); //each

         });  //data
    //})
   
}

