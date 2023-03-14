<%@ Page Title="Direct Stock Entry" Language="C#" AutoEventWireup="true" CodeFile="DirectStock.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_process_itemRecieve" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <%--<meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />--%>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function PreviewImg(imgFile) {
            var newPreview = document.getElementById("newPreview");
            document.getElementById("newPreview").value = "";
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreview.style.width = "111px";
            newPreview.style.height = "66px";
            var control = document.getElementById("newPreview1");
            control.style.visibility = "hidden";
        }
        function PreviewReferenceImage(imgFile) {
            var newPreviewRef = document.getElementById("DivReferenceImage");
            document.getElementById("DivReferenceImage").value = "";
            newPreviewRef.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreviewRef.style.width = "111px";
            newPreviewRef.style.height = "66px";
            var controlRef = document.getElementById("ImageReferenceImage");
            controlRef.style.visibility = "hidden";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "DirectStock.aspx";
        }
        function logout() {
            window.location.href = "../../Login.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function AddItemCategory() {
            window.open('../Carpet/AddItemCategory.aspx', '', 'Height=500px,width=700px');
        }
        function AddQuality() {
            var a3 = document.getElementById('CPH_Form_TxtFinishedid').value;
            window.open('../Carpet/AddQuality.aspx?' + a3, '', 'Height=500px,width=500px');
        }
        function AddDesign() {
            window.open('../Carpet/AddDesign.aspx', '', 'Height=500px,width=500px');
        }
        function AddColor() {
            window.open('../Carpet/AddColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShadecolor() {
            window.open('../Carpet/AddShadeColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShape() {
            window.open('../Carpet/AddShape.aspx', '', 'Height=400px,width=500px');
        }
        function AddSize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var e = document.getElementById("<%=ddshape.ClientID %>");



                if (document.getElementById("<%=HDF1.ClientID %>").value == "7") {
                    var shapeid = e.options[e.selectedIndex].value;
                    window.open('../Carpet/frmSizeForLocal.aspx?shapeid=' + shapeid + '', '', 'Height=400px,width=600px');
                    return false;
                }
                else {

                    var shapeid = document.getElementById("<%=ddshape.ClientID %>").value;

                    if (document.getElementById("<%=hnVarNewQualitySize.ClientID %>").value == "1") {

                        window.open('../Carpet/FrmNewSize.aspx?shapeid=' + shapeid + '', '', 'Height=500px,width=1000px');
                        return false;
                    }
                    else {
                        window.open('../Carpet/AddSize.aspx?shapeid=' + shapeid + '', '', 'Height=500px,width=1000px');
                        return false;

                    }


                }
            }
        }

        function Additem() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var varcode = document.getElementById('CPH_Form_ddlcatagoryname').value;
                window.open('../Carpet/AddItemName.aspx?Category=' + varcode + '', '', 'width=550px,Height=500px');
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function isNumberKeypoint(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="txtstockid" runat="server" Visible="false" Width="0px" BorderStyle="None"></asp:TextBox>
                        <asp:TextBox ID="TxtFinishedid" ForeColor="White" BorderStyle="None" runat="server"
                            Width="0px"></asp:TextBox>
                    </td>
                    <td colspan="3" align="right">
                        <asp:TextBox ID="txtprefix" runat="server" Visible="false" AutoPostBack="True" OnTextChanged="txtprefix_TextChanged"
                            Width="85px" CssClass="textb"></asp:TextBox>
                        &nbsp;<asp:TextBox ID="Txtpostfix" runat="server" Visible="false" Width="75px" CssClass="textb"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck"
                            Type="Integer" ControlToValidate="Txtpostfix" Text="Text must be an integer."
                            ForeColor="Red" SetFocusOnError="true" />
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lblcomp" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList ID="ddlcompany" runat="server" Width="150px" CssClass="dropdown"
                            TabIndex="1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblcategorytype" runat="server" Text="Catagory Type" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlcatagorytype" runat="server" Width="150px" OnSelectedIndexChanged="ddlcatagorytype_SelectedIndexChanged"
                            AutoPostBack="True" CssClass="dropdown" TabIndex="2">
                            <asp:ListItem> Select </asp:ListItem>
                            <asp:ListItem Value="1">Raw Material</asp:ListItem>
                            <asp:ListItem Value="0">Finished Item</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td id="Td5" align="left" class="tdstyle">
                        <asp:Label ID="Label1" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;
                        <br />
                        <asp:TextBox ID="txtdate" runat="server" TabIndex="3" Width="120px" AutoPostBack="True"
                            CssClass="textb"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="txtdate">
                        </asp:CalendarExtender>
                    </td>
                    <td id="code" runat="server" class="tdstyle">
                        <asp:Label ID="Label3" runat="server" Text="ProdCode" CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="TxtProdCode" runat="server" AutoPostBack="True" OnTextChanged="TxtProdCode_TextChanged"
                            Width="150px" CssClass="textb"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                            UseContextKey="True">
                        </cc1:AutoCompleteExtender>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnadditemcategory" runat="server" CssClass="buttonsmalls" OnClientClick="return AddItemCategory()"
                            Text="ADD" TabIndex="5" Width="40px" UseSubmitBehavior="False" />
                        <br />
                        <asp:DropDownList ID="ddlcatagoryname" runat="server" Width="150px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcatagoryname_SelectedIndexChanged" CssClass="dropdown"
                            TabIndex="4">
                        </asp:DropDownList>
                        <asp:Button CssClass="refreshcategory" ID="refreshcategory" runat="server" Text=""
                            BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                            BorderStyle="None" ForeColor="White" OnClick="refreshcategory_Click" />
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="lblitemname" runat="server" Text="ItemName" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="BtnAdd0" runat="server" CssClass="buttonsmalls" OnClientClick="return Additem();"
                            Text="ADD" TabIndex="7" Width="40px" /><br />
                        <asp:DropDownList ID="ddlitemname" runat="server" Width="150px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlitemname_SelectedIndexChanged" CssClass="dropdown"
                            TabIndex="6">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlitemname"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                        <asp:Button CssClass="buttonnorm" ID="BtnRefreshItem" runat="server" Text="" Style="display: none"
                            OnClick="BtnRefreshItem_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr id="Tr5">
                    <td id="ql" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmalls" OnClientClick="return AddQuality()"
                            Text="ADD" TabIndex="9" Width="40px" />
                        <br />
                        <asp:DropDownList ID="dquality" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged" TabIndex="8">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="dquality"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                        <asp:Button CssClass="refreshquality" ID="refreshquality" runat="server" Text=""
                            BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                            BorderStyle="None" ForeColor="White" OnClick="refreshquality_Click" />
                    </td>
                    <td id="dsn" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmalls" OnClientClick="return AddDesign()"
                            Text="ADD" TabIndex="11" Width="40px" /><br />
                        <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged" TabIndex="10">
                        </asp:DropDownList>
                        <asp:Button CssClass="refreshdesign" ID="refreshdesign" runat="server" Text="" BorderWidth="0px"
                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                            ForeColor="White" OnClick="refreshdesign_Click" />
                    </td>
                    <td id="clr" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddColor()"
                            Text="ADD" TabIndex="13" Width="40px" /><br />
                        <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                            OnSelectedIndexChanged="ddcolor_SelectedIndexChanged" TabIndex="12">
                        </asp:DropDownList>
                        <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Text="" BorderWidth="0px"
                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                            ForeColor="White" OnClick="refreshcolor_Click" />
                    </td>
                    <td id="shp" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShape()"
                            TabIndex="15" Text="ADD" Width="40px" />
                        <br />
                        <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                            CssClass="dropdown" TabIndex="14">
                        </asp:DropDownList>
                        <asp:Button CssClass="buttonnorm" ID="refreshshape" runat="server" Text="" BorderWidth="0px"
                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                            ForeColor="White" OnClick="refreshshape_Click" />
                    </td>
                    <td id="sz" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                        <asp:CheckBox ID="ChkMtr" runat="server" Text="Mtr" CssClass="checkboxnormal" AutoPostBack="True"
                            OnCheckedChanged="ChkMtr_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAddSize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddSize();"
                            TabIndex="17" Text="ADD" Width="40px" />
                        <br />
                        <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                            OnSelectedIndexChanged="ddsize_SelectedIndexChanged" TabIndex="16">
                        </asp:DropDownList>
                        <asp:Button CssClass="buttonnorm" ID="BtnRefreshSize" runat="server" Text="" Style="display: none"
                            ForeColor="White" OnClick="BtnRefreshSize_Click" />
                    </td>
                    <td id="shd" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnaddshadecolor" runat="server"
                            CssClass="buttonsmalls" OnClientClick="return AddShadecolor()" TabIndex="24"
                            Text="ADD" Width="40px" />
                        <br />
                        <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" TabIndex="18">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="Listddlshade" runat="server" TargetControlID="ddlshade"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                        <asp:Button CssClass="buttonnorm" ID="refreshshade" runat="server" Text="" Style="display: none"
                            ForeColor="White" OnClick="refreshshade_Click" />
                    </td>
                    <td id="TdFINISHED_TYPE" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="LblFINISHED_TYPE" runat="server" Text="Finish_Type" CssClass="labelbold"></asp:Label>
                        &nbsp;<br />
                        <asp:DropDownList ID="ddFINISHED_TYPE" runat="server" Width="150px" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle" id="TDGodownId" runat="server" visible="true">
                        <asp:Label ID="lblgodown" runat="server" Text="Godown Name" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlgodown" runat="server" Width="150px" CssClass="dropdown"
                            TabIndex="19" AutoPostBack="true" OnSelectedIndexChanged="ddlgodown_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label4" runat="server" Text="Unit" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList ID="ddlunit" runat="server" Width="150px" CssClass="dropdown" TabIndex="20">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle" runat="server" id="tdlotno">
                        <asp:Label ID="LblLotNo" runat="server" Text="Lot No." CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="txtlotno" runat="server" Width="123px" CssClass="textb" TabIndex="21"></asp:TextBox>
                    </td>
                    <td class="tdstyle" runat="server" id="TDTagNo" visible="false">
                        <asp:Label ID="Label5" runat="server" Text="Tag No." CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="txttagno" runat="server" Width="123px" CssClass="textb" TabIndex="22"></asp:TextBox>
                    </td>
                    <td class="tdstyle" runat="server" id="TDBinnowise" visible="false">
                        <asp:Label ID="Label10" runat="server" Text="Bin No." CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList ID="DDBinNo" runat="server" Width="150px" CssClass="dropdown" TabIndex="23">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label6" runat="server" Text="Opening Stock" CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="txtopeningstock" runat="server" Width="91px" CssClass="textb" TabIndex="23"
                            BackColor="Beige" onkeypress="return isNumberKeypoint(event);"></asp:TextBox>
                    </td>
                    <td class="tdstyle" runat="server" id="tdrate">
                        <asp:Label ID="Label7" runat="server" Text="Rate" CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="TxtRate" runat="server" Width="46px" CssClass="textb" Text="0" TabIndex="24"></asp:TextBox>
                    </td>
                    <td class="tdstyle" runat="server" id="tdminstock" visible="false">
                        <asp:Label ID="Label8" runat="server" Text="Min. Stock" CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="Txtminstock" runat="server" Width="57px" CssClass="textb" Text="0"
                            TabIndex="25"></asp:TextBox>
                    </td>
                    <td id="TDnoofCone" runat="server">
                        <asp:Label ID="lblNoofcone" runat="server" Text="No of Cone" CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="txtnoofcone" runat="server" Width="60px" CssClass="textb" onkeypress="return isNumberKey(event);"
                            TabIndex="26"></asp:TextBox>
                    </td>
                    <td runat="server">
                        <br />
                        <asp:TextBox Visible="false" ID="txtcode" runat="server" CssClass="textb"></asp:TextBox>
                    </td>
                    <td id="TDDirectStockRemark" runat="server" visible="false">
                        <asp:Label ID="Label11" runat="server" Text="Remark" CssClass="labelbold"></asp:Label><br />
                        <asp:TextBox ID="txtDirectStockRemark" runat="server" Width="150px" CssClass="textb"
                            TabIndex="27"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" align="right">
                        <asp:CheckBox ID="ChkForExcelExport" runat="server" Text="Check For ExcelExport"
                            CssClass="checkboxbold" Visible="false" />
                        <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" TabIndex="27" Width="70px" />
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do You Want To Save?')"
                            CssClass="buttonnorm" TabIndex="28" Width="70px" />
                        <asp:Button ID="btnpriview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                            OnClick="btnpriview_Click" TabIndex="29" Width="70px" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClick="btnclose_Click"
                            TabIndex="30" Width="70px" />
                        <asp:Button ID="BtnStockNoToVCMSale" runat="server" Text="VCM SALE" CssClass="buttonnorm"
                            Visible="false" OnClick="BtnStockNoToVCMSale_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblmessage" runat="server" Text="Data Saved Succsesfully" ForeColor="Red"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblerror" runat="server" Text="Carpet no. Already Exist" ForeColor="Red"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblsave" runat="server" Text="Plz Enter Opening Stock" ForeColor="Red"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="Label2" runat="server" Text="ProdCode doesnot exist" ForeColor="Red"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessageVal" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnshowDetail" Text="Click to show existing record" CssClass="buttonnorm"
                            runat="server" OnClick="btnshowDetail_Click" />
                        <asp:Label ID="lblloading" Text="Loading..............." runat="server" CssClass="labelbold"
                            ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                    <td id="TDUpdatePrice" runat="server" visible="false">
                        <asp:Button ID="BtnUpdatePrice" Text="Update Price" CssClass="buttonnorm" runat="server"
                            OnClick="BtnUpdatePrice_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center" id="TDgvcarpetdetail" runat="server">
                        <div style="overflow: auto; width: 650px; height: 280px;">
                            <br />
                            <asp:GridView ID="gvcarpetdetail" runat="server" OnRowDataBound="gvcarpetdetail_RowDataBound"
                                DataKeyNames="stockid" OnSelectedIndexChanged="gvcarpetdetail_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="stockid" HeaderText="stockid" Visible="False" />
                                    <asp:BoundField DataField="Category_Name" HeaderText="Category Name">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Font-Size="Smaller" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Item_name" HeaderText="Item">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Unitname" HeaderText="Unit">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GodownName" HeaderText="Godown">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LotNo" HeaderText="LotNo">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TagNo" HeaderText="TagNo">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NoofCone" HeaderText="NoofCone">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BinNo" HeaderText="BinNo">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OpenStock" HeaderText="Opening Stock">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qtyinhand" HeaderText="Qty Inhand">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblunitid" runat="server" Text='<%#Bind("Unitid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="StockNos" HeaderText="Carpet NOs" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table id="TbFinished" runat="server" visible="false">
                <tr>
                    <td>
                        <div style="overflow: auto; max-height: 500px">
                            <asp:GridView ID="GDFinishedDetail" runat="server" AutoGenerateColumns="false" CssClass="grid-views"
                                OnRowDeleting="GDFinishedDetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SR NO.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ITEM_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemname" Text='<%#Bind("item_name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="QUALITY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblquality" Text='<%#Bind("QualityName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DESIGN">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesign" Text='<%#Bind("designname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="COLOR">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcolor" Text='<%#Bind("ColorName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SHAPE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblshape" Text='<%#Bind("ShapeName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SIZE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsize" Text='<%#Bind("SizeFt") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="STOCK NO.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltstockno" Text='<%#Bind("TStockNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtPrice" Text='<%#Bind("Price") %>' runat="server" Width="70px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
                                            <asp:Label ID="LblPrice" Text='<%#Bind("Price") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr id="TRcurrentstock" runat="server" visible="false">
                    <td>
                        <asp:Label Text="Transaction Date" runat="server" CssClass="labelbold" />
                    </td>
                    <td>
                        <asp:TextBox ID="txttrandate" CssClass="textb" runat="server" Width="80px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="txttrandate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label9" Text="Current Stock" runat="server" CssClass="labelbold" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtcurrentstock" CssClass="textb" runat="server" Width="80px" />
                    </td>
                    <td>
                        <asp:Button ID="btnupdatestock" Text="Update stock" runat="server" CssClass="buttonnorm"
                            OnClick="btnupdatestock_Click" Enabled="false" />
                    </td>
                </tr>
            </table>

            <table>
                <tr id="TRFinishedStockReport" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label12" Text="From Date" runat="server" CssClass="labelbold" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" CssClass="textb" runat="server" Width="80px" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label13" Text="To Date" runat="server" CssClass="labelbold" />
                    </td>
                    <td>
                        <asp:TextBox ID="TxtToDate" CssClass="textb" runat="server" Width="80px" />
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtToDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="BtnExcelPreview" Text="Excel Preview" runat="server" CssClass="buttonnorm"
                            OnClick="BtnExcelPreview_Click" />
                    </td>
                </tr>
            </table>

            <asp:HiddenField ID="HDF1" runat="server" />
            <asp:HiddenField ID="hnstockid" runat="server" Value="0" />
            <asp:HiddenField ID="hnqtyinhand" runat="server" Value="0" />
            <asp:HiddenField ID="hnVarNewQualitySize" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpriview" />
            <asp:PostBackTrigger ControlID="BtnExcelPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
