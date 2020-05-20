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
    public partial class frmHolidayManagement : Form
    {
        public frmHolidayManagement()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void LoadGrid()
        {

        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmHolidayManagement_Load(object sender, EventArgs e)
        {
            var Date1 = new DateTime(DateTime.Now.Year, 07, 15);
            var Date2 = new DateTime(DateTime.Now.Year, 08, 31);
            var Date3 = new DateTime(DateTime.Now.Year, 12, 15);
            var Date4 = new DateTime(DateTime.Now.Year, 12, 22);
            // TODO: This line of code loads data into the 'straightWallsDataSet.LeaveRejected' table. You can move, or remove it, as needed.
            this.leaveRejectedTableAdapter.Fill(this.straightWallsDataSet.LeaveRejected);
            // TODO: This line of code loads data into the 'straightWallsDataSet.HolidayManagementTA' table. You can move, or remove it, as needed.
            this.holidayManagementTableAdapter1.FillByPending(this.straightWallsDataSet.HolidayManagementTA, Date1, Date2, Date3, Date4);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            var chrismasVacationfrom = new DateTime(DateTime.Now.Year, 12, 23);
            var chrismasVacationto = new DateTime(DateTime.Now.Year + 1, 01, 03);


            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {

                var row = dataGridView1.Rows[e.RowIndex];
                var EmpId = int.Parse(row.Cells["gcEmpID"].Value.ToString());
                var from = DateTime.Parse(row.Cells["gcFrom"].Value.ToString());
                var designation = row.Cells["gcDesignation"].Value.ToString();
                var to = DateTime.Parse(row.Cells["gcTo"].Value.ToString());
                var Id = row.Cells["gcId"].Value.ToString();
                if (senderGrid.Columns[e.ColumnIndex].Name == "gcReject")
                {
                    changeApprovel(int.Parse(Id), false);
                }
                else
                {
                    using (StraightWallsEntities context = new StraightWallsEntities())
                    {
                        var emp = context.Employees.Where(w => w.employee_id == EmpId).FirstOrDefault();
                        var departmentEmployee = context.Employees.Where(w => w.department_id == emp.department_id && w.is_active == true).Count();
                        var LeaveInSamePeriod = context.HolidayManagements.Where(w => w.is_approved == true && w.is_canceled==false && w.from_date > from && w.to_date < w.to_date).Select(s => s.Employee).Where(w => w.department_id == emp.employee_id).Distinct().Count();
                        if (emp != null)
                        {
                            if (from < chrismasVacationfrom && to > chrismasVacationto)
                            {
                                changeApprovel(int.Parse(Id), true);
                            }
                            else if (from.Month == 8 && to.Month == 8)
                            {
                                if (((LeaveInSamePeriod + 1) / departmentEmployee) * 100 >= 40)
                                {
                                    if(designation == "Head" )
                                    {
                                        var isdeputyonleave = context.HolidayManagements
                                            .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                            .Select(s => s.Employee)
                                            .Where(w => w.department_id == emp.department_id)
                                            .Select(s => s.Designation).Where(w => w.description == "Deputy Head").Distinct().Count();

                                        if(isdeputyonleave > 0)
                                        {
                                            changeApprovel(int.Parse(Id), true);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Either the head or the deputy head of the department must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else if(designation == "Deputy Head")
                                    {
                                        var isheadonleave = context.HolidayManagements
                                            .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                            .Select(s => s.Employee)
                                            .Where(w => w.department_id == emp.department_id)
                                            .Select(s => s.Designation).Where(w => w.description == "Head").Distinct().Count();

                                        if (isheadonleave > 0)
                                        {
                                            changeApprovel(int.Parse(Id), true);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Either the head or the deputy head of the department must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else if (designation == "Manager")
                                    {
                                        var managersonleave = context.HolidayManagements
                                            .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                            .Select(s => s.Employee)
                                            .Where(w => w.department_id == emp.department_id)
                                            .Select(s => s.Designation).Where(w => w.description == "Manager").Distinct().Count();
                                        var totalmanager = context.Employees
                                            .Where(w => w.department_id == emp.department_id)
                                            .Select(s => s.Designation).Where(w => w.description == "Manager").Distinct().Count();

                                        if (managersonleave < totalmanager)
                                        {
                                            changeApprovel(int.Parse(Id), true);
                                        }
                                        else
                                        {
                                            MessageBox.Show("At least one manager must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else if (designation == "Senior member")
                                    {

                                        var seneiormembersonleave = context.HolidayManagements
                                            .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                            .Select(s => s.Employee)
                                            .Where(w => w.department_id == emp.department_id)
                                            .Select(s => s.Designation).Where(w => w.description == "Senior member").Distinct().Count();
                                        var totalseniormembers = context.Employees
                                            .Where(w => w.department_id == emp.department_id)
                                            .Select(s => s.Designation).Where(w => w.description == "Senior member").Distinct().Count();

                                        if (seneiormembersonleave < totalseniormembers)
                                        {
                                            changeApprovel(int.Parse(Id), true);
                                        }
                                        else
                                        {
                                            MessageBox.Show("At least one senior staff member  must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else
                                    {
                                        changeApprovel(int.Parse(Id), true);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("On duty staff is bellow than 40%", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else if (((LeaveInSamePeriod + 1) / departmentEmployee) * 100 >= 60)
                            {
                                if (designation == "Head")
                                {
                                    var isdeputyonleave = context.HolidayManagements
                                        .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                        .Select(s => s.Employee)
                                        .Where(w => w.department_id == emp.department_id)
                                        .Select(s => s.Designation).Where(w => w.description == "Deputy Head").Distinct().Count();

                                    if (isdeputyonleave > 0)
                                    {
                                        changeApprovel(int.Parse(Id), true);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Either the head or the deputy head of the department must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else if (designation == "Deputy Head")
                                {
                                    var isheadonleave = context.HolidayManagements
                                        .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                        .Select(s => s.Employee)
                                        .Where(w => w.department_id == emp.department_id)
                                        .Select(s => s.Designation).Where(w => w.description == "Head").Distinct().Count();

                                    if (isheadonleave > 0)
                                    {
                                        changeApprovel(int.Parse(Id), true);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Either the head or the deputy head of the department must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else if (designation == "Manager")
                                {
                                    var managersonleave = context.HolidayManagements
                                        .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                        .Select(s => s.Employee)
                                        .Where(w => w.department_id == emp.department_id)
                                        .Select(s => s.Designation).Where(w => w.description == "Manager").Distinct().Count();
                                    var totalmanager = context.Employees
                                        .Where(w => w.department_id == emp.department_id)
                                        .Select(s => s.Designation).Where(w => w.description == "Manager").Distinct().Count();

                                    if (managersonleave < totalmanager)
                                    {
                                        changeApprovel(int.Parse(Id), true);
                                    }
                                    else
                                    {
                                        MessageBox.Show("At least one manager must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else if (designation == "Senior member")
                                {

                                    var seneiormembersonleave = context.HolidayManagements
                                        .Where(w => w.is_approved == true && w.is_canceled == false && w.from_date > from && w.to_date < w.to_date)
                                        .Select(s => s.Employee)
                                        .Where(w => w.department_id == emp.department_id)
                                        .Select(s => s.Designation).Where(w => w.description == "Senior member").Distinct().Count();
                                    var totalseniormembers = context.Employees
                                        .Where(w => w.department_id == emp.department_id)
                                        .Select(s => s.Designation).Where(w => w.description == "Senior member").Distinct().Count();

                                    if (seneiormembersonleave < totalseniormembers)
                                    {
                                        changeApprovel(int.Parse(Id), true);
                                    }
                                    else
                                    {
                                        MessageBox.Show("At least one senior staff member  must be on duty", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    changeApprovel(int.Parse(Id), true);
                                }
                            }
                            else
                            {
                                MessageBox.Show("On duty staff is bellow than 40%", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                    }
                }

            }
        }

        private void changeApprovel(int holidayId, bool isApprove)
        {
            using (StraightWallsEntities context = new StraightWallsEntities())
            {
                try
                {
                    var holiday = context.HolidayManagements.Where(w => w.id == holidayId).FirstOrDefault();
                    if (holiday != null)
                    {
                        holiday.is_approved = isApprove;
                        holiday.updated_by = frmHome.empId.ToString();
                        holiday.updated_on = DateTime.Now;

                        context.SaveChanges();
                        if (isApprove)
                            MessageBox.Show("Approve Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Reject Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Approve Fail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
