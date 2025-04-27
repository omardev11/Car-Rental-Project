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
    public class RentalTransactionBusiness : IRentalTransactionBusiness
    {
        private readonly IRentalTransactionData _rentalTransactionData;

        public enum enMode { AddNewMode = 1, UpdateMode = 2 }
        public enMode Mode { get; set; }
        public DTO.RentalTransactionDTO RentalTransactionInfo { get; set; }

        public RentalTransactionBusiness(IRentalTransactionData rentalTransactionData)
        {
            _rentalTransactionData = rentalTransactionData;
            RentalTransactionInfo = new DTO.RentalTransactionDTO();
        }

        public void Initialize(DTO.RentalTransactionDTO rentalTransaction, enMode mode = enMode.AddNewMode)
        {
            RentalTransactionInfo = rentalTransaction;
            Mode = mode;
        }

        private bool _AddNewRentalTransaction()
        {
            RentalTransactionInfo.TransactionId = _rentalTransactionData.AddNewRentalTransaction(RentalTransactionInfo);
            return RentalTransactionInfo.TransactionId.HasValue;
        }

        private bool _UpdateRentalTransaction()
        {
            return _rentalTransactionData.UpdateRentalTransaction(RentalTransactionInfo);
        }

        public List<DTO.RentalTransactionDTO> GetAllRentalTransactionsBy(DTO.RentalTransactionDTO rentaltransaction)
        {
            return _rentalTransactionData.GetAllRentalTransactionsBy(rentaltransaction);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    return _AddNewRentalTransaction();
                case enMode.UpdateMode:
                    return _UpdateRentalTransaction();
                default:
                    return false;
            }
        }
    }
}
