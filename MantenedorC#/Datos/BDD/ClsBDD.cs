using System;
using System.Data;
using System.Data.SqlClient;

namespace Datos.BDD
{
    public class ClsBDD
    {

        #region Variables privadas

        private SqlConnection _objSqlConnection;
        private SqlDataAdapter _objSqlDataAdapter;
        private SqlCommand _objSqlCommand;
        private DataSet _dsResultados;
        private DataTable _dtParametros;
        private string _nombreTabla, _nombrePA, _mensajeErrorBDD, _valorScalar, _nombreBDD;
        private bool _scalar;

        #endregion

        #region Variables públicas

        public SqlConnection ObjSqlConnection { get => _objSqlConnection; set => _objSqlConnection = value; }
        public SqlDataAdapter ObjSqlDataAdapter { get => _objSqlDataAdapter; set => _objSqlDataAdapter = value; }
        public SqlCommand ObjSqlCommand { get => _objSqlCommand; set => _objSqlCommand = value; }
        public DataSet DsResultados { get => _dsResultados; set => _dsResultados = value; }
        public DataTable DtParametros { get => _dtParametros; set => _dtParametros = value; }
        public string NombreTabla { get => _nombreTabla; set => _nombreTabla = value; }
        public string NombrePA { get => _nombrePA; set => _nombrePA = value; }
        public string MensajeErrorBDD { get => _mensajeErrorBDD; set => _mensajeErrorBDD = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public string NombreBDD { get => _nombreBDD; set => _nombreBDD = value; }
        public bool Scalar { get => _scalar; set => _scalar = value; }


        #endregion

        #region Constructores

        public ClsBDD()
        {
            DtParametros = new DataTable("SpParametros");
            DtParametros.Columns.Add("Nombre");
            DtParametros.Columns.Add("TipoDato");
            DtParametros.Columns.Add("Valor");

            NombreBDD = "Bodega";
        }

        #endregion

        #region Métodos privados

        private void CrearConexionBDD(ref ClsBDD ObjBDD)
        {
            switch (ObjBDD.NombreBDD)
            {
                case "Bodega":
                    ObjBDD.ObjSqlConnection = new SqlConnection(Properties.Settings.Default.cadenaConexion_Bodega);
                    break;
                default:
                    break;
            }
        }

        private void ValidarConexionBDD(ref ClsBDD ObjBDD)
        {
            if(ObjBDD.ObjSqlConnection.State == ConnectionState.Closed)
            {
                ObjBDD.ObjSqlConnection.Open();
            }
            else
            {
                ObjBDD.ObjSqlConnection.Close();
                ObjBDD.ObjSqlConnection.Dispose();
            }
        }

        private void AgregarParametros(ref ClsBDD ObjBDD)
        {
            if(ObjBDD.DtParametros != null)
            {
                SqlDbType TipoDatoSQL = new SqlDbType();
                foreach (DataRow item in ObjBDD.DtParametros.Rows)
                {
                    switch (item[1])
                    {
                        case "1":
                            TipoDatoSQL = SqlDbType.Bit;
                            break;
                        case "2":
                            TipoDatoSQL = SqlDbType.TinyInt;
                            break;
                        case "3":
                            TipoDatoSQL = SqlDbType.SmallInt;
                            break;
                        case "4":
                            TipoDatoSQL = SqlDbType.Int;
                            break;
                        case "5":
                            TipoDatoSQL = SqlDbType.BigInt;
                            break;
                        case "6":
                            TipoDatoSQL = SqlDbType.Decimal;
                            break;
                        case "7":
                            TipoDatoSQL = SqlDbType.SmallMoney;
                            break;
                        case "8":
                            TipoDatoSQL = SqlDbType.Money;
                            break;
                        case "9":
                            TipoDatoSQL = SqlDbType.Float;
                            break;
                        case "10":
                            TipoDatoSQL = SqlDbType.Real;
                            break;
                        case "11":
                            TipoDatoSQL = SqlDbType.Date;
                            break;
                        case "12":
                            TipoDatoSQL = SqlDbType.Time;
                            break;
                        case "13":
                            TipoDatoSQL = SqlDbType.SmallDateTime;
                            break;
                        case "14":
                            TipoDatoSQL = SqlDbType.DateTime;
                            break;
                        case "15":
                            TipoDatoSQL = SqlDbType.Char;
                            break;
                        case "16":
                            TipoDatoSQL = SqlDbType.NChar;
                            break;
                        case "17":
                            TipoDatoSQL = SqlDbType.VarChar;
                            break;
                        case "18":
                            TipoDatoSQL = SqlDbType.NVarChar;
                            break;
                        default:
                            break;
                    }

                    if (ObjBDD.Scalar)
                    {
                        if (item[2].ToString().Equals(string.Empty))
                        {
                            ObjBDD.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value;
                        }
                        else
                        {
                            ObjBDD.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString();
                        }
                    }
                    else
                    {
                        if (item[2].ToString().Equals(string.Empty))
                        {
                            ObjBDD.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value;
                        }
                        else
                        {
                            ObjBDD.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString();
                        }
                    }
                }
            }
        }

        private void PrepararConexionBDD(ref ClsBDD ObjBDD)
        {
            CrearConexionBDD(ref ObjBDD);
            ValidarConexionBDD(ref ObjBDD);
        }

        private void EjercutarDataAdapter(ref ClsBDD ObjBDD)
        {
            try
            {
                PrepararConexionBDD(ref ObjBDD);
                ObjBDD.ObjSqlDataAdapter = new SqlDataAdapter(ObjBDD.NombrePA, ObjBDD.ObjSqlConnection);
                ObjBDD.ObjSqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                AgregarParametros(ref ObjBDD);
                ObjBDD.DsResultados = new DataSet();
                ObjBDD.ObjSqlDataAdapter.Fill(ObjBDD.DsResultados, ObjBDD.NombreTabla);
            }
            catch(Exception ex)
            {
                ObjBDD.MensajeErrorBDD = ex.Message.ToString();
            }
            finally
            {
                if(ObjBDD.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBDD(ref ObjBDD);
                }
            }
        }

        private void EjecutarCommand(ref ClsBDD ObjBDD)
        {
            try
            {
                PrepararConexionBDD(ref ObjBDD);
                ObjBDD.ObjSqlCommand = new SqlCommand(ObjBDD.NombrePA, ObjBDD.ObjSqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                AgregarParametros(ref ObjBDD);

                if (ObjBDD.Scalar)
                {
                    ObjBDD.ValorScalar = ObjBDD.ObjSqlCommand.ExecuteScalar().ToString().Trim();
                }
                else
                {
                    ObjBDD.ObjSqlCommand.ExecuteNonQuery();
                }
               
            }
            catch(Exception ex)
            {
                ObjBDD.MensajeErrorBDD = ex.Message.ToString();
            }
            finally
            {
                if(ObjBDD.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBDD(ref ObjBDD);
                }
            }
        }

        #endregion

        #region Métodos públicos

        public void CRUD(ref ClsBDD ObjBDD)
        {
            if (ObjBDD.Scalar)
            {
                EjecutarCommand(ref ObjBDD);
            }
            else
            {
                EjercutarDataAdapter(ref ObjBDD);
            }
        }
        #endregion
    }
}
