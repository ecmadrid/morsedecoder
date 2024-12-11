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

        string morseCode = "--.--.---.......-.---.-.-.-..-.....--..-....-.-----..-";

        // Load Morse letters file and english words file.
        LoadFiles();

        // Get a tree with possible letters from morse code.

        // Root word (null parent).
        Word rootWord = new Word(null);

        // Genetate words tree from Morse code.

        DecodeMorse(morseCode, ref rootWord);
        
        // Generate sentences from words tree.
        List<string> sentences = GenerateText(rootWord);

        // Show sentences in console.
        foreach (var sentence in sentences)
        {
            Console.WriteLine(sentence);
        }

    }

/// <summary>
/// Load Morse letter codes ad english word list.
/// </summary>
    static void LoadFiles()
    {
        string line;

        // Morse codes.

        morseCodes = new List<Tuple<string, string>>();
        StreamReader sr = new StreamReader("files" + Path.DirectorySeparatorChar + "kata.txt");
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

        sr = new StreamReader("files" + Path.DirectorySeparatorChar + "words.txt");
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

/// <summary>
/// Convert a string into Morse code.
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
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


/// <summary>
/// Decode a Morse code string and generate a Word class based instances tree.
/// This is a recursive function.
/// </summary>
/// <param name="morseCode"></param>
/// <param name="parentWord"></param>
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

/// <summary>
/// Generate a sentences list from a Word class based instances tree.
/// </summary>
/// <param name="rootWord"></param>
/// <param name="sentences"></param>
/// <returns></returns>
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

                    // It's an ended word in branch. We start from it to
                    // top parent.
                    words.Add(word.alphaWord);
                        Word parentLetter = word.Parent;
                        while (parentLetter != null)
                        {
                        words.Add(parentLetter.alphaWord);
                            parentLetter = parentLetter.Parent;
                        }

                    // We need to reverse the list.
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