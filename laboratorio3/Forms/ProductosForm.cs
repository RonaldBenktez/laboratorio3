using laboratorio3.Data.Repositories;
using laboratorio3.Models;

namespace laboratorio3.Forms
{
    public class ProductosForm : UserControl
    {
        private DataGridView dgvProductos = null!;
        private Button btnNuevo = null!;
        private Button btnEditar = null!;
        private TextBox txtBuscar = null!;

        private readonly ProductoRepository _repo = new();
        private List<Producto> _productos = new();

        public ProductosForm()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(30, 30, 46);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(49, 50, 68) };

            var lblTitulo = new Label
            {
                Text = "Gestión de Productos",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                AutoSize = true,
                Location = new Point(12, 14)
            };
            pnlHeader.Controls.Add(lblTitulo);

            txtBuscar = new TextBox
            {
                PlaceholderText = "🔍 Buscar producto...",
                Location = new Point(280, 15),
                Size = new Size(200, 28),
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9)
            };
            txtBuscar.TextChanged += (s, e) => FiltrarProductos();
            pnlHeader.Controls.Add(txtBuscar);

            btnNuevo = MakeButton("+ Nuevo", Color.FromArgb(166, 227, 161), Color.FromArgb(30, 30, 46));
            btnNuevo.Location = new Point(500, 13);
            btnNuevo.Click += BtnNuevo_Click;
            pnlHeader.Controls.Add(btnNuevo);

            btnEditar = MakeButton("✏ Editar", Color.FromArgb(250, 179, 135), Color.FromArgb(30, 30, 46));
            btnEditar.Location = new Point(600, 13);
            btnEditar.Click += BtnEditar_Click;
            pnlHeader.Controls.Add(btnEditar);

            Controls.Add(pnlHeader);

            dgvProductos = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(49, 50, 68);
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(137, 180, 250);
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvProductos.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
            dgvProductos.DefaultCellStyle.ForeColor = Color.White;
            dgvProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(69, 71, 90);
            dgvProductos.RowTemplate.Height = 28;
            dgvProductos.CellFormatting += Dgv_CellFormatting;
            Controls.Add(dgvProductos);
        }

        private Button MakeButton(string text, Color back, Color fore)
        {
            var b = new Button
            {
                Text = text,
                Size = new Size(90, 28),
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void CargarProductos()
        {
            _productos = _repo.GetActivos().ToList();
            MostrarProductos(_productos);
        }

        private void MostrarProductos(IEnumerable<Producto> lista)
        {
            dgvProductos.DataSource = lista.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Categoria,
                Precio = p.Precio.ToString("C2"),
                p.Stock,
                Estado = p.Stock <= 5 ? "⚠ Crítico" : p.Stock <= 15 ? "Bajo" : "Normal"
            }).ToList();

            if (dgvProductos.Columns.Contains("Id"))
                dgvProductos.Columns["Id"].Visible = false;
        }

        private void FiltrarProductos()
        {
            var q = txtBuscar.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
                MostrarProductos(_productos);
            else
                MostrarProductos(_productos.Where(p =>
                    p.Nombre.ToLower().Contains(q) || p.Categoria.ToLower().Contains(q)));
        }

        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvProductos.Columns.Count == 0) return;
            var row = dgvProductos.Rows[e.RowIndex];
            if (row.Cells["Stock"].Value is int stock)
            {
                if (stock <= 5)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(80, 30, 30);
                else if (stock <= 15)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(70, 55, 20);
                else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
            }
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            using var dlg = new ProductoForm();
            if (dlg.ShowDialog() == DialogResult.OK)
                CargarProductos();
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null) return;
            int id = (int)dgvProductos.CurrentRow.Cells["Id"].Value;
            var prod = _productos.First(p => p.Id == id);
            using var dlg = new ProductoForm(prod);
            if (dlg.ShowDialog() == DialogResult.OK)
                CargarProductos();
        }
    }
}
