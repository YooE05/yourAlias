using System.Collections.Generic;
namespace yourAlias
{
    public class CollectionEntity
    {
        public CollectionEntity() { }
        public CollectionEntity(string name, List<string> wordList)
        {
            this.name = name;
            this.wordList = wordList.ToArray();
        }

        public string name;
        public string[] wordList;
    }
}