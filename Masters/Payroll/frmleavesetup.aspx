<%@ Page Title="Leave Setup" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmleavesetup.aspx.cs" Inherits="Masters_Payroll_frmleavesetup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Addgroupmaster() {
            window.open('../Payroll/AddGroupMaster.aspx', '', 'Height=350px,width=500px');
        }
        function NewForm() {
            window.location.href = "frmleavesetup.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var selectedindex = $("#<%=DDfrommonth.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select From Month!!\n";
            }
            var selectedindex = $("#<%=DDfromyear.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select From Year!!\n";
            }
            var selectedindex = $("#<%=DDtomonth.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select To Month!!\n";
            }
            var selectedindex = $("#<%=DDtoyear.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select To Year!!\n";
            }

            selectedindex = $("#<%=DDempgroup.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Employee Group. !!\n";
            }


            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table border="1" cellspacing="2" width="100%">
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label CssClass="labelbold" Text="From" runat="server" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:DropDownList ID="DDfrommonth" CssClass="dropdown" runat="server" Width="100%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:DropDownList ID="DDfromyear" CssClass="dropdown" runat="server" Width="100%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70%; border-style: dotted">
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label1" CssClass="labelbold" Text="To" runat="server" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:DropDownList ID="DDtomonth" CssClass="dropdown" runat="server" Width="100%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:DropDownList ID="DDtoyear" CssClass="dropdown" runat="server" Width="100%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70%; border-style: dotted">
                    </td>
                </tr>
                <tr>
                    <td style="width: 11%; border-style: dotted">
                        <asp:Label ID="Label2" CssClass="labelbold" Text="Employee Group" runat="server" />
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="DDempgroup" CssClass="dropdown" Width="100%" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="DDempgroup_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5%">
                        <asp:Button ID="btnaddgroup" runat="server" CssClass="buttonnorm" OnClientClick="Addgroupmaster();"
                            Text="+" ToolTip="Add New Group" />
                        <asp:Button ID="btnrefreshgrouponleavetype" runat="server" Style="display: none"
                            OnClick="btnrefreshgrouponleavetype_Click" />
                    </td>
                    <td style="width: 70%; border-style: dotted">
                    </td>
                </tr>
            </table>
            <table border="1" cellspacing="2">
                <tr>
                    <td style="width: 100%; border-style: dotted">
                        <div style="width: 100%; overflow: auto; max-height: 500px">
                            <asp:GridView ID="dgdetail" CssClass="grid-views" runat="server" AutoGenerateColumns="false"
                                EmptyDataText="No data fetched..." ShowFooter="true" Width="100%" OnRowDataBound="dgdetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Leave">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddleavename" CssClass="dropdown" Width="100%" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <HeaderStyle Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type of Leave">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdfixed" Text="Fixed" CssClass="radiobutton" GroupName="rd"
                                                runat="server" Checked="true" />
                                            <asp:RadioButton ID="rdcalculate" Text="Calculate" CssClass="radiobutton" GroupName="rd"
                                                runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maximum No. of Leaves alloted">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmaxleaveallot" CssClass="textboxm" Width="100%" runat="server"
                                                onkeypress="return isNumberKey(event);" Style="text-align: center" />
                                            <asp:DropDownList ID="ddmaxleavetype" CssClass="dropdown" Width="100%" runat="server">
                                                <asp:ListItem Text="per month" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="per year" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Days worked in a Month">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtdaysworkedinmonth" CssClass="textboxm" Width="100%" runat="server"
                                                onkeypress="return isNumberKey(event);" Style="text-align: center" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Duration of Leave Earned">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtdurationofleaveearned" CssClass="textboxm" Width="100%" runat="server"
                                                onkeypress="return isNumberKey(event);" Style="text-align: center" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Leave">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDballeave" CssClass="dropdown" Width="100%" runat="server">
                                                <asp:ListItem Text="Carry Forward" Value="1" />
                                                <asp:ListItem Text="Lapse" Value="2" />
                                                <asp:ListItem Text="Encash" Value="3" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Gender">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDleavegender" CssClass="dropdown" Width="100%" runat="server">
                                                <asp:ListItem Text="Male" Value="1" />
                                                <asp:ListItem Text="Female" Value="2" />
                                                <asp:ListItem Text="Both" Value="3" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnaddnewrow" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                CausesValidation="false" OnClick="btnaddnewrow_Click" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltypeofleave" Text='<%#Bind("typeofleave") %>' runat="server" />
                                            <asp:Label ID="lblleaveid" Text='<%#Bind("leaveid") %>' runat="server" />
                                            <asp:Label ID="lblmaxleaveallotid" Text='<%#Bind("maxleaveallotid") %>' runat="server" />
                                            <asp:Label ID="lblballeaveid" Text='<%#Bind("balleaveid") %>' runat="server" />
                                            <asp:Label ID="lblleavegenderid" Text='<%#Bind("leavegenderid") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; border-style: dotted" align="right">
                        <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click"
                            OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                        <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return NewForm();" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; border-style: dotted">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnid" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
