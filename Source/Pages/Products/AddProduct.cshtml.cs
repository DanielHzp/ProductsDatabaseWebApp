using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//SQL Client Namespace 
using System.Data.SqlClient;



namespace ProductOrders.Pages.Products
{
    public class AddProductModel : PageModel
    {
        //Object reference of product data model CLASS (created in Main page model)
        public ProductInfo productInfInstance=new ProductInfo();

        public String captureErrorMessage = "";


        public String captureSuccessMessage = "";

        public void OnGet()
        {
        }

        //This handler method is executed when data of this form is submitted, a new product is added by the user
        public void OnPost()
        {
            //Use Request.Form http 'inherited' function to make the server read the input values
            productInfInstance.ProductName = Request.Form["Name"];
            productInfInstance.SkuReference = Request.Form["SkuReference"];
            productInfInstance.CostCenterId=Request.Form["CostCenterId"];
            productInfInstance.ProductDescription = Request.Form["ProductDescription"];
            //Request.form[@form control name], POST method is used with a form used to input values


            if (productInfInstance.ProductName.Length == 0 || productInfInstance.SkuReference.Length == 0 ||
          productInfInstance.CostCenterId.Length == 0 || productInfInstance.ProductDescription.Length == 0)
            {
                captureErrorMessage = "All input parameters are required";

                //Quite OnPost method
                return;
            }

            //Save the created new product instance in the database
            try
            {
                //Connect to the data base in order TO ADD NEW RECORD INSTANCE
                //Connection string of the db is found in server explorer properties
                String connectionDBString = "Data Source=.\\SQLEXPRESS;Initial Catalog=ProductOrdersDatabase;Integrated Security=True";

                using (SqlConnection PostDBConnection=new SqlConnection(connectionDBString))
                {
                    //We need to add the SQL Client namespace at the beginning
                    PostDBConnection.Open();

                    //Query to execute in order to add new DB RECORD/ROW
                    String AddDbData = "INSERT INTO products " +
                        "(ProductName,SkuReference, CostCenterId, ProductDescription) VALUES" +
                        "(@Name, @SkuReference, @CostCenterId, @ProductDescription);";
                    // use @ with the database attributes name to capture attributes data received from the form controls 


                    //Create SQL command to execute query script
                    using (SqlCommand execute=new SqlCommand(AddDbData,PostDBConnection))
                    {
                        //Map values to data model
                        execute.Parameters.AddWithValue("@Name", productInfInstance.ProductName);
                        execute.Parameters.AddWithValue("@SkuReference", productInfInstance.SkuReference);
                        execute.Parameters.AddWithValue("@CostCenterId", productInfInstance.CostCenterId);
                        execute.Parameters.AddWithValue("@ProductDescription", productInfInstance.ProductDescription);
                        
                        execute.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex) 
            {
                //Exit method and save error message
                captureErrorMessage=ex.Message;
                return;
            }

            productInfInstance.ProductName = "";
            productInfInstance.SkuReference = "";
            productInfInstance.CostCenterId = "";
            productInfInstance.ProductDescription = "";
            captureSuccessMessage = "Product added correctly";


            //Redirect to starting page after clicking 'add new product'
            Response.Redirect("/Products/Main");


        }

    }
}
