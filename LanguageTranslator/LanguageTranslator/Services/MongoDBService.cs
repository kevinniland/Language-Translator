using LanguageTranslator.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace LanguageTranslator.Services
{
    /* 
     * This service creates and gets data from a MongoDB collection. Help with the MLabDataCollection function was gotten from:
     * https://stackoverflow.com/questions/43803451/azure-documentdb-with-mongodb-net-driver-how-to-set-id-manually
     */ 
    class MongoDBService
    {
        public List<MLabData> mLabData;
        public string database = "mobile_apps_dev";
        public string collection = "translations_history";

        public IMongoCollection<MLabData> _mLabDataCollection;

        public IMongoCollection<MLabData> MLabDataCollection
        {
            get
            {
                if (_mLabDataCollection == null)
                {
                    MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl
                        ("mongodb://<kevin_niland>:test123@ds133256.mlab.com:33256/mobile_apps_dev"));

                    mongoClientSettings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

                    var mongoClient = new MongoClient(mongoClientSettings);
                    var mongoDatabase = mongoClient.GetDatabase(database);

                    var mongoCollectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    _mLabDataCollection = mongoDatabase.GetCollection<MLabData>(collection, mongoCollectionSettings);
                }

                return _mLabDataCollection;
            }
        }

        public async void saveTranslations(MLabData m)
        {
            await MLabDataCollection.InsertOneAsync(m);
        }

        public List<MLabData> getTranslations()
        {
            mLabData = new List<MLabData>();
            var allTranslations = MLabDataCollection.Find(new BsonDocument()).ToList();

            foreach (var translation in allTranslations)
            {
                mLabData.Add(new MLabData(translation.userInput, translation.srcLang, translation.chosenLang, translation.translation));
            }

            return mLabData;
        }
    }
}
