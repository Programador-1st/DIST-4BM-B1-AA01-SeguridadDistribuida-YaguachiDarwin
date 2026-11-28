
CREATE DATABASE UsuariosDB;
GO

USE UsuariosDB;
GO

CREATE TABLE Perfiles (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UsuarioId NVARCHAR(450) NOT NULL, 
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NULL,
    Direccion NVARCHAR(255) NULL,
    FechaNacimiento DATE NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL
);
GO


CREATE INDEX IX_Perfiles_UsuarioId ON Perfiles (UsuarioId);
GO



ALTER TABLE Perfiles ADD Correo NVARCHAR(256) NOT NULL DEFAULT '';
GO


USE UsuariosDB;
UPDATE Perfiles SET Correo = 'mateoNar1@gmail.com' WHERE UsuarioId = '6176a540-b962-44d2-98fe-c6d39e9f17bb';