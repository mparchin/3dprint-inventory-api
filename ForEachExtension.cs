namespace _3dprint_inventory_api;

public static class ForEachExtension
{
    public static void ForEach<T>(this List<T> list, Func<T, object> func) =>
        list.ForEach(t => func(t));
}