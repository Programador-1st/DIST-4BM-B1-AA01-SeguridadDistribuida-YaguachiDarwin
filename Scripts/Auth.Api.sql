
CREATE DATABASE AuthDB;
GO

USE AuthDB;
GO


CREATE TABLE Roles (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(256) NULL,
    NombreNormalizado NVARCHAR(256) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL
);
GO
CREATE UNIQUE INDEX IX_Roles_NombreNormalizado ON Roles (NombreNormalizado);
GO


CREATE TABLE Usuarios (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    NombreUsuario NVARCHAR(256) NULL,
    NombreUsuarioNormalizado NVARCHAR(256) NULL,
    Correo NVARCHAR(256) NULL,
    CorreoNormalizado NVARCHAR(256) NULL,
    CorreoConfirmado BIT NOT NULL DEFAULT 0,
    HashContrasena NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    Telefono NVARCHAR(MAX) NULL,
    TelefonoConfirmado BIT NOT NULL DEFAULT 0,
    DosFactoresHabilitado BIT NOT NULL DEFAULT 0,
    BloqueoFin DATETIMEOFFSET NULL,
    BloqueoHabilitado BIT NOT NULL DEFAULT 0,
    IntentosFallidos INT NOT NULL DEFAULT 0
);
GO
CREATE INDEX IX_Usuarios_CorreoNormalizado ON Usuarios (CorreoNormalizado);
CREATE UNIQUE INDEX IX_Usuarios_NombreUsuarioNormalizado ON Usuarios (NombreUsuarioNormalizado);
GO

CREATE TABLE UsuariosClaims (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    UsuarioId NVARCHAR(450) NOT NULL,
    TipoClaim NVARCHAR(MAX) NULL,
    ValorClaim NVARCHAR(MAX) NULL,
    CONSTRAINT FK_UsuariosClaims_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id) ON DELETE CASCADE
);
GO
CREATE INDEX IX_UsuariosClaims_UsuarioId ON UsuariosClaims (UsuarioId);
GO


CREATE TABLE UsuariosLogins (
    ProveedorLogin NVARCHAR(450) NOT NULL,
    ClaveProveedor NVARCHAR(450) NOT NULL,
    NombreMostrarProveedor NVARCHAR(MAX) NULL,
    UsuarioId NVARCHAR(450) NOT NULL,
    CONSTRAINT PK_UsuariosLogins PRIMARY KEY (ProveedorLogin, ClaveProveedor),
    CONSTRAINT FK_UsuariosLogins_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id) ON DELETE CASCADE
);
GO
CREATE INDEX IX_UsuariosLogins_UsuarioId ON UsuariosLogins (UsuarioId);
GO


CREATE TABLE UsuariosTokens (
    UsuarioId NVARCHAR(450) NOT NULL,
    ProveedorLogin NVARCHAR(450) NOT NULL,
    Nombre NVARCHAR(450) NOT NULL,
    Valor NVARCHAR(MAX) NULL,
    CONSTRAINT PK_UsuariosTokens PRIMARY KEY (UsuarioId, ProveedorLogin, Nombre),
    CONSTRAINT FK_UsuariosTokens_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id) ON DELETE CASCADE
);
GO


CREATE TABLE UsuariosRoles (
    UsuarioId NVARCHAR(450) NOT NULL,
    RolId NVARCHAR(450) NOT NULL,
    CONSTRAINT PK_UsuariosRoles PRIMARY KEY (UsuarioId, RolId),
    CONSTRAINT FK_UsuariosRoles_Roles FOREIGN KEY (RolId) REFERENCES Roles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_UsuariosRoles_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id) ON DELETE CASCADE
);
GO
CREATE INDEX IX_UsuariosRoles_RolId ON UsuariosRoles (RolId);
GO


CREATE TABLE RolesClaims (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    RolId NVARCHAR(450) NOT NULL,
    TipoClaim NVARCHAR(MAX) NULL,
    ValorClaim NVARCHAR(MAX) NULL,
    CONSTRAINT FK_RolesClaims_Roles FOREIGN KEY (RolId) REFERENCES Roles (Id) ON DELETE CASCADE
);
GO
CREATE INDEX IX_RolesClaims_RolId ON RolesClaims (RolId);
GO


INSERT INTO Roles (Id, Nombre, NombreNormalizado, ConcurrencyStamp) 
VALUES 
(NEWID(), 'Administrador', 'ADMINISTRADOR', NEWID()),
(NEWID(), 'Cliente', 'CLIENTE', NEWID());
GO


ALTER TABLE Usuarios ADD FechaRegistro DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME();
ALTER TABLE Usuarios ADD NombreCompleto NVARCHAR(150) NULL;
ALTER TABLE Usuarios ADD EstadoCuenta INT NOT NULL DEFAULT 1;
GO
