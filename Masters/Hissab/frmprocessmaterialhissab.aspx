<%@ Page Title="Material Hissab" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmprocessmaterialhissab.aspx.cs" Inherits="Masters_Hissab_frmprocessmaterialhissab" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmprocessmaterialhissab.aspx";
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function CheckBoxClick(objref) {

            var gridView = document.getElementById('<%= DGDetail.ClientID %>');
            var Totalamt = 0;
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');

                if (inputs[0].type == "checkbox") {

                    if (inputs[0].checked == true) {

                        if (inputs[1].type == "text") {
                            Totalamt += parseFloat(inputs[1].value);
                        }
                    }
                }
            }
            debugger;
            var NetAmt = 0;
            var DeductionAmt = 0;
            var AdditionAmt = 0;
            var txttotalamt = document.getElementById('<%=txttotalamt.ClientID %>');
            var txtNetAmt = document.getElementById('<%=txtNetAmt.ClientID %>');
            DeductionAmt = document.getElementById('<%=txtDeductionAmt.ClientID %>').value;
            if (DeductionAmt == '') {
                DeductionAmt = 0;
            }
            AdditionAmt = document.getElementById('<%=txtAdditionAmt.ClientID %>').value;
            if (AdditionAmt == '') {
                AdditionAmt = 0;
            }

            txttotalamt.value = Totalamt.toFixed(2);
            NetAmt = parseFloat(txttotalamt.value) + parseFloat(AdditionAmt) - parseFloat(DeductionAmt);
            txtNetAmt.value = NetAmt;
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=BtnSave.ClientID %>").click(function () {
                    var Message = "";

                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDProcessname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process. !!\n";
                    }
                    selectedindex = $("#<%=DDpartyname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Party Name. !!\n";
                    }

                    if ($("#<%=Tdbillno.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDBillNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Bill No. !!\n";
                        }
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <table>
                <tr>
                    <td>
                        <asp:Label Text="Company Name" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" Text="Process Name" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDProcessname" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="DDProcessname_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" Text="Party Name" CssClass="labelbold" runat="server" />
                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" Edit Bill No." CssClass="checkboxbold"
                            AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" Visible="false" />
                        <br />
                        <asp:DropDownList ID="DDpartyname" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="DDpartyname_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td id="Tdbillno" runat="server" visible="false">
                        <asp:Label ID="Label3" Text="Bill No." CssClass="labelbold" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDBillNo" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="DDBillNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label5" Text="Bill Date" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtbilldate" CssClass="textb" runat="server" Width="100px" />
                        <asp:CalendarExtender ID="calbilldate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtbilldate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <div style="height: 300px; overflow: scroll; margin-left: 5%">
                            <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                OnRowDataBound="DGDetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" onclick="return CheckBoxClick(this);" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="issueno" HeaderText="Issue No.">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Total Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txttotalamt" Text='<%#Bind("totalamt") %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblflag" runat="server" Text='<%#Bind("flag") %>' />
                                            <asp:Label ID="lblissueid" runat="server" Visible="false" Text='<%# Bind("issueid") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td valign="top">
                        <asp:Label Text="Total Amount" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalamt" CssClass="textb" Width="120px" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label4" Text="Bill No." CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtbillno" CssClass="textb" runat="server" Width="100px" />
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label7" Text="Deduction Amount" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtDeductionAmt" CssClass="textb" Width="120px" Enabled="true" runat="server"
                            AutoPostBack="true" onkeypress="return isNumber(event);" OnTextChanged="txtDeductionAmt_TextChanged" />
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label8" Text="Addition Amount" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtAdditionAmt" CssClass="textb" Width="120px" Enabled="true" runat="server"
                            AutoPostBack="true" onkeypress="return isNumber(event);" OnTextChanged="txtAdditionAmt_TextChanged" />
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label9" Text="Net Amount" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtNetAmt" CssClass="textb" Width="120px" Enabled="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="Label6" Text="Remark" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtremark" CssClass="textb" Width="500px" runat="server" TextMode="MultiLine"
                            Height="43px" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click" />
                        <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                        <asp:Button CssClass="buttonnorm" ID="Btnpreview" runat="server" Text="Preview" OnClick="Btnpreview_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
