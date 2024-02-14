using System.Xml.Serialization;

namespace ConsoleApp.XMLModel
{
    [XmlRoot(ElementName="Source")]
    public class Source {
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName="Message")]
    public class Message {
        [XmlElement(ElementName="ConversationId")]
        public string ConversationId { get; set; }
        [XmlElement(ElementName="Version")]
        public string Version { get; set; }
        [XmlElement(ElementName="CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [XmlElement(ElementName="SentDate")]
        public DateTime SentDate { get; set; }
        [XmlElement(ElementName="IsPartial")]
        public string IsPartial { get; set; }
        [XmlElement(ElementName="IsComplete")]
        public string IsComplete { get; set; }
        [XmlElement(ElementName="ContentType")]
        public string ContentType { get; set; }
    }

    [XmlRoot(ElementName="Header")]
    public class Header {
        [XmlElement(ElementName="Source")]
        public Source Source { get; set; }
        [XmlElement(ElementName="Message")]
        public Message Message { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchyLevel")]
    public class ArticleHierarchyLevel {
        [XmlElement(ElementName="ArticleHierarchyLevelNo")]
        public string ArticleHierarchyLevelNo { get; set; }
        [XmlElement(ElementName="ArticleHierarchyLevelName")]
        public string ArticleHierarchyLevelName { get; set; }
        [XmlElement(ElementName="ArticleHierarchyLevelStatusNo")]
        public string ArticleHierarchyLevelStatusNo { get; set; }
        [XmlElement(ElementName="DeletedDate")]
        public DateTime DeletedDate { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchyLevels")]
    public class ArticleHierarchyLevels {
        [XmlElement(ElementName="ArticleHierarchyLevel")]
        public List<ArticleHierarchyLevel> ArticleHierarchyLevel { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchy")]
    public class ArticleHierarchy {
        [XmlElement(ElementName="ArticleHierarchyId")]
        public string ArticleHierarchyId { get; set; }
        [XmlElement(ElementName="ArticleHierarchyDisplayId")]
        public string ArticleHierarchyDisplayId { get; set; }
        [XmlElement(ElementName="ArticleHierarchyName")]
        public string ArticleHierarchyName { get; set; }
        [XmlElement(ElementName="ArticleHierarchyLevelNo")]
        public string ArticleHierarchyLevelNo { get; set; }
        [XmlElement(ElementName="ArticleHierarchyStatusNo")]
        public string ArticleHierarchyStatusNo { get; set; }
        [XmlElement(ElementName="ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
        [XmlElement(ElementName="ParentArticleHierarchyId")]
        public string ParentArticleHierarchyId { get; set; }
        [XmlElement(ElementName="DefaultMarkupPercentage")]
        public string DefaultMarkupPercentage { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchies")]
    public class ArticleHierarchies {
        [XmlElement(ElementName="ArticleHierarchy")]
        public List<ArticleHierarchy> ArticleHierarchy { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchyExtraInfo")]
    public class ArticleHierarchyExtraInfo {
        [XmlElement(ElementName="ExtraInfoId")]
        public string ExtraInfoId { get; set; }
        [XmlElement(ElementName="ExtraInfoValue")]
        public string ExtraInfoValue { get; set; }
        [XmlElement(ElementName="ArticleHierarchyId")]
        public string ArticleHierarchyId { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchyExtraInfos")]
    public class ArticleHierarchyExtraInfos {
        [XmlElement(ElementName="ArticleHierarchyExtraInfo")]
        public ArticleHierarchyExtraInfo ArticleHierarchyExtraInfo { get; set; }
    }

    [XmlRoot(ElementName="ArticleHierarchyData")]
    public class ArticleHierarchyData {
        [XmlElement(ElementName="ArticleHierarchyLevels")]
        public ArticleHierarchyLevels ArticleHierarchyLevels { get; set; }
        [XmlElement(ElementName="ArticleHierarchies")]
        public ArticleHierarchies ArticleHierarchies { get; set; }
        [XmlElement(ElementName="ArticleHierarchyExtraInfos")]
        public ArticleHierarchyExtraInfos ArticleHierarchyExtraInfos { get; set; }
    }

    [XmlRoot(ElementName="RS")]
    public class RS {
        [XmlElement(ElementName="Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName="ArticleHierarchyData")]
        public ArticleHierarchyData ArticleHierarchyData { get; set; }
    }
}