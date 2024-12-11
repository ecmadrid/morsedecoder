using morsedecoder;
using System.ComponentModel;
using System.Linq;
using System.Xml;

class Program
{

    static List<Tuple<string, string>>? morseCodes;
    static private List<Tuple<string, string>>? wordlist;
    static void Main(string[] args)
    {
        // Display the number of command line arguments.
        //Console.WriteLine(args.Length);

        //string morseCode = "--.--.---.......-.---.-.-.-..-.....--..-....-.-----..-";
        //string morseCode = "-..";
        string morseCode = "--.--.---.......-.---.-.-.-..-.....--..-....-.-----..-";

    
        LoadFiles();

        // Get a tree with possible letters from morse code.

        Word rootWord = new Word(null);
        DecodeMorse(morseCode, ref rootWord);
        List<string> sentences = GenerateText(rootWord);

        foreach (var sentence in sentences)
        {
            Console.WriteLine(sentence);
        }

    }

    static void LoadFiles()
    {
        string line;

        // Morse codes.

        morseCodes = new List<Tuple<string, string>>();
        StreamReader sr = new StreamReader("files\\kata.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            // Add morse code.
            string[] data = line.Split(':');
            morseCodes.Add(new Tuple<string, string>(data[1].Trim(), data[0].Trim().ToUpper()));

            //Read the next line
            line = sr.ReadLine();
        }
        //close the file
        sr.Close();

        //Word list

        wordlist = new List<Tuple<string, string>>();

        sr = new StreamReader("files\\words.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            wordlist.Add(new Tuple<string, string>(line.ToUpper().Trim(), CodeToMorse(line.ToUpper().Trim())));

            //Read the next line
            line = sr.ReadLine();
        }

        //close the file
        sr.Close();
    }

    static string CodeToMorse(string value)
    {
        string result = "";

        foreach (char c in value)
        {
            foreach (var code in morseCodes)
            {
                if (code.Item2[0] == c)
                {
                    result += code.Item1;
                    break;
                }
            }
            }

        return result;
    }
        static void DecodeMorse(string morseCode, ref Word parentWord)
     {

        foreach (var morseWord in wordlist)
        {
            if (morseCode.StartsWith(morseWord.Item2))
            {
                Word word = new Word(parentWord);
                word.alphaWord = morseWord.Item1;
                parentWord.Words.Add(word);                
                if (morseCode.Length > morseWord.Item2.Length)
                {
                    string subMorseCode = morseCode.Substring(morseWord.Item2.Length);
      
                     DecodeMorse(subMorseCode, ref word);
                }
            
            }
        }

        return;
    }

    static List<string> GenerateText(Word rootWord, List<string> sentences = null)
    {
        if (sentences == null)
        {
            sentences = new List<string>();
        }
        foreach (var word in rootWord.Words)
            {
            List<string> words = new List<string>();
                string alphaMessage = "";

                if (word.Parent != null)
                {
                    if (word.Words.Count > 0)
                    {
                        GenerateText(word, sentences);
                    }
                    else
                    {

                    words.Add(word.alphaWord);
                        Word parentLetter = word.Parent;
                        while (parentLetter != null)
                        {
                        words.Add(parentLetter.alphaWord);
                            parentLetter = parentLetter.Parent;
                        }

                    words.Reverse();
                    alphaMessage = String.Join(' ', words);
                    sentences.Add(alphaMessage);

                    alphaMessage = "";
                    words.Clear();
                    }
                }
        }

        return sentences;
    }
}