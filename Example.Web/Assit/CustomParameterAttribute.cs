using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;

namespace Example.Web.Assit
{
    using Example.Domain;

    public class CustomParameterAttribute : ModelBinderAttribute
    {
        public CustomParameterAttribute()
        {
            var result = new PackageResult<string>("");
            foreach (var s in result)
            {
                
            }
        }
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            throw new NullReferenceException();
        }
    }
}