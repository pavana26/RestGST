using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestGST.Models;
using System.Web.Http;
using RestGST.Helpers;

namespace RestGST.Controllers
{
    public class ExtractDataController : ApiController
    {

        private const string ExpenseMarkupText = "expense";
        private const string TotalMarkupText = "total";
        private const string CostCentreMarkupText = "cost_centre";
        private const string PaymentMethodMarkupText = "payment_method";
        private const string VendorMarkupText = "vendor";
        private const string ReservationDescriptionMarkupText = "description";
        private const string ReservationDateMarkupText = "date";
        public ExtractedData ExtractDataFromEmailText(string emailText)
        {
            ExtractedData extractedData = new ExtractedData();
            extractedData.ExpenseClaims = new List<ExpenseClaim>();
            extractedData.Reservations = new List<Reservation>();
            extractedData.ErrorsInProcesing = new List<string>();
            ExpenseClaim expenseClaim = new ExpenseClaim();
            Reservation reservation = new Reservation();
            if (HelperClass.CheckBothMarkupsExist(emailText, ExpenseMarkupText))
            {
                string expenseStr = HelperClass.GetTextBetweenMarkups(emailText, ExpenseMarkupText);

                if (!expenseStr.Contains("<total>"))
                {
                    extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("<total>"));
                    return extractedData;
                }
                if (!expenseStr.Contains("<cost_centre>"))
                {
                    expenseClaim.CostCentre = "UNKNOWN";
                }

                if (HelperClass.CheckBothMarkupsExist(expenseStr, TotalMarkupText))
                {
                    string totalStr = HelperClass.GetTextBetweenMarkups(expenseStr, TotalMarkupText);
                    expenseClaim.TotalExpense = Convert.ToDouble(totalStr != "" ? totalStr : "0");
                    expenseClaim.GSTApplied = (expenseClaim.TotalExpense * 3) / 23; // Inland Revenue’s recommended formula to find the GST amount from a GST inclusive price
                    expenseClaim.TotalExcludingGST = expenseClaim.TotalExpense - expenseClaim.GSTApplied;
                }
                else
                {
                    extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Total"));
                    return extractedData;
                }
                if (expenseClaim.CostCentre != "UNKNOWN")
                {
                    if (HelperClass.CheckBothMarkupsExist(expenseStr, CostCentreMarkupText))
                    {
                        expenseClaim.CostCentre = HelperClass.GetTextBetweenMarkups(expenseStr, CostCentreMarkupText);
                    }
                    else
                    {
                        extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Cost Centre"));
                        return extractedData;
                    }
                }
                if (HelperClass.CheckBothMarkupsExist(expenseStr, PaymentMethodMarkupText))
                {
                    expenseClaim.PaymentMethod = HelperClass.GetTextBetweenMarkups(expenseStr, PaymentMethodMarkupText);
                }
                else
                {
                    extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Payment Method"));
                    return extractedData;
                }
            }
            else
            {
                extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Expense"));
                return extractedData;
            }

            if (HelperClass.CheckBothMarkupsExist(emailText, VendorMarkupText))
            {
                reservation.Vendor = HelperClass.GetTextBetweenMarkups(emailText, VendorMarkupText);
            }
            else
            {
                extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Vendor"));
                return extractedData;
            }
            if (HelperClass.CheckBothMarkupsExist(emailText, ReservationDescriptionMarkupText))
            {
                reservation.Description = HelperClass.GetTextBetweenMarkups(emailText, ReservationDescriptionMarkupText);
            }
            else
            {
                extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Reservation Description"));
                return extractedData;
            }
            if (HelperClass.CheckBothMarkupsExist(emailText, ReservationDateMarkupText))
            {
                reservation.Date = HelperClass.GetTextBetweenMarkups(emailText, ReservationDateMarkupText);
            }
            else
            {
                extractedData.ErrorsInProcesing.Add(HelperClass.GetFormattedErrorMessage("Reservation Date"));
                return extractedData;
            }

            if (expenseClaim != null)
            {
                extractedData.ExpenseClaims.Add(expenseClaim);
            }
            if (reservation != null)
            {
                extractedData.Reservations.Add(reservation);
            }

            return extractedData;
        }
    }
}