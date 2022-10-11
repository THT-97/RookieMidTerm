create database Ecommerce on primary(
 name = 'Ecom_Data',
 filename = 'D:\LABS\Intern\NashTech\MidTerm\Ecom_Data.mdf'
)
 log on(
 name = 'Ecom_Log',
 filename = 'D:\LABS\Intern\NashTech\MidTerm\Ecom_Log.mdf'
)

USE Ecommerce

CREATE TABLE Category(
	CategoryID VARCHAR(5) PRIMARY KEY NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	Description NVARCHAR(MAX) NULL,
	Status TINYINT NOT NULL DEFAULT 1
)

CREATE TABLE Brand(
	BrandID VARCHAR(5) PRIMARY KEY NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	Description NVARCHAR(MAX) NULL,
	Status TINYINT NOT NULL DEFAULT 1
)

CREATE TABLE Product(
	ProductID VARCHAR(10) PRIMARY KEY NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	Colors NVARCHAR(200) NULL DEFAULT 'Black',
	Sizes VARCHAR(50) NULL DEFAULT 'M',
	Description NVARCHAR(MAX) NULL,
	ListPrice MONEY NOT NULL,
	SalePrice MONEY NULL,
	Images VARCHAR(MAX) NULL DEFAULT 'default.png',
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
	UpdatedDate DATETIME NULL,
	Quantity SMALLINT NOT NULL DEFAULT 1,
	Rating TINYINT NULL DEFAULT 0,
	RatingCount INT NULL DEFAULT 0,
	Status TINYINT NOT NULL DEFAULT 1,
	CategoryID VARCHAR(5) NOT NULL FOREIGN KEY(CategoryID) REFERENCES dbo.Category,
	BrandID VARCHAR(5) NOT NULL FOREIGN KEY(BrandID) REFERENCES dbo.Brand
)