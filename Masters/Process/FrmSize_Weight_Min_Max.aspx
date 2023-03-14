<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmSize_Weight_Min_Max.aspx.cs"
    Inherits="Masters_Process_FrmSize_Weight_Min_Max" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Size Weight Min Max Value" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server" ID="Page">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        // $(document).ready(function () {
        function jScript() {
            $("#CPH_Form_btnsave").click(function () {
                var Message = "";
                if ($("#CPH_Form_DDCompanyName").val() == "") {
                    Message = Message + "Please,Select Company Name!!!\n";
                }
                if ($("#<%=Divuniname.ClientID %>").is(':visible')) {
                    if ($("#CPH_Form_DDUnitName").val() == "") {
                        Message = Message + "Please,Select Unit Name!!!\n";
                    }
                }
                if ($("#CPH_Form_ddJob")) {
                    var selectedIndex = $('#CPH_Form_ddJob').attr('selectedIndex');

                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Job Name !!!\n";
                    }
                }
                if ($("#CPH_Form_DDArticleName")) {
                    var selectedIndex = $('#CPH_Form_DDArticleName').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Article Name !!!\n";
                    }
                }

                if ($("#CPH_Form_ddquality")) {
                    var selectedIndex = $('#CPH_Form_ddquality').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Quality Name !!!\n";
                    }
                }
                if ($("#CPH_Form_DDDesign")) {
                    var selectedIndex = $('#CPH_Form_DDDesign').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Design Name !!!\n";
                    }
                }

                if ($("#CPH_Form_DDColor")) {
                    var selectedIndex = $('#CPH_Form_DDColor').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Colour Name !!!\n";
                    }
                }
                if ($("#CPH_Form_DDShape").length) {
                    if ($("#CPH_Form_DDShape").val() == "") {
                        Message = Message + "Please,select Shape Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddSize").length) {
                    var selectedIndex = $('#CPH_Form_ddSize').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Size !!!\n";
                    }
                }
                if ($("#CPH_Form_txtWidthMin").val() == "") {
                    Message = Message + "Width Min can not be blank and Zero !!!\n";
                }
                if ($("#CPH_Form_txtWidthMax").val() == "") {
                    Message = Message + "Width Max can not be blank and Zero !!!\n";
                }
                if ($("#CPH_Form_txtLengthMin").val() == "") {
                    Message = Message + "Length Min can not be blank and Zero !!!\n";
                }
                if ($("#CPH_Form_txtLengthMax").val() == "") {
                    Message = Message + "Length Max can not be blank and Zero !!!\n";
                }
                if (Message == "") {
                    return true;
                }
                else {
                    alert(Message);
                    return false;
                }
            });
            //now use keypress event for Pincode and Mobile No
            $("#CPH_Form_txtrate").keypress(function (event) {

                if (event.which >= 46 && event.which <= 58) {
                    return true;
                }
                else {
                    return false;
                }

            });
        }
        // });
     
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
            </script>
            <div class="Maindiv">
                <div style="width: 500px; margin: 0px auto;">
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblCompanyName" runat="server" CssClass="labelbold" Text="COMPANY NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="Divuniname" runat="server">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblUnitName" runat="server" CssClass="labelbold" Text="UNIT NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDUnitName" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDUnitName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblJob" runat="server" CssClass="labelbold" Text="JOB NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="ddJob" runat="server" Width="250px" CssClass="dropdown" OnSelectedIndexChanged="ddJob_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divProdcode" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblProdcode" runat="server" CssClass="labelbold" Text="PRODUCT CODE"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:TextBox ID="TxtProductCode" runat="server" Width="148px" />
                        </div>
                    </div>
                    <div id="divCategory" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label1" runat="server" Text="CATEGORY" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDCategory" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lbluni" runat="server" CssClass="labelbold" Text="ARTICLE NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDArticleName" runat="server" Width="250px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDArticleName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divQuality" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblqualityname" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="ddquality" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divDesign" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblDesign" runat="server" Text="DESIGN NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDDesign" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divColor" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblColorname" runat="server" Text="COLOUR NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDColor" runat="server" Width="250px" CssClass="dropdown" OnSelectedIndexChanged="DDColor_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divShape" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label3" runat="server" Text="SHAPE NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDShape" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divSize" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblSize" runat="server" Text="SIZE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="ddSize" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:DropDownList ID="DDsizeType" CssClass="dropdown" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDsizeType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                     <div id="divshade" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblshade" runat="server" CssClass="labelbold" Text="SHADE NAME"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="ddlshade" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblWidthMin" runat="server" Text="Width Min" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtWidthMin" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label2" runat="server" Text="Width Max" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtWidthMax" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label4" runat="server" Text="Length Min" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtLengthMin" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label5" runat="server" Text="Length Max" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtLengthMax" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label6" runat="server" Text="Weight Min" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtWeightMin" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label7" runat="server" Text="Weight Max" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtWeightMax" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <%-- <div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label4" runat="server" Text="COMM. RATE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtcommrate" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>--%>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkcurrentwidthlengthweight" CssClass="checkboxbold" Text="Print Current Length Width Weight Only"
                                        runat="server" Visible="true" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClick="btnsave_Click" />
                                    <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClientClick="return CloseForm();" />
                                    <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="lblMessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Bold="true" />
                        </div>
                    </div>
                </div>
                <div style="width: 650px; margin: 0px auto; overflow: auto; max-height: 400px">
                    <asp:GridView ID="DGWidthLengthWeight" runat="server" AutoGenerateColumns="False"
                        CssClass="grid-view" Width="600px" OnRowDataBound="DGWidthLengthWeight_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                         <Columns>

                           <asp:TemplateField HeaderText="Process Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProcessName" runat="server" Text='<%# Bind("ProcessName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="Article Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("Item_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Quality Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualityName" runat="server" Text='<%# Bind("QualityName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Design Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbldesignName" runat="server" Text='<%# Bind("designName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Color Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblColour" runat="server" Text='<%# Bind("Colour") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Shape">
                                <ItemTemplate>
                                    <asp:Label ID="lblshapeName" runat="server" Text='<%# Bind("shapeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Size">
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" Text='<%# Bind("Size") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Width Min">
                                <ItemTemplate>
                                    <asp:Label ID="lblWidthMin" runat="server" Text='<%# Bind("WidthMin") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Width Max">
                                <ItemTemplate>
                                    <asp:Label ID="lblWidthMax" runat="server" Text='<%# Bind("WidthMax") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Length Min">
                                <ItemTemplate>
                                    <asp:Label ID="lblLengthMin" runat="server" Text='<%# Bind("LengthMin") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Length Max">
                                <ItemTemplate>
                                    <asp:Label ID="lblLengthMax" runat="server" Text='<%# Bind("LengthMax") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weight Min">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightMin" runat="server" Text='<%# Bind("WeightMin") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weight Max">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightMax" runat="server" Text='<%# Bind("WeightMax") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            
                            <asp:BoundField DataField="AddDate" HeaderText="DATE">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" BackColor="Yellow" />
                            </asp:BoundField>
                           
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
