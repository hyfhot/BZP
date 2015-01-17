using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMM;

namespace DAL
{
    /// <summary>
    /// 房屋管理相关，楼房、出租屋的增删改查等
    /// </summary>
    public class RentManager
    {

        #region 出租屋相关
        /// <summary>
        /// 根据房间ID列表取房间数据
        /// </summary>
        /// <param name="roomidlist"></param>
        /// <returns></returns>
        public static IList<rooms> GetRoomsById(long[] roomidlist)
        {
            IList<rooms> roomlist = new List<rooms>();

            using (Entities db = new Entities())
            {
                //根据房间ID集合取房间列表
                roomlist = (from g in db.rooms where roomidlist.Any(c => c == g.roomid) select g).ToList();
            }

            return roomlist;
        }

        /// <summary>
        /// 根据房间ID列表取房间数据
        /// </summary>
        /// <param name="roomidlist"></param>
        /// <returns></returns>
        public static rooms GetRoomsById(long roomid)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    //根据房间ID取房间列表
                    return db.rooms.Single(c => c.roomid == roomid);
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

        #region 楼房相关
        /// <summary>
        /// 取账号下面所有楼房信息
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static IList<building> GetAppBuilds(long appid)
        {
            using (Entities db = new Entities())
            {
                return (from g in db.building where g.appid == appid && g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK select g).ToList();
            }
        }

        /// <summary>
        /// 取楼房下面所有出租屋信息
        /// </summary>
        /// <param name="buildid"></param>
        /// <returns></returns>
        public static IList<rooms> GetBuildRooms(long buildid)
        {
            using (Entities db = new Entities())
            {
                return (from g in db.rooms where g.buildid == buildid && g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK select g).ToList();
            }
        }

        /// <summary>
        /// 根据ID获取楼房
        /// </summary>
        /// <param name="buildid">楼房id</param>
        /// <returns></returns>
        public static building GetBuildByID(long buildid)
        {
            building build = null;
            using (Entities db = new Entities())
            {
                build = db.building.FirstOrDefault(c => c.buildid == buildid);
            }

            return build;
        }
        #endregion

        #region 出租合同相关
        /// <summary>
        /// 根据参数获取指定时间点正在出租的合同信息
        /// </summary>
        /// <param name="renttime">基准时间点</param>
        /// <param name="appid">账号ID</param>
        /// <param name="buildid">楼房ID</param>
        /// <param name="roomid">房间ID</param>
        /// <returns>有效的合同集合</returns>
        public static IList<rent> GetRentingContracts(DateTime renttime, long appid, long buildid = 0, long roomid = 0)
        {
            IList<rent> contractlist = new List<rent>();

            using (Entities db = new Entities())
            {
                contractlist = (from g in db.rent
                                where g.contract_begin < renttime /*&& g.contract_end > renttime 过期未续租则默认合同继续有效！*/ && g.appid == appid && g.rentstatus == ConstantDefine.DB_RENT_COL_RENSTATUS_RENT && g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK &&
                                      (buildid == 0 || g.buildid == buildid) && (roomid == 0 || g.roomid == roomid)
                                select g).ToList().Select(g => new rent
                                {
                                    rentid = g.rentid,
                                    appid = g.appid,
                                    buildid = g.buildid,
                                    roomid = g.roomid,
                                    contract_begin = g.contract_begin,
                                    contract_end = g.contract_end,
                                    contract_pic = g.contract_pic,
                                    contract_date = g.contract_date,
                                    contract_memo = g.contract_memo,
                                    contract_room_price = g.contract_room_price,
                                    contract_room_pic = g.contract_room_pic,
                                    contract_room_pledge = g.contract_room_pledge,
                                    contract_room_peoples = g.contract_room_peoples,
                                    contract_room_payment = g.contract_room_payment,
                                    contract_water_value = g.contract_water_value,
                                    contract_elec_value = g.contract_elec_value,
                                    contract_customer_id = g.contract_customer_id,
                                    contract_customer_name = g.contract_customer_name,
                                    contract_operator_id = g.contract_operator_id,
                                    contract_operator_name = g.contract_operator_name
                                }
                                ).ToList();
            }

            return contractlist;
        }

        /// <summary>
        /// 根据参数获取指定时间点正在出租的房间
        /// </summary>
        /// <param name="renttime">基准时间点</param>
        /// <param name="appid">账号ID</param>
        /// <param name="buildid">楼房ID</param>
        /// <param name="roomid">房间ID</param>
        /// <returns>正在出租的房间</returns>
        public static IList<rooms> GetRentingRooms(DateTime renttime, long appid, long buildid = 0, long roomid = 0)
        {
            IList<rooms> roomlist = new List<rooms>();

            using (Entities db = new Entities())
            {
                //获取生效的租赁合同
                IList<rent> rentlist = GetRentingContracts(renttime, appid, buildid, roomid);

                //根据租赁合同获取房间ID集合(去重)
                var roomsid = (from g in rentlist group g by g.roomid into p select p.Key).ToArray();

                //根据房间ID集合取房间列表
                roomlist = GetRoomsById(roomsid);
            }

            return roomlist;
        }

        /// <summary>
        /// 获取出租屋有效的出租合同
        /// </summary>
        /// <param name="roomid"></param>
        /// <returns></returns>
        public static rent GetRoomActiveRentContract(long roomid)
        {
            using (Entities db = new Entities())
            {
                return db.rent.OrderByDescending(c => c.rentid).FirstOrDefault(c => c.roomid == roomid
                    && c.rentstatus == ConstantDefine.DB_RENT_COL_RENSTATUS_CONTINUE
                    && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        #endregion

    }
}
