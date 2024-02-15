using System;
using System.Collections.Generic;
namespace yourAlias
{
    public class CollectionEntity
    {
        public CollectionEntity() { }
        public CollectionEntity(string name, List<WordData> wordList)
        {
            this.name = name;
            this.wordList = wordList.ToArray();
        }

        public string name;
        public WordData[] wordList;
    }

    [Serializable]
    public struct WordData
    {
        public WordData(string name, string discription)
        {
            this.WordName = name;
            this.WordDiscription = discription;
        }

        public string WordName;
        public string WordDiscription;

    }



}