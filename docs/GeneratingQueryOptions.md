# Generating query options can be made easily by utilizing extension methods that provide a range of features for querying. These methods include: `1.2`

- CreateQuery: creates a new query object for the specified entity type.
- AddFilter: filters data based on specific criteria, such as location or ID.
- AddSearch: searches for specific data within a data source using a keyword.
- AddInclude: includes related entities when querying data.
- AddSort: sorts data in ascending or descending order based on a specified property.
- AddPagination: retrieves data in chunks or pages, making it easier to work with large data sources.

These extension methods simplify the querying process and make selecting properties much easier with the KeySelector parameter.

# For instance, creating a complex query can be done with the following code:
```text
var complexQuery = Company.CreateQuery()
   .AddSearch(keyword, true)
   .AddFilter(x => x.Location, currentLocation)
   .AddFilter(x => x.ParentId, parentId)
   .AddInclude(x => x.Employees)
   .AddSort(x => x.EstablishedDate, false)
   .AddPagination(10, 1);
```
By utilizing these extension methods, developers can streamline their code, save time, and improve the performance and scalability of their applications.
