<%@ Page Title="MATERIAL RECEIVE FROM MACHINE" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmMaterialReceiveFromMachine.aspx.cs" Inherits="Masters_MachineProcess_FrmMaterialReceiveFromMachine" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">

        function NewForm() {
            window.location.href = "FrmMaterialReceiveFromMachine.aspx";
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

                    }
                    else {
                        inputlist[i].checked = false;


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
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDProcessName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process Name. !!\n";
                    }
                    selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Production Unit. !!\n";
                    }
                    selectedindex = $("#<%=DDMachineNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Machine No. !!\n";
                    }
                    var txtissuedate = document.getElementById('<%=txtReceiveDate.ClientID %>');
                    if (txtReceiveDate.value == "") {
                        Message = Message + "Please Enter Issue Date. !!\n";
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
    <div>
        <asp:UpdatePanel ID="upd2" runat="server">
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
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                            </td>
                           <%-- <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="checkboxbold" runat="server" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" Width="150px" >
                                </asp:DropDownList>
                            </td>
                            
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Machine No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDMachineNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDMachineNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDissue" runat="server" visible="true">
                                <asp:Label ID="Label7" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDIssueNo" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDIssueNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceiveNo" CssClass="textb" Width="90px" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtReceiveDate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtReceiveDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:Label ID="Label6" runat="server" Text="Godown" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDGodown" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                            </td>
                            <td id="TDReceiveNo" runat="server" visible="false">
                                <asp:Label ID="Label5" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDReceiveNo" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>               
                <fieldset>
                    <legend>
                        <asp:Label ID="Label8" runat="server" Text="MATERIAL ISSUED DETAIL" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGIssueDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
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
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGodownName" Text='<%#Bind("GodownName") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LotNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Consume Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConsumeQty" Text='<%#Bind("ConsumeQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AlreadyRecQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAlreadyReceivedQty" Text='<%#Bind("AlreadyReceivedQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BalToRecQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalToRecQty" Text='<%#Bind("BalToRecQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReceiveQty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblhqty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                                <asp:Label ID="lblMaterialIssueId" Text='<%#Bind("MaterialIssueID") %>' runat="server" />
                                                <asp:Label ID="lblMaterialIssueDetailId" Text='<%#Bind("MaterialIssueDetailId") %>'
                                                    runat="server" />
                                                <asp:Label ID="lblMachineNoId" Text='<%#Bind("MachineNoId") %>' runat="server" />
                                                <asp:Label ID="lblprocessid" Text='<%#Bind("ProcessId") %>' runat="server" />
                                                <asp:Label ID="lblIssueNo" Text='<%#Bind("IssueNo") %>' runat="server" />
                                                <asp:Label ID="lblUnitId" Text='<%#Bind("UnitId") %>' runat="server" />
                                                <asp:Label ID="lblFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
                                                <asp:Label ID="lblGodownId" Text='<%#Bind("GodownId") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
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
                <fieldset>
                    <legend>
                        <asp:Label ID="Label88" runat="server" Text="MATERIAL RECEIVE DETAIL" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="Div2" runat="server" style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGReceiveDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDeleting="DGReceiveDetail_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemDescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGodownName" Text='<%#Bind("GodownName") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LotNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLotNo" Text='<%#Bind("Lotno") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TagNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiveQty" Text='<%#Bind("ReceiveQty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialReceiveId" Text='<%#Bind("MaterialReceiveId") %>' runat="server" />
                                                    <asp:Label ID="lblMaterialReceiveDetailId" Text='<%#Bind("MaterialReceiveDetailId") %>' runat="server" />
                                                    <asp:Label ID="lblMaterialIssueId" Text='<%#Bind("MaterialIssueId") %>' runat="server" />
                                                    <asp:Label ID="lblFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
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
                </fieldset>
                <asp:HiddenField ID="hnMaterialReceiveId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
