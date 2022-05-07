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
using BSLib.Design.MVP.Controls;
using GDModel;
using GKCore.MVP;
using GKCore.MVP.Views;
using GKCore.Operations;
using GKCore.Types;

namespace GKCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FamilyEditDlgController : DialogController<IFamilyEditDlg>
    {
        private GDMFamilyRecord fFamily;

        public GDMFamilyRecord Family
        {
            get { return fFamily; }
            set {
                if (fFamily != value) {
                    fFamily = value;
                    UpdateView();
                }
            }
        }


        public FamilyEditDlgController(IFamilyEditDlg view) : base(view)
        {
            for (GDMRestriction res = GDMRestriction.rnNone; res <= GDMRestriction.rnLast; res++) {
                fView.Restriction.Add(LangMan.LS(GKData.Restrictions[(int)res]));
            }

            for (int i = 0; i < GKData.MarriageStatus.Length; i++) {
                fView.MarriageStatus.Add(LangMan.LS(GKData.MarriageStatus[i].Name));
            }
        }

        public void SetTarget(TargetMode targetType, GDMIndividualRecord target)
        {
            if (targetType == TargetMode.tmNone || target == null) return;

            bool result = false;
            switch (targetType) {
                case TargetMode.tmSpouse:
                    result = fLocalUndoman.DoOrdinaryOperation(OperationType.otFamilySpouseAttach, fFamily, target);
                    break;
                case TargetMode.tmFamilyChild:
                    result = fLocalUndoman.DoOrdinaryOperation(OperationType.otIndividualParentsAttach, target, fFamily);
                    break;
            }

            if (result) UpdateControls();
        }

        public override bool Accept()
        {
            try {
                fFamily.Status = (GDMMarriageStatus)fView.MarriageStatus.SelectedIndex;
                fFamily.Restriction = (GDMRestriction)fView.Restriction.SelectedIndex;

                fBase.Context.ProcessFamily(fFamily);

                fLocalUndoman.Commit();

                fBase.NotifyRecord(fFamily, RecordAction.raEdit);

                return true;
            } catch (Exception ex) {
                Logger.WriteError("FamilyEditDlgController.Accept()", ex);
                return false;
            }
        }

        public override void UpdateView()
        {
            try {
                fView.ChildrenList.ListModel.DataOwner = fFamily;
                fView.EventsList.ListModel.DataOwner = fFamily;
                fView.NotesList.ListModel.DataOwner = fFamily;
                fView.MediaList.ListModel.DataOwner = fFamily;
                fView.SourcesList.ListModel.DataOwner = fFamily;

                if (fFamily == null) {
                    fView.MarriageStatus.Enabled = false;
                    fView.MarriageStatus.SelectedIndex = 0;
                    fView.Restriction.SelectedIndex = 0;
                } else {
                    fView.MarriageStatus.Enabled = true;
                    fView.MarriageStatus.SelectedIndex = (int)fFamily.Status;
                    fView.Restriction.SelectedIndex = (sbyte)fFamily.Restriction;
                }

                UpdateControls();
            } catch (Exception ex) {
                Logger.WriteError("FamilyEditDlgController.SetFamily()", ex);
            }
        }

        private void UpdateControls()
        {
            GDMIndividualRecord husband, wife;

            if (fFamily == null) {
                husband = null;
                wife = null;

                fView.LockEditor(true);
            } else {
                fBase.Context.Tree.GetSpouses(fFamily, out husband, out wife);

                fView.LockEditor(fFamily.Restriction == GDMRestriction.rnLocked);
            }

            fView.SetHusband((husband != null) ? GKUtils.GetNameString(husband, true, false) : null);
            fView.SetWife((wife != null) ? GKUtils.GetNameString(wife, true, false) : null);

            fView.ChildrenList.UpdateSheet();
            fView.EventsList.UpdateSheet();
            fView.NotesList.UpdateSheet();
            fView.MediaList.UpdateSheet();
            fView.SourcesList.UpdateSheet();
        }

        public void AddHusband()
        {
            if (BaseController.AddFamilyHusband(fBase, fLocalUndoman, fFamily)) {
                UpdateControls();
            }
        }

        public void DeleteHusband()
        {
            if (BaseController.DeleteFamilyHusband(fBase, fLocalUndoman, fFamily)) {
                UpdateControls();
            }
        }

        public void AddWife()
        {
            if (BaseController.AddFamilyWife(fBase, fLocalUndoman, fFamily)) {
                UpdateControls();
            }
        }

        public void DeleteWife()
        {
            if (BaseController.DeleteFamilyWife(fBase, fLocalUndoman, fFamily)) {
                UpdateControls();
            }
        }

        public void JumpToRecord(GDMRecord record)
        {
            if (record != null && Accept()) {
                fBase.SelectRecordByXRef(record.XRef, true);
                fView.Close();
            }
        }

        public void JumpToRecord(GDMPointer pointer)
        {
            if (pointer != null && Accept()) {
                fBase.SelectRecordByXRef(pointer.XRef, true);
                fView.Close();
            }
        }

        public void JumpToHusband()
        {
            JumpToRecord(fFamily.Husband);
        }

        public void JumpToWife()
        {
            JumpToRecord(fFamily.Wife);
        }

        public override void SetLocale()
        {
            GetControl<IButton>("btnAccept").Text = LangMan.LS(LSID.LSID_DlgAccept);
            GetControl<IButton>("btnCancel").Text = LangMan.LS(LSID.LSID_DlgCancel);
            GetControl<IGroupBox>("GroupBox1").Text = LangMan.LS(LSID.LSID_Family);
            GetControl<ILabel>("lblHusband").Text = LangMan.LS(LSID.LSID_Husband);
            GetControl<ILabel>("lblWife").Text = LangMan.LS(LSID.LSID_Wife);
            GetControl<ILabel>("lblStatus").Text = LangMan.LS(LSID.LSID_Status);
            GetControl<ITabPage>("pageChilds").Text = LangMan.LS(LSID.LSID_Childs);
            GetControl<ITabPage>("pageEvents").Text = LangMan.LS(LSID.LSID_Events);
            GetControl<ITabPage>("pageNotes").Text = LangMan.LS(LSID.LSID_RPNotes);
            GetControl<ITabPage>("pageMultimedia").Text = LangMan.LS(LSID.LSID_RPMultimedia);
            GetControl<ITabPage>("pageSources").Text = LangMan.LS(LSID.LSID_RPSources);
            GetControl<ILabel>("lblRestriction").Text = LangMan.LS(LSID.LSID_Restriction);

            SetToolTip("btnHusbandAdd", LangMan.LS(LSID.LSID_HusbandAddTip));
            SetToolTip("btnHusbandDelete", LangMan.LS(LSID.LSID_HusbandDeleteTip));
            SetToolTip("btnHusbandSel", LangMan.LS(LSID.LSID_HusbandSelTip));
            SetToolTip("btnWifeAdd", LangMan.LS(LSID.LSID_WifeAddTip));
            SetToolTip("btnWifeDelete", LangMan.LS(LSID.LSID_WifeDeleteTip));
            SetToolTip("btnWifeSel", LangMan.LS(LSID.LSID_WifeSelTip));
        }
    }
}
