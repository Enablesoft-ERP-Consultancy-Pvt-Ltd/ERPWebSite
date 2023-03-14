<%@ Page Language="C#" Title="ProcessReceiveNew" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="ProcessReceiveNew.aspx.cs" Inherits="Masters_process_ProcessReceiveNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%-- <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "ProcessReceiveNew.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        //window.setTimeout(function () { document.getElementById('txtKhapWidth').focus(); }, 0);

    </script>
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
    <%--<script type="text/javascript">
    function showmodalpopup(){
  
       var mpePopup=$find("<%=ModalPopupExtender1.ClientID%>");   
       mpePopup.show()
       setTimeout(txtsetfocus(),100);          //time is in milliseconds
        } 
        function txtsetfocus(){
            $("<%=txtJobName.ClientID%>").focus();
        }
    </script>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left; background-color: #E3E3E3">
                <span style="float: right;">&nbsp;<asp:CheckBox ID="chkboxManualWeight" runat="server"
                    AutoPostBack="True" OnCheckedChanged="chkboxManualWeight_CheckedChanged" />
                    &nbsp;<asp:Label ID="Label16" runat="server" Text="Check For Manual Weight Entry"
                        CssClass="labelbold"></asp:Label>
                    &nbsp;<asp:CheckBox ID="chkboxSampleFlag" runat="server" />
                    &nbsp;<asp:Label ID="Label39" runat="server" Text="Check For Sample" CssClass="labelbold"></asp:Label>
                    &nbsp;&nbsp;&nbsp;<asp:Label ID="Label15" Text="Last ChallanNo:" runat="server" CssClass="labelbold"
                        ForeColor="Red" />
                    <asp:Label ID="lblChallanNo" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    &nbsp;<asp:CheckBox ID="chkboxRoundFullArea" runat="server" />
                    &nbsp;<asp:Label ID="Label38" runat="server" Text="Check For Full Area When Round"
                        CssClass="labelbold"></asp:Label>
                    <asp:Label ID="lblProcessRecID" Text="" runat="server" CssClass="labelbold" Visible="false" /></span>
                <asp:TabContainer ID="tbsample" runat="server" Width="100%" ActiveTabIndex="0">
                    <asp:TabPanel ID="TabMainInformation" HeaderText="Weaver/Contractor Information"
                        runat="server">
                        <ContentTemplate>
                            <div style="width: 100%; float: left; background-color: #E3E3E3">
                                <table width="50%">
                                    <tr id="trprifix" runat="server">
                                        <td>
                                            <asp:HiddenField ID="hncomp" runat="server" />
                                        </td>
                                        <td align="center" class="tdstyle">
                                            <asp:Label ID="lbl" Text="PreFix" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="TxtPrefix" runat="server" CssClass="textb" AutoPostBack="True" OnTextChanged="TxtPrefix_TextChanged"></asp:TextBox>
                                        </td>
                                        <td align="center" class="tdstyle">
                                            <asp:Label ID="lblNo" Text=" No." runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="TxtPostfix" runat="server" CssClass="textb" AutoPostBack="True"
                                                OnTextChanged="TxtPostfix_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--  <td class="tdstyle">
                            <asp:Label ID="Label4" Text="POrderNo" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPOrderNo" runat="server" CssClass="textb" OnTextChanged="TxtPOrderNo_TextChanged"
                                Width="80px" AutoPostBack="True"></asp:TextBox>
                        </td>--%>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label9" Text="Rec.Date" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="TxtRecDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label5" Text="CompanyName" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList ID="DDCompanyName" AutoPostBack="true" runat="server" Width="200px"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle" id="TDProcessName" runat="server" visible="false">
                                            <asp:Label ID="Label6" Text=" ProcessName" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label7" Text=" Vendor Name" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList ID="DDEmployeeNamee" runat="server" AutoPostBack="true" Width="250px"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDEmployeeNamee_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--<cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmployeeNamee"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label8" Text="Challan No" runat="server" CssClass="labelbold" Visible="false" />
                                            <br />
                                            <asp:TextBox ID="TxtChallanNo" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                                Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td id="TDDDPONO" runat="server">
                                            <asp:Label ID="Label26" runat="server" Text="PO No. " CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDPONo" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDPONo_SelectedIndexChanged" Width="150px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDPONo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <%--<td id="TDTextProductCode" runat="server">
                            <asp:Label ID="LblProdCode" class="tdstyle" runat="server" Text="Prod Code " CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtProductCode" runat="server" AutoPostBack="true" OnTextChanged="TxtProductCode_TextChanged"
                                Width="125px" CssClass="textb"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>--%>
                                        <td id="tdCategory" runat="server" visible="false">
                                            <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Category " CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDCategoryName" runat="server" AutoPostBack="true" Width="150px"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategoryName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDItemName" runat="server" Width="150px" AutoPostBack="true"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDItemName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Label ID="Label1" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDQualityName" runat="server" Width="150px" AutoPostBack="true"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDQualityName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDQualityName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Label ID="Label2" class="tdstyle" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDDesignName" runat="server" Width="150px" AutoPostBack="true"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDDesignName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDDesignName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Label ID="Label3" class="tdstyle" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDColorName" runat="server" Width="150px" AutoPostBack="true"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDColorName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDColorName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:Label ID="Label4" class="tdstyle" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDSize" runat="server" Width="150px" AutoPostBack="true" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle" runat="server" id="td22">
                                            <asp:Label ID="Label37" Text="Consumption" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:RadioButtonList runat="server" ID="RBLConsumption" RepeatDirection="Horizontal"
                                                Enabled="true" RepeatLayout="Flow" CssClass="labels">
                                                <asp:ListItem Text="On Fixed" Value="0"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="On Weight" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <%-- <td colspan="2">
                            <asp:Label ID="Label10" Text="Description" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDDescription" Width="320px" runat="server" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDDescription"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>--%>
                                    </tr>
                                </table>
                                <table>
                                    <tr id="trUnitCalType" runat="server">
                                        <td class="tdstyle">
                                            <asp:Label ID="Label13" Text=" Unit" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList ID="DDunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                Enabled="False">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDunit"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle" runat="server" id="tdcalname">
                                            <asp:Label ID="Label12" Text=" Cal Type" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Enabled="False"
                                                Width="100px">
                                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                                <asp:ListItem Value="4">L-2</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <%--<table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label11" Text=" Quality Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="ddStockQualityType" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Width="110px" OnSelectedIndexChanged="ddStockQualityType_SelectedIndexChanged">
                                <asp:ListItem Value="1">Finished</asp:ListItem>
                                <asp:ListItem Value="2">Second</asp:ListItem>
                                <asp:ListItem Value="3">Rejected</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" runat="server" id="tdcalname">
                            <asp:Label ID="Label12" Text=" Cal Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Enabled="False"
                                Width="100px">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label13" Text=" Unit" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                Enabled="False">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>                       
                    </tr>
                </table>--%>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabCarpetInformation" HeaderText="Carpet Receive" runat="server">
                        <ContentTemplate>
                            <div style="width: 100%; float: left; background-color: #E3E3E3">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div style="height: 250px; overflow: auto; width: 1200px;">
                                                <asp:GridView ID="GVCarpetReceive" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                    OnRowDataBound="GVCarpetReceive_RowDataBound" OnRowDeleting="GVCarpetReceive_RowDeleting"
                                                    ShowFooter="true">
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
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <div style="width: 300px;">
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                                </div>
                                                            </ItemTemplate>
                                                            <%--<ItemStyle HorizontalAlign="Center" Width="400px"  />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ItemName" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quality" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Design" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Color" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColor" runat="server" Text='<%#Bind("Color") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Shape" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Size" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Khap Width">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKhapWidth" runat="server" Text='<%#Bind("KhapWidth") %>' Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtKhapWidth" runat="server" Text='<%#Eval("KhapWidth") %>' OnTextChanged="txtKhapWidth_TextChanged"
                                                                    onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Khap Length">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKhapLength" runat="server" Text='<%#Bind("KhapLength") %>' Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtKhapLength" runat="server" Text='<%#Eval("KhapLength") %>' OnTextChanged="txtKhapLength_TextChanged"
                                                                    onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AfterKhapSize">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAfterKhapSize" runat="server" Text='<%#Bind("AfterKhapSize") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Font-Bold="true" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>' Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtReqQty" runat="server" Text='<%#Eval("Qty") %>' Width="40px"
                                                                    AutoPostBack="true" onFocus="this.select()" OnTextChanged="txtReqQty_TextChanged"
                                                                    onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblGrandTQty" runat="server" Font-Bold="true" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RecWt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRecWt" runat="server" Text='<%#Bind("RecWt") %>' Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtRecWt" runat="server" Text='<%#Bind("RecWt") %>' OnTextChanged="txtRecWt_TextChanged"
                                                                    onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ActualWt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActualWt" runat="server" Text='<%#Bind("ActualWt") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FinalWt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFinalWt" runat="server" Text='<%#Bind("FinalWt") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Area">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArea" runat="server" Text='<%#Bind("Area") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TotalArea">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalArea" runat="server" Text=''></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="80px" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblGrandTArea" runat="server" Font-Bold="true" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="JobName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblJobName" runat="server" Text='<%#Bind("JobName") %>' Visible="false"></asp:Label>
                                                                <asp:DropDownList ID="DDJobName" CssClass="dropdown" Width="120px" runat="server"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="DDJobName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <%--<asp:LinkButton ID="popup" runat="server" Text="Click to Contact" OnClick="popup_Click"></asp:LinkButton>                                       
                                         <asp:TextBox ID="txtJobName" runat="server" Text='<%#Eval("JobName") %>' OnTextChanged="txtJobName_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FinisherName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFinisherName" runat="server" Text='<%#Bind("FinisherName") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:DropDownList ID="DDFinisherName" CssClass="dropdown" Width="120px" runat="server"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="DDFinisherName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Penality">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPenality" runat="server" Text='<%#Bind("Penality") %>' Visible="false"></asp:Label>
                                                                <asp:LinkButton ID="popup" runat="server" Text="Add Penality" OnClick="popup_Click"></asp:LinkButton>
                                                                <%-- <asp:Label ID="lblPenalityName" runat="server" Text='<%#Bind("Penality") %>'></asp:Label> --%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PenalityName">
                                                            <ItemTemplate>
                                                                <div style="width: 120px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                    <asp:Label ID="lblPenalityName" runat="server" ToolTip='<%#Bind("Penality") %>' Text='<%#Bind("Penality") %>'></asp:Label>
                                                                </div>
                                                            </ItemTemplate>
                                                            <%-- <ItemStyle HorizontalAlign="Center" Width="0px"  />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%#Bind("Type") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FinishingSize">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfinishingSize" runat="server" Text='<%#Bind("FinishingSize") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="WOrderId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOrderId" runat="server" Text='<%#Bind("WOrderId") %>'></asp:Label>
                                                                <asp:Label ID="lblFinishedId" runat="server" Text='<%#Bind("FinishedId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("ItemId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("Qualityid") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="hnKhapWidth" runat="server" Text='<%#Bind("hnKhapWidth") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="hnKhapLength" runat="server" Text='<%#Bind("hnKhapLength") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="hnAfterKhapSize" runat="server" Text='<%#Bind("hnAfterKhapSize") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="hnArea" runat="server" Text='<%#Bind("hnArea") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblShapeId" runat="server" Text='<%#Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="Issue_Detail_Id" runat="server" Text='<%# Bind("Issue_Detail_Id") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="hnBalQty" runat="server" Text='<%# Bind("BalQty") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblComm" runat="server" Text='<%#Bind("Comm") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblOrderId" runat="server" Text='<%#Bind("OrderId") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblFlagFixOrWeight" runat="server" Text='<%#Bind("FlagFixOrWeight") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTotalLagat" runat="server" Text='<%#Bind("TotalLagat") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td id="td1" runat="server">
                                            <asp:Label ID="Label10" Text="Total Weight" runat="server" CssClass="labelbold" />
                                            <br>
                                            <asp:TextBox ID="txtTotalWeight" Width="80px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td id="td2" runat="server">
                                            <asp:Label ID="Label11" Text="Check Pcs" runat="server" CssClass="labelbold" />
                                            <br>
                                            <asp:TextBox ID="txtCheckPcs" Width="80px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td id="td3" runat="server">
                                            <asp:Label ID="Label14" Text="Check Weight" runat="server" CssClass="labelbold" />
                                            <br>
                                            <asp:TextBox ID="txtCheckWeight" Width="80px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td id="tdRemarks" runat="server">
                                            <asp:Label ID="Label25" Text="Remarks" runat="server" CssClass="labelbold" />
                                            <br>
                                            <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" TextMode="MultiLine" Height="50px"
                                                CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
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
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnWeaver" Enabled="true"
                                runat="server" Text="Weaver Report" OnClick="BtnWeaver_Click" Width="100px" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnFinisher" Enabled="true"
                                runat="server" Text="Finisher Report" OnClick="BtnFinisher_Click" Width="120px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="7">
                            <div style="height: 250px; overflow: auto; width: 950px;">
                                <asp:GridView ID="dgorder" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnSelectedIndexChanged="dgorder_SelectedIndexChanged" OnRowDataBound="dgorder_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <%--  <asp:BoundField DataField="IssueOrderId" HeaderText="WOrderNo" >

                                    <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="WOrderNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("Item") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Design" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblColor" runat="server" Text='<%#Bind("Color") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shape" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ordered Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance to Receive">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFinishedId" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("ITEM_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("ShadecolorId") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                <%-- <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>--%>
                                                <asp:Label ID="LBLUNIT" runat="server" Text='<%# Bind("unit") %>' Visible="false"></asp:Label>
                                                <%--<asp:Label ID="lblIssueOrderId" runat="server" Text='<%# Bind("IssueOrderId") %>' Visible="false"></asp:Label>--%>
                                                <asp:Label ID="Issue_Detail_Id" runat="server" Text='<%# Bind("Issue_Detail_Id") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblProdWidthFt" runat="server" Text='<%# Bind("ProdWidthFt") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblProdLengthFt" runat="server" Text='<%# Bind("ProdLengthFt") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblFinishingFtSize" runat="server" Text='<%# Bind("Finishing_Ft_Size") %>'
                                                    Visible="false"></asp:Label>
                                                <%--<asp:Label ID="lblFinishingFtArea" runat="server" Text='<%# Bind("Finishing_Ft_Area") %>' Visible="false"></asp:Label>--%>
                                                <asp:Label ID="lblFinishingFtArea" runat="server" Text='<%# Bind("Area") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblFinishingMtSize" runat="server" Text='<%# Bind("Finishing_Mt_Size") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblComm" runat="server" Text='<%# Bind("Comm") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblOrderid" runat="server" Text='<%# Bind("Orderid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblFlagFixOrWeight" runat="server" Text='<%# Bind("FlagFixOrWeight") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="hnBalanceQty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "Issue_Detail_Id").ToString()) %>'
                                                    Visible="false" />
                                                <asp:Label ID="lblbalnce" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "Issue_Detail_Id").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assign Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssignDate" runat="server" Text='<%#Bind("AssignDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
                                <%--<HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkboxitem" runat="server" AutoPostBack="true" OnCheckedChanged="Chkboxitem_CheckedChanged" />
                                    <%--onclick="return CheckBoxClick(this);"--%>
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
                                    <%-- <asp:Label ID="lblAmt" Text='' runat="server" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityId" Text='<%#Bind("PenalityId") %>' runat="server" Visible="false" />
                                    <asp:Label ID="lblPenalityType" Text='<%#Bind("PenalityType") %>' runat="server"
                                        Visible="false" />
                                    <%--<asp:Label ID="lblSrNo" Text='<%#Bind("lblSrNo") %>' runat="server"  Visible="false"/>--%>
                                    <%--<asp:Label ID="lblPPId" Text='<%#Bind("PPId") %>' runat="server" />
                                                    <asp:Label ID="lblIFinishedId" Text='<%#Bind("IFinishedId") %>' runat="server" />
                                                    <asp:Label ID="lblOFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
                                                    <asp:Label ID="lblUnitTypeId" Text='<%#Bind("UnitTypeID") %>' runat="server" />
                                                    <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId") %>' runat="server" />
                                                    <asp:Label ID="lblOrderDetailId" Text='<%#Bind("OrderDetailId") %>' runat="server" />--%>
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
