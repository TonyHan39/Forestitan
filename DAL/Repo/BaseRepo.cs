using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class BaseRepo
    {
        private Forestitan context;

        public Forestitan Context
        {
            get
            {
                if (context == null)
                {
                    context = new Forestitan();
                }
                return context;
            }
        }
    }
}
