using System.Collections.Generic;

namespace yourAlias
{
    public class Collection
    {
        public Collection() { }

        public Collection(string name, List<WordData> wordList)
        {
            this.name = name;
            this.wordList = wordList;
        }

        private string name;
       // private Dictionary<string, string> wordAndDiscriptionDictionary;
        private List<WordData> wordList;
        // private List<string> wordList;

        public string Name { get => this.name; set => this.name = value; }
         public List<WordData> WordList { get => this.wordList;}
        //public Dictionary<string, string> WordDictionary { get => this.wordAndDiscriptionDictionary; }

        public void UpdateCollection(Collection collectionSample)
        {
            this.name = collectionSample.Name;
            //this.wordAndDiscriptionDictionary = collectionSample.wordAndDiscriptionDictionary;

            this.wordList = collectionSample.WordList;
        }

        public void AddWord(string newWord, string wordDiscription)
        {
            //this.wordAndDiscriptionDictionary.Add(newWord, wordDiscription);
            this.wordList.Add(new WordData(newWord,wordDiscription));
        }

        public void RemoveWord(string removedWord, string wordDiscription)
        {
           // WordDictionary.Remove(removedWord);

            this.wordList.Remove(wordList.Find(wD=>wD.WordName==removedWord&& wD.WordDiscription == wordDiscription));
        }


    }
}