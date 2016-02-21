using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SortingPagingGridView
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int totalRows = 0;
                GridView1.DataSource = AccesOT.GetAllOT(GridView1.PageIndex, GridView1.PageSize, GridView1.Attributes["CustomSortField"], 
                                                        GridView1.Attributes["CustomSortDirection"], out totalRows);
                GridView1.DataBind();

                DatabindRepeater(GridView1.PageIndex, GridView1.PageSize, totalRows);
            }
        }

        protected void linkButton_Click(object sender, EventArgs e)
        {
            int totalRows = 0;
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            pageIndex -= 1;
            GridView1.PageIndex = pageIndex;
            GridView1.DataSource = AccesOT.GetAllOT(pageIndex, GridView1.PageSize, GridView1.Attributes["CustomSortField"], 
                                                        GridView1.Attributes["CustomSortDirection"], out totalRows);
            GridView1.DataBind();
            DatabindRepeater(pageIndex, GridView1.PageSize, totalRows);
        }

        private void DatabindRepeater(int pageIndex, int pageSize, int totalRows)
        {
            int totalPages = totalRows / pageSize;
            if ((totalRows % pageSize) != 0)
            {
                totalPages += 1;
            }

            List<ListItem> pages = new List<ListItem>();
            if (totalPages > 1)
            {
                for (int i = 1; i <= totalPages ; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != (pageIndex + 1)));
                }
            }
            reapeaterPaging.DataSource = pages;
            reapeaterPaging.DataBind();
        }

        private void SortGridView(GridView gridView, GridViewSortEventArgs e, out SortDirection sortDirection, out string sortField)
        {
            sortField = e.SortExpression;
            sortDirection = e.SortDirection;

            if (gridView.Attributes["CustomSortField"] != null && gridView.Attributes["CustomSortDirection"] != null)
            {
                if (sortField == gridView.Attributes["CustomSortField"])
                {
                    if (gridView.Attributes["CustomSortDirection"] == "ASC")
                    {
                        sortDirection = SortDirection.Descending;
                    }
                    else
                    {
                        sortDirection = SortDirection.Ascending;
                    }
                }
                gridView.Attributes["CustomSortField"] = sortField;
                gridView.Attributes["CustomSortDirection"] = (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string sortField = string.Empty;

            SortGridView((GridView)sender, e, out sortDirection, out sortField);
            string strSortDirection = sortDirection == SortDirection.Ascending ? "ASC" : "DESC";

            int totalRows = 0;
            GridView1.DataSource = AccesOT.GetAllOT(GridView1.PageIndex, GridView1.PageSize, e.SortExpression, strSortDirection, out totalRows);
            GridView1.DataBind();
            DatabindRepeater(GridView1.PageIndex, GridView1.PageSize, totalRows);

        }


        protected void GridView1_RowCreated1(object sender, GridViewRowEventArgs e)
        {
            if (GridView1.Attributes["CustomSortField"] != null && GridView1.Attributes["CustomSortDirection"] != null)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tableCell in e.Row.Cells)
                    {
                        if (tableCell.HasControls())
                        {
                            LinkButton sortLinkButton = null;
                            if (tableCell.Controls[0] is LinkButton)
                            {
                                sortLinkButton = (LinkButton)tableCell.Controls[0];
                            }

                            if (sortLinkButton != null && GridView1.Attributes["CustomSortField"] == sortLinkButton.CommandArgument)
                            {
                                Image image = new Image();
                                if (GridView1.Attributes["CustomSortDirection"] == "ASC")
                                {
                                    image.ImageUrl = "~/Images/haut.png";
                                }
                                else
                                {
                                    image.ImageUrl = "~/Images/bas.png";
                                }
                                tableCell.Controls.Add(new LiteralControl("&nbsp;"));
                                tableCell.Controls.Add(image);
                            }
                        }
                    }
                }
            }
        }


            

    }
}