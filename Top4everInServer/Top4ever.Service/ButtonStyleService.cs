using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Interface;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for ButtonStyleService.
    /// </summary>
    public class ButtonStyleService
    {
        #region Private Fields

        private static readonly ButtonStyleService _instance = new ButtonStyleService();
        private readonly IDaoManager _daoManager;
        private readonly IButtonStyleDao _buttonStyleDao;

        #endregion

        #region Constructor

        private ButtonStyleService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _buttonStyleDao = _daoManager.GetDao(typeof(IButtonStyleDao)) as IButtonStyleDao;
        }

        #endregion

        #region Public methods

        public static ButtonStyleService GetInstance()
        {
            return _instance;
        }

        public IList<ButtonStyle> GetButtonStyleList()
        {
            IList<ButtonStyle> buttonStyleList = null;
            try
            {
                _daoManager.OpenConnection();
                buttonStyleList = _buttonStyleDao.GetButtonStyleList();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetButtonStyleList]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return buttonStyleList;
        }

        #endregion
    }
}
