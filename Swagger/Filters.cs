using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TakeHomeAPI.Filters  // Use your project’s appropriate namespace.
{
    public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var newPaths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                // Replace "{version}" with the actual document version (ex: "v1", "v2", etc.)
                newPaths.Add(path.Key.Replace("{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = newPaths;
        }
    }


    public class CustomTagDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedTags = swaggerDoc.Tags.Select(tag =>
            {
                if (tag.Name.StartsWith("ProductsV", StringComparison.InvariantCultureIgnoreCase))
                {
                    tag.Name = "Products";
                }
                return tag;
            }).ToList();
            swaggerDoc.Tags = updatedTags;
        }
    }


}
