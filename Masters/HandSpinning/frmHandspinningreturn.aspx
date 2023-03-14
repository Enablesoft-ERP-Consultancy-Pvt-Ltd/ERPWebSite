<%@ Page Title="HandSpinning Return" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmHandspinningreturn.aspx.cs" Inherits="Masters_HandSpinning_frmHandspinningreturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmhandspinningreturn.aspx";
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
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDPartyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Vendor Name. !!\n";
                    }
                    selectedindex = $("#<%=DDissueno.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Issue No. !!\n";
                    }

                    selectedindex = $("#<%=DDGodown.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Godown !!\n";
                    }

                    if ($("#<%=TDBinNo.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDBinNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Bin No. !!\n";
                        }
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" ForeColor="Red" CssClass="labelbold" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TREdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="Edit" CssClass="checkboxbold" Font-Size="Small"
                                    AutoPostBack="true" runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td id="TDComplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="Fill Complete Issue No." CssClass="checkboxbold"
                                    Font-Size="Small" AutoPostBack="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td id="TRempcodescan" runat="server" visible="false">
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcompname" Text="Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDPartyName" Width="180px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDissueno" runat="server">
                                <asp:Label ID="lblindentnoedit" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDissueno" Width="130px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDreturnNo" runat="server" visible="false">
                                <asp:Label ID="Label8" runat="server" Text="Return No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDreturnno" Width="130px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDreturnno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Return No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtretNo" CssClass="textb" Width="90px" runat="server" Enabled="false" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Return Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtretdate" CssClass="textb" Width="80px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtretdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" Text="Godown Name" CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label11" Text="Bin No." CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp
                            </td>
                            <td>
                                <asp:Label ID="lbltotalrec" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label1" runat="server" Text="Return Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 400px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views">
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
                                            <asp:TemplateField HeaderText="Godown" Visible="false">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" runat="server">
                                                    </asp:DropDownList>
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
                                            <asp:TemplateField HeaderText="Issued Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblisuedQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Returned Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblreturnedqty" Text='<%#Bind("Returnedqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PQty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpqty" runat="server" Text='<%# System.Math.Round(Convert.ToDouble(Eval("IssueQty")) -Convert.ToDouble(Eval("Returnedqty")),3) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ret. Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtretqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuemasterid" Text='<%#Bind("Id") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("ifinishedid") %>' runat="server" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                                    <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                    <asp:Label ID="lblissuemasterdetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                                    <asp:Label ID="lblRFinishedID" Text='<%#Bind("RFinishedID") %>' runat="server" />
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
                <fieldset>
                    <legend>
                        <asp:Label ID="Label6" runat="server" Text="Returned Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="Div1" runat="server" style="max-height: 300px">
                                    <asp:GridView ID="DGReturnedDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnRowDeleting="DGReturnedDetail_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Return No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrecno" Text='<%#Bind("ReturnNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                                    <asp:Label ID="lblgodown" Text='<%#Bind("GodownName")%>' runat="server" />
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
                                            <asp:TemplateField HeaderText="Ret. Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblretqty" Text='<%#Bind("Returnqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" Text='<%#Bind("id") %>' runat="server" />
                                                    <asp:Label ID="lbldetailid" Text='<%#Bind("detailid") %>' runat="server" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
