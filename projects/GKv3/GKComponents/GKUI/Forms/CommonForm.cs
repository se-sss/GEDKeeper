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

using System;
using System.ComponentModel;
using System.Reflection;
using Eto.Forms;
using GKCore;
using GKCore.Design;
using GKCore.Interfaces;
using GKUI.Themes;

namespace GKUI.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonForm : Form, IView, IThemedView
    {
        private readonly ControlsManager fControlsManager;

        #region View Interface

        public new string Title
        {
            get { return base.Title; }
            set { base.Title = value; }
        }

        #endregion

        public CommonForm()
        {
            fControlsManager = new ControlsManager(this);
        }

        public void SetToolTip(object component, string toolTip)
        {
            if (component != null && !string.IsNullOrEmpty(toolTip)) {
                if (component is Control) {
                    ((Control)component).ToolTip = toolTip;
                } else if (component is ToolItem) {
                    ((ToolItem)component).ToolTip = toolTip;
                }
            }
        }

        public void Activate()
        {
            Focus();
        }

        protected T GetControlHandler<T>(object control) where T : class, IControl
        {
            return fControlsManager.GetControl<T>(control);
        }

        public object GetControl(string controlName)
        {
            var field = this.GetType().GetField(controlName, BindingFlags.NonPublic | BindingFlags.Instance);
            object result = (field == null) ? null : field.GetValue(this);
            if (result == null) {
                result = this.FindChild(controlName);
            }
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }

        public virtual void ApplyTheme()
        {
            if (AppHost.Instance != null) {
                AppHost.Instance.ApplyTheme(this);
            }
        }

        public virtual bool SkipTheme(IDisposable component)
        {
            return false;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CommonWindow : CommonForm, IWindow
    {
        public virtual void Show(bool showInTaskbar)
        {
            ShowInTaskbar = showInTaskbar;
            Show();
        }

        public virtual void SetLocale()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            AppHost.Instance.LoadWindow(this);
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            AppHost.Instance.CloseWindow(this);
            base.OnClosed(e);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CommonWindow<TView, TController> : CommonWindow
        where TView : IView
        where TController : FormController<TView>
    {
        protected TController fController;


        public override void ApplyTheme()
        {
            base.ApplyTheme();
            fController.ApplyTheme();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CommonDialog : Dialog<DialogResult>, ICommonDialog, IThemedView
    {
        private readonly ControlsManager fControlsManager;

        #region View Interface

        public new string Title
        {
            get { return base.Title; }
            set { base.Title = value; }
        }

        #endregion

        public DialogResult DialogResult
        {
            get { return base.Result; }
            set {
                if (base.Result != value) {
                    base.Result = value;
                    if (value != DialogResult.None) {
                        Close();
                    }
                }
            }
        }

        public CommonDialog()
        {
            Maximizable = false;
            Minimizable = false;
            Resizable = false;
            ShowInTaskbar = false;

            fControlsManager = new ControlsManager(this);
        }

        public void SetToolTip(object component, string toolTip)
        {
            if (component != null && !string.IsNullOrEmpty(toolTip)) {
                if (component is Control) {
                    ((Control)component).ToolTip = toolTip;
                } else if (component is ToolItem) {
                    ((ToolItem)component).ToolTip = toolTip;
                }
            }
        }

        public void Activate()
        {
            Focus();
        }

        public virtual bool ShowModalX(IView owner)
        {
            return (ShowModal(owner as Control) == DialogResult.Ok);
        }

        protected virtual void CancelClickHandler(object sender, EventArgs e)
        {
            Close(DialogResult.Cancel);
        }

        protected T GetControlHandler<T>(object control) where T : class, IControl
        {
            return fControlsManager.GetControl<T>(control);
        }

        public object GetControl(string controlName)
        {
            var field = this.GetType().GetField(controlName, BindingFlags.NonPublic | BindingFlags.Instance);
            object result = (field == null) ? null : field.GetValue(this);
            if (result == null) {
                result = this.FindChild(controlName);
            }
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }

        /*protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //BringToFront();
            if (ParentWindow != null) {
                ParentWindow.SendToBack();
            }
        }*/

        public virtual void ApplyTheme()
        {
            if (AppHost.Instance != null) {
                AppHost.Instance.ApplyTheme(this);
            }
        }

        public virtual bool SkipTheme(IDisposable component)
        {
            return false;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CommonDialog<TView, TController> : CommonDialog
        where TView : IView
        where TController : DialogController<TView>
    {
        protected TController fController;

        protected virtual void AcceptClickHandler(object sender, EventArgs e)
        {
            try {
                DialogResult = fController.Accept() ? DialogResult.Ok : DialogResult.None;
            } catch (Exception ex) {
                Logger.WriteError("CommonDialog<>.AcceptClickHandler()", ex);
            }
        }

        protected override void CancelClickHandler(object sender, EventArgs e)
        {
            try {
                DialogResult = fController.Cancel() ? DialogResult.Cancel : DialogResult.None;
            } catch (Exception ex) {
                Logger.WriteError("CommonDialog<>.CancelClickHandler()", ex);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = fController.CheckChangesPersistence();
        }

        /// <summary>
        /// This is necessary so that when user switch to the desired tab,
        /// its nested control can immediately receive events - this is useful for bulk data input.
        /// </summary>
        protected static void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabCtl = (TabControl)sender;
            var selectedTab = tabCtl.SelectedPage;
            if (selectedTab != null && selectedTab.Content != null) {
                selectedTab.Content.Focus();
            }
        }

        public override void ApplyTheme()
        {
            base.ApplyTheme();
            fController.ApplyTheme();
        }
    }
}
