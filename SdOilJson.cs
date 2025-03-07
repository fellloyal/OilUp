using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SdOilJson
{
   


    /// <summary>
    ///  油罐流水类
    ///  </summary>
    public class OilTankRecord
    {
        /// <summary>
        /// 油量 单位：升
        /// </summary>
        public string oilSale { get; set; }
        /// <summary>
        /// 油站code 唯一编号
        /// </summary>
        public string siteCode { get; set; }
        /// <summary>
        ///  油罐编号
        /// </summary>
        public string tankNum { get; set; }
        /// <summary>
        ///  开始油量   
        /// </summary>
        public string beginVolume { get; set; }
        /// <summary>
        /// 结束油高
        /// </summary>
        public string endHeigth { get; set; }
        /// <summary>
        /// 油品
        /// </summary>
        public string oilName { get; set; }
        /// <summary>
        /// 进出油类型 1油罐进油，2油罐出油
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 开始油高
        /// </summary>
        public string beginHeigth { get; set; }
        /// <summary>
        /// 液位仪编号
        /// </summary>
        public string deviceNum { get; set; }
        /// <summary>
        /// 开始温度
        /// </summary>
        public string beginTemp { get; set; }
        /// <summary>
        /// 结束油量
        /// </summary>
        public string endVolume { get; set; }
        /// <summary>
        /// 开始水体积
        /// </summary>
        public string beginWaterVolume { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 结束水体积
        /// </summary>
        public string endWaterVolume { get; set; }
        /// <summary>
        /// 结束水位
        /// </summary>
        public string endWater { get; set; }
        /// <summary>
        /// 结束温度
        /// </summary>
        public string endTemp { get; set; }
        /// <summary>
        /// 开始水位
        /// </summary>
        public string beginWater { get; set; }
    }

    /// <summary>
    /// 油站详情类
    /// </summary>
    public class StationInfo
    {
        /// <summary>
        /// 操作标识，add表示添加
        /// </summary>
        public string operateCode { get; set; }
        /// <summary>
        /// 油站名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 油站code，唯一标识
        /// </summary>
        public string siteCode { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public string legal { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 就业人数
        /// </summary>
        public string peopleNum { get; set; }
        /// <summary>
        /// 加油站类型，1:民营站, 2:中石油, 3:中石化, 4:中海油
        /// </summary>
        public string groupId { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 县区
        /// </summary>
        public string county { get; set; }
        /// <summary>
        /// 乡镇
        /// </summary>
        public string town { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// 油站地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 注册类型（例如：国有企业，股份有限公司，集体企业等）
        /// </summary>
        public string registerType { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 开业日期，例如：2015-06-11
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 加油站状态，1:营业, 2:未营业
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 秘钥，由平台方分配
        /// </summary>
        public string providerKey { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime { get; set; }
     

    }

    /// <summary>
    /// 油罐信息类
    /// </summary>
    public class TankInfo
    {
        /// <summary>
        /// 操作标识，add表示添加
        /// </summary>
        public string operateCode { get; set; }

        /// <summary>
        /// 秘钥，由平台方分配
        /// </summary>
        public string providerKey { get; set; }

        /// <summary>
        /// 油品（例如：92#、95#、98#等）
        /// </summary>
        public string oilType { get; set; }

        /// <summary>
        /// 油罐编号
        /// </summary>
        public string tankNum { get; set; }

        /// <summary>
        /// 油站编号
        /// </summary>
        public string siteCode { get; set; }

        /// <summary>
        /// 最大罐容，单位：升
        /// </summary>
        public string maxAllowance { get; set; }
    }

    /// <summary>
    /// 油机信息类
    /// </summary>
    public class TankerInfo
    {
        /// <summary>
        /// 操作标识，add表示添加
        /// </summary>
        public string operateCode { get; set; }

        /// <summary>
        /// 秘钥，由平台方分配
        /// </summary>
        public string providerKey { get; set; }

        /// <summary>
        /// 油站编号
        /// </summary>
        public string siteCode { get; set; }

        /// <summary>
        /// 油机编号
        /// </summary>
        public string tankerCode { get; set; }

        /// <summary>
        /// 加油机型号
        /// </summary>
        public string machineType { get; set; }

        /// <summary>
        /// 出厂编号
        /// </summary>
        public string factoryNum { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string manufacturer { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 铅封编号
        /// </summary>
        public string sealCode { get; set; }

        /// <summary>
        /// 加油机状态，0:启用, 1:停用
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 加油站名称
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// 油枪信息类
    /// </summary>
    public class GunInfo
    {
        /// <summary>
        /// 操作标识，add表示添加
        /// </summary>
        public string operateCode { get; set; }

        /// <summary>
        /// 秘钥，由平台方分配
        /// </summary>
        public string providerKey { get; set; }

        /// <summary>
        /// 枪号
        /// </summary>
        public string gunNum { get; set; }

        /// <summary>
        /// 油站编号
        /// </summary>
        public string siteCode { get; set; }

        /// <summary>
        /// 油机编号
        /// </summary>
        public string tankerCode { get; set; }

        /// <summary>
        /// 主板内枪号
        /// </summary>
        public int gunNum2 { get; set; }

        /// <summary>
        /// 油罐编号
        /// </summary>
        public int tankNum { get; set; }

        /// <summary>
        /// 油品
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 税口号
        /// </summary>
        public int portNum { get; set; }

        /// <summary>
        /// 编码器序列号
        /// </summary>
        public string codeNum { get; set; }

        /// <summary>
        /// 微处理器序列号
        /// </summary>
        public string cpuCode { get; set; }

        /// <summary>
        /// 防作弊状态，是否开启，开启:1 未开启:0
        /// </summary>
        public int status { get; set; }
    }

    /// <summary>
    /// 加油流水类
    /// </summary>
    public class SiteRecord
    {
        /// <summary>
        /// 油站code，唯一标识
        /// </summary>
        public string siteCode { get; set; }

        /// <summary>
        /// 油机编号
        /// </summary>
        public string tankerCode { get; set; }

        /// <summary>
        /// 油枪编号
        /// </summary>
        public int gunNum { get; set; }

        /// <summary>
        /// 油品（例如：92#、95#、98#等）
        /// </summary>
        public string oilType { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string unitPrice { get; set; }

        /// <summary>
        /// 油量，单位：升
        /// </summary>
        public double quantity { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string total { get; set; }

        /// <summary>
        /// 数据提交时间，例如：2024-06-17 13:56:30
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 加油机内时间，例如：2024-06-17 13:56:30
        /// </summary>
        public string tankerTime { get; set; }

        /// <summary>
        /// 数据采集类型，2：加油机设备，3：零管，4：集线监听器
        /// </summary>
        public int dataType { get; set; }

        /// <summary>
        /// 操作标识，add表示添加
        /// </summary>
        public string operateCode { get; set; }

        /// <summary>
        /// 平台ID
        /// </summary>
        public string platformId { get; set; }
    }

    /// <summary>
    /// 更新记录类
    /// </summary>
    public class UpdateRecord
    {
        /// <summary>
        /// 油站社会信用代码
        /// </summary>
        public string siteCode { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string dutyId { get; set; }

        /// <summary>
        /// 状态，1:离线, 2:恢复
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 恢复时间，状态为恢复时必填
        /// </summary>
        public string onTime { get; set; }

        /// <summary>
        /// 离线时间，状态为离线时必填
        /// </summary>
        public string offTime { get; set; }

        /// <summary>
        /// 报警类型，0:采集设备, 1:采集端口
        /// </summary>
        public int alarmType { get; set; }

        /// <summary>
        /// 油枪号
        /// </summary>
        public int gunNum { get; set; }

        /// <summary>
        /// 设备端口
        /// </summary>
        public int portNum { get; set; }

        /// <summary>
        /// 微监控处理器号
        /// </summary>
        public string cpuCode { get; set; }
    }


}
