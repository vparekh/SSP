<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false" CodeBehind="Default.aspx.cs" Inherits="Demo._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        function remove(el, className)
        {
            alert(className + '-' + el.classList.contains(className));
            el.classList.remove(className);

        }

        function findelement(el, html, replacevalue)
        {
            //locates the element with the html of the document
            var test = html; // $("html").html();
            var elementtext = $(el).text();
            

            var id = $(el).attr('id');
            var type = $(el).get(0).tagName.toLowerCase();
                       
            //var p = "(<" + type + " id=" + "\"?" + id + "\"?.*?>)(" + $(el).text() + ")(</" + type + ">)";
            var p = "(<" + type + " id=" + "\"?" + id + "\"?.*?>)((.|[\r\n])*?)(</" + type + ">)";
            //console.log("pattern = " + p);

            patt = new RegExp(p)
           
            var res = test.match(patt);

            if (replacevalue != null)
                test = test.replace(patt, replacevalue);


            //if (res != null)
            //{
            //    console.log("matchedString=" + res[0]);
            //    console.log("startTag=" + res[1]);  //start tag of the diff element 
            //    console.log("innerText=" + res[2]);  //inner text
            //    console.log("endTag=" + res[3]);  //end tag of the diff element
            //}
           
            return [res, test];

            //console.log("innerText=" + $(el).text());
           
        }
        function ListDiff()
        {
            //find all deletes            

            $(".diffdel").each(function (index) {
                //console.log($(this).attr('id') + ":deleted: " + $(this).text() + "<" + $(this).get(0).tagName + '>');
                $('#lstDiffs').append('<option  value=' + $(this).get(0).tagName + '_' + $(this).attr('id') + '_' + $(this).attr('class')
                    + '>deleted: ' + $(this).text() + '</option>'); 
                
            });


            //replaced
            var message = '';
            $(".diffmod").each(function (index) {
                if (index % 2 == 0)
                {
                    //message = $(this).text() + "<" + $(this).get(0).tagName + '>';
                    $('#lstDiffs').append('<option  value=' + $(this).get(0).tagName + '_' + $(this).attr('id') + '_' + $(this).attr('class')
                    + '>deleted: ' + $(this).text() + '</option>');
                }
                    
                else {
                    //message = message + " replaced by " + $(this).text() + "<" + $(this).get(0).tagName + '>';
                    //console.log($(this).attr('id') + ": " + message);

                    //$('#lstDiffs').append('<option  value=' + $(this).get(0).tagName + '_' + $(this).attr('id') + '_' + $(this).attr('class')
                    //+ '>' + message +  ' replaced by ' + $(this).text() + '</option>');

                    //message = "";
                    $('#lstDiffs').append('<option  value=' + $(this).get(0).tagName + '_' + $(this).attr('id') + '_' + $(this).attr('class')
                    + '>inserted: ' + $(this).text() + '</option>');
                }

            });

            $(".mod").each(function (index) {
                //console.log($(this).attr('id') + ":format changed: " + $(this).text() + "<" + $(this).get(0).tagName + '>');
                $('#lstDiffs').append('<option  value=' + $(this).get(0).tagName + '_' + $(this).attr('id') + '_' + $(this).attr('class')
                   + '>format change: ' + $(this).text() + '</option>'); 
            });
           
            $(".diffins").each(function (index) {
                //console.log($(this).attr('id') + ":inserted: " + $(this).text() + "<" + $(this).get(0).tagName + '>');
                $('#lstDiffs').append('<option  value=' + $(this).get(0).tagName + '_' + $(this).attr('id') + '_' + $(this).attr('class')
                   + '>inserted: ' + $(this).text() + '</option>'); 
            });


            $("#lstDiffs option").attr("title", "");
            $("#lstDiffs option").each(function (i) {
                this.title = this.value;
            });

            //Attach a tooltip to select elements
            //$("#lstDiffs").tooltip({
            //    left: 25
            //});
        }

        function Accept()
        {
            
            var id = $("#lstDiffs option:selected").val();
            id =  id.split('_')[1]+ '_' + id.split('_')[2]
          
            //var changetype = id.split('_')[3];
            

            var element = $('#' + id);
            if (element.length == 0) {
                alert('element not found');
                return;
            }
            var changetype = $(element).get(0).tagName.toLowerCase();
            //console.log(element);
            //alert(changetype);
            if (changetype == 'del')
            {
                var html = $('#litDiffText').html();
              
                var res = findelement(element,html,'');
                //replace matched string with empty string
                //console.log(res[1]);
                $('#litDiffText').html(res[1]);

                $("#lstDiffs option:selected").remove();
            }
            if(changetype=='ins')
            {
                var html = $('#litDiffText').html();

                var res = findelement(element, html);
                if (res != null)
                {
                    var innerText = res[0][2];
                    console.log('inner: ' + innerText);
                    res = findelement(element, html, innerText);
                    $('#litDiffText').html(res[1]);
                    $("#lstDiffs option:selected").remove();
                }
                //replace matched string with empty string
                //console.log(res[1]);
               
            }

        }

        function Reject()
        {
            var id = $("#lstDiffs option:selected").val();
            id = id.split('_')[1] + '_' + id.split('_')[2]

            //var changetype = id.split('_')[3];

            var element = $('#' + id);
            if (element.length == 0) {
                alert('element not found');
            }

            var changetype = $(element).get(0).tagName.toLowerCase();

            if (changetype = 'del') {
                var html = $('#litDiffText').html();

                var res = findelement(element, html, element.html());
                //replace matched string with empty string
                console.log(res[1]);
                $('#litDiffText').html(res[1]);

                $("#lstDiffs option:selected").remove();
            }
            if (changetype == 'ins') {
                var html = $('#litDiffText').html();

                var res = findelement(element, html);
                if (res != null) {
                    var innerText = res[0][2];
                    console.log('inner: ' + innerText);
                    res = findelement(element, html, '');
                    $('#litDiffText').html(res[1]);
                    $("#lstDiffs option:selected").remove();
                }
                //replace matched string with empty string
                //console.log(res[1]);

            }
           
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float:left;width:45%;">
            <h2>Original Note</h2>
            <hr />
            <asp:Literal runat="server" ID="litOldText"></asp:Literal>
        </div>
        <div style="float:left;width:45%;margin-left:4px">
            <h2>Revised Note</h2>
            <hr />
            <asp:Literal runat="server" ID="litNewText"></asp:Literal>
        </div>
        <div style="float:left;width:45%">
            <h2>Diff Visualisation</h2>
            <hr />
            <asp:Label runat="server" ID="litDiffText" ClientIDMode="Static"></asp:Label>
        </div>
        <div  style="float:left;width:45%; margin-top:100px; margin-left:20px">

            <asp:Button OnClientClick="ListDiff();return false;" Text="Show Diff" runat="server" />
            <asp:Button OnClientClick="Accept();return false;" Text="Accept" runat="server" />
            <asp:Button OnClientClick="Reject();return false;" Text="Reject" runat="server" />
            <br />
            <asp:ListBox runat="server" ID="lstDiffs" Width="300px" Height="300px" ClientIDMode="Static"></asp:ListBox>
    
        </div>
        
    </form>

   
</body>
</html>
