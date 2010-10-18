using System;
namespace pos
{
    public interface IDisplay
    {
        void PrintPrice(Product iProduct);
        string PrintBill();
    }
}
