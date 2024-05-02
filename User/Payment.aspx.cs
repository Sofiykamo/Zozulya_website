using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Shop.User
{
    public partial class Payment : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr, dr1;
        DataTable dt;
        SqlTransaction transaction = null;
        string _name = string.Empty;
        string _cardNo=string.Empty;
        string _expiryDate=string.Empty;
        string _cvv=string.Empty;
        string _address=string.Empty;
        string _paymentMode=string.Empty; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                
            }
        }

        protected void lbCardSubmit_Click(object sender, EventArgs e)
        {
            _name=txtName.Text.Trim();
            _cardNo=txtCardNo.Text.Trim();
            _cardNo = string.Format("************{0}", txtCardNo.Text.Trim().Substring(12, 4));
            _expiryDate=txtExpMonth.Text.Trim()+"/"+txtExpYear.Text.Trim();
            _cvv=txtCvv.Text.Trim();
            _address=txtAddress.Text.Trim();
            _paymentMode = "card";
            if(Session["userId"] != null)
            {
                OrderPayment(_name, _cardNo, _expiryDate, _cvv, _address, _paymentMode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }

        //protected void lbCodSubmit_Click(object sender, EventArgs e)
        //{
        //    _address = txtCODAddress.Text.Trim();
        //    _paymentMode = "cod";
        //    if (Session["userId"] != null)
        //    {
        //        OrderPayment(_name, _cardNo, _expiryDate, _cvv, _address, _paymentMode);
        //    }
        //    else
        //    {
        //        Response.Redirect("Login.aspx");
        //    }
        //}
        void OrderPayment(string name, string cardNo, string expiryDate, string cvv, string address, string paymentMode)
        {
            int paymentId;
            int instrumentId;
            int quantity;
            dt=new DataTable();
            dt.Columns.AddRange(new DataColumn[7] {
                new DataColumn("OrderNo", typeof(string)),
                new DataColumn("InstrumentId",typeof(int)),
                new DataColumn("Quantity",typeof (int)),
                new DataColumn("UserId", typeof(int)),
                new DataColumn ("Status",typeof(string)),
                new DataColumn("PaymentId",typeof(int)),
                new DataColumn("OrderDate", typeof(DateTime)),

            });
            con = new SqlConnection(Connection.GetConnectionString());
            con.Open();
            #region Sql Transaction
            transaction = con.BeginTransaction();
            cmd = new SqlCommand("Save_Payment", con, transaction);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@CardNo", cardNo);
            cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);
            cmd.Parameters.AddWithValue("@Cvv", cvv);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
            cmd.Parameters.Add("@InsertedId", SqlDbType.Int);
            cmd.Parameters["@InsertedId"].Direction= ParameterDirection.Output;
            try
            {
                cmd.ExecuteNonQuery();
                paymentId=Convert.ToInt32(cmd.Parameters["@InsertedId"].Value);

                #region Getting Cart Item's

              
                cmd = new SqlCommand("Cart_Crud", con,transaction);
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                dr=cmd.ExecuteReader();
                while (dr.Read())
                {
                    instrumentId = (int)dr["InstrumentId"];
                    quantity = (int)dr["Quantity"];
                    //Update user quantity
                    UpdateQuantity(instrumentId, quantity, transaction,con);
                    //Update user quantity
                    //Delete cart item
                    DeleteCartItem(instrumentId, transaction, con);
                    //Delete cart item
                    dt.Rows.Add(Utils.GetUniqueId(),instrumentId,quantity,(int)Session["userId"],"Pending", paymentId, 
                        Convert.ToDateTime(DateTime.Now));
                }
                dr.Close();
                #endregion Getting Cart Item's
                #region Order Details
                if (dt.Rows.Count > 0)
                {

                    cmd = new SqlCommand("Save_Orders", con, transaction);
                    cmd.Parameters.AddWithValue("@tblOrders", dt);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                #endregion Order Details

                transaction.Commit();

                // Встановити видимість та текс повідомлення про успіх
                lblMsg.Visible = true;
                lblMsg.Text = "Your item ordered successfully!";
                lblMsg.CssClass = "alert alert-success";

                // Викликати DataBind(), якщо це необхідно
                // Page.DataBind();

                // Здійснити перехід на сторінку Invoice.aspx з передачею параметру id
                // Здійснити перехід на сторінку Invoice.aspx з передачею параметру id
                Response.Redirect("Invoice.aspx");

            }
            catch (Exception e)
            {
                try{
                    transaction.Rollback(); }
                catch(Exception ex) {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }


            }
            #endregion Sql Transaction
              finally
            {
                con.Close();
            }


        }



        void UpdateQuantity(int _instrumentId, int _quantity, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            int dbQuantity;
            cmd = new SqlCommand("Instrument_Crud", sqlConnection, sqlTransaction);
            cmd.Parameters.AddWithValue("@Action", "GETBYID");
            cmd.Parameters.AddWithValue("@InstrumentId", _instrumentId);
            cmd.CommandType=CommandType.StoredProcedure;
            try
            {
                dr1=cmd.ExecuteReader();
                while (dr1.Read())
                {
                    dbQuantity = (int)dr1["Quantity"];

                    if(dbQuantity > _quantity && dbQuantity > 2)
                    {
                        dbQuantity = dbQuantity - _quantity;
                        cmd = new SqlCommand("Instrument_Crud", sqlConnection, sqlTransaction);
                        cmd.Parameters.AddWithValue("@Action", "QTYUPDATE");
                        cmd.Parameters.AddWithValue("@Quantity", dbQuantity);
                        cmd.Parameters.AddWithValue("@InstrumentId", _instrumentId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                    }
                }
                dr1.Close();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('"+ex.Message+"');</script>");
            }
          
        }

        void DeleteCartItem(int _instrumentId, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            cmd = new SqlCommand("Cart_Crud", sqlConnection, sqlTransaction);
            cmd.Parameters.AddWithValue("@Action", "DELETE");
            cmd.Parameters.AddWithValue("@InstrumentId", _instrumentId);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}