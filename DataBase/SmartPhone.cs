//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class SmartPhone
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public Nullable<decimal> OLD_PRICE { get; set; }
        public Nullable<decimal> SAVING { get; set; }
        public string LINK { get; set; }
        public Nullable<int> CONDITION { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public string SHOP { get; set; }
        public string IMAGE { get; set; }
        public Nullable<System.DateTime> DATE_PRICE_CHANGE { get; set; }
    }
}
