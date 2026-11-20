using laboratorio3.Helpers;

namespace laboratorio3.Forms
{
    public class MainForm : Form
    {
        private Panel pnlSidebar = null!;
        private Panel pnlContent = null!;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Tienda Electrónica - Sistema de Ventas";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 580);
            BackColor = Color.FromArgb(30, 30, 46);

            BuildSidebar();
            BuildContent();
        }

        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(49, 50, 68)
            };
            Controls.Add(pnlSidebar);

            var lblApp = new Label
            {
                Text = "🛒 TiendaTech",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlSidebar.Controls.Add(lblApp);

            var lblUser = new Label
            {
                Text = $"👤 {SessionHelper.UsuarioActual?.Nombre ?? ""}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(166, 227, 161),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlSidebar.Controls.Add(lblUser);

            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(88, 91, 112) };
            pnlSidebar.Controls.Add(sep);

            AddNavButton("📦  Productos", () => ShowPanel(new ProductosForm()));
            AddNavButton("🛍️  Nueva Venta", () =>
            {
                using var dlg = new NuevaVentaForm();
                dlg.ShowDialog(this);
            });
            AddNavButton("📋  Historial de Ventas", () => ShowPanel(new VentasForm()));
            AddNavButton("📊  Reporte de Ventas", () => ShowPanel(new ReporteVentasForm()));
            AddNavButton("📦  Reporte de Stock", () => ShowPanel(new ReporteStockForm()));

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.FromArgb(49, 50, 68) };
            var btnSalir = new Button
            {
                Text = "⏻  Cerrar Sesión",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(243, 139, 168),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => { Application.Restart(); };
            pnlBottom.Controls.Add(btnSalir);
            pnlSidebar.Controls.Add(pnlBottom);
        }

        private void AddNavButton(string text, Action onClick)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 46,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(69, 71, 90);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.Transparent;
            btn.Click += (s, e) => onClick();
            pnlSidebar.Controls.Add(btn);
            btn.BringToFront();
        }

        private void BuildContent()
        {
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 46),
                Padding = new Padding(16)
            };
            Controls.Add(pnlContent);

            ShowPanel(new ProductosForm());
        }

        private void ShowPanel(UserControl panel)
        {
            pnlContent.Controls.Clear();
            panel.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(panel);
        }
    }
}
