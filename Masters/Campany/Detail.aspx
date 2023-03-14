<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Masters_Campany_Detail"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Detail</title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
            //parent.document.location.reload();
        }
        function RefreshCombo() {
            if (window.opener.document.getElementById('x')) {
                window.opener.document.getElementById('x').click();
                self.close();
            }
            else if (window.opener.document.getElementById('Button3')) {
                window.opener.document.getElementById('Button3').click();
                self.close();
            }

        }
        function closedetail() {
            self.close();
        }
        function validateSave() {
            alert('hi')
            var varacno = document.getElementById('txtacno').value;
            if (varacno == "") {
                alert('Please select Acno ...........');

            }
        }
     
    </script>
    <style type="text/css">
        .gridview
        {
        }
        .style6
        {
            width: 114px;
        }
    </style>
</head>
<body>
    <form id="detail" runat="server">
    <table>
        <tr>
            <td colspan="2" class="tdstyle">
                Name
            </td>
            <td>
                <asp:TextBox CssClass="textb" ID="txtname" runat="server" Width="125px"></asp:TextBox>
            </td>
            <td>
                Father's Name
            </td>
            <td>
                <asp:TextBox CssClass="textb" ID="txtfathername" runat="server" Width="125px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdstyle" colspan="2">
                RESI. Address
            </td>
            <td>
                <asp:TextBox ID="txtresiaddress" CssClass="textb" runat="server" Width="125px" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>
                TEL No
            </td>
            <td>
                <asp:TextBox ID="txttelno" CssClass="textb" runat="server" Width="125px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style6">
            </td>
            <td style="text-align: right" colspan="5">
                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnSave_Click" />
                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return closedetail();" />
                <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                    Text="Delete" Visible="false" OnClick="btndelete_Click" />
            </td>
        </tr>
        <tr>
            <td class="style6">
                <asp:Label ID="lblErr" runat="server" CssClass="labelbold" ForeColor="Red" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <div style="width: 400px; height: 149px; overflow: scroll">
                    <asp:GridView ID="Gvdetail" runat="server" CssClass="grid-view" AutoGenerateColumns="false"
                        Width="373px" OnRowCreated="Gvdetail_RowCreated" DataKeyNames="detailid" OnRowDataBound="Gvdetail_RowDataBound"
                        OnSelectedIndexChanged="Gvdetail_SelectedIndexChanged">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:TemplateField HeaderText="DName">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblDName" runat="server" Text='<%#Bind("DName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fathers_Name">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblFathers_Name" runat="server" Text='<%#Bind("Fathers_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RESI_Address">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblRESI_Address" runat="server" Text='<%#Bind("RESI_Address") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TEL_No">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTEL_No" runat="server" Text='<%#Bind("TEL_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
