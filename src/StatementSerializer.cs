using Mobius.Qonto.BankStatements.Model;
using OfficeOpenXml;
using System.Linq;

namespace Mobius.Qonto.BankStatements;

public class StatementSerializer
{
    private readonly List<Transaction> _Transactions;
    private readonly DateTime _From;
    private readonly DateTime _To;
    private readonly string? _LegalName;
    private readonly BankAccount? _BankAccount;
    public StatementSerializer(string? legalName, BankAccount? bankAccount, DateTime from, DateTime to, List<Transaction> transactions)
    {
        _LegalName = legalName;
        _BankAccount = bankAccount;
        _From = from;
        _To = to;
        _Transactions = transactions;

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
    public void ToExcel(string filename)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Statement");

        // NOTE: Top Page
        worksheet.Cells[1, 1].Value = $"Account name: {_BankAccount?.Name}";
        worksheet.Cells[2, 1].Value = "Bank statement";
        worksheet.Cells[3, 1].Value = $"From {_From:d} to {_To:d}";

        worksheet.Cells[5, 1].Value = _LegalName;
        worksheet.Cells[6, 1].Value = $"IBAN: {_BankAccount?.IBAN}";
        worksheet.Cells[7, 1].Value = $"BIB: {_BankAccount?.BIC}";

        worksheet.Cells[9, 1].Value = "Total credit:"; 
        worksheet.Cells[9, 2].Value = _Transactions.Where(t => t.Side == "credit").Sum(t => t.Amount);
        worksheet.Cells[10, 1].Value = "Total debit:";
        worksheet.Cells[10, 2].Value = _Transactions.Where(t => t.Side == "debit").Sum(t => t.Amount);

        worksheet.Cells[2, 1].Style.Font.Bold = true;
        worksheet.Cells[2, 1].Style.Font.Size = 14;
        worksheet.Cells[5, 1].Style.Font.Bold = true;
        worksheet.Cells[5, 1].Style.Font.Size = 14;
        worksheet.Cells[9, 1, 10, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        worksheet.Cells[9, 1, 10, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        worksheet.Cells[9, 2, 10, 2].Style.Numberformat.Format = "#,##0.00";

        // NOTE: Transactions
        worksheet.Cells[12, 1].Value = "Settled At";
        worksheet.Cells[12, 2].Value = "Label";
        worksheet.Cells[12, 3].Value = "Reference";
        worksheet.Cells[12, 4].Value = "Amount";
        worksheet.Cells[12, 5].Value = "Local Amount";
        worksheet.Cells[12, 6].Value = "Side";
        worksheet.Cells[12, 7].Value = "Operation Type";
        worksheet.Cells[12, 8].Value = "Currency";
        worksheet.Cells[12, 9].Value = "Local Currency";
        for (var i = 0; i < _Transactions.Count; i++)
        {
            var transaction = _Transactions[i];
            worksheet.Cells[13 + i, 1].Value = transaction.SettledAt;
            worksheet.Cells[13 + i, 2].Value = transaction.Label;
            worksheet.Cells[13 + i, 3].Value = transaction.Reference;
            worksheet.Cells[13 + i, 4].Value = transaction.Amount;
            worksheet.Cells[13 + i, 5].Value = transaction.LocalAmount;
            worksheet.Cells[13 + i, 6].Value = transaction.Side;
            worksheet.Cells[13 + i, 7].Value = transaction.OperationType;
            worksheet.Cells[13 + i, 8].Value = transaction.Currency;
            worksheet.Cells[13 + i, 9].Value = transaction.LocalCurrency;
        }

        var tabSize = _Transactions.Count;
        worksheet.Cells[12, 1, 12 + tabSize, 9].AutoFitColumns(13, 30);
        worksheet.Cells[12, 1, 12, 9].Style.Font.Bold = true;
        worksheet.Cells[12, 1, 12, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        worksheet.Cells[12, 1, 12, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        worksheet.Cells[13, 1, 12 + tabSize, 1].Style.Numberformat.Format = "dd/MM/yyyy";
        worksheet.Cells[13, 4, 12 + tabSize, 5].Style.Numberformat.Format = "#,##0.00";
        worksheet.Cells[12, 1, 12 + tabSize, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        worksheet.Cells[12, 1, 12 + tabSize, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);

        package.SaveAs(filename); 
    }
}