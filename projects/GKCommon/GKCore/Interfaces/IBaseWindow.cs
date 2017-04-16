﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2017 by Sergey V. Zhdanovskih.
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
using System.Collections.Generic;

using GKCommon;
using GKCommon.GEDCOM;
using GKCore.Types;

namespace GKCore.Interfaces
{
    public interface IBaseWindow : IWorkWindow, ILocalization
    {
        IHost Host { get; }
        IBaseContext Context { get; }

        bool Modified { get; set; }
        ShieldState ShieldState { get; set; }

        void Activate();
        void ApplyFilter(GEDCOMRecordType recType = GEDCOMRecordType.rtNone);
        void ChangeRecord(GEDCOMRecord record);
        void Close();

        bool IsUnknown();
        void CreateNewFile();
        void LoadFile(string fileName);
        void SaveFile(string fileName);

        List<GEDCOMRecord> GetContentList(GEDCOMRecordType recType);
        StringList GetRecordContent(GEDCOMRecord record);
        string GetRecordName(GEDCOMRecord record, bool signed);
        IListManager GetRecordsListManByType(GEDCOMRecordType recType);
        GEDCOMIndividualRecord GetSelectedPerson();
        GEDCOMRecordType GetSelectedRecordType();
        void RefreshLists(bool titles);
        //void RefreshRecordsView(GEDCOMRecordType recType);
        void ShowRecordsTab(GEDCOMRecordType recType);

        void AddRecord();
        void DeleteRecord();
        void EditRecord();
        bool RecordIsFiltered(GEDCOMRecord record);
        void NotifyRecord(GEDCOMRecord record, RecordAction action);

        void SelectRecordByXRef(string xref);
        void Show();
        void ShowMedia(GEDCOMMultimediaRecord mediaRec, bool modal);
    }
}
