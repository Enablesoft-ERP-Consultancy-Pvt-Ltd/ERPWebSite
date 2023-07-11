<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeMaster.aspx.cs" MasterPageFile="~/ERPmaster.master" Inherits="Master_Recipe_RecipeMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
     <script type="text/javascript">
         function CloseForm() {
             window.location.href = "../../main.aspx";
         }
         function Preview() {
             window.open("../../ReportViewer.aspx", "GenrateIndentReport");
         }
       
         function ClickNew() {
             window.location.href = "RecipeMaster.aspx";
         }
       
    </script>
     <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
         <script type="text/javascript" language="javascript">
             Sys.Application.add_load(Jscriptvalidate);
            </script>
        <div style="height:300px" style="margin: 0% 10% 0% 10%">
                <table>
                <tr><td>DYEING HOUSE-RECIPE</td></tr>
                </table>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label1" Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                   
                    <table width="100%" border="1px solid grey">
                     <tr>
                         <td align="right" colspan="3">
                             <strong><span style="font:15px;">Receipe No.</span></strong><asp:Label 
                                 ID="lblredno" runat="server" style="font:15px;color:red">0</asp:Label>
                         </td>
                        <tr>
                            <td align="right" colspan="3">
                                <strong><span style="font:15px;">Receipe Date.</span></strong><asp:Label 
                                    ID="lblresdate" runat="server" style="font:15px;color:red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="lblcompany" runat="server" CssClass="labelbold" Text="Company" />
                                <br />
                                <asp:DropDownList ID="ddlcompany" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="Buyer Code" />
                                <br />
                                <asp:DropDownList ID="ddBuyerCode" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" OnSelectedIndexChanged="ddBuyerCode_SelectedIndexChanged" 
                                    Width="150px">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblcategory" runat="server" CssClass="labelbold" 
                                    Text="Category" />
                                <br />
                                <asp:DropDownList ID="ddlcategory" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Item Name" />
                                <br />
                                <asp:DropDownList ID="ddItemname" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" OnSelectedIndexChanged="ddItemname_SelectedIndexChanged" 
                                    Width="150px">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblquality" runat="server" CssClass="labelbold" Text="Quality" />
                                <br />
                                <asp:DropDownList ID="ddLocalQuality" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" 
                                    OnSelectedIndexChanged="ddLocalQuality_SelectedIndexChanged" Width="150px">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                             <td class="tdstyle">
                                <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Shade Color" />
                                <br />
                                <asp:DropDownList ID="ddlcolor" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" Width="150px" OnSelectedIndexChanged="ddlcolor_SelectedIndexChanged">
                                </asp:DropDownList>
                                
                            </td>
                          <%--  <td class="tdstyle">
                                <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="Design No." />
                                <br />
                                <asp:DropDownList ID="ddldesign" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" OnSelectedIndexChanged="ddldesign_SelectedIndexChanged" 
                                    Width="150px">
                                </asp:DropDownList>
                                
                            </td>--%>
                        </tr>
                        <tr>
                        <td class="tdstyle">
                                <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Dyeing Type" />
                                <br />
                                <asp:DropDownList ID="dddyingtype" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" 
                                    Width="150px">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                           <td class="tdstyle">
                                <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="DYES STUFF" />
                                <br />
                                <asp:DropDownList ID="dddyesstuff" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" Width="150px"  OnSelectedIndexChanged="dddyesstuff_SelectedIndexChanged" > </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            
                           <%-- <td>
                                <asp:Button ID="btnsearchedit" runat="server" CssClass="buttonnormal" 
                                    OnClick="btnsearchedit_Click" Style="width:72px;height:30px" Text="ADD" />
                            </td>--%>
                        </tr>
                         <tr>
                        <td class="tdstyle">
                                <asp:Label ID="lbltemp" runat="server" CssClass="labelbold" Text="Temperature (°C)" />
                                <br />
                               <%-- <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" 
                                    Width="150px">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txttemp" runat="server" Width="90px" CssClass="textb" ></asp:TextBox>
                                <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                           <td class="tdstyle">
                                <asp:Label ID="lblphvalue" runat="server" CssClass="labelbold" Text="pH Value" />
                                <br />
                                  <asp:TextBox ID="txtphvalue" runat="server" Width="90px" CssClass="textb" ></asp:TextBox>
                             <%--   <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" Width="150px"  OnSelectedIndexChanged="dddyesstuff_SelectedIndexChanged" > </asp:DropDownList>--%>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                             <td class="tdstyle">
                                <asp:Label ID="lblholdtime" runat="server" CssClass="labelbold" Text="Holding Time (Minutes)" />
                                <br />
                                  <asp:TextBox ID="txtholdtime" runat="server" Width="90px" CssClass="textb" ></asp:TextBox>
                             <%--   <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" Width="150px"  OnSelectedIndexChanged="dddyesstuff_SelectedIndexChanged" > </asp:DropDownList>--%>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            
                           <%-- <td>
                                <asp:Button ID="btnsearchedit" runat="server" CssClass="buttonnormal" 
                                    OnClick="btnsearchedit_Click" Style="width:72px;height:30px" Text="ADD" />
                            </td>--%>
                        </tr>
                         <tr>
                        <td class="tdstyle">
                                <asp:Label ID="lblpretreatment" runat="server" CssClass="labelbold" Text="Pre Treatment" />
                                <br />
                                <asp:DropDownList ID="ddlpretreatment" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown" 
                                    Width="150px" OnSelectedIndexChanged="ddlpretreatment_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Wetting"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Scouring"></asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                          
                                <%--<asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>--%>
                            </td>
                            
                           <%-- <td>
                                <asp:Button ID="btnsearchedit" runat="server" CssClass="buttonnormal" 
                                    OnClick="btnsearchedit_Click" Style="width:72px;height:30px" Text="ADD" />
                            </td>--%>
                        </tr>
                    </table>
                </fieldset>
                <%--<fieldset>
                    <legend>
                        <asp:Label ID="Label2" Text="Item Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td class="tdstyle">
                                <asp:Label ID="LblCategory" runat="server" Text="" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddCatagory" runat="server" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           
                        </tr>
                    </table>
                    
                </fieldset>--%>
              <asp:Label ID="lblmsg" ForeColor="Red" runat="server"></asp:Label>
            </div>
            <div style="overflow:auto">
                <fieldset>
                    <legend>
                        <asp:Label ID="Label8" Text="Pre-treatment" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table width="100%" border="1px solid grey">
                     <tr>
                     <td colspan="3">
                     <asp:GridView ID="grdpretreatment" Width="100%"  AutoGenerateColumns="false" CssClass="grid-views"
                                            runat="server" ShowHeader="true" OnRowDataBound="grdpretreatment_DataBound" EmptyDataText="No Chemical">
                                           <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                             <asp:BoundField DataField="SRNO" HeaderText="SR NO." />
                                           <%--   <asp:BoundField  DataField="QualityName" HeaderText="Chemical" />--%>
                                                <%--<HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />--%>
                                           <%-- </asp:BoundField>--%>
                                                <asp:TemplateField HeaderText="Chemical Type">
                                                    <ItemTemplate>
                                                  
                                                  <asp:Label ID="lblchemqualityname" Text='<%#Bind("QualityName") %>' runat="server" />

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BRAND">
                                                    <ItemTemplate>
                                                      <asp:DropDownList ID="dgprebrand" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgprebrand_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 
                                                 <asp:TemplateField HeaderText="UoM">
                                                    <ItemTemplate>
                                                     <asp:DropDownList ID="dgpreunit" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dgpreunit_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                        <%--<asp:Label ID="lblchemresunit" Text='<%#Bind("UNITNAME") %>' runat="server" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="PORTION">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtprechemresportion" OnTextChanged="txtprechemresportion_TextChanged" AutoPostBack="true" Text='<%#Bind("PORTION") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                 <asp:TemplateField HeaderText="Quantity(gms per Kg)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprechemresqty" Text='<%#Bind("QTY") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField HeaderText="Rate per Kg.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprechemresprate" Text='<%#Bind("PRATE") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField HeaderText="COST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprechemrescost" Text='<%#Bind("COST") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldgprequalityid" Text='<%#Bind("qualityid") %>' runat="server" />
                                                   
                                                </ItemTemplate>
                                            </asp:TemplateField> 

                                            </Columns>
                                        </asp:GridView>
                     </td>
                     </tr>
                     </table>
                     </fieldset>
                     </div>
            <div style="overflow:auto">
                <fieldset>
                    <legend>
                        <asp:Label ID="Label7" Text="Dye Bath" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table width="100%" border="1px solid grey">
                     <tr>
                     <td colspan="3">
                     <asp:GridView ID="dgchemical" Width="100%"  AutoGenerateColumns="false" CssClass="grid-views"
                                            runat="server" ShowHeader="true" OnRowDataBound="dgchemical_DataBound" EmptyDataText="No Chemical">
                                           <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                             <asp:BoundField DataField="SRNO" HeaderText="SR NO." />
                                              <asp:BoundField Visible="false" DataField="QualityName" HeaderText="Chemical" />
                                                <%--<HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />--%>
                                           <%-- </asp:BoundField>--%>
                                                <%--<asp:TemplateField HeaderText="DYES STUFF">
                                                    <ItemTemplate>
                                                      <asp:DropDownList ID="dgquality" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgquality_SelectedIndexChanged">
                                                    </asp:DropDownList>


                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Chemical Type">
                                                    <ItemTemplate>
                                                      <asp:DropDownList ID="dgchemdesign" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgchemdesign_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="BRAND">
                                                    <ItemTemplate>
                                                         <asp:DropDownList ID="dgchembrand" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgchembrand_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="UoM">
                                                    <ItemTemplate>
                                                       <asp:DropDownList ID="dgchemunit" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgchemunit_SelectedIndexChanged" >
                                                    </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="PORTION">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtchemresportion" OnTextChanged="txtchemresportion_TextChanged" AutoPostBack="true" Text='<%#Bind("PORTION") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                 <asp:TemplateField HeaderText="Quantity(gms per Kg)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblchemresqty" Text='<%#Bind("QTY") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField HeaderText="Rate per Kg.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblchemresprate" Text='<%#Bind("PRATE") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField HeaderText="COST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblchemrescost" Text='<%#Bind("COST") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                   <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldgchemqualityid" Text='<%#Bind("qualityid") %>' runat="server" />
                                                   
                                                </ItemTemplate>
                                            </asp:TemplateField> 

                                            </Columns>
                                        </asp:GridView>
                     </td>
                     </tr>
                     </table>
                     </fieldset>
                     </div>
        <div style="overflow:auto;">
                
                <fieldset>
                    <legend>
                        <asp:Label ID="Label2" Text="Dyes Stuff" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                   
                    <table width="100%" border="1px solid grey">
                     <tr>
                     <td colspan="3">
                     <asp:GridView Width="100%" ID="grdreceipe" AutoGenerateColumns="false" CssClass="grid-views"
                                            runat="server" ShowHeader="true" 
                             EmptyDataText="No Dyes Stuff" OnRowDataBound="grdreceipe_DataBound">
                                           <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                             <asp:BoundField DataField="SRNO" HeaderText="SR NO." />
                                            <%--  <asp:BoundField DataField="QualityName" HeaderText="DYES STUFF" />--%>
                                                <%--<HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />--%>
                                           <%-- </asp:BoundField>--%>
                                               <%-- <asp:TemplateField HeaderText="DYES STUFF">
                                                    <ItemTemplate>
                                                    <asp:Label ID="dglblquality" Text='<%#Bind("QualityName") %>' runat="server" />


                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="BRAND">
                                                    <ItemTemplate>
                                                      <asp:DropDownList ID="dgdesign" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgdesign_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="COLOR">
                                                    <ItemTemplate>
                                                         <asp:DropDownList ID="dgcolor" CssClass="dropdown" Width="150px" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="dgcolor_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="UoM">
                                                    <ItemTemplate>
                                                         <asp:DropDownList ID="dgresunit" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dgresunit_SelectedIndexChanged"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="PORTION">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtresportion" AutoPostBack="true" OnTextChanged="txtresportion_TextChanged" Text='<%#Bind("PORTION") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                 <asp:TemplateField HeaderText="Quantity per Kg.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblresqty" Text='<%#Bind("QTY") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField HeaderText="Rate per Kg.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblresprate" Text='<%#Bind("PRATE") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                  <asp:TemplateField HeaderText="COST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrescost" Text='<%#Bind("COST") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldgqualityid" Text='<%#Bind("QualityId") %>' runat="server" />
                                                   
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            </Columns>
                                        </asp:GridView>
                     </td>
                     </tr>
                     </table>
                     </fieldset>
                     </div>
        
                     <div>
                     <table width="100%">
                     <tr>
                     <td colspan="3" align="right">
                     <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew();" />
                            <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" Text="Save" ValidationGroup="f1" OnClick="btnsave_Click"  />
                            <asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                           <%-- <asp:Button CssClass="buttonnorm" ID="BtnCancel" OnClientClick="return confirm('Do you want to Cancel Indent?')"
                                runat="server" Text="Cancel Indent" OnClick="BtnCancel_Click" />--%>
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                     </td>
                     </tr>
                     </table>
                     </div>

        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
        </asp:UpdatePanel>

</asp:Content>

