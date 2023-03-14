<%@ Page Language="C#" Title="UPDATE YARN/WARPING/MOTTELING RATE" AutoEventWireup="true"
    CodeFile="frmUpdate_YarnWarpingMottelingRate.aspx.cs" Inherits="Masters_ReportForms_frmUpdate_YarnWarpingMottelingRate"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('CPH_Form_DDjob').selectedIndex <= "0") {
                alert('Plz Select Job...')
                document.getElementById('CPH_Form_DDjob').focus()
                return false;
            }
            return confirm('Do You want to Update Rate?')
        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <%--"width: 900px; height: 1000px; --%>
            <%--style="background-color: #edf3fe"--%>
            <div>
                <div style="width: 900px;">
                    <div style="width: 319px; margin-left: 300px; height: 146px; margin-top: 20px">
                        <asp:Panel runat="server" ID="panel1" Style="border-style: groove; width: 310px;
                            border-color: Teal; border-width: 1px; border: 5px solid #c8e5f6;">
                            <div style="padding: 0px 0px 0px 20px">
                                <table style="width: 290px; height: 158px;">
                                    <%--<tr>
                                        <td>
                                            <asp:Label ID="lblUnits" runat="server" Text="Unit Name" CssClass="labelnormal "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDunits" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbljob" runat="server" Text="Job" CssClass="labelnormal "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDjob" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" Text="From Date" CssClass="labelnormal "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromdate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalenderExtendertxtdate" runat="server" TargetControlID="txtFromdate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="To Date" CssClass="labelnormal "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp<asp:RadioButton ID="RDRecRate" runat="server" Text="RECEIVE RATE" Visible="false"
                                                CssClass="radiobuttonnormal" GroupName="p" />
                                        </td>
                                        <td>
                                            &nbsp
                                            <asp:RadioButton ID="RDOrderRate" runat="server" Text="ORDER RATE" CssClass="radiobuttonnormal"
                                                GroupName="p" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnUpdate" runat="server" CssClass="buttonnorm" Text="Update" OnClick="btnUpdate_Click"
                                                OnClientClick="return validate();" />
                                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                                OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
