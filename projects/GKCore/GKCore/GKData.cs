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

using GDModel.Providers.GEDCOM;
using GKCore.Types;

namespace GKCore
{
    /// <summary>
    /// Global container for various constants.
    /// </summary>
    public static class GKData
    {
        // Until the next major version, we cannot change this line,
        // because it including defines the path to the profile of the program.
        public const string APP_TITLE = "GEDKeeper";

        public const string APP_COPYRIGHT = "Copyright © 2009-2023 by Sergey V. Zhdanovskih";

        public const string APP_VERSION_2X = "2.26.1.0";
        public const string APP_VERSION_3X = "3.2.1.0";

#if !GK3
        public const string UpdateURL = "https://sourceforge.net/projects/gedkeeper/files/gk_version.xml";
#else
        public const string UpdateURL = "https://sourceforge.net/projects/gedkeeper/files/gk3_version.xml";
#endif


        public const string APP_MAIL = "gedkeeper@yandex.ru";

        public const string APP_FORUM_EN = "https://groups.google.com/g/gedkeeper-en";
        public const string APP_CHANNEL_EN = "https://t.me/gedkeeper_en";

        public const string APP_FORUM_RU = "https://groups.google.com/g/gedkeeper-ru";
        public const string APP_CHANNEL_RU = "https://t.me/gedkeeper_ru";

        public const int APP_FORMAT_DEFVER = 39; // don't change it!
        public const int APP_FORMAT_CURVER = 44; // v2.19.0

        public const string GEDCOM_EXT = "ged";
        public const string GEDCOM_SEC_EXT = "geds";
        public const string LUA_EXT = "lua";

        public const int NOTE_NAME_MAX_LENGTH = 64;
        public const int NOTE_NAME_DBL_MAX_LENGTH = 128;

        public const string GAPI_KEY = "AIzaSyCyPx_u1PhHaN2d3ld4J8hsgASF5lOdpGY";
        public const string YAPI_KEY = "a5653896-1335-477f-aac7-10a2ba9e52c5";

        public static int HighlightUnparentedColor = 0xFFCACA;
        public static int HighlightUnmarriedColor = 0xFFFFA1;
        public static int HighlightInaccessibleFiles = 0xFFCACA;
        public static int HighlightReadabilityRows = 0xEFEFEF;

        public const string INFO_HTTP_PREFIX = "http";
        public const string INFO_HREF_VIEW = "view_";
        public const string INFO_HREF_FILTER_INDI = "filter_indi_by_";

        #if OS_LINUX || OS_FREEBSD
        public const string DEF_FONT = "Noto Sans";
        #else
        public const string DEF_FONT = "Tahoma";
        #endif

        public sealed class SexStruct
        {
            public LSID NameId;
            public string Sign;

            public SexStruct(LSID name, string sign) {
                NameId = name;
                Sign = sign;
            }
        }

        public sealed class MarStatusStruct
        {
            public LSID Name;
            public string StatSign;

            public MarStatusStruct(LSID name, string sign) {
                Name = name;
                StatSign = sign;
            }
        }

        public sealed class EventStruct
        {
            public LSID Name;
            public string Sign;
            public PersonEventKind Kind;

            public EventStruct(LSID name, string sign, PersonEventKind kind) {
                Name = name;
                Sign = sign;
                Kind = kind;
            }
        }

        public sealed class DateKindStruct
        {
            public LSID Name;
            public byte Dates;

            public DateKindStruct(LSID name, byte dates) {
                Name = name;
                Dates = dates;
            }
        }

        public sealed class CalendarStruct
        {
            public LSID Name;
            public string Sign;
            public bool HasSupport;

            public CalendarStruct(LSID name, string sign, bool hasSupport) {
                Name = name;
                Sign = sign;
                HasSupport = hasSupport;
            }
        }

        public sealed class StoreTypeRec
        {
            public LSID Name;
            public string Sign;

            public StoreTypeRec(LSID name, string sign) {
                Name = name;
                Sign = sign;
            }
        }

        public sealed class StatsTitleStruct
        {
            public LSID Title;
            public LSID Cap;

            public StatsTitleStruct(LSID title, LSID cap) {
                Title = title;
                Cap = cap;
            }
        }

        public sealed class SpecialUserRef
        {
            public LSID Title;
            public string ResName;

            public SpecialUserRef(LSID title, string resName)
            {
                Title = title;
                ResName = resName;
            }
        }

        public sealed class RecordTypeStruct
        {
            public LSID Name;
            public string Sign;

            public RecordTypeStruct(LSID name, string sign)
            {
                Name = name;
                Sign = sign;
            }
        }

        public sealed class FARPropertyStruct
        {
            public LSID Name;
            public bool Enabled;

            public FARPropertyStruct(LSID name, bool enabled)
            {
                Name = name;
                Enabled = enabled;
            }
        }

        public static readonly LSID[] Restrictions;
        public static readonly RecordTypeStruct[] RecordTypes;
        public static readonly SexStruct[] SexData;
        public static readonly MarStatusStruct[] MarriageStatus;
        public static readonly EventStruct[] PersonEvents;
        public static readonly DateKindStruct[] DateKinds;
        public static readonly CalendarStruct[] DateCalendars;
        public static readonly EventStruct[] FamilyEvents;
        public static readonly StoreTypeRec[] GKStoreTypes;
        public static readonly LSID[] MediaTypes;
        public static readonly LSID[] PriorityNames;
        public static readonly LSID[] StatusNames;
        public static readonly LSID[] CommunicationNames;
        public static readonly LSID[] CommunicationDirs;
        public static readonly LSID[] GoalNames;
        public static readonly LSID[] CertaintyAssessments;
        public static readonly SpecialUserRef[] SpecialUserRefs;
        public static readonly LSID[] RelationKinds;
        public static readonly string[] Numerals;
        public static readonly string[] NumKinship;
        public static readonly LSID GreatPrefix;
        public static readonly StatsTitleStruct[] StatsTitles;
        public static readonly LSID[] CheckSolveNames;
        public static readonly LSID[] NameTypes;
        public static readonly LSID[] ParentTypes;
        public static readonly string[] CondSigns;
        public static readonly string BloodGroups;
        public static readonly FARPropertyStruct[] FARPropertyTypes;
        public static readonly LSID URTreeNoteType;


        /// <summary>
        /// Bounds checks data for correctness.
        /// </summary>
        public const int PROVED_LIFE_LENGTH = 122; // max. duration of a person's life (proved)
        public const int MIN_PARENT_AGE = 10; // min. age of parent at child's birth
        public const int MAX_MOTHER_AGE = 55; // max. age of mother at child's birth
        public const int MAX_FATHER_AGE = 85; // max. age of father at child's birth
        public const int MIN_MARRIAGE_AGE = 13; // min. age for marriage
        public const int MAX_SPOUSES_DIFF = 90; // max. age difference between spouses
        public const int MAX_SIBLINGS_DIFF = 40; // max. age difference between brothers/sisters


        static GKData()
        {
            GreatPrefix = LSID.LSID_RK_GreatPrefix;

            // TODO: need to find a way of localization
            NumKinship = new string[] {
                "-",
                "юродный",
                "юродная",
                ""
            };

            // TODO: need to find a way of localization
            Numerals = new string[] {
                "-",
                "дво",
                "тро",
                "четверо",
                "пяти",
                "шести",
                "семи",
                "восьми",
                "девяти"
            };

            RelationKinds = new LSID[] {
                /* 00 */ LSID.LSID_RK_Unk,
                /* 01 */ LSID.LSID_None,
                /* 02 */ LSID.LSID_None,
                /* 03 */ LSID.LSID_None,
                /* 04 */ LSID.LSID_RK_Father,
                /* 05 */ LSID.LSID_RK_Mother,
                /* 06 */ LSID.LSID_RK_Husband,
                /* 07 */ LSID.LSID_RK_Wife,
                /* 08 */ LSID.LSID_RK_Son,
                /* 09 */ LSID.LSID_RK_Daughter,
                /* 10 */ LSID.LSID_RK_Grandfather,
                /* 11 */ LSID.LSID_RK_Grandmother,
                /* 12 */ LSID.LSID_RK_Grandson,
                /* 13 */ LSID.LSID_RK_Granddaughter,
                /* 14 */ LSID.LSID_RK_Brother,
                /* 15 */ LSID.LSID_RK_Sister,
                /* 16 */ LSID.LSID_RK_SonInLaw,
                /* 17 */ LSID.LSID_RK_DaughterInLaw,
                /* 18 */ LSID.LSID_RK_HusbandFather,
                /* 19 */ LSID.LSID_RK_HusbandMother,
                /* 20 */ LSID.LSID_RK_WifeFather,
                /* 21 */ LSID.LSID_RK_WifeMother,
                /* 22 */ LSID.LSID_RK_Uncle_FatherBrother,
                /* 23 */ LSID.LSID_RK_Aunt_FatherSister,
                /* 24 */ LSID.LSID_RK_Uncle_MotherBrother,
                /* 25 */ LSID.LSID_RK_Aunt_MotherSister,
                /* 26 */ LSID.LSID_RK_Nephew,
                /* 27 */ LSID.LSID_RK_Niece,
                /* 28 */ LSID.LSID_RK_CousinM,
                /* 29 */ LSID.LSID_RK_CousinF,
                /* 30 */ LSID.LSID_RK_BrotherInLaw_H,
                /* 31 */ LSID.LSID_RK_SisterInLaw_H,
                /* 32 */ LSID.LSID_RK_BrotherInLaw_W,
                /* 33 */ LSID.LSID_RK_SisterInLaw_W,
                /* 34 */ LSID.LSID_RK_Stepfather,
                /* 35 */ LSID.LSID_RK_Stepmother,
                /* 36 */ LSID.LSID_RK_FathersWife,
                /* 37 */ LSID.LSID_RK_MothersHusband,
                /* 38 */ LSID.LSID_None,
                /* 39 */ LSID.LSID_RK_Unk
            };

            SpecialUserRefs = new SpecialUserRef[] {
                new SpecialUserRef(LSID.LSID_RI_GeorgeKnight, "tg_george_cross.gif"),
                new SpecialUserRef(LSID.LSID_USSR_WWII_Combatant, "tg_soldier.gif"),
                new SpecialUserRef(LSID.LSID_USSR_WWII_KilledInBattle, "tg_soldier_fall.gif"),
                new SpecialUserRef(LSID.LSID_USSR_WWII_WorkerInRear, "tg_veteran_rear.gif"),
                new SpecialUserRef(LSID.LSID_USSR_Repressed, "tg_barbed_wire.gif"),
                new SpecialUserRef(LSID.LSID_USSR_CPSUMember, "tg_cpsu.png"),
                new SpecialUserRef(LSID.LSID_Religion_Islam, "tg_islam_sym.gif"),
                new SpecialUserRef(LSID.LSID_Religion_Catholicism, "tg_latin_cross.gif"),
                new SpecialUserRef(LSID.LSID_Religion_Orthodoxy, "tg_orthodox_cross.gif"),
                new SpecialUserRef(LSID.LSID_Religion_TheOldBelievers, "tg_oldritual_cross.gif"),
            };

            CertaintyAssessments = new LSID[] {
                LSID.LSID_Cert_1,
                LSID.LSID_Cert_2,
                LSID.LSID_Cert_3,
                LSID.LSID_Cert_4
            };

            GoalNames = new LSID[] {
                LSID.LSID_G_1,
                LSID.LSID_G_2,
                LSID.LSID_G_3,
                LSID.LSID_G_4
            };

            CommunicationDirs = new LSID[] {
                LSID.LSID_CD_1,
                LSID.LSID_CD_2
            };

            CommunicationNames = new LSID[] {
                LSID.LSID_Com_1,
                LSID.LSID_Com_2,
                LSID.LSID_Com_3,
                LSID.LSID_Com_4,
                LSID.LSID_Com_5,
                LSID.LSID_Com_6
            };

            StatusNames = new LSID[] {
                LSID.LSID_RStat_1,
                LSID.LSID_RStat_2,
                LSID.LSID_RStat_3,
                LSID.LSID_RStat_4,
                LSID.LSID_RStat_5,
                LSID.LSID_RStat_6
            };

            PriorityNames = new LSID[] {
                LSID.LSID_Prt_1,
                LSID.LSID_Prt_2,
                LSID.LSID_Prt_3,
                LSID.LSID_Prt_4,
                LSID.LSID_Prt_5
            };

            MediaTypes = new LSID[] {
                LSID.LSID_DefaultValue, // TODO: may be LSID.LSID_MT_15 (Unknown)?
                LSID.LSID_MT_02,
                LSID.LSID_MT_03,
                LSID.LSID_MT_04,
                LSID.LSID_MT_05,
                LSID.LSID_MT_06,
                LSID.LSID_MT_07,
                LSID.LSID_MT_08,
                LSID.LSID_MT_09,
                LSID.LSID_MT_10,
                LSID.LSID_MT_11,
                LSID.LSID_MT_12,
                LSID.LSID_MT_13,
                LSID.LSID_MT_14
                //LSID.LSID_MT_15 <Unknown removed to first position>
            };

            GKStoreTypes = new StoreTypeRec[] {
                new StoreTypeRec(LSID.LSID_STRef, ""),
                new StoreTypeRec(LSID.LSID_STStg, "stg:"),
                new StoreTypeRec(LSID.LSID_STArc, "arc:"),
                new StoreTypeRec(LSID.LSID_STRel, "rel:"),
                new StoreTypeRec(LSID.LSID_STWeb, "http")
            };

            FamilyEvents = new EventStruct[] {
                new EventStruct(LSID.LSID_Event, GEDCOMTagName.EVEN, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_1, GEDCOMTagName.ENGA, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_2, GEDCOMTagName.MARR, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_3, GEDCOMTagName.MARB, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_4, GEDCOMTagName.MARC, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_5, GEDCOMTagName.MARL, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_6, GEDCOMTagName.MARS, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_7, GEDCOMTagName.ANUL, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_8, GEDCOMTagName.DIVF, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FEvt_9, GEDCOMTagName.DIV, PersonEventKind.ekEvent),
            };

            // гр|юл|евр|фр|рим|исл(хид?)|?
            // н.ст.|ст.ст.|евр|фр|рим|исл(хид?)|?
            // G|J|H|FR|R|I|?
            // FIXME: use calendars for StrToGEDCOMDate() and class EventEditDlg
            DateCalendars = new CalendarStruct[] {
                new CalendarStruct(LSID.LSID_Cal_Gregorian, " [G]", true),
                new CalendarStruct(LSID.LSID_Cal_Julian, " [J]", true),
                new CalendarStruct(LSID.LSID_Cal_Hebrew, " [H]", false), // !
                new CalendarStruct(LSID.LSID_Cal_French, " [FR]", false),
                new CalendarStruct(LSID.LSID_Cal_Roman, " [R]", false),
                new CalendarStruct(LSID.LSID_Cal_Islamic, " [I]", false), // !
                new CalendarStruct(LSID.LSID_Unknown, "", false)
            };

            DateKinds = new DateKindStruct[] {
                new DateKindStruct(LSID.LSID_DK_0, 1), // 1
                new DateKindStruct(LSID.LSID_DK_1, 2), // 2
                new DateKindStruct(LSID.LSID_DK_2, 1), // 1
                new DateKindStruct(LSID.LSID_DK_3, 3), // 1,2
                new DateKindStruct(LSID.LSID_DK_4, 1), // 1
                new DateKindStruct(LSID.LSID_DK_5, 2), // 2
                new DateKindStruct(LSID.LSID_DK_6, 3), // 1,2
                new DateKindStruct(LSID.LSID_DK_7, 1), // 1
                new DateKindStruct(LSID.LSID_DK_8, 1), // 1
                new DateKindStruct(LSID.LSID_DK_9, 1)  // 1
            };

            PersonEvents = new EventStruct[] {
                new EventStruct(LSID.LSID_Event, GEDCOMTagName.EVEN, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Birth, GEDCOMTagName.BIRT, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Adoption, GEDCOMTagName.ADOP, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Christening, GEDCOMTagName.CHR, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Graduation, GEDCOMTagName.GRAD, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Retirement, GEDCOMTagName.RETI, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Naturalization, GEDCOMTagName.NATU, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Emigration, GEDCOMTagName.EMIG, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Immigration, GEDCOMTagName.IMMI, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Census, GEDCOMTagName.CENS, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_LastWill, GEDCOMTagName.WILL, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_ProbateOfWill, GEDCOMTagName.PROB, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Death, GEDCOMTagName.DEAT, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Burial, GEDCOMTagName.BURI, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Cremation, GEDCOMTagName.CREM, PersonEventKind.ekEvent),

                new EventStruct(LSID.LSID_Fact, GEDCOMTagName.FACT, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Religion, GEDCOMTagName.RELI, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Nationality, GEDCOMTagName.NATI, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Residence, GEDCOMTagName.RESI, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_PhysicalDesc, GEDCOMTagName.DSCR, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_NationalIDNumber, GEDCOMTagName.IDNO, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_SocialSecurityNumber, GEDCOMTagName.SSN, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_ChildsCount, GEDCOMTagName.NCHI, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_MarriagesCount, GEDCOMTagName.NMR, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Education, GEDCOMTagName.EDUC, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Occupation, GEDCOMTagName.OCCU, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Caste, GEDCOMTagName.CAST, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_PersonalProperty, GEDCOMTagName.PROP, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_NobilityTitle, GEDCOMTagName.TITL, PersonEventKind.ekFact),

                new EventStruct(LSID.LSID_Travel, GEDCOMTagName._TRAVEL, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Hobby, GEDCOMTagName._HOBBY, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Award, GEDCOMTagName._AWARD, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_Mili, GEDCOMTagName._MILI, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_MiliInd, GEDCOMTagName._MILI_IND, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_MiliDis, GEDCOMTagName._MILI_DIS, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_MiliRank, GEDCOMTagName._MILI_RANK, PersonEventKind.ekFact),

                new EventStruct(LSID.LSID_BloodGroup, GEDCOMTagName._BGRO, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_HairColor, GEDCOMTagName._HAIR, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_EyesColor, GEDCOMTagName._EYES, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_MDNAHaplogroup, GEDCOMTagName._MDNA, PersonEventKind.ekFact),
                new EventStruct(LSID.LSID_YDNAHaplogroup, GEDCOMTagName._YDNA, PersonEventKind.ekFact),

                new EventStruct(LSID.LSID_Baptism, GEDCOMTagName.BAPM, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_BarMitzvah, GEDCOMTagName.BARM, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_BatMitzvah, GEDCOMTagName.BASM, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Blessing, GEDCOMTagName.BLES, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_AdultChristening, GEDCOMTagName.CHRA, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Confirmation, GEDCOMTagName.CONF, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_FirstCommunion, GEDCOMTagName.FCOM, PersonEventKind.ekEvent),
                new EventStruct(LSID.LSID_Ordination, GEDCOMTagName.ORDN, PersonEventKind.ekEvent),
            };

            MarriageStatus = new MarStatusStruct[] {
                new MarStatusStruct(LSID.LSID_Unknown, ""),
                new MarStatusStruct(LSID.LSID_MarrRegistered, "MARRIED"),
                new MarStatusStruct(LSID.LSID_MarrNotRegistered, "MARRNOTREG"),
                new MarStatusStruct(LSID.LSID_MarrDivorced, "NOTMARR")
            };

            SexData = new SexStruct[] {
                new SexStruct(LSID.LSID_SexU, "U"),
                new SexStruct(LSID.LSID_SexM, "M"),
                new SexStruct(LSID.LSID_SexF, "F"),
                new SexStruct(LSID.LSID_SexX, "X")
            };

            RecordTypes = new RecordTypeStruct[] {
                new RecordTypeStruct(LSID.LSID_None, ""),
                new RecordTypeStruct(LSID.LSID_Person, "Individual"),
                new RecordTypeStruct(LSID.LSID_Family, "Family"),
                new RecordTypeStruct(LSID.LSID_Note, "Note"),
                new RecordTypeStruct(LSID.LSID_RPMultimedia, "Multimedia"),
                new RecordTypeStruct(LSID.LSID_Source, "Source"),
                new RecordTypeStruct(LSID.LSID_Repository, "Repository"),
                new RecordTypeStruct(LSID.LSID_Group, "Group"),
                new RecordTypeStruct(LSID.LSID_Research, "Research"),
                new RecordTypeStruct(LSID.LSID_Task, "Task"),
                new RecordTypeStruct(LSID.LSID_Communication, "Communication"),
                new RecordTypeStruct(LSID.LSID_Location, "Location"),
                new RecordTypeStruct(LSID.LSID_Submission, "Submission"),
                new RecordTypeStruct(LSID.LSID_Submitter, "Submitter")
            };

            Restrictions = new LSID[] {
                LSID.LSID_RestrictNone,
                LSID.LSID_RestrictLocked,
                LSID.LSID_RestrictConfidential,
                LSID.LSID_RestrictPrivacy
            };

            StatsTitles = new StatsTitleStruct[] {
                new StatsTitleStruct(LSID.LSID_AncestorsCount, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_DescendantsCount, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_GenerationsCount, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_Surname, LSID.LSID_Surname),
                new StatsTitleStruct(LSID.LSID_Name, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_Patronymic, LSID.LSID_Patronymic),
                new StatsTitleStruct(LSID.LSID_Age, LSID.LSID_Age),
                new StatsTitleStruct(LSID.LSID_LifeExpectancy, LSID.LSID_Age),
                new StatsTitleStruct(LSID.LSID_BirthYears, LSID.LSID_BirthYears),
                new StatsTitleStruct(LSID.LSID_BirthYearsDec, LSID.LSID_BirthYears),
                new StatsTitleStruct(LSID.LSID_DeathYears, LSID.LSID_DeathYears),
                new StatsTitleStruct(LSID.LSID_DeathYearsDec, LSID.LSID_DeathYears),
                new StatsTitleStruct(LSID.LSID_ChildsCount, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_DistrChilds, LSID.LSID_ChildsCount),
                new StatsTitleStruct(LSID.LSID_BirthPlace, LSID.LSID_BirthPlace),
                new StatsTitleStruct(LSID.LSID_DeathPlace, LSID.LSID_DeathPlace),
                new StatsTitleStruct(LSID.LSID_Residence, LSID.LSID_Residence),
                new StatsTitleStruct(LSID.LSID_Occupation, LSID.LSID_Occupation),
                new StatsTitleStruct(LSID.LSID_Religion, LSID.LSID_Religion),
                new StatsTitleStruct(LSID.LSID_Nationality, LSID.LSID_Nationality),
                new StatsTitleStruct(LSID.LSID_Education, LSID.LSID_Education),
                new StatsTitleStruct(LSID.LSID_Caste, LSID.LSID_Caste),
                new StatsTitleStruct(LSID.LSID_AgeFirstborn, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_MarriagesCount, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_MarriagesAge, LSID.LSID_Name),
                new StatsTitleStruct(LSID.LSID_DiffSpouses, LSID.LSID_Family),
                new StatsTitleStruct(LSID.LSID_Hobby, LSID.LSID_Hobby),
                new StatsTitleStruct(LSID.LSID_Award, LSID.LSID_Award),
                new StatsTitleStruct(LSID.LSID_Mili, LSID.LSID_Mili),
                new StatsTitleStruct(LSID.LSID_MiliInd, LSID.LSID_MiliInd),
                new StatsTitleStruct(LSID.LSID_MiliDis, LSID.LSID_MiliDis),
                new StatsTitleStruct(LSID.LSID_MiliRank, LSID.LSID_MiliRank),
                new StatsTitleStruct(LSID.LSID_AAF_1, LSID.LSID_AAF_1),
                new StatsTitleStruct(LSID.LSID_AAF_2, LSID.LSID_AAF_2),
                new StatsTitleStruct(LSID.LSID_CertaintyIndex, LSID.LSID_CertaintyIndex),
                new StatsTitleStruct(LSID.LSID_BirthByMonth, LSID.LSID_BirthByMonth),
                new StatsTitleStruct(LSID.LSID_Demography, LSID.LSID_Demography),
            };

            CheckSolveNames = new LSID[] {
                LSID.LSID_RM_Skip,
                LSID.LSID_SetIsDead,
                LSID.LSID_DefineSex,
                LSID.LSID_DoDelete,
                LSID.LSID_DoEdit,
                LSID.LSID_Repair
            };

            CondSigns = new string[] {
                "!=", "<", "<=", "==", "=>", ">", "contains", "not contains"
            };

            NameTypes = new LSID[] {
                LSID.LSID_DefaultValue,
                LSID.LSID_NTAka,
                LSID.LSID_NTBirth,
                LSID.LSID_NTImmigrant,
                LSID.LSID_NTMaiden,
                LSID.LSID_NTMarried
            };

            ParentTypes = new LSID[] {
                LSID.LSID_DefaultValue,
                LSID.LSID_PLT_Adopted,
                LSID.LSID_PLT_Birth,
                LSID.LSID_PLT_Foster,
            };

            BloodGroups = "|(I) O+|(I) O-|(II) A+|(II) A-|(III) B+|(III) B-|(IV) AB+|(IV) AB-";

            FARPropertyTypes = new FARPropertyStruct[] {
                new FARPropertyStruct(LSID.LSID_Name, true),
                new FARPropertyStruct(LSID.LSID_Place, false),
                new FARPropertyStruct(LSID.LSID_Address, false),
                new FARPropertyStruct(LSID.LSID_Fact, true),
                new FARPropertyStruct(LSID.LSID_Event, false),
                new FARPropertyStruct(LSID.LSID_Association, true),
            };

            URTreeNoteType = LSID.LSID_URTreeNoteType;
        }
    }
}
