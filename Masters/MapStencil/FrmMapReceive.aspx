<%@ Page Title="Map Receive" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmMapReceive.aspx.cs" Inherits="Masters_MapStencil_FrmMapReceive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/Fixfocus.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmMapReceive.aspx";
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
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <%--  <td id="TDEditReceiveNo" runat="server" visible="false">
                                <asp:Label ID="Label8" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtEditReceiveNo" CssClass="textb" Width="61px" runat="server" Enabled="true" AutoPostBack="true" 
                                OnTextChanged="txtEditReceiveNo_TextChanged" />
                            </td>--%>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtIssueNo" CssClass="textb" Width="61px" runat="server" Enabled="true"
                                    AutoPostBack="true" OnTextChanged="txtIssueNo_TextChanged" />
                            </td>
                            <td id="TDCustomerCode" runat="server" visible="true">
                                <asp:Label ID="Label5" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDCustomerCode" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Customer Order No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCustomerOrderNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label66" runat="server" Text="Designer Name." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDesignerName" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDDesignerName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDMapStencilType" runat="server" visible="true">
                                <asp:Label ID="Label1" runat="server" Text="Map/Trace Type" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDMapStencilType" runat="server" CssClass="dropdown" Width="150px"
                                    Enabled="true" AutoPostBack="true" OnSelectedIndexChanged="DDMapStencilType_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Text="Map" Selected="True">Map</asp:ListItem>
                                    <asp:ListItem Value="2" Text="Trace">Trace</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceiveDate" CssClass="textb" Width="80px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtReceiveDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <%-- <td>
                                <asp:Label ID="Label8" runat="server" Text="Required Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtRequiredDate" CssClass="textb" Width="80px" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtRequiredDate">
                                </asp:CalendarExtender>
                            </td>--%>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceiveNo" CssClass="textb" Width="61px" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td id="TDIssueNo" runat="server" visible="true">
                                <asp:Label ID="Label7" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDissueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDReceiveNo" runat="server" visible="false">
                                <asp:Label ID="lblReceiveNo" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDReceiveNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td1" colspan="3" runat="server">
                                <asp:Label ID="Label12" runat="server" Text="Remarks" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtRemarks" CssClass="textb" Width="300px" runat="server" />
                            </td>
                            <td id="Td2" colspan="0" runat="server" visible="false">
                                <asp:Label ID="Label8" runat="server" Text="Unit" CssClass="labelbold"></asp:Label><br />
                                 <asp:Label ID="lblMapIssueUnit" runat="server" CssClass="labelbold"></asp:Label>
                            </td>
                            <td id="Td3" colspan="0" runat="server" visible="false">
                                <asp:Label ID="Label11" runat="server" Text="Rate CalcType" CssClass="labelbold"></asp:Label><br />
                                <asp:Label ID="lblMapIssueRateCalcType" runat="server" CssClass="labelbold"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label6" runat="server" Text="Issue Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
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
                                            <%-- <asp:TemplateField HeaderText="Raw Material Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
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
                                            <asp:TemplateField HeaderText="IssueQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMapIssueQty" Text='<%#Bind("MapIssueQty")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Map IssueRate">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblMapIssueRate" Text='<%#Bind("Rate")%>' runat="server" />--%>
                                                 <asp:TextBox ID="txtMapIssueRate" Width="70px" BackColor="Yellow" runat="server" Text='<%#Bind("Rate")%>'
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMapIssueAmount" Text='<%#Bind("Amount")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pre ReceiveQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreReceiveQty" Text='<%#Bind("PreReceiveQty")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMapIssueType" Text='<%#Bind("MapIssueTypeName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Receive Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMapReceiveQty" Width="70px" BackColor="Yellow" runat="server"
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemFinishedId" Text='<%#Bind("ItemFinishedId") %>' runat="server" />
                                                    <asp:Label ID="lblOrderId" Text='<%#Bind("OrderID") %>' runat="server" />
                                                    <asp:Label ID="lblId" Text='<%#Bind("Id") %>' runat="server" />
                                                    <asp:Label ID="lblDetailId" Text='<%#Bind("DetailId") %>' runat="server" />
                                                    <asp:Label ID="lblMapIssueTypeid" Text='<%#Bind("MapIssueType") %>' runat="server" />
                                                     <asp:Label ID="lblRateCalcType" Text='<%#Bind("RateCalcType") %>' runat="server" />
                                                      <asp:Label ID="lblMapIssueArea" Text='<%#Bind("MapArea") %>' runat="server" />
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
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click"
                                    Visible="true" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found.."
                        OnRowDataBound="RowDataBound" OnRowCancelingEdit="gvdetail_RowCancelingEdit"
                        OnRowEditing="gvdetail_RowEditing" OnRowUpdating="gvdetail_RowUpdating" OnRowDeleting="gvdetail_RowDeleting">
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
                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblMapType" Text='<%#Bind("MapIssueTypeName")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Receive Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Text='<%#Bind("ReceiveQty") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiveQty" Text='<%#Bind("ReceiveQty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Receive Rate">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMapReceiveRate" Text='<%#Bind("MapReceiveRate") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMapReceiveRate" Text='<%#Bind("MapReceiveRate") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Receive Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblMapReceiveAmount" Text='<%#Bind("ReceiveAmount")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblhqty" Text='<%#Bind("ReceiveQty") %>' runat="server" />
                                    <asp:Label ID="lblRId" Text='<%#Bind("RId") %>' runat="server" />
                                    <asp:Label ID="lblRDetailId" Text='<%#Bind("RDetailId") %>' runat="server" />
                                    <asp:Label ID="lblId" Text='<%#Bind("Id") %>' runat="server" />
                                    <asp:Label ID="lblDetailId" Text='<%#Bind("DetailId") %>' runat="server" />
                                    <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId") %>' runat="server" />
                                    <asp:Label ID="lblItemFinishedId" Text='<%#Bind("ItemFinishedId") %>' runat="server" />
                                    <asp:Label ID="lblReceiveMapArea" Text='<%#Bind("ReceiveMapArea") %>' runat="server" />
                                    <asp:Label ID="lblReceiveRateCalcType" Text='<%#Bind("ReceiveRateCalcType") %>' runat="server" />
                                    <asp:Label ID="lblMapIssueType" Text='<%#Bind("MapIssueType") %>' runat="server" />
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
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hnReceiveid" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
