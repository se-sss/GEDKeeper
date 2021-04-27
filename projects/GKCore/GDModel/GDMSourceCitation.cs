﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2021 by Sergey V. Zhdanovskih.
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
using GDModel.Providers.GEDCOM;

namespace GDModel
{
    public sealed class GDMSourceCitation : GDMPointer, IGDMTextObject, IGDMStructWithNotes, IGDMStructWithMultimediaLinks
    {
        private int fCertaintyAssessment;
        private GDMSourceCitationData fData;
        private GDMLines fDescription;
        private GDMList<GDMMultimediaLink> fMultimediaLinks;
        private GDMList<GDMNotes> fNotes;
        private string fPage;
        private GDMTextTag fText;


        public int CertaintyAssessment
        {
            get { return fCertaintyAssessment; }
            set { fCertaintyAssessment = value; }
        }

        public GDMSourceCitationData Data
        {
            get { return fData; }
        }

        public GDMLines Description
        {
            get {
                if (IsPointer) {
                    throw new InvalidOperationException("GDMSourceCitation is a pointer, please dereference");
                }

                return fDescription;
            }
        }

        GDMLines IGDMTextObject.Lines
        {
            get { return fDescription; }
        }

        public GDMList<GDMMultimediaLink> MultimediaLinks
        {
            get { return fMultimediaLinks; }
        }

        public GDMList<GDMNotes> Notes
        {
            get { return fNotes; }
        }

        public string Page
        {
            get { return fPage; }
            set { fPage = value; }
        }

        public GDMTextTag Text
        {
            get { return fText; }
        }


        public GDMSourceCitation()
        {
            SetName(GEDCOMTagType.SOUR);

            fCertaintyAssessment = -1;
            fDescription = new GDMLines();
            fPage = string.Empty;

            fData = new GDMSourceCitationData();
            fText = new GDMTextTag((int)GEDCOMTagType.TEXT);
            fNotes = new GDMList<GDMNotes>();
            fMultimediaLinks = new GDMList<GDMMultimediaLink>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fData.Dispose();
                fText.Dispose();
                fNotes.Dispose();
                fMultimediaLinks.Dispose();
            }
            base.Dispose(disposing);
        }

        internal override void TrimExcess()
        {
            base.TrimExcess();

            fData.TrimExcess();
            fDescription.TrimExcess();
            fText.TrimExcess();
            fNotes.TrimExcess();
            fMultimediaLinks.TrimExcess();
        }

        public override void Assign(GDMTag source)
        {
            GDMSourceCitation sourceObj = (source as GDMSourceCitation);
            if (sourceObj == null)
                throw new ArgumentException(@"Argument is null or wrong type", "source");

            base.Assign(sourceObj);

            fCertaintyAssessment = sourceObj.fCertaintyAssessment;
            fPage = sourceObj.fPage;
            fData.Assign(sourceObj.fData);
            fDescription.Assign(sourceObj.fDescription);
            fText.Assign(sourceObj.fText);
            AssignList(sourceObj.Notes, fNotes);
            AssignList(sourceObj.MultimediaLinks, fMultimediaLinks);
        }

        public override void Clear()
        {
            base.Clear();

            fCertaintyAssessment = -1;
            fData.Clear();
            fDescription.Clear();
            fPage = string.Empty;
            fText.Clear();
            fNotes.Clear();
            fMultimediaLinks.Clear();
        }

        public override bool IsEmpty()
        {
            bool result;

            bool isCommonEmpty = string.IsNullOrEmpty(fPage)
                && (fNotes.Count == 0) && (fMultimediaLinks.Count == 0);

            if (IsPointer) {
                result = base.IsEmpty() && isCommonEmpty && fData.IsEmpty();
            } else {
                result = fDescription.IsEmpty() && (SubTags.Count == 0) && fText.IsEmpty() && isCommonEmpty;
            }

            return result;
        }

        public override void ReplaceXRefs(GDMXRefReplacer map)
        {
            base.ReplaceXRefs(map);

            fNotes.ReplaceXRefs(map);
            fMultimediaLinks.ReplaceXRefs(map);
        }

        protected override string GetStringValue()
        {
            string result = IsPointer ? base.GetStringValue() : ((fDescription.Count > 0) ? fDescription[0] : string.Empty);
            return result;
        }

        public override string ParseString(string strValue)
        {
            string result = base.ParseString(strValue);
            if (!IsPointer) {
                fDescription.Clear();
                if (!string.IsNullOrEmpty(result)) {
                    fDescription.Add(result);
                }
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Strange values were found, possibly from other genealogical programs.
        /// </summary>
        /// <returns>Checked value of CertaintyAssessment</returns>
        public int GetValidCertaintyAssessment()
        {
            int val = fCertaintyAssessment;
            return (val >= 0 && val <= 3) ? val : 0;
        }
    }
}
