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


namespace LicenseManagementSystemPresentationLayer
{
    public partial class Welcome : System.Web.UI.Page
    {
        // WebService Client instance.
        LicenseManagementSystemWebServiceSoapClient wsClient = new LicenseManagementSystemWebServiceSoapClient();

        // Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Display a name of logged user on page.
                lblUserName.Text = User.Identity.Name.ToString();
                // LicensesDataBind(User.Identity.Name.ToString(), (Guid)Session["loggedUsersAccessNumber"], 1, 1, true);
#if (!DEBUG)
                throw new Exception("Uncomment the line above and a line in an authorization tag in web.config, change userdata in the line 136 ");
#endif
                // Read a first portion of data and display it in a gridview.
                licensesDataBind(0, 0, true, 10);

                // Save an index of sorted column.
                ViewState["indexSortedColumn"] = 0;

                // Save a sort direction (ASC - true, DESC - false).
                ViewState["sortDirection"] = false;
            }
        }

        protected void lbnLogout_Click(object sender, EventArgs e)
        {
            // Remove logging data from web service.
            wsClient.LogoutUser(User.Identity.Name.ToString(), (Guid)Session["loggedUsersAccessNumber"]);

            // Remove authentications data from the browser. 
            FormsAuthentication.SignOut();

            // Back to login page with the message "You have successfully logout.".
            FormsAuthentication.RedirectToLoginPage("message=You have successfully logout.");
        }

        protected void grvLicenseData_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Take current page number.
            int currentPageNumber = getCurrentPageNumber();

            // Take index of column to sort.      
            TableCell x1 = ((GridView)sender).HeaderRow.Cells.OfType<TableCell>().Where(t => t.Controls.OfType<LinkButton>().Any(l => l.Text == e.SortExpression)).First();
            var columnIndex = ((GridView)sender).HeaderRow.Cells.GetCellIndex(x1);

            // Save the sorted column index.
            ViewState["indexSortedColumn"] = columnIndex;

            // From a drope down list take a number of rows to display in a gridview.
            int rows = int.Parse(ddlRowsPerPage.SelectedItem.Value);

            // Choose a sorting direction (if true then ASC if false then DESC).           
            licensesDataBind(currentPageNumber, columnIndex, (bool)ViewState["sortDirection"], rows);
            ViewState["sortDirection"] = !(bool)ViewState["sortDirection"];
        }

        protected void lbnPage_Click(object sender, EventArgs e)
        {
            // Take a number of a clicked gridview page.
            int pageNumber = int.Parse(((LinkButton)sender).CommandArgument);

            // Take a number of rows to display in a gridview
            int rows = int.Parse(ddlRowsPerPage.SelectedItem.Value);

            // Bind a chosen portion of data to the gridview
            licensesDataBind(pageNumber, (int)ViewState["indexSortedColumn"], !(bool)ViewState["sortDirection"], rows);
        }

        protected void ddlRowsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Take a number of rows to display in a gridview
            int rows = int.Parse(((DropDownList)sender).SelectedItem.Value);

            // Bind a chosen portion of data to the gridview
            licensesDataBind(0, (int)ViewState["indexSortedColumn"], true, rows);
            ViewState["sortDirection"] = false;
        }

        // Helper methods - to redesin into another class.

        protected void dataBindRepeater(int pageNumber, int numberOfAllRows, int numberOfRowsPerPage)
        {
            // Create a container for page numbers
            List<ListItem> pages = new List<ListItem>();

            // Create containers to keep temp data for loops.
            int i = 0, allPages = 0;

            // If number o all rows in a data table are greater than number of rows displays on a single page,
            // then calculate a number of all pages.
            if (numberOfAllRows >= numberOfRowsPerPage)
            {
                allPages = (numberOfAllRows % numberOfRowsPerPage) == 0 ? (numberOfAllRows / numberOfRowsPerPage) : (numberOfAllRows / numberOfRowsPerPage) + 1;

                // Create and add all calculated pages into the container.
                do
                {
                    // Selected property is used to set visibility of the number on a web page.
                    pages.Add(new ListItem() { Value = i.ToString(), Text = (i + 1).ToString(), Enabled = i != pageNumber, Selected = true });
                    i++;
                } while (i < allPages);
            }
            else
            {
                // if number of rows on a page are greater than number of rows in a table then add only one page and set it visibility to false.
                pages.Add(new ListItem() { Value = i.ToString(), Text = (i + 1).ToString(), Enabled = i != pageNumber, Selected = false });
            }



            // Bind the container to the repeater.
            rprPages.DataSource = pages;
            rprPages.DataBind();
        }

        protected void licensesDataBind(int pageNumber, int columnToSort, bool typeOfSorting, int rowsOnPage)
        {
            // Get Licenses data from a database.
            LicensesContainer result = wsClient.GetLicensesData("DupaEmail4", Guid.Parse("d2d647d0-dfbd-40c2-a372-c14f6b88bf5a"), pageNumber, columnToSort, rowsOnPage, typeOfSorting);

            // Set number of rows to display in gridview
            gvLicenseData.PageSize = rowsOnPage;

            // Bind the data            
            gvLicenseData.DataSource = result.LicensesDataSet;
            gvLicenseData.DataBind();

            // Bind a page numbers.
            dataBindRepeater(pageNumber, result.NumberOfAllLicenses, int.Parse(ddlRowsPerPage.SelectedItem.Value));
        }

        protected int getCurrentPageNumber()
        {
            // Take current page number.
            var pageNumberCollection = rprPages.Items.OfType<RepeaterItem>().ToList().Select(r => r.Controls);
            // This part below need to be redesign into LINQ sequence            
            int currentPageNumber = -1;
            foreach (ControlCollection cc in pageNumberCollection)
            {
                foreach (Control c in cc)
                {
                    if (c is LinkButton)
                    {
                        if (currentPageNumber != -1)
                            break;
                        currentPageNumber = ((LinkButton)c).Enabled == false ? int.Parse(((LinkButton)c).CommandArgument) : -1;
                    }
                }
            }
            return currentPageNumber;
        }

    }
}