using BLL.Lib.CalcFun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMM;
using DAL;

namespace BLL
{
    /// <summary>
    /// 单个收费项目
    /// </summary>
    public class FinanceItem
    {
        private Session _session;

        private decimal _value_end;
        private decimal _value_init;
        private decimal _price;
        private string _customprice;
        private bool _usedefaulconfig;

        public long calclogid { get; set; }
        public short year { get; set; }
        public short month { get; set; }
        public long appid { get; set; }
        public long buildid { get; set; }
        public long classid { get; set; }
        public string classname { get; set; }
        public long roomid { get; set; }
        public short calctype { get; set; }
        public string unit { get; set; }
        public decimal price
        {
            get
            {
                return _price;
            }
            set
            {
                if (_price != value)
                {
                    if (!_usedefaulconfig)
                    {
                        _price = value;
                        UpdatePriceAndValueLogToDB();
                    }
                    else
                    {
                        //记录权限警告日志，提示ULL层，如需修改计费设置需要到系统参数中修改
                        throw new OperationException(ErrorCode.OP_CALCCONFIG_MODIFY_CANT, "此出租屋采用默认计费设置，请到系统设置中修改，或取消采用默认设置！");
                    }
                }
            }
        }
        
        public string customprice
        {
            get
            {
                return _customprice;
            }
            set
            {
                if (_customprice != value)
                {
                    if (!_usedefaulconfig)
                    {
                        _customprice = value;
                        UpdatePriceAndValueLogToDB();
                    }
                    else
                    {
                        //记录权限警告日志，提示ULL层，如需修改计费设置需要到系统参数中修改
                        throw new OperationException(ErrorCode.OP_CALCCONFIG_MODIFY_CANT, "此出租屋采用默认计费设置，请到系统设置中修改，或取消采用默认设置！");
                    }
                }
            }
        }
        public decimal value_init
        {
            get
            {
                return _value_init;
            }
            set
            {
                if (_value_init != value)
                {
                    _value_init = value;
                    UpdateValueLogToDB();
                }
            }
        }
        public decimal value_end { 
            get{
                return _value_end;
            }
            set{
                if(_value_end != value)
                {
                    _value_end = value;
                    UpdateValueLogToDB();
                }
            } 
        }
        public decimal value_real { get; set; }
        public decimal money { get; set; }
        //是否采用默认设置？
        public bool UseDefaulConfig
        {
            get
            {
                return _usedefaulconfig;
            }
            set
            {
                if (_usedefaulconfig != value)
                {
                    _usedefaulconfig = value;
                    UpdateDefaultConfig();
                }
            }
        }

        //定义delegate
        public delegate void UpdateEventHandler(object sender);
        //用event 关键字声明事件对象
        public event UpdateEventHandler UpdateEvent;

        //事件触发方法
        protected virtual void OnUpdateEvent()
        {
            if (UpdateEvent != null)
                UpdateEvent(this);
        }

        //引发事件,更新
        public void RaiseUpdateEvent()
        {
            OnUpdateEvent();
        }

        //重新计算金额
        public void ReCalcMoney()
        {
            value_real = value_end - value_init;
            CalcValue calcvalue = new CalcValue(price, customprice, value_real);
            money = CalcFactory.GetCalculator(calctype).Calc(calcvalue);

            //触发更新
            RaiseUpdateEvent();
        }

        //更新价格信息到数据库
        private void UpdatePrice()
        {
            DAL.FinanceManager.UpdateCalcConfig(appid, buildid, roomid, classid, price, customprice, _session.UserID);
        }

        //更新抄表记录到数据库
        private void UpdateCalcValueLog()
        {
            DAL.FinanceManager.UpdateCalcValueLog(calclogid,value_init,value_end,price,customprice,money);
        }

        /// <summary>
        /// 设置是否采用默认设置
        /// </summary>
        private void UpdateDefaultConfig()
        {
            //由采用改为不采用，删除出租屋设定(改为作废)
            if(!_usedefaulconfig)
            {
                FinanceManager.InvalidCalcConfig(this.appid, this.buildid, this.roomid, this.classid);
            }
            else //由不采用改为采用，使用默认设置新增一条设定记录
            {
                FinanceManager.SetRoomUseCustomCalcConfig(appid, buildid, roomid, classid, _session.UserID);
            }
        }

        /// <summary>
        /// 更新价格设置和抄表试算数据到数据库
        /// </summary>
        private void UpdatePriceAndValueLogToDB()
        {
            UpdatePrice();
            UpdateValueLogToDB();
        }

        /// <summary>
        /// 更新抄表试算数据到数据库
        /// </summary>
        private void UpdateValueLogToDB()
        {
            ReCalcMoney();
            UpdateCalcValueLog();
            RaiseUpdateEvent();
        }

        public FinanceItem(Session session,short year,short month,long roomid,long classid)
        {
            _session = session;
            this.year = year;
            this.month = month;
            this.roomid = roomid;
            this.classid = classid;

            LoadData(year, month, roomid, classid);
        }

        /// <summary>
        /// 加载收费项目数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="roomid"></param>
        /// <param name="classid"></param>
        private void LoadData(short year, short month, long roomid, long classid)
        {
            //获取试算表记录
            calcvaluelog calclog = FinanceManager.GetCalcValueLogs(roomid, year, month, classid);
            //获取出租屋信息
            rooms room = RentManager.GetRoomsById(roomid);
            //获取收费项目默认设定
            financeclass fclass = FinanceManager.GetFinanceClassByID(classid);
            //获取收费项目设定
            calcconfig config = FinanceManager.GetRoomCalcConfig(roomid, classid);
            if(config == null)
            {
                //获取收费项目设定(默认)
                config = FinanceManager.GetRoomDefaultCalcConfig(room.appid, room.buildid,roomid, classid);
                this._usedefaulconfig = true;
            } else
            {
                this._usedefaulconfig = false;
            }

            //保存数据
            this.calclogid = calclog.calclogid;
            this.appid = calclog.appid;
            this.buildid = calclog.buildid;
            this.classname = fclass.classname;
            this.unit = fclass.unit;
            this.calctype = fclass.calctype;
            this._price = config.price;
            this._customprice = config.customprice;
            this._value_init = calclog.value_init.HasValue ? calclog.value_init.Value : 0;
            this._value_end = calclog.value_end.HasValue ? calclog.value_end.Value : 0;
            this.value_real = calclog.value_real.HasValue ? calclog.value_real.Value : 0;
            this.money = calclog.money;
        }
    }

    /// <summary>
    /// 单条房间收费记录
    /// </summary>
    public class RentItem
    {
        private decimal _totalmoney;
        private Session _session;

        public short year { get; set; }
        public short month { get; set; }
        public long appid { get; set; }
        public long buildid { get; set; }
        public long roomid { get; set; }
        public string roomcode { get; set; }
        public long customerid { get; set; }
        public string customername { get; set; }
        public string demo { get; set; }
        public decimal totalmoney
        {
            get
            {
                return _totalmoney;
            }
        }
        public IList<FinanceItem> financelist = new List<FinanceItem>();

        public RentItem(Session session, short year, short month, long roomid)
        {
            this._session = session;
            this.year = year;
            this.month = month;
            this.roomid = roomid;
            
            //取房间信息
            DAL.rooms room = DAL.RentManager.GetRoomsById(roomid);
            if (room != null)
            {
                this.appid = room.appid;
                this.buildid = room.appid;
                this.roomcode = room.code;
                //取合同信息
                rent current = RentManager.GetRoomActiveRentContract(roomid);
                if (current == null) //尚未出租
                {
                    this.customerid = 0;
                    this.customername = "";
                    this.demo = "";
                }
                else
                {
                    this.customerid = current.contract_customer_id.HasValue ? current.contract_customer_id.Value : 0;
                    this.customername = current.contract_customer_name;
                    this.demo = current.contract_memo;
                }
                
                //取月度收租信息
                rentmonth rentm = FinanceManager.GetRoomRentMoonth(roomid, year, month);
                if(rentm != null)
                {
                    this._totalmoney = rentm.payment_money;
                }
                else
                {
                    this._totalmoney = 0;
                }
            }
            else
            {
                throw new DBException(ErrorCode.DB_ROOM_RECORD_NOEXIST, string.Format("系统不存在编号为：{0}的出租屋！",roomid));
            }

            //创建收费项目列表
            IList<financeclass> fclasslist = DAL.FinanceManager.GetFinanceClassList(room.appid);
            foreach(financeclass fclass in fclasslist)
            {
                try
                {
                    FinanceItem fitem = new FinanceItem(session, year, month, roomid, fclass.classid);
                    fitem.UpdateEvent += OnFinanceItemUpdate;
                    financelist.Add(fitem);
                }
                catch { }
            }

            //计算总金额
            OnFinanceItemUpdate(null);
        }

        /// <summary>
        /// 当收费项目发生变化时，重算月度租金
        /// </summary>
        /// <param name="sender"></param>
        private void OnFinanceItemUpdate(object sender)
        {
            decimal newmoney = 0;
            foreach(FinanceItem fitem in financelist)
            {
                newmoney += fitem.money;
            }

            _totalmoney = newmoney;
        }
    }
}
