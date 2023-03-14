<%@ Page Title="Finisher Raw Issue Receive" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/ERPmaster.master" CodeFile="FrmFinisherRawIssueReceive.aspx.cs"
    Inherits="Masters_RawMaterial_FrmFinisherRawIssueReceive" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"> </script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <%-- <script type="text/javascript">

        function SaveData() {

            var id = document.getElementById('<%=hncomp.ClientID %>').value;

            if (id != "20") {
                var answer = confirm("Do you want to Save?")

                if ((answer)) {
                    return true;
                }
                else {
                    return false;
                }

            }
            return true;
        }


        function UpdateData() {

            var id = document.getElementById('CPH_Form_hncomp').value;
            if (id != "20") {
                alert('Do you want to Update?');
                return false;
            }
        }      
    
    </script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmFinisherRawIssueReceive.aspx";
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
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDJobType.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Job Name. !!\n";
                    }
                    selectedindex = $("#<%=DDFinisherName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select FinisherName. !!\n";
                    }
                    selectedindex = $("#<%=DDItemName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Item !!\n";
                    }
                    selectedindex = $("#<%=DDQuality.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Quality. !!\n";
                    }
                    selectedindex = $("#<%=DDShadeColor.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select ShadeColor. !!\n";
                    }
                    selectedindex = $("#<%=DDGodownName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Godown !!\n";
                    }
                    selectedindex = $("#<%=DDLotNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Lot No !!\n";
                    }
                    var txtrecqty = document.getElementById('<%=txtIssueQty.ClientID %>');
                    if (txtrecqty.value == "" || txtrecqty.value == "0") {
                        Message = Message + "Please Enter QTY. !!\n";
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
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(Jscriptvalidate);
                </script>
                <table>
                    <tr>
                        <td colspan="2">
                            &nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" Check For Edit" CssClass="checkboxbold"
                                AutoPostBack="true" OnCheckedChanged="ChKForEdit_CheckedChanged" /><br />
                        </td>
                        <td>
                            <asp:CheckBox ID="ChkForDate" runat="server" Text=" Check For UpdateDate" CssClass="checkboxbold"
                                AutoPostBack="true" OnCheckedChanged="ChKForEdit_CheckedChanged" /><br />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblUnitId" runat="server" CssClass="labelbold" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" id="TDLabelChallanNo" runat="server" visible="false">
                            <asp:Label ID="Label10" Text=" ChallanNo" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text=" Company Name" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" Text="Job Type " runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Finisher Name " runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" Text="Tran Type" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label7" Text="Tran Date" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" Text="Challan No" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td id="TDTxtChallanNo" runat="server" visible="false">
                            <asp:TextBox ID="txtEditChallanNo" runat="server" Width="150px" CssClass="textb"
                                OnTextChanged="txtEditChallanNo_TextChanged" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDJobType" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDJobType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDFinisherName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDFinisherName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDTranType" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDTranType_SelectedIndexChanged">
                                <asp:ListItem Text="Issue" Value="0">Issue</asp:ListItem>
                                <asp:ListItem Text="Receive" Value="1">Receive</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAssignDate" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtAssignDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChallanNo" runat="server" Width="150px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="TR2" runat="server" visible="true">
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text=" Quality Type" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text=" Godown Name" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label25" Text="Item Name " runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label26" Text="SubItem Name" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="TR3" runat="server" visible="true">
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDQualityType" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDGodownName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblRate" runat="server" CssClass="labelbold" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id="TR4" runat="server" visible="true">
                        <td class="tdstyle">
                            <asp:Label ID="Label27" Text="Shade ColorName" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label8" Text="LotNo" runat="server" CssClass="labelbold" /><b style="color: Red">*</b>
                        </td>
                        <td class="tdstyle" id="lblStockQty" runat="server">
                            <asp:Label ID="Label9" Text="Stock Qty" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label29" Text="Qty Issue" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="TR5" runat="server" visible="true">
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDShadeColor" Width="150px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDShadeColor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDLotNo" Width="150px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="txtStockQty" runat="server">
                            <asp:TextBox ID="txtQtyInHand" runat="server" Width="150px" CssClass="textb" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIssueQty" runat="server" Width="150px" CssClass="textb" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            &nbsp;<asp:HiddenField ID="hnid" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <span style="color: Red">* Fields are mandatory</span>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <b>
                                <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                                <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                                <asp:Button ID="btnpreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="btnpreview_Click" />
                                <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return NewForm();" />
                                <asp:Button ID="btncancelorder" CssClass="buttonnorm" Text="Cancel Order" runat="server"
                                    OnClick="btncancelorder_Click" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"
                                Visible="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
                                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    Width="100%" OnRowDeleting="DG_RowDeleting" ShowFooter="true" OnRowDataBound="DG_RowDataBound"
                                    OnRowEditing="DG_RowEditing" OnRowCancelingEdit="DG_RowCancelingEdit" OnRowUpdating="DG_RowUpdating"
                                    DataKeyNames="TranDetailid">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <FooterStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QualityType">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityType" Text='<%#Bind("QualityType") %>' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" Text='<%#Bind("ItemName") %>' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ColorName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShadeColorName" Text='<%#Bind("ShadeColorName") %>' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <%-- <EditItemTemplate>
                                            <asp:Label ID="lblqty" Text='<%#Bind("issueqty") %>' runat="server" Visible="false" />
                                                <asp:TextBox ID="txtqty" Width="70px" Text='<%#Bind("Issueqty") %>' runat="server"
                                                    BackColor="Yellow" onkeypress="return isNumberKey(event);" />
                                            </EditItemTemplate>--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssRecqty" Text='<%#Bind("IssRecQty") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtIssRecQty" Text='<%#Bind("IssRecQty") %>' onkeypress="return isNumber(event);"
                                                    Width="80px" runat="server" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTQty" runat="server" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LotNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGodown" Text='<%#Bind("GodownName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTranid" Text='<%#Bind("TranId") %>' runat="server" />
                                                <asp:Label ID="lblTrandetailid" Text='<%#Bind("TranDetailid") %>' runat="server" />
                                                <asp:Label ID="lblFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
                                                <asp:Label ID="lblTranType" Text='<%#Bind("TranType") %>' runat="server" />
                                                <asp:Label ID="lblGodownId" Text='<%#Bind("GodownId") %>' runat="server" />
                                                <asp:Label ID="lblQualityTypeId" Text='<%#Bind("QualityTypeId") %>' runat="server" />
                                                <asp:Label ID="lblUnitId" Text='<%#Bind("UnitId") %>' runat="server" />
                                                <asp:Label ID="lblhnIssRecQty" Text='<%#Bind("IssRecQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:CommandField ShowEditButton="True" ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />--%>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Width="50px" Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />
                                        </ItemTemplate>
                                        </asp:TemplateField>   --%>
                                        <%--<asp:CommandField ShowEditButton="True" HeaderText="Edit" ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Left" Visible="false" />--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
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
                        $('#<%=pnlpopup.ClientID %>').show();
                    }
                    function HidePopup() {
                        $('#mask').hide();
                        $('#<%=pnlpopup.ClientID %>').hide();
                    }
                    $(".btnPwd").live('click', function () {
                        HidePopup();
                    });
                </script>
                <div id="mask">
                </div>
                <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
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
                                <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px"
                                    OnTextChanged="txtpwd_TextChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                                <input type="button" value="Cancel" class="btnPwd" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
