<%@ Page Title="Doura Report Save Detail" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmDouraReportSaveDetail.aspx.cs" Inherits="Masters_Loom_FrmDouraReportSaveDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmDouraReportSaveDetail.aspx";
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
                $("#<%=BtnSave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    var selectedindex = $("#<%=DDBranch.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Branch Name!!\n";
                    }
                    var selectedindex = $("#<%=DDEmployeeName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Employee Name!!\n";
                    }
                    var selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Folio No !!\n";
                    }
                    var txtDouraDate = document.getElementById('<%=txtDouraDate.ClientID %>');
                    if (txtDouraDate.value == "" || txtDouraDate.value == "0") {
                        Message = Message + "Please Enter Doura Date. !!\n";
                    }
                    var txtDouraInspector = document.getElementById('<%=txtDouraInspector.ClientID %>');
                    if (txtDouraInspector.value == "" || txtDouraInspector.value == "0") {
                        Message = Message + "Please Enter Inspector Name. !!\n";
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
                                <asp:Label ID="Label2" CssClass="labelbold" Text="Branch Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDBranch" CssClass="dropdown" Width="200px" runat="server">
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
                             <td>
                                <asp:Label ID="Label6" CssClass="labelbold" Text="Category" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDCategory" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>                            
                            <td >
                                <asp:Label ID="Label5" CssClass="labelbold" Text="Item Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDItemName" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           <td id="Trquality" runat="server" visible="false">
                                <asp:Label ID="Label7" CssClass="labelbold" Text="Quality" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDQuality" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Trdesign" runat="server" visible="false">
                                <asp:Label ID="Label10" CssClass="labelbold" Text="Design" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDDesign" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDDesign_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Trcolor" runat="server" visible="false">
                                <asp:Label ID="Label11" CssClass="labelbold" Text="Color" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDColor" CssClass="dropdown" Width="200px" 
                                    runat="server" AutoPostBack="true" onselectedindexchanged="DDColor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Trsize" runat="server" visible="false">
                                <asp:Label ID="Label12" CssClass="labelbold" Text="Size" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDSize" CssClass="dropdown" Width="200px" 
                                    runat="server">
                                </asp:DropDownList>
                                 <asp:CheckBox ID="Chkmtrsize" Text="Mtr Size" runat="server" AutoPostBack="true"
                                            CssClass="checkboxbold" OnCheckedChanged="Chkmtrsize_CheckedChanged" />
                            </td>
                             <td id="Trshadecolor" runat="server" visible="false">
                                <asp:Label ID="Label13" CssClass="labelbold" Text="ShadeColor" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDshade" CssClass="dropdown" Width="200px" 
                                    runat="server"> </asp:DropDownList>
                                 
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>                            
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Doura Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtDouraDate" CssClass="textb" runat="server" Width ="200px" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtDouraDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Doura Inspector" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtDouraInspector" CssClass="textb" runat="server" Width ="200px" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Doura Remark" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtDouraRemark" CssClass="textb" runat="server" Width ="200px" />
                            </td>
                            <td>
                            <asp:Button CssClass="buttonnorm" ID="BtnShowData" runat="server" Text="ShowData" OnClick="BtnShowData_Click" />

                            </td>
                            <%--<td>
                                <asp:Label ID="Label5" Text="Enter Stock No." CssClass="labelbold" Font-Size="Small"
                                    runat="server" />
                                <br />
                                <asp:TextBox ID="txtstockno" CssClass="textb" Width="200px" runat="server" onKeypress="KeyDownHandler(event);" />
                                <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="txtstockno_TextChanged" />
                            </td>--%>
                        </tr>
                    </table>
                </fieldset>
                <table style="width: 100%">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" ValidationGroup="f1"
                                OnClientClick="return Validation();" Width="50px" OnClick="BtnSave_Click" />
                            <%-- <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" />--%>
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
                                           <%-- <asp:TemplateField HeaderText="Iss/Rec No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssRecNo" Text='<%#Bind("IssRecNo") %>' runat="server" Width="100px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
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
                                             <asp:TemplateField HeaderText="Off Loom">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOffLoom" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Loom Position">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLoomPosition" Width="70px" BackColor="Yellow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueOrderID" Text='<%#Bind("IssueOrderID") %>' runat="server" />  
                                                    <asp:Label ID="lblItemFinishedId" Text='<%#Bind("Item_Finished_Id") %>' runat="server" />                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <%-- <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblDel" runat="server" OnClick="lbDelete_Click" ToolTip="Delete"
                                                        OnClientClick="return confirm('Do you want to delete this row?');" CausesValidation="False">Delete</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnDouraId" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
