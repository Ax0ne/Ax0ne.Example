using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;

namespace Example.Web.Assit
{
    public class CustomParameterAttribute : ModelBinderAttribute
    {
        public CustomParameterAttribute()
        {
            var result = new PackageResult<string>(new []{""});
            foreach (var s in result)
            {

            }
        }
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            throw new NullReferenceException();
        }
    }

    public interface IPackageResult
    {
        bool IsSuccess { get; set; }
        string Error { get; set; }
    }
    public class PackageResult<T>:List<T>,IPackageResult
    {
        public PackageResult(IEnumerable<T> list)
        {
            this.AddRange(list);
        }
        public bool IsSuccess
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}