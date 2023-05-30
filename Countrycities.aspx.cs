using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace task5withGridviewfixed
{
    public partial class Countrycities : System.Web.UI.Page
    {
        public List<City> Cities
        {
            get
            {
                if (ViewState["city"] == null)
                {
                    List<City> cty = new List<City>();
                    ViewState["city"] = cty as List<City>;
                }
                return ViewState["city"] as List<City>;
            }
            set  { ViewState["city"] = value as List<City>; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                defaultsettings();
            }
            else
            {

            }

        }


        public void  SearchValuesFromDb()
        {

            List<City> newallofcites = new List<City>();
            string connectionString = "Data Source= SOFSRVDB02\\CLIENT2016; Initial Catalog= TrainingDB ; User ID=traininguser; pwd=Sofcomtu";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT t1.citycode,t1.cityname , t2.countrycode,t2.countryname FROM Hamza_Cities t1 RIGHT JOIN Hamza_Country t2 on t1.countrycode = t2.countrycode where t2.countrycode = @countrycode; ", connection);
            command.Parameters.Add("@countrycode", SqlDbType.VarChar);
            command.Parameters["@countrycode"].Value = countrycode_TextBox.Text;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.HasRows)
            {
                int citycode = reader.GetOrdinal("citycode");
                int cityname = reader.GetOrdinal("cityname");
                int countrycode = reader.GetOrdinal("countrycode");
                int countryname = reader.GetOrdinal("countryname");


                while (reader.Read())
                {
                    if (!reader.IsDBNull(citycode))
                    {
                        newallofcites.Add(new City { CityCode = reader.GetString(citycode), CityName = reader.GetString(cityname), CountryCode = reader.GetString(countrycode), ExistsOrNotinDB = true });
                        countrycode_TextBox.Text = reader.GetString(countrycode);
                        countryname_textbox.Text = reader.GetString(countryname);

                    }
                    else
                    {
                        countrycode_TextBox.Text = reader.GetString(countrycode);
                        countryname_textbox.Text = reader.GetString(countryname);

                    }
                }
                reader.NextResult();

            }
            connection.Close();
            Cities = newallofcites;
        }


 

        public void defaultsettings()
        {
            countrycode_TextBox.ReadOnly = false;
            countryname_textbox.ReadOnly = true;
            countrycode_TextBox.Text = "";
            countryname_textbox.Text = "";
            find_button.Visible = true;
            CANCEL_button.Visible = true;
            addnewrow.Visible = false;
            Saveallrows.Visible = false;
            Cities = null;
            GridView1.DataBind();
            Delete_all_rows.Visible = false;

        }


        protected void find_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(countrycode_TextBox.Text))
            {
                Response.Write(" INVALID !");
            }
            else
            {
                Response.Write(" VALID !");
                SearchValuesFromDb();
                if (Cities != null)
                {
                    GridView1.DataBind();
                    Response.Write("NO of Countries : " + Cities.Count());
                    if (Cities.Count > 0)
                    {
                        Delete_all_rows.Visible = true;
                        
                    }
                }
                //DataBind();
                countrycode_TextBox.ReadOnly = true;
                find_button.Visible = false;
                Saveallrows.Visible = true;
                addnewrow.Visible = true;
                
            }

        }


        protected void CANCEL_button_Click(object sender, EventArgs e)
        {
            defaultsettings();
            Cities = null;
            GridView1.DataBind();
        }





        public IQueryable<task5withGridviewfixed.City> GridView1_GetData()
        {
            return Cities.AsQueryable();
        }



        public void GridView1_UpdateItem()
        {

            var objecttoupdate = Cities[GridView1.EditIndex];
            if (objecttoupdate != null)
            {
                TryUpdateModel(objecttoupdate);
            }
        }



        protected void addnewrow_Click(object sender, EventArgs e)
        {
            ValidateGridrows();
            Cities.Add(new City { CityCode = "", CityName = "", CountryCode = "", ExistsOrNotinDB = false });


            GridView1.DataBind();
            //var lastrow = GridView1.Rows[GridView1.Rows.Count - 1];
            //lastrow.FindControl("lnkdelete").Visible = false;

        }

        // The id parameter name should match the DataKeyNames value set on the control
        //public void GridView1_DeleteItem(string Citycode)
        //{
        //   Response.Write("to del ");
        //    var objtodel = Cities.FirstOrDefault(c => c.CityCode == Citycode);
        //    Cities.Remove(objtodel);

        //}

        private void ValidateGridrows()
        {

            for (int i = 0; i < Cities.Count; i++)
            {
                GridView1.EditIndex = i;
                GridView1.UpdateRow(i, true);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
            if (e.CommandName == "Deleteitems")
            {
                ValidateGridrows();

                if (e.CommandArgument != null)
                {
                    int row = Convert.ToInt32(e.CommandArgument);
                    Cities.RemoveAt(row);
                    if (Cities.Count < 1)
                    {
                        Delete_all_rows.Visible = false;
                    }
                    GridView1.DataBind();
                }

            }
        }

        protected void Saveallrows_Click(object sender, EventArgs e)
        {
            ValidateGridrows();
            GridView1.DataBind();
            List<City> citiesfrom_db = new List<City>();

            string connectionString = "Data Source= SOFSRVDB02\\CLIENT2016; Initial Catalog= TrainingDB ; User ID=traininguser; pwd=Sofcomtu";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT t1.citycode,t1.cityname , t2.countrycode,t2.countryname FROM Hamza_Cities t1 RIGHT JOIN Hamza_Country t2 on t1.countrycode = t2.countrycode where t2.countrycode = @countrycode; ", connection);

            command.Parameters.Add("@countrycode", SqlDbType.VarChar);
            command.Parameters["@countrycode"].Value = countrycode_TextBox.Text;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.HasRows)
            {
                int citycode = reader.GetOrdinal("citycode");
                int cityname = reader.GetOrdinal("cityname");
                int countrycode = reader.GetOrdinal("countrycode");
                int countryname = reader.GetOrdinal("countryname");

                while (reader.Read())
                {
                    if (!reader.IsDBNull(citycode))
                    {
                        citiesfrom_db.Add(new City { CityCode = reader.GetString(citycode), CityName = reader.GetString(cityname), CountryCode = reader.GetString(countrycode), ExistsOrNotinDB = true });
                        countrycode_TextBox.Text = reader.GetString(countrycode);
                        countryname_textbox.Text = reader.GetString(countryname);
                    }
                    else
                    {
                        countrycode_TextBox.Text = reader.GetString(countrycode);
                        countryname_textbox.Text = reader.GetString(countryname);
                    }
                }
                reader.NextResult();

            }
            connection.Close();

            List < City > cities_requring_deleting = citiesfrom_db;
            

            foreach (var city in Cities )
            {
                var obj = citiesfrom_db.FirstOrDefault(x => x.CityCode == city.CityCode);           
                if (obj != null)
                {
                    SqlCommand cmd1 = new SqlCommand("update Hamza_Cities set cityname = @cityname  where citycode = @citycode ;", connection);
                    cmd1.Parameters.Add("@citycode", SqlDbType.VarChar);
                    cmd1.Parameters.Add("@cityname", SqlDbType.VarChar);
                    cmd1.Parameters.Add("@countrycode", SqlDbType.VarChar);
                    cmd1.Parameters["@citycode"].Value = city.CityCode.ToString();
                    cmd1.Parameters["@cityname"].Value = city.CityName.ToString();
                    cmd1.Parameters["@countrycode"].Value = city.CountryCode.ToString(); ;
                    int result;
                    connection.Open();
                    result = cmd1.ExecuteNonQuery();
                    connection.Close();

                    citiesfrom_db.Remove(obj);
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("INSERT INTO Hamza_Cities VALUES(@citycode ,@cityname ,@countrycode);", connection);
                    cmd.Parameters.Add("@citycode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@cityname", SqlDbType.VarChar);
                    cmd.Parameters.Add("@countrycode", SqlDbType.VarChar);
                    cmd.Parameters["@citycode"].Value = city.CityCode.ToString();
                    cmd.Parameters["@cityname"].Value = city.CityName.ToString();
                    cmd.Parameters["@countrycode"].Value = countrycode_TextBox.Text;

                    int result;
                    connection.Open();
                    result = cmd.ExecuteNonQuery();
                    connection.Close();
                }  
            }
            foreach (var objToDelete in citiesfrom_db)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Hamza_Cities WHERE citycode = @citycode ;", connection);
                cmd.Parameters.Add("@citycode", SqlDbType.VarChar);
                cmd.Parameters["@citycode"].Value = objToDelete.CityCode.ToString();
                
                int result;
                connection.Open();
                result = cmd.ExecuteNonQuery();
                connection.Close();

            }

            defaultsettings();

        }

        protected void Delete_all_rows_Click(object sender, EventArgs e)
        {
            ValidateGridrows();
            GridView1.DataBind();
            List<City> citiesfrom_db_fordeletion = new List<City>();

            string connectionString = "Data Source= SOFSRVDB02\\CLIENT2016; Initial Catalog= TrainingDB ; User ID=traininguser; pwd=Sofcomtu";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT t1.citycode,t1.cityname , t2.countrycode,t2.countryname FROM Hamza_Cities t1 RIGHT JOIN Hamza_Country t2 on t1.countrycode = t2.countrycode where t2.countrycode = @countrycode; ", connection);
            command.Parameters.Add("@countrycode", SqlDbType.VarChar);
            command.Parameters["@countrycode"].Value = countrycode_TextBox.Text;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.HasRows)
            {
                int citycode = reader.GetOrdinal("citycode");
                int cityname = reader.GetOrdinal("cityname");
                int countrycode = reader.GetOrdinal("countrycode");
                int countryname = reader.GetOrdinal("countryname");

                while (reader.Read())
                {
                    if (!reader.IsDBNull(citycode))
                    {
                        citiesfrom_db_fordeletion.Add(new City { CityCode = reader.GetString(citycode), CityName = reader.GetString(cityname), CountryCode = reader.GetString(countrycode), ExistsOrNotinDB = true });
                        countrycode_TextBox.Text = reader.GetString(countrycode);
                        countryname_textbox.Text = reader.GetString(countryname);
                    }
                    else
                    {
                        countrycode_TextBox.Text = reader.GetString(countrycode);
                        countryname_textbox.Text = reader.GetString(countryname);
                    }
                }
                reader.NextResult();

            }
            connection.Close();


            foreach (var itemstodelete in citiesfrom_db_fordeletion)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Hamza_Cities WHERE citycode = @citycode ;", connection);
                cmd.Parameters.Add("@citycode", SqlDbType.VarChar);
                cmd.Parameters["@citycode"].Value = itemstodelete.CityCode.ToString();

                int result;
                connection.Open();
                result = cmd.ExecuteNonQuery();
                connection.Close();

            }

            defaultsettings();

        }

        // The id parameter name should match the DataKeyNames value set on the control

    }
}