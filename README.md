# Shengqian

> 省钱 (shěng qián): "to save money"

An early-stage research project shaped as a savings and finance app, hence the name: pinyin for "save money". The goals are to study a complex domain shared across a single-language monolith (one domain layer for backend, mobile and beyond), and to push C# file-based apps to their limits.

## Structure

- `Shengqian.Domain`: shared domain models and DTOs
- `Shengqian.Backend`: API and database
- `Shengqian.Android`: mobile app
- `Shengqian.Vendor`: vendored dependencies
  - [SmartCtor](https://github.com/pedromigueldev/SmartCtor): constructor generator and monadic types (`Option`, `Result`)

## Getting started

```sh
git clone --recurse-submodules <repo-url>
```

Already cloned without submodules? Run:

```sh
git submodule update --init
```
