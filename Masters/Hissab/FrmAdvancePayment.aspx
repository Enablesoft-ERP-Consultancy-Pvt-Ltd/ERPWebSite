<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAdvancePayment.aspx.cs"
    Inherits="Masters_Hissab_FrmAdvancePayment" MasterPageFile="~/ERPmaster.master"
    Title="ADVANCE PAYMENT" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmAdvancePayment.aspx";
        }
        function KeyDownHandlerWeaverIdscan(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnsearchemp.ClientID %>').click();
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx");
        }
        function ValidationSave() {
            if (document.getElementById('CPH_Form_DDCompanyName') != null) {
                if (document.getElementById('CPH_Form_DDCompanyName').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
            }
            //            if (document.getElementById('CPH_Form_DDProcessName') != null) {
            //                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
            //                    alert("Process name must have a value....!");
            //                    document.getElementById("CPH_Form_DDProcessName").focus();
            //                    return false;
            //                }
            //                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
            //                    alert("Please select process name ....!");
            //                    document.getElementById("CPH_Form_DDProcessName").focus();
            //                    return false;
            //                }
            //            }
            if (document.getElementById('CPH_Form_DDEmployerName') != null) {
                if (document.getElementById('CPH_Form_DDEmployerName').options.length == 0) {
                    alert("Employee name must have a value....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
                    alert("Please select employee name ....!");
                    document.getElementById("CPH_Form_DDEmployerName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtAdvance').value == "" || document.getElementById('CPH_Form_TxtAdvance').value == "0") {
                alert("Pls fill Advance amount....!");
                document.getElementById('CPH_Form_TxtAdvance').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Please Enter Numeric vlaue....');
                return false;

            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 20%">
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="Label14" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="95%" onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                        <asp:Button Text="employee" ID="btnsearchemp" CssClass="buttonnorm" runat="server"
                            OnClick="btnsearchemp_Click" Style="display: none" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label Text="Company Name" runat="server" ID="lbl" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="100%" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20%">
                        <asp:Label Text=" Process Name" runat="server" ID="Label7" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDProcessName" runat="server" Width="100%" CssClass="dropdown"
                            OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20%">
                        <asp:Label Text="  Party Name" runat="server" ID="Label1" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDEmployerName" runat="server" Width="100%" CssClass="dropdown"
                            OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td id="TDDate" runat="server" visible="true" style="width: 15%">
                        <asp:Label Text="  Date" runat="server" ID="Label2" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtDate" runat="server" Width="95%" CssClass="textb"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 15%">
                        <asp:Label Text=" Advance" runat="server" ID="Label3" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtAdvance" runat="server" Width="95%" CssClass="textb" BackColor="Beige"
                            Text="" onkeypress="return isNumberKey(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 15%">
                        <asp:Label Text=" Voucher No" runat="server" ID="Label4" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtVoucherNo" runat="server" Width="95%" CssClass="textb" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label Text=" Cheque No" runat="server" ID="Label5" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtChequeno" runat="server" Width="95%" CssClass="textb" Height="20px"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20%">
                        <asp:Label Text="Remarks" runat="server" ID="Label6" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtremarks" runat="server" Width="95%" CssClass="textb" TextMode="MultiLine"
                            Height="55px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblErr" ForeColor="Red" CssClass="labelbold" Text="" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="right">
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClientClick="return ValidationSave();"
                            OnClick="BtnSave_Click" />
                        &nbsp;<asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                            OnClick="BtnPriview_Click" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                            OnClientClick="return CloseForm();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <div style="width: 100%; height: 350px; overflow: scroll">
                            <asp:GridView ID="DGAdvanceAmount" Width="100%" runat="server" AllowPaging="True"
                                PageSize="100" AutoGenerateColumns="False" OnRowDataBound="DGAdvanceAmount_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                <PagerStyle CssClass="PagerStyle" />
                                <Columns>
                                    <asp:BoundField DataField="Employee" HeaderText="Employee">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AdvanceAmt" HeaderText="AdvanceAmt">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DeductAmt" HeaderText="DeductAmt">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Date" HeaderText="Date">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VoucherNo" HeaderText="VoucherNo">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Hissab_VoucherNo" HeaderText="Hissab_VoucherNo">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ChequeNo" HeaderText="ChequeNo">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblprocessid" Text='<%#Bind("Processid") %>' runat="server" />
                                            <asp:Label ID="lblid" Text='<%#Bind("VoucherNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" OnClientClick="return confirm('Do you want to delete this row?')"
                                                OnClick="lnkdel_Click" Text="Del"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Voucher">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVoucherPrint" runat="server" CausesValidation="False"
                                                OnClick="lnkVoucherPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        #mask
        {
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 4;
            opacity: 0.4;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
            filter: alpha(opacity=40); /* second!*/
            background-color: Gray;
            display: none;
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowPopup() {
            $('#mask').show();
            $('#<%=pnlpopup.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlpopup.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
        Style="z-index: 111; background-color: White; position: absolute; left: 35%;
        top: 40%; border: outset 2px gray; padding: 5px; display: none">
        <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
            <tr style="background-color: #8B7B8B; height: 1px">
                <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                    align="center">
                    ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                        class="btnPwd" href="#">X</a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Enter Password:
                </td>
                <td>
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px"
                        OnTextChanged="txtpwd_TextChanged" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label19" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
