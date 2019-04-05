using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LanguageTranslator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TranslatorPage : ContentPage
	{
        #region Interfaces
        private IVoiceRecognition _iVoiceRecognition;
        #endregion

        #region Lists
        private List<string> LanguagesList;
        #endregion

        public TranslatorPage()
        {
            InitializeComponent();
        }

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
            #region Load Languages
            // Load all available languages into the picker 'pckLangugages'
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

            #region Detect Source Language
            // Take in the entered text and pass it into the 'detectSourceLanguage' function - the language the user is writing in will be returned
            var serverResponse = Request(string.Format(ApiSetup.detectSourceLanguage, ApiSetup.APIKey, entText.Text));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format 
            var statusCode = dictionary["code"].ToString();

            if (statusCode.Equals("200"))
            {
                lblSourceLanguage.Text = dictionary["lang"].ToString();
            }
            #endregion
        }

        private void BtnTranslate_Clicked(object sender, EventArgs e)
        {
            #region Translation
            /*
             * Take in the entered text and pass it into the 'translateLanguage' function. The language the user has chosen will also be
             * retrieved using SelectedIndex - the translation will then be returned and printed in the 'entTranslation' entry box
             */
            var serverResponse = Request(string.Format(ApiSetup.translateLanguage, ApiSetup.APIKey, entText.Text, LanguagesList[pckLanguages.SelectedIndex]));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format
            var statusCode = dictionary["code"].ToString();

            if (statusCode.Equals("200"))
            {
                entTranslation.Placeholder = string.Join("", dictionary["text"]);
            }
            #endregion
        }

        private async void BtnReadFile_Clicked(object sender, EventArgs e)
        {
            #region Read in a file from anywhere
            string fileText;

            // Allows the user to choose a file from any location
            FileData fileData = await CrossFilePicker.Current.PickFile();

            if (fileData != null)
            {
                lblFileRead.Text = fileData.FileName; // Displays the name of the file
                fileText = System.Text.Encoding.UTF8.GetString(fileData.DataArray);

                entText.Text = fileText;
            }
            else
            {
                return;
            }
            #endregion
        }

        private void SpeechToText(string args)
        {
            entText.Text = args;
        }

        private void BtnRecordVoice_Clicked(object sender, EventArgs e)
        {
            try
            {
                _iVoiceRecognition.startSpeechToText();
            }
            catch (Exception exception)
            {
                entText.Text = exception.Message;
            }
        }
    }
    #endregion
}
