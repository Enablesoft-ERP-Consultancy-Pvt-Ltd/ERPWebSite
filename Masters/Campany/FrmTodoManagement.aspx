<%@ Page Title="To-do Management" Language="C#" AutoEventWireup="true" CodeFile="FrmTodoManagement.aspx.cs"
    Inherits="Masters_Campany_FrmTodoManagement" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "FrmTodoManagement.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function SaveValidation() {
            if (document.getElementById('CPH_Form_DDCompanyName') != null) {
                if (document.getElementById('CPH_Form_DDCompanyName').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompanyName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDUserName') != null) {
                if (document.getElementById('CPH_Form_DDUserName').options.length == 0) {
                    alert("User name must have a value....!");
                    document.getElementById("CPH_Form_DDUserName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDUserName').options[document.getElementById('CPH_Form_DDUserName').selectedIndex].value == 0) {
                    alert("Please select user name ....!");
                    document.getElementById("CPH_Form_DDUserName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDPriorityLevel') != null) {
                if (document.getElementById('CPH_Form_DDPriorityLevel').options.length == 0) {
                    alert("Priority level must have a value....!");
                    document.getElementById("CPH_Form_DDPriorityLevel").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtDueDate').value == "") {
                alert("Pls fill due date....!");
                document.getElementById('CPH_Form_TxtDueDate').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtJobStatus').value == "") {
                alert("Pls fill job status....!");
                document.getElementById('CPH_Form_TxtJobStatus').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtWorkToDo').value == "") {
                alert("Pls fill work to do....!");
                document.getElementById('CPH_Form_TxtWorkToDo').focus();
                return false;
            }
            return confirm('Do you want to save data?')
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <span class="labelbold">Company Name</span>
                        <br />
                        <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="labelbold">User Name</span>
                        <br />
                        <asp:DropDownList ID="DDUserName" CssClass="dropdown" Width="250px" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="DDUserName_SelectedIndexChanged"
                            TabIndex="1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="labelbold">Priority Level</span>
                        <br />
                        <asp:DropDownList ID="DDPriorityLevel" CssClass="dropdown" Width="150px" runat="server"
                            TabIndex="2">
                            <asp:ListItem Value="0">Normal</asp:ListItem>
                            <asp:ListItem Value="1">Urgent</asp:ListItem>
                            <asp:ListItem Value="2">Top Urgent</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="labelbold">Due Date</span>
                        <br />
                        <asp:TextBox ID="TxtDueDate" runat="server" Width="100px" CssClass="textb" TabIndex="3"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtDueDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span class="labelbold">Job Status</span>
                        <br />
                        <asp:TextBox ID="TxtJobStatus" runat="server" Width="150px" CssClass="textb" TabIndex="4">Not Done</asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <span class="labelbold">Work To Do</span>
                        <br />
                        <asp:TextBox ID="TxtWorkToDo" runat="server" Width="450px" CssClass="textb" TextMode="MultiLine"
                            TabIndex="5"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Remark</span>
                        <br />
                        <asp:TextBox ID="TxtRemark" runat="server" Width="450px" CssClass="textb" TextMode="MultiLine"
                            TabIndex="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="right">
                        <asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" TabIndex="8" />
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return SaveValidation();"
                            CssClass="buttonnorm" TabIndex="7" />
                        <%--<asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm"
                            OnClick="BtnPreview" />--%>
                        <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" TabIndex="9" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="width: 900px; height: 200px; overflow: auto">
                            <asp:GridView ID="DGToDoManagment" runat="server" OnRowDataBound="DGToDoManagment_RowDataBound"
                                DataKeyNames="SrNo" OnSelectedIndexChanged="DGToDoManagment_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views" OnRowDeleting="DGToDoManagment_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="WorkToDo" HeaderText="Work To Do">
                                        <HeaderStyle Width="500px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="500px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                        <HeaderStyle Width="500px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="500px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PriorityLevel" HeaderText="Priority Level">
                                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DueDate" HeaderText="DueDate">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JobStatus" HeaderText="JobStatus">
                                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
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
</asp:Content>
