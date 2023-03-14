<%@ Page Title="Mock drill Entry" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmMockdrillEntry.aspx.cs" Inherits="Masters_Payroll_FrmMockdrillEntry" %>

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
        function keypressintime(id) {
            debugger;
            var row = id.parentNode.parentNode;
            var rowindex = row.rowIndex - 1;
            var lblshiftintime = document.getElementById("CPH_Form_Dgdetail_lblshiftintime_" + rowindex);
            var lblshiftouttime = document.getElementById("CPH_Form_Dgdetail_lblshiftouttime_" + rowindex);
            var lblintime = document.getElementById("CPH_Form_Dgdetail_lblintime_" + rowindex);
            var txtintime = document.getElementById("CPH_Form_Dgdetail_txtintime_" + rowindex);

            if (lblshiftintime.innerHTML == "" || lblshiftouttime.innerHTML == "") {
                alert("Shift In Time or Shift Out Time can not blank.");
                txtintime.value = lblintime.value;
                return;
            }
            else {
                var shiftintime = getNum(parseFloat(lblshiftintime.innerHTML));
                var shiftouttime = getNum(parseFloat(lblshiftouttime.innerHTML)) + 0.15;
                var intime = getNum(parseFloat(txtintime.value.replace(":", ".")));

                if (intime >= shiftintime && intime <= shiftouttime) {

                }
                else {
                    alert("Invalid Intime entered. In time should enter between Shift in Time and Shift Out Time");
                    txtintime.value = lblintime.innerHTML;
                }
            }
        }
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label9" CssClass="labelbold" Text="Company" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="95%">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Branch" />
                            <br />
                            <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="95%">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Year" />
                            <br />
                            <asp:DropDownList ID="DDYear" runat="server" CssClass="dropdown" Width="95%" OnSelectedIndexChanged="DDYear_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table border="1" cellspacing="2" width="100%">
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
                </table>
                <table border="1" cellspacing="2" width="100%">
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
                            <asp:Label ID="Label4" CssClass="labelbold" Text="Remark" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:TextBox ID="TxtRemark" CssClass="textboxm" Width="95%" MaxLength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; border-style: dotted" align="right">
                            <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click"
                                UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to Save Data?')) return; this.disabled=true;this.value = 'wait ...';" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm()" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; border-style: dotted">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin: 0% 20% 0% 20%">
                <asp:GridView ID="Dgdetail" runat="server" CssClass="grid-views" AutoGenerateColumns="false"
                    DataKeyNames="ID" EmptyDataText="No data fetched...." Width="100%" OnRowDeleting="Dgdetail_RowDeleting">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Emp. Code">
                            <ItemTemplate>
                                <asp:Label ID="lblempcode" Text='<%#Bind("Empcode") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Emp. Name">
                            <ItemTemplate>
                                <asp:Label ID="lblempname" Text='<%#Bind("EmpName") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="150px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lbldate" Text='<%#Bind("Date") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" Text='<%#Bind("Remark") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DEL" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="DEL" OnClientClick="return confirm('Do You Want To Delete?')"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
