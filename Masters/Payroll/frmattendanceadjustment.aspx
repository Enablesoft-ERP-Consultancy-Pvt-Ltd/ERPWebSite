<%@ Page Title="Attendance Adjustment" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmattendanceadjustment.aspx.cs" Inherits="Masters_Payroll_frmattendanceadjustment" %>

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
    <script type="text/javascript">
        function validategetdata() {
            var Message = "";
            var txtfrom = document.getElementById('<%=txtfromdate.ClientID %>');
            var txtto = document.getElementById('<%=txttodate.ClientID %>');

            if (txtfrom.value == "") {
                Message = Message + "Please Enter From Date!!\n";
            }
            if (txtto.value == "") {
                Message = Message + "Please Enter To Date!!\n";
            }
            var txttimebefore_after = document.getElementById('<%=txttimebefore_after.ClientID %>');
            if (txttimebefore_after.value == "") {
                Message = Message + "Please Enter Time Before/After!!\n";
            }
            var datefrom = new Date(txtfrom.value);
            var dateto = new Date(txtto.value);
            if (datefrom > dateto) {
                Message = Message + "From Date can not greater than To Date !!\n";
            }
            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
            }
        }
        function validatesave() {

            var Message = "";
            var txtfrom = document.getElementById('<%=txtfromdate.ClientID %>');
            var txtto = document.getElementById('<%=txttodate.ClientID %>');

            if (txtfrom.value == "") {
                Message = Message + "Please Enter From Date!!\n";
            }
            if (txtto.value == "") {
                Message = Message + "Please Enter To Date!!\n";
            }
            var txttimebefore_after = document.getElementById('<%=txttimebefore_after.ClientID %>');
            if (txttimebefore_after.value == "") {
                Message = Message + "Please Enter Time Before/After!!\n";
            }
            var txttimeset = document.getElementById('<%=txttimeset.ClientID %>');
            if (txttimeset.value == "") {
                Message = Message + "Please Enter Time Set!!\n";
            }
            var txtminutes = document.getElementById('<%=txtminutes.ClientID %>');
            if (txtminutes != null) {
                if (txtminutes.value == "") {
                    Message = Message + "Please Enter Time Difference!!\n";
                }
            }

            var datefrom = new Date(txtfrom.value);
            var dateto = new Date(txtto.value);
            if (datefrom > dateto) {
                Message = Message + "From Date can not greater than To Date !!\n";
            }
            if (Message == "") {
                return confirm("Do you want to save data?");
            }
            else {
                alert(Message);
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td style="width: 40%" valign="top">
                        <table border="1" cellspacing="2" width="100%">
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label9" CssClass="labelbold" Text="Company" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label10" CssClass="labelbold" Text="Branch" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="lblempcode" CssClass="labelbold" Text="Emp. Code" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <asp:TextBox ID="txtempcode" CssClass="textboxm" Width="95%" MaxLength="100" runat="server"
                                        placeholder="Type Emp. Code here (Optional)" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; border-style: dotted" colspan="2" align="center">
                                    <asp:Label ID="Label3" CssClass="labelbold" Text="Type More than one emp. code comma Separated like (001,002)"
                                        ForeColor="Red" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label6" CssClass="labelbold" Text="Department" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <asp:DropDownList ID="DDdepartment" CssClass="dropdown" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label7" CssClass="labelbold" Text="Designation" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <asp:DropDownList ID="DDDesignation" CssClass="dropdown" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label8" CssClass="labelbold" Text="Shift" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <asp:DropDownList ID="DDShift" CssClass="dropdown" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label1" CssClass="labelbold" Text="From Date" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtfromdate" CssClass="textboxm" Width="95%" runat="server" Enabled="false" />
                                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                                    runat="server" PopupButtonID="imgfromdate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:RequiredFieldValidator ID="reqfromdate" ControlToValidate="txtfromdate" runat="server"
                                                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="imgfromdate" CausesValidation="false" AlternateText="Click here to display calender"
                                                    ImageUrl="~/Images/calendar.png" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label2" CssClass="labelbold" Text="To Date" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txttodate" CssClass="textboxm" Width="95%" runat="server" Enabled="false" />
                                                <asp:CalendarExtender ID="calto" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                                    runat="server" PopupButtonID="imgtodate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtfromdate"
                                                    runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="imgtodate" CausesValidation="false" AlternateText="Click here to display calender"
                                                    ImageUrl="~/Images/calendar.png" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%; border-style: none">
                                    <fieldset>
                                        <legend>
                                            <asp:Label Text="Adjustment Type" ForeColor="Red" CssClass="labelbold" runat="server" />
                                        </legend>
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:RadioButton ID="Rdintime" CssClass="radiobuttonnormal" Text="In Time" runat="server"
                                                        Checked="true" GroupName="a" />
                                                    <asp:RadioButton ID="Rdouttime" CssClass="radiobuttonnormal" Text="Out Time" runat="server"
                                                        GroupName="a" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%" colspan="2">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 30%">
                                                <asp:Label ID="lbltime" Text="Time Before/After" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 25%">
                                                <asp:TextBox ID="txttimebefore_after" CssClass="textboxm" Width="95%" runat="server"
                                                    placeholder="hh:mm" Style="text-align: center" onblur="validatetime(this);" onkeypress="AddColon(this)" />
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="Label4" Text="Time Set" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 25%">
                                                <asp:TextBox ID="txttimeset" CssClass="textboxm" Width="95%" runat="server" placeholder="hh:mm"
                                                    Style="text-align: center" onblur="validatetime(this);" onkeypress="AddColon(this)" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%" colspan="2" runat="server" visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:Label ID="Label5" Text="Time Difference(in Minutes)" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 70%">
                                                <asp:TextBox ID="txtminutes" CssClass="textboxm" Width="95%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%; border-style: dotted" align="right">
                                    <asp:Button ID="btngetdata" Text="Get Data" CssClass="buttonnorm" runat="server"
                                        OnClick="btngetdata_Click" OnClientClick="if (!validategetdata())return; this.disabled=true;this.value = 'wait ...';"
                                        UseSubmitBehavior="false" />
                                    <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click"
                                        UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                    <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%; border-style: dotted">
                                    <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 60%" valign="top">
                        <fieldset>
                            <legend>
                                <asp:Label Text="Employee Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                            </legend>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 100%">
                                        <div style="width: 100%; max-height: 450px; overflow: auto">
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
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldate" Text='<%#Bind("Date") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="In / Out Time ">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblin_outtime" Text='<%#Bind("IN_OUTTIME") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblempid" Text='<%#Bind("empid") %>' runat="server" />
                                                            <asp:Label ID="lblin_outsecond" Text='<%#Bind("IN_OUTSECOND") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
