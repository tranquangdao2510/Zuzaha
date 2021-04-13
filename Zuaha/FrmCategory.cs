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
    public partial class FrmCategory : Form
    {
        zuhahaEntities db = new zuhahaEntities();
        int posistion;
        bool edit = true;
        public FrmCategory()
        {
            InitializeComponent();
            dgvCategory.AutoGenerateColumns = false;
            displayCategory();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmCategory_Load(object sender, EventArgs e)
        {
            displayCategory();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtId.Text = txtName.Text = "";


            txtId.Focus();
            edit = false;
            txtId.ReadOnly = false;
        }
        private void displayCategory()
        {
            var data = from c in db.tbl_Category
                       select new
                       {
                           cate_id = c.cate_id,
                           cate_name = c.cate_name,
                           cate_status = (bool)c.cate_status ? "Hiện" : "Ẩn"
                       };
            dgvCategory.DataSource = db.tbl_Category.ToList();
            displayCategoryDetail();


        }
        //private void ShowRowCurrent()
        //{
        //    if (dgvCategory.CurrentRow != null)
        //    {
        //        DataGridViewRow row = dgvCategory.CurrentRow;
        //        txtId.Text= row.Cells[0].Value.ToString() ;
        //        txtName.Text = row.Cells[1].Value.ToString();
        //        cbStatus.Checked =Convert.ToBoolean(row.Cells[2].Value.ToString());
        //        txtId.ReadOnly = true;
        //        edit = true;
        //    }
        //}
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                var cate = db.tbl_Category.FirstOrDefault(x => x.cate_id == txtId.Text);
                if (cate != null)
                {
                    cate.cate_name = txtName.Text;
                    cate.cate_status = cbStatus.Checked;
                    db.SaveChanges();
                    displayCategory();
                    //dgvCategory.Rows[0].Selected = false;
                    //dgvCategory.Rows[posistion].Selected = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!ExitCategoryId(txtId.Text))
                {
                    var cate = new tbl_Category();
                    cate.cate_id = txtId.Text;
                    cate.cate_name = txtName.Text;
                    cate.cate_status = cbStatus.Checked;
                    db.tbl_Category.Add(cate);
                    db.SaveChanges();
                    displayCategory();
                }
                else
                {
                    MessageBox.Show("Trùng mã danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void dgvCategory_Click(object sender, EventArgs e)
        {
            displayCategoryDetail();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategory.CurrentRow != null)
            {
                if (MessageBox.Show("Bạn có muốn xóa không  ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var cate = db.tbl_Category.FirstOrDefault(x => x.cate_id == txtId.Text);
                    if (cate != null)
                    {
                        db.tbl_Category.Remove(cate);
                        db.SaveChanges();
                        displayCategory();
                    }
                }
            }
        }
        private void displayCategoryDetail()
        {
            if (dgvCategory.CurrentRow != null)
            {
                DataGridViewRow row = dgvCategory.CurrentRow;
                txtId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                cbStatus.Checked = (bool)row.Cells[2].Value;
                posistion = dgvCategory.CurrentRow.Index;
                edit = true;
                txtId.ReadOnly = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //string search;
            //var sreach_name = from cate in db.tbl_Category
            //                  where cate.cate_name.Contains(txtName.Text)
            //                  select cate;
            var search_name = db.tbl_Category.Where(c => c.cate_name.Contains(txtSearch.Text));
            dgvCategory.DataSource = search_name.ToList();
            //displayCategory();
        }
        private bool ExitCategoryId(string categoryId)
        {
            var p = db.tbl_Category.SingleOrDefault(x => x.cate_id == categoryId);
            return (p != null);
        }
    }
}
