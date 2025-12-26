CREATE DATABASE CachuelosSa;

GO

USE CachuelosSa;

GO

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
	('Historial_Policial', ''),
	('Cedula', ''),
	('Comprobante de pago', '');
	
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
ON Catalogo
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
	('SMTP_CONFIG', 'MAIL', 'cachuelos.sa@gmail.com', '', ''),
	('TIPO_OTP', 'VU', 'Verificar Usuario', '', ''),
	('TIPO_OTP', 'CC', 'Cambio Contraseña', '', ''),
	('TIPO_OTP', 'EU', 'Eliminar Usuario', '', ''),
	('TIPO_OTP', 'DU', 'Desbloquear Usuario', '', ''),
	('TIPO_OTP', 'IT', 'Iniciar Trabajo', '', ''),
	('TIPO_OTP', 'FT', 'Finalizar Trabajo', '', '');
	
GO

CREATE TABLE OtpAction (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    CodigoOtp NVARCHAR(255) NOT NULL,
	TipoOtp NVARCHAR(5) NOT NULL,
	Expiracion DATETIME NOT NULL,
	Usado BIT DEFAULT 0,
	FechaGeneracion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,

    CONSTRAINT FK_Usuario_OtpAction FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(Id)
);

GO

CREATE TRIGGER trg_OtpAction_Update
ON OtpAction
AFTER UPDATE
AS
BEGIN
    UPDATE OtpAction
    SET FechaActualizacion = GETDATE()
    FROM Inserted
    WHERE OtpAction.Id = Inserted.Id;
END;

GO

INSERT INTO Catalogo (NombreCat, Codigo, Nombre, Descripcion,Adicional)
VALUES
    ('EST_TRABAJO', 'PE', 'Pendiente', 'Publicado pero sin postulaciones',''),
    ('EST_TRABAJO', 'AS', 'Asignado', 'Cliente seleccionó a un trabajador',''),
    ('EST_TRABAJO', 'EP', 'En Proceso', 'El trabajador está realizando el trabajo',''),
    ('EST_TRABAJO', 'FN', 'Finalizado', 'Trabajo completado por el trabajador',''),
    ('EST_TRABAJO', 'CN', 'Cancelado', 'Trabajo cancelado por el cliente',''),
	('EST_SUBASTA', 'AB', 'Abierta', 'Recibiendo ofertas',''),
	('EST_SUBASTA', 'FI', 'Finalizada', 'Oferta ganadora seleccionada',''),
	('EST_SUBASTA', 'CA', 'Cancelada', 'Subasta cerrada sin ganador','');

GO

CREATE TABLE CategoriaTrabajo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE()
);

CREATE TRIGGER trg_CategoriaTrabajo_Update
ON CategoriaTrabajo
AFTER UPDATE
AS
BEGIN
    UPDATE ct
    SET ct.FechaActualizacion = GETDATE()
    FROM CategoriaTrabajo ct
    INNER JOIN Inserted i ON ct.Id = i.Id;
END;

GO

INSERT INTO CategoriaTrabajo (Nombre, Descripcion, Activo)
VALUES
    ('Pintar', 'Servicios de pintura en paredes, techos y estructuras', 1),
    ('Barrer', 'Limpieza básica de pisos en casas, patios o locales', 1),
    ('Cocinar', 'Preparación de alimentos por horas o por evento', 1),
    ('Mover muebles', 'Ayuda para mover, cargar o reubicar muebles', 1),
    ('Colocar cuadros', 'Instalación de cuadros, repisas y adornos', 1),
    ('Limpiar', 'Limpieza general o profunda de espacios', 1),
    ('Plomería', 'Reparación de fugas, grifería y tuberías', 1),
    ('Electricidad', 'Revisión o reparación de instalaciones eléctricas', 1),
    ('Jardinería', 'Mantenimiento de jardines, podas y riego', 1),
    ('Lavado de autos', 'Lavado interno y externo de vehículos', 1),
    ('Ensamblaje', 'Armar muebles o equipos en el hogar', 1),
    ('Cuidado de mascotas', 'Paseo, baño o cuidado temporal de mascotas', 1);


GO

CREATE TABLE Trabajo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuarioCreador INT NOT NULL,
    IdCategoria INT NOT NULL,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    Direccion NVARCHAR(255) NULL,
    Latitud DECIMAL(10,7) NULL,
    Longitud DECIMAL(10,7) NULL,
    FechaFinSubasta DATETIME NULL,
    FechaTrabajo DATETIME NULL,
    TiempoEstimadoTrabajo INT NULL,
    PrecioReferencial DECIMAL(10,2) NULL,
    Especial BIT DEFAULT 0,
    Estado NVARCHAR(5) NOT NULL,
    FechaPublicacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,

    CONSTRAINT FK_Trabajo_Usuario FOREIGN KEY (IdUsuarioCreador)
        REFERENCES Usuarios(Id),

    CONSTRAINT FK_Trabajo_Categoria FOREIGN KEY (IdCategoria)
        REFERENCES CategoriaTrabajo(Id)
);

GO

CREATE TABLE TrabajoImagen (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdTrabajo INT NOT NULL,
    UrlImagen NVARCHAR(500) NOT NULL,
    FechaIngreso DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,

    CONSTRAINT FK_TrabajoImagen_Trabajo FOREIGN KEY (IdTrabajo)
        REFERENCES Trabajo(Id)
);


GO

CREATE TABLE Subasta (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdTrabajo INT NOT NULL,

    Estado NVARCHAR(5) NOT NULL,
    FechaInicio DATETIME DEFAULT GETDATE(),
    FechaFin DATETIME NULL,

    CONSTRAINT FK_Subasta_Trabajo FOREIGN KEY (IdTrabajo)
        REFERENCES Trabajo(Id)
);


GO

CREATE TABLE SubastaOferta (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdSubasta INT NOT NULL,
    IdUsuarioTrabajador INT NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    Mensaje NVARCHAR(500) NULL,
    FechaOferta DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Oferta_Subasta FOREIGN KEY (IdSubasta)
        REFERENCES Subasta(Id),

    CONSTRAINT FK_Oferta_Usuario FOREIGN KEY (IdUsuarioTrabajador)
        REFERENCES Usuarios(Id)
);


GO

CREATE TABLE OfertaImagen (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdOferta INT NOT NULL,
    UrlImagen NVARCHAR(500) NOT NULL,
    FechaIngreso DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,

    CONSTRAINT FK_OfertaImagen_Oferta FOREIGN KEY (IdOferta)
        REFERENCES SubastaOferta(Id)
);


