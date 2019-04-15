using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageTranslator.Models
{
    class MLabData
    {
        public ObjectId objectId { get; set; }
        public string userInput { get; set; }
        public string srcLang { get; set; }
        public int chosenLang { get; set; }
        public string translation { get; set; }

        public MLabData(string userInput, string srcLang, int chosenLang, string translation)
        {
            this.userInput = userInput;
            this.srcLang = srcLang;
            this.chosenLang = chosenLang;
            this.translation = translation;
        }
    }
}
