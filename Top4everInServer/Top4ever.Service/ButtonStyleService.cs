using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for ButtonStyleService.
    /// </summary>
    public class ButtonStyleService
    {
        #region Private Fields

        private static ButtonStyleService _instance = new ButtonStyleService();
        private IDaoManager _daoManager = null;
        private IButtonStyleDao _buttonStyleDao = null;

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

            _daoManager.OpenConnection();
            buttonStyleList = _buttonStyleDao.GetButtonStyleList();
            _daoManager.CloseConnection();

            return buttonStyleList;
        }

        #endregion
    }
}
