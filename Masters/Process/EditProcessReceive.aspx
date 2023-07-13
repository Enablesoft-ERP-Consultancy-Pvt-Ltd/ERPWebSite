<%@ Page Language="C#" Title="EditProcessReceive" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="EditProcessReceive.aspx.cs" Inherits="Masters_Process_EditProcessReceive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function NewForm() {
            window.location.href = "EditProcessReceive.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
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
                <table>
                    <tr>
                        <td>
                        </td>
                        <td id="trprifix" align="center" runat="server">
                            <asp:Label Text="Prefix" runat="server" ID="lbl" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPrefix" runat="server" CssClass="textb" AutoPostBack="True" OnTextChanged="TxtPrefix_TextChanged"></asp:TextBox>
                        </td>
                        <td align="center" runat="server">
                            <asp:Label Text=" No." runat="server" ID="Label3" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPostfix" runat="server" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtPostfix_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="POrderNo" runat="server" ID="Label4" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" CompanyName" runat="server" ID="Label5" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="ProcessName" runat="server" ID="Label7" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="EmployeeName" runat="server" ID="Label8" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" PO No." runat="server" ID="Label9" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtPOrderNo" runat="server" CssClass="textb" Width="100px" OnTextChanged="TxtPOrderNo_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDEmployeeNamee" runat="server" AutoPostBack="true" Width="200px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDEmployeeNamee_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDPONo" runat="server" AutoPostBack="true" Width="100px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDPONo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Challan No" runat="server" ID="Label10" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Rec Date" runat="server" ID="Label11" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkEdit" class="tdstyle" runat="server" Text="To Edit" AutoPostBack="True"
                                Enabled="false" Checked="true" OnCheckedChanged="ChkEdit_CheckedChanged" CssClass="checkboxbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category " CssClass="labelbold"></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtChallanNo" runat="server" CssClass="textb" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRecDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDReceiveNo" runat="server" AutoPostBack="true" Width="150px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged"
                                Visible="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCategoryName" runat="server" AutoPostBack="true" Width="200px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="DDItemName" runat="server" AutoPostBack="true" Width="150px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2" class="tdstyle">
                            <asp:Label Text="Description" runat="server" ID="Label12" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Quality Type" runat="server" ID="Label13" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle" runat="server" id="tdcalname">
                            <asp:Label Text=" Cal Type" runat="server" ID="Label14" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Unit" runat="server" ID="Label15" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="P.Iss.Qty." runat="server" ID="Label16" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Pend.Qty." runat="server" ID="Label17" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:DropDownList ID="DDDescription" Width="350px" runat="server" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddStockQualityType" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Width="110px" OnSelectedIndexChanged="ddStockQualityType_SelectedIndexChanged">
                                <asp:ListItem Value="1">Finished</asp:ListItem>
                                <asp:ListItem Value="2">Second</asp:ListItem>
                                <asp:ListItem Value="3">Rejected/Return</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="tdcaltype" runat="server">
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                Enabled="False">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtIssuQty" runat="server" CssClass="textb" Width="70px" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtPQty" runat="server" CssClass="textb" Width="70px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle" runat="server" id="TDwwidth">
                            <asp:Label Text="Width" runat="server" ID="Label19" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtWidth" runat="server" AutoPostBack="True" CssClass="textb" OnTextChanged="TxtWidth_TextChanged"
                                Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDleng">
                            <asp:Label Text=" Length" runat="server" ID="Label18" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Txtlength" runat="server" AutoPostBack="True" CssClass="textb" OnTextChanged="Txtlength_TextChanged"
                                Width="100px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDactualW">
                            <asp:Label ID="Label28" Text="Actual Width" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtactualW" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDactualL">
                            <asp:Label ID="Label6" Text="Actual Length" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtactualL" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDArea">
                            <asp:Label Text=" Area" runat="server" ID="Label20" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtArea" runat="server" CssClass="textb" Enabled="False" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDcomm">
                            <asp:Label Text=" Comm" runat="server" ID="Label21" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtCommission" runat="server" CssClass="textb" ReadOnly="True" Width="80px"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDrate">
                            <asp:Label Text="Rate" runat="server" ID="Label22" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRate" runat="server" CssClass="textb" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDrecqty">
                            <asp:Label Text=" RecQty" runat="server" ID="Label23" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRecQty" runat="server" AutoPostBack="True" CssClass="textb" OnTextChanged="TxtRecQty_TextChanged"
                                Width="90px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="TDweigth">
                            <asp:Label Text=" Weight" runat="server" ID="Label24" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtWeight" runat="server" CssClass="textb" Width="90px" onkeydown="return (event.keyCode!=13);"
                            AutoPostBack="True" OnTextChanged="TxtWeight_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle" runat="server" id="Tdpenality">
                            <asp:Label Text=" Penality" runat="server" ID="Label25" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPEnality" runat="server" CssClass="textb"></asp:TextBox>
                        </td>
                        <td id="TDQaname" runat="server" visible="false">
                            <asp:Label ID="Label29" Text="QA NAME" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDQaname" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label Text=" P Remarks" runat="server" ID="Label26" CssClass="labelbold" />
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="TxtPRemarks" runat="server" CssClass="textb" Width="440px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <tr>
                            <td>
                                <asp:Label Text=" Remarks" runat="server" ID="Label27" CssClass="labelbold" />
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="TxtRemarks" Width="440px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField ID="hncomp" runat="server" />
                                 <asp:HiddenField ID="hnQualityGrmPerMeterMinus" runat="server" />
                                 <asp:HiddenField ID="hnQualityGrmPerMeterPlus" runat="server" />
                                 <asp:HiddenField ID="hnAreaMeter" runat="server" />
                            </td>
                        </tr>
                </table>
                <table width="80%">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:CheckBox ID="chkForSlip" runat="server" Text="For Slip Print" CssClass="labelnormalMM"
                                Font-Bold="true" Visible="false" />
                                 &nbsp
                                  <asp:CheckBox ID="ChkForRejectPcsSlip" runat="server" Text="For Reject Pcs Slip" CssClass="labelnormalMM" AutoPostBack="true" 
                                  OnCheckedChanged="ChkForRejectPcsSlip_CheckedChanged"   Font-Bold="true" Visible="false" />
                            &nbsp
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';"
                                OnClick="BtnSave_Click" UseSubmitBehavior="false" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnUpdate" runat="server" OnClick="BtnUpdate_Click"
                                Text="Update" Visible="False" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button ID="btnqcchkpreview" runat="server" CssClass="buttonnorm" Text="QCReport"
                                OnClick="btnqcchkpreview_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnGatePass" runat="server" Text="GatePass"
                                OnClick="BtnGatePass_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnCurrentConsumption" runat="server"
                                Text="Current Consumption" OnClick="BtnCurrentConsumption_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnPreviewWithConsmp" runat="server"
                                Text="Preview Consmp" OnClick="BtnPreviewWithConsmp_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="CloseForm();"
                                runat="server" Text="Close" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="llMessageBox" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="max-height: 500px; overflow: auto; width: 900px;">
                                <asp:GridView ID="DGRec" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                    CssClass="grid-view" OnRowDeleting="DGRec_RowDeleting" OnRowDataBound="DGRec_RowDataBound1"
                                    OnRowCancelingEdit="DGRec_RowCancelingEdit" OnRowEditing="DGRec_RowEditing" OnRowUpdating="DGRec_RowUpdating">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <%--      <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcategory" Text='<%#Bind("Category") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitem" Text='<%#Bind("Item") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLength" Text='<%#Bind("Length") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWidth" Text='<%#Bind("Width") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actual Length">
                                            <ItemTemplate>
                                                <asp:Label ID="lblactualL" Text='<%#Bind("ActualLength") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtgridactualL" runat="server" Text='<%#Bind("ActualLength") %>'
                                                    CssClass="textb" Width="80px"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actual Width">
                                            <ItemTemplate>
                                                <asp:Label ID="lblactualW" Text='<%#Bind("ActualWidth") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtgridactualW" runat="server" Text='<%#Bind("ActualWidth") %>'
                                                    CssClass="textb" Width="80px"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtrateedit" runat="server" Text='<%#Bind("Rate") %>' Width="70px"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comm. Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcomm" Text='<%#Bind("Comm") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtcommedit" runat="server" Text='<%#Bind("Comm") %>' Width="70px"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Area">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArea" Text='<%#Bind("Area") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Text='<%#Bind("Amount") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weight">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWeight" Text='<%#Bind("Weight") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtweightedit" Text='<%#Bind("Weight") %>' runat="server" Width="60px" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Penality">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPenality" Text='<%#Bind("Penality") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtpenalityedit" Text='<%#Bind("Penality") %>' runat="server" Width="60px" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Penality Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPenalityRemark" Text='<%#Bind("Premarks") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtpremarksedit" Text='<%#Bind("Premarks") %>' runat="server" Width="250px" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StockNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockNo" Text='<%#Bind("TStockNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QA NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblqaname" Text='<%#Bind("Qaname") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtqanameedit" Text='<%#Bind("Qaname") %>' runat="server" Width="250px" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField DeleteText="" ShowEditButton="True" />
                                        <asp:TemplateField HeaderText="QCCHECK">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkqccheck" Text="QCCHECK" runat="server" OnClick="lnkqccheck" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemidgrid" Text='<%#Bind("Item_id") %>' runat="server" />
                                                <asp:Label ID="lblcategoryidgrid" Text='<%#Bind("category_id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td runat="server" id="tdordergrid">
                            <div id="Div1" runat="server" style="max-height: 250px; overflow: auto; width: 250px">
                                <asp:GridView ID="dgorder" AutoGenerateColumns="false" runat="server" OnSelectedIndexChanged="dgorder_SelectedIndexChanged"
                                    OnRowDataBound="dgorder_RowDataBound" AutoGenerateSelectButton="true">
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
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="qulitychk" runat="server">
                            <div id="Div2" runat="server" style="max-height: 300px; overflow: auto;">
                                <asp:GridView ID="grdqualitychk" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                    CssClass="grid-views">
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
                    </tr>
                    <tr runat="server" id="Trupdateqcdetail" visible="false">
                        <td>
                            <asp:LinkButton ID="lnkupdateqcdetail" runat="server" Text="Update QC Detail" Font-Bold="true"
                                OnClick="lnkupdateqcdetail_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
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

</asp:Content>
