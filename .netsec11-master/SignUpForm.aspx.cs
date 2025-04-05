using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Tour_Management
{
    public partial class SignUpForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["tourdb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Check if the email already exists
                string checkQuery = "SELECT COUNT(*) FROM UserInfo WHERE Email = @Email";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email.Text);
                    int userCount = (int)checkCmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        // User already exists, show an alert message
                        Response.Write("<script>alert('User already exists! Please use a different email.');</script>");
                        return; // Stop further execution
                    }
                }

                // Step 2: Insert new user if the email does not exist
                string insertQuery = "INSERT INTO UserInfo (Email, FirstName, LastName, Gender, Password, dob, Street, City, State) " +
                                     "VALUES (@Email, @FirstName, @LastName, @Gender, @Password, @dob, @Street, @City, @State)";

                using (SqlCommand com = new SqlCommand(insertQuery, conn))
                {
                    com.Parameters.AddWithValue("@Email", email.Text);
                    com.Parameters.AddWithValue("@FirstName", fname.Text);
                    com.Parameters.AddWithValue("@LastName", lname.Text);
                    com.Parameters.AddWithValue("@Gender", gender.Text);
                    com.Parameters.AddWithValue("@Password", password1.Text); // 🔴 Hash this in a real project!
                    com.Parameters.AddWithValue("@dob", dob.Text);
                    com.Parameters.AddWithValue("@Street", street.Text);
                    com.Parameters.AddWithValue("@City", city.Text);
                    com.Parameters.AddWithValue("@State", state.Text);

                    com.ExecuteNonQuery();
                }
            }

            // Registration successful, redirect to login page
            Response.Redirect("userlogin.aspx");
        }

    }
}
