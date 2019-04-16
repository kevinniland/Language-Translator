using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Xamarin.Forms;
using Android.Speech;

namespace LanguageTranslator.Droid
{
    [Activity(Label = "LanguageTranslator", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IMessageSender 
    {
        #region Variables
        private readonly int VOICE = 10;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        /* 
         * Intent is returened in this callback - in order to access this method, must override it with our activity
         * which implements FormsAppCompatActivity
         */
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                    /*
                     * We are passing the result back via MessagingSender but since it requires a Sender argument, we just create a blank
                     * interface called IMessageSender. We can then start receiving messages like:
                     * 
                     *      MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) => {
                     * 
                     *      });
                     */
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0];
                        MessagingCenter.Send<IMessageSender, string>(this, "STT", textInput);
                    }
                    else
                    {
                        MessagingCenter.Send<IMessageSender, string>(this, "STT", "No input");
                    }

                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}