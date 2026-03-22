# REGON

A .NET client library for the Polish REGON (GUS BIR1) SOAP API — query company data from the National Business Registry by NIP or KRS number.

![GitHub](https://img.shields.io/github/license/jarmatys/REGON) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/jarmatys/REGON/release-package.yml?label=release) ![Nuget](https://img.shields.io/nuget/v/REGON?label=version) ![Nuget](https://img.shields.io/nuget/dt/REGON) ![GitHub issues](https://img.shields.io/github/issues/jarmatys/REGON) ![GitHub pull requests](https://img.shields.io/github/issues-pr/jarmatys/REGON)

## Features

- Look up companies by **NIP** (tax identification number) or **KRS** (National Court Register number)
- Built-in **dependency injection** support via `IServiceCollection`
- Strongly-typed response model with address, legal form, activity codes (PKD), and more
- Automatic detection of **legal form** (sole proprietorship, LLC, joint-stock company, etc.)
- Active/suspended status resolution
- Handles the underlying SOAP/WCF communication so you don't have to

## Prerequisites

- .NET 6.0 or later
- A GUS API key — register at [api.stat.gov.pl/Home/RegonApi](https://api.stat.gov.pl/Home/RegonApi) to obtain one

## Installation

```bash
dotnet add package REGON
```

## Quick Start

### 1. Register the client

```csharp
services.AddRegonClient("<YOUR-API-KEY>");
```

### 2. Inject and use

```csharp
public class CompanyService
{
    private readonly IRegonClient _regonClient;

    public CompanyService(IRegonClient regonClient)
    {
        _regonClient = regonClient;
    }

    public async Task<Company> GetCompany(string nip)
    {
        return await _regonClient.GetCompanyDataByNip(nip);
    }
}
```

## API Reference

### Methods

| Method | Description |
|---|---|
| `GetCompanyDataByNip(string nip)` | Fetch company data by NIP (tax ID) |
| `GetCompanyDataByKrs(string krs)` | Fetch company data by KRS (court register number) |

Both methods return `Task<Company>`.

### `Company` Response Model

| Property | Type | Description |
|---|---|---|
| `Nip` | `string` | Tax identification number |
| `Krs` | `string` | National Court Register number |
| `Name` | `string` | Company name |
| `REGON` | `string` | REGON statistical number |
| `Street` | `string` | Street name |
| `BuildingNumber` | `string` | Building number |
| `FlatNumber` | `string` | Flat/apartment number |
| `City` | `string` | City |
| `PostCode` | `string` | Postal code |
| `Commune` | `string` | Commune (gmina) |
| `District` | `string` | District (powiat) |
| `Voivodeship` | `string` | Voivodeship (province) |
| `LegalForm` | `LegalForm` | Legal form enum (see below) |
| `MainPkd` | `string` | Main PKD activity code |
| `Pkds` | `List<Pkd>` | All registered PKD activity codes |
| `StartDate` | `DateTime` | Registration date |
| `EndDate` | `DateTime?` | Deregistration date (null if active) |
| `IsActive` | `bool` | Whether the company is currently active |
| `IsSuspended` | `bool` | Whether the company is suspended |
| `PhoneNumber` | `string` | Phone number |
| `Email` | `string` | Email address |
| `WebsiteUrl` | `string` | Website URL |

### `LegalForm` Enum

| Value | Name | Description |
|---|---|---|
| 1 | `JDG` | Sole proprietorship (jednoosobowa działalność gospodarcza) |
| 2 | `SPZOO` | Limited liability company (spółka z o.o.) |
| 3 | `SA` | Joint-stock company (spółka akcyjna) |
| 4 | `SC` | Civil partnership (spółka cywilna) |
| 5 | `SJ` | General partnership (spółka jawna) |
| 6 | `SK` | Limited partnership (spółka komandytowa) |
| 7 | `SPZOOSK` | Ltd. limited partnership (sp. z o.o. sp.k.) |
| 8 | `SKA` | Limited joint-stock partnership (spółka komandytowo-akcyjna) |

## Example Response

```json
{
  "Nip": "7010790303",
  "Krs": "",
  "Name": "RK RECOVERY SPÓŁKA AKCYJNA",
  "Street": "Plac Konesera",
  "City": "Warszawa",
  "BuildingNumber": "10 A",
  "FlatNumber": "",
  "PostCode": "03-736",
  "REGON": "369010805",
  "Commune": "Praga-Północ",
  "District": "Warszawa",
  "Voivodeship": "MAZOWIECKIE",
  "LegalForm": 3,
  "MainPkd": "6920Z",
  "StartDate": "2017-12-15T00:00:00",
  "EndDate": null,
  "IsSuspended": false,
  "IsActive": true,
  "PhoneNumber": null,
  "Email": null,
  "WebsiteUrl": null,
  "Pkds": [
    {
      "Value": "6920Z",
      "Name": "DZIAŁALNOŚĆ RACHUNKOWO-KSIĘGOWA; DORADZTWO PODATKOWE"
    },
    {
      "Value": "6910Z",
      "Name": "DZIAŁALNOŚĆ PRAWNICZA"
    }
  ]
}
```

## Running Tests

```bash
dotnet test
```

## Contributing

Contributions are welcome! Here's how to get started:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes (`git commit -m 'Add my feature'`)
4. Push to the branch (`git push origin feature/my-feature`)
5. Open a Pull Request

Please make sure all existing tests pass before submitting a PR.

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## Author

Created by Jarosław Armatys — [armatys.me](https://armatys.me)
