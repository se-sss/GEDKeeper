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
using System.Collections.ObjectModel;
using Eto.Forms;
using GKCore;
using GKCore.Design.Controls;
using GKCore.Interfaces;

namespace GKUI.Components
{
    public class FilterGridView : GridView, IFilterGridView
    {
        private class FilterConditionRow : FilterCondition
        {
            private readonly FilterGridView fGrid;

            public string ColumnText
            {
                get {
                    return fGrid.fFields[ColumnIndex + 1];
                }
                set {
                    ColumnIndex = fGrid.fListMan.GetFieldColumnId(fGrid.fFields, value);
                }
            }

            public string ConditionText
            {
                get {
                    int condIndex = ((IConvertible)Condition).ToByte(null);
                    return GKData.CondSigns[condIndex];
                }
                set {
                    Condition = fGrid.fListMan.GetCondByName(value);
                }
            }

            public string ValueText
            {
                get {
                    return Value.ToString();
                }
                set {
                    Value = value;
                }
            }


            public FilterConditionRow(FilterGridView grid, FilterCondition filterCondition)
                : base(filterCondition.ColumnIndex, filterCondition.Condition, filterCondition.Value)
            {
                fGrid = grid;
            }
        }


        private IRecordsListModel fListMan;
        private ObservableCollection<FilterConditionRow> fCollection;
        private string[] fFields;


        public IRecordsListModel ListMan
        {
            get {
                return fListMan;
            }
            set {
                fListMan = value;
                fFields = fListMan.CreateFields();
                InitGrid();
            }
        }


        public FilterGridView()
        {
            fCollection = new ObservableCollection<FilterConditionRow>();
            DataStore = fCollection;
        }

        public int Count
        {
            get { return fCollection.Count; }
        }

        public FilterCondition this[int index]
        {
            get { return fCollection[index]; }
        }

        public void AddCondition(FilterCondition fcond)
        {
            fCollection.Add(new FilterConditionRow(this, fcond));
        }

        public void RemoveCondition(int index)
        {
            if (index >= 0 && index < fCollection.Count) {
                fCollection.RemoveAt(index);
            }
        }

        public void Clear()
        {
            fCollection.Clear();
        }

        public void Activate()
        {
            Focus();
        }

        #region Private functions

        private void InitGrid()
        {
            // "00/00/0000"

            var clmCell = new ComboBoxCell();
            clmCell.DataStore = fFields;
            clmCell.Binding = Binding.Property<FilterConditionRow, object>(r => r.ColumnText);

            Columns.Add(new GridColumn {
                DataCell = clmCell,
                HeaderText = LangMan.LS(LSID.LSID_Field),
                Width = 200,
                Editable = true
            });

            var condCell = new ComboBoxCell();
            condCell.DataStore = GKData.CondSigns;
            condCell.Binding = Binding.Property<FilterConditionRow, object>(r => r.ConditionText);

            Columns.Add(new GridColumn {
                DataCell = condCell,
                HeaderText = LangMan.LS(LSID.LSID_Condition),
                Width = 150,
                Editable = true
            });

            Columns.Add(new GridColumn {
                DataCell = new TextBoxCell { Binding = Binding.Property<FilterConditionRow, string>(r => r.ValueText) },
                HeaderText = LangMan.LS(LSID.LSID_Value),
                Width = 300,
                Editable = true
            });
        }

        private bool IsGEDCOMDateCell(int rowIndex)
        {
            /*DataGridViewRow row = dataGridView1.Rows[rowIndex];

            string fld = (string)row.Cells[0].Value;
            if (!string.IsNullOrEmpty(fld)) {
                int colId = fController.GetFieldColumnId(fld);
                DataType dataType = fListMan.GetColumnDataType(colId);

                return (dataType == DataType.dtGEDCOMDate);
            }*/

            return false;
        }

        /*
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex < dataGridView1.NewRowIndex) {
                if (IsGEDCOMDateCell(e.RowIndex)) {
                }
            }
        }
        */

        #endregion

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // does not receive focus without this handler,
            // without focus does not receive keyboard events
            if (!HasFocus) {
                Focus();
            }

            e.Handled = true;
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key) {
                case Keys.I:
                    if (e.Control) {
                        FilterCondition fcond = new FilterCondition(0, ConditionKind.ck_Contains, "");
                        AddCondition(fcond);
                    }
                    break;

                case Keys.D:
                    if (e.Control) {
                        RemoveCondition(SelectedRow);
                    }
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }
    }
}
