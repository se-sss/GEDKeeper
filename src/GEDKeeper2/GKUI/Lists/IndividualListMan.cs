﻿using System;
using GKCommon.GEDCOM;
using GKCommon.GEDCOM.Enums;
using GKCore;
using GKCore.Interfaces;
using GKCore.Options;
using GKCore.Types;
using GKUI.Controls;

/// <summary>
/// 
/// </summary>

namespace GKUI.Lists
{

	public enum PersonColumnType : byte
	{
		pctPatriarch,
		pctName,
		pctNick,
		pctSex,
		pctBirthDate,
		pctDeathDate,
		pctBirthPlace,
		pctDeathPlace,
		pctResidence,
		pctAge,
		pctLifeExpectancy,
		pctDaysForBirth,
		pctGroups,
		pctReligion,
		pctNationality,
		pctEducation,
		pctOccupation,
		pctCaste,
		pctMili,
		pctMiliInd,
		pctMiliDis,
		pctMiliRank,
		pctChangeDate,
		pctBookmark,
		pctTitle
	}

	public sealed class IndividualListColumns : ListColumns
	{
		protected override void InitColumnStatics()
		{
			this.AddStatic(LSID.LSID_Patriarch, TDataType.dtString, 25, true);
			this.AddStatic(LSID.LSID_FullName, TDataType.dtString, 300, true);
			this.AddStatic(LSID.LSID_Nickname, TDataType.dtString, 75, false);
			this.AddStatic(LSID.LSID_Sex, TDataType.dtString, 45, true);
			this.AddStatic(LSID.LSID_BirthDate, TDataType.dtString, 100, true);
			this.AddStatic(LSID.LSID_DeathDate, TDataType.dtString, 100, true);
			this.AddStatic(LSID.LSID_BirthPlace, TDataType.dtString, 100, true);
			this.AddStatic(LSID.LSID_DeathPlace, TDataType.dtString, 100, true);
			this.AddStatic(LSID.LSID_Residence, TDataType.dtString, 100, true);
			this.AddStatic(LSID.LSID_Age, TDataType.dtString, 100, false);
			this.AddStatic(LSID.LSID_LifeExpectancy, TDataType.dtString, 100, false);
			this.AddStatic(LSID.LSID_DaysForBirth, TDataType.dtString, 100, false);
			this.AddStatic(LSID.LSID_RPGroups, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Religion, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Nationality, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Education, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Occupation, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Caste, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Mili, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_MiliInd, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_MiliDis, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_MiliRank, TDataType.dtString, 200, false);
			this.AddStatic(LSID.LSID_Changed, TDataType.dtDateTime, 150, true);
			this.AddStatic(LSID.LSID_Bookmark, TDataType.dtString, 25, true);
			this.AddStatic(LSID.LSID_NobilityTitle, TDataType.dtString, 200, false);
		}

		public IndividualListColumns()
		{
			InitData(typeof(PersonColumnType));
		}
	}

	public class IndividualListFilter : ListFilter, IIndividualListFilter
	{
		public string AliveBeforeDate;
		public FilterGroupMode FilterGroupMode;
		public string GroupRef;
		public string Name;
		public bool PatriarchOnly;
		public string Residence;
		public GEDCOMSex Sex;
		public FilterGroupMode SourceMode;
		public string SourceRef;
		public string EventVal;

		public FilterLifeMode FilterLifeMode { get; set; }

		public IndividualListFilter()
		{
			this.Clear();
		}
		
		public override void Clear()
		{
			base.Clear();

			this.FilterGroupMode = FilterGroupMode.gmAll;
			this.GroupRef = "";
			if (this.FilterLifeMode != FilterLifeMode.lmTimeLocked)
			{
				this.FilterLifeMode = FilterLifeMode.lmAll;
			}
			this.Name = "*";
			this.AliveBeforeDate = "";
			this.PatriarchOnly = false;
			this.Residence = "*";
			this.Sex = GEDCOMSex.svNone;
			this.SourceMode = FilterGroupMode.gmAll;
			this.SourceRef = "";
			this.EventVal = "*";
		}
	}

	public sealed class IndividualListMan : ListManager
	{
		private GEDCOMIndividualRecord fRec;
		private GEDCOMGroupRecord filter_grp;
		private DateTime filter_abd;
		private GEDCOMSourceRecord filter_source;

		protected override void CreateFilter()
		{
			this.fFilter = new IndividualListFilter();
		}

		private string GetGroups()
		{
			string result = "";

			int count = this.fRec.Groups.Count;
			for (int idx = 0; idx < count; idx++)
			{
				GEDCOMGroupRecord grp = this.fRec.Groups[idx].Value as GEDCOMGroupRecord;
				if (grp != null)
				{
					result += grp.GroupName;
					if (idx < count - 1) result += "; ";
				}
			}

			return result;
		}

		private bool HasPlace()
		{
			bool res = false;

			IndividualListFilter iFilter = fFilter as IndividualListFilter;
			bool addr = TfmGEDKeeper.Instance.Options.PlacesWithAddress;
			int num = this.fRec.IndividualEvents.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				string place = GKUtils.GetPlaceStr(this.fRec.IndividualEvents[i], addr);
				res = IsMatchesMask(place, iFilter.Residence);
				if (res) break;
			}

			return res;
		}

		private bool HasEventVal()
		{
			bool result = false;
			
			IndividualListFilter iFilter = fFilter as IndividualListFilter;
			int num = this.fRec.IndividualEvents.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				result = IsMatchesMask(this.fRec.IndividualEvents[i].StringValue, iFilter.EventVal);
				if (result) break;
			}

			return result;
		}

		private bool CheckSpecificFilter(ShieldState aShieldState)
		{
			bool result = false;

			IndividualListFilter iFilter = fFilter as IndividualListFilter;

			string fullname = this.fRec.aux_GetNameStr(true, false);

			if ((this.fRec.Restriction != GEDCOMRestriction.rnPrivacy || aShieldState == ShieldState.ssNone)
			    && (iFilter.Sex == GEDCOMSex.svNone || this.fRec.Sex == iFilter.Sex)
			    && (iFilter.Name == "*" || IsMatchesMask(fullname, iFilter.Name))
			    && (this.QuickFilter == "*" || IsMatchesMask(fullname, this.QuickFilter))
				&& (iFilter.Residence == "*" || this.HasPlace())
				&& (iFilter.EventVal == "*" || this.HasEventVal())
				&& (!iFilter.PatriarchOnly || this.fRec.Patriarch))
			{
				bool isLive = (buf_dd == null);

				switch (iFilter.FilterLifeMode) {
					case FilterLifeMode.lmOnlyAlive:
						if (!isLive) return result;
						break;

					case FilterLifeMode.lmOnlyDead:
						if (isLive) return result;
						break;

					case FilterLifeMode.lmAliveBefore:
                        DateTime bdt = ((buf_bd == null) ? new DateTime(0) : GKUtils.GEDCOMDateToDate(buf_bd.Detail.Date));
                        DateTime ddt = ((buf_dd == null) ? new DateTime(0) : GKUtils.GEDCOMDateToDate(buf_dd.Detail.Date));
						if ((bdt > this.filter_abd) || (ddt < this.filter_abd)) return result;
						break;

					case FilterLifeMode.lmTimeLocked:
						break;
				}

				switch (iFilter.FilterGroupMode) {
					case FilterGroupMode.gmAll:
						break;
					case FilterGroupMode.gmNone:
						if (this.fRec.Groups.Count != 0) return result;
						break;
					case FilterGroupMode.gmAny:
						if (this.fRec.Groups.Count == 0) return result;
						break;
					case FilterGroupMode.gmSelected:
						if (this.fRec.IndexOfGroup(this.filter_grp) < 0) return result;
						break;
				}

				switch (iFilter.SourceMode) {
					case FilterGroupMode.gmAll:
						break;
					case FilterGroupMode.gmNone:
						if (this.fRec.SourceCitations.Count != 0) return result;
						break;
					case FilterGroupMode.gmAny:
						if (this.fRec.SourceCitations.Count == 0) return result;
						break;
					case FilterGroupMode.gmSelected:
						if (this.fRec.IndexOfSource(this.filter_source) < 0) return result;
						break;
				}

				result = true;
			}

			return result;
		}

		public override bool CheckFilter(ShieldState aShieldState)
		{
			string fullname = this.fRec.aux_GetNameStr(true, false);
			bool res = (this.QuickFilter == "*" || IsMatchesMask(fullname, this.QuickFilter));

			res = res && base.CheckNewFilter() && this.CheckSpecificFilter(aShieldState);

            if (this.fExternalFilter != null) {
                res = res && this.fExternalFilter(this.fRec);
            }

			return res;
		}

		protected override object GetColumnValueEx(int colType, int colSubtype)
		{
			PersonColumnType pct = (PersonColumnType)colType;

			object result = null;

			switch (pct) {
				case PersonColumnType.pctPatriarch:
					result = ((this.fRec.Patriarch) ? "*" : " ");
					break;

				case PersonColumnType.pctName:
				{
					if (colSubtype == -1) {
						result = this.fRec.aux_GetNameStr(true, false);
					} else {
						NameFormat defNameFormat = TfmGEDKeeper.Instance.Options.DefNameFormat;
						string f, i, p;

						switch (defNameFormat) {
							case NameFormat.nfFNP:
								result = this.fRec.aux_GetNameStr(true, false);
								break;

							case NameFormat.nfF_NP:
								this.fRec.GetNameParts(out f, out i, out p);
								switch (colSubtype) {
									case 0:
										result = f;
										break;
									case 1:
										result = i + " " + p;
										break;
								}
								break;

							case NameFormat.nfF_N_P:
								this.fRec.GetNameParts(out f, out i, out p);
								switch (colSubtype) {
									case 0:
										result = f;
										break;
									case 1:
										result = i;
										break;
									case 2:
										result = p;
										break;
								}
								break;
						}
					}
					
					break;
				}

				case PersonColumnType.pctNick:
					result = this.fRec.aux_GetNickStr();
					break;

				case PersonColumnType.pctSex:
					result = new string(GKUtils.SexStr(this.fRec.Sex)[0], 1);
					break;

				case PersonColumnType.pctBirthDate:
					result = GKUtils.GEDCOMEventToDateStr(buf_bd, TfmGEDKeeper.Instance.Options.DefDateFormat, false);
					break;

				case PersonColumnType.pctBirthPlace:
					result = GKUtils.GetPlaceStr(buf_bd, false);
					break;

				case PersonColumnType.pctDeathDate:
					result = GKUtils.GEDCOMEventToDateStr(buf_dd, TfmGEDKeeper.Instance.Options.DefDateFormat, false);
					break;

				case PersonColumnType.pctDeathPlace:
					result = GKUtils.GetPlaceStr(buf_dd, false);
					break;

				case PersonColumnType.pctResidence:
					result = buf_residence;
					break;

				case PersonColumnType.pctAge:
					result = GKUtils.GetAge(this.fRec, -1);
					break;

				case PersonColumnType.pctLifeExpectancy:
					result = GKUtils.GetLifeExpectancy(this.fRec);
					break;

				case PersonColumnType.pctDaysForBirth:
					result = GKUtils.GetDaysForBirth(this.fRec);
					break;

				case PersonColumnType.pctGroups:
					result = this.GetGroups();
					break;

				case PersonColumnType.pctReligion:
					result = buf_religion;
					break;

				case PersonColumnType.pctNationality:
					result = buf_nationality;
					break;

				case PersonColumnType.pctEducation:
					result = buf_education;
					break;

				case PersonColumnType.pctOccupation:
					result = buf_occupation;
					break;

				case PersonColumnType.pctCaste:
					result = buf_caste;
					break;

				case PersonColumnType.pctMili:
					result = buf_mili;
					break;

				case PersonColumnType.pctMiliInd:
					result = buf_mili_ind;
					break;

				case PersonColumnType.pctMiliDis:
					result = buf_mili_dis;
					break;

				case PersonColumnType.pctMiliRank:
					result = buf_mili_rank;
					break;

				case PersonColumnType.pctChangeDate:
					result = this.fRec.ChangeDate.ChangeDateTime;
					break;

				case PersonColumnType.pctBookmark:
					result = ((this.fRec.Bookmark) ? "*" : " ");
					break;

				case PersonColumnType.pctTitle:
					result = buf_title;
					break;
			}
			return result;
		}

		public override void InitFilter()
		{
			IndividualListFilter iFilter = (IndividualListFilter)fFilter;
			
			if (!DateTime.TryParse(iFilter.AliveBeforeDate, out this.filter_abd)) {
				this.filter_abd = new DateTime(0);
			}

			if (iFilter.GroupRef == "") {
				this.filter_grp = null;
			} else {
				this.filter_grp = this.fTree.XRefIndex_Find(iFilter.GroupRef) as GEDCOMGroupRecord;
			}

			if (iFilter.SourceRef == "") {
				this.filter_source = null;
			} else {
				this.filter_source = this.fTree.XRefIndex_Find(iFilter.SourceRef) as GEDCOMSourceRecord;
			}
		}

		private GEDCOMCustomEvent buf_bd;
		private GEDCOMCustomEvent buf_dd;

		private string buf_residence;
		private string buf_religion;
		private string buf_nationality;
		private string buf_education;
		private string buf_occupation;
		private string buf_caste;
		private string buf_mili;
		private string buf_mili_ind;
		private string buf_mili_dis;
		private string buf_mili_rank;
		private string buf_title;

		public override void Fetch(GEDCOMRecord aRec)
		{
			this.fRec = (aRec as GEDCOMIndividualRecord);

			buf_bd = null;
			buf_dd = null;
			buf_residence = "";
			buf_religion = "";
			buf_nationality = "";
			buf_education = "";
			buf_occupation = "";
			buf_caste = "";
			buf_mili = "";
			buf_mili_ind = "";
			buf_mili_dis = "";
			buf_mili_rank = "";
			buf_title = "";

			GlobalOptions gOptions = TfmGEDKeeper.Instance.Options;
			int num = this.fRec.IndividualEvents.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				GEDCOMCustomEvent ev = this.fRec.IndividualEvents[i];

				if (ev.Name == "BIRT" && buf_bd == null)
				{
					buf_bd = ev;
				}
				else if (ev.Name == "DEAT" && buf_dd == null)
				{
					buf_dd = ev;
				}
				else if (ev.Name == "RESI" && buf_residence == "")
				{
					buf_residence = GKUtils.GetPlaceStr(ev, gOptions.PlacesWithAddress);
				}
				else if (ev.Name == "RELI" && buf_religion == "")
				{
					buf_religion = ev.StringValue;
				}
				else if (ev.Name == "NATI" && buf_nationality == "")
				{
					buf_nationality = ev.StringValue;
				}
				else if (ev.Name == "EDUC" && buf_education == "")
				{
					buf_education = ev.StringValue;
				}
				else if (ev.Name == "OCCU" && buf_occupation == "")
				{
					buf_occupation = ev.StringValue;
				}
				else if (ev.Name == "CAST" && buf_caste == "")
				{
					buf_caste = ev.StringValue;
				}
				else if (ev.Name == "_MILI" && buf_mili == "")
				{
					buf_mili = ev.StringValue;
				}
				else if (ev.Name == "_MILI_IND" && buf_mili_ind == "")
				{
					buf_mili_ind = ev.StringValue;
				}
				else if (ev.Name == "_MILI_DIS" && buf_mili_dis == "")
				{
					buf_mili_dis = ev.StringValue;
				}
				else if (ev.Name == "_MILI_RANK" && buf_mili_rank == "")
				{
					buf_mili_rank = ev.StringValue;
				}
				else if (ev.Name == "TITL" && buf_title == "")
				{
					buf_title = ev.StringValue;
				}
			}
		}

		public override void UpdateItem(GKListItem item, bool isMain)
		{
			base.UpdateItem(item, isMain);

			GlobalOptions gOptions = TfmGEDKeeper.Instance.Options;

			if ((fRec.ChildToFamilyLinks.Count == 0) && (gOptions.ListPersons_HighlightUnparented))
			{
				item.BackColor = System.Drawing.Color.FromArgb(0xFFCACA);
			}
			else if ((fRec.SpouseToFamilyLinks.Count == 0) && (gOptions.ListPersons_HighlightUnmarried))
			{
				item.BackColor = System.Drawing.Color.FromArgb(0xFFFFCA);
			}
		}

		public override void UpdateColumns(GKListView aList, bool isMain)
		{
			IndividualListColumns columns = TfmGEDKeeper.Instance.Options.IndividualListColumns;
			NameFormat defNameFormat = TfmGEDKeeper.Instance.Options.DefNameFormat;

			this.ColumnsMap_Clear();
			this.AddListColumn(aList, "№", 50, false, 0, 0);

			for (int i = 0; i < columns.Count; i++)
			{
				if (columns[i].colActive) {
					byte bColType = columns[i].colType;
					PersonColumnType colType = (PersonColumnType)bColType;
					int colWidth = columns[i].colWidth;

					if (colType == PersonColumnType.pctName) {
					    const bool asz = false;

					    switch (defNameFormat) {
							case NameFormat.nfF_N_P:
					    		this.AddListColumn(aList, LangMan.LS(LSID.LSID_Surname), 150, asz, bColType, 0);
					    		this.AddListColumn(aList, LangMan.LS(LSID.LSID_Name), 100, asz, bColType, 1);
					    		this.AddListColumn(aList, LangMan.LS(LSID.LSID_Patronymic), 150, asz, bColType, 2);
								break;

							case NameFormat.nfF_NP:
								this.AddListColumn(aList, LangMan.LS(LSID.LSID_Surname), 150, asz, bColType, 0);
								this.AddListColumn(aList, LangMan.LS(LSID.LSID_Name) + "," + LangMan.LS(LSID.LSID_Patronymic), 150, asz, bColType, 1);
								break;

							case NameFormat.nfFNP:
								this.AddListColumn(aList, LangMan.LS(LSID.LSID_FullName), colWidth, asz, bColType, 0);
								break;
						}
					} else {
						string colName = LangMan.LS(columns.ColumnStatics[bColType].colName);
						this.AddListColumn(aList, colName, colWidth, false, bColType, 0);
					}
				}
			}
		}

		public override Type GetColumnsEnum()
		{
			return typeof(PersonColumnType);
		}

		public override ListColumns GetDefaultListColumns()
		{
			return new IndividualListColumns();
		}

		public IndividualListMan(GEDCOMTree aTree) : base(aTree)
		{
		}
	}
}
