using System.Collections.Generic;
using Examine;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Examine;
using Umbraco.Web.Search;

namespace WebApplicationShimi.Indexer
{
    public class ArticleIndexCreator:LuceneIndexCreator, IUmbracoIndexesCreator
    {
        private readonly IProfilingLogger _profilingLogger;
        private readonly ILocalizationService _localizationService;
        private readonly IPublicAccessService _publicAccessService;

        public ArticleIndexCreator(IProfilingLogger profilingLogger, ILocalizationService localizationService, IPublicAccessService publicAccessService)
        {
            _profilingLogger = profilingLogger;
            _localizationService = localizationService;
            _publicAccessService = publicAccessService;
        }

        public override IEnumerable<IIndex> Create()
        {
            var index = new UmbracoContentIndex("ArticleIndex",
                CreateFileSystemLuceneDirectory("ArticleIndex"),
                new UmbracoFieldDefinitionCollection(),
                new StandardAnalyzer(Version.LUCENE_30),
                _profilingLogger,
                _localizationService,
                // We can use the ContentValueSetValidator to set up rules for the content we
                // want to have indexed. In our case we want published, non-protected nodes of the type "product".
                new ContentValueSetValidator(true, false,
                    _publicAccessService, includeItemTypes: new string[] { "article" }));

            return new[] { index };
        }
    }
}