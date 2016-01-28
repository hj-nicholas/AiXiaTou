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

namespace UL.AXT.TimerTask
{
    public partial class AddShrimpRD : Form
    {
        private BLL.UserInfo user = new BLL.UserInfo();
        private BLL.Product prod = new BLL.Product();
        private string[] s = new string[1];

        public AddShrimpRD(string[] args)
        {
            InitializeComponent();
            s = args;
            StartAC();
        }

        public void StartAC()
        {
            timer1.Interval = 1000*60*60;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //开始时分计时
            Random rd = new Random();
            int minAndSec= rd.Next(1, 3600);
            timer2.Interval = 1000*minAndSec;
            timer2.Start();
            //停止小时计时
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BaseResult result = new BaseResult();
            int periodId = Convert.ToInt32(s[0]);
            if (periodId != 0)
            {
            //判断添加虾码数量是否超过
            BaseResult br1 = prod.GetRestShrimpNum(periodId);
            int restNum =Convert.ToInt32( br1.ResultId);
            
                int xnum = Convert.ToInt32(restNum * 0.1);
            if (xnum >1)
            {

                //随机X用户购买虾码
                List<UserDTO> lstUser = user.GetUserByType(2);
                Random rd = new Random();
                int userNo = rd.Next(0, lstUser.Count - 1);
                UserDTO userDto = lstUser[userNo];

                string codes = GenerateCode(periodId, xnum);

                result = prod.AddLotByXUser(userDto.UserID, xnum, periodId, codes,1);

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
    }
}
