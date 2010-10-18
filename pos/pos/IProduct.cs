using System;
namespace pos
{
    interface IProduct
    {
        string Code { get; }
        bool IsProvincialTaxApplicable { get; }
        decimal Price { get; }
    }
}
