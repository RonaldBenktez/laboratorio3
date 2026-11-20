using laboratorio3.Data.Repositories;
using laboratorio3.Models;

namespace laboratorio3.Forms
{
    public class ReporteStockForm : UserControl
    {
        private DataGridView dgvStock = null!;
        private ComboBox cmbFiltroCategoria = null!;
        private TextBox txtBuscar = null!;
        private Label lblResumen = null!;

        private readonly ProductoRepository _repo = new();
        private List<Producto> _productos = new();

        public ReporteStockForm()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(30, 30, 46);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(49, 50, 68) };

            var lblTitulo = new Label
            {
                Text = "Reporte de Stock / Inventario",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                AutoSize = true,
                Location = new Point(12, 14)
            };
            pnlHeader.Controls.Add(lblTitulo);

            var lblCat = new Label { Text = "Categoría:", ForeColor = Color.White, AutoSize = true, Location = new Point(290, 18), Font = new Font("Segoe UI", 9) };
            pnlHeader.Controls.Add(lblCat);

            cmbFiltroCategoria = new ComboBox
            {
                Location = new Point(355, 15),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };
            cmbFiltroCategoria.SelectedIndexChanged += (s, e) => Filtrar();
            pnlHeader.Controls.Add(cmbFiltroCategoria);

            txtBuscar = new TextBox
            {
                PlaceholderText = "🔍 Buscar...",
                Location = new Point(515, 15),
                Size = new Size(150, 28),
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9)
            };
            txtBuscar.TextChanged += (s, e) => Filtrar();
            pnlHeader.Controls.Add(txtBuscar);

            Controls.Add(pnlHeader);

            var pnlLeyenda = new Panel { Dock = DockStyle.Bottom, Height = 56, BackColor = Color.FromArgb(49, 50, 68), Padding = new Padding(10, 6, 0, 0) };

            lblResumen = new Label
            {
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                Location = new Point(12, 6)
            };
            pnlLeyenda.Controls.Add(lblResumen);

            var lblLeyenda = new Label
            {
                Text = "  Crítico (≤5)      Bajo (≤15)      Normal",
                Location = new Point(12, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 8)
            };

            var picRojo = new Panel { Location = new Point(12, 32), Size = new Size(14, 14), BackColor = Color.FromArgb(80, 30, 30) };
            var picAmbar = new Panel { Location = new Point(85, 32), Size = new Size(14, 14), BackColor = Color.FromArgb(70, 55, 20) };
            var picVerde = new Panel { Location = new Point(155, 32), Size = new Size(14, 14), BackColor = Color.FromArgb(30, 50, 30) };

            pnlLeyenda.Controls.Add(picRojo);
            pnlLeyenda.Controls.Add(picAmbar);
            pnlLeyenda.Controls.Add(picVerde);
            pnlLeyenda.Controls.Add(new Label { Text = "Crítico (≤5)", ForeColor = Color.FromArgb(243, 139, 168), AutoSize = true, Location = new Point(28, 32), Font = new Font("Segoe UI", 8) });
            pnlLeyenda.Controls.Add(new Label { Text = "Bajo (≤15)", ForeColor = Color.FromArgb(250, 179, 135), AutoSize = true, Location = new Point(101, 32), Font = new Font("Segoe UI", 8) });
            pnlLeyenda.Controls.Add(new Label { Text = "Normal", ForeColor = Color.FromArgb(166, 227, 161), AutoSize = true, Location = new Point(171, 32), Font = new Font("Segoe UI", 8) });

            Controls.Add(pnlLeyenda);

            dgvStock = new DataGridView
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
            dgvStock.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(49, 50, 68);
            dgvStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(137, 180, 250);
            dgvStock.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvStock.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
            dgvStock.DefaultCellStyle.ForeColor = Color.White;
            dgvStock.DefaultCellStyle.SelectionBackColor = Color.FromArgb(69, 71, 90);
            dgvStock.RowTemplate.Height = 28;
            dgvStock.CellFormatting += Dgv_CellFormatting;
            Controls.Add(dgvStock);
        }

        private void CargarDatos()
        {
            _productos = _repo.GetActivos().ToList();

            var categorias = new List<string> { "Todas" };
            categorias.AddRange(_productos.Select(p => p.Categoria).Distinct().OrderBy(c => c));
            cmbFiltroCategoria.DataSource = categorias;

            Filtrar();
        }

        private void Filtrar()
        {
            var lista = _productos.AsEnumerable();

            string cat = cmbFiltroCategoria.SelectedItem?.ToString() ?? "Todas";
            if (cat != "Todas")
                lista = lista.Where(p => p.Categoria == cat);

            string q = txtBuscar.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(q))
                lista = lista.Where(p => p.Nombre.ToLower().Contains(q));

            var resultado = lista.OrderBy(p => p.Stock).ToList();

            dgvStock.DataSource = resultado.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Categoria,
                Precio = p.Precio.ToString("C2"),
                p.Stock,
                Alerta = p.Stock <= 5 ? "🔴 CRÍTICO" : p.Stock <= 15 ? "🟡 Bajo" : "🟢 Normal"
            }).ToList();

            if (dgvStock.Columns.Contains("Id"))
                dgvStock.Columns["Id"].Visible = false;

            int critico = resultado.Count(p => p.Stock <= 5);
            int bajo = resultado.Count(p => p.Stock > 5 && p.Stock <= 15);
            lblResumen.Text = $"Total productos: {resultado.Count}   |   Críticos (≤5): {critico}   |   Bajo stock (≤15): {bajo}";
        }

        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvStock.Columns.Contains("Stock")) return;
            var row = dgvStock.Rows[e.RowIndex];
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
    }
}
