using laboratorio3.Data.Repositories;
using laboratorio3.Helpers;

namespace laboratorio3.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblError = null!;

        private readonly UsuarioRepository _usuarioRepo = new();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Tienda Electrónica - Iniciar Sesión";
            Size = new Size(420, 320);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(30, 30, 46);

            var pnlCard = new Panel
            {
                Size = new Size(360, 240),
                Location = new Point(30, 30),
                BackColor = Color.FromArgb(49, 50, 68),
                Padding = new Padding(20)
            };
            Controls.Add(pnlCard);

            var lblTitle = new Label
            {
                Text = "INICIAR SESIÓN",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(137, 180, 250),
                AutoSize = true,
                Location = new Point(100, 15)
            };
            pnlCard.Controls.Add(lblTitle);

            var lblEmailLbl = new Label { Text = "Email:", ForeColor = Color.White, AutoSize = true, Location = new Point(20, 60), Font = new Font("Segoe UI", 9) };
            pnlCard.Controls.Add(lblEmailLbl);

            txtEmail = new TextBox
            {
                Location = new Point(20, 78),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlCard.Controls.Add(txtEmail);

            var lblPassLbl = new Label { Text = "Contraseña:", ForeColor = Color.White, AutoSize = true, Location = new Point(20, 115), Font = new Font("Segoe UI", 9) };
            pnlCard.Controls.Add(lblPassLbl);

            txtPassword = new TextBox
            {
                Location = new Point(20, 133),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(30, 30, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlCard.Controls.Add(txtPassword);

            lblError = new Label
            {
                ForeColor = Color.FromArgb(243, 139, 168),
                AutoSize = true,
                Location = new Point(20, 170),
                Font = new Font("Segoe UI", 8),
                Text = ""
            };
            pnlCard.Controls.Add(lblError);

            btnLogin = new Button
            {
                Text = "Ingresar",
                Location = new Point(110, 190),
                Size = new Size(140, 36),
                BackColor = Color.FromArgb(137, 180, 250),
                ForeColor = Color.FromArgb(30, 30, 46),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            pnlCard.Controls.Add(btnLogin);

            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            string email = txtEmail.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                lblError.Text = "Complete todos los campos.";
                return;
            }

            string hash = CryptoHelper.ComputeSha256(pass);

            try
            {
                var usuario = _usuarioRepo.GetByEmailAndHash(email, hash);
                if (usuario == null)
                {
                    lblError.Text = "Credenciales incorrectas.";
                    txtPassword.Clear();
                    return;
                }

                SessionHelper.UsuarioActual = usuario;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error de conexión: {ex.Message}";
            }
        }
    }
}
