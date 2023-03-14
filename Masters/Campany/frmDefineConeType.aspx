<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDefineConeType.aspx.cs"
    Inherits="Masters_Campany_frmDefineConeType" MasterPageFile="~/ERPmaster.master"
    Title="Define Cone Type" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function newForm() {
            window.location.href = "frmDefineConeType.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function validate() {
            if (document.getElementById("<%=txtConeType.ClientID %>").value == "") {
                alert('Plz Enter Cone Type');
                document.getElementById("<%=txtConeType.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDItem.ClientID %>").value <= "0") {
                alert('Plz Select Item...');
                document.getElementById("<%=DDItem.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDQuality.ClientID %>").value <= "0") {
                alert('Plz Select SubItem...');
                document.getElementById("<%=DDQuality.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDColours.ClientID %>").value <= "0") {
                alert('Plz Select Colours...');
                document.getElementById("<%=DDColours.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtQty.ClientID %>").value == "") {
                alert('Plz Enter Weight');
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            if (document.getElementById('CPH_DDGodown')) {
                if (document.getElementById("<%=DDGodown.ClientID %>").value <= "0") {
                    alert('Plz Select Godown...');
                    document.getElementById("<%=DDGodown.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDLotNo.ClientID %>").value <= "0") {
                alert('Plz Select Lot No...');
                document.getElementById("<%=DDLotNo.ClientID %>").focus();
                return false;
            }

            return confirm('Do you want to save Data');
        }
    </script>
    <asp:UpdatePanel ID="UpdataPanel1" runat="server">
        <ContentTemplate>
            <table style="width: 582px">
                <tr>
                    <td class="tdstyle">
                        <span class="labelbold">Cone Type</span>
                    </td>
                    <td class="style10">
                        <asp:TextBox ID="txtConeType" runat="server" CssClass="textb" TabIndex="1" Width="122px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqBranch" runat="server" ControlToValidate="txtConeType"
                            ErrorMessage="Please Enter The Branch Name" ValidationGroup="M" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Item</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDItem" runat="server" CssClass="dropdown" TabIndex="2" Width="150px"
                            OnSelectedIndexChanged="DDItem_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDItem"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">SubItem</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" TabIndex="3"
                            Width="150px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDQuality"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <span class="labelbold">Colours</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDColours" runat="server" CssClass="dropdown" TabIndex="4"
                            Width="150px" OnSelectedIndexChanged="DDColours_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDColours"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Weight</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQty" CssClass="textb" runat="server" TabIndex="5" Width="122px"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td class="tdstyle" runat="server" visible="false">
                        <span class="labelbold">Godown</span>
                    </td>
                    <td runat="server" visible="false">
                        <asp:DropDownList ID="DDGodown" runat="server" CssClass="dropdown" TabIndex="6" Width="150px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDGodown"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="tdstyle">
                        <span class="labelbold">LotNo</span>
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="DDLotNo" runat="server" CssClass="dropdown" TabIndex="7" Width="150px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDLotNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowSummary="true"
                            ShowMessageBox="false" Font-Bold="true" Font-Italic="true" Font-Names="Times new Roman"
                            Font-Overline="false" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="DGConeType" runat="server" EmptyDataText="No Data Found !" Width="572px"
                            PageSize="6" CellPadding="4" ForeColor="#333333" AllowPaging="True" CssClass="grid-view"
                            DataKeyNames="ID" OnRowDataBound="DGConeType_RowDataBound" AutoGenerateColumns="False"
                            OnPageIndexChanging="DGConeType_PageIndexChanging">
                            <HeaderStyle CssClass="gvheaders" HorizontalAlign="Center" Height="20px" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <PagerStyle CssClass="PagerStyle" />
                            <Columns>
                                <asp:BoundField DataField="ConeType" HeaderText="ConeType" />
                                <asp:BoundField DataField="Item_Description" HeaderText="Item_Description" />
                                <asp:BoundField DataField="LotNo" HeaderText="LotNo" Visible="false" />
                                <asp:BoundField DataField="Qty" HeaderText="Weight" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: right">
                        <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" TabIndex="9"
                            OnClientClick="return newForm();" />
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return validate()"
                            Text="Save" ValidationGroup="M" CssClass="buttonnorm" TabIndex="8" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" TabIndex="10" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
