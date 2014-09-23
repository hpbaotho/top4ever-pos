using System;

using IBatisNet.DataAccess;

using Top4ever.Domain.MembershipCard;
using Top4ever.Interface.MembershipCard;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for VIPCardService.
    /// </summary>
    public class VIPCardService
    {
        #region Private Fields

        private const char Delim = '*';

        private static VIPCardService _instance = new VIPCardService();
        private readonly IDaoManager _daoManager;
        private readonly IVIPCardDao _vipCardDao;

        #endregion

        #region Constructor

        private VIPCardService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _vipCardDao = _daoManager.GetDao(typeof(IVIPCardDao)) as IVIPCardDao;
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
                string str = string.Empty;
                string saltedPassword = _vipCardDao.GetCardPassword(cardNo);
                if (!string.IsNullOrEmpty(saltedPassword))
                {
                    int index = saltedPassword.IndexOf(Delim);
                    string salt = saltedPassword.Substring(0, index);
                    str = salt + Delim + PasswordCryptographer.SaltPassword(password, salt);
                }
                card = _vipCardDao.GetVIPCard(cardNo, str);
                result = card == null ? 2 : 1;
            }
            catch (Exception exception)
            {
                card = null;
                result = 0;
                LogHelper.GetInstance().Error(string.Format("[GetVIPCard]参数：cardNo_{0}", cardNo), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public string GetCardPassword(string cardNo)
        {
            string password;
            try
            {
                _daoManager.OpenConnection();
                password = _vipCardDao.GetCardPassword(cardNo);
            }
            catch (Exception exception)
            {
                password = string.Empty;
                LogHelper.GetInstance().Error(string.Format("[GetCardPassword]参数：cardNo_{0}", cardNo), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return password;
        }

        public decimal GetCardDiscountRate(string cardNo)
        {
            decimal discountRate = 0M;
            try
            {
                _daoManager.OpenConnection();
                discountRate = _vipCardDao.GetCardDiscountRate(cardNo);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetCardDiscountRate]参数：cardNo_{0}", cardNo), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return discountRate;
        }

        public Int32 UpdateCardPassword(string cardNo, string currentPassword, string newPassword)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                //加密的密码
                string str = string.Empty;
                string saltedPassword = _vipCardDao.GetCardPassword(cardNo);
                if (!string.IsNullOrEmpty(saltedPassword))
                {
                    int index = saltedPassword.IndexOf(Delim);
                    string salt = saltedPassword.Substring(0, index);
                    str = salt + Delim + PasswordCryptographer.SaltPassword(currentPassword, salt);
                }
                string newSaltedPassword = PasswordCryptographer.GenerateSaltedPassword(newPassword);
                result = _vipCardDao.UpdateVIPCardPassword(cardNo, str, newSaltedPassword);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateCardPassword]参数：cardNo_{0}", cardNo), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public Int32 UpdateVIPCardStatus(string cardNo, string password, int status)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                //加密的密码
                string str = string.Empty;
                if (status == 1)
                {
                    str = PasswordCryptographer.GenerateSaltedPassword(password);
                }
                else
                {
                    string saltedPassword = _vipCardDao.GetCardPassword(cardNo);
                    if (!string.IsNullOrEmpty(saltedPassword))
                    {
                        int index = saltedPassword.IndexOf(Delim);
                        string salt = saltedPassword.Substring(0, index);
                        str = salt + Delim + PasswordCryptographer.SaltPassword(password, salt);
                    }
                }
                result = _vipCardDao.UpdateVIPCardStatus(cardNo, str, status);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateVIPCardStatus]参数：cardNo_{0},status_{1}", cardNo, status), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        #endregion
    }
}
