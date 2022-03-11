<p align="center">
  <img src="https://github.com/Xavier-Mobius/Qonto.BankStatements/raw/main/doc/logo.png" width="256" title="Qonto Logo">
</p>

# Qonto.BankStatements

Retrieve bank statements from Qonto automatically.

## Output

![output](https://github.com/Xavier-Mobius/Qonto.BankStatements/raw/main/doc/Qonto_Statement.png)

## Usage

### Basic usage

```shell
Mobius.Qonto.BankStatements.exe --IBAN FR7612345678901234567890123 --Login company-1234 --SecretKey 01234abc5678def901234abcdef56789 --Directory C:\Temp
```

A XLSX statement file with the transactions of the last full month will be genereted with a default filename.

### Choose the output filename

```shell
--Filename MyFileName.xlsx
```

### Choose the date for the statement

```shell
--Month 6 --Year 2021
```

The statement will be generated with the transactions of June 2021. 
If no year is set, the current year is used.
If no month is set, the last finished month is used.

## Quick Links

Quonto API: https://api-doc.qonto.com/

## Contributing

If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are welcome. Please respect existing style in code.

## Licensing

The code in this project is licensed under BSD-3-Clause license.
