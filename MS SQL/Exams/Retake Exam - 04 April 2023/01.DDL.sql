CREATE DATABASE [Accounting]

GO

USE [Accounting]

GO

-- Categories
-- Countries
-- Addresses
-- Vendors
-- Clients
-- Invoices
-- Products
-- ProductsClients

CREATE TABLE [Categories] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(10) NOT NULL,
)

GO

CREATE TABLE [Countries] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(10) NOT NULL,
)

GO

CREATE TABLE [Addresses] (
	[Id] INT PRIMARY KEY IDENTITY,
	[StreetName] NVARCHAR(20) NOT NULL,
	[StreetNumber] INT,
	[PostCode] INT NOT NULL,
	[City] VARCHAR(25) NOT NULL,
	[CountryId] INT FOREIGN KEY REFERENCES [Countries]([Id]) NOT NULL
)

GO

CREATE TABLE [Vendors] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(25) NOT NULL,
	[NumberVAT] NVARCHAR(15) NOT NULL,
	[AddressId] INT FOREIGN KEY REFERENCES [Addresses]([Id]) NOT NULL
)

GO

CREATE TABLE [Clients] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(25) NOT NULL,
	[NumberVAT] NVARCHAR(15) NOT NULL,
	[AddressId] INT FOREIGN KEY REFERENCES [Addresses]([Id]) NOT NULL
)

GO

CREATE TABLE [Invoices] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Number] INT UNIQUE NOT NULL,
	[IssueDate] DATETIME2 NOT NULL,
	[DueDate] DATETIME2 NOT NULL,
	[Amount] DECIMAL(18, 2) NOT NULL,
	[Currency] VARCHAR(5) NOT NULL,
	[ClientId] INT FOREIGN KEY REFERENCES [Clients]([Id]) NOT NULL
)

GO

CREATE TABLE [Products] (
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(35) NOT NULL,
	[Price] DECIMAL(18, 2) NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES [Categories]([Id]) NOT NULL,
	[VendorId] INT FOREIGN KEY REFERENCES [Vendors]([Id]) NOT NULL
)

GO

CREATE TABLE [ProductsClients] (
	[ProductId] INT FOREIGN KEY REFERENCES [Products]([Id]) NOT NULL,
	[ClientId] INT FOREIGN KEY REFERENCES [Clients]([Id]) NOT NULL,
	PRIMARY KEY([ProductId], [ClientId])
)