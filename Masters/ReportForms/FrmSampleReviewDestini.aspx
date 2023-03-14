<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmSampleReviewDestini.aspx.cs" Inherits="Masters_ReportForms_FrmSampleReviewDestini" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function New() {
            window.location.href = "FrmSampleReviewDestini.aspx";
        }
        function priview() {
            {
                window.open('../../ReportViewer.aspx', '');
            }
        }
    </script>
    <table>
        <tr>
            <td>
                Company Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddCompName" runat="server" Width="250px" TabIndex="1" CssClass="dropdown">
                </asp:DropDownList>
                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
            <td id="Td1" runat="server" class="tdstyle">
                Customer Name&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddcustomername" runat="server" Width="250px" TabIndex="1" CssClass="dropdown">
                </asp:DropDownList>
                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddcustomername"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
        </tr>
        <tr>
            <td class="tdstyle">
                <asp:Label ID="lblItemCode" runat="server" Text="Item Code"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="90px" AutoPostBack="True"
                    OnTextChanged="TxtProdCode_TextChanged" TabIndex="1"></asp:TextBox>
                <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality1" TargetControlID="TxtProdCode"
                    UseContextKey="True">
                </cc1:AutoCompleteExtender>
            </td>
            <td class="tdstyle">
                Price
                <asp:Label ID="lblcurrency" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtprice" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
            </td>
            <td class="tdstyle">
                Weight &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtweigth" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Item Description :-<br />
                <asp:TextBox ID="txtdescription" runat="server" CssClass="textb" Width="250px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Material :-<br />
                <asp:TextBox ID="txtmaterial" runat="server" CssClass="textb" Width="250px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Finish :-<asp:TextBox ID="Txtfinish" runat="server" CssClass="textb" Width="250px"
                    TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Sample Test&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList CssClass="dropdown" ID="DDSample" runat="server" Width="100px">
                    <asp:ListItem Value="0">Yes</asp:ListItem>
                    <asp:ListItem Value="1">No</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                Item Approved&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList CssClass="dropdown" ID="ddlitemapp" runat="server" Width="100px">
                    <asp:ListItem Value="0">Yes</asp:ListItem>
                    <asp:ListItem Value="1">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tdstyle">
                <table>
                    <tr>
                        <td>
                            Length
                            <br />
                            <asp:TextBox ID="txtlength" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            width
                            <br />
                            <asp:TextBox ID="txtwidth" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            height
                            <br />
                            <asp:TextBox ID="txtheigth" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            DIA
                            <br />
                            <asp:TextBox ID="txtdia" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="tdstyle">
                Bulb/Cfl
                <br />
                <asp:TextBox ID="txtbulb" runat="server" CssClass="textb" Width="90px" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Inner Ctn Qty
                <br />
                <asp:TextBox ID="txtinnerqty" runat="server" CssClass="textb" Width="150px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Inner Ctn Size
                <br />
                <asp:TextBox ID="txtinnersize" runat="server" CssClass="textb" Width="150px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Master Ctn Qty
                <br />
                <asp:TextBox ID="txtmasterqty" runat="server" CssClass="textb" Width="150px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Master Ctn Size
                <br />
                <asp:TextBox ID="txtmastersize" runat="server" CssClass="textb" Width="150px" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Test Required :-
                <br />
                <asp:TextBox ID="txttestreq" runat="server" TextMode="MultiLine" CssClass="textb"
                    Width="200px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Transit Test :-
                <br />
                <asp:TextBox ID="txttranreq" runat="server" TextMode="MultiLine" CssClass="textb"
                    Width="200px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Labeling
                <br />
                <asp:TextBox ID="txtlabel" runat="server" TextMode="MultiLine" CssClass="textb" Width="200px"
                    TabIndex="1"></asp:TextBox>
            </td>
            <td>
                Wiring
                <br />
                <asp:TextBox ID="txtwiring" runat="server" CssClass="textb" TextMode="MultiLine"
                    Width="200px" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                Comment
                <br />
                <asp:TextBox ID="txtcomment" runat="server" CssClass="textb" TextMode="MultiLine"
                    Width="400px" TabIndex="1"></asp:TextBox>
            </td>
            <td colspan="2">
                <asp:Button ID="btnnew" runat="server" Text="New" TabIndex="22" CssClass="buttonnorm"
                    OnClientClick="New()" />
                &nbsp;
                <asp:Button ID="btnsybmit" runat="server" Text="Submit" TabIndex="22" CssClass="buttonnorm"
                    OnClick="btnsybmit_Click" />
                &nbsp;<asp:Button ID="btnpreview" runat="server" Visible="false" Text="Preview" TabIndex="22"
                    CssClass="buttonnorm" OnClick="btnpreview_Click1" />
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <br />
                <asp:GridView ID="DGsampleprivew" AutoGenerateColumns="False" AllowPaging="True"
                    OnRowDataBound="DGsampleprivew_RowDataBound" CellPadding="4" runat="server" DataKeyNames="Sampleid"
                    OnSelectedIndexChanged="DGsampleprivew_SelectedIndexChanged" Width="100%" OnRowDeleting="DGsampleprivew_RowDeleting"
                    CssClass="grid-view" OnRowCreated="DGsampleprivew_RowCreated">
                    <HeaderStyle CssClass="gvheader" />
                    <AlternatingRowStyle CssClass="gvalt" />
                    <RowStyle CssClass="gvrow" />
                    <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:BoundField DataField="Sampleid" HeaderText="Sampleid" Visible="false" />
                        <%--<asp:BoundField DataField="ChallanNo" HeaderText="" >
                                        <HeaderStyle Width="0px" />
                                        <ItemStyle Width="0px" />
                                        </asp:BoundField>--%>
                        <asp:BoundField DataField="productcode" HeaderText="Item #" />
                        <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                        <asp:BoundField DataField="Price" HeaderText="Price" />
                        <asp:BoundField DataField="Weight" HeaderText="Weight" />
                        <asp:BoundField DataField="Length" HeaderText="Length" />
                        <asp:BoundField DataField="Width" HeaderText="Width" />
                        <asp:TemplateField HeaderText="Height">
                            <ItemTemplate>
                                <asp:Label ID="lblheight" runat="server" Text='<%# Bind("height") %>' />
                                <asp:Label ID="Lblcompanyid" runat="server" Visible="false" Text='<%# Bind("Companyid") %>' />
                                <asp:Label ID="lblcustomerid" runat="server" Visible="false" Text='<%# Bind("Customerid") %>' />
                                <asp:Label ID="lblfinishedid" runat="server" Visible="false" Text='<%# Bind("Finishedid") %>' />
                                <asp:Label ID="lblprod" runat="server" Visible="false" Text='<%# Bind("ProductCode") %>' />
                                <asp:Label ID="lblprice" runat="server" Visible="false" Text='<%# Bind("Price") %>' />
                                <asp:Label ID="lblweigth" runat="server" Visible="false" Text='<%# Bind("Weight") %>' />
                                <asp:Label ID="lblItemDescription" runat="server" Visible="false" Text='<%# Bind("ItemDescription") %>' />
                                <asp:Label ID="lblMaterial" runat="server" Visible="false" Text='<%# Bind("Material") %>' />
                                <asp:Label ID="lblfinish" runat="server" Visible="false" Text='<%# Bind("finish") %>' />
                                <asp:Label ID="LblSampleTest" runat="server" Visible="false" Text='<%# Bind("SampleTest") %>' />
                                <asp:Label ID="lblitemApp" runat="server" Visible="false" Text='<%# Bind("ItemApproved") %>' />
                                <asp:Label ID="lbllength" runat="server" Visible="false" Text='<%# Bind("Length") %>' />
                                <asp:Label ID="lblWidth" runat="server" Visible="false" Text='<%# Bind("Width") %>' />
                                <asp:Label ID="lblDIA" runat="server" Visible="false" Text='<%# Bind("DIA") %>' />
                                <asp:Label ID="lblBulb_Cfl" runat="server" Visible="false" Text='<%# Bind("Bulb_Cfl") %>' />
                                <asp:Label ID="lblinnerctnqty" runat="server" Visible="false" Text='<%# Bind("InnerCtnQty") %>' />
                                <asp:Label ID="lblinnerctnsize" runat="server" Visible="false" Text='<%# Bind("InnerCtnSize") %>' />
                                <asp:Label ID="lblmasterqty" runat="server" Visible="false" Text='<%# Bind("MasterCtnQty") %>' />
                                <asp:Label ID="lblmastersize" runat="server" Visible="false" Text='<%# Bind("MasterCtnSize") %>' />
                                <asp:Label ID="lbltestreq" runat="server" Visible="false" Text='<%# Bind("TestRequired") %>' />
                                <asp:Label ID="lbltranreq" runat="server" Visible="false" Text='<%# Bind("TransitTest") %>' />
                                <asp:Label ID="lbllableing" runat="server" Visible="false" Text='<%# Bind("Lableling") %>' />
                                <asp:Label ID="lblwiring" runat="server" Visible="false" Text='<%# Bind("Wiring") %>' />
                                <asp:Label ID="lblcomment" runat="server" Visible="false" Text='<%# Bind("Comment") %>' />
                                <asp:Label ID="lblphoto" runat="server" Visible="false" Text='<%# Bind("photo") %>' />
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DIA" HeaderText="DIA" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')" CssClass="buttonnorm"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
