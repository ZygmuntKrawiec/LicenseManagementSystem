using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using LicenseManagementSystemPresentationLayer.LicenseManagementSystemWebService;
using System.Data.SqlClient;
using System.Data;
using LicenseManagementSystemPresentationLayer.Code;


namespace LicenseManagementSystemPresentationLayer
{
    public partial class Welcome : System.Web.UI.Page
    {
        // WebService Client instance.
        LicenseManagementSystemWebServiceSoapClient wsClient = new LicenseManagementSystemWebServiceSoapClient();
        User user;
        // Helper class 
        LicensesDataBinder licenseDataBinder;

        // Events
        protected override void OnPreInit(EventArgs e)
        {
            // Check if user is authenticated or user guid contains a value, to prevent user null exception.
            if (User.Identity.Name.ToString() == null || Session.Count == 0)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("Login.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Create data binder
            licenseDataBinder = new LicensesDataBinder(gvLicenseData, rprPages, ddlRowsPerPage);
            user = new User() { UserEmail = User.Identity.Name.ToString(), UserAccessNumber = (Guid)Session["loggedUsersAccessNumber"] };
            if (!Page.IsPostBack)
            {
                // Display a name of logged user on page.
                lblUserName.Text = User.Identity.Name.ToString();

                // Read a first portion of data and display it in a gridview.
                licenseDataBinder.LicensesDataBind(wsClient, user, 0, 0, true, 10);

                // Save an index of sorted column.
                ViewState["indexSortedColumn"] = 0;

                // Save a sort direction (ASC - true, DESC - false).
                ViewState["sortDirection"] = true;
            }
        }

        protected void lbnLogout_Click(object sender, EventArgs e)
        {
            // Remove logging data from web service.
            wsClient.LogoutUser(user);

            // Remove authentications data from the browser. 
            FormsAuthentication.SignOut();

            // Back to login page with the message "You have successfully logout.".
            FormsAuthentication.RedirectToLoginPage("message=You have successfully logout.");
        }

        protected void grvLicenseData_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Take current page number.
            int currentPageNumber = LicensesDataBinder.GetCurrentPageNumber(rprPages);

            // Take index column to sort.      
            TableCell x1 = ((GridView)sender).HeaderRow.Cells.OfType<TableCell>().Where(t => t.Controls.OfType<LinkButton>().Any(l => l.Text == e.SortExpression)).First();
            var columnIndex = ((GridView)sender).HeaderRow.Cells.GetCellIndex(x1);

            // Save the sorted column index.
            ViewState["indexSortedColumn"] = columnIndex;

            // From a drope down list take a number of rows to display in a gridview.
            int rows = int.Parse(ddlRowsPerPage.SelectedItem.Value);

            // Choose a sorting direction (if true then ASC if false then DESC).            
            licenseDataBinder.LicensesDataBind(wsClient, user, currentPageNumber, columnIndex, !(bool)ViewState["sortDirection"], rows);
            ViewState["sortDirection"] = !(bool)ViewState["sortDirection"];
        }

        protected void lbnPage_Click(object sender, EventArgs e)
        {
            // Take a number of a clicked gridview page.
            int pageNumber = int.Parse(((LinkButton)sender).CommandArgument);

            // Take a number of rows to display in a gridview
            int rows = int.Parse(ddlRowsPerPage.SelectedItem.Value);

            // Bind a chosen portion of data to the gridview           
            licenseDataBinder.LicensesDataBind(wsClient, user, pageNumber, (int)ViewState["indexSortedColumn"], (bool)ViewState["sortDirection"], rows);
            
            // Clear all texboxes
            txtUserEmail.Text = string.Empty;
            txtUserName.Text = string.Empty;

        }

        protected void ddlRowsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Take a number of rows to display in a gridview
            int rows = int.Parse(((DropDownList)sender).SelectedItem.Value);

            // Bind a chosen portion of data to the gridview           
            licenseDataBinder.LicensesDataBind(wsClient, user, 0, (int)ViewState["indexSortedColumn"], (bool)ViewState["sortDirection"], rows);
            //ViewState["sortDirection"] = false;

            // Clear all texboxes
            txtUserEmail.Text = string.Empty;
            txtUserName.Text = string.Empty;
        }

        protected void gvLicenseData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowType != DataControlRowType.Header)
            {
                // Adds to all cells OnClick event and change the cursor to a pointer. 
                for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    e.Row.Cells[columnIndex].Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink((Control)sender, $"${e.Row.RowIndex.ToString()}");
                    e.Row.Cells[columnIndex].Attributes["style"] = "cursor:pointer;cursor:hand;";
                }
            }
        }

        protected void gvLicenseData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Make all texboxes visible
            // If's to REFACTOR          

            if (((GridView)sender).Rows.OfType<GridViewRow>().Any(x => x.Attributes["Style"] == "Background:Green"))
            {
                ((GridView)sender).Rows.OfType<GridViewRow>().First(x => x.Attributes["Style"] == "Background:Green").Attributes["Style"] = "Background:none";
            }

            int colRowIndexes;
            if (int.TryParse(e.CommandArgument.ToString(), out colRowIndexes))
            {
                txtUserName.Text = ((GridView)sender).Rows[colRowIndexes].Cells[0].Text;
                txtUserEmail.Text = ((GridView)sender).Rows[colRowIndexes].Cells[1].Text;
                ((GridView)sender).Rows[colRowIndexes].Attributes["Style"] = "Background:Green";
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtUserEmail.Text))
            {
                // Add a new license to a database and reads the result. If true license was added if false license could not be added.
                bool result = wsClient.AddNewLicenseData("DupaEmail4", Guid.Parse("d2d647d0-dfbd-40c2-a372-c14f6b88bf5a"), txtUserName.Text, txtUserEmail.Text);

                // Sets a proper text colour to the label which displays result message.
                lblMessages.ForeColor = result ? System.Drawing.Color.Green : System.Drawing.Color.Red;

                // Displays in the label a message about result of adding a license into database.
                lblMessages.Text = result ? "User license data added" : $"User with {txtUserEmail.Text} email already exists.";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete a license from a database and reads the result. If true license was deleted if false license could not be deleted.
            bool result = wsClient.DeleteLicenseFromDatabase("DupaEmail4", Guid.Parse("d2d647d0-dfbd-40c2-a372-c14f6b88bf5a"), txtUserName.Text, txtUserEmail.Text);

            // Sets a proper text colour to the label which displays result message.
            lblMessages.ForeColor = result ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            // Displays in the label a message about result of deleting a license from a database.
            lblMessages.Text = result ? "User license data deleted." : $"User with {txtUserName.Text} name and {txtUserEmail.Text} email could not be deleted.";
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            // TODO: Check whats happen if js is disabled on client side
            bool result = false;
            if (gvLicenseData.Rows.OfType<GridViewRow>().Any(x => x.Attributes["Style"] == "Background:Green"))
            {
                int rowIndex = gvLicenseData.Rows.OfType<GridViewRow>().First(x => x.Attributes["Style"] == "Background:Green").RowIndex;
                string oldUserName = gvLicenseData.Rows[rowIndex].Cells[0].Text;
                string oldUserEmail = gvLicenseData.Rows[rowIndex].Cells[1].Text;
                // Modify a license from a database and reads the result. If true license was deleted if false license could not be deleted.
                result = wsClient.ModifyLicenseData("DupaEmail4", Guid.Parse("d2d647d0-dfbd-40c2-a372-c14f6b88bf5a"), txtUserName.Text, txtUserEmail.Text, oldUserName, oldUserEmail);
            }
            else
            {
                result = false;
            }
            // Sets a proper text colour to the label which displays result message.
            lblMessages.ForeColor = result ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            // Displays in the label a message about result of deleting a license from a database.
            lblMessages.Text = result ? "User license data was modyfied." : $"User with {txtUserName.Text} name and {txtUserEmail.Text} email could not be modyfied.";

        }
    }
}