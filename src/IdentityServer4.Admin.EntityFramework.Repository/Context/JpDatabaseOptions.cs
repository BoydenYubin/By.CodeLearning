using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearning.Admin.EntityFramework.Repository.Context
{
    public class JpDatabaseOptions
    {
        public bool MustThrowExceptionIfDatabaseDontExist { get; set; } = true;
    }
}
