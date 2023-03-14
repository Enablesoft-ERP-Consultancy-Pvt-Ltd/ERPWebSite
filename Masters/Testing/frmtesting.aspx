<%@ Page Title="TESTING" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmtesting.aspx.cs" Inherits="Masters_Testing_frmtesting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmtesting.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    if ($("#<%=DDCompanyName.ClientID %>")) {
                        var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                        if (selectedindex < 0) {
                            Message = Message + "Please select Company Name!!\n";
                        }
                    }
                    if ($("#<%=DDJobname.ClientID %>")) {
                        var selectedindex = $("#<%=DDJobname.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Job Name!!\n";
                        }
                    }
                    if ($("#<%=DDPartyName.ClientID %>")) {
                        var selectedindex = $("#<%=DDPartyName.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Party Name!!\n";
                        }
                    }
                    if ($("#<%=DDPONo.ClientID %>")) {
                        var selectedindex = $("#<%=DDPONo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select PO No.!!\n";
                        }
                    }

                    if ($("#<%=ddlrecchalanno.ClientID %>")) {
                        var selectedindex = $("#<%=ddlrecchalanno.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Challan No.!!\n";
                        }
                    }

                    if ($("#<%=DDDescription.ClientID %>")) {
                        var selectedindex = $("#<%=DDDescription.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select ItemDescription.!!\n";
                        }
                    }
                    if ($("#<%=DDtestresult.ClientID %>")) {

                        var selectedindex = $("#<%=DDtestresult.ClientID %>").attr('selectedIndex');
                        if (selectedindex == 2) {
                            var val1 = $("#<%=txtpassby.ClientID %>").attr('value');
                            if (val1 == "") {
                                Message = Message + "Please Fill Pass by!!\n";
                            }
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

                //**********
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
                        <asp:Label ID="lblCompany" Text="Item Description" ForeColor="Red" runat="server"
                            CssClass="labelbold" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Job Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDJobname" runat="server" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDJobname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Supplier Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="PO No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDPONo" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDPONo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Rec. Challan No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="ddlrecchalanno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                    Width="150px" OnSelectedIndexChanged="ddlrecchalanno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="Label1" runat="server" Text="Item Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDescription" runat="server" CssClass="dropdown" AutoPostBack="True"
                                    Width="450px" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblRecQty" Text="Rec. Qty" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtRecQty" CssClass="textb" Width="140px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label2" Text="Test Date" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txttestdate" CssClass="textb" Width="110px" runat="server" />
                                <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txttestdate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDTestNo" runat="server" visible="false">
                                <asp:Label ID="lbltestno" Text="Test No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDtestNo" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDtestNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="Testing Result" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDtestresult" CssClass="dropdown" Width="150px" runat="server">
                                    <asp:ListItem Value="PASS">PASS</asp:ListItem>
                                    <asp:ListItem Value="FAIL">FAIL</asp:ListItem>
                                    <asp:ListItem Value="COND.PASS">COND. PASS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label9" Text="Pass by" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtpassby" CssClass="textb" Width="140px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label3" Text="Testing Report" ForeColor="Red" runat="server" CssClass="labelbold" />
                    </legend>
                    <div style="height: auto; overflow: auto; width: 800px; margin-left: 50px">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="GVDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found..."
                                        OnDataBound="GVDetail_DataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Test" DataField="Category">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="" DataField="Subcategory">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Requirement">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrequirement" Text='<%#bind("Requirement") %>' Style="text-align: center"
                                                        Width="100px" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Received">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtreceived" Text='<%#bind("Received") %>' Width="100px" runat="server"
                                                        Style="text-align: center" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ids" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcategoryId" Text='<%#Bind("CategoryId") %>' runat="server" />
                                                    <asp:Label ID="lblsubcategoryId" Text='<%#Bind("SubCategoryId") %>' runat="server" />
                                                    <%--  <asp:Label ID="lblpindentissueid" Text='<%#Bind("pindentissueid") %>' runat="server" />
                                                    <asp:Label ID="lblpurchasereceiveid" Text='<%#Bind("PurchasereceiveId") %>' runat="server" />
                                                    <asp:Label ID="lbltestid" Text='<%#Bind("testid") %>' runat="server" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblremark" Text="Remark" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtremark" TextMode="MultiLine" Width="550px" Height="60px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <asp:HiddenField ID="hnflagsize" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
