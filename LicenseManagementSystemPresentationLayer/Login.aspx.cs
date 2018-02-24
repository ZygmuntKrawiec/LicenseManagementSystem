using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using LicenseManagementSystemPresentationLayer.LicenseManagementSystemWebService;

namespace LicenseManagementSystemPresentationLayer
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["message"] != null)
            {
                lblMessage.Visible = true;
                lblMessage.Text = Request.QueryString["message"];
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LicenseManagementSystemWebServiceSoapClient wsClient = new LicenseManagementSystemWebServiceSoapClient();
            Guid loggedUsersAccessNumber = wsClient.Login(txtEmail.Text, txtPassword.Text);
            if (Page.IsValid && loggedUsersAccessNumber != Guid.Empty)
            {
                Session["loggedUsersAccessNumber"] = loggedUsersAccessNumber;
                FormsAuthentication.RedirectFromLoginPage(txtEmail.Text, cbxRememberMe.Checked);
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Wrong user name or/and password";
            }
        }
    }
}