<%@ Page Title="Salary Process" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsalaryprocessmaster.aspx.cs" Inherits="Masters_Payroll_frmsalaryprocessmaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var selectedindex = $("#<%=DDmonth.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Month!!\n";
            }
            selectedindex = $("#<%=DDyear.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please Select year !!\n";
            }
            if (Message == "") {
                return confirm('Do you want to Process Salary ?');
            }
            else {
                alert(Message);
                return false;
            }
        }
        function NewForm() {
            window.location.href = "frmsalaryprocessmaster.aspx";
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
                            <asp:Label Text="Company" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label10" CssClass="labelbold" Text="Branch" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Department" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDDepartment" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" Text="Month" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDmonth" CssClass="dropdown" Width="50%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" Text="Salary Date" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtSalaryDate" CssClass="textb" Width="100px" runat="server" />
                            <asp:CalendarExtender ID="caleffectfrom" runat="server" TargetControlID="TxtSalaryDate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label3" Text="Year" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDyear" Width="50%" CssClass="dropdown" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label6" Text="Wages Calculation" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDwagescalculation" CssClass="dropdown" runat="server">
                                <asp:ListItem Text="Monthly" Value="1" />
                                <asp:ListItem Text="Daily" Value="2" />
                                <asp:ListItem Text="Pcs Wise" Value="3" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chkResignStatus" Text="For Resign Status" CssClass="checkboxbold"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label4" Text="Emp. Code" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:TextBox ID="txtempcode" CssClass="textboxm" MaxLength="100" Width="95%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <asp:Label ID="Label5" Text="For multiple emp. Code use commas(,)eg:0001,0002" CssClass="labelbold"
                                            ForeColor="Red" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; border-style: dotted" colspan="2" align="right">
                            <asp:Button ID="btnprocess" Text="Process" runat="server" CssClass="buttonnorm" OnClick="btnprocess_Click"
                                UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'Salary Processing wait ...';" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
