<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GodownTransfer.aspx.cs" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Inherits="Masters_RawMaterial_GodownTransfer" %>

<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "GodownTransfer.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
    </script>
    <div>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtgdid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text="Company Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddlcompany" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="ProdCode" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtProdCode" runat="server" AutoPostBack="True" Height="18px" OnTextChanged="TxtProdCode_TextChanged"
                                Width="76px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlcatagoryname" runat="server" Width="115px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlcatagoryname_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="ItemName" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlitemname" runat="server" Width="115px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlitemname_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" Text=" Unit" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddlunit" runat="server" Width="115px" AutoPostBack="true" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" runat="server" align="center" visible="false" class="tdstyle">
                            <asp:Label ID="Label3" Text="Recieve Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtdate" runat="server" TabIndex="5" Width="68px" CssClass="textb"
                                Height="18px"></asp:TextBox>
                            <%-- <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtdate"></asp:CalendarExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td id="ql" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="dsn" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="clr" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="shp" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="115px" TabIndex="15" CssClass="dropdown"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShasdeColor" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="LblShadeColor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddShadeColor" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text=" From Godown" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddfromgodown" runat="server" Width="115px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddfromgodown_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text="To Godown" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddtogodown" runat="server" Width="115px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" Text=" Quantity" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtqty" runat="server" Width="77px" Height="21px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left" class="tdstyle">
                            <asp:Label ID="lblcode" runat="server" Text="ProdCode doesnot exist" ForeColor="Red"
                                Visible="false" CssClass="labelbold"></asp:Label>
                            <asp:Label ID="lblqty" runat="server" Text="Transfer quantity is greater than godownstock"
                                ForeColor="Red" Visible="false" CssClass="labelbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="buttonnorm" />
                            <asp:Button ID="btnpriview" runat="server" Text="Preview" OnClientClick="return priview();"
                                CssClass="buttonnorm preview_width" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <br />
                            <asp:GridView ID="gvdetail" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                OnPageIndexChanging="gvdetail_PageIndexChanging" OnRowDataBound="gvdetail_RowDataBound"
                                OnSelectedIndexChanged="gvdetail_SelectedIndexChanged" PageSize="8" DataKeyNames="gdtranid"
                                CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="Category_Name" HeaderText="Category Name" />
                                    <asp:BoundField DataField="Item_name" HeaderText="Item" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="GodownName" HeaderText="Godown Name" />
                                    <asp:BoundField DataField="qtyinhand" HeaderText="Qty Inhand" Visible="False" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Transfer Quantity" />
                                </Columns>
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
