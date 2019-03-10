using System;
using System.Collections.Generic;
using System.Text;

namespace PaySlipGenerator.ApL.Services.Interfaces
{
    public interface ITaxCalculatorService
    {
        void GeneratePayslips();
        void StoreData();
    }
}
