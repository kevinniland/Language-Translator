using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LanguageTranslator
{
    class AppCache
    {
        // Read only properties - No need for 'set'
        public string APIKey { get; } = @"trnsl.1.1.20190314T153953Z.82626c9c38180ce0.c966002352eefc59e595db2a168f997d19814cbc";

        /* To check if this link is valid and the API key is valid, replace '{0}' with the API key and '{1}' with your source language e.g. 'en'
         * All available languages - and the shorthand for each - will be returned in JSON format
         * Here, the service used is 'getLangs'
         */
        public string getLanguages { get; } = @"https://translate.yandex.net/api/v1.5/tr.json/getLangs?key={0}&ui={1}";

        /* To check if this link is valid and the API key is valid, replace '{0}' with the API key and 'ui={1}' with text equals a word e.g text=Hello
         * A status code plus the shorthand of the detected language will be returned. In this example, ' "code": 200, "lang": "en" ' will be returned
         * Here, the service used here is 'detect'
         */ 
        public string detectSourceLanguage { get; } = @"https://translate.yandex.net/api/v1.5/tr.json/detect?key={0}&ui={1}";
    }

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void EntText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnNAME_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnTranslate_Clicked(object sender, EventArgs e)
        {

        }
    }
}
