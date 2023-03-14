<%@ Page Title="Staff Emp Salary Data" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmStaffEmpSalaryData.aspx.cs" Inherits="Masters_Payroll_FrmStaffEmpSalaryData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmStaffEmpSalaryData.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }      
        }
    </script>
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <%-- <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>--%>
            <table border="1" style="width: 100%">
                <tr>
                    <td id="TDDelete" runat="server" visible="true">
                        <table border="1" style="width: 60%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkDelete" Text="For Delete" CssClass="labelbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkDelete_CheckedChanged" />
                                </td>
                                <%--   <td>
                                        <asp:Label ID="lblfromdate" Text="Data From" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" Width="100px" AutoPostBack="true" 
                                            OnTextChanged="txtfromdate_TextChanged" />
                                        <asp:CalendarExtender ID="caltxtfromdate" runat="server" TargetControlID="txtfromdate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>--%>
                                <td id="TDCompanyName" runat="server" visible="false">
                                    <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    <asp:DropDownList ID="DDCompanyName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                        Width="300px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDCompanyUnit" runat="server" visible="false">
                                    <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="UnitName"></asp:Label>
                                    <asp:DropDownList ID="DDCompanyUnit" runat="server" AutoPostBack="True" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDCompanyUnit_SelectedIndexChanged" Width="300px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDreceiveno" runat="server" visible="false">
                                    <asp:Label ID="lblReceiveNo" Text="ReceiveNo" runat="server" CssClass="labelbold" />
                                    <asp:DropDownList ID="DDreceiveNo" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDDeleteBtn" runat="server" visible="false">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="buttonnorm" Text="Delete" OnClick="btnDelete_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <div style="float: left; width: 450px; margin: 0% 10% 3% 20%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRProcessName" runat="server">
                                        <td>
                                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Unit Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDCompanyUnitM" runat="server" CssClass="dropdown" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDates" runat="server" visible="true">
                                        <td>
                                            <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Month"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="DDMonth" runat="server" CssClass="dropdown" Width="100px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="Year" Width="80px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDYear" runat="server" CssClass="dropdown" Width="100px">
                                                        </asp:DropDownList>
                                                        <%--<asp:TextBox ID="TxtToDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtToDate">
                                            </asp:CalendarExtender>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--  <asp:TextBox ID="TxtFromDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtFromDate">
                                            </asp:CalendarExtender>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            &nbsp;<asp:Button ID="BtnShow" runat="server" CssClass="buttonnorm" OnClick="BtnShow_Click"
                                                OnClientClick="return Validate();" Text="Show" />
                                            &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <div style="max-height: 500px; overflow: auto; width: 100%;">
                            <asp:GridView ID="GVStaffEmpSalary" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                DataKeyNames="EmpId" OnRowDataBound="GVStaffEmpSalary_RowDataBound">
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
                                    <asp:TemplateField HeaderText="Emp Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Bind("EmpName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Father Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFatherName" runat="server" Text='<%#Bind("FatherName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="400px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Present">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalPresent" runat="server" Text='<%#Bind("TotalPresent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Absent">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalAbsent" runat="server" Text='<%#Bind("TotalAbsent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total WeekOff">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalWeekOff" runat="server" Text='<%#Bind("TotalWeekOff") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Holidays">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalHolidays" runat="server" Text='<%#Bind("TotalHolidays") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Basic Salary">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBasicSalary" runat="server" Text='<%#Bind("BasicSalary") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Allowence">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllowence" runat="server" Text='<%#Bind("allow") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total GrossSalary">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalGrossSalary" runat="server" Text='<%#Bind("TotalGrossSalary") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeduction" runat="server" Text='<%#Bind("Ded") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField HeaderText="Total Ded">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalDed" runat="server" Text='<%#Bind("TotalDed") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Net Salary">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetSalary" runat="server" Text='<%#Bind("NetSalary") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPerDaySalary" runat="server" Text='<%# Bind("PerDaySalary") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("EmpId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblCompanyid" runat="server" Text='<%# Bind("Companyid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UnitId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblSalMonth" runat="server" Text='<%# Bind("SalMonth") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblSalYear" runat="server" Text='<%# Bind("SalYear") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </td>
                    <tr>
                        <td align="center" colspan="3">
                            &nbsp;<asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                Visible="false" OnClientClick="return Validate();" Text="Save" />
                        </td>
                    </tr>
                </tr>
            </table>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShow" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
