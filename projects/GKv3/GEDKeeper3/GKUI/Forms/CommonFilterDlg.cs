﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2020 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using Eto.Forms;
using GKCore;
using GKCore.Controllers;
using GKCore.Interfaces;
using GKCore.MVP.Controls;
using GKCore.MVP.Views;
using GKUI.Components;

namespace GKUI.Forms
{
    public partial class CommonFilterDlg : CommonDialog, ICommonFilterDlg
    {
        private readonly CommonFilterDlgController fController;

        private readonly IBaseWindow fBase;
        private readonly IListManager fListMan;

        private FilterGridView filterView;

        public IBaseWindow Base
        {
            get { return fBase; }
        }

        public IListManager ListMan
        {
            get { return fListMan; }
        }

        #region View Interface

        public IFilterGridView FilterGrid
        {
            get { return filterView; }
        }

        #endregion

        public CommonFilterDlg()
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("Resources.btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("Resources.btn_cancel.gif");
        }

        public CommonFilterDlg(IBaseWindow baseWin, IListManager listMan) : this()
        {
            if (baseWin == null)
                throw new ArgumentNullException("baseWin");

            if (listMan == null)
                throw new ArgumentNullException("listMan");

            fBase = baseWin;
            fListMan = listMan;
            fController = new CommonFilterDlgController(this, listMan);
            fController.Init(baseWin);

            filterView = new FilterGridView(fListMan);
            filterView.Height = 260;
            tsFieldsFilter.Content = filterView;

            SetLocale();

            fController.UpdateView();
            this.KeyDown += Form_KeyDown;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try {
                AcceptChanges();
                DialogResult = DialogResult.Ok;
            } catch (Exception ex) {
                Logger.WriteError("CommonFilterDlg.btnAccept_Click()", ex);
                DialogResult = DialogResult.None;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DoReset();
        }

        public virtual void AcceptChanges()
        {
            fController.Accept();
            DialogResult = DialogResult.Ok;
        }

        public void SetLocale()
        {
            GKData.CondSigns[6] = LangMan.LS(LSID.LSID_CondContains);
            GKData.CondSigns[7] = LangMan.LS(LSID.LSID_CondNotContains);

            btnAccept.Text = LangMan.LS(LSID.LSID_DlgAccept);
            btnCancel.Text = LangMan.LS(LSID.LSID_DlgCancel);
            btnReset.Text = LangMan.LS(LSID.LSID_DlgReset);
            tsFieldsFilter.Text = LangMan.LS(LSID.LSID_FieldsFilter);
        }

        public virtual void DoReset()
        {
            fListMan.Filter.Clear();
            fController.UpdateView();
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) {
                case Keys.I:
                    if (e.Control) {
                        FilterCondition fcond = new FilterCondition(0, ConditionKind.ck_Contains, "");
                        filterView.AddCondition(fcond);
                    }
                    break;

                case Keys.D:
                    if (e.Control) {
                        filterView.RemoveCondition(filterView.SelectedRow);
                    }
                    break;
            }
        }
    }
}
