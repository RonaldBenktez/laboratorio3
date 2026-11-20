using laboratorio3.Data.Repositories;
using laboratorio3.Models;

namespace laboratorio3.Forms
{
    public class ProductoForm : Form
    {
        private TextBox txtNombre = null!;
        private TextBox txtDescripcion = null!;
        private TextBox txtCategoria = null!;
        private NumericUpDown nudPrecio = null!;
        private NumericUpDown nudStock = null!;
        private Button btnGuardar = null!;

        private readonly ProductoRepository _repo = new();
        private readonly Producto? _producto;

        public ProductoForm(Producto? producto = null)
        {
            _producto = producto;
            InitializeComponent();
            if (_producto != null) CargarDatos();
        }

        private void InitializeComponent()
        {
            Text = _producto == null ? "Nuevo Producto" : "Editar Producto";
            Size = new Size(420, 390);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(49, 50, 68);

            int y = 20;
            AddLabel("Nombre:", 20, ref y);
            txtNombre = AddTextBox(20, ref y);

            AddLabel("Descripción:", 20, ref y);
            txtDescripcion = AddTextBox(20, ref y);

            AddLabel("Categoría:", 20, ref y);
            txtCategoria = AddTextBox(20, ref y);

            AddLabel("Precio:", 20, ref y);
            nudPrecio = new NumericUpDown
            {
                Location = new Point(20, y),
                Size = new Size(360, 28),
                Minimum = 0,
                Maximum = 99999,
                DecimalPlaces = 2,
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            Controls.Add(nudPrecio);
            y += 36;

            AddLabel("Stock:", 20, ref y);
            nudStock = new NumericUpDown
            {
                Location = new Point(20, y),
                Size = new Size(360, 28),
                Minimum = 0,
                Maximum = 99999,
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            Controls.Add(nudStock);
            y += 46;

            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(130, y),
                Size = new Size(140, 36),
                BackColor = Color.FromArgb(137, 180, 250),
                ForeColor = Color.FromArgb(30, 30, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;
            Controls.Add(btnGuardar);
            AcceptButton = btnGuardar;
        }

        private void AddLabel(string text, int x, ref int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9)
            });
            y += 20;
        }

        private TextBox AddTextBox(int x, ref int y)
        {
            var tb = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(360, 28),
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };
            Controls.Add(tb);
            y += 36;
            return tb;
        }

        private void CargarDatos()
        {
            txtNombre.Text = _producto!.Nombre;
            txtDescripcion.Text = _producto.Descripcion;
            txtCategoria.Text = _producto.Categoria;
            nudPrecio.Value = _producto.Precio;
            nudStock.Value = _producto.Stock;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_producto == null)
            {
                var nuevo = new Producto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    Categoria = txtCategoria.Text.Trim(),
                    Precio = nudPrecio.Value,
                    Stock = (int)nudStock.Value
                };
                _repo.Insert(nuevo);
            }
            else
            {
                _producto.Nombre = txtNombre.Text.Trim();
                _producto.Descripcion = txtDescripcion.Text.Trim();
                _producto.Categoria = txtCategoria.Text.Trim();
                _producto.Precio = nudPrecio.Value;
                _producto.Stock = (int)nudStock.Value;
                _repo.Update(_producto);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
