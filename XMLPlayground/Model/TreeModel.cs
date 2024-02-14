using System.Xml.Serialization;
using ConsoleApp.XMLModel;

namespace ConsoleApp.TreeModel
{
    public class RSTree // main root
    {
        [XmlElement(ElementName="Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName="Articles")]
        public ArticleHierarchiesTree ArticleHierarchiesTree { get; set; } = new ArticleHierarchiesTree();
    }

    public class ArticleHierarchiesTree
    {
        [XmlElement(ElementName="Article")]
        public List<ArticleHierarchyTree> Children { get; set; } = new List<ArticleHierarchyTree>();
    }

    public class ArticleHierarchyTree 
    {
        [XmlElement(ElementName="Id")]
        public string ArticleHierarchyId { get; set; }
        [XmlElement(ElementName="DisplayId")]
        public string ArticleHierarchyDisplayId { get; set; }
        [XmlElement(ElementName="Name")]
        public string ArticleHierarchyName { get; set; }
        [XmlElement(ElementName="LevelNo")]
        public string ArticleHierarchyLevelNo { get; set; }
        [XmlElement(ElementName="StatusNo")]
        public string ArticleHierarchyStatusNo { get; set; }
        [XmlElement(ElementName="ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
        [XmlElement(ElementName="ParentArticleId")]
        public string ParentArticleHierarchyId { get; set; }
        [XmlElement(ElementName="DefaultMarkupPercentage")]
        public string DefaultMarkupPercentage { get; set; }
        [XmlIgnore]
        public ArticleHierarchyTree Root { get; set; }
        [XmlElement(ElementName="Articles")]
        public ArticleHierarchiesTree ArticleHierarchiesTree { get; set; } = new ArticleHierarchiesTree();
    }
}