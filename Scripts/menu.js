


$(document).ready(function () {

    setCurrentTab();
    //does not fire upon partial load
    //
    // Set focus on the correct tab in the menu.
    function setCurrentTab() {
        
        var tabID = "#" + $("#tabid").text();
        $(tabID).addClass("current");
    }
});



