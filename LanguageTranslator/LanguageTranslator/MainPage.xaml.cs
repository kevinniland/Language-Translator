#region Imports
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Plugin.FilePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Plugin.FilePicker.Abstractions;
using Plugin.TextToSpeech;
#endregion

namespace LanguageTranslator
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.CurrentPage = this.Children[1];
        }
    }
}

