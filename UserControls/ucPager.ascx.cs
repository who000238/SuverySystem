using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem.UserControls
{
    public partial class ucPager : System.Web.UI.UserControl
    {
        /// <summary> 頁面Url </summary>
        public string Url { get; set; }
        /// <summary> 總筆數 </summary>
        public int TotalSize { get; set; }
        /// <summary>頁面筆數/ </summary>
        public int PageSize { get; set; }
        /// <summary>目前頁數 </summary>
        public int CurrentPage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string txtSearch { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int GetCurrentPage()
        {
            string pageText = Request.QueryString["Page"];

            if (string.IsNullOrWhiteSpace(pageText))
                return 1;

            int intPage;
            if (!int.TryParse(pageText, out intPage))
                return 1;

            if (intPage <= 0)
                return 1;

            return intPage;
        }

        public void Bind()
        {
            //檢查一頁筆數
            if (this.PageSize <= 0)                                //總頁數小於零 顯示錯誤訊息
            {
                throw new DivideByZeroException();
            }
            //算總頁數
            int totalPage = this.TotalSize / this.PageSize;        //總頁數等於總資料筆數/一頁筆數
            if (this.TotalSize % this.PageSize > 0)                //資料筆數非一頁筆數的整倍數時頁數加1
            {
                totalPage += 1;
            }

            this.aLinkFirst.HRef = $"{this.Url}?page=1";           //設定起點頁、預設永遠都有一頁
            this.aLinkLast.HRef = $"{this.Url}?page={totalPage}";  //設定終點頁、為總頁數

            //依目前頁數計算
            this.CurrentPage = this.GetCurrentPage();
            this.ltlCurrent.Text = this.CurrentPage.ToString();

            //}
            //計算頁數
            int prevM1 = this.CurrentPage - 1;
            int prevM2 = this.CurrentPage - 2;
            int nextP1 = this.CurrentPage + 1;
            int nextP2 = this.CurrentPage + 2;


            this.aLink2.HRef = $"{this.Url}?page={prevM1}";
            this.aLink2.InnerText = prevM1.ToString();              //修正頁數文字
            this.aLink1.HRef = $"{this.Url}?page={prevM2}";
            this.aLink1.InnerText = prevM2.ToString();



            this.aLink4.HRef = $"{this.Url}?page={nextP1}";
            this.aLink4.InnerText = nextP1.ToString();
            this.aLink5.HRef = $"{this.Url}?page={nextP2}";
            this.aLink5.InnerText = nextP2.ToString();

            //依頁數，決定是否隱藏超連結，並處理提示文字

            this.aLink1.Visible = (prevM2 > 0);
            this.aLink2.Visible = (prevM1 > 0);
            this.aLink4.Visible = (nextP1 <= totalPage);
            this.aLink5.Visible = (nextP2 <= totalPage);

            this.ltPager.Text = $"共 {this.TotalSize } 筆， 共 {totalPage}頁，目前在第{this.GetCurrentPage()}頁<br/>";

        }
        public void BindWithSerach()
        {
            //檢查一頁筆數
            if (this.PageSize <= 0)                                //總頁數小於零 顯示錯誤訊息
            {
                throw new DivideByZeroException();
            }
            //算總頁數
            int totalPage = this.TotalSize / this.PageSize;        //總頁數等於總資料筆數/一頁筆數
            if (this.TotalSize % this.PageSize > 0)                //資料筆數非一頁筆數的整倍數時頁數加1
            {
                totalPage += 1;
            }

            this.aLinkFirst.HRef = $"{this.Url}?page=1&Search={txtSearch}&StartDate={StartDate}&EndDate={EndDate}";           //設定起點頁、預設永遠都有一頁
            this.aLinkLast.HRef = $"{this.Url}?page={totalPage}&Search={txtSearch}&StartDate={StartDate}&EndDate={EndDate}";  //設定終點頁、為總頁數

            //依目前頁數計算
            this.CurrentPage = this.GetCurrentPage();
            this.ltlCurrent.Text = this.CurrentPage.ToString();

            //}
            //計算頁數
            int prevM1 = this.CurrentPage - 1;
            int prevM2 = this.CurrentPage - 2;
            int nextP1 = this.CurrentPage + 1;
            int nextP2 = this.CurrentPage + 2;


            this.aLink2.HRef = $"{this.Url}?page={prevM1}&Search={txtSearch}&StartDate={StartDate}&EndDate={EndDate}";
            this.aLink2.InnerText = prevM1.ToString();              //修正頁數文字
            this.aLink1.HRef = $"{this.Url}?page={prevM2}&Search={txtSearch}&StartDate={StartDate}&EndDate={EndDate}";
            this.aLink1.InnerText = prevM2.ToString();



            this.aLink4.HRef = $"{this.Url}?page={nextP1}&Search={txtSearch}&StartDate={StartDate}&EndDate={EndDate}";
            this.aLink4.InnerText = nextP1.ToString();
            this.aLink5.HRef = $"{this.Url}?page={nextP2}&Search={txtSearch}&StartDate={StartDate}&EndDate={EndDate}";
            this.aLink5.InnerText = nextP2.ToString();

            //依頁數，決定是否隱藏超連結，並處理提示文字

            this.aLink1.Visible = (prevM2 > 0);
            this.aLink2.Visible = (prevM1 > 0);
            this.aLink4.Visible = (nextP1 <= totalPage);
            this.aLink5.Visible = (nextP2 <= totalPage);

            this.ltPager.Text = $"共 {this.TotalSize } 筆， 共 {totalPage}頁，目前在第{this.GetCurrentPage()}頁<br/>";

        }

    }
}