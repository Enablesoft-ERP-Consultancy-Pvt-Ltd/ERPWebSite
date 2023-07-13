<%@ Page Title="Dyer Direct Stock Receive" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="DyerDirectStockReceive.aspx.cs" Inherits="Masters_Process_DyerDirectStockReceive"
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
            window.location.href = "DyerDirectStockReceive.aspx";
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
                    selectedindex = $("#<%=DDDyerName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Vendor. !!\n";
                    }
                    if ($("#<%=TDChallanNo.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDChallanNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select challan No. !!\n";
                        }
                    }
                    selectedindex = $("#<%=DDItemName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Item !!\n";
                    }
                    selectedindex = $("#<%=DDQuality.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Quality. !!\n";
                    }

                    selectedindex = $("#<%=DDGivenColor.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Given Shade. !!\n";
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
                        <td id="TREdit" runat="server" visible="false">
                            <asp:CheckBox ID="chkedit" Text="Edit" CssClass="checkboxbold" Font-Size="Small"
                                AutoPostBack="true" runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                        </td>
                        <td id="TDComplete" runat="server" visible="false">
                            <asp:CheckBox ID="chkcomplete" Text="Fill Complete Challan" CssClass="checkboxbold"
                                Font-Size="Small" AutoPostBack="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblUnitId" runat="server" CssClass="labelbold" Visible="false" />
                        </td>
                        <td colspan="3">
                            <%-- <asp:RadioButton ID="rdoDyerStock" Text="Dyer Stock" CssClass="radiobuttonnormal"
                                runat="server" GroupName="OrderType" />
                            <asp:RadioButton ID="rdoStock" Text="Stock" CssClass="radiobuttonnormal" runat="server"
                                GroupName="OrderType" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text=" Company Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Dyer Name " runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" Text="Today Date" runat="server" CssClass="labelbold" />
                        </td>
                        <%-- <td class="tdstyle">
                            <asp:Label ID="Label7" Text="Required Date" runat="server" CssClass="labelbold" />
                        </td>--%>
                        <td id="TDChallanNo" runat="server" visible="false">
                            <asp:Label ID="lblChallanNo" runat="server" Text="Challan No" CssClass="labelbold"></asp:Label>
                            <br />
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
                            <asp:DropDownList CssClass="dropdown" ID="DDDyerName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAssignDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtAssignDate">
                            </asp:CalendarExtender>
                        </td>
                        <%-- <td>
                            <asp:TextBox ID="txtRequiredDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtRequiredDate">
                            </asp:CalendarExtender>
                        </td>--%>
                        <td id="TDChallanNoDD" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" Width="130px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChallanNo" runat="server" Width="90px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--<td class="tdstyle">
                            <asp:Label ID="Label5" Text=" Godown Name" runat="server" CssClass="labelbold" />
                        </td>--%>
                        <td class="tdstyle">
                            <asp:Label ID="Label25" Text="Item Name " runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label26" Text="SubItem Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label27" Text="Given ColorName" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label8" Text="LotNo" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label9" Text="Stock Qty" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <%--<td>
                            <asp:DropDownList CssClass="dropdown" ID="DDGodownName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>--%>
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
                            <asp:DropDownList CssClass="dropdown" ID="DDGivenColor" Width="100px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDGivenColor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDLotNo" Width="100px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQtyInHand" runat="server" Width="90px" CssClass="textb" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text="Godown Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text="Receive Color" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label29" Text="Qty Issue" runat="server" CssClass="labelbold" />
                        </td>
                        <%--<td class="tdstyle">
                            <asp:Label ID="Label28" Text="Receive ColorName" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label32" Text="Match Process" runat="server" CssClass="labelbold" />
                        </td> 
                        <td class="tdstyle">
                            <asp:Label ID="Label30" Text="Rate" runat="server" CssClass="labelbold" />
                        </td>
                                             
                        <td class="tdstyle">
                            &nbsp;
                        </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDGodownName" Width="100px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDReceiveColor" Width="100px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIssueQty" runat="server" Width="90px" CssClass="textb" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <%-- <td>
                          <asp:Button ID="refreshDyerColor" runat="server" Style="display: none" OnClick="refreshDyerColor_Click" />
                            <asp:DropDownList CssClass="dropdown" ID="DDReceiveColor" Width="120px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDReceiveColor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="BtnAddDyerColorRate" runat="server" CssClass="buttonsmall" OnClientClick="return AddDyerColorRate();"
                                Text="&#43;" />
                        </td>

                         <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDDyeingMatchProcess" Width="100px" runat="server">
                                <asp:ListItem Value="Cut">Cut</asp:ListItem>
                                <asp:ListItem Value="Side">Side</asp:ListItem>
                                <asp:ListItem Value="Cut/Side">Cut/Side</asp:ListItem>
                                <asp:ListItem Value="N/A">N/A</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRate" runat="server" Width="90px" CssClass="textb" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                       
                       
                        <td class="tdstyle">
                            &nbsp;
                        </td>--%>
                    </tr>
                    <%--<tr>
                        <td class="tdstyle">
                            Assign Date
                        </td>
                        <td class="tdstyle">
                            Required Date
                        </td>
                        <td class="tdstyle">
                            Unit
                        </td>
                        <td class="tdstyle">
                            Cal Type
                        </td>
                    </tr>--%>
                </table>
                <table>
                    <tr>
                        <asp:HiddenField ID="hnid" runat="server" Value="0" />
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
                                    Width="100%" OnRowEditing="DG_RowEditing" OnRowCancelingEdit="DG_RowCancelingEdit"
                                    OnRowDeleting="DG_RowDeleting" OnRowUpdating="DG_RowUpdating" OnRowDataBound="DG_RowDataBound"
                                    ShowFooter="true">
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
                                        <asp:TemplateField HeaderText="RDescription">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRitemdescription" Text='<%#Bind("Ritemdescription") %>' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lot No">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllotno" Text='<%#Bind("lotno") %>' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <EditItemTemplate>
                                                <asp:Label ID="lblqty" Text='<%#Bind("RecQty") %>' runat="server" Visible="false" />
                                                <asp:TextBox ID="txtqty" Width="70px" Text='<%#Bind("RecQty") %>' runat="server"
                                                    BackColor="Yellow" onkeypress="return isNumberKey(event);" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblqty" Text='<%#Bind("RecQty") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTQty" runat="server" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Req Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" Text='<%#Bind("Date","{0:dd-MMM-yyyy}") %>' runat="server" />
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
                                                <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
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
