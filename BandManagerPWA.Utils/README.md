# BandManagerPWA.Utils

## Table of Contents
- [BandManagerPWA.Utils](#bandmanagerpwautils)
  - [Table of Contents](#table-of-contents)
  - [PagedList](#pagedlist)
	- [Overview](#overview)
	- [Properties](#properties)
	- [Usage](#usage)
	  - [Exposing Pagination Metadata with X-Pagination Header](#exposing-pagination-metadata-with-x-pagination-header)
	  - [Reading X-Pagination Header in Client](#reading-x-pagination-header-in-client)
	- [Async Methods](#async-methods)
---
## Overview
A collection of utility classes used in the BandManagerPWA project.

---


## Utilities
### PagedList

#### Overview
`PagedList` is a utility class that allows you to easily paginate `IQueryable` collections and retrieve metadata about the pagination status.


#### Properties

- `CurrentPage`: The current page index.
- `TotalPages`: Total number of pages.
- `PageSize`: Number of items per page.
- `TotalCount`: Total number of items.
- `HasPrevious`: Indicates if there is a previous page.
- `HasNext`: Indicates if there is a next page.

#### Usage

##### Exposing Pagination Metadata with X-Pagination Header

In your API, you can include pagination metadata in the response headers using the custom header `X-Pagination`. This allows clients to easily understand the pagination status without inspecting the payload.

Update your CORS policy to allow 'X-Pagination' as an exposed header:
```csharp
services.AddCors(options =>
{
    options.AddPolicy(policyName, policyBuilder =>
    {
        policyBuilder.WithOrigins(context.Configuration.GetValue<string>("CORSWhitelist").Split(","))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("x-pagination") <--
        .AllowCredentials();
    });
});
```

Here is how you can include `X-Pagination` in your API controller:

```csharp
[HttpGet]
public IActionResult GetPagedEmployees(int pageIndex, int pageSize)
{
    var source = GetQueryableSource();  // Replace this with your IQueryable source
    int pageIndex = 0;  // Replace with the current page index
    int pageSize = 10;  // Replace with the desired page size

    var pagedResult = PagedList<Employee>.ToPagedList(source, pageIndex, pageSize);

    var metadata = new
    {
        pagedResult.TotalCount,
        pagedResult.PageSize,
        pagedResult.CurrentPage,
        pagedResult.TotalPages,
        pagedResult.HasNext,
        pagedResult.HasPrevious
    };

    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

    return Ok(pagedResult);
}
```

##### Reading X-Pagination Header in Client

Client applications can read the `X-Pagination` header to get information about the current pagination status.

Example in JavaScript:

```javascript
fetch("https://your-api.com/employees?pageIndex=0&pageSize=10")
.then(response => {
    const headers = JSON.parse(response.headers["x-pagination"]);
    setHasNext(headers.HasNext)
    setHasPrevious(headers.HasPrevious)
    setTotalCount(headers.TotalCount)
    setTotalPages(headers.TotalPages)
})
```


#### Async Methods

Note that asynchronous methods for generating `PagedList` instances are currently not supported in the .NET standard version. They may be added in future releases.