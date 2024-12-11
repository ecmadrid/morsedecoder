using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morsedecoder
{
    internal class Word
    {
        private Word? parent;
        private List<Word>? wordList;
        private string word;
        public Word? Parent
        {
            get { return parent; }
            set
            {
                parent = value;
            }
        }

        public List<Word>? Words
        {
            get { return wordList; }
            set { wordList = value; }
        }

        public string alphaWord
        {
            get { return word; }
            set { word = value; }
        }

        public Word(Word parentLetter)
        {
            parent = parentLetter;
            wordList = new List<Word>();
        }

        ~Word()
        {
            wordList.Clear();
        }

    }
}
