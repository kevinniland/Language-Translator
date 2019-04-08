using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LanguageTranslator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class History : ContentPage
	{
		public History ()
		{
			InitializeComponent ();
		}

        public void ReadDatabase()
        {
            string connectionString = "mongodb://kevin_niland:test123@ds133256.mlab.com:33256/mobile_apps_dev";
            MongoClient client = new MongoClient(connectionString);

            var database = client.GetDatabase("mobile_apps_dev");
            var collection = database.GetCollection<BsonDocument>("translations_history");

            //entData.Text = collection.Find(new BsonDocument()).ForEachAsync(document);
        }
	}
}