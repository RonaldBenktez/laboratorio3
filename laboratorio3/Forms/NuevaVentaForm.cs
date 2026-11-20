using laboratorio3.Data.Repositories;
using laboratorio3.Helpers;
using laboratorio3.Models;

namespace laboratorio3.Forms
{
    public class NuevaVentaForm : Form
    {
        private ComboBox cmbProducto = null!;
        private NumericUpDown nudCantidad = null!;
        private Button btnAgregar = null!;
        private DataGridView dgvItems = null!;
        private Label lblTotal = null!;
        private Button btnConfirmar = null!;
        private Button btnCancelar = null!;

        private readonly ProductoRepository _productoRepo = new();
        private readonly VentaRepository _ventaRepo = new();
        private readonly DetalleVentaRepository _detalleRepo = new();

        private List<Producto> _productos = new();
        private readonly List<(Producto Prod, int Cantidad)> _items = new();

        public NuevaVentaForm()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void InitializeComponent()
        {
            Text = "Nueva Venta";
            Size = new Size(680, 520);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(30, 30, 46);

            var lblTitulo = new Label
            {
                Text = "Registrar Nueva Venta",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            Controls.Add(lblTitulo);

            var lblProd = new Label { Text = "Producto:", ForeColor = Color.White, AutoSize = true, Location = new Point(20, 60), Font = new Font("Segoe UI", 9) };
            Controls.Add(lblProd);

            cmbProducto = new ComboBox
            {
                Location = new Point(20, 78),
                Size = new Size(360, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(49, 50, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            Controls.Add(cmbProducto);

            var lblCant = new Label { Text = "Cantidad:", ForeColor = Color.White, AutoSize = true, Location = new Point(400, 60), Font = new Font("Segoe UI", 9) };
            Controls.Add(lblCant);

            nudCantidad = new NumericUpDown
            {
                Location = new Point(400, 78),
                Size = new Size(100, 30),
                Minimum = 1,
                Maximum = 9999,
                Value = 1,
                BackColor = Color.FromArgb(49, 50, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            Controls.Add(nudCantidad);

            btnAgregar = new Button
            {
                Text = "+ Agregar",
                Location = new Point(515, 76),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(166, 227, 161),
                ForeColor = Color.FromArgb(30, 30, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            Controls.Add(btnAgregar);

            dgvItems = new DataGridView
            {
                Location = new Point(20, 125),
                Size = new Size(620, 280),
                BackgroundColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(69, 71, 90),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(49, 50, 68);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(137, 180, 250);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvItems.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
            dgvItems.DefaultCellStyle.ForeColor = Color.White;
            dgvItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(69, 71, 90);
            dgvItems.RowTemplate.Height = 28;
            Controls.Add(dgvItems);

            lblTotal = new Label
            {
                Text = "Total: $0.00",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(166, 227, 161),
                AutoSize = true,
                Location = new Point(20, 420)
            };
            Controls.Add(lblTotal);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(430, 415),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(243, 139, 168),
                ForeColor = Color.FromArgb(30, 30, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancelar);

            btnConfirmar = new Button
            {
                Text = "✔ Confirmar",
                Location = new Point(540, 415),
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(137, 180, 250),
                ForeColor = Color.FromArgb(30, 30, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.Click += BtnConfirmar_Click;
            Controls.Add(btnConfirmar);
        }

        private void CargarProductos()
        {
            _productos = _productoRepo.GetActivos().Where(p => p.Stock > 0).ToList();
            cmbProducto.DataSource = _productos;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "Id";
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem is not Producto prod) return;
            int cantidad = (int)nudCantidad.Value;

            var existente = _items.FirstOrDefault(i => i.Prod.Id == prod.Id);
            int yaEnCarrito = existente == default ? 0 : existente.Cantidad;

            if (yaEnCarrito + cantidad > prod.Stock)
            {
                MessageBox.Show($"Stock insuficiente. Disponible: {prod.Stock - yaEnCarrito}", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (existente != default)
            {
                _items.Remove(existente);
                _items.Add((prod, existente.Cantidad + cantidad));
            }
            else
            {
                _items.Add((prod, cantidad));
            }

            RefrescarGrid();
        }

        private void RefrescarGrid()
        {
            dgvItems.DataSource = _items.Select(i => new
            {
                Producto = i.Prod.Nombre,
                i.Cantidad,
                PrecioUnit = i.Prod.Precio.ToString("C2"),
                Subtotal = (i.Prod.Precio * i.Cantidad).ToString("C2")
            }).ToList();

            decimal total = _items.Sum(i => i.Prod.Precio * i.Cantidad);
            lblTotal.Text = $"Total: {total:C2}";
        }

        private void BtnConfirmar_Click(object? sender, EventArgs e)
        {
            if (_items.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto.", "Venta vacía", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = _items.Sum(i => i.Prod.Precio * i.Cantidad);
            var venta = new Venta
            {
                UsuarioId = SessionHelper.UsuarioActual!.Id,
                Fecha = DateTime.Now,
                Total = total
            };

            int ventaId = _ventaRepo.Insert(venta);

            foreach (var (prod, cantidad) in _items)
            {
                var detalle = new DetalleVenta
                {
                    VentaId = ventaId,
                    ProductoId = prod.Id,
                    Cantidad = cantidad,
                    PrecioUnitario = prod.Precio,
                    Subtotal = prod.Precio * cantidad
                };
                _detalleRepo.Insert(detalle);
                _productoRepo.ActualizarStock(prod.Id, prod.Stock - cantidad);
            }

            MessageBox.Show($"Venta registrada exitosamente.\nTotal: {total:C2}", "Venta Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
