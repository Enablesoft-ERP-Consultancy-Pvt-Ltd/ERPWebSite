<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmWarpReceive.aspx.cs" Inherits="Masters_WARP_frmWarpReceive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDDept.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Dept. !!\n";
                    }
                    selectedindex = $("#<%=DDemployee.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Employee. !!\n";
                    }

                    selectedindex = $("#<%=DDIssueno.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select issue No. !!\n";
                    }
                    selectedindex = $("#<%=DDgodown.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Godown Name. !!\n";
                    }
                    var txtpcs = document.getElementById('<%=txtpcs.ClientID %>');
                    if (txtpcs.value == "" || txtpcs.value == "0") {
                        Message = Message + "Please Enter Pcs(Beam) !!\n";
                    }
                    var txtgrossweight = document.getElementById('<%=txtgrossweight.ClientID %>');
                    if (txtgrossweight.value == "" || txtgrossweight.value == "0") {
                        Message = Message + "Please Enter Gross Weight. !!\n";
                    }
                    var txttareweight = document.getElementById('<%=txttareweight.ClientID %>');
                    if (txttareweight.value == "") {
                        Message = Message + "Please Enter Tare Weight. !!\n";
                    }
                    var txtnetweight = document.getElementById('<%=txtnetweight.ClientID %>')
                    if (txtnetweight.value == "" || txtnetweight.value == "0") {
                        Message = Message + "Please Enter Net Weight. !!\n";
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
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function NewForm() {
            window.location.href = "frmWarpReceive.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function WeightTextChanged() {

            var grosswt = document.getElementById('<%=txtgrossweight.ClientID %>');
            var tareweight = document.getElementById('<%=txttareweight.ClientID %>');
            var Netweight = document.getElementById('<%=txtnetweight.ClientID %>');

            Netweight.value = (getNum(parseFloat(grosswt.value)) - getNum(parseFloat(tareweight.value))).toFixed(3);
        }
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
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
        function CheckOne(obj) {
            var isValid = false;
            var j = 0;
            var gridView = document.getElementById("<%=DG.ClientID %>");
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            j = j + 1;
                            if (j > 1) {
                                alert("Please Select Only One Item");
                                inputs[0].checked = false;
                                return false;
                            }
                        }
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        .WordWrap
        {
            width: 100%;
            word-break: break-all;
        }
        .WordBreak
        {
            width: 100px;
            overflow: hidden;
            text-overflow: ellipsis;
        }
    </style>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="Chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="Chkedit_CheckedChanged" />
                            </td>
                            <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="labelbold" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Department" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Process" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProcess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Employee Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDemployee" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="130px" OnSelectedIndexChanged="DDemployee_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIssueNo" runat="server">
                                <asp:Label ID="Label4" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDIssueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDIssueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBeamNo" runat="server" visible="false">
                                <asp:Label ID="Label14" runat="server" Text="Beam No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDBeamNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="130px" OnSelectedIndexChanged="DDBeamNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Beam No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtloomno" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceivedate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtReceivedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Godown Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDgodown" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Remark" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtRemark" CssClass="textb" Width="500px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="Beam Description" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="True"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 500px; width: 950px; overflow: auto"
                                    class="WordWrap">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <%--    <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" OnCheckedChanged="Chkboxitem_CheckedChanged"
                                                        onclick="CheckOne(this)" AutoPostBack="true" Width="10px" />
                                                </ItemTemplate>
                                                <%-- <ItemStyle HorizontalAlign="Center" Width="500px" />--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Beam Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server"
                                                        Width="150px" />
                                                </ItemTemplate>
                                                <%--<ItemStyle Width="550px" />--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLotno" Text='<%#Bind("Lotno")%>' runat="server" Width="250px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTagno" Text='<%#Bind("TagNo") %>' runat="server" Width="250px" />
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Middle" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs(Beam)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpcs" Text='<%#Bind("Pcs")%>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NoofBeamReq.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnoofbeamreq" Text='<%#Bind("noofbeamreq") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="Yellow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Already Received">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblalreadyeReceived" Text='<%#Bind("ReceivedQty") %>' runat="server"
                                                        Width="50px" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="Yellow" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" Width="50px" />
                                                    <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" Width="50px" />
                                                    <asp:Label ID="lblofinishedid" Text='<%#Bind("ofinishedid") %>' runat="server" Width="50px" />
                                                    <asp:Label ID="lblounitid" Text='<%#Bind("ounitid") %>' runat="server" Width="50px" />
                                                    <asp:Label ID="lblosizeflag" Text='<%#Bind("osizeflag") %>' runat="server" Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr id="TRWeight" runat="server" visible="false">
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmaterialissued" Text="Total Material Issued(Kg.)" runat="server"
                                                ForeColor="Red" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="txtmaterialissued" Width="135px" Enabled="false" runat="server"
                                                CssClass="textb" BackColor="LightGray" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" Text="Total Weight Recd.(Kg.)" runat="server" ForeColor="Red"
                                                CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="txtweightrecd" Width="135px" Enabled="false" runat="server" CssClass="textb"
                                                BackColor="LightGray" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" CssClass="labelbold" Text="Pcs(Beam)" runat="server" />
                                            <br />
                                            <asp:TextBox ID="txtpcs" Width="100px" CssClass="textb" runat="server" onkeypress="return isNumberKey(event);" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" CssClass="labelbold" Text="M/C No." runat="server" />
                                            <br />
                                            <asp:TextBox ID="txtm_cno" Width="100px" CssClass="textb" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblgrosswt" CssClass="labelbold" Text="Gross Weight(kg.)" runat="server" />
                                            <br />
                                            <asp:TextBox ID="txtgrossweight" Width="100px" CssClass="textb" runat="server" onchange="javascript: WeightTextChanged();" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" CssClass="labelbold" Text="Tare Weight(kg.)" runat="server" />
                                            <br />
                                            <asp:TextBox ID="txttareweight" Width="100px" CssClass="textb" runat="server" onchange="javascript: WeightTextChanged();" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" CssClass="labelbold" Text="Net Weight(kg.)" runat="server" />
                                            <br />
                                            <asp:TextBox ID="txtnetweight" Width="100px" CssClass="textb" runat="server" onchange="javascript: WeightTextChanged();"
                                                Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
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
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 500px; overflow: auto">
                                    <asp:GridView ID="DGBeam" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnRowEditing="DGBeam_RowEditing" OnRowCancelingEdit="DGBeam_RowCancelingEdit"
                                        OnRowUpdating="DGBeam_RowUpdating" OnRowDeleting="DGBeam_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Beam Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("BeamDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="350px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs(Beam)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpcs" Text='<%#Bind("Pcs")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gross Wt(Kg.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrossweight" Text='<%#Bind("grossweight") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtgrossweight" Text='<%#Bind("grossweight") %>' runat="server"
                                                        Width="70px" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tare Wt(Kg.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltareweight" Text='<%#Bind("Tareweight") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txttareweight" Text='<%#Bind("Tareweight") %>' runat="server" Width="70px" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Net Wt(Kg.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnetweight" Text='<%#Bind("Netweight") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                    <asp:Label ID="lblbeamno" Text='<%#Bind("loomno") %>' runat="server" />
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
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
