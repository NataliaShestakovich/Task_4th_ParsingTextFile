using System.Text.RegularExpressions;

namespace Task_4th_ParsingTextFile
{
   public class ParsingService
   {
        public ParsingService(string text)
        {
            OriginalText = text ?? throw new ArgumentNullException(nameof(text), "The text is null");

            ParsedWords = ParseByWords();

            ParsedSentences = ParseBySentences();

            ParsedPartsByPunctuation = ParseByPunctuation();
        }
        public string OriginalText { get; }

        public List<string> ParsedWords { get; }

        public List<string> ParsedSentences { get; }

        public List<string> ParsedPartsByPunctuation { get; }

        private List<string> ParseByWords()
        {
            if (OriginalText == null)
            {
                throw new ArgumentNullException(nameof(OriginalText), "The text is null");
            }

            var patternForParsing = new Regex(@"(\w*[^\W\d]'[^\W\d]\w*)|(\w*[^\W\d])"); 
            
            MatchCollection matches = patternForParsing.Matches(OriginalText);

            var parsingResult = new List<string>();

            foreach (var matche in matches)
            {
                bool IsDigitSimbol = Regex.IsMatch(matche.ToString(), @"[\d\s]+");

                if (matche != null && !IsDigitSimbol)
                {
                    string word = matche.ToString();

                    word = word.Trim(' ','_');

                    parsingResult.Add(word);
                }               
            }
            parsingResult.RemoveAll(s => s == "");

            return parsingResult;
        }

        private List<string> ParseBySentences() 
        {
            if (OriginalText == null)
            {
                throw new ArgumentNullException(nameof(OriginalText), "The text is null");
            }

            var correctedText = Regex.Replace(OriginalText, @"\n\n\s+C.A.B.\n\s+M.R.B.[\w\W]+618-619", " ");

            correctedText = Regex.Replace(correctedText, @"INDEX\n+Abolition, 318, 331[\w\W]+Proofreading Team at http:\/\/www.pgdp.net\n+", " ");

            correctedText = Regex.Replace(correctedText, @"(.References[\w\W\s\n\t]+?=Questions=\n*)|(=Research Topics=[\w\W\s\n\t]+?C[Hh][Aa][Pp][Tt][Ee][Rr].*)", " ");

            correctedText = Regex.Replace(correctedText, @"POPULATION OF THE UNITED STATES, BY STATES[\w\W\s\n\t]+?A TOPICAL SYLLABUS", " ");

            correctedText = Regex.Replace(correctedText, @"[\*=_]+", " ");

            correctedText = Regex.Replace(correctedText, @"^\[Illustration[\p{P}\w\W-[\]]]+\]", " "); 

            correctedText = Regex.Replace(correctedText, @"(?<=[a-z;:\-\s\.\d|])\n+(?=[a-zA-Z])", " "); 

            correctedText = Regex.Replace(correctedText, @"['""] I ", " i# "); 

            string patternForParsing = @"(?<![M]..)(?<![A-Z].)[.!?]+[\s""'_\-=]+(?![a-z\s\d])(?!_[a-z]\.)(?![A-Z][.])(?!([Ff]ig))(?!\n\()";

            string[] result = Regex.Split(correctedText, patternForParsing);

            var parsingResult = new List<string>();

            foreach (var item in result)
            {
                string sentence = Regex.Replace(item, @"[\n\t]", " ");
                sentence = Regex.Replace(sentence, @"\p{Z}+", " ");
                sentence += ".";
                sentence = Regex.Replace(sentence, " i# ", @" I ");
                
                parsingResult.Add(sentence);
            }

            return parsingResult;
        }

        private List<string> ParseByPunctuation ()
        {
            if (OriginalText == null)
            {
                throw new ArgumentNullException(nameof(OriginalText), "The text is null");
            }

            var correctedText = Regex.Replace(OriginalText, @"(\s+)", " ");

            string[] parsingResult = Regex.Split(correctedText, @"\p{P}+\s*");

            return parsingResult.ToList();
        }
   }
}
