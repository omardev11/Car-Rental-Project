using CarRentalDataLayer;
using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalBusinessLayer
{
    public class LicenseBusiness : ILicenseBusiness
    {
        private readonly ILicenseData _LicenseData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2 , DeleteMode = 3}

        public enMode Mode;
        public DTO.LicenseDTO LicenseInfo { get; set; }
        public LicenseBusiness(ILicenseData licensedata)
        {
            _LicenseData = licensedata;
            LicenseInfo = new DTO.LicenseDTO();
        }
        public void Initialize(DTO.LicenseDTO licenseinfo, enMode mode = enMode.AddNewMode)
        {
            LicenseInfo = licenseinfo;
            Mode = mode;
        }

        private bool _AddNewLicense()
        {
            LicenseInfo.LicenseId = _LicenseData.AddNewLicense(LicenseInfo);
            return LicenseInfo.LicenseId != -1;
        }

        private bool _UpdateLicense()
        {
            return _LicenseData.UpdateLicense(LicenseInfo);
        }
        private bool _DeleteLicense()
        {
            return _LicenseData.DeleteLicense(LicenseInfo.LicenseId);
        }

        public List<DTO.LicenseDTO> GetLicenseByCustomerId(int customerid)
        {
            return _LicenseData.GetAllLicenseForThiCustomerId(customerid);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateLicense())
                    {
                        return true;
                    }
                    else { return false; }
                    case enMode.DeleteMode:
                    if (_DeleteLicense())
                    {
                        return true;
                    }
                    else { return false; }
                default:
                    return false;
            }
        }

    }
}
