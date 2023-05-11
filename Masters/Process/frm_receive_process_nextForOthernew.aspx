<%@ Page Title="JOB RECEIVE" Language="C#" AutoEventWireup="true" CodeFile="frm_receive_process_nextForOthernew.aspx.cs"
    Inherits="Masters_Process_frm_receive_process_next" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function HandleTextKeypress(e) {
            var key = e.keycode || e.which;
            if (key == 13) {
                var txtControl = document.getElementById('<%= TxtStockNo.ClientID %>');
                e.preventDefault();
                txtControl.focus();
            }
        }
        function EmpSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
        function NewForm() {
            window.location.href = "frm_receive_process_nextforother.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
            }
        }

        function YourFunctionName(msg) {
            var txt = msg;
            alert(txt);
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

    </script>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="width: 100%">
                                <div style="width: 80%; float: left;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 70%" valign="top">
                                                <table style="width: 100%" border="1" cellpadding="5" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblunitname" Text="Unit Name" runat="server" CssClass="labelbold" />
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <asp:DropDownList ID="ddUnits" runat="server" Width="100%" CssClass="dropdown" OnSelectedIndexChanged="ddUnits_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 50%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label3" Text="Job Name" runat="server" CssClass="labelbold" /><br />
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <asp:DropDownList ID="ddprocess" runat="server" CssClass="dropdown" Width="100%"
                                                                            OnSelectedIndexChanged="ddprocess_SelectedIndexChanged1" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Tdrecdate" class="tdstyle" runat="server">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <span class="labelbold">Rec.Date</span><br />
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <asp:TextBox CssClass="textb" ID="TxtreceiveDate" runat="server" Width="90%" AutoPostBack="true"
                                                                            OnTextChanged="TxtreceiveDate_TextChanged"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                                            TargetControlID="TxtreceiveDate">
                                                                        </asp:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td id="Td6" class="tdstyle" runat="server">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblchallanNo" Text="Challan No." CssClass="labelbold" runat="server" /><br />
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <asp:TextBox CssClass="textb" ReadOnly="true" ID="TxtChallanNo" runat="server" Width="90%"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td runat="server" id="TDQDCS">
                                                                        <table style="width: 100%" border="1" cellpadding="5" cellspacing="0">
                                                                            <tr>
                                                                                <td id="TDcategory" runat="server" style="width: 50%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                         <td>
                                                                                                <asp:Label ID="lbldesignname" CssClass="labelbold" runat="server" Text="Design"></asp:Label><br />
                                                                                            </td>
                                                                                            <td style="width: 70%">
                                                                                                <asp:DropDownList ID="ddldesig" runat="server" CssClass="dropdown" Width="100%" AutoPostBack="True"
                                                                                                    OnSelectedIndexChanged="ddldesig_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                           
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td runat="server" id="tdquality" style="width: 50%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblqualityname" CssClass="labelbold" runat="server" Text="Quality"></asp:Label><br />
                                                                                            </td>
                                                                                            <td style="width: 70%">
                                                                                                <asp:DropDownList ID="ddquality" runat="server" CssClass="dropdown" Width="100%"
                                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td runat="server" id="tddesign" style="width: 50%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                           <td>
                                                                                                <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Item Category"
                                                                                                    CssClass="labelbold"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 70%">
                                                                                                <asp:DropDownList ID="ddcattype" runat="server" Width="100%" CssClass="dropdown"
                                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddcattype_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td> 
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td id="tdcolor" runat="server" style="width: 50%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblcolorname" CssClass="labelbold" runat="server" Text="Color"></asp:Label><br />
                                                                                            </td>
                                                                                            <td style="width: 70%">
                                                                                                <asp:DropDownList ID="ddcolour" runat="server" CssClass="dropdown" Width="100%" AutoPostBack="True"
                                                                                                    OnSelectedIndexChanged="ddcolour_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table style="width: 100%" border="1" cellpadding="5" cellspacing="0">
                                                                            <tr>
                                                                                <td id="tdsize" runat="server" style="width: 50%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 70%">
                                                                                                <asp:DropDownList ID="ddsize" runat="server" CssClass="dropdown" Width="100%">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td id="tdLengthWidth" runat="server" style="width: 50%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td id="tdActualWidth" runat="server" style="width: 50%">
                                                                                                <asp:Label ID="LblWidth" Text="Actual Width" CssClass="labelbold" runat="server" />&nbsp;
                                                                                                <asp:TextBox CssClass="textb" ID="TxtWidth" runat="server" Width="50%"></asp:TextBox>
                                                                                            </td>
                                                                                            <td id="tdActualLength" runat="server" style="width: 50%">
                                                                                                <asp:Label ID="lblLength" Text="Actual Length" CssClass="labelbold" runat="server" />&nbsp;
                                                                                                <asp:TextBox CssClass="textb" ID="TxtLength" runat="server" Width="50%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="TBDDIssueNo" runat="server" visible="false" style="width: 100%" border="1"
                                                    cellpadding="5" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="BtnGetIssueNo" runat="server" Text="Get Issue No" CssClass="buttonnorm"
                                                                            Width="100px" OnClick="BtnGetIssueNo_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 50%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label2" CssClass="labelbold" runat="server" Text="Issue No"></asp:Label><br />
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <asp:DropDownList ID="DDIssueNo" runat="server" CssClass="dropdown" Width="100%">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="TablePartyChallanNo" runat="server" visible="false" style="width: 100%"
                                                    border="1" cellpadding="5" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label5" CssClass="labelbold" runat="server" Text="Party ChallanNo"></asp:Label><br />
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <asp:TextBox CssClass="textb" ID="txtPartyChallanNo" runat="server" Width="50%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 50%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblReceiveQty" runat="server" Text="Rec Qty" CssClass="labelbold"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TxtReceiveQty" CssClass="textb" Width="100px" runat="server" Enabled="False"
                                                                            AutoPostBack="true" OnTextChanged="TxtReceiveQty_TextChanged" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 30%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td id="TDemployee" runat="server" style="width: 100%">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkscan" Text="For Scan Employee" CssClass="checkboxbold" runat="server"
                                                                            AutoPostBack="true" OnCheckedChanged="chkscan_CheckedChanged" Font-Size="Small" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblname" Text="Enter Employee Name/Code" runat="server" CssClass="labelbold" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="90%" Height="20px" CssClass="textb"
                                                                            AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                                                        <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="90%" Height="20px"
                                                                            AutoPostBack="true" Visible="false" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                                                                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>

                                                                        <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                                                            BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeForJobNew"
                                                                            EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                                                            ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                                                            ContextKey="0#0#0" MinimumPrefixLength="2">
                                                                        </asp:AutoCompleteExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <div style="overflow: auto; width: 100%">
                                                                            <asp:ListBox ID="lstWeaverName" runat="server" Width="90%" Height="142px" SelectionMode="Multiple">
                                                                            </asp:ListBox>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnShowDetail" runat="server" Text="Get Stock No." CssClass="buttonnorm"
                                                                            Width="100px" OnClick="btnShowDetail_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:LinkButton ID="btnDeleteName" Text="Remove Employee" CssClass="labelbold" runat="server"
                                                                            OnClick="btnDeleteName_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <fieldset>
                                        <legend>
                                            <asp:Label Text="..." ForeColor="Red" CssClass="labelbold" runat="server" /></legend>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%">
                                                    <asp:Label ID="Label14" Text="Hold/Rejected" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:DropDownList ID="ddStockQualityType" runat="server" CssClass="dropdown">
                                                        <asp:ListItem Value="1">Finished</asp:ListItem>
                                                        <asp:ListItem Value="2">Hold</asp:ListItem>
                                                        <asp:ListItem Value="3">Rejected</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 15%">
                                                    <asp:Label ID="Label1" Text="Stock No. Remarks" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td style="width: 40%">
                                                    <asp:TextBox ID="txtstocknoremarks" CssClass="textb" Width="90%" TextMode="MultiLine"
                                                        runat="server" />
                                                </td>
                                                <td id="TRWeight" runat="server" visible="false">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblweight" Text="Weight" CssClass="labelbold" runat="server"></asp:Label>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtweight" CssClass="textb" Width="100px" runat="server" onkeypress="HandleTextKeypress(event);" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 13%">
                                                    <asp:Label ID="lblStockCarpetNo" runat="server" Text="Enter Stock No" class="labelbold"></asp:Label>
                                                </td>
                                                <td style="width: 25%">
                                                    <%--<span class="labelbold">Enter Stock No</span>--%>
                                                    <asp:TextBox ID="TxtStockNo" runat="server" Width="90%" CssClass="textb" TabIndex="8"
                                                        onKeypress="KeyDownHandler(event);" Height="30px"></asp:TextBox>
                                                    <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="TxtStockNo_TextChanged" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblErrorMessage" runat="server" Text="" CssClass="labelbold" ForeColor="Red"
                                                        Font-Bold="true" Font-Size="Small"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <div style="margin-top: 10px" runat="server">
                                        <asp:Panel ID="panelMaster" runat="server">
                                            <%--BackColor="#DEB887"--%>
                                            <table>
                                                <tr id="Tr1" runat="server">
                                                    <td id="Td1" class="tdstyle" runat="server" visible="false">
                                                        Company Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b>
                                                        <br />
                                                        <asp:DropDownList ID="ddCompName" runat="server" TabIndex="1" CssClass="dropdown"
                                                            Width="100px" OnSelectedIndexChanged="ddCompName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td2" class="tdstyle" runat="server" visible="false">
                                                        Emp/Contractor&nbsp;&nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                                                        <asp:CheckBox ID="ChkForEdit" runat="server" CssClass="checkboxnormal" Text="For Edit"
                                                            OnCheckedChanged="ChkForEdit_CheckedChanged" AutoPostBack="True" />
                                                        <br />
                                                        <asp:DropDownList ID="ddemp" runat="server" CssClass="dropdown" Width="100px" OnSelectedIndexChanged="ddemp_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td id="TDDDChallanNo" runat="server" visible="false" class="tdstyle">
                                                        Challan No&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList ID="DDChallanNo" runat="server" Width="100px" CssClass="dropdown"
                                                            AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td3" class="tdstyle" runat="server" visible="false">
                                                        P OrderNo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" Width="100px"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td4" class="tdstyle" runat="server" visible="false">
                                                        Cal Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                                            Enabled="false">
                                                            <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                            <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                            <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                            <asp:ListItem Value="3">W-2</asp:ListItem>
                                                            <asp:ListItem Value="4">L-2</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td5" runat="server" visible="false">
                                                        Unit
                                                        <br />
                                                        <asp:DropDownList CssClass="dropdown" ID="DDUnit" runat="server" Width="100px" AutoPostBack="True"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="80%">
                                                <tr id="Tr2" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name"></asp:Label><br />
                                                        <asp:DropDownList ID="dditem" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="dditem_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td runat="server" id="tdshape" visible="false">
                                                        <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape"></asp:Label><br />
                                                        <asp:DropDownList ID="ddshape" runat="server" CssClass="dropdown" Width="100px" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="tdshadecolor" runat="server" visible="false">
                                                        <asp:Label ID="lblshadecolor" runat="server" Text="Shade Color"></asp:Label>
                                                        &nbsp;<br />
                                                        <asp:DropDownList ID="ddlshade" runat="server" Width="100px" CssClass="dropdown"
                                                            OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                        <td colspan="6">
                                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>--%>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <div style="max-height: 300px; background-color: Gray; overflow: scroll; width: 100%">
                                                    <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="process_rec_Detail_Id"
                                                        OnRowDeleting="DGDetail_RowDeleting" OnRowDataBound="DGDetail_RowDataBound" CssClass="grid-view" Width="100%">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Item Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblitem" Text='<%#Bind("Item_Name") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quality">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuality" Text='<%#Bind("Qualityname") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Design">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldesign" Text='<%#Bind("Designname") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Color">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcolor" Text='<%#Bind("Colorname") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Shape">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblshapename" Text='<%#Bind("shapename") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Size">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsize" Text='<%#Bind("Size") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="StockNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstockno" Text='<%#Bind("StockNo") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Area">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
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
                                                                        Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="BtnPenality" runat="server" Text="Penality" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        CommandName="Penality"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProcessId" runat="server" Text='<%#Bind("ProcessId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssueDetailId" runat="server" Text='<%#Bind("Issue_Detail_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="ChkForSummary" Text="For Print Summary" runat="server" CssClass="checkboxbold" />
                                                <asp:CheckBox ID="ChkForActualSize" Text="For Actual Size" runat="server" CssClass="checkboxbold" />
                                                <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" />
                                                <asp:Button ID="btnqcreport" Text="QcReport" runat="server" CssClass="buttonnorm"
                                                    OnClick="btnqcreport_Click" Visible="false" />
                                                <asp:Button CssClass="buttonnorm" ID="btnQcPreview" runat="server" Text="QC CHECK"
                                                    OnClick="btnQcPreview_Click" Visible="false" />
                                                <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick=" return CloseForm(); " />
                                                <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr id="Tr3" runat="server">
                                            <td class="tdstyle">
                                                <span class="labelbold">Total Pcs</span>
                                                <asp:TextBox CssClass="textb" ID="TxtTotalPcs" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td id="Td7" class="tdstyle" runat="server">
                                                <span class="labelbold">Total Area</span>
                                                <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td id="Td8" class="tdstyle" runat="server">
                                                <span class="labelbold">Total Amount</span>
                                                <asp:TextBox CssClass="textb" ID="TxtAmount" runat="server" Width="100px"></asp:TextBox>
                                            </td>
                                            <td id="Td9" runat="server" visible="false">
                                                <asp:Button ID="btnShowdata" runat="server" Text="Click For Data Show" CssClass="buttonnorm"
                                                    OnClick="btnShowdata_Click" />
                                            </td>
                                        </tr>
                                        <tr id="Tr4" runat="server" visible="false">
                                            <td class="tdstyle">
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="100%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:HiddenField ID="hn_finished" runat="server" />
                                                <asp:HiddenField ID="hnstockno" runat="server" />
                                                <asp:HiddenField ID="hnrate1" runat="server" />
                                                <asp:HiddenField ID="hnorderid" runat="server" />
                                                <asp:HiddenField ID="hn_recieve_id" runat="server" />
                                                <asp:HiddenField ID="Hn_Qty" runat="server" />
                                                <asp:HiddenField ID="Hn_ProcessId" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 20%; float: right">
                                    <table>
                                        <tr>
                                            <td align="justify">
                                                <div style="max-width: 250px; max-height: 500px; overflow: auto;">
                                                    <asp:GridView ID="DGStockDetail" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                                        PageSize="200" OnPageIndexChanging="DGStockDetail_PageIndexChanging" OnRowDataBound="DGStockDetail_RowDataBound">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" Height="20px" />
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
                                                            <asp:BoundField DataField="PROCESS_NAME" HeaderText="Process Name">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="StockNo.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDButtonsavegrid" runat="server" visible="false">
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
                            <div>
                                <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                                <asp:ModalPopupExtender ID="Modalpopupextqc" runat="server" PopupControlID="pnModelPopup"
                                    TargetControlID="btnModalPopUp" BackgroundCssClass="modalBackground" CancelControlID="btnqcclose">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: ActiveCaption;
                                    display: none">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="lblqc" Text="QC PARAMETER" runat="server" ForeColor="Red" CssClass="labelbold" />
                                        </legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="max-height: 500px; overflow: auto; width: 800px">
                                                        <asp:GridView ID="GDQC" CssClass="grid-views" runat="server" OnRowDataBound="GDQC_RowDataBound"
                                                            OnRowCreated="GDQC_RowCreated">
                                                            <HeaderStyle CssClass="gvheaders" Font-Size="12px" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnqcsavenew" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnqcsavenew_Click" />
                                                    <asp:Button ID="btnqcclose" Text="Close" runat="server" CssClass="buttonnorm" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblqcmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
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
</asp:Content>
