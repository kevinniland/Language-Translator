#region Imports
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
#endregion 

namespace LanguageTranslator
{
    public partial class MainPage : ContentPage
    {
        #region Lists
        private List<string> LanguagesList;
        #endregion

        #region Main Page function
        public MainPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Request function
        private IRestResponse Request(string url)
        {
            // Create new RestSharp client
            var client = new RestClient()
            {
                BaseUrl = new Uri(url)
            };

            // Create new RestSharp request
            var request = new RestRequest()
            {
                Method = Method.GET
            };

            return client.Execute(request);
        }
        #endregion

        #region private methods - provides the main functionality of the app
        private void loadLanguages()
        {
            #region Load all available languages into the picker 'pckLangugages'
            // Fills the picker 'pckLanguages' with all available langauges when the main page is loaded
            var serverResponse = Request(string.Format(ApiSetup.getLanguages, ApiSetup.APIKey, lblSourceLanguage.Text));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format 

            foreach (DictionaryEntry dictionaryEntry in dictionary)
            {
                if (dictionaryEntry.Key.Equals("langs"))
                {
                    var languages = (JObject)dictionaryEntry.Value;
                    LanguagesList = new List<string>();

                    pckLanguages.Items.Clear();

                    foreach (var lang in languages)
                    {
                        if (!lang.Equals(lblSourceLanguage.Text))
                        {
                            pckLanguages.Items.Add(lang.Value.ToString());
                            LanguagesList.Add(lang.Key);
                        }
                    }
                }
            }
            #endregion
        }

        private void EntText_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadLanguages();

            var serverResponse = Request(string.Format(ApiSetup.detectSourceLanguage, ApiSetup.APIKey, entText.Text));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format 
            var statusCode = dictionary["code"].ToString();

            if (statusCode.Equals("200"))
            {
                lblSourceLanguage.Text = dictionary["lang"].ToString();
            }
        }

        private void BtnTranslate_Clicked(object sender, EventArgs e)
        {
            var serverResponse = Request(string.Format(ApiSetup.translateLanguage, ApiSetup.APIKey, entText.Text, LanguagesList[pckLanguages.SelectedIndex]));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format
            var statusCode = dictionary["code"].ToString();

            if (statusCode.Equals("200"))
            {
                entTranslation.Placeholder = string.Join("", dictionary["text"]);
            }
        }
    }
    #endregion
}
