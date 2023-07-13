<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderToReadyForInspection.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Title="Order Inspection" Inherits="Masters_Order_FrmOrderToReadyForInspection"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lfffd" Text=" Company Name" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label1" Text="Customer Code" runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label2" Text="Order No." runat="server" CssClass="labelbold" />
                        &nbsp; <b style="color: Red">*</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="DDLInCompanyName" runat="server" Width="300px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLInCompanyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLCustomerCode" runat="server" Width="250px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLOrderNo" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="95%">
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" UseSubmitBehavior="false"
                            OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';"
                            CssClass="buttonnorm" Width="70px" />
                        <asp:Button ID="Btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" Width="70px" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="3" align="left">
                        <div style="width: 100%; max-height: 400px; overflow: auto">
                            <asp:GridView ID="DGOrderDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="FinishedID"
                                CssClass="grid-views" OnRowDataBound="DGOrderDetail_RowDataBound1">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemname" Text='<%#Bind("Item_Name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesc" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="O Qty.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloqty" Text='<%#Bind("OQty") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inspection Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Txt_Inspection_Qty" runat="server" align="right" Width="75px" Text='<%# Bind("InspectionQty") %>'
                                                onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
