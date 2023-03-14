<%@ Page Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmIssueToDepartment.aspx.cs" Inherits="Masters_Loom_FrmIssueToDepartment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            if (document.getElementById('CPH_Form_chkEdit') == null) {
                window.location.href = "FrmIssueToDepartment.aspx";
            }
            else if (document.getElementById('CPH_Form_chkEdit').disabled == true && document.getElementById('CPH_Form_chkEdit').checked == true) { //here
                window.location.href = "FrmIssueToDepartment.aspx?ForEdit=1";
            } else {
                window.location.href = "FrmIssueToDepartment.aspx";
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
        function cancelvalidation() {
            var Message = "";
            var selectedindex = $("#<%=DDIssueNo.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Folio No!!\n";
            }
            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
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
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDDepartmentName.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Department Name. !!\n";
            }
            selectedindex = $("#<%=ddunit.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Unit Name. !!\n";
            }
            if (document.getElementById("<%=TDIssueNo.ClientID %>")) {
                selectedindex = $("#<%=DDIssueNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Folio No. !!\n";
                }
            }
            var txtissuedate = document.getElementById('<%=txtissuedate.ClientID %>');
            if (txtissuedate.value == "") {
                Message = Message + "Please Enter Issue Date. !!\n";
            }
            var txttargetdate = document.getElementById('<%=txttargetdate.ClientID %>');
            if (txttargetdate.value == "") {
                Message = Message + "Please Enter Target Date. !!\n";
            }
            selectedindex = $("#<%=DDcustcode.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Buyer. !!\n";
            }
            selectedindex = $("#<%=DDorderNo.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Order No. !!\n";
            }
            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
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
                    <div style="width: 100%; float: left;">
                        <table>
                            <tr>
                                <td id="TDIssueNoEdit" runat="server" visible="false">
                                    <asp:Label CssClass="labelbold" Text="Issue No." runat="server" />
                                    <br />
                                    <asp:TextBox ID="TxtIssueNoEdit" CssClass="textb" Width="80px" runat="server" AutoPostBack="true"
                                        OnTextChanged="TxtIssueNoEdit_TextChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdstyle">
                                    <asp:Label ID="Label33" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                                    &nbsp;<asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdstyle">
                                    <asp:Label ID="Label1" runat="server" Text="Department Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDDepartmentName" Width="150" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDDepartmentName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDIssueNo" runat="server" visible="false">
                                    <asp:Label ID="Label7" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDIssueNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="150px" OnSelectedIndexChanged="DDIssueNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="TxtIssueNo" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblunit" CssClass="labelbold" Text="Unit" runat="server" />
                                <br />
                                <asp:DropDownList ID="ddunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblCalType" CssClass="labelbold" Text="Cal Type" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDCalType" runat="server" CssClass="dropdown">
                                    <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                    <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Target Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttargetdate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal2" TargetControlID="txttargetdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 60%; float: left;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Buyer" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="150px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Order No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDorderNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="150px" OnSelectedIndexChanged="DDorderNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="Order Description" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 300px; overflow: auto">
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
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="400px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty. Required">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblqtyreq" Text='<%#Bind("Qtyrequired") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ordered Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderedQty" Text='<%#Bind("orderedqty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pending Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPendingQty" runat="server" Text='<%#Convert.ToInt32(Eval("BalToIssue")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty." Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtIssueQty" Width="70px" BackColor="Yellow" runat="server" Text='<%#Convert.ToInt32(Eval("BalToIssue")) %>'
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
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
                                <asp:CheckBox ID="ChkForDyeingConsumption" runat="server" Text="Check For Dyeing Consumption"
                                    class="tdstyle" Visible="false" CssClass="checkboxbold" />
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="BtnUpdateConsumption" runat="server" Text="Update Consumption" Visible="false"
                                    CssClass="buttonnorm" OnClick="BtnUpdateConsumption_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
                <div>
                    <div style="width: 60%; max-height: 250px; overflow: auto; float: left">
                        <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                            EmptyDataText="No Records found." OnRowEditing="DGOrderdetail_RowEditing" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit"
                            OnRowUpdating="DGOrderdetail_RowUpdating" OnRowDataBound="DGOrderdetail_RowDataBound">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <Columns>
                                <asp:TemplateField HeaderText="OrderDescription">
                                    <ItemTemplate>
                                        <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="350px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblqty" Text='<%#Bind("Qty") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtqty" Text='<%#Bind("Qty") %>' Width="50px" runat="server" onkeypress="return isNumberKey(event);" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblhqty" Text='<%#Bind("Qty") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtrategrid" Width="70px" Text='<%#Bind("Rate") %>' runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Area" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArea" Text='<%#Bind("Area") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblissueorderid" Text='<%#Bind("Issueorderid") %>' runat="server" />
                                        <asp:Label ID="lblissuedetailid" Text='<%#Bind("issue_Detail_Id") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField EditText="Edit" ShowEditButton="true" CausesValidation="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                            OnClick="lnkdelClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
