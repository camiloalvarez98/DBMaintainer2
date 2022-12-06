using Datos.BDD;
using Entidades.Articulos;
using System;
using System.Data;

namespace Negocio.Articulos
{
    public class ClsArticuloLn
    {

        #region Variables privadas

        private ClsBDD ObjBDD = null;

        #endregion

        #region Metodo Index

        public void Index(ref ClsArticulo ObjArticulo)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Articulos",
                NombrePA = "dbo.SP_Articulo_Index",
                Scalar = false
            };
            Ejecutar(ref ObjArticulo);
        }

        #endregion

        #region CRUD Usuario

        public void Create(ref ClsArticulo ObjArticulo)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Articulos",
                NombrePA = "dbo.SP_Articulo_Create",
                Scalar = true
            };
            
            ObjBDD.DtParametros.Rows.Add(@"@Descripcion", "17", ObjArticulo.Descripcion);
            ObjBDD.DtParametros.Rows.Add(@"@Fecha_Ingreso", "14", ObjArticulo.Fecha_Ingreso);
            ObjBDD.DtParametros.Rows.Add(@"@Valor", "4", ObjArticulo.Valor);
            ObjBDD.DtParametros.Rows.Add(@"@StockMinimo", "4", ObjArticulo.StockMinimo);
            ObjBDD.DtParametros.Rows.Add(@"@Codigo_Bodega", "4", ObjArticulo.Codigo_Bodega);

            Ejecutar(ref ObjArticulo);
        }

        public void Read(ref ClsArticulo ObjArticulo)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Articulos",
                NombrePA = "dbo.SP_Articulo_Read",
                Scalar = false
            };
            ObjBDD.DtParametros.Rows.Add(@"@Codigo", "4", ObjArticulo.Codigo);
            Ejecutar(ref ObjArticulo);
        }

        public void Update(ref ClsArticulo ObjArticulo)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Articulos",
                NombrePA = "dbo.SP_Articulo_Update",
                Scalar = true
            };
            ObjBDD.DtParametros.Rows.Add(@"@Codigo", "4", ObjArticulo.Codigo);
            ObjBDD.DtParametros.Rows.Add(@"@Descripcion", "17", ObjArticulo.Descripcion);
            ObjBDD.DtParametros.Rows.Add(@"@Fecha_Ingreso", "14", ObjArticulo.Fecha_Ingreso);
            ObjBDD.DtParametros.Rows.Add(@"@Valor", "4", ObjArticulo.Valor);
            ObjBDD.DtParametros.Rows.Add(@"@StockMinimo", "4", ObjArticulo.StockMinimo);
            ObjBDD.DtParametros.Rows.Add(@"@Codigo_Bodega", "4", ObjArticulo.Codigo_Bodega);

            Ejecutar(ref ObjArticulo);
        }

        public void Delete(ref ClsArticulo ObjArticulo)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Articulos",
                NombrePA = "dbo.SP_Articulo_Delete",
                Scalar = true
            };
            ObjBDD.DtParametros.Rows.Add(@"@Codigo", "4", ObjArticulo.Codigo);
            Ejecutar(ref ObjArticulo);
        }

        public void Consultar(ref ClsArticulo ObjArticulo)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Articulos",
                NombrePA = "dbo.SP_Articulo_Consultar",
                Scalar = false
            };
            ObjBDD.DtParametros.Rows.Add(@"@Codigo", "4", ObjArticulo.Codigo);
            Ejecutar2(ref ObjArticulo);

        }


        #endregion

        #region Metodos privados

        private void Ejecutar(ref ClsArticulo ObjArticulo)
        {
            ObjBDD.CRUD(ref ObjBDD);

            if (ObjBDD.MensajeErrorBDD == null)
            {
                if (ObjBDD.Scalar)
                {
                    ObjArticulo.ValorScalar = ObjBDD.ValorScalar;
                }
                else
                {
                    ObjArticulo.DtResultados = ObjBDD.DsResultados.Tables[0];
                    if (ObjArticulo.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjArticulo.DtResultados.Rows)
                        {
                            ObjArticulo.Codigo = Convert.ToInt32(item["Codigo"].ToString());
                            ObjArticulo.Descripcion = item["Descripcion"].ToString();
                            ObjArticulo.Fecha_Ingreso = Convert.ToDateTime(item["Fecha_Ingreso"].ToString());
                            ObjArticulo.Valor = Convert.ToInt32(item["Valor"].ToString());
                            ObjArticulo.StockMinimo = Convert.ToInt32(item["StockMinimo"].ToString());
                            ObjArticulo.Codigo_Bodega = Convert.ToInt32(item["Codigo_Bodega"].ToString());
                        }

                    }
                }
            }
            else
            {
                ObjArticulo.MensajeError = ObjBDD.MensajeErrorBDD;
            }
        }

        private void Ejecutar2(ref ClsArticulo ObjArticulo)
        {
            ObjBDD.CRUD(ref ObjBDD);

            if (ObjBDD.MensajeErrorBDD == null)
            {
                if (ObjBDD.Scalar)
                {
                    ObjArticulo.ValorScalar = ObjBDD.ValorScalar;
                }
                else
                {
                    ObjArticulo.DtResultados = ObjBDD.DsResultados.Tables[0];
                    if (ObjArticulo.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjArticulo.DtResultados.Rows)
                        {
                            ObjArticulo.Descripcion = item["nombre_Articulo"].ToString();
                            ObjArticulo.Nombre_Bodega = item["nombre_Bodega"].ToString();
                        }

                    }
                }
            }
            else
            {
                ObjArticulo.MensajeError = ObjBDD.MensajeErrorBDD;
            }
        }

        #endregion
    }
}
