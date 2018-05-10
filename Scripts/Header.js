function saveHeader() {
        
    var title = $("#txtTitle").val();
    var subtitle = $("#txtSubtitle").val();
    var ajcc = $("#ajcc :selected").val();
    var protocolversionckey = $("#txtProtocolVersion").val();
    var userckey = $("#hdnUserCKey").val();
    var headerckey = "0";
    
    var hdr = new Object();
    hdr.Title = title;
    hdr.Subtitle = subtitle;
    hdr.ProtocolVersionCKey = protocolversionckey;
    hdr.ProtocolHeaderCKey = headerckey;
    hdr.BaseVersions = ajcc;
    hdr.UserCKey = userckey;

    //cover page
    var coverpagedata = [];
    var coverpage = new Object();

    $.each($('.category'), function (index, value) {
        
        var category = $(this);
        
        $.each($(category).find('.usage_div'), function (i, v) {
            var usage = $(this);
            
            $.each($(usage).find("tr:not(:first)"), function (x, y) {
                var row = $(this);
                
                coverpage = new Object();
                coverpage.CategoryCKey = category.attr("id").replace('_', '.');
                coverpage.UsageCKey = usage.attr("id").split('__')[1].replace('_', '.');
                
                coverpage.ProtocolVersionCKey = protocolversionckey;
                coverpage.CategoryType = $(row).find('.usagename').val();
                coverpage.CategoryTypeComment = $(row).find('.usagedescription').val();
                coverpagedata.push(coverpage);
            })
        });
    })

    hdr.CoverPageData = coverpagedata;
        

    $.ajax({
        url: '../api/ProtocolHeader/Update',
        type: 'POST',
        dataType: 'json',
        data: hdr,
        success: function (data, textStatus, xhr) {
            console.log(data);
        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
    

}