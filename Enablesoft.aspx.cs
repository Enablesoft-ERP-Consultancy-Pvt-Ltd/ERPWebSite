using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Enablesoft : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btngetdecryptvalue_Click(object sender, EventArgs e)
    {
        lblval.Text = UtilityModule.Decrypt(txtpwd.Text);
    }
    protected void btnencrypted_Click(object sender, EventArgs e)
    {
        lblencryptval.Text = UtilityModule.Encrypt(txtencryptval.Text);
    }
}