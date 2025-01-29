# Northwind API
## Swagger URL
https://data-northwind.appbuilder.dev/swagger/index.html

## Highlights
- Textbook REST example.
- POST accepts the whole entity, even the "id", although it gets ignored and returns the created entity
- PUT accepts the whole entity, even the "id"
- DELETE methods need to map the id, like id=customerId
- DELETE methods return a copy of the deleted entity
- Role-based auth support - [blog](https://www.infragistics.com/community/blogs/b/infragistics/posts/create-role-based-web-api-with-asp-net-core)
- In order to create a Bearer token, use the POST -> `/Auth/Register` with the example request body:
```
{
  "email": "zkolev@infragistics.com",
  "password": "test",
  "confirmedPassword": "test"
}
```

This will return a token in the format:

`eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6Im....`
