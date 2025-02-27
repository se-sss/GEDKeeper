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
using System.Collections.Generic;
using System.IO;
using BSLib;
using GDModel;
using GKCore.Design;
using GKCore.Design.Graphics;
using GKCore.Types;

namespace GKCore.Interfaces
{
    public interface IBaseContext : IDisposable
    {
        ICulture Culture { get; }
        GDMLanguageID DefaultLanguage { get; set; }
        Dictionary<string, int> EventStats { get; }
        string FileName { get; }
        GDMTree Tree { get; }
        ValuesCollection ValuesCollection { get; }
        ShieldState ShieldState { get; set; }
        bool Modified { get; set; }
        IBaseWindow Viewer { get; }
        List<GDMLanguageID> LangsList { get; }

        bool IsUnknown();
        void Clear();
        bool FileLoad(string fileName, bool showProgress = true);
        bool FileSave(string fileName);
        void SetFileName(string fileName);
        void CriticalSave();

        // Data manipulation
        GDMCustomEvent CreateEventEx(GDMRecordWithEvents aRec, string evSign, string evDate, string evPlace);
        GDMCustomEvent CreateEventEx(GDMRecordWithEvents aRec, string evSign, GDMCustomDate evDate, string evPlace);
        GDMIndividualRecord CreatePersonEx(string iName, string iPatronymic, string iSurname, GDMSex iSex, bool birthEvent);
        bool DeleteRecord(GDMRecord record);
        bool IsRecordAccess(GDMRestriction restriction);

        // Individual utils
        GDMFamilyRecord GetMarriageFamily(GDMIndividualRecord iRec, bool canCreate = false);
        GDMFamilyRecord GetParentsFamily(GDMIndividualRecord iRec, bool canCreate = false);
        bool IsChildless(GDMIndividualRecord iRec);
        int FindBirthYear(GDMIndividualRecord iRec);
        int FindDeathYear(GDMIndividualRecord iRec);
        void CollectEventValues(GDMCustomEvent evt);
        void CollectNameLangs(GDMPersonalName persName);
        void CollectTips(StringList tipsList);
        void ImportNames(GDMIndividualRecord iRec);
        IList<ISearchResult> FindAll(GDMRecordType recordType, string searchPattern);

        // Multimedia support
        string GetArcFileName();
        string GetStgFolder(bool create);
        bool CheckBasePath();
        MediaStore GetStoreType(GDMFileReference fileReference);
        Stream MediaLoad(GDMFileReference fileReference, bool throwException);
        string MediaLoad(GDMFileReference fileReference);
        bool MediaSave(GDMFileReference fileReference, string fileName, MediaStoreType storeType);
        MediaStoreStatus VerifyMediaFile(GDMFileReference fileReference, out string fileName);

        /// <summary>
        /// Loading an image from a multimedia link with the features to get a thumbnail for the trees and cut out a part from the whole.
        /// </summary>
        /// <param name="mmRec"></param>
        /// <param name="thumbWidth">thumbnail width, if <= 0 - then the image width is unchanged</param>
        /// <param name="thumbHeight">thumbnail height, if <= 0 - then the image height is unchanged</param>
        /// <param name="cutoutArea"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        IImage LoadMediaImage(GDMMultimediaRecord mmRec, int thumbWidth, int thumbHeight, ExtRect cutoutArea, bool throwException);

        // Used in FamilyBookExporter, TreeChart and PersonEdit
        IImage GetPrimaryBitmap(GDMIndividualRecord iRec, int thumbWidth, int thumbHeight, bool throwException);
        string GetPrimaryBitmapUID(GDMIndividualRecord iRec);

        bool IsUpdated();
        void BeginUpdate();
        void EndUpdate();
        void SwitchShieldState();

        void DoUndo();
        void DoRedo();
        void DoCommit();
        void DoRollback();

        void LockRecord(GDMRecord record);
        void UnlockRecord(GDMRecord record);
        bool IsAvailableRecord(GDMRecord record);

        GDMSourceRecord FindSource(string sourceName);
        void GetSourcesList(StringList sources);

        string DefinePatronymic(IView owner, string name, GDMSex sex, bool confirm);
        GDMSex DefineSex(IView owner, string iName, string iPatr);
        void CheckPersonSex(IView owner, GDMIndividualRecord iRec);

        GDMFamilyRecord SelectFamily(IView owner, GDMIndividualRecord target, TargetMode targetMode = TargetMode.tmFamilyChild);
        GDMIndividualRecord SelectPerson(IView owner, GDMIndividualRecord target, TargetMode targetMode, GDMSex needSex);
        GDMRecord SelectRecord(IView owner, GDMRecordType mode, params object[] args);
        GDMFamilyRecord GetChildFamily(GDMIndividualRecord iChild,
                                          bool canCreate,
                                          GDMIndividualRecord newParent);
        GDMFamilyRecord AddFamilyForSpouse(GDMIndividualRecord spouse);
        GDMIndividualRecord AddChildForParent(IView owner, GDMIndividualRecord parent, GDMSex needSex);
        GDMIndividualRecord SelectSpouseFor(IView owner, GDMIndividualRecord iRec);

        void ProcessFamily(GDMFamilyRecord famRec);
        void ProcessIndividual(GDMIndividualRecord indiRec);

        bool CopyFile(string sourceFileName, string destFileName, bool showProgress = true);
        void MoveMediaContainers(string oldFileName, string newFileName, bool createCopy = false);

        void IncrementEventStats(string tagName);
    }
}
