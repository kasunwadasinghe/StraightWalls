using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmSignup : Form
    {
        public frmSignup()
        {
            InitializeComponent();
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            frmHome frmHome = new frmHome();
            frmHome.Show();
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            var empId = int.Parse(cbEmployee.SelectedValue.ToString());
            var userName = txtUserName.Text.Trim();
            var password = txtPassword.Text.Trim();
            var RePassword = txtConfirm_Password.Text.Trim();
            var role = cbRole.SelectedItem.ToString();

            if (userName.Length < 0)
            {
                MessageBox.Show("Please enter user name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else if(password.Length < 0)
            {
                MessageBox.Show("Please enter password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (RePassword != password)
            {
                MessageBox.Show("Password and confirm password is not matched", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (StraightWallsEntities context = new StraightWallsEntities())
                {
                    var user = context.Users.Where(w => w.employee_id == empId).FirstOrDefault();
                    if (user != null)
                    {
                        user.user_name = userName;
                        user.role = role;
                        user.password = password;
                        context.SaveChanges();
                    }
                    else
                    {
                        user = new User
                        {
                            employee_id = empId,
                            role = role,
                            password = password,
                            created_on = DateTime.Now,
                            user_name = userName
                        };

                        context.Users.Add(user);
                        context.SaveChanges();
                        MessageBox.Show("Save Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Fail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Clear();
            }
        }

        private void Clear()
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtConfirm_Password.Clear();
            cbDepartment.SelectedIndex = 0;
            cbRole.SelectedIndex = 0;
        }

        private void frmSignup_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'straightWallsDataSet.Department' table. You can move, or remove it, as needed.
            this.departmentTableAdapter.Fill(this.straightWallsDataSet.Department);
            cbDepartment.SelectedIndex = 0;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var departmentId = 0;
            var i = int.TryParse(cbDepartment.SelectedValue.ToString(),out departmentId);
            this.employeeTableAdapter.FillByDepartmentId(this.straightWallsDataSet.Employee,departmentId);
        }

        private void cbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            var employeeId =0;
            var i = int.TryParse(cbEmployee.SelectedValue.ToString(),out employeeId);
            using (StraightWallsEntities context = new StraightWallsEntities())
            {
                var user = context.Users.Where(w => w.employee_id == employeeId).FirstOrDefault();
                if (user != null)
                {
                    txtUserName.Text = user.user_name;
                }
                else
                {
                    txtUserName.Clear();
                }
            }
        }
    }
}
