<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmUpdateFinisherJobRate.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Hissab_FrmUpdateFinisherJobRate"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmUpdateFinisherJobRate.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx");
        }
        //        function CheckAllCheckBoxes() {
        //            if (document.getElementById('CPH_Form_ChkForAllSelect').checked == true) {
        //                var gvcheck = document.getElementById('CPH_Form_DGDetail');
        //                var i;
        //                for (i = 1; i < gvcheck.rows.length; i++) {
        //                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
        //                    inputs[0].checked = true;
        //                }
        //            }
        //            else {
        //                var gvcheck = document.getElementById('CPH_Form_DGDetail');
        //                var i;
        //                for (i = 1; i < gvcheck.rows.length; i++) {
        //                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
        //                    inputs[0].checked = false;
        //                }
        //            }
        //        }

        function ValidationSave() {

            if (document.getElementById('CPH_Form_DDProcessName') != null) {
                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
                    alert("Process name must have a value....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name ....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
            }
            //            if (document.getElementById('CPH_Form_DDEmployerName') != null) {
            //                if (document.getElementById('CPH_Form_DDEmployerName').options.length == 0) {
            //                    alert("Employee name must have a value....!");
            //                    document.getElementById("CPH_Form_DDEmployerName").focus();
            //                    return false;
            //                }
            //                else if (document.getElementById('CPH_Form_DDEmployerName').options[document.getElementById('CPH_Form_DDEmployerName').selectedIndex].value == 0) {
            //                    alert("Please select employee name ....!");
            //                    document.getElementById("CPH_Form_DDEmployerName").focus();
            //                    return false;
            //                }
            //            }
            //            if (document.getElementById('CPH_Form_ChkForEdit').checked == true) {
            //                if (document.getElementById('CPH_Form_DDSlipNo') != null) {
            //                    if (document.getElementById('CPH_Form_DDSlipNo').options.length == 0) {
            //                        alert("Slip no must have a value....!");
            //                        document.getElementById("CPH_Form_DDSlipNo").focus();
            //                        return false;
            //                    }
            //                    else if (document.getElementById('CPH_Form_DDSlipNo').options[document.getElementById('CPH_Form_DDSlipNo').selectedIndex].value == 0) {
            //                        alert("Please select slip no ....!");
            //                        document.getElementById("CPH_Form_DDSlipNo").focus();
            //                        return false;
            //                    }
            //                }
            //            }
            //            var k = 0;
            //            for (i = 1; i < document.getElementById('CPH_Form_DGDetail').rows.length; i++) {
            //                var inputs = document.getElementById('CPH_Form_DGDetail').rows[i].getElementsByTagName('input');
            //                if (inputs[0].checked == true) {
            //                    k = k + 1;
            //                    i = document.getElementById('CPH_Form_DGDetail').rows.length;
            //                }
            //            }
            //            if (k == 0) {
            //                alert("Please select atleast one check box....!");
            //                return false;
            //            }
            return confirm('Do You Want To Save?')
        }
        
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td id="TDRadioButton" runat="server" colspan="2" align="right" visible="false">
                        <asp:RadioButton ID="RDStockWise" runat="server" Text="Stock Wise" Font-Bold="True"
                            GroupName="OrderType" Visible="false" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RDQtyWise" runat="server" Text="Qty Wise" Font-Bold="True" GroupName="OrderType"
                            Visible="false" />
                    </td>
                    <td align="right" colspan="3">
                        <asp:CheckBox ID="ChkForEdit" Font-Bold="True" runat="server" Text=" For Edit" AutoPostBack="True"
                            OnCheckedChanged="ChkForEdit_CheckedChanged" Visible="false" />
                        &nbsp; &nbsp;<asp:CheckBox ID="ChkForExist" Font-Bold="True" runat="server" Text="Check to existing voucher show"
                            AutoPostBack="true" OnCheckedChanged="ChkForExist_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" Text=" Process Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDProcessName" runat="server" TargetControlID="DDProcessName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label1" Text="Report Type" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDReportType" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDReportType_SelectedIndexChanged">
                            <asp:ListItem Value="0">Both</asp:ListItem>
                            <asp:ListItem Value="1">Checked</asp:ListItem>
                            <asp:ListItem Value="2">UnChecked</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td id="TDQuality" runat="server" visible="false">
                        <asp:Label ID="Label9" Text="Quality Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDQuality" runat="server" Width="150px" CssClass="dropdown">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDQuality"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label3" Text="Party Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDEmployerName" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDEmployerName" runat="server" TargetControlID="DDEmployerName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TDPoOrderNo" runat="server" visible="false">
                        <asp:Label ID="Label4" Text="PO OrderNO." runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDPOOrderNo" runat="server" Width="100px" AutoPostBack="true"
                            CssClass="dropdown" OnSelectedIndexChanged="DDPOOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPOOrderNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <%-- <td id="TDsrno" runat="server" visible="false">
                        <asp:Label ID="lblsrno" Text="Sr No." CssClass="labelbold" runat="server" />
                        <br />
                        <asp:DropDownList ID="DDsrno" CssClass="dropdown" Width="100px" runat="server">
                        </asp:DropDownList>
                    </td>--%>
                    <td id="TDDDSlipNo" runat="server">
                        <asp:Label ID="Label5" Text="Slip No" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDSlipNo" runat="server" Width="100px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDSlipNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDSlipNo" runat="server" TargetControlID="DDSlipNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TDFFromDate" runat="server" visible="true" class="style4">
                        <asp:Label ID="Label6" Text="From Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtFromDate" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtFromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td id="TDFToDate" runat="server" visible="true" style="width: 102px">
                        <asp:Label ID="Label7" Text=" To Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtToDate" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtToDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="right">
                        <asp:Button ID="BtnShowData" runat="server" Text="Show Data" ForeColor="White" CssClass="buttonnorm"
                            UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to Show Data?')) return; this.disabled=true;this.value = 'wait ...';"
                            OnClick="BtnShowData_Click" Visible="False" />
                        <%--  &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnDelete" Text="Delete" runat="server"
                            OnClick="BtnDelete_Click" OnClientClick="return ValidationDelete();" Visible="false" />--%>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div id="DivDGDetail" style="max-height: 500px; width: 1000px; overflow: scroll">
                            <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                OnSorting="DGDetail_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="gvheaders" ForeColor="White" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
                                    <asp:TemplateField HeaderText="Sr.No" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" Text=' <%#Container.DisplayIndex+1 %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Code" SortExpression="CustomerCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerCode" Text='<%#Bind("CustomerCode") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RecpNo" SortExpression="RcpNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecpNo" Text='<%#Bind("RcpNo") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contractor" SortExpression="EmpName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quality" SortExpression="QualityName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Design" SortExpression="DesignName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignName" Text='<%#Bind("DesignName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color" SortExpression="Colorname">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColorName" Text='<%#Bind("Colorname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shape" SortExpression="ShapeName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShapeName" Text='<%#Bind("ShapeName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size" SortExpression="FinishingMtSize">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" Text='<%#Bind("FinishingMtSize") %>' runat="server" Visible="false" />
                                            <asp:TextBox ID="txtSize" runat="server" Text='<%#Eval("FinishingMtSize") %>' OnTextChanged="txtSize_TextChanged"
                                                AutoPostBack="True" Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pcs" SortExpression="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RPcs" SortExpression="RPcs">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRPcs" Text='<%#Bind("RPcs") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Area" SortExpression="Area">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArea" Text='<%#Bind("Area") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate2" SortExpression="Rate2">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate2" Text='<%#Bind("Rate2") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" Text='<%#Bind("Amount") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Penality" SortExpression="Penality">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPenality" Text='<%#Bind("Penality") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Paid Amount" SortExpression="PaidAmt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaidAmt" Text='<%#Bind("PaidAmt") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receive Date" SortExpression="ReceiveDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceiveDate" Text='<%#Bind("ReceiveDate","{0:dd-MMM-yyyy}") %>'
                                                runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TDS Amt" SortExpression="TDSAmt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTDSAmt" Text='<%#Bind("TDSAmt") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Calc Option" SortExpression="CalcName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCalcName" Text='<%#Bind("CalcName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VoucherNo" SortExpression="VoucherNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVoucherNo" Text='<%#Bind("VoucherNo") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VoucherDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVoucherDate" Text='<%#Bind("VoucherDate","{0:dd-MMM-yyyy}") %>'
                                                runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag" Visible="false">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblflag" Text='<%#Bind("Flag") %>' runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("Item_Id") %>'></asp:Label>
                                            <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("QualityId") %>'></asp:Label>
                                            <asp:Label ID="lblDesignId" runat="server" Text='<%#Bind("DesignId") %>'></asp:Label>
                                            <asp:Label ID="lblColorId" runat="server" Text='<%#Bind("ColorId") %>'></asp:Label>
                                            <asp:Label ID="lblSizeId" runat="server" Text='<%#Bind("SizeId") %>'></asp:Label>
                                            <asp:Label ID="lblShapeId" runat="server" Text='<%#Bind("ShapeId") %>'></asp:Label>
                                            <asp:Label ID="lblFRRateId" runat="server" Text='<%#Bind("FRRateId") %>'></asp:Label>
                                            <asp:Label ID="lblCalOptionId" runat="server" Text='<%#Bind("CalOptionId") %>'></asp:Label>
                                            <asp:Label ID="lblCustomerId" runat="server" Text='<%#Bind("CustomerId") %>'></asp:Label>
                                            <asp:Label ID="lblItemFinishedId" runat="server" Text='<%#Bind("Item_Finished_id") %>'></asp:Label>
                                            <asp:Label ID="lblTDSPercentage" runat="server" Text='<%#Bind("TDSPercentage") %>'></asp:Label>
                                            <asp:Label ID="lblProcessRecDetailId" runat="server" Text='<%#Bind("Process_Rec_Detail_Id") %>'></asp:Label>
                                            <asp:Label ID="hnRate" runat="server" Text='<%#Bind("hnRate") %>'></asp:Label>
                                            <asp:Label ID="hnRateId" runat="server" Text='<%#Bind("hnRateId") %>'></asp:Label>
                                            <asp:Label ID="hnRate2" runat="server" Text='<%#Bind("hnRate2") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label Text="Total Pcs" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalpcs" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:Label ID="Label10" Text="Total Area" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalarea" CssClass="textb" Width="120px" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:Label ID="Label11" Text="Total Amount" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtamount" CssClass="textb" Width="120px" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:Label ID="Label13" Text="Total Penality" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txttotalpenality" CssClass="textb" Width="120px" runat="server"
                            Enabled="false" />
                    </td>
                    <td>
                        <asp:Label ID="Label12" Text="Total PaidAmt" CssClass="labelbold" runat="server" />
                        <br />
                        <asp:TextBox ID="txtTotalPaidAmt" CssClass="textb" Width="120px" runat="server" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td id="TDDate" runat="server" visible="true" colspan="5">
                        <asp:Label ID="Label8" Text="Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtDate" runat="server" Width="90px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtDate">
                        </asp:CalendarExtender>
                        &nbsp; &nbsp;<asp:CheckBox ID="ChkForVoucher" Font-Bold="True" runat="server" Text="Check to create new voucher"
                            AutoPostBack="true" OnCheckedChanged="ChkForVoucher_CheckedChanged" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <%-- <asp:CheckBox ID="chkstocknowise" Text="Stock No Wise Report" runat="server" CssClass="labelbold" />--%>
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                            OnClientClick="return ValidationSave();" />
                        <asp:Button CssClass="buttonnorm" ID="BtnSaveFinishingConsumption" Text="Save Consumption"
                            runat="server" OnClick="BtnSaveFinishingConsumption_Click" UseSubmitBehavior="false"
                            OnClientClick="if (!ValidationSave())return; this.disabled=true;this.value = 'wait ...';" />
                        <%-- <asp:Button CssClass="buttonnorm" ID="Button1" Text="Save Consumption" runat="server" OnClick="BtnSaveFinishingConsumption_Click"
                             UseSubmitBehavior="false" OnClientClick="if (!ValidationSave())return; this.disabled=true;this.value = 'wait ...';" />--%>
                        <%--<asp:Button ID="BtnPriview" runat="server" Text="Preview" CssClass="buttonnorm"
                            OnClick="BtnPriview_Click" />
                               <asp:Button ID="btnprintvoucher" runat="server" Text="Print Voucher" 
                            CssClass="buttonnorm" onclick="btnprintvoucher_Click"
                             />--%>
                        <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
