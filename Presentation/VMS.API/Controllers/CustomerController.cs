using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMS.Application.Enums;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly ICustomerRepository _customerRepository;
        private readonly IContactDetailRepository _contactDetailRepository;
        private readonly IAddressRepository _billingDetailsRepository;
        private readonly IShippingAddressRepository _shippingDetailsRepository;
        //private readonly ILoginCredentialsRepository _loginCredentialsRepository;
        private IFileManager _fileManager;

        public CustomerController(ICustomerRepository customerRepository,
            IContactDetailRepository contactDetailRepository,
            IAddressRepository billingDetailsRepository,
            IShippingAddressRepository shippingDetailsRepository,
            //ILoginCredentialsRepository loginCredentialsRepository,
            IFileManager fileManager)
        {
            _customerRepository = customerRepository;
            _contactDetailRepository = contactDetailRepository;
            _billingDetailsRepository = billingDetailsRepository;
            _shippingDetailsRepository = shippingDetailsRepository;
            //_loginCredentialsRepository = loginCredentialsRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        //[AllowAnonymous]
        public async Task<ResponseModel> SaveCustomer(Customer_Request parameters)
        {
            //auto generated password
            //if (parameters.Id == 0)
            //{
            //    var resultPass = await _loginCredentialsRepository.GetAutoGenPassword("");
            //    if (!string.IsNullOrEmpty(resultPass))
            //    {
            //        parameters.AutoPassword = resultPass;
            //    }
            //}

            bool bIsCustomerDeleted = false;
            int result = await _customerRepository.SaveCustomer(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else if (result == -3)
            {
                _response.Message = "Mobile Number is exists";
            }
            else if (result == -4)
            {
                _response.Message = "Email is exists";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";

                if (result > 0)
                {
                    string strContactErrorMsg = "";
                    string strLoginErrorMsg = "";

                    // Add data into Contact details
                    foreach (var items in parameters.ContactDetailList)
                    {
                        var vContactDetails = new ContactDetail_Request()
                        {
                            Id = items.Id,
                            RefId = result,
                            RefType = "Customer",
                            ContactName = items.ContactName,
                            MobileNumber = items.MobileNumber,
                            EmailId = items.EmailId,
                            IsActive = items.IsActive,
                        };

                        int resultContactDetails = await _contactDetailRepository.SaveContactDetail(vContactDetails);

                        if (resultContactDetails == (int)SaveOperationEnums.NoRecordExists)
                        {
                            strContactErrorMsg = "No record exists in Contact detail";
                        }
                        else if (resultContactDetails == (int)SaveOperationEnums.ReocrdExists)
                        {
                            strContactErrorMsg = "Record is already exists in Contact detail";
                        }
                        else if (resultContactDetails == (int)SaveOperationEnums.NoResult)
                        {
                            strContactErrorMsg = "Something went wrong in Contact detail, please try again";
                        }
                        else if (resultContactDetails == -3)
                        {
                            strContactErrorMsg = "Mobile Number is exists in Contact detail";
                        }

                        if (!string.IsNullOrWhiteSpace(strContactErrorMsg))
                        {
                            _response.Message = strContactErrorMsg;

                            int resultDeleteCustomer = await _customerRepository.DeleteCustomer(result);
                            if (resultDeleteCustomer > 0)
                            {
                                bIsCustomerDeleted = true;
                            }
                        }
                    }

                    if (bIsCustomerDeleted == false)
                    {
                        // Add data into Billing details
                        foreach (var items in parameters.BillingAddressList)
                        {
                            //GST Upload
                            if (!string.IsNullOrWhiteSpace(items.GST_Base64))
                            {
                                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(items.GST_Base64, "\\Uploads\\BillingAddress\\", items.GSTOriginalFileName);

                                if (!string.IsNullOrWhiteSpace(vUploadFile))
                                {
                                    items.GSTFileName = vUploadFile;
                                }
                            }

                            var vBillingDetails = new Address_Request()
                            {
                                Id = items.Id,
                                RefId = result,
                                RefType = "Customer",
                                IsNational_Or_International = items.IsNational_Or_International,
                                Address1 = items.Address1,
                                CountryId = items.CountryId,
                                StateId = items.StateId,
                                DistrictId = items.DistrictId,
                                CityId = items.CityId,
                                PinCode = items.PinCode,
                                IsGST = items.IsGST,
                                GSTNumber = items.GSTNumber,
                                GSTOriginalFileName = items.GSTOriginalFileName,
                                GSTFileName = items.GSTFileName,
                                IsActive = items.IsActive,
                            };

                            int resultBillingDetails = await _billingDetailsRepository.SaveAddress(vBillingDetails);
                        }

                        // Add data into Shipping details
                        foreach (var items in parameters.ShippingAddressList)
                        {
                            //GST Upload
                            if (!string.IsNullOrWhiteSpace(items.GST_Base64))
                            {
                                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(items.GST_Base64, "\\Uploads\\ShippingAddress\\", items.GSTOriginalFileName);

                                if (!string.IsNullOrWhiteSpace(vUploadFile))
                                {
                                    items.GSTFileName = vUploadFile;
                                }
                            }

                            var vShippingDetails = new ShippingAddress_Request()
                            {
                                Id = items.Id,
                                RefId = result,
                                RefType = "Customer",
                                IsNational_Or_International = items.IsNational_Or_International,
                                Address1 = items.Address1,
                                CountryId = items.CountryId,
                                StateId = items.StateId,
                                DistrictId = items.DistrictId,
                                CityId = items.CityId,
                                PinCode = items.PinCode,
                                IsGST = items.IsGST,
                                GSTNumber = items.GSTNumber,
                                GSTOriginalFileName = items.GSTOriginalFileName,
                                GSTFileName = items.GSTFileName,
                                IsActive = items.IsActive,
                            };

                            int resultShippingDetails = await _shippingDetailsRepository.SaveShippingAddress(vShippingDetails);
                        }
                    }
                }
            }

            if (bIsCustomerDeleted == true)
            {
                _response.Id = 0;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCustomerList(CustomerSearch_Request parameters)
        {
            IEnumerable<Customer_Response> lstCustomers = await _customerRepository.GetCustomerList(parameters);
            _response.Data = lstCustomers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCustomerById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _customerRepository.GetCustomerById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }

        /*
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportCustomer()
        {
            _response.IsSuccess = false;
            byte[] result;

            var parameters = new CustomerSearch_Request();
            IEnumerable<Customer_Response> lstCustomerListObj = await _customerRepository.GetCustomerList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    int recordIndex;
                    ExcelWorksheet WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Customer");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Parent Customer";
                    WorkSheet1.Cells[1, 3].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 4].Value = "LandLine Number";
                    WorkSheet1.Cells[1, 5].Value = "Customer Type";
                    WorkSheet1.Cells[1, 6].Value = "Email";
                    WorkSheet1.Cells[1, 7].Value = "Country Name";
                    WorkSheet1.Cells[1, 8].Value = "Country Code";
                    WorkSheet1.Cells[1, 9].Value = "Contact Name";
                    WorkSheet1.Cells[1, 10].Value = "IsActive";


                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.ParentCustomer;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.MobileNo;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.LandlineNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.CustomerType;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.EmailId;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.CountryCode;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.ContactName;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();


                    // Contact
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Contact");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Contact Person";
                    WorkSheet1.Cells[1, 3].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 4].Value = "Email";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj.ToList().Distinct())
                    {
                        var vContactDetail_Search = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstContactListObj = await _customerRepository.GetContactDetailsList(vContactDetail_Search);
                        foreach (var itemContact in lstContactListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemContact.ContactPerson;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemContact.MobileNo;
                            WorkSheet1.Cells[recordIndex, 4].Value = itemContact.EmailId;

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();


                    // Billing
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Billing");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Street Name";
                    WorkSheet1.Cells[1, 3].Value = "Country Name";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        var vBillingSearch = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstAddressListObj = await _customerRepository.GetBillingDetailsList(vBillingSearch);
                        foreach (var itemAddress in lstAddressListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemAddress.StreetName;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemAddress.CountryName;

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();

                    // Shipping
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Shipping");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Street Name";
                    WorkSheet1.Cells[1, 3].Value = "Country Name";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        var vShippingSearch = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstAddressListObj = await _customerRepository.GetShippingDetailsList(vShippingSearch);
                        foreach (var itemAddress in lstAddressListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemAddress.StreetName;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemAddress.CountryName;

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();

                    // Shipping
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Login");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Contact Name";
                    WorkSheet1.Cells[1, 3].Value = "Mobile";
                    WorkSheet1.Cells[1, 4].Value = "IsActive";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        var vLoginSearch = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstAddressListObj = await _customerRepository.GetLoginCredentialsList(vLoginSearch);
                        foreach (var itemAddress in lstAddressListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemAddress.ContactName;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemAddress.Username;
                            WorkSheet1.Cells[recordIndex, 4].Value = items.IsActive == true ? "Active" : "Inactive";

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();


                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }


            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Exported successfully";
            }

            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportChildCustomerListOfParentCustomer(int ParentCustomerId = 0)
        {
            _response.IsSuccess = false;
            byte[] result;

            var parameters = new CustomerSearch_Request()
            {
                ParentCustomerId = ParentCustomerId,
                CustomerId = 0,
                CountryId = 0,
                LeadStatusId = 0,
                SearchText = ""
            };
            IEnumerable<Customer_Response> lstCustomerListObj = await _customerRepository.GetCustomerList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    int recordIndex;
                    ExcelWorksheet WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Customer");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Parent Customer";
                    WorkSheet1.Cells[1, 3].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 4].Value = "LandLine Number";
                    WorkSheet1.Cells[1, 5].Value = "Customer Type";
                    WorkSheet1.Cells[1, 6].Value = "Email";
                    WorkSheet1.Cells[1, 7].Value = "Country Name";
                    WorkSheet1.Cells[1, 8].Value = "Country Code";
                    WorkSheet1.Cells[1, 9].Value = "Contact Name";
                    WorkSheet1.Cells[1, 10].Value = "IsActive";


                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.ParentCustomer;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.MobileNo;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.LandlineNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.CustomerType;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.EmailId;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.CountryCode;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.ContactName;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();


                    // Contact
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Contact");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Contact Person";
                    WorkSheet1.Cells[1, 3].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 4].Value = "Email";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj.ToList().Distinct())
                    {
                        var vContactDetail_Search = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstContactListObj = await _customerRepository.GetContactDetailsList(vContactDetail_Search);
                        foreach (var itemContact in lstContactListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemContact.ContactPerson;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemContact.MobileNo;
                            WorkSheet1.Cells[recordIndex, 4].Value = itemContact.EmailId;

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();


                    // Billing
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Billing");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Street Name";
                    WorkSheet1.Cells[1, 3].Value = "Country Name";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        var vBillingSearch = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstAddressListObj = await _customerRepository.GetBillingDetailsList(vBillingSearch);
                        foreach (var itemAddress in lstAddressListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemAddress.StreetName;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemAddress.CountryName;

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();

                    // Shipping
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Shipping");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Street Name";
                    WorkSheet1.Cells[1, 3].Value = "Country Name";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        var vShippingSearch = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstAddressListObj = await _customerRepository.GetShippingDetailsList(vShippingSearch);
                        foreach (var itemAddress in lstAddressListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemAddress.StreetName;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemAddress.CountryName;

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();

                    // Shipping
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Login");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Customer Name";
                    WorkSheet1.Cells[1, 2].Value = "Contact Name";
                    WorkSheet1.Cells[1, 3].Value = "Mobile";
                    WorkSheet1.Cells[1, 4].Value = "IsActive";

                    recordIndex = 2;
                    foreach (var items in lstCustomerListObj)
                    {
                        var vLoginSearch = new Search_Request()
                        {
                            CustomerId = items.Id,
                            SearchText = ""
                        };

                        var lstAddressListObj = await _customerRepository.GetLoginCredentialsList(vLoginSearch);
                        foreach (var itemAddress in lstAddressListObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerName;
                            WorkSheet1.Cells[recordIndex, 2].Value = itemAddress.ContactName;
                            WorkSheet1.Cells[recordIndex, 3].Value = itemAddress.Username;
                            WorkSheet1.Cells[recordIndex, 4].Value = items.IsActive == true ? "Active" : "Inactive";

                            recordIndex += 1;
                        }
                    }
                    WorkSheet1.Columns.AutoFit();


                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }


            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Exported successfully";
            }

            return _response;
        }
        */

    }
}
