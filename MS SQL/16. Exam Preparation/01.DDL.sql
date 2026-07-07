CREATE DATABASE [LibraryDb]

GO

USE [LibraryDb]

GO

CREATE TABLE [Genres] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL
)

GO

/*
PK -> Guarantees uniqueness. unique identifier, NOT NULL values,
FK -> Allow duplicates, Nullable values, but creates relationships between tables
*/

CREATE TABLE [Contacts] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Email] NVARCHAR(100),
	[PhoneNumber] NVARCHAR(20),
	[PostAddress] NVARCHAR(200),
	[Website] NVARCHAR(50)
)

GO

CREATE TABLE [Authors] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(100) NOT NULL,
	[ContactId] INT FOREIGN KEY REFERENCES [Contacts]([Id]) NOT NULL
)

CREATE TABLE [Libraries] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[ContactId] INT FOREIGN KEY REFERENCES [Contacts]([Id]) NOT NULL
)

GO

CREATE TABLE [Books] (
   [Id] INT PRIMARY KEY IDENTITY,
   [Title] NVARCHAR(100) NOT NULL,
   [YearPublished] INT NOT NULL,
   [ISBN] NVARCHAR(13) UNIQUE NOT NULL,
   [AuthorId] INT FOREIGN KEY REFERENCES [Authors]([Id]) NOT NULL,
   [GenreId] INT FOREIGN KEY REFERENCES [Genres]([Id]) NOT NULL
)

GO

CREATE TABLE LibrariesBooks (
	[LibraryId] INT FOREIGN KEY REFERENCES [Libraries]([Id]),
	[BookId] INT FOREIGN KEY REFERENCES [Books]([Id]),
	PRIMARY KEY([LibraryId], [BookId])
)

GO