using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Shop.Admin
{
    public partial class Instrument : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Instrument";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else {    getInstruments();
}
             
            }
            lblMsg.Visible = false;

        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int instrumentId = Convert.ToInt32(hdnId.Value);
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Instrument_Crud", con);
            cmd.Parameters.AddWithValue("@Action", instrumentId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@InstrumentId", instrumentId);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
            cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text.Trim());
            cmd.Parameters.AddWithValue("@CategoryId", ddlCategories.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            if (fuInstrumentImage.HasFile) //checks if a file has been uploaded
            {
                if (Utils.IsValidExtension(fuInstrumentImage.FileName)) //checks if the file extension is valid
                {
                    Guid obj = Guid.NewGuid(); //generates a new GUID
                    fileExtension = Path.GetExtension(fuInstrumentImage.FileName); //gets the file extension
                    imagePath = "Images/Instrument/" + obj.ToString() + fileExtension; //creates the path to the file

                    fuInstrumentImage.PostedFile.SaveAs(Server.MapPath("~/Images/Instrument/") + obj.ToString() + fileExtension); //saves the file to the server
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath); //adds a parameter to a command object with the path to the uploaded file
                    isValidToExecute = true;

                }
                else //if the file extension is not valid
                {
                    lblMsg.Visible = true; //makes the error message label visible
                    lblMsg.Text = "Please select .jpg, .jpeg or .png image"; //sets the error message text
                    lblMsg.CssClass = "alert alert-danger"; //sets the CSS class for the error message label
                    isValidToExecute = false;
                }
            }
            else
            {
                isValidToExecute = true;
            }
            if (isValidToExecute)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    actionName = instrumentId == 0 ? "inserted" : "updated";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Instrument " + actionName + " successful!";
                    lblMsg.CssClass = "alert alert-success";
                    getInstruments();
                    clear();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void rInstrument_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")

            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Instrument_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");

                cmd.Parameters.AddWithValue("@InstrumentId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;

                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();

                sda.Fill(dt);
                txtName.Text = dt.Rows[0]["Name"].ToString();
                txtDescription.Text = dt.Rows[0]["Description"].ToString();
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                ddlCategories.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                imgInstrument.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ? "../Images/No_image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                imgInstrument.Height = 200;
                imgInstrument.Width = 200;

                hdnId.Value = dt.Rows[0]["InstrumentId"].ToString();
                btnAddOrUpdate.Text = "Update";

                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";

            }
            else if (e.CommandName == "delete")
            {
                //con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Instrument_Crud", con);

                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@InstrumentId", e.CommandArgument);

                cmd.CommandType = CommandType.StoredProcedure;
                try

                {
                    con.Open();

                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;

                    lblMsg.Text = "Musical Instrument deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getInstruments();
                }

                catch (Exception ex)
                {

                    lblMsg.Visible = true;
                    lblMsg.Text = "Error-" + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }


                finally
                {
                    con.Close();
                }
            }

        }

        protected void rInstrument_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label lblIsActive = e.Item.FindControl("lblIsActive") as Label;
                Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;
                if (lblIsActive.Text == "True")

                {
                    lblIsActive.Text = "Active";
                    lblIsActive.CssClass = "badge badge-success";
                }
                else
                {
                    lblIsActive.Text = "In-Active";
                    lblIsActive.CssClass = "badge badge-danger";
                }
                if (Convert.ToInt32(lblQuantity.Text) <= 5)
                {
                    lblQuantity.CssClass = "badge badge-danger";
                    lblQuantity.ToolTip = "Item about to be 'Out of stock!'";
                }

            }
        }


        private void getInstruments()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Instrument_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rInstrument.DataSource = dt;
            rInstrument.DataBind();
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtPrice.Text = string.Empty;
            ddlCategories.ClearSelection();
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            imgInstrument.ImageUrl = String.Empty;

        }
    }
}