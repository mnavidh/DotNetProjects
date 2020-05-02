<%@ Page Language="C#" AutoEventWireup="true" Title="CCM Dashboard" CodeFile="Default.aspx.cs"
    Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .LabelHeading
        {
            color: #FFFFFF;
            font-family: Arial;
            font-size: 20px;
            font-weight: bold;
        }
        .tblLayout
        {
            height: 100%;
            width: 100%;
        }
        #header
        {
            height: 70px;
            padding-bottom: 50px;
        }
        html, body
        {
            margin: 0;
            padding: 0;
            height: 100%;
        }
        #wrapper
        {
            min-height: 100%;
            position: relative;
        }
        #header
        {
            padding: 10px;
            background: blue;
        }
        #content
        {
            padding: 10px;
            padding-bottom: 80px; /* Height of the footer element */
        }
    </style>
    <link id="Link1" runat="server" rel="shortcut icon" href="~/Images/favicon.ico" type="image/x-icon" />
    <link id="Link2" runat="server" rel="icon" href="~/Images/favicon.ico" type="image/ico" />
    <script type="text/javascript" language="javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            //eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,location=0,statusbar=0,menubar=0,scrollbars=1,width=1200');");
            eval("page" + id + " = window.open(URL);");
        }   
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <table border="0" cellpadding="0" cellspacing="0" class="tblLayout">
                <tr>
                    <td>
                        <asp:Image ID="imgLogo" runat="server" AlternateText="Logo" ImageUrl="~/Images/halogo_blue.gif" />
                    </td>
                    <td>
                        <asp:Label ID="lblHeader" CssClass="LabelHeading" Text="CCM DASHBOARD" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <b>
            <asp:Label ID="lblMaxLoadDate" runat="server" /></b>
        <div id="content" style="text-align: center">
            <div style="float: left; width: 50%">
                <h3 id="headerMaster" style="text-align: center" runat="server">
                </h3>
                <asp:GridView ID="gridSnapshot" OnRowDataBound="gridSnapshot_RowDataBound" HorizontalAlign="Center"
                    runat="server">
                    <HeaderStyle BackColor="#00BFFF" Font-Bold="true" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:GridView>
            </div>
            <div style="float: left; width: 50%">
                <h3 style="text-align: center">
                    Ageing Incidents*</h3>
                <asp:GridView ID="gridAgeing" OnRowDataBound="gridAgeing_RowDataBound" HorizontalAlign="Center"
                    runat="server">
                    <HeaderStyle BackColor="#00BFFF" Font-Bold="true" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:GridView>                
            </div>
            <div style="float: left; width: 50%">
                <h3 style="text-align: center" runat="server">
                    Support Staff Queue
                </h3>
                <asp:GridView ID="gridSupportQueue" OnRowDataBound="gridSupportQueue_RowDataBound"
                    HorizontalAlign="Center" runat="server">
                    <HeaderStyle BackColor="#00BFFF" Font-Bold="true" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:GridView>
            </div>
        </div>
    </div>

    <div style="text-align: center;clear:both;padding-top:100px">
                    * - Includes CCM with status "Work in progress", "Pending with user" & "Pending
                    with vendor"</div>
    </form>
</body>
</html>
