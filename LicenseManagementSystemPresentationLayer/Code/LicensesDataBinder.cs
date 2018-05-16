using LicenseManagementSystemPresentationLayer.LicenseManagementSystemWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LicenseManagementSystemPresentationLayer.Code
{
    /// <summary>
    /// Binds licenses data to controls in presentation layer.
    /// </summary>
    public class LicensesDataBinder
    {
        // Fields keep references to controls used to display licenses and other informations.
        GridView gridView;
        Repeater repeater;
        DropDownList dropDownList;

        /// <summary>
        /// Creates instance of LicensesDataBinder with reference to controls where licenses data are displayed.
        /// </summary>
        /// <param name="gridView">A GridView to display licenses data.</param>
        /// <param name="repeater">A Repeater to display number of pages.</param>
        /// <param name="dropDownList">A DropDownList to read how many rows should be displayed.</param>
        public LicensesDataBinder(GridView gridView, Repeater repeater, DropDownList dropDownList)
        {
            this.gridView = gridView;
            this.repeater = repeater;
            this.dropDownList = dropDownList;
        }

        // Instance methods
        /// <summary>
        /// Binds a portion of licenses recived from  LicenseManagementSystemWebService client.
        /// </summary>
        /// <param name="wsClient">LicenseManagementSystemWebService client.</param>
        /// <param name="user">A user with permission to read licenses data.</param>
        /// <param name="pageNumber">A number of current page displayed in GridView.</param>
        /// <param name="columnToSort">A number of column to sort.</param>
        /// <param name="typeOfSorting">True - ascending, false - descending.</param>
        /// <param name="rowsOnPage">A number of rows displayed on one page.</param>
        public void LicensesDataBind(LicenseManagementSystemWebServiceSoapClient wsClient, User user, int pageNumber, int columnToSort, bool typeOfSorting, int rowsOnPage)
        {
            // Get Licenses data from a database.
            LicensesContainer result = wsClient.GetLicensesData(user.UserEmail, user.UserAccessNumber, pageNumber, columnToSort, rowsOnPage, typeOfSorting);

            // Set number of rows to display in gridview
            gridView.PageSize = rowsOnPage;

            // Bind the data                   
            gridView.DataSource = result.LicensesDataSet;
            gridView.DataBind();

            // Bind a page numbers.
            repeaterDataBind(pageNumber, result.NumberOfAllLicenses, int.Parse(dropDownList.SelectedItem.Value));
        }

        /// <summary>
        /// Calculates number of all pages which can be displaed in GridView  and binds it to repeater.
        /// </summary>
        /// <param name="pageNumber">A number of current page displayed in GridView.</param>
        /// <param name="numberOfAllRows">A number of all records in a database table with licenses data.</param>
        /// <param name="numberOfRowsPerPage">A number of rows displayed on one page.</param>
        private void repeaterDataBind(int pageNumber, int numberOfAllRows, int numberOfRowsPerPage)
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
            repeater.DataSource = pages;
            repeater.DataBind();
        }

        // Static methods
        /// <summary>
        /// Finds an active licenses page number from a Repeater web control.
        /// </summary>
        /// <param name="rprPages">Repeater with LinkButtons to represent a page numbers.</param>
        /// <returns>Number of active page.</returns>
        public static int GetCurrentPageNumber(Repeater rprPages)
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