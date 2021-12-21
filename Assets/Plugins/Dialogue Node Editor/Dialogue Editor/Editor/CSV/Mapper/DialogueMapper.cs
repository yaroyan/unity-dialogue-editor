using CsvHelper.Configuration.Attributes;

namespace Dialogue.Editor.Mapper
{
    public class Dialogue
    {
        [Name("Dialogue Name")]
        public string dialogueName;
        [Name("Node GUID")]
        public string nodeGUID;
        [Name("Text GUID")]
        public string textGUID;
        [Name("Japanese")]
        public string japanese;
        [Name("English")]
        public string english;
    }
}
