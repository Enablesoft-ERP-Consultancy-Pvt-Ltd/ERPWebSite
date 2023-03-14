<%@ Page Title="Weaver Map Receive" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmMapReceive_OnProductionOrder.aspx.cs" Inherits="Masters_MapStencil_FrmMapReceive_OnProductionOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmMapReceive_OnProductionOrder.aspx";
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
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnMapStencilStockNo.ClientID %>').click();
            }
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd2" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkEdit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td id="TDChallanNo" runat="server" visible="false">
                                <asp:Label ID="Label10" CssClass="labelbold" Text="Receive No." runat="server" />
                                <br />
                                <asp:TextBox ID="txtChallanNo" CssClass="textb" Width="150px" runat="server" AutoPostBack="true"
                                    OnTextChanged="txtChallanNo_TextChanged" />
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDMapStencilType" runat="server" visible="true">
                                <asp:Label ID="Label3" runat="server" Text="Map/Trace Type" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDMapStencilType" runat="server" CssClass="dropdown" Width="150px"
                                    Enabled="true" AutoPostBack="true" OnSelectedIndexChanged="DDMapStencilType_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Text="Map" Selected="True">Map</asp:ListItem>
                                    <asp:ListItem Value="2" Text="Trace">Trace</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceivedate" CssClass="textb" Width="150px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtReceivedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceiveNo" CssClass="textb" Width="61px" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <%-- <td id="TDFolioNo" runat="server" visible="true">
                                <asp:Label ID="Label7" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>                          
                            
                            
                           
                            
                             <td id="TDIssueNo" runat="server" visible="true">
                                <asp:Label ID="Label10" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDIssueNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDIssueNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>  
                             <td>
                                <asp:Label ID="Label8" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtIssueNo" CssClass="textb" Width="61px" runat="server" Enabled="true" />
                            </td>   --%>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" Text="Map/Trace No" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td id="Tdstockno" runat="server" visible="true">
                                <table border="1" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" Text="Enter Map/Trace No." CssClass="labelbold" Font-Size="Small"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMapStencilstockno" CssClass="textb" Height="40px" Width="250px"
                                                runat="server" onKeypress="KeyDownHandler(event);" />
                                            <asp:Button ID="btnMapStencilStockNo" runat="server" Style="display: none" OnClick="txtMapStencilstockno_TextChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found.">
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
                                            <%-- <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="400px" />
                                            </asp:TemplateField> --%>
                                            <asp:TemplateField HeaderText="Qty.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderID" Text='<%#Bind("OrderId") %>' runat="server" />
                                                    <asp:Label ID="lblIssueId" Text='<%#Bind("IssueId") %>' runat="server" />
                                                    <asp:Label ID="lblIssueDetailId" Text='<%#Bind("IssueDetailId") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblIssueOrderID" Text='<%#Bind("IssueOrderID") %>' runat="server" />
                                                    <asp:Label ID="lblMapStencilNo" Text='<%#Bind("MapStencilNo") %>' runat="server" />
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
                                    UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found.."
                        OnRowDataBound="RowDataBound" OnRowDeleting="gvdetail_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>
                            <asp:TemplateField HeaderText="ItemName">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemName" Text='<%#Bind("Item_Name")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QualityName">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Design">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignName" Text='<%#Bind("DesignName")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Color">
                                <ItemTemplate>
                                    <asp:Label ID="lblColorName" Text='<%#Bind("ColorName")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shape">
                                <ItemTemplate>
                                    <asp:Label ID="lblShapeName" Text='<%#Bind("ShapeName")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Size">
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" Text='<%#Bind("Size")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Map/TraceNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblMapTraceNo" Text='<%#Bind("MSStockNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRecId" Text='<%#Bind("RecId") %>' runat="server" />
                                    <asp:Label ID="lblRecDetailId" Text='<%#Bind("RecDetailId") %>' runat="server" />
                                    <asp:Label ID="lblMapStencilNo" Text='<%#Bind("MapStencilNo") %>' runat="server" />
                                    <asp:Label ID="lblItemFinishedID" Text='<%#Bind("ItemFinishedID") %>' runat="server" />
                                    <asp:Label ID="lblIssueId" Text='<%#Bind("IssueId") %>' runat="server" />
                                    <asp:Label ID="lblIssueDetailId" Text='<%#Bind("IssueDetailId") %>' runat="server" />
                                    <asp:Label ID="lblIssueOrderId" Text='<%#Bind("IssueOrderId") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hnRecid" runat="server" />
                <%--<asp:HiddenField ID="hnissueorderid" runat="server" />--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
