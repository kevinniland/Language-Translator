using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageTranslator.Models
{
    class TranslationObject
    {
        public ObjectId toID { get; set; }
        public string toUserInput { get; set; }
        public string toSrcLang { get; set; }
        public int toChosenLang { get; set; }
        public string toTranslation { get; set; }
    }
}
