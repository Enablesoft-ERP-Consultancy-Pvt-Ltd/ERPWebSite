<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmvendorwiserate.aspx.cs"
    Inherits="Masters_Campany_frmvendorwiserate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function jScriptValidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var message = "";
                    if ($("#<%=DDprocess.ClientID %>")) {
                        var selectedIndex = $("#<%=DDprocess.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Process Name!!!\n";
                        }
                    }
                    if ($("#<%=ddCatagory.ClientID %>")) {
                        var selectedIndex = $("#<%=ddCatagory.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select category Name!!!\n";
                        }
                    }

                    if ($("#<%=dditemname.ClientID %>")) {
                        var selectedIndex = $("#<%=dditemname.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Item Name!!!\n";
                        }
                    }

                    if ($("#TDQuality").is(':visible')) {
                        var selectedIndex = $("#<%=ddquality.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Quality Name!!!\n";
                        }
                    }
                    if ($("#TDDesign").is(':visible')) {
                        var selectedIndex = $("#<%=dddesign.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Design!!!\n";
                        }
                    }
                    if ($("#TDColor").is(':visible')) {
                        var selectedIndex = $("#<%=ddcolor.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Color!!!\n";
                        }
                    }
                    if ($("#TDShape").is(':visible')) {
                        var selectedIndex = $("#<%=ddshape.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Shape!!!\n";
                        }
                    }
                    if ($("#TDSize").is(':visible')) {
                        var selectedIndex = $("#<%=ddsize.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Size!!!\n";
                        }
                    }
                    if ($("#TDShade").is(':visible')) {
                        var selectedIndex = $("#<%=ddlshade.ClientID %>").attr('selectedIndex');
                        if (selectedIndex <= 0) {
                            message = message + "Please select Shade!!!\n";
                        }
                    }
                    // txtrate
                    if ($("#<%=txtrate.ClientID %>")) {
                        var val = $("#<%=txtrate.ClientID %>").val();
                        if (val == "") {
                            message = message + "Please Enter Rate!!!\n";
                        }
                    }
                    if (message == "") {
                        return true;
                    }
                    else {
                        alert(message);
                        return false;
                    }
                });
                $("#<%=btnclose.ClientID %>").click(function () {
                    window.close();
                });

            });
        }        
    </script>
    <script type="text/javascript">
        function isNumber(evt) {
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="scr1">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScriptValidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr id="TDprocess" runat="server">
                            <td runat="server" visible="false">
                                ProdCode
                                <br />
                                <asp:TextBox ID="txtprodcode" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblProcess" runat="server" class="tdstyle" Text="Process Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDprocess" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                    CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddquality" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false" class="style5">
                                <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" class="tdstyle" visible="false">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <%--<asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged" />Check
                                    for Mtr--%><asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                        <%--  <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                        <asp:ListItem Value="1">MTR</asp:ListItem>
                                        <asp:ListItem Value="2">Inch</asp:ListItem>--%>
                                    </asp:DropDownList>
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" class="tdstyle" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" runat="server" visible="false">
                                <asp:Label ID="Label16" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddlunit" runat="server" Width="115px" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="Td1" class="tdstyle" runat="server">
                                <asp:Label ID="lblrate" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtrate" runat="server" CssClass="textb" Width="70px" onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="margin-top: 10px; width: 550px">
                <table style="width: 550px">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" runat="server" CssClass="labelbold" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="height: 200px; width: 600px; overflow: auto">
                <asp:GridView ID="GDDetail" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                    EmptyDataText="No records found..." Width="590px">
                    <Columns>
                        <asp:BoundField DataField="Category_Name" HeaderText="Category" />
                        <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription" />
                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Dateadded" HeaderText="Dateadded">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="160px" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
