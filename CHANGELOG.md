### Changelog

#### Version 2.0.1
- [x] The assembly version matches the package version (2.0.0 shipped as assembly 1.0.0.0)
- [x] `SOFTURE.Settings` 2.0.0

#### Version 2.0.0
- [x] `AddRegonClient<TSettings>()` reads the API key from the `Regon` section of a settings class (`IRegonSettings`,
  `SOFTURE.Settings`), the same way as `AddFakturownia<TSettings>()`
- [x] Breaking: `AddRegonClient(string apiKey)` is removed

#### Version 1.7.0
- [x] Code refactoring
- [x] Fix and stabilisation of `IsActive`
- [x] Unit tests

#### Version 1.1.0 - 1.6.0
- [x] Published to nuget.org and stabilised the REGON package
