using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ProductOrders.Pages.Products
{
    public class MainModel : PageModel
    {
        //Store fetched data in a list, from products database instance
        //The list contains database object references (ProductInfo class)
        public List<ProductInfo> ProductsList=new List<ProductInfo>();

        //Used by the page when we use a http method to read data from our database connection
        public void OnGet()
        {
            //Connect to the database
            try
            {
                //Declare connection string
                String databaseConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=ProductOrdersDatabase;Integrated Security=True";

                //Create SQL connection
                using (SqlConnection connection=new SqlConnection(databaseConnection))
                {
                    //Open connection 
                    connection.Open();

                    //Use SQL query to read products data
                    String FetchDBData = "SELECT * FROM products";


                    //Creat SQL command to execute query
                    //We need a SqlCommand object reference
                    using (SqlCommand ReadData = new SqlCommand(FetchDBData, connection)) 
                    {
                        //With SqlCommand reference we need to create a data reader object reference
                        using (SqlDataReader ExecuteSql= ReadData.ExecuteReader())
                        {
                            //Loop over table stored in SqlDataReader reference (ExecuteSql)
                            while (ExecuteSql.Read())
                            {
                                //Save data in product info object
                                ProductInfo ProductInfoInstance = new ProductInfo();

                                //Get values of specified column attribute and save them
                                ProductInfoInstance.PrimaryKeyId = "" + ExecuteSql.GetInt32(0);
                                ProductInfoInstance.ProductName = ExecuteSql.GetString(1);
                                ProductInfoInstance.SkuReference = ExecuteSql.GetString(2);
                                ProductInfoInstance.CostCenterId = ExecuteSql.GetString(3);
                                ProductInfoInstance.ProductDescription = ExecuteSql.GetString(4);
                                ProductInfoInstance.CreationTime= ExecuteSql.GetDateTime(5).ToString();
                               
                                
                                //ProductInfoInstance object/ROW must be added to the List<>
                                ProductsList.Add(ProductInfoInstance);
                            
                            
                            }
                        }
                    }
                }

            }
            catch (Exception ex) 
            {
                //Display error in the console in case of an exception
                Console.WriteLine("Exception message: "+ex.ToString());
            }


        }
    }
    //New class for product info., It stores data OF ONE PRODUCT on the database
    public class ProductInfo
    {
        public String PrimaryKeyId;
        public String ProductName;
        public String SkuReference;
        public String CostCenterId;
        public String ProductDescription;
        public String CreationTime; 
        

    }

}
