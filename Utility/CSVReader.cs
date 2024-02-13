using Microsoft.VisualBasic.FileIO;

namespace Utility 
{
    public class CSVReader
    {
        public CSVReader() { }

        public List<string[]> Read(string file)
        {
            List<string[]> data = [];

            using (TextFieldParser textFieldParser = new TextFieldParser(file))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");

                while (!textFieldParser.EndOfData)
                {
                    string[] fields = textFieldParser.ReadFields();
                    data.Add(fields ?? []);
                }
            }
            return data;
        }
    }
}