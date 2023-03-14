<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmTasselMakingOrder.aspx.cs" Inherits="Masters_TasselMaking_FrmTasselMakingOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CallConfirmBox() {
            if (confirm("Confirm Proceed Further?")) {
                //OK – Do your stuff or call any callback method here..
                alert("You pressed OK!");
            } else {
                //CANCEL – Do your stuff or call any callback method here..
                alert("You pressed Cancel!");
            }
        }
        function NewForm() {
            var LbProcess = document.getElementById('<%=TxtInOtherProcess.ClientID %>');
            if (LbProcess.value == "1") {
                window.location.href = "FrmTasselMakingOrder.aspx?InOtherProcess=1";
            }
            else {
                window.location.href = "FrmTasselMakingOrder.aspx";
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
            var selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
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
            if (document.getElementById("<%=TDUNIT.ClientID %>")) {
                selectedindex = $("#<%=ddunit.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Unit Name. !!\n";
                }
            }
            if (document.getElementById("<%=TDFolioNo.ClientID %>")) {
                selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
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
                <div style="width: 100%; float: left;">
                    <table>
                        <tr>
                            <td id="TDFolioNotext" runat="server" visible="false">
                                <asp:Label CssClass="labelbold" Text="Challan No" runat="server" />
                                <br />
                                <asp:TextBox ID="txtfolionoedit" CssClass="textb" Width="80px" runat="server" AutoPostBack="true"
                                    OnTextChanged="txtfolionoedit_TextChanged" />
                            </td>
                            <td>
                                <asp:TextBox ID="TxtInOtherProcess" CssClass="textb" Width="0px" runat="server" />
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                &nbsp;<asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Employee Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDEmployeeName" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDFolioNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtfoliono" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
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
                            <td id="TDUNIT" runat="server">
                                <asp:Label ID="lblunit" CssClass="labelbold" Text="Unit" runat="server" />
                                <br />
                                <asp:DropDownList ID="ddunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TD1" runat="server">
                                <asp:Label ID="Label8" CssClass="labelbold" Text="Cal Type" runat="server" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDcaltype_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                    <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
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
                <table>
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
                                        <asp:TemplateField HeaderText="Order Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderdescription" Text='<%#Bind("OrderDescription") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="400px" />
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
                                                <asp:Label ID="Label3" runat="server" Text='<%#Convert.ToInt32(Eval("Qtyrequired")) - Convert.ToInt32(Eval("orderedqty")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Qty." Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtloomqty" Width="70px" BackColor="Yellow" runat="server" Text='<%#Convert.ToInt32(Eval("Qtyrequired")) - Convert.ToInt32(Eval("orderedqty")) %>'
                                                    onkeypress="return isNumberKey(event);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtrate" Width="90px" BackColor="Yellow" runat="server" Text='<%#Bind("Rate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemfinishedid" Text='<%#Bind("OFinishedID") %>' runat="server" />
                                                <asp:Label ID="lblOrderDetailID" Text='<%#Bind("OrderDetailID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
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
                                <asp:Button ID="BtnUpdateConsumption" runat="server" Text="Update Consumption" Visible="false"
                                    CssClass="buttonnorm" OnClick="BtnUpdateConsumption_Click" />
                                <asp:Button ID="BtnComplete" runat="server" CssClass="buttonnorm" Text="Complete"
                                    OnClick="BtnComplete_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
                <div>
                    <%--OnRowEditing="DGOrderdetail_RowEditing" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit"
                    OnRowUpdating="DGOrderdetail_RowUpdating"--%>
                    <div style="width: 100%; max-height: 250px; overflow: auto; float: left">
                        <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                            EmptyDataText="No Records found." OnRowDataBound="DGOrderdetail_RowDataBound">
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
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtrategrid" Width="70px" Text='<%#Bind("Rate") %>' runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblissueorderid" Text='<%#Bind("Issueorderid") %>' runat="server" />
                                        <asp:Label ID="lblissuedetailid" Text='<%#Bind("IssueDetailId") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>--%>
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
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" Text="Remarks" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRemarks" runat="server" Width="500px" TextMode="MultiLine" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
