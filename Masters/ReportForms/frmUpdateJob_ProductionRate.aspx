<%@ Page Language="C#" Title="UPDATE PRODUCTION/JOB RATE" AutoEventWireup="true"
    CodeFile="frmUpdateJob_ProductionRate.aspx.cs" Inherits="Masters_ReportForms_frmUpdateJob_ProductionRate"
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
            if (document.getElementById('CPH_Form_DDunits').selectedIndex <= "0") {
                alert('Plz Select Unit...')
                document.getElementById('CPH_Form_DDunits').focus()
                return false;
            }
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
                    <div style="width: 350; margin-left: 300px; height: 146px; margin-top: 20px">
                        <asp:Panel runat="server" ID="panel1" Style="border-style: groove; width: 450px;
                            border-color: Teal; border-width: 1px; border: 5px solid #c8e5f6;">
                            <div style="padding: 0px 0px 0px 20px">
                                <table style="width: 350px; height: 158px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUnits" runat="server" Text="Unit Name" CssClass="labelnormal "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDunits" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbljob" runat="server" Text="Job" CssClass="labelnormal "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDjob" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trCategoryName" runat="server">
                                        <td>
                                            <asp:Label ID="Label8" Text="Category Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trItemName" runat="server">
                                        <td>
                                            <asp:Label ID="Label4" Text="Item Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDQtype" runat="server" CssClass="dropdown" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDQtype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trquality" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label9" Text="Quality" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trdesign" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trcolor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label6" Text="Color" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trsize" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label7" Text="Size" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="200px">
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="Chkmtrsize" Text="Mtr Size" runat="server" AutoPostBack="true"
                                                CssClass="checkboxbold" OnCheckedChanged="Chkmtrsize_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr id="Trshadecolor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label10" Text="Shadecolor" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="200px">
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
                                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Update" OnClick="btnprint_Click"
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
