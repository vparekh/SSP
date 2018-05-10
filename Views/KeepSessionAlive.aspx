<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KeepSessionAlive.aspx.cs" Inherits="SSPWebUI.Views.KeepSessionAlive" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta id="MetaRefresh" http-equiv="refresh" content="21600;url=KeepAlive.aspx" runat="server" />
    <script language="javascript">
        window.status = "<%=WindowStatusText%>";
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
