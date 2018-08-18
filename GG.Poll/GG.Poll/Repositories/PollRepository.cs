using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GG.Poll.Repositories
{
    public class PollRepository<T> : IDataRepository<T>
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public IMongoCollection<T> Collection => throw new NotImplementedException();
        PollRepository()
        {
            //_client = new MongoClient(_option.DefaultConnection);
            //_database = _client.GetDatabase(_option.DatabaseName);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return Collection.Find(expression).FirstOrDefault();
        }

        public List<T> List(Expression<Func<T, bool>> expression)
        {
            return Collection.Find(expression).ToList();
        }

        public void Create(T document)
        {
            Collection.InsertOne(document);
        }

        public void CreateMany(List<T> documents)
        {
            Collection.InsertMany(documents);
        }

        public void DeleteOne(Expression<Func<T, bool>> expression)
        {
            Collection.DeleteMany(expression);
        }

        public void Update(T document)
        {
            var update = Builders<T>.Update
                .Set(x => x.SoldType, document.SoldType)
                .Set(x => x.SoldDate, document.SoldDate);
            Collection.UpdateOne(x => x.Id == document.Id, update);
        }
    }
}
