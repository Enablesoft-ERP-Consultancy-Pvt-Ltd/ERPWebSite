<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MasterEmployee.ascx.cs"
    Inherits="UserControls_MasterEmployee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
<script type="text/javascript">
    function report() {
        window.open('../../ReportViewer.aspx', '');
    }
    function EmpSelected(source, eventArgs) {
        document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
    }
    function AddDepartment() {
        window.open('AddDepartment.aspx', '', 'Height=400px,width=600px');
    }
    function Addgroupmaster() {
        window.open('../Payroll/AddGroupMaster.aspx', '', 'Height=350px,width=500px');
    }
    function Addloom() {
        var answer = confirm("Do you want to ADD?")
        if (answer) {
            var a = document.getElementById('<%=txtEmpId.ClientID %>').value;
            if (a == "" || a == "0") {
                alert('Plz Select or Insert Employee');
                return false;
            }

            var left = (screen.width / 2) - (1225 / 2);
            var top = (screen.height / 2) - (500 / 2);

            //                window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
            window.open('FrmLoommaster.aspx?a=' + a, 'ADD LOOM MASTER', 'width=1225px, height=500px, top=' + top + ', left=' + left);


        }
    }
    function VendorItemDetail() {
        var answer = confirm("Do you want to SHOW?")

        if (answer) {
            //                var a = document.getElementById('txtEmpId').value;
            //                if (a == "" || a == "0") {
            //                    alert('Plz Select or Insert Employee');
            //                    return false;
            //                }

            var left = (screen.width / 2) - (500 / 2);
            var top = (screen.height / 2) - (400 / 2);


            //                window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
            window.open('FrmVendor_Item_Search.aspx', 'VENDOR ITEM DETAIL', 'width=550px, height=400px, top=' + top + ', left=' + left);
        }
    }

    function Checkforitem() {
        if (document.getElementById('<%=chkforitem.ClientID %>').checked) {
            var answer = confirm("Do you want to ADD?")

            if (answer) {
                var a = document.getElementById('<%=txtEmpId.ClientID %>').value;
                if (a == "" || a == "0") {
                    alert('Plz Select or Insert Employee');
                    return false;
                }

                var left = (screen.width / 2) - (500 / 2);
                var top = (screen.height / 2) - (400 / 2);

                //                window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
                window.open('frmitemspecification.aspx?a=' + a, 'ADD Item ', 'width=550px, height=400px, top=' + top + ', left=' + left);


            }
        }
    }
    function CheckforitemRate() {
        if (document.getElementById('<%=chkvendorrate.ClientID %>').checked) {
            var answer = confirm("Do you want to ADD?")

            if (answer) {
                var a = document.getElementById('<%=txtEmpId.ClientID %>').value;
                if (a == "" || a == "0") {
                    alert('Plz Select or Insert Employee');
                    return false;
                }

                var left = (screen.width / 2) - (500 / 2);
                var top = (screen.height / 2) - (400 / 2);

                //                window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
                window.open('frmvendorwiserate.aspx?vid=' + a, 'ADD Item ', 'width=850px, height=400px, top=' + top + ', left=' + left);


            }
        }
    }

    function CloseForm() {

        var objParent = window.opener;
        if (objParent != null) {
            if (window.opener.document.getElementById('CPH_Form_refreshEmp2')) {
                window.opener.document.getElementById('CPH_Form_refreshEmp2').click();
                self.close();
            }
            else if (window.opener.document.getElementById('refreshEmp')) {
                window.opener.document.getElementById('refreshEmp').click();
                self.close();
            }
            else if (window.opener.document.getElementById('CPH_Form_refreshEmployee')) {
                window.opener.document.getElementById('CPH_Form_refreshEmployee').click();
                self.close();
            }

        }
        else {
            window.location.href = "../../main.aspx";
        }
    }

    function validate() {
        if (document.getElementById('<%=DDDepartment.ClientID %>').options.length == 0) {
            alert("Department Must have a vlaue");
            document.getElementById('<%=DDDepartment.ClientID %>').focus();
            return false;
        }
        else if (document.getElementById('<%=DDDepartment.ClientID %>').selectedIndex == 0) {
            alert("Plz Select Department");
            document.getElementById('<%=DDDepartment.ClientID %>').focus();
            return false;
        }
        if (document.getElementById('<%=txtEmpName.ClientID %>').value == "") {
            alert("Plz Enter Employee Name");
            document.getElementById('<%=txtEmpName.ClientID %>').focus();
            return false;
        }
        return confirm('Do you Want to Save?');
    }
</script>
<div style="width: 100%; background-color: #E3E3E3">
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="Tabemp" ActiveTabIndex="0" runat="server" Width="100%">
                <asp:TabPanel ID="Tabpanelemployeeinformation" HeaderText="Employee/Vendor Information"
                    runat="server">
                    <ContentTemplate>
                        <div style="background-color: #E3E3E3; width: 100%">
                            <div style="width: 100%; float: left; background-color: #E3E3E3">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="tdstyle">
                                                        <asp:TextBox ID="txtEmpId" runat="server" Style="display: none"></asp:TextBox>
                                                        <asp:Label Text="Department" runat="server" ID="lbl" CssClass="labelbold" />
                                                        <asp:Button ID="BtnAddDepartment" runat="server" Style="display: none" EnableTheming="True"
                                                            OnClick="BtnAddDepartment_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDDepartment" CssClass="dropdown" runat="server" Width="140px"
                                                            AutoPostBack="True" OnSelectedIndexChanged="DDDepartment_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnadd" runat="server" Visible="False" CssClass="buttonnorm" OnClientClick="AddDepartment();"
                                                            Text="..." />
                                                    </td>
                                                    <td>
                                                        <asp:Label Text="Emp/Vendor" runat="server" ID="Label3" CssClass="labelbold" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDparttype" runat="server" CssClass="dropdown" Width="150px"
                                                            AutoPostBack="True" OnSelectedIndexChanged="DDparttype_SelectedIndexChanged">
                                                            <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Vendor" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Employee" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2" id="trEmployeeType" runat="server" visible="False">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label21" runat="server" Text="Location" CssClass="labelbold"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlEmployeeType" runat="server" CssClass="dropdown" Width="150px"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged">
                                                                        <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                                                        <asp:ListItem Text="InHouse" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="OutSide" Value="1"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Emp/Vendor Code" runat="server" ID="Label4" CssClass="labelbold" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                                        <asp:TextBox ID="txtempcode" runat="server" CssClass="textb" AutoPostBack="True"
                                                            OnTextChanged="txtempcode_TextChanged" Width="200px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="txtempcode_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeecode" CompletionSetCount="20"
                                                            OnClientItemSelected="EmpSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtempcode"
                                                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label Text="Emp/Vendor Name" runat="server" ID="Label1" CssClass="labelbold" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpName" runat="server" CssClass="textb" Width="200px" AutoPostBack="True"
                                                            OnTextChanged="txtEmpName_TextChanged"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter EmpName"
                                                            ControlToValidate="txtEmpName" Display="Dynamic" ValidationGroup="s" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:AutoCompleteExtender ID="txtEmpName_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeName" CompletionSetCount="20"
                                                            OnClientItemSelected="EmpSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtEmpName"
                                                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label Text="S/O" runat="server" ID="Label18" CssClass="labelbold" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="address" runat="server" Text="Address1" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="Address2" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:TextBox ID="txtaddress2" Width="100%" TextMode="MultiLine" runat="server" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" Text="Address3" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtaddress3" Width="100%" runat="server" TextMode="MultiLine" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 40%">
                                                                    <asp:Label ID="Label118" runat="server" CssClass="labelbold" Text="Branch Name"></asp:Label>
                                                                </td>
                                                                <td style="width: 60%">
                                                                    <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="95%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" runat="server" Text="District" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDistrict" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" Text="State" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDstate" runat="server" CssClass="dropdown" Width="100%" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="Label10" runat="server" Text="Mobile No." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="textb" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="Phone No." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td id="tdfax" runat="server">
                                                        <asp:Label ID="Label12" runat="server" Text=" Pin Code." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td id="tdfaxid" runat="server">
                                                        <asp:TextBox ID="txtFax" runat="server" CssClass="textb" Width="200px" MaxLength="6"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Text="Email" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmailadd" runat="server" CssClass="textb" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="Label16" runat="server" Text="Pan No" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtPanNo" style="text-transform:uppercase" runat="server" CssClass="textb" Width="200px" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label13" runat="server" Text="CST No." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCST" style="text-transform:uppercase" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label14" runat="server" Text="Tin No." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTin" style="text-transform:uppercase" runat="server" CssClass="textb" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label19" runat="server" Text="GST No." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td id="tdopen" runat="server">
                                                        <asp:TextBox ID="txtGstNo" style="text-transform:uppercase" runat="server" CssClass="textb" Width="200px" MaxLength="15"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label20" runat="server" Text=" Aadhar No." CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtaadharNo" style="text-transform:uppercase" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label17" runat="server" Text="TDSType" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDType" CssClass="dropdown" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label15" runat="server" Text="  Remarks" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="textb" Width="90%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCapacity" runat="server" Text="Capacity" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcapacity" runat="server" CssClass="textb"></asp:TextBox>
                                                        <asp:TextBox ID="txtOpeningBal" runat="server" CssClass="textb" Visible="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblGuarantor" CssClass="labelbold" Text="Contact Person" runat="server" />
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtGuarantor" Width="90%" runat="server" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label36" runat="server" Text="GST Type" CssClass="labelbold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDGSType" CssClass="dropdown" runat="server" Width="100%">
                                                            <asp:ListItem Value="0" Selected="True">----Select----</asp:ListItem>
                                                            <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                                            <asp:ListItem Value="2">IGST</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" id="Tremptype" runat="server" visible="False">
                                                        <table>
                                                            <tr>
                                                                <td id="Td1" runat="server">
                                                                    <asp:Label Text="Emp/Vendor Type" runat="server" ID="Label34" CssClass="labelbold" />
                                                                </td>
                                                                <td id="Td2" runat="server">
                                                                    <asp:DropDownList ID="DDemptype" runat="server" CssClass="dropdown" Width="150px"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged">
                                                                        <asp:ListItem></asp:ListItem>
                                                                        <asp:ListItem Text="Loop" Value="Loop"></asp:ListItem>
                                                                        <asp:ListItem Text="Cut" Value="Cut"></asp:ListItem>
                                                                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td runat="server">
                                                        <asp:Label Text="Designation " CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td runat="server">
                                                        <asp:TextBox ID="txtdesignation" runat="server" CssClass="textb" Width="200px" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td id="TDBankDetails" runat="server">
                                                        <asp:Label ID="Label37" Text="Bank Details " CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td id="Td4" runat="server">
                                                        <asp:TextBox ID="txtBankDetails" runat="server" CssClass="textb" TextMode="MultiLine"
                                                            Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="TREmpName2" runat="server" visible="False">
                                                    <td runat="server">
                                                        <asp:Label ID="Label41" Text="Emp/Vendor Name(2) " CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td runat="server">
                                                        <asp:TextBox ID="txtEmpName2" runat="server" CssClass="textb" Width="200px" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td runat="server">
                                                        <asp:Label ID="Label39" Text="Emp Address(2) " CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td runat="server">
                                                        <asp:TextBox ID="txtEmpAddress2" runat="server" CssClass="textb" Width="200px" MaxLength="50"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="TREmpName2SecondRow" runat="server" visible="False">
                                                    <td runat="server">
                                                        <asp:Label ID="Label38" Text="Emp(2) Pan No" CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td runat="server">
                                                        <asp:TextBox ID="TxtEmp2PanNo" runat="server" CssClass="textb" Width="200px" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td runat="server">
                                                        <asp:Label ID="Label40" Text="Emp(2) Aadhar No" CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td runat="server">
                                                        <asp:TextBox ID="TxtEmp2AadharNo" runat="server" CssClass="textb" Width="200px" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td runat="server">
                                                        <asp:Label ID="Label42" Text="Emp(2) Mobile No" CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td runat="server">
                                                        <asp:TextBox ID="TxtEmp2MobileNo" runat="server" CssClass="textb" Width="200px" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" style="padding-left: 2%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; max-height: 300px; max-width: 200PX">
                                                            <asp:Label Text="Employee Process" runat="server" ID="Label2" Font-Bold="True" />
                                                            <br />
                                                            <asp:CheckBoxList ID="ChLProcess" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkforitem" runat="server" Text="Vendor Item Association" CssClass="checkboxnormal"
                                                            AutoPostBack="True" onclick="return Checkforitem();" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkvendorrate" runat="server" Text="Vendor Item Rate" CssClass="checkboxnormal"
                                                            AutoPostBack="True" onclick="return CheckforitemRate();" Visible="False" />
                                                        <asp:CheckBox ID="chkblacklist" runat="server" Text="Vendor Black List" CssClass="checkboxnormal" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnvendoritem" runat="server" Text="Vendor Item Detail" CssClass="buttonnorm"
                                                            OnClientClick="VendorItemDetail();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Employee Photo" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:Image ID="lblphotoimage" runat="server" Height="100px" Width="170px" />
                                                    </td>
                                                    <td>
                                                        <asp:FileUpload ID="fileuploadphoto" ViewStateMode="Enabled" runat="server" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                                            ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="fileuploadphoto"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                <asp:GridView ID="dgRaw1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    DataKeyNames="SrNo" EmptyDataText="No Data Found!" OnPageIndexChanging="dgRaw1_PageIndexChanging"
                                    OnRowDataBound="dgRaw1_RowDataBound" OnSelectedIndexChanged="dgRaw1_SelectedIndexChanged"
                                    Width="100%">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="Label35" runat="server" Text='<%#Bind("Department") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emp/Vendor">
                                            <ItemTemplate>
                                                <asp:Label ID="lblempname" runat="server" Text='<%#Bind("EMpname") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Father Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label33" runat="server" Text='<%#Bind("Fathername") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgAddress" runat="server" Text='<%#Bind("Address") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgAddress2" runat="server" Text='<%#Bind("Address2") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgPhoneNo" runat="server" Text='<%#Bind("PhoneNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgMobile" runat="server" Text='<%#Bind("Mobile") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="District">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgdistrict" runat="server" Text='<%#Bind("District") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StateName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgstatename" runat="server" Text='<%#Bind("StateName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgempcode" runat="server" Text='<%#Bind("EMpcode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pan No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgpanno" runat="server" Text='<%#Bind("PanNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aadhar No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgaadharno" runat="server" Text='<%#Bind("Aadharno") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GST No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblggstno" runat="server" Text='<%#Bind("GSTNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstatus" runat="server" Text='<%#Bind("Status") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Upload Docs" ShowHeader="False" Visible="False">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlDetails2" runat="server" NavigateUrl='<%# "../Masters/Campany/frmuploadocs.aspx?srno=" + Eval("SrNo") +""%>'
                                                    Target="_blank" Text="Upload Docs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdel" runat="server" OnClick="lnkdel" OnClientClick="return confirm('Do you want to delete this row?')"
                                                    Text="Delete"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabpanelHrDetail" runat="server" HeaderText="HR Details" BackColor="#E3E3E3">
                    <ContentTemplate>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr id="TRHrDetail" runat="server" visible="False">
                                    <td runat="server">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="Label22" Text="HR DETAIL" CssClass="labelbold" runat="server" ForeColor="Red" />
                                            </legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldob" Text="D.O.B" CssClass="labelbold" runat="server" />
                                                        <br />
                                                        <asp:TextBox ID="txtdob" Width="120px" CssClass="textb" runat="server" />
                                                        <asp:CalendarExtender ID="caldob" runat="server" TargetControlID="txtdob" Format="dd-MMM-yyyy"
                                                            Enabled="True">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblnominee" Text="Nominee" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="txtnominee" Width="250px" CssClass="textb" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label23" Text="Date of Joining" CssClass="labelbold" runat="server" />
                                                        <br />
                                                        <asp:TextBox ID="txtdoj" Width="120px" CssClass="textb" runat="server" />
                                                        <asp:CalendarExtender ID="caldoj" runat="server" TargetControlID="txtdoj" Format="dd-MMM-yyyy"
                                                            Enabled="True">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td id="TDPendionallow" runat="server" visible="False">
                                                        <asp:Label ID="lblpensionallow" Text="Pension Allow" CssClass="labelbold" runat="server" />
                                                        <br />
                                                        <asp:DropDownList ID="DDpensionallow" CssClass="dropdown" Width="80px" runat="server">
                                                            <asp:ListItem Text="No" />
                                                            <asp:ListItem Text="Yes" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label24" Text="Over Time Allow" CssClass="labelbold" runat="server" />
                                                        <br />
                                                        <asp:DropDownList ID="DDOvertimeallow" CssClass="dropdown" Width="80px" runat="server">
                                                            <asp:ListItem Text="No" />
                                                            <asp:ListItem Text="Yes" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label25" Text="Staff Type" CssClass="labelbold" runat="server" />
                                                        <br />
                                                        <asp:DropDownList ID="DDstafftype" CssClass="dropdown" Width="80px" runat="server">
                                                            <asp:ListItem Text="P" />
                                                            <asp:ListItem Text="T" />
                                                            <asp:ListItem Text="D" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" valign="top">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label26" Text="Emp A/C No." CssClass="labelbold" runat="server" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtempacno" CssClass="textb" Width="120px" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label27" Text="IP No." CssClass="labelbold" runat="server" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtIPno" CssClass="textb" Width="250px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="Label32" Text="Employee Group" CssClass="labelbold" runat="server" />
                                                                    <br />
                                                                    <asp:DropDownList ID="DDempgroup" CssClass="dropdown" runat="server" Width="145px">
                                                                    </asp:DropDownList>
                                                                    <asp:Button ID="btnaddgroup" runat="server" CssClass="buttonnorm" OnClientClick="Addgroupmaster();"
                                                                        Text="+" ToolTip="Add New Group" />
                                                                    <asp:Button ID="refreshgrouponemployee" runat="server" Style="display: none" OnClick="refreshgrouponemployee_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td colspan="4">
                                                        <fieldset>
                                                            <legend>
                                                                <asp:Label ID="lblbankdetail" Text="Bank Detail" CssClass="labelbold" runat="server" />
                                                            </legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label28" Text="Bank Name" CssClass="labelbold" runat="server" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtbankname" CssClass="textb" Width="250px" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label29" Text="Branch Name" CssClass="labelbold" runat="server" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtbranchname" CssClass="textb" Width="250px" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label30" Text="IFSC Code" CssClass="labelbold" runat="server" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtifsccode" CssClass="textb" Width="250px" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label31" Text="A/C No." CssClass="labelbold" runat="server" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtacno" CssClass="textb" Width="250px" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblsubdoc" BackColor="LightYellow" Font-Bold="True" Text="Submitted Documents"
                                                                        runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="overflow: scroll; max-height: 200px; width: 250PX">
                                                                        <asp:CheckBoxList ID="chkdocuments" CssClass="checkboxbold" runat="server">
                                                                        </asp:CheckBoxList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6" align="right">
                                                        <asp:Label ID="lblstaff" Text="Staff : P=Permanent, T=Temporary, D=Daily Wages" BackColor="LightYellow"
                                                            Font-Bold="True" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="false"
                            ShowSummary="true" Font-Bold="true" Font-Italic="true" Font-Names="Times new Roman"
                            Font-Overline="false" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblErr" runat="server" CssClass="errormsg" ForeColor="Red"></asp:Label>
                        <asp:Button CssClass="buttonnorm" ID="btnSave" runat="server" OnClientClick="return validate();"
                            OnClick="btnSave_Click" Width="70px" Text="Save" ValidationGroup="s" />
                        <asp:Button CssClass="buttonnorm" ID="btnCancel" runat="server" OnClick="btnCancel_Click"
                            Text="Clear" />
                        <asp:Button CssClass="buttonnorm" ID="Button2" runat="server" OnClick="Button2_Click"
                            Text="Preview" />
                        <asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                        <%-- <asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();" OnClick="btnclose_Click" />--%>
                        <asp:Button CssClass="buttonnorm" ID="addloom" runat="server" Text="ADDLOOM" OnClientClick="Addloom();"
                            ToolTip="click for add new loom master" />
                         <asp:Button ID="btnidentitycardpdf" Text="I-Card PDF" CssClass="buttonnorm" runat="server"
                            OnClick="btnidentitycardpdf_Click" />
                        <asp:Button ID="btnallidentitycard" Text="I-Card JPG" CssClass="buttonnorm" runat="server"
                            OnClick="btnallidentitycard_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:TextBox ID="txtempcode_identitycard" Width="300px" TextMode="MultiLine" runat="server"
                            placeholder="Type Card No. Comma Separated to Print Identity card for more than one card No." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 990px; height: 200px; overflow: auto">
                        </div>
                    </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="Button2" />
        </Triggers>
    </asp:UpdatePanel>
</div>
