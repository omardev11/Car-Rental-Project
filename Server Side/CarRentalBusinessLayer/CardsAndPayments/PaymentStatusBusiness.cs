using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace CarRentalBusinessLayer.CardsAndPayments
{
    public class PaymentStatusBusiness : IPaymentStatusBusiness
    {
        private readonly IPaymentStatusData _paymentStatusData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2 }

        public enMode Mode;
        public DTO.PaymentStatusDTO PaymentStatusInfo { get; set; }

        public PaymentStatusBusiness(IPaymentStatusData paymentStatusData)
        {
            _paymentStatusData = paymentStatusData;
            PaymentStatusInfo = new DTO.PaymentStatusDTO();
        }

        public void Initialize(DTO.PaymentStatusDTO paymentStatusInfo, enMode mode = enMode.AddNewMode)
        {
            PaymentStatusInfo = paymentStatusInfo;
            Mode = mode;
        }

        private bool _AddNewPaymentStatus()
        {
            PaymentStatusInfo.PaymentStatusId = _paymentStatusData.AddNewPaymentStatus(PaymentStatusInfo);
            return PaymentStatusInfo.PaymentStatusId.HasValue;
        }

        private bool _UpdatePaymentStatus()
        {
            return _paymentStatusData.UpdatePaymentStatus(PaymentStatusInfo);
        }

        public bool DeletePaymentStatus(int paymentstatusid)
        {
            return _paymentStatusData.DeletePaymentStatus(paymentstatusid);
        }

        public DTO.PaymentStatusDTO GetPaymentStatusInfoBy(int? paymentStatusId, string statusName)
        {
            return _paymentStatusData.GetPaymentStatusBy(paymentStatusId, statusName);
        }

        public List<DTO.PaymentStatusDTO> GetAllPaymentStatuses()
        {
            return _paymentStatusData.GetAllPaymentStatuses();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    return _AddNewPaymentStatus();
                case enMode.UpdateMode:
                    return _UpdatePaymentStatus();
                default:
                    return false;
            }
        }
    }
}
