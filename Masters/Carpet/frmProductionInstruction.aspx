<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmProductionInstruction.aspx.cs"
    Inherits="Masters_frmProductionInstruction" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx");
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%;">
        <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="False"></asp:TextBox>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 50;">
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategory" runat="server" Text="CATEGORY" CssClass="labelbold"></asp:Label>
                        </td>
                        &nbsp;
                        <td>
                            <asp:DropDownList ID="ddlcategory" CssClass="dropdown" runat="server" Width="165px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlcategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="ITEM NAME" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDMasterQulaty" runat="server" Width="165px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDMasterQulaty_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDMasterQulaty"
                                ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDMasterQulaty"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDQualityName" runat="server" Width="165px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDQualityName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDQualityName"
                                ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDQualityName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text="DESIGN NAME" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDDesignName" runat="server" Width="165px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDDesignName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DDDesignName"
                                ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDDesignName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trLoss" runat="server">
                        <td id="TDLossLabel" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="LOSS" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="TDLossText" runat="server" visible="false">
                            <asp:TextBox ID="TxtLoss" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                Width="159px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trHSCode" runat="server">
                        <td>
                            <asp:Label Text="HS CODE" ID="lblss" runat="server" CssClass="labelbold" Visible="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txthscode" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                Width="159px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="TrQualityRemark" runat="server" visible="true">
                        <td>
                            <span class="labelbold">REMARKS</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" CssClass="textb" TextMode="MultiLine" Width="300px" Height="100px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr id="Trinstruction" runat="server" visible="true">
                        <td>
                            <span class="labelbold">INSTRUCTIONS</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtinstruction" CssClass="textb" TextMode="MultiLine" Width="300px"
                                Height="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close"
                                OnClientClick="return CloseForm()" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="Button1" runat="server"
                                Text="Preview" OnClick="Button1_Click" />
                            &nbsp;<asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
                <table style="margin-left: 80px">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div style="overflow: auto; max-height: 250px; width: 500px">
                                <asp:GridView ID="gdDesign" runat="server" AllowPaging="True" PageSize="6" DataKeyNames="Sr_No"
                                    OnPageIndexChanging="gdDesign_PageIndexChanging" OnRowDataBound="gdDesign_RowDataBound"
                                    OnSelectedIndexChanged="gdDesign_SelectedIndexChanged" CaptionAlign="Left" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
