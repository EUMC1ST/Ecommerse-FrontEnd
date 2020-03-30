using EcommerseClient.Models;

namespace EcommerseClient.Services
{
    public class ProductCatalogService
    {
        public static string BaseUrl = "http://localhost:5001/";

        public static ProductCatalog Catalog(string page = "1")
        {
            return new HttpRequests().TheGet<ProductCatalog>("api/ProductCatalogService?pageNumber=" + page, BaseUrl);
        }

        public static Producto Info(string id)
        {
            return new HttpRequests().TheGet<Producto>("api/ProductCatalogService/" + id, BaseUrl);
        }

        public static ProductCatalog CatalogByName(string name)
        {
            return new HttpRequests().TheGet<ProductCatalog>("api/ProductCatalogService?name=" + name, BaseUrl);
        }
    }
}
