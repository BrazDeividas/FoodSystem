using System.Xml.Serialization;
using ConsoleApp.TreeModel;
using ConsoleApp.XMLModel;

namespace ConsoleApp
{
    public static class Program
    {
        static void Main()
        {
            var filename = "produktai.xml";
            var filePath = Path.Combine(Environment.CurrentDirectory, @"../../../files", filename);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RS));

            RS rs;

            using (Stream reader = new FileStream(filePath, FileMode.Open))
            {
                rs = (RS)xmlSerializer.Deserialize(reader);
            }

            TreeBuilder treeBuilder = new TreeBuilder();

            RSTree tree = treeBuilder.Build(rs);

            using(StreamWriter writer = new StreamWriter(@"../../../files/tree.xml"))
            {
                XmlSerializer treeSerializer = new XmlSerializer(typeof(RSTree));
                treeSerializer.Serialize(writer, tree);
            }
        }
    }
}