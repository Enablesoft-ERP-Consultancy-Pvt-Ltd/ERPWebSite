<%@ Page Title="PRODUCTION RECEIVE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmProductionReceiveLoomStockWise.aspx.cs" Inherits="Masters_Loom_frmProductionReceiveLoomStockWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function EmpSelectedEdit(source, eventArgs) {
            document.getElementById('<%=txteditempid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnsearchedit.ClientID%>').click();
        }
        function Loomnoselected(source, eventArgs) {
            document.getElementById('<%=txtloomid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function NewForm() {
            var TxtUserType = document.getElementById('<%=TxtUserType.ClientID %>');
            if (TxtUserType.value == "1") {
                window.location.href = "frmProductionReceiveLoomStockWise.aspx?UserType=1";
            }
            else {
                window.location.href = "frmproductionReceiveLoomStockwise.aspx";
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
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
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
            }
        }
      
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnconfirm.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Production Unit. !!\n";
                    }
                    selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Folio No. !!\n";
                    }
                    if ($("#<%=TDLoomno.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDLoomNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Loom No. !!\n";
                        }
                    }
                    var txtloomno = document.getElementById('<%=txtloomno.ClientID %>');
                    if (txtloomno.value == "" || txtloomno.value == "0") {
                        Message = Message + "Please Enter Loom No. !!\n";
                    }
                    var txtstockno = document.getElementById('<%=txtstockno.ClientID %>');
                    if (txtstockno.value == "") {
                        Message = Message + "Please Enter Stock No. !!\n";
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
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(Jscriptvalidate);
                </script>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table style="width: 100%">
                        <tr>
                            <td id="TDchkedit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td id="TDEditReceiveNoForCI" runat="server" visible="false">
                                <asp:Label ID="Label25" runat="server" Text="Rec ChallanNo" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtEditReceiveNoForCI" CssClass="textb" Width="100px" runat="server"
                                    AutoPostBack="true" OnTextChanged="txtEditReceiveNoForCI_TextChanged" />
                            </td>
                            <td>
                                <asp:TextBox ID="TxtUserType" runat="server" Style="display: none"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 80%" valign="top">
                                <table>
                                    <tr>
                                        <td id="TDEMPEDIT" runat="server">
                                            <asp:Label ID="lblempcodeedit" CssClass="labelbold" Text="Employee Code." runat="server" />
                                            <br />
                                            <asp:TextBox ID="txteditempcode" CssClass="textb" runat="server" Width="100px" />
                                            <asp:TextBox ID="txteditempid" runat="server" Style="display: none"></asp:TextBox>
                                            <asp:Button ID="btnsearchedit" runat="server" Text="Button" OnClick="btnsearchedit_Click"
                                                Style="display: none;" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="SrchAutoComplete1"
                                                CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeAll" EnableCaching="true"
                                                CompletionSetCount="20" OnClientItemSelected="EmpSelectedEdit" ServicePath="~/Autocomplete.asmx"
                                                TargetControlID="txteditempcode" UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td id="TDFolioNotext" runat="server" visible="false">
                                            <asp:Label ID="Label23" CssClass="labelbold" Text="Folio No." runat="server" />
                                            <br />
                                            <asp:TextBox ID="txtfolionoedit" CssClass="textb" Width="80px" runat="server" AutoPostBack="true"
                                                OnTextChanged="txtfolionoedit_TextChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label35" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                Width="150px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td runat="server" visible="false" id="TDLoomno">
                                                        <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                                                        <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                            Width="150px" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="TDLoomNotextbox" runat="server">
                                                        <asp:Label ID="Label8" Text=" Loom No." runat="server" CssClass="labelbold" />
                                                        <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                                            Style="display: none;" />
                                                        <asp:TextBox ID="txtloomid" runat="server" Style="display: none"></asp:TextBox>
                                                        <br />
                                                        <asp:TextBox ID="txtloomno" CssClass="textb" runat="server" Width="150px" />
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtenderloomno" runat="server" BehaviorID="LoomSrchAutoComplete"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetLoomNo" EnableCaching="true"
                                                            CompletionSetCount="30" OnClientItemSelected="Loomnoselected" ServicePath="~/Autocomplete.asmx"
                                                            TargetControlID="txtloomno" UseContextKey="true" ContextKey="0" MinimumPrefixLength="1">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td id="TDreceiveNo" runat="server" visible="false">
                                            <asp:Label ID="Label5" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDreceiveNo" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDreceiveNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDStockStatus" runat="server" visible="false">
                                            <asp:Label ID="Label37" runat="server" Text="Stock Status" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDStockStatus" runat="server" CssClass="dropdown" OnSelectedIndexChanged="DDStockStatus_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Half Finish</asp:ListItem>
                                                <asp:ListItem Value="2">Full Finish</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtreceiveno" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtrecdate" CssClass="textb" Width="95px" runat="server" OnTextChanged="txtrecdate_TextChanged"
                                                AutoPostBack="true" />
                                            <asp:CalendarExtender ID="cal1" TargetControlID="txtrecdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td id="TDcheckedby" runat="server">
                                            <asp:Label ID="lblcheckedby" CssClass="labelbold" Text="Checked By" runat="server" /><br />
                                            <asp:TextBox ID="txtcheckedby" CssClass="textb" Width="250px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReceiveQty" runat="server" Text="Rec Qty" CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="TxtReceiveQty" CssClass="textb" Width="100px" runat="server" Enabled="False"
                                                AutoPostBack="true" OnTextChanged="TxtReceiveQty_TextChanged" />
                                        </td>
                                        <td id="TDPartyChallanNo" runat="server" visible="false">
                                            <asp:Label ID="Label36" runat="server" Text="Party ChallanNo" CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtPartyChallanNo" CssClass="textb" Width="100px" runat="server" />
                                        </td>
                                        <td id="TDbatch" runat="server" visible="false">
                                            <asp:Label ID="Label26" runat="server" Text="Batch ChallanNo" CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtBatchChallanNo" CssClass="textb" Width="100px" runat="server" />
                             </td>
                                    </tr>
                                </table>
                            </td>
                              
                            <td style="width: 20%" valign="top">
                                <table width="100%">
                                    <tr>
                                        <td id="Td1" runat="server" visible="false">
                                            <asp:Label ID="lblemp" Text="Employee" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label19" Text="Employee Detail" CssClass="labelbold" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td2" valign="top" runat="server" visible="false">
                                            <div style="overflow: auto; width: 150px">
                                                <asp:ListBox ID="listWeaverName" runat="server" Width="150px" Height="70px" SelectionMode="Multiple"
                                                    Style="overflow: auto"></asp:ListBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="max-height: 100px; overflow: scroll">
                                                <asp:GridView ID="DGEmployee" AutoGenerateColumns="false" CssClass="grid-views" runat="server"
                                                    EmptyDataText="No Employee Data Fetched." OnRowDataBound="DGEmployee_RowDataBound">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No.">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Employee">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblgridemp" Text='<%#Bind("empname") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Work(%)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtworkpercentage" Width="50px" Text='<%#Bind("Workpercentage") %>'
                                                                    runat="server" onkeypress="return isNumberKey(event);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblgridempid" Text='<%#Bind("empid") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 35%; float: right">
                    </div>
                    <table>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label4" Text="Product Description" runat="server" CssClass="labelbold"
                            ForeColor="Red" />
                    </legend>
                    <table width="100%">
                        <tr>
                            <td width="50%">
                                <table>
                                    <tr>
                                        <td id="TDprodshift" runat="server" visible="false" style="width: 10%">
                                            <asp:Label ID="Label30" Text="PROD. SHIFT" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList ID="DDprodshift" runat="server" CssClass="dropdown">
                                                <asp:ListItem>SHIFT1</asp:ListItem>
                                                <asp:ListItem>SHIFT2</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="lblstockno" Text="Enter Stock No./Scan" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 40%">
                                            <asp:TextBox ID="txtstockno" CssClass="textb" Height="40px" Width="250px" runat="server"
                                                onKeypress="KeyDownHandler(event);" />
                                            <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="txtstockno_TextChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%">
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="LblPcsType" Text="" CssClass="labelbold" runat="server" ForeColor="Red"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%">
                                    <tr id="TRItemdetail" runat="server" visible="false">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="labelboldred">
                                                        <asp:Label ID="txtitem" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelboldred">
                                                        <asp:Label ID="txtQuality" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelboldred">
                                                        <asp:Label ID="txtDesign" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelboldred">
                                                        <asp:Label ID="txtcolor" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelboldred">
                                                        <asp:Label ID="txtshape" runat="server"></asp:Label>:<asp:Label ID="txtsize" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--<table>
                                                <tr><td style="width:50%" class="labelnormal">Item</td><td class="labelboldred"><asp:Label ID="Label24" runat="server" ></asp:Label></td></tr>
                                                <tr><td class="labelnormal">Quality</td ><td class="labelboldred"> <asp:Label ID="Label25" runat="server" ></asp:Label></td></tr>
                                                <tr><td class="labelnormal">Design</td><td class="labelboldred"> <asp:Label ID="Label26" runat="server" ></asp:Label></td></tr>
                                                <tr><td class="labelnormal">Color</td><td class="labelboldred"> <asp:Label ID="Label27" runat="server" ></asp:Label></td></tr>
                                                <tr><td class="labelnormal">Shape</td><td class="labelboldred"><asp:Label ID="Label28" runat="server" ></asp:Label></td></tr>
                                                <tr><td class="labelnormal">Size</td><td class="labelboldred"><asp:Label ID="Label38" runat="server" ></asp:Label></td></tr>
                                            </table>--%>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgProduct" runat="server" Height="100px" Width="100px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%" border="1" cellspacing="3">
                        <tr>
                            <td style="width: 80%" valign="top">
                                <table style="width: 100%" border="1" cellspacing="3">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label Text="Width" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtwidth" CssClass="textb" Width="90%" runat="server" AutoPostBack="true"
                                                OnTextChanged="txtwidth_TextChanged" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label12" Text="Length" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtlength" CssClass="textb" Width="90%" runat="server" AutoPostBack="true"
                                                OnTextChanged="txtlength_TextChanged" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label13" Text="Area" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="txtarea" CssClass="textb" Width="90%" runat="server" Enabled="false"
                                                BackColor="LightGray" />
                                        </td>
                                    </tr>
                                    <tr id="Tractualwidthlength" runat="server" visible="false">
                                        <td style="width: 10%">
                                            <asp:Label ID="Label17" Text="Actual Width" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtactualwidth" CssClass="textb" Width="90%" runat="server" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label18" Text="Actual Length" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtactuallength" CssClass="textb" Width="90%" runat="server" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label32" Text="Consumption" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="TxtConsumption" CssClass="textb" Width="90%" runat="server" Enabled="false"
                                                BackColor="LightGray" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label6" Text="Enter Stock Weight" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtstockweight" CssClass="textb" Width="90%" runat="server" onkeypress="return isNumberKey(event);" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label10" Text="Penality Amt." CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtpenalityamt" CssClass="textb" Width="90%" runat="server" onkeypress="return isNumberKey(event);" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label11" Text="Penality Remarks" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="txtpenalityremarks" CssClass="textb" Width="90%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label16" Text="Comm. Rate" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtcommrate" CssClass="textb" Width="90%" runat="server" onkeypress="return isNumberKey(event);" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="Label14" Text="Hold/Rejected" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td style="width: 15%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddStockQualityType" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                            OnSelectedIndexChanged="ddStockQualityType_SelectedIndexChanged">
                                                            <asp:ListItem Value="1">Finished</asp:ListItem>
                                                            <asp:ListItem Value="2">Hold</asp:ListItem>
                                                            <asp:ListItem Value="3">Rejected</asp:ListItem>
                                                            <asp:ListItem Value="4">Return</asp:ListItem>
                                                            <asp:ListItem Value="5">Fail</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="Tdreturnremark" runat="server" visible="false">
                                                        <asp:Label ID="lblretremark" Text="Return Remark" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="txtretremark" CssClass="textb" Width="100%" runat="server" TextMode="MultiLine"
                                                            Height="70px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="TDQaname" runat="server">
                                            <asp:Label ID="Label29" Text="QA NAME" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList ID="DDQaname" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label34" Text="Grade" runat="server" CssClass="labelbold" Visible="false" />
                                            <br />
                                            <asp:DropDownList ID="DDCarpetGrade" runat="server" CssClass="dropdown" Width="100px"
                                                Visible="false">
                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">A</asp:ListItem>
                                                <asp:ListItem Value="2">B</asp:ListItem>
                                                <asp:ListItem Value="3">C</asp:ListItem>
                                                <asp:ListItem Value="4">F</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnconfirm" Text="Confirm to Save" CssClass="buttonnorm" runat="server"
                                                OnClick="btnconfirm_Click" />
                                            <asp:Button ID="BtnShowData" Text="ShowData" CssClass="buttonnorm" runat="server"
                                                OnClick="ShowData_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 30%" valign="top" runat="server" id="TdRawmaterialissued" visible="false">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="background-color: #faf9f7">
                                                        <asp:Label ID="Label31" Text="Raw Material Issued Details" CssClass="labelbold" ForeColor="Red"
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="Div3" runat="server" style="max-height: 200px; width: 100%; overflow: auto">
                                                            <asp:GridView ID="GVRawMaterialIssued" AutoGenerateColumns="False" runat="server"
                                                                CssClass="grid-views" EmptyDataText="No. Records found." Width="100%">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Issue Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblissuedate" Text='<%#Bind("Issuedate") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblitemname" Text='<%#Bind("Itemname") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Shade Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblshadename" Text='<%#Bind("Shadecolorname") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Issued Qty.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIssuedQty" Text='<%#Bind("IssuedQty") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <table id="TableIssueDetail" runat="server" visible="true" width="100%">
                        <tr>
                            <td style="width: 80%" valign="top">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="background-color: #faf9f7">
                                            <asp:Label ID="Label22" Text="Issued Details" CssClass="labelbold" ForeColor="Red"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="gride" runat="server" style="max-height: 200px; width: 100%; overflow: auto">
                                                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                    DataKeyNames="item_finished_id" EmptyDataText="No. Records found." Width="100%"
                                                    AutoGenerateSelectButton="true" OnSelectedIndexChanged="DG_SelectedIndexChanged">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" Visible="false">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chkboxitem" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="400px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Unit">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ordered Qty">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblorderedqty" Text='<%#Bind("orderedqty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Received Qty.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblreceivedqty" Text='<%#Bind("Receivedqty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pending Qty.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Convert.ToInt32(Eval("orderedqty")) -Convert.ToInt32(Eval("Receivedqty")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rec Qty." Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtrecqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight (Kg.)" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtweight" Width="70px" BackColor="Yellow" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                                <asp:Label ID="lbllength" Text='<%#Bind("length") %>' runat="server" />
                                                                <asp:Label ID="lblwidth" Text='<%#Bind("width") %>' runat="server" />
                                                                <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                                                <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                                                                <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                                <asp:Label ID="lblissuedetailid" Text='<%#Bind("Issue_Detail_Id") %>' runat="server" />
                                                                <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                                <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                                                <asp:Label ID="lblcaltype" Text='<%#Bind("caltype") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="TDstockno" runat="server" visible="false" valign="top" style="width: 20%"
                                height="110px">
                                <table>
                                    <tr>
                                        <td>
                                            <div style="max-height: 100px; overflow: auto; margin-left: 10%">
                                                <asp:GridView ID="DGStockDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Stock No. found to Receive."
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
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr id="Trsave" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lbltpcs" Text="Total Pcs" CssClass="labelbold" runat="server" />
                                                        <br />
                                                        <asp:TextBox ID="txttotalpcsgrid" runat="server" CssClass="textb" Width="90px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnsavefrmgrid" Text="Save" CssClass="buttonnorm" runat="server"
                                                            OnClick="btnsavefrmgrid_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 80%" valign="top">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="background-color: #faf9f7">
                                            <asp:Label Text="Received Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <div id="Div1" runat="server" style="max-height: 300px; width: 100%; overflow: auto">
                                                            <asp:GridView ID="DGRecDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                                EmptyDataText="No. Records found." OnRowCancelingEdit="DGRecDetail_RowCancelingEdit"
                                                                OnRowEditing="DGRecDetail_RowEditing" OnRowUpdating="DGRecDetail_RowUpdating"
                                                                OnRowDeleting="DGRecDetail_RowDeleting" OnRowDataBound="DGRecDetail_RowDataBound"
                                                                Width="100%">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Item Description">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rec Qty.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblrecqty" Text='<%#Bind("Recqty") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Weight (Kg.)">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtweight" Text='<%#Bind("Weight") %>' runat="server" Width="80px" />
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblweight" Text='<%#Bind("Weight") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtrategrid" Width="70px" Text='<%#Bind("Rate") %>' runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm.Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommrate" Text='<%#Bind("Comm") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtcommrategrid" Width="70px" Text='<%#Bind("comm") %>' runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpenalitygrid" Text='<%#Bind("penality") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtpenalitygrid" Width="70px" Text='<%#Bind("penality") %>' runat="server"
                                                                                onkeypress="return isNumberKey(event);" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality Remark">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpenalityremarkgrid" Text='<%#Bind("Premarks") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtpenalityremarkgrid" Width="250px" Text='<%#Bind("Premarks") %>'
                                                                                runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Stock No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblstockno" Text='<%#Bind("StockNo") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Grade">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCarpetGradeName" runat="server" Text='<%#Bind("CarpetGradeName") %>'></asp:Label>
                                                                            <asp:Label ID="lblCarpetGradeId" runat="server" Text='<%#Bind("CarpetGrade") %>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="QA NAME">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblqanamegrid" Text='<%#Bind("Qaname") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtqanamegrid" Width="250px" Text='<%#Bind("Qaname") %>' runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprocessrecid" Text='<%#Bind("Process_Rec_Id") %>' runat="server" />
                                                                            <asp:Label ID="lblprocessrecdetailid" Text='<%#Bind("Process_Rec_Detail_Id") %>'
                                                                                runat="server" />
                                                                            <asp:Label ID="lblHrate" Text='<%#Bind("Rate") %>' runat="server" />
                                                                            <asp:Label ID="lblHcommrate" Text='<%#Bind("comm") %>' runat="server" />
                                                                            <asp:Label ID="lblDefectStatus" Text='<%#Bind("DefectStatus") %>' runat="server" />
                                                                            <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />
                                                                            <asp:Label ID="lblCalType" Text='<%#Bind("CalType") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Width">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblactualwidth" Text='<%#Bind("Actualwidth") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtawidthgrid" Text='<%#Bind("Actualwidth") %>' Width="80px" runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Length">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblactuallength" Text='<%#Bind("ActualLength") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtalengthgrid" Text='<%#Bind("ActualLength") %>' Width="80px" runat="server" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="QC CHECK">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkQccheck" runat="server" CausesValidation="False" Text="QCCHECK"
                                                                                OnClientClick="return confirm('Do you want to check QC?')" OnClick="lnkqcparameter_Click"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:CommandField>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkRemoveQccheck" runat="server" CausesValidation="False" Text="REMOVE QC"
                                                                                OnClientClick="return confirm('Do you want to remove QC?')" OnClick="lnkRemoveQccheck_Click"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Add Penality" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkAddPenality" runat="server" Text="Add Penality" OnClick="lnkAddPenality_Click">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbltotalpcs" runat="server" CssClass="labelbold" Text="Total Pcs" />
                                                                    <br />
                                                                    <asp:TextBox ID="txttotalpcs" runat="server" BackColor="LightYellow" CssClass="textb"
                                                                        Enabled="false" Width="70px" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btncheckallpcsqc" runat="server" CssClass="buttonnorm" OnClick="btncheckallpcsqc_Click"
                                                                        Text="QC Check All Pcs" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label24" runat="server" CssClass="labelbold" Text="Remarks" />
                                                                    <br />
                                                                    <asp:TextBox ID="TxtRemarks" runat="server" CssClass="textb" Width="500px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdtotalwt" runat="server" visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label20" Text="Enter Total Weight(kgs)" CssClass="labelbold" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txttotalstockwt" CssClass="textb" Width="100px" runat="server" onkeypress="return isNumberKey(event);" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnsubmitweight" CssClass="buttonnorm" Text="Submit Weight" runat="server"
                                                                        OnClick="btnsubmitweight_Click" OnClientClick="return confirm('Do you want to update?');" />
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
                            <td style="width: 20%" valign="top">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="background-color: #faf9f7">
                                                        <asp:Label ID="Label21" Text="Consumed Material Details" CssClass="labelbold" ForeColor="Red"
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="Div2" runat="server" style="max-height: 300px; width: 100%; overflow: auto">
                                                            <asp:GridView ID="DGconsumedDetails" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                                EmptyDataText="No. Records found." Width="100%">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Stock No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblstockno" Text='<%#Bind("TStockno") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblitemname" Text='<%#Bind("Item_name") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Consumed Qty.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblconsmpqty" Text='<%#Bind("consmpqty") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Loss Qty.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbllossqty" Text='<%#Bind("Lossqty") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td align="right">
                            <asp:CheckBox ID="ChkForSummaryReport" Text="For Summary" CssClass="labelbold" runat="server" Visible="false" />
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    Visible="false" UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnQcreport" runat="server" Text="QC Report" CssClass="buttonnorm"
                                    OnClick="btnQcreport_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnReturnGatePass" runat="server" Text="Return GatePass" CssClass="buttonnorm"
                                    OnClick="btnReturnGatePass_Click" />
                                <asp:Button ID="BtnUpdateRate" runat="server" Text="Update Rate FolioNo Wise" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnUpdateRate_Click" />
                                <asp:Button ID="BtnUpdateConsumption" runat="server" Text="Update Consumption" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnUpdateConsumption_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnprocessrecid" runat="server" Value="0" />
                <asp:HiddenField ID="hn100_ISSUEORDERID" runat="server" Value="0" />
                <asp:HiddenField ID="hn100_PROCESS_REC_ID" runat="server" Value="0" />
                <asp:HiddenField ID="hnunitid" runat="server" Value="0" />
                <asp:HiddenField ID="hncaltype" runat="server" Value="0" />
                <asp:HiddenField ID="hnshapeid" runat="server" Value="0" />
                <asp:HiddenField ID="hnlastfoliono" runat="server" Value="0" />
                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                <asp:HiddenField ID="hnRejectedGatePassNo" runat="server" Value="0" />
                <asp:HiddenField ID="hnlastempids" runat="server" Value="" />
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                    <asp:ModalPopupExtender ID="Modalpopupext" runat="server" PopupControlID="pnModelPopup"
                        TargetControlID="btnModalPopUp" BackgroundCssClass="modalBackground" CancelControlID="btnqcclose">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: ActiveCaption;
                        display: none;">
                        <fieldset>
                            <legend>
                                <asp:Label ID="lblqc" Text="QC PARAMETER" runat="server" ForeColor="Red" CssClass="labelbold" />
                            </legend>
                            <table>
                                <tr>
                                    <td>
                                        <div style="max-height: 500px; overflow: scroll; width: 850px" id="divqc">
                                            <asp:GridView ID="GDQC" CssClass="grid-views" runat="server" OnRowDataBound="GDQC_RowDataBound"
                                                OnRowCreated="GDQC_RowCreated">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnqcsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnqcsave_Click" />
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
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp2" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopuptext2" runat="server" PopupControlID="pnModelPopup2"
                        TargetControlID="btnModalPopUp2" BackgroundCssClass="modalBackground" CancelControlID="BtnRemoveQCClose">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup2" runat="server" Style="background-color: ActiveCaption;
                        display: none;">
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label33" Text="QC REMOVE DATE" runat="server" ForeColor="Red" CssClass="labelbold" />
                            </legend>
                            <table>
                                <tr>
                                    <td>
                                        <div style="max-height: 300px; overflow: scroll; width: 250px" id="div4">
                                            <asp:TextBox ID="txtRemoveQCDate" CssClass="textb" Width="95px" runat="server" />
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtRemoveQCDate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="BtnRemoveQCSave" Text="Save" runat="server" CssClass="buttonnorm"
                                            OnClick="BtnRemoveQCSave_Click" />
                                        <asp:Button ID="BtnRemoveQCClose" Text="Close" runat="server" CssClass="buttonnorm" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRemoveqcmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblRemoveQCProcessRecId" runat="server" CssClass="labelbold" ForeColor="Red"
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblRemoveQCProcessRecDetailId" runat="server" CssClass="labelbold"
                                            ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </div>
                <div>
                    <asp:LinkButton ID="btnModalPopUp3" runat="server"></asp:LinkButton>
                    <asp:ModalPopupExtender ID="ModalPopuptext3" runat="server" PopupControlID="pnModelPopup3"
                        TargetControlID="btnModalPopUp3" BackgroundCssClass="modalBackground" CancelControlID="btnHide">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup3" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                            <asp:GridView ID="GVPenalty" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                Width="350px" OnRowDataBound="GVPenalty_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts1" />
                                <RowStyle CssClass="gvrow1" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <%--<HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkboxitem" runat="server" />
                                            <%--<asp:CheckBox ID="Chkboxitem" runat="server" AutoPostBack="true" OnCheckedChanged="Chkboxitem_CheckedChanged" />--%>
                                            <%--onclick="return CheckBoxClick(this);"--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Penality Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPenalityName" Text='<%#Bind("PenalityName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtQty_TextChanged" onkeypress="return isNumberKey(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" Text='<%#Bind("rate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAmt" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtAmt_TextChanged" />                                   
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPenalityId" Text='<%#Bind("PenalityId") %>' runat="server" Visible="false" />
                                            <asp:Label ID="lblPenalityType" Text='<%#Bind("PenalityType") %>' runat="server"
                                                Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="header">
                            <asp:Label ID="lblPenalityUpdateMsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                            <asp:Button ID="btnSavePenality" runat="server" Text="Submit" OnClick="btnSavePenality_Click" />
                            <asp:Button ID="btnHide" runat="server" Text="Close" />
                            <asp:Label ID="lblPenalityProcessRecId" runat="server" CssClass="labelbold" ForeColor="Red"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblPenalityProcessRecDetailId" runat="server" CssClass="labelbold"
                                ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
                <asp:HiddenField ID="HnWPenalityId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
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
                    $('#<%=pnlpopup4.ClientID %>').show();
                }
                function HidePopup() {
                    $('#mask').hide();
                    $('#<%=pnlpopup4.ClientID %>').hide();
                }
                $(".btnPwd").live('click', function () {
                    HidePopup();
                });
            </script>
            <div id="mask">
            </div>
            <asp:Panel ID="pnlpopup4" runat="server" BackColor="White" Height="175px" Width="300px"
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
        </div>
    </div>
</asp:Content>
