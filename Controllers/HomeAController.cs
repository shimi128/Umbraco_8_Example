using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Examine;
using Examine.Search;
using Serilog.Core;
using Umbraco.Examine;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using WebApplicationShimi.Models;

namespace WebApplicationShimi.Controllers
{
    public class HomeAController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult NewsLetter()
        {
            Logger.Info(typeof(HomeAController), "NewsLetter Get");
            //var articles = Umbraco.ContentAtXPath($"\\{Articles.ModelTypeAlias}").OfType<Articles>();
            var model = new NewsLetterModel();
            return PartialView("_NewsLetterForm", model);
        }

        [HttpPost]
        public ActionResult NewsLetterPost(NewsLetterModel model)
        {
            if (!ModelState.IsValid) return CurrentUmbracoPage();

            return Content(model.EmailAddress);
        }

        public ActionResult Search(string query)
        {
            var searchResPage = CurrentPage.AncestorOrSelf(1).FirstChild<SearchResults>();
            if (ExamineManager.Instance.TryGetIndex("ArticleIndex", out var index))
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
                   return RedirectToUmbracoPage(searchResPage);

                }
            }

            return CurrentUmbracoPage();
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