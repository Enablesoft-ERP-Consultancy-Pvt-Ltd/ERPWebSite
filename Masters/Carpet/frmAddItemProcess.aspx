<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAddItemProcess.aspx.cs"
    Inherits="Masters_Process_frmAddItemProcess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Jobs</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 800px; background-color: #B5C7DE">
        <div style="margin-top: 20px; margin-left: 10px">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblItemName" runat="server" Style="font-weight: bold; font-size: 20px;
                            color: Red" Text="MK"></asp:Label>
                    </td>
                    <td id="TDquality" runat="server">
                        <asp:Label ID="lblquality" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>
                        <asp:DropDownList ID="DDQuality" CssClass="dropdown" Width="200px" AutoPostBack="true"
                            runat="server" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>

                        <asp:RadioButtonList ID="rdbtnLst" runat="server" RepeatDirection="Horizontal" DataValueField="ItemId" DataTextField="ItemName"></asp:RadioButtonList>

                    </td>
                    <td id="TDDesign" runat="server" visible="false">
                        <asp:Label ID="Label1" runat="server" Text="DESIGN NAME" CssClass="labelbold"></asp:Label>
                        <asp:DropDownList ID="DDDesign" CssClass="dropdown" Width="200px" AutoPostBack="true"
                            runat="server" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 800px; height: 250px; margin-top: 30px">
            <div style="float: left; margin-left: 30px; width: 220px; overflow: scroll">
                <span style="color: Black; font-weight: bold; font-size: 20px">Jobs</span>
                <asp:ListBox ID="lstProcess" runat="server" Width="200px" Height="200px" SelectionMode="Single">
                </asp:ListBox>
            </div>
            <div style="float: left; margin-left: 30px; margin-top: 50px">
                <asp:Button ID="btngo" runat="server" Text=">>" Width="50px" OnClick="btngo_Click" /><br />
                <br />
                <asp:Button ID="btnDelete" runat="server" Text="<<" Width="50px" OnClick="btnDelete_Click" />
            </div>
            <div style="float: right; margin-right: 190px; width: 220px; overflow: scroll">
                <span style="color: Black; font-weight: bold; font-size: 20px">Items Job Sequence</span>
                <asp:ListBox ID="lstSelectProcess" runat="server" Width="200px" Height="200px" SelectionMode="Multiple">
                </asp:ListBox>
            </div>
        </div>
        <div style="width: 607px;">
            <table style="width: 607px;">
                <tr>
                    <td align="right">
                        &nbsp;
                        <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" Width="75px"
                            OnClick="btnsave_Click" />
                        &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="buttonnorm" Text="Close"
                            Width="75px" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
