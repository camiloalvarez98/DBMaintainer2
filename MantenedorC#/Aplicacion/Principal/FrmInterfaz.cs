using Entidades.Articulos;
using Entidades.Bodegas;
using Negocio.Articulos;
using Negocio.Bodegas;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aplicacion.Principal
{
    public partial class FrmInterfaz : Form
    {

        #region Variables privadas

        private ClsArticulo ObjArticulo = null;
        private ClsBodega ObjBodega = null;
        private readonly ClsArticuloLn ObjArticuloLn = new ClsArticuloLn();
        private readonly ClsBodegasLn ObjBodegaLn = new ClsBodegasLn();

        #endregion

        #region Inicialización formulario
        public FrmInterfaz()
        {
            InitializeComponent();
            CargarListaArticulos();
            label11.Visible = false;
            button3.Visible = false;
        }

        #endregion

        #region Métodos de DataGrid

        private void CargarListaArticulos()
        {
            ObjArticulo = new ClsArticulo();
            ObjArticuloLn.Index(ref ObjArticulo);
            if (ObjArticulo.MensajeError == null)
            {
                DgvArticulos.DataSource = ObjArticulo.DtResultados;
                DataGridViewCellStyle estilo = DgvArticulos.ColumnHeadersDefaultCellStyle;
                estilo.Alignment = DataGridViewContentAlignment.MiddleCenter;
                estilo.Font = new Font(DgvArticulos.Font, FontStyle.Bold);
                DataGridViewCellStyle estilo2 = DgvArticulos.RowsDefaultCellStyle;
                estilo2.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvArticulos.AllowUserToAddRows = false;
                DgvArticulos.AllowUserToDeleteRows = false;
                DgvArticulos.ReadOnly = true;
            }
            else
            {
                MessageBox.Show(ObjArticulo.MensajeError, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DgvArticulos.Columns[e.ColumnIndex].Name == "Editar")
                {
                    button3.Visible = true;
                    Cargar_Datos_Update(e);
                }

                if (DgvArticulos.Columns[e.ColumnIndex].Name == "Eliminar")
                {
                    Delete(e);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Eventos de botones
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Rellene los campos de artículo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (int.TryParse(textBox3.Text.Trim(), out int value) && int.TryParse(textBox2.Text.Trim(), out int value2) && int.TryParse(textBox5.Text.Trim(), out int value3))
                {
                    if (Validar_Bodega())
                    {
                        DialogResult dialogResult = MessageBox.Show("Está a punto de crear el artículo " + textBox1.Text + "\n¿Desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (CreateM())
                            {
                                MessageBox.Show("El Articulo " + ObjArticulo.Descripcion + " fue ingresado correctamente con el ID: " + ObjArticulo.ValorScalar, "Ingreso exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CargarListaArticulos();
                                textBox1.Clear();
                                textBox2.Clear();
                                textBox3.Clear();
                                textBox5.Clear();
                            }
                            else
                            {
                                MessageBox.Show(ObjArticulo.MensajeError, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("El código de bodega ingresado no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Los campos Valor, Stock mínimo y Código deben ser valores numéricos (enteros)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("No se ha ingresado un código de artículo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (int.TryParse(textBox4.Text.Trim(), out int value))
                {
                    ObjArticulo = new ClsArticulo()
                    {
                        Codigo = Convert.ToInt32(textBox4.Text)
                    };
                    ObjArticuloLn.Consultar(ref ObjArticulo);
                    if (ObjArticulo.Descripcion == null)
                    {
                        MessageBox.Show("Articulo (Código: " + textBox4.Text + ") no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox4.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Articulo " + ObjArticulo.Descripcion + " (Código: " + textBox4.Text + ")\nNombre bodega: " + ObjArticulo.Nombre_Bodega, "Consulta exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox4.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("El campo 'Código artículo' debe ser un valor numérico (entero)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Rellene los campos de artículo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (int.TryParse(textBox3.Text.Trim(), out int value) && int.TryParse(textBox2.Text.Trim(), out int value2) && int.TryParse(textBox5.Text.Trim(), out int value3))
                {
                    if (Validar_Bodega())
                    {
                        DialogResult dialogResult = MessageBox.Show("Está a punto de actualizar un artículo\n¿Desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (UpdateM())
                            {
                                MessageBox.Show("El Artículo (Código: " + label11.Text + ") fue actualizado correctamente", "Actualización exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                textBox1.Clear();
                                textBox2.Clear();
                                textBox3.Clear();
                                textBox5.Clear();
                                CargarListaArticulos();
                                button3.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show(ObjArticulo.MensajeError, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("El código de bodega ingresado no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Los campos Valor, Stock mínimo y Código deben ser valores numéricos (enteros)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Acciones

        private bool CreateM()
        {
            ObjArticulo = new ClsArticulo()
            {
                Descripcion = textBox1.Text,
                Fecha_Ingreso = dateTimePicker1.Value,
                Valor = Convert.ToInt32(textBox2.Text),
                StockMinimo = Convert.ToInt32(textBox3.Text),
                Codigo_Bodega = Convert.ToInt32(textBox5.Text)
            };
            ObjArticuloLn.Create(ref ObjArticulo);
            if (ObjArticulo.MensajeError == null)
            {
                return true;
            }
            return false;
        }

        private bool UpdateM()
        {
            ObjArticulo = new ClsArticulo()
            {
                Codigo = Convert.ToInt32(label11.Text),
                Descripcion = textBox1.Text,
                Fecha_Ingreso = dateTimePicker1.Value,
                Valor = Convert.ToInt32(textBox2.Text),
                StockMinimo = Convert.ToInt32(textBox3.Text),
                Codigo_Bodega = Convert.ToInt32(textBox5.Text)
            };

            ObjArticuloLn.Update(ref ObjArticulo);
            if (ObjArticulo.MensajeError == null)
            {
                return true;
            }
            return false;
        }

        private void Cargar_Datos_Update(DataGridViewCellEventArgs e)
        {
            ObjArticulo = new ClsArticulo()
            {
                Codigo = Convert.ToInt32(DgvArticulos.Rows[e.RowIndex].Cells["Codigo"].Value.ToString())
            };

            label11.Text = ObjArticulo.Codigo.ToString();

            ObjArticuloLn.Read(ref ObjArticulo);

            textBox1.Text = ObjArticulo.Descripcion;
            textBox2.Text = ObjArticulo.Valor.ToString();
            textBox3.Text = ObjArticulo.StockMinimo.ToString();
            dateTimePicker1.Value = ObjArticulo.Fecha_Ingreso;
            textBox5.Text = ObjArticulo.Codigo_Bodega.ToString();

        }

        private void Delete(DataGridViewCellEventArgs e)
        {
            ObjArticulo = new ClsArticulo()
            {
                Codigo = Convert.ToInt32(DgvArticulos.Rows[e.RowIndex].Cells["Codigo"].Value.ToString())
            };

            DialogResult dialogResult = MessageBox.Show("¿Estás seguro de eliminar el artículo " + ObjArticulo.Descripcion + " (Código: " + ObjArticulo.Codigo.ToString() + ")?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                ObjArticuloLn.Delete(ref ObjArticulo);
                if (ObjArticulo.MensajeError == null)
                {
                    MessageBox.Show("El Artículo (Código " + ObjArticulo.Codigo + ") fue eliminado correctamente","Transacción exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarListaArticulos();
                }
                else
                {
                    MessageBox.Show(ObjArticulo.MensajeError, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                CargarListaArticulos();
            }
        }

        #endregion
        
        #region Métodos de validación 

        private bool Validar_Bodega()
        {
            ObjBodega = new ClsBodega()
            {
                Codigo_Bodega = Convert.ToInt32(textBox5.Text)
            };
            ObjBodegaLn.ValidarBodega(ref ObjBodega);
            if (ObjBodega.ValorScalar != null)
            {
                return true;

            }
            return false;
        }

        #endregion
    }
}
