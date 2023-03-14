<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmItemWiseRateDefine.aspx.cs" Inherits="Masters_Order_FrmItemWiseRateDefine" %>

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
        function ValidateShowData() {
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
            else if (document.getElementById("<%=DDCategory.ClientID %>").value == "0") {
                alert("Pls Select Category");
                document.getElementById("<%=DDCategory.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDItemName.ClientID %>").value == "0") {
                alert("Pls Select Item Name");
                document.getElementById("<%=DDItemName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDQualityName.ClientID %>").value == "0") {
                alert("Pls Select Quality Name");
                document.getElementById("<%=DDQualityName.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Show Data?')
            }
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
                        <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Category Type"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDCategoryType" runat="server" AutoPostBack="True" CssClass="dropdown"
                            Width="125px" OnSelectedIndexChanged="DDCategoryType_SelectedIndexChanged" Enabled="false">
                            <asp:ListItem Value="0">Finished Item</asp:ListItem>
                            <asp:ListItem Value="1">Raw Material</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td id="TDCustomer" runat="server">
                        <asp:Label ID="lblCustname" runat="server" CssClass="labelbold" Text="Customer"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDcustomer" runat="server" AutoPostBack="True" CssClass="dropdown"
                            Width="200px" OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCategory" runat="server" CssClass="labelbold" Text="Category"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Item"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDItemName" runat="server" CssClass="dropdown" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td id="TDQuality" runat="server" visible="False">
                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Quality Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDQualityName" runat="server" CssClass="dropdown" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="DDQualityName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td id="TdDESIGN" runat="server" visible="False">
                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Design Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDDesignName" runat="server" CssClass="dropdown" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="DDDesignName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td id="TDColor" runat="server" visible="False">
                        <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Color Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDColorName" runat="server" CssClass="dropdown" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td id="TDShadeColor" runat="server" visible="False">
                        <asp:Label ID="Label8" runat="server" CssClass="labelbold" Text="Shade Color Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Cal Type"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDCalType" runat="server" CssClass="dropdown" Width="100px">
                            <asp:ListItem Value="0">Area Wise</asp:ListItem>
                            <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="Unit Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDUnitName" runat="server" CssClass="dropdown" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="BtnShowData" Text="Show Data" runat="server" OnClick="BtnShowData_Click"
                            OnClientClick="return ValidateShowData();" />
                    </td>
                </tr>
            </table>
            <table width="75%">
                <tr>
                    <td>
                        <div style="overflow: scroll; width: 1000px; height: 350px;">
                            <asp:GridView ID="DGItemDetail" runat="server" AutoGenerateColumns="false" CssClass="grid-views"
                                Width="980px">
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
                                        <ItemStyle HorizontalAlign="Left" Width="130px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QualityName" HeaderText="Quality">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DesignName" HeaderText="Design">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ColorName" HeaderText="ColorName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtRate" Width="75px" align="right" runat="server" Text='<%#Bind ("Rate") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SrNo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategoryID" Text='<%#Bind("CategoryID") %>' runat="server" />
                                            <asp:Label ID="lblItemID" Text='<%#Bind("ItemID") %>' runat="server" />
                                            <asp:Label ID="lblQualityID" Text='<%#Bind("QualityID") %>' runat="server" />
                                            <asp:Label ID="lblDesignID" Text='<%#Bind("DesignID") %>' runat="server" />
                                            <asp:Label ID="lblColorID" Text='<%#Bind("ColorID") %>' runat="server" />
                                        </ItemTemplate>
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
                        &nbsp;
                        <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                            OnClientClick="return Validate();" />
                        &nbsp;
                        <asp:Button ID="BtnPrev" Text="Preview" runat="server" CssClass="buttonnorm preview_width"
                            OnClick="BtnPrev_Click" Visible="false" />
                        &nbsp;
                        <asp:Button ID="BtnClose" Text="Close" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
