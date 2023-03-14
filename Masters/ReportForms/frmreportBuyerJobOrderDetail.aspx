<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmreportBuyerJobOrderDetail.aspx.cs"
    Inherits="Masters_ReportForms_frmreportBuyerJobOrderDetail" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../FrmCmpRawMaterialStock.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('CPH_Form_RDjob_PurchaseDetail').checked) {
                if (document.getElementById('CPH_Form_DDcustomer').selectedIndex <= "0") {
                    alert('Plz Select Customer Code...')
                    document.getElementById('CPH_Form_DDcustomer').focus()
                    return false;
                }
                if (document.getElementById('CPH_Form_DDOrder').selectedIndex <= "0") {
                    alert('Plz Select Order No...')
                    document.getElementById('CPH_Form_DDOrder').focus()
                    return false;
                }
            }
            //            else if (document.getElementById('CPH_Form_RDOrderDetail').checked) {
            //                if (document.getElementById('CPH_Form_DDcustomer').selectedIndex <= "0") {
            //                    alert('Plz Select Customer Code...')
            //                    document.getElementById('CPH_Form_DDcustomer').focus()
            //                    return false;
            //                }
            //            }

        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table style="width: 89%">
                <tr style="width: 100%">
                    <td style="width: 80px">
                    </td>
                    <td>
                        <div style="width: 800px; height: auto;">
                            <div style="width: 600px; height: auto; margin-left: 50px; margin-top: 20px">
                                <div style="width: 600px; height: auto">
                                    <fieldset style="border: 1px groove Teal; border: 5px solid #c8e5f6;">
                                        <legend></legend>
                                        <div style="float: left; height: auto; width: 200px; border: 1px groove; margin-top: 10px;
                                            margin-left: 5px; padding: 0px 0px 2px 2px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RDjob_PurchaseDetail" runat="server" Text="Job/Purchase Status"
                                                            CssClass="labelnormalMM" GroupName="m" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RDOrderDetail" runat="server" Text="Order Detail" CssClass="labelnormalMM"
                                                            GroupName="m" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="float: right; height: auto">
                                            <div style="padding: 0px 0px 0px 20px">
                                                <table style="height: 150px">
                                                    <tr id="TRDDCompany" runat="server">
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" CssClass="labelnormalMM" Text="CompanyName"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDCustName" runat="server">
                                                        <td>
                                                            <asp:Label ID="lblCustname" runat="server" CssClass="labelnormalMM" Text="Customer"></asp:Label>
                                                        </td>
                                                        <td colspan="3" class="style2">
                                                            <asp:DropDownList ID="DDcustomer" runat="server" CssClass="dropdown" Width="250px"
                                                                OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDOrder" runat="server">
                                                        <td>
                                                            <asp:Label ID="lblOrder" runat="server" CssClass="labelnormalMM" Text="Order No"></asp:Label>
                                                        </td>
                                                        <td colspan="3" class="style2">
                                                            <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRProcess" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblProcess" runat="server" CssClass="labelnormalMM" Text="Process Name"></asp:Label>
                                                        </td>
                                                        <td colspan="3" class="style2">
                                                            <asp:DropDownList ID="DDprocessName" runat="server" CssClass="dropdown" Width="250px"
                                                                OnSelectedIndexChanged="DDprocessName_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="TREmployee" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblemployee" runat="server" CssClass="labelnormalMM" Text="Employee/Party"></asp:Label>
                                                        </td>
                                                        <td colspan="3" class="style2">
                                                            <asp:DropDownList ID="DDEmployee" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRCategory" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelnormalMM" Text="Category Name"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRddItemName" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblitemname" runat="server" CssClass="labelnormalMM" Text="Item Name"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="ddItemName" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDQuality" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelnormalMM" Text="Quality"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDDesign" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lbldesignname" runat="server" CssClass="labelnormalMM" Text="Design"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDColor" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblcolorname" runat="server" CssClass="labelnormalMM" Text="Color"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDShadeColor" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblshadename" runat="server" CssClass="labelnormalMM" Text="Shade Color"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDShape" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblshapename" runat="server" CssClass="labelnormalMM" Text="Shape"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDShape" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                            <asp:CheckBox ID="chkmtr" runat="server" Text="Check For Mtr" Font-Bold="true" AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDSize" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblsizename" runat="server" CssClass="labelnormalMM" Text="Size"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDLotNo" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="LblLotNo" runat="server" CssClass="labelnormalMM" Text="Lot No"></asp:Label>
                                                        </td>
                                                        <td colspan="3" class="style2">
                                                            <asp:DropDownList ID="DDLotNo" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="TRDDGodow" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblGudownname" runat="server" CssClass="labelnormalMM" Width="90px"
                                                                Text="Godown Name"></asp:Label>
                                                        </td>
                                                        <td colspan="3" class="style2">
                                                            <asp:DropDownList ID="DDGudown" runat="server" CssClass="dropdown" Width="250px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="right">
                                                            &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                                                OnClick="BtnPreview_Click" OnClientClick="return validate();" />
                                                            &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                                                OnClientClick="return CloseForm();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
