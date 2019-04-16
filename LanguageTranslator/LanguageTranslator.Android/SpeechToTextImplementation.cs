using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using LanguageTranslator.Droid;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextImplementation))]
namespace LanguageTranslator.Droid
{
    public class SpeechToTextImplementation : IVoiceRecognition
    {
        private readonly int VOICE = 10;
        private Activity _activity;

        public SpeechToTextImplementation()
        {
            // Reference to the current activity (from MainApplication)
            _activity = CrossCurrentActivity.Current.Activity;
        }

        public void startSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;

            // First, check if the microphone on the device is working
            if (rec == "android.hardware.microphone")
            {
                try
                {
                    // Call Recognizerintent
                    var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                    // Prompts the user to speak now
                    voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");
                    
                    /* 
                     * If the user does not speak for the specified amount of time, RecogniserIntent will assume the user has 
                     * finished speaking i.e. "submitted" their input
                     */ 
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);

                    // Minimum amount of the speech input length
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);

                    // Defines how many inputs the user can enter/speak
                    voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    _activity.StartActivityForResult(voiceIntent, VOICE);
                }
                catch (ActivityNotFoundException activityNotFoundException)
                {
                    String appPackageName = "com.google.android.googlequicksearchbox";

                    try
                    {
                        Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("market://details?id=" + appPackageName));
                        _activity.StartActivityForResult(intent, VOICE);

                    }
                    catch (ActivityNotFoundException e)
                    {
                        Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName));
                        _activity.StartActivityForResult(intent, VOICE);
                    }
                }
            }
            else
            {
                throw new Exception("No mic found");
            }
        }

        public void stopSpeechToText()
        {

        }
    }
}