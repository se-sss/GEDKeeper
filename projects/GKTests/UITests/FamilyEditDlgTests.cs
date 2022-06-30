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

#if !MONO

using System;
using System.Windows.Forms;
using GDModel;
using GKCore.Interfaces;
using GKTests;
using GKTests.Stubs;
using GKUI.Platform;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace GKUI.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class FamilyEditDlgTests : CustomWindowTest
    {
        private GDMFamilyRecord fFamilyRecord;
        private IBaseWindow fBase;
        private FamilyEditDlg fDialog;

        public override void Setup()
        {
            TestUtils.InitGEDCOMProviderTest();
            WFAppHost.ConfigureBootstrap(false);

            fBase = new BaseWindowStub();
            fFamilyRecord = new GDMFamilyRecord(fBase.Context.Tree);

            fDialog = new FamilyEditDlg(fBase);
            fDialog.FamilyRecord = fFamilyRecord;
            fDialog.Show();
        }

        public override void TearDown()
        {
            fDialog.Dispose();
            fFamilyRecord.Dispose();
        }

        [Test]
        public void Test_Cancel()
        {
            ClickButton("btnCancel", fDialog);
        }

        [Test]
        public void Test_EnterDataAndApply()
        {
            Assert.AreEqual(fFamilyRecord, fDialog.FamilyRecord);

            SelectCombo("cmbMarriageStatus", fDialog, 0);

            // The links to other records can be added or edited only in MainWinTests
            // (where there is a complete infrastructure of the calls to BaseWin.ModifyX)

            // empty family record, no effects
            // FIXME: fail! why?!
            //ClickButton("btnHusbandSel", fDialog);
            //ClickButton("btnWifeSel", fDialog);

            ClickButton("btnAccept", fDialog);
        }

        [STAThread]
        [Test]
        public void Test_Common()
        {
            GDMFamilyRecord familyRecord = fDialog.FamilyRecord;
            var tabs = new TabControlTester("tabsFamilyData", fDialog);

            // father
            PersonEditDlgTests.SetCreateIndividualHandler(this, GDMSex.svMale);
            ClickButton("btnHusbandAdd", fDialog);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnHusbandDelete", fDialog);

            // mother
            PersonEditDlgTests.SetCreateIndividualHandler(this, GDMSex.svFemale);
            ClickButton("btnWifeAdd", fDialog);
            ModalFormHandler = MessageBox_YesHandler;
            ClickButton("btnWifeDelete", fDialog);

            // children
            Assert.AreEqual(0, familyRecord.Children.Count);
            tabs.SelectTab(0);
            PersonEditDlgTests.SetCreateIndividualHandler(this, GDMSex.svFemale);
            ClickToolStripButton("fChildsList_ToolBar_btnAdd", fDialog);
            Assert.AreEqual(1, familyRecord.Children.Count);

            SelectSheetListItem("fChildsList", fDialog, 0);
            ModalFormHandler = PersonEditDlgTests.IndividualEdit_Mini_Handler;
            ClickToolStripButton("fChildsList_ToolBar_btnEdit", fDialog);
            Assert.AreEqual(1, familyRecord.Children.Count);

            ModalFormHandler = MessageBox_YesHandler;
            SelectSheetListItem("fChildsList", fDialog, 0);
            ClickToolStripButton("fChildsList_ToolBar_btnDelete", fDialog);
            Assert.AreEqual(0, familyRecord.Children.Count);

            // events
            Assert.AreEqual(0, familyRecord.Events.Count);
            tabs.SelectTab(1);
            SetModalFormHandler(this, EventEditDlgTests.EventEditDlg_Select_Handler);
            ClickToolStripButton("fEventsList_ToolBar_btnAdd", fDialog);
            Assert.AreEqual(1, familyRecord.Events.Count);

            SelectSheetListItem("fEventsList", fDialog, 0);
            SetModalFormHandler(this, EventEditDlgTests.EventEditDlg_Select_Handler);
            ClickToolStripButton("fEventsList_ToolBar_btnEdit", fDialog);
            Assert.AreEqual(1, familyRecord.Events.Count);

            ModalFormHandler = MessageBox_YesHandler;
            SelectSheetListItem("fEventsList", fDialog, 0);
            ClickToolStripButton("fEventsList_ToolBar_btnDelete", fDialog);
            Assert.AreEqual(0, familyRecord.Events.Count);

            StructsDlg_Handler(familyRecord, fDialog, tabs, new int[] { 2, 3, 4 });

            ClickButton("btnAccept", fDialog);
        }

        #region Handlers for external tests

        public static void SpouseEdit_Handler(string name, IntPtr ptr, Form form)
        {
            ClickButton("btnAccept", form);
        }

        public static void FamilyAdd_Mini_Handler(string name, IntPtr ptr, Form form)
        {
            ClickButton("btnAccept", form);
        }

        #endregion
    }
}

#endif
