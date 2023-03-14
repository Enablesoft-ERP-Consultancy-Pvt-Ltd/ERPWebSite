<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false"
    CodeFile="FrmProcessHissabPayment.aspx.cs" Inherits="Masters_Hissab_FrmProcessHissabPayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmProcessHissabPayment.aspx";
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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckAmount() {

            var chkPassport = document.getElementById("CPH_Form_chkByFolio");
            if (chkPassport.checked) {
                return true;
            }
            else {
                var VarAmt = 0;
                var VarBalAmt = 0;
                if (document.getElementById('CPH_Form_TxtBalAmt').value != "") {
                    VarBalAmt = document.getElementById('CPH_Form_TxtBalAmt').value;
                }
                if (document.getElementById('CPH_Form_TxtAmt').value != "") {
                    VarAmt = document.getElementById('CPH_Form_TxtAmt').value;
                }
                if (parseFloat(VarAmt) > parseFloat(VarBalAmt)) {
                    alert("Pls enter correct amount ....!");
                    document.getElementById('CPH_Form_TxtAmt').value = "";
                    document.getElementById("CPH_Form_TxtAmt").focus();
                    return false;
                }
            }


        }
        function ValidationForDelete() {
            if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                alert("Please select company name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                alert("Please select process name....!");
                document.getElementById("CPH_Form_DDProcessName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
                alert("Please select employee name....!");
                document.getElementById("CPH_Form_DDEmployerName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDApprvNo').options[document.getElementById('CPH_Form_DDApprvNo').selectedIndex].value == 0) {
                alert("Please select approval no....!");
                document.getElementById("CPH_Form_DDApprvNo").focus();
                return false;
            }
            return confirm('Do you want to delete?')
        }
        function Validation() {
            if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                alert("Please select company name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                alert("Please select process name....!");
                document.getElementById("CPH_Form_DDProcessName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
                alert("Please select employee name....!");
                document.getElementById("CPH_Form_DDEmployerName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDApprvNo').options[document.getElementById('CPH_Form_DDApprvNo').selectedIndex].value == 0) {
                alert("Please select approval no....!");
                document.getElementById("CPH_Form_DDApprvNo").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_Date').value == "") {
                alert("Pls fill date....!");
                document.getElementById('CPH_Form_Date').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_PayThrough').options[document.getElementById('CPH_Form_PayThrough').selectedIndex].value == 1) {
                if (document.getElementById('CPH_Form_DDBankName').options[document.getElementById('CPH_Form_DDBankName').selectedIndex].value == 0) {
                    alert("Please select bank name....!");
                    document.getElementById("CPH_Form_DDBankName").focus();
                    return false;
                }
                if (document.getElementById('CPH_Form_TxtChkNo').value == "") {
                    alert("Pls fill check no....!");
                    document.getElementById('CPH_Form_TxtChkNo').focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtAmt').value == "") {
                alert("Pls fill amount....!");
                document.getElementById('CPH_Form_TxtAmt').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="80%">
                <table>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label20" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="95%" onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                            <asp:Button Text="employee" ID="btnsearchemp" CssClass="buttonnorm" runat="server"
                                OnClick="btnsearchemp_Click" Style="display: none" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label Text="Company Name" runat="server" ID="lbl" CssClass="labelbold" />
                            <asp:CheckBox ID="Chkedit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                ToolTip="Check For Edit" AutoPostBack="True" OnCheckedChanged="Chkedit_CheckedChanged"
                                Visible="false" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label Text=" Process Name" runat="server" ID="Label1" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:Label Text=" Party Name" runat="server" ID="Label2" CssClass="labelbold" />
                            <asp:CheckBox ID="chkByFolio" runat="server" Text=" By Folio" CssClass="checkboxbold"
                                ToolTip="Check By Folio" Visible="false" AutoPostBack="True" OnCheckedChanged="chkByFolio_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDEmployerName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%--<td colspan="2" id="tdFolioNo" runat="server" visible="false">                       
                           <asp:Label Text="Folio No" runat="server" ID="Label15" CssClass="labelbold" />                            
                            <br />
                            <asp:DropDownList ID="DDFolioNo" runat="server"  CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>--%>
                        <td colspan="2" id="tdApprovalNo" runat="server" visible="true">
                            <asp:Label Text="Approval No" runat="server" ID="Label3" CssClass="labelbold" />
                            <asp:CheckBox ID="chkcommpay" Text="Comm. payment" CssClass="checkboxbold" runat="server"
                                Visible="false" AutoPostBack="true" OnCheckedChanged="chkcommpay_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDApprvNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="DDApprvNo_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" id="tdVoucherNo" runat="server" visible="false">
                            <asp:Label Text="Voucher No" runat="server" ID="Label15" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDVoucherNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="DDVoucherNo_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label Text=" Date" runat="server" ID="Label4" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Date" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="Date_CalendarExtender" runat="server" Enabled="True" Format="dd-MMM-yyyy"
                                TargetControlID="Date">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label Text="Voucher No" runat="server" ID="Label5" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtVocNo" runat="server" Width="100px" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label Text=" Credit/Debit" runat="server" ID="Label6" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDTrType" runat="server" CssClass="dropdown" Width="100px"
                                OnSelectedIndexChanged="DDTrType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Debit</asp:ListItem>
                                <asp:ListItem Value="1">Credit</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label Text="Pay Amount" runat="server" ID="Label7" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtAmt" runat="server" Width="100px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                onchange="return CheckAmount();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td id="TDuseadvanceamt" runat="server" visible="false">
                            <asp:Label Text="Use Adv. Amount" runat="server" ID="Label21" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtuseadvamt" runat="server" Width="100px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label Text=" Paid Through" runat="server" ID="Label8" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="PayThrough" runat="server" CssClass="dropdown" Width="100px"
                                OnSelectedIndexChanged="PayThrough_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">Cash</asp:ListItem>
                                <asp:ListItem Value="1">Bank</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="Bname" runat="server" colspan="3">
                            <asp:Label Text="Bank Name" runat="server" ID="Label9" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDBankName" runat="server" Height="19px" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="chqNo" runat="server">
                            <asp:Label Text=" Cheque No/ UTR NO" runat="server" ID="Label10" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtChkNo" runat="server" Height="16px" Width="155px" CssClass="textb"
                                 onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>

                        <td id="TDPaymentTransferMode" runat="server" visible="false">
                            <asp:Label Text=" Transfer Mode" runat="server" ID="Label22" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDPaymentTransferMode" runat="server" CssClass="dropdown" Width="100px">                              
                            </asp:DropDownList>
                        </td>

                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label Text="Narration" runat="server" ID="Label11" CssClass="labelbold" />
                            <asp:TextBox ID="TxtNarration" runat="server" CssClass="textb" Width="600px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="tdCrAmt" runat="server" visible="true">
                            <asp:Label Text="Cr Amt" runat="server" ID="Label12" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtCrAmt" runat="server" CssClass="textb" Width="150px" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="tdDrAmt" runat="server" visible="true">
                            <asp:Label Text=" Dr Amt" runat="server" ID="Label13" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtDrAmt" runat="server" CssClass="textb" Width="150px" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="tdBalAmt" runat="server" visible="true">
                            <asp:Label Text="Bal Amt" runat="server" ID="Label14" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtBalAmt" runat="server" CssClass="textb" Width="150px" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:CheckBox ID="ChkAdvanceAmt" runat="server" Text=" Use Advance Amount" CssClass="checkboxbold"
                                OnCheckedChanged="ChkAdvanceAmt_CheckedChanged" AutoPostBack="true" />
                            &nbsp;&nbsp;&nbsp<asp:Label ID="lblAdvance" runat="server" Text=""></asp:Label>
                        </td>
                        <td id="tdFolioAmt" runat="server" visible="false">
                            <asp:Label Text="Total FolioAmt" runat="server" ID="Label16" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtFolioTotalAmt" runat="server" CssClass="textb" Width="120px"
                                ReadOnly="true" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="tdpaidfolioamt" runat="server" visible="false">
                            <asp:Label Text="Total Paid FolioAmt" runat="server" ID="Label17" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtpaidfolioamt" runat="server" CssClass="textb" Width="120px" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="tdbalfolioamt" runat="server" visible="false">
                            <asp:Label Text="Bal. Folio Amt" runat="server" ID="Label18" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtbalfolioamt" runat="server" CssClass="textb" Width="120px" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblErr" runat="server" CssClass="labelbold" Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td colspan="5" align="right">
                            <asp:Button ID="BtnNew" runat="server" CssClass="buttonnorm" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClientClick="return Validation();"
                                Text="Save" OnClick="BtnSave_Click" />
                            <asp:Button ID="BtnUpdate" runat="server" CssClass="buttonnorm" OnClientClick="return Validation();"
                                Text="Update" OnClick="BtnUpdate_Click" Visible="false" />
                            <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm preview_width" Visible="false"
                                Text="Preview" />
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div style="height: 180px; overflow: auto">
                                <asp:GridView ID="dgPayment" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CellPadding="4" DataKeyNames="SrNo" EmptyDataText="No Data Found!" ForeColor="#333333"
                                    OnPageIndexChanging="dgPayment_PageIndexChanging" Width="100%" OnRowUpdating="dgPayment_RowUpdating"
                                    OnRowDataBound="dgPayment_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" HorizontalAlign="Left" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <Columns>
                                       <%-- <asp:BoundField DataField="ApprovalNo" HeaderText="AppNo" />
                                        <asp:BoundField DataField="Date" HeaderText="PayDate" />
                                        <asp:BoundField DataField="VoucherNo" HeaderText="VoucherNo" />
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                        <asp:BoundField DataField="chqCash" HeaderText="PayMode" />
                                        <asp:BoundField DataField="crdr" HeaderText="Credit/Debit" />--%>
                                        
                                        
                                        <asp:TemplateField HeaderText="AppNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApprovalNo" Text='<%#Bind("ApprovalNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="PayDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" Text='<%#Bind("Date") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="VoucherNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVoucherNo" Text='<%#Bind("VoucherNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="TDSAmt" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTdsAmt" Text='<%#Bind("TdsAmt") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountAfterTds" Text='<%#Bind("AmountAfterTds") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="TotalAmount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Text='<%#Bind("Amount") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PayMode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblchqCash" Text='<%#Bind("chqCash") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Credit/Debit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcrdr" Text='<%#Bind("crdr") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="DEL" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" OnClientClick="return ValidationForDelete();"
                                                    OnClick="lnkdel_Click" Text="Del"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Update"
                                                    Text="Preview"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr id="TRApprovalAmt" runat="server" visible="false">
                        <td colspan="8">
                            <div style="height: 100px; overflow: auto">
                                <asp:GridView ID="GVApprovalAmt" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CellPadding="4" DataKeyNames="ApprovalNo" EmptyDataText="No Data Found!" ForeColor="#333333" PageSize="100"
                                    OnPageIndexChanging="GVApprovalAmt_PageIndexChanging" Width="100%" OnRowDataBound="GVApprovalAmt_RowDataBound" 
                                    OnSelectedIndexChanged="GVApprovalAmt_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" HorizontalAlign="Left" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Process Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcessName" Text='<%#Bind("Process_Name") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emp Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approval No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApprovalNo" Text='<%#Bind("ApprvNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="TDS(%)" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTdsPercentage" Text='<%#Bind("TdsPercentage") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="TDSAmt" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTdsAmt" Text='<%#Bind("TdsAmt") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Amount" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountAfterTds" Text='<%#Bind("AmountAfterTds") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Approve Amt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApproveAmt" Text='<%#Bind("PendingHissabPayment") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApproveId" runat="server" Text='<%#Bind("ApprovalNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
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
