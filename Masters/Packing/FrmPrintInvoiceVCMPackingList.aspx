<%@ Page Title="INVOICE/PACKING PRINT" Language="C#" AutoEventWireup="true" CodeFile="FrmPrintInvoiceVCMPackingList.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_Packing_FrmPrintInvoiceVCMPackingList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function OPEN() {
            window.open('../../ViewReport.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnsearchinvoice.ClientID %>').click();
            }
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table runat="server" width="100%" border="1">
                <tr>
                    <td runat="server" height="inherit" valign="top" class="style1" colspan="2">
                        <div>
                            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div runat="server" style="margin-left: 350px; width: 450px; margin-top: 20px;">--%>
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtinvoiceno" CssClass="textb" Width="240px" placeholder="Type here Invoice No. to Search"
                                            runat="server" onKeypress="KeyDownHandler(event);" />
                                        <asp:Button ID="btnsearchinvoice" Text="Search Invoice" runat="server" Style="display: none"
                                            OnClick="btnsearchinvoice_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LbCompanyName" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="DDCompanyName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblInvoiceYear" runat="server" Text="Invoice Year" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDInvoiceYear" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDInvoiceYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDInvoiceYear"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCustomer" runat="server" Text="Customer" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustomer" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustomer_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblDocumentType" runat="server" Text="Document Type" Width="150px"
                                            CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDocumentType" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDDocumentType_SelectedIndexChanged">
                                            <asp:ListItem Value="1">EXPORT INVOICE</asp:ListItem>
                                            <asp:ListItem Value="20">TAX INVOICE</asp:ListItem>
                                            <asp:ListItem Value="2">PACKING LIST</asp:ListItem>
                                            <asp:ListItem Value="3">Duty Draw Back LIST</asp:ListItem>
                                            <asp:ListItem Value="4">Shipping Instruction</asp:ListItem>
                                            <asp:ListItem Value="5">Letter To Bank</asp:ListItem>
                                            <asp:ListItem Value="6">Bill Of Exchange</asp:ListItem>
                                            <asp:ListItem Value="7">BRC</asp:ListItem>
                                            <asp:ListItem Value="8">Shipping Advice</asp:ListItem>
                                            <asp:ListItem Value="9">Packing List Summary</asp:ListItem>
                                            <asp:ListItem Value="10">Express B/L</asp:ListItem>
                                            <asp:ListItem Value="11">Export Value Declaration</asp:ListItem>
                                            <asp:ListItem Value="12">Single country Declaration</asp:ListItem>
                                            <asp:ListItem Value="13">GR Release</asp:ListItem>
                                            <asp:ListItem Value="14">Annexure-I</asp:ListItem>
                                            <asp:ListItem Value="15">Weight List</asp:ListItem>
                                            <asp:ListItem Value="16">ACD</asp:ListItem>
                                            <asp:ListItem Value="17">Form Sdf </asp:ListItem>
                                            <asp:ListItem Value="18">Packing Detail</asp:ListItem>
                                            <asp:ListItem Value="19">Invoice Detail</asp:ListItem>
                                            <asp:ListItem Value="21">TO WHOMSOEVER IT MAY CONCERN</asp:ListItem>
                                            <asp:ListItem Value="22">Cenvat</asp:ListItem>
                                            <asp:ListItem Value="23">Bill of Exchange-Commerical</asp:ListItem>
                                            <asp:ListItem Value="24">Textle Declartion</asp:ListItem>
                                            <asp:ListItem Value="25">VGM</asp:ListItem>
                                            <asp:ListItem Value="26">ShippingInstruction-BRAND</asp:ListItem>
                                            <asp:ListItem Value="27">ShippingInstruction-URBAN</asp:ListItem>
                                             <asp:ListItem Value="28">SCDeclaration</asp:ListItem>
                                            <asp:ListItem Value="29">GSP</asp:ListItem>
                                            <asp:ListItem Value="30">Special Custom Invoice</asp:ListItem>
                                              
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <%--</div>--%>
                            <div runat="server" style="width: 300px;">
                                <table runat="server">
                                    <tr runat="server">
                                        <td>
                                            <asp:CheckBox ID="ChkboxCustomReport" runat="server" Width="200px" Font-Bold="true"
                                                Text="Chk For Custom Report" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="height: 300px; width: 500px; overflow: auto">
                                                <asp:GridView ID="GDItemDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                    OnRowCommand="GDItemDetail_RowCommand" OnRowDataBound="GDItemDetail_RowDataBound">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Sr_No" HeaderText="Sr_No">
                                                            <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date">
                                                            <HeaderStyle HorizontalAlign="Center" Width="130px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No">
                                                            <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                        </asp:BoundField>
                                                        <%-- <asp:TemplateField HeaderText="CustomReport" Visible="false">
                                                            <ItemTemplate>                                                               
                                                                <asp:CheckBox ID="Chkboxitem" runat="server" Width="10px" />                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Show">
                                                            <ItemTemplate>
                                                                <%--  <asp:Button ID="BTNShowAmt" runat="server" Text="Preview" CssClass="dropdown" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"                                                               
                                                                    OnClick="BTNShowAmt_Click" />
                                                                --%>
                                                                <asp:Button ID="BTNShowAmt" runat="server" Text="Preview" CssClass="dropdown" OnClick="BTNShowAmt_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div runat="server" style="width: 450px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblPrintType" runat="server" Text="Print Type" Width="150px" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDPrintType" runat="server" Width="250px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDPrintType_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Type 1</asp:ListItem>
                                                <asp:ListItem Value="2">Type 2</asp:ListItem>
                                                <asp:ListItem Value="3">Type 3</asp:ListItem>
                                                <asp:ListItem Value="4">Type 4</asp:ListItem>
                                                <asp:ListItem Value="5">Type 5</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">
                                            <%--<asp:Button ID="BtnPreview"    CssClass="dropdown" runat="server" Text="Preview"
                                        OnClick="BtnPreview_Click" />
                                    &nbsp;--%><asp:Button ID="BtnClose" CssClass="buttonnorm" runat="server" Text="Close"
                                        OnClientClick="return CloseForm();" OnClick="BtnClose_Click" />
                                            &nbsp;<asp:Button ID="BtnExport" CssClass="buttonnorm" Width="100px" runat="server"
                                                Text="Excel Export" OnClick="BtnExport_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trgrid" runat="server" visible="false">
                                        <td colspan="2" runat="server">
                                            <asp:GridView runat="server" ID="Gridexport" AutoGenerateColumns="False" GridLines="None"
                                                ShowFooter="true">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <Columns>
                                                    <asp:BoundField DataField="Quality" HeaderText="Item"></asp:BoundField>
                                                    <asp:BoundField DataField="CustomerCode" HeaderText="Buyer Code"></asp:BoundField>
                                                    <asp:BoundField DataField="Pcs" HeaderText="QTY"></asp:BoundField>
                                                    <asp:BoundField DataField="brass" HeaderText="Net Weight Of Brass Per Pcs"></asp:BoundField>
                                                    <asp:BoundField DataField="Iron" HeaderText="Net Weight Of Iron Per Pcs"></asp:BoundField>
                                                    <%--<asp:BoundField DataField="Glass" HeaderText="Net Weight Of Glass Per Pcs">
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Net Weight Of Glass Per Pcs">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotbrassff" runat="server" Visible="true" Text='<%# Bind("Glass") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrassFOT" Visible="true" Text="TOTAL (KGS):-" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle Width="50PX" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Wt Of Brass">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotbrass" runat="server" Visible="true" Text='<%# Bind("TOTbrass") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrass" Visible="true" Text='<%# gettotbrass().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                        <HeaderStyle Width="50PX" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Wt Of Iron">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotiron" runat="server" Visible="true" Text='<%# Bind("TOTiron") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrass" Visible="true" Text='<%# gettotiron().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                        <HeaderStyle Width="50PX" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Wt Of Glass">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotglass" runat="server" Visible="true" Text='<%# Bind("TOTGlass") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrass" Visible="true" Text='<%# gettotglass().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr id="TRDuty" runat="server" visible="false">
                                        <td id="Td1" colspan="2" runat="server">
                                            <asp:GridView runat="server" ID="GDVDuty" AutoGenerateColumns="False" GridLines="None"
                                                ShowFooter="true">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <Columns>
                                                    <asp:BoundField DataField="Quality" HeaderText="Item"></asp:BoundField>
                                                    <asp:BoundField DataField="CustomerCode" HeaderText="Buyer Code"></asp:BoundField>
                                                    <asp:BoundField DataField="Pcs" HeaderText="QTY"></asp:BoundField>
                                                    <asp:BoundField DataField="brass" HeaderText="Net Weight Of Brass Per Pcs"></asp:BoundField>
                                                    <asp:BoundField DataField="Iron" HeaderText="Net Weight Of Iron Per Pcs"></asp:BoundField>
                                                    <%--<asp:BoundField DataField="Glass" HeaderText="Net Weight Of Glass Per Pcs">
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Net Weight Of Glass Per Pcs">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotbrassff" runat="server" Visible="true" Text='<%# Bind("Glass") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrassFOT" Visible="true" Text="TOTAL (KGS):-" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle Width="50PX" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Wt Of Brass">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotbrass" runat="server" Visible="true" Text='<%# Bind("TOTbrass") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrass" Visible="true" Text='<%# gettotbrassDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                        <HeaderStyle Width="50PX" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Wt Of Iron">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotiron" runat="server" Visible="true" Text='<%# Bind("TOTiron") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrass" Visible="true" Text='<%# gettotironDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                        <HeaderStyle Width="50PX" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Wt Of Glass">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotglass" runat="server" Visible="true" Text='<%# Bind("TOTGlass") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totbrass" Visible="true" Text='<%# gettotglassDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty Per Carton">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyPerCarton" runat="server" Visible="true" Text='<%# Bind("QtyPerCarton") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totQtyPerCarton" Visible="true" Text='<%# gettotQtyPerCartonDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Carton">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotCarton" runat="server" Visible="true" Text='<%# Bind("TotalCarton") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totQtyPerCarton" Visible="true" Text='<%# gettotCartonDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="NetWt Per Carton">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotNetWtPerCarton" runat="server" Visible="true" Text='<%# Bind("NetWtPerCarton") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GrossWt Per Carton">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotGrossWtpercarton" runat="server" Visible="true" Text='<%# Bind("GrossWtPerCarton") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Net Weight">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotNetWt" runat="server" Visible="true" Text='<%# Bind("TotalNetWt") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totNetwt" Visible="true" Text='<%# gettotNetWeightDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Gross Weight">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltotGrossWt" runat="server" Visible="true" Text='<%# Bind("TotalGrossWt") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="Server" ID="totGrossWt" Visible="true" Text='<%# gettotGrosswtDetail().ToString() %>' />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GDItemDetail" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
