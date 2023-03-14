<%@ Page Title="Process Rec Rate Update" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmProcessRecRateUpdate.aspx.cs" Inherits="Masters_Process_FrmProcessRecRateUpdate"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"> </script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">
        function isnumber(event, pointflag) {
            var keycode = event.which;
            if ((keycode > 47 && keycode <= 58) || (keycode == 46 && pointflag == true)) {
                return true;
            }
            else {
                return false;
            }
        }       
     
    </script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "ProcessIssue.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate_RequiredDate() {
            var required_date = document.getElementById('TxtRequiredDate').Value;
            var assign_date = document.getElementById('TxtAssignDate').value;
            if (assign_date < required_date) {
                alert("Required Date must Be greater than assign Date");
            }
        }
        function AddLoomDetail() {
            var answer = confirm("Do you want to ADD?")

            if (answer) {

                var a = document.getElementById('CPH_Form_hnIssueOrderId').value;
                var b = document.getElementById('CPH_Form_DDProcessName');
                var processId = b.options[b.selectedIndex].value;

                if (a == "" || a == "0") {
                    alert('Plz fill Order Detail first');
                    return false;
                }
                var left = (screen.width / 2) - (650 / 2);
                var top = (screen.height / 2) - (300 / 2);

                //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=150px');
                window.open('frmWeaverLoomDetail.aspx?a=' + a + '&b=' + processId + '', 'Loom Detail', 'width=650px, height=400px, top=' + top + ', left=' + left);
            }
        }

        function AddEmployee() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (1000 / 2);
            var top = (screen.height / 2) - (800 / 2);

            if (answer) {
                window.open('../Campany/AddFrmWeaver.aspx', '', 'width=1150px,Height=801px,top=' + top + ',left=' + left, 'resizeable=yes');
            }
        }        
        
    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl" Text=" Company Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label33" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="Process Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" Text=" Employee Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" Text=" Issue No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDIssueNo" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDIssueNo_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="tdCalType" runat="server" visible="false">
                            <asp:Label ID="Label2" Text=" Cal Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCalType" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"
                                Visible="false"></asp:Label>
                            <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnUpdate" runat="server" Text="Update" OnClick="BtnUpdate_Click"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <div style="width: 100%; max-height: 250px; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Item_Finished_Id"
                                    OnRowDataBound="DGOrderdetail_RowDataBound" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="Item" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Area" HeaderText="Area">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtRate" Text='<%#Bind("Rate") %>' Width="80px" runat="server" Font-Size="Small"
                                                    Font-Bold="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BonusRate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtBonusRate" Text='<%#Bind("BonusRate") %>' Width="80px" runat="server" Font-Size="Small"
                                                    Font-Bold="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                                <asp:Label ID="LblBonusRate" Text='<%#Bind("BonusRate") %>' runat="server" />
                                                <asp:Label ID="lblItem_Finished_Id" Text='<%#Bind("Item_Finished_Id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
