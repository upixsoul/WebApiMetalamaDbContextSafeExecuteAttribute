create database dagg
go

use dagg
go
     
create table dbo.Product
(
    Id       int identity,
    Name   nchar(50) not null,
    Price decimal not null
)
    go

INSERT INTO dagg.dbo.Product (Name, Price) VALUES (N'product 1', 11111);
INSERT INTO dagg.dbo.Product (Name, Price) VALUES (N'product 2',22222);
go
