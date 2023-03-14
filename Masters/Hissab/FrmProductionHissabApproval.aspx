<%@ Page Title="HISSAB APPROVAL" Language="C#" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="FrmProductionHissabApproval.aspx.cs"
    Inherits="Masters_Hissab_FrmProductionHissabApproval" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmProductionHissabApproval.aspx";
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
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
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
        function validate() {
            if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                alert("Please select company name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            var isValid = false;
            var i = 0;
            var gridView = document.getElementById('CPH_Form_DGBillDetail');
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            isValid = true;
                        }
                    }
                }
            }
            if (isValid == false) {
                alert("Please select at least one item");
                inputs[0].checked = false;
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
            if ($("#<%=TDddApprovalNo.ClientID %>").is(':visible')) {
                selectedindex = $("#<%=DDApprovalNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    alert("Please Select Approval No.");
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ChkEdit').checked == true) {
                if (document.getElementById('CPH_Form_DDApprovalNo').options[document.getElementById('CPH_Form_DDApprovalNo').selectedIndex].value == 0) {
                    alert("Please select approval no....!");
                    document.getElementById("CPH_Form_DDApprovalNo").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtAppDate').value == "") {
                alert("Pls fill approval date....!");
                document.getElementById('CPH_Form_TxtAppDate').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtAmt').value == "") {
                alert("Pls fill amount....!");
                document.getElementById('CPH_Form_TxtAmt').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtTDS').value == "") {
                alert("Pls fill tds....!");
                document.getElementById('CPH_Form_TxtTDS').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtNetAmt').value == "") {
                alert("Pls fill netamount....!");
                document.getElementById('CPH_Form_TxtNetAmt').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
     <script type="text/javascript">
         var xPos, yPos;
         var prm = Sys.WebForms.PageRequestManager.getInstance();

         function BeginRequestHandler(sender, args) {
             if ($get('div2') != null) {
                 xPos = $get('div2').scrollLeft;
                 yPos = $get('div2').scrollTop;
             }
         }

         function EndRequestHandler(sender, args) {
             if ($get('div2') != null) {
                 $get('div2').scrollLeft = xPos;
                 $get('div2').scrollTop = yPos;
             }
         }

         prm.add_beginRequest(BeginRequestHandler);
         prm.add_endRequest(EndRequestHandler);
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="50%">
                <tr>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        <asp:Label ID="Label14" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                            onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                        <asp:Button Text="employee" ID="btnsearchemp" CssClass="buttonnorm" runat="server"
                            OnClick="btnsearchemp_Click" Style="display: none" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl" Text="Company Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDCompanyName" runat="server" TargetControlID="DDCompanyName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label1" Text=" Process Name" runat="server" CssClass="labelbold" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="ChkEdit" runat="server" AutoPostBack="True" ForeColor="Red" OnCheckedChanged="ChkEdit_CheckedChanged"
                            Text="Check To Edit" CssClass="checkboxbold" Visible="false" />
                        <br />
                        <asp:DropDownList ID="DDProcessName" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDProcessName" runat="server" TargetControlID="DDProcessName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label2" Text=" Party Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDEmployerName" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchDDEmployerName" runat="server" TargetControlID="DDEmployerName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TDddApprovalNo" runat="server" visible="false">
                        <asp:Label ID="Label3" Text="Approval No" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDApprovalNo" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDApprovalNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDApprovalNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label4" Text="Approval Date" runat="server" CssClass="labelbold" />
                        <asp:TextBox ID="TxtAppDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                        <cc1:CalendarExtender ID="AppDate_CalendarExtender" runat="server" Enabled="True"
                            TargetControlID="TxtAppDate" Format="dd-MMM-yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label5" Text="Approval No" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="TxtAppNo" runat="server" Width="100px" CssClass="textb" ReadOnly="true"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" rowspan="2">
                        <div id="div2" style="height: 180px; overflow: scroll">
                            <asp:GridView ID="DGBillDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                OnRowDataBound="DGBillDetail_RowDataBound" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                OnCheckedChanged="Chkchallan_CheckedChanged" />
                                            <asp:Label ID="lblHissabid" runat="server" Visible="false" Text='<%# Bind("Hissabid") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill NO" />
                                    <asp:BoundField DataField="Amt" HeaderText="Total Amount" />
                                    <asp:BoundField DataField="TDS" HeaderText="Tds" />
                                    <asp:BoundField DataField="Flag" HeaderStyle-ForeColor="AntiqueWhite" ItemStyle-ForeColor="AntiqueWhite">
                                        <ItemStyle ForeColor="AntiqueWhite"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Width="0px" />
                                        <ItemStyle HorizontalAlign="Center" Width="0px" />
                                    </asp:BoundField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgstgrid" Text='<%#Bind("gst") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdditionAmt" Text='<%#Bind("AdditionAmt") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeductionAmt" Text='<%#Bind("DeductionAmt") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaterialDeductionAmt" Text='<%#Bind("MaterialDeductionAmt") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdvanceAmountFolioWise" Text='<%#Bind("AdvanceAmountFolioWise") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                    <td colspan="3" valign="middle">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" Text="Amount" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtAmt" runat="server" Width="150px" CssClass="textb" ReadOnly="True"
                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" Text="GST(%)" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtgst" runat="server" Width="100px" CssClass="textb" AutoPostBack="true"
                                        onkeydown="return (event.keyCode!=13);" onkeypress="return isNumber(event);"
                                        OnTextChanged="txtgst_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" Text=" TDS(%)" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtTDS" runat="server" AutoPostBack="True" Width="100px" CssClass="textb"
                                        OnTextChanged="TxtTDS_TextChanged" onkeypress="return isNumber(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" Text=" TCS(%)" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtTCSPercentage" runat="server" AutoPostBack="True" Width="100px"
                                        CssClass="textb" onkeypress="return isNumber(event);" OnTextChanged="TxtTCSPercentage_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" Text=" Net Amount" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtNetAmt" runat="server" Width="100px" ReadOnly="True" CssClass="textb"
                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="Label9" Text="Remarks" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtRemarks" runat="server" Width="300px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblErr" runat="server" CssClass="labelbold" Font-Size="Small" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="right">
                        <asp:Button ID="BtnNew" runat="server" CssClass="buttonnorm" Text="New" OnClientClick="return NewForm();" />
                        <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClientClick="return validate()"
                            Text="Save" OnClick="BtnSave_Click" />
                        <asp:Button ID="btndel" runat="server" CssClass="buttonnorm" Text="Delete" Visible="false"
                            OnClientClick="return confirm('Do you want to delete this Approval No.?')" OnClick="btndel_Click" />
                        <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm preview_width" OnClick="Preview_Click"
                             Text="Preview" />
                        <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
