<%@ Page Title="Employee Monthly Working Days" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmEmployeeMonthlyWorkingDays.aspx.cs" Inherits="Masters_Payroll_FrmEmployeeMonthlyWorkingDays" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

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
        function AddColon(txt) {
            if (txt.value.length == 2) {
                txt.value += ":";
            }
        }
        function validatetime(id) {
            var time = id.value;
            //alert(time);
            var re = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;

            if (re.test(time)) {
                //alert(re.test(time));               

            } else {
                id.value = "";
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label9" CssClass="labelbold" Text="Company" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label10" CssClass="labelbold" Text="Branch" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label6" CssClass="labelbold" Text="Department" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDdepartment" CssClass="dropdown" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label7" CssClass="labelbold" Text="Designation" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDDesignation" CssClass="dropdown" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblempcode" CssClass="labelbold" Text="Emp. Code" runat="server" />
                        <br />
                        <asp:TextBox ID="txtempcode" CssClass="textboxm" Width="200px" MaxLength="100" runat="server"
                            placeholder="Type Emp. Code here (Optional)" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label8" CssClass="labelbold" Text="Shift" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDShift" CssClass="dropdown" Width="150px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" CssClass="labelbold" Text="MonthName" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDMonthName" CssClass="dropdown" Width="150px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" CssClass="labelbold" Text="Year" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDYear" CssClass="dropdown" Width="150px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2" style="width: 100%" align="right">
                        <asp:Button ID="btngetdata" Text="Get Data" CssClass="buttonnorm" runat="server"
                            OnClick="btngetdata_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="width: 100%">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 100%" valign="top">
                        <div style="width: 100%; max-height: 250px; overflow: auto">
                            <asp:GridView ID="Dgdetail" runat="server" CssClass="grid-views" AutoGenerateColumns="false"
                                EmptyDataText="No data fetched...." Width="100%">
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
                                        <HeaderStyle Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Emp. Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempcode" Text='<%#Bind("EMpcode") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Emp. Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempname" Text='<%#Bind("EmpName") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="250px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldept" Text='<%#Bind("Departmentname") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesignation" Text='<%#Bind("Designation") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Days" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtDays" Width="70px" runat="server" Text='<%#Bind("Days") %>'
                                                onkeypress="return isNumberKey(event);" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hour" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtHour" Width="70px" runat="server" Text='<%#Bind("Hour") %>' onkeypress="return isNumberKey(event);" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Minutes" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtMinutes" Width="70px" runat="server" Text='<%#Bind("Minutes") %>'
                                                onkeypress="return isNumberKey(event);" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempid" Text='<%#Bind("empid") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
