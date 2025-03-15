using System;
using System.Collections.Generic;

namespace QuizApp.Repositories
{
    public interface IGenericRepository<T> : IDisposable
    {
        string? StatusMessage { get; set; }
        void RemoveItem(T entity);
        List<T> GetItems();
        T? GetItem(int id);
        void StoreItem(T entity);
        void StoreItemWithChildren(T entity, bool recursive = false);
        List<T> GetItemsWithChildren();
        void RemoveItemWithChildren(T entity);
    }
}
