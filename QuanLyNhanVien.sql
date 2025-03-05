-- Tạo database
CREATE DATABASE NhanVien;
GO

-- Sử dụng database
USE NhanVien;
GO

-- Tạo bảng Employee
CREATE TABLE Employee (
    EmployeeID INT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Surname NVARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    CCCD NVARCHAR(20) UNIQUE NOT NULL,
    SDT NVARCHAR(15) NOT NULL,
    DepartmentID NVARCHAR(10) NOT NULL,
    Division NVARCHAR(50) NOT NULL
);
GO


