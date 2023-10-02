using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ProductOrders.Pages.Products
{
    public class EditProductModel : PageModel
    {
        //Create object instance of data model class (created in Main page model)
        public ProductInfo productInfInstance=new ProductInfo();

        public String captureErrorMessage = "";
        public String captureSuccessMessage = "";



        //GET to obtain data of the product selected for edition and display on the form
        //POST update product details in the database

        public void OnGet()
        {
            //We can use this.Request.Query to obtain the collection of products
            //Use Request.Query http inherited function to read the primary key of the current product
            //Provide name of the paramater of the product id
            //(URL that the edit button redirects to: href="/Products/EditProduct?   >>ID<<   =@productIndex.PrimaryKeyId" )
            String id = Request.Query["id"]; //"id" value is inherited from the http request

            try
            {
                //Declare connection string
                String databaseConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=ProductOrdersDatabase;Integrated Security=True";

                //Create SQL connection
                using (SqlConnection connection = new SqlConnection(databaseConnection))
                {
                    connection.Open();

                    //Select the product with the coresponding id 
                    String GetCurrentProdData = "SELECT * FROM products WHERE PrimaryKey=@PrimaryKeyId"; //Use @VariableName inside SQL query
                    
                    //Crete SQL script command 
                    using(SqlCommand mapProductIdDB=new SqlCommand(GetCurrentProdData,connection) )
                    {
                        //Map primary key obtained value from http request method
                        //Assign the value to @PrimaryKeyId SQL variable in the script that will be executed
                        mapProductIdDB.Parameters.AddWithValue("@PrimaryKeyId",id);

                        using (SqlDataReader ExecuteSql = mapProductIdDB.ExecuteReader())
                        {
                            if(ExecuteSql.Read())
                            {                          

                                //Get values of the specified column attribute and save them
                                productInfInstance.PrimaryKeyId = "" + ExecuteSql.GetInt32(0);
                                productInfInstance.ProductName = ExecuteSql.GetString(1);
                                productInfInstance.SkuReference = ExecuteSql.GetString(2);
                                productInfInstance.CostCenterId = ExecuteSql.GetString(3);
                                productInfInstance.ProductDescription = ExecuteSql.GetString(4);
                            }
                        }
                    }
                }
                }
            catch(Exception ex)
            {
                captureErrorMessage = ex.Message;
            }

        }





        //POST to update product details INPUTS in the database
        public void OnPost()       
       {
            productInfInstance.PrimaryKeyId = Request.Form["PrimaryKeyCurrentProduct"];

            //Use Request.Form http 'inherited' function to make the server read the NEW values input by the USER
            productInfInstance.ProductName = Request.Form["Name"];
            productInfInstance.SkuReference = Request.Form["SkuReference"];
            productInfInstance.CostCenterId = Request.Form["CostCenterId"];
            productInfInstance.ProductDescription = Request.Form["ProductDescription"];
            //Request.form[@form control name], POST method is used with a form control used to input values


            if (productInfInstance.ProductName.Length == 0 || productInfInstance.SkuReference.Length == 0 ||
          productInfInstance.CostCenterId.Length == 0 || productInfInstance.ProductDescription.Length == 0)
            {
                captureErrorMessage = "All input parameters are required";

                //Quite OnPost method
                return;
            }

            try
            {
                //Connect to the database and update the data of the product EDITED
                //Connection string of the db is found in server explorer properties
                String connectionDBString = "Data Source=.\\SQLEXPRESS;Initial Catalog=ProductOrdersDatabase;Integrated Security=True";

                using (SqlConnection PostDBConnection = new SqlConnection(connectionDBString))
                {
                    //We need to add the SQL Client namespace at the beginning
                    PostDBConnection.Open();

                    //Query that UPDATES the product selected DB RECORD/ROW
                    String UpdateDbData = "UPDATE products " +
                        "SET ProductName=@ProductName, SkuReference=@SkuReference, CostCenterId=@CostCenterId, ProductDescription=@ProductDescription " +
                        "WHERE PrimaryKey=@PrimaryKeyId";
                    // use @ with the database attributes name to capture attributes data received from the form controls 


                    //Create SQL command to execute query script
                    using (SqlCommand execute = new SqlCommand(UpdateDbData, PostDBConnection))
                    {
                        //Map SQL script @variables to data model
                        execute.Parameters.AddWithValue("@ProductName", productInfInstance.ProductName);
                        execute.Parameters.AddWithValue("@SkuReference", productInfInstance.SkuReference);
                        execute.Parameters.AddWithValue("@CostCenterId", productInfInstance.CostCenterId);
                        execute.Parameters.AddWithValue("@ProductDescription", productInfInstance.ProductDescription);
                        execute.Parameters.AddWithValue("@PrimaryKeyId", productInfInstance.PrimaryKeyId);

                        execute.ExecuteNonQuery();
                    }
                }

                }
            catch(Exception ex)
            {
                captureErrorMessage = ex.Message;

                //Quite OnPost method
                return;
            }

            //Redirect user to the list of products
            Response.Redirect("/Products/Main");

        }
    }
}
