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
using BSLib;
using GDModel;
using GKCore.Design.Controls;
using GKCore.Design;
using GKCore.Design.Views;
using GKCore.Options;
using GKCore.Search;

namespace GKCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FARDlgController : DialogController<IFARDlg>
    {
        private FARParameters fParameters;
        private FARStrategy fStrategy;


        public FARDlgController(IFARDlg view) : base(view)
        {
        }

        public override void SetLocale()
        {
            fView.Title = LangMan.LS(LSID.LSID_FindAndReplace);

            GetControl<ILabel>("lblPattern").Text = LangMan.LS(LSID.LSID_Find);
            GetControl<ILabel>("lblReplacement").Text = LangMan.LS(LSID.LSID_ReplaceWith);

            GetControl<ICheckBox>("chkMatchCase").Text = LangMan.LS(LSID.LSID_MatchCase);
            GetControl<ICheckBox>("chkMatchWildcards").Text = LangMan.LS(LSID.LSID_MatchWildcards);
            GetControl<ICheckBox>("chkWholeWord").Text = LangMan.LS(LSID.LSID_WholeWord);

            GetControl<IGroupBox>("gbFilters").Text = LangMan.LS(LSID.LSID_MIFilter);
            GetControl<ILabel>("lblRecord").Text = LangMan.LS(LSID.LSID_Record);
            GetControl<ILabel>("lblProperty").Text = LangMan.LS(LSID.LSID_Property);

            GetControl<IButton>("btnPrev").Text = LangMan.LS(LSID.LSID_Prev);
            GetControl<IButton>("btnNext").Text = LangMan.LS(LSID.LSID_Next);
            GetControl<IButton>("btnReplace").Text = LangMan.LS(LSID.LSID_Replace);
            GetControl<IButton>("btnReplaceAll").Text = LangMan.LS(LSID.LSID_ReplaceAll);

            var cmbRecord = GetControl<IComboBox>("cmbRecord");
            for (var rt = GDMRecordType.rtIndividual; rt < GDMRecordType.rtFamily; rt++) {
                cmbRecord.AddItem(LangMan.LS(GKData.RecordTypes[(int)rt].Name), rt);
            }

            var cmbProperty = GetControl<IComboBox>("cmbProperty");
            for (var pt = FARPropertyType.ptName; pt <= FARPropertyType.ptLast; pt++) {
                var propStruct = GKData.FARPropertyTypes[(int)pt];
                if (propStruct.Enabled) {
                    cmbProperty.AddItem(LangMan.LS(propStruct.Name), pt);
                }
            }
        }

        private void CheckListValue(StringList list, string value)
        {
            string val = value.Trim();
            if (!string.IsNullOrEmpty(val) && list.IndexOf(val) < 0) {
                list.Add(val);
            }
        }

        public override void UpdateView()
        {
            GetControl<IComboBox>("cmbPattern").AddStrings(GlobalOptions.Instance.FARPatterns);
            GetControl<IComboBox>("cmbReplacement").AddStrings(GlobalOptions.Instance.FARReplacements);

            GetControl<ICheckBox>("chkWholeWord").Enabled = false; // TODO: next version

            GetControl<IComboBox>("cmbRecord").SetSelectedTag(GDMRecordType.rtIndividual);
            GetControl<IComboBox>("cmbRecord").Enabled = false; // TODO: next version

            GetControl<IComboBox>("cmbProperty").SetSelectedTag(FARPropertyType.ptName);
        }

        private void GetParameters()
        {
            bool paramsChanged = false;

            var pattern = GetControl<IComboBox>("cmbPattern").Text;
            if (fParameters.Pattern != pattern) paramsChanged = true;

            var replacement = GetControl<IComboBox>("cmbReplacement").Text;
            if (fParameters.Replacement != replacement) paramsChanged = true;

            var matchCase = GetControl<ICheckBox>("chkMatchCase").Checked;
            if (fParameters.MatchCase != matchCase) paramsChanged = true;

            var matchWildcards = GetControl<ICheckBox>("chkMatchWildcards").Checked;
            if (fParameters.MatchWildcards != matchWildcards) paramsChanged = true;

            var wholeWord = GetControl<ICheckBox>("chkWholeWord").Checked;
            if (fParameters.WholeWord != wholeWord) paramsChanged = true;

            var recordType = GetControl<IComboBox>("cmbRecord").GetSelectedTag<GDMRecordType>();
            if (fParameters.RecordType != recordType) paramsChanged = true;

            var propertyType = GetControl<IComboBox>("cmbProperty").GetSelectedTag<FARPropertyType>();
            if (fParameters.PropertyType != propertyType) paramsChanged = true;

            if (paramsChanged) {
                fParameters.Pattern = pattern;
                fParameters.Replacement = replacement;
                fParameters.MatchCase = matchCase;
                fParameters.MatchWildcards = matchWildcards;
                fParameters.WholeWord = wholeWord;
                fParameters.RecordType = recordType;
                fParameters.PropertyType = propertyType;
                fStrategy = new FARStrategy(fBase, fParameters);
            }

            // save values
            CheckListValue(GlobalOptions.Instance.FARPatterns, pattern);
            CheckListValue(GlobalOptions.Instance.FARReplacements, replacement);
        }

        private void SelectResult(SearchResult result)
        {
            if (result != null && result.Record != null) {
                fBase.SelectByRec(result.Record);
            }
        }

        private bool RequireResults()
        {
            GetParameters();

            if (fStrategy == null || !fStrategy.HasResults()) {
                AppHost.StdDialogs.ShowError(LangMan.LS(LSID.LSID_NoMatchesFound));
                return false;
            }

            return true;
        }

        public void Prev()
        {
            try {
                if (!RequireResults()) return;
                SelectResult(fStrategy.FindPrev() as SearchResult);
            } catch (Exception ex) {
                Logger.WriteError("FARDlgController.Prev()", ex);
            }
        }

        public void Next()
        {
            try {
                if (!RequireResults()) return;
                SelectResult(fStrategy.FindNext() as SearchResult);
            } catch (Exception ex) {
                Logger.WriteError("FARDlgController.Next()", ex);
            }
        }

        public void Replace()
        {
            try {
                if (!RequireResults()) return;
                fStrategy.ReplaceCurrent();
            } catch (Exception ex) {
                Logger.WriteError("FARDlgController.Replace()", ex);
            }
        }

        public void ReplaceAll()
        {
            try {
                if (!RequireResults()) return;
                fStrategy.ReplaceAll();
            } catch (Exception ex) {
                Logger.WriteError("FARDlgController.ReplaceAll()", ex);
            }
        }
    }
}
