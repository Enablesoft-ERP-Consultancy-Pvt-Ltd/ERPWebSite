<%@ Page Title="Process Time Master" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmItemWiseDefineTime.aspx.cs" Inherits="Masters_ProductionPlaning_FrmItemWiseDefineTime"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"> </script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "FrmItemWiseDefineTime.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function ValidateSave() {
            if (document.getElementById('CPH_Form_DDCategoryName').options[document.getElementById('CPH_Form_DDCategoryName').selectedIndex].value == 0) {
                alert("Please select company name....!");
                document.getElementById("CPH_Form_DDCategoryName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDItemName').options[document.getElementById('CPH_Form_DDItemName').selectedIndex].value == 0) {
                alert("Please select item Name....!");
                document.getElementById("CPH_Form_DDItemName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TDQuality')) {
                if (document.getElementById('CPH_Form_DDQuality').options[document.getElementById('CPH_Form_DDQuality').selectedIndex].value == 0) {
                    alert("Please select quality name....!");
                    document.getElementById("CPH_Form_DDQuality").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                alert("Please select process name....!");
                document.getElementById("CPH_Form_DDProcessName").focus();
                return false;
            }
            return confirm('Do You Want To Show Data?')
        }
    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCategoryName" class="tdstyle" runat="server" Text="Category Name"
                                CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCategoryName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblItemName" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDQuality" runat="server" visible="False">
                            <asp:Label ID="lblQualityName" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDesign" runat="server" class="tdstyle">
                            <asp:Label ID="lblDesignName" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TDColor" runat="server" visible="False">
                            <asp:Label ID="lblColorName" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShape" runat="server" visible="False" class="tdstyle">
                            <asp:Label ID="lblShapeName" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:CheckBox ID="ChkForMtr" runat="server" Text="For Mtr" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged"
                                CssClass="checkboxbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDSize" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblSizeName" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDSize" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShadeColor" runat="server" visible="False">
                            <asp:Label ID="LblShadeColor" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="ddShadeColor" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblProcessName" class="tdstyle" runat="server" Text="Process Name"
                                CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="BtnShowData" runat="server" Text="ShowData" OnClick="BtnShowData_Click"
                                CssClass="buttonnorm" OnClientClick="return ValidateSave();" />
                            <%--<asp:Label ID="LblTime" runat="server" Text="Time" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:CheckBox ID="ChkForMinutes" runat="server" Text="For Min" CssClass="checkboxbold" />--%>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <b>
                                <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                    CssClass="buttonnorm" />
                                <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return ValidateSave();"
                                    CssClass="buttonnorm" />
                                <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                    Visible="true" CssClass="buttonnorm preview_width" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <div style="width: 100%; max-height: 250px; overflow: auto">
                                <asp:GridView ID="DGItemDescription" runat="server" AutoGenerateColumns="False" DataKeyNames="Item_Finished_ID"
                                    OnRowDataBound="DGItemDescription_RowDataBound" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="QualityName" HeaderText="QualityName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DesignName" HeaderText="DesignName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ColorName" HeaderText="ColorName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShapeName" HeaderText="Shape">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="Size">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Days">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtDays" Width="50px" Text='<%#Bind("Days") %>' BackColor="Yellow"
                                                    runat="server" onkeypress="return isNumberKey(event);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hours">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtHour" Width="50px" Text='<%#Bind("Hours") %>' BackColor="Yellow"
                                                    runat="server" onkeypress="return isNumberKey(event);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Min">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtMinutes" Width="50px" Text='<%#Bind("Minutes") %>' BackColor="Yellow"
                                                    runat="server" onkeypress="return isNumberKey(event);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem_Finished_ID" Text='<%#Bind("Item_Finished_ID") %>' runat="server" />
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
