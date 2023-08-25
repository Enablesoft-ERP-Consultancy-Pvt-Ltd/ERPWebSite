<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" ViewStateMode="Enabled"
    CodeFile="NextIssueForother.aspx.cs" Inherits="Masters_Process_NextIssue" Title="Job Issue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function EmpSelected(source, eventArgs) {
            document.getElementById('txtgetvalue').value = eventArgs.get_value();
        }
        function NewForm() {
            window.location.href = "NextIssueForOther.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13 || event.keyCode == 9) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
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
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
        function onDatechange() {
            var txtissuedate = document.getElementById("TxtIssueDate").value;
            var txtreqdate = document.getElementById("TxtReqDate").value;
            var issuedate = new Date(txtissuedate);
            var reqdate = new Date(txtreqdate);
            if (issuedate > reqdate) {
                document.getElementById("TxtReqDate").value = txtissuedate;
            }
        }
     
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--style="background-image:url(Images/header.jpg)" --%>
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" Width="900px" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="LblCompanyName" runat="server" Text="" CssClass="labelnormal" Style="font-style: italic"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: center">
                            <i><font size="2" face="GEORGIA">
                                <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                            </i>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td class="style1">
                <uc1:ucmenu ID="ucmenu1" runat="server" />
                <asp:Label ID="LblFrmName" ForeColor="White" Font-Bold="true" Font-Size="Large" runat="server"
                    CssClass="labelbold" Text="Job Issue"></asp:Label>
                <asp:ScriptManager ID="ScriptManager2" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                            Text="Logout" OnClick="BtnLogout_Click" UseSubmitBehavior="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 75%; float: left">
                    <div style="margin-bottom: 2px; border: 1px Solid; background-color: #DEB887;">
                        <table>
                            <tr>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblcomp" Text="Company Name" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label15" Text="Branch Name" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblunit" Text="Unit Name" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:DropDownList ID="ddUnits" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="ddUnits_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" Text="Job Name" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDTOProcess" runat="server" Width="150px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDTOProcess_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDIssueno" runat="server">
                                                <asp:Label ID="lblissueno" Text="Issue No." runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtChallanNO" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDcheckjobseq" runat="server" visible="false">
                                                <asp:CheckBox ID="chkjobseqno" Text="Not Maintain Job Seq." runat="server" CssClass="labelbold" />
                                            </td>
                                            <td id="TDReworkof" runat="server" visible="false">
                                                <span class="labelbold">Re-work of</span>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDreworkof" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDDatestamp" runat="server" visible="false">
                                                <asp:Label ID="lbldatestamp" Text="Date_Stamp" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox ID="txtdatestamp" CssClass="textb" Width="100px" runat="server" />
                                            </td>
                                            <td id="TDRecchallanNo" runat="server" visible="false">
                                                <asp:Label ID="Label3" Text="Rec Challan No." runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox ID="txtrecchallanNo" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr id="TrBindRecChallanNoJobWise" runat="server" visible="false">
                                            <td id="Td9" class="tdstyle" runat="server">
                                                &nbsp;
                                            </td>
                                            <td id="Td10" class="tdstyle" runat="server">
                                                <asp:Label ID="Label18" runat="server" Text="From Process" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDFromProcessForRecChallanNo" runat="server" CssClass="dropdown" Width="150px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDFromProcessForRecChallanNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TD11" runat="server" visible="true">
                                                <asp:Label ID="Label20" runat="server" Text="EmpName" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDProcessWiseEmpName" runat="server" CssClass="dropdown" Width="150px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDProcessWiseEmpName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TD12" runat="server" visible="true">
                                                <asp:Label ID="Label19" runat="server" Text="Rec ChallanNo." CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDRecChallanNoJobWise" runat="server" CssClass="dropdown" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        
                                        </tr>

                                        <tr>
                                            <td id="Tdissuedate" class="tdstyle" runat="server">
                                                <span class="labelbold">IssueDate</span>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtIssueDate" runat="server" Width="100px" onchange="javascript: onDatechange();"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtIssueDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="Tdreqdate" class="tdstyle" runat="server">
                                                <span class="labelbold">ReqDate</span>
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="100px" onchange="javascript: onDatechange();"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtReqDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="TDCustomerCode" runat="server" visible="false">
                                                <asp:Label ID="Label12" runat="server" Text="Buyer" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="150px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDCustomerOrderNo" runat="server" visible="false">
                                                <asp:Label ID="Label13" runat="server" Text="Order No." CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDorderNo" OnSelectedIndexChanged="DDorderNo_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="dropdown" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDCategory" runat="server">
                                                <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDCategory" runat="server" Width="150px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="tdqualityname1" runat="server">
                                                <asp:Label ID="Label2" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" Width="150px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="tddesign1" runat="server">
                                                <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="true"
                                                    Width="150px" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdColor1" runat="server">
                                                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="tdsize1" runat="server">
                                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDSize" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DDSize_SelectedIndexChanged1">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDEWayBillNo" runat="server" visible="false">
                                                <asp:Label ID="Label16" runat="server" Text="EWayBill No" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtEWayBillNo" CssClass="textb" Width="100px" runat="server" />
                                            </td>
                                            <td id="TDGSTType" runat="server" visible="false">
                                                <asp:Label ID="Label17" runat="server" Text="GST TYPE" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDGSTType" runat="server">
                                                    <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                                    <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                                    <asp:ListItem Value="2">IGST</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDPacktype" runat="server" visible="false">
                                                <asp:Label ID="Label4" runat="server" Text="Pack Type" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDPacktype" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="DDPacktype_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDarticleNo" runat="server" visible="false">
                                                <asp:Label ID="Label5" runat="server" Text="Article No." CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDarticleno" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DDarticleno_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDBatchNo" runat="server" visible="false">
                                                <asp:Label ID="Label6" runat="server" Text="Batch No." CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDbatchNo" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom" id="TDgetstockdetail" runat="server">
                                                <asp:Button ID="btngetstock" runat="server" CssClass="buttonnorm" Text="Get Stock"
                                                    OnClick="btngetstock_Click" />
                                            </td>
                                            <td id="TD3" runat="server">
                                                <asp:Label ID="LblDataFromDate" Text="Data From Date" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtDataFromDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtDataFromDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="TDgetDataupto" runat="server">
                                                <asp:Label ID="lblgetdataToDate" Text="Data To Date" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="txtgetdataupto" runat="server" Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="txtgetdataupto">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="TDWeight" runat="server" visible="false">
                                                <asp:Label ID="Label14" runat="server" Text="Weight" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtWeight" CssClass="textb" Width="100px" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIssueQty" runat="server" Text="Iss Qty" CssClass="labelbold"></asp:Label><br />
                                                <asp:TextBox ID="TxtIssueQty" CssClass="textb" Width="100px" runat="server" Enabled="False"
                                                    AutoPostBack="true" OnTextChanged="TxtIssueQty_TextChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="justify" id="Tdemployee" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkscan" Text="For Scan Employee" CssClass="checkboxbold" runat="server"
                                                    AutoPostBack="true" OnCheckedChanged="chkscan_CheckedChanged" Font-Size="Small" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <span class="labelbold">Enter Employee Name/Code </span>
                                                <br />
                                                <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="250px" Height="20px" CssClass="textb"
                                                    AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="250px"
                                                    Height="20px" AutoPostBack="true" Visible="false" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                                                <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                                    BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeForJobNew"
                                                    EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                                    ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                                    ContextKey="0" MinimumPrefixLength="2">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto; width: 250px">
                                                                <asp:ListBox ID="lstWeaverName" runat="server" Width="250px" Height="100px" SelectionMode="Multiple">
                                                                </asp:ListBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:LinkButton ID="btnDeleteName" Text="Remove Employee" CssClass="linkbuttonnew"
                                                                runat="server" OnClick="btnDeleteName_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-bottom: 2px; border: 1px Solid; background-color: #DEB887;">
                        <table>
                            <tr>
                                <td class="tdstyle" id="TDFromProcess" runat="server" visible="false">
                                    FromProcess
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDFromProcess" runat="server" Width="150px"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDFromProcess_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDFromProcess"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td class="tdstyle" id="TdDDContractor" runat="server" visible="false" style="display: none">
                                    Emp/Contractor
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDContractor" Width="150px" runat="server"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDContractor_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDContractor"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td class="tdstyle">
                                    <%-- <span class="labelbold">Enter Stock No</span>--%>
                                    <asp:Label ID="lblStockCarpetNo" runat="server" Text="Enter Stock No" class="labelbold"></asp:Label>
                                    <asp:TextBox ID="TxtStockNo" runat="server" Width="200px" CssClass="textb" TabIndex="8"
                                        onKeyDown="KeyDownHandler('btnStockNo');" AutoPostBack="True" Height="30px"></asp:TextBox>
                                    <td>
                                        <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="TxtStockNo_TextChanged" />
                                    </td>
                                </td>
                                <td id="TDAreaNew" runat="server" visible="false" class="tdstyle">
                                    <span class="labelbold">Area</span>
                                    <asp:CheckBox ID="ChkForArea" runat="server" Text="Chk For Area" CssClass="checkboxbold" />
                                    <br />
                                    <asp:TextBox CssClass="textb" ID="TxtAreaNew" onkeypress="return isNumberKey(event);"
                                        runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td id="TDRateNew" runat="server" class="tdstyle" visible="false">
                                    <span class="labelbold">Rate</span>
                                    <asp:CheckBox ID="ChkForRate" runat="server" Text="Chk For Rate" CssClass="checkboxbold" />
                                    <br />
                                    <asp:TextBox CssClass="textb" ID="TxtRateNew" onkeypress="return isNumberKey(event);"
                                        runat="server" Width="70px"></asp:TextBox>
                                </td>
                                <td id="TDExportSize" runat="server" class="tdstyle" visible="false">                                    
                                    <asp:CheckBox ID="ChkForExportSize" runat="server" Text="Chk For Export Size" CssClass="checkboxbold" />
                                    <br />                                    
                                </td>

                                <td align="center">
                                    <asp:Label ID="LblErrorMessage" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                                <td id="Td1" class="tdstyle" runat="server" visible="false">
                                    CalType
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDcaltype_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                        <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                        <%--  <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                        <asp:ListItem Value="3">W-2</asp:ListItem>
                                        <asp:ListItem Value="4">L-2</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td id="Td2" runat="server" visible="false">
                                    Unit
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDUnit" runat="server" Width="100px" AutoPostBack="True"
                                        OnSelectedIndexChanged="DDUnit_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="LSEDDUnit" runat="server" TargetControlID="DDUnit" ViewStateMode="Disabled"
                                        PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr id="TRItemdetail" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="label11" Text="Item :" runat="server" CssClass="labelbold" />
                                    &nbsp
                                    <asp:Label ID="txtitem" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labelQ" Text="Quality :" runat="server" CssClass="labelbold" />
                                    &nbsp
                                    <asp:Label ID="txtQuality" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label7" Text="Design :" runat="server" CssClass="labelbold" />
                                    &nbsp
                                    <asp:Label ID="txtDesign" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label8" Text="Color :" runat="server" CssClass="labelbold" />
                                    &nbsp
                                    <asp:Label ID="txtcolor" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label9" Text="Shape :" runat="server" CssClass="labelbold" />
                                    &nbsp
                                    <asp:Label ID="txtshape" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label10" Text="Size :" runat="server" CssClass="labelbold" />
                                    &nbsp
                                    <asp:Label ID="txtsize" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td id="Td4" runat="server" visible="false">
                                    <asp:CheckBox ID="ChkForIssue_Receive" runat="server" Text="Chk For Issue/Receive in one Time"
                                        Font-Bold="true" OnCheckedChanged="ChkForIssue_Receive_CheckedChanged" AutoPostBack="true"
                                        Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table style="width: 90%">
                            <tr>
                                <td>
                                    <div style="max-height: 400px; overflow: auto;">
                                        <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Issue_Detail_Id"
                                            OnRowDeleting="DGDetail_RowDeleting" Width="100%" OnRowDataBound="DGDetail_RowDataBound">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblitemname" Text='<%#Bind("Item_Name") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quality">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuality" Text='<%#Bind("QualityName") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Design">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesign" Text='<%#Bind("Designname") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Color">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblColor" Text='<%#Bind("Colorname") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Shape">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblShapename" Text='<%#Bind("shapename") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Size">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSize" Text='<%#Bind("Size") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Area">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblArea" Text='<%#Bind("Area") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" Text='<%#Bind("Amount") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Bonus" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBonus" Text='<%#Bind("Bonus") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                            Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
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
                                <td id="Td8">
                                    <span class="labelbold">Total Pcs</span>
                                    <asp:TextBox CssClass="textb" ID="TxtTotalPcs" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td id="Td5">
                                    <span class="labelbold">Area</span>
                                    <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td id="Td6">
                                    <span class="labelbold">Amount</span>
                                    <asp:TextBox CssClass="textb" ID="TxtAmount" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table style="width: 90%">
                            <tr>
                                <td id="Td7" runat="server" align="right">
                                    <asp:Button ID="BtnShowData" runat="server" CssClass="buttonnorm " Text="Click For Show Data"
                                        OnClick="BtnShowData_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdstyle" align="right">
                                    <asp:CheckBox ID="ChkForSummary" CssClass="checkboxbold" runat="server" Text="For Print Summary"
                                        Visible="true" />
                                         &nbsp;<asp:CheckBox ID="ChkForWithoutRate" runat="server" Text="For Without Rate" CssClass="checkboxbold" Visible="false" />
                                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview"
                                        OnClick="BtnPreview_Click" />
                                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                                        OnClick="BtnClose_Click" />
                                    &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                                </td>
                            </tr>
                            <tr id="Tr2" runat="server" >
                                <td class="tdstyle">
                                    Remarks&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="Tr3" runat="server" visible="false">
                                <td class="tdstyle">
                                    Instructions
                                    <asp:TextBox CssClass="textb" ID="TxtInsructions" Width="90%" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr id="Tr1" runat="server" visible="false">
                                <td id="ProCod1" runat="server" visible="false">
                                    Prod Code
                                    <br />
                                    <asp:TextBox CssClass="textb" ID="TxtItemCode" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtItemCode"
                                        UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td id="TDitemName" runat="server" visible="false">
                                    <asp:Label ID="lblitemname" runat="server" Text="Articles"></asp:Label>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDItemName" runat="server" Width="150px"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchDDItemName" runat="server" TargetControlID="DDItemName"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="tdShape1" runat="server" visible="false">
                                    <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" Width="100px" AutoPostBack="True"
                                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchDDShape" runat="server" TargetControlID="DDShape"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="tdshadecolor" runat="server" visible="false">
                                    <asp:Label ID="lblshadecolor" runat="server" Text="Shade Color"></asp:Label>
                                    &nbsp;<br />
                                    <asp:DropDownList ID="ddlshade" runat="server" Width="100px" CssClass="dropdown"
                                        OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchddlshade" runat="server" TargetControlID="ddlshade"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="width: 25%; float: right" id="DIVStockDetail" runat="server">
                    <table>
                        <tr>
                            <td align="justify">
                                <div style="max-height: 600px; overflow: auto;">
                                    <asp:GridView ID="DGStockDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found."
                                        AllowPaging="true" PageSize="200" OnPageIndexChanging="DGStockDetail_PageIndexChanging">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdesc" Text='<%#Bind("itemdescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CurrentJob">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbljob" Text='<%#Bind("Currentjob") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstatus" Text='<%#Bind("currentstatus") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDButtonsavegrid" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltpcs" Text="Total Pcs" CssClass="labelbold" runat="server" />
                                            <br />
                                            <asp:TextBox ID="txttotalpcsgrid" runat="server" CssClass="textb" Width="90px" />
                                        </td>
                                        <td valign="bottom">
                                            <asp:Button ID="btnsavegrid" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsavegrid_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hnfromprocessid" runat="server" Value="0" />
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
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
