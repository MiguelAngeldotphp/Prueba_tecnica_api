using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace prueba_tecnica_api.Models
{
    public class db
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-3GQ631GT\\SQLEXPRESS;Initial Catalog=PRUEBA_TECNICA;Integrated Security=True;MultipleActiveResultSets=true;");

        #region Compra
        public bool CompraPost(Compra comp)
        {
            con.Open();
            SqlTransaction transaction;
            transaction = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_COMPRA", con);
                cmd.Transaction = transaction;

                //añadir parámetros
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMP_ID_COMPRA", comp.comp_id_compra);
                cmd.Parameters.AddWithValue("@COMP_MONTO_TOTAL", comp.comp_monto_total);
                cmd.Parameters.AddWithValue("@COMP_PROVEEDOR", comp.comp_proveedor);
                cmd.Parameters.AddWithValue("@COMP_USUARIO", comp.comp_usuario);
                cmd.Parameters.AddWithValue("@TYPE", comp.type);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    return false;
                }
                else
                {
                    string cadena = "SELECT IDENT_CURRENT('COMPRA') as ID_COMPRA";
                    SqlCommand cmdCadena = new SqlCommand(cadena, con);
                    cmdCadena.Transaction = transaction;
                    int compraId = 0;
                    SqlDataReader compraIngresada = cmdCadena.ExecuteReader();

                    if (compraIngresada.Read())
                    {
                        compraId = Convert.ToInt32(compraIngresada["ID_COMPRA"]);
                        Debug.WriteLine("id compra: " + compraId);

                        compraIngresada.Close();

                        DetalleCompra[] detalles = comp.detalles;

                        for (int i = 0; i < detalles?.Length; i++)
                        {
                            SqlCommand cmdDetalles = new SqlCommand("SP_DETALLE_COMPRA", con);
                            cmdDetalles.Transaction = transaction;
                            //añadir parámetros
                            cmdDetalles.CommandType = CommandType.StoredProcedure;
                            cmdDetalles.Parameters.AddWithValue("@DETC_NOMBRE_PRODUCTO", detalles[i].detc_nombre_producto);
                            cmdDetalles.Parameters.AddWithValue("@DETC_PRECIO_PRODUCTO", detalles[i].detc_precio_producto);
                            cmdDetalles.Parameters.AddWithValue("@DETC_CANTIDAD_PRODUCTO", detalles[i].detc_cantidad_producto);
                            cmdDetalles.Parameters.AddWithValue("@DETC_IMPORTE", detalles[i].detc_importe);
                            cmdDetalles.Parameters.AddWithValue("@DETC_COMP_ID_COMPRA", compraId);
                            cmdDetalles.Parameters.AddWithValue("@TYPE", comp.type);

                            if (cmdDetalles.ExecuteNonQuery() == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                con.Close();
                return false;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public DataSet CompraGet(Compra comp, out string msg)
        {
            msg = string.Empty;

            DataSet ds = new DataSet();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_COMPRA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMP_ID_COMPRA", 0);
                cmd.Parameters.AddWithValue("@COMP_MONTO_TOTAL", 0);
                cmd.Parameters.AddWithValue("@COMP_PROVEEDOR", "");
                cmd.Parameters.AddWithValue("@COMP_USUARIO", "");
                cmd.Parameters.AddWithValue("@TYPE", comp.type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                msg = "Exito";
                con.Close();

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ds;
        }
        #endregion

        #region Venta

        public bool VentaPost(Venta vent)
        {
            con.Open();
            SqlTransaction transaction;
            transaction = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_VENTA", con);
                cmd.Transaction = transaction;

                //añadir parámetros
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VEN_ID_VENTA", vent.ven_id_venta);
                cmd.Parameters.AddWithValue("@VEN_MONTO_TOTAL", vent.ven_monto_total);
                cmd.Parameters.AddWithValue("@VEN_CLIENTE", vent.ven_cliente);
                cmd.Parameters.AddWithValue("@VEN_USUARIO", vent.ven_usuario);
                cmd.Parameters.AddWithValue("@TYPE", vent.type);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    return false;
                }
                else
                {
                    string cadena = "SELECT IDENT_CURRENT('VENTA') as ID_VENTA";
                    SqlCommand cmdCadena = new SqlCommand(cadena, con);
                    cmdCadena.Transaction = transaction;
                    int ventaId = 0;
                    SqlDataReader ventaIngresada = cmdCadena.ExecuteReader();

                    if (ventaIngresada.Read())
                    {
                        ventaId = Convert.ToInt32(ventaIngresada["ID_VENTA"]);

                        ventaIngresada.Close();

                        DetalleVenta[] detalles = vent.detalles;

                        for (int i = 0; i < detalles?.Length; i++)
                        {
                            SqlCommand cmdDetalles = new SqlCommand("SP_DETALLE_VENTA", con);
                            cmdDetalles.Transaction = transaction;
                            //añadir parámetros
                            cmdDetalles.CommandType = CommandType.StoredProcedure;
                            cmdDetalles.Parameters.AddWithValue("@DETV_NOMBRE_PRODUCTO", detalles[i].detv_nombre_producto);
                            cmdDetalles.Parameters.AddWithValue("@DETV_PRECIO_PRODUCTO", detalles[i].detv_precio_producto);
                            cmdDetalles.Parameters.AddWithValue("@DETV_CANTIDAD_PRODUCTO", detalles[i].detv_cantidad_producto);
                            cmdDetalles.Parameters.AddWithValue("@DETV_IMPORTE", detalles[i].detv_importe);
                            cmdDetalles.Parameters.AddWithValue("@DETV_VEN_ID_VENTA", ventaId);
                            cmdDetalles.Parameters.AddWithValue("@TYPE", vent.type);

                            if (cmdDetalles.ExecuteNonQuery() == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                con.Close();
                return false;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public DataSet VentaGet(Venta vent, out string msg)
        {
            msg = string.Empty;

            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_VENTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VEN_ID_VENTA", 0);
                cmd.Parameters.AddWithValue("@VEN_MONTO_TOTAL",0);
                cmd.Parameters.AddWithValue("@VEN_CLIENTE", "");
                cmd.Parameters.AddWithValue("@VEN_USUARIO", "");
                cmd.Parameters.AddWithValue("@TYPE", vent.type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                msg = "Exito";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ds;
        }

        #endregion

        #region Detalle de venta

        public DataSet DetalleVentaGetById(Venta vent, out string msg)
        {
            msg = string.Empty;

            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_DETALLE_VENTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DETV_NOMBRE_PRODUCTO", "");
                cmd.Parameters.AddWithValue("@DETV_PRECIO_PRODUCTO", 0);
                cmd.Parameters.AddWithValue("@DETV_CANTIDAD_PRODUCTO", 0);
                cmd.Parameters.AddWithValue("@DETV_IMPORTE",0);
                cmd.Parameters.AddWithValue("@DETV_VEN_ID_VENTA", vent.ven_id_venta);
                cmd.Parameters.AddWithValue("@TYPE", vent.type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                msg = "Exito";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ds;
        }


        #endregion

        #region Detalle de compra

        public DataSet DetalleCompraGetById(Compra comp, out string msg)
        {
            msg = string.Empty;

            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_DETALLE_COMPRA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DETC_NOMBRE_PRODUCTO", "");
                cmd.Parameters.AddWithValue("@DETC_PRECIO_PRODUCTO", 0);
                cmd.Parameters.AddWithValue("@DETC_CANTIDAD_PRODUCTO", 0);
                cmd.Parameters.AddWithValue("@DETC_IMPORTE", 0);
                cmd.Parameters.AddWithValue("@DETC_COMP_ID_COMPRA", comp.comp_id_compra);
                cmd.Parameters.AddWithValue("@TYPE", comp.type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                msg = "Exito";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ds;
        }

        #endregion
    }

}
