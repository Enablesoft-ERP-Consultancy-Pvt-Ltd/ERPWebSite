<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="FrmAddCollection.aspx.cs"
    Inherits="Masters_Packing_FrmAddCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closecollection() {
            //window.opener.document.getElementById('btnclose').click();
            self.close();
        }
        function CheckAllCheckBoxes() {
            {
                var gvcheck = document.getElementById('chlDesign');
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
    <div style="margin-left: 50px">
        <table>
            <tr>
                <td class="tdstyle">
                    Customer Code<br />
                    <asp:DropDownList ID="ddcustomercode" Width="170px" runat="server" CssClass="dropdown"
                        OnSelectedIndexChanged="ddcustomercode_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    Type Collection Name<br />
                    <asp:TextBox ID="txttypeofcollection" runat="server" Width="170px" CssClass="textb"></asp:TextBox>
                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="reqtxttypeofcollection" runat="server"
                        ErrorMessage="please Enter Collection Name" ControlToValidate="txttypeofcollection"
                        ForeColor="Red">*</asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div style="margin-left: 50px; height: 150px; width: 400px">
        <div style="float: left; overflow: auto; width: 150px; height: 150px">
            <asp:CheckBoxList ID="chlDesign" CssClass="checkboxbold" runat="server">
            </asp:CheckBoxList>
        </div>
        <div style="float: right; overflow: auto; width: 250px; height: 150px">
            <asp:GridView runat="server" ID="DGCollection" AutoGenerateColumns="False" CssClass="grid-view"
                OnRowDeleting="DGCollection_RowDeleting">
                <HeaderStyle CssClass="gvheader" />
                <AlternatingRowStyle CssClass="gvalt" />
                <RowStyle CssClass="gvrow" />
                <EmptyDataRowStyle CssClass="gvemptytext" />
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerid" runat="server" Text='<%#Bind("Customerid") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDesignId" runat="server" Text='<%#Bind("DesignId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCollectionId" runat="server" Text='<%#Bind("CollectionId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomer" runat="server" Text='<%#Bind("Customer") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Design">
                        <ItemTemplate>
                            <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CollectionName">
                        <ItemTemplate>
                            <asp:Label ID="lblCollection" runat="server" Text='<%#Bind("CollectionName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <table style="margin-left: 200px;">
        <tr>
            <td align="left">
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnSave_Click" />
                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return closecollection();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
