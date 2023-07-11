<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="AddDyeingRat.aspx.cs"
    Title="Add Dyeing Rate" Inherits="Masters_Process_AddDyeingRat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        }
        function addRate() {
            confirm('Do you want to close');
            self.close();
            //  self.close();
        }
    
    </script>
</head>
<body>
    <form id="frmBank1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--Page Working table--%>
    <div>
        <table>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="Label1" Text="Company Name" runat="server" CssClass="labelbold" />
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="130px" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="Label2" Text=" Dyer Name" runat="server" CssClass="labelbold" />
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDDyerName" Width="130px" runat="server"
                        AutoPostBack="True" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDDyerName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdItemCode" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="Label3" Text=" TextItemCode" runat="server" CssClass="labelbold" />
                    <br />
                    <asp:TextBox ID="TextItemCode" CssClass="textb" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblCategory" class="tdstyle" runat="server" AutoPostBack="true" Text=""
                        CssClass="labelbold"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDCategory"
                        ErrorMessage="please Select Category" ForeColor="Red" SetFocusOnError="true"
                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="110" AutoPostBack="True"
                        runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCategory"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:Label ID="LblItemName" class="tdstyle" runat="server" AutoPostBack="true" Text="Label"
                        CssClass="labelbold"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDItem"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="110" AutoPostBack="True"
                        runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDItem"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdQuality" runat="server" visible="false">
                    <asp:Label ID="LblQuality" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="110" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDQuality"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdDesign" runat="server" visible="false">
                    <asp:Label ID="LblDesign" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="110" ID="DDDesign" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDDesign"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdColor" runat="server" visible="false">
                    <asp:Label ID="LblColor" class="tdstyle" runat="server" Text="" CssClass="labelbold"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="110" ID="DDColor" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDColor"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdColorShade" runat="server" visible="false">
                    <asp:Label ID="LblColorShade" class="tdstyle" runat="server" Text="" CssClass="labelbold"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="110" ID="DDColorShade" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDColorShade"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdShape" runat="server" visible="false">
                    <asp:Label ID="LblShape" class="tdstyle" runat="server" Text="" CssClass="labelbold"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDShape" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDShape"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdSize" runat="server" class="tdstyle" visible="false">
                    <asp:Label ID="LblSize" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                    <asp:CheckBox ID="ChkFt" runat="server" AutoPostBack="True" Text="Ft" OnCheckedChanged="ChkFt_CheckedChanged"
                        Visible="false" />
                    <br />
                    <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDSize" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDSize"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="Label4" Text=" Dyeing Type" runat="server" CssClass="labelbold" />
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDDyeingType" Width="130px" runat="server"
                        OnSelectedIndexChanged="DDDyeingType_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDDyeingType"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="Label5" Text=" From QTY" runat="server" CssClass="labelbold" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtFQty"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TxtFQty"
                        ForeColor="Red" ValidationExpression="^\d*[0-9](\d*[0-9])?$" ValidationGroup="f1"></asp:RegularExpressionValidator>
                    <br />
                    <asp:TextBox ID="TxtFQty" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="Label6" Text="To QTY" runat="server" CssClass="labelbold" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtToQty"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtToQty"
                        ForeColor="Red" ValidationExpression="^\d*[0-9](\d*[0-9])?$" ValidationGroup="f1"></asp:RegularExpressionValidator>
                    <br />
                    <asp:TextBox ID="TxtToQty" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="Label7" Text=" Rate" runat="server" CssClass="labelbold" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtRate"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtRate"
                        ForeColor="Red" ValidationExpression="^\d*[0-9](\.\d*[0-9])?$" ValidationGroup="f1"></asp:RegularExpressionValidator>
                    <br />
                    <asp:TextBox ID="TxtRate" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="Lblmessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:LinkButton ID="lnkgetdata" runat="server" Text="Show Existing Data" OnClick="lnkgetdata_Click"></asp:LinkButton>
                    <asp:Button ID="BtnDelete" runat="server" OnClientClick="confirm('Do you want to Delete');"
                        Text="Delete" Visible="False" OnClick="BtnDelete_Click" CssClass="buttonnorm " />
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                        ValidationGroup="f1" CssClass="buttonnorm" />
                    <asp:Button ID="BtnPreview" runat="server" Visible="false" OnClientClick="Preview();"
                        Text="Preview" CssClass="buttonnorm preview_width" />
                    <asp:Button ID="BtnPreviewExcel" runat="server" Visible="true" OnClick="BtnPreviewExcel_Click"
                        Text="Preview Excel" CssClass="buttonnorm preview_width" Width="100px" />
                    <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="self.close();"
                        CssClass="buttonnorm " />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <div style="max-height: 500px; overflow: auto">
                        <asp:GridView ID="DGDyeingRateDetail" AutoGenerateColumns="False" OnRowDataBound="DGDyeingRateDetail_RowDataBound"
                            runat="server" DataKeyNames="DRateDetailId" OnSelectedIndexChanged="DGDyeingRateDetail_SelectedIndexChanged"
                            CssClass="grid-views">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <asp:BoundField DataField="DRateDetailId" HeaderText="DRateDetailId" Visible="false" />
                                <asp:BoundField DataField="EmpName" HeaderText="Dyer Name" />
                                <asp:BoundField DataField="DyingType" HeaderText="DyingType" />
                                <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" />
                                <asp:BoundField DataField="FromoQty" HeaderText="FQty" />
                                <asp:BoundField DataField="ToQty" HeaderText="TQty" />
                                <asp:BoundField DataField="Rate" HeaderText="Rate" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
