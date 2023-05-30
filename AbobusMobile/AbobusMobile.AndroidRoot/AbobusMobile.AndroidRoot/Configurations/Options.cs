using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.AndroidRoot.Configurations
{
    public class Options<TEntity>
    {
        private readonly string _entity;

        public Options(string entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public TEntity Value => JsonConvert.DeserializeObject<TEntity>(_entity);
    }
}
