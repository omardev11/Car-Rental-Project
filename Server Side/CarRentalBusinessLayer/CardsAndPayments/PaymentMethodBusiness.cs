using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalBusinessLayer.CardsAndPayments
{
    public class PaymentMethodBusiness : IPaymentMethodBusiness
    {
        private readonly IPaymentMethodData _PaymentMethodData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2 }

        public enMode Mode;
        public DTO.PaymentMethodDTO PaymentMethodInfo { get; set; }

        public PaymentMethodBusiness(IPaymentMethodData paymentMethodData)
        {
            _PaymentMethodData = paymentMethodData;
            PaymentMethodInfo = new DTO.PaymentMethodDTO();
        }

        public void Initialize(DTO.PaymentMethodDTO paymentMethodInfo, enMode mode = enMode.AddNewMode)
        {
            PaymentMethodInfo = paymentMethodInfo;
            Mode = mode;
        }

        private bool _AddNewPaymentMethod()
        {
            PaymentMethodInfo.PaymentMethodId = _PaymentMethodData.AddNewPaymentMethod(PaymentMethodInfo);
            return PaymentMethodInfo.PaymentMethodId.HasValue;
        }

        private bool _UpdatePaymentMethod()
        {
            return _PaymentMethodData.UpdatePaymentMethod(PaymentMethodInfo);
        }

        public bool DeletePaymentMethod(int paymentmethodid)
        {
            return _PaymentMethodData.DeletePaymentMethod(paymentmethodid);
        }

        public DTO.PaymentMethodDTO GetPaymentMethodById(int? paymentMethodId, string? paymentMethod)
        {
            return _PaymentMethodData.GetPaymentMethodByIdOrName(paymentMethodId, paymentMethod);
        }

        public List<DTO.PaymentMethodDTO> GetAllPaymentMethods()
        {
            return _PaymentMethodData.GetAllPaymentMethods();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewPaymentMethod())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    return _UpdatePaymentMethod();
                default:
                    return false;
            }
        }
    }
}
