﻿using System.Collections.Generic;
using GKCommon.GEDCOM;

namespace GKCore.Interfaces
{
	public interface IWorkWindow
	{
		string GetStatusString();

		bool NavCanBackward();
		bool NavCanForward();
		void NavNext();
		void NavPrev();

		IList<ISearchResult> FindAll(string searchPattern);
		void QuickFind();
		void SelectByRec(GEDCOMIndividualRecord iRec);
	}
}
