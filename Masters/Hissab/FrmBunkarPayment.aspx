<%@ Page Language="C#" Title="Bunkar Payment" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="FrmBunkarPayment.aspx.cs" Inherits="Masters_Hissab_FrmBunkarPayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>--%>
    <%-- <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmBunkarPayment.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        //window.setTimeout(function () { document.getElementById('txtKhapWidth').focus(); }, 0);

    </script>
    <%--<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $('#<%=GVBazaarDetail.ClientID %>').Scrollable();
    }
)
</script>--%>
    <script type="text/javascript" language="javascript">
        jQuery(function ($) {
            var focusedElementSelector = "";
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(function (source, args) {
                var fe = document.activeElement;
                focusedElementSelector = "";

                if (fe != null) {
                    if (fe.id) {
                        focusedElementSelector = "#" + fe.id;
                    } else {
                        // Handle Chosen Js Plugin
                        var $chzn = $(fe).closest('.chosen-container[id]');
                        if ($chzn.size() > 0) {
                            focusedElementSelector = '#' + $chzn.attr('id') + ' input[type=text]';
                        }
                    }
                }
            });

            prm.add_endRequest(function (source, args) {
                if (focusedElementSelector) {
                    $(focusedElementSelector).focus();
                }
            });
        });
    </script>
    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[id*=GVCarpetReceive] input[type=text]").on("keypress", function (e) {
        if (e.keyCode == 13) {
            var next = $(this).closest("tr").next().find("input[type=text]"); ;
            if (next.length > 0) {
                next.focus();

            } else {
                next = $("[id*=GVCarpetReceive] input[type=text]").eq(0);
                next.focus();
            }
            return false;
        }
    })
</script>--%>
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('DivBazaarDetail') != null) {
                xPos = $get('DivBazaarDetail').scrollLeft;
                yPos = $get('DivBazaarDetail').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('DivBazaarDetail') != null) {
                $get('DivBazaarDetail').scrollLeft = xPos;
                $get('DivBazaarDetail').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <script language="javascript" type="text/javascript">

        var scrollTop;
        //Register Begin Request and End Request
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Get The Div Scroll Position
        function BeginRequestHandler(sender, args) {
            var m = document.getElementById("DivBunkarCarpetReceive");
            m.scrollTop = m.scrollHeight;
        }

        function EndRequestHandler(sender, args) {
            var m = document.getElementById("DivBunkarCarpetReceive");
            m.scrollTop = m.scrollHeight;
        }
    </script>
    <%--<style type="text/css">
     .HeaderFreez
{
position:relative ;
top:expression(this.offsetParent.scrollTop);
z-index: 10
}
    </style>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left; background-color: #E3E3E3">
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table width="50%">
                        <tr>
                            <td class="tdstyle">
                                <asp:HiddenField ID="hncomp" runat="server" />
                                <asp:TextBox ID="TxtChallanNo" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                    Visible="false"></asp:TextBox>
                                <asp:Label ID="Label5" Text="CompanyName" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDCompanyName" AutoPostBack="true" runat="server" Width="200px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label7" Text="Contractor Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDContractorName" runat="server" AutoPostBack="true" Width="250px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDContractorName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDContractorName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Quality Type" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDItemName" runat="server" Width="150px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDItemName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label1" class="tdstyle" runat="server" Text="Bunkar Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDBunkarName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDBunkarName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label2" class="tdstyle" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDMonth" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label3" class="tdstyle" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDYear" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;<br />
                                <asp:Button CssClass="buttonnorm preview_width" ID="BtnShow" Enabled="true" runat="server"
                                    Text="Show" OnClick="BtnShow_Click" Width="100px" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDDDPONO" runat="server">
                                &nbsp;
                            </td>
                            <td style="width: 100px">
                                &nbsp;
                            </td>
                            <td style="width: 100px">
                                &nbsp;
                            </td>
                            <td style="width: 100px">
                                &nbsp;
                            </td>
                            <td class="tdstyle" runat="server" id="td22">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <%--           <div style="width:1200px;border: solid 2px #E0E0E0;color:#ffffff;font-weight:bold;padding: 3px;font-size: 12px;">
        <table bgcolor="#8B7B8B" rules="all" >
            <tr>
                <td style ="width:80px;">Sr.No</td>
                <td style ="width:100px;">Challan No</td>
                <td style ="width:100px;">Receive Date</td>
                <td style ="width:100px;">Item</td>
                <td style ="width:150px;">Quality</td>
                <td style ="width:100px;">Design</td>
                <td style ="width:100px;">Color</td>
                <td style ="width:100px;">Shape</td>
                <td style ="width:100px;">Size</td>
                 <td style ="width:100px;">Bazaar Qty</td>
                  <td style ="width:100px;">PQty</td>
            </tr>
        </table>
        </div>--%>
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table width="100%">
                        <tr>
                            <td>
                                <div id="DivBazaarDetail" style="height: 250px; overflow: auto; width: 1200px;">
                                    <asp:GridView ID="GVBazaarDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnSelectedIndexChanged="GVBazaarDetail_SelectedIndexChanged" OnRowDataBound="GVBazaarDetail_RowDataBound"
                                        OnSorting="GVBazaarDetail_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="gvheaders" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <%--<asp:TemplateField HeaderText="Sr.No">
                                            <ItemTemplate>
                                              <asp:Label ID="lblSrNo" runat="server" Text='<%#Bind("srno") %>'></asp:Label>
                                            </ItemTemplate>
                                           <ItemStyle HorizontalAlign="Center" Width="100px"  />
                                        </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Sr.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Bind("ChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Receive Date" SortExpression="ReceiveDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiveDate" runat="server" Text='<%#Bind("ReceiveDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="400px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item" Visible="true" SortExpression="Item">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("Item") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality" Visible="true" SortExpression="Quality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Design" Visible="true" SortExpression="Design">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" Visible="true" SortExpression="Color">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColor" runat="server" Text='<%#Bind("Color") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shape" Visible="true" SortExpression="Shape">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size" SortExpression="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bazaar Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBazaarQty" runat="server" Text='<%#Bind("BazaarQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PQty">
                                                <ItemTemplate>
                                                    <%-- <asp:Label ID="lblPQty" runat="server" Text='<%#Bind("PQty") %>'></asp:Label>--%>
                                                    <asp:Label ID="hnBalanceQty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "BazaarQty").ToString(),DataBinder.Eval(Container.DataItem, "Item_Finished_ID").ToString(),DataBinder.Eval(Container.DataItem, "Process_Rec_Id").ToString(),DataBinder.Eval(Container.DataItem, "Process_Rec_Detail_Id").ToString()) %>'
                                                        Visible="false" />
                                                    <asp:Label ID="lblbalnce" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "BazaarQty").ToString(),DataBinder.Eval(Container.DataItem, "Item_Finished_ID").ToString(),DataBinder.Eval(Container.DataItem, "Process_Rec_Id").ToString(),DataBinder.Eval(Container.DataItem, "Process_Rec_Detail_Id").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance to Receive" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemFinishedId" runat="server" Text='<%# Bind("Item_Finished_ID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("ITEM_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblArea" runat="server" Text='<%# Bind("Area") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblLagat" runat="server" Text='<%# Bind("lagat") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Id" runat="server" Text='<%# Bind("Process_Rec_Id") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Detail_Id" runat="server" Text='<%# Bind("Process_Rec_Detail_Id") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblHPQty" runat="server" Text='<%# Bind("PQty") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblPenalityRemark" runat="server" Text='<%# Bind("PRemarks") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UnitId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblQualityId" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCalType" runat="server" Text='<%# Bind("CalType") %>' Visible="false"></asp:Label>
                                                    <%-- <asp:Label ID="lblComm" runat="server" Text='<%# Bind("Comm") %>' Visible="false"></asp:Label>
                                                 <asp:Label ID="lblOrderid" runat="server" Text='<%# Bind("Orderid") %>' Visible="false"></asp:Label>
                                                 <asp:Label ID="lblFlagFixOrWeight" runat="server" Text='<%# Bind("FlagFixOrWeight") %>' Visible="false"></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table width="100%">
                        <tr>
                            <td>
                                <div id="DivBunkarCarpetReceive" style="height: 200px; overflow: auto; width: 1200px;">
                                    <asp:GridView ID="GVBunkarCarpetReceive" AutoGenerateColumns="False" runat="server"
                                        CssClass="grid-views" OnRowDataBound="GVBunkarCarpetReceive_RowDataBound" OnRowDeleting="GVBunkarCarpetReceive_RowDeleting"
                                        ShowFooter="true" HeaderStyle-CssClass="FixedHeader">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <FooterStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                        CommandName="Delete"></asp:LinkButton></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr.No2" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Bind("SrNo") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" Visible="false">
                                                <ItemTemplate>
                                                    <div style="width: 300px;">
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("ItemDescription") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Bind("BazaarChallanNo") %>'
                                                        Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ReceiveDate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtRecDate" runat="server" Width="90px" Text='<%#Bind("ReceiveDate","{0:dd-MMM-yyyy}") %>'
                                                        CssClass="textb" AutoPostBack="true" OnTextChanged="TxtRecDate_TextChanged"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemName" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Design" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColor" runat="server" Text='<%#Bind("Color") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shape" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BunkarQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBunkarQty" runat="server" Text='<%#Bind("BunkarQty") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtBunkarQty" runat="server" Text='<%#Eval("BunkarQty") %>' OnTextChanged="txtBunkarQty_TextChanged"
                                                        onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BZ Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBZWeight" runat="server" Text='<%#Bind("BZWeight") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtBZWeight" runat="server" Text='<%#Eval("BZWeight") %>' OnTextChanged="txtBZWeight_TextChanged"
                                                        onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="St Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStWeight" runat="server" Text='<%#Bind("StWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                <%-- <FooterTemplate>
                                           <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Font-Bold="true" />
                                          </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenality" runat="server" Text='<%#Bind("Penality") %>' Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="popup" runat="server" Text="Add Penality" OnClick="popup_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PenalityName">
                                                <ItemTemplate>
                                                    <div style="width: 120px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                        <asp:Label ID="lblPenalityName" runat="server" ToolTip='<%#Bind("Penality") %>' Text='<%#Bind("Penality") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Area">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArea" runat="server" Text='<%#Bind("Area") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TArea">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTArea" runat="server" Text='<%#Bind("TArea") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lagat">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLagat" runat="server" Text='<%#Bind("Lagat") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Def Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDefWeight" runat="server" Text='<%#Bind("DefWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraWeight" runat="server" Text='<%#Bind("ExtraWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actual%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualPercentage" runat="server" Text='<%#Bind("ActualPercentage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Less%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLessPercentage" runat="server" Text='<%#Bind("LessPercentage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="W Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight PRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWeightPRate" runat="server" Text='<%#Bind("WeightPRate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="W Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWAmount" runat="server" Text='<%#Bind("WAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenalityAmount" runat="server" Text='<%#Bind("PenalityAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight Penality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWeightPenality" runat="server" Text='<%#Bind("WeightPenality") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidAmount" runat="server" Text='<%#Bind("PaidAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WOrderId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("ItemId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("Qualityid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblItemFinishedId" runat="server" Text='<%#Bind("Item_Finished_Id") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Id" runat="server" Text='<%#Bind("Process_Rec_Id") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Detail_ID" runat="server" Text='<%#Bind("Process_Rec_Detail_ID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCalType" runat="server" Text='<%# Bind("CalType") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="hnBalQty" runat="server" Text='<%# Bind("BalQty") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="80%">
                    <tr>
                        <td colspan="4">
                            <%--<asp:Label ID="Label25" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="right">
                            <asp:Label ID="llMessageBox" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:CheckBox ID="chkForSlip" runat="server" Text="For Slip Print" CssClass="labelnormalMM"
                                Font-Bold="true" Visible="false" />
                            &nbsp
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="height: 200px; overflow: auto; width: 1200px;">
                                    <asp:GridView ID="GVBunkarPaymentDetail" AutoGenerateColumns="False" runat="server"
                                        CssClass="grid-views" OnRowDataBound="GVBunkarPaymentDetail_RowDataBound" ShowFooter="true">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <FooterStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Bind("BazaarChallanNo") %>'
                                                        Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ReceiveDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiveDate" runat="server" Text='<%#Bind("ReceiveDate","{0:dd-MMM-yyyy}") %>'
                                                        Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemName" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("Item_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Design" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColor" runat="server" Text='<%#Bind("ColorName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shape" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShape" runat="server" Text='<%#Bind("ShapeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BunkarQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BZ Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBZWeight" runat="server" Text='<%#Bind("BZWeight") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="St Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStWeight" runat="server" Text='<%#Bind("StWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                <%-- <FooterTemplate>
                                           <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Font-Bold="true" />
                                          </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PenalityName">
                                                <ItemTemplate>
                                                    <div style="width: 120px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                        <asp:Label ID="lblPenalityName" runat="server" ToolTip='<%#Bind("PenaltyName") %>'
                                                            Text='<%#Bind("PenaltyName") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("Item_Id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Id" runat="server" Text='<%#Bind("Process_Rec_Id") %>'
                                                        Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="100%">
                    <tr>
                        <td colspan="7">
                            <div style="height: 250px; overflow: auto; width: 950px;">
                            </div>
                        </td>
                        <td id="qulitychk" runat="server" valign="top">
                            <div id="Div2" runat="server" style="height: 250px; overflow: auto;">
                            </div>
                        </td>
                        <td runat="server" id="tdordergrid">
                            <div id="Div1" runat="server" style="height: 250px; overflow: auto; width: 250px;">
                                <asp:HiddenField ID="hnBalQty" runat="server" Visible="false" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
                PopupControlID="pnlPopup" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground"
                CancelControlID="btnHide">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                <div class="header">
                    <asp:GridView ID="GVPenalty" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        Width="350px" OnRowDataBound="GVPenalty_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts1" />
                        <RowStyle CssClass="gvrow1" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkboxitem" runat="server" AutoPostBack="true" OnCheckedChanged="Chkboxitem_CheckedChanged" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Penalty Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityName" Text='<%#Bind("PenalityName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtQty_TextChanged" onkeypress="return isNumberKey(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" Text='<%#Bind("rate") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAmt" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtAmt_TextChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityId" Text='<%#Bind("PenalityId") %>' runat="server" Visible="false" />
                                    <asp:Label ID="lblPenalityType" Text='<%#Bind("PenalityType") %>' runat="server"
                                        Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="header">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnHide" runat="server" Text="Close" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
