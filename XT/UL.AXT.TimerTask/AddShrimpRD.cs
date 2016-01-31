using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Model;

namespace UL.AXT.TimerTask
{
    public partial class AddShrimpRD : Form
    {
        private BLL.UserInfo user = new BLL.UserInfo();
        private BLL.Product prod = new BLL.Product();
        private string[] s = new string[2];

        public AddShrimpRD(string[] args)
        {
            InitializeComponent();
            s = args;
            StartAC();
           
        }

        public void StartAC()
        {
            timer1.Interval = 1000*60* 60;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //开始时分计时
            Random rd = new Random();
            int minAndSec = rd.Next(1, 3600);
            //int minAndSec = rd.Next(1, 20);
            timer2.Interval = 1000 * minAndSec;
            timer2.Start();
            //停止小时计时
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BaseResult result = new BaseResult();
            int periodId = Convert.ToInt32(s[0]);
            decimal rate = Convert.ToDecimal(s[1]);
            //int periodId = Convert.ToInt32("2024");
            //decimal rate = Convert.ToDecimal("0.3");

            if (periodId != 0 && rate != 0)
            {
                //判断添加虾码数量是否超过
                BaseResult br1 = prod.GetRestShrimpNum(periodId);
                ProductModel prodDto = prod.GetProductByPeriodId(periodId);
                int restNum = Convert.ToInt32(br1.ResultId);
                if (restNum == 0)
                {
                    OpenLottery(periodId);
                    Application.Exit();
                }
                int xnum = Convert.ToInt32(prodDto.ProductPrice * rate);

                //随机X用户购买虾码
                List<UserDTO> lstUser = user.GetUserByType(2);
                Random rd = new Random();
                int userNo = rd.Next(0, lstUser.Count - 1);
                UserDTO userDto = lstUser[userNo];
                string codes = "";

                if (xnum > restNum)
                {
                    xnum = restNum;
                    codes = GenerateCode(periodId, xnum);

                    result = prod.AddLotByXUser(userDto.UserID, xnum, periodId, codes, 2);

                    OpenLottery(periodId);
                    Application.Exit();
                }
                else
                {
                    codes = GenerateCode(periodId, xnum);
                    result = prod.AddLotByXUser(userDto.UserID, xnum, periodId, codes, 2);

                }

            }
            timer2.Stop();
            timer1.Start();
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

        public void OpenLottery(int periodId)
        {
            BaseResult result = new BaseResult();
            result = prod.IsBuyOver(periodId);
            //自动获取彩票
            if (result.ResultId == "0")
            {
                string path =  "/LotteryExe/UL.AXT.TimerTask.exe";
                DirectoryInfo dir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
                string PhysicalPath = dir.Parent.FullName+path;
                //Log.WriteLog("path:", PhysicalPath);
                Process proc = new Process();
                proc.StartInfo.FileName = PhysicalPath;
                proc.StartInfo.Arguments = periodId.ToString();
                proc.Start();
            }
        }
    }
}
