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
    public partial class FrmClassified : Form
    {
        zuhahaEntities db = new zuhahaEntities();
        bool edit = true;
        public FrmClassified()
        {
            InitializeComponent();
            dgvClassified.AutoGenerateColumns = false;
            DisplayClassified();
        }
        private void DisplayClassified()
        {
            var data = from c in db.tbl_Classifed
                       select new
                       {
                           classifed_id = c.classifed_id,
                           classifed_name = c.classifed_name,
                           status_classifed = (bool)c.status_classifed ? "Hiện" : "Ẩn"
                       };
            dgvClassified.DataSource = data.ToList();
            DisplayClassifiedDetail();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtId.Text = txtName.Text = "";
            txtId.Focus();
            edit = false;
            txtId.ReadOnly = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                String class_id = txtId.Text;
                var classifies = db.tbl_Classifed.Find(class_id);
                if (classifies != null)
                {
                    classifies.classifed_name = txtName.Text;
                    classifies.status_classifed = ckStatus.Checked;
                    db.SaveChanges();
                    DisplayClassified();
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (!ExitClassifiedId(txtId.Text))
            {
                var c = new tbl_Classifed();
                c.classifed_id = txtId.Text;
                c.classifed_name = txtName.Text;
                c.status_classifed = ckStatus.Checked;
                db.tbl_Classifed.Add(c);
                db.SaveChanges();
                DisplayClassified();
            }
            else
            {
                MessageBox.Show("Trùng mã danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ExitClassifiedId(string classifies_Id)
        {
            var p = db.tbl_Classifed.SingleOrDefault(x => x.classifed_id == classifies_Id);
            return (p != null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvClassified.CurrentRow != null)
            {
                if (MessageBox.Show("Bạn có muốn xóa không  ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var cl = db.tbl_Classifed.FirstOrDefault(x => x.classifed_id == txtId.Text);
                    if (cl != null)
                    {
                        db.tbl_Classifed.Remove(cl);
                        db.SaveChanges();
                        DisplayClassified();
                    }
                }
            }
        }
        public void DisplayClassifiedDetail()
        {
            if (dgvClassified.CurrentRow != null)
            {
                DataGridViewRow row = dgvClassified.CurrentRow;
                txtId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                ckStatus.Checked = (bool)row.Cells[2].Value;
                txtId.ReadOnly = true;
                edit = true;
            }
        }
        private void dgvClassified_Click(object sender, EventArgs e)
        {
            DisplayClassifiedDetail();
        }
    }
}
