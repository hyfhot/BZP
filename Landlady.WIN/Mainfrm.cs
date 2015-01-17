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
using BLL;
using COMM;

namespace Landlady.WIN
{
    public partial class Mainfrm : Form
    {
        Administrator _admin = null;
        public Mainfrm(Administrator admin)
        {
            InitializeComponent();

            this._admin = admin;
        }

        private void Mainfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        #region 创建树形列表
        /// <summary>
        /// 创建树形列表
        /// </summary>
        /// <param name="app"></param>
        private void BuildInfoTreeView(BLL.App app)
        {
            //增加主项目
            TreeNode tnMainApp = new TreeNode(app.appname, new System.Windows.Forms.TreeNode[] {BuildUserTree(app),BuildBuildsTree(app)});
            tnMainApp.Tag = _admin._session._app;
            tvApp.Nodes.Add(tnMainApp);
        }

        private TreeNode BuildUserTree(BLL.App app)
        {
            IList<TreeNode> tnChilds = new List<TreeNode>();
            foreach(BLL.User useritem in app._users)
            {
                TreeNode tnUser = new TreeNode(useritem.username);
                tnUser.Tag = useritem;
                tnChilds.Add(tnUser);
            }
            TreeNode tnUsers = new TreeNode("用户", tnChilds.ToArray());
            tnUsers.Tag = app._users;

            return tnUsers;
        }

        private TreeNode BuildBuildsTree(BLL.App app)
        {
            IList<TreeNode> tnChilds = new List<TreeNode>();
            foreach (BLL.Building item in app._builds)
            {
                IList<TreeNode> tnRooms = new List<TreeNode>();
                foreach(BLL.Room roomitem in item._rooms)
                {
                    TreeNode tnRoomNode = new TreeNode(roomitem.code);
                    tnRoomNode.Tag = roomitem;
                    tnRooms.Add(tnRoomNode);
                }
                TreeNode tnNode = new TreeNode(item.name, tnRooms.ToArray());
                tnNode.Tag = item;
                tnChilds.Add(tnNode);
            }
            TreeNode tnBuilds = new TreeNode("出租屋", tnChilds.ToArray());
            tnBuilds.Tag = app._builds;

            return tnBuilds;
        }

        #endregion

        #region 创建收租表格

        private void BuildFinanceGrid(BLL.App app)
        {
            BuildFinanceGridColumn(app);
            BuildFinanceGridData(app);
        }

        /// <summary>
        /// 初始化收租表格的列
        /// </summary>
        /// <param name="app"></param>
        private void BuildFinanceGridColumn(BLL.App app)
        {
            //删除所有数据
            dgv_Rent.Rows.Clear();
            //删除所有列
            dgv_Rent.Columns.Clear();

            #region 增加固定列
            //楼房列，如果有多栋楼的情况下显示
            DataGridViewTextBoxColumn buildname = new DataGridViewTextBoxColumn();
            buildname.HeaderText = "楼房";
            buildname.ReadOnly = true;
            dgv_Rent.Columns.Add(buildname);
            //房间号
            DataGridViewTextBoxColumn roomcode = new DataGridViewTextBoxColumn();
            roomcode.HeaderText = "房间号";
            roomcode.ReadOnly = true;
            dgv_Rent.Columns.Add(roomcode);
            //租客姓名
            DataGridViewTextBoxColumn customername = new DataGridViewTextBoxColumn();
            customername.HeaderText = "租客";
            customername.ReadOnly = true;
            dgv_Rent.Columns.Add(customername);
            //收费类型
            foreach(BLL.FinanceClass fc in app._financeclasslist)
            {
                if (fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTATIC || fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MANUAL)
                {
                    DataGridViewTextBoxColumn fccol = new DataGridViewTextBoxColumn();
                    fccol.HeaderText = fc.classname;
                    dgv_Rent.Columns.Add(fccol);
                }
                else if (fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHAMOUNT || fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTEPAMOUNT)
                {
                    DataGridViewTextBoxColumn fccolinit = new DataGridViewTextBoxColumn();
                    fccolinit.HeaderText = string.Format("{0}|期初", fc.classname);
                    DataGridViewTextBoxColumn fccolend = new DataGridViewTextBoxColumn();
                    fccolend.HeaderText = string.Format("{0}|期末", fc.classname);
                    DataGridViewTextBoxColumn fccolreal = new DataGridViewTextBoxColumn();
                    fccolreal.HeaderText = string.Format("{0}|用量", fc.classname);
                    DataGridViewTextBoxColumn fccolmoney = new DataGridViewTextBoxColumn();
                    fccolmoney.HeaderText = string.Format("{0}|金额", fc.classname);
                    dgv_Rent.Columns.Add(fccolinit);
                    dgv_Rent.Columns.Add(fccolend);
                    dgv_Rent.Columns.Add(fccolreal);
                    dgv_Rent.Columns.Add(fccolmoney);
                }
            }
            //合计金额
            DataGridViewTextBoxColumn coltotal = new DataGridViewTextBoxColumn();
            coltotal.HeaderText = "合计";
            coltotal.ReadOnly = true;
            dgv_Rent.Columns.Add(coltotal);

            #endregion
        }

        /// <summary>
        /// 初始化收租表格的数据
        /// </summary>
        /// <param name="app"></param>
        private void BuildFinanceGridData(BLL.App app)
        {
            //删除所有数据
            dgv_Rent.Rows.Clear();

            //创建一新行
            foreach (Building build in app._builds)
            {
                foreach(Room room in build._rooms)
                {
                    foreach(RentItem rent in room.RentItemList)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dgv_Rent);
                        int i = 0;
                        row.Cells[i].Tag = build;
                        row.Cells[i++].Value = build.name;
                        row.Cells[i].Tag = room;
                        row.Cells[i++].Value = room.code;
                        row.Cells[i].Tag = rent;
                        row.Cells[i++].Value = rent.customername;

                        //收费类型
                        foreach (FinanceClass fc in app._financeclasslist)
                        {
                            FinanceItem fi = rent.financelist.FirstOrDefault(c => c.classid == fc.classid);
                            if (fi == null)
                            {
                                if (fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTATIC || fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MANUAL)
                                {
                                    i++;
                                }
                                else if (fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHAMOUNT || fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTEPAMOUNT)
                                {
                                    i = i + 4;
                                }
                                continue;
                            }
                            if (fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTATIC || fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MANUAL)
                            {
                                row.Cells[i].Tag = fi;
                                row.Cells[i++].Value = fi.money;
                            }
                            else if (fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHAMOUNT || fc.calctype == ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTEPAMOUNT)
                            {
                                row.Cells[i].Tag = fi;
                                row.Cells[i++].Value = fi.value_init;
                                row.Cells[i].Tag = fi;
                                row.Cells[i++].Value = fi.value_end;
                                row.Cells[i].Tag = fi;
                                row.Cells[i++].Value = fi.value_real;
                                row.Cells[i].Tag = fi;
                                row.Cells[i++].Value = fi.money;
                            }
                        }
                        row.Cells[i].Tag = rent;
                        row.Cells[i].Value = rent.totalmoney;

                        //增加行
                        dgv_Rent.Rows.Add(row);
                    }
                }
            }
        }

        #endregion


        private void Mainfrm_Load(object sender, EventArgs e)
        {
            if (_admin._session.UserID > 0)
            {
                //创建树列表
                BuildInfoTreeView(_admin._session._app);
                //创建收租表格
                BuildFinanceGrid(_admin._session._app);
            }
        }
    }
}
