<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmRateUpdateOnFolioAndReceiveNew.aspx.cs"
    Inherits="Masters_Process_FrmRateUpdateOnFolioAndReceiveNew" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Update Job Rate On Folio and Receive" %>

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
                //                if ($("#CPH_Form_DDColor")) {
                //                    var selectedIndex = $('#CPH_Form_DDColor').attr('selectedIndex');
                //                    if (selectedIndex <= 0) {
                //                        Message = Message + "Please,select Colour Name !!!\n";
                //                    }
                //                }

                if ($("#CPH_Form_DDShape").length) {
                    if ($("#CPH_Form_DDShape").val() == "") {
                        Message = Message + "Please,select Shape Name !!!\n";
                    }
                }
                //                if ($("#CPH_Form_ddSize").length) {
                //                    var selectedIndex = $('#CPH_Form_ddSize').attr('selectedIndex');
                //                    if (selectedIndex <= 0) {
                //                        Message = Message + "Please,select Size !!!\n";
                //                    }
                //                }
                if ($("#CPH_Form_txtrate").val() == "") {
                    Message = Message + "Unit Price can not be blank and Zero !!!\n";
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
                    <div id="divratetype" runat="server">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label2" runat="server" Text="RATE TYPE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDRatetype" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="DDRatetype_SelectedIndexChanged">
                                <asp:ListItem Text="Pcs Wise" Value="1" />
                                <asp:ListItem Text="Area Wise" Value="0" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="DivRateLocation" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label5" runat="server" Text="RATE LOCATION" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDRateLocation" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="DDRateLocation_SelectedIndexChanged">
                                <asp:ListItem Value="0">InHouse</asp:ListItem>
                                <asp:ListItem Value="1">OutSide</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="DivWeavingEmployee" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label6" runat="server" Text="Weaving Emp" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDWeavingEmp" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblPrice" runat="server" Text="UNIT PRICE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtrate" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>
                    <div id="DivCommRate" runat="server" visible="false" style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label4" runat="server" Text="COMM. RATE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtcommrate" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div> 
                    <div id="DivDateRange" runat="server" visible="false" style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:CheckBox ID="ChkForDate" CssClass="checkboxbold" Text="Chk For Date" runat="server" Visible="false" />
                            &nbsp;
                            <asp:Label ID="Label9" runat="server" Text="From" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>&nbsp
                            <asp:CalendarExtender ID="cal1" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                            &nbsp;
                            <asp:Label ID="Label10" runat="server" Text="TO" CssClass="labelbold"></asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>&nbsp
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                            &nbsp;
                        </div>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkcurrentrate" CssClass="checkboxbold" Text="Print Current Rate Only"
                                        runat="server"  Visible="false"/>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClick="btnsave_Click" />
                                    <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClientClick="return CloseForm();" />
                                    <%--<asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />--%>
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
                    
                </div>
            </div>
        </ContentTemplate>
       <%-- <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
