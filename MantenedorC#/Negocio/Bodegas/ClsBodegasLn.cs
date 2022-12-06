using Datos.BDD;
using Entidades.Bodegas;
using System;
using System.Data;

namespace Negocio.Bodegas
{
    public class ClsBodegasLn
    {
        #region Variables privadas

        private ClsBDD ObjBDD = null;

        #endregion

        #region Métodos públicos

        public void ValidarBodega(ref ClsBodega ObjBodega)
        {
            ObjBDD = new ClsBDD()
            {
                NombreTabla = "Bodegas",
                NombrePA = "dbo.SP_Bodega_ValidarBodega",
                Scalar = true
            };
            ObjBDD.DtParametros.Rows.Add(@"@Codigo_Bodega", "4", ObjBodega.Codigo_Bodega);
            Ejecutar(ref ObjBodega);
        }

        #endregion

        #region Métodos privados

        private void Ejecutar(ref ClsBodega ObjBodega)
        {
            ObjBDD.CRUD(ref ObjBDD);

            if (ObjBDD.MensajeErrorBDD == null)
            {
                if (ObjBDD.Scalar)
                {
                    ObjBodega.ValorScalar = ObjBDD.ValorScalar;
                }
                else
                {
                    ObjBodega.DtResultados = ObjBDD.DsResultados.Tables[0];
                    if (ObjBodega.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjBDD.DtParametros.Rows)
                        {
                            ObjBodega.Codigo_Bodega = Convert.ToInt32(item["Codigo_Bodega"].ToString());
                        }

                    }
                }
            }
            else
            {
                ObjBodega.MensajeError = ObjBDD.MensajeErrorBDD;
            }
        }

        #endregion


    }
}
