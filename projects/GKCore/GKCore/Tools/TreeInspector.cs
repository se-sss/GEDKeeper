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
using System.Collections.Generic;
using BSLib;
using GDModel;
using GDModel.Providers.GEDCOM;
using GKCore.Controllers;
using GKCore.Interfaces;
using GKCore.Types;

namespace GKCore.Tools
{
    public class TreeInspectionOptions
    {
        public bool CheckIndividualPlaces;
    }

    /// <summary>
    ///
    /// </summary>
    public static class TreeInspector
    {
        #region Base Checks

        public enum CheckDiag
        {
            cdPersonLonglived,
            cdPersonSexless,
            cdLiveYearsInvalid,
            cdStrangeSpouse,
            cdStrangeParent,
            cdEmptyFamily,
            cdFatherAsChild,
            cdMotherAsChild,
            cdDuplicateChildren,
            csDateInvalid,
            csCycle,
            cdChildWithoutParents,
            cdFamilyRecordWithoutFamily,
            cdMediaRecordWithoutFiles,
            cdStgNotFound,
            cdArcNotFound,
            cdFileNotFound,
            cdHalfSpsFamLink,
            cdHalfChdFamLink,
            cdHalfFamHusbLink,
            cdHalfFamWifeLink,
            cdHalfFamChldLink,
            cdGarbledSpouses,
            cdSeveralParents,
            cdUnknownPlaceOfPerson,
            cdHighSpousesDifference,
            cdHighSiblingsDifference,
        }

        public enum CheckSolve
        {
            csSkip,
            csSetIsDead,
            csDefineSex,
            csRemove,
            csEdit,
            csRepair,
        }

        public sealed class CheckObj
        {
            public string Comment;
            public CheckDiag Diag;
            public GDMRecord Rec;
            public CheckSolve Solve;
            public GDMRecord Target;

            public CheckObj(GDMRecord rec, CheckDiag diag, CheckSolve solve)
            {
                Rec = rec;
                Diag = diag;
                Solve = solve;
            }

            public CheckObj(GDMRecord rec, GDMRecord target, CheckDiag diag, CheckSolve solve)
            {
                Rec = rec;
                Diag = diag;
                Solve = solve;
                Target = target;
            }

            public string GetRecordName(GDMTree tree)
            {
                string result = string.Empty;

                switch (Rec.RecordType) {
                    case GDMRecordType.rtIndividual:
                        result = GKUtils.GetNameString(((GDMIndividualRecord)Rec), true, false);
                        break;

                    case GDMRecordType.rtFamily:
                        result = GKUtils.GetFamilyString(tree, (GDMFamilyRecord)Rec);
                        break;
                }

                result = string.Concat(result, " [ ", Rec.XRef, " ]");

                return result;
            }
        }

        private static void CheckRecordWithEvents(GDMRecordWithEvents rec, List<CheckObj> checksList)
        {
            var dateZero = new DateTime(0);

            int num = rec.Events.Count;
            for (int i = 0; i < num; i++) {
                GDMCustomEvent evt = rec.Events[i];

                bool invalid = false;
                try {
                    var dtx = evt.Date.GetDateTime();

                    /*if (dtx == dateZero) {
                        invalid = true;
                    }*/
                } catch {
                    invalid = true;
                }

                if (invalid) {
                    CheckObj checkObj = new CheckObj(rec, CheckDiag.csDateInvalid, CheckSolve.csEdit);
                    checkObj.Comment = LangMan.LS(LSID.LSID_DateInvalid) + " (" + evt.Date.StringValue + ")";
                    checksList.Add(checkObj);
                }
            }
        }

        private static void CheckIndividualLinks(GDMTree tree, GDMIndividualRecord iRec, List<CheckObj> checksList)
        {
            for (int i = iRec.ChildToFamilyLinks.Count - 1; i >= 0; i--) {
                var cfl = iRec.ChildToFamilyLinks[i];
                if (cfl == null) {
                    iRec.ChildToFamilyLinks.DeleteAt(i);
                    continue;
                }

                GDMFamilyRecord family = tree.GetPtrValue(cfl);
                if (family == null) {
                    iRec.ChildToFamilyLinks.DeleteAt(i);
                    continue;
                }

                if (!family.HasChild(iRec)) {
                    var checkObj = new CheckObj(iRec, family, CheckDiag.cdHalfChdFamLink, CheckSolve.csRepair);
                    checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_PersonHasHalfLinkOfChildToFamily), iRec.XRef, family.XRef);
                    checksList.Add(checkObj);
                }
            }

            if (iRec.ChildToFamilyLinks.Count > 1) {
                var checkObj = new CheckObj(iRec, CheckDiag.cdSeveralParents, CheckSolve.csSkip);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_SeveralFamiliesOfParents), iRec.XRef);
                checksList.Add(checkObj);
            }

            for (int i = iRec.SpouseToFamilyLinks.Count - 1; i >= 0; i--) {
                var sfl = iRec.SpouseToFamilyLinks[i];
                if (sfl == null) {
                    iRec.SpouseToFamilyLinks.DeleteAt(i);
                    continue;
                }

                GDMFamilyRecord family = tree.GetPtrValue(sfl);
                if (family == null) {
                    iRec.SpouseToFamilyLinks.DeleteAt(i);
                    continue;
                }

                if (!family.HasSpouse(iRec)) {
                    var checkObj = new CheckObj(iRec, family, CheckDiag.cdHalfSpsFamLink, CheckSolve.csRepair);
                    checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_PersonHasHalfLinkOfSpouseToFamily), iRec.XRef, family.XRef);
                    checksList.Add(checkObj);
                }
            }
        }

        private static void CheckFamilyLinks(GDMTree tree, GDMFamilyRecord fRec, List<CheckObj> checksList)
        {
            var husb = tree.GetPtrValue<GDMIndividualRecord>(fRec.Husband);
            if (husb != null && husb.IndexOfSpouse(fRec) < 0) {
                CheckObj checkObj = new CheckObj(fRec, husb, CheckDiag.cdHalfFamHusbLink, CheckSolve.csRepair);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_FamilyHasHalfLinkOfHusbandToPerson), fRec.XRef, husb.XRef);
                checksList.Add(checkObj);
            }

            var wife = tree.GetPtrValue<GDMIndividualRecord>(fRec.Wife);
            if (wife != null && wife.IndexOfSpouse(fRec) < 0) {
                CheckObj checkObj = new CheckObj(fRec, wife, CheckDiag.cdHalfFamWifeLink, CheckSolve.csRepair);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_FamilyHasHalfLinkOfWifeToPerson), fRec.XRef, wife.XRef);
                checksList.Add(checkObj);
            }

            if ((husb != null && husb.Sex != GDMSex.svMale) || (wife != null && wife.Sex != GDMSex.svFemale)) {
                CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdGarbledSpouses, CheckSolve.csRepair);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_GarbledSpouses), fRec.XRef);
                checksList.Add(checkObj);
            }

            for (int j = fRec.Children.Count - 1; j >= 0; j--) {
                var cl = fRec.Children[j];
                if (cl == null) {
                    fRec.Children.DeleteAt(j);
                    continue;
                }

                GDMIndividualRecord child = tree.GetPtrValue(cl);
                if (child == null) {
                    fRec.Children.DeleteAt(j);
                    continue;
                }

                if (child.FindChildToFamilyLink(fRec) == null) {
                    CheckObj checkObj = new CheckObj(fRec, child, CheckDiag.cdHalfFamChldLink, CheckSolve.csRepair);
                    checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_FamilyHasHalfLinkOfChildToParentsFamily), fRec.XRef, child.XRef);
                    checksList.Add(checkObj);
                }
            }
        }

        private static void CheckIndividualPlaces(GDMIndividualRecord iRec, List<CheckObj> checksList)
        {
            bool hasPlace = false;

            int num = iRec.Events.Count;
            for (int i = 0; i < num; i++) {
                GDMCustomEvent evt = iRec.Events[i];
                if (evt.HasPlace) {
                    hasPlace = true;
                    break;
                }
            }

            if (!hasPlace) {
                CheckObj checkObj = new CheckObj(iRec, CheckDiag.cdUnknownPlaceOfPerson, CheckSolve.csSkip);
                checkObj.Comment = LangMan.LS(LSID.LSID_UnknownPlaceOfPerson);
                checksList.Add(checkObj);
            }
        }

        private static void CheckIndividualRecord(GDMTree tree, GDMIndividualRecord iRec, List<CheckObj> checksList, TreeInspectionOptions options)
        {
            CheckRecordWithEvents(iRec, checksList);

            CheckIndividualLinks(tree, iRec, checksList);

            if (iRec.FindEvent(GEDCOMTagType.DEAT) == null) {
                int age = GKUtils.GetAge(iRec, -1);

                if (age != -1 && age >= GKData.PROVED_LIFE_LENGTH) {
                    CheckObj checkObj = new CheckObj(iRec, CheckDiag.cdPersonLonglived, CheckSolve.csSetIsDead);
                    checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_PersonLonglived), age);
                    checksList.Add(checkObj);
                }
            }

            GDMSex sex = iRec.Sex;
            if (sex < GDMSex.svMale || sex > GDMSex.svFemale) {
                CheckObj checkObj = new CheckObj(iRec, CheckDiag.cdPersonSexless, CheckSolve.csDefineSex);
                checkObj.Comment = LangMan.LS(LSID.LSID_PersonSexless);
                checksList.Add(checkObj);
            }

            int yBirth = iRec.GetChronologicalYear(GEDCOMTagName.BIRT);
            int yDeath = iRec.GetChronologicalYear(GEDCOMTagName.DEAT);
            if (yBirth != 0 && yDeath != 0) {
                int delta = (yDeath - yBirth);
                if (delta < 0) {
                    CheckObj checkObj = new CheckObj(iRec, CheckDiag.cdLiveYearsInvalid, CheckSolve.csSkip);
                    checkObj.Comment = LangMan.LS(LSID.LSID_LiveYearsInvalid);
                    checksList.Add(checkObj);
                }
            }

            int iAge = GKUtils.GetMarriageAge(tree, iRec);
            if (iAge > 0 && (iAge <= GKData.MIN_MARRIAGE_AGE || iAge >= GKData.PROVED_LIFE_LENGTH)) {
                CheckObj checkObj = new CheckObj(iRec, CheckDiag.cdStrangeSpouse, CheckSolve.csSkip);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_StrangeSpouse), iAge.ToString());
                checksList.Add(checkObj);
            }

            iAge = GKUtils.GetFirstbornAge(iRec, GKUtils.GetFirstborn(tree, iRec));
            if (iAge > 0 && ((iAge <= GKData.MIN_PARENT_AGE) || (sex == GDMSex.svFemale && iAge >= GKData.MAX_MOTHER_AGE) || (sex == GDMSex.svMale && iAge >= GKData.MAX_FATHER_AGE))) {
                CheckObj checkObj = new CheckObj(iRec, CheckDiag.cdStrangeParent, CheckSolve.csSkip);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_StrangeParent), iAge.ToString());
                checksList.Add(checkObj);
            }

            string cycle = CheckCycle(tree, iRec);
            if (!string.IsNullOrEmpty(cycle)) {
                CheckObj checkObj = new CheckObj(iRec, CheckDiag.csCycle, CheckSolve.csSkip);
                checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_DetectedDataLoop), cycle);
                checksList.Add(checkObj);
            }

            if (options != null && options.CheckIndividualPlaces) {
                CheckIndividualPlaces(iRec, checksList);
            }
        }

        private static void CheckFamilyRecord(GDMTree tree, GDMFamilyRecord fRec, List<CheckObj> checksList)
        {
            CheckRecordWithEvents(fRec, checksList);

            CheckFamilyLinks(tree, fRec, checksList);

            var husb = tree.GetPtrValue<GDMIndividualRecord>(fRec.Husband);
            var wife = tree.GetPtrValue<GDMIndividualRecord>(fRec.Wife);

            bool empty = (!fRec.HasNotes && !fRec.HasSourceCitations && !fRec.HasMultimediaLinks && !fRec.HasUserReferences);
            empty = empty && (!fRec.HasEvents && fRec.Children.Count == 0);
            empty = empty && (husb == null && wife == null);

            if (empty) {
                CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdEmptyFamily, CheckSolve.csRemove);
                checkObj.Comment = LangMan.LS(LSID.LSID_EmptyFamily);
                checksList.Add(checkObj);
            } else {
                int chNum = fRec.Children.Count;

                if (husb == null && wife == null) {
                    if (chNum > 0) {
                        CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdChildWithoutParents, CheckSolve.csSkip);
                        checkObj.Comment = LangMan.LS(LSID.LSID_ChildWithoutParents);
                        checksList.Add(checkObj);
                    } else {
                        CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdFamilyRecordWithoutFamily, CheckSolve.csSkip);
                        checkObj.Comment = LangMan.LS(LSID.LSID_FamilyRecordWithoutFamily);
                        checksList.Add(checkObj);
                    }
                } else {
                    if (fRec.IndexOfChild(husb) >= 0) {
                        CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdFatherAsChild, CheckSolve.csRemove);
                        checkObj.Comment = LangMan.LS(LSID.LSID_FatherAsChild);
                        checksList.Add(checkObj);
                    }

                    if (fRec.IndexOfChild(wife) >= 0) {
                        CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdMotherAsChild, CheckSolve.csRemove);
                        checkObj.Comment = LangMan.LS(LSID.LSID_MotherAsChild);
                        checksList.Add(checkObj);
                    }

                    int spousesDiff = GKUtils.GetSpousesDiff(tree, fRec);
                    if (spousesDiff > GKData.MAX_SPOUSES_DIFF) {
                        CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdHighSpousesDifference, CheckSolve.csSkip);
                        checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_DifferenceBetweenSpousesIsAbnormallyHigh), spousesDiff);
                        checksList.Add(checkObj);
                    }
                }

                bool hasDup = false;
                int minBirth = int.MaxValue, maxBirth = int.MinValue;

                for (int i = 0; i < chNum; i++) {
                    var child1 = fRec.Children[i];

                    for (int k = i + 1; k < chNum; k++) {
                        var child2 = fRec.Children[k];
                        if (child2.XRef == child1.XRef) {
                            hasDup = true;
                            break;
                        }
                    }

                    var childRec = tree.GetPtrValue<GDMIndividualRecord>(child1);
                    int yBirth = childRec.GetChronologicalYear(GEDCOMTagName.BIRT);
                    if (yBirth != 0) {
                        if (yBirth < minBirth) minBirth = yBirth;
                        if (yBirth > maxBirth) maxBirth = yBirth;
                    }
                }

                if (hasDup) {
                    CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdDuplicateChildren, CheckSolve.csEdit);
                    checkObj.Comment = LangMan.LS(LSID.LSID_DuplicateChildrenInFamily);
                    checksList.Add(checkObj);
                }

                if (minBirth != int.MaxValue && maxBirth != int.MinValue) {
                    int delta = (maxBirth - minBirth);
                    if (delta > GKData.MAX_SIBLINGS_DIFF) {
                        CheckObj checkObj = new CheckObj(fRec, CheckDiag.cdHighSiblingsDifference, CheckSolve.csSkip);
                        checkObj.Comment = string.Format(LangMan.LS(LSID.LSID_DifferenceBetweenSiblingsIsAbnormallyHigh), delta);
                        checksList.Add(checkObj);
                    }
                }
            }
        }

        private static void CheckMultimediaRecord(IBaseContext baseContext, GDMMultimediaRecord mmRec, List<CheckObj> checksList)
        {
            if (mmRec.FileReferences.Count <= 0) {
                CheckObj checkObj = new CheckObj(mmRec, CheckDiag.cdMediaRecordWithoutFiles, CheckSolve.csRemove);
                checkObj.Comment = LangMan.LS(LSID.LSID_MediaRecordWithoutFiles);
                checksList.Add(checkObj);
                return;
            }

            string fileName;
            MediaStoreStatus storeStatus = baseContext.VerifyMediaFile(mmRec.FileReferences[0], out fileName);

            switch (storeStatus) {
                case MediaStoreStatus.mssExists:
                    break;

                case MediaStoreStatus.mssFileNotFound: {
                        CheckObj checkObj = new CheckObj(mmRec, CheckDiag.cdFileNotFound, CheckSolve.csRemove);
                        checkObj.Comment = LangMan.LS(LSID.LSID_FileNotFound, fileName);
                        checksList.Add(checkObj);
                    }
                    break;

                case MediaStoreStatus.mssStgNotFound: {
                        CheckObj checkObj = new CheckObj(mmRec, CheckDiag.cdStgNotFound, CheckSolve.csRemove);
                        checkObj.Comment = LangMan.LS(LSID.LSID_StgNotFound);
                        checksList.Add(checkObj);
                    }
                    break;

                case MediaStoreStatus.mssArcNotFound: {
                        CheckObj checkObj = new CheckObj(mmRec, CheckDiag.cdArcNotFound, CheckSolve.csRemove);
                        checkObj.Comment = LangMan.LS(LSID.LSID_ArcNotFound);
                        checksList.Add(checkObj);
                    }
                    break;

                case MediaStoreStatus.mssBadData:
                    // TODO: can be deleted?
                    break;
            }
        }

        public static void CheckBase(IBaseWindow baseWin, List<CheckObj> checksList, IProgressController progress, TreeInspectionOptions options = null)
        {
            if (baseWin == null)
                throw new ArgumentNullException("baseWin");

            if (checksList == null)
                throw new ArgumentNullException("checksList");

            try {
                GDMTree tree = baseWin.Context.Tree;
                progress.Begin(LangMan.LS(LSID.LSID_ToolOp_7), tree.RecordsCount);
                checksList.Clear();

                for (int i = 0, num = tree.RecordsCount; i < num; i++) {
                    progress.Increment();

                    GDMRecord rec = tree[i];
                    switch (rec.RecordType) {
                        case GDMRecordType.rtIndividual:
                            CheckIndividualRecord(tree, rec as GDMIndividualRecord, checksList, options);
                            break;

                        case GDMRecordType.rtFamily:
                            CheckFamilyRecord(tree, rec as GDMFamilyRecord, checksList);
                            break;

                        case GDMRecordType.rtMultimedia:
                            CheckMultimediaRecord(baseWin.Context, rec as GDMMultimediaRecord, checksList);
                            break;
                    }
                }
            } finally {
                progress.End();
            }
        }

        public static void RepairProblem(IBaseWindow baseWin, CheckObj checkObj)
        {
            if (baseWin == null)
                throw new ArgumentNullException("baseWin");

            if (checkObj == null)
                throw new ArgumentNullException("checkObj");

            GDMTree tree = baseWin.Context.Tree;

            switch (checkObj.Diag) {
                case CheckDiag.cdPersonLonglived: {
                        var iRec = checkObj.Rec as GDMIndividualRecord;
                        baseWin.Context.CreateEventEx(iRec, GEDCOMTagName.DEAT, "", "");
                        baseWin.NotifyRecord(iRec, RecordAction.raEdit);
                    }
                    break;

                case CheckDiag.cdPersonSexless: {
                        var iRec = checkObj.Rec as GDMIndividualRecord;
                        baseWin.Context.CheckPersonSex(iRec);
                        baseWin.NotifyRecord(iRec, RecordAction.raEdit);
                    }
                    break;

                case CheckDiag.cdEmptyFamily:
                    tree.DeleteRecord(checkObj.Rec);
                    break;

                case CheckDiag.cdFatherAsChild: {
                        var fRec = (GDMFamilyRecord)checkObj.Rec;
                        fRec.DeleteChild(fRec.Husband);
                    }
                    break;

                case CheckDiag.cdMotherAsChild: {
                        var fRec = (GDMFamilyRecord)checkObj.Rec;
                        fRec.DeleteChild(fRec.Wife);
                    }
                    break;

                case CheckDiag.cdDuplicateChildren:
                    if (checkObj.Solve == CheckSolve.csEdit) {
                        BaseController.EditRecord(baseWin, checkObj.Rec);
                    }
                    break;

                case CheckDiag.csDateInvalid:
                    if (checkObj.Solve == CheckSolve.csEdit) {
                        BaseController.EditRecord(baseWin, checkObj.Rec);
                    }
                    break;

                case CheckDiag.cdLiveYearsInvalid:
                    break;

                case CheckDiag.cdStrangeSpouse:
                    break;

                case CheckDiag.cdStrangeParent:
                    break;

                case CheckDiag.csCycle:
                    break;

                case CheckDiag.cdChildWithoutParents:
                    break;

                case CheckDiag.cdFamilyRecordWithoutFamily:
                    break;

                case CheckDiag.cdMediaRecordWithoutFiles:
                    break;

                case CheckDiag.cdStgNotFound:
                    break;

                case CheckDiag.cdArcNotFound:
                    break;

                case CheckDiag.cdFileNotFound:
                    break;

                case CheckDiag.cdHalfSpsFamLink:
                    if (checkObj.Solve == CheckSolve.csRepair) {
                        var indiRec = (GDMIndividualRecord)checkObj.Rec;
                        var famRec = (GDMFamilyRecord)checkObj.Target;

                        // fuse: these two problems often occur together
                        CheckAndRepairGarbledSpouses(tree, famRec);

                        switch (indiRec.Sex) {
                            case GDMSex.svMale:
                                famRec.Husband.XRef = indiRec.XRef;
                                break;

                            case GDMSex.svFemale:
                                famRec.Wife.XRef = indiRec.XRef;
                                break;
                        }
                    }
                    break;

                case CheckDiag.cdHalfChdFamLink:
                    if (checkObj.Solve == CheckSolve.csRepair) {
                        var indiRec = (GDMIndividualRecord)checkObj.Rec;
                        var famRec = (GDMFamilyRecord)checkObj.Target;

                        famRec.Children.Add(new GDMChildLink(indiRec.XRef));
                    }
                    break;

                case CheckDiag.cdHalfFamHusbLink:
                    if (checkObj.Solve == CheckSolve.csRepair) {
                        var famRec = (GDMFamilyRecord)checkObj.Rec;
                        var husb = (GDMIndividualRecord)checkObj.Target;

                        husb.SpouseToFamilyLinks.Add(new GDMSpouseToFamilyLink(famRec.XRef));
                    }
                    break;

                case CheckDiag.cdHalfFamWifeLink:
                    if (checkObj.Solve == CheckSolve.csRepair) {
                        var famRec = (GDMFamilyRecord)checkObj.Rec;
                        var wife = (GDMIndividualRecord)checkObj.Target;

                        wife.SpouseToFamilyLinks.Add(new GDMSpouseToFamilyLink(famRec.XRef));
                    }
                    break;

                case CheckDiag.cdHalfFamChldLink:
                    if (checkObj.Solve == CheckSolve.csRepair) {
                        var famRec = (GDMFamilyRecord)checkObj.Rec;
                        var child = (GDMIndividualRecord)checkObj.Target;

                        child.ChildToFamilyLinks.Add(new GDMChildToFamilyLink(famRec.XRef));
                    }
                    break;

                case CheckDiag.cdGarbledSpouses:
                    if (checkObj.Solve == CheckSolve.csRepair) {
                        var famRec = (GDMFamilyRecord)checkObj.Rec;

                        CheckAndRepairGarbledSpouses(tree, famRec);
                    }
                    break;
            }
        }

        private static void CheckAndRepairGarbledSpouses(GDMTree tree, GDMFamilyRecord famRec)
        {
            var husb = tree.GetPtrValue<GDMIndividualRecord>(famRec.Husband);
            var wife = tree.GetPtrValue<GDMIndividualRecord>(famRec.Wife);

            if ((husb != null && husb.Sex != GDMSex.svMale) || (wife != null && wife.Sex != GDMSex.svFemale)) {
                var husbXRef = famRec.Husband.XRef;
                var wifeXRef = famRec.Wife.XRef;

                famRec.Husband.XRef = wifeXRef;
                famRec.Wife.XRef = husbXRef;
            }
        }

        #endregion

        #region Detect cycles

        private enum DCFlag { dcfAncWalk, dcfDescWalk }

        private static void SetIndiFlag(GKVarCache<GDMIndividualRecord, int> indiFlags, GDMIndividualRecord iRec, DCFlag flag)
        {
            int flags = indiFlags[iRec];
            flags = BitHelper.SetBit(flags, (int)flag);
            indiFlags[iRec] = flags;
        }

        private static bool HasIndiFlag(GKVarCache<GDMIndividualRecord, int> indiFlags, GDMIndividualRecord iRec, DCFlag flag)
        {
            int flags = indiFlags[iRec];
            return BitHelper.IsSetBit(flags, (int)flag);
        }

        private static GDMIndividualRecord DetectCycleAncestors(GDMTree tree, GDMIndividualRecord iRec,
                                                                Stack<GDMIndividualRecord> stack,
                                                                GKVarCache<GDMIndividualRecord, int> indiFlags)
        {
            if (iRec == null) return null;

            if (stack.Contains(iRec)) return iRec;

            SetIndiFlag(indiFlags, iRec, DCFlag.dcfAncWalk);

            stack.Push(iRec);

            GDMFamilyRecord family = tree.GetParentsFamily(iRec);
            if (family != null) {
                var res = DetectCycleAncestors(tree, tree.GetPtrValue(family.Husband), stack, indiFlags);
                if (res != null) return res;

                res = DetectCycleAncestors(tree, tree.GetPtrValue(family.Wife), stack, indiFlags);
                if (res != null) return res;
            }

            stack.Pop();
            return null;
        }

        private static GDMIndividualRecord DetectCycleDescendants(GDMTree tree, GDMIndividualRecord iRec,
                                                                  Stack<GDMIndividualRecord> stack,
                                                                  GKVarCache<GDMIndividualRecord, int> indiFlags)
        {
            if (iRec == null) return null;

            if (stack.Contains(iRec)) return iRec;

            SetIndiFlag(indiFlags, iRec, DCFlag.dcfDescWalk);

            stack.Push(iRec);

            int num = iRec.SpouseToFamilyLinks.Count;
            for (int i = 0; i < num; i++) {
                GDMFamilyRecord family = tree.GetPtrValue(iRec.SpouseToFamilyLinks[i]);
                if (family == null) continue;

                int num2 = family.Children.Count;
                for (int j = 0; j < num2; j++) {
                    GDMIndividualRecord child = tree.GetPtrValue(family.Children[j]);
                    if (child == null) continue;

                    var res = DetectCycleDescendants(tree, child, stack, indiFlags);
                    if (res != null) return res;
                }
            }

            stack.Pop();
            return null;
        }

        public static string DetectCycle(GDMTree tree, GDMIndividualRecord iRec)
        {
            var stack = new Stack<GDMIndividualRecord>();
            var indiDCFlags = new GKVarCache<GDMIndividualRecord, int>();

            var hasCycle = DetectCycleAncestors(tree, iRec, stack, indiDCFlags);
            if (hasCycle != null) {
                var lastRec = stack.Pop();
                return iRec.XRef + " ... " + lastRec.XRef + " -> " + hasCycle.XRef;
            }

            stack.Clear();

            hasCycle = DetectCycleDescendants(tree, iRec, stack, indiDCFlags);
            if (hasCycle != null) {
                var lastRec = stack.Pop();
                return iRec.XRef + " ... " + lastRec.XRef + " -> " + hasCycle.XRef;
            }

            return string.Empty;
        }

        private static string CheckCycle(GDMTree tree, GDMIndividualRecord iRec)
        {
            var stack = new Stack<GDMIndividualRecord>();
            GDMIndividualRecord hasCycle = null;
            var indiDCFlags = new GKVarCache<GDMIndividualRecord, int>();

            if (!HasIndiFlag(indiDCFlags, iRec, DCFlag.dcfAncWalk)) {
                hasCycle = DetectCycleAncestors(tree, iRec, stack, indiDCFlags);
                if (hasCycle != null) {
                    var lastRec = stack.Pop();
                    return iRec.XRef + " ... " + lastRec.XRef + " -> " + hasCycle.XRef;
                }
                stack.Clear();
            }

            if (!HasIndiFlag(indiDCFlags, iRec, DCFlag.dcfDescWalk)) {
                hasCycle = DetectCycleDescendants(tree, iRec, stack, indiDCFlags);
                if (hasCycle != null) {
                    var lastRec = stack.Pop();
                    return iRec.XRef + " ... " + lastRec.XRef + " -> " + hasCycle.XRef;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
