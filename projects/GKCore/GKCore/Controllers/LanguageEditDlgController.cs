﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2023 by Sergey V. Zhdanovskih.
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
using GDModel;
using GDModel.Providers.GEDCOM;
using GKCore.Design.Controls;
using GKCore.Design;
using GKCore.Design.Views;

namespace GKCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageEditDlgController : DialogController<ILanguageEditDlg>
    {
        private GDMLanguageID fLanguageID;

        public GDMLanguageID LanguageID
        {
            get { return fLanguageID; }
            set {
                if (fLanguageID != value) {
                    fLanguageID = value;
                    UpdateView();
                }
            }
        }

        public LanguageEditDlgController(ILanguageEditDlg view) : base(view)
        {
            for (var lid = GDMLanguageID.Unknown; lid < GDMLanguageID.Yiddish; lid++) {
                fView.LanguageCombo.AddItem(GEDCOMUtils.GetLanguageStr(lid), lid);
            }
        }

        public override bool Accept()
        {
            try {
                fLanguageID = fView.LanguageCombo.GetSelectedTag<GDMLanguageID>();
                return true;
            } catch (Exception ex) {
                Logger.WriteError("LanguageEditDlgController.Accept()", ex);
                return false;
            }
        }

        public override bool Cancel()
        {
            return true;
        }

        public override void UpdateView()
        {
            fView.LanguageCombo.Text = GEDCOMUtils.GetLanguageStr(fLanguageID);
        }

        public override void SetLocale()
        {
            fView.Title = LangMan.LS(LSID.LSID_Language);

            GetControl<IButton>("btnAccept").Text = LangMan.LS(LSID.LSID_DlgAccept);
            GetControl<IButton>("btnCancel").Text = LangMan.LS(LSID.LSID_DlgCancel);
            GetControl<ILabel>("lblLanguage").Text = LangMan.LS(LSID.LSID_Language);
        }
    }
}
