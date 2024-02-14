using ConsoleApp.TreeModel;
using ConsoleApp.XMLModel;

namespace ConsoleApp
{
    public class TreeBuilder
    {
        public RSTree Build(RS rs)
        {
            var tree = new RSTree
            {
                Header = rs.Header
            };
            foreach (var article in rs.ArticleHierarchyData.ArticleHierarchies.ArticleHierarchy)
            {
                AddNode(tree, article);
            }
            return tree;
        }

        public void AddNode(RSTree root, ArticleHierarchy article)
        {
            if(article.ArticleHierarchyLevelNo == "1")
                {
                    var node = BuildNode(article);
                    root.ArticleHierarchiesTree.Children.Add(node);
                }
                else
                {
                    ArticleHierarchyTree? parent = null;

                    parent = root.ArticleHierarchiesTree.Children.FirstOrDefault(x => x.ArticleHierarchyId.Equals(article.ParentArticleHierarchyId));

                    foreach(var childNode in root.ArticleHierarchiesTree.Children)
                    {
                        if (parent != null)
                            break;
                        parent = FindParent(childNode, article.ParentArticleHierarchyId);
                    }
                    if (parent != null)
                    {
                        var node = BuildNode(article);
                        node.Root = parent;
                        parent.ArticleHierarchiesTree.Children.Add(node);
                    }
                    else
                    {
                        var node = BuildNode(article);
                        node.ArticleHierarchyLevelNo = "1";
                        root.ArticleHierarchiesTree.Children.Add(node);
                    }
                }
        }

        public ArticleHierarchyTree? FindParent(ArticleHierarchyTree root, string parentArticleHierarchyId)
        {
            foreach(var node in root.ArticleHierarchiesTree.Children)
            {
                if(node.ArticleHierarchyId.Equals(parentArticleHierarchyId))
                {
                    return node;
                }
                else
                {
                    var parent = FindParent(node, parentArticleHierarchyId);
                    if (parent != null)
                        return parent;
                }
            }
            return null;
        }

        public ArticleHierarchyTree BuildNode(ArticleHierarchy article)
        {
            return new ArticleHierarchyTree
            {
                ArticleHierarchyId = article.ArticleHierarchyId,
                ArticleHierarchyDisplayId = article.ArticleHierarchyDisplayId,
                ArticleHierarchyName = article.ArticleHierarchyName,
                ArticleHierarchyLevelNo = article.ArticleHierarchyLevelNo,
                ArticleHierarchyStatusNo = article.ArticleHierarchyStatusNo,
                ModifiedDate = article.ModifiedDate,
                ParentArticleHierarchyId = article.ParentArticleHierarchyId,
                DefaultMarkupPercentage = article.DefaultMarkupPercentage,
            };
        }
    }
}