using System;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Project4
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=(local); integrated security=SSPI; Database=Project6";
		private SqlConnection connection = new SqlConnection();
        public Form1()
        {
            InitializeComponent();
            try
			{
				connection.ConnectionString = connectionString;
				
				UpdateGrid();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
			}
        }

        private void buttonTotalPrice_Click(object sender, EventArgs e)
        {
            // Total price button
            var dt = DateTime.Now;
            decimal finalPrice = 0.00M;
            for (int i = 0; i < textBoxOrders.Lines.Length; i++)
            {
                decimal d;
                if(decimal.TryParse(textBoxOrders.Lines[i].TrimStart('$'), out d))
                {
                    finalPrice += d;
                }
            }
            // Setting label price.
            labelTotalPrice.Text = "$" + finalPrice.ToString();

            // Sql connection
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = "Insert Into Orders (DateTime, OrderTotalPrice) Values ("
                + "'" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + finalPrice + "'" + ")";

            connection.Open();

            try
			{
				command.ExecuteNonQuery();
				Console.WriteLine("Record inserted...");
				UpdateGrid();
			}
			catch (Exception exception)
			{
                Console.WriteLine(exception.Message);
			}
			finally
			{
				connection.Close();
			}
        }

        private void groupBoxFlavor_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // On load, load in data from Data.txt.
            StreamReader reader = new StreamReader("Data.txt");

            string record = reader.ReadLine();

            while (record != null)
            {
                // split the record to remove date/time from loading into textbox.
                string[] parts = record.Split('#');
                // If data is long enough to split, work with it.
                if(parts.Length >= 2)
                {
                    // If the file was not tampered with, this will give us correct data.
                    textBoxOrders.Text = textBoxOrders.Text + "$" + parts[1] + Environment.NewLine;
                }
                record = reader.ReadLine();
            }
            reader.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Introduce decimal variables for manipulation.
            const decimal ICE_CREAM_FLAVOR_CHOCOLATE = 4.00M;
            const decimal ICE_CREAM_FLAVOR_VANILLA = 4.00M;
            const decimal ICE_CREAM_FLAVOR_STRAWBERRY = 4.00M;

            const decimal TOPPING_SPRINKLES = 0.50M;
            const decimal TOPPING_CHOPPED_NUTS = 1.75M;
            const decimal TOPPING_CHERRY = 1.00M;

            decimal totalPrice = 0;

            // radioButton & checkBox variable attachment & check.
            if (radioButtonChocolate.Checked == true)
            {
                totalPrice = ICE_CREAM_FLAVOR_CHOCOLATE;
            }
            else if (radioButtonVanilla.Checked == true)
            {
                totalPrice = ICE_CREAM_FLAVOR_VANILLA;
            }
            else
            {
                totalPrice = ICE_CREAM_FLAVOR_STRAWBERRY;
            }

            if (checkBoxSprinkles.Checked == true)
            {
                totalPrice += TOPPING_SPRINKLES;
            }

            if (checkBoxChoppedNuts.Checked == true)
            {
                totalPrice += TOPPING_CHOPPED_NUTS;
            }

            if (checkBoxCherry.Checked == true)
            {
                totalPrice += TOPPING_CHERRY;
            }

            //Write the total price to the data file.
            StreamWriter writer = new StreamWriter("Data.txt", true);
            var dt = DateTime.Now;
            writer.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "#" + totalPrice.ToString());
            writer.Flush();
            writer.Close();

            textBoxOrders.Text = textBoxOrders.Text + "$" + totalPrice.ToString() + Environment.NewLine;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter writer = new StreamWriter("Data.txt");
            writer.Flush();
            writer.Close();
            textBoxOrders.Text = "";
        }

        private void UpdateGrid()
		{
			SqlCommand command = new SqlCommand();
			command.Connection = connection;
			command.CommandText = "Select * From Orders";

			SqlDataAdapter dataAdapter = new SqlDataAdapter();
			dataAdapter.SelectCommand = command;

            DataSet dataSet = new DataSet();
			dataAdapter.Fill(dataSet);

			DataTable dataTable = dataSet.Tables[0];

            textBoxAllOrders.Text = "";
            foreach (DataRow row in dataTable.Rows)
            {
                textBoxAllOrders.Text = textBoxAllOrders.Text + "$" + row[1].ToString() + Environment.NewLine;
            }
		}

        private void textBoxAllOrders_TextChanged(object sender, EventArgs e)
        {

        }

        private void totalAllOrders_Click(object sender, EventArgs e)
        {
            // Grab all numerical data.
            decimal finalPrice = 0.00M;
            for (int i = 0; i < textBoxAllOrders.Lines.Length; i++)
            {
                decimal d;
                if (decimal.TryParse(textBoxAllOrders.Lines[i].TrimStart('$'), out d))
                {
                    finalPrice += d;
                }
            }
            // Setting label price.
            labelTotalAllOrders.Text = "$" + finalPrice.ToString();
        }

        private void labelTotalAllOrders_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
