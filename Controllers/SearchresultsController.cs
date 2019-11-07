using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Examine;
using Examine.Search;
using Umbraco.Examine;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace WebApplicationShimi.Controllers
{
    public class SearchResultsController : RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            var query = Request.QueryString["query"];

            if (ExamineManager.Instance.TryGetIndex("ExternalIndex", out var index))
            {
                var searcher = index.GetSearcher();
                var searchTerms = SplitSearchTerms(query);
                var querySearcher = searcher.CreateQuery("content",BooleanOperation.Or).NodeTypeAlias("article");
                foreach (var examineValue in searchTerms)
                {
                    querySearcher.And().Field("nodeName", examineValue);
                }
                var results = querySearcher.Not().Field("umbracoNaviHide","1").Execute();
                if (results.Any())
                {
                    var total = results.TotalItemCount;
                    var res = Umbraco.Content(results.Select(x => x.Id)).ToList();
                    TempData["Result"]=res;
                }
            }

            return base.Index(model);
        }

        private IEnumerable<IExamineValue> SplitSearchTerms(string searchTerm)
        {
            var arr = Regex.Replace(searchTerm, "\\s+", " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return arr.Select((x, i) =>
            {
                var val = x.Escape();
                //We want the last word to have a * wildcard attached
                if (i == arr.Length - 1)
                {
                    val = val.Value.MultipleCharacterWildcard();
                }


                return val;

            }).ToArray();
        }
    }
}