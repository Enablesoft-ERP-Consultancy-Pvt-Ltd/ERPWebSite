<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Navigation.ascx.cs" Inherits="UserControls_Navigation" %>

<div class="navbar navbar-default">
    <div class="container-fluid">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1"
                aria-expanded="false">
                <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span
                    class="icon-bar"></span><span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="#">ASPSnippets</a>
        </div>
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav navbar-right">
                <li><a href="#"><span class="glyphicon glyphicon-log-in"></span>Login</a></li>
            </ul>
            <asp:Menu ID="tblMenu" runat="server" Orientation="Horizontal" RenderingMode="List"
                IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav navbar-nav" DynamicMenuStyle-CssClass="dropdown-menu">
            </asp:Menu>

        </div>


    </div>
</div>

<script type="text/javascript">
    //Disable the default MouseOver functionality of ASP.Net Menu control.
    Sys.WebForms.Menu._elementObjectMapper.getMappedObject = function () {
        return false;
    };

</script>
































