using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using numl.Math;
using System.Globalization;

namespace numl.Visualizers
{
    public partial class DataVisualizerForm : Form
    {
        private Matrix _x = null;
        private Vector _v = null;
        public DataVisualizerForm()
        {
            InitializeComponent();
        }

        public void SetData(Matrix x)
        {
            _x = x;
            BindMatrix();
            gridData.AutoResizeColumns();
        }

        public void SetData(Vector v)
        {
            _v = v;
            BindVector();
            gridData.AutoResizeColumns();
        }

        private void BindMatrix()
        {
            gridData.Columns.Clear();
            DataGridViewColumn[] cols = new DataGridViewColumn[_x.Cols];
            for (int i = 0; i < _x.Cols; i++)
            {
                gridData.Columns.Add(i.ToString(), i.ToString());
                gridData.Columns[i].Width = 160;
            }
                
            
            gridData.Rows.Clear();
            gridData.Rows.Add(_x.Rows);

            for (int i = 0; i < _x.Rows; i++)
                for (int j = 0; j < _x.Cols; j++)
                    gridData.Rows[i].Cells[j].Value = _x[i, j];
        }

        private void BindVector()
        {
            gridData.BeginEdit(false);
            gridData.Columns.Clear();
            gridData.Columns.Add("0", "0");
            gridData.Columns[0].Width = 160;
            gridData.Rows.Clear();
            gridData.Rows.Add(_v.Length);

            for(int i = 0; i < _v.Length; i++)
                gridData.Rows[i].Cells[0].Value = _v[i];
            gridData.EndEdit();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            double value = 0;
            if (double.TryParse(gridData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out value))
            {
                if (_x == null)
                    _v[e.RowIndex] = value;
                else
                    _x[e.RowIndex, e.ColumnIndex] = value;
            }
            else
            {
                MessageBox.Show("Entry must be numeric!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (_x == null)
                    gridData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _v[e.RowIndex];
                else
                    gridData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =_x[e.RowIndex, e.ColumnIndex];
            }
        }
    }
}
