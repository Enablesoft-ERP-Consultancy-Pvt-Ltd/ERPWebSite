<%@ Page Title="Leave Requests" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmleaverequest.aspx.cs" Inherits="Masters_Payroll_frmleaverequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;
                    }
                }
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label Text="Find By" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 60%; border-style: dotted">
                            <asp:DropDownList ID="ddfindby" CssClass="dropdown" runat="server" Width="100%">
                                <asp:ListItem Text="All" />
                                <asp:ListItem Text="Pending" />
                                <asp:ListItem Text="Approved" />
                                <asp:ListItem Text="Rejected" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label3" Text="Find By Emp. Code" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:TextBox ID="txtempcode" CssClass="textboxm" Width="50%" placeholder="Type Emp. Code here..."
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-style: dotted; width: 100%" align="center">
                            <asp:CheckBox ID="chkfilterbydate" CssClass="checkboxbold" Text="Filter By Date"
                                runat="server" AutoPostBack="true" OnCheckedChanged="chkfilterbydate_CheckedChanged" />
                        </td>
                    </tr>
                    <tr runat="server" id="Trfromto" visible="false">
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label1" Text="From" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <table width="100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtfrom" CssClass="textboxm" Width="95%" runat="server" placeholder="click to set" />
                                        <asp:CalendarExtender ID="calfrom" TargetControlID="txtfrom" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label2" Text="To" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 50%">
                                        <asp:TextBox ID="txtto" CssClass="textboxm" Width="95%" runat="server" placeholder="click to set" />
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtto" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; border-style: dotted" align="right">
                            <asp:Button ID="btnfinddata" Text="Find Data" runat="server" CssClass="buttonnorm"
                                OnClick="btnfinddata_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; border-style: dotted">
                            <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div style="width: 100%; max-height: 400px; overflow: auto">
                            <asp:GridView ID="Dgdetail" CssClass="grid-views" AutoGenerateColumns="false" runat="server"
                                Width="100%" EmptyDataText="No data fetched for this Search Criteria.." 
                                onrowdatabound="Dgdetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkitem" Text="" runat="server" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkall" Text="" runat="server" onclick="return CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Emp. Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempcode" Text='<%#Bind("EMpcode") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Emp. Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempname" Text='<%#Bind("empname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblleavetype" Text='<%#Bind("Leavetype") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Duration">
                                        <ItemTemplate>
                                            <asp:Label ID="lblleaveduration" Text='<%#Bind("Leaveduration") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfromdate" Text='<%#Bind("fromdate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltodate" Text='<%#Bind("todate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lblleavecount" Text='<%#Bind("leavecount") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreason" Text='<%#Bind("reason") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtcomments" CssClass="textboxm" Text='<%#Bind("Comments") %>' runat="server"
                                                Width="95%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Applied on">
                                        <ItemTemplate>
                                            <asp:Label ID="lblappliedon" Text='<%#Bind("Appliedon") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" Text='<%#Bind("status") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblapplicationid" Text='<%#Bind("Applicationid") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table width="100%" runat="server" id="TBsave" visible="false">
                            <tr>
                                <td style="width: 70%">
                                </td>
                                <td style="width: 30%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:Label ID="Label4" Text="Action" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 50%">
                                                <asp:DropDownList ID="DDaction" Width="100%" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Approved" />
                                                    <asp:ListItem Text="Rejected" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" 
                                                    onclick="btnsave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
