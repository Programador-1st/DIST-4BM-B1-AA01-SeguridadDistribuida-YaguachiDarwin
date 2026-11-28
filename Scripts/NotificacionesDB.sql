CREATE DATABASE NotificacionesDB;
GO

USE NotificacionesDB;
GO

CREATE TABLE EnviosCorreo (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Destinatario NVARCHAR(255) NOT NULL,
    Asunto NVARCHAR(255) NOT NULL,
    Cuerpo NVARCHAR(MAX) NULL, 
    FechaEnvio DATETIME NOT NULL DEFAULT GETDATE(),
    Estado NVARCHAR(50) NOT NULL, 
    MensajeError NVARCHAR(MAX) NULL 
);
GO


CREATE TABLE Plantillas (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Asunto NVARCHAR(255) NOT NULL,
    CuerpoHtml NVARCHAR(MAX) NOT NULL
);
GO


USE NotificacionesDB;
GO


ALTER TABLE EnviosCorreo ADD UsuarioId NVARCHAR(450) NULL;
ALTER TABLE EnviosCorreo ADD TipoPlantilla NVARCHAR(100) NULL;
GO


INSERT INTO Plantillas (Nombre, Asunto, CuerpoHtml) 
VALUES ('RecuperacionPassword', 'Restablece tu contraseña', '<h1>Hola</h1><p>Haz clic en el siguiente enlace para restablecer tu contraseña:</p><a href="{{enlace}}">Restablecer Contraseña</a>');
GO


INSERT INTO Plantillas (Nombre, Asunto, CuerpoHtml) 
VALUES ('Bienvenida', 'Bienvenido a la Plataforma', '<h1>Hola</h1><p>Gracias por registrarte.</p>');
GO