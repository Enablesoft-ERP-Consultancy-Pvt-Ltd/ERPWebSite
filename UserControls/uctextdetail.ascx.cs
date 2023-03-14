using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_uctextdetail : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public String TextBox1Text
    {
        get
        {
            return TextBox1.Text;
        }
        set
        {
            TextBox1.Text = value;
        }
    }
}