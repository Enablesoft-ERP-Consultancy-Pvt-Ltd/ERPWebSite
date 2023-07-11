<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDepartment.aspx.cs" Inherits="Masters_Campany_AddDepartment"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {

            var objParent = window.opener;
            if (objParent != null) {
                if (window.opener.document.getElementById('CPH_Form_EmployeeUserControl_Tabemp_Tabpanelemployeeinformation_BtnAddDepartment')) {
                    window.opener.document.getElementById('CPH_Form_EmployeeUserControl_Tabemp_Tabpanelemployeeinformation_BtnAddDepartment').click();
                    self.close();
                }
            }
            else {
                window.location.href = "../../main.aspx";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="margin-left: 15%; margin-right: 15%">
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="lbla" Text="Department" runat="server" Font-Bold="true" />
                            </td>
                            <td>
                                <asp:TextBox CssClass="textb" ID="txtDepartment" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkShowOrNotInHR" Text="For HR SHOW" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox CssClass="textb" ID="txtid" Width="0px" runat="server" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                <div style="overflow: auto; height: 250px; width: 200PX">
                                    <asp:GridView ID="DgDepartment" runat="server" DataKeyNames="DepartmentId" OnRowDataBound="DgDepartment_RowDataBound"
                                        OnSelectedIndexChanged="DgDepartment_SelectedIndexChanged" AutoGenerateColumns="False"
                                        CssClass="grid-views" OnRowCreated="DgDepartment_RowCreated">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <Columns>
                                            <asp:BoundField DataField="DepartmentId" HeaderText="SrNo." />
                                            <asp:BoundField DataField="DepartmentName" HeaderText="DepartmentName" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; height: 250px; width: 200PX">
                                                <asp:CheckBoxList ID="ChkForBranch" CssClass="checkboxbold" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="font-family: Times New Roman; font-size: 18px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right">
                                <asp:Button ID="Button1" runat="server" Text="New" CssClass="buttonnorm" OnClick="Button1_Click" />
                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
