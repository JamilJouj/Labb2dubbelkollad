using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;


namespace Labb2
{
    public static class Connection
    {
        private static readonly string connectionUri = "mongodb+srv://jamiljouj:Mousa9804@jones.takgb.mongodb.net/?retryWrites=true&w=majority&appName=Jones";
        private static readonly MongoClient Client;
        private static readonly IMongoDatabase Database;

    
    
    
        static Connection()
        {
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            Client = new MongoClient(settings);
            Database = Client.GetDatabase("Butik");
        }

        public static IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }

        public static void DeleteProduct()
        {
            var productsCollection = GetCollection<BsonDocument>("Products");
            var filter = Builders<BsonDocument>.Filter.Empty;
            var products = productsCollection.Find(filter).ToList();
            foreach (var doc in products)
            {
                Console.WriteLine();
                Console.WriteLine($"\nProduct: {doc["ProductName"]}");
                Console.WriteLine();
                Thread.Sleep(250);
            }

            Console.Write("type the product you would like to delete: ");
            string input = Console.ReadLine();
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("ProductName", input);
            var result = productsCollection.DeleteOne(deleteFilter);
            if (result.DeletedCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Successfully deleted product with name {input}.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"No product found with the name: {input}.");
            }
        }

        public static void AddProductToDb()
        {
            var productsCollection = GetCollection<BsonDocument>("Products");
            Console.Write("Type in the product name: ");
            string inputName = Console.ReadLine();
            Console.WriteLine();
            Console.Write("enter the price: ");
            string inputprice = Console.ReadLine();
            Console.WriteLine();
            var newproduct = new BsonDocument
            {
                {"ProductName",inputName },
                {"Price",inputprice},

            };
            productsCollection.InsertOne(newproduct);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"succesfully added {newproduct}");
            Console.ResetColor();
        }


        public static void DisplayAllProducts()
        {
            var productsCollection = GetCollection<BsonDocument>("Products");
            var filter = Builders<BsonDocument>.Filter.Empty;
            var productDocuments = productsCollection.Find(filter).ToList();

            foreach (var doc in productDocuments)
            {
                Console.WriteLine();
                Console.WriteLine($"\nProduct: {doc["ProductName"]}");
                Console.WriteLine($"Price: {doc["Price"].ToDouble():F2}");
                Console.WriteLine();
                Thread.Sleep(250);
            }


        }








    }

    

}
