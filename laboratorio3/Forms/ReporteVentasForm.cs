using laboratorio3.Data.Repositories;

namespace laboratorio3.Forms
{
    public class ReporteVentasForm : UserControl
    {
        private DataGridView dgvReporte = null!;
        private Label lblResumen = null!;
        private Button btnActualizar = null!;

        private readonly VentaRepository _ventaRepo = new();

        public ReporteVentasForm()
        {
            InitializeComponent();
            CargarReporte();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(30, 30, 46);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(49, 50, 68) };

            var lblTitulo = new Label
            {
                Text = "Reporte de Productos Vendidos",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                AutoSize = true,
                Location = new Point(12, 14)
            };
            pnlHeader.Controls.Add(lblTitulo);

            btnActualizar = new Button
            {
                Text = "↻ Actualizar",
                Location = new Point(530, 13),
                Size = new Size(110, 28),
                BackColor = Color.FromArgb(137, 180, 250),
                ForeColor = Color.FromArgb(30, 30, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += (s, e) => CargarReporte();
            pnlHeader.Controls.Add(btnActualizar);

            Controls.Add(pnlHeader);

            var pnlResumen = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.FromArgb(49, 50, 68) };
            lblResumen = new Label
            {
                ForeColor = Color.FromArgb(166, 227, 161),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(12, 10)
            };
            pnlResumen.Controls.Add(lblResumen);
            Controls.Add(pnlResumen);

            dgvReporte = new DataGridView
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
            dgvReporte.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(49, 50, 68);
            dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(137, 180, 250);
            dgvReporte.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvReporte.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
            dgvReporte.DefaultCellStyle.ForeColor = Color.White;
            dgvReporte.DefaultCellStyle.SelectionBackColor = Color.FromArgb(69, 71, 90);
            dgvReporte.RowTemplate.Height = 28;
            Controls.Add(dgvReporte);
        }

        private void CargarReporte()
        {
            var datos = _ventaRepo.GetReporteVentas().ToList();

            dgvReporte.DataSource = datos.Select(d => new
            {
                Producto = (string)d.Producto,
                Categoria = (string)d.Categoria,
                TotalVendido = (int)d.TotalVendido,
                MontoTotal = ((decimal)d.MontoTotal).ToString("C2"),
                UltimaVenta = ((DateTime)d.UltimaVenta).ToString("dd/MM/yyyy")
            }).ToList();

            decimal totalGeneral = 0;
            int unidadesTotal = 0;
            foreach (var d in datos)
            {
                totalGeneral += (decimal)d.MontoTotal;
                unidadesTotal += (int)d.TotalVendido;
            }

            lblResumen.Text = $"Total de ventas: {totalGeneral:C2}   |   Unidades vendidas: {unidadesTotal}   |   Productos distintos: {datos.Count}";
        }
    }
}
