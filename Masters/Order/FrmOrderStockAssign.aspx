<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmOrderStockAssign.aspx.cs" Inherits="Masters_Order_FrmOrderStockAssign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function Preview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
            if (document.getElementById("<%=DDCompany.ClientID %>").value == "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompany.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDcustomer.ClientID %>").value == "0") {
                alert("Pls Select Customer Name");
                document.getElementById("<%=DDcustomer.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDOrder.ClientID %>").value == "0") {
                alert("Pls Select Order No");
                document.getElementById("<%=DDOrder.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }

        }
        function CheckStock(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowindex = row.rowindex - 1;
            var StockQty = row.cells[6].innerHTML;
            var AssignQty = row.cells[7].getElementsByTagName("input")[0].value;
            if (parseFloat(StockQty) < parseFloat(AssignQty)) {
                alert('Assign Qty Can not be greater than Stock Qty...');
                row.cells[7].getElementBytagName("input")[0].value;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblComp" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                            Width="200px" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblCustname" runat="server" CssClass="labelbold" Text="Customer"></asp:Label>
                        <asp:CheckBox ID="ChkEdit" runat="server" Text="Check For Edit" Visible="true" OnCheckedChanged="ChkEdit_CheckedChanged"
                            AutoPostBack="True" CssClass="checkboxbold" />
                        <br />
                        <asp:DropDownList ID="DDcustomer" runat="server" AutoPostBack="True" CssClass="dropdown"
                            Width="200px" OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblOrder" runat="server" CssClass="labelbold" Text="Order"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="200px" AutoPostBack="True"
                            OnSelectedIndexChanged="DDOrder_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Lot No"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtLotNo" CssClass="textb" runat="server" Width="300px" />
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="BtnPreviewLotWise" Text="Preview" runat="server" CssClass="buttonnorm preview_width"
                            OnClick="BtnPreviewLotWise_Click" />
                    </td>
                </tr>
            </table>
            <table width="75%">
                <tr>
                    <td colspan="3">
                        <div style="overflow: scroll; height: 350px;">
                            <asp:GridView ID="GVOrderStockAssign" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="IFinishedid" CssClass="grid-views" Width="500px" OnSelectedIndexChanged="GVOrderStockAssign_SelectedIndexChanged"
                                OnRowDataBound="GVOrderStockAssign_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="Category_Name" HeaderText="Category Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Item_name" HeaderText="Item">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DSC" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <%-- <asp:BoundField DataField="IssQty" HeaderText="IssueQty" Visible="false">
                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="OrderConsmpQty" HeaderText="OrderConspQty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="75px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIFinishedid" runat="server" Text='<%# Bind("IFinishedid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--   <asp:BoundField DataField="ActualStockQty" HeaderText="ActualStockQty" Visible="false">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Assign Qty" Visible="false">
                                        <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtAssignqty" Width="100px" align="right" runat="server" Text='<%#Bind ("AssignQty") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>--%>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                    <td>
                        <div style="overflow: scroll; width: 453px; height: 350px;">
                            <asp:GridView ID="DGForAssign" runat="server" AutoGenerateColumns="false" CssClass="grid-views"
                                Width="500px" DataKeyNames="Finishedid">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="Category_Name" HeaderText="Category Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Item_name" HeaderText="Item">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DSC" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="LotNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLotNo" runat="server" Text='<%#Bind("LotNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TagNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTagNo" runat="server" Text='<%#Bind("TagNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="IssQty" HeaderText="IssueQty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OrderConsmpQty" HeaderText="OrderConspQty" Visible="false">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ActualStockQty" HeaderText="ActualStockQty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Assign Qty">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtAssignqty" Width="70px" align="right" runat="server" Text='<%#Bind ("AssignQty") %>'
                                                OnTextChanged="TextAssign_Changed" AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="right">
                        <asp:Label ID="LblMessage" runat="server" align="left" ForeColor="Red"></asp:Label>
                        <asp:Button ID="BtnNew" Text="New" runat="server" CssClass="buttonnorm" OnClick="BtnNew_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                            OnClientClick="return Validate();" />
                        &nbsp; &nbsp;
                        <asp:Button ID="BtnPrev" Text="Preview" runat="server" CssClass="buttonnorm preview_width"
                            OnClick="BtnPrev_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="BtnClose" Text="Close" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
