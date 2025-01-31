using Cdcn.Enterprise.Library.Domain.Primitives.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.FileShare
{
    internal static class FileShareErrors
    {

        public static Error GenericError => new Error(
           "FileShare.GenericError",
           "Generic Error");
        public static Error ErrorWhenDeleting => new Error(
            "FileShare.ErrorWhenDeleting",
            "Error When Deleting");

        public static Error FileNotExist => new Error(
           "FileShare.FileNotExist",
           "File Not Exist");
    }
}
