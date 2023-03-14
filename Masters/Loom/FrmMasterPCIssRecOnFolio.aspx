<%@ Page Title="Issue Master PC" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmMasterPCIssRecOnFolio.aspx.cs" Inherits="Masters_Loom_FrmMasterPCIssRecOnFolio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmMasterPCIssRecOnFolio.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=BtnSave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    var selectedindex = $("#<%=DDunitname.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select unit Name!!\n";
                    }
                    var selectedindex = $("#<%=DDEmployeeName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Employee Name!!\n";
                    }
                    var selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Folio No !!\n";
                    }
                    var txtLoomNo = document.getElementById('<%=txtstockno.ClientID %>');
                    if (txtLoomNo.value == "" || txtLoomNo.value == "0") {
                        Message = Message + "Please Enter Stock No. !!\n";
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
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" CssClass="labelbold" Text="Company Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="200px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" CssClass="labelbold" Text="Unit Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDunitname" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDunitname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" CssClass="labelbold" Text="Iss Rec Type" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDIssRecType" CssClass="dropdown" Width="200px" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDIssRecType_SelectedIndexChanged">
                                    <asp:ListItem Value="0"> Issue</asp:ListItem>
                                    <asp:ListItem Value="1"> Receive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" CssClass="labelbold" Text="Employee Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDEmployeeName" CssClass="dropdown" Width="200px" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" CssClass="labelbold" Text="Folio No" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDFolioNo" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDFolioNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Iss/Rec No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtIssRecNo" CssClass="textb" runat="server" Width ="200px" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Iss/Rec Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtIssRecDate" CssClass="textb" runat="server" Width ="200px"/>
                                <asp:CalendarExtender ID="cal1" TargetControlID="TxtIssRecDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label5" Text="Enter Stock No." CssClass="labelbold" Font-Size="Small"
                                    runat="server" />
                                <br />
                                <asp:TextBox ID="txtstockno" CssClass="textb" Width="200px" runat="server" onKeypress="KeyDownHandler(event);" />
                                <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="txtstockno_TextChanged" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table style="width: 100%">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" ValidationGroup="f1"
                                OnClientClick="return Validation();" Width="50px" OnClick="BtnSave_Click" />
                             <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" Width="50px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No records found." OnRowDataBound="DG_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Iss/Rec No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssRecNo" Text='<%#Bind("IssRecNo") %>' runat="server" Width="100px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" Width="450px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="StockNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTStockNo" Text='<%#Bind("TStockNo") %>' runat="server" Width="100px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueOrderID" Text='<%#Bind("IssueOrderID") %>' runat="server" />
                                                    <asp:Label ID="lblStockNo" Text='<%#Bind("StockNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblDel" runat="server" OnClick="lbDelete_Click" ToolTip="Delete"
                                                        OnClientClick="return confirm('Do you want to delete this row?');" CausesValidation="False">Delete</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnuid" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
