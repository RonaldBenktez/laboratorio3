# Laboratorio 3 — Sistema de Ventas con Login y Reportes

Aplicación de escritorio en **.NET 10 / Windows Forms** para una tienda de productos electrónicos.

## Requisitos

- .NET 10 SDK
- SQL Server (Express o superior)
- SQL Server Management Studio (opcional)

## Configuración de la base de datos

1. Abrir `database_setup.sql` en SSMS y ejecutarlo.  
   Esto crea la base de datos `TiendaElectronica` con tablas y datos de prueba.

2. Ajustar la cadena de conexión en `laboratorio3/Program.cs` si el servidor no es `localhost`:

```csharp
"Server=TU_SERVIDOR;Database=TiendaElectronica;Trusted_Connection=True;TrustServerCertificate=True;"
```

## Usuarios de prueba

| Email                    | Contraseña  | Rol       |
|--------------------------|-------------|-----------|
| admin@tienda.com         | admin123    | Admin     |
| vendedor@tienda.com      | password    | Vendedor  |

Las contraseñas se almacenan como **hash SHA-256** (nunca en texto plano).

## Arquitectura

```
laboratorio3/
├── Models/              Entidades mapeadas a tablas (RepoDb)
│   ├── Usuario.cs
│   ├── Producto.cs
│   ├── Venta.cs
│   └── DetalleVenta.cs
├── Data/
│   ├── DatabaseConfig.cs          Inicialización de RepoDb + cadena de conexión
│   ├── Interfaces/
│   │   └── IRepository<T>.cs      Contrato genérico CRUD
│   └── Repositories/
│       ├── BaseRepository<T>.cs   Implementación genérica con RepoDb
│       ├── UsuarioRepository.cs
│       ├── ProductoRepository.cs
│       ├── VentaRepository.cs
│       └── DetalleVentaRepository.cs
├── Helpers/
│   ├── CryptoHelper.cs    Hash SHA-256
│   └── SessionHelper.cs   Usuario autenticado en sesión
└── Forms/
    ├── LoginForm.cs         Inicio de sesión (SHA-256)
    ├── MainForm.cs          Formulario principal con sidebar
    ├── ProductosForm.cs     CRUD de productos + semáforo de stock
    ├── ProductoForm.cs      ★ Formulario modal (ShowDialog)
    ├── NuevaVentaForm.cs    ★ Formulario modal de registro de venta
    ├── VentasForm.cs        Historial de ventas con filtro por fecha
    ├── ReporteVentasForm.cs Reporte de productos vendidos (cantidad + monto)
    └── ReporteStockForm.cs  Reporte de stock con filtro por categoría
```

## Características

- 🔐 **Login con SHA-256** — contraseñas nunca en texto plano
- 🗄️ **RepoDb ORM** — repositorio genérico `IRepository<T>` con `BaseRepository<T>`
- 🖥️ **Formulario Principal** — sidebar de navegación con panel de contenido intercambiable
- 🪟 **Formularios Modales** — `ProductoForm` y `NuevaVentaForm` usan `ShowDialog()`
- 📊 **Reporte de Ventas** — productos vendidos, cantidades y montos acumulados
- 📦 **Reporte de Stock** — inventario completo con filtros por categoría y búsqueda
- 🚦 **Semáforo de Stock (BONUS)** — filas en DataGridView coloreadas automáticamente:
  - 🔴 Rojo oscuro: stock ≤ 5 (crítico)
  - 🟡 Ámbar oscuro: stock ≤ 15 (bajo)
  - ⬛ Normal: stock > 15
