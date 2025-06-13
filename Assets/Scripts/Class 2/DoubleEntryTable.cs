using System;

namespace Class2
{
    public class DoubleEntryTable<RT, CT, VT>
    {
        private readonly RT[] rows;
        private readonly CT[] columns;
        private readonly VT[,] values;


        public DoubleEntryTable (RT[] rows, CT[] columns)
        {
            this.rows = rows;
            this.columns = columns;
            values = new VT[rows.Length, columns.Length];
        }

        public VT this[RT row, CT column]
        {
            get
            {
                int i = Array.IndexOf(rows, row);
                int j = Array.IndexOf(columns, column);

                return values[i, j];
            }

            set
            {
                int i = Array.IndexOf(rows, row);
                int j = Array.IndexOf(columns, column);

                values[i, j] = value;
            }
        }
    }
}