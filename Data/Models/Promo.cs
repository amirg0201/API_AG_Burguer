using System;
using System.Collections.Generic;

namespace API_AG_Burguer.Data.Models;

public partial class Promo
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public DateTime FechaPromo { get; set; }

    public int Burgerid { get; set; }

    public virtual Burger Burger { get; set; } = null!;
}
