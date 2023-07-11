<%@ Page Title="Yarn opening Issue" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmyarnopeningIssueNew.aspx.cs" Inherits="Masters_YarnOpening_frmyarnopeningIssueNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmyarnopeningissuenew.aspx";
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
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    var Dept = document.getElementById('<%=DDdept.ClientID %>');
                    if (Dept != null) {
                        selectedindex = $("#<%=DDdept.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Department Name!!\n";
                        }
                    }
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDvendor.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Dept. !!\n";
                    }
                    selectedindex = $("#<%=DDcustcode.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Customer code !!\n";
                    }
                    selectedindex = $("#<%=DDorderNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Order No. !!\n";
                    }
                    selectedindex = $("#<%=DDitemdescription.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Item Description. !!\n";
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
    <script type="text/javascript" language="javascript">
        jQuery(function ($) {
            var focusedElementSelector = "";
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(function (source, args) {
                var fe = document.activeElement;
                focusedElementSelector = "";

                if (fe != null) {
                    if (fe.id) {
                        focusedElementSelector = "#" + fe.id;
                    } else {
                        // Handle Chosen Js Plugin
                        var $chzn = $(fe).closest('.chosen-container[id]');
                        if ($chzn.size() > 0) {
                            focusedElementSelector = '#' + $chzn.attr('id') + ' input[type=text]';
                        }
                    }
                }
            });

            prm.add_endRequest(function (source, args) {
                if (focusedElementSelector) {
                    $(focusedElementSelector).focus();
                }
            });
        });
    </script>
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('Div1') != null) {
                xPos = $get('Div1').scrollLeft;
                yPos = $get('Div1').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('Div1') != null) {
                $get('Div1').scrollLeft = xPos;
                $get('Div1').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <asp:UpdatePanel ID="R" runat="server">
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
                            <td>
                                <asp:CheckBox ID="Chkedit" Text="For Edit" CssClass="checkboxbold" AutoPostBack="true"
                                    runat="server" OnCheckedChanged="Chkedit_CheckedChanged" />
                                <br />
                            </td>
                            <td id="TRempcodescan" runat="server" visible="false">
                                <asp:Label ID="Label6" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td id="TDdept" runat="server" visible="false">
                                <asp:DropDownList ID="DDdept" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDdept_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="EWay Bill No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtEWayBillNo" CssClass="textb" Width="150px" runat="server" Enabled="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblyarnopendept" runat="server" Text="Yarn Opening Dept." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDvendor" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDvendor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDcustcode" runat="server">
                                <asp:Label ID="Label15" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDorderNo" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Order No" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDorderNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDorderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDItemDescription" runat="server">
                                <asp:Label ID="Label5" runat="server" Text="Item Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDitemdescription" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="300px" OnSelectedIndexChanged="DDitemdescription_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIssuedNo" runat="server" visible="false">
                                <asp:Label ID="Label14" runat="server" Text="Issued No." CssClass="labelbold"></asp:Label>
                                <asp:CheckBox ID="chkforComp" Text="For Complete" CssClass="checkboxbold" AutoPostBack="true"
                                    runat="server" OnCheckedChanged="chkforComp_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="DDissuedNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissuedNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissueno" CssClass="textb" Width="100px" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Target Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttargetdate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="cal2" TargetControlID="txttargetdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label18" runat="server" Text="Issue Details ( * Mandatory Fields)"
                            CssClass="labelbold" ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 400px; overflow: auto">
                                    <asp:GridView ID="GVItemDetails" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnRowDataBound="GVItemDetails_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <%--  <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemDescription">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit *">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddunitgrid" CssClass="dropdown" Width="70px" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown *">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddgodowngrid" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDgodowngrid_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No. *">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlotnogrid" CssClass="dropdown" Width="120px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDLotnogrid_SelectedIndexChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No. *">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddtagnogrid" CssClass="dropdown" Width="120px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDTagnogrid_SelectedIndexChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstockqty" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reqd. Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblreqdqty" Text='<%#Bind("reqdqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issued Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuedqty" Text='<%#Bind("issuedqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PQty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpqty" runat="server" Text='<%# System.Math.Round(Convert.ToDouble(Eval("reqdqty")) -Convert.ToDouble(Eval("issuedqty")),3) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue. Qty *">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtissueqtygrid" Width="70px" BackColor="Yellow" runat="server"
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddrectypegrid" CssClass="dropdown" runat="server">
                                                        <asp:ListItem Text="Cone" />
                                                        <asp:ListItem Text="Hank" />
                                                        <asp:ListItem Text="" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofconegrid" Width="50px" BackColor="Yellow" runat="server"
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cone Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddconetypegrid" CssClass="dropdown" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ply">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDPlyType" CssClass="dropdown" runat="server">
                                                        <asp:ListItem Text="" />
                                                        <asp:ListItem Text="1 Ply" />
                                                        <asp:ListItem Text="2 Ply" />
                                                        <asp:ListItem Text="3 Ply" />
                                                        <asp:ListItem Text="4 Ply" />
                                                        <asp:ListItem Text="5 Ply" />
                                                        <asp:ListItem Text="6 Ply" />
                                                        <asp:ListItem Text="7 Ply" />
                                                        <asp:ListItem Text="8 Ply" />
                                                        <asp:ListItem Text="9 Ply" />
                                                        <asp:ListItem Text="10 Ply" />
                                                        <asp:ListItem Text="11 Ply" />
                                                        <asp:ListItem Text="12 Ply" />
                                                        <asp:ListItem Text="8-32 Ply" />
                                                        <asp:ListItem Text="30 Ply" />
                                                        <asp:ListItem Text="15 Ply" />
                                                        <asp:ListItem Text="21 Ply" />
                                                        <asp:ListItem Text="13 Ply" />
                                                        <asp:ListItem Text="14 Ply" />
                                                        <asp:ListItem Text="20 Ply" />
                                                        <asp:ListItem Text="28 Ply" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transport">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDTransportType" CssClass="dropdown" runat="server">
                                                        <asp:ListItem Text="" />
                                                        <asp:ListItem Text="Self" />
                                                        <asp:ListItem Text="Company" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate*">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtYarnRate" Width="70px" BackColor="Yellow" runat="server" Text='<%# System.Math.Round(Convert.ToDouble(Eval("rate")),2) %>'
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Machine No*">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIssueMachineNo" Width="70px" BackColor="Yellow" runat="server"
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblitemid" Text='<%#Bind("item_id") %>' runat="server" />
                                                    <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                    <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
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
                <div id="gride" runat="server" style="height: 300px; overflow: auto">
                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        OnRowCancelingEdit="DG_RowCancelingEdit" OnRowEditing="DG_RowEditing" OnRowUpdating="DG_RowUpdating"
                        OnRowDeleting="DG_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="ItemDescription">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit">
                                <ItemTemplate>
                                    <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Godown">
                                <ItemTemplate>
                                    <asp:Label ID="lblGodown" Text='<%#Bind("GodownName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lot No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblLotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tag No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblTagno" Text='<%#Bind("Tagno") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issue Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Width="60px" align="right" runat="server" Text='<%# Bind("IssueQty") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rec Type">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtrectype" Width="50px" runat="server" Text='<%# Bind("Rectype") %>'
                                        BackColor="#FFFF66"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRecType" Text='<%#Bind("Rectype") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No of Cone">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtnoofcone" Width="50px" align="right" runat="server" Text='<%# Bind("Noofcone") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblnoofcone" Text='<%#Bind("noofcone") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cone Type">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtconetype" Width="50px" runat="server" Text='<%# Bind("conetype") %>'
                                        BackColor="#FFFF66"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblconetype" Text='<%#Bind("conetype") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Buyer Order No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblbuyerorderno" Text='<%#Bind("customerorderno") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                    <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                    <asp:Label ID="lbldeptid" Text='<%#Bind("departmentid") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
