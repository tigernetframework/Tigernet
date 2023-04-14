## PaginationExtension

The `PaginationExtension` class provides extension methods for paginating `IEnumerable<T>` and `IQueryable<T>` objects.

### Methods

#### Paginate<T>(IEnumerable<T> source, PaginationOptions options)

Paginates an `IEnumerable<T>` object.

##### Parameters

- `source`: The `IEnumerable<T>` object to paginate.
- `options`: The pagination options.

##### Returns

An `IEnumerable<T>` object that contains the items for the current page.

#### Paginate<T>(IQueryable<T> source, PaginationOptions options)

Paginates an `IQueryable<T>` object.

##### Parameters

- `source`: The `IQueryable<T>` object to paginate.
- `options`: The pagination options.

##### Returns

An `IQueryable<T>` object that contains the items for the current page.
