using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class frmEmployeeRegistration : Form
    {
        public frmEmployeeRegistration()
        {
            InitializeComponent();
            setEmpId();
        }

        private void setEmpId()
        {
            using (StraightWallsEntities context = new StraightWallsEntities())
            {
                try
                {
                    var emp_id = context.Employees.Max(m => m.employee_id);
                    emp_id++;
                    txtEmpId.Text = emp_id.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            frmHome Q = new frmHome();
            Q.Show();
        }

        private void clear()
        {
            txtEmpId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtContactNo.Clear();
            txtEmail.Clear();
            cbDepartment.SelectedIndex = 0;
            cbDesignation.SelectedIndex = 0;
            cbDesignation.SelectedIndex = 0;
            dtJoinDate.Text = DateTime.Now.ToString();
            setEmpId();
            this.employeeTableAdapter.Fill(this.straightWallsDataSet.Employee);
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmEmployeeRegistration_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'straightWallsDataSet.Designation' table. You can move, or remove it, as needed.
            this.designationTableAdapter.Fill(this.straightWallsDataSet.Designation);
            // TODO: This line of code loads data into the 'straightWallsDataSet.Department' table. You can move, or remove it, as needed.
            this.departmentTableAdapter.Fill(this.straightWallsDataSet.Department);
            // TODO: This line of code loads data into the 'straightWallsDataSet.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.straightWallsDataSet.Employee);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var emp_id = int.Parse(txtEmpId.Text.Trim());
            var name = txtName.Text.Trim();
            var address = txtAddress.Text.Trim();
            var contact = txtContactNo.Text.Trim();
            var email = txtEmail.Text.Trim();
            var department = int.Parse(cbDepartment.SelectedValue.ToString());
            var designation =int.Parse(cbDesignation.SelectedValue.ToString());
            var isActive = cbStatus.SelectedItem.ToString();
            var joinDate = dtJoinDate.Text;

            if(string.IsNullOrEmpty(name.Trim()))
            {
                MessageBox.Show("Please enter name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if(string.IsNullOrEmpty(address.Trim()))
            {
                MessageBox.Show("Please enter address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else if (string.IsNullOrEmpty(contact.Trim()))
            {
                MessageBox.Show("Please enter contact no", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(email.Trim()))
            {
                MessageBox.Show("Please enter email address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (StraightWallsEntities context = new StraightWallsEntities())
            {
                try
                {
                    var emp = context.Employees.Where(employee => employee.employee_id == emp_id).FirstOrDefault();
                    if (emp == null)
                    {
                        emp = new Employee()
                        {
                            employee_id = emp_id,
                            name = name,
                            address = address,
                            mobile = contact,
                            email = email,
                            department_id = department,
                            designation_id = designation,
                            is_active = isActive == "ACTIVE" ? true : false,
                            join_date = DateTime.Parse(joinDate)
                        };
                        context.Employees.Add(emp);
                        context.SaveChanges();
                        MessageBox.Show("Save Sucessful !", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        emp.name = name;
                        emp.address = address;
                        emp.mobile = contact;
                        emp.email = email;
                        emp.department_id = department;
                        emp.designation_id = designation;
                        emp.is_active = isActive == "ACTIVE" ? true : false;
                        emp.join_date = DateTime.Parse(joinDate);
                        context.SaveChanges();
                        MessageBox.Show("Update Sucessful !", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save fail !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    clear();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string empName = txtSearch.Text.Trim().Length > 0 ? txtSearch.Text.Trim() : null;
            try
            {
                this.employeeTableAdapter.FillBy(this.straightWallsDataSet.Employee,empName);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex];

            txtEmpId.Text = row.Cells["gcEmp_id"].Value.ToString();
            txtName.Text = row.Cells["gcName"].Value.ToString();
            txtAddress.Text = row.Cells["gcAddress"].Value.ToString();
            txtContactNo.Text = row.Cells["gcContactNo"].Value.ToString();
            txtEmail.Text = row.Cells["gcEmail"].Value.ToString();
            cbDepartment.SelectedValue= row.Cells["gcDepartmentId"].Value.ToString();
            cbDesignation.SelectedValue = row.Cells["gcDesignationId"].Value.ToString();
            cbStatus.SelectedIndex = cbStatus.Items.IndexOf(bool.Parse(row.Cells["gcIsActive"].Value.ToString()) ? "ACTIVE" : "INACTIVE");
            dtJoinDate.Text = row.Cells["gcJoinDate"].Value.ToString();
        }

    }
}
