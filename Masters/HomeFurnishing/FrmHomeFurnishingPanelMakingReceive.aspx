<%@ Page Title="PRODUCTION RECEIVE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmHomeFurnishingPanelMakingReceive.aspx.cs"
    Inherits="Masters_HomeFurnishing_FrmHomeFurnishingPanelMakingReceive" %>

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
        function NewForm() {
            window.location.href = "FrmHomeFurnishingPanelMakingReceive.aspx";
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
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(Jscriptvalidate);
                </script>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
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
                                        <td id="TDFolioNotext" runat="server">
                                            <asp:Label ID="Label23" CssClass="labelbold" Text="Challan No." runat="server" />
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
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                            &nbsp;
                                            <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                                OnCheckedChanged="chkedit_CheckedChanged" />
                                            <br />
                                            <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                Width="170px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDreceiveNo" runat="server" visible="false">
                                            <asp:Label ID="Label5" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDreceiveNo" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                                runat="server" OnSelectedIndexChanged="DDreceiveNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
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
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 20%" valign="top">
                                <table width="100%">
                                    <tr>
                                        <td id="TdWeaverName" runat="server">
                                            <asp:Label ID="lblemp" Text="Employee" CssClass="labelbold" runat="server" />
                                            <br />
                                            <div style="overflow: auto; width: 150px">
                                                <asp:ListBox ID="listWeaverName" runat="server" Width="150px" Height="70px" SelectionMode="Multiple"
                                                    Style="overflow: auto"></asp:ListBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label4" Text="Product Description" runat="server" CssClass="labelbold"
                            ForeColor="Red" />
                    </legend>
                    <table width="100%">
                        <tr>
                            <td style="width: 80%; height: 110px" valign="top">
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
                                                    DataKeyNames="IssueDetailID" Width="100%" EmptyDataText="No. Records found.">
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
                                                                <asp:Label ID="lblPendingQty" runat="server" Text='<%#Convert.ToInt32(Eval("orderedqty")) -Convert.ToInt32(Eval("Receivedqty")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rec Qty.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtrecqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight (Kg.)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtweight" Width="70px" BackColor="Yellow" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Penality">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPenality" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="P Remark">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtPenalityRemark" Width="70px" BackColor="Yellow" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItem_Finished_ID" Text='<%#Bind("Item_Finished_ID") %>' runat="server" />
                                                                <asp:Label ID="lbllength" Text='<%#Bind("length") %>' runat="server" />
                                                                <asp:Label ID="lblwidth" Text='<%#Bind("width") %>' runat="server" />
                                                                <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                                                <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                                                                <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                                <asp:Label ID="lblissuedetailid" Text='<%#Bind("IssueDetailId") %>' runat="server" />
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
                                                                EmptyDataText="No. Records found." OnRowDeleting="DGRecDetail_RowDeleting" OnRowDataBound="DGRecDetail_RowDataBound"
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
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblweight" Text='<%#Bind("Weight") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm.Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommrate" Text='<%#Bind("Comm") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpenalitygrid" Text='<%#Bind("penality") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality Remark">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpenalityremarkgrid" Text='<%#Bind("Premarks") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprocessrecid" Text='<%#Bind("ProcessRecId") %>' runat="server" />
                                                                            <asp:Label ID="lblprocessrecdetailid" Text='<%#Bind("ProcessRecDetailId") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
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
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                    Font-Size="Small" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnprocessrecid" runat="server" Value="0" />
                <asp:HiddenField ID="hnunitid" runat="server" Value="0" />
                <asp:HiddenField ID="hncaltype" runat="server" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
