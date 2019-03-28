﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2019 by Sergey V. Zhdanovskih.
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

namespace GKCommon.GEDCOM
{
    public sealed class GEDCOMIndividualEvent : GEDCOMCustomEvent
    {
        public GEDCOMPointer Family
        {
            // define 'FAMC' tag constructor by default (GEDCOMTree.cctor) for AddTag()
            get { return base.TagClass(GEDCOMTagType.FAMC, GEDCOMPointer.Create) as GEDCOMPointer; }
        }


        public GEDCOMIndividualEvent(GEDCOMObject owner) : base(owner)
        {
        }

        public GEDCOMIndividualEvent(GEDCOMObject owner, string tagName, string tagValue) : this(owner)
        {
            SetNameValue(tagName, tagValue);
        }

        public new static GEDCOMTag Create(GEDCOMObject owner, string tagName, string tagValue)
        {
            return new GEDCOMIndividualEvent(owner, tagName, tagValue);
        }
    }
}
