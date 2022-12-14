using EEANWorks.WinFormsApps.AIDELInformationManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinForms
{
    public static class FormExtension
    {
        public static void SwitchTo(this Form _form, Form _targetForm)
        {
            try
            {
                _form.Hide();
                _targetForm.FormClosed += (_sender, _e) => _form.Close();
                _targetForm.StartPosition = FormStartPosition.Manual;
                _targetForm.Location = _form.Location;
                _targetForm.Show();
            }
            catch (Exception ex)
            {
                // Do nothing (Just to prevent app hanging on form closing)
            }
        }

        public static void LogAndSwitchTo(this Form _form, Form _targetForm)
        {
            Log.Instance.PreviousForms.Add(_form);

            _form.SwitchTo(_targetForm);
        }

        public static void SwitchToPrevious(this Form _form)
        {
            var log = Log.Instance.PreviousForms;

            _form.SwitchTo(log.Last());

            log.RemoveAt(log.Count - 1);
        }
    }

    public static class DataGridViewExtension
    {
        public static bool AllRowsEqual(this DataGridView _dataGridView, DataGridView _targetDgv)
        {
            var rows = _dataGridView.Rows;
            var targetRows = _targetDgv.Rows;

            if (rows.Count != targetRows.Count)
                return false;

            foreach (DataGridViewRow row in rows)
            {
                if (!targetRows.Any(row))
                    return false;
            }

            return true;
        }
    }

    public static class DataGridViewRowCollectionExtension
    {
        public static DataGridViewRow Last(this DataGridViewRowCollection _dataGridViewRowCollection)
        {
            return _dataGridViewRowCollection[_dataGridViewRowCollection.GetLastRow(DataGridViewElementStates.None)];
        }

        public static bool Any(this DataGridViewRowCollection _dataGridViewRowCollection, DataGridViewRow _targetRow)
        {
            foreach (DataGridViewRow row in _dataGridViewRowCollection)
            {
                if (row.AllCellsEqual(_targetRow))
                    return true;
            }

            return false;
        }

        public static void Load(this DataGridViewRowCollection _dataGridViewRowCollection, DataGridViewRowCollection _targetRows)
        {
            foreach (DataGridViewRow row in _targetRows)
            {
                DataGridViewRow copy = (DataGridViewRow)(row.Clone());

                int cellIndex = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    copy.Cells[cellIndex].Value = cell.Value;
                    cellIndex++;
                }

                _dataGridViewRowCollection.Add(copy);
            }
        }
    }

    public static class DataGridViewRowExtension
    {
        public static bool AllCellsEqual(this DataGridViewRow _dataGridViewRow, DataGridViewRow _targetRow)
        {
            var cells = _dataGridViewRow.Cells;
            var targetCells = _targetRow.Cells;

            if (cells.Count != targetCells.Count)
                return false;

            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].Value != targetCells[i].Value)
                    return false;

                if (cells[i].Style.BackColor != targetCells[i].Style.BackColor)
                    return false;
            }

            return true;
        }

        public static int CopyTo(this DataGridViewRow _row, DataGridView _targetDgv, int _numOfCellsToCopy = -1)
        {
            var cells = _row.Cells;

            int numOfCells = cells.Count;
            if (_numOfCellsToCopy > 0 && _numOfCellsToCopy < numOfCells)
                numOfCells = _numOfCellsToCopy;
            object[] cellValues = new object[numOfCells];
            for (int i = 0; i < numOfCells; i++)
            {
                cellValues[i] = cells[i].Value;
            }

            int newRowIndex = _targetDgv.Rows.Add(cellValues);
            for (int i = 0; i < numOfCells; i++)
            {
                _targetDgv.Rows[newRowIndex].Cells[i].Style.BackColor = cells[i].Style.BackColor;
            }

            return newRowIndex;
        }

        public static int MoveTo(this DataGridViewRow _row, DataGridView _targetDgv, int _numOfCellsToMove = -1)
        {
            // Copy row to target table
            int newRowIndex = _row.CopyTo(_targetDgv, _numOfCellsToMove);

            // Remove row from this table
            _row.DataGridView.Rows.Remove(_row);

            return newRowIndex;
        }
    }
}
