<%@ Page Title="Payroll Type Define" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpayrollparameterdetails.aspx.cs" Inherits="Masters_Payroll_frmpayrollparameterdetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Addpayrolltypemaster() {
            window.open('../Payroll/AddPayrollTypeMaster.aspx', '', 'Height=350px,width=500px');
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 10%">
                <table border="0" style="width: 100%">
                    <tr>
                        <td align="center">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpayrolltype" Text="Payroll Type" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDpayrolltype" CssClass="dropdown" runat="server" Width="200px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDpayrolltype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqfield" ValidationGroup="a" runat="server" ControlToValidate="DDpayrolltype"
                                            InitialValue="0" ErrorMessage="*" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnaddpayrolltype" Text="+" runat="server" CssClass="buttonnorm"
                                            ToolTip="Add New Payroll Type" OnClientClick="Addpayrolltypemaster();" />
                                        <asp:Button ID="refreshpayrolltypemaster" runat="server" Style="display: none" 
                                            onclick="refreshpayrolltypemaster_Click"  />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin: 0% 10% 0% 10%">
                <fieldset>
                    <legend>
                        <asp:Label ID="lblfsallowances" Text="Allowances" CssClass="labelbold" ForeColor="Red"
                            Font-Size="Small" runat="server" />
                    </legend>
                    <table border="0">
                        <tr>
                            <td>
                                <div style="max-height: 200px; overflow: auto">
                                    <asp:GridView ID="DGallowances" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found...">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkallowances" Text="" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Allowances">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblallowances" Text='<%#Bind("ParameterName") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDallowancetaxable" runat="server">
                                                        <asp:ListItem Text="No" />
                                                        <asp:ListItem Text="Yes" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Allowance Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDallowancetype" runat="server">
                                                        <asp:ListItem Text="Percentage of Basic Salary" Value="0" />
                                                        <asp:ListItem Text="Fixed Amount" Value="1" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount/Percentage">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtallowanceamount" Width="100px" runat="server" onkeypress="return isNumberKey1(event);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Min. Caping Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtallowancemincapingamt" Width="100px" runat="server" onkeypress="return isNumberKey1(event);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max. Caping Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtallowancemaxcapingamt" Width="100px" runat="server" onkeypress="return isNumberKey1(event);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblallowanceid" Text='<%#Bind("parameterid") %>' runat="server" />
                                                    <asp:Label ID="lblallowancemastertypeid" runat="server" Text='<%#Bind("Typeid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="margin: 0% 10% 0% 10%">
                <fieldset>
                    <legend>
                        <asp:Label ID="Label1" Text="Deductions" CssClass="labelbold" ForeColor="Red" Font-Size="Small"
                            runat="server" />
                    </legend>
                    <table border="0">
                        <tr>
                            <td>
                                <div style="max-height: 200px; overflow: auto">
                                    <asp:GridView ID="DGDeductions" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found...">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkdeductions" Text="" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deductions">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldeductions" Text='<%#Bind("ParameterName") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDdeductiontaxable" runat="server">
                                                        <asp:ListItem Text="No" />
                                                        <asp:ListItem Text="Yes" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deduction Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDdeductiontype" runat="server">
                                                        <asp:ListItem Text="Percentage of Basic Salary" Value="0" />
                                                        <asp:ListItem Text="Fixed Amount" Value="1" />
                                                        <asp:ListItem Text="Percentage of Gross Salary" Value="2" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount/Percentage">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtdeductionamount" Width="100px" runat="server" onkeypress="return isNumberKey1(event);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Min. Caping Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtdeductionmincapingamt" Width="100px" runat="server" onkeypress="return isNumberKey1(event);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max. Caping Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtdeductionmaxcapingamt" Width="100px" runat="server" onkeypress="return isNumberKey1(event);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldeductionid" Text='<%#Bind("parameterid") %>' runat="server" />
                                                    <asp:Label ID="lbldeductionmastertypeid" runat="server" Text='<%#Bind("Typeid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="margin: 0% 20% 0% 10%">
                <table border="0" style="width: 100%">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                ValidationGroup="a" />
                            <asp:Button ID="brnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
