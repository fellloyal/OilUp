using System;
using System.Collections.Generic;



////////////////////////////////////////////////////////////////// 3.1 GasConfigure
namespace ns31
{

    public class GunListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GunNo { get; set; }
    }

    public class TankListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string TankNo { get; set; }
        /// <summary>
        /// 第T00号油罐
        /// </summary>
        public string TankName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TankVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OilCode { get; set; }
        /// <summary>
        /// 92#汽油
        /// </summary>
        public string OilName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GunListItem> GunList { get; set; }
    }

    public class Param
    {
        /// <summary>
        /// 
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// 淄博市*****加油站
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TankListItem> TankList { get; set; }
    }

    public class GasConfigure
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Param Param { get; set; }
    }
}



//////////////////////////////////////////////////////////////////  3.4 GasSaleData
namespace ns34
{
    public class SaleDataListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GunNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OptTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OilNo { get; set; }
        /// <summary>
        /// 92#汽油 
        /// </summary>
        public string OilAbbreviate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TTC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double EndTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TransType { get; set; }
        /// <summary>
        /// 胡某某
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 张某某
        /// </summary>
        public string Consumer { get; set; }
        /// <summary>
        /// 鲁 C16E62
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TankNo { get; set; }
    }

    public class Param
    {
        /// <summary>
        /// 
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SaleDataListItem> SaleDataList { get; set; }
    }

    public class GasSaleData
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Param Param { get; set; }
    }

}



////////////////////////////////////////////////////////////////// 3.2 TankConfigure

namespace ns32
{
    public class Param
    {
        /// <summary>
        /// 
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TankNo { get; set; }
        /// <summary>
        /// 第T001 号油罐
        /// </summary>
        public string TankName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TankVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OilCode { get; set; }
        /// <summary>
        /// 92#汽油
        /// </summary>
        public string OilName { get; set; }
    }

    public class TankConfigure
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Param Param { get; set; }
    }
}


//////////////////////////////////////////////////////////////////  3.3 GunConfigure
namespace ns33
{
    public class Param
    {
        /// <summary>
        /// 
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GunNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SysId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TankNo { get; set; }
    }

    public class GunConfigure
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Param Param { get; set; }
    }
}



////////////////////////////////////////////////////////////////// 3.6 GasPurchaseInfo

namespace ns36
{
    public class PurchDetailListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string TankNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double StartTemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double StartOilLen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StartWaterLen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double StartQtyLiter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double EndOilLen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NewWaterLen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double EndTemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double EndQtyLiter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PeriodSaleQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double NetVol { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
    }

    public class Param
    {
        /// <summary>
        /// 
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// 淄博市*****加油站
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SaleDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ShiftNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MakeTime { get; set; }
        /// <summary>
        /// 汇丰石化
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OilNo { get; set; }
        /// <summary>
        /// 92#汽油
        /// </summary>
        public string OilAbbreviate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double SendVt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GaugerNo { get; set; }
        /// <summary>
        /// 胡某某
        /// </summary>
        public string Guager { get; set; }
        /// <summary>
        /// 鲁CJ0133
        /// </summary>
        public string CarNo { get; set; }
        /// <summary>
        /// 毕某某
        /// </summary>
        public string MotorMan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EmplNo { get; set; }
        /// <summary>
        /// 徐某某
        /// </summary>
        public string EmplName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PurchDetailListItem> PurchDetailList { get; set; }
    }

    public class GasPurchaseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Param Param { get; set; }
    }
}




////////////////////////////////////////////////////////////////// 3.5 GasTankData

namespace ns35
{
    public class TankDataListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string TankNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReadTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OilNo { get; set; }
        /// <summary>
        /// 92#汽油
        /// </summary>
        public string OilAbbreviate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Stock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OilLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int WaterLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Temp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TankEmpty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int State { get; set; }
    }

    public class Param
    {
        /// <summary>
        /// 
        /// </summary>
        public string StationNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FlowNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TankDataListItem> TankDataList { get; set; }
    }

    public class GasTankData
    {
        /// <summary>
        /// 
        /// </summary>
        public string SysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Param Param { get; set; }
    }
}

////////////////////////////////////////////////////////////////// 3.0 Result
public class JsonResult
{
    /// <summary>
    /// 
    /// </summary>
    public int Result { get; set; }
    /// <summary>
    /// TankNo参数错误
    /// </summary>
    public string Msg { get; set; }
}