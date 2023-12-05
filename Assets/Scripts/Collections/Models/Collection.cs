using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace yourAlias
{
    public class Collection
    {
        public Collection() { }

        public Collection(string name, List<string> wordList)
        {
            this.name = name;
            this.wordList = wordList;
        }

        private string name;
        private List<string> wordList;

        public string Name { get => this.name; set => this.name = value; }
        public List<string> WordList { get => this.wordList;}

        public void UpdateCollection(Collection collectionSample)
        {
            this.name = collectionSample.Name;
            this.wordList = collectionSample.WordList;
        }

        public void AddWord(string newWord)
        {
            this.wordList.Add(newWord);
        }

        public void RemoveWord(string removedWord)
        {
            this.wordList.Remove(removedWord);
        }


    }
}