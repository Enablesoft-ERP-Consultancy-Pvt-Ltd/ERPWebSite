<%@ Page Title="ORDER SUMMARY REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmOrderSummaryReport.aspx.cs" Inherits="Masters_ReportForms_FrmOrderSummaryReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table>
                    <tr id="TRDDCompany" runat="server">
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDCustName" runat="server">
                        <td>
                            <asp:Label ID="lblCustname" runat="server" CssClass="labelbold" Text="Customer"></asp:Label>
                        </td>
                        <td colspan="3" class="style2">
                            <asp:DropDownList ID="DDcustomer" runat="server" CssClass="dropdown" Width="250px"
                                OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRDDOrder" runat="server">
                        <td>
                            <asp:Label ID="lblOrder" runat="server" CssClass="labelbold" Text="Order No"></asp:Label>
                        </td>
                        <td colspan="3" class="style2">
                            <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="GET DATA"
                                OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="CLOSE"
                                OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr>
                        <td>
                            <div style="max-height: 500px; overflow: auto">
                                <asp:GridView ID="GVDetails" AutoGenerateColumns="False" EmptyDataText="No records Found..."
                                    runat="server" CssClass="grid-view" OnRowDataBound="GVDetails_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="BUYER ORDER NO.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderNo" Text='<%#Bind("customerorderNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LOCAL ORDER NO.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllocalorderNo" Text='<%#Bind("Localorder") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ORDER DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderdate" Text='<%#Bind("OrderDate") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DISPATCH DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldispatchdate" Text='<%#Bind("Dispatchdate") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QUALITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblquality" Text='<%#Bind("QualityName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DESIGN NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesign" Text='<%#Bind("Designname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="COLOR">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcolor" Text='<%#Bind("Colorname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SIZE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsize" Text='<%#Bind("Size") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TOTAL PCS">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalpcs" Text='<%#Bind("Totalpcs") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TOTAL AREA">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalarea" Text='<%#Bind("TOTALAREA") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TO BE ISSUED">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltobeissued" Text='<%#Bind("tobeissued") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ON LOOM">
                                            <ItemTemplate>
                                                <asp:Label ID="lblonloom" Text='<%#Bind("Onloom") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OFF LOOM">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloffloom" Text='<%#Bind("Offloom") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UNDER FINISHING">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblUF" Text='<%#Bind("Underfinishing") %>' runat="server" />--%>
                                                <asp:LinkButton ID="lblUF" runat="server" Text='<%#Bind("Underfinishing") %>' CssClass="labelbold"
                                                    ForeColor="DarkOrange" ToolTip="Show Under Finishing Data" OnClick="lnkunderfinishing_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FINISHED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfinished" Text='<%#Bind("FINISHED") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PACKED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpacked" Text='<%#Bind("Packed") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="DISPATCHED" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDispatched" Text='<%#Bind("DISPATCHED") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="BALANCE" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceQty" Text='<%#Convert.ToInt32(Eval("Totalpcs")) -Convert.ToInt32(Eval("DISPATCHED")) %>' runat="server" />

                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                <asp:Label ID="lblunit" Text='<%#Bind("unit") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" id="TDExport" runat="server" visible="false">
                            <asp:Button ID="btnexport" Text="Export To Excel" CssClass="buttonnorm" runat="server"
                                OnClick="btnexport_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexport" />
            <asp:AsyncPostBackTrigger ControlID="GVDetails" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
