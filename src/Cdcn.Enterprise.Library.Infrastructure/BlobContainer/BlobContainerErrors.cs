using Cdcn.Enterprise.Library.Domain.Primitives;

namespace Cdcn.Enterprise.Library.Infrastructure.BlobContainer
{
    internal static class BlobContainerErrors
    {
        public static Error ErrorWhenDeleting => new Error(
            "BlobContainer.ErrorWhenDeleting",
            "Error When Deleting");

        public static Error FileNotExist => new Error(
           "BlobContainer.FileNotExist",
           "File Not Exist");
    }
}
