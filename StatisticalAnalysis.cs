using System.Text;
using System.Text.RegularExpressions;


namespace Task_4th_ParsingTextFile
{
    public class StatisticalAnalysis
    {
        const string SORTED_WORDS_FILE = "SortedWords.txt";

        const string STATISTIC_DATA_FILE = "StatisticData.txt";

        public static void GetAndSaveStaticData(string text, List<string> sentences, List<string> words)
        {
            string longestSentences = SearchLongestSentenceByCharacters(sentences);

            string shortestSentences = SearchShortesSentenceByWords(sentences);

            var (letter, count) = FindMostFrequencyLetter(text);

            SaveSortedWordsStatistic(words);

            using var sw = new StreamWriter(STATISTIC_DATA_FILE, false, Encoding.UTF8) { };
            sw.WriteLine($"***** The longest sentence for the number of characters:");
            sw.WriteLine($"\t{longestSentences}\n");
            sw.WriteLine($"***** The shortest sentence for the number of words:");
            sw.WriteLine($"\t{shortestSentences}\n");
            sw.WriteLine($"***** The most common letter in the text:");
            sw.WriteLine($"\t{letter} - {count}");
        }

        private static string SearchLongestSentenceByCharacters(List<string> sentences)
        {
            _ = sentences ?? throw new ArgumentNullException(nameof(sentences), "The sentence list is null");

            var sortedSentences = sentences.OrderByDescending(s => s.Length);

            var longestSentence = sortedSentences.First();

            return longestSentence;
        }

        private static string SearchShortesSentenceByWords(List<string> sentences)
        {
            _ = sentences ?? throw new ArgumentNullException(nameof(sentences), "The sentence list is null");

            var sortedSentences = sentences.OrderBy(s => s.Count(c => c == ' '));

            var longestSentence = sortedSentences.First();

            return longestSentence;
        }

        private static (char letter, int count) FindMostFrequencyLetter(string text)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text), "The text is null");

            string correctedText = new(text);

            correctedText = Regex.Replace(text, @"[^\w]|\d+|_", "");

            correctedText = correctedText.ToLower();

            var counts = correctedText.GroupBy(c => c).OrderByDescending(c => c.Count());

            (char character, int count) letter = (counts.First().Key, counts.First().Count());

            return letter;
        }

        private static void SaveSortedWordsStatistic(List<string> words)
        {
            _ = words ?? throw new ArgumentNullException(nameof(words), "The words list is null");

            var correctedWords = new List<string>();

            foreach (var word in words)
            {
                correctedWords.Add(word.ToLower());
            }

            var sortedWords = correctedWords.GroupBy(x => x).OrderBy(t => t.Key);

            using var sw = new StreamWriter(SORTED_WORDS_FILE, false, Encoding.UTF8);

            sw.WriteLine($"Words used in the text with an indicator of their frequency{Environment.NewLine}");

            foreach (var word in sortedWords)
            {
                sw.WriteLine($"{word.Key}- {word.Count()}");
            }
        }
    }
}
