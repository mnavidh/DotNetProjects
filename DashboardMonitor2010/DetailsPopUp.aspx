<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetailsPopUp.aspx.cs" Inherits="DetailsPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CCM Details</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="~/Images/favicon.ico" type="image/x-icon" />
    <link id="Link2" runat="server" rel="icon" href="~/Images/favicon.ico" type="image/ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gridDetails" HorizontalAlign="Center" runat="server" EnableSortingAndPagingCallback="True"
            AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gridDetails_PageIndexChanging"
            OnRowDataBound="gridDetails_RowDataBound" OnSorting="gridDetails_Sorting" PageSize="15">
            <HeaderStyle BackColor="#00BFFF" Font-Bold="true" />
            <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
