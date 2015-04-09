﻿using System;
using GKCommon.GEDCOM;
using GKCore;
using GKCore.Interfaces;
using GKCore.Types;

/// <summary>
/// 
/// </summary>

namespace GKUI.Lists
{
	public enum RepositoryColumnType : byte
	{
		rctName,		
		rctChangeDate
	}

	public sealed class RepositoryListColumns : ListColumns
	{
		protected override void InitColumnStatics()
		{
			this.AddStatic(LangMan.LS(LSID.LSID_Repository), TDataType.dtString, 400, true);
			this.AddStatic(LangMan.LS(LSID.LSID_Changed), TDataType.dtDateTime, 150, true);
		}

		public RepositoryListColumns() : base()
		{
			InitData(typeof(RepositoryColumnType));
		}
	}

	public sealed class RepositoryListMan : ListManager
	{
		private GEDCOMRepositoryRecord fRec;

		public override bool CheckFilter(ShieldState aShieldState)
		{
			bool res = (this.QuickFilter == "*" || IsMatchesMask(this.fRec.RepositoryName, this.QuickFilter));

			res = res && base.CheckNewFilter();

			return res;
		}

		public override void Fetch(GEDCOMRecord aRec)
		{
			this.fRec = (aRec as GEDCOMRepositoryRecord);
		}

		protected override object GetColumnValueEx(int colType, int colSubtype)
		{
			switch (colType) {
				case 0:
					return this.fRec.RepositoryName;
				case 1:
					return this.fRec.ChangeDate.ChangeDateTime;
				default:
					return null;
			}
		}

		public override Type GetColumnsEnum()
		{
			return typeof(RepositoryColumnType);
		}

		public override ListColumns GetDefaultListColumns()
		{
			return new RepositoryListColumns();
		}

		public RepositoryListMan(GEDCOMTree aTree) : base(aTree)
		{
		}
	}
}
