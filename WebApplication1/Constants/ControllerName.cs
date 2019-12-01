using System;
using WebApplication1.Data;

namespace WebApplication1.Constants
{
    public static class ControllerName
    {
        public const string Tenant = nameof(Tenant);
   
   /// <summary>
   /// a.k.a Controller, SystemController, SbcController etc.
   /// </summary>
   /// <returns></returns>
        public const string Hub = nameof(Hub);
        public const string Terminal = nameof(Terminal);
    }
}
