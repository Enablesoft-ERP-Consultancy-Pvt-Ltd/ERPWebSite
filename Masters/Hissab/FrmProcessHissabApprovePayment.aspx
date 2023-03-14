<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master" 
    CodeFile="FrmProcessHissabApprovePayment.aspx.cs" Inherits="Masters_Hissab_FrmProcessHissabApprovePayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmProcessHissabApprovePayment.aspx";
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
            if (document.getElementById('CPH_Form_Date').value == "") {
                alert("Pls fill date....!");
                document.getElementById('CPH_Form_Date').focus();
                return false;
            }
            
//            if (document.getElementById('CPH_Form_TxtAmt').value == "") {
//                alert("Pls fill amount....!");
//                document.getElementById('CPH_Form_TxtAmt').focus();
//                return false;
//            }
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
                            <br />
                            <asp:DropDownList ID="DDEmployerName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>                      
                       
                        <td>
                            <asp:Label Text=" Date" runat="server" ID="Label4" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="Date_CalendarExtender" runat="server" Enabled="True" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="TRApprovePaymentNo" runat="server" visible="false" >
                        <td>
                            <asp:Label Text="Approve PaymentNo" runat="server" ID="Label5" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtApprovePaymentNo" runat="server" Width="100px" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
               
                <table>
                               
                    <tr id="TRApprovalAmt" runat="server" visible="false">
                        <td colspan="8">
                            <div style="height: 180px; overflow: auto">
                                <asp:GridView ID="GVApprovalAmt" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CellPadding="4" DataKeyNames="Id" EmptyDataText="No Data Found!" ForeColor="#333333" PageSize="100"
                                    OnPageIndexChanging="GVApprovalAmt_PageIndexChanging" Width="100%" OnRowDataBound="GVApprovalAmt_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" HorizontalAlign="Left" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <Columns>
                                    <asp:TemplateField HeaderText="">
                                               <%-- <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
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
                                        <asp:TemplateField HeaderText="Approve Bill Amt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApproveBillAmt" Text='<%#Bind("ApproveAmt") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Already Approve Amt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAlreadyApproveAmt" Text='<%#Bind("AlreadyApproveAmt") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Pending Approve Amt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPendingApproveAmt" Text='<%#Bind("PendingApproveAmt") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                         <asp:TemplateField HeaderText="Pending Approve Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPendingApproveAmt" Width="70px" BackColor="Yellow" runat="server" Text='<%#Bind("PendingApproveAmt") %>' onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApproveId" runat="server" Text='<%#Bind("Id") %>'></asp:Label>
                                                 <asp:Label ID="lblProcessId" Text='<%#Bind("ProcessId") %>' runat="server" />
                                                  <asp:Label ID="lblPartyId" Text='<%#Bind("EmpId") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                </asp:GridView>
                            </div>
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
