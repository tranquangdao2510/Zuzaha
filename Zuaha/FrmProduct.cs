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
    public partial class FrmProduct : Form
    {
        zuhahaEntities db = new zuhahaEntities();
        bool edit = true;
        int posistion;

        public FrmProduct()
        {
            InitializeComponent();
            dgvProduct.AutoGenerateColumns = false;
            ReadCategory();
            ReadCategorySearch();
            DisplayProduct();
        }


        private void ReadCategory()
        {
            cbCategory.DataSource = db.tbl_Category.ToList();
            cbCategory.DisplayMember = "cate_name";
            cbCategory.ValueMember = "cate_id";
        }
        private void ReadCategorySearch()
        {
            cbCategorySearch.DataSource = db.tbl_Category.ToList();
            cbCategorySearch.DisplayMember = "cate_name";
            cbCategorySearch.ValueMember = "cate_id";
        }

        private void DisplayProduct()
        {
            var data = from p in db.tbl_Product
                       join c in db.tbl_Category
                       on p.cate_id equals c.cate_id
                       select new
                       {
                           product_id = p.product_id,
                           product_name = p.product_name,
                           product_desc = p.product_desc,
                           cate_id = c.cate_name,
                           Price_Imp = p.product_import_price
                       }
                       ;
            dgvProduct.DataSource = data.ToList();
            displayProductDetail();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dgvProduct_Click(object sender, EventArgs e)
        {
            displayProductDetail();
        }

        private void displayProductDetail()
        {
            if (dgvProduct.CurrentRow != null)
            {
                DataGridViewRow row = dgvProduct.CurrentRow;
                txtId.Text = row.Cells[0].Value.ToString();
                var data_product = db.tbl_Product.Find(txtId.Text);
                if (data_product != null)
                {
                    txtName.Text = data_product.product_name;
                    txtDesc.Text = data_product.product_desc;
                    cbCategory.SelectedValue = data_product.cate_id;
                    txtPriceImp.Value = decimal.Parse(data_product.product_import_price.Value.ToString());
                    txtPriceExp.Value = decimal.Parse(data_product.product_export_price.Value.ToString());
                    txtQuantity.Value = decimal.Parse(data_product.product_quantity.Value.ToString());
                    cbStatus.Checked = (bool)data_product.product_status;
                }


                //posistion = dgvProduct.CurrentRow.Index;
                edit = true;
                txtId.ReadOnly = true;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                String product_id = txtId.Text;
                var product = db.tbl_Product.Find(product_id);
                if (product != null)
                {
                    product.product_name = txtName.Text;
                    product.product_desc = txtDesc.Text;
                    product.product_import_price = (float)txtPriceImp.Value;
                    product.product_export_price = (float)txtPriceExp.Value;
                    product.product_quantity = (int)txtQuantity.Value;
                    product.product_status = cbStatus.Checked;
                    product.cate_id = cbCategory.SelectedValue.ToString();
                    db.SaveChanges();
                    DisplayProduct();
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!ExitCategoryId(txtId.Text))
                {
                    tbl_Product product = new tbl_Product();
                    product.product_id = txtId.Text;
                    product.product_name = txtName.Text;
                    product.product_desc = txtDesc.Text;
                    product.product_import_price = (float)txtPriceImp.Value;
                    product.product_export_price = (float)txtPriceExp.Value;
                    product.product_quantity = (int)txtQuantity.Value;
                    product.product_status = cbStatus.Checked;
                    product.cate_id = cbCategory.SelectedValue.ToString();
                    db.tbl_Product.Add(product);
                    db.SaveChanges();
                    DisplayProduct();
                }
                else
                {
                    MessageBox.Show("Trùng mã sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ExitCategoryId(string productId)
        {
            var p = db.tbl_Product.SingleOrDefault(x => x.product_id == productId);
            return (p != null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtId.Text = txtName.Text = txtDesc.Text = txtPriceImp.Text = txtPriceExp.Text = txtQuantity.Text = "";
            txtId.Focus();
            edit = false;
            txtId.ReadOnly = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProduct.CurrentRow != null)
            {
                if (MessageBox.Show("Bạn có muốn xóa không  ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var product = db.tbl_Product.FirstOrDefault(x => x.product_id == txtId.Text);
                    if (product != null)
                    {
                        db.tbl_Product.Remove(product);
                        db.SaveChanges();
                        DisplayProduct();
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            String cateid = cbCategorySearch.SelectedValue.ToString();
            var products = from p in db.tbl_Product
                           where p.cate_id == cateid && p.product_name.Contains(txtSearch.Text)
                           select p;
            dgvProduct.DataSource = products.ToList();
        }

        private void ctMenuDetail_Click(object sender, EventArgs e)
        {
            var product_detail = db.tbl_Product.FirstOrDefault(x => x.product_id == txtId.Text);
            if (product_detail != null)
            {
                MessageBox.Show(" Tên sản phẩm :  " + product_detail.product_name + "/ Mô tả :  " + product_detail.product_desc + "/ Giá nhâp :  " + product_detail.product_import_price + " / Giá xuất : " + product_detail.product_export_price + " / Số lượng  : " + product_detail.product_quantity + " / Danh mục  : " + product_detail.cate_id + " / Trạng thái : " + product_detail.product_status, "Chi tiết sản phẩm", MessageBoxButtons.OK);
            }
        }
    }
}
