using System;
using System.Collections.Generic;

using GKCommon;
using GKCommon.GEDCOM;
using GKCore;
using GKCore.Interfaces;
using GKCore.Types;
using GKUI.Controls;

namespace GKUI.Lists
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ListManager : BaseObject, IListManager
	{
		protected struct TColMapRec
		{
			public byte ColType;
			public byte ColSubtype;
		}

		protected ListFilter fFilter;
		protected GEDCOMTree fTree;
	    protected ExternalFilterHandler fExternalFilter;

		private readonly ListColumns fListColumns;
		private readonly List<TColMapRec> fColumnsMap;

		public string QuickFilter = "*";

	    public ExternalFilterHandler ExternalFilter
	    {
	        get { return this.fExternalFilter; }
	        set { this.fExternalFilter = value; }
	    }

		public IListFilter Filter
		{
			get { return this.fFilter; }
		}

		public ListColumns ListColumns
		{
			get { return this.fListColumns; }
		}

	    protected ListManager(GEDCOMTree tree)
		{
			this.fTree = tree;
			this.fListColumns = GetDefaultListColumns();
			this.fColumnsMap = new List<TColMapRec>();

			CreateFilter();
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dummy
            }
            base.Dispose(disposing);
        }

		protected void AddListColumn(GKListView list, string caption, int width, bool autoSize, byte colType, byte colSubType)
		{
			list.AddListColumn(caption, width, autoSize);

			TColMapRec cr = new TColMapRec();
			cr.ColType = colType;
			cr.ColSubtype = colSubType;
			fColumnsMap.Add(cr);
		}

		protected void ColumnsMap_Clear()
		{
			fColumnsMap.Clear();
		}

		protected virtual void CreateFilter()
		{
			this.fFilter = new ListFilter();
		}

		protected static bool IsMatchesMask(string str, string mask)
		{
			bool result = false;

			if (str != null && mask != null && str != "" && mask != "")
			{
				string stx = str.ToLower();
				string[] masks = mask.ToLower().Split(new char[] { '|' });

				int num = masks.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					result = (result || GKUtils.MatchesMask(stx, masks[i]));
				}
			}

			return result;
		}

		public abstract bool CheckFilter(ShieldState aShieldState);
		public abstract void Fetch(GEDCOMRecord aRec);

		public object GetColumnValue(int colIndex)
		{
			// col_index - from 1
			TColMapRec colrec = this.fColumnsMap[colIndex];
			return GetColumnValueEx(colrec.ColType, colrec.ColSubtype);
		}

		protected virtual object GetColumnValueEx(int colType, int colSubtype)
		{
			return null;
		}

		public virtual void InitFilter()
		{
		}

		public virtual void UpdateItem(GKListItem item, bool isMain)
		{
			if (item == null) return;

			for (int i = 1; i < fColumnsMap.Count; i++)
			{
				TColMapRec colrec = this.fColumnsMap[i];

				// aColIndex - from 1
				TColumnStatic cs = this.fListColumns.ColumnStatics[colrec.ColType];
				object val = GetColumnValueEx(colrec.ColType, colrec.ColSubtype);
				string res = ConvertColumnValue(val, cs);

				item.SubItems.Add(res);
			}
		}

		public virtual void UpdateColumns(GKListView listView, bool isMain)
		{
            if (listView == null) return;

			this.ColumnsMap_Clear();
			this.AddListColumn(listView, "�", 50, false, 0, 0);

			for (int i = 0; i < this.fListColumns.ColumnStatics.Count; i++) {
				TColumnStatic cs = this.fListColumns.ColumnStatics[i];
				this.AddListColumn(listView, LangMan.LS(cs.colName), cs.width, false, (byte)i, 0);
			}
		}

		//

		public string GetColumnName(Enum colType)
		{
			int col = (colType as IConvertible).ToByte(null);

            if (col >= 0 && col < fListColumns.ColumnStatics.Count) {
				return LangMan.LS(fListColumns.ColumnStatics[col].colName);
			}

            return "<?>";
		}

		public TDataType GetColumnDataType(int index)
		{
			int col = index - 1;

            if (col >= 0 && col < fListColumns.ColumnStatics.Count) {
				return fListColumns.ColumnStatics[col].dataType;
			}

            return TDataType.dtString;
		}

		public abstract ListColumns GetDefaultListColumns();

		/// <summary>
		/// ������������ � ����� ��������� ����� ����������, TfmComFilter
		/// </summary>
		public abstract Type GetColumnsEnum();

		private static string ConvertColumnValue(object val, TColumnStatic cs)
		{
			switch (cs.dataType) {
				case TDataType.dtString:
					return val.ToString();

				case TDataType.dtInteger:
					return val.ToString();

				case TDataType.dtDate:
					return val.ToString();

				case TDataType.dtDateTime:
					DateTime dtx = ((DateTime)val);
					return ((dtx.Ticks == 0) ? "" : dtx.ToString("yyyy.MM.dd HH:mm:ss", null));

				case TDataType.dtFloat:
					return ((double)val).ToString(cs.format, cs.nfi);
					
				default:
					return val.ToString();
			}
		}

		private static object ConvertColumnStr(string val, TDataType type)
		{
			switch (type) {
				case TDataType.dtString:
					return val;
				case TDataType.dtInteger:
					return SysUtils.ParseInt(val, 0);
				case TDataType.dtFloat:
					return SysUtils.ParseFloat(val, 0.0);
				case TDataType.dtDate:
				case TDataType.dtDateTime:
					return DateTime.Parse(val);
			}
			return val;
		}

		public void AddCondition(Enum column, TConditionKind condition, string value)
		{
			int col = (column as IConvertible).ToByte(null);

			TFilterCondition fltCond = new TFilterCondition();
			fltCond.column = column;
			fltCond.col_index = col;
			fltCond.condition = condition;
			fltCond.value = ConvertColumnStr(value, this.GetColumnDataType(col));
			this.Filter.ColumnsFilter.Add(fltCond);
		}

		private bool CheckCondition(TFilterCondition fcond)
		{
			object dataval = this.GetColumnValueEx(fcond.col_index, -1);
		    if (dataval == null) return true;

            bool res = true;

            int compRes = 0;
			if (fcond.condition != TConditionKind.ck_Contains) {
				compRes = (dataval as IComparable).CompareTo(fcond.value);
			}

			switch (fcond.condition) {
				case TConditionKind.ck_NotEq:
					res = compRes != 0;
					break;
				case TConditionKind.ck_LT:
					res = compRes < 0;
					break;
				case TConditionKind.ck_LET:
					res = compRes <= 0;
					break;
				case TConditionKind.ck_Eq:
					res = compRes == 0;
					break;
				case TConditionKind.ck_GET:
					res = compRes >= 0;
					break;
				case TConditionKind.ck_GT:
					res = compRes > 0;
					break;
				case TConditionKind.ck_Contains:
					res = GKUtils.MatchesMask(dataval.ToString(), "*" + fcond.value.ToString() + "*");
					break;
				case TConditionKind.ck_NotContains:
					res = !GKUtils.MatchesMask(dataval.ToString(), "*" + fcond.value.ToString() + "*");
					break;
			}
			return res;
		}

		protected bool CheckNewFilter()
		{
			bool res = true;

			if (this.Filter.ColumnsFilter.Count > 0)
			{
			    int num = this.Filter.ColumnsFilter.Count;
				for (int i = 0; i < num; i++) {
					TFilterCondition fcond = this.Filter.ColumnsFilter[i];
					res = res && this.CheckCondition(fcond);
				}
			}

			return res;
		}
	}
}
