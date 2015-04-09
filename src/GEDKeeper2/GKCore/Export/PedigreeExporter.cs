﻿using System;
using System.Collections.Generic;

using GKCommon;
using GKCommon.GEDCOM;
using GKCommon.GEDCOM.Enums;
using GKCore.Interfaces;
using GKCore.Types;
using iTextSharp.text;
using iTextSharp.text.pdf;
using it = iTextSharp.text;

namespace GKCore.Export
{
    /// <summary>
    /// Localization: dirty
    /// Code-Quality: dirty
    /// </summary>
    public sealed class PedigreeExporter : PDFExporter
	{
		private class PedigreePerson
		{
			public PedigreePerson Parent;
			public string Id;
			public GEDCOMIndividualRecord IRec;
			public int Level;
			//public string BirthDate;
			public readonly List<string> Sources;
			public int FamilyOrder;
			public int ChildIdx;

            public PedigreePerson()
            {
                this.Sources = new List<string>();
            }

			public string GetOrderStr()
			{
				char order = (char)this.FamilyOrder;
				string result = ((this.Parent == null) ? new string(order, 1) : this.Parent.GetOrderStr() + order);
				return result;
			}
		}

		private class PedigreeEvent
		{
			public readonly GEDCOMCustomEvent Event;
			public readonly GEDCOMIndividualRecord IRec;

			public PedigreeEvent(GEDCOMCustomEvent aEvent, GEDCOMIndividualRecord aRec)
			{
				this.Event = aEvent;
				this.IRec = aRec;
			}

			public DateTime GetDate()
			{
				DateTime result = ((this.Event == null) ? new DateTime(0) : GKUtils.GEDCOMDateToDate(this.Event.Detail.Date));
				return result;
			}
		}

		public enum PedigreeKind
		{
			pk_dAboville,
			pk_Konovalov
		}

		private GEDCOMIndividualRecord fAncestor;
		private PedigreeKind fKind;
		private ExtList<PedigreePerson> fPersonList;
		private ShieldState fShieldState;
		private StringList fSourceList;

		private Font fTitleFont;
		private Font fChapFont;
		private Font fPersonFont;
		private Font fLinkFont;
		private Font fTextFont, fSupText;
		
		private PedigreeFormat fFormat;

		public GEDCOMIndividualRecord Ancestor
		{
			get { return this.fAncestor; }
			set { this.fAncestor = value; }
		}

		public PedigreeKind Kind
		{
			get { return this.fKind; }
			set { this.fKind = value; }
		}

		public ShieldState ShieldState
		{
			get { return this.fShieldState; }
			set { this.fShieldState = value; }
		}

		private PedigreePerson FindPerson(GEDCOMIndividualRecord iRec)
		{
            if (iRec == null) return null;

            PedigreePerson res = null;

			int num = this.fPersonList.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				if (this.fPersonList[i].IRec == iRec)
				{
					res = this.fPersonList[i];
					break;
				}
			}

			return res;
		}

		private void WritePerson(PedigreePerson person)
		{
			Paragraph p = new Paragraph();
			p.Alignment = Element.ALIGN_JUSTIFIED;
			p.SpacingBefore = 6f;
			p.SpacingAfter = 6f;
			p.Add(new Chunk(this.GetIdStr(person) + ". " + person.IRec.aux_GetNameStr(true, false), fPersonFont).SetLocalDestination(person.Id));
			p.Add(new Chunk(this.GetPedigreeLifeStr(person.IRec), fTextFont));

			if (this.FOptions.PedigreeOptions.IncludeSources && person.Sources.Count > 0)
			{
				p.Add(new Chunk(" ", fTextFont));

				for (int i = 0; i < person.Sources.Count; i++) {
					string lnk = person.Sources[i];
					
					if (i > 0) {
						p.Add(new Chunk(", ", fTextFont).SetTextRise(4));
					}

					Chunk lnkChunk = new Chunk(lnk, fSupText);
					lnkChunk.SetTextRise(4);
					lnkChunk.SetLocalGoto("src_" + lnk);
					lnkChunk.SetUnderline(0.5f, 3f);
					p.Add(lnkChunk);
				}
			}

			fDocument.Add(p);

			switch (fFormat) {
				case PedigreeFormat.pfExcess:
					this.WriteExcessFmt(person);
					break;

				case PedigreeFormat.pfCompact:
					this.WriteCompactFmt(person);
					break;
			}
		}

		private Chunk idLink(PedigreePerson person)
		{
			if (person != null) {
				return new Chunk(person.Id, fLinkFont).SetLocalGoto(person.Id);
			} else {
				return new Chunk();
			}
		}

		private string GetIdStr(PedigreePerson person)
		{
			string result = person.Id;

			if (this.fKind == PedigreeKind.pk_Konovalov && person.Parent != null)
			{
				GEDCOMFamilyRecord family = person.IRec.ChildToFamilyLinks[0].Family;
				string spStr = "";
				int idx = person.Parent.IRec.IndexOfSpouse(family);
				if (person.Parent.IRec.SpouseToFamilyLinks.Count > 1)
				{
					spStr = "/" + (idx + 1).ToString();
				}
				result += spStr;
			}
			return result;
		}

		private void WriteExcessFmt(PedigreePerson person)
		{
			fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_Sex) + ": " + GKUtils.SexStr(person.IRec.Sex), fTextFont));

			string st = GKUtils.GetLifeExpectancy(person.IRec);
			if (st != "?" && st != "") {
				fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_LifeExpectancy) + ": " + st, fTextFont));
			}

			GEDCOMIndividualRecord father, mother;
			person.IRec.aux_GetParents(out father, out mother);
			if (father != null) {
				Paragraph p = new Paragraph();
				p.Add(new Chunk(LangMan.LS(LSID.LSID_Father) + ": " + father.aux_GetNameStr(true, false) + " ", fTextFont));
				p.Add(this.idLink(this.FindPerson(father)));
				fDocument.Add(p);
			}
			if (mother != null) {
				Paragraph p = new Paragraph();
				p.Add(new Chunk(LangMan.LS(LSID.LSID_Mother) + ": " + mother.aux_GetNameStr(true, false) + " ", fTextFont));
				p.Add(this.idLink(this.FindPerson(mother)));
				fDocument.Add(p);
			}

			ExtList<PedigreeEvent> evList = new ExtList<PedigreeEvent>(true);
			try
			{
				int i;
				if (person.IRec.IndividualEvents.Count > 0)
				{
					fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_Events) + ":", fTextFont));

					int num = person.IRec.IndividualEvents.Count - 1;
					for (i = 0; i <= num; i++)
					{
						GEDCOMCustomEvent evt = person.IRec.IndividualEvents[i];
						if (!(evt is GEDCOMIndividualAttribute) || (evt is GEDCOMIndividualAttribute && this.FOptions.PedigreeOptions.IncludeAttributes))
						{
							evList.Add(new PedigreeEvent(evt, person.IRec));
						}
					}
					this.WriteEventList(person, evList);
				}

				int num2 = person.IRec.SpouseToFamilyLinks.Count - 1;
				for (i = 0; i <= num2; i++)
				{
					GEDCOMFamilyRecord family = person.IRec.SpouseToFamilyLinks[i].Family;
					if (GKUtils.IsRecordAccess(family.Restriction, this.fShieldState))
					{
						GEDCOMPointer sp;
						string unk;
						if (person.IRec.Sex == GEDCOMSex.svMale) {
							sp = family.Wife;
							st = LangMan.LS(LSID.LSID_Wife) + ": ";
							unk = LangMan.LS(LSID.LSID_UnkFemale);
						} else {
							sp = family.Husband;
							st = LangMan.LS(LSID.LSID_Husband) + ": ";
							unk = LangMan.LS(LSID.LSID_UnkMale);
						}

						GEDCOMIndividualRecord irec = sp.Value as GEDCOMIndividualRecord;
						string sps;
						if (irec != null) {
							sps = st + irec.aux_GetNameStr(true, false) + this.GetPedigreeLifeStr(irec) + this.idLink(this.FindPerson(irec));
						} else {
							sps = st + unk;
						}

						fDocument.Add(new Paragraph(sps, fTextFont));

						evList.Clear();
						int num3 = family.Childrens.Count - 1;
						for (int j = 0; j <= num3; j++)
						{
							irec = (family.Childrens[j].Value as GEDCOMIndividualRecord);
							evList.Add(new PedigreeEvent(irec.GetIndividualEvent("BIRT"), irec));
						}
						this.WriteEventList(person, evList);
					}
				}
			}
			finally
			{
				evList.Dispose();
			}

			if (this.FOptions.PedigreeOptions.IncludeNotes && person.IRec.Notes.Count != 0)
			{
				fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_RPNotes) + ":", fTextFont));
				
				it.List list = new it.List(it.List.UNORDERED);
				list.SetListSymbol("\u2022");
				list.IndentationLeft = 10f;

				int num4 = person.IRec.Notes.Count - 1;
				for (int i = 0; i <= num4; i++)
				{
					GEDCOMNotes note = person.IRec.Notes[i];
					list.Add(new it.ListItem(new Chunk(" " + GKUtils.ConStrings(note.Notes), fTextFont)));
				}
				fDocument.Add(list);
			}
		}

		private void WriteCompactFmt(PedigreePerson person)
		{
			if (this.FOptions.PedigreeOptions.IncludeNotes && person.IRec.Notes.Count != 0)
			{
				int num = person.IRec.Notes.Count - 1;
				for (int i = 0; i <= num; i++)
				{
					GEDCOMNotes note = person.IRec.Notes[i];
					fDocument.Add(new Paragraph(GKUtils.ConStrings(note.Notes), fTextFont));
				}
			}

			try
			{
				bool spIndex = person.IRec.SpouseToFamilyLinks.Count > 1;

				int num2 = person.IRec.SpouseToFamilyLinks.Count - 1;
				for (int i = 0; i <= num2; i++)
				{
					GEDCOMFamilyRecord family = person.IRec.SpouseToFamilyLinks[i].Family;
					if (GKUtils.IsRecordAccess(family.Restriction, this.fShieldState))
					{
						GEDCOMPointer sp;
						string st;
						string unk;
						if (person.IRec.Sex == GEDCOMSex.svMale)
						{
							sp = family.Wife;
							st = "Ж";
							unk = LangMan.LS(LSID.LSID_UnkFemale);
						}
						else
						{
							sp = family.Husband;
							st = "М";
							unk = LangMan.LS(LSID.LSID_UnkMale);
						}

						if (spIndex)
						{
							st += (i + 1).ToString();
						}
						st += " - ";

						GEDCOMIndividualRecord irec = sp.Value as GEDCOMIndividualRecord;
						if (irec != null)
						{
							st = st + irec.aux_GetNameStr(true, false) + this.GetPedigreeLifeStr(irec) + this.idLink(this.FindPerson(irec));
						}
						else
						{
							st += unk;
						}

						fDocument.Add(new Paragraph(st, fTextFont));
					}
				}
			}
			finally
			{
			}
		}

		private string GetPedigreeLifeStr(GEDCOMIndividualRecord iRec)
		{
			string res_str = "";

			PedigreeFormat fmt = this.FOptions.PedigreeOptions.Format;

			if (fmt != PedigreeFormat.pfExcess)
			{
				if (fmt == PedigreeFormat.pfCompact)
				{
					string ds = GKUtils.GetBirthDate(iRec, DateFormat.dfDD_MM_YYYY, true);
					string ps = GKUtils.GetBirthPlace(iRec);
					if (ps != "")
					{
						if (ds != "")
						{
							ds += ", ";
						}
						ds += ps;
					}
					if (ds != "")
					{
						ds = "*" + ds;
					}
					res_str += ds;
					ds = GKUtils.GetDeathDate(iRec, DateFormat.dfDD_MM_YYYY, true);
					ps = GKUtils.GetDeathPlace(iRec);
					if (ps != "")
					{
						if (ds != "")
						{
							ds += ", ";
						}
						ds += ps;
					}
					if (ds != "")
					{
						ds = "+" + ds;
					}
					if (ds != "")
					{
						res_str = res_str + " " + ds;
					}
				}
			}
			else
			{
				string ds = GKUtils.GetBirthDate(iRec, DateFormat.dfDD_MM_YYYY, true);
				if (ds == "")
				{
					ds = "?";
				}
				res_str += ds;
				ds = GKUtils.GetDeathDate(iRec, DateFormat.dfDD_MM_YYYY, true);
				if (ds == "")
				{
					GEDCOMCustomEvent ev = iRec.GetIndividualEvent("DEAT");
					if (ev != null)
					{
						ds = "?";
					}
				}
				if (ds != "")
				{
					res_str = res_str + " - " + ds;
				}
			}

			string result;
			if (res_str == "" || res_str == " ")
			{
				result = "";
			}
			else
			{
				result = " (" + res_str + ")";
			}
			return result;
		}

		private void WriteEventList(PedigreePerson aPerson, ExtList<PedigreeEvent> evList)
		{
			int num = evList.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				int num2 = evList.Count - 1;
				for (int j = i + 1; j <= num2; j++)
				{
					if (evList[i].GetDate() > evList[j].GetDate())
					{
						evList.Exchange(i, j);
					}
				}
			}

			int num3 = evList.Count - 1;
			for (int i = 0; i <= num3; i++)
			{
				GEDCOMCustomEvent evt = evList[i].Event;
				if (evt != null && object.Equals(evList[i].IRec, aPerson.IRec))
				{
					if (evt.Name == "BIRT") {
						evList.Exchange(i, 0);
					} else if (evt.Name == "DEAT") {
						evList.Exchange(i, evList.Count - 1);
					}
				}
			}

			it.List list = new it.List(it.List.UNORDERED);
			list.SetListSymbol("\u2022");
			list.IndentationLeft = 10f;

			int num4 = evList.Count - 1;
			for (int i = 0; i <= num4; i++)
			{
				PedigreeEvent evObj = evList[i];
				GEDCOMCustomEvent evt = evObj.Event;
				string li;
				Paragraph p = new Paragraph();
				if (evObj.IRec == aPerson.IRec)
				{
					int ev = GKUtils.GetPersonEventIndex(evt.Name);
					string st;
					if (ev == 0) {
						st = evt.Detail.Classification;
					} else {
						if (ev > 0) {
							st = LangMan.LS(GKData.PersonEvents[ev].Name);
						} else {
							st = evt.Name;
						}
					}

					string dt = GKUtils.GEDCOMCustomDateToStr(evt.Detail.Date, DateFormat.dfDD_MM_YYYY, false);
					li = dt + ": " + st + ".";
					if (evt.Detail.Place.StringValue != "")
					{
						li = li + " " + LangMan.LS(LSID.LSID_Place) + ": " + evt.Detail.Place.StringValue;
					}

					p.Add(new Chunk(" " + li, fTextFont));
				}
				else
				{
					string dt = (evt == null) ? "?" : GKUtils.GEDCOMCustomDateToStr(evt.Detail.Date, DateFormat.dfDD_MM_YYYY, false);

				    string st = (evObj.IRec.Sex == GEDCOMSex.svMale) ? ": Родился " : ": Родилась ";

					li = dt + st + evObj.IRec.aux_GetNameStr(true, false);
					p.Add(new Chunk(" " + li + " ", fTextFont));
					p.Add(this.idLink(this.FindPerson(evObj.IRec)));
				}

				list.Add(new it.ListItem(p));
			}

			fDocument.Add(list);
		}

		protected override void InternalGenerate()
		{
			try
			{
				fFormat = this.FOptions.PedigreeOptions.Format;

				if (this.fAncestor == null)
				{
					GKUtils.ShowError(LangMan.LS(LSID.LSID_NotSelectedPerson));
				}
				else
				{
					string title = LangMan.LS(LSID.LSID_ExpPedigree) + ": " + this.fAncestor.aux_GetNameStr(true, false);

					fDocument.AddTitle("Pedigree");
					fDocument.AddSubject("Pedigree");
					fDocument.AddAuthor("");
					fDocument.AddCreator(GKData.AppTitle);
					fDocument.Open();

					BaseFont baseFont = BaseFont.CreateFont(Environment.ExpandEnvironmentVariables(@"%systemroot%\fonts\Times.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
					fTitleFont = new Font(baseFont, 20f, Font.BOLD);
					fChapFont = new Font(baseFont, 16f, Font.BOLD, BaseColor.BLACK);
					fPersonFont = new Font(baseFont, 10f, Font.BOLD, BaseColor.BLACK);
					fLinkFont = new Font(baseFont, 8f, Font.UNDERLINE, BaseColor.BLUE);
					fTextFont = new Font(baseFont, 8f, Font.NORMAL, BaseColor.BLACK);
					fSupText = new Font(baseFont, 5f, Font.NORMAL, BaseColor.BLUE);

					fDocument.Add(new Paragraph(title, fTitleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 6f });

					this.fPersonList = new ExtList<PedigreePerson>(true);
					this.fSourceList = new StringList();
					try
					{
						this.GenStep(null, this.fAncestor, 1, 1);
						this.ReIndex();

						int curLevel = 0;
						int num = this.fPersonList.Count - 1;
						for (int i = 0; i <= num; i++)
						{
							PedigreePerson person = this.fPersonList[i];
							if (curLevel != person.Level)
							{
								curLevel = person.Level;
								string genTitle = LangMan.LS(LSID.LSID_Generation)+" "+RomeNumbers.GetRome(curLevel);
								fDocument.Add(new Paragraph(genTitle, fChapFont) { Alignment = Element.ALIGN_LEFT, SpacingBefore = 2f, SpacingAfter = 2f });
							}

							this.WritePerson(person);
						}

						if (this.fSourceList.Count > 0)
						{
							fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_RPSources), fChapFont) { Alignment = Element.ALIGN_CENTER });

							int num2 = this.fSourceList.Count - 1;
							for (int j = 0; j <= num2; j++)
							{
								string sn = (j + 1).ToString();
								Chunk chunk = new Chunk(sn + ". " + this.fSourceList[j], fTextFont);
								chunk.SetLocalDestination("src_" + sn);
								fDocument.Add(new Paragraph(chunk));
							}
						}
					}
					finally
					{
                        this.fSourceList.Dispose();
						this.fPersonList.Dispose();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public PedigreeExporter(IBase aBase) : base(aBase)
		{
		}

		private void GenStep(PedigreePerson parent, GEDCOMIndividualRecord iRec, int level, int familyOrder)
		{
			if (iRec != null)
			{
				PedigreePerson res = new PedigreePerson();
				res.Parent = parent;
				res.IRec = iRec;
				res.Level = level;
				res.ChildIdx = 0;
				//res.BirthDate = GKUtils.GetBirthDate(iRec, TDateFormat.dfYYYY_MM_DD, true);
				res.FamilyOrder = familyOrder;
				this.fPersonList.Add(res);

				//string[] i_sources = new string[0];
				int j;

				if (this.FOptions.PedigreeOptions.IncludeSources)
				{
					int num = iRec.SourceCitations.Count - 1;
					for (int i = 0; i <= num; i++)
					{
						GEDCOMSourceRecord sourceRec = iRec.SourceCitations[i].Value as GEDCOMSourceRecord;

						if (sourceRec != null) {
							string srcName = GKUtils.ConStrings(sourceRec.Title);
							if (srcName == "") {
								srcName = sourceRec.FiledByEntry;
							}

							j = this.fSourceList.IndexOf(srcName);
							if (j < 0) {
								j = this.fSourceList.Add(srcName);
							}

							res.Sources.Add((j + 1).ToString());
						}
					}
				}

				int num2 = iRec.SpouseToFamilyLinks.Count - 1;
				for (j = 0; j <= num2; j++)
				{
					GEDCOMFamilyRecord family = iRec.SpouseToFamilyLinks[j].Family;
					if (GKUtils.IsRecordAccess(family.Restriction, this.fShieldState))
					{
						family.aux_SortChilds();

						int num3 = family.Childrens.Count - 1;
						for (int i = 0; i <= num3; i++)
						{
							GEDCOMIndividualRecord child = family.Childrens[i].Value as GEDCOMIndividualRecord;
							GenStep(res, child, level + 1, i + 1);
						}
					}
				}
			}
		}

		private void ReIndex()
		{
			int num = this.fPersonList.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				int num2 = this.fPersonList.Count - 1;
				for (int j = i + 1; j <= num2; j++)
				{
					PedigreePerson obj = this.fPersonList[i];
					PedigreePerson obj2 = this.fPersonList[j];
					string i_str = (char)obj.Level + obj.GetOrderStr();
					string k_str = (char)obj2.Level + obj2.GetOrderStr();
					if (string.Compare(i_str, k_str, false) > 0)
					{
						this.fPersonList.Exchange(i, j);
					}
				}
			}

			int num3 = this.fPersonList.Count - 1;
			for (int i = 0; i <= num3; i++)
			{
				PedigreePerson obj = this.fPersonList[i];

				switch (this.fKind) {
					case PedigreeKind.pk_dAboville:
						if (obj.Parent == null) {
							obj.Id = "1";
						} else {
							obj.Parent.ChildIdx++;
							obj.Id = obj.Parent.Id + "." + obj.Parent.ChildIdx.ToString();
						}
						break;

					case PedigreeKind.pk_Konovalov:
						obj.Id = (i + 1).ToString();
						if (obj.Parent != null)
						{
							string pid = obj.Parent.Id;

							int p = pid.IndexOf("-");
							if (p >= 0) pid = pid.Substring(0, p);

							obj.Id = obj.Id + "-" + pid;
						}
						break;
				}
			}
		}
	}
}
