using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        #region Variables
        bool isRunning;
        string userInput, srcLang, translation;
        int chosenLang;
        #endregion
        
        #region Interfaces
        private IVoiceRecognition _iVoiceRecognition;
        #endregion

        #region Lists
        private List<string> LanguagesList;
        #endregion

        // readonly MongoDatabase mongoDatabase;

        public TranslatorPage()
        {
            InitializeComponent();
            isRunning = true;

            try
            {
                _iVoiceRecognition = DependencyService.Get<IVoiceRecognition>();
            }
            catch (Exception ex)
            {
                lblRecText.Text = ex.Message;
            }


            MessagingCenter.Subscribe<IVoiceRecognition, string>(this, "STT", (sender, args) =>
            {
                SpeechToText(args);
            });

            MessagingCenter.Subscribe<IVoiceRecognition>(this, "Final", (sender) =>
            {
                btnRecordVoice.IsEnabled = true;
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToText(args);
            });
        }

        #region RestSharp
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

        #region public methods - provides the main functionality of the app -> Methods are public to allow testing in LanguageTranslatorTest
        public void LoadLanguages()
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

        public void EntText_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadLanguages();

            #region Detect Source Language
            // Take in the entered text and pass it into the 'detectSourceLanguage' function - the language the user is writing in will be returned
            var serverResponse = Request(string.Format(ApiSetup.detectSourceLanguage, ApiSetup.APIKey, entText.Text));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format 
            var statusCode = dictionary["code"].ToString();

            string connectionString = "mongodb://kevin_niland:test123@ds133256.mlab.com:33256/mobile_apps_dev";
            MongoClient client = new MongoClient(connectionString);

            var database = client.GetDatabase("mobile_apps_dev");
            var collection = database.GetCollection<BsonDocument>("translations_history");

            var userInput = entText.Text;

            if (statusCode.Equals("200"))
            {
                lblSourceLanguage.Text = dictionary["lang"].ToString();
                var srcLang = lblSourceLanguage.Text;

                var document = new BsonDocument
                {
                    { "User Input", userInput },
                    { "Source Language", srcLang }
                };

                collection.InsertOneAsync(document);
            }
            #endregion
        }

        public void BtnTranslate_Clicked(object sender, EventArgs e)
        {
            #region Translation
            /*
             * Take in the entered text and pass it into the 'translateLanguage' function. The language the user has chosen will also be
             * retrieved using SelectedIndex - the translation will then be returned and printed in the 'entTranslation' entry box
             */
            var serverResponse = Request(string.Format(ApiSetup.translateLanguage, ApiSetup.APIKey, entText.Text, LanguagesList[pckLanguages.SelectedIndex]));
            var dictionary = JsonConvert.DeserializeObject<IDictionary>(serverResponse.Content); // Converts the server response into JSON format
            var statusCode = dictionary["code"].ToString();

            string connectionString = "mongodb://kevin_niland:test123@ds133256.mlab.com:33256/mobile_apps_dev";
            MongoClient client = new MongoClient(connectionString);

            var database = client.GetDatabase("mobile_apps_dev");
            var collection = database.GetCollection<BsonDocument>("translations_history");

            chosenLang = pckLanguages.SelectedIndex;

            if (statusCode.Equals("200"))
            {
                entTranslation.Placeholder = string.Join("", dictionary["text"]);
                translation = entTranslation.Placeholder;

                var document = new BsonDocument
                {
                    { "Chosen Language", chosenLang },
                    { "Translation", translation }
                };

                collection.InsertOneAsync(document);
            }
            #endregion
        }

        #region MongoDB/MLab
        public void SaveToDatabase(string userInput, string srcLang, int chosenLang, string translation)
        {
            string connectionString = "mongodb://kevin_niland:test123@ds133256.mlab.com:33256/mobile_apps_dev";
            MongoClient client = new MongoClient(connectionString);

            var database = client.GetDatabase("mobile_apps_dev");
            var collection = database.GetCollection<BsonDocument>("translations_history");

            while (isRunning)
            {
                var document = new BsonDocument
                {
                    { "User Input", userInput },
                    { "Source Language", srcLang },
                    { "Chosen Language", chosenLang },
                    { "Translation", translation }
                };

                collection.InsertOneAsync(document);
            }
        }
        #endregion

        public async void BtnReadFile_Clicked(object sender, EventArgs e)
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

        public void SpeechToText(string args)
        {
            entText.Text = args;
        }

        public void BtnRecordVoice_Clicked(object sender, EventArgs e)
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

        public void SpeechToTextFinalResultRecieved(string args)
        {
            lblRecText.Text = args;
        }

        public void Start_Clicked(object sender, EventArgs e)
        {
            try
            {
                _iVoiceRecognition.startSpeechToText();
            }
            catch (Exception ex)
            {
                lblRecText.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                btnRecordVoice.IsEnabled = false;
            }



        }
    }
    #endregion
}
