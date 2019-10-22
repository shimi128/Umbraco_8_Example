using Umbraco.Core;
using Umbraco.Core.Composing;
using WebApplicationShimi.Indexer;

namespace WebApplicationShimi.Components
{
    public class ArticleComposer:IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<ArticleComponent>();
            composition.RegisterUnique<ArticleIndexCreator>();
        }
    }
}