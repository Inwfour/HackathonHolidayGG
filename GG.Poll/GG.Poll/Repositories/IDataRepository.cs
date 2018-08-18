using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GG.Poll.Repositories
{
    public interface IDataRepository<T>
    {
        IMongoCollection<T> Collection { get; }

        T Get(Expression<Func<T, bool>> expression);
        List<T> List(Expression<Func<T, bool>> expression);
        void Create(T document);
        void CreateMany(List<T> documents);
        void DeleteOne(Expression<Func<T, bool>> expression);
        void UpdateOne(Expression<Func<T, bool>> expression, T document);
    }
}
