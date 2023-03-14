<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Warpingratemaster.ascx.cs"
    Inherits="UserControls_Warpingratemaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    function closeForm() {

        var objParent = window.opener;
        if (objParent != null) {
            self.close();
        }
        else {
            window.location.href = "../../main.aspx";
        }
    }
    function Jscriptvalidate() {
        $(document).ready(function () {
            $("#<%=btnsave.ClientID %>").click(function () {
                var Message = "";
                var selectedindex = $("#<%=DDcategoryName.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Category Name!!\n";
                }
                var selectedindex = $("#<%=DDitemname.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Item Name!!\n";
                }
//                if ($("#<%=Tdquality.ClientID %>").is(':visible')) {
//                    var selectedindex = $("#<%=DDquality.ClientID %>").attr('selectedIndex');
//                    if (selectedindex <= 0) {
//                        Message = Message + "Please select Quality. !!\n";
//                    }
//                }
//                //                if ($("#<%=Tddesign.ClientID %>").is(':visible')) {
                //                    var selectedindex = $("#<%=DDdesign.ClientID %>").attr('selectedIndex');
                //                    if (selectedindex <= 0) {
                //                        Message = Message + "Please select Design. !!\n";
                //                    }
                //                }
                //                if ($("#<%=Tdcolor.ClientID %>").is(':visible')) {
                //                    var selectedindex = $("#<%=DDcolor.ClientID %>").attr('selectedIndex');
                //                    if (selectedindex <= 0) {
                //                        Message = Message + "Please select Color. !!\n";
                //                    }
                //                }
                //                if ($("#<%=Tdshadename.ClientID %>").is(':visible')) {
                //                    var selectedindex = $("#<%=DDshadename.ClientID %>").attr('selectedIndex');
                //                    if (selectedindex <= 0) {
                //                        Message = Message + "Please select Shade Name. !!\n";
                //                    }
                //                }
                                if ($("#<%=Tdshape.ClientID %>").is(':visible')) {
                                    var selectedindex = $("#<%=DDshape.ClientID %>").attr('selectedIndex');
                                    if (selectedindex <= 0) {
                                        Message = Message + "Please select Shape. !!\n";
                                    }
                                }
                //                if ($("#<%=TdSize.ClientID %>").is(':visible')) {
                //                    var selectedindex = $("#<%=DDsize.ClientID %>").attr('selectedIndex');
                //                    if (selectedindex <= 0) {
                //                        Message = Message + "Please select Size. !!\n";
                //                    }
                //                }
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
<asp:UpdatePanel ID="upd1" runat="server">
    <ContentTemplate>
        <script type="text/javascript" language="javascript">
            Sys.Application.add_load(Jscriptvalidate);
        </script>
        <div style="margin: 1% 20% 0% 20%">
            <div style="width: 75%">
                <table border="1"  cellspacing="2" >
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label10" Text="Category Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDcategoryName" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDcategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="border-style: dotted">
                            <asp:Label ID="lblitemName" Text="Item Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDitemname" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDitemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Tdquality" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label1" Text="Quality Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDquality" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" onselectedindexchanged="DDquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Tddesign" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label3" Text="Design Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDdesign" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" onselectedindexchanged="DDdesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="Tdcolor" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label11" Text="Color Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDcolor" CssClass="dropdown" runat="server" Width="200px"  AutoPostBack="true"
                                onselectedindexchanged="DDcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Tdshadename" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label7" Text="Shade Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDshadename" CssClass="dropdown" runat="server"  AutoPostBack="true"
                                Width="200px" onselectedindexchanged="DDshadename_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Tdshape" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label12" Text="Shape Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDshape" CssClass="dropdown" runat="server" Width="200px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TdSize" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label2" Text="Size" runat="server" CssClass="labelbold" />
                            <asp:DropDownList CssClass="dropdown" Width="50px" ID="DDsizetype" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="DDsize" CssClass="dropdown" runat="server" Width="200px" 
                                onselectedindexchanged="DDsize_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                         <td id="TDJobName" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label6" Text="Job Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDjobname" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDjobname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                       
                    </tr>
                     <tr>
                      <td id="TDEmpName" runat="server" visible="false" style="border-style: dotted">
                            <asp:Label ID="Label8" Text="Emp Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDEmpName" CssClass="dropdown" runat="server">
                            </asp:DropDownList>
                        </td>
                     </tr>
                </table>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblrrate" CssClass="labelbold" Text="Rate" runat="server" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblRate" Text="Rate(Rs.)" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtRate" CssClass="textb" Width="120px" runat="server" onkeypress="return isNumberKey(event);" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="Effective Date" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txteffectivedate" CssClass="textb" Width="120px" runat="server" />
                                <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txteffectivedate"
                                    Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" Text="New" OnClick="btnnew_Click" />
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" ForeColor="Red" CssClass="labelbold" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div style="overflow: auto; max-height: 500px">
                                <asp:GridView ID="DgDetail" CssClass="grid-views" runat="server" AutoGenerateColumns="false"
                                    EmptyDataText="No records found." AutoGenerateSelectButton="true" OnRowDataBound="DgDetail_RowDataBound"
                                    OnSelectedIndexChanged="DgDetail_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemName" Text='<%#Bind("Item_name") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" Text='<%#Bind("Qualityname") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Design Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesign" Text='<%#Bind("Designname") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcolorname" Text='<%#Bind("colorname") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shade Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblshade" Text='<%#Bind("shadecolorname") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shape">
                                            <ItemTemplate>
                                                <asp:Label ID="lblshape" Text='<%#Bind("shapename") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsize" Text='<%#Bind("Size") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job_Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcessName" Text='<%#Bind("ProcessName") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emp_Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Outside Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorate" Text='<%#Bind("ORate") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Effective From">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleffectivefrom" Text='<%#Bind("Effectivedate") %>' runat="server"
                                                    CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective To">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" Text='<%#Bind("Todate") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemid" Text='<%#Bind("Itemid") %>' runat="server" />
                                                <asp:Label ID="lblqualityid" Text='<%#Bind("Qualityid") %>' runat="server" />
                                                <asp:Label ID="lbldesignid" Text='<%#Bind("designid") %>' runat="server" />
                                                <asp:Label ID="lblcolorid" Text='<%#Bind("colorid") %>' runat="server" />
                                                <asp:Label ID="lblid" Text='<%#Bind("id") %>' runat="server" />
                                                <asp:Label ID="lblcategoryid" Text='<%#Bind("categoryid") %>' runat="server" />
                                                <asp:Label ID="lblshadecolorid" Text='<%#Bind("SHADEID") %>' runat="server" />
                                                <asp:Label ID="lblshapeid" Text='<%#Bind("shapeid") %>' runat="server" />
                                                <asp:Label ID="lblsizeid" Text='<%#Bind("sizeid") %>' runat="server" />
                                                <asp:Label ID="lblsizeflag" Text='<%#Bind("sizeflag") %>' runat="server" />
                                                <asp:Label ID="lblprocessid" Text='<%#Bind("processid") %>' runat="server" CssClass="labelbold" />
                                                <asp:Label ID="lblEmpId" Text='<%#Bind("EmpId") %>' runat="server" CssClass="labelbold" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
