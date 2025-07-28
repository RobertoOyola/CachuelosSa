CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) NOT NULL ,
    Correo NVARCHAR(100) NOT NULL ,
    ContrasenaHash NVARCHAR(255) NOT NULL,

    Verificado BIT DEFAULT 0,
    Activo BIT DEFAULT 1,
	Subscrito BIT DEFAULT 0,
	FechaFinSubscrito DATETIME NULL,

    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaUltimoLogin DATETIME NULL,
    FechaActualizacion DATETIME DEFAULT GETDATE(),

    RolId INT DEFAULT 1,

    TokenRecuperacion NVARCHAR(255) NULL,
    ExpiracionToken DATETIME NULL
);

GO

CREATE TRIGGER trg_Usuarios_Update
ON Usuarios
AFTER UPDATE
AS
BEGIN
    UPDATE Usuarios
    SET FechaActualizacion = GETDATE()
    FROM Inserted
    WHERE Usuarios.Id = Inserted.Id;
END;

GO

CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL
);

GO

ALTER TABLE Usuarios
ADD CONSTRAINT FK_Usuarios_Roles
    FOREIGN KEY (RolId) REFERENCES Roles(Id);

GO

INSERT INTO Roles (NombreRol, Descripcion)
VALUES 
('usuario', 'Rol por defecto para usuarios normales'),
('admin', 'Administrador con acceso total'),
('moderador', 'Puede gestionar contenidos moderados');

GO

-- Tabla de tipos de documentos
CREATE TABLE TipoDocumentos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreDocumento NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL
);

GO

-- Tabla de documentos
CREATE TABLE Documento (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UrlDocumento NVARCHAR(255) NOT NULL,
    IdTipoDocumentos INT NOT NULL,
    Activo BIT DEFAULT 1,
    FechaIngreso DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Documento_TipoDocumento FOREIGN KEY (IdTipoDocumentos)
        REFERENCES TipoDocumentos(Id)
);

GO

-- Trigger para actualizar fecha de modificación
CREATE TRIGGER trg_Documento_Update
ON Documento
AFTER UPDATE
AS
BEGIN
    UPDATE Documento
    SET FechaActualizacion = GETDATE()
    FROM Inserted
    WHERE Documento.Id = Inserted.Id;
END;

GO

-- Tabla intermedia entre usuarios y documentos
CREATE TABLE UsuariosXDocumentos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdDocumento INT NOT NULL,

    FechaAsignacion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_UsuariosXDocumentos_Usuarios FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(Id),

    CONSTRAINT FK_UsuariosXDocumentos_Documento FOREIGN KEY (IdDocumento)
        REFERENCES Documento(Id)
);

GO

INSERT INTO TipoDocumentos (NombreDocumento, Descripcion)
values 
	('Curriculum', ''),
	('Titulos', ''),
	('Historial_Policial', '');
	
GO

CREATE TABLE UsuarioInfo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
	Nombre NVARCHAR(200) NULL,
	Apellido NVARCHAR(200) NULL,
	FechaNacimiento DATETIME DEFAULT GETDATE(),
	TipoIdentificacion NVARCHAR(1) NULL,
	Identificacion NVARCHAR(50) NULL,
	EstadoCivil NVARCHAR(1) NULL,
	Direccion NVARCHAR(255) NULL,
	Telefono NVARCHAR(20) NULL,
    Ciudad NVARCHAR(5) NULL,
    Provincia NVARCHAR(5) NULL,
	Nacionalidad NVARCHAR(5) NULL,
	Discapacidad BIT DEFAULT 0,
	TipoDiscapacidad NVARCHAR(100) NULL,
    UrlImg NVARCHAR(500) NULL,
    Descripcion NVARCHAR(MAX) NULL,
    Activo BIT DEFAULT 1,
    FechaUltimaConexion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Usuario_UsuarioInfo FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(Id)
);

GO

CREATE TRIGGER trg_UsuariosInfo_Update
ON UsuarioInfo
AFTER UPDATE
AS
BEGIN
    UPDATE UsuarioInfo
    SET FechaActualizacion = GETDATE()
    FROM Inserted
    WHERE UsuarioInfo.Id = Inserted.Id;
END;

GO

CREATE TABLE Catalogo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCat NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(50) NOT NULL,
	Nombre NVARCHAR(100) NULL,
	Descripcion NVARCHAR(200) NULL,
	Adicional NVARCHAR(100) NULL,
	FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1

);

GO

CREATE TRIGGER trg_Catalogo_Update
ON UsuarioInfo
AFTER UPDATE
AS
BEGIN
    UPDATE Catalogo
    SET FechaActualizacion = GETDATE()
    FROM Inserted
    WHERE Catalogo.Id = Inserted.Id;
END;

GO

INSERT INTO Catalogo (NombreCat, Codigo, Nombre, Adicional, Descripcion)
values 
	('TIPO_IDENTIFICACION', 'C', 'CEDULA', '10', ''),
	('TIPO_IDENTIFICACION', 'R', 'RUC', '13', ''),
	('TIPO_IDENTIFICACION', 'E', 'EXTRANJERO', '30', ''),
	('ESTADO_CIVIL', 'S', 'Soltero/A', '', ''),
	('ESTADO_CIVIL', 'C', 'Casado/A', '', ''),
	('ESTADO_CIVIL', 'D', 'Divorciado/A', '', ''),
	('ESTADO_CIVIL', 'V', 'Viudo/A', '', ''),
	('ESTADO_CIVIL', 'U', 'Union Libre', '', ''),
	('SMTP_CONFIG', 'HOST', 'smtp.gmail.com', '', ''),
	('SMTP_CONFIG', 'PORT', '587', '', ''),
	('SMTP_CONFIG', 'MAIL', 'cachuelos.sa@gmail.com', '', '');