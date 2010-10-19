using System;
namespace pos
{
    public interface IProduct
    {
        string Code { get; }
        bool PstExempt { get; }
        decimal Price { get; }
    }
}
