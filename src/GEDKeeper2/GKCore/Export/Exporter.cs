using System;
using System.Windows.Forms;

using GKCommon;
using GKCommon.GEDCOM;
using GKCore.Interfaces;
using GKCore.Options;

namespace GKCore.Export
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Exporter : BaseObject
	{
		protected GlobalOptions fOptions;
		protected string fPath;
		protected readonly IBaseWindow fBase;
		protected GEDCOMTree fTree;
		protected CustomWriter fWriter;

		public GlobalOptions Options
		{
			get { return this.fOptions; }
			set { this.fOptions = value; }
		}

	    protected Exporter(IBaseWindow baseWin)
		{
            if (baseWin == null)
            {
                throw new ArgumentNullException("baseWin");
            }

            this.fBase = baseWin;
            this.fTree = baseWin.Tree;

			//if (!Directory.Exists(this.FPath)) Directory.CreateDirectory(this.FPath);
		}

		public abstract void Generate(bool show);

		protected bool IsRequireFilename(string filter)
		{
			bool result;

			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				dlg.Filter = filter;
				result = (dlg.ShowDialog() == DialogResult.OK);

				if (result) this.fPath = dlg.FileName;
			}

			return result;
		}

		protected void ShowResult()
		{
            GKUtils.LoadExtFile(this.fPath);
		}

		protected static void PrepareSpecIndex(StringList index, string val, GEDCOMIndividualRecord iRec)
		{
            if (index == null) {
                throw new ArgumentNullException("index");
            }

            if (iRec == null) {
                throw new ArgumentNullException("iRec");
            }

            StringList persons;

			int idx = index.IndexOf(val);
			if (idx < 0) {
				persons = new StringList();
				index.AddObject(val, persons);
			} else {
                persons = (StringList)index.GetObject(idx);
			}

			if (persons.IndexOfObject(iRec) < 0) {
				persons.AddObject(iRec.GetNameString(true, false), iRec);
			}
		}

		protected static void PrepareEventYear(StringList index, GEDCOMCustomEvent evt, GEDCOMIndividualRecord iRec)
		{
			if (evt != null) {
				AbsDate dtx = GEDCOMUtils.GetAbstractDate(evt);
				if (dtx.IsValid()) {
					PrepareSpecIndex(index, dtx.Year.ToString(), iRec);
				}
			}
		}
	}
}
