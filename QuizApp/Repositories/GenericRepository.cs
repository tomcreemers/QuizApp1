using SQLite;
using SQLiteNetExtensions.Extensions;
using QuizApp.Models;

namespace QuizApp.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : TableEntity, new()
    {
        private readonly SQLiteConnection _connection;
        public string? StatusMessage { get; set; }

        public GenericRepository()
        {
            // Maak of open de database
            _connection = new SQLiteConnection(AppConstants.DatabasePath, AppConstants.Flags);
            _connection.CreateTable<T>();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void RemoveItem(T entity)
        {
            try
            {
                _connection.Delete(entity);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public List<T> GetItems()
        {
            try
            {
                return _connection.Table<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            return null;
        }

        public T? GetItem(int id)
        {
            try
            {
                return _connection.Table<T>().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            return null;
        }

        public void StoreItem(T entity)
        {
            try
            {
                if (entity.Id != 0)
                {
                    var updated = _connection.Update(entity);
                    StatusMessage = $"{updated} record(s) updated";
                }
                else
                {
                    var inserted = _connection.Insert(entity);
                    StatusMessage = $"{inserted} record(s) inserted";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public void StoreItemWithChildren(T entity, bool recursive = false)
        {
            _connection.InsertWithChildren(entity, recursive);
        }

        public List<T> GetItemsWithChildren()
        {
            try
            {
                return _connection.GetAllWithChildren<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            return null;
        }

        public void RemoveItemWithChildren(T entity)
        {
            try
            {
                _connection.Delete(entity, true);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }
    }
}
