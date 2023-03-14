<%@ Page Title="Size" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmNewSize.aspx.cs" Inherits="Masters_Carpet_FrmNewSize" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">
        function CloseForm() {
            if (window.opener.document.getElementById('CPH_Form_refreshsize2')) {
                window.opener.document.getElementById('CPH_Form_refreshsize2').click();
                self.close();
            }
            else if (window.opener.document.getElementById('refreshsize')) {
                window.opener.document.getElementById('refreshsize').click();
                self.close();
            }

            else if (window.opener.document.getElementById('CPH_Form_BtnRefreshSize')) {
                window.opener.document.getElementById('CPH_Form_BtnRefreshSize').click();
                self.close();
            }
            else if (window.opener.document.getElementById('BtnRefreshSize')) {
                window.opener.document.getElementById('BtnRefreshSize').click();
                self.close();
            }
            else {
                window.location.href = "../../main.aspx";
            }


        }
    </script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function ClickNew() {
            window.location.href = "FrmNewSize.aspx";
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
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else if (charCode == 46) {
                return false;
            }
            else {
                return true;
            }
        }


        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }
        }
            
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 450px" class="controls">
                <table cellspacing="5" cellpadding="5">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtsize" runat="server" CssClass="textb" Visible="false" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="false" CssClass="textb" Width="80px"></asp:TextBox>
                            <asp:Label ID="lblStatus" runat="server" Font-Bold="true" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right; display: none;">
                            <asp:Label ID="lblew" Text="Unit Name" runat="server" Font-Bold="true" />
                        </td>
                        <td style="display: none">
                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="dropdown" Width="80px" Enabled="false">
                            </asp:DropDownList>
                            <%--<asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="ddunit_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown" Width="80px" >
                            </asp:DropDownList>--%>
                            <%--<cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                        </td>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblQualityType" runat="server" Text="Quality Type" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlQualityType" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlQualityType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblQuality" runat="server" Text="Quality" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlQuality" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblshapeyname" runat="server" Text="Shape" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlShape" runat="server" Width="120px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <div runat="server" id="EffectiveDate">
                            <td class="tdstyle" style="text-align: right">
                                <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" Font-Bold="true"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEffectiveDate"
                                    ErrorMessage="Please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <%--<b style="color: Red"> &nbsp; *</b>--%>
                                <br />
                            </td>
                            <td>
                                <asp:TextBox ID="tbEffectiveDate" runat="server" CssClass="textb" Width="150px" ReadOnly="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="tbEffectiveDate">
                                </asp:CalendarExtender>
                            </td>
                        </div>
                    </tr>
                    <tr id="Tr4" runat="server" visible="true">
                        <td colspan="3">
                            <asp:Label ID="lblAreaSquareFt" runat="server" Text="Export Size" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblAWidthFt" runat="server" Text="Width (Ft)" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbAWidthFt" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbAWidthFt_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" onkeypress="return isNumberKey(event);" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblALengthFt" runat="server" Text="Length (Ft)" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbALengthFt" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbALengthFt_TextChanged"
                                onkeypress="return isNumberKey(event);" AutoPostBack="True" onFocus="this.select()"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblAAreaSqFt" runat="server" Text="Area In Sq.Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbAAreaSqFt" runat="server" CssClass="textb" Width="120px" ReadOnly="true"
                                onFocus="this.select()"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="BtnCalCulate" CssClass="buttonnorm" runat="server" Text="Calculate"
                                OnClick="BtnCalCulate_Click" Width="50%" />
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblAWidthMtr" runat="server" Text="Width (Cm)" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbAWidthMtr" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbAWidthMtr_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblALengthMtr" runat="server" Text="Length (Cm)" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbALengthMtr" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbALengthMtr_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblAAreaSqMtr" runat="server" Text="Area In Sq.Mtr" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbAAreaSqMt" runat="server" CssClass="textb" Width="120px" ReadOnly="true"
                                onFocus="this.select()"></asp:TextBox>
                        </td>
                        <td class="tdstyle" colspan="2">
                            <asp:CheckBox ID="cbProduction" runat="server" AutoPostBack="true" OnCheckedChanged="cbProduction_CheckedChanged" />
                            Production Full Area&nbsp;&nbsp;
                            <asp:CheckBox ID="cbFinishing" runat="server" AutoPostBack="true" OnCheckedChanged="cbFinishing_CheckedChanged" />
                            Finishing Full Area
                            <%-- <asp:Button ID="Button1" CssClass="buttonnorm" runat="server" Text="Calculate"
                                OnClick="BtnCalCulate_Click"  Width="90%" />--%>
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="2" class="tdstyle">
                         <asp:Label ID="lblTextMsg" runat="server" Text="Check For Checkbox to insert width or lengthin Foot like (5.11)" Font-Bold="true"></asp:Label>
                           
                        </td>
                      
                        <td class="tdstyle">
                        &nbsp;
                           
                        </td>
                        <td>
                          &nbsp;
                        </td>
                        <td class="tdstyle">
                        &nbsp;
                          
                        </td>
                        <td>
                            Production Full Area
                        </td>
                        <td class="tdstyle">
                          &nbsp;
                           
                        </td>
                        <td>
                            Finishing Full Area
                        </td>
                        
                    </tr>--%>
                    <%--<tr> 
                    <td colspan="2" class="tdstyle">
                       &nbsp;
                           
                        </td>
                      
                        <td class="tdstyle">
                        &nbsp;
                           
                        </td>
                        <td>
                          &nbsp;
                        </td>
                        <td class="tdstyle">
                        &nbsp;
                          
                        </td>
                        <td>
                           &nbsp;
                        </td>
                        
                        <td colspan="2">
                          
                        </td></tr>--%>
                    <tr id="Tr1" runat="server" visible="true">
                        <td colspan="3">
                            <asp:Label ID="lblProductionArea" runat="server" Text="Production Size" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblPWidthFt" runat="server" Text="Width Ft(')" Font-Bold="true"></asp:Label>
                            <asp:HiddenField ID="hnPWidthFt" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbPWidthFt" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPWidthFt_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey1(event);" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPWidthIn" runat="server" Text="Width In(')" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPWidthIn" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPWidthIn_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey1(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPKhapW" runat="server" Text="Khap W" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPKhapW" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPKhapW_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey1(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPAreaSqYd" runat="server" Text="Area In Sq. Yd" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPAreaSqYd" runat="server" CssClass="textb" Width="120px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblPLengthFt" runat="server" Text="Length Ft(')" Font-Bold="true"></asp:Label>
                            <asp:HiddenField ID="hnPLengthFt" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbPLengthFt" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPLengthFt_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey1(event);" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPLengthIn" runat="server" Text="Length In(')" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPLengthIn" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPLengthIn_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey1(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPKhapL" runat="server" Text="Khap L" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPKhapL" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPKhapL_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey1(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPFullAreaSqYd" runat="server" Text="Full Area Sq. Yd" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPFullAreaSqYd" runat="server" CssClass="textb" Width="120px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblPWidthCm" runat="server" Text="Width Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPWidthCm" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPWidthCm_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPLengthCm" runat="server" Text="Length Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPLengthCm" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbPLengthCm_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblPAreaSqMt" runat="server" Text="Area Sq. Mt" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPAreaSqMt" runat="server" CssClass="textb" Width="120px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr5" runat="server" visible="true">
                        <td colspan="3">
                            <asp:Label ID="lblFinishingSize" runat="server" Text="Finishing Size" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblFWidthFt" runat="server" Text="Width Ft(')" Font-Bold="true"></asp:Label>
                            <asp:HiddenField ID="hnFWidthFt" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbFWidthFt" runat="server" CssClass="textb1" ReadOnly="true" Width="120px"
                                OnTextChanged="tbFWidthFt_TextChanged" onkeypress="return isNumberKey(event);"
                                onFocus="this.select()" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblFWidthIn" runat="server" Text="Width In(')" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbFWidthIn" runat="server" CssClass="textb1" ReadOnly="true" Width="120px"
                                OnTextChanged="tbFWidthIn_TextChanged" onkeypress="return isNumberKey(event);"
                                onFocus="this.select()" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblFLengthFt" runat="server" Text="Length Ft(')" Font-Bold="true"></asp:Label>
                            <asp:HiddenField ID="hnFLengthFt" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbFLengthFt" runat="server" CssClass="textb1" ReadOnly="true" Width="120px"
                                OnTextChanged="tbFLengthFt_TextChanged" onkeypress="return isNumberKey(event);"
                                onFocus="this.select()" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblFLengthIn" runat="server" Text="Length In(')" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbFLengthIn" runat="server" CssClass="textb1" ReadOnly="true" Width="120px"
                                OnTextChanged="tbFLengthIn_TextChanged" onkeypress="return isNumberKey(event);"
                                onFocus="this.select()" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblFAreaSqYd" runat="server" Text="Area In Sq Yd" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbFAreaSqYd" runat="server" CssClass="textb1" Width="120px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: right">
                            <asp:Label ID="lblFWidthCm" runat="server" Text="Width Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbFWidthCm" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbFWidthCm_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                            <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" ></asp:TextBox>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblFLengthCm" runat="server" Text="Length Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbFLengthCm" runat="server" CssClass="textb" Width="120px" OnTextChanged="tbFLengthCm_TextChanged"
                                onFocus="this.select()" onkeypress="return isNumberKey(event);" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblFAreaSqMt" runat="server" Text="Area Sq Mt" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbFAreaSqMt" runat="server" CssClass="textb1" Width="120px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10" runat="server" align="right">
                            <asp:Button ID="btnClear0" CssClass="buttonnorm" Text="New" runat="server" Width="56px"
                                OnClientClick="return ClickNew()" />
                            <asp:Button ID="btnSave" CssClass="buttonnorm" runat="server" OnClick="btnSave_Click"
                                Text="Save" Width="56px" OnClientClick="return preventMultipleSubmissions();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            <%--  <asp:Button ID="btnUpdate" CssClass="buttonnorm" runat="server" OnClick="btnUpdate_Click" Text="Update" Width="56px"
                            OnClientClick="return preventMultipleSubmissions();" />--%>
                            <%-- <asp:Button ID="btnclose0" CssClass="buttonnorm" Text="Close" runat="server" Width="48px"
                                OnClientClick="return CloseForm();" OnClick="btnclose0_Click"  />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="False" OnClick="btndelete_Click"  />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" OnClientClick="return priview();"
                                CssClass="buttonnorm preview_width" OnClick="btnpreview_Click" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td id="td1" runat="server" colspan="10">
                            
                            
                        </td>
                    </tr>--%>
                </table>
                <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                <div id="Gride" style="width: 945px; max-height: 600px; overflow: auto; float: left">
                    <asp:GridView ID="gdMasterSize" runat="server" DataKeyNames="SizeId" CssClass="grid-views"
                        Width="60%" OnRowDataBound="gdMasterSize_RowDataBound" EmptyDataText="No. Records found."
                        OnPageIndexChanging="gdMasterSize_PageIndexChanging" OnSelectedIndexChanged="gdMasterSize_SelectedIndexChanged"
                        AllowPaging="True" PageSize="50" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Do you want to update status?');"
                                        Text="Enable/Disable" OnClick="LinkButton1_Click" />
                                </ItemTemplate>
                                <ItemStyle Width="400px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" Text='<%#Bind("adddate")%>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblToDate" Text='<%#Bind("UpdateDate") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitName" Text='<%#Bind("UnitName") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="400px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExpWidthMs_Ft">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpWidthMs_Ft" Text='<%#Bind("ExpWidthMs_Ft")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExpLengthMs_Ft">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpLengthMs_Ft" Text='<%#Bind("ExpLengthMs_Ft") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExpAreaMs_Ft">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpAreaMs_Ft" Text='<%#Bind("Export_Area") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shape Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblShapeName" runat="server" Text='<%#Bind("ShapeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FootWidth">
                                <ItemTemplate>
                                    <asp:Label ID="lblFootWidth" runat="server" Text='<%#Bind("FootWidth") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="InchWidthofFoot">
                                <ItemTemplate>
                                    <asp:Label ID="lblInchWidthofFoot" runat="server" Text='<%#Bind("InchWidthofFoot") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ProductionAreaSqYard">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductionAreSqYard" Text='<%#Bind("ProductionAreaSqYard") %>'
                                        runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FinishingAreaSqYard">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinishingAreSqYard" Text='<%#Bind("FinishingAreaSqYard") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ProdWidth_Ms">
                                <ItemTemplate>
                                    <asp:Label ID="lblProdWidth_Ms" Text='<%#Bind("ProdWidth_Ms") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ProdLength_Ms">
                                <ItemTemplate>
                                    <asp:Label ID="lblProdLength_Ms" Text='<%#Bind("ProdLength_Ms") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ProdArea_Ms">
                                <ItemTemplate>
                                    <asp:Label ID="lblProdArea_Ms" Text='<%#Bind("Production_MT_Area") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ActualAreaSqYard">
                                <ItemTemplate>
                                    <asp:Label ID="lblActualAreaSqYard" Text='<%#Bind("ActualAreaSqYard") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ActualFullAreaSqYard">
                                <ItemTemplate>
                                    <asp:Label ID="lblActualFullAreaSqYard" Text='<%#Bind("ActualFullAreaSqYard") %>'
                                        runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MtrWidthxLength">
                                <ItemTemplate>
                                    <asp:Label ID="lblMtrWidthxLength" Text='<%#Bind("MtrSize") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MtrArea">
                                <ItemTemplate>
                                    <asp:Label ID="lblMtrArea" Text='<%#Bind("MtrArea") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finishing_Ft_Size">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinishing_Ft_Size" Text='<%#Bind("Finishing_Ft_Size") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finishing_Mt_Size">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinishing_Mt_Size" Text='<%#Bind("Finishing_Mt_Size") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="KhapWidth">
                                <ItemTemplate>
                                    <asp:Label ID="lblKhapWidth" Text='<%#Bind("KhapWidth") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="KhapLength">
                                <ItemTemplate>
                                    <asp:Label ID="lblKhapLength" Text='<%#Bind("KhapLength") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" Text='<%#Bind("TypeS") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSizeId" Text='<%#Bind("SizeId") %>' runat="server" />
                                    <asp:Label ID="lblQualityTypeId" Text='<%#Bind("QualityTypeId") %>' runat="server" />
                                    <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />
                                    <asp:Label ID="ShapeId" Text='<%#Bind("ShapeId") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--  <asp:GridView ID="GridView1" runat="server" OnRowDataBound="gdSize_RowDataBound" OnSelectedIndexChanged="gdSize_SelectedIndexChanged"
                                    DataKeyNames="Sr_No" CssClass="grid-views" >
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass ="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                </asp:GridView>--%>
                </div>
                <div id="divattached" runat="server" style="width: 945px; max-height: 200px; overflow: auto;
                    float: left; display: block">
                    <table cellspacing="5" cellpadding="5">
                        <tr>
                            <td>
                                <b>Quality Type</b>
                                <asp:DropDownList ID="ddlQualityTypeAttac" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlQualityTypeAttac_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <b>Quality</b>
                                <asp:DropDownList ID="ddlQualityAttac" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnAttached" runat="server" CssClass="buttonnorm" OnClick="btnAttached_Click"
                                    Text="Attached Size" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hnSizeId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
