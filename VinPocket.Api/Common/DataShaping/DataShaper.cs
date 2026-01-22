using FluentValidation;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;

namespace VinPocket.Api.Common.DataShaping;

public static class DataShaper
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache = [];

    public static ExpandoObject ShapeData<T>(
        T entity,
        string? fields = null)
    {
        if (!AreAllFieldsValid<T>(fields))
        {
            throw new ValidationException([new("fields", $"Fields value '{fields}' is not valid")]);
        }

        HashSet<string> fieldsSet = fields?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        PropertyInfo[] propertyInfos = GetFilteredProperties<T>(fieldsSet);

        IDictionary<string, object?> shapedObject = new ExpandoObject();

        foreach (var propertyInfo in propertyInfos)
        {
            shapedObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
        }


        return (ExpandoObject)shapedObject;
    }

    public static ExpandoObject ShapeData<T>(
        T entity)
    {
        PropertyInfo[] propertyInfos = GetFilteredProperties<T>([]);

        IDictionary<string, object?> shapedObject = new ExpandoObject();

        foreach (var propertyInfo in propertyInfos)
        {
            shapedObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
        }


        return (ExpandoObject)shapedObject;
    }

    public static IReadOnlyCollection<ExpandoObject> ShapeCollectionData<T>(
        IReadOnlyCollection<T> entities,
        string? fields = null)
    {
        if (!AreAllFieldsValid<T>(fields))
        {
            throw new ValidationException([new("fields", $"Fields value '{fields}' is not valid")]);
        }

        HashSet<string> fieldsSet = fields?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        PropertyInfo[] propertyInfos = GetFilteredProperties<T>(fieldsSet);

        List<ExpandoObject> shapedObjects = new(entities.Count);

        foreach (var entity in entities)
        {
            IDictionary<string, object?> shapedObject = new ExpandoObject();

            foreach (var propertyInfo in propertyInfos)
            {
                shapedObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
            }


            shapedObjects.Add((ExpandoObject)shapedObject);
        }

        return shapedObjects;
    }

    private static bool AreAllFieldsValid<T>(string? fields)
    {
        HashSet<string> fieldsSet = fields?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        string[] propertyNames = [.. PropertiesCache
            .GetOrAdd(
                typeof(T),
                type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            .Select(x => x.Name)];

        return fieldsSet.All(x => propertyNames.Contains(x, StringComparer.OrdinalIgnoreCase));
    }

    private static PropertyInfo[] GetFilteredProperties<T>(HashSet<string> fieldsSet)
    {
        PropertyInfo[] properties = PropertiesCache.GetOrAdd(
            typeof(T),
            type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        return fieldsSet.Count > 0
            ? properties.Where(x => fieldsSet.Contains(x.Name)).ToArray()
            : properties;
    }
}