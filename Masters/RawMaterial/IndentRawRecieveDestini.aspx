<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndentRawRecieveDestini.aspx.cs"
    EnableEventValidation="false" MasterPageFile="~/ERPmaster.master" Inherits="Masters_RawMaterial_ProcessRawRecieve" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function validate() {
            if (document.getElementById("<%=ddgodown.ClientID %>").value == "0") {
                alert("Please select Godown");
                document.getElementById("<%=ddgodown.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddCompName.ClientID %>").value == "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px">
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnl1" runat="server">
                    <table>
                        <tr id="Tr1" runat="server">
                            <td id="tdIndentNo" runat="server" visible="false" class="tdstyle">
                                Challan No.<br />
                                <asp:TextBox ID="txtidnt" runat="server" OnTextChanged="txtidnt_TextChanged" AutoPostBack="True"
                                    CssClass="textb" Width="71px" TabIndex="1"></asp:TextBox>
                            </td>
                            <td id="Td1" class="tdstyle">
                                Company Name<br />
                                <asp:DropDownList CssClass="dropdown" ID="ddCompName" runat="server" Width="115px"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </td>
                            <td id="Td2" class="tdstyle">
                                Process Name<br />
                                <asp:DropDownList CssClass="dropdown" ID="ddProcessName" runat="server" Width="115px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </td>
                            <td id="Td6" class="tdstyle">
                                Indent No.<br />
                                <asp:DropDownList CssClass="dropdown" ID="ddindent" Width="101px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddindent_SelectedIndexChanged" TabIndex="5">
                                </asp:DropDownList>
                            </td>
                            <td id="Td4" class="tdstyle">
                                Emp name<br />
                                <asp:DropDownList CssClass="dropdown" ID="ddempname" runat="server" Width="115px"
                                    TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td7" class="tdstyle" visible="false" runat="server">
                                Challan No.<br />
                                <asp:DropDownList CssClass="dropdown" ID="DropDownList1" Width="101px" runat="server"
                                    AutoPostBack="True" TabIndex="5" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdChallanno" class="tdstyle" visible="false" runat="server">
                                Party Challan No.<br />
                                <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" Width="101px" runat="server"
                                    AutoPostBack="True" TabIndex="5" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td5" align="center" class="tdstyle">
                                Rec Date<br />
                                <asp:TextBox ID="txtdate" runat="server" TabIndex="6" Width="90px" CssClass="textb"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>
                            </td>
                            <td class="tdstyle">
                                GatePass<br />
                                <asp:TextBox ID="txtgatepass" Width="70px" runat="server" CssClass="textb" Visible="true"
                                    TabIndex="7"></asp:TextBox>
                            </td>
                            <td>
                                <%--ChallanNo--%><br />
                                <asp:TextBox ID="txtchallan" Width="70px" Visible="false" runat="server" AutoPostBack="True"
                                    CssClass="textb" TabIndex="8"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtprmid" runat="server" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="TXTPRTID" runat="server" Visible="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtindent" runat="server" Height="19px" CssClass="textb" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="txtissue" runat="server" Width="67px" ReadOnly="true" Height="19px"
                                    CssClass="textb" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr id="Tr2" runat="server">
                                    <td id="procode" runat="server" visible="false" colspan="2" class="tdstyle">
                                        Product Code&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtcode" Width="115px" runat="server" CssClass="textb" OnTextChanged="txtcode_TextChanged"
                                            AutoPostBack="True" TabIndex="9"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="txtcode"
                                            UseContextKey="True">
                                        </cc1:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr id="Tr3" runat="server">
                                    <td class="tdstyle">
                                        <asp:Label ID="LblCategory" runat="server" Text=""></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddCatagory" runat="server" Width="115px"
                                            TabIndex="10" AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="LblItemName" runat="server" Text="Label"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="dditemname" runat="server" Width="115px"
                                            OnSelectedIndexChanged="dditemname_SelectedIndexChanged" TabIndex="11" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="ql" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblQuality" runat="server" Text="Label"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="dquality" runat="server" Width="115px"
                                            TabIndex="12" OnSelectedIndexChanged="dquality_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="dsn" runat="server" visible="false">
                                        <asp:Label ID="LblDesign" runat="server" Text="Label" class="tdstyle"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="dddesign" runat="server" Width="115px"
                                            TabIndex="13" AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="clr" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblColor" runat="server" Text="Label"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddcolor" runat="server" Width="115px" TabIndex="14"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="shp" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblShape" runat="server" Text="Label"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddshape" runat="server" Width="115px" TabIndex="15"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="sz" runat="server" visible="false" class="tdstyle">
                                        &nbsp;<asp:Label ID="LblSize" runat="server" Text="Label"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddsize" runat="server" Width="115px" TabIndex="16"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="shd" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblColorShade" runat="server" Text="Label"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddlshade" runat="server" Width="100px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" TabIndex="17">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="TdFinish_Type" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblFinish_Type" runat="server" Text="Finish_Type"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" Width="110px" ID="ddFinish_Type" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddFinish_Type_SelectedIndexChanged"
                                            TabIndex="18">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        Godown Name<br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddgodown" runat="server" Width="115px"
                                            TabIndex="19" AutoPostBack="True" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        Unit<br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddlunit" runat="server" Width="80px" TabIndex="20">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        Lot No.<br />
                                        <asp:TextBox ID="txtlotno" runat="server" Width="100px" CssClass="textb" TabIndex="21"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Issued Qty<br />
                                        <asp:TextBox ID="txtrecqty" runat="server" Width="65px" Enabled="false" CssClass="textb"
                                            TabIndex="22"></asp:TextBox>
                                    </td>
                                    <td id="Td3" runat="server" visible="false" class="tdstyle">
                                        Stock<br />
                                        <asp:TextBox ID="txtstock" runat="server" Width="65px" Enabled="false" CssClass="textb"
                                            TabIndex="23"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        PreRec Qty<br />
                                        <asp:TextBox ID="txtprerec" runat="server" Width="65px" Enabled="false" CssClass="textb"
                                            TabIndex="24"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Pending Qty<br />
                                        <asp:TextBox ID="txtpending" runat="server" Width="65px" Enabled="false" CssClass="textb"
                                            TabIndex="25"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Rec Qty<br />
                                        <asp:TextBox ID="txtrec" runat="server" Width="65px" CssClass="textb" AutoPostBack="True"
                                            OnTextChanged="txtrec_TextChanged" TabIndex="26"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Ret Qty<br />
                                        <asp:TextBox ID="txtretrn" runat="server" Width="65px" CssClass="textb" TabIndex="27"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblmsg" runat="server" Text="ProdCode doesnot exist" Visible="false"
                                            ForeColor="Red"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="ProdCode doesnot exist with respect to this chalanno."
                                            Visible="false"></asp:Label>
                                        &nbsp;<asp:Button ID="btnNew0" runat="server" Width="0px" Height="0px" ForeColor="White"
                                            BorderStyle="None" Text="" />
                                        <asp:Label ID="lblind" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lbreclqty" runat="server" ForeColor="Red" Text="Qty is greater than pending qty"
                                            Visible="False" Width="300px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:GridView ID="DGSHOWDATA" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                OnPageIndexChanging="DGSHOWDATA_PageIndexChanging" PageSize="5" CssClass="grid-view"
                                OnRowCreated="DGSHOWDATA_RowCreated" OnRowDataBound="DGSHOWDATA_RowDataBound"
                                OnSelectedIndexChanged="DGSHOWDATA_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCTCODE" />
                                </Columns>
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Label ID="lblqty" runat="server" ForeColor="Red" Text="Recieve qty must be less than or equal to issue qty"
                                Visible="false"></asp:Label>
                            <td>
                            </td>
                        </td>
                        <td colspan="6" align="right">
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return validate()"
                                CssClass="buttonnorm" TabIndex="28" />
                            <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" CssClass="buttonnorm" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" OnClick="btnclose_Click" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" Visible="false" CssClass="buttonnorm"
                                OnClick="btndelete_Click" />
                            <asp:Button ID="Privew" runat="server" Text="Preview" OnClick="btnpreview_Click"
                                CssClass="buttonnorm" TabIndex="28" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <div style="width: 110%; height: 200px; overflow: scroll">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="prtid" OnSelectedIndexChanged="gvdetail_SelectedIndexChanged" CssClass="grid-view"
                                    OnRowCreated="gvdetail_RowCreated" OnRowDeleting="gvdetail_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="prtid" HeaderText="prtid" Visible="False" />
                                        <asp:BoundField DataField="category_name" HeaderText="Category Name" />
                                        <asp:BoundField DataField="Item_name" HeaderText="Item" />
                                        <asp:BoundField DataField="GodownName" HeaderText="Godown Name" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:BoundField DataField="RecQuantity" HeaderText="Quantity" />
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Visible="true" Text='<%# Bind("PRMID") %>' />
                                                <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("PRTID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    OnClientClick="javascript:return doConfirm();" Text="Del"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
