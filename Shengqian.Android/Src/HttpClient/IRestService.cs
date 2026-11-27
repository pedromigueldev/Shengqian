namespace Shengqian.Android.Http;
public interface IRestService<T>
{
    Task<List<T>> RefreshDataAsync();
    Task SaveTodoItemAsync(T item, bool isNewItem);
    Task DeleteTodoItemAsync(string id);
}