-- Script MySQL para TiendaElectronica
CREATE DATABASE IF NOT EXISTS TiendaElectronica CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE TiendaElectronica;

CREATE TABLE IF NOT EXISTS Usuarios (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Nombre      VARCHAR(100) NOT NULL,
    Email       VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(64) NOT NULL,
    Rol         VARCHAR(50) NOT NULL DEFAULT 'Vendedor',
    Activo      TINYINT(1) NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Productos (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Nombre      VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(500) NULL,
    Categoria   VARCHAR(100) NOT NULL,
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT NOT NULL DEFAULT 0,
    Activo      TINYINT(1) NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Ventas (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId   INT NOT NULL,
    Fecha       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Total       DECIMAL(10,2) NOT NULL,
    Estado      VARCHAR(50) NOT NULL DEFAULT 'Completada',
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

CREATE TABLE IF NOT EXISTS DetalleVentas (
    Id              INT AUTO_INCREMENT PRIMARY KEY,
    VentaId         INT NOT NULL,
    ProductoId      INT NOT NULL,
    Cantidad        INT NOT NULL,
    PrecioUnitario  DECIMAL(10,2) NOT NULL,
    Subtotal        DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (VentaId) REFERENCES Ventas(Id),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);

-- Usuarios de prueba
-- admin123  -> bdf5df9986eb2f893e55cf22507ed7f9b16fd590ad5b7be3d614c5af0a1e15fe
-- password  -> 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8
INSERT INTO Usuarios (Nombre, Email, PasswordHash, Rol) VALUES
('Administrador', 'admin@tienda.com', 'bdf5df9986eb2f893e55cf22507ed7f9b16fd590ad5b7be3d614c5af0a1e15fe', 'Admin'),
('Vendedor Demo',  'vendedor@tienda.com', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Vendedor');

INSERT INTO Productos (Nombre, Descripcion, Categoria, Precio, Stock) VALUES
('Laptop Gamer X500',    'Procesador i7, 16GB RAM, RTX 3060',  'Laptops',     1299.99, 15),
('Mouse Inalambrico Pro','DPI ajustable, 3 botones',            'Perifericos',   29.99, 50),
('Teclado Mecanico RGB', 'Switches Blue, retroiluminado',       'Perifericos',   89.99,  8),
('Monitor 24 FHD',       'IPS 144Hz, HDMI+DP',                  'Monitores',    249.99, 20),
('Auriculares Gaming',   '7.1 Surround, microfono retractil',   'Audio',         59.99,  4),
('SSD 1TB NVMe',         'PCIe 4.0, 7000MB/s lectura',          'Almacenamiento',89.99,  3),
('Webcam 4K',            'Autofocus, microfono estereo',        'Perifericos',   79.99, 25),
('Hub USB-C 7en1',       'HDMI 4K, USB 3.0, SD card reader',   'Accesorios',    39.99,  5),
('Pad de Carga Qi',      '15W carga rapida, compatible multiples', 'Accesorios', 19.99,  2),
('GPU RTX 4070',         '12GB GDDR6X, ray tracing',            'Tarjetas Graficas', 599.99, 1);

INSERT INTO Ventas (UsuarioId, Fecha, Total) VALUES (1, NOW(), 1389.98);
INSERT INTO DetalleVentas (VentaId, ProductoId, Cantidad, PrecioUnitario, Subtotal) VALUES
(1, 1, 1, 1299.99, 1299.99),
(1, 2, 3,   29.99,   89.99);

INSERT INTO Ventas (UsuarioId, Fecha, Total) VALUES (2, DATE_SUB(NOW(), INTERVAL 3 DAY), 339.98);
INSERT INTO DetalleVentas (VentaId, ProductoId, Cantidad, PrecioUnitario, Subtotal) VALUES
(2, 4, 1, 249.99, 249.99),
(2, 3, 1,  89.99,  89.99);
