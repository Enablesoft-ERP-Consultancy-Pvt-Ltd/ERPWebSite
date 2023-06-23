<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddItemName.aspx.cs" Inherits="Masters_Carpet_AddItemName"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            if (window.opener.document.getElementById('refreshitem11')) {
                window.opener.document.getElementById('refreshitem11').click();
                self.close();

            }
            else if (window.opener.document.getElementById('refreshitem')) {
                window.opener.document.getElementById('refreshitem').click();
                self.close();

            }
            else if (window.opener.document.getElementById('CPH_Form_refreshitemdr')) {
                window.opener.document.getElementById('CPH_Form_refreshitemdr').click();
                self.close();

            }
            else if (window.opener.document.getElementById('CPH_Form_BtnRefreshItem')) {
                window.opener.document.getElementById('CPH_Form_BtnRefreshItem').click();
                self.close();
            }

            else { }
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 131px;
        }
        .style2
        {
            width: 126px;
        }
        .style3
        {
            width: 89px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <div style="height: 400px">
                    <asp:TextBox ID="txtid" runat="server" Visible="false" Text="0"></asp:TextBox>
                    <table width="50%">
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="lblcategoryname" runat="server" Text="CATEGORY" Font-Bold="true"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList CssClass="dropdown" ID="ddCategory" runat="server" Width="150px"
                                    OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Plese Select Category"
                                    ControlToValidate="ddCategory" runat="server" ForeColor="red" InitialValue="0">*</asp:RequiredFieldValidator>
                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCategory"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td align="right" class="tdstyle">
                                <asp:Label ID="lblitemname" runat="server" Text="ITEM NAME" Font-Bold="true"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName" runat="server" CssClass="textb">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtItemName"
                                    ErrorMessage="Please Enter Item Name" ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkFlagFixWeight" CssClass="checkboxbold" runat="server" Text="For Fix Or Weight" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label1" runat="server" Text="UNIT TYPE" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="dropdown" ID="ddUnit" runat="server" Width="150px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddUnit"
                                    ErrorMessage="Please Select Unit Type" ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddUnit"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td align="right" class="tdstyle">
                                <asp:Label ID="lblitemcode" runat="server" Text="ITEM CODE" Font-Bold="true"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCode" runat="server" CssClass="textb" onkeypress="return isNumberKey(event)"
                                    onkeydown="return (event.keyCode!=13);">
                                </asp:TextBox>
                            </td>
                            <td id="TDkatiwithexportsize" runat="server" visible="false">
                                <asp:CheckBox ID="chkkatiwithexportsize" CssClass="checkboxbold" runat="server" Text="Kati With Export Size" />
                            </td>
                        </tr>
                        <tr id="tr2" runat="server" visible="true">
                            <td class="tdstyle">
                                <asp:Label ID="Label2" runat="server" Text="ITEM TYPE" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="dropdown" ID="ddlItemType" runat="server" Width="150px" />
                            </td>
                            <td id="TDMasterQualityType" align="left" class="tdstyle" runat="server" visible="false">
                                <asp:Label ID="Label3" runat="server" Text="Master QualityType" Font-Bold="true"></asp:Label>
                            </td>
                            <td id="TDMasterQualityType2" runat="server" visible="false">
                                <asp:DropDownList CssClass="dropdown" ID="DDMasterQualityType" runat="server" Width="150px" />
                            </td>
                            <td class="tdstyle">
                                <asp:CheckBox ID="ChkForCushionTypeItem" CssClass="checkboxbold" runat="server" Text="Cushion Type Item" />
                            </td>
                            <td class="tdstyle">
                                <asp:CheckBox ID="ChkForSizeWiseConsumption" CssClass="checkboxbold" runat="server"
                                    Text="For Size Wise Consumption" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" />
                                <asp:ValidationSummary runat="server" ID="vs" ShowMessageBox="true" ShowSummary="false"
                                    HeaderText="Mandatory fields:" />
                                <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" OnClientClick="CloseForm();"
                                    Text="Close" ValidationGroup="m" />
                                <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                    CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" ValidationGroup="m" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" colspan="4">
                                <asp:Label ID="Lblerr" runat="server" Text="" ForeColor="Red"></asp:Label>
                                <asp:GridView ID="gdItem" runat="server" Width="80%" Height="600px" OnSelectedIndexChanged="gdItem_SelectedIndexChanged"
                                    OnRowDataBound="gdItem_RowDataBound" PageSize="50" DataKeyNames="Sr_No" AllowPaging="True"
                                    OnPageIndexChanging="gdItem_PageIndexChanging" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
