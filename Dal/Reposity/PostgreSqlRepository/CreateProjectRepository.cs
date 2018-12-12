using System;
using System.Collections.Generic;
using System.Text;
using DAL.Context;

namespace DAL.Reposity.PostgreSqlRepository
{
    class CreateProjectRepository
    {
        private PostgreSqlNativeContext context;

        public CreateProjectRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add( )
        {
            throw new NotImplementedException();
        }

    }
}
