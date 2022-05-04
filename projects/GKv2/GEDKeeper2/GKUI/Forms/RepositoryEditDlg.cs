﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2022 by Sergey V. Zhdanovskih.
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
using System.Windows.Forms;
using BSLib.Design.MVP.Controls;
using GDModel;
using GKCore.Controllers;
using GKCore.Interfaces;
using GKCore.Lists;
using GKCore.MVP.Views;
using GKUI.Components;

namespace GKUI.Forms
{
    public sealed partial class RepositoryEditDlg : EditorDialog, IRepositoryEditDlg
    {
        private readonly RepositoryEditDlgController fController;

        private readonly GKSheetList fNotesList;

        public GDMRepositoryRecord Repository
        {
            get { return fController.Repository; }
            set { fController.Repository = value; }
        }

        #region View Interface

        ISheetList IRepositoryEditDlg.NotesList
        {
            get { return fNotesList; }
        }

        ITextBox IRepositoryEditDlg.Name
        {
            get { return GetControlHandler<ITextBox>(txtName); }
        }

        #endregion

        public RepositoryEditDlg(IBaseWindow baseWin)
        {
            InitializeComponent();

            btnAccept.Image = UIHelper.LoadResourceImage("Resources.btn_accept.gif");
            btnCancel.Image = UIHelper.LoadResourceImage("Resources.btn_cancel.gif");

            fNotesList = new GKSheetList(pageNotes);

            fController = new RepositoryEditDlgController(this);
            fController.Init(baseWin);

            fNotesList.ListModel = new NoteLinksListModel(baseWin, fController.LocalUndoman);
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            fController.ModifyAddress();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogResult = fController.Accept() ? DialogResult.OK : DialogResult.None;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = fController.Cancel() ? DialogResult.Cancel : DialogResult.None;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = fController.CheckChangesPersistence();
        }
    }
}
