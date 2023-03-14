<%@ Page Title="PRODUCTION RECEIVE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmProductionReceiveLoom.aspx.cs" Inherits="Masters_Loom_frmProductionReceiveLoom" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Loomnoselected(source, eventArgs) {
            document.getElementById('<%=txtloomid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function EmpSelectedEdit(source, eventArgs) {
            document.getElementById('<%=txteditempid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnsearchedit.ClientID%>').click();
        }
        function NewForm() {
            window.location.href = "frmproductionReceiveLoom.aspx";
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
      
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <div style="width: 80%; float: left;">
                        <table id="TBStockseries" runat="server" visible="false" border="1" cellpadding="0"
                            cellspacing="3" style="width: 20%; text-align: center; margin-left: 30%">
                            <tr>
                                <td colspan="2">
                                    <span class="labelbold">Stock No. Series</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px">
                                    <asp:Label ID="lblprefix" CssClass="labelbold" Text="Prefix" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtprefix" CssClass="textb" Width="100px" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtprefix_TextChanged" />
                                </td>
                                <td style="padding: 5px">
                                    <asp:Label ID="Label6" CssClass="labelbold" Text="No." runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtpostfix" CssClass="textb" Width="100px" runat="server" AutoPostBack="true"
                                        onkeypress="return isNumberKey(event);" OnTextChanged="txtpostfix_TextChanged" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TDchkedit" runat="server" visible="false">
                                    <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkedit_CheckedChanged" />
                                </td>
                            </tr>
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
                                <td>
                                    <asp:Label Text="Folio No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtfoliono" runat="server" CssClass="textb" Width="100px" OnTextChanged="txtfoliono_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
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
                                            <td runat="server" visible="false" id="TDloomno">
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
                    </div>
                    <div style="width: 20%; float: right">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblemp" Text="Employee" CssClass="labelbold" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: auto; width: 150px">
                                        <asp:ListBox ID="listWeaverName" runat="server" Width="150px" Height="70px" SelectionMode="Multiple"
                                            Style="overflow: auto"></asp:ListBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td id="TDreceiveNo" runat="server" visible="false">
                                <asp:Label ID="Label5" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDreceiveNo" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDreceiveNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtreceiveno" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtrecdate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtrecdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td id="TDcheckedby" runat="server">
                                <asp:Label ID="lblcheckedby" CssClass="labelbold" Text="Checked By" runat="server" /><br />
                                <asp:TextBox ID="txtcheckedby" CssClass="textb" Width="250px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label4" Text="Product Description" runat="server" CssClass="labelbold"
                            ForeColor="Red" />
                    </legend>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 300px; width: 100%; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." Width="100%">
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
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" OnCheckedChanged="chkitem_CheckedChanged"
                                                        AutoPostBack="true" />
                                                </ItemTemplate>
                                                <%--<ItemStyle HorizontalAlign="Center" Width="10px" />--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="500px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Width">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtwidth" Text='<%#Bind("Width") %>' Width="100%" runat="server"
                                                        AutoPostBack="true" OnTextChanged="Txtwidthlength_TextChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Length">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlength" Text='<%#Bind("Length") %>' Width="100%" runat="server"
                                                        AutoPostBack="true" OnTextChanged="Txtwidthlength_TextChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Area">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                                </ItemTemplate>
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
                                            <asp:TemplateField HeaderText="Hold/Rejected">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddStockQualityType" runat="server" AutoPostBack="true" CssClass="dropdown">
                                                        <asp:ListItem Value="1">Finished</asp:ListItem>
                                                        <asp:ListItem Value="2">Hold</asp:ListItem>
                                                        <asp:ListItem Value="3">Rejected</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec_Qty.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrecqty" Width="100%" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight (Kg.)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtweight" Width="100%" BackColor="Yellow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality Amt.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtpenalityamt" Width="100%" BackColor="Yellow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality Remark">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtpenalityremark" Width="150px" BackColor="Yellow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remark">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtremark" Width="150px" runat="server" TextMode="MultiLine" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lbllength" Text='<%#Bind("length") %>' runat="server" />
                                                    <asp:Label ID="lblwidth" Text='<%#Bind("width") %>' runat="server" />
                                                    <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                                                    <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                    <asp:Label ID="lblissuedetailid" Text='<%#Bind("Issue_Detail_Id") %>' runat="server" />
                                                    <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                                    <asp:Label ID="lblcaltype" Text='<%#Bind("caltype") %>' runat="server" />
                                                    <asp:Label ID="lblshapeid" Text='<%#Bind("shapeid") %>' runat="server" />
                                                    <asp:Label ID="lblitemcode" Text='<%#Bind("Item_code") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 300px; width: 800px; overflow: auto">
                                    <asp:GridView ID="DGRecDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowCancelingEdit="DGRecDetail_RowCancelingEdit"
                                        OnRowEditing="DGRecDetail_RowEditing" OnRowUpdating="DGRecDetail_RowUpdating"
                                        OnRowDeleting="DGRecDetail_RowDeleting" OnRowDataBound="DGRecDetail_RowDataBound">
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
                                            <asp:TemplateField HeaderText="Rate" Visible="false">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtRate" Text='<%#Bind("Rate") %>' runat="server" Width="80px" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
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
                                            <asp:TemplateField HeaderText="Stock No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstockno" Text='<%#Bind("StockNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality Amt.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpenalityamt" Text='<%#Bind("Penality") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtpenalityamt" Width="80px" Text='<%#Bind("Penality") %>' runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprocessrecid" Text='<%#Bind("Process_Rec_Id") %>' runat="server" />
                                                    <asp:Label ID="lblprocessrecdetailid" Text='<%#Bind("Process_Rec_Detail_Id") %>'
                                                        runat="server" />
                                                    <asp:Label ID="lblReccaltype" Text='<%#Bind("caltype") %>' runat="server" />
                                                    <asp:Label ID="lblRecArea" Text='<%#Bind("AREA") %>' runat="server" />
                                                </ItemTemplate>
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
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btncheckallpcsqc" runat="server" CssClass="buttonnorm" Text="QC Check All Pcs"
                                    OnClick="btncheckallpcsqc_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnprocessrecid" runat="server" />
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                    <asp:ModalPopupExtender ID="Modalpopupext" runat="server" PopupControlID="pnModelPopup"
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
                                        <div style="max-height: 500px; overflow: auto">
                                            <asp:GridView ID="GDQC" CssClass="grid-views" runat="server" OnRowDataBound="GDQC_RowDataBound">
                                                <HeaderStyle CssClass="gvheaders" Font-Size="12px" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <%-- <Columns>--%>
                                                <%--<asp:TemplateField HeaderText="">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="ChkqcAllItem" runat="server" onclick="return CheckAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chkqcboxitem" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stock No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstockno" Text="text" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Parameter">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblparameter" Text="text" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="value">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chkparamvalue" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                <%--</Columns>--%>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
