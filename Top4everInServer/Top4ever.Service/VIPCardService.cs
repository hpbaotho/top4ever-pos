using System;
using System.Collections;
using System.Collections.Generic;

using IBatisNet.Common.Logging;
using IBatisNet.DataAccess;

using Top4ever.Common;
using Top4ever.Domain.MembershipCard;
using Top4ever.Interface.MembershipCard;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for VIPCardService.
    /// </summary>
    public class VIPCardService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(VIPCardService));

        #region Private Fields

        private static VIPCardService _instance = new VIPCardService();
        private IDaoManager _daoManager = null;
        private IVIPCardDao _VIPCardDao = null;

        #endregion

        #region Constructor

        private VIPCardService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _VIPCardDao = _daoManager.GetDao(typeof(IVIPCardDao)) as IVIPCardDao;
        }

        #endregion

        #region Public methods

        public static VIPCardService GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 获取会员卡信息 0:数据库操作失败, 1:成功, 2:会员卡号或者密码错误
        /// </summary>
        public int GetVIPCard(string cardNo, string password, out VIPCard card)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                //加密的密码
                string saltedPassword = _VIPCardDao.GetCardPassword(cardNo);
                int index = saltedPassword.IndexOf("*");
                string salt = saltedPassword.Substring(0, index);
                string str = salt + "*" + PasswordCryptographer.SaltPassword(password, salt);
                card = _VIPCardDao.GetVIPCard(cardNo, str);
                if (card == null)
                {
                    result = 2;
                }
                else
                {
                    result = 1;
                }
                _daoManager.CloseConnection();
            }
            catch (Exception ex)
            {
                card = null;
                result = 0;
                logger.Error("Database operation failed !", ex);
            }
            return result;
        }

        public string GetCardPassword(string cardNo)
        {
            string password = string.Empty;
            _daoManager.OpenConnection();
            password = _VIPCardDao.GetCardPassword(cardNo);
            _daoManager.CloseConnection();
            return password;
        }

        public bool UpdateCardPassword(string cardNo, string currentPassword, string newPassword)
        {
            bool result = false;
            _daoManager.OpenConnection();
            //加密的密码
            string saltedPassword = _VIPCardDao.GetCardPassword(cardNo);
            int index = saltedPassword.IndexOf("*");
            string salt = saltedPassword.Substring(0, index);
            string str = salt + "*" + PasswordCryptographer.SaltPassword(currentPassword, salt);
            string newSaltedPassword = PasswordCryptographer.GenerateSaltedPassword(newPassword);
            result = _VIPCardDao.UpdateCardPassword(cardNo, str, newSaltedPassword);
            _daoManager.CloseConnection();
            return result;
        }

        #endregion
    }
}
