<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDirectProductionorderNew.aspx.cs"
    Inherits="Masters_Process_frmDirectProductionorderNew" MasterPageFile="~/ERPmaster.master"
    ViewStateMode="Enabled" Title="Production Order" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "frmDirectProductionorderNew.aspx";
        }
        function Preview() {
            window.open('../../reportViewer1.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate_RequiredDate() {
            var required_date = document.getElementById('TxtRequiredDate').Value;
            var assign_date = document.getElementById('TxtAssignDate').value;
            if (assign_date < required_date) {
                alert("Required Date Must Be Greater Then Assign Date");
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            else {
                return true;
            }
        }
        function isNumberWith(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function checkstockQty(txt) {
            var row = txt.parentNode.parentNode;
            var rowindex = row.rowIndex - 1;
            var txtIssueQty = document.getElementById("CPH_Form_DGConsumption_txtIssueQty_" + rowindex + "");
            var txtStockQty = document.getElementById("CPH_Form_DGConsumption_txtStockQty_" + rowindex + "");
            var stockQty = parseFloat(txtStockQty.value);
            var IssueQty = parseFloat(txtIssueQty.value);
            if (IssueQty > stockQty) {
                alert('Issue Qty. can not be greater than stock Qty...');
                txtIssueQty.value = 0;
            }
        }
        function CheckIssueQty(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowindex = row.rowIndex - 1;
            var txtIssueQty = document.getElementById("CPH_Form_DGConsumption_txtIssueQty_" + rowindex + "");
            var IssueQty = txtIssueQty.value;
            if (IssueQty <= 0 || IssueQty == NaN) {
                alert("Issue Qty should be greater than zero....");
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function jScriptValidate() {
            $("#CPH_Form_BtnSave").click(function () {
                var Message = "";
                if ($("#CPH_Form_ddUnits")) {
                    var selectedIndex = $('#CPH_Form_ddUnits').attr('selectedIndex');
                    if (selectedIndex < 0) {
                        Message = Message + "Please,Select Unit Name!!!\n";
                    }
                }

                if ($("#CPH_Form_dditemname")) {
                    var selectedIndex = $('#CPH_Form_dditemname').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Article Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddcolor")) {
                    var selectedIndex = $('#CPH_Form_ddcolor').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Colour Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddshape").length) {
                    var selectedIndex = $('#CPH_Form_ddshape').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Shape Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddsize").length) {
                    var selectedIndex = $('#CPH_Form_ddsize').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Size !!!\n";
                    }
                }
                if ($("#CPH_Form_TxtRate").val() == "" || $("#CPH_Form_TxtRate").val() == "0") {
                    Message = Message + "Rate can not be blank and Zero !!!\n";
                }
                if ($("#CPH_Form_TxtQtyRequired").val() == "" || $("#CPH_Form_TxtQtyRequired").val() == "0") {
                    Message = Message + "Order Qty. can not be blank and Zero !!!\n";
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
            //on DropDown Selected Index
            $("#CPH_Form_dditemname").change(function () {
                $("#CPH_Form_ddcolor").attr('selectedIndex', 0);
                $("#CPH_Form_ddsize").attr('selectedIndex', 0);
            });
            $("#CPH_Form_ddcolor").change(function () {

                $("#CPH_Form_ddsize").attr('selectedIndex', 0);
            });
            // ENd
        }
    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(jScriptValidate);
                </script>
                <div id="main" style="width: 900px; height: 160px;">
                    <div style="width: 800px; height: 155px; border-style: groove; background-color: #DEB887;
                        margin-left: 50px">
                        <div style="float: left; margin-left: 170px; text-align: center; margin-top: 50px;
                            width: 139px">
                            <font size="2"><span class="labelbold">Enter Weaver ID No.</span> </font>
                            <br />
                            <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="134px" Height="20px" CssClass="textb"
                                OnTextChanged="txtWeaverIdNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                        <div style="float: right; margin-right: 230px; margin-top: 20px; overflow: auto;
                            width: 250px">
                            <table>
                                <tr>
                                    <td>
                                        <div style="overflow: auto; width: 200px">
                                            <asp:ListBox ID="listWeaverName" runat="server" Width="200px" Height="100px" SelectionMode="Multiple">
                                            </asp:ListBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnDelete" Text="Delete" runat="server" OnClick="btnDelete_Click"
                                            CssClass="buttonnorm" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%--  <div style="float: right; margin-right: 230px; height: 110px; overflow: auto; width: 250px;
                            margin-top: 20px">
                            <asp:ListBox ID="listWeaverName" runat="server" Width="230px" Height="100px" SelectionMode="Multiple">
                            </asp:ListBox>
                        </div>
                        <div style="float: right; width: 100px; margin-right: 230px">
                            <asp:Button ID="btnDelete" Text="Delete" runat="server" OnClick="btnDelete_Click"
                                CssClass="buttonnorm" />
                            &nbsp;&nbsp;&nbsp;
                        </div>--%>
                    </div>
                </div>
                <div style="width: 850px; height: 30px; margin-top: 10px">
                    <asp:Panel ID="panelMaster" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        BackColor="#DEB887" Width="850px">
                        <table width="600px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblUnits" runat="server" Text="Units" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddUnits" runat="server" Width="100px" CssClass="dropdown" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblAssigndate" runat="server" Text="AssignDate" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAssignDate" runat="server" Width="90px" CssClass="textb" BackColor="Beige"
                                        TabIndex="2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="TxtAssignDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td id="TdlblRequiredate" runat="server" visible="false">
                                    <asp:Label ID="lblRequiredate" runat="server" Text="RequireDate" CssClass="labelbold"></asp:Label>
                                </td>
                                <td id="TdtxtRequiredate" runat="server" visible="false">
                                    <asp:TextBox ID="TxtRequiredDate" runat="server" AutoPostBack="true" Width="90px"
                                        CssClass="textb" BackColor="beige" TabIndex="3"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="TxtRequiredDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td id="Tdlblunit" runat="server" visible="false">
                                    <asp:Label ID="lblUnit" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                </td>
                                <td id="TdDDUnit" runat="server" visible="false">
                                    <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px" TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                                <td runat="server" visible="false">
                                    <asp:Label ID="lblCalType" runat="server" Text="CalType" CssClass="labelbold"></asp:Label>
                                </td>
                                <td runat="server" visible="false">
                                    <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                        TabIndex="5">
                                        <%--<asp:ListItem Value="0">Area Wise</asp:ListItem>--%>
                                        <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblChallanno" runat="server" Text="FolioNo." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtChallanNo" runat="server" Width="90px" CssClass="textb" ReadOnly="True"
                                        TabIndex="6"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <font size="3">ItemDetail</font>
                    <asp:Panel ID="panelitemDetail" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        BackColor="#DEB887" Width="854px">
                        <table>
                            <tr>
                                <td id="TDProductCode" runat="server" visible="false">
                                    Product Code<br />
                                    <asp:TextBox ID="TxtProductCode" runat="server" TabIndex="8" Width="80px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                        UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td runat="server" visible="false">
                                    <asp:Label ID="lblCategory" runat="server" Text="Category Name"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" AutoPostBack="True"
                                        TabIndex="9" CssClass="dropdown" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblItemName" runat="server" Text="Articles"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dditemname" runat="server" Width="150px" TabIndex="10" AutoPostBack="True"
                                        CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDQuality" runat="server" visible="false">
                                    <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddquality" runat="server" Width="150px" TabIndex="11" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDDesign" runat="server" visible="false">
                                    <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="12" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDColor" runat="server" visible="false">
                                    <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddcolor" runat="server" Width="150px" TabIndex="13" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TDShape" runat="server" visible="false">
                                    <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" TabIndex="14"
                                        CssClass="dropdown" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TdSize" runat="server" visible="false">
                                    <asp:Label ID="lblsize" runat="server" Text="Size"></asp:Label>
                                    <asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" Text="Check For Mtr."
                                        Visible="false" /><br />
                                    <asp:DropDownList ID="ddsize" runat="server" Width="150px" TabIndex="15" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDShade" runat="server" visible="false">
                                    <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor"></asp:Label>
                                    &nbsp;<br />
                                    <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                        TabIndex="16" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TdWidth" runat="server" visible="false">
                                    Width
                                    <br />
                                    <asp:TextBox ID="TxtWidth" runat="server" Width="90px" Enabled="false" CssClass="textb"
                                        TabIndex="17"></asp:TextBox>
                                </td>
                                <td id="TdLength" runat="server" visible="false">
                                    Length
                                    <br />
                                    <asp:TextBox ID="TxtLength" runat="server" Width="90px" Enabled="false" CssClass="textb"
                                        TabIndex="18"></asp:TextBox>
                                </td>
                                <td id="TdArea" runat="server" visible="false">
                                    Area
                                    <br />
                                    <asp:TextBox ID="TxtArea" runat="server" Width="70px" Enabled="false" CssClass="textb"
                                        TabIndex="19"></asp:TextBox><br />
                                </td>
                                <td>
                                    Rate
                                    <br />
                                    <asp:TextBox ID="TxtRate" runat="server" Width="70px" AutoPostBack="True" CssClass="textb"
                                        TabIndex="20" onkeypress="return isNumber(event);" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td id="Td1" runat="server" visible="false">
                                    Commission<br />
                                    <asp:TextBox ID="TxtCommission" runat="server" Width="70px" CssClass="textb" TabIndex="21"></asp:TextBox>
                                </td>
                                <td>
                                    OrderQty<br />
                                    <asp:TextBox ID="TxtQtyRequired" runat="server" Width="70px" CssClass="textb" BackColor="beige"
                                        TabIndex="22" onkeypress=" return isNumberWith(event);"></asp:TextBox>
                                </td>
                                <td align="right">
                                    <br />
                                    <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                        TabIndex="23" Width="75px" />
                                    &nbsp;<asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                        CssClass="buttonnorm" TabIndex="24" Width="75px" />
                                    &nbsp;<asp:Button ID="BtnUpdate" runat="server" Text="Update" Visible="False" OnClientClick="return confirm('Do you want to Update?')"
                                        CssClass="buttonnorm" TabIndex="25" />
                                    &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="true" CssClass="buttonnorm"
                                        TabIndex="26" OnClick="BtnPreview_Click" Width="75px" />
                                    &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                        CssClass="buttonnorm" TabIndex="27" Width="75px" />
                                </td>
                            </tr>
                        </table>
                        <table width="800px">
                            <tr runat="server" visible="false">
                                <td colspan="2">
                                    Remarks
                                    <asp:TextBox ID="TxtRemarks" runat="server" CssClass="textb" Width="618px" TabIndex="28"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td>
                                    Instruction
                                    <br />
                                    <asp:TextBox ID="TxtInstructions" runat="server" Width="485px" Height="50px" CssClass="textb"
                                        TextMode="MultiLine" TabIndex="29"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hncomp" runat="server" />
                                    <asp:HiddenField ID="hdArea" runat="server" />
                                </td>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"></asp:Label>
                                    </td>
                                </tr>
                        </table>
                    </asp:Panel>
                </div>
                <table width="90%">
                    <tr>
                        <td colspan="3">
                            <div style="width: 850px; height: 89px; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="IssueDetailId"
                                    OnRowDataBound="DGOrderdetail_RowDataBound" Width="601px" OnRowDeleting="DGOrderdetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="Category" HeaderText="CATEGORY" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Articles" HeaderText="ARTICLES">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Colour" HeaderText="COLOUR">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="SIZE">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Length" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtlength" runat="server" Text='<%#Bind("Length") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWidth" runat="server" Text='<%#Bind("Width") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Rate" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#Bind("Rate") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Area" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtArea" runat="server" Text='<%#Bind("Area") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("AMount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderId" runat="server" Text='<%#Bind("OrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ItemFInishedid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemFinishedId" runat="server" Text='<%#Bind("Item_Finished_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkForCone" runat="server" Text=" Check For Cone Issue" Style="font-weight: bold" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnShowConsumption" runat="server" Text="Issue Raw Material" Width="173px"
                                CssClass="buttonnorm" OnClick="btnShowConsumption_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" id="TdDgConsumption" runat="server" style="display: none">
                            <div style="height: 400px; overflow: auto; width: 869px">
                                <asp:GridView ID="DGConsumption" runat="server" AutoGenerateColumns="False" DataKeyNames="IFinishedid"
                                    OnRowDataBound="DGConsumption_RowDataBound" OnRowCommand="DGConsumption_RowCommand"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ITEM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConsmpQTY" HeaderText="Consmp Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IssuedQty" HeaderText="Issued Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Pend Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPendQty" runat="server" Text='<%#Bind("PendQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown" Visible="false">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddgodown" runat="server" Width="150px" OnSelectedIndexChanged="ddgodown_onSelectedindexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LotNo/BatchNo">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlotNo" runat="server" Width="150px" OnSelectedIndexChanged="ddlotnoDgConsumption_onSelectedindexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtStockQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIssueQty" runat="server" Width="75px" CssClass="textb" onchange="return checkstockQty(this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkSave" runat="server" CausesValidation="False" CommandName="Save"
                                                    OnClientClick="return  CheckIssueQty(this);" Text="Save"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" id="TdDGConsumptionConeType" runat="server" style="display: none">
                            <div style="height: 400PX; overflow: auto; width: 867px">
                                <asp:GridView ID="DGConsumptionConeType" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="IFinishedid" CssClass="grid-views" OnRowDataBound="DGConsumptionConeType_RowDataBound"
                                    OnRowCommand="DGConsumptionConeType_RowCommand">
                                    <HeaderStyle CssClass="gvheaders" Wrap="False" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" Width="12px" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConsmpQTY" HeaderText="Consmp Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IssuedQty" HeaderText="Issued Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PendQty" HeaderText="Pend Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Godown" Visible="false">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddgodown" runat="server" Width="150px" OnSelectedIndexChanged="ddgodown_onSelectedindexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LotNo/BatchNo">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlotNo" runat="server" Width="150px" OnSelectedIndexChanged="ddlotnoDgConsumptionConeType_onSelectedindexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtStockQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ConeType">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DDConeType" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. Of Cones">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNoofCones" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIssueQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkSave" runat="server" CausesValidation="False" CommandName="Save"
                                                    Text="Save"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblConsumption" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
