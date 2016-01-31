using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Model;
using UL.AXT;

namespace UL.AXT.TimerTask
{
    public partial class Form1 : Form
    {
        private string[]s = new string[1];
        
        public Form1(string[] args)
        {
            InitializeComponent();
            s = args;
            Start();
            Log.WriteLog("period:", s[0]);
            //LotteryRequest lot = new LotteryRequest();
            //LotPerModel model = lot.GetLottery();
            //string period= calcPeriod("2016-01-30 22:26:18.337");
            //MessageBox.Show(period);
        }

        //启动定时器
        public void Start()
        {
             LotteryTimer.Interval = 1000*60*3;
            LotteryTimer.Start();
        }

        private void LotteryTimer_Tick(object sender, EventArgs e)
        {
            BLL.Product prod = new BLL.Product();
            BaseResult result = new BaseResult();

            //到达指定时间，执行程序
            //if (true)
            //{
            int periodId = Convert.ToInt32(s[0]);
            //int periodId =2021;
            //彩票期号
            ProductModel prodDto = prod.GetProductByPeriodId(periodId);
            string LotPeriod = "";
            int per = 0;
            if (prodDto.OpenTime != null)
            {
                //开奖规律06:00 - 10:00销售当天第一期，10:00统一开奖。10:00 - 22:00(10分钟一期，共72期)，22:00 - 02:00(5分钟一期, 共48期)，02:00 - 06:00暂停销售，全天共计120期。
                var openTime = prodDto.OpenTime;
                int hour = openTime.Hour;
                
                int min = openTime.Minute;

                if (hour >= 0 && hour < 2)
                {
                    per = (hour * 12 + min / 5 + 1);
                }
                else if (hour >= 2 && hour < 10)
                {
                    per = 24;
                }
                else if (hour >= 10 && hour < 22)
                {
                    per = 24 + ((hour - 10)*6 + min/10 + 1);
                }
                else if (hour >= 22 && hour < 24)
                {
                    per=96+ ((hour-22) * 12 + min / 5 + 1);
                }


                LotPeriod = string.Format("{0:yyyyMMdd}", openTime)+ per.ToString().PadLeft(3,'0');
            }
            Log.WriteLog("LotPeriod:", LotPeriod);
            LotteryRequest lot = new LotteryRequest();
            LotPerModel model = lot.GetLottery();
            Log.WriteLog("model.LotPer:", model.LotPer);
            Log.WriteLog("model.number:", model.number);
            if (model.LotPer == LotPeriod)
            {
                //未分享完全部返还给分享者
                List<T_User_Share> lst = prod.GetRestShare(periodId);
                foreach (var shareDto in lst)
                {
                    var num = shareDto.ShareNum - shareDto.RevGiftNum;
                    if (num > 0)
                    {
                        string codes = GenerateCode(periodId, num);

                        result = prod.AddLotByXUser(shareDto.ShareUserId, num, periodId, codes, 1);
                        if (!result.Succeeded)
                        {
                            Log.WriteLog("未分享完赠送返还给自己:", result.ErrMsg);
                        }
                    }
                    
                }

                result = prod.OpenLot(periodId, model.number);

                //执行完成，程序退出
                Application.Exit();
            }

           
          
        }

        //计算用户当期的随机虾仔号码
        public string GenerateCode(int periodId, int codeNum)
        {
            string codes = string.Empty;
            var code = "";
            Random rd = new Random();
            var flag = false;
            //生成不重复的抽奖号码
            for (int i = 0; i < codeNum; i++)
            {
                do
                {
                    code = rd.Next(10000, 100000).ToString();
                } while (isExistsNo(code, periodId));

                codes += code + ",";
            }
            if (codes.Length > 0)
                codes = codes.Substring(0, codes.Length - 1);
            return codes;
        }

        public bool isExistsNo(string code, int periodId)
        {
            BLL.Order order = new Order();
            bool flag = order.IsExistsLottery(code, periodId);
            return flag;
        }

        public string calcPeriod(string date)
        {
            var openTime =Convert.ToDateTime( date);
            int per = 0;
            int hour = openTime.Hour;

            int min = openTime.Minute;

            if (hour >= 0 && hour <= 2)
            {
                per = (hour * 12 + min / 5 + 1);
            }
            else if (hour > 2 && hour < 10)
            {
                per = 24;
            }
            else if (hour >= 10 && hour < 22)
            {
                per = 24 + ((hour - 10) * 6 + min / 10 + 1);
            }
            else if (hour >= 22 && hour < 24)
            {
                per = 96 + ((hour - 22) * 12 + min / 5 + 1);
            }


          string  LotPeriod = string.Format("{0:yyyyMMdd}", openTime) + per.ToString().PadLeft(3, '0');
            return LotPeriod;
        }

    }
}
