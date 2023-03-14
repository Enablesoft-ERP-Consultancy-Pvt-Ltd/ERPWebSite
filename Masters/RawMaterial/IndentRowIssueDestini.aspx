<%@ Page Title="Indent Raw Issue" Language="C#" AutoEventWireup="true" CodeFile="IndentRowIssueDestini.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_process_PRI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "IndentRowIssueDestini.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function validate() {
            if (document.getElementById("<%=ddgodown.ClientID %>").value == "0") {
                alert("Please select Godown");
                document.getElementById("<%=ddgodown.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtissqty.ClientID %>").value == "") {
                alert("Issue Qty Is Zero");
                document.getElementById("<%=txtissqty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px; width: 1200px; margin-right: 0px;">
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
                            <td id="Tdchallan" runat="server" visible="false" class="tdstyle">
                                Challan No.<br />
                                <asp:TextBox ID="txt_challanno" Width="100px" runat="server" TabIndex="6" AutoPostBack="True"
                                    CssClass="dropdown" OnTextChanged="txt_challanno_TextChanged"></asp:TextBox>
                            </td>
                            <td id="Td1" style="margin-left: 40px" class="tdstyle">
                                Company Name<br />
                                <asp:DropDownList ID="ddCompName" runat="server" Width="115px" TabIndex="1" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td2" class="tdstyle">
                                Process Name<br />
                                <asp:DropDownList ID="ddProcessName" runat="server" Width="115px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged" TabIndex="2" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td3" align="center" class="tdstyle">
                                Indent.No.<br />
                                <asp:DropDownList ID="ddindentno" runat="server" Width="80px" TabIndex="3" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddindentno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td4" class="tdstyle">
                                Party Name<br />
                                <asp:DropDownList ID="ddempname" runat="server" Width="115px" TabIndex="4" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddempname_SelectedIndexChanged" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td7" class="tdstyle" runat="server" visible="false">
                                Challan No<br />
                                <asp:DropDownList ID="DDChallanNo" runat="server" Width="115px" AutoPostBack="True"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Td5" align="center" class="tdstyle">
                                Issue Date<br />
                                <asp:TextBox ID="txtdate" runat="server" TabIndex="5" Width="100px" AutoPostBack="True"
                                    OnTextChanged="txtdate_TextChanged" CssClass="textb"></asp:TextBox>
                                <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                    ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                    ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>
                            </td>
                            <td id="Td6" class="tdstyle" runat="server">
                                Challan No.<br />
                                <asp:TextBox ID="txtchalanno" Width="100px" Enabled="false" runat="server" OnTextChanged="txtchalan_ontextchange"
                                    TabIndex="6" AutoPostBack="True" CssClass="dropdown"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr id="Tr3" runat="server">
                                    <td id="procode" runat="server" visible="false" class="tdstyle">
                                        Product Code
                                        <br />
                                        <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" OnTextChanged="TxtProdCode_TextChanged"
                                            AutoPostBack="True" Width="100px" TabIndex="6"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                            UseContextKey="True">
                                        </cc1:AutoCompleteExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblcategoryname" runat="server" Text="Category Name"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddCatagory" runat="server" Width="115px" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged"
                                            AutoPostBack="True" TabIndex="7" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="dditemname" runat="server" Width="115px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                            TabIndex="8" AutoPostBack="True" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="ql" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="dquality" runat="server" Width="115px" TabIndex="12" CssClass="dropdown"
                                            OnSelectedIndexChanged="dquality_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="dsn" runat="server" visible="true" class="tdstyle">
                                        <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="dddesign" runat="server" Width="115px" TabIndex="13" CssClass="dropdown"
                                            OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="clr" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddcolor" runat="server" Width="115px" TabIndex="14" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="shp" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddshape" runat="server" Width="115px" AutoPostBack="True" TabIndex="15"
                                            CssClass="dropdown" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="sz" runat="server" visible="false" class="tdstyle">
                                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />Check
                                        for Mtr
                                        <br />
                                        <asp:DropDownList ID="ddsize" runat="server" Width="115px" TabIndex="16" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="shd" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddlshade" runat="server" Width="115px" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="TdFinish_Type" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblFinish_Type" runat="server" Text="Finish_Type"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="ddFinish_Type" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddFinish_Type_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        Godown Name<br />
                                        <asp:DropDownList ID="ddgodown" runat="server" Width="115px" TabIndex="9" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        Unit<br />
                                        <asp:DropDownList ID="ddlunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                            TabIndex="11" Width="88px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        Indent Qty<br />
                                        <asp:TextBox ID="txtissue" CssClass="textb" runat="server" Enabled="false" Width="75px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        PreIssue Qty<br />
                                        <asp:TextBox ID="txtpreissue" runat="server" Enabled="false" Width="75px" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Pending Qty<br />
                                        <asp:TextBox ID="txtpendingqty" runat="server" Enabled="false" Width="75px" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        LotNo.<br />
                                        <asp:DropDownList ID="ddlotno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlotno_SelectedIndexChanged"
                                            CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        Stock<br />
                                        <asp:TextBox ID="txtstock" runat="server" Enabled="false" Width="75px" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Iss Qty<br />
                                        <asp:TextBox ID="txtissqty" runat="server" OnTextChanged="txtissqty_TextChanged"
                                            Width="75px" AutoPostBack="True" TabIndex="7" CssClass="textb"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtissqty"
                                            ErrorMessage="please Enter qty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldate" runat="server" ForeColor="Red" Text="Plz enter the date"
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="ProdCode not exist" Visible="false"
                                            Width="115px"></asp:Label>
                                    </td>
                                    <td class="style5">
                                        <asp:Label ID="Lblfinished" runat="server" ForeColor="Red" Text="Allready Issued Data not save...."
                                            Visible="False" Width="124px"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblqty" runat="server" ForeColor="Red" Text="Please Check Qty........."
                                            Visible="False" Width="124px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                                        Remarks
                                        <br />
                                        <asp:TextBox ID="txtremarks" runat="server" Width="250px" CssClass="textb" TextMode="MultiLine"
                                            TabIndex="26"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                                        <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm(); "
                                            TabIndex="17" CssClass="buttonnorm" />
                                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return validate();"
                                            TabIndex="8" CssClass="buttonnorm" />
                                        <asp:Button ID="btnpriview" runat="server" Text="Preview" TabIndex="19" CssClass="buttonnorm"
                                            Visible="False" OnClick="btnpriview_Click" />
                                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                            TabIndex="20" CssClass="buttonnorm" OnClick="btnclose_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:GridView ID="DGSHOWDATA" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                OnPageIndexChanging="DGSHOWDATA_PageIndexChanging" PageSize="5" CssClass="grid-view"
                                OnRowCreated="DGSHOWDATA_RowCreated" DataKeyNames="ITEM_FINISHED_ID" OnSelectedIndexChanged="DGSHOWDATA_SelectedIndexChanged"
                                OnRowDataBound="DGSHOWDATA_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCTCODE" />
                                </Columns>
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="6" align="center">
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="prtid" OnSelectedIndexChanged="gvdetail_SelectedIndexChanged" CssClass="grid-view"
                                    OnRowCreated="gvdetail_RowCreated" OnRowDeleting="gvdetail_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="PRTid" HeaderText="Sr.No." Visible="False" />
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory" />
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name" />
                                        <asp:BoundField DataField="GodownName" HeaderText="Godown Name" />
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                                        <asp:BoundField DataField="LotNo" HeaderText="LotNo" />
                                        <asp:BoundField DataField="issuequantity" HeaderText="Issue Quantity" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Visible="false" Text='<%# Bind("prmid") %>' />
                                                <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("prtid") %>' />
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="javascript:doConfirm();"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
