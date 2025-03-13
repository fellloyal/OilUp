using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using IniParser.Model;
using IniParser.Parser;
using HandleINI;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ns31;
using ns32;
using ns33;
using ns34;
using ns35;
using ns36;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using IniParser.Model.Formatting;
using System.Web;
using System.Threading.Tasks;
using System.Linq;

using SdOilJson;
using System.Threading;

/*


1.增加上传效验逻辑:Up__EndFile.ini记录已经处理完毕的交易记录文件,up_20200107.ini为根据日期命名的已经处理完毕的交易文件

2.处理逻辑
上传程序每次启动遍历检查所有tradexx.ini文件
    当不是最后一个文件
        查找Up__EndFile.ini中的section
        如果没找到
            在tradexx.ini(当前文件) 中开始逐条处理
                如果记录没有上传, 比对up_xx.ini
                    开始组合十条记录上传
                        如果本批上传数量小于10条, 而且tradexx.ini(当前文件) 中的xx不是当天日期, 证明此日期已经处理完毕
                            记录tradexx.ini(当前文件) 文件到Up__EndFile.ini:[tradexx.ini] end= datetime, 此文件不再处理
                    
					
3.文件示例
示例:Up__EndFile.ini
[Trade20200106.ini]
uptime = 0
[Trade20200107.ini]
uptime= 0

示例:up_20200107.ini
[UpSucess20200107]
iTodaySum = 19
up1= 10331
up2= 10332


*/


namespace OilUp
{
    public partial class Form1 : Form
    {
        string g_URL_GasConfigure    = "https://cpy.zibo.gov.cn:8085/SoftInterface/GasConfigure";
        string g_URL_TankConfigure   = "https://cpy.zibo.gov.cn:8085/SoftInterface/TankConfigure";
        string g_URL_GunConfigure    = "https://cpy.zibo.gov.cn:8085/SoftInterface/GunConfigure";
        string g_URL_GasSaleData     = "https://cpy.zibo.gov.cn:8085/SoftInterface/GasSaleData";
        string g_URL_GasTankData     = "https://cpy.zibo.gov.cn:8085/SoftInterface/GasTankData"; //油罐数据
        string g_URL_GasPurchaseInfo = "https://cpy.zibo.gov.cn:8085/SoftInterface/GasPurchaseInfo"; //进油数据

        /// <summary>
        /// API网关
        /// </summary>
        string g_API_gateWay = "http://192.168.1.125:6085";
        /// <summary>
        /// 油站信息接口地址
        /// </summary> 
        string g_URL_stationInfo = "/api/oilStation/stationInfo";
        /// <summary>
        /// 油罐信息接口地址
        /// </summary>
        string g_URL_tankInfo = "/api/tank/tankInfo";
        /// <summary>
        /// 油机信息接口地址
        /// </summary>
        string g_URL_tankerInfo = "/api/tanker/tankerInfo";
        /// <summary>
        /// 油枪信息接口地址
        /// </summary>
        string g_URL_gunInfo = "/api/gun/gunInfo";
        /// <summary>
        /// 加油流水接口地址
        /// </summary>
        string g_URL_siteRecord = "/api/record/siteRecord";
        /// <summary>
        /// 油罐流水接口地址
        /// </summary>
        string g_URL_oilTankRecord = "/api/tankSum/oilTankRecord";
        /// <summary>
        /// 更新加油设备、税口报警记录
        /// </summary>
        string g_URL_updateRecord = "/api/almTanker/updateRecord";
        /// <summary>
        /// 更新液位仪报警记录
        /// </summary>
        string g_URL_updateAlmLGInfoRecord = "/api/AlmLG/updateAlmLGInfoRecord";
        /// <summary>
        /// 接口密钥(由平台提供)
        /// </summary>
        string g_providerKey = "V5g0r9doO3";

        string g_siteCode = ""; //油站编号
        string g_operateCode = "add"; //操作编码

        SdOilJson.StationInfo cur_station;
        //生成cur_station
        
       
            






        string g_sysNo      = "9000";                                
        string g_GasCode    = "xLa^CPZAEQcERjK7";                    
                           
        string g_StationNo  = "00000016";
        string g_StationName= "淄博市xx加油站";

        /// <summary>
        /// 上传设置文件名
        /// </summary>
        string iniUp                = "up.ini";                   //上传设置文件
        /// <summary>
        /// 油站系统配置文件1 路径+文件名
        /// </summary>
        string iniSysSet            = "SysSet.ini";               //油站系统配置文件1
        /// <summary>
        /// 油站系统数据文件夹
        /// </summary>
        string iniTradeDataIni_Path = "d:\\";                     //油站系统数据文件夹
        /// <summary>
        /// 已上传文件名
        /// </summary>
        string iniUpEndFile       = "Up__EndFile.ini";         //已上传文件名

        
    
        /// <summary>
        ///记录日志次数
        /// </summary>
        int iTimeSum =0;            //记录日志次数
        /// <summary>
        /// 定时器时间 默认60 配置为5 单位为秒
        /// </summary>
        int iLoop = 60;            //定时器时间
        /// <summary>
        /// 当天上传次数
        /// </summary>
        int iTodaySum = 0;         // 当天上传次数


        struct OilType
        {
            public string OilCode;
            public string OilName;
        };


        /// <summary>
        ///循环处理待上传数据 主循环
        /// </summary>
        private void upLoop()
        {
            MessageBox.Show("upLoop");
            get_station(); //读取及上传油站信息
            get32();//读取及上传油罐信息
            get33();//读取及上传油枪信息

            string postPathDir   = System.Environment.CurrentDirectory + "\\data";
            string UpEndFilePath = System.Environment.CurrentDirectory + "\\data\\"+ iniUpEndFile;
            List<string> lsUpEndFile = new List<string>();        //已经处理完毕的交易文件
            lsUpEndFile= INIHelper.ReadSections(UpEndFilePath);

            List<string> lsCurrentTradeFile = new List<string>(); //当前交易文件的所有section
         

            string sCurrentTradeFile;          //当前交易文件
            string sCurrentTradeFileDir;       //当前交易文件带路径


            var TradeFiles = Directory.GetFiles(iniTradeDataIni_Path, "*.ini");   //读入所有交易文件
            foreach (var file in TradeFiles) 
            {
                sCurrentTradeFileDir = file;
                sCurrentTradeFile = System.IO.Path.GetFileName(sCurrentTradeFileDir);//获取不含路径文件名
                if (!lsUpEndFile.Contains(sCurrentTradeFile))  //如果当前文件未处理
                {
                   
                    get34(sCurrentTradeFileDir, sCurrentTradeFile); //处理交易上传
                }
            }
         
            

        }

        public void get_station()
        { 
            if(cur_station.IfUpdate == "1")
            {
                var json = JsonConvert.SerializeObject(cur_station);
                string directValue = up(g_URL_stationInfo, json);
                if (GetResult(directValue))
                {
                    INIHelper.Write("StationInfo", "IfUpdate", "0", iniUp);
                }
            }

        }
        //测试函数
        private void timerTest_Tick(object sender, EventArgs e)
        {
            //timerTest.Enabled = false;
           // upLoop();
        }


        //测试函数
        private void button8_Click(object sender, EventArgs e)
        {
            upLoop();
            return;

            string postPathDir = System.Environment.CurrentDirectory + "\\data";//路径+文件名
            string FilePath   = System.Environment.CurrentDirectory + "\\data\\Up__EndFile.ini";
            List<string> ls = new List<string>();
            string s;


            var files = Directory.GetFiles(iniTradeDataIni_Path, "*.ini");
            foreach (var file in files)
             {
                s = file;
                    lll(s);
              }
            return;

            ls = INIHelper.ReadSections(FilePath);
            foreach (var stu in ls)
            {
                lll(stu);
                string sTmp = "Trade20200105.ini";
                if (ls.Contains(sTmp))
                {
                    lll("Trade20200105.ini have");
                }
                sTmp = "Trade20200101.ini";
                if (ls.Contains(sTmp))
                {
                    lll("Trade20200101.ini have");
                }
            }
        }


        /// <summary>
        /// 处理交易上传
        /// </summary>
        /// <param name="sCurrentTradeFileDir">当前交易文件带路径</param>
        /// <param name="sCurrentTradeFile">当前交易文件</param>
        public void get34(string sCurrentTradeFileDir, string sCurrentTradeFile)
        {
           
            string sCurrentDate = sCurrentTradeFile.Substring(6, 8);  //获取当前待操作日期
            string iniToday = sCurrentTradeFileDir;  //当前交易文件带路径   

            if (!System.IO.File.Exists(iniToday)) //判断文件是否存在
            {
                lll("file not Exists:" + iniToday);
                return;
            }

            if (!System.IO.File.Exists(iniSysSet)) //判断文件是否存在
            {
                lll("file not Exists:" + iniSysSet);
                return;
            }

            get34GasSaleData(iniToday);
            

           
        }

        /// <summary>
        /// 读取十条交易数据
        /// </summary>
        /// <param name="v_file">配置文件param>
        /// <returns></returns>
        public string get34GasSaleData(string v_file) //string v_section,
        {
            string directValue = "";
            string v2_section, sFlowNoSum = ""; //v_section1, , sSectionTmp

            ns34.GasSaleData vGasSaleData = new ns34.GasSaleData(); ;
            vGasSaleData.SysNo = g_sysNo;
            vGasSaleData.GasCode = g_GasCode;

            ns34.Param vParam = new ns34.Param(); ;
            vParam.StationNo = g_StationNo;

            vGasSaleData.Param = vParam;

            vGasSaleData.Param.SaleDataList = new List<ns34.SaleDataListItem>();

            int i = 0;
            int i2 = 0;
            int pos = 0;

            //获取不含路径文件名的当前交易文件
            string sCurrentTradeFile = System.IO.Path.GetFileName(v_file);
            //当前处理日期已完成的文件
            string sCurrentDayEndUpFileDir = System.Environment.CurrentDirectory + "\\data\\" +  "up_" + sCurrentTradeFile.Substring(5, 8) + ".ini";
           
            List<string> lsCurrentDayEndUpFile    = new List<string>();//当前处理日期已完成的交易列表
            List<string> lsCurrentDayEndUpFileAdd = new List<string>();//当前处理日期本次新增的交易列表
            lsCurrentDayEndUpFile = INIHelper.ReadSections(sCurrentDayEndUpFileDir); //获取列

            //当前文件所有待处理交易列表的section
            List<string> lsTradeFile = new List<string>();       
            lsTradeFile = INIHelper.ReadSections(v_file);
            //交易发生时间
            List<string> lsCurrentDayEndUpFileTimeAdd = new List<string>(); 

            string sCurrentTrade; //当前交易
            string sCurrentFlowNo="0"; //当前流水号
            int iBatchSum=1;  //本次处理记录数

            foreach (var file in lsTradeFile)
            {
                sCurrentTrade = file;

                if (!lsCurrentDayEndUpFile.Contains(sCurrentTrade))  //如果当前文件未处理
                {
                    //  for (i = 0; i < 10; i++)
                    //  {
                    try
                    {
                        if (iBatchSum>10)  //一批次最多十条
                            break;
      
                        v2_section = sCurrentTrade;

                        ns34.SaleDataListItem vTankDataListItem = new ns34.SaleDataListItem();

                        vTankDataListItem.FlowNo = ReadIni(v_file, v2_section, "FlowNo");
                        string s = ReadIni(v_file, v2_section, "FlowNo");
                        if (s == null || s.Length == 0)
                        {
                            lll("trade not exist,flowno=" + v2_section);
                            break;
                        }
                        sFlowNoSum += s + ",";
                        sCurrentFlowNo = s;
                        iBatchSum++;
                        lsCurrentDayEndUpFileAdd.Add(sCurrentTrade);
                        string sTmp;
                        sTmp = ReadIni(v_file, v2_section, "GunNo");
                        vTankDataListItem.GunNo = sTmp.PadLeft(2, '0');
                        vTankDataListItem.OptTime = ReadIni(v_file, v2_section, "OptTime");

                        lsCurrentDayEndUpFileTimeAdd.Add(vTankDataListItem.OptTime);

                        string sOil = ReadIni(v_file, v2_section, "OilNo");
                        OilType o = GetOil(sOil);
                        vTankDataListItem.OilNo = o.OilCode;
                        vTankDataListItem.OilAbbreviate = o.OilName;

                        vTankDataListItem.Price = Convert.ToDouble(ReadIni(v_file, v2_section, "Price"));
                        vTankDataListItem.Qty = Convert.ToDouble(ReadIni(v_file, v2_section, "Qty"));
                        vTankDataListItem.Amount = Convert.ToDouble(ReadIni(v_file, v2_section, "Amount"));
                        vTankDataListItem.TTC = Convert.ToInt32(ReadIni(v_file, v2_section, "TTC", 0));
                        vTankDataListItem.EndTotal = Convert.ToDouble(ReadIni(v_file, v2_section, "EndTotal"));
                        vTankDataListItem.TransType = ReadIni(v_file, v2_section, "TransType");
                        vTankDataListItem.OperatorName = ReadIni(v_file, v2_section, "OperatorName");
                        vTankDataListItem.Consumer = ReadIni(v_file, v2_section, "Consumer");
                        vTankDataListItem.PlateNum = ReadIni(v_file, v2_section, "PlateNum");

                        sTmp = ReadIni(v_file, v2_section, "TankNo");
                        vTankDataListItem.TankNo = sTmp;

                        vGasSaleData.Param.SaleDataList.Add(vTankDataListItem);
                        //pos = iSectionTmp;
                    }
                    catch (DivideByZeroException e)
                    {
                        break;
                    }
                }
            }

            string sCurrentDay = sCurrentTradeFile.Substring(5, 8); //当前处理日期
            string sToday      = DateTime.Now.ToString("yyyyMMdd"); //当前处理日期
            string UpEndFilePath = System.Environment.CurrentDirectory + "\\data\\" + iniUpEndFile;

            if (iBatchSum == 1)            //当前处理日期没有待上传交易
            {
                if (sCurrentDay != sToday)  //如果当前处理日期不是当天的
                {
                    INIHelper.Write(sCurrentTradeFile, "upEndTime", Convert.ToString(DateTime.Now.ToString("yyyyMMddHHmmss")), UpEndFilePath);
                    lll( "processing completed :" + sCurrentDay );
                    return directValue;
                }
            }


            if (iBatchSum > 1)
            {
                var json = JsonConvert.SerializeObject(vGasSaleData);
                directValue = up(g_URL_GasSaleData, json);

                if (GetResult(directValue))  //提交成功,记录新的位置
                {
                    lll("Upload trade sucess,FlowNo:" + sFlowNoSum+"  LastTime:"+ lsCurrentDayEndUpFileTimeAdd.Last() );
                    INIHelper.Write("upload", "position", sCurrentFlowNo, iniUp);

                    string s;
                    int i3=0;
                    foreach (var file in lsCurrentDayEndUpFileAdd)
                    {
                        s = file;
                        INIHelper.Write(s, "upTime", Convert.ToString(DateTime.Now.ToString("yyyyMMddHHmmss")), sCurrentDayEndUpFileDir);
                        INIHelper.Write(s, "OptTime", lsCurrentDayEndUpFileTimeAdd[i3], sCurrentDayEndUpFileDir);
                        i3++;
                    }
               

                        if (lsCurrentDayEndUpFileAdd.Count<10)
                        {
                           if (sCurrentDay != sToday)  //如果处理的不是当天的
                           {
                              INIHelper.Write(sCurrentTradeFile, "upEndTime", Convert.ToString(DateTime.Now.ToString("yyyyMMddHHmmss")), UpEndFilePath);
                           }
                           return directValue;
                        }
                        else
                        {
                            get34GasSaleData( v_file);  
                        }  
                }
            }
            else
            {

                lll("No new records found" );
            }
            return directValue;
        }


        //上传成功的数据记录,不检查从前的交易时使用
        public void WriteUpSucess(string sFlowNo)
        {

            //不用
            //读取交易数据

            //上传位置由上传程序维护

            //如果上传位置是0
            //找到最新记录,
            //开始尝试读取十条记录

            //并记录上传位置
            //如果上传位置不是0
            //那么根据上传位置, 开始尝试读取十条记录
            //并记录上传位置

            FileStream fs, fs2;
            StreamWriter sw, sw2;
            string postPathDir = System.Environment.CurrentDirectory + "\\data";//路径+文件名
            string postPath = System.Environment.CurrentDirectory + "\\data\\up_" + DateTime.Now.ToString("yyyyMMdd") + ".ini";//路径+文件名
            string sSection = "UpSucess" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(postPathDir))
            {
                Directory.CreateDirectory(postPathDir);
            }

            if (!File.Exists(postPath))
            {
                fs = new FileStream(postPath, FileMode.OpenOrCreate);
                sw = new StreamWriter(fs);
                sw.WriteLine("[" + sSection + "]");
                sw.WriteLine("iTodaySum=0");
                sw.Flush();
                sw.Close();
                fs.Close();
                iTodaySum = 0;
            }
            else
            {
                string sTodaySum = INIHelper.Read(sSection, "iTodaySum", postPath, "1");
                iTodaySum = Convert.ToInt32(sTodaySum);
            }


            iTodaySum++;
            INIHelper.Write(sSection, "iTodaySum", Convert.ToString(iTodaySum), postPath);
            string sNewLine;
            sNewLine = "up" + Convert.ToInt32(iTodaySum) + "=" + sFlowNo;
            sw2 = new StreamWriter(postPath, true);
            sw2.WriteLine(sNewLine);
            sw2.Close();

        }
        /// <summary>
        /// 提取文本最后一行数据
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>最后一行数据</returns>
        private string GetLastLine(FileStream fs)
        {
            int seekLength = (int)(fs.Length < 1024 ? fs.Length : 1024);  // 这里需要根据自己的数据长度进行调整，也可写成动态获取，可自己实现
            byte[] buffer = new byte[seekLength];
            fs.Seek(-buffer.Length, SeekOrigin.End);
            fs.Read(buffer, 0, buffer.Length);
            string multLine = System.Text.Encoding.UTF8.GetString(buffer);
            string[] lines = multLine.Split(new string[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries);
            string line = lines[lines.Length - 1];

            return line;
        }







        //循环调用上传功能
        private void timer31_Tick(object sender, EventArgs e)
        {
            upLoop();
            iTimeSum++;
            if (iTimeSum > 1440)  //保存日志,循环清除旧的日志
            {
                //删除旧的日志
                DeleteOldFiles(System.Environment.CurrentDirectory + "\\logs", 30);
                iTimeSum = 0;
                textBox1.Text = "";
            }

        }




        /// <summary>
        ///是否调试模式 在当前目录下有debug.dat文件就是调试模式
        /// </summary>
        /// <returns>true表示是</returns>
        private bool IsDebug()
        {
            string FilePath = "debug.dat";
            if (File.Exists(FilePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 删除文件夹strDir中nDays天以前的文件
        /// </summary>
        /// <param name="dir"></param> 文件夹
        /// <param name="days"></param> 天数
        void DeleteOldFiles(string dir, int days)
        {
            try
            {
                if (!Directory.Exists(dir) || days < 1) return;

                var now = DateTime.Now;
                foreach (var f in Directory.GetFileSystemEntries(dir).Where(f => File.Exists(f)))
                {
                    var t = File.GetCreationTime(f);

                    var elapsedTicks = now.Ticks - t.Ticks;
                    var elapsedSpan = new TimeSpan(elapsedTicks);

                    if (elapsedSpan.TotalDays > days) File.Delete(f);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///读ini文件
        /// </summary>
        /// <param name="v_file"> 文件名</param>
        /// <param name="v_section">节名</param>
        /// <param name="v_key">字段</param>
        /// <param name="iFlag">默认值-123456代表字符串,其他的代表是读整形字段</param>
        /// <returns></returns>
        public string ReadIni(string v_file, string v_section, string v_key,int iFlag= -123456)
        {

            string directValue;
            if (iFlag == -123456)
                directValue = INIHelper.Read(v_section, v_key, v_file, "");
            else
                directValue = INIHelper.Read(v_section, v_key, v_file, Convert.ToString(iFlag));
            return directValue;
            //INIHelper.Read("upload", "position", Convert.ToString(pos), iniUp);

            var testIniFileName = v_file;
            var parser = new IniDataParser();
            IniData parsedData;
            using (FileStream fs = File.Open(testIniFileName, FileMode.Open, FileAccess.Read))
            {
                //using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8)) 
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    parsedData = parser.Parse(sr.ReadToEnd());
                }
            }
            string directValue2 = parsedData[v_section][v_key];
            return directValue2;
        }



        //显示信息
        private void lll(string s)
        {
            textBox1.Text += "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + s;


            string file = System.Environment.CurrentDirectory + "\\logs\\log_" + DateTime.Now.ToString("yyyyMMdd") + ".ini"; ;
            StreamWriter sw = new StreamWriter(file,true);
            sw.WriteLine(s);
            sw.Flush();
            sw.Close();
        }



        public Form1()
        {
            InitializeComponent();
            Init();
        }
        //获取结果
        private bool GetResult(string jsonR)
        {
            //反序列化jsonR
            try
            {
                // 尝试将 JSON 字符串解析为 JObject 对象
                JObject jsonObj = JObject.Parse(jsonR);

                // 检查 "Result" 属性是否存在且值为 0
                if (jsonObj["status"] != null && jsonObj["status"].Type == JTokenType.Integer && (int)jsonObj["status"] == 0)
                {
                    
                        return true;
                    
                }
            }
            catch (Newtonsoft.Json.JsonException)
            {
                // 处理 JSON 解析异常
                return false;
            }

            return false;

            /*
            if (jsonR.IndexOf("\"Result\":0,\"Msg\":\"OK\"") <= 0)
                return false;
            else
                return true;
            */
        }


        /// <summary>
        /// 初始化函数
        /// </summary>
        private void Init()
        {
            /// <summary>
            /// 当前工作目录下的data文件夹
            /// </summary>
            string postDataDir = System.Environment.CurrentDirectory + "\\data";
            /// <summary>
            /// 当前工作目录下的logs文件夹
            /// </summary>
            string postLogsDir = System.Environment.CurrentDirectory + "\\logs";

            if (!Directory.Exists(postDataDir)) //如果不存在则创建
            {
                Directory.CreateDirectory(postDataDir);
            };
            if (!Directory.Exists(postLogsDir)) //如果不存在则创建
            {
                Directory.CreateDirectory(postLogsDir);
            }

            iniUp = System.Environment.CurrentDirectory + "\\" + iniUp; //给up.ini文件加上路径

            g_StationName = ReadIni(iniUp, "set", "g_StationName");
          //  MessageBox.Show(g_StationName);

            g_URL_GasConfigure = ReadIni(iniUp, "set", "g_URL_GasConfigure");
            g_URL_TankConfigure = ReadIni(iniUp, "set", "g_URL_TankConfigure");
            g_URL_GunConfigure = ReadIni(iniUp, "set", "g_URL_GunConfigure");
            g_URL_GasSaleData = ReadIni(iniUp, "set", "g_URL_GasSaleData");
            g_URL_GasTankData = ReadIni(iniUp, "set", "g_URL_GasTankData");
            g_URL_GasPurchaseInfo = ReadIni(iniUp, "set", "g_URL_GasPurchaseInfo");



            g_API_gateWay = ReadIni(iniUp, "set", "g_API_gateWay");
            g_URL_stationInfo = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_stationInfo");
            g_URL_tankInfo = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_tankInfo");
            g_URL_tankerInfo = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_tankerInfo");
            g_URL_gunInfo = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_gunInfo");
            g_URL_siteRecord = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_siteRecord");
            g_URL_oilTankRecord = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_oilTankRecord");
            g_URL_updateRecord = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_updateRecord");
            g_URL_updateAlmLGInfoRecord = g_API_gateWay + ReadIni(iniUp, "set", "g_URL_updateAlmLGInfoRecord");

            g_providerKey = ReadIni(iniUp, "set", "g_providerKey");

            cur_station = new SdOilJson.StationInfo();
            //读取配置文件中的StationInfo段
            cur_station.address = ReadIni(iniUp, "StationInfo", "address");
            cur_station.state = ReadIni(iniUp, "StationInfo", "state");
            cur_station.city = ReadIni(iniUp, "StationInfo", "city");
            cur_station.operateCode = ReadIni(iniUp, "StationInfo", "operateCode");
            cur_station.province = ReadIni(iniUp, "StationInfo", "province");
            cur_station.lat = ReadIni(iniUp, "StationInfo", "lat");
            cur_station.lng = ReadIni(iniUp, "StationInfo", "lng");
            cur_station.registerType = ReadIni(iniUp, "StationInfo", "registerType");
            cur_station.startDate = ReadIni(iniUp, "StationInfo", "startDate");
            cur_station.town = ReadIni(iniUp, "StationInfo", "town");
            cur_station.county = ReadIni(iniUp, "StationInfo", "county");
            cur_station.groupId = ReadIni(iniUp, "StationInfo", "groupId");

            cur_station.peopleNum = ReadIni(iniUp, "StationInfo", "peopleNum");
            cur_station.telephone = ReadIni(iniUp, "StationInfo", "telephone");
            cur_station.name = ReadIni(iniUp, "StationInfo", "name");
            cur_station.siteCode = ReadIni(iniUp, "StationInfo", "siteCode");
            g_siteCode = cur_station.siteCode;
            cur_station.providerKey = g_providerKey;
            cur_station.IfUpdate = ReadIni(iniUp, "StationInfo", "IfUpdate");








            g_sysNo = ReadIni(iniUp, "set", "g_sysNo");
            g_GasCode = ReadIni(iniUp, "set", "g_GasCode");
       
            g_StationNo = ReadIni(iniUp, "set", "g_StationNo");

         
            iniSysSet = ReadIni(iniUp, "set", "iniSysSet");
            iniTradeDataIni_Path = ReadIni(iniUp, "set", "iniTradePath");
            iLoop = Convert.ToInt32(ReadIni(iniUp, "set", "loop",60));

            timerMain.Interval = iLoop * 1000; //计时器的单位为毫秒
            timerMain.Enabled  = true;

            if (!IsDebug())
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                timerTest.Enabled = false; //测试用的定时器
                if (g_providerKey == "")
                {
                    lll("调试状态不能使用正式参数");
                    timerMain.Enabled = false;
                    timerOnce.Enabled = false;
                }

            }
            else              //调试状态
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
              
                button6.Visible = true;
                button7.Visible = true;
                button8.Visible = true;

                timerMain.Enabled = false;
                timerOnce.Enabled = false;

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }



        //http调用信息设置1
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //http调用信息设置2
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;   
        }
        

        //上传数据 string v_url, 
        public string up(string _url,string v_json)
        {
            

            lll("json=" + v_json);
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            string jsonParam = v_json ;
            //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls;
             
            var request = (HttpWebRequest)WebRequest.Create(_url);

            //request.KeepAlive = false;
            //request.ProtocolVersion = HttpVersion.Version10;
            //request.ServicePoint.ConnectionLimit = 1;
            //System.Net.ServicePointManager.Expect100Continue = false;

            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
            int length = byteData.Length;
            request.ContentLength = length;
            Stream writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();

            lll("response=" + responseString.ToString());
            return responseString.ToString();
        }




        //1000 	汽油	    2000 	柴油
        //1001 	80# 汽油	2001 	0#柴油
        //1010 	90# 汽油	2006 	-5#柴油
        //1020 	92# 汽油	2011 	-10#柴油
        //1030 	93# 汽油	2016 	-15#柴油
        //1040 	95# 汽油	2017 	0#柴油（ 国三）
        //1050 	97# 汽油	2018 	-10#柴油（ 国三）
        //1060 	120# 汽油	2021 	-20#柴油
        //1080 	其他车用汽油	2031 	-30# 柴油
        //1090 	98# 汽油	2036 	-35#柴油
        //1100 	车用汽油	2900 	其他柴油
        //1200 	航空汽油	2912 	10#柴油
        //1201 	75# 航空汽油	2913 	20# 柴油
        //3100 	CNG 	2914 	其他重柴油
        //3200 	LPG 	2915 	-50# 柴油
        //3300 	LNG 	2916 	其他轻柴油
        //5500 	甲醇




        //获取油品信息
        //ShortOil:mis的油品
        private OilType GetOil(string ShortOil)
        {
            OilType o;
            o.OilCode = "0000";
            o.OilName = "未知油品";



            //vvvvvvvvvvvvvvvvvvvvv  以下为可能被其他油品包含的油品编码
            //20210514 找到相等的就返回,避免继续查找

            if (ShortOil == "0#")
            {
                o.OilCode = "2017";
                o.OilName = "0#柴油(国三)";
                return o;
            }

            
            if (ShortOil == "5#")
            {
                o.OilCode = "2006";
                o.OilName = "-5#柴油";
                return o;
            }

            if (ShortOil == "20#")
            {
                o.OilCode = "2021";
                o.OilName = "-20#柴油";
                return o;
            }


            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


            if (ShortOil.IndexOf("120#") > -1)
            {
                o.OilCode = "1060";
                o.OilName = "120#汽油";
                return o;
            }


            if (ShortOil.IndexOf("00#") > -1)
            {
                o.OilCode = "2001";
                o.OilName = "0#柴油";
                return o;
            }

            if (ShortOil.IndexOf("10#") > -1)
            {
                o.OilCode = "2011";
                o.OilName = "-10#柴油";
                return o;
            }

            if (ShortOil.IndexOf("80#") > -1)
            {
                o.OilCode = "1001";
                o.OilName = "80#汽油";
                return o;
            }

            if (ShortOil.IndexOf("90#") > -1)
            {
                o.OilCode = "1010";
                o.OilName = "90#汽油";
                return o;
            }

            if (ShortOil.IndexOf("92#") > -1)
            {
                o.OilCode = "1020";
                o.OilName = "92#汽油";
                return o;
            }

            if (ShortOil.IndexOf("93#") > -1)
            {
                o.OilCode = "1030";
                o.OilName = "93#汽油";
                return o;
            }

            if (ShortOil.IndexOf("95#") > -1)
            {
                o.OilCode = "1040";
                o.OilName = "95#汽油";
                return o;
            }

            if (ShortOil.IndexOf("97#") > -1)
            {
                o.OilCode = "1050";
                o.OilName = "97#汽油";
                return o;
            }

 
            if (ShortOil.IndexOf("98#") > -1)
            {
                o.OilCode = "1090";
                o.OilName = "98#汽油";
                return o;
            }

            if (ShortOil.IndexOf("75#") > -1)
            {
                o.OilCode = "1201";
                o.OilName = "75#航空汽油";
                return o;
            }

            if (ShortOil.IndexOf("15#") > -1)
            {
                o.OilCode = "2016";
                o.OilName = "-15#柴油";
                return o;
            }

            if (ShortOil.IndexOf("30#") > -1)
            {
                o.OilCode = "2031";
                o.OilName = "-30#柴油";
                return o;
            }

            if (ShortOil.IndexOf("35#") > -1)
            {
                o.OilCode = "2036";
                o.OilName = "-35#柴油";
                return o;
            }

            if (ShortOil.IndexOf("50#") > -1)
            {
                o.OilCode = "2915";
                o.OilName = "-50#柴油";
                return o;
            }

            //--------------------------------------

            if (ShortOil.IndexOf("1000") > -1)
            {
                o.OilCode = "1000";
                o.OilName = "汽油";
                return o;
            }
            if (ShortOil.IndexOf("1080") > -1)
            {
                o.OilCode = "1080";
                o.OilName = "其他车用汽油";
                return o;
            }

            if (ShortOil.IndexOf("2900") > -1)
            {
                o.OilCode = "2900";
                o.OilName = "其他柴油";
                return o;
            }

            if (ShortOil.IndexOf("2914") > -1)
            {
                o.OilCode = "2914";
                o.OilName = "其他重柴油";
                return o;
            }


            if (ShortOil.IndexOf("2916") > -1)
            {
                o.OilCode = "2916";
                o.OilName = "其他轻柴油";
                return o;
            }


            if (ShortOil.IndexOf("1100") > -1)
            {
                o.OilCode = "1100";
                o.OilName = "车用汽油";
                return o;
            }

            if (ShortOil.IndexOf("1200") > -1)
            {
                o.OilCode = "1200";
                o.OilName = "航空汽油";
                return o;
            }

            if (ShortOil.IndexOf("3100") > -1)
            {
                o.OilCode = "3100";
                o.OilName = "CNG";
                return o;
            }

            if (ShortOil.IndexOf("3200") > -1)
            {
                o.OilCode = "LPG";
                o.OilName = "LPG";
                return o;
            }

            if (ShortOil.IndexOf("3300") > -1)
            {
                o.OilCode = "3300";
                o.OilName = "LNG";
                return o;
            }
            if (ShortOil.IndexOf("5500") > -1)
            {
                o.OilCode = "5500";
                o.OilName = "甲醇";
                return o;
            }

            return o;
        }


        /// ///////////////////////////////////////////////////////////////////////////

        //
        //格式化json字符串
        private string ConvertStringToJson(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        //读取1条基础信息
        public string get31GasConfigure(string v_file) //string v_section,
        {
            string directValue = "";
            string v2_section, sTankNo, sTankNo2;     //v_section1, , sSectionTmp

            int TankNum = Convert.ToInt32(ReadIni(iniSysSet, "TankConfigure", "TankNum",1));
            int GunNum  = Convert.ToInt32(ReadIni(iniSysSet, "GunConfigure" , "GunNum" ,1));

            ns31.GasConfigure vGasConfigure = new ns31.GasConfigure(); ;
            vGasConfigure.SysNo   = g_sysNo;
            vGasConfigure.GasCode = g_GasCode;

            ns31.Param vParam  = new ns31.Param(); ;
            vParam.StationNo   = g_StationNo;
            vParam.StationName = g_StationName;

            vGasConfigure.Param = vParam;

            vGasConfigure.Param.TankList = new List<ns31.TankListItem>();

            int i = 0;
            int i2 = 0;
            int pos = 0;
            for (i = 0; i < TankNum; i++)
            {
                    try
                {
                    ns31.TankListItem vTankListItem = new ns31.TankListItem();

                    v2_section = "Tank" + Convert.ToInt32(i);
                    sTankNo = ReadIni(v_file, v2_section, "TankNo");
                    string s = sTankNo;
                    if (s == null || s.Length == 0)
                    {
                        break;
                    }
                    sTankNo = sTankNo.PadLeft(2, '0');
                    //vTankListItem.TankName = ReadIni(v_file, v2_section, "TankName"); 
                    vTankListItem.TankNo     = sTankNo;
                    vTankListItem.TankName   = sTankNo;
                    vTankListItem.TankVolume = Convert.ToInt32(ReadIni(v_file, v2_section, "TankVolume",30000));

                    string sOil = ReadIni(v_file, v2_section, "OilCode");
                    OilType o = GetOil(sOil);
                    vTankListItem.OilCode = o.OilCode;
                    vTankListItem.OilName = o.OilName;
                    //vTankListItem.OilCode    = ReadIni(v_file, v2_section, "OilCode");
                    //vTankListItem.OilName    = ReadIni(v_file, v2_section, "OilName");

                    vGasConfigure.Param.TankList.Add(vTankListItem);

                    vTankListItem.GunList    = new List<ns31.GunListItem>();
                    for (i2 = 0; i2 < GunNum; i2++)
                    {
                        ns31.GunListItem vGunListItem = new ns31.GunListItem();
                        string sGunNo;
                        v2_section = "Gun" + Convert.ToInt32(i2);
                        sGunNo     = ReadIni(v_file, v2_section, "GunNo");
                        sTankNo2   = ReadIni(v_file, v2_section, "TankNo");

                        sTankNo2 = sTankNo2.PadLeft(2, '0');
                        sGunNo   = sGunNo.PadLeft(2, '0');

                        if (sTankNo2 == sTankNo)
                        {
                            vGunListItem.SysId = sGunNo;
                            vGunListItem.GunNo = sGunNo;
                            vTankListItem.GunList.Add(vGunListItem);
                        }
                    }


                    pos++;
                }
                catch (DivideByZeroException e)
                {
                    break;
                }
            }

            var json = JsonConvert.SerializeObject(vGasConfigure)+"}}";
            //lll("-----------------------31:\r\n"+ConvertStringToJson(json));
            //MessageBox.Show(json);
            directValue = up(g_URL_GasConfigure, json);

            if (GetResult(directValue)) //提交
            {
                INIHelper.Write("upload", "positionGasConfigure", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), iniUp);
            }

            return directValue;
        }



        //准备读取基础信息
        public void get31()
        {
            string pos = ReadIni(iniUp, "upload", "positionGasConfigure");
            if (pos == null || pos == "0")
            {
                get31GasConfigure(iniSysSet);
                lll("set GasConfigure end");
            }
        }


 

        //写ini文件
        public string WriteIni(string v_file, string v_section, string v_key, string v_value)
        {
            // string v_file, v_section, v_key;
            var testIniFileName = v_file;
            var parser = new IniDataParser();
            IniData parsedData;
            using (FileStream fs = File.Open(testIniFileName, FileMode.Open, FileAccess.Write))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    parsedData = parser.Parse(sr.ReadToEnd());
                }
            }
            string directValue = parsedData[v_section][v_key];
            return directValue;
        }




        /// <summary>
        /// 读取油枪配置信息2
        /// </summary>
        /// <param name="v_file">配置文件</param>
        /// <param name="gunNum">枪数目</param>
        /// <returns></returns>
        public string get33GunConfigure(string v_file, int gunNum) 
        {
            string directValue = "";
            string sGunNo, sTankNo; 

          
            int i = 0;
            int pos = 0;
            for (i = 0; i < gunNum; i++)  
            {
                string v2_section;
                try
                {
                    v2_section = "Gun" + Convert.ToInt32(i);
                    sGunNo  = ReadIni(v_file, v2_section, "GunNo");
                    sTankNo = ReadIni(v_file, v2_section, "TankNo");
                    string s = sGunNo;
                    if (s == null || s.Length == 0)  
                    {
                        break;
                    }
                    sGunNo  = sGunNo.PadLeft(2, '0');
                    sTankNo = sTankNo.PadLeft(2, '0');


                    SdOilJson.TankerInfo tankerInfo = new SdOilJson.TankerInfo();
                    tankerInfo.operateCode = g_operateCode;
                    tankerInfo.siteCode = g_siteCode;
                    tankerInfo.providerKey = g_providerKey;
                    tankerInfo.tankerCode = sGunNo;
                    tankerInfo.machineType = "1";
                    tankerInfo.factoryNum = "1";
                    tankerInfo.manufacturer = "1";
                    tankerInfo.startDate = "2021-01-01";
                    tankerInfo.sealCode = "1";
                    tankerInfo.state = "0";
                    tankerInfo.name = cur_station.name;

                    var json = JsonConvert.SerializeObject(tankerInfo);
                    directValue = up(g_URL_tankerInfo, json);
                    if (!GetResult(directValue))  //提交失败,退出循环
                    {
                        lll("set tanker Result error : json=" + json);
                        break;
                    }
                    Thread.Sleep(300);

                    SdOilJson.GunInfo gunInfo = new SdOilJson.GunInfo();
                    gunInfo.operateCode = g_operateCode;
                    gunInfo.providerKey = g_providerKey;
                    gunInfo.siteCode = g_siteCode;
                    gunInfo.gunNum = sGunNo;
                    gunInfo.tankerCode = sGunNo;
                    gunInfo.gunNum2 = 0;
                    // Change the type of gunInfo.tankNum to int
                    gunInfo.tankNum = int.Parse(sTankNo);
                    //按油罐取油品
                    string tank_section = "Tank" + (gunInfo.tankNum - 1);
    
                    string sOil = ReadIni(v_file, tank_section, "OilCode");
                    gunInfo.type = sOil;
                    gunInfo.portNum = 1;
                    gunInfo.codeNum = "7";
                    gunInfo.cpuCode = "89";
                    gunInfo.status = 1;
                 



                    //   gunInfo.type = sTankNo;




                    json = JsonConvert.SerializeObject(gunInfo);
                    directValue = up(g_URL_gunInfo, json);
                    if (!GetResult(directValue))  //提交失败,退出循环
                    {
                        lll("set gun Result error : json=" + json);
                        break;
                    }
                    Thread.Sleep(300);

                    pos++ ;
                }
                catch (DivideByZeroException e)
                {
                    break;
                }
            }

            if (pos == gunNum)  //所有全部提交成功,那么更新标记
            {
                INIHelper.Write("GunConfigure", "IfUpdate", "0", v_file);
                lll("set all gun sucess");
            }
            return directValue;
        }


         /// <summary>
        ///  读取油枪配置信息1
        /// </summary>
        public void get33()
        {
            if (!System.IO.File.Exists(iniSysSet)) //判断文件是否存在
            {
                lll("file not Exists:" + iniSysSet);
                return;
            }

            string IfUpdate = ReadIni(iniSysSet, "GunConfigure", "IfUpdate");
            string GunNum   = ReadIni(iniSysSet, "GunConfigure", "GunNum");
            if (IfUpdate == "1")
            {
                get33GunConfigure(iniSysSet, Convert.ToInt32(GunNum));
            }
            else
                lll("No new gun found");
        }


        /// <summary>
        /// 读取油罐配置信息2
        /// </summary>
        /// <param name="v_file">配置文件</param>
        /// <param name="TankNum">油罐数目</param>
        /// <returns></returns>
        public string get32TankConfigure(string v_file, int TankNum)
        {
            string  directValue = "";
            string  sTankNo;
                        
            int i = 0;
            int pos = 0;
            for (i = 0; i < TankNum; i++)
            {
                string v2_section;
                try
                {
                    v2_section = "Tank" + Convert.ToInt32(i);
                    sTankNo = ReadIni(v_file, v2_section, "TankNo");
                    string s = sTankNo;
                    if (s == null || s.Length == 0)
                    {
                        break;
                    }
                    SdOilJson.TankInfo tankInfo = new SdOilJson.TankInfo();
                    tankInfo.tankNum = sTankNo;
                    string sOil = ReadIni(v_file, v2_section, "OilCode");
                    tankInfo.oilType = sOil;
                    tankInfo.maxAllowance = ReadIni(v_file, v2_section, "TankVolume", 30000);
                    tankInfo.operateCode = g_operateCode;
                    tankInfo.siteCode = g_siteCode;
                    tankInfo.providerKey = g_providerKey;




                    var json = JsonConvert.SerializeObject(tankInfo);
                    directValue = up(g_URL_tankInfo, json);

                    if (!GetResult(directValue))  //提交失败,退出循环 
                    {
                        lll("set tank Result error: json=" + json);
                        break;
                    }
                    pos++;
                }
                catch (DivideByZeroException e)
                {
                    break;
                }
            }

            if (pos == TankNum)  //所有全部提交成功,那么更新标记
            {
                INIHelper.Write("TankConfigure", "IfUpdate", "0", v_file);
                lll("set all tank sucess");
            }

            return directValue;
        }




       
        /// <summary>
        /// 读取油罐配置信息1
        /// </summary>
        public void get32()
        {
            if (!System.IO.File.Exists(iniSysSet)) //判断文件是否存在
            {
                lll("file not Exists:" + iniSysSet);
                return;
            }

            string IfUpdate = ReadIni(iniSysSet, "TankConfigure", "IfUpdate");
            string TankNo   = ReadIni(iniSysSet, "TankConfigure", "TankNum");
            if (IfUpdate == "1")
            {
                get32TankConfigure(iniSysSet, Convert.ToInt32(TankNo));
            }
            else
                lll("No new tank found ");
        }
              

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }            


        private void button2_Click(object sender, EventArgs e)
        {
            return;    
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {

    
        }

        private void button5_Click(object sender, EventArgs e)
        {

 
        }

        private void button6_Click(object sender, EventArgs e)
        {


        }

 



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length; //Set the current caret position at the end
            textBox1.ScrollToCaret(); //Now scroll it automatically
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void HYnotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false)
            {
                this.Visible = true;
                this.HYnotifyIcon1.Visible = false;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.HYnotifyIcon1.Visible = true;
                this.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timerOnce.Enabled = false;
            if (this.WindowState == FormWindowState.Normal)
            {
                this.HYnotifyIcon1.Visible = true;
                this.Visible = false;
                this.ShowInTaskbar = true;
            }
            get31();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            get_station();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            get32();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            get33();

        }
    }
}

