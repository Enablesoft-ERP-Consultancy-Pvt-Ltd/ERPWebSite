<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmProcessKit.aspx.cs"
    Inherits="Masters_Process_frmProcessKit" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Kit Making" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server" ID="Page">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmProcessKit.aspx";
        }
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
//                if ($("#CPH_Form_ddJob")) {
//                    var selectedIndex = $('#CPH_Form_ddJob').attr('selectedIndex');

//                    if (selectedIndex <= 0) {
//                        Message = Message + "Please,select Job Name !!!\n";
//                    }
//                }
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
//        function isNumberKey(evt) {
//            var charCode = (evt.which) ? evt.which : event.keyCode
//            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
//                return false;
//            }
//            else {
//                return true;
//            }
//        }
        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
            </script>
            <div class="Maindiv" style="width:100%">
                <div style="width: 800px; margin: 0px auto;">
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
                     <div id="DivOrderType" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="ORDER TYPE"></asp:Label>
                        </div>
                        <div style="height: 25px">
                            <asp:DropDownList ID="DDOrderType" runat="server" Width="250px" CssClass="dropdown">
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
                            <asp:DropDownList ID="DDWeavingEmp" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="DDWeavingEmp_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblpcs" runat="server" Text="PCS" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtpcs" runat="server" CssClass="textb" 
                                ontextchanged="txtpcs_TextChanged" AutoPostBack="true" ></asp:TextBox>&nbsp
                        </div>
                        <div style="display:none">
                         <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="90px" AutoPostBack="True"
                                ></asp:TextBox>
                                  <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                             <asp:CheckBox ID="CHKFORALLDESIGN" runat="server" />
                              <asp:CheckBox ID="CHKFORALLCOLOR" runat="server"  />
                               <asp:CheckBox ID="CHKFORALLSIZE" runat="server"/>
                                </div>
                    </div>
                    <%--<div style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label4" runat="server" Text="COMM. RATE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtcommrate" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>--%>
                   <%-- <div id="DivBonus" runat="server" visible="false" style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label7" runat="server" Text="Bouns" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="TxtBouns" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>--%>
                  <%--  <div id="DivFinisherRate" runat="server" visible="false" style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label8" runat="server" Text="Finisher Rate" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="TxtFinisherRate" runat="server" CssClass="textb"></asp:TextBox>&nbsp
                        </div>
                    </div>--%>

                   <%-- <div id="DivDateRange" runat="server" visible="false" style="margin-top: 1%">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                          <asp:CheckBox ID="ChkForDate" CssClass="checkboxbold" Text="Chk For Date"
                                        runat="server" /> &nbsp; <asp:Label ID="Label9" runat="server" Text="From" CssClass="labelbold"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>&nbsp
                             <asp:CalendarExtender ID="cal1" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"
                                    runat="server"> </asp:CalendarExtender>&nbsp;
                                <asp:Label ID="Label10" runat="server" Text="TO" CssClass="labelbold"></asp:Label>
                                <asp:TextBox ID="txtToDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>&nbsp
                             <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                                    runat="server"> </asp:CalendarExtender>&nbsp;
                        </div>
                    </div>--%>

                   
                    <div>
                        <div>
                            <asp:Label ID="lblMessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Bold="true" />
                        </div>
                    </div>
                </div>
                <div>
                 
                                <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkEdit_CheckedChanged" />
                           
                          <asp:Label ID="Label4" runat="server" Text="Kit No" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissueno" CssClass="textb" Width="61px" runat="server" AutoPostBack="true" Enabled="false" OnTextChanged="txtissueno_TextChanged" />
                        </div>
                        
                           <table width="100%">
                        <tr>
                            <td>
               <div runat="server"  style="max-height: 500px;width:100%;overflow: auto" >
                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDataBound="DG_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Raw Material Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemCode") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Input Item Desc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblinputitemdes" Text='<%#Bind("INPUT_ITEM") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunit" Text='<%#Bind("I_UNIT")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BinNo">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" runat="server" OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No.">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDLotNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No.">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="DDTagno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstockqty"  Text='<%#Bind("IQTY") %>'  runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <%-- <asp:TemplateField HeaderText="Consmp 1 Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOnePcsconsmpqty" Text='<%#Bind("OnePcdconsmpqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Consmp Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblconsmpqty" Text='<%#Bind("consmpqty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Already Issued">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblalreadyeissued" Text='<%#Bind("issuedQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BalanceQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceQty" Text='<%#Bind("BalanceQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Issue Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtissueqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--  <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofcone" Width="50px" BackColor="Yellow" runat="server" 
                                                    onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblifinishedid" Text='<%#Bind("Ifinishedid") %>' runat="server" />
                                                   <%-- <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />--%>
                                                    <asp:Label ID="lbliunitid" Text='<%#Bind("i_unit") %>' runat="server" />
                                                    <%--<asp:Label ID="lblisizeflag" Text='<%#Bind("isizeflag") %>' runat="server" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>


                    <%--<asp:GridView ID="DGRateDetail" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                        Width="600px" OnRowDataBound="DGRateDetail_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:BoundField DataField="Articles" HeaderText="ARTICLE NAME">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="JobName" HeaderText="JOB NAME">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Colour" HeaderText="COLOUR">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Shape" HeaderText="Shape">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Size" HeaderText="SIZE">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Ratetype" HeaderText="RATE TYPE">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UNITRATE" HeaderText="UNIT RATE">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" BackColor="Yellow" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Commrate" HeaderText="COMM. RATE">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" BackColor="Yellow" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Date" HeaderText="DATE">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" BackColor="Yellow" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Rate Location">
                                <ItemTemplate>
                                    <asp:Label ID="lblratelocation" runat="server" Text='<%# Bind("Ratelocation") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emp Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EmpName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bonus">
                                <ItemTemplate>
                                    <asp:Label ID="lblBonus" runat="server" Text='<%# Bind("Bonus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finisher Rate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinisherRare" runat="server" Text='<%# Bind("FinisherRate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Order Type" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderType" runat="server" Text='<%# Bind("OrderType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>--%>
                </div>
                 </td>
                        </tr>
                    </table>
              

                 <div>
                        <table width="100%">
                            <tr>
                                <%--<td>
                                    <asp:CheckBox ID="chkcurrentrate" CssClass="checkboxbold" Text="Print Current Rate Only"
                                        runat="server" />
                                </td>--%>
                                <td align="right">
                                    
                                         <asp:Button ID="btnnew" Width="75px" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClick="btnsave_Click" />
                                    <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" Width="75px"
                                        OnClientClick="return CloseForm();" />
                                 <%--   <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />--%>
                                   
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gvdetail_RowCancelingEdit"
                        EmptyDataText="No records found.." OnRowEditing="gvdetail_RowEditing" OnRowUpdating="gvdetail_RowUpdating"
                        OnRowDeleting="gvdetail_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LotNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TagNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Text='<%#Bind("IssueQuantity") %>' Width="70px" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblissueqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblhqty" Text='<%#Bind("IssueQuantity") %>' runat="server" />
                                    <asp:Label ID="lblprmid" Text='<%#Bind("prmid") %>' runat="server" />
                                    <asp:Label ID="lblprtid" Text='<%#Bind("prtid") %>' runat="server" />
                                    <%--<asp:Label ID="lblprorderid" Text='<%#Bind("prorderid") %>' runat="server" />--%>
                                    <asp:Label ID="lblprocessid" Text='<%#Bind("processid") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                        </Columns>
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>


                </div>



            </div>
            <asp:HiddenField ID="hngodownid" runat="server" Value="0" />
            <asp:HiddenField ID="hnissueid" runat="server" />
               <%-- <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />--%>
        </ContentTemplate>
        <Triggers>
           <%-- <asp:PostBackTrigger ControlID="btnpreview" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
