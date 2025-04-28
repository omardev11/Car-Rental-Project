using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalBusinessLayer.CardsAndPayments
{
    public class TransactionTypeBusiness : BusinessLayerInterfaces.ITransactionTypeBusiness
    {
        private readonly DataLayerInterfaces.ITransactionTypeData _transactionTypeData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2 }
        public enMode Mode { get; set; }
        public DTO.TransactionTypeDTO TransactionTypeInfo { get; set; }

        public TransactionTypeBusiness(DataLayerInterfaces.ITransactionTypeData transactionTypeData)
        {
            _transactionTypeData = transactionTypeData;
            TransactionTypeInfo = new DTO.TransactionTypeDTO(0, new DTO.TransactionTypeDTO.TransactionTypeInfo());
        }

        public void Initialize(DTO.TransactionTypeDTO transactionTypeInfo, enMode mode = enMode.AddNewMode)
        {
            TransactionTypeInfo = transactionTypeInfo;
            Mode = mode;
        }

        private bool _AddNewTransactionType()
        {
            TransactionTypeInfo.TransactionTypeId = _transactionTypeData.AddNewTransactionType(TransactionTypeInfo);
            return TransactionTypeInfo.TransactionTypeId.HasValue;
        }

        private bool _UpdateTransactionType()
        {
            return _transactionTypeData.UpdateTransactionType(TransactionTypeInfo);
        }

        public bool DeleteTransactionType(int transactiontypeId)
        {
            return _transactionTypeData.DeleteTransactionType(transactiontypeId);
        }

        public DTO.TransactionTypeDTO GetTransactionTypeBy(int? transactionTypeId, string typeName)
        {
            return _transactionTypeData.GetTransactionTypeBy(transactionTypeId, typeName);
        }

        public List<DTO.TransactionTypeDTO> GetAllTransactionTypes()
        {
            return _transactionTypeData.GetAllTransactionTypes();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    return _AddNewTransactionType();
                case enMode.UpdateMode:
                    return _UpdateTransactionType();
                default:
                    return false;
            }
        }
    }
}
