<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" EnableViewStateMac="false"
    AutoEventWireup="true" CodeFile="FrmQualityDetails.aspx.cs" Inherits="Masters_Carpet_FrmQualityDetails"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            window.location.href = "FrmQualityDetails.aspx";
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
                    var selectedindex = $("#<%=ddlType.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Type Name!!\n";
                    }
                    selectedindex = $("#<%=ddlQualityName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Quality Name. !!\n";
                    }

                    var txtissuedate = document.getElementById('<%=tbWeavingCharges.ClientID %>');
                    if (txtissuedate.value == "") {
                        Message = Message + "Please Enter Weaving Charges. !!\n";
                    }
                    var txttargetdate = document.getElementById('<%=tbCommission.ClientID %>');
                    if (txttargetdate.value == "") {
                        Message = Message + "Please Enter Commission Charges. !!\n";
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
                                    <asp:Label ID="lType" runat="server" Text="Type" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
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
                                    <asp:Label ID="lUnit" runat="server" Text="Unit" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">Sq.Yard</asp:ListItem>
                                        <asp:ListItem Value="1">Sq.Meter</asp:ListItem>
                                        <%--<asp:ListItem Value="2">Sq.Feet</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lWeavingCharges" runat="server" Text="Weaving Charges" CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="tbWeavingCharges" CssClass="textb" runat="server" Width="150px"
                                        AutoPostBack="true" onkeypress="return isNumberKey(event);" OnTextChanged="tbWeavingCharges_TextChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="lCommission" runat="server" Text="Commission" CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="tbCommission" CssClass="textb" runat="server" Width="150px" AutoPostBack="true"
                                        onkeypress="return isNumberKey(event);" OnTextChanged="tbCommission_TextChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="lProductionPrice" runat="server" Text="Production Price" CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="tbProductionPrice" CssClass="textb" runat="server" ReadOnly="true"
                                        Width="150px" />
                                </td>
                                <td>
                                    <asp:Label ID="lLossSq" runat="server" Text="Loss/SqYd(in %)" CssClass="labelbold"></asp:Label>
                                    <asp:Label ID="lLossMeter" runat="server" Text="Loss/SqMt(in %)" CssClass="labelbold"
                                        Visible="false"></asp:Label><br />
                                    <asp:TextBox ID="tbLossSqYd" CssClass="textb" runat="server" Width="150px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%; float: left; padding-top: 20px">
                        <table>
                            <tr>
                                <td style="width: 20%; vertical-align: top">
                                    <asp:Label ID="lblDesignName" Text="Design Name With Consumption" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                    <fieldset style="width: 250px;">
                                        <legend>Select Design Name</legend>
                                        <div id="loadingDesignName" style="display: none">
                                            Loding Design Name....
                                        </div>
                                        <asp:Panel ID="PDesignName" runat="server" Visible="false">
                                            <div id="divDesignName" style="display: block">
                                                <asp:CheckBoxList ID="cblDesignName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblDesignName_SelectedIndexChnaged">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                                <td style="width: 20%; vertical-align: top">
                                    <asp:Label ID="lColorName" runat="server" Text="Color Name" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="ddlColorName" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label2" runat="server" Text="Relaxation in(%)" CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="tbRelaxation" CssClass="textb" runat="server" Width="150px" onkeypress="return isNumberKey(event);" />
                                </td>
                                <td style="width: 30%; vertical-align: top">
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
                                <td style="width: 30%; vertical-align: top">
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
                                            <%--   <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                            OnRowDeleting="DGReceivedDetail_RowDeleting" 
                                            onselectedindexchanged="DGReceivedDetail_SelectedIndexChanged">--%>
                                            <asp:GridView ID="DGReceivedDetail" AutoGenerateColumns="true" runat="server" CssClass="grid-views"
                                                Width="60%" OnRowDataBound="DGReceivedDetail_RowDataBound" EmptyDataText="No. Records found."
                                                DataKeyNames="Quality1TableId" OnPageIndexChanging="DGReceivedDetail_PageIndexChanging"
                                                OnSelectedIndexChanged="DGReceivedDetail_SelectedIndexChanged" AllowPaging="True"
                                                PageSize="50">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
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
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    OnClientClick="return preventMultipleSubmissions();" />
                                <%--<asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="buttonnorm"
                                    Visible="false" OnClientClick="return cancelvalidation();" OnClick="btncancel_Click" />--%>
                                <%--<asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />--%>
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
                <asp:HiddenField ID="hnQuality1TableId" runat="server" />
                <asp:HiddenField ID="hnModeUpdate" runat="server" />
                <asp:HiddenField ID="hnQualityId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
