<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmPNMToChampoCarpetOutWardChallan.aspx.cs" Inherits="Masters_Packing_FrmPNMToChampoCarpetOutWardChallan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=BtnStockNo.ClientID %>').click();
            }
        }

        function NewForm() {
            window.location.href = "frmproductionReceiveLoomStockwise.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text="Edit" AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged"
                                CssClass="checkboxbold" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDDIssueNo" runat="server" visible="false">
                            <asp:Label ID="Label1" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDIssueNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDIssueNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="TxtIssueNo" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="TxtIssueDate" CssClass="textb" Width="95px" runat="server" />
                            <asp:CalendarExtender ID="cal1" TargetControlID="TxtIssueDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblstockno" Text="Enter Stock No./Scan" CssClass="labelbold" runat="server"></asp:Label><br />
                            <asp:TextBox ID="TxtStockNo" CssClass="textb" Width="250px" runat="server" onKeypress="KeyDownHandler(event);" />
                            <asp:Button ID="BtnStockNo" runat="server" Style="display: none" OnClick="TxtStockNo_TextChanged" />
                        </td>
                        <td>
                            <asp:Label ID="LblRemark" Text="Remark" CssClass="labelbold" runat="server"></asp:Label><br />
                            <asp:TextBox ID="TxtRemark" CssClass="textb" Width="350px" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label4" Text="No of PC" CssClass="labelbold" runat="server"></asp:Label><br />
                            <asp:TextBox ID="TxtNoofPc" CssClass="textb" Width="100px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div id="Div1" runat="server" style="max-height: 300px; width: 100%; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="StockNo"
                                    OnRowDataBound="DGOrderdetail_RowDataBound" OnRowDeleting="DGOrderdetail_RowDeleting"
                                    CssClass="grid-view" Width="922px">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px" />
                                            <ItemStyle Width="70px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemDescription" HeaderText="Item Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TStockNo" HeaderText="StockNo">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="BtnShowDataInGrid" runat="server" Text="Show Data In Grid" CssClass="buttonnorm"
                                OnClick="BtnShowDataInGrid_Click" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
