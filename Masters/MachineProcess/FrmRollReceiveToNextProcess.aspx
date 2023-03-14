<%@ Page Title="Roll Receive to Latexing" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmRollReceiveToNextProcess.aspx.cs" Inherits="Masters_MachineProcess_FrmRollReceiveToNextProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmRollReceiveToNextProcess.aspx";
        }

        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }

            else {
                return true;
            }
        }
        function Addsize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var Shapeid = 0;
                if (document.getElementById('CPH_Form_ddshape')) {
                    var e = document.getElementById("CPH_Form_ddshape");
                    if (e.options.length > 0) {
                        var Shapeid = e.options[e.selectedIndex].value;
                    }
                }
                e = document.getElementById("CPH_Form_DDUnit");
                var VarUnitID = e.options[e.selectedIndex].value;
                window.open('../Carpet/AddSize.aspx?ShapeID=' + Shapeid + '&UnitID=' + VarUnitID + '', '', 'width=1000px,Height=501px');
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

        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }
        function Validation() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDProcessName.ClientID %>").value <= "0") {
                alert("Pls Select Process Name");
                document.getElementById("<%=DDProcessName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDIssueNo').value <= "0") {
                alert("Please select Issue no....!");
                document.getElementById("CPH_Form_DDIssueNo").focus();
                return false;
            }
            if (document.getElementById("<%=TxtReceiveDate.ClientID %>").value == "") {
                alert("Pls Select Issue Date");
                document.getElementById("<%=TxtReceiveDate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=Td3.ClientID %>")) {
                if (document.getElementById("<%=DDReceiveNo.ClientID %>")) {
                    if (document.getElementById('CPH_Form_DDReceive').value <= "0") {
                        alert("Please Select Receive No....!");
                        document.getElementById("CPH_Form_DDReceive").focus();
                        return false;
                    }
                }
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="Tr1" runat="server">
                        <td id="Td1" colspan="2" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td4" class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text=" Process Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                OnCheckedChanged="ChKForEdit_CheckedChanged" AutoPostBack="true" />
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label17" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDIssueNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDIssue_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td3" runat="server" visible="false">
                            <asp:Label ID="Label3" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDReceiveNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" class="tdstyle">
                            <asp:Label ID="Label4" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtReceiveDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtReceiveDate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="Td6" class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text=" Receive No." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtReceiveNo" Width="100px" runat="server" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div id="gride" runat="server" style="max-height: 400px; overflow: auto">
                                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnRowDataBound="DG_RowDataBound">
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
                                        <asp:TemplateField HeaderText="Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblRollNo" Text='<%#Bind("MainRollID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Main Roll">
                                            <ItemTemplate>
                                                <asp:Label ID="LblMainRollDescription" Text='<%#Bind("MainRollDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSubRollNo" Text='<%#Bind("SubRollID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Roll">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSubRollDescription" Text='<%#Bind("SubRollDescription")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblOrderNo" Text='<%#Bind("OrderNo")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Size">
                                            <ItemTemplate>
                                                <asp:Label ID="LblOrderSize" Text='<%#Bind("OrderSize")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="LblOrderQty" Text='<%#Bind("OrderQty")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtWidth" Width="60px" Text='<%#Bind("Width")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtLength" Width="60px" Text='<%#Bind("Length")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No of Pati">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtNoofPati" Width="60px" Text='<%#Bind("NoofPati")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reject Pcs">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtRejectPcs" Width="60px" Text='<%#Bind("RejectPcs")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblRollIssueToNextID" Text='<%#Bind("RollIssueToNextID") %>' runat="server" />
                                                <asp:Label ID="LblRollIssueToNextDetailID" Text='<%#Bind("RollIssueToNextDetailID") %>'
                                                    runat="server" />
                                                <asp:Label ID="LblRollIssueToNextDetailDetailID" Text='<%#Bind("RollIssueToNextDetailDetailID") %>'
                                                    runat="server" />
                                                <asp:Label ID="LblMaterialReceiveInPcsID" Text='<%#Bind("MainRollID") %>' runat="server" />
                                                <asp:Label ID="LblMaterialReceiveInPcsDetailID" Text='<%#Bind("SubRollID") %>' runat="server" />
                                                <asp:Label ID="LblMainRollFinishedID" Text='<%#Bind("MainRollFinishedID") %>' runat="server" />
                                                <asp:Label ID="LblOrderID" Text='<%#Bind("OrderID") %>' runat="server" />
                                                <asp:Label ID="LblItem_Finished_ID" Text='<%#Bind("Item_Finished_ID") %>' runat="server" />
                                                <asp:Label ID="LblUnitID" Text='<%#Bind("UnitID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td colspan="8" align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew()"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return Validation();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                OnClick="btnPreview_Click" Visible="false" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div id="Div1" runat="server" style="max-height: 400px; overflow: auto">
                                <asp:GridView ID="gvdetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnRowDataBound="gvdetail_RowDataBound" OnRowDeleting="gvdetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Main Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblMainRollNo" Text='<%#Bind("MainRollID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Main Roll Description">
                                            <ItemTemplate>
                                                <asp:Label ID="LblMainRollDescription" Text='<%#Bind("MainRollDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSubRollNo" Text='<%#Bind("SubRollID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Roll Description">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSubRollDescription" Text='<%#Bind("SubRollDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Sub Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSubSubRollNo" Text='<%#Bind("SubSubRollID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Sub Roll Description">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSubSubRollDescription" Text='<%#Bind("SubSubRollDescription") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblOrderNo" Text='<%#Bind("OrderNo")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="LblQty" Text='<%#Bind("Qty")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reject Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="LblRejectQty" Text='<%#Bind("RejectQty")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Delete Row ?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblRollReceiveToNextID" Text='<%#Bind("RollReceiveToNextID") %>' runat="server" />
                                                <asp:Label ID="LblRollReceiveToNextDetailID" Text='<%#Bind("RollReceiveToNextDetailID") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="HnReceiveID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
