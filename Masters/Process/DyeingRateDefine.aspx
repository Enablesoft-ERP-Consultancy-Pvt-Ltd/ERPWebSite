<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" CodeFile="DyeingRateDefine.aspx.cs" Inherits="Masters_Process_DyeingRateDefine"
    Title="Add Dyeing Rate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        }      
    </script>
    <div>
        <table>
            <tr>
                <td class="tdstyle">
                    Company Name<br />
                    <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="130px" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    Dyer Name
                    <br />
                    <asp:DropDownList CssClass="dropdown" ID="DDDyerName" Width="130px" runat="server"
                        AutoPostBack="True" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDDyerName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdItemCode" runat="server" visible="false" class="tdstyle">
                    TextItemCode
                    <br />
                    <asp:TextBox ID="TextItemCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblCategory" class="tdstyle" runat="server" AutoPostBack="true" Text=""></asp:Label>
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
                    <asp:Label ID="LblItemName" class="tdstyle" runat="server" AutoPostBack="true" Text="Label"></asp:Label>
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
                    <asp:Label ID="LblQuality" class="tdstyle" runat="server" Text="Label"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="110" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDQuality"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdDesign" runat="server" visible="false">
                    <asp:Label ID="LblDesign" class="tdstyle" runat="server" Text="Label"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="110" ID="DDDesign" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDDesign"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdColor" runat="server" visible="false">
                    <asp:Label ID="LblColor" class="tdstyle" runat="server" Text=""></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="110" ID="DDColor" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDColor"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdColorShade" runat="server" visible="false">
                    <asp:Label ID="LblColorShade" class="tdstyle" runat="server" Text=""></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="110" ID="DDColorShade" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDColorShade"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdShape" runat="server" visible="false">
                    <asp:Label ID="LblShape" class="tdstyle" runat="server" Text=""></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDShape" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDShape"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TdSize" runat="server" visible="false">
                    <asp:Label ID="LblSize" class="tdstyle" runat="server" Text="Label"></asp:Label>
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
                    Dyeing Type<br />
                    <asp:DropDownList CssClass="dropdown" ID="DDDyeingType" Width="130px" runat="server">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDDyeingType"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                </td>
                <td class="tdstyle">
                    From QTY<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtFQty"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TxtFQty"
                        ForeColor="Red" ValidationExpression="^\d*[0-9](\.\d*[0-9])?$" ValidationGroup="f1"></asp:RegularExpressionValidator>
                    <br />
                    <asp:TextBox ID="TxtFQty" runat="server" Width="80px"></asp:TextBox>
                    kg.
                </td>
                <td class="tdstyle">
                    To QTY<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtToQty"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtToQty"
                        ForeColor="Red" ValidationExpression="^\d*[0-9](\.\d*[0-9])?$" ValidationGroup="f1"></asp:RegularExpressionValidator>
                    <br />
                    <asp:TextBox ID="TxtToQty" runat="server" Width="80px"></asp:TextBox>
                    kg.
                </td>
                <td class="tdstyle">
                    Rate<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtRate"
                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtRate"
                        ForeColor="Red" ValidationExpression="^\d*[0-9](\.\d*[0-9])?$" ValidationGroup="f1"></asp:RegularExpressionValidator>
                    <br />
                    <asp:TextBox ID="TxtRate" runat="server" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="Lblmessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Button ID="BtnDelete" runat="server" OnClientClick="confirm('Do you want to Delete');"
                        Text="Delete" Visible="False" OnClick="BtnDelete_Click" CssClass="buttonnorm" />
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" ValidationGroup="f1"
                        CssClass="buttonnorm" />
                    <asp:Button ID="BtnPreview" runat="server" Visible="false" OnClientClick="Preview();"
                        Text="Preview" CssClass="buttonnorm" />
                    <asp:Button ID="BtnClose" runat="server" Text="Close" OnClick="BtnClose_Click" CssClass="buttonnorm" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:GridView ID="DGDyeingRateDetail" AutoGenerateColumns="False" AllowPaging="True"
                        OnPageIndexChanging="DGDyeingRateDetail_PageIndexChanging" OnRowDataBound="DGDyeingRateDetail_RowDataBound"
                        runat="server" DataKeyNames="DRateDetailId" OnSelectedIndexChanged="DGDyeingRateDetail_SelectedIndexChanged"
                        CssClass="grid-view" OnRowCreated="DGDyeingRateDetail_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <PagerStyle CssClass="PagerStyle" />
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
