using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zuaha
{
    public partial class FrmEmployes : Form
    {
        zuhahaEntities db = new zuhahaEntities();
        int posistion;
        bool edit = true;
        public FrmEmployes()
        {
            InitializeComponent();
            dgvEmployes.AutoGenerateColumns = false;
            ReadClassified();
            DisplayEmployes();
        }
        private void ReadClassified()
        {
            //cbClassifi.DataSource = db.tbl_Classifed.ToList();
            //cbClassifi.DisplayMember = "classifed_name";
            //cbClassifi.ValueMember = "classifed_id";
            cbClassifi.DataSource = db.tbl_Classifed.ToList();
            cbClassifi.DisplayMember = "classifed_name";
            cbClassifi.ValueMember = "classifed_id";
        }

        public void DisplayEmployes()
        {
            //var data =  ;
            dgvEmployes.DataSource = (from e in db.tbl_Employees
                                     join cls in db.tbl_Classifed
                                     on e.classifed_id equals cls.classifed_id
                                     select new
                                     {
                                         employes_id = e.employes_id,
                                         employes_name = e.employes_name,
                                         employes_phone = e.employes_phone,
                                         employes_address = e.employes_address,
                                         email = e.email,
                                         classifed_id = cls.classifed_name,
                                     }).ToList();
            //dgvEmployes.DataSource = db.tbl_Employees.ToList();
            DisplayEmployesDetail();
        }

        private void DisplayEmployesDetail()
        {
            if (dgvEmployes.CurrentRow != null)
            {
                DataGridViewRow row = dgvEmployes.CurrentRow;
                txtId.Text = row.Cells[0].Value.ToString();
                var data_employes = db.tbl_Employees.Find(txtId.Text);
                if (data_employes != null)
                {
                    txtName.Text = data_employes.employes_name;
                    txtPhone.Text = data_employes.employes_phone;
                    txtAddress.Text = data_employes.employes_address;
                    txtEmail.Text = data_employes.email;
                    txtPassword.Text = data_employes.password;
                    dtBrithday.Value = (DateTime)data_employes.brithday;
                    cbClassifi.SelectedValue = data_employes.classifed_id;
                    rdGender.Checked = (bool)data_employes.gender;
                    ckStatus.Checked = (bool)data_employes.status_Employes;
                }


                //posistion = dgvProduct.CurrentRow.Index;
                edit = true;
                txtId.ReadOnly = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClassified_Click(object sender, EventArgs e)
        {
            FrmClassified frmClassified = new FrmClassified();
            frmClassified.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtId.Text = txtName.Text = txtPhone.Text = txtAddress.Text = txtAddress.Text = txtEmail.Text = txtPassword.Text = "";
            rdGender.Checked = false;
            txtId.Focus();
            edit = false;
            txtId.ReadOnly = false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                String empId = txtId.Text;
                var employes = db.tbl_Employees.Find(empId);
                if (employes != null)
                {
                    employes.employes_name = txtName.Text;
                    employes.employes_phone = txtPhone.Text;
                    employes.employes_address = txtAddress.Text;
                    employes.email = txtEmail.Text;
                    employes.password = txtPassword.Text;
                    employes.brithday = (dtBrithday.Value);
                    employes.gender = rdGender.Checked;
                    employes.status_Employes = ckStatus.Checked;
                    employes.classifed_id = cbClassifi.SelectedValue.ToString();
                    db.SaveChanges();
                    DisplayEmployes();

                }
                else
                {
                    MessageBox.Show("Mã nhân viên không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!ExitEmployesId(txtId.Text))
                {
                    tbl_Employees emp = new tbl_Employees();
                    emp.employes_id = txtId.Text;
                    emp.employes_name = txtName.Text;
                    emp.employes_address = txtAddress.Text;
                    emp.employes_phone = txtPhone.Text;
                    emp.email = txtEmail.Text;
                    emp.password = txtPassword.Text;
                    emp.brithday = dtBrithday.Value;
                    emp.gender = rdGender.Checked;
                    emp.classifed_id = cbClassifi.SelectedValue.ToString();
                    emp.status_Employes = ckStatus.Checked;


                    db.tbl_Employees.Add(emp);
                    db.SaveChanges();
                    DisplayEmployes();
                }
                else
                {
                    MessageBox.Show("Trùng mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ExitEmployesId(String empID)
        {
            var e = db.tbl_Employees.SingleOrDefault(x => x.employes_id == empID);
            return (e != null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployes.CurrentRow != null)
            {
                if (MessageBox.Show("Bạn có muốn xóa không  ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var emp = db.tbl_Employees.FirstOrDefault(x => x.employes_id == txtId.Text);
                    if (emp != null)
                    {
                        db.tbl_Employees.Remove(emp);
                        db.SaveChanges();
                        DisplayEmployes();
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FrmSearchEmployes frmSearchEmployes = new FrmSearchEmployes();
            frmSearchEmployes.ShowDialog();
        }

        private void tsDetail_Click(object sender, EventArgs e)
        {
            var employes_detail = db.tbl_Employees.FirstOrDefault(x => x.employes_id == txtId.Text);
            if (employes_detail != null)
            {
                MessageBox.Show(" Tên nhân viên :  " + employes_detail.employes_name + "/ Ngày sinh :  " + employes_detail.gender + "/ Email :  " + employes_detail.email + " / Giá xuất : " + employes_detail.password + " / Trạng thái  : " + employes_detail.status_Employes , "Chi tiết Nhân viên", MessageBoxButtons.OK);
            }
        }

        private void dgvEmployes_Click(object sender, EventArgs e)
        {
            DisplayEmployesDetail();
        }
    }
}
