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
        private const string CollectionName = "slug_to_url_maps";
        private readonly IMongoCollection<UrlMaps> _urlMaps;


        public ShortenerData(ISettings settings)
        {
            _settings = settings;
            DbClient = new Lazy<MongoClient>(GetDbClient, true);
            _urlMaps = GetCollection<UrlMaps>(CollectionName);

        }
        public string GetActualUrlBySlug(string slug)
        {
            var record = _urlMaps
                .Find(x => x.Slug.ToLower() == slug.ToLower());
            
            return record.FirstOrDefault()?.Url;
        }

        public bool GetSlugByUrl(string url, out string slug)
        {
            var record = _urlMaps
                .Find(x => x.Url.ToLower() == url.ToLower());

            slug = record.FirstOrDefault()?.Slug;

            return slug != default;
        }

        public string SaveShortenedUrl(string slug, string url)
        {
            _urlMaps
                .InsertOne(new UrlMaps
                { 
                    Slug = slug, Url = url, CreatedDate = DateTime.UtcNow 
                });

            return slug;
        }

        public bool DoesSlugExist(string slug)
        {
            return _urlMaps
                .CountDocuments(x => x.Slug.ToLower() == slug.ToLower()) > 0;
        }

        public void UpdateLastAccessedDateForSlug(string slug)
        {
            var updateOptions = new UpdateOptions { IsUpsert = false };
            var updateDef = Builders<UrlMaps>.Update
                .Set(nameof(UrlMaps.LastAccessedDate), DateTime.UtcNow);

            _urlMaps.UpdateMany(x => x.Slug.ToLower() == slug.ToLower(), 
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
