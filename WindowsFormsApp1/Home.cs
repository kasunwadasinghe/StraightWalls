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
    public partial class frmHome : Form
    {
        public static int empId = 0;
        public frmHome()
        {
            InitializeComponent();
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            ClearContainer();
            frmSignup signup = new frmSignup();
            signup.TopLevel = false;
            pnlFormContainer.Controls.Add(signup);
            signup.Show();
        }

        private void ClearContainer()
        {
            pnlFormContainer.Controls.Clear();
        }

        private void btnEmployeeManagement_Click(object sender, EventArgs e)
        {
            ClearContainer();
            frmEmployeeRegistration employee = new frmEmployeeRegistration();
            employee.TopLevel = false;
            pnlFormContainer.Controls.Add(employee);
            employee.Show();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
            this.Dispose();
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnHolidayManagement_Click(object sender, EventArgs e)
        {
            ClearContainer();
            frmHolidayManagement holiday = new frmHolidayManagement();
            holiday.TopLevel = false;
            pnlFormContainer.Controls.Add(holiday);
            holiday.Show();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            using(StraightWallsEntities context =new StraightWallsEntities())
            {
                var user = context.Users.Where(w => w.employee_id == empId).FirstOrDefault();
                if (user != null)
                {
                }
            }
        }
    }
}
