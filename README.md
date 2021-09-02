# MovieApi

This is a ASP.NET Core Web API project made for learning purposes. It is an application constructed in ASP.NET Core. It includes a RESTful API to interact with an SQL Server database built with Entity Framework.
The RESTful API can be used to create, read, update and delete data from the database. The API consists of three objects:

- Movies - A movie can have multiple characters and one franchise.
- Characters - A character can be a part of multiple movies
- Franchises - A franchise can have multiple movies

The database can be duplicated through the add-migration, following the Model Creation rules of the MovieDbContext.cs file.
