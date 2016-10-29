using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace Aida.Model
{
    public class Result
    {
        public bool success;
        public string msg;
    }

    public class ListItem
    {
        public string name { get; set; }
        public Sklad itemList { get; set; }
    }

    public class ListItemApp
    {
        public string name { get; set; }
        public ApplicationDoc itemList { get; set; }
    }

    public class NewDataGridTextBoxColumn : DataGridTextBoxColumn
    {        
        public DataTable dt;
        public Font dgFont;

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            Rectangle rc = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            //var dt = (DataTable)dg.DataSource;
            
            if (dt.Rows[rowNum].Field<bool>("dopis"))
            {
                // раскарасим допись в оранжевый
                g.FillRectangle(new SolidBrush(Color.Orange), rc);
            }
            else
            {
                var val_doc = Convert.ToDouble(dt.Rows[rowNum].Field<string>("good_count_doc"));
                var val_fact = Convert.ToDouble(dt.Rows[rowNum].Field<string>("good_count_fact"));

                if (val_doc == val_fact)
                {
                    g.FillRectangle(new SolidBrush(Color.GreenYellow), rc);
                }

                if (val_doc != val_fact && val_fact > 0)
                {
                    g.FillRectangle(new SolidBrush(Color.MistyRose), rc);
                }

                if (val_fact == 0)
                {
                    g.FillRectangle(backBrush, rc);
                }
            }
                       

            RectangleF textBounds = new RectangleF(bounds.X + 4, bounds.Y, bounds.Width - 2, bounds.Height);

            // Set text bounds.
            var cellData = Convert.ToString(this.PropertyDescriptor.GetValue(source.List[rowNum])); // Get data for this cell from data source.
            var f = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            g.DrawString(cellData, dgFont, foreBrush, textBounds, f);
        }
    }
}
