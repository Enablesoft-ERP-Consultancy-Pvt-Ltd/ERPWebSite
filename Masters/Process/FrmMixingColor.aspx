<%@ Page Title="MIX COLOR" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmMixingColor.aspx.cs" Inherits="Masters_Process_FrmMixingColor" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validation() {

            if (document.getElementById("<%=ddcompanyname.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcompanyname').value <= "0") {
                    alert("Please select Company Name....!");
                    document.getElementById("CPH_Form_ddcompanyname").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddprocessname.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddprocessname').value <= "0") {
                    alert("Please select Process Name....!");
                    document.getElementById("CPH_Form_ddprocessname").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=ddgodownname.ClientID %>").value <= "0") {
                alert("Pls Select GodownName");
                document.getElementById("<%=ddgodownname.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlotno.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddlotno').value <= "0") {
                    alert("Please Select LotNo.....!");
                    document.getElementById("CPH_Form_ddlotno").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddcategoryname.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcategoryname').value <= "0") {
                    alert("Please Select CategoryName....!");
                    document.getElementById("CPH_Form_ddcategoryname").focus();
                    return false;
                }
            }
           
           
            if (document.getElementById("<%=dditemname.ClientID %>")) {
                if (document.getElementById('CPH_Form_dditemname').value <= "0") {
                    alert("Please Select ItemName.....!");
                    document.getElementById("CPH_Form_dditemname").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddqualityname.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddqualityname').value <= "0") {
                    alert("Please Select QualityName....!");
                    document.getElementById("CPH_Form_ddqualityname").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddcategoryname1.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcategoryname1').value <= "0") {
                    alert("Please Select CategoryName for Mix With Color....!");
                    document.getElementById("CPH_Form_ddcategoryname1").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=dditemname1.ClientID %>")) {
                if (document.getElementById('CPH_Form_dditemname1').value <= "0") {
                    alert("Please Select Item Name for Mix With Color....!");
                    document.getElementById("CPH_Form_dditemname1").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddqualityname1.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddqualityname1').value <= "0") {
                    alert("Please Select Quality Name for Mix With Color....!");
                    document.getElementById("CPH_Form_ddqualityname1").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddcolorname.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcolorname').value <= "0") {
                    alert("Please Select Color Name for Mix With Color....!");
                    document.getElementById("CPH_Form_ddcolorname").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=dddesign1.ClientID %>")) {
                if (document.getElementById('CPH_Form_dddesign1').value <= "0") {
                    alert("Please Select Design Name....!");
                    document.getElementById("CPH_Form_dddesign1").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddshapename.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddshapename').value <= "0") {
                    alert("Please Select Shape Name for Mix With Color....!");
                    document.getElementById("CPH_Form_ddshapename").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddsize.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddsize').value <= "0") {
                    alert("Please Select Size for Mix With Color....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddshadecolor.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddshadecolor').value <= "0") {
                    alert("Please Select Shade Color for Mix With Color....!");
                    document.getElementById("CPH_Form_ddshadecolor").focus();
                    return false;
                }
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
    </script>

    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 100px;">
                <table>
                    <tr>
                        <td>
                            Company Name<br />
                            <asp:DropDownList ID="ddcompanyname" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Process Name<br />
                            <asp:DropDownList ID="ddprocessname" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Godown Name<br />
                            <asp:DropDownList ID="ddgodownname" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddgodownname_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Lot No.<br />
                            <asp:DropDownList ID="ddlotno" runat="server" Width="150px"  AutoPostBack="true"
                                CssClass="dropdown" onselectedindexchanged="ddlotno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="margin-left: 100px;">
                <table>
                    <tr>
                        <td>
                            Category Name<br />
                            <asp:DropDownList ID="ddcategoryname" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Item Name<br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Quality Name<br />
                            <asp:DropDownList ID="ddqualityname" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddqualityname_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="margin-left: 100px; height: 250px; overflow: scroll; width: 500px">
                <b style="color: Red;">Shade Color Details </b>
                <asp:GridView ID="gvmixingcolor" AutoGenerateColumns="False" runat="server" CssClass="grid-view">
                    <HeaderStyle CssClass="gvheader" Height="25px" />
                    <AlternatingRowStyle CssClass="gvalt" />
                    <RowStyle CssClass="gvrow" Height="20px" />
                    <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkmix" Width="70px" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ShadeColorName" HeaderText="ShadeColor">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StockQty" HeaderText="StockQty">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Mix Qty">
                            <ItemTemplate >
                                <asp:TextBox ID="txtmixQty" runat="server" Width="75px" style="text-align:center" ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblshadecolorid" runat="server" Text='<%#Bind("ShadeColorId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblitemfinishedid" runat="server" Text='<%#Bind("Item_Finished_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblgodownid" runat="server" Text='<%#Bind("GodownId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCompanyid" runat="server" Text='<%#Bind("CompanyId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLotno" runat="server" Text='<%#Bind("LotNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="height: 30px; margin-left: 100px; width: 500px; background: teal; padding-top: 10px">
                &nbsp;
                <asp:Label ID="lbl1" runat="server" Text="Mix With Color" ForeColor="White" Font-Bold="true"></asp:Label>
            </div>
            <div style="margin-left: 100px;">
                <table>
                    <tr>
                        <td id="TDProdcode" runat="server" visible="false">
                            Product Code<br />
                            <asp:TextBox ID="TxtProdCode" runat="server" Width="100px" CssClass="textb" Visible="false">
                            </asp:TextBox>
                        </td>
                        <td>
                            Category Name<br />
                            <asp:DropDownList ID="ddcategoryname1" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddcategoryname1_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Item Name<br />
                            <asp:DropDownList ID="dditemname1" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="dditemname1_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TdQuality" runat="server" visible="false">
                            Quality Name<br />
                            <asp:DropDownList ID="ddqualityname1" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="TdColor" visible="false">
                            Color Name<br />
                            <asp:DropDownList ID="ddcolorname" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="TdDesign" visible="false">
                            Design<br />
                            <asp:DropDownList ID="dddesign1" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TdShape" runat="server" visible="false">
                            Shape Name<br />
                            <asp:DropDownList ID="ddshapename" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TdSize" runat="server" visible="false">
                            Size<br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="TdColorShade" visible="false">
                            Shade Color<br />
                            <asp:DropDownList ID="ddshadecolor" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="margin-left: 100px;">
                <table width="500px">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnsave" CssClass="buttonnorm" Width="50px" runat="server" Text="Save"
                                OnClick="btnsave_Click" OnClientClick="return Validation();" />&nbsp;
                            <asp:Button ID="btnclose" CssClass="buttonnorm" Width="50px" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:Label ID="lblmsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnMixId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
