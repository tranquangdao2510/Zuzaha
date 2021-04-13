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
    public partial class FrmSearchEmployes : Form
    {
        zuhahaEntities db = new zuhahaEntities();
        bool edit = true;
        int posistion;
        public FrmSearchEmployes()
        {
            InitializeComponent();
            dgvEmployes.AutoGenerateColumns = false;
            ReadClassified();
            ReadEmployes();
        }
        public void ReadClassified()
        {
            cbClassified.DataSource = db.tbl_Classifed.ToList();
            cbClassified.DisplayMember = "classifed_name";
            cbClassified.ValueMember = "classifed_id";
        }
        public void ReadEmployes()
        {
            //dgvEmployes.DataSource = db.tbl_Employees.ToList();
            dgvEmployes.DataSource = (from e in db.tbl_Employees
                                      join c in db.tbl_Classifed
                                      on e.classifed_id equals c.classifed_id
                                      select new
                                      {
                                          employes_id = e.employes_id,
                                          employes_name = e.employes_name,
                                          employes_phone = e.employes_phone,
                                          employes_address = e.employes_address,
                                          email = e.email,
                                          password = e.password,
                                          gender = (bool)e.gender ? "nam" : "nu",
                                          brithday = (DateTime)e.brithday,
                                          classifed_id = c.classifed_name,
                                          status_Employes = (bool)e.status_Employes?"Hoạt động":"Không hoạt đông"
                                      }
                                    ).ToList();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            String clsId = cbClassified.SelectedValue.ToString();
            var employes = from emp in db.tbl_Employees
                           where emp.classifed_id == clsId && emp.employes_name.Contains(txtSearch.Text)
                           select emp;
            dgvEmployes.DataSource = employes.ToList();
        }
    }
}
