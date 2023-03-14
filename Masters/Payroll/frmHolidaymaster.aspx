<%@ Page Title="HOLIDAY MASTER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmHolidaymaster.aspx.cs" Inherits="Masters_Payroll_frmHolidaymaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmHolidaymaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company !!\n";
                    }

                    var txtchoosedate = document.getElementById('<%=txtchoosedate.ClientID %>');
                    if (txtchoosedate.value == "") {
                        Message = Message + "Please Enter Choose Date !!\n";
                    }
                    var txtholidaytype = document.getElementById('<%=txtholidaytype.ClientID %>');
                    if (txtholidaytype.value == "") {
                        Message = Message + "Please Enter Holiday Type !!\n";
                    }

                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin: 1% 30% 0% 30%">
                <table border="1">
                    <tr>
                        <td>
                            <asp:Label ID="lblfilterby" Text="Data Filter By" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblyear" CssClass="labelbold" Text="Year" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDyear" CssClass="dropdown" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDyear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="Trcompanyid">
                        <td>
                            <asp:Label ID="lblcompany" Text="Company Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="250px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblBranchMaster" Text="Branch Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="250px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblchoosedate" Text="Choose Date" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtchoosedate" runat="server" Width="120px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="cal1" TargetControlID="txtchoosedate" runat="server" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblHtype" Text="Holiday Type" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtholidaytype" runat="server" CssClass="textb" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <div style="max-height: 300px; overflow: auto">
                                            <asp:GridView ID="GDDetail" runat="server" CssClass="grid-views" AutoGenerateColumns="False"
                                                EmptyDataText="No records found...." OnRowDeleting="GDDetail_RowDeleting">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldate" Text='<%#Bind("Date") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Holiday Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblholidaytype" Text='<%#Bind("Holidaytype") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                            <asp:Label ID="lblcompanyid" Text='<%#Bind("companyid") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkupdate" runat="server" Text="Update" CausesValidation="false"
                                                                OnClick="lnklnkupdate"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkdel" runat="server" Text="Delete" CausesValidation="false"
                                                                CommandName="Delete" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
