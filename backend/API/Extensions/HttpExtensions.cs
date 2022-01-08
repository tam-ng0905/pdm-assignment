using System.Text.Json;


//This is the extension used to return pagination 
namespace API.Extensions
{
    public static class HttpExtensions
    {
        
        //Create the HttpHeader to content information about pagination and send it back to the client
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
            int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new 
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}