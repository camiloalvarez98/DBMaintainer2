using System.Data;

namespace Entidades.Bodegas
{
    public class ClsBodega
    {
        #region Atributos privados
        private int _codigo_Bodega;
        private string _descripcion;
        //atributos de manejo de la base de datos
        private string _mensajeError, _valorScalar;
        private DataTable _dtResultados;

        #endregion

        #region Atributos públicas
        public int Codigo_Bodega { get => _codigo_Bodega; set => _codigo_Bodega = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }
        public string MensajeError { get => _mensajeError; set => _mensajeError = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public DataTable DtResultados { get => _dtResultados; set => _dtResultados = value; }
        #endregion

    }
}
