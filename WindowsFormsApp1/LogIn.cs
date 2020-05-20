using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string UN = txtUser.Text;
            string PW = txtPassword.Text;

            StraightWallsEntities context = new StraightWallsEntities();
            var emp = context.Users.Where(w => w.user_name == UN && w.password == PW).Select(s => s.Employee).Where(w => w.is_active == true).FirstOrDefault();
            if (emp != null)
            {
                frmHome _dashboard = new frmHome();
                frmHome.empId = emp.employee_id;
                this.Hide();
                _dashboard.ShowDialog();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Your user name or passowrd is incorrect", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
