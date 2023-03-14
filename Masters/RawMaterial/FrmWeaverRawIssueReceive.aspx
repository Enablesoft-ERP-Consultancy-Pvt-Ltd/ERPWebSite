<%@ Page Title="Weaver Raw Issue Receive" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmWeaverRawIssueReceive.aspx.cs" Inherits="Masters_RawMaterial_FrmWeaverRawIssueReceive"
    EnableEventValidation="false" %>

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
            window.location.href = "FrmWeaverRawIssueReceive.aspx";
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
                    selectedindex = $("#<%=DDWeaverName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select WeaverName. !!\n";
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
    <script type="text/javascript">
        function AddDyerColorRate() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (601 / 2);
            var top = (screen.height / 2) - (450 / 2);

            if (answer) {
                window.open('AddDyerColorRate.aspx', '', 'width=601px,Height=450px,top=' + top + ',left=' + left);
            }
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
                            <asp:Label ID="lblUnitId" runat="server" CssClass="labelbold" Visible="false" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text=" Company Name" runat="server" CssClass="labelbold" /><b
                                style="color: Red">*</b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Weaver Name " runat="server" CssClass="labelbold" /><b
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
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDWeaverName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDWeaverName_SelectedIndexChanged">
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
                    <tr>
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
                    <tr>
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
                    <tr>
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
                    <tr>
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
                                    Width="100%" OnRowDeleting="DG_RowDeleting" ShowFooter="true" OnRowDataBound="DG_RowDataBound">
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
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:CommandField ShowEditButton="True" ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />--%>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Width="50px" Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
