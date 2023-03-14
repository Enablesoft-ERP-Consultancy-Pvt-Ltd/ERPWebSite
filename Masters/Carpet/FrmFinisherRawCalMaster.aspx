<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" EnableViewStateMac="false"
    AutoEventWireup="true" CodeFile="FrmFinisherRawCalMaster.aspx.cs" Inherits="Masters_Carpet_FrmFinisherRawCalMaster"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <style type='text/css'>
        .dvContent
        {
            width: 200px;
            height: 10px; /*overflow:scroll;*/
            overflow: auto;
        }
    </style>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmFinisherRawCalMaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        //        function addpriview() {
        //            window.open("../../ReportViewer.aspx");
        //        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDJobType.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Job Name!!\n";
                    }
                    var selectedindex = $("#<%=DDCalOption.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Cal Option Name!!\n";
                    }
                    var selectedindex = $("#<%=DDQualityType.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Type Name!!\n";
                    }
                    selectedindex = $("#<%=ddlQualityName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Quality Name. !!\n";
                    }


                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(Jscriptvalidate);
                </script>
                <span style="padding-left: 400px;">
                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label></span>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblCarpetQualityDetails" runat="server" Text="Carpet Quality Details"
                            CssClass="labelbold" ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <div style="width: 100%; float: left;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lType" runat="server" Text="Job Type" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDJobType" runat="server" CssClass="dropdown" Width="150px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDJobType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Cal Option" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDCalOption" runat="server" CssClass="dropdown" Width="150px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDCalOption_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Quality Type" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDQualityType" runat="server" AutoPostBack="true" CssClass="dropdown"
                                        Width="150px" OnSelectedIndexChanged="DDQualityType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfSelectedValue" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="lQualityName" runat="server" Text="Quality Name" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="ddlQualityName" runat="server" CssClass="dropdown" Width="150px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlQualityName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Design Name" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDDesign" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" Text="Effective Date" runat="server" CssClass="labelbold" /><span
                                        style="color: Red">*</span>
                                    <br />
                                    <asp:TextBox ID="txtEffectiveDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEffectiveDate" Format="dd-MMM-yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%; float: left; padding-top: 20px">
                        <table>
                            <tr>
                                <td style="width: 20%; vertical-align: top">
                                    &nbsp;
                                    <%-- <asp:Label ID="Label2" Text="Last Consp Date" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />                                
                                <asp:Label ID="lblLastConspDate" runat="server" CssClass="labelbold"></asp:Label>  --%>
                                </td>
                                <td style="width: 40%; vertical-align: top">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label8" Text="Raw Items" ForeColor="Red" CssClass="labelbold" runat="server" />
                                        </legend>
                                        <asp:Label ID="msg" runat="server"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="dvCustomers" style="overflow: auto; max-height: 300px; max-width: 100%">
                                                    <asp:Repeater ID="rptRawItems" runat="server" OnItemCommand="rptRawItems_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table class="tblCustomer" cellpadding="3" cellspacing="3" border="1">
                                                                <tr>
                                                                    <th style="display: none;">
                                                                        Raw Item id
                                                                    </th>
                                                                    <th>
                                                                        Raw Item Name
                                                                    </th>
                                                                    <th>
                                                                        Total Quantity
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        Unit id
                                                                    </th>
                                                                    <th>
                                                                        Unit
                                                                    </th>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr id="row" runat="server">
                                                                <td style="display: none;">
                                                                    <span class="ITEM_ID">
                                                                        <asp:Label ID="lbIemId" runat="server" Text='<%# Eval("ITEM_ID") %>' Visible="false" />
                                                                        <%# Eval("ITEM_ID")%></span>
                                                                </td>
                                                                <td>
                                                                    <span class="ITEM_NAME">
                                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%#Eval("ITEM_ID") %>'
                                                                            CommandName="subitem"><%# Eval("ITEM_NAME")%> </asp:LinkButton>
                                                                        <%--<%# Eval("ITEM_NAME")%>--%></span>
                                                                </td>
                                                                <td>
                                                                    <span class="TotalQty">
                                                                        <%--<asp:Label ID="Label4" runat="server" Text='<%# Eval("ITEM_Qty") %>' Visible="false" />--%>
                                                                        <asp:TextBox ID="TextBox1" runat="server" Width="120" Text='0' Visible="true" ReadOnly="true"
                                                                            onkeypress="return isNumberKey(event);" /></span>
                                                                </td>
                                                                <td style="display: none;">
                                                                    <span class="UnitTypeID">
                                                                        <%# Eval("UnitTypeID")%></span>
                                                                </td>
                                                                <td>
                                                                    <span class="UnitName">
                                                                        <%# Eval("UnitType")%></span>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                            <br />
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td style="width: 40%; vertical-align: top">
                                    <%--<asp:Label ID="Label7" runat="server" Text="Click Here To Add New Raw Item" CssClass="labelbold"></asp:Label>--%><br />
                                    <div id="dvRawSubItems" style="overflow: auto; max-height: 300px; max-width: 100%">
                                        <asp:Repeater ID="rptRawSubItems" runat="server">
                                            <HeaderTemplate>
                                                <table class="tblRawSubItems" cellpadding="3" cellspacing="3" border="1">
                                                    <tr>
                                                        <th style="display: none;">
                                                            Item id
                                                        </th>
                                                        <th style="display: none;">
                                                            Sub Item id
                                                        </th>
                                                        <th>
                                                            Sub Item Name
                                                        </th>
                                                        <th>
                                                            Quantity
                                                        </th>
                                                        <%--<th>
                                            Color Name
                                        </th>--%>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="display: none;">
                                                        <asp:Label ID="lblItemId" runat="server" Text='<%# Eval("ITEM_ID") %>' />
                                                    </td>
                                                    <td style="display: none;">
                                                        <asp:Label ID="lblQualityID" runat="server" Text='<%# Eval("QualityID") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblQualityName" runat="server" Text='<%# Eval("QualityName") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbqty" runat="server" Width="120" AutoPostBack="true" Text='<%# Eval("qty") %>'
                                                            onkeypress="return isNumberKey(event);" OnTextChanged="tbqty_TextChanged" />
                                                    </td>
                                                    <%--<td>
                                        <asp:TextBox ID="tbColorName" runat="server" Width="120" />
                                    </td>--%>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label1" Text="Order Description" runat="server" CssClass="labelbold"
                                ForeColor="Red" />
                        </legend>
                        <div style="width: 500px; float: left; padding-top: 20px">
                            <table width="500px">
                                <tr>
                                    <td>
                                        <div id="Div1" runat="server" style="width: 945px; max-height: 250px; overflow: auto;
                                            float: left">
                                            <%--  <asp:GridView ID="DGReceivedDetail" AutoGenerateColumns="true" runat="server" CssClass="grid-views" Width="60%"
                                            OnRowDataBound="DGReceivedDetail_RowDataBound" EmptyDataText="No. Records found."  DataKeyNames="Quality1TableId"
                                            OnPageIndexChanging="DGReceivedDetail_PageIndexChanging" OnSelectedIndexChanged="DGReceivedDetail_SelectedIndexChanged" AllowPaging="True" PageSize="50">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            
                                        </asp:GridView>--%>
                                            <asp:GridView ID="DGFinisherQualityDetail" AutoGenerateColumns="False" runat="server"
                                                CssClass="grid-views" OnSelectedIndexChanged="DGFinisherQualityDetail_SelectedIndexChanged"
                                                DataKeyNames="RawId" EmptyDataText="No. Records found." OnRowDataBound="DGFinisherQualityDetail_RowDataBound"
                                                OnRowDeleting="DGFinisherQualityDetail_RowDeleting">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Job Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobName" runat="server" Text='<%#Bind("Process_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cal Option">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCalcOption" runat="server" Text='<%#Bind("CalcName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quality Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQualityType" runat="server" Text='<%#Bind("Item_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quality Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQualityName" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Design" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("StartDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRawId" runat="server" Text='<%# Bind("RawId") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </fieldset>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="ChkForConsumptionNotDefine" class="tdstyle" runat="server" Text="Print Consumption Not Define"
                                    CssClass="checkboxbold" />
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    OnClientClick="return preventMultipleSubmissions();" />
                                <%--<asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="buttonnorm"
                                    Visible="false" OnClientClick="return cancelvalidation();" OnClick="btncancel_Click" />--%>
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
                <div>
                    <div style="width: 60%; max-height: 250px; overflow: auto; float: left">
                    </div>
                    <div style="max-height: 400px; overflow: auto; width: 39%; float: right">
                    </div>
                </div>
                <asp:HiddenField ID="hnRawId" runat="server" />
                <asp:HiddenField ID="hnModeUpdate" runat="server" />
                <asp:HiddenField ID="hnQualityId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
