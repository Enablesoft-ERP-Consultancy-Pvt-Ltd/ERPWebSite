<%@ Page Language="C#" Title="ProcessReceive" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="ProcessReceive.aspx.cs" Inherits="Masters_process_ProcessReceive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "ProcessReceive.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validator() {
            if (document.getElementById("<%=TxtRecQty.ClientID %>").value == "" || document.getElementById("<%=TxtRecQty.ClientID %>").value == "0") {
                alert("Rec Quantity Cann't be blank or Zero.. ");
                document.getElementById("<%=TxtRecQty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?');
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
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="80%">
                    <tr id="trprifix" runat="server">
                        <td>
                            <asp:HiddenField ID="hncomp" runat="server" />
                        </td>
                        <td align="center" class="tdstyle">
                            <asp:Label ID="lbl" Text="PreFix" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPrefix" runat="server" CssClass="textb" AutoPostBack="True" OnTextChanged="TxtPrefix_TextChanged"></asp:TextBox>
                        </td>
                        <td align="center" class="tdstyle">
                            <asp:Label ID="Label3" Text=" No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPostfix" runat="server" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtPostfix_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td id="tdSrNo" runat="server" class="tdstyle" visible="false">
                            <asp:Label ID="Label30" Text="SrNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtLocalOrderNo" runat="server" CssClass="textb" OnTextChanged="TxtLocalOrderNo_TextChanged"
                                Width="80px" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td id="tdFolioNo" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label31" Text="FolioNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDFolioNo" AutoPostBack="true" runat="server" Width="200px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged"
                                CssClass="dropdown">
                            </asp:DropDownList>                          
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text="POrderNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPOrderNo" runat="server" CssClass="textb" OnTextChanged="TxtPOrderNo_TextChanged"
                                Width="80px" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text="CompanyName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" AutoPostBack="true" runat="server" Width="200px"
                                CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" Text=" ProcessName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label7" Text=" Vendor Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDEmployeeNamee" runat="server" AutoPostBack="true" Width="250px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDEmployeeNamee_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmployeeNamee"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label8" Text="Challan No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtChallanNo" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label9" Text="Rec.Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRecDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>

                         <td id="TDPartyChallanNo" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label32" Text="Party ChallanNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtPartyChallanNo" runat="server" Width="90px" CssClass="textb"></asp:TextBox>                           
                        </td>

                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label26" runat="server" Text="PO No. " CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDPONo" runat="server" AutoPostBack="true" CssClass="dropdown"
                                OnSelectedIndexChanged="DDPONo_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDPONo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDTextProductCode" runat="server">
                            <asp:Label ID="LblProdCode" class="tdstyle" runat="server" Text="Prod Code " CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtProductCode" runat="server" AutoPostBack="true" OnTextChanged="TxtProductCode_TextChanged"
                                Width="125px" CssClass="textb"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Category " CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDCategoryName" runat="server" AutoPostBack="true" Width="150px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategoryName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td style="width: 100px">
                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDItemName" runat="server" Width="150px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDItemName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label10" Text="Description" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDDescription" Width="320px" runat="server" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDDescription"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            &nbsp
                        </td>
                        <td colspan="2" id="TDLagat" runat="server" visible="false">
                            <asp:Label ID="lblLagat" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label11" Text=" Quality Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddStockQualityType" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Width="110px" OnSelectedIndexChanged="ddStockQualityType_SelectedIndexChanged">
                                <asp:ListItem Value="1">Finished</asp:ListItem>
                                <asp:ListItem Value="2">Second</asp:ListItem>
                                <asp:ListItem Value="3">Rejected/Return</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" runat="server" id="tdcalname">
                            <asp:Label ID="Label12" Text=" Cal Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Enabled="False"
                                Width="100px">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label13" Text=" Unit" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Enabled="False">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label14" Text=" P.Iss.Qty." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtIssuQty" runat="server" Width="80px" CssClass="textb" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label15" Text="Pend.Qty." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPQty" runat="server" Width="80px" CssClass="textb" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDwwidth">
                            <asp:Label ID="Label17" Text="Width" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtWidth" runat="server" CssClass="textb" Width="80px" OnTextChanged="TxtWidth_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDleng">
                            <asp:Label ID="Label16" Text="Length" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Txtlength" runat="server" CssClass="textb" Width="80px" OnTextChanged="Txtlength_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDactualW">
                            <asp:Label ID="Label28" Text="Actual Width" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtactualW" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDactualL">
                            <asp:Label ID="Label27" Text="Actual Length" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtactualL" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDComm">
                            <asp:Label ID="Label18" Text="Comm" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtCommission" runat="server" Width="70px" CssClass="textb" ReadOnly="True"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDArea">
                            <asp:Label ID="Label19" Text="Area" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtArea" runat="server" Width="80px" CssClass="textb" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDrate">
                            <asp:Label ID="Label20" Text="Rate" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRate" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle" runat="server" id="Tdpenality">
                            <asp:Label ID="Label21" Text="Penality" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtPEnality" runat="server" Width="80px" CssClass="textb" OnTextChanged="TxtPEnality_TextChanged"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td colspan="3" class="tdstyle">
                            <asp:Label ID="Label22" Text=" P Remarks" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtPRemarks" runat="server" CssClass="textb" Width="500px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDrecqty">
                            <asp:Label ID="Label23" Text="RecQty" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRecQty" runat="server" Width="100px" CssClass="textb" BackColor="#FFFFF3"
                                AutoPostBack="True" OnTextChanged="TxtRecQty_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDweigth">
                            <asp:Label ID="Label24" Text=" Weight" runat="server" CssClass="labelbold" />
                            <asp:RangeValidator ID="ValQty1" runat="server" ControlToValidate="TxtWeight" ForeColor="Red"
                                MaximumValue="1000000" MinimumValue="0.1" Text="Fill Weight" Type="Double" />
                            <br>
                            <asp:TextBox ID="TxtWeight" runat="server" CssClass="textb" Width="120px" onkeydown="return (event.keyCode!=13);"
                            AutoPostBack="True" OnTextChanged="TxtWeight_TextChanged"></asp:TextBox>
                        </td>
                        <td id="TDQaname" runat="server" visible="false">
                            <asp:Label ID="Label29" Text="QA NAME" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDQaname" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>

                        <td class="tdstyle" runat="server" id="TDhnQualityGrmPerMeterPlusMinus" visible="false">
                            <asp:HiddenField ID="hnQualityGrmPerMeterMinus" runat="server" />
                             <asp:HiddenField ID="hnQualityGrmPerMeterPlus" runat="server" />
                             <asp:HiddenField ID="hnAreaMeter" runat="server" />
                           
                        </td>
                    </tr>
                </table>
                <table width="80%">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label25" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="right">
                            <asp:Label ID="llMessageBox" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:CheckBox ID="chkForSlip" runat="server" Text="For Slip Print" CssClass="labelnormalMM"
                                Font-Bold="true" Visible="false" />
                                 &nbsp
                                <asp:CheckBox ID="ChkForRejectPcsSlip" runat="server" Text="For Reject Pcs Slip" CssClass="labelnormalMM" AutoPostBack="true" 
                                  OnCheckedChanged="ChkForRejectPcsSlip_CheckedChanged"   Font-Bold="true" Visible="false" />
                            &nbsp
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                UseSubmitBehavior="false" OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" Enabled="false"
                                runat="server" Text="Preview" OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm " ID="BtnGatePass" Enabled="false" runat="server"
                                Text="GatePass" OnClick="BtnGatePass_Click" Width="100px" />
                            &nbsp;<asp:Button ID="btnqcchkpreview" runat="server" CssClass="buttonnorm" Text="QCReport"
                                OnClick="btnqcchkpreview_Click" Width="100px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
                <table width="90%">
                    <tr>
                        <td colspan="7">
                            <div style="height: 250px; overflow: auto; width: 800px;">
                                <asp:GridView ID="DGRec" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                    OnRowDataBound="DGRec_RowDataBound" OnSelectedIndexChanged="DGRec_SelectedIndexChanged"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SrNo.">
                                            <ItemTemplate>
                                                <%#Container.DisplayIndex+1 %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Category" HeaderText="Category">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Item" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Length" HeaderText="Length">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Width" HeaderText="Width">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Area" HeaderText="Area">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Weight" HeaderText="Weight">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Penality" HeaderText="Penality">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TStockNo" HeaderText="StockNo">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="300px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td id="qulitychk" runat="server" valign="top">
                            <div id="Div2" runat="server" style="height: 250px; overflow: auto;">
                                <asp:GridView ID="grdqualitychk" runat="server" AutoGenerateColumns="False" DataKeyNames="ID">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkall" Text="" runat="server" onclick="return CheckAll(this);" />
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SrNo">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ParaName">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ParaName") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ParaName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtqcreason" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                        <td runat="server" id="tdordergrid">
                            <div id="Div1" runat="server" style="height: 250px; overflow: auto; width: 300px;">
                                <asp:GridView ID="dgorder" AutoGenerateColumns="false" runat="server" CssClass="grid-views"
                                    OnSelectedIndexChanged="dgorder_SelectedIndexChanged" OnRowDataBound="dgorder_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="Description" HeaderText="Order Description" />
                                        <asp:BoundField DataField="Qty" HeaderText="Ordered Qty" />
                                        <asp:TemplateField HeaderText="Balance to Receive">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("ITEM_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("ShadecolorId") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="LBLUNIT" runat="server" Text='<%# Bind("unit") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="Issue_Detail_Id" runat="server" Text='<%# Bind("Issue_Detail_Id") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblbalnce" runat="server" Text='<%#Bind("issueqqty")%> ' />
                                                 <%--<asp:Label ID="lblbalnce" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "Issue_Detail_Id").ToString()) %>' />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Local OrderNo">                                           
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("LocalOrder") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
