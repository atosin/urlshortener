using MongoDB.Bson;
using MongoDB.Driver;
using server.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace server.Data
{
    public class ShortenerData : IShortenerData
    {

        private readonly ISettings _settings;
        private readonly Lazy<MongoClient> DbClient;
        private const string CollectionName = "hash_to_url_maps";
        private readonly IMongoCollection<UrlMaps> _urlMaps;


        public ShortenerData(ISettings settings)
        {
            _settings = settings;
            DbClient = new Lazy<MongoClient>(GetDbClient, true);
            _urlMaps = GetCollection<UrlMaps>(CollectionName);

        }
        public string GetActualUrlByHash(string hash)
        {
            var record = _urlMaps
                .Find(x => x.Hash.ToLower() == hash.ToLower());
            
            return record.FirstOrDefault()?.Url;
        }

        public bool GetHashByUrl(string url, out string hash)
        {
            var record = _urlMaps
                .Find(x => x.Url.ToLower() == url.ToLower());

            hash = record.FirstOrDefault()?.Hash;

            return hash != default;
        }

        public string SaveShortenedUrl(string hash, string url)
        {
            _urlMaps
                .InsertOne(new UrlMaps
                { 
                    Hash = hash, Url = url, CreatedDate = DateTime.UtcNow 
                });

            return hash;
        }

        public bool DoesHashExist(string hash)
        {
            return _urlMaps
                .CountDocuments(x => x.Hash.ToLower() == hash.ToLower()) > 0;
        }

        public void UpdateLastAccessedDateForHash(string hash)
        {
            var updateOptions = new UpdateOptions { IsUpsert = false };
            var updateDef = Builders<UrlMaps>.Update
                .Set(nameof(UrlMaps.LastAccessedDate), DateTime.UtcNow);

            _urlMaps.UpdateMany(x => x.Hash.ToLower() == hash.ToLower(), 
                    updateDef, updateOptions);
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var database = GetDatabase();
            var collectionSettings = new MongoCollectionSettings
            {
                GuidRepresentation = GuidRepresentation.Standard
            };

            return database.GetCollection<T>(collectionName, collectionSettings);
        }

        private IMongoDatabase GetDatabase()
        {
            return DbClient.Value.GetDatabase(_settings.DbName);
        }

        private MongoClient GetDbClient()
        {
            var connectionUrl = new MongoUrl(_settings.DbServer);
            var settings = MongoClientSettings.FromUrl(connectionUrl);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12,
            };

            return new MongoClient(settings);
        }

    }
}
