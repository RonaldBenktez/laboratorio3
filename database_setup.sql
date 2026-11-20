-- Script de creación de base de datos: TiendaElectronica
-- Ejecutar en SQL Server Management Studio

CREATE DATABASE TiendaElectronica;
GO

USE TiendaElectronica;
GO

CREATE TABLE Usuarios (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL,
    Email       NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(64) NOT NULL,  -- SHA-256 hex = 64 chars
    Rol         NVARCHAR(50) NOT NULL DEFAULT 'Vendedor',
    Activo      BIT NOT NULL DEFAULT 1
);

CREATE TABLE Productos (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    Categoria   NVARCHAR(100) NOT NULL,
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT NOT NULL DEFAULT 0,
    Activo      BIT NOT NULL DEFAULT 1
);

CREATE TABLE Ventas (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId   INT NOT NULL REFERENCES Usuarios(Id),
    Fecha       DATETIME2 NOT NULL DEFAULT GETDATE(),
    Total       DECIMAL(10,2) NOT NULL,
    Estado      NVARCHAR(50) NOT NULL DEFAULT 'Completada'
);

CREATE TABLE DetalleVentas (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    VentaId         INT NOT NULL REFERENCES Ventas(Id),
    ProductoId      INT NOT NULL REFERENCES Productos(Id),
    Cantidad        INT NOT NULL,
    PrecioUnitario  DECIMAL(10,2) NOT NULL,
    Subtotal        DECIMAL(10,2) NOT NULL
);
GO

-- Datos de prueba
-- Contraseña "admin123" => SHA-256
INSERT INTO Usuarios (Nombre, Email, PasswordHash, Rol) VALUES
('Administrador', 'admin@tienda.com', 'bdf5df9986eb2f893e55cf22507ed7f9b16fd590ad5b7be3d614c5af0a1e15fe', 'Admin'),
('Vendedor Demo',  'vendedor@tienda.com', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Vendedor');
-- admin123  -> bdf5df9986eb2f893e55cf22507ed7f9b16fd590ad5b7be3d614c5af0a1e15fe
-- password  -> 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8

INSERT INTO Productos (Nombre, Descripcion, Categoria, Precio, Stock) VALUES
('Laptop Gamer X500',    'Procesador i7, 16GB RAM, RTX 3060',  'Laptops',     1299.99, 15),
('Mouse Inalámbrico Pro','DPI ajustable, 3 botones',            'Periféricos',   29.99, 50),
('Teclado Mecánico RGB', 'Switches Blue, retroiluminado',       'Periféricos',   89.99,  8),
('Monitor 24" FHD',      'IPS 144Hz, HDMI+DP',                  'Monitores',    249.99, 20),
('Auriculares Gaming',   '7.1 Surround, micrófono retráctil',   'Audio',         59.99,  4),
('SSD 1TB NVMe',         'PCIe 4.0, 7000MB/s lectura',          'Almacenamiento',89.99,  3),
('Webcam 4K',            'Autofocus, micrófono estéreo',        'Periféricos',   79.99, 25),
('Hub USB-C 7en1',       'HDMI 4K, USB 3.0, SD card reader',   'Accesorios',    39.99,  5),
('Pad de Carga Qi',      '15W carga rápida, compatible múltiples',  'Accesorios',19.99,  2),
('GPU RTX 4070',         '12GB GDDR6X, ray tracing',            'Tarjetas Gráficas', 599.99, 1);

-- Ventas de ejemplo
INSERT INTO Ventas (UsuarioId, Fecha, Total) VALUES (1, GETDATE(), 1389.98);
INSERT INTO DetalleVentas (VentaId, ProductoId, Cantidad, PrecioUnitario, Subtotal) VALUES
(1, 1, 1, 1299.99, 1299.99),
(1, 2, 3,   29.99,   89.99);

INSERT INTO Ventas (UsuarioId, Fecha, Total) VALUES (2, DATEADD(day,-3,GETDATE()), 339.98);
INSERT INTO DetalleVentas (VentaId, ProductoId, Cantidad, PrecioUnitario, Subtotal) VALUES
(2, 4, 1, 249.99, 249.99),
(2, 3, 1,  89.99,  89.99);
GO
