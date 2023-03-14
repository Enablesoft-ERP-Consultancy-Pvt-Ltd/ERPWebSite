<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartmentJobAssign.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Campany_DepartmentJobAssign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="tdstyle">
                        Company Name&nbsp; *
                    </td>
                    <td class="tdstyle">
                        Customer Code&nbsp; *
                    </td>
                    <td class="tdstyle">
                        Order No.&nbsp; *
                    </td>
                    <td align="center" class="tdstyle">
                        Order Date
                    </td>
                    <td align="center" class="tdstyle">
                        Delivery Date
                    </td>
                    <td align="center" class="tdstyle">
                        Req Date
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="DDLInCompanyName" runat="server" Width="100%" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLInCompanyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLCustomerCode" runat="server" Width="100%" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLOrderNo" runat="server" Width="100%" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDLOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="TxtOrderDate" runat="server" Format="dd-MMM-yyyy" Width="100px"
                            CssClass="textb"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="TxtDeliveryDate" runat="server" Format="dd-MMM-yyyy" Width="100px"
                            CssClass="textb"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="TxtReqDate" runat="server" Width="100px" Format="dd-MMM-yyyy" align="left"
                            CssClass="textb"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtReqDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" align="left">
                        <div style="width: 100%; height: 450px; overflow: scroll">
                            <asp:GridView ID="DGOrderDetail" runat="server" SelectedRowStyle-ForeColor="#999999"
                                Font-Bold="True" Font-Size="Medium" Width="100%" AutoGenerateColumns="False"
                                DataKeyNames="OrderId" OnRowCancelingEdit="DGOrderDetail_RowCancelingEdit" OnRowUpdating="DGOrderDetail_RowUpdating"
                                OnRowEditing="DGOrderDetail_RowEditing" CssClass="grid-view" OnRowCreated="DGOrderDetail_RowCreated">
                                <AlternatingRowStyle />
                                <Columns>
                                    <asp:BoundField DataField="ITEM_NAME" HeaderText="Item" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QtyRequired" HeaderText="Order Qty." ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Avialable_stock" HeaderText="Stock Qty." ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Tag Qty.">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTagStock" Width="40" align="right" runat="server" Text='<%# Bind("TagStock") %>'>
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTagStock"
                                                ErrorMessage="qty ">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("TagStock") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pre Prod Qty.">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPreProdAssignedQty" Width="40" align="right" ReadOnly="true"
                                                runat="server" Text='<%# Bind("PreProdAssignedQty") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("PreProdAssignedQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prod Qty Req.">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProd_Qty_Req" runat="server" align="right" Width="40" Text='<%# Bind("Prod_Qty_Req") %>'>
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProd_Qty_Req"
                                                ErrorMessage="qty required ">
                                                
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Prod_Qty_Req") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="True" />
                                </Columns>
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr id="ErrorMessage" runat="server" visible="false">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <div id="total" runat="server" visible="false">
                            Total
                            <asp:TextBox ID="TxtTOtalO_Quantity" runat="server" Width="70"></asp:TextBox>
                            <asp:TextBox ID="TxtTotal_Com_Stock" runat="server" Width="70"></asp:TextBox>
                            <asp:TextBox ID="TxtTotal_Tag_Quantity" runat="server" Width="70"></asp:TextBox>
                            <asp:TextBox ID="TxtTotal_Prod_Quantity" runat="server" Width="70"></asp:TextBox>
                            <asp:TextBox ID="TxtTotal_Remaining_Quantity" runat="server" Width="70"></asp:TextBox>
                        </div>
                    </td>
                    <td colspan="3" align="right">
                        <asp:Button ID="Button1" CssClass="buttonnorm" runat="server" Text="Preview" Visible="false" />
                        <asp:Button ID="BtnForStockItem" CssClass="buttonnorm" runat="server" Text="For Stock Item" />
                        <asp:Button ID="BtnForProductionItem" runat="server" CssClass="buttonnorm" Enabled="false"
                            OnClientClick="priview();" Text="ForProductionItem" />
                        <asp:Button ID="Btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
