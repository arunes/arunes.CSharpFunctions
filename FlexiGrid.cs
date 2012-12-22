using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace arunes
{
    public class FlexiGrid
    {
        #region Definitions
        public struct Grid
        {
            public int page { get; set; }
            public int total { get; set; }
            public List<GridRow> rows { get; set; }
        }

        public struct GridRow
        {
            public string id { get; set; }
            public List<string> cell { get; set; }
        }

        public class GridColumn
        {
            public string Title { get; set; }
            public string FieldName { get; set; }
            public int Width { get; set; }
            public bool Sortable { get; set; }
            public System.Web.UI.WebControls.HorizontalAlign Align { get; set; }
            public bool DefaultSort { get; set; }
            public bool ShowInQuickSearch { get; set; }
            public bool DefaultQuickSearch { get; set; }

            public GridColumn(string title, string fieldName, int width)
            {
                Title = title;
                FieldName = fieldName;
                Width = width;
                Sortable = true;
                Align = System.Web.UI.WebControls.HorizontalAlign.Left;
                DefaultSort = false;
                ShowInQuickSearch = true;
            }

            public GridColumn(string title, string fieldName, int width, bool defaultSort)
            {
                Title = title;
                FieldName = fieldName;
                Width = width;
                Sortable = true;
                Align = System.Web.UI.WebControls.HorizontalAlign.Left;
                DefaultSort = defaultSort;
                ShowInQuickSearch = true;
                DefaultQuickSearch = false;
            }

            public GridColumn(string title, string fieldName, int width, bool defaultSort, bool showInQuickSearch)
            {
                Title = title;
                FieldName = fieldName;
                Width = width;
                Sortable = true;
                Align = System.Web.UI.WebControls.HorizontalAlign.Left;
                DefaultSort = defaultSort;
                ShowInQuickSearch = showInQuickSearch;
                DefaultQuickSearch = false;
            }

            public GridColumn(string title, string fieldName, int width, bool defaultSort, bool showInQuickSearch, bool defaultQuickSearch)
            {
                Title = title;
                FieldName = fieldName;
                Width = width;
                Sortable = true;
                Align = System.Web.UI.WebControls.HorizontalAlign.Left;
                DefaultSort = defaultSort;
                ShowInQuickSearch = showInQuickSearch;
                DefaultQuickSearch = defaultQuickSearch;
            }

            public GridColumn(string title, string fieldName, int width,
                bool sortable, System.Web.UI.WebControls.HorizontalAlign align,
                bool defaultSort, bool showInQuickSearch, bool defaultQuickSearch)
            {
                Title = title;
                FieldName = fieldName;
                Width = width;
                Sortable = sortable;
                Align = align;
                DefaultSort = defaultSort;
                ShowInQuickSearch = showInQuickSearch;
                DefaultQuickSearch = defaultQuickSearch;
            }
        }
        #endregion

        public string GridTableId { get; set; }
        public string Url { get; set; }

        public List<GridColumn> Columns;
        public string Title { get; set; }
        public string PagingMsg { get; set; }
        public string LoadingMsg { get; set; }
        public string QuickSearchMsg { get; set; }
        public string ClearMsg { get; set; }
        public string NoRecordMsg { get; set; }
        public int Height { get; set; }

        public System.Web.UI.WebControls.SortDirection SortDirection = System.Web.UI.WebControls.SortDirection.Ascending;
        public bool UsePager = true;
        public bool HideNavigation = true;
        public bool SingleSelect = true;
        public bool ShowQuickSearch = true;
        public bool ShowResultsPerPageBox = false;

        public FlexiGrid()
        {
            Columns = new List<GridColumn>();
        }

        public string GetJSCode()
        {
            return GetJSCode(false);
        }

        public string GetJSCode(bool addJsTags)
        {
            StringBuilder sb = new StringBuilder();

            if (addJsTags) sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function() {");
            sb.AppendLine("    $('#" + GridTableId + "').flexigrid({");
            sb.AppendLine("        url: '" + Url + "',");
            sb.AppendLine("        dataType: 'json',");
            sb.AppendLine("        colModel: [");

            if (Columns.Count > 0)
            { // column varsa
                foreach (var column in Columns)
                {
                    sb.AppendLine("            { display: '" + column.Title + "', name: '" + column.FieldName + "', width: "
                        + column.Width + ", sortable: " + FixBool(column.Sortable) + ", align: '" + column.Align + "' }" +
                        ((Columns.IndexOf(column) + 1) < Columns.Count ? "," : ""));
                }

                sb.AppendLine("        ],");

                string sortCol = Columns.Where(x => x.DefaultSort).Count() > 0 ? Columns.Where(x => x.DefaultSort).First().FieldName : Columns.First().FieldName;
                sb.AppendLine("        sortname: '" + sortCol + "',");
            }

            IEnumerable<GridColumn> SearchItems = Columns.Where(x => x.ShowInQuickSearch);
            if (SearchItems.Count() > 0)
            { // arama varsa
                sb.AppendLine("        searchitems: [");
                foreach (var sItem in SearchItems)
                {
                    sb.AppendLine("            { display: '" + sItem.Title + "', name: '" + sItem.FieldName + "' " + (sItem.DefaultQuickSearch ? ", isdefault: true" : "") + " }" +
                        (SearchItems.ElementAt(SearchItems.Count() - 1) != sItem ? "," : ""));
                }
                sb.AppendLine("        ],");
            }

            string sortDir = (SortDirection == System.Web.UI.WebControls.SortDirection.Ascending ? "asc" : "desc");
            sb.AppendLine("        sortorder: '" + sortDir + "',");
            sb.AppendLine("        usepager: " + FixBool(UsePager) + ",");
            sb.AppendLine("        hidenavigation: " + FixBool(HideNavigation) + ",");
            sb.AppendLine("        singleSelect: " + FixBool(SingleSelect) + ",");
            sb.AppendLine("        title: '" + Title + "',");

            if (!string.IsNullOrEmpty(PagingMsg))
            { // paging msg
                sb.AppendLine("        pagestat: '" + PagingMsg + "',");
            }

            if (!string.IsNullOrEmpty(LoadingMsg))
            { // loading msg
                sb.AppendLine("        procmsg: '" + LoadingMsg + "',");
            }

            if (!string.IsNullOrEmpty(QuickSearchMsg))
            { // quicksearch msg
                sb.AppendLine("        quicksearchmsg: '" + QuickSearchMsg + "',");
            }

            if (!string.IsNullOrEmpty(ClearMsg))
            { // clear msg
                sb.AppendLine("        clearmsg: '" + ClearMsg + "',");
            }

            if (!string.IsNullOrEmpty(NoRecordMsg))
            { // no rec msg
                sb.AppendLine("        nomsg: '" + NoRecordMsg + "',");
            }

            sb.AppendLine("        showquicksearch: " + FixBool(ShowQuickSearch) + ",");
            sb.AppendLine("        useRp: " + FixBool(ShowResultsPerPageBox) + ",");
            sb.AppendLine("        height: " + Height + "");
            sb.AppendLine("    });");
            sb.AppendLine("});");
            if (addJsTags) sb.AppendLine("</script>");

            return sb.ToString();
        }

        private string FixBool(bool val)
        {
            return val ? "true" : "false";
        }

        public static void GetFilteredData<modelType>(string sqlStr, System.Data.Linq.DataContext dataContext, out IEnumerable<modelType> model)
        {
            System.Collections.Specialized.NameValueCollection req = HttpContext.Current.Request.Form;
            string paramStr = null;
            if (!string.IsNullOrEmpty(req["query"]))
            {
                sqlStr += (sqlStr.Contains("WHERE") ? " AND " : " WHERE ") + "(" + req["qtype"] + " LIKE {0})";
                paramStr = string.Format("%{0}%", req["query"]);
            }

            sqlStr += " ORDER BY " + req["sortname"] + " " + req["sortorder"].ToUpper();

            if (string.IsNullOrEmpty(paramStr))
                model = dataContext.ExecuteQuery<modelType>(sqlStr);
            else
                model = dataContext.ExecuteQuery<modelType>(sqlStr, paramStr);
        }

        public static string GetJsonData(int page, List<GridRow> rows)
        {
            arunes.FlexiGrid.Grid grid = new arunes.FlexiGrid.Grid { page = 1, total = rows.Count(), rows = rows };
            JavaScriptSerializer ser = new JavaScriptSerializer();
            
            return ser.Serialize(grid);
        }
    }
}