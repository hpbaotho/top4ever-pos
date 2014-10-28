namespace Top4ever.Print.Entity
{
    public class PrintMemberCard
    {
        private const string _cardHolder = "持卡人存根";
        private const string _enCardHolder = "CARDHOLDER COPY";

        public string CardHolder
        {
            get { return _cardHolder; }
        }

        public string EnCardHolder 
        {
            get { return _enCardHolder; }
        }

        /// <summary>
        /// 会员凭单
        /// </summary>
        public string MemberVoucher { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string TranSequence { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TradeTime { get; set; }
        /// <summary>
        /// 充值前金额
        /// </summary>
        public string PreTradeAmount { get; set; }
        /// <summary>
        /// 充值后金额
        /// </summary>
        public string PostTradeAmount { get; set; }
        /// <summary>
        /// 消费前金额
        /// </summary>
        public string PreConsumeAmount { get; set; }
        /// <summary>
        /// 消费后金额
        /// </summary>
        public string PostConsumeAmount { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string Payment { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public string StoreValue { get; set; }
        /// <summary>
        /// 赠送金额
        /// </summary>
        public string GivenAmount { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public string PresentExp { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public string ConsumeAmount { get; set; }
        /// <summary>
        /// 消费积分
        /// </summary>
        public string ConsumePoints { get; set; }
        /// <summary>
        /// 可用积分
        /// </summary>
        public string AvailablePoints { get; set; }
        /// <summary>
        /// 最后消费时间
        /// </summary>
        public string LastConsumeTime { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="fieldName">属性字符</param>
        /// <returns></returns>
        public string GetValue(string fieldName)
        {
            string result;
            switch (fieldName.ToLower())
            {
                case "cardholder":
                    result = this.CardHolder;
                    break;
                case "encardholder":
                    result = this.EnCardHolder;
                    break;
                case "membervoucher":
                    result = this.MemberVoucher;
                    break;
                case "cardno":
                    result = this.CardNo;
                    break;
                case "shopname":
                    result = this.ShopName;
                    break;
                case "tradetype":
                    result = this.TradeType;
                    break;
                case "transequence":
                    result = this.TranSequence;
                    break;
                case "tradetime":
                    result = this.TradeTime;
                    break;
                case "pretradeamount":
                    result = this.PreTradeAmount;
                    break;
                case "posttradeamount":
                    result = this.PostTradeAmount;
                    break;
                case "preconsumeamount":
                    result = this.PreConsumeAmount;
                    break;
                case "postconsumeamount":
                    result = this.PostConsumeAmount;
                    break;
                case "payment":
                    result = this.Payment;
                    break;
                case "storevalue":
                    result = this.StoreValue;
                    break;
                case "givenamount":
                    result = this.GivenAmount;
                    break;
                case "presentexp":
                    result = this.PresentExp;
                    break;
                case "consumeamount":
                    result = this.ConsumeAmount;
                    break;
                case "consumepoints":
                    result = this.ConsumePoints;
                    break;
                case "availablepoints":
                    result = this.AvailablePoints;
                    break;
                case "lastconsumetime":
                    result = this.LastConsumeTime;
                    break;
                case "operator":
                    result = this.Operator;
                    break;
                case "remark":
                    result = this.Remark;
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
    }
}
