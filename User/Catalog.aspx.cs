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

   
    public partial class Catalog : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getCategories();
                getInstruments();
            }
        }


        private void getInstruments()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Instrument_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "ACTIVEINS");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rInstruments.DataSource = dt;
            rInstruments.DataBind();
        }
        private void getCategories()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "ACTIVECAT");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }


        public string LowerCase(object obj)
        {
            return obj.ToString().ToLower();
        }

        protected void rInstruments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["userId"] != null)
            {
                bool IsCartItemUpdated=false;
                int i = isItemExistInCart(Convert.ToInt32(e.CommandArgument));
                if (i == 0)
                {
                    //Add new item in cart
                    con = new SqlConnection(Connection.GetConnectionString());
                    cmd = new SqlCommand("Cart_Crud", con);
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@InstrumentId", e.CommandArgument);
                    cmd.Parameters.AddWithValue("Quantity", 1);
                    cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error- " + ex.Message + " ');<script>");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    //Add existing item into cart
                    Utils utils = new Utils();
                    IsCartItemUpdated=utils.updateCartQuantity(i+1,Convert.ToInt32(e.CommandArgument),Convert.ToInt32(Session["userId"]));
                   


                }lblMsg.Visible= true;
                    lblMsg.Text = "Item added successfully in your cart!";
                    lblMsg.CssClass = "alert alert-success";
                    Response.AddHeader("REFRESH", "1;URL=Cart.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        int isItemExistInCart(int instrumentId)
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "GETBYID");
            cmd.Parameters.AddWithValue("@InstrumentId", instrumentId);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            int quantity = 0;
            if(dt.Rows.Count > 0)
            {
                quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);

            }
            return quantity;
        }

        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "getProd")
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Instrument_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "SELECTBYCATEGORY");
                cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(e.CommandArgument));
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                rCategory.DataSource = dt;
                rCategory.DataBind();
            }
        }
    }
}