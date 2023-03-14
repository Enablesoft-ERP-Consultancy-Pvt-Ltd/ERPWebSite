<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmLocalConsumptionForOrder.aspx.cs"
    Inherits="Masters_Order_FrmLocalConsumptionForOrder" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Define Local Consumption</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeform() {
            window.opener.document.getElementById('refreshgrid').click();
            window.close();
        }
        function save() {
            if (document.getElementById("<%=TxtQty.ClientID %>").value == "" || document.getElementById("<%=TxtQty.ClientID %>").value == "0") {
                alert("Qty Cannot Be Blank Or Zero");
                document.getElementById("<%=TxtQty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        function AddItum() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddItemName.aspx?' + a3, '', 'Height=400px,width=500px');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
        function AddItemCode() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddItemCode.aspx?' + a3);
        }
        function AddItemCategory() {
            window.open('../Carpet/AddItemCategory.aspx', '', 'Height=400px,width=500px');
        }
        function AddQuality() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddQuality.aspx?' + a3, '', 'Height=500px,width=500px');
        }
        function AddDesign() {
            window.open('../Carpet/AddDesign.aspx', '', 'Height=500px,width=500px');
        }
        function AddColor() {
            window.open('../Carpet/AddColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShadecolor() {
            window.open('../Carpet/AddShadeColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShape() {
            window.open('../Carpet/AddShape.aspx', '', 'Height=400px,width=500px');
        }
        function AddSize() {
            var e = document.getElementById("DDShape");
            var shapeid = e.options[e.selectedIndex].value;
            window.open('../Carpet/AddSize.aspx?shapeid=' + shapeid + '', '', 'Height=500px,width=1000px');
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding: 10px 5px 20px 0px">
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Itemdescrip" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TxtFinishedid" ForeColor="White" BorderStyle="None" Width="0px"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="lblcategoryname" runat="server" Text="Item Category"></asp:Label><br />
                    <asp:DropDownList ID="DDItemCategory" AutoPostBack="true" runat="server" Width="150px"
                        CssClass="dropdown" ValidationGroup="f1" TabIndex="1" OnSelectedIndexChanged="DDItemCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                    <b style="color: Red">&nbsp; *</b>
                    <asp:Button ID="btnadditemcategory" runat="server" align="right" CssClass="buttonsmall"
                        OnClientClick="return AddItemCategory()" Text="ADD" TabIndex="12" />
                    <asp:Button CssClass="refreshcategory" ID="refreshcategory" runat="server" Text=""
                        Style="display: none" OnClick="refreshcategory_Click" />
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDItemName" runat="server" AutoPostBack="True"
                        Width="150" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" ValidationGroup="f1"
                        TabIndex="2">
                    </asp:DropDownList>
                    <asp:Button ID="BtnAdd0" runat="server" align="right" CssClass="buttonsmall" OnClientClick="return AddItum()"
                        Text="ADD" TabIndex="14" />
                </td>
                <td id="TDQuality" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" AutoPostBack="True"
                        Width="150px" TabIndex="3">
                    </asp:DropDownList>
                    <asp:Button ID="btnaddquality" runat="server" align="right" CssClass="buttonsmall"
                        OnClientClick="return AddQuality()" Text="ADD" TabIndex="16" />
                    <asp:Button ID="refreshquality" runat="server" Text="" Style="display: none" OnClick="refreshquality_Click" />
                    <asp:Button ID="fillitemcode" runat="server" Text="" Style="display: none" OnClick="fillitemcode_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td id="TDDesign" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="LblDesignName" runat="server" Text="Design"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="True"
                        Width="150px" TabIndex="4">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmall" OnClientClick="return AddDesign()"
                        Text="ADD" TabIndex="20" />
                    <asp:Button CssClass="refreshdesign" ID="refreshdesign" runat="server" Text="" ForeColor="White"
                        OnClick="refreshdesign_Click" />
                </td>
                <td id="TDColor" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" AutoPostBack="True"
                        Width="150px" TabIndex="5">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="btnaddcolor" runat="server" align="right" CssClass="buttonsmall"
                        OnClientClick="return AddColor()" Text="ADD" TabIndex="22" />
                    <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Text="" Style="display: none"
                        OnClick="refreshcolor_Click" />
                </td>
                <td id="TDShade" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="LblShadeColor" runat="server" Text="Shade Color"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="ddshadecolor" runat="server" AutoPostBack="True"
                        Width="150px" TabIndex="6">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="btnaddshadecolor" runat="server" align="right" CssClass="buttonsmall"
                        OnClientClick="return AddShadecolor()" Text="ADD" TabIndex="24" />
                    <asp:Button CssClass="buttonnorm" ID="refreshshade" runat="server" Text="" Style="display: none"
                        OnClick="refreshshade_Click" />
                </td>
                <td id="TDShape" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="150px" TabIndex="7">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="btnaddshape" runat="server" align="right" CssClass="buttonsmall"
                        OnClientClick="return AddShape()" Text="ADD" TabIndex="26" />
                    <asp:Button CssClass="buttonnorm" ID="refreshshape" runat="server" Text="" Style="display: none"
                        OnClick="refreshshape_Click" />
                </td>
                <td id="TDSize" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                    <asp:CheckBox ID="ChFtSize" Text="FtSize" runat="server" OnCheckedChanged="ChMeteerSize_CheckedChanged"
                        AutoPostBack="True" /><br />
                    <asp:DropDownList CssClass="dropdown" ID="DDSize" runat="server" AutoPostBack="True"
                        Width="150px" TabIndex="8">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddSize" runat="server" align="right" CssClass="buttonsmall" OnClientClick="return AddSize()"
                        Text="ADD" TabIndex="28" />
                    <asp:Button CssClass="buttonnorm" ID="BtnRefreshSize" runat="server" Text="" Style="display: none"
                        OnClick="BtnRefreshSize_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2" class="tdstyle">
                    <asp:Label ID="LblUnitName" runat="server" Text="Unit Name"></asp:Label>
                    &nbsp;
                    <asp:DropDownList CssClass="dropdown" ID="DDUnitName" runat="server" AutoPostBack="True"
                        Width="150px" TabIndex="9">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblperqty" runat="server" Text="Pcs/Qty"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtperqty" runat="server" CssClass="textb" TabIndex="10" BackColor="#FFFF66"
                        onkeypress="return isNumber(event);" Width="75px" OnTextChanged="txtperqty_TextChanged"
                        AutoPostBack="true"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="LblQty" runat="server" Text="Qty"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="TxtQty" runat="server" CssClass="textb" TabIndex="10" BackColor="#FFFF66"
                        onkeypress="return isNumber(event);" Width="75px" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <span than length></span>&nbsp;
                    <asp:TextBox ID="txtthanlength" runat="server" CssClass="textb" TabIndex="10" BackColor="#FFFF66"
                        onkeypress="return isNumber(event);" Width="75px" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="750px">
            <tr>
                <td colspan="2">
                    Remarks
                    <br />
                    <asp:TextBox ID="txtremark" runat="server" CssClass="textb" TextMode="MultiLine"
                        TabIndex="10" Width="250px" runat="server"></asp:TextBox>
                </td>
                <td colspan="2" align="right">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" TabIndex="11"
                        OnClientClick="return save();" CssClass="buttonnorm" />
                    <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return closeform();"
                        TabIndex="12" CssClass="buttonnorm" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="LblErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="750px">
            <tr>
                <td runat="server" visible="false" id="tdsidegrid" align="right">
                    <div style="width: 100%; height: 200px; overflow: auto;">
                        <asp:GridView ID="DGShowConsumption" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                            DataKeyNames="finishedid" OnSelectedIndexChanged="DGShowConsumption_SelectedIndexChanged"
                            OnRowCreated="DGShowConsumption_RowCreated" OnRowDataBound="DGShowConsumption_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Description" HeaderText="Description">
                                    <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <table>
        <tr>
            <td colspan="6">
                <div style="height: 300px; overflow: auto">
                    <asp:GridView ID="DGConsumption" Width="760px" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        OnRowDataBound="DGConsumption_RowDataBound" OnRowDeleting="DGConsumption_RowDeleting"
                        OnSelectedIndexChanged="DGConsumption_SelectedIndexChanged" CssClass="grid-view"
                        OnRowCreated="DGConsumption_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <%--<asp:BoundField DataField="Category" HeaderText="Category">
                                <HeaderStyle Width="75px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Item" HeaderText="Item">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="Description" HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Unit" HeaderText="Unit">
                                <HeaderStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Qty" HeaderText="Qty">
                                <HeaderStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                <HeaderStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ThanLength" HeaderText="ThanLength">
                                <HeaderStyle Width="50px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Do you want to Delete data?')"
                                        CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
