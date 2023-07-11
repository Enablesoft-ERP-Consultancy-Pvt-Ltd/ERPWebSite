<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DirectStockDestini.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_RawMaterial_DirectStockDestini" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "DirectStockDestini.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 750px; width: 900px">
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtstockid" runat="server" Visible="false" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprefix" runat="server" Visible="false" AutoPostBack="True" OnTextChanged="txtprefix_TextChanged"
                                Width="85px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="Txtpostfix" runat="server" Visible="false" Width="85px"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck"
                                Type="Integer" ControlToValidate="Txtpostfix" Text="Text must be an integer."
                                ForeColor="Red" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Company Name<br />
                            <asp:DropDownList ID="ddlcompany" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategorytype" runat="server" Text="Catagory Type"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlcatagorytype" runat="server" Width="115px" OnSelectedIndexChanged="ddlcatagorytype_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                                <asp:ListItem Value="-1">--SELECT--</asp:ListItem>
                                <asp:ListItem Value="1">RAW MATERIAL</asp:ListItem>
                                <asp:ListItem Value="0">FINISHED ITEM</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" align="center" class="tdstyle">
                            Issue Date<br />
                            <asp:TextBox ID="txtdate" runat="server" TabIndex="5" Width="115px" AutoPostBack="True"
                                CssClass="textb"></asp:TextBox>
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="code" runat="server" class="tdstyle">
                            ProdCode<br />
                            <asp:TextBox ID="TxtProdCode" runat="server" AutoPostBack="True" OnTextChanged="TxtProdCode_TextChanged"
                                Width="115px" CssClass="textb"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr id="Tr5">
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlcatagoryname" runat="server" Width="115px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlcatagoryname_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="ItemName"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlitemname" runat="server" Width="115px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlitemname_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="ql" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="115px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="dsn" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="115px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="clr" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="115px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="shp" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="115px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="115px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="shd" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddlshade" runat="server" Width="115px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TdFINISHED_TYPE" runat="server" visible="true" class="tdstyle">
                            <asp:Label ID="LblFINISHED_TYPE" runat="server" Text="Finish_Type"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddFINISHED_TYPE" runat="server" Width="115px" CssClass="dropdown"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Godown Name<br />
                            <asp:DropDownList ID="ddlgodown" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            Unit<br />
                            <asp:DropDownList ID="ddlunit" runat="server" Width="115px" AutoPostBack="true" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            Lot No.<br />
                            <asp:TextBox ID="txtlotno" runat="server" Width="115px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Opening Stock<br />
                            <asp:TextBox ID="txtopeningstock" runat="server" Width="115px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td runat="server">
                            <asp:TextBox Visible="false" ID="txtcode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblmessage" runat="server" Text="Data Saved Succsesfully" ForeColor="Red"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblerror" runat="server" Text="Carpet no. Already Exist" ForeColor="Red"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblsave" runat="server" Text="Plz Enter Opening Stock" ForeColor="Red"
                                Visible="false"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label2" runat="server" Text="ProdCode doesnot exist" ForeColor="Red"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnpriview" runat="server" Text="Preview" OnClientClick="return priview();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClick="btnclose_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="8">
                            <div style="overflow: scroll; width: 500px; height: 400px;">
                                <asp:GridView ID="gvcarpetdetail" runat="server" OnRowDataBound="gvcarpetdetail_RowDataBound"
                                    DataKeyNames="stockid" OnSelectedIndexChanged="gvcarpetdetail_SelectedIndexChanged"
                                    AutoGenerateColumns="False" CssClass="grid-view" OnRowCreated="gvcarpetdetail_RowCreated">
                                    <HeaderStyle Font-Bold="true" CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="stockid" HeaderText="stockid" Visible="False" />
                                        <asp:BoundField DataField="Category_Name" HeaderText="Category Name" />
                                        <asp:BoundField DataField="Item_name" HeaderText="Item" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:BoundField DataField="godown" HeaderText="GoDown" />
                                        <asp:BoundField DataField="OpenStock" HeaderText="Opening Stock" />
                                        <asp:BoundField DataField="Qtyinhand" HeaderText="Qty Inhand" />
                                        <%-- <asp:BoundField DataField="StockNos" HeaderText="Carpet NOs" />--%>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
