<%@ Control Language="C#" AutoEventWireup="true" CodeFile="iexproMenu.ascx.cs" Inherits="UserControls_iexproMenu" %>









<nav class="navbar navbar-expand-sm menuss">
    <div class="container">
  
        <button class="navbar-toggler navbar-toggler-right" type="button" data-bs-toggle="collapse"
            data-bs-target="#navbar1">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbar1">



            <ul class="navbar-nav">

                <asp:Repeater ID="rptMenu" runat="server" OnItemDataBound="rptMenu_OnItemBound">
                    <ItemTemplate>
                        <li <%#  Convert.ToBoolean(Eval("IsChild")) ? "class='nav-item dropdown'" : "class='nav-item'" %>>
                            <a <%#  Convert.ToBoolean(Eval("IsChild")) ? "data-bs-toggle='dropdown' class='nav-link dropdown-toggle' data-bs-display='static'" : "class='nav-link'" %>
                                href="<%# this.GetItemUrl(Eval("MenuUrl") as string) %>">
                                <%# FirstCharToUpper( Eval("MenuName").ToString()) %>
                            </a>
                            <asp:Literal ID="ltrlSubMenu" runat="server"></asp:Literal>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>















            </ul>

        </div>
    </div>
</nav>






















