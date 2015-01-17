using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ULL;

namespace Landlady.WIN
{
    public partial class Loginfrm : Form
    {
        Administrator admin = null;

        public Loginfrm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (admin == null) admin = new Administrator();

            if (admin.Login(txtUserName.Text,txtPassword.Text))
            {
                this.Hide();
                Mainfrm mainfrm = new Mainfrm(admin);
                mainfrm.ShowDialog();
            }
            else
            {
                MessageBox.Show("用户名或密码错误!");
            }
        }
    }
}
