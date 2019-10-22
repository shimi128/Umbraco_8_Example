using Examine;
using Umbraco.Core.Composing;
using WebApplicationShimi.Indexer;

namespace WebApplicationShimi.Components
{
    public class ArticleComponent:IComponent
    {
        private readonly IExamineManager _examineManager;
        private readonly ArticleIndexCreator _articleIndexCreator;

        public ArticleComponent(IExamineManager examineManager, ArticleIndexCreator articleIndexCreator)
        {
            _examineManager = examineManager;
            _articleIndexCreator = articleIndexCreator;
        }

        public void Initialize()
        {
            foreach (var index in _articleIndexCreator.Create())
            {
                _examineManager.AddIndex(index);
            };
        }

        public void Terminate()
        {
            
        }
    }
}