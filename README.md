# REGON

A .NET client library for the Polish REGON (GUS BIR1) SOAP API — query company data from the National Business Registry by NIP, KRS, or REGON number.

![GitHub](https://img.shields.io/github/license/SOFTURE/REGON) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/SOFTURE/REGON/release.yml?label=release) ![Nuget](https://img.shields.io/nuget/v/REGON?label=version) ![Nuget](https://img.shields.io/nuget/dt/REGON) ![GitHub issues](https://img.shields.io/github/issues/SOFTURE/REGON) ![GitHub pull requests](https://img.shields.io/github/issues-pr/SOFTURE/REGON)

## Features

- Look up companies by **NIP** (tax identification number), **KRS** (National Court Register number), or **REGON** (statistical number)
- Built-in **dependency injection** support via `IServiceCollection`
- Strongly-typed response model with address, legal form, activity codes (PKD), and more
- Automatic detection of **legal form** (sole proprietorship, LLC, joint-stock company, etc.)
- Active/suspended status resolution
- Handles the underlying SOAP/WCF communication so you don't have to

## Prerequisites

- .NET 6.0 or later (the package ships `net6.0` and `net10.0` builds)
- A GUS API key — register at [api.stat.gov.pl/Home/RegonApi](https://api.stat.gov.pl/Home/RegonApi) to obtain one

## Installation

```bash
dotnet add package REGON
```

## Quick Start

### 1. Register the client

The client reads its key from the `Regon` section of your settings class, the same way as the other SOFTURE
packages (`SOFTURE.Settings`):

```csharp
public sealed class AppSettings : IRegonSettings
{
    public RegonSettings Regon { get; init; }
}
```

```json
"Regon": {
  "ApiKey": "<YOUR-API-KEY>"
}
```

```csharp
services
    .ConfigureSettings<AppSettings>(configuration)
    .AddRegonClient<AppSettings>();
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
| `GetCompanyDataByRegon(string regon)` | Fetch company data by REGON (statistical number) |

All methods return `Task<Company>`.

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

The integration tests call the public GUS BIR **test** environment (`wyszukiwarkaregontest.stat.gov.pl`, test key
`abcde12345abcde12345`) and are skipped unless you opt in:

```bash
REGON_INTEGRATION=1 dotnet test
```

## Releasing

The single workflow [`release.yml`](.github/workflows/release.yml) releases the package to nuget.org and creates a
GitHub Release with the `.nupkg` attached.

A release starts from a tag `regon@<version>` (e.g. `regon@1.9.0`) created by hand; the workflow never creates
tags. In GitHub: Releases → **Draft a new release** → **Choose a tag** → type `regon@1.9.0` → **Create new tag on
publish**, target `master` → **Publish release**. Pushing the tag from a terminal works the same way.

Before publishing, the workflow checks that the commit is on `master`, builds for `net6.0` and `net10.0`, runs the
unit tests and the integration tests against the GUS BIR test environment, blocks on known vulnerabilities on
`net10.0` (`net6.0` only warns) and checks the package contents. **Run workflow** on a branch is a dry run.

Publishing goes through [nuget.org trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing):
the policy (Package Owner `SOFTURE`, Repository `SOFTURE/REGON`, Workflow File `release.yml`) and the organization
secret `NUGET_USER` (the nuget.org profile name of the policy owner). Keep the workflow file name.

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
