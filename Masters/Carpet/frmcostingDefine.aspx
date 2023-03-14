<%@ Page Title="Costing" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmcostingDefine.aspx.cs" Inherits="Masters_Carpet_frmcostingDefine" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmcostingDefine.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isnumber(evt) {
            evt = (evt) ? evt : window.event;
            var charcode = (evt.which) ? evt.which : evt.keycode;
            if (charcode > 31 && (charcode < 48 || charcode > 57) && charcode != 46) {
                return false;
            }
            return true;
        }
        function ItemDetailTextChanged() {
            var Rate = document.getElementById('<%=txtRate.ClientID %>');
            var Qty = document.getElementById('<%=txtQty.ClientID %>');
            var amount = document.getElementById('<%=txtAmount.ClientID %>');
            amount.value = (getNum(parseFloat(Rate.value)) * getNum(parseFloat(Qty.value))).toFixed(2);

        }
        function ProcessTextChanged() {
            var Rate = document.getElementById('<%=txtProcessRate.ClientID %>');
            var Qty = document.getElementById('<%=txtProcessQty.ClientID %>');
            var amount = document.getElementById('<%=txtProcessAmount.ClientID %>');
            amount.value = (getNum(parseFloat(Rate.value)) * getNum(parseFloat(Qty.value))).toFixed(2);

        }
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    if ($("#<%=DDCategoryname.ClientID %>")) {
                        var selectedindex = $("#<%=DDCategoryname.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Category!!\n";
                        }
                    }
                    if ($("#<%=DDItemName.ClientID %>")) {
                        var selectedindex = $("#<%=DDItemName.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select ItemName!!\n";
                        }
                    }
                    if ($("#<%=TDQuality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDQuality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Qualityname!!\n";
                        }
                    }
                    if ($("#<%=TDDesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDDesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design!!\n";
                        }
                    }
                    if ($("#<%=TDColor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDColor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color!!\n";
                        }
                    }
                    if ($("#<%=TDShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDshape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape!!\n";
                        }
                    }
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size!!\n";
                        }
                    }
                    if ($("#<%=txtsamplecode.ClientID %>")) {
                        var selectedindex = $("#<%=txtsamplecode.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter Sample code No.!!\n";
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
                if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                    var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        $("#<%=TDCBM.ClientID%>").hide();
                    }
                    else {
                        $("#<%=TDCBM.ClientID%>").show();
                    }
                }
                $("#<%=DDSize.ClientID %>").change(function () {

                    var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                    if (selectedindex == 0) {
                        $("#<%=TDCBM.ClientID%>").hide();
                    }
                    else {
                        $("#<%=TDCBM.ClientID%>").show();
                    }
                });
                //link button click
                $("#<%=lnkcbm.ClientID%>").click(function () {
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            alert("Please select size!!!.");
                        }
                        else {
                            var Sizeid = $("#<%=DDSize.ClientID %>").val();
                            var size = $('#<%=DDSize.ClientID %> :selected').text();
                            var sizeflag = $('#<%=DDsizetype.ClientID %> :selected').text();
                            var sizeflagId = $('#<%=DDsizetype.ClientID %>').val();
                            var left = (screen.width / 2) - (500 / 2);
                            var top = (screen.height / 2) - (400 / 2);

                            window.open('frmcalculatecbm.aspx?a=' + Sizeid + '&b=' + size + '&c=' + sizeflag + '&d=' + sizeflagId, 'ADD LOOM MASTER', 'width=500px, height=200px, top=' + top + ', left=' + left);
                        }
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
            <fieldset>
                <legend>
                    <asp:Label ID="lblSearchDetail" runat="server" Text="Item Description" CssClass="labelbold"
                        ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                <asp:Panel ID="pnl1" runat="server">
                    <table>
                        <tr>
                            <td id="TDProdcode" runat="server" visible="false">
                                <asp:Label ID="lblItemCode" runat="server" Text="ItemCode" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtprodcode" runat="server" Width="95px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblcategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCategoryname" runat="server" CssClass="dropdown" Width="150px"
                                    OnSelectedIndexChanged="DDCategoryname_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblIemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDItemName" runat="server" CssClass="dropdown" Width="150px"
                                    OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false">
                                <asp:Label ID="lblDesign" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblColor" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDshape" runat="server" CssClass="dropdown" Width="150px" OnSelectedIndexChanged="DDshape_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" visible="false">
                                <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                &nbsp;
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                <br />
                                <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshade" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblSampleCode" Text="Sample Code" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtsamplecode" CssClass="textb" runat="server" Width="130px" />
                                <asp:AutoCompleteExtender ID="AutoCompleteExtenderSamplecode" runat="server" BehaviorID="SamplecodeSrchAutoComplete"
                                    CompletionInterval="20" Enabled="True" ServiceMethod="GetCostingsamplecode" EnableCaching="true"
                                    CompletionSetCount="30" ServicePath="~/Autocomplete.asmx" TargetControlID="txtsamplecode"
                                    UseContextKey="true" ContextKey="0#0#0" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblCosting" Text="Costing For" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDCostingFor" CssClass="dropdown" runat="server" Width="130px">
                                    <asp:ListItem Value="0">Sample</asp:ListItem>
                                    <asp:ListItem Value="1">Customer</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnshowdetail" Text="Show Detail" CssClass="buttonnorm" runat="server"
                                    OnClick="btnshowdetail_Click" />
                            </td>
                            <td id="TDCBM" runat="server">
                                <asp:LinkButton ID="lnkcbm" Text="Calculate CBM" ForeColor="Blue" runat="server"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcostingDate" Text="Costing Define Date" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtcostingDate" runat="server" CssClass="textb" Width="130px" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtcostingDate" runat="server" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label ID="Label1" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                        Font-Bold="true"></asp:Label></legend>
                <asp:Panel ID="Panel1" runat="server">
                    <table>
                        <tr>
                            <td id="TDItemCodeDetail" runat="server" visible="false">
                                <asp:Label ID="lblItemCodeD" runat="server" Text="ItemCode" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtItemCodeRD" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblCategoryRD" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCategoryRD" runat="server" CssClass="dropdown" Width="150px"
                                    OnSelectedIndexChanged="DDCategoryRD_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblItemRD" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDItemNameRD" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDItemNameRD_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQualityRD" runat="server" visible="false">
                                <asp:Label ID="lblQualityRD" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDQualityRD" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesignRD" runat="server" visible="false">
                                <asp:Label ID="lblDesignRD" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDDesignRD" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColorRD" runat="server" visible="false">
                                <asp:Label ID="lblColorRD" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDColorRD" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDShapeRD" runat="server" visible="false">
                                <asp:Label ID="lblShapeRD" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDShapeRD" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDShapeRD_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSizeRD" runat="server" visible="false">
                                <asp:Label ID="Label9" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                &nbsp;
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDSizeTypeRD" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDSizeTypeRD_SelectedIndexChanged" />
                                <br />
                                <asp:DropDownList ID="DDSizeRD" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShadeRD" runat="server" visible="false">
                                <asp:Label ID="Label10" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDShadeRD" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblRate" Text="Rate" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtRate" runat="server" CssClass="textb" Width="60px" onkeypress=" return isnumber(event)"
                                    onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td>
                                <asp:Label ID="lblQty" Text="Avg." runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtQty" runat="server" CssClass="textb" Width="60px" onkeypress=" return isnumber(event)"
                                    onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td>
                                <asp:Label ID="lblamount" Text="Amount" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="textb" Width="100px" onkeypress=" return isnumber(event)" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label ID="lblProcessCosting" runat="server" Text="Process Costing" CssClass="labelbold"
                        ForeColor="Red" Font-Bold="true"></asp:Label>
                </legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblProcess" Text="Process" runat="server" CssClass="labelbold" /><br />
                            <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblPRate" Text="Rate" runat="server" CssClass="labelbold" /><br />
                            <asp:TextBox ID="txtProcessRate" runat="server" CssClass="textb" Width="60px" onkeypress=" return isnumber(event)"
                                onchange="javascript: ProcessTextChanged();" />
                        </td>
                        <td>
                            <asp:Label ID="lblPQty" Text="Avg." runat="server" CssClass="labelbold" /><br />
                            <asp:TextBox ID="txtProcessQty" runat="server" CssClass="textb" Width="60px" onkeypress=" return isnumber(event)"
                                onchange="javascript: ProcessTextChanged();" />
                        </td>
                        <td>
                            <asp:Label ID="lblPAmount" Text="Amount" runat="server" CssClass="labelbold" /><br />
                            <asp:TextBox ID="txtProcessAmount" runat="server" CssClass="textb" Width="100px"
                                onkeypress=" return isnumber(event)" />
                        </td>
                        <td>
                            <asp:Label ID="lblremark" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtprocessremark" runat="server" CssClass="textb" Width="500px" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="height: 300px; overflow: auto; width: 800px">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GVDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found..."
                                OnRowDeleting="GVDetail_RowDeleting" OnRowDataBound="GVDetail_RowDataBound" ShowFooter="true"
                                OnDataBound="GVDetail_DataBound" OnRowCommand="GVDetail_RowCommand">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAddImage" runat="server" CausesValidation="False" CommandName="AddImage"
                                                Text="Add Image" CssClass="buttonnorm"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnlFillershell" Text="Profit_cal" runat="server" CommandName="Filler"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item_Description">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SampleCode" HeaderText="Sample_Code">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CostingType" HeaderText="Costing_Type">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Details" HeaderText="Details">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Avg.">
                                        <ItemTemplate>
                                            <div style="text-align: right;">
                                                <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lbltotal" runat="server" Text="Total"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <div style="text-align: right;">
                                                <asp:Label ID="lblAmount" Text='<%#Bind("Amount") %>' runat="server" />
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="text-align: right;">
                                                <asp:Label ID="lblTotalqty" runat="server" />
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="costingid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcostingid" Text='<%#Bind("costingid") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDel" Text="Delete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');"
                                                CommandName="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FinishedId_ProcessId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinishedId_ProcessId" runat="server" Text='<%#Bind("FinishedId_ProcessId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DetailType" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDetailType" runat="server" Text='<%#Bind("DetailType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#cccccc" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
