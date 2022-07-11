using System.Text;

namespace Task_4th_ParsingTextFile
{
    public static class Program
    {
        const string FILE_NAME = "TextFile.txt";

        static void Main(string[] args)
        {
            var path = Path.Combine(Environment.CurrentDirectory, FILE_NAME);

            string textForParsing;

            using (var sw = new StreamReader(path, Encoding.UTF8))
            {
                textForParsing = sw.ReadToEnd();                
            }

            ParsingService parsingService = new(textForParsing);

            StatisticalAnalysis.GetAndSaveStaticData(parsingService.OriginalText, parsingService.ParsedSentences, parsingService.ParsedWords);
        }
    }
}