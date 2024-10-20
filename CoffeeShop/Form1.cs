using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter adapter;
        DataSet ds;

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCoffeeData();
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        adapter = new SqlDataAdapter("select * from Coffee", conn);

        //        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);



        //        ds = new DataSet();
        //        adapter.Fill(ds, "Coffee");
        //        dataGridView1.DataSource = null;
        //        dataGridView1.DataSource = ds.Tables["Coffee"];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);

        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameTextBox.Text;
                int countryId = int.Parse(countryIdTextBox.Text);
                int coffeeTypeId = int.Parse(coffeeTypeIdTextBox.Text);
                string description = descriptionTextBox.Text;
                int grams = int.Parse(gramsTextBox.Text);
                decimal costPrice = decimal.Parse(costPriceTextBox.Text);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string insertQuery = @"INSERT INTO CoffeeVarieties (Name, CountryId, CoffeeTypeId, Description, Grams, CostPrice) 
                                   VALUES (@name, @countryId, @coffeeTypeId, @description, @grams, @costPrice)";

                    using (SqlCommand command = new SqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@countryId", countryId);
                        command.Parameters.AddWithValue("@coffeeTypeId", coffeeTypeId);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@grams", grams);
                        command.Parameters.AddWithValue("@costPrice", costPrice);
                        command.ExecuteNonQuery();
                    }
                }
                LoadCoffeeData();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных: " + ex.Message);
            }
        }

        private void LoadCoffeeData()
        {
            try
            {
                conn.Open();
                adapter = new SqlDataAdapter("SELECT * FROM CoffeeVarieties", conn);
                ds = new DataSet();
                adapter.Fill(ds, "CoffeeVarieties");
                dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    int coffeeId = (int)dataGridView1.Rows[selectedRowIndex].Cells["id"].Value;
                    conn.Open();

                    string deleteQuery = "DELETE FROM CoffeeVarieties WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(deleteQuery, conn))
                    {
                        command.Parameters.AddWithValue("@id", coffeeId);

                        command.ExecuteNonQuery();
                    }
                    adapter = new SqlDataAdapter("SELECT * FROM CoffeeVarieties", conn);
                    ds = new DataSet();
                    adapter.Fill(ds, "CoffeeVarieties");
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                }
                else
                {
                    MessageBox.Show("Please select a coffee variety to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    int coffeeId = (int)dataGridView1.Rows[selectedRowIndex].Cells["id"].Value;
                    string newName = nameTextBox.Text;
                    int newCountryId = int.Parse(countryIdTextBox.Text);
                    int newCoffeeTypeId = int.Parse(coffeeTypeIdTextBox.Text);
                    string newDescription = descriptionTextBox.Text;
                    int newGrams = int.Parse(gramsTextBox.Text);
                    decimal newCostPrice = decimal.Parse(costPriceTextBox.Text);
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                    {
                        conn.Open();
                        string updateQuery = @"UPDATE CoffeeVarieties 
                                       SET Name = @name, CountryId = @countryId, CoffeeTypeId = @coffeeTypeId, 
                                           Description = @description, Grams = @grams, CostPrice = @costPrice
                                       WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(updateQuery, conn))
                        {
                            command.Parameters.AddWithValue("@id", coffeeId);
                            command.Parameters.AddWithValue("@name", newName);
                            command.Parameters.AddWithValue("@countryId", newCountryId);
                            command.Parameters.AddWithValue("@coffeeTypeId", newCoffeeTypeId);
                            command.Parameters.AddWithValue("@description", newDescription);
                            command.Parameters.AddWithValue("@grams", newGrams);
                            command.Parameters.AddWithValue("@costPrice", newCostPrice);
                            command.ExecuteNonQuery();
                        }
                    }
                    LoadCoffeeData();
                }
                else
                {
                    MessageBox.Show("выберите строку для редактирования.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                adapter = new SqlDataAdapter("SELECT Name, Description, Grams, CostPrice\r\nFROM CoffeeVarieties\r\nWHERE description LIKE '%cherry%';", conn);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                ds = new DataSet();
                adapter.Fill(ds, "CoffeeVarieties");
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                decimal minCostPrice = decimal.Parse(minCostTextBox.Text);
                decimal maxCostPrice = decimal.Parse(maxCostTextBox.Text);

                if (minCostPrice > maxCostPrice)
                {
                    MessageBox.Show("Минимальная себестоимость не может быть больше максимальной.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT * FROM CoffeeVarieties 
                                   WHERE CostPrice BETWEEN @minCostPrice AND @maxCostPrice";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@minCostPrice", minCostPrice);
                        adapter.SelectCommand.Parameters.AddWithValue("@maxCostPrice", maxCostPrice);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("введите корректные значения для диапазона себестоимости.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int minGrams = int.Parse(minGramsTextBox.Text);
                int maxGrams = int.Parse(maxGramsTextBox.Text);

                if (minGrams > maxGrams)
                {
                    MessageBox.Show("Минимальное количество грамм не может быть больше максимального.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT * FROM CoffeeVarieties 
                                   WHERE Grams BETWEEN @minGrams AND @maxGrams";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@minGrams", minGrams);
                        adapter.SelectCommand.Parameters.AddWithValue("@maxGrams", maxGrams);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("введите корректные значения для диапазона количества грамм.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = (countriesTextBox.Text);


                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT CV.[Name], CV.[Description], CV.[Grams], CV.[CostPrice]
                                           FROM [CoffeeVarieties] CV
                                           join [Countries] C on C.[Id] = CV.[CountryId]
                                           where C.[Name] = @Countries";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = (countriesTextBox.Text);


                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT CV.[Name], CV.[Description], CV.[Grams], CV.[CostPrice]
                                           FROM [CoffeeVarieties] CV
                                           join [Countries] C on C.[Id] = CV.[CountryId]
                                           where C.[Name] = @Countries";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {


                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT C.Name, COUNT(CV.[Id]) AS CoffeeCount
                                            FROM [Countries] C
                                            JOIN [CoffeeVarieties] CV ON C.[Id] = CV.[countryId]
                                            GROUP BY C.[Name];
                                            ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        ds = new DataSet();
                        adapter.Fill(ds, "Countries");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["Countries"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {


                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT C.[Name], AVG(CV.[Grams]) AS AverageGrams
                                           FROM [Countries] C
                                           JOIN [CoffeeVarieties] CV ON C.[Id] = CV.[CountryId]
                                          GROUP BY C.[Name];";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        ds = new DataSet();
                        adapter.Fill(ds, "Countries");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["Countries"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox1.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT Top 3 CV.[Name], CV.[Grams], CV.[CostPrice]
                                           FROM [CoffeeVarieties] CV
                                           join [Countries] C on C.[Id] = CV.[CountryId]
                                           where C.[Name] = @Countries
                                           ORDER BY costPrice ASC;
                                            ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT Top 3 CV.[Name], CV.[Grams], CV.[CostPrice]
                                           FROM [CoffeeVarieties] CV
                                           join [Countries] C on C.[Id] = CV.[CountryId]
                                           where C.[Name] = @Countries
                                           ORDER BY costPrice DESC;
                                            ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT Top 3 C.[Name], CV.[grams], CV.[costPrice]
                                            FROM [Countries] C
                                            Join [CoffeeVarieties] CV on CV.[CountryId] = C.[Id]
                                            ORDER BY CV.[CostPrice] ASC
                                            ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT Top 3 C.[Name], CV.[grams], CV.[costPrice]
                                            FROM [Countries] C
                                            Join [CoffeeVarieties] CV on CV.[CountryId] = C.[Id]
                                            ORDER BY CV.[CostPrice] DESC
                                            ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT Top 3 c.Name, COUNT(cv.id) AS coffeeCount
                                            FROM Countries c
                                            JOIN CoffeeVarieties cv ON c.id = cv.countryId
                                            GROUP BY c.Name
                                            ORDER BY coffeeCount DESC;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT TOP 3 c.Name, SUM(cv.grams) AS totalGrams
                                        FROM Countries c
                                        JOIN CoffeeVarieties cv ON c.id = cv.countryId
                                        GROUP BY c.Name
                                        ORDER BY totalGrams DESC
                                        ;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT TOP 3 CV.Name, SUM(CV.Grams) AS totalGrams
                                        FROM [CoffeeVarieties] CV
                                        join [CoffeeTypes] CT on CT.[Id] = CV.[CoffeeTypeId]
                                        where CT.[Name] = 'Arabica'
                                        GROUP BY CV.Name
                                        ORDER BY totalGrams DESC
                                        ;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT TOP 3 CV.Name, SUM(CV.Grams) AS totalGrams
                                        FROM [CoffeeVarieties] CV
                                        join [CoffeeTypes] CT on CT.[Id] = CV.[CoffeeTypeId]
                                        where CT.[Name] = 'Blend' or CT.[Name] = 'Robusta'
                                        GROUP BY CV.Name
                                        ORDER BY totalGrams DESC
                                        ;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                string Countries = countriesTextBox2.Text;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlConnLibrary"].ConnectionString))
                {
                    conn.Open();

                    string selectQuery = @"SELECT TOP 3 CT.Name, SUM(CV.Grams) AS totalGrams
                                           FROM [CoffeeVarieties] CV
                                           join [CoffeeTypes] CT on CT.[Id] = CV.[CoffeeTypeId]
                                           GROUP BY CT.[Name]
                                           ORDER BY totalGrams DESC
                                        ;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@Countries", Countries);

                        ds = new DataSet();
                        adapter.Fill(ds, "CoffeeVarieties");

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = ds.Tables["CoffeeVarieties"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
