<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderChecklist.aspx.cs"
    Inherits="Masters_Order_FrmOrderChecklist" Title="Repeat Order" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <%-- <script language ="javascript" type ="text/javascript" ></script>--%>
    <%--<script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
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
        function ontextchanged() {
            var intFlag = 0;
            var strErrMsg = "";
            var dtDate = document.getElementById("CPH_Form_TxtOrderDate").value
            var dtDate1 = document.getElementById("CPH_Form_TxtDeliveryDate").value
            var orderdate = new Date(dtDate);
            var delivery = new Date(dtDate1);
            if (orderdate > delivery) {
                strErrMsg = strErrMsg + "Delivery Date Cann't Be Shorter Than Order Date \n";
                document.getElementById("CPH_Form_TxtDeliveryDate").value = document.getElementById("CPH_Form_TxtOrderDate").value;
                intFlag++;
            }
            // Display error message if a field is not completed
            if (intFlag != 0) {
                alert(strErrMsg);

                return false;
            }
            else
                return true;
        }
        function checkDate() {
            // define date string to test
            var SDate = new date(document.getElementById("CPH_Form_TxtOrderDate").value).toDateString();
            var EDate = new date(document.getElementById("CPH_Form_TxtDeliveryDate").value).toDateString();
            var alertReason1 = 'Dispatch Date must be greater than or equal to Order Date.'
            var alertReason2 = 'Dispatch Date can not be less than Order Date.';
            var endDate = new Date(EDate);
            var startDate = new Date(SDate);
            if (SDate != '' && EDate != '' && startDate > endDate) {
                alert(alertReason1);
                document.getElementById(DispatchDate).value = "";
                return false;
            }
            else if (SDate == '') {
                alert("Please enter Start Date");
                return false;
            }
            else if (EDate == '') {
                alert("Please enter End Date");
                return false;
            }
        }
        
    </script>--%>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkedit" Text="FOR EDIT" CssClass="checkboxbold" AutoPostBack="True"
                            runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                    </td>
                </tr>
            </table>
            <div id="maindiv">
                <table width="50%" style="border: 1px solid Gray; border-radius: 1px; width: 50%;">
                    <tr>
                        <td align="right" colspan="2" class="tdstyle">
                            <%-- <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" EDIT ORDER" AutoPostBack="True"
                                OnCheckedChanged="ChkEditOrder_CheckedChanged" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            COMPANY NAME*
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="CUSTOMER CODE*"></asp:Label>
                        </td>
                        <td class="tdstyle" colspan="2">
                            CUST ORDER NO*
                        </td>
                        <td class="tdstyle">
                            Doc. No.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="150px" TabIndex="0"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                Width="150px" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TxtCustOrderNo" runat="server" CssClass="textb" AutoPostBack="True"
                                TabIndex="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqUser" runat="server" ControlToValidate="TxtCustOrderNo"
                                CssClass="errormsg" ErrorMessage="Please, Enter Customer Number!">*</asp:RequiredFieldValidator>
                            <%-- <asp:DropDownList CssClass="dropdown" ID="DDCustOrderNo" runat="server" AutoPostBack="True"
                                Visible="false" Width="150px" OnSelectedIndexChanged="DDCustOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                        </td>
                        <td id="TDDocno" runat="server" visible="false">
                            <table>
                                <tr>
                                    <%--<td>
                                    <asp:Label ID="Label38" Text="Doc No." CssClass="labelbold" runat="server" />
                                </td>--%>
                                    <td>
                                        <asp:DropDownList ID="DDDocNo" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDDocNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtdocno" CssClass="textb" runat="server" Enabled="false" BackColor="LightGray" />
                        </td>
                        <%--<td>
                            <asp:DropDownList CssClass="dropdown" ID="ddordertype" runat="server" AutoPostBack="True"
                                Width="150px" TabIndex="4">
                            </asp:DropDownList>
                        </td>--%>
                    </tr>
                    <%--<tr id="trneworder" runat="server">
                        <td class="tdstyle">
                            OUR ORDER NO*
                        </td>
                        <td class="tdstyle">
                            ORDER DATE*
                        </td>
                        <td id="td1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblcustname" runat="server" Text="Cust.Order DATE*"></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="LblDelvDate" runat="server" Text="DELIVERY DATE*"></asp:Label>
                        </td>
                        <td id="td" runat="server" class="tdstyle">
                            <asp:Label ID="lblduedate" runat="server" Text="FROM ORDER NO"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtLocalOrderNo" runat="server" CssClass="textb" TabIndex="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtOrderDate" runat="server" TabIndex="7" OnTextChanged="TxtOrderDate_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtOrderDate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="TDTxtcustorderdt" runat="server" visible="false">
                            <asp:TextBox CssClass="textb" ID="Txtcustorderdt" runat="server" AutoPostBack="true"
                                Visible="false" TabIndex="9"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                TargetControlID="Txtcustorderdt">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" AutoPostBack="true"
                                TabIndex="8" onchange="javascript: ontextchanged();"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtDeliveryDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDFromOrderNo" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDFromOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:CheckBox ID="CHKFORCURRENTCONSUMPTION" Text="  FOR CURRENT CONSUMPTION" runat="server"
                                Visible="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <asp:Label ID="Lblmessage" runat="server" Text="" ForeColor="red" Font-Bold="true"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <asp:Label ID="lblvalidMessage" runat="server" Text="" ForeColor="red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>--%>
                    <td colspan="6" align="left">
                        <asp:Label ID="Lblmessage" runat="server" Text="" ForeColor="red" Font-Bold="true"
                            Visible="false"></asp:Label>
                    </td>
                    <tr>
                        <td colspan="6" align="left">
                            <asp:Label ID="lblvalidMessage" runat="server" Text="" ForeColor="red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="50%" style="border: 1px solid Gray; border-radius: 1px; width: 50%;">
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="ChkbuyerOrder" Text="buyer order sheet" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtdatebuyer" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="caldate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtdatebuyer">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chksystemorder" Text="system order entry" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtsystemorder" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtsystemorder">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkproforma" Text="PI(PROFORMA INVOICE)" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtproforma" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtproforma">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkrawmaterial" Text="raw material consumption entry" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtrawmetrial" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtrawmetrial">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkaccessories" Text="accessories consumption entry" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtaccessories" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtaccessories">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkBOM" Text="BOM" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtBOM" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtBOM">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkspecification" Text="specification" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtspecification" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtspecification">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkppm" Text="ppm(PRE PRODUCTION MEETING)" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtppm" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtppm">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chckprocessrate" Text="PROCESS RATES" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtprocessrate" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender8" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtprocessrate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkta" Text="T&A & PRODUCTION PLANNING" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtta" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender9" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtta">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" class="tdstyle">
                            <asp:CheckBox ID="chkchecklist" Text="DOCUMENT CHECKLIST" runat="server" />
                        </td>
                        <td style="width: 25%;" align="right">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ID="txtchecklist" CssClass="textb" Width="95%" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender10" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtchecklist">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table width="50%">
                    <tr>
                        <td style="text-align: cente">
                            <asp:Button Text="Save" ID="BtnSave" CssClass="buttonnorm" runat="server" OnClick="BtnSave_Click"
                                OnClientClick="return confirm('Do you want to Save Data ?')" />
                        </td>
                        <td style="text-align: right">
                            <asp:Button Text="Preview" ID="btnpreview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" runat="server" ForeColor="Red" Font-Size="Small" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hndocid" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
