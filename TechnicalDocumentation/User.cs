//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechnicalDocumentation
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int Id { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string Pname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Role_id { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
