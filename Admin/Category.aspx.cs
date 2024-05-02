using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Shop.Admin
{
    public partial class Category : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                Session["breadCrum"] = "Category";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else {getCategories(); }
                

            }
            lblMsg.Visible = false;
            
            

        }
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value);
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            if (fuCategoryImage.HasFile) //checks if a file has been uploaded
            {
                if (Utils.IsValidExtension(fuCategoryImage.FileName)) //checks if the file extension is valid
                {
                    Guid obj = Guid.NewGuid(); //generates a new GUID
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName); //gets the file extension
                    imagePath = "Images/Category/" + obj.ToString() + fileExtension; //creates the path to the file

                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension); //saves the file to the server
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
                isValidToExecute=true;
            }
            if (isValidToExecute)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    actionName = categoryId == 0 ? "inserted" : "updated";
                    lblMsg.Visible=true;
                    lblMsg.Text = "Category " + actionName + " successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                    clear();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible=true;
                    lblMsg.Text="Error - "+ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally {
                    con.Close();
                }
            }
        

        }

        private void getCategories()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        private void clear()
        {
            txtName.Text =string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text="Add";
            imgCategory.ImageUrl=String.Empty;

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")

            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");

                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.CommandType= CommandType.StoredProcedure;

                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();

                sda.Fill(dt);
                txtName.Text = dt.Rows[0]["Name"].ToString();

                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                imgCategory.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ? "../Images/No_image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                imgCategory.Height = 200;
                imgCategory.Width = 200;

                hdnId.Value = dt.Rows[0]["CategoryId"].ToString();
                btnAddOrUpdate.Text = "Update";

                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";

            }
            else if (e.CommandName == "delete")
            {
                //con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Category_Crud", con);

                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);

                cmd.CommandType = CommandType.StoredProcedure;
                try

                {
                    con.Open();

                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;

                    lblMsg.Text = "Category deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
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

        protected void rCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label lbl = e.Item.FindControl("lblIsActive") as Label;
                if (lbl.Text == "True")

                {
                    lbl.Text = "Active";
                    lbl.CssClass = "badge badge-success";
                }
                else
                {
                    lbl.Text = "In-Active";
                    lbl.CssClass = "badge badge-danger";
                }
            }
        }
            }
}
       