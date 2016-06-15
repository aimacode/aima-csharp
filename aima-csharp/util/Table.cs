using System.Collections.Generic;
using System.Text;

namespace aima.core.util
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
    public class Table<RowHeaderType, ColumnHeaderType, ValueType> // where ValueType : struct
    {
        private List<RowHeaderType> rowHeaders;
        private List<ColumnHeaderType> columnHeaders;
        private Dictionary<RowHeaderType, Dictionary<ColumnHeaderType, ValueType>> rows;

        public Table(List<RowHeaderType> rowHeaders,
                List<ColumnHeaderType> columnHeaders)
        {

            this.rowHeaders = rowHeaders;
            this.columnHeaders = columnHeaders;
            this.rows = new Dictionary<RowHeaderType, Dictionary<ColumnHeaderType, ValueType>>();
            foreach (RowHeaderType rowHeader in rowHeaders)
            {
                rows.Add(rowHeader, new Dictionary<ColumnHeaderType, ValueType>());
            }
        }

        public void set(RowHeaderType r, ColumnHeaderType c, ValueType v)
        {
            if (rows[r].ContainsKey(c))
            {
                rows[r][c] = v;
            }
            else
            {
                rows[r].Add(c, v);
            }
        }

        public ValueType get(RowHeaderType r, ColumnHeaderType c)
        {
            if (!rows.ContainsKey(r))
            {
                return default(ValueType);
            }
            Dictionary<ColumnHeaderType, ValueType> rowValues = rows[r];

            if (rowValues == null || !rowValues.ContainsKey(c))
            {
                return default(ValueType);
            }
            return rowValues[c];
        }

        public override System.String ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (RowHeaderType r in rowHeaders)
            {
                foreach (ColumnHeaderType c in columnHeaders)
                {
                    buf.Append(get(r, c).ToString());
                    buf.Append(" ");
                }
                buf.Append("\n");
            }
            return buf.ToString();
        }

        class Row<R>
        {
            private Dictionary<ColumnHeaderType, ValueType> c;

            public Row()
            {

                this.c = new Dictionary<ColumnHeaderType, ValueType>();
            }

            public Dictionary<ColumnHeaderType, ValueType> cells()
            {
                return this.c;
            }

        }

        class Cell<ValueHeaderType>
        {
            private ValueHeaderType val;

            public Cell()
            {
                val = default(ValueHeaderType);
            }

            public Cell(ValueHeaderType value)
            {
                this.val = value;
            }

            public void set(ValueHeaderType value)
            {
                this.val = value;
            }

            public ValueHeaderType value()
            {
                return val;
            }

        }
    }
}