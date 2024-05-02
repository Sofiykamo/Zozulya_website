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
    public partial class Cart : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        decimal grandTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    getCartItems();
                }
            }

        }
        void getCartItems()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCartItem.DataSource = dt;
            if (dt.Rows.Count == 0)
            {

                rCartItem.FooterTemplate = null;
                rCartItem.FooterTemplate = new CustomTemplate(ListItemType.Footer);

            }
            rCartItem.DataBind();

            // Оновити відображення цін товарів
            UpdateProductPrices();
        }

        void UpdateProductPrices()
        {
            foreach (RepeaterItem item in rCartItem.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblPrice = item.FindControl("lblPrice") as Label;
                    HiddenField hdnQuantity = item.FindControl("hdnQuantity") as HiddenField;
                    TextBox txtQuantity = item.FindControl("txtQuantity") as TextBox;
                    Label lblTotalPrice = item.FindControl("lblTotalPrice") as Label;

                    // Отримати ціну товару та кількість
                    decimal price = Convert.ToDecimal(lblPrice.Text);
                    int quantity = Convert.ToInt32(txtQuantity.Text);

                    // Розрахувати загальну вартість товару
                    decimal totalPrice = price * quantity;

                    // Оновити відображення загальної вартості товару
                    lblTotalPrice.Text = totalPrice.ToString();
                }
            }
        }
        protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Utils utils = new Utils();
            if (e.CommandName == "remove") {

                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@InstrumentId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    getCartItems();

                    //Cart count
                    Session["cartCount"]=utils.cartCount(Convert.ToInt32(Session["userId"]));

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error - " + ex.Message + " ');<script>");
                }
                finally
                {
                    con.Close();
                }
              
            }

            if (e.CommandName == "updateCart")
            {
                bool isCartUpdated = false;
                for(int item = 0; item < rCartItem.Items.Count; item++)
                {
                    if(rCartItem.Items[item].ItemType==ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox quantity= rCartItem.Items[item].FindControl("txtQuantity") as TextBox;
                        HiddenField _instrumentId = rCartItem.Items[item].FindControl("hdnInstrumentId") as HiddenField;
                        HiddenField _quantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        int quantityFromCart= Convert.ToInt32(quantity.Text);
                        int InstrumentId=Convert.ToInt32(_instrumentId.Value);
                        int quantityFromDB = Convert.ToInt32(_quantity.Value);
                        bool isTrue = false;
                        int updatedQuantity = 1;
                        if(quantityFromCart > quantityFromDB)
                        {
                            updatedQuantity = quantityFromCart;
                            isTrue = true;
                        }
                        else if(quantityFromCart<quantityFromDB)
                        {
                            updatedQuantity = quantityFromCart;
                            isTrue=true;
                        }
                        if (isTrue)
                        {
                            //Update cart item's quantity in db
                            isCartUpdated=utils.updateCartQuantity(updatedQuantity,InstrumentId,Convert.ToInt32(Session["userId"]));
                        }

                    }
                }

                getCartItems();

            }

            if (e.CommandName == "checkout")
            {
                bool isTrue = false;
                string pName = string.Empty;
                //check quality
                for (
                    
                    int item = 0; item < rCartItem.Items.Count; item++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {

                        HiddenField _instrumentId = rCartItem.Items[item].FindControl("hdnInstrumentId") as HiddenField;
                        HiddenField _cartQuantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        HiddenField _instrumentQuantity = rCartItem.Items[item].FindControl("hdnInsQuantity") as HiddenField;
                        Label instrumentName = rCartItem.Items[item].FindControl("lblName") as Label;
                        int InstrumentId = Convert.ToInt32(_instrumentId.Value);
                        int cartQuantity = Convert.ToInt32(_cartQuantity.Value);
                        int instrumentQuantity = Convert.ToInt32(_instrumentQuantity.Value);
                        if (instrumentQuantity > cartQuantity && instrumentQuantity > 2)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            pName = instrumentName.Text.ToString();
                            break;
                        }
                    }
                }
                if (isTrue)
                {
                    Response.Redirect("Payment.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Item <b>'" + pName + "'</b> is out of stock!";
                    lblMsg.CssClass = "alert alert-warning";
                }
            }

        }

        protected void rCartItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
           if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label totalPrice=e.Item.FindControl("lblTotalPrice") as Label;  
                Label instrumentPrice=e.Item.FindControl("lblPrice") as Label;
                TextBox quantity=e.Item.FindControl("txtQuantity") as TextBox;
                decimal calTotalPrice= Convert.ToDecimal(instrumentPrice.Text)*Convert.ToDecimal(quantity.Text);
                totalPrice.Text = calTotalPrice.ToString();
                grandTotal += calTotalPrice;
            }
           Session["grandTotalPrice"] = grandTotal;
        }

        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }
            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {
                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Your Cart is empty.</b><a href='Catalog.aspx' class='bange bange-info ml-2'>Continue Shopping</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }
        }
    }
}