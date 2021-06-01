using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace ChallengeRecursiva
{
    public partial class Default : System.Web.UI.Page
    {
        protected static DataTable DataSourceCompleto { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            CSVTable.DataSource = "";
            CSVTable.DataBind();
            JovenesUniversitariosCasados.Visible = false;
            NombresRiver.Visible = false;
            EdadesEquipos.Visible = false;
        }

        protected void Upload(object sender, EventArgs e)
        {
            if (FileUploadCSV.PostedFile != null && FileUploadCSV.PostedFile.FileName.Length > 0)
            {
                if (FileUploadCSV.FileName.EndsWith(".csv"))
                {
                    string path = Server.MapPath("~/" + Path.GetFileName(FileUploadCSV.FileName));
                    DataSourceCompleto = ConvertCSVtoDataTable(path);
                    FileUploadCSV.SaveAs(path);

                    int HinchasRacing = 0;
                    EnumerableRowCollection<DataRow> EdadesRacing;
                    Int64  EdadAcumuladaRacing = 0;

                    if (DataSourceCompleto != null)
                    {
                        HinchasRacing = DataSourceCompleto.AsEnumerable().Count(x => x.Field<String>("Equipo") == "Racing");
                        EdadesRacing = DataSourceCompleto.AsEnumerable().Where(y => y.Field<String>("Equipo") == "Racing");

                        foreach (var item in EdadesRacing.ToList())
                        {
                            EdadAcumuladaRacing += long.Parse(item.ItemArray[1].ToString());
                        }
                    }

                    if (EdadAcumuladaRacing != 0)
                    { CantidadTotal.Text = "Cantidad total de personas registradas: " + DataSourceCompleto.Rows.Count; }
                    else
                    { CantidadTotal.Text = ""; }

                    if (HinchasRacing != 0)
                    { PromedioRacing.Text = "Promedio de edad en Racing: " + (EdadAcumuladaRacing / HinchasRacing); }
                    else
                    { PromedioRacing.Text = ""; }

                    lblMensaje.Text = "\"" + FileUploadCSV.FileName + "\" cargado de forma exitosa.";
                    JovenesUniversitariosCasados.Visible = true;
                    NombresRiver.Visible = true;
                    EdadesEquipos.Visible = true;
                }
                else
                { lblMensaje.Text = "Tipo de Archivo Incorrecto, vuelva a intentarlo con un .CSV"; }
            }

            
        }

        protected static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath, Encoding.Default, true))
            {
                string[] headers = "Nombre;Edad;Equipo;Estado Civil;Nivel de Estudios".Split(';');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(';');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        protected void JovenesUniversitariosCasados_Click(object sender, EventArgs e)
        {
            if (DataSourceCompleto != null)
            {
                DataTable dt = DataSourceCompleto;

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr[3].ToString() == "Casado" && dr[4].ToString() == "Universitario") { }
                    else
                    { dr.Delete(); }
                }

                dt.AcceptChanges();
                dt.DefaultView.Sort = "Edad ASC";
                dt = dt.DefaultView.ToTable();
                dt.Columns.Remove("Estado Civil");
                dt.Columns.Remove("Nivel de Estudios");
                dt = dt.AsEnumerable().Take(100).CopyToDataTable();
                CSVTable.DataSource = dt;
                CSVTable.DataBind();   
            }
        }

        protected void NombresComunesRiver_Click(object sender, EventArgs e)
        {
            if (DataSourceCompleto != null)
            {
                DataTable dt = DataSourceCompleto;

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr[2].ToString() != "River") 
                    { 
                        dr.Delete(); 
                    }
                }

                dt.AcceptChanges();
                dt.Columns.Remove("Edad");
                dt.Columns.Remove("Equipo");
                dt.Columns.Remove("Estado Civil");
                dt.Columns.Remove("Nivel de Estudios");
                
                CSVTable.DataSource = dt.AsEnumerable().GroupBy(r => new { Nombre = r.Field<string>("Nombre") }).Select(grp => new { Nombre = grp.Key.Nombre, Cantidad = grp.Count() });
                CSVTable.DataBind();
            }
        }

        protected void EdadesPorEquipos_Click(object sender, EventArgs e)
        {
            if (DataSourceCompleto != null)
            {
                DataTable dt = 
                    DataSourceCompleto.AsEnumerable()
                    .GroupBy(r => new { Equipo = r["Equipo"] })
                    .OrderByDescending(id => id.Count())
                    .Select(d => 
                        DataSourceCompleto.Rows.Add
                        (
                            d.Key.Equipo, 
                            (int)d.Average(r => int.Parse(r.Field<string>("Edad"))), 
                            d.Min(r => long.Parse(r.Field<string>("Edad"))), 
                            d.Max(r => long.Parse(r.Field<string>("Edad"))))
                        )
                    .CopyToDataTable();

                dt.Columns["Equipo"].ColumnName = "Edad Minima";
                dt.Columns["Nombre"].ColumnName = "Equipo";
                dt.Columns["Edad"].ColumnName = "Edad Promedio";
                dt.Columns["Estado Civil"].ColumnName = "Edad Maxima";

                dt.Columns.Remove("Nivel de Estudios");
                CSVTable.DataSource = dt;
                CSVTable.DataBind();
            }
        }
    }
}