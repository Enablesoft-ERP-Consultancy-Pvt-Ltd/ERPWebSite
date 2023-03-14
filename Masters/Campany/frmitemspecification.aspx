<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmitemspecification.aspx.cs"
    Inherits="Masters_Campany_frmitemspecification" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1">
    <title></title>
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Closeitem() {

            self.close();


        }

    </script>
    <script type="text/javascript">
        function validate() {

            if (document.getElementById('DDitem').selectedIndex == 0) {
                alert("Plz Select item");
                document.getElementById('DDitem').focus();
                return false;
            }
        }

        function getbacktostepone() {
            window.location = "frmloommaster.aspx";
        }

        function onSuccess() {
            ta
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');

        }
        function CheckAllCheckBoxes() {
            if (document.getElementById('ChkForAllSelect').checked == true) {
                var gvcheck = document.getElementById('DGDetail');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('DGDetail');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <table>
        <tr>
            <td>
                <b>Category</b>
            </td>
            <td>
                <asp:DropDownList ID="ddcategory" runat="server" CssClass="dropdown" Width="150px"
                    AutoPostBack="true" OnSelectedIndexChanged="ddcategory_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <b>
                    <asp:Label ID="lblitem" runat="server" Text="Item"></asp:Label></b>
            </td>
            <td>
                <asp:DropDownList ID="dditem" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="dditem_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="40%">
        <tr>
            <td align="center" id="selectall" runat="server" colspan="2">
                <asp:CheckBox ID="ChkForAllSelect" runat="server" Text="Select All" ForeColor="Red"
                    onclick="return CheckAllCheckBoxes();" CssClass="checkboxnormal" AutoPostBack="True" />
                <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForRawMaterial" runat="server" Text="For Raw Material" Checked="true"
                                            ForeColor="Red" CssClass="checkboxnormal" AutoPostBack="True" OnCheckedChanged="ChkForRawMaterial_CheckedChanged" />--%>
            </td>
            <td colspan="5" align="right">
                <asp:Button ID="BtnShowData" runat="server" Text="Show Data" ForeColor="White" CssClass="buttonnorm"
                    Visible="false" />
                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnDelete" Text="Delete" runat="server"
                    Visible="false" />
            </td>
        </tr>
    </table>
    <div id="DivDGDetail" style="height: 200px; width: 300px; margin-left: 60px; overflow: scroll">
        <asp:GridView ID="DGDetail" Height="50px" Width="250px" runat="server" AllowPaging="True"
            PageSize="100" AutoGenerateColumns="False" OnRowCreated="DGDetail_RowCreated"
            OnRowDataBound="DGDetail_RowDataBound">
            <HeaderStyle ForeColor="white" BackColor="#0080C0" CssClass="gvheader" />
            <AlternatingRowStyle CssClass="gvalt" />
            <RowStyle CssClass="gvrow" />
            <Columns>
                <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="Chkbox" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblitemid" runat="server" Text='<%#Bind("Item_Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblqualityid" runat="server" Text='<%#Bind("QualityId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="QualityName" HeaderText="SubItem" />
            </Columns>
        </asp:GridView>
    </div>
    <table>
        <tr>
            <td align="right">
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="right">
                <div style="margin-left: 290px;">
                    <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" Visible="false" />
                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click" />
                    &nbsp;<asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm"
                        Visible="false" />
                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                        OnClientClick="return Closeitem();" />
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
