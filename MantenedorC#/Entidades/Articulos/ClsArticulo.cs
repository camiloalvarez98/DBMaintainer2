using System;
using System.Data;

namespace Entidades.Articulos
{
    public class ClsArticulo
    {

        #region Atributos privados

        private int _codigo, _valor, _stockMinimo, _codigo_Bodega;
        private string _descripcion,_nombre_Bodega;
        private DateTime _fecha_ingreso;
        //atributos de manejo de la base de datos
        private string _mensajeError, _valorScalar;
        private DataTable _dtResultados;

        #endregion

        #region Atributos públicos


        public int Codigo { get => _codigo; set => _codigo = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }
        public DateTime Fecha_Ingreso { get => _fecha_ingreso; set => _fecha_ingreso = value; }
        public int Valor { get => _valor; set => _valor = value; }
        public int StockMinimo { get => _stockMinimo; set => _stockMinimo = value; }
        public int Codigo_Bodega { get => _codigo_Bodega; set => _codigo_Bodega = value; }
        public string MensajeError { get => _mensajeError; set => _mensajeError = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public DataTable DtResultados { get => _dtResultados; set => _dtResultados = value; }
        public string Nombre_Bodega { get => _nombre_Bodega; set => _nombre_Bodega = value; }



        #endregion
    }
}
