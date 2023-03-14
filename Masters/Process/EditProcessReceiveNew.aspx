<%@ Page Language="C#" Title="EditProcessReceiveNew" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="EditProcessReceiveNew.aspx.cs"
    Inherits="Masters_process_EditProcessReceiveNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "EditProcessReceiveNew.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
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
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('div2') != null) {
                xPos = $get('div2').scrollLeft;
                yPos = $get('div2').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('div2') != null) {
                $get('div2').scrollLeft = xPos;
                $get('div2').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <script type="text/javascript">
        function ConfirmSave() {
            var Ok = confirm('Are you sure want to submit final bazaar?');
            if (Ok) {
                //$("#hdnbox").val('Yes');
                return true;
            }
            else {
                //$("#hdnbox").val('No');
                document.getElementById('<%=chkFinalBazaar.ClientID %>').checked = false;
                return false;
            }
        }

    </script>
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
                <table style="padding-bottom: 10px">
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
                            <asp:Label ID="Label5" Text="CompanyName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" AutoPostBack="true" runat="server" Width="200px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle" id="tdProcess" runat="server">
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
                            <asp:Label ID="Label10" Text="Challan No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDChallanNo" AutoPostBack="true" runat="server" Width="100px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDChallanNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle" align="left">
                            <asp:Label ID="Label7" Text=" Vendor Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDEmployeeNamee" runat="server" AutoPostBack="true" Width="250px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDEmployeeNamee_SelectedIndexChanged">
                            </asp:DropDownList>
                            <%-- <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmployeeNamee"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>--%>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label9" Text="Rec.Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRecDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            <span style="float: right;">&nbsp;<asp:CheckBox ID="chkboxSampleFlag" runat="server"
                                AutoPostBack="true" OnCheckedChanged="chkboxSampleFlag_CheckedChanged" />
                                &nbsp;<asp:Label ID="Label39" runat="server" Text="CHECK FOR SAMPLE" CssClass="labelbold"></asp:Label>
                                &nbsp;<asp:CheckBox ID="chkboxRoundFullArea" runat="server" />
                                &nbsp;<asp:Label ID="Label38" runat="server" Text="Check For Full Area When Round"
                                    CssClass="labelbold"></asp:Label>
                                &nbsp;<asp:CheckBox ID="chkboxManualWeight" runat="server" AutoPostBack="True" OnCheckedChanged="chkboxManualWeight_CheckedChanged" />
                                &nbsp;<asp:Label ID="Label40" runat="server" Text="Check For Manual Weight Entry"
                                    CssClass="labelbold"></asp:Label><br />
                                <asp:Label ID="lblTotalTDS" runat="server" CssClass="labelbold" Visible="false" />
                                <span style="margin-left: 145px">
                                    <asp:CheckBox ID="CBTDS" runat="server" />
                                    &nbsp;<asp:Label ID="Label17" runat="server" Text="CHECK FOR TDS" CssClass="labelbold"></asp:Label><br />
                                </span></span>
                            <asp:Label ID="Label8" Text="Challan No" runat="server" CssClass="labelbold" Visible="false" />
                            <br />
                            <asp:TextBox ID="TxtChallanNo" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                Visible="false"></asp:TextBox>
                        </td>
                        <td id="tdTds" runat="server">
                        </td>
                    </tr>
                </table>
                <table cellspacing="5" cellpadding="5" width="100%">
                    <tr>
                        <td>
                            <asp:TabContainer ID="tbsample" runat="server" Width="100%" ActiveTabIndex="1">
                                <asp:TabPanel ID="TabMainInformation" HeaderText="Weaver/Contractor Information"
                                    runat="server">
                                    <ContentTemplate>
                                        <div style="width: 100%; float: left; background-color: #E3E3E3">
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
                                                            Enabled="false" RepeatLayout="Flow" CssClass="labels">
                                                            <asp:ListItem Text="On Fixed" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="On Weight" Value="1"></asp:ListItem>
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
                                                <tr>
                                                    <td colspan="8">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div style="min-height: 250px; overflow: auto; width: 950px;">
                                                                        <asp:GridView ID="dgorder" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                                            OnSelectedIndexChanged="dgorder_SelectedIndexChanged" OnRowDataBound="dgorder_RowDataBound">
                                                                            <HeaderStyle CssClass="gvheaders" />
                                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                                            <RowStyle CssClass="gvrow" />
                                                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                            <Columns>
                                                                                <%--<asp:BoundField DataField="IssueOrderId" HeaderText="WOrderNo" >
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
                                                                                        <asp:Label ID="lblFinishingFtArea" runat="server" Text='<%# Bind("Finishing_Ft_Area") %>'
                                                                                            Visible="false"></asp:Label>
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
                                                                <td runat="server" id="tdordergrid">
                                                                    <div id="Div1" runat="server" style="height: 250px; overflow: auto; width: 250px;">
                                                                        <asp:HiddenField ID="hnBalQty" runat="server" Visible="false" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
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
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabCarpetInformation" HeaderText="Carpet Receive" runat="server">
                                    <ContentTemplate>
                                        <div style="width: 100%; float: left; background-color: #E3E3E3">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <div style="max-height: 250px; overflow: auto; width: 1200px">
                                                            <asp:GridView ID="GVCarpetReceive" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                                OnRowDataBound="GVCarpetReceive_RowDataBound" OnRowDeleting="GVCarpetReceive_RowDeleting"
                                                                ShowFooter="true">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <FooterStyle CssClass="gvrow" />
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblText" runat="server" Text="Add"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
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
                                                                                AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Khap Length">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblKhapLength" runat="server" Text='<%#Bind("KhapLength") %>' Visible="false"></asp:Label>
                                                                            <asp:TextBox ID="txtKhapLength" runat="server" Text='<%#Eval("KhapLength") %>' OnTextChanged="txtKhapLength_TextChanged"
                                                                                AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
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
                                                                                onFocus="this.select()" AutoPostBack="true" OnTextChanged="txtReqQty_TextChanged"
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
                                                                                AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
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
                                                <tr>
                                                    <td>
                                                        <div id="div2" style="max-height: 250px; overflow: auto; width: 1200px; padding-top: 30px;
                                                            padding-bottom: 20px">
                                                            <asp:GridView ID="GVCarpetReceiveEdit" AutoGenerateColumns="False" runat="server"
                                                                CssClass="grid-views" OnRowDataBound="GVCarpetReceiveEdit_RowDataBound" OnRowDeleting="GVCarpetReceiveEdit_RowDeleting"
                                                                ShowFooter="true" OnRowCancelingEdit="GVCarpetReceiveEdit_RowCancelingEdit" OnRowEditing="GVCarpetReceiveEdit_RowEditing"
                                                                OnRowUpdating="GVCarpetReceiveEdit_RowUpdating">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <FooterStyle CssClass="gvrow" />
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblText" runat="server" Text="Edit"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                                CommandName="Delete"></asp:LinkButton></span>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                                                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Sr.No">
                                                                        <ItemTemplate>
                                                                            <%#Container.DisplayIndex+1 %>
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
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("Item") %>'></asp:Label>
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
                                                                            <asp:Label ID="lblKhapWidth" runat="server" Text='<%#Bind("Width") %>' Visible="true"></asp:Label>
                                                                            <%--  <asp:TextBox ID="txtKhapWidth" runat="server" Text='<%#Eval("Width") %>' OnTextChanged="txtKhapWidth_TextChanged"
                                            AutoPostBack="True" Width="50px"></asp:TextBox>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtWidthEdit" runat="server" Text='<%#Eval("Width") %>' Width="50px"
                                                                                OnTextChanged="txtWidthEdit_TextChanged" onkeypress="return isNumberKey1(event);"
                                                                                AutoPostBack="True" onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Khap Length">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblKhapLength" runat="server" Text='<%#Bind("Length") %>' Visible="true"></asp:Label>
                                                                            <%--  <asp:TextBox ID="txtKhapLength" runat="server" Text='<%#Eval("KhapLength") %>' OnTextChanged="txtKhapLength_TextChanged"
                                            AutoPostBack="True" Width="50px"></asp:TextBox>  --%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtLengthEdit" runat="server" Text='<%#Eval("Length") %>' Width="50px"
                                                                                onkeypress="return isNumberKey1(event);" OnTextChanged="txtLengthEdit_TextChanged"
                                                                                AutoPostBack="True" onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AfterKhapSize">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAfterKhapSize" runat="server" Text='<%#Bind("CalcSize") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtAfterKhapSizeEdit" runat="server" Text='<%#Eval("CalcSize") %>'
                                                                                Width="100px" OnTextChanged="txtAfterKhapSizeEdit_TextChanged" AutoPostBack="True"
                                                                                onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>' Visible="true"></asp:Label>
                                                                            <%--<asp:TextBox ID="txtReqQty" runat="server" Text='<%#Eval("Qty") %>' Width="40px" AutoPostBack="true" OnTextChanged="txtReqQty_TextChanged" onkeypress="return isNumberKey(event);"></asp:TextBox>  --%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblQtyEdit" runat="server" Text='<%#Bind("Qty") %>' Visible="false"></asp:Label>
                                                                            <asp:TextBox ID="txtReqQtyEdit" runat="server" Text='<%#Eval("Qty") %>' Width="40px"
                                                                                AutoPostBack="true" OnTextChanged="txtReqQtyEdit_TextChanged" onkeypress="return isNumberKey(event);"
                                                                                onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGrandTQty" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="RecWt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRecWt" runat="server" Text='<%#Bind("Weight") %>' Visible="true"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblRecWtEdit" runat="server" Text='<%#Bind("Weight") %>' Visible="false"></asp:Label>
                                                                            <asp:TextBox ID="txtRecWtEdit" runat="server" Text='<%#Eval("Weight") %>' Width="40px"
                                                                                AutoPostBack="true" OnTextChanged="txtRecWtEdit_TextChanged" onkeypress="return isNumberKey1(event);"
                                                                                onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGrandTWt" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="ActualWt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblActualWtEdit" runat="server" Text='<%#Bind("ActualWt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGrandTActualWt" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FinalWt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFinalWtEdit" runat="server" Text='<%#Bind("FinalWt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGrandTFinalWt" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
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
                                                                    <asp:TemplateField HeaderText="Rate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtRateEdit" runat="server" Text='<%#Eval("Rate") %>' Width="40px"
                                                                                AutoPostBack="true" OnTextChanged="txtRateEdit_TextChanged" onkeypress="return isNumberKey1(event);"
                                                                                onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Amt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalAmt" runat="server" Text=''></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGrandTotalAmt" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Lagat">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLagat" runat="server" Text='<%#Bind("BazaarConsump") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalLagat" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblComm" runat="server" Text='<%#Bind("Comm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCommEdit" runat="server" Text='<%#Eval("Comm") %>' Width="40px"
                                                                                onkeypress="return isNumberKey1(event);" onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalComm" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="JobName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblJobName" runat="server" Text='<%#Bind("Process_Name") %>' Visible="true"></asp:Label>
                                                                            <asp:Label ID="lblJobNameDel" runat="server" Text='<%#Bind("FinisherJobId") %>' Visible="false"></asp:Label>
                                                                            <%--<asp:LinkButton ID="popup" runat="server" Text="Click to Contact" OnClick="popup_Click"></asp:LinkButton>                                       
                                         <asp:TextBox ID="txtJobName" runat="server" Text='<%#Eval("JobName") %>' OnTextChanged="txtJobName_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblJobNameEdit" runat="server" Text='<%#Bind("FinisherJobId") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:DropDownList ID="DDJobNameEdit" CssClass="dropdown" Width="120px" runat="server"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="DDJobNameEdit_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FinisherName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFinisherName" runat="server" Text='<%#Bind("EmpName") %>' Visible="true"></asp:Label>
                                                                            <asp:Label ID="lblFinisherNameDel" runat="server" Text='<%#Bind("FinisherNameId") %>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblFinisherNameEdit" runat="server" Text='<%#Bind("FinisherNameId") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:DropDownList ID="DDFinisherNameEdit" CssClass="dropdown" Width="120px" runat="server"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="DDFinisherNameEdit_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" Text='<%#Bind("Type") %>'></asp:Label>
                                                                            <asp:Label ID="lblTypeId" runat="server" Text='<%#Bind("TDSType") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:Label ID="lblTypeEdit" runat="server" Text='<%#Bind("TDSType") %>' Visible="false"></asp:Label>
                                                                            <asp:DropDownList ID="DDType" CssClass="dropdown" Width="120px" runat="server" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="DDType_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FinishingSize">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfinishingSize" runat="server" Text='<%#Bind("FinishingMtSize") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtfinishingSizeEdit" runat="server" Text='<%#Eval("FinishingMtSize") %>'
                                                                                Width="90px" onFocus="this.select()"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Penality" Visible="false">
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
                                                                                <asp:Label ID="lblPenalityName" runat="server" ToolTip='<%#Bind("PRemarks") %>' Text='<%#Bind("PRemarks") %>'></asp:Label>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <%-- <ItemStyle HorizontalAlign="Center" Width="0px"  />--%>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PenAmt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPenAmt" runat="server" Text='<%#Bind("Penality") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalPenAmt" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="TDS Amt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTDSAmt" runat="server" Text='<%#Bind("TDSAmt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalTDS" runat="server" Font-Bold="true" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="TotalLagat" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalLagatEdit" runat="server" Visible="true"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="WOrderId" Visible="false">
                                                                        <ItemTemplate>
                                                                            <%-- <asp:Label ID="lblWOrderId" runat="server" Text='<%#Bind("WOrderId") %>'></asp:Label>--%>
                                                                            <asp:Label ID="hnlblQty" runat="server" Text='<%#Bind("Qty") %>' Visible="true"></asp:Label>
                                                                            <asp:Label ID="lblFinishedId" runat="server" Text='<%#Bind("FinishedId") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("Item_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("Qualityid") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="hnKhapWidth" runat="server" Text='<%#Bind("hnKhapWidth") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="hnKhapLength" runat="server" Text='<%#Bind("hnKhapLength") %>' Visible="false"></asp:Label>
                                                                            <%--<asp:Label ID="hnAfterKhapSize" runat="server" Text='<%#Bind("CalcSize") %>' Visible="false"></asp:Label>--%>
                                                                            <asp:Label ID="hnAfterKhapSize" runat="server" Text='<%#Bind("AfterKhapSize") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="hnArea" runat="server" Text='<%#Bind("Area") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblShapeId" runat="server" Text='<%#Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="Issue_Detail_Id" runat="server" Text='<%# Bind("Issue_Detail_Id") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="hnFinisherJobID" runat="server" Text='<%#Bind("FinisherJobId") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="hnFinisherNameID" runat="server" Text='<%#Bind("FinisherNameId") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <%--<asp:Label ID="hnBalQty" runat="server" Text='<%# Bind("BalQty") %>' Visible="false"></asp:Label>--%>
                                                                            <%--<asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblComm" runat="server" Text='<%#Bind("Comm") %>' Visible="false"></asp:Label>--%>
                                                                            <asp:Label ID="lblProcessRecDetailId" runat="server" Text='<%#Bind("Process_Rec_Detail_Id") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblOrderId" runat="server" Text='<%#Bind("OrderId") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblFlagFixOrWeight" runat="server" Text='<%#Bind("FlagFixOrWeight") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblProcessRecId" runat="server" Text='<%#Bind("Process_Rec_Id") %>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                        <td colspan="4" id="tdRemarks" runat="server">
                            <asp:Label ID="Label25" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" TextMode="MultiLine" Height="50px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>--%>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td id="td9" runat="server">
                                                        <asp:Label ID="Label23" Text="Tharri" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTharri" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtTharri_TextChanged" onkeydown="return (event.keyCode!=13);"
                                                            BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td10" runat="server">
                                                        <asp:Label ID="Label24" Text="Lachhi" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtLachhi" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtLachhi_TextChanged" onkeydown="return (event.keyCode!=13);"
                                                            BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td11" runat="server">
                                                        <asp:Label ID="Label25" Text="Tar" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTar" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtTar_TextChanged" onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td12" runat="server">
                                                        <asp:Label ID="Label27" Text="Misc" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtMisc" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtMisc_TextChanged" onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td13" runat="server">
                                                        <asp:Label ID="Label28" runat="server" Text="Extra" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtExtra" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtExtra_TextChanged" onkeydown="return (event.keyCode!=13);"
                                                            BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td14" runat="server">
                                                        <asp:Label ID="Label29" runat="server" Text="Lagat" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtLagat" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtLagat_TextChanged" onkeydown="return (event.keyCode!=13);"
                                                            BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td15" runat="server">
                                                        <asp:Label ID="Label30" runat="server" Text="Loss" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtLoss" Width="80px" runat="server" CssClass="textb" AutoPostBack="true"
                                                            OnTextChanged="txtLoss_TextChanged" onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td23" runat="server">
                                                        <asp:Label ID="lblTotalLagat" runat="server" CssClass="labelbold" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td id="td1" runat="server">
                                                        <asp:Label ID="Label11" Text="Total Weight" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalWeight" Width="80px" runat="server" CssClass="textb" onkeypress="return isNumberKey1(event);"
                                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="td2" runat="server">
                                                        <asp:Label ID="Label14" Text="Check Pcs" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtCheckPcs" Width="80px" runat="server" CssClass="textb" onkeypress="return isNumberKey1(event);"
                                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="td3" runat="server">
                                                        <asp:Label ID="Label15" Text="Check Weight" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtCheckWeight" Width="80px" runat="server" CssClass="textb" onkeypress="return isNumberKey1(event);"
                                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="tdRemarks" runat="server">
                                                        <asp:Label ID="Label16" Text="Remarks" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" TextMode="MultiLine" Height="50px"
                                                            CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td id="td4" runat="server">
                                                        <asp:Label ID="Label18" Text="Total Amount" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalAmt" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td5" runat="server">
                                                        <asp:Label ID="Label19" Text="Penality" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalPen" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td6" runat="server">
                                                        <asp:Label ID="Label20" Text="Comm" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalComm" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td7" runat="server">
                                                        <asp:Label ID="Label21" Text="TDS Amt" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalTDSAmt" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td8" runat="server">
                                                        <asp:Label ID="Label22" runat="server" Text="Net Amt" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalNetAmt" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td id="td16" runat="server">
                                                        <asp:Label ID="Label31" Text="Total Tharri" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalTharri" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td17" runat="server">
                                                        <asp:Label ID="Label32" Text="Total Lachhi" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalLachhi" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td18" runat="server">
                                                        <asp:Label ID="Label33" Text="Total Tar" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalTar" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td19" runat="server">
                                                        <asp:Label ID="Label34" Text="Total Misc" runat="server" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalMisc" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td20" runat="server">
                                                        <asp:Label ID="Label35" runat="server" Text="Total Extra" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalExtra" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <td id="td21" runat="server">
                                                        <asp:Label ID="Label36" runat="server" Text="Total Lagat" CssClass="labelbold" />
                                                        <br>
                                                        <asp:TextBox ID="txtTotalLagat" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                                                    </td>
                                                    <%--<td id="td22" runat="server">
                                            <asp:Label ID="Label37" runat="server" Text="Total Loss" CssClass="labelbold" />
                                            <br>
                                            <asp:TextBox ID="txtTotalLoss" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>--%>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                            </asp:TabContainer>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="4">
                            <%--<asp:Label ID="Label25" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                            <span style="color: Red; font-weight: bold">Note:-Please Update Penality at Last</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="right">
                            <asp:Label ID="llMessageBox" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                            <asp:CheckBox ID="chkForSlip" runat="server" Text="For Slip Print" CssClass="labelnormalMM"
                                Font-Bold="true" Visible="false" />
                            <asp:CheckBox ID="chkFinalBazaar" runat="server" Text="CHECK FOR FINAL BAZAAR" onchange="ConfirmSave();"
                                ClientIDMode="Static" />
                            <%--<asp:CheckBox ID="CheckBox1" runat="server" Text="CHECK FOR FINAL BAZAAR" AutoPostBack="true"  OnCheckedChanged="chkFinalBazaar_CheckedChanged"  />--%>
                            <%--  &nbsp;<asp:Label ID="lblCheckForFinalBazaar" runat="server" Text="CHECK FOR FINAL BAZAAR" CssClass="labelbold"></asp:Label>--%>
                            &nbsp
                            <%--  <asp:Button CssClass="buttonnorm" ID="BtnFinalBazaar" runat="server" Text="Final Bazaar" Visible="true"
                                OnClick="BtnFinalBazaar_Click" OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="100px" />--%>
                            &nbsp
                            <asp:Button CssClass="buttonnorm" ID="BtnDryWeight" runat="server" Text="Dry Weight"
                                OnClick="BtnDryWeight_Click" OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="100px" />
                            <%-- <asp:Button CssClass="buttonnorm" ID="BtnUpdateTDS" runat="server" Text="Update TDS"
                                OnClick="BtnUpdateTDS_Click" OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="100px" />--%>
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnWeaver" Enabled="true"
                                Visible="true" runat="server" Text="Weaver Report" OnClick="BtnWeaver_Click"
                                Width="100px" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnFinisher" Enabled="true"
                                Visible="true" runat="server" Text="Finisher Report" OnClick="BtnFinisher_Click"
                                Width="120px" />
                            <asp:Button CssClass="buttonnorm" ID="BtnCancelChallan" runat="server" Text="Cancel Challan"
                                OnClick="BtnCancelChallan_Click" OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="100px" />
                            <%-- <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"  UseSubmitBehavior="false"
                                 OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';" Width="50px" />
                           &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" Enabled="false"
                                runat="server" Text="Preview" OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm " ID="BtnGatePass" Enabled="false" runat="server"
                                Text="GatePass" OnClick="BtnGatePass_Click" Width="100px" />
                            &nbsp;<asp:Button ID="btnqcchkpreview" runat="server" CssClass="buttonnorm" Text="QCReport"
                                OnClick="btnqcchkpreview_Click" Width="100px" />--%>
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
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
            <asp:LinkButton ID="lnkDryWeight" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" BehaviorID="mpe1" runat="server"
                PopupControlID="pnlPopupDry" TargetControlID="lnkDryWeight" BackgroundCssClass="modalBackground"
                CancelControlID="btnHide">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlPopupDry" runat="server" CssClass="modalPopup" Style="display: none;
                width: 900px">
                <div class="header">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkboxDryWeight" runat="server" AutoPostBack="true" OnCheckedChanged="ChkboxDryWeight_CheckedChanged" />
                                Check For Dried Weight Entry
                            </td>
                            <td>
                                <asp:TextBox ID="txtDryWeightDate" Width="90px" BackColor="White" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtDryWeightDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDDryType" CssClass="dropdown" Width="120px" runat="server">
                                    <asp:ListItem Value="0" Text="Already Filled"></asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1" Text="Not Filled"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GVDryWeight" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        Width="650px" OnRowDataBound="GVDryWeight_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts1" />
                        <RowStyle CssClass="gvrow1" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="Challan No">
                                <ItemTemplate>
                                    <asp:Label ID="lblChallanNo" Text='<%#Bind("ChallanNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emp Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiveDate" Text='<%#Bind("ReceiveDate") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QualityType">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualityType" Text='<%#Bind("Item_Name") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TotalPcs" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalQty" Text='<%#Bind("TotalQty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TotalWeight">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalWeight" Text='<%#Bind("TotalWeight") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TotalWeight2" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTotalWeight" Width="50px" BackColor="White" runat="server" Text='<%#Bind("TotalWeight") %>'
                                        onkeypress="return isNumberKey1(event);" />
                                    <%--<asp:Label ID="lblTotalWeight" Text='<%#Bind("TotalWeight") %>' runat="server" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CheckPcs">
                                <ItemTemplate>
                                    <asp:Label ID="lblCheckPcs" Text='<%#Bind("CheckPcs") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CheckPcs2" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCheckPcs" Width="50px" BackColor="White" runat="server" Text='<%#Bind("CheckPcs") %>'
                                        onkeypress="return isNumberKey(event);" />
                                    <%-- <asp:Label ID="lblCheckPcs" Text='<%#Bind("CheckPcs") %>' runat="server" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CheckWeight">
                                <ItemTemplate>
                                    <asp:Label ID="lblCheckWeight" Text='<%#Bind("CheckWeight") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CheckWeight2" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCheckWeight" Width="50px" BackColor="White" runat="server" Text='<%#Bind("CheckWeight") %>'
                                        onkeypress="return isNumberKey1(event);" />
                                    <%-- <asp:Label ID="lblCheckWeight" Text='<%#Bind("CheckWeight") %>' runat="server" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dried WT">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDriedWeight" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        Text='<%#Bind("DryWeight") %>' OnTextChanged="txtDriedWeight_TextChanged" onkeypress="return isNumberKey1(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DeductionIn Per">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeductionInPer" Text='<%#Bind("DeductionInPer") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblProcessRecId" Text='<%#Bind("Process_Rec_Id") %>' runat="server"
                                        Visible="false" />
                                    <asp:Label ID="lblEmpId" Text='<%#Bind("EmpId") %>' runat="server" Visible="false" />
                                    <asp:Label ID="lblItemId" Text='<%#Bind("Item_Id") %>' runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="header">
                    <asp:Button ID="BtnSaveDryWeight" runat="server" Text="Submit" OnClick="BtnSaveDryWeight_Click" />
                    <asp:Button ID="btnHide1" runat="server" Text="Close" />
                    <asp:HiddenField ID="hdnbox" runat="server" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        #mask
        {
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 4;
            opacity: 0.4;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
            filter: alpha(opacity=40); /* second!*/
            background-color: Gray;
            display: none;
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowPopup() {
            $('#mask').show();
            $('#<%=pnlPopPass.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlPopPass.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlPopPass" runat="server" BackColor="White" Height="175px" Width="300px"
        Style="z-index: 111; background-color: White; position: absolute; left: 35%;
        top: 40%; border: outset 2px gray; padding: 5px; display: none">
        <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
            <tr style="background-color: #8B7B8B; height: 1px">
                <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                    align="center">
                    ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                        class="btnPwd" href="#">X</a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Enter Password:
                </td>
                <td>
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
