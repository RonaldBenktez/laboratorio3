using laboratorio3.Data.Repositories;
using laboratorio3.Models;

namespace laboratorio3.Forms
{
    public class VentasForm : UserControl
    {
        private DataGridView dgvVentas = null!;
        private DateTimePicker dtpDesde = null!;
        private DateTimePicker dtpHasta = null!;
        private Button btnFiltrar = null!;

        private readonly VentaRepository _ventaRepo = new();

        public VentasForm()
        {
            InitializeComponent();
            CargarVentas();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(30, 30, 46);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(49, 50, 68) };

            var lblTitulo = new Label
            {
                Text = "Historial de Ventas",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                AutoSize = true,
                Location = new Point(12, 14)
            };
            pnlHeader.Controls.Add(lblTitulo);

            var lblDesde = new Label { Text = "Desde:", ForeColor = Color.White, AutoSize = true, Location = new Point(230, 18), Font = new Font("Segoe UI", 9) };
            pnlHeader.Controls.Add(lblDesde);

            dtpDesde = new DateTimePicker { Location = new Point(275, 15), Size = new Size(130, 28), Format = DateTimePickerFormat.Short };
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            pnlHeader.Controls.Add(dtpDesde);

            var lblHasta = new Label { Text = "Hasta:", ForeColor = Color.White, AutoSize = true, Location = new Point(415, 18), Font = new Font("Segoe UI", 9) };
            pnlHeader.Controls.Add(lblHasta);

            dtpHasta = new DateTimePicker { Location = new Point(460, 15), Size = new Size(130, 28), Format = DateTimePickerFormat.Short };
            dtpHasta.Value = DateTime.Today;
            pnlHeader.Controls.Add(dtpHasta);

            btnFiltrar = new Button
            {
                Text = "Filtrar",
                Location = new Point(600, 13),
                Size = new Size(80, 28),
                BackColor = Color.FromArgb(137, 180, 250),
                ForeColor = Color.FromArgb(30, 30, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.Click += (s, e) => CargarVentas();
            pnlHeader.Controls.Add(btnFiltrar);

            Controls.Add(pnlHeader);

            dgvVentas = new DataGridView
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
            dgvVentas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(49, 50, 68);
            dgvVentas.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(137, 180, 250);
            dgvVentas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvVentas.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
            dgvVentas.DefaultCellStyle.ForeColor = Color.White;
            dgvVentas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(69, 71, 90);
            dgvVentas.RowTemplate.Height = 28;
            Controls.Add(dgvVentas);
        }

        private void CargarVentas()
        {
            var ventas = _ventaRepo.GetByFecha(dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));
            dgvVentas.DataSource = ventas.Select(v => new
            {
                v.Id,
                Fecha = v.Fecha.ToString("dd/MM/yyyy HH:mm"),
                v.Estado,
                Total = v.Total.ToString("C2")
            }).ToList();

            if (dgvVentas.Columns.Contains("Id"))
                dgvVentas.Columns["Id"].Visible = false;
        }
    }
}
