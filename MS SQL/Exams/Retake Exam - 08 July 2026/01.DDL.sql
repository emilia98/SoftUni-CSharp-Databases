CREATE DATABASE [TechStore]

GO

USE [TechStore]

GO

-- Categories
-- Manufacturers
-- Customers
-- Stores
-- Products
-- StoresProducts
-- Orders

CREATE TABLE [Categories] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) UNIQUE NOT NULL,
)

GO

CREATE TABLE [Manufacturers] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[Country] NVARCHAR(50) NOT NULL
)

GO

CREATE TABLE [Customers] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(80) NOT NULL,
	[PhoneNumber] NVARCHAR(20) NOT NULL,
	[Email] NVARCHAR(80)
)

GO

CREATE TABLE [Stores] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(100) NOT NULL,
	[IsOnline] BIT NOT NULL
)

GO

CREATE TABLE [Products] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(100) NOT NULL,
	[Price] DECIMAL(18, 2) NOT NULL,
	[Specifications] NVARCHAR(1000),
	[ManufacturerId] INT FOREIGN KEY REFERENCES [Manufacturers]([Id]) NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES [Categories]([Id]) NOT NULL
)

GO

CREATE TABLE [StoresProducts] (
	[StoreId] INT FOREIGN KEY REFERENCES [Stores]([Id]) NOT NULL,
	[ProductId] INT FOREIGN KEY REFERENCES [Products]([Id]) NOT NULL,
	PRIMARY KEY([StoreId], [ProductId])
)

GO

CREATE TABLE [Orders] (
	[Id] INT PRIMARY KEY IDENTITY,
	[OrderDate] DATETIME2 NOT NULL,
	[CustomerId] INT FOREIGN KEY REFERENCES [Customers]([Id]) NOT NULL,
	[ProductId] INT FOREIGN KEY REFERENCES [Products]([Id]) NOT NULL,
	[StoreId] INT FOREIGN KEY REFERENCES [Stores]([Id]) NOT NULL
)