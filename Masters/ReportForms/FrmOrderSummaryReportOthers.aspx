<%@ Page Title="ORDER SUMMARY REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmOrderSummaryReportOthers.aspx.cs" Inherits="Masters_ReportForms_FrmOrderSummaryReportOthers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function CheckListbox() {
            var message = "";
            var lstSelectProcess = document.getElementById('<%=lstSelectProcess.ClientID%>');
            if (lstSelectProcess.length == 0) {
                message = "Please select atleast one Job !!!"
            }
            if (message == "") {
                return true;
            }
            else {
                alert(message);
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <table width="100%">
                    <tr>
                        <td style="width: 40%" valign="top">
                            <table width="100%">
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
                                        <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="250px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDOrder_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfromdate" CssClass="labelbold" Text="Ship From" runat="server" />
                                                    <br />
                                                    <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtfromdate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label2" CssClass="labelbold" Text="Ship To" runat="server" />
                                                    <br />
                                                    <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    <asp:Button ID="BtnBuyerDetail" Text="Buyer Detail" CssClass="buttonnorm" runat="server"
                                                        OnClick="BtnBuyerDetail_Click" />
                                                    &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="GET DATA"
                                                        OnClick="BtnPreview_Click" />
                                                    &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="CLOSE"
                                                        OnClientClick="return CloseForm();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 60%" valign="top">
                            <table width="100%">
                                <tr>
                                    <td style="width: 45%" valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <div style="width: 100%; overflow: scroll">
                                                        <span class="labelbold">Jobs</span>
                                                        <asp:ListBox ID="lstProcess" runat="server" Width="95%" Height="200px" SelectionMode="Single">
                                                        </asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 10%" valign="middle">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%" align="center">
                                                    <asp:Button ID="btngo" runat="server" Text=">>" CssClass="buttonnorm" OnClick="btngo_Click" /><br />
                                                    <br />
                                                    <asp:Button ID="btnDelete" runat="server" Text="<<" CssClass="buttonnorm" OnClick="btnDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 45%" valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <div style="width: 100%; overflow: scroll">
                                                        <span class="labelbold">Items Job Sequence</span>
                                                        <asp:ListBox ID="lstSelectProcess" runat="server" Width="95%" Height="200px" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:Button ID="btngetdatajobwise" Text="GET DATA JOB WISE" CssClass="buttonnorm"
                                            OnClientClick="return CheckListbox()" runat="server" OnClick="btngetdatajobwise_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
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
                                        <asp:TemplateField HeaderText="PACKED_TO_OTHER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpackedtoother" Text='<%#Bind("Packed_to_other") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PACKED_FROM_OTHER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpackedfromother" Text='<%#Bind("Packed_From_other") %>' runat="server" />
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
            <asp:PostBackTrigger ControlID="btngetdatajobwise" />
            <asp:PostBackTrigger ControlID="btnexport" />
            <asp:PostBackTrigger ControlID="BtnBuyerDetail" />
            <asp:AsyncPostBackTrigger ControlID="GVDetails" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
