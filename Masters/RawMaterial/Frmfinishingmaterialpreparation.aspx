<%@ Page Title="Finishing Material Preparation" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="Frmfinishingmaterialpreparation.aspx.cs" Inherits="Masters_RawMaterial_Frmfinishingmaterialpreparation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "Frmfinishingmaterialpreparation.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
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
        function validateshow() {

            var Message = "";
            var selectedindex = $("#<%=DDCompanyname.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDunit.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Unit. !!\n";
            }

            selectedindex = $("#<%=DDjobname.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Job Name. !!\n";
            }

            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
            }

        }
        function validatesave() {

            var Message = "";
            var selectedindex = $("#<%=DDCompanyname.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDunit.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Unit. !!\n";
            }

            selectedindex = $("#<%=DDjobname.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Job Name. !!\n";
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table border="1" cellpadding="10" cellspacing="5" style="width: 100%">
                <tr>
                    <td style="border: 0">
                        &nbsp
                    </td>
                    <td style="border: 0">
                        &nbsp
                    </td>
                    <td id="TDcheckedit" runat="server" visible="false">
                        <asp:CheckBox ID="chkedit" CssClass="checkboxbold" Text="For Edit" runat="server"
                            AutoPostBack="true" Font-Size="Small" OnCheckedChanged="chkedit_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label Text="Company Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDCompanyname" CssClass="dropdown" runat="server" Width="95%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="Label1" Text="Unit Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDunit" CssClass="dropdown" runat="server" Width="95%">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="Label2" Text="Job Name" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDjobname" CssClass="dropdown" runat="server" Width="95%" AutoPostBack="true"
                            OnSelectedIndexChanged="DDjobname_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20%" id="TDissueno" runat="server" visible="false">
                        <asp:Label ID="Label5" Text="Issue No." runat="server" CssClass="labelbold" />
                        <br />
                        <asp:DropDownList ID="DDissueno" CssClass="dropdown" runat="server" Width="95%" AutoPostBack="true"
                            OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="Label3" Text="Date" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtdate" CssClass="textb" runat="server" Width="95%" />
                        <asp:CalendarExtender ID="caldate" Format="dd-MMM-yyyy" TargetControlID="txtdate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="Label4" Text="Issue No." runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txtissueno" CssClass="textb" runat="server" Width="95%" Enabled="false" />
                    </td>
                    <td style="width: 10%">
                        <br />
                        <asp:Button ID="btnshow" runat="server" CssClass="buttonnorm" Text="Show Detail"
                            OnClick="btnshow_Click" UseSubmitBehavior="false" OnClientClick="if (!validateshow())return; this.disabled=true;this.value = 'wait ...';" />
                    </td>
                </tr>
            </table>
            <table border="1" cellpadding="0" cellspacing="5" style="width: 100%">
                <tr>
                    <td>
                        <div style="max-height: 400px; overflow: auto">
                            <asp:GridView ID="DGDetail" CssClass="grid-views" AutoGenerateColumns="false" EmptyDataText="No records fetched for this Combination."
                                runat="server">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkheader" Text="" runat="server" onclick="return CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkitem" Text="" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemdesc" Text='<%#Bind("itemdesc") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                            <asp:Label ID="lblissuedetailid" Text='<%#Bind("Issue_Detail_Id") %>' runat="server" />
                                            <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
                                            <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                            <asp:Label ID="lblwidth" Text='<%#Bind("width") %>' runat="server" />
                                            <asp:Label ID="lbllength" Text='<%#Bind("length") %>' runat="server" />
                                            <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label Text="Total Pcs" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txttotalpcs" CssClass="textb" Width="90px" runat="server" Enabled="false"
                            BackColor="LightYellow" />
                    </td>
                    <td>
                        <asp:Label ID="Label6" Text="Total Area" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txttotalarea" CssClass="textb" Width="90px" runat="server" Enabled="false"
                            BackColor="LightYellow" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 70%">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Font-Size="Small" Text=""
                            runat="server" />
                    </td>
                    <td align="right" style="width: 30%">
                        <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm();" />
                        <asp:Button Text="Save" ID="btnsave" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click"
                            UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                        <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        <asp:Button Text="Preview" ID="btnpreview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div style="max-height: 400px; overflow: auto">
                            <asp:Label ID="lblissue" Text="ISSUED DETAIL" CssClass="labelbold" runat="server"
                                Visible="false" />
                            <asp:GridView ID="DGIssueDetail" CssClass="grid-views" AutoGenerateColumns="false"
                                EmptyDataText="No records fetched for this Combination." runat="server">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemdesc" Text='<%#Bind("itemdesc") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                            <asp:Label ID="lblissuedetailid" Text='<%#Bind("Issue_Detail_Id") %>' runat="server" />
                                            <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                                OnClick="lnkdelClick"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td style="width: 50%" valign="top">
                        <div style="max-height: 400px; overflow: auto">
                            <asp:Label ID="lblconsmp" Text="CONSUMPTION DETAIL" CssClass="labelbold" runat="server"
                                Visible="false" />
                            <asp:GridView ID="DGConsmpdetail" CssClass="grid-views" AutoGenerateColumns="false"
                                EmptyDataText="No records fetched for this Combination." runat="server">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemdesc" Text='<%#Bind("itemdesc") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblqty" Text='<%#Bind("Qty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblunitname" Text='<%#Bind("Unitname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
