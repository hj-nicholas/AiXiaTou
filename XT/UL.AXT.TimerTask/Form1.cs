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
            //LotteryRequest lot = new LotteryRequest();
            //string str = lot.GetLottery();
        }

        //启动定时器
        public void Start()
        {
             LotteryTimer.Interval = 1000*10*1;
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
            if (prodDto.OpenTime != null)
            {
                var openTime = prodDto.OpenTime;
                int hour = openTime.Hour;
                int min = openTime.Minute;
                string LotPeriod = string.Format("{0:yyyyMMdd}", openTime)+(hour*6+min/10+1).ToString().PadLeft(3,'0');
            }
            LotteryRequest lot = new LotteryRequest();
                string str = lot.GetLottery();
            
            
            //未分享完全部返还给分享者
            List<T_User_Share> lst = prod.GetRestShare(periodId);
            foreach (var shareDto in lst)
            {
                var num = shareDto.ShareNum - shareDto.RevGiftNum;
                string codes = GenerateCode(periodId, num);

                result = prod.AddLotByXUser(shareDto.ShareUserId, num, periodId, codes, 1);
                if (!result.Succeeded)
                {
                    Log.WriteLog("未分享完赠送返还给自己:",result.ErrMsg );
                }
            }

            result = prod.OpenLot(periodId, str);
            
            //执行完成，程序退出
            Application.Exit();
            //}

            
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



    }
}
