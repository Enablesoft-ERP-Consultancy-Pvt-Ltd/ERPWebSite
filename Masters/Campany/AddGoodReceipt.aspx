<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddGoodReceipt.aspx.cs" Inherits="Masters_Campany_GoodReceipt"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            var opener = window.opener;
            if (opener != null) {
                window.opener.document.getElementById('CPH_Form_cariage').click();
                self.close();
            }
            else {
                window.opener.document.getElementById('cariage').click();
                self.close();
            }

        }
      
    </script>
    <style type="text/css">
        .style2
        {
            width: 152px;
        }
        .textbox
        {
        }
        .style3
        {
            width: 85px;
        }
    </style>
</head>
<body>
    <form id="frmgoods" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-left: 15%; margin-right: 15%">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl1" Text="Station Name" runat="server" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtStationname" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter StationName"
                                ControlToValidate="txtStationName" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdGoods" runat="server" Width="280px" AllowPaging="True" CellPadding="4"
                                PageSize="6" ForeColor="#333333" OnPageIndexChanging="gdGoods_PageIndexChanging"
                                OnRowDataBound="gdGoods_RowDataBound" OnSelectedIndexChanged="gdGoods_SelectedIndexChanged"
                                DataKeyNames="Sr_No" CssClass="grid-views" OnRowCreated="gdGoods_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <table style="width: 83%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                                    ValidationGroup="m" />
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClick="btnclose_Click"
                                    OnClientClick="return CloseForm();" />
                                <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                                    OnClick="btndelete_Click" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
