<%@ Page Title="Salary Define Master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsalarymaster.aspx.cs" Inherits="Masters_Payroll_frmsalarymaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function NewForm() {
            window.location.href = "frmsalarymaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
//                    selectedindex = $("#<%=DDpayrolltype.ClientID %>").attr('selectedIndex');
//                    if (selectedindex <= 0) {
//                        Message = Message + "Please Select Payroll Type !!\n";
//                    }

                    var txtbasicpay = document.getElementById('<%=txtbasicpay.ClientID %>');
                    if (txtbasicpay.value == "" || txtbasicpay.value == "0") {
                        Message = Message + "Please Enter Basic Pay. !!\n";
                    }
                    var txteffectivefrom = document.getElementById('<%=txteffectivefrom.ClientID %>');
                    if (txteffectivefrom.value == "" || txteffectivefrom.value == "0") {
                        Message = Message + "Please Enter Effective from !!\n";
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
        function CheckBoxClick(objref) {
            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <table border="1">
                    <tr>
                        <td>
                            <asp:Label ID="LblCompanyName" Text="Company Name" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="180px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LblBranchMaster" Text="Branch Name" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="180px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LblDepartment" Text="Department" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDDepartment" CssClass="dropdown" Width="180px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LblDesignation" Text="Designation" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDDesignation" CssClass="dropdown" Width="180px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Payroll_Type" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDpayrolltype" CssClass="dropdown" Width="180px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDpayrolltype_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Employee_Name" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDemp" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDemp_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="BtnShow" CssClass="buttonnorm" Text="Show" runat="server" OnClick="BtnShow_Click" />
                        </td>
                    </tr>
                </table>
                <table border="1">
                    <tr>
                        <td>
                            <asp:Label ID="lblempcode" Text="Employee_Code" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:TextBox ID="txtempcode" CssClass="textb" Width="150px" runat="server" AutoPostBack="true"
                                OnTextChanged="txtempcode_TextChanged" />
                            <asp:AutoCompleteExtender ID="txtempcode_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeecode" EnableCaching="true"
                                CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtempcode"
                                UseContextKey="True" ContextKey="0" MinimumPrefixLength="2">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label6" Text="Effective From" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:TextBox ID="txteffectivefrom" CssClass="textb" Width="100px" runat="server" />
                            <asp:CalendarExtender ID="caleffectfrom" runat="server" TargetControlID="txteffectivefrom"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="Basic_Pay" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:TextBox ID="txtbasicpay" CssClass="textb" Width="150px" runat="server" AutoPostBack="true"
                                OnTextChanged="txtbasicpay_TextChanged" onkeypress="return isNumberKey(event);" />
                        </td>
                        <td>
                            <asp:Label ID="lblgrosssal" CssClass="labelbold" Text="Gross salary" runat="server" />
                            <br />
                            <asp:TextBox ID="txtgrosssal" CssClass="textb" Width="150px" Enabled="false" BackColor="LightYellow"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label5" CssClass="labelbold" Text="Net. salary" runat="server" />
                            <br />
                            <asp:TextBox ID="txtnetsal" CssClass="textb" Width="150px" Enabled="false" BackColor="LightYellow"
                                runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="ChkForAllowanceChange" Text ="For Allowance Change" runat="server" />
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; margin-top: 0%">
                    <div style="float: left; width: 45%">
                        <fieldset>
                            <legend>
                                <asp:Label ID="lblallowances" Text="Allowances" CssClass="labelbold" ForeColor="Red"
                                    runat="server" />
                            </legend>
                            <table border="0">
                                <tr>
                                    <td>
                                        <div style="max-height: 200px; overflow: auto">
                                            <asp:GridView ID="DGallowances" runat="server" AutoGenerateColumns="false" CssClass="grid-views"
                                                EmptyDataText="No records found.." OnRowDataBound="DGallowances_RowDataBound">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Allowance Title">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblallowancetitle" Text='<%#Bind("ParameterName") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Allowance Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblallowancetype" Text='<%#Bind("Allowance_deductiontype") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtallowanceamount" Text='<%#Bind("AMount") %>' Width="70px" runat="server"
                                                                AutoPostBack="true" OnTextChanged="txtallowanceamount_TextChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblallowanceid" Text='<%#Bind("ParameterId") %>' runat="server" />
                                                            <asp:Label ID="lblallowancemaxcapingamt" Text='<%#Bind("Maxcapingamount") %>' runat="server" />
                                                            <asp:Label ID="lblallowancemincapingamt" Text='<%#Bind("Mincapingamount") %>' runat="server" />
                                                            <asp:Label ID="lblallowance_type" Text='<%#Bind("Allowance_Type") %>' runat="server" />
                                                            <asp:Label ID="lblallowancepercent_amount" Text='<%#Bind("percent_amount") %>' runat="server" />
                                                            <asp:Label ID="lblAllowance_Deduction_Id" Text='<%#Bind("Allowance_Deduction_Id") %>'
                                                                runat="server" />
                                                            <asp:Label ID="lbltaxableallowance" Text='<%#Bind("Taxable") %>' runat="server" />
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
                    <div style="float: right; width: 45%">
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label4" Text="Deductions" CssClass="labelbold" ForeColor="Red" runat="server" />
                            </legend>
                            <table border="0">
                                <tr>
                                    <td>
                                        <div style="max-height: 200px; overflow: auto">
                                            <asp:GridView ID="DGDeductions" runat="server" AutoGenerateColumns="false" CssClass="grid-views"
                                                EmptyDataText="No records found.." OnRowDataBound="DGDeductions_RowDataBound">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Deduction Title">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldeductiontitle" Text='<%#Bind("ParameterName") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Deduction Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldeductiontype" Text='<%#Bind("Allowance_deductiontype") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtdeductionamount" Text='<%#Bind("AMount") %>' Width="70px" runat="server"
                                                                AutoPostBack="true" OnTextChanged="txtdeductionamount_TextChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldeductionid" Text='<%#Bind("ParameterId") %>' runat="server" />
                                                            <asp:Label ID="lbldeductionmaxcapingamt" Text='<%#Bind("Maxcapingamount") %>' runat="server" />
                                                            <asp:Label ID="lbldeductionmincapingamt" Text='<%#Bind("Mincapingamount") %>' runat="server" />
                                                            <asp:Label ID="lbldeduction_type" Text='<%#Bind("Allowance_Type") %>' runat="server" />
                                                            <asp:Label ID="lbldeductionpercent_amount" Text='<%#Bind("percent_amount") %>' runat="server" />
                                                            <asp:Label ID="lbldeductiontaxable" Text='<%#Bind("Taxable") %>' runat="server" /><asp:Label
                                                                ID="lbldeductionmastertypeid" Text='<%#Bind("Allowance_Deduction_Id") %>' runat="server" />
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
                </div>
                <div style="width: 100%; margin-top: 0%">

                </div> 

                <div style="width: 90%;">                   
                    <table border="0" style="width: 100%">
                        
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="Label9" Text="Emp Deduction" CssClass="labelbold" ForeColor="Red"
                                            Font-Size="Small" runat="server" />
                                    </legend>
                                    <div style="max-height: 300px; overflow: auto">
                                       <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    EmptyDataText="No. Records found." OnRowDataBound="DG_RowDataBound">
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
                                        <asp:TemplateField HeaderText="EMP CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEMPCODE" Text='<%#Bind("EMPCODE") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EMP NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEMPNAME" Text='<%#Bind("EMPNAME")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BASIC PAY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBASICPAY" Text='<%#Bind("BASICPAY")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EFFECTIVE FROM">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEFFECTIVEFROM" Text='<%#Bind("EFFECTIVEFROM")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GROSS PAY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGROSSPAY" Text='<%#Bind("GROSSPAY")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NET PAY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNETPAY" Text='<%#Bind("NETPAY")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" Text='<%#Bind("ID") %>' runat="server" />
                                                <asp:Label ID="lblEmpID" Text='<%#Bind("EmpID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>



              <%--  <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
                    <table border="0">
                        <tr>
                            <td>
                                
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <div style="width: 90%;">
                    <table border="1" style="width: 100%;">
                        <tr>
                            <td align="right">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                                            <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                                            <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return NewForm();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width: 100%">
                        <%--<tr>
                            <td runat="server" visible="false">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblalrdy" Text="Already Defined" CssClass="labelbold" ForeColor="Red"
                                            Font-Size="Small" runat="server" />
                                    </legend>
                                    <div style="max-height: 300px; overflow: auto">
                                        <asp:GridView ID="DGdetails" AutoGenerateColumns="False" CssClass="grid-views" EmptyDataText="No Records found.."
                                            runat="server" OnRowCancelingEdit="DGdetails_RowCancelingEdit" OnRowEditing="DGdetails_RowEditing"
                                            OnRowDataBound="DGdetails_RowDataBound" OnRowUpdating="DGdetails_RowUpdating">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <PagerStyle CssClass="PagerStyle" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Basic Pay">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbasicpaydetail" Text='<%#Bind("Basic_pay") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txteditbasicpay" Text='<%#Bind("Basic_pay") %>' Width="100px" runat="server"
                                                            onkeypress="return isNumberKey(event);" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payroll Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpayrolltypedetail" Text='<%#Bind("Payrolltypename") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="DDgridpayrolltype" CssClass="dropdown" Width="150px" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Effective From">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbleffectivefromdetail" Text='<%#Bind("Effectivefrom") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtediteffectivefrom" Text='<%#Bind("effectivefrom") %>' Width="100px"
                                                            runat="server" />
                                                        <asp:CalendarExtender ID="caltxtediteffectivefrom" TargetControlID="txtediteffectivefrom"
                                                            runat="server" Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Effective To">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbleffectivetodetail" Text='<%#Bind("Effectiveto") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsalaryid" Text='<%#Bind("Id") %>' runat="server" />
                                                        <asp:Label ID="lblgridpayrolltypeid" Text='<%#Bind("payrolltypeid") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField DeleteText="" ShowEditButton="false" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="Label7" Text="Already Defined" CssClass="labelbold" ForeColor="Red"
                                            Font-Size="Small" runat="server" />
                                    </legend>
                                    <div style="max-height: 300px; overflow: auto">
                                        <asp:GridView ID="DgallowancesDetail" CssClass="grid-views" EmptyDataText="No Records found.."
                                            runat="server">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <PagerStyle CssClass="PagerStyle" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
