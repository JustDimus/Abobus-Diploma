using AbobusMobile.Utilities.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Models
{
    public abstract class BaseResponse
    {
        protected int statusCode;

        protected object responseContent;

        public int StatusCode => statusCode;

        public abstract bool Succeeded { get; }

        public object ResponseContent => responseContent;

        public Exception Exception { get; set; }

        public TEntity As<TEntity>() where TEntity : class
        {
            responseContent.ValidateIsNotNull();

            return JsonConvert.DeserializeObject<TEntity>(ResponseContent.ToString());
        }

        public Stream AsStream()
            => ResponseContent as Stream;
    }
}
